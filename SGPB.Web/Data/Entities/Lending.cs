using SGPB.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Data.Entities
{
        public class Lending
        {
                public int Id { get; set; }

                [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
                [Display(Name = "Fecha")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public DateTime Date { get; set; }

                [Display(Name = "Número de copias")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public int Quantity { get; set; }

                public Book Book { get; set; }

                public User User { get; set; }


                [Display(Name = "Observaciones")]
                public string? Remarks { get; set; }

                public LendingStatus LendingStatus { get; set; }


        }
}
