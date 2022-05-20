using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGPB.Web.Helpers
{
        public interface ICombosHelper
        {
                IEnumerable<SelectListItem> GetComboCategories();
                IEnumerable<SelectListItem> GetComboEditoriales();

                Task<IEnumerable<SelectListItem>> GetComboDocumentTypesAsync();
        }

}
