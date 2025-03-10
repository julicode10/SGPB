﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Controllers
{
        [Authorize(Roles = "Admin")]
        public class DocumentTypesController : Controller
        {
                private readonly ApplicationDbContext _context;

                public DocumentTypesController(ApplicationDbContext context)
                {
                        _context = context;
                }

                public async Task<IActionResult> Index()
                {
                        return View(await _context.DocumentTypes
                                .OrderByDescending(b => b.Id)
                                .ToListAsync());
                }

                public IActionResult Create()
                {
                        return View();
                }


                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(DocumentType documentType)
                {
                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Add(documentType);
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
                                                ModelState.AddModelError(string.Empty,
                                               dbUpdateException.InnerException.Message);
                                        }
                                }
                                catch (Exception exception)
                                {
                                        ModelState.AddModelError(string.Empty, exception.Message);
                                }
                        }
                        return View(documentType);
                }

                public async Task<IActionResult> Edit(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var documentType = await _context.DocumentTypes.FindAsync(id);
                        if (documentType == null)
                        {
                                return NotFound();
                        }
                        return View(documentType);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, DocumentType documentType)
                {
                        if (id != documentType.Id)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(documentType);
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

                        return View(documentType);
                }

                public async Task<IActionResult> Delete(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }
                        DocumentType documentType = await _context.DocumentTypes
                        .FirstOrDefaultAsync(m => m.Id == id);
                        if (documentType == null)
                        {
                                return NotFound();
                        }
                        _context.DocumentTypes.Remove(documentType);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }
        }
}
