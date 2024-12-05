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
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            using (var context = new LibraryEntities())
            {
                return Ok(await context.Categories
                    .Select(s=>new CategoryDto() { 
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToListAsync());
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
                    var result = await context.Categories
                        .Where(x => x.Id == id)
                        .FirstOrDefaultAsync();

                    if (result == null)
                        return NotFound();

                    return Ok(mapper.Map<CategoryDto>(result));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Formato errato");
            }
        }

        [Route("{id}/books")]
        public async Task<IHttpActionResult> GetBooksByCategory(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id deve essere positivo.");

                var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());

                using (var context = new LibraryEntities())
                {
                    return Ok(await context.Books
                        .Where(x => x.CategoryId == id)
                        .ProjectTo<BookDto>(conf)
                        .ToListAsync());
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Formato errato");
            }
        }

        public async Task<IHttpActionResult> Post([FromBody] CategoryDto nuovoDto)
        {
            if (nuovoDto == null) return BadRequest();

            var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            var mapper = new Mapper(conf);

            var nuovo = mapper.Map<Category>(nuovoDto);

            using (var context = new LibraryEntities())
            {
                context.Categories.Add(nuovo);
                await context.SaveChangesAsync();
                return Ok( mapper.Map<CategoryDto>(nuovo) );
            }
        }

        public async Task<IHttpActionResult> Put(int id,[FromBody] CategoryDto nuovoDto)
        {
            if (nuovoDto == null) return BadRequest();
            if( id!=nuovoDto.Id) return BadRequest();

            var conf = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            var mapper = new Mapper(conf);

            using (var context = new LibraryEntities())
            {
                var candidate = await context.Categories
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (candidate == null)
                    return NotFound();

                candidate.Name = nuovoDto.Name;

                await context.SaveChangesAsync();
                return Ok( nuovoDto );
            }
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id deve essere positivo.");

                using (var context = new LibraryEntities())
                {
                    var candidate = await context.Categories
                        .Where(x => x.Id == id)
                        .FirstOrDefaultAsync();

                    if (candidate == null)
                        return NotFound();

                    context.Categories.Remove(candidate);
                    await context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Formato errato");
            }
        }
    }
}