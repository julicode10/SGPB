﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Helpers;
using SGPB.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Controllers
{
        [Authorize(Roles = "Admin")]
        public class CategoriesController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly IBlobHelper _blobHelper;
                private readonly IConverterHelper _converterHelper;

                public CategoriesController(ApplicationDbContext context, IBlobHelper blobHelper, IConverterHelper converterHelper)
                {
                        _context = context;
                        _blobHelper = blobHelper;
                        _converterHelper = converterHelper;

                }
                public async Task<IActionResult> Index()
                {
                        return View(await _context.Categories
                                        .OrderByDescending(b => b.Id)
                                        .ToListAsync()
                                );
                }

                public IActionResult Create()
                {
                        CategoryViewModel model = new CategoryViewModel();
                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(CategoryViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                Guid imageId = Guid.Empty;

                                if (model.ImageFile != null)
                                {
                                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                                }

                                try
                                {
                                        Category category = _converterHelper.ToCategory(model, imageId, true);
                                        _context.Add(category);
                                        await _context.SaveChangesAsync();
                                        return RedirectToAction(nameof(Index));
                                }
                                catch (DbUpdateException dbUpdateException)
                                {
                                        if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                                        {
                                                ModelState.AddModelError(string.Empty, "Ya existe un registro con el mismo nombre.");
                                        }
                                        else
                                        {
                                                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                                        }
                                }
                                catch (Exception exception)
                                {
                                        ModelState.AddModelError(string.Empty, exception.Message);
                                }
                        }
                        return View(model);
                }

                public async Task<IActionResult> Edit(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        Category category = await _context.Categories.FindAsync(id);
                        if (category == null)
                        {
                                return NotFound();
                        }

                        CategoryViewModel model = _converterHelper.ToCategoryViewModel(category);
                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(CategoryViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                Guid imageId = model.ImageId;

                                if (model.ImageFile != null)
                                {
                                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                                }

                                try
                                {
                                        Category category = _converterHelper.ToCategory(model, imageId, false);
                                        _context.Update(category);
                                        await _context.SaveChangesAsync();
                                        return RedirectToAction(nameof(Index));

                                }
                                catch (DbUpdateException dbUpdateException)
                                {
                                        if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                                        {
                                                ModelState.AddModelError(string.Empty, "Ya hay un registro con el mismo nombre.");
                                        }
                                        else
                                        {
                                                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                                        }
                                }
                                catch (Exception exception)
                                {
                                        ModelState.AddModelError(string.Empty, exception.Message);
                                }
                        }

                        return View(model);
                }

                public async Task<IActionResult> Delete(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        Category category = await _context.Categories
                            .FirstOrDefaultAsync(m => m.Id == id);
                        if (category == null)
                        {
                                return NotFound();
                        }

                        try
                        {
                                _context.Categories.Remove(category);
                                await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                                ModelState.AddModelError(string.Empty, ex.Message);
                        }

                        return RedirectToAction(nameof(Index));
                }


        }
}
