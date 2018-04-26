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

namespace LandingPad.Controllers
{
    [Authorize]
    public class LPUsersController : Controller
    {
        private LandingPadContext db = new LandingPadContext();

        // GET: LPUsers
        public ActionResult Index()
        {
            return View(db.LPUsers.ToList());
        }

        // GET: LPUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LPUser lPUser = db.LPUsers.Find(id);
            if (lPUser == null)
            {
                return HttpNotFound();
            }
            return View(lPUser);
        }

        // GET: LPUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LPUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Email,Birthdate,GivenName,Surname,PhoneNumber,Username")] LPUser lPUser)
        {
            if (ModelState.IsValid)
            {
                db.LPUsers.Add(lPUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lPUser);
        }

        // GET: LPUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LPUser lPUser = db.LPUsers.Find(id);
            if (lPUser == null)
            {
                return HttpNotFound();
            }
            return View(lPUser);
        }

        // POST: LPUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Email,Birthdate,GivenName,Surname,PhoneNumber,Username")] LPUser lPUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lPUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lPUser);
        }

        // GET: LPUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LPUser lPUser = db.LPUsers.Find(id);
            if (lPUser == null)
            {
                return HttpNotFound();
            }
            return View(lPUser);
        }

        // POST: LPUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LPUser lPUser = db.LPUsers.Find(id);
            db.LPUsers.Remove(lPUser);
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
