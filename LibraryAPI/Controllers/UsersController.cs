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
    public class UsersController : ApiController
    {
        public async Task<List<UserDto>> Get()
        {
            using(var context = new LibraryEntities())
            {
                return await context
                    .Users
                    .Select(s=> new UserDto() { 
                        Username=s.Username
                    })
                    .ToListAsync();
            }
        }
    }
}