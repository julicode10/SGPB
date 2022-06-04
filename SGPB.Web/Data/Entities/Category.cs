using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Data.Entities
{
        public class Category
        {
                public int Id { get; set; }
                [Display(Name = "Categoría")]
                [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Name { get; set; }
                [Display(Name = "Imagen")]
                public Guid ImageId { get; set; }
                //TODO: Pending to put the correct paths
                [Display(Name = "Imagen")]

                public ICollection<Book> Books { get; set; }
                public string ImageFullPath => ImageId == Guid.Empty
                ? $"https://localhost:44369/images/noimage.png"
                : $"https://sistemabibliotecario.blob.core.windows.net/categories/{ImageId}"; // blob en Azure
        }
}
