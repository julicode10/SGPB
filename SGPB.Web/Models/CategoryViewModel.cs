using Microsoft.AspNetCore.Http;
using SGPB.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Models
{
        public class CategoryViewModel : Category
        {
                [Display(Name = "Imagen")]
                public IFormFile ImageFile { get; set; }
        }
}
