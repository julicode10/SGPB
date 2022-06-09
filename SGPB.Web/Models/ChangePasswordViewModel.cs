using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Models
{
        public class ChangePasswordViewModel
        {
                [Display(Name = "Contraseña actual")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                [DataType(DataType.Password)]
                [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
                public string OldPassword { get; set; }

                [Display(Name = "Nueva contraseña")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                [DataType(DataType.Password)]
                [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
                public string NewPassword { get; set; }

                [Display(Name = "Confirmar contraseña")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                [DataType(DataType.Password)]
                [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
                [Compare("NewPassword")]
                public string Confirm { get; set; }

        }
}
