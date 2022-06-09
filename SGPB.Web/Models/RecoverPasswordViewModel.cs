using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Models
{
        public class RecoverPasswordViewModel
        {
                [Display(Name = "Correo")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
                public string Email { get; set; }
        }
}
