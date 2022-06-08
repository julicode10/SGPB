using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Data.Entities
{
        public class Editorial
        {
                public int Id { get; set; }
                [Display(Name = "Nombre editorial")]
                [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Name { get; set; }

                public ICollection<Book> Books { get; set; }

                [Display(Name = "Numero de libros")]
                public int BooksNumber => Books == null ? 0 : Books.Count;
        }
}
