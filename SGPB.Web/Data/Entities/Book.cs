﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SGPB.Web.Data.Entities
{
        public class Book
        {
                public int Id { get; set; }

                [Display(Name = "ISBN")]
                [MaxLength(15, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Serial { get; set; }

                [Display(Name = "Nombre")]
                [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public string Name { get; set; }


                [DataType(DataType.MultilineText)]
                [Display(Name = "Descripción")]
                [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
                public string Description { get; set; }

                [Display(Name = "Número de páginas")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public int NumPages { get; set; }

                [Display(Name = "Número de copias")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public int NumCopies { get; set; }

                [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
                [Display(Name = "Fecha")]
                [Required(ErrorMessage = "El campo {0} es obligatorio.")]
                public DateTime EditionDate { get; set; }

                [Display(Name = "Está activo")]
                public bool IsActive { get; set; }
                [Display(Name = "Está protagonizado")]
                public bool IsStarred { get; set; }

                public Category Category { get; set; }

                public Editorial Editorial { get; set; }

                public ICollection<BookImage> BookImages { get; set; }

                public ICollection<LendingDetail> LendingDetails { get; set; }

                [Display(Name = "Número de imágenes del libro")]
                public int BookImagesNumber => BookImages == null ? 0 : BookImages.Count;


                //TO DO: Pendiente cambiar los paths por los de Azure
                [Display(Name = "Imagen")]
                public string ImageFullPath => BookImages == null || BookImages.Count == 0
                ? $"https://localhost:44369/images/noimage.png"
                : BookImages.FirstOrDefault().ImageFullPath;

        }
}
