using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using System;
using System.Threading.Tasks;

namespace SGPB.Web.Controllers
{
        [Authorize(Roles = "Admin")]
        public class EditorialesController  :  Controller
        {
                private readonly ApplicationDbContext _context;
          
                public EditorialesController(ApplicationDbContext context)
                {
                        _context = context;
                }

                public async Task<IActionResult> Index()
                {
                        return View(await _context.Editoriales.ToListAsync());
                }

                public IActionResult Create()
                {
                        return View();
                }

                
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(Editorial editorial)
                {
                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Add(editorial);
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
                        return View(editorial);
                }

                public async Task<IActionResult> Edit(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var editorial = await _context.Editoriales.FindAsync(id);
                        if (editorial == null)
                        {
                                return NotFound();
                        }
                        return View(editorial);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, Editorial editorial)
                {
                        if (id != editorial.Id)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(editorial);
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

                        return View(editorial);
                }

                public async Task<IActionResult> Delete(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }
                        Editorial editorial = await _context.Editoriales
                        .FirstOrDefaultAsync(m => m.Id == id);
                        if (editorial == null)
                        {
                                return NotFound();
                        }
                        _context.Editoriales.Remove(editorial);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }
        }
}
