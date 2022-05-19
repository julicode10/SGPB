using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Data.Entities
{
        public class Editorial
        {
                public int Id { get; set; }
                [Display(Name = "Categoría")]
                [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Name { get; set; }
        }
}
