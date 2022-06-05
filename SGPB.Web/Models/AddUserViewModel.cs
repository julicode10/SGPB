using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Models
{
        public class AddUserViewModel : EditUserViewModel
        {
                [Display(Name = "Correo electrónico")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [EmailAddress]
                public string Username { get; set; }

                [Display(Name = "Contraseña")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                [DataType(DataType.Password)]
                [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
                public string Password { get; set; }

                [Display(Name = "Confirmar contraseña")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                [DataType(DataType.Password)]
                [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
                [Compare("Password")]
                public string PasswordConfirm { get; set; }
        }
}
