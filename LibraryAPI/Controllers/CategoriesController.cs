using LibraryAPI.Dtos;
using LibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LibraryAPI.Controllers
{
    public class CategoriesController : ApiController
    {
        public async Task<List<CategoryDto>> Get()
        {
            using (var context = new LibraryEntities())
            {
                return await context.Categories
                    .Select(s=>new CategoryDto() { 
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToListAsync();
            }
        }
    }
}