using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Models
{
        public class AddBookImageViewModel
        {
                public int BookId { get; set; }

                [Display(Name = "Image")]
                [Required]
                public IFormFile ImageFile { get; set; }

        }
}
