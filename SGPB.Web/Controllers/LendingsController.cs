using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using SGPB.Web.Helpers;
using SGPB.Web.Models;

namespace SGPB.Web.Controllers
{
        public class LendingsController : Controller
        {


                private readonly ApplicationDbContext _context;
                private readonly IBlobHelper _blobHelper;
                private readonly ICombosHelper _combosHelper;
                private readonly IConverterHelper _converterHelper;
                private readonly IUserHelper _userHelper;

                public LendingsController(ApplicationDbContext context, IBlobHelper blobHelper, ICombosHelper combosHelper, IConverterHelper converterHelper, IUserHelper userHelper)
                {
                        _context = context;
                        _blobHelper = blobHelper;
                        _combosHelper = combosHelper;
                        _converterHelper = converterHelper;
                        _userHelper = userHelper;
                }

                public async Task<IActionResult> Index()
                {
                        User user = await _userHelper.GetUserAsync(User.Identity.Name);
                        if (user == null)
                        {
                                return NotFound();
                        }
                        if (User.Identity.IsAuthenticated && User.IsInRole("Admin")){
                                return View(await _context.Lendings
                            .Include(b => b.User)
                            .Include(b => b.Book)
                            .OrderByDescending(b => b.Id)
                              .ToListAsync());
                        }
                        else
                        {
                                return View(await _context.Lendings
                            .Include(b => b.User)
                            .Include(b => b.Book)
                            .Where(ts => ts.User.Id == user.Id)
                            .OrderByDescending(b => b.Id)
                              .ToListAsync());
                        }

                }

                public async Task<IActionResult> Create(int? id)
                {

                        if (id == null)
                        {
                                return NotFound();
                        }

                        if (!User.Identity.IsAuthenticated)
                        {
                                return RedirectToAction("Login", "Account");
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

                        User user = await _userHelper.GetUserAsync(User.Identity.Name);
                        if (user == null)
                        {
                                return NotFound();
                        }

                        AddBookToLendingViewModel model = new()
                        {
                                BookImages = book.BookImages,
                                Serial = book.Serial,
                                Name = book.Name,
                                Description = book.Description,
                                NumCopies = book.NumCopies,
                                NumPages = book.NumPages,
                                IsActive = book.IsActive,
                                IsStarred = book.IsStarred,
                                ImageFullPath = book.ImageFullPath,
                                BookId = book.Id,
                                User = user
                        };




                        return View(model);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(AddBookToLendingViewModel model)
                {

                        User user = await _userHelper.GetUserAsync(User.Identity.Name);
                        if (user == null)
                        {
                                return NotFound();
                        }

                        try
                        {
                                model.User = user;
                                //Lending lending = await _converterHelper.ToAddBookToLendingAsync(model, user.Id, true);

                                Lending lending = new()
                                {
                                        Date = DateTime.UtcNow,
                                        User = model.User,
                                        Remarks = model.Remarks,
                                        LendingStatus = LendingStatus.New,
                                        Book = await _context.Books.FindAsync(model.BookId),
                                        Quantity = model.NumCopies
                                };
                                _context.Lendings.Add(lending);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                        }
                        catch (Exception exception)
                        {
                                return View(model);
                        }


                }

                public async Task<IActionResult> Delete(int? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        Lending lending = await _context.Lendings
                            .FirstOrDefaultAsync(p => p.Id == id);
                        if (lending == null)
                        {
                                return NotFound();
                        }
                        try
                        {
                                _context.Lendings.Remove(lending);
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
                        Lending lending = await _context.Lendings
                            .Include(b => b.User)
                            .Include(b => b.Book)
                            .FirstOrDefaultAsync(m => m.Id == id);
                        if (lending == null)
                        {
                                return NotFound();
                        }
                        return View(lending);
                }
        }


}
