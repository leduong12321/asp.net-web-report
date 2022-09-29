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
    }
}
