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
        public ActionResult Create()
        {
            return View(db);
        }

        [HttpPost]
        public ActionResult Create(string[] FormatTags, string[] Pseudonyms, FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
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
                    Document = Encoding.Unicode.GetBytes(form["EditorContent"])
                };
                db.Writings.Add(wr);
                db.SaveChanges();

                int id = wr.WritingID;

                foreach(var selection in FormatTags)
                {
                    WritingFormat wf = new WritingFormat()
                    {
                        WritingID = id,
                        FormatID = Int32.Parse(selection)
                    };
                    db.WritingFormats.Add(wf);
                    db.SaveChanges();
                }

                foreach(var selection in Pseudonyms)
                {
                    WritingPseudonym wp = new WritingPseudonym()
                    {
                        WritingID = id,
                        PseudonymID = Int32.Parse(selection)
                    };
                    db.WritingPseudonyms.Add(wp);
                    db.SaveChanges();
                }

                return RedirectToAction("ViewWriting", "Writing", new { @id = id });
            }
            else
            {
                ViewBag.FileStatus = "Model Invalid";
                return View(db);
            }
        }

        [HttpGet]
        public ActionResult ViewWriting(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Writing");
            }
            Writing wr = db.Writings.Find(id);
            if (wr == null)
            {
                return HttpNotFound();
            }

            string doc = "";

            if (wr.DocType == "HTML")
            {
                doc = HTMLByteArrayToString(wr.Document);
            }

            ViewBag.Document = doc;

            string byline = "";

            if(wr.WritingPseudonyms.Count > 0)
            {
                byline = " by " + wr.WritingPseudonyms.First().Pseudonym.Pseudonym1;
            }
            else if(wr.LPProfile.DisplayRealName == true)
            {
                byline = " by " + wr.LPProfile.LPUser.FirstName + " " + wr.LPProfile.LPUser.LastName;
            }

            string pageTitle = wr.Title + byline;

            ViewBag.Title = pageTitle;

            if(byline.Length > 0)
            {
                byline = byline.TrimStart();
                byline = byline[0].ToString().ToUpper() + byline.Substring(1);
            }

            ViewBag.Byline = byline;

            return View(wr);
        }

        public string HTMLByteArrayToString(byte[] input)
        {
            if (input == null)
                return null;

            string output = Encoding.Unicode.GetString(input);
            output = output.Replace("&lt;", "<").Replace("&gt;", ">");

            return output;
        }
    }
}