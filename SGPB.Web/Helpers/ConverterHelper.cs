using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Models;
using System;

namespace SGPB.Web.Helpers
{
        public class ConverterHelper : IConverterHelper
        {
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
        }

}
