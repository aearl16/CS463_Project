using LandingPad.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Controllers
{
    public class PseudonymController : Controller
    {
        LandingPadContext db = new LandingPadContext();

        // GET: Pseudonym
        public ActionResult Index()
        {
            return View(db.Pseudonyms.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Pseudonym pn = db.Pseudonyms.Find(id);
            if (pn == null)
            {
                return HttpNotFound();
            }
            return View(pn);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileID, Pseudonym")] Pseudonym ps)
        {
            if (ModelState.IsValid)
            {
                db.Pseudonyms.Add(ps);
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

            Pseudonym ps = db.Pseudonyms.Find(id);
            if (ps == null)
            {
                return HttpNotFound();
            }
            return View(ps);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Pseudonym ps = db.Pseudonyms.Find(id);
            if (ps == null)
            {
                return HttpNotFound();
            }
            return View(ps);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pseudonym ps = db.Pseudonyms.Find(id);
            db.Pseudonyms.Remove(ps);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}