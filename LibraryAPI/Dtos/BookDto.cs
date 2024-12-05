using LibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryAPI.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        //public int CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
    }
}