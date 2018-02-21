using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Controllers
{
    public class WritingController : Controller
    {
        // GET: Writing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Document()
        {
            return View();
        }
    }
}