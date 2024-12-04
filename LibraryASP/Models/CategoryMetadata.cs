using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryASP.Models
{
    public class CategoryMetadata
    {
        [Display(Name = "Categoria")]
        [StringLength(25, ErrorMessage = "Lunghezza Massima 25")]
        [Required]
        public string Name { get; set; }
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    { }
}