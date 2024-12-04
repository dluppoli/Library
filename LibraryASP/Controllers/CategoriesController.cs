using LibraryASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LibraryASP.Controllers
{
    [Authorize]
    [RoutePrefix("Categories")]
    public class CategoriesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (LibraryEntities context = new LibraryEntities())
            {
                var results = await context.Categories.ToListAsync();
                return View(results);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            using (LibraryEntities context = new LibraryEntities())
            {
                var result = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if(result == null) return HttpNotFound();
                return View(result);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            using (LibraryEntities context = new LibraryEntities())
            {
                var result = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (result == null) return HttpNotFound();
                return View(result);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Category newData)
        {
            if( !ModelState.IsValid ) return HttpNotFound();

            using (LibraryEntities context = new LibraryEntities())
            {
                var result = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (result == null || result.Id!=newData.Id ) 
                    return HttpNotFound();

                result.Name = newData.Name;
                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Category newData)
        {
            if (newData == null || !ModelState.IsValid) return HttpNotFound();

            using (LibraryEntities context = new LibraryEntities())
            {
                context.Categories.Add(newData);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");   
            }
        }

        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            using (LibraryEntities context = new LibraryEntities())
            {
                var result = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (result == null) return HttpNotFound();
                return View(result);
            }
        }

        [HttpPost]
        [Route("Delete/{id}")]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            using (LibraryEntities context = new LibraryEntities())
            {
                var result = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (result == null) return HttpNotFound();
                
                context.Categories.Remove(result);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}