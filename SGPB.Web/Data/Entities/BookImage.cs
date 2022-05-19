using System;
using System.ComponentModel.DataAnnotations;

namespace SGPB.Web.Data.Entities
{
        public class BookImage
        {
                public int Id { get; set; }

                [Display(Name = "Foto")]
                public Guid ImageId { get; set; }
                //TODO: Pending to put the correct path
                [Display(Name = "Foto")]
                public string ImageFullPath => ImageId == Guid.Empty
                    ? $"https://localhost:44369/images/noimage.png"
                    : $"https://sistemabibliotecario.blob.core.windows.net/books/{ImageId}";
        }
}
