using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Request
{
        public class EmailRequest
        {
                [EmailAddress]
                [Required]
                public string Email { get; set; }

        }
}
