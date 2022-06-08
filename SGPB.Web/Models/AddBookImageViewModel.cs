using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Models
{
        public class AddBookImageViewModel
        {
                public int BookId { get; set; }

                [Display(Name = "Imagen")]
                [Required]
                public IFormFile ImageFile { get; set; }

        }
}
