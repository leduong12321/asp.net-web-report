using Microsoft.IdentityModel.Tokens;
using ReportAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ReportAPI.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        User[] user = new User[]
        {
            new User { Id = 1, Name = "Admin", UserName = "admin", Password = "admin", Description = "Truy cập đầy đủ", Role = 0 },
            new User { Id = 2, Name = "KTV", UserName = "ktv", Password = "ktv", Description = "Truy cập giới hạn", Role = 1 },
        };

        public dynamic Get()
        {
            return user;
        }
        public dynamic Post([FromBody] Dictionary<string, string> value)
        {
            try
            {
                dynamic people = null;
                foreach (dynamic item in user)
                {
                    if (item.UserName == value["Username"] && item.Password == value["Password"])
                    {
                        people = item;
                    }
                }
                return people;
            }
            catch (Exception)
            {
                return "error";
            }
        }


        [HttpGet]
        public Object GetToken()
        {
            string key = "my_secret_key_12345";
            var issuer = "http://mysite.com";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer,
                            issuer,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };


        }


        [HttpPost]
        public String GetName1()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                }
                return "Valid";
            }
            else
            {
                return "Invalid";
            }
        }

        [Authorize]
        [HttpPost]
        public Object GetName2()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                //var name = claims.Where(p => p.Type == "name").FirstOrDefault()?.Value;
                return new
                {
                    data = claims
                };

            }

            return null;
        }

    }
}
