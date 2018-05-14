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
using LandingPad.Repositories;

namespace LandingPad.Controllers
{
    [RequireHttps]
    [Authorize]
    public class TwittersController : Controller
    {
        private LandingPadContext db = new LandingPadContext();
        private ITwitterRepository twitterRepo;

        public TwittersController(ITwitterRepository twitterRepository)
        {
            this.twitterRepo = twitterRepository;
        }

        public bool CreatedMoq()
        {
            if(this.twitterRepo.GetAll().Count() < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckSquareOne(int id)
        {
           if(this.twitterRepo.GetTwitterId(id) == id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Twitters
        public ActionResult Index()
        {
            var twitters = db.Twitters.Include(t => t.LPUser);
            return View(twitters.ToList());
        }

        // GET: Twitters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Twitter twitter = db.Twitters.Find(id);
            if (twitter == null)
            {
                return HttpNotFound();
            }
            return View(twitter);
        }

        // GET: Twitters/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email");
            return View();
        }

        // POST: Twitters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TwitterID,UserID,TwName,TwTag,TwOauth,TwVOauth")] Twitter twitter)
        {
            if (ModelState.IsValid)
            {
                db.Twitters.Add(twitter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", twitter.UserID);
            return View(twitter);
        }

        // GET: Twitters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Twitter twitter = db.Twitters.Find(id);
            if (twitter == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", twitter.UserID);
            return View(twitter);
        }

        // POST: Twitters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TwitterID,UserID,TwName,TwTag,TwOauth,TwVOauth")] Twitter twitter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(twitter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", twitter.UserID);
            return View(twitter);
        }

        // GET: Twitters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Twitter twitter = db.Twitters.Find(id);
            if (twitter == null)
            {
                return HttpNotFound();
            }
            return View(twitter);
        }

        // POST: Twitters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Twitter twitter = db.Twitters.Find(id);
            db.Twitters.Remove(twitter);
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
