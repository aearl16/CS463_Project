using LandingPad.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandingPad.Models;
using System.Text;

namespace LandingPad.Controllers
{
    [Authorize]
    public class WritingController : Controller
    {
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

        //[HttpPost]
        //public ActionResult Create(FormCollection form)
        //{
        //    DateTime AddDate = DateTime.Now;
        //    int ProfileID = Int32.Parse(form["ProfileID"]);
        //    string Title = form["Title"];
        //    string DocType = ".HTML";
        //    bool LikesOn = Boolean.Parse(form["LikesOn"]);
        //    bool CommentsOn = Boolean.Parse(form["CommentsOn"]);
        //    bool CritiqueOn = Boolean.Parse(form["CritiqueOn"]);
        //    string DescriptionText = form["DescriptionText"];

        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileID, Title, Document, AddDate, LikesOn," +
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

        public PartialViewResult Editor()
        {
            return PartialView();
        }

        public PartialViewResult _SelectAuthor()
        {
            return PartialView(db.LPProfiles.ToList());
        }

        public PartialViewResult _SelectFormat()
        {
            return PartialView(db.FormatTags.ToList());
        }

        public PartialViewResult _SelectPermissions()
        {
            return PartialView(db);
        }

        public PartialViewResult _Confirmation()
        {
            return PartialView(db);
        }
        
        public PartialViewResult _Menu()
        {
            return PartialView(db);
        }

        [HttpGet]
        public ActionResult Test()
        {
            return View(db);
        }

        [HttpPost]
        public ActionResult Test(System.Web.Mvc.FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing ed)
        {
            if (!ModelState.IsValid)
            {
                Writing wr = new Writing()
                {
                    ProfileID = Int32.Parse(form["ProfileID"]),
                    Title = form["Title"],
                    AddDate = DateTime.Now,
                    EditDate = null,
                    LikesOn = form["LikesOn"] != null ? true : false,
                    CommentsOn = form["CommentsOn"] != null ? true : false,
                    CritiqueOn = form["CritiqueOn"] != null ? true : false,
                    DocType = form["DocType"],
                    DescriptionText = form["DescriptionText"],
                    Document = Encoding.UTF8.GetBytes(form["EditorContent"])
                };
                db.Writings.Add(wr);
                db.SaveChanges();
                return View(db);
            }
            else
            {
                ViewBag.FileStatus = "Model Invalid";
                return View(db);
            }
        }
    }
}