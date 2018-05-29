using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Controllers
{
    [RequireHttps]
    public class ErrorController : Controller
    {
        // GET: Error
        [HttpGet]
        public ActionResult EditError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DeleteError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DownloadErorr()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InternalError()
        {
            return View();
        }

       [HttpGet]
       public ActionResult ProfileEditError()
        {
            return View();
        }
    }
}