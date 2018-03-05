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

        public ActionResult Contact()
        {
            string fbId;
            var fileStream = new FileStream(@"C:\Users\Rahevin\Desktop\LandingPad\fbId.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                fbId = streamReader.ReadToEnd();
            }
            ViewBag.Message = fbId;
            return View();
        }
    }
}