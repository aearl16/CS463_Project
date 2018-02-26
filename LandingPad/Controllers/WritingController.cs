<<<<<<< HEAD
﻿using System;
=======
﻿using LandingPad.DAL;
using System;
>>>>>>> bbb2415edf38e44468055dbeb522865fda7ab03e
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Controllers
{
    [Authorize]
    public class WritingController : Controller
    {
<<<<<<< HEAD
        // GET: Writing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Document()
        {
            return View();
        }
=======
        LandingPadContext db = new LandingPadContext();

        // GET: Pseudonym
        public ActionResult Index()
        {
            return View(db.Writings.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Writing wr = db.Writings.Find(id);
            if (wr == null)
            {
                return HttpNotFound();
            }
            return View(wr);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileID, Title, Title, Document, AddDate, EditDate, LikesOn," +
        "CommentsOn, CritiqueOn, DocType, DescriptionText")] Writing wr)
        {
            if (ModelState.IsValid)
            {
                db.Writings.Add(wr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Writing wr = db.Writings.Find(id);
            if (wr == null)
            {
                return HttpNotFound();
            }
            return View(wr);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Writing wr = db.Writings.Find(id);
            if (wr == null)
            {
                return HttpNotFound();
            }
            return View(wr);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Writing wr = db.Writings.Find(id);
            db.Writings.Remove(wr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
>>>>>>> bbb2415edf38e44468055dbeb522865fda7ab03e
    }
}