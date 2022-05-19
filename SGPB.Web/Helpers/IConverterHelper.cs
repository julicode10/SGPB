using SGPB.Web.Data.Entities;
using SGPB.Web.Models;
using System;

namespace SGPB.Web.Helpers
{
        public interface IConverterHelper
        {
                Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew);

                CategoryViewModel ToCategoryViewModel(Category category);
        }

}
