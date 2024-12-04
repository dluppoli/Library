using LibraryASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryASP.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login(string returnUrl="/")
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(User credentials, string returnUrl="/")
        {
            using (LibraryEntities context = new LibraryEntities())
            {
                var result = await context.Users.FirstOrDefaultAsync(q => q.Username == credentials.Username && q.Password == credentials.Password);
                if (result == null)
                {
                    TempData["LoginError"] = 1;
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData.Remove("LoginError");
                    FormsAuthentication.SetAuthCookie(result.Username, false);
                    return Redirect(returnUrl);
                }
            }
        }

        [Authorize]
        public ActionResult Logout() { 
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}