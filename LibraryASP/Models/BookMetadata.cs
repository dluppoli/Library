using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryASP.Models
{
    public class BookMetadata
    {
        [Display(Name ="Titolo")]
        public string Title { get; set; }

        [Display(Name = "Autore")]
        public string Author { get; set; }

        [Display(Name = "Prezzo")]
        public double Price { get; set; }

        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }
    }

    [MetadataType(typeof(BookMetadata))]
    public partial class Book
    { }
}