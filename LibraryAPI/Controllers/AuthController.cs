using LibraryAPI.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LibraryAPI.Controllers
{
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(UserDto credentials)
        {
            return Ok(GenerateToken(credentials.Username));
        }

        private string GenerateToken(string username)
        {
            var secretKey = Encoding.UTF8.GetBytes("1234567890123456");
            var simmetricKey = new SymmetricSecurityKey(secretKey);

            var signingAlg = new SigningCredentials(simmetricKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId",username)
            };

            var token = new JwtSecurityToken(
                "yourIssuer", "yourAudience",
                claims,
                DateTime.Now, DateTime.Now.AddDays(1),
                signingAlg
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}