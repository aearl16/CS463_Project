using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace LandingPad.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        public string Cap(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}