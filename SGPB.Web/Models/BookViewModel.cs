using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGPB.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SGPB.Web.Models
{
        public class BookViewModel : Book
        {
                [Display(Name = "Categoria")]
                [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]
                [Required]
                [JsonIgnore]
                [NotMapped]
                public int CategoryId { get; set; }

                [Display(Name = "Editorial")]
                [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una editorial.")]
                [Required]
                [JsonIgnore]
                [NotMapped]
                public int EditorialId { get; set; }
                public IEnumerable<SelectListItem> Categories { get; set; }

                public IEnumerable<SelectListItem> Editoriales { get; set; }

                [Display(Name = "Imagen")]
                public IFormFile ImageFile { get; set; }
        }
}
