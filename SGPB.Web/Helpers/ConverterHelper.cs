using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using SGPB.Web.Models;
using System;
using System.Threading.Tasks;

namespace SGPB.Web.Helpers
{
        

        public class ConverterHelper : IConverterHelper
        {
                private readonly ApplicationDbContext _context;
                private readonly ICombosHelper _combosHelper;

                public ConverterHelper(ApplicationDbContext context, ICombosHelper combosHelper)
                {
                        _context = context;
                        _combosHelper = combosHelper;
                }


                public Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew)
                {
                        return new Category
                        {
                                Id = isNew ? 0 : model.Id,
                                ImageId = imageId,
                                Name = model.Name
                        };
                }

                public CategoryViewModel ToCategoryViewModel(Category category)
                {
                        return new CategoryViewModel
                        {
                                Id = category.Id,
                                ImageId = category.ImageId,
                                Name = category.Name
                        };
                }

                public async Task<Book> ToBookAsync(BookViewModel model, bool isNew)
                {
                        return new Book
                        {
                                Id = isNew ? 0 : model.Id,
                                Category = await _context.Categories.FindAsync(model.CategoryId),
                                Editorial = await _context.Editoriales.FindAsync(model.EditorialId),
                                Serial = model.Serial,
                                Name = model.Name,
                                Description = model.Description,
                                NumPages = model.NumPages,
                                NumCopies = model.NumCopies,
                                EditionDate = model.EditionDate,
                                IsActive = model.IsActive,
                                IsStarred = model.IsStarred,
                                BookImages = model.BookImages
                        };
                }

                public BookViewModel ToBookViewModel(Book book)
                {
                        return new BookViewModel
                        {
                                Categories = _combosHelper.GetComboCategories(),
                                Category = book.Category,
                                CategoryId = book.Category.Id,
                                Editoriales = _combosHelper.GetComboEditoriales(),
                                Editorial = book.Editorial,
                                EditorialId = book.Editorial.Id,
                                
                                Id = book.Id,
                                Serial = book.Serial,
                                Name = book.Name,
                                Description = book.Description,
                                NumPages = book.NumPages,
                                NumCopies = book.NumCopies,
                                EditionDate = book.EditionDate,
                                IsActive = book.IsActive,
                                IsStarred = book.IsStarred,
                                
                                BookImages = book.BookImages
                        };
                }

                public async Task<Lending> ToLendingAsync(LendingViewModel model, bool isNew)
                {
                        return new Lending
                        {
                                Id = isNew ? 0 : model.Id,
                                User = await _context.Users.FindAsync(model.UserId),
                                Book = await _context.Books.FindAsync(model.BookId),
                                Date = model.Date,
                                Quantity = model.Quantity,
                                Remarks = model.Remarks,
                                LendingStatus = model.LendingStatus
                        };
                }


                public LendingViewModel ToLendingViewModel(Lending lending)
                {
                        return new LendingViewModel
                        {

                                User = lending.User,
                                UserId = lending.User.Id,
                                Book = lending.Book,
                                BookId = lending.Book.Id,

                                Id = lending.Id,
                                Date = lending.Date,
                                Quantity = lending.Quantity,
                                Remarks = lending.Remarks,
                                LendingStatus = lending.LendingStatus
                        };
                }

                public async Task<Lending> ToAddBookToLendingAsync(AddBookToLendingViewModel model, string userId, bool isNew)
                {
                        return new Lending
                        {
                                Id = isNew ? 0 : model.Id,
                                Date = DateTime.Now,
                                Quantity = model.NumCopies,
                                Book = await _context.Books.FindAsync(model.BookId),
                                User = await _context.Users.FindAsync(model.UserId),
                                Remarks = model.Remarks,
                                LendingStatus = LendingStatus.New,
                        };
                }

        }

}
