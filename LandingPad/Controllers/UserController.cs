using LandingPad.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        LandingPadContext db = new LandingPadContext();

        // GET: Pseudonym
        public ActionResult Index()
        {
            return View(db.LPUsers.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            LPUser usr = db.LPUsers.Find(id);
            if (usr == null)
            {
                return HttpNotFound();
            }
            return View(usr);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email, Birthdate, FirstName, LastName, PhoneNumber, Username")] LPUser usr)
        {
            if (ModelState.IsValid)
            {
                db.LPUsers.Add(usr);
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

            LPUser usr = db.LPUsers.Find(id);
            if (usr == null)
            {
                return HttpNotFound();
            }
            return View(usr);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            LPUser usr = db.LPUsers.Find(id);
            if (usr == null)
            {
                return HttpNotFound();
            }
            return View(usr);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LPUser usr = db.LPUsers.Find(id);
            db.LPUsers.Remove(usr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}