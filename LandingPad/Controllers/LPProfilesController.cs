using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LandingPad.DAL;
using LandingPad.Models;
using System.Data.Entity.Infrastructure;

namespace LandingPad.Controllers
{
    [Authorize]
    public class LPProfilesController : Controller
    {
        private LandingPadContext db = new LandingPadContext();

        // GET: LPProfiles

        public ActionResult Index()
        {
            var lPProfiles = db.LPProfiles.Include(l => l.LPUser); 
            return View(lPProfiles.ToList());

            //var model = new ProfileUser();
            //model.LPProfile = db.LPProfiles.ToList();
            //model.LPUser = db.LPUsers.ToList();
            //model.Pseudonym = db.Pseudonyms.ToList();
            //return View(model);
        }

        // GET: LPProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LPProfile lPProfile = db.LPProfiles.Find(id);
            if (lPProfile == null)
            {
                return HttpNotFound();
            }
            return View(lPProfile);
        }

        // GET: LPProfiles/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email");
            return View();
        }

        // POST: LPProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileID,UserID,PseudonymID,Birthdate,PhoneNumber,LPDescription,ProfilePhoto,DisplayRealName,Friends,Followers,Writers,Pseudonym")] LPProfile lPProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.LPProfiles.Add(lPProfile);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Failed to create Profile");
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", lPProfile.UserID);
            return View(lPProfile);
        }

        // GET: LPProfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LPProfile lPProfile = db.LPProfiles.Find(id);

            if (lPProfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", lPProfile.UserID);
            return View(lPProfile);
        }

        // POST: LPProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileID,UserID,PseudonymID,Birthdate,PhoneNumber,LPDescription,ProfilePhoto,DisplayRealName,Friends,Followers,Writers,Pseudonym")] LPProfile lPProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    db.Entry(lPProfile).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Failed to edit Profile");
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", lPProfile.UserID);
            return View(lPProfile);
        }

        // GET: LPProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LPProfile lPProfile = db.LPProfiles.Find(id);
            if (lPProfile == null)
            {
                return HttpNotFound();
            }
            return View(lPProfile);
        }

        // POST: LPProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LPProfile lPProfile = db.LPProfiles.Find(id);
            db.LPProfiles.Remove(lPProfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
