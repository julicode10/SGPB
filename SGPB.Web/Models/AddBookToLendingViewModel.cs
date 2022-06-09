using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Models
{
        public class AddBookToLendingViewModel
        {
                public int Id { get; set; }

                [Display(Name = "Nombre")]
                [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Name { get; set; }

                [Display(Name = "ISBN")]
                public string Serial { get; set; }

                public ICollection<BookImage> BookImages { get; set; }

                [DataType(DataType.MultilineText)]
                [Display(Name = "Descripción")]
                [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                public string Description { get; set; }

                [DataType(DataType.MultilineText)]
                [Display(Name = "Comentarios")]
                public string? Remarks { get; set; }
                

                [Display(Name = "Número de páginas")]
                public int NumPages { get; set; }

                [Display(Name = "Número de copias")]
                public int NumCopies { get; set; }

                
                [Display(Name = "Fecha")]
                public DateTime Date { get; set; }

                [Display(Name = "Está activo")]
                public bool IsActive { get; set; }
                [Display(Name = "Está protagonizado")]
                public bool IsStarred { get; set; }
               
                public Category Category { get; set; }
                public Editorial Editorial { get; set; }

                public string ImageFullPath { get; set; }

                public int BookId { get; set; }

                public string UserId { get; set; }
                public LendingStatus LendingStatus { get; set; }

                public User User { get; set; }



        }
}
