using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using SGPB.Web.Helpers;
using SGPB.Web.Models;
using System;
using System.Threading.Tasks;

namespace SGPB.Web.Controllers
{
        [Authorize(Roles = "Admin")]
        public class UsersController : Controller
        {
                private readonly IUserHelper _userHelper;
                private readonly ApplicationDbContext _context;
                private readonly ICombosHelper _combosHelper;
                private readonly IBlobHelper _blobHelper;

                public UsersController(IUserHelper userHelper, ApplicationDbContext context, ICombosHelper combosHelper, IBlobHelper blobHelper)
                {
                        _userHelper = userHelper;
                        _context = context;
                        _combosHelper = combosHelper;
                        _blobHelper = blobHelper;
                }

                public async Task<IActionResult> Index()
                {
                        return View(await _context.Users
                            .Include(u => u.DocumentType)
                            .ToListAsync());
                }

                public async Task<IActionResult> Create()
                {
                        AddUserViewModel model = new AddUserViewModel
                        {
                                Id = Guid.Empty.ToString(),
                                DocumentTypes = await _combosHelper.GetComboDocumentTypesAsync(),

                                UserType = UserType.Admin,
                        };

                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(AddUserViewModel model)
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
                                        return View(model);
                                }

                                return RedirectToAction(nameof(Index));
                        }

                        return View(model);
                }

                public IActionResult Login()
                {
                        if (User.Identity.IsAuthenticated)
                        {
                                return RedirectToAction("Index", "Home");
                        }

                        return View(new LoginViewModel());
                }
        }
}
