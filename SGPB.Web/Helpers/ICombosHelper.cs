using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SGPB.Web.Helpers
{
        public interface ICombosHelper
        {
                IEnumerable<SelectListItem> GetComboCategories();
                IEnumerable<SelectListItem> GetComboEditoriales();

                IEnumerable<SelectListItem> GetComboDocumentTypes();
        }

}
