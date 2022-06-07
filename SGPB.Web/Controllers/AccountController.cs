using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using SGPB.Web.Helpers;
using SGPB.Web.Models;
using SGPB.Web.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Controllers
{
        public class AccountController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly IUserHelper _userHelper;
                private readonly ICombosHelper _combosHelper;
                private readonly IBlobHelper _blobHelper;
                private readonly IMailHelper _mailHelper;


                public AccountController(ApplicationDbContext context,
                                                        IUserHelper userHelper,
                                                        ICombosHelper combosHelper,
                                                        IBlobHelper blobHelper,
                                                        IMailHelper mailHelper)
                {
                        _context = context;
                        _userHelper = userHelper;
                        _combosHelper = combosHelper;
                        _blobHelper = blobHelper;
                        _mailHelper = mailHelper;
                }

                public IActionResult Login()
                {
                        if (User.Identity.IsAuthenticated)
                        {
                                return RedirectToAction("Index", "Home");
                        }

                        return View(new LoginViewModel());
                }

                [HttpPost]
                public async Task<IActionResult> Login(LoginViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                                if (result.Succeeded)
                                {
                                        if (Request.Query.Keys.Contains("ReturnUrl"))
                                        {
                                                return Redirect(Request.Query["ReturnUrl"].First());
                                        }

                                        return RedirectToAction("Index", "Home");
                                }

                                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                        }

                        return View(model);
                }

                public async Task<IActionResult> Logout()
                {
                        await _userHelper.LogoutAsync();
                        return RedirectToAction("Index", "Home");
                }

                public IActionResult NotAuthorized()
                {
                        return View();
                }

                public IActionResult Register()
                {
                        AddUserViewModel model = new AddUserViewModel
                        {
                                DocumentTypes = _combosHelper.GetComboDocumentTypes()
                        };

                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Register(AddUserViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                Guid imageId = Guid.Empty;

                                if (model.ImageFile != null)
                                {
                                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                                }

                                User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
                                if (user == null)
                                {
                                        ModelState.AddModelError(string.Empty, "Este correo ya está en uso.");
                                        model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
                                        return View(model);
                                }


                                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                                {
                                        userid = user.Id,
                                        token = myToken
                                }, protocol: HttpContext.Request.Scheme);

                                Response response = _mailHelper.SendMail(model.Username, "Confirmación de Email", $"<h1>Confirmación de Email</h1>" +
                                    $"Permitir al usuario, " +
                                    $"por favor haga clic en este enlace:</br></br><a href = \"{tokenLink}\">Confirmar Correo</a>");
                                if (response.IsSuccess)
                                {
                                        ViewBag.Message = "Las instrucciones para permitir que su usuario ha sido enviado al correo electrónico.";
                                        return View(model);
                                }

                                ModelState.AddModelError(string.Empty, response.Message);


                                LoginViewModel loginViewModel = new LoginViewModel
                                {
                                        Password = model.Password,
                                        RememberMe = false,
                                        Username = model.Username
                                };

                                var result2 = await _userHelper.LoginAsync(loginViewModel);

                                if (result2.Succeeded)
                                {
                                        return RedirectToAction("Index", "Home");
                                }
                        }
                        model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
                        return View(model);
                }

                public async Task<IActionResult> ChangeUser()
                {
                        User user = await _userHelper.GetUserAsync(User.Identity.Name);
                        if (user == null)
                        {
                                return NotFound();
                        }

                        EditUserViewModel model = new EditUserViewModel
                        {
                                Address = user.Address,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                PhoneNumber = user.PhoneNumber,
                                ImageId = user.ImageId,
                                DocumentTypes = _combosHelper.GetComboDocumentTypes(),
                                DocumentTypeId = user.DocumentType.Id,

                                Id = user.Id,
                                Document = user.Document
                        };
                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> ChangeUser(EditUserViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                Guid imageId = model.ImageId;

                                if (model.ImageFile != null)
                                {
                                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                                }

                                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                                user.FirstName = model.FirstName;
                                user.LastName = model.LastName;
                                user.Address = model.Address;
                                user.PhoneNumber = model.PhoneNumber;
                                user.ImageId = imageId;
                                user.DocumentType = await _context.DocumentTypes.FindAsync(model.DocumentTypeId);
                                user.Document = model.Document;

                                await _userHelper.UpdateUserAsync(user);
                                return RedirectToAction("Index", "Home");
                        }
                        model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
                        return View(model);
                }

                public IActionResult ChangePassword()
                {
                        return View();
                }

                [HttpPost]
                public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                var user = await _userHelper.GetUserAsync(User.Identity.Name);
                                if (user != null)
                                {
                                        var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                                        if (result.Succeeded)
                                        {
                                                return RedirectToAction("ChangeUser");
                                        }
                                        else
                                        {
                                                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                                        }
                                }
                                else
                                {
                                        ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                                }
                        }
                        return View(model);
                }

                public async Task<IActionResult> ConfirmEmail(string userId, string token)
                {
                        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                        {
                                return NotFound();
                        }

                        User user = await _userHelper.GetUserAsync(new Guid(userId));
                        if (user == null)
                        {
                                return NotFound();
                        }

                        IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
                        if (!result.Succeeded)
                        {
                                return NotFound();
                        }

                        return View();
                }

                public IActionResult RecoverPassword()
                {
                        return View();
                }

                [HttpPost]
                public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                User user = await _userHelper.GetUserAsync(model.Email);
                                if (user == null)
                                {
                                        ModelState.AddModelError(string.Empty, "El correo electrónico no corresponde a un usuario registrado.");
                                        return View(model);
                                }

                                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                                string link = Url.Action(
                                    "ResetPassword",
                                    "Account",
                                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                                _mailHelper.SendMail(model.Email, "Restablecimiento de contraseña", $"<h1>Restablecimiento de contraseña</h1>" +
                                    $"Para restablecer la contraseña haz click en este enlace:</br></br>" +
                                    $"<a href = \"{link}\">Restablecer la contraseña</a>");
                                ViewBag.Message = "Las instrucciones para recuperar su contraseña han sido enviadas al correo electrónico.";
                                return View();

                        }

                        return View(model);
                }

                public IActionResult ResetPassword(string token)
                {
                        return View();
                }

                [HttpPost]
                public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
                {
                        User user = await _userHelper.GetUserAsync(model.UserName);
                        if (user != null)
                        {
                                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                                if (result.Succeeded)
                                {
                                        ViewBag.Message = "Restablecimiento de contraseña exitoso.";
                                        return View();
                                }

                                ViewBag.Message = "Error al restablecer la contraseña.";
                                return View(model);
                        }

                        ViewBag.Message = "Usuario no encontrado.";
                        return View(model);
                }
        }
}
