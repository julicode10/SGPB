﻿using Microsoft.AspNetCore.Mvc;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using SGPB.Web.Helpers;
using SGPB.Web.Models;
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


                public AccountController(ApplicationDbContext context,
                                                        IUserHelper userHelper,
                                                        ICombosHelper combosHelper,
                                                        IBlobHelper blobHelper)
                {
                        _context = context;
                        _userHelper = userHelper;
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
                                        ModelState.AddModelError(string.Empty, "Este correo electrónico ya está en uso.");
                                        model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
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
        }
}
