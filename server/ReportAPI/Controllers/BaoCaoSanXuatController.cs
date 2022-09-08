using ReportAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace ProductsApp.Controllers
{
    public class BaoCaoSanXuatController : ApiController
    {
        public string myData = "TRANG CHỦ";

        public string Get()
        {
            return myData;
        }
    }
}