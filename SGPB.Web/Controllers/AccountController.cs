using Microsoft.AspNetCore.Mvc;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using SGPB.Web.Helpers;
using SGPB.Web.Models;
using System;
using System.Threading.Tasks;

public class AccountController : Controller
{
        private readonly IUserHelper _userHelper;
        private readonly ApplicationDbContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;


        public AccountController(IUserHelper userHelper, ApplicationDbContext context, ICombosHelper combosHelper, IBlobHelper blobHelper)
        {
                _userHelper = userHelper;
                _context = context;
                _combosHelper = combosHelper;
                _blobHelper = blobHelper;

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
                                return RedirectToAction("Index", "Home");
                        }

                        ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
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

        public async Task<IActionResult> Register()
        {
                AddUserViewModel model = new AddUserViewModel
                {
                        Id = Guid.Empty.ToString(),
                        DocumentTypes = await _combosHelper.GetComboDocumentTypesAsync(),
                        UserType = UserType.User,
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

                        User user = await _userHelper.AddUserAsync(model);
                        if (user == null)
                        {
                                ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                                model.DocumentTypes = await _combosHelper.GetComboDocumentTypesAsync();
                                return View(model);
                        }

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
                model.DocumentTypes = await _combosHelper.GetComboDocumentTypesAsync();
                return View(model);
        }

}

