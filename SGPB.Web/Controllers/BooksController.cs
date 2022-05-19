using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Helpers;
using SGPB.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Controllers
{
        public class BooksController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly IBlobHelper _blobHelper;
                private readonly ICombosHelper _combosHelper;
                private readonly IConverterHelper _converterHelper;

                public BooksController(ApplicationDbContext context, IBlobHelper blobHelper, ICombosHelper combosHelper, IConverterHelper converterHelper)
                {
                        _context = context;
                        _blobHelper = blobHelper;
                        _combosHelper = combosHelper;
                        _converterHelper = converterHelper;
                }

                public async Task<IActionResult> Index()
                {
                        return View(await _context.Books
                            .Include(c => c.Category)
                            .Include(e => e.Editorial)
                            .Include(p => p.BookImages)
                              .ToListAsync());
                }

                public IActionResult Create()
                {
                        BookViewModel model = new BookViewModel
                        {
                                Categories = _combosHelper.GetComboCategories(),
                                Editoriales = _combosHelper.GetComboEditoriales(),
                                IsActive = true,
                                IsStarred = true
                        };

                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(BookViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        Book book = await _converterHelper.ToBookAsync(model, true);

                                        if (model.ImageFile != null)
                                        {
                                                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "books");
                                                book.BookImages = new List<BookImage>
                                                {
                                                    new BookImage { ImageId = imageId }
                                                };
                                        }


                                        _context.Add(book);
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

                        model.Categories = _combosHelper.GetComboCategories();
                        model.Editoriales = _combosHelper.GetComboEditoriales();
                        return View(model);
                }
                public async Task<IActionResult> Edit(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        Book book = await _context.Books
                            .Include(p => p.Category)
                            .Include(e => e.Editorial)
                            .Include(p => p.BookImages)
                            .FirstOrDefaultAsync(p => p.Id == id);
                        if (book == null)
                        {
                                return NotFound();
                        }

                        BookViewModel model = _converterHelper.ToBookViewModel(book);
                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(BookViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        Book book = await _converterHelper.ToBookAsync(model, false);

                                        if (model.ImageFile != null)
                                        {
                                                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "books");
                                                if (book.BookImages == null)
                                                {
                                                        book.BookImages = new List<BookImage>();
                                                }

                                                book.BookImages.Add(new BookImage { ImageId = imageId });
                                        }

                                        _context.Update(book);
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

                        model.Categories = _combosHelper.GetComboCategories();
                        model.Editoriales = _combosHelper.GetComboEditoriales();
                        return View(model);
                }

                public async Task<IActionResult> Delete(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        Book book = await _context.Books
                            .Include(p => p.BookImages)
                            .FirstOrDefaultAsync(p => p.Id == id);
                        if (book == null)
                        {
                                return NotFound();
                        }

                        try
                        {
                                _context.Books.Remove(book);
                                await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                                ModelState.AddModelError(string.Empty, ex.Message);
                        }

                        return RedirectToAction(nameof(Index));
                }

                public async Task<IActionResult> Details(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }
                        Book book = await _context.Books
                            .Include(c => c.Category)
                            .Include(c => c.Editorial)
                            .Include(c => c.BookImages)
                            .FirstOrDefaultAsync(m => m.Id == id);
                        if (book == null)
                        {
                                return NotFound();
                        }
                        return View(book);
                }

                public async Task<IActionResult> AddImage(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        Book Book = await _context.Books.FindAsync(id);

                        if (Book == null)
                        {
                                return NotFound();
                        }

                        AddBookImageViewModel model = new AddBookImageViewModel { BookId = Book.Id };
                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> AddImage(AddBookImageViewModel model)
                {
                        if (ModelState.IsValid)
                        {
                                Book book = await _context.Books
                                    .Include(p => p.BookImages)
                                    .FirstOrDefaultAsync(p => p.Id == model.BookId);
                                if (book == null)
                                {
                                        return NotFound();
                                }

                                try
                                {
                                        Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "books");
                                        if (book.BookImages == null)
                                        {
                                                book.BookImages = new List<BookImage>();
                                        }

                                        book.BookImages.Add(new BookImage { ImageId = imageId });
                                        _context.Update(book);
                                        await _context.SaveChangesAsync();
                                        return RedirectToAction(nameof(Details), new { Id = book.Id });

                                }
                                catch (Exception exception)
                                {
                                        ModelState.AddModelError(string.Empty, exception.Message);
                                }
                        }

                        return View(model);
                }

                public async Task<IActionResult> DeleteImage(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        BookImage bookImage = await _context.BookImages
                            .FirstOrDefaultAsync(m => m.Id == id);
                        if (bookImage == null)
                        {
                                return NotFound();
                        }

                        Book book = await _context.Books.FirstOrDefaultAsync(p => p.BookImages.FirstOrDefault(pi => pi.Id == bookImage.Id) != null);
                        _context.BookImages.Remove(bookImage);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { Id = book.Id });
                }


        }
}
