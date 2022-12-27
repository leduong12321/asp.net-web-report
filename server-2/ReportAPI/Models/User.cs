using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int Role { get; set; }

    }
}