using LandingPad.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Controllers
{
    public class ProfileController : Controller
    {
        LandingPadContext db = new LandingPadContext();

        // GET: Pseudonym
        public ActionResult Index()
        {
            return View(db.LPProfiles.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            LPProfile pf = db.LPProfiles.Find(id);
            if (pf == null)
            {
                return HttpNotFound();
            }
            return View(pf);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID, ProfilePhoto, DisplayRealName, Friends, Followers, Writers")] LPProfile pf)
        {
            if (ModelState.IsValid)
            {
                db.LPProfiles.Add(pf);
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

            LPProfile pf = db.LPProfiles.Find(id);
            if (pf == null)
            {
                return HttpNotFound();
            }
            return View(pf);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            LPProfile pf = db.LPProfiles.Find(id);
            if (pf == null)
            {
                return HttpNotFound();
            }
            return View(pf);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LPProfile pf = db.LPProfiles.Find(id);
            db.LPProfiles.Remove(pf);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}