using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LandingPad.DAL;

namespace LandingPad.Controllers
{
    [Authorize]
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
            try
            {
                if (ModelState.IsValid)
                {
                    db.LPProfiles.Add(pf);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(".", "An Error occured, please try again or contact our Admin on our Contact Page.");
            }
            return View(pf);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileID,UserID,ProfilePhoto,DisplayRealName,Friends,Followers,Writers")] LPProfile lPProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lPProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", lPProfile.UserID);
            return View(lPProfile);
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