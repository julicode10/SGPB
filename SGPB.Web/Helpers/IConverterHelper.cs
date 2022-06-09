using SGPB.Web.Data.Entities;
using SGPB.Web.Models;
using System;
using System.Threading.Tasks;

namespace SGPB.Web.Helpers
{
        public interface IConverterHelper
        {
                Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew);

                CategoryViewModel ToCategoryViewModel(Category category);

                Task<Book> ToBookAsync(BookViewModel model, bool isNew);

                BookViewModel ToBookViewModel(Book book);

                Task<Lending> ToLendingAsync(LendingViewModel model, bool isNew);

                LendingViewModel ToLendingViewModel(Lending lending);

                Task<Lending> ToAddBookToLendingAsync(AddBookToLendingViewModel model, string userId, bool isNew);

        }

}
