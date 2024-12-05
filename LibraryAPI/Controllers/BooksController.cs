using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class BooksController : ApiController
    {
        public async Task<List<BookDto>> Get()
        {
            var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            using (var context = new LibraryEntities())
            {
                return await context.Books
                    .ProjectTo<BookDto>(conf)
                    /*.Select(s => new BookDto()
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Author = s.Author,
                        Price = s.Price,
                        Category = new CategoryDto()
                        {
                            Id = s.Category.Id,
                            Name = s.Category.Name
                        }
                    })*/
                    .ToListAsync();
            }
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id deve essere positivo.");

                var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
                var mapper = new Mapper(conf);
                using (var context = new LibraryEntities())
                {
                    var result = await context.Books
                        .Where(x => x.Id == id)
                        .FirstOrDefaultAsync();

                    if (result == null)
                        return NotFound();

                    return Ok(mapper.Map<BookDto>(result));
                }
            }
            catch (Exception ex) { 
                return BadRequest("Formato errato");
            }
        }
    }
}