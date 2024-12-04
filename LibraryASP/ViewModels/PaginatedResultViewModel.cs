using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryASP.ViewModels
{
    public class PaginatedResultViewModel<T>
    {
        public List<T> results;
        public string search = "";
        public int page = 1;
        public int pages = 1;
    }
}