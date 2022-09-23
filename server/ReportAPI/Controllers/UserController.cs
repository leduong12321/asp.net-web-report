using ReportAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ReportAPI.Controllers
{
    [EnableCors(origins: "https://localhost:44315", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        User[] user = new User[]
        {
            new User { Id = 1, Name = "Admin", UserName = "admin", Password = "admin", Description = "Full access control", Role = 0 },
            new User { Id = 2, Name = "KTV", UserName = "ktv", Password = "ktv", Description = "Limited access", Role = 1 },
        };

        public dynamic Get()
        {
            return user;
        }
        [HttpPost]
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
    }
}
