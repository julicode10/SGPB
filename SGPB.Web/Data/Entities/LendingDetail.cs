using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Data.Entities
{
        public class LendingDetail
        {
                public int Id { get; set; }

                public Book Book { get; set; }

                [Display(Name = "Cantidad de copias")]
                public float Quantity { get; set; }


                [DataType(DataType.MultilineText)]
                [Display(Name = "Comentario")]
                public string Remarks { get; set; }
        }
}
