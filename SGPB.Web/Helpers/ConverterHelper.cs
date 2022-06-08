using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
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


        }

}
