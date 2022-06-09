using SGPB.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SGPB.Web.Models
{
        public class LendingViewModel: Lending
        {        
                [JsonIgnore]
                [NotMapped]
                public string UserId { get; set; }
              
                [JsonIgnore]
                [NotMapped]
                public int BookId { get; set; }
        }
}
