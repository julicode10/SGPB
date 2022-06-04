using SGPB.Web.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SGPB.Web.Data.Entities
{
        public class Lending
        {
                public int Id { get; set; }

                [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
                [Display(Name = "Fecha")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public DateTime Date { get; set; }

                [Display(Name = "Fecha prestamo ó rechazado")]
                public DateTime? DateStatus { get; set; }

                public User User { get; set; }

                [DataType(DataType.MultilineText)]
                [Display(Name = "Observaciones")]
                public string? Remarks { get; set; }

                public LendingStatus LendingStatus { get; set; }

                public ICollection<LendingDetail> LendingDetails { get; set; }

                [DisplayFormat(DataFormatString = "{0:N0}")]
                [Display(Name = "Número de líneas")]
                public int Lines => LendingDetails == null ? 0 : LendingDetails.Count;

                [DisplayFormat(DataFormatString = "{0:N2}")]
                [Display(Name = "Cantidad")]
                public float Quantity => LendingDetails == null ? 0 : LendingDetails.Sum(sd => sd.Quantity);

        }
}
