using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Data.Entities
{
        public class DocumentType
        {
                public int Id { get; set; }

                [Display(Name = "Abreviatura")]
                [MaxLength(3, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Abbreviation { get; set; }

                [Display(Name = "Nombre")]
                [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Name { get; set; }
        }
}
