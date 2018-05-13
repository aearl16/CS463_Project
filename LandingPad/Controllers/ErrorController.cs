﻿using System;
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
    }
}