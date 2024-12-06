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
        public async Task<List<BookDto>> Get(int page=1, string search="")
        {
            int pageSize = 10;
            var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            using (var context = new LibraryEntities())
            {
                int resultsNumber = await context.Books
                    .Where(w => search == "" || w.Title.Contains(search) || w.Author.Contains(search))
                    .CountAsync();


                double pages = Math.Ceiling((double)resultsNumber / pageSize);
                if (page > pages) page = 1;


                return await context.Books
                    .Where(w => search == "" || w.Title.Contains(search) || w.Author.Contains(search))
                    .OrderBy(ob=>ob.Id)
                    .Skip( (page-1)*pageSize )
                    .Take(pageSize)
                    .ProjectTo<BookDto>(conf)
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
    
        public async Task<IHttpActionResult> Post(BookDto nuovoDto)
        {
            var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            var mapper = new Mapper(conf);
            using (var context = new LibraryEntities())
            {
                var nuovo = mapper.Map<Book>(nuovoDto);
                context.Books.Add(nuovo);
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        public async Task<IHttpActionResult> Put(int id, BookDto nuovoDto)
        {
            var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            var mapper = new Mapper(conf);
            using (var context = new LibraryEntities())
            {
                if (id != nuovoDto.Id) return BadRequest();
                if (id <= 0) return BadRequest();

                var candidate = await context.Books.FirstOrDefaultAsync(q => q.Id == id);
                if (candidate == null) return NotFound();

                candidate.Title = nuovoDto.Title;
                candidate.Author = nuovoDto.Author;
                candidate.Price = nuovoDto.Price;
                candidate.CategoryId = nuovoDto.Category.Id;

                await context.SaveChangesAsync();
                return Ok();
            }
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            using (var context = new LibraryEntities())
            {
                if (id <= 0) return BadRequest();

                var candidate = await context.Books.FirstOrDefaultAsync(q => q.Id == id);
                if (candidate == null) return NotFound();

                context.Books.Remove(candidate);
                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}