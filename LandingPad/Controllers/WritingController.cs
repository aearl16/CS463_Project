using LandingPad.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandingPad.Models;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.Entity;

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

            string doc = "";

            if (wr.DocType == "HTML")
            {
                doc = HTMLByteArrayToString(wr.Document);
            }

            ViewBag.Document = doc;

            ViewBag.ID = id;

            return View(db);
        }

        [HttpPost]
        public ActionResult Edit(string[] FormatTags, string[] Pseudonyms, FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing ed)
        {
            if (!ModelState.IsValid)
            {
                int id = Int32.Parse(form["WritingID"]);
               
                Writing wr = db.Writings.Find(id);
                wr.Title = form["Title"];
                wr.EditDate = DateTime.Now;
                wr.LikesOn = form["LikesOn"] != null ? true : false;
                wr.CommentsOn = form["CommentsOn"] != null ? true : false;
                wr.CritiqueOn = form["CritiqueOn"] != null ? true : false;
                wr.DescriptionText = form["DescriptionText"];
                wr.Document = Encoding.Unicode.GetBytes(form["EditorContent"]);
                db.Entry(wr).State = EntityState.Modified;
                db.SaveChanges();

                ICollection<FormatTag> allFT = db.FormatTags.ToList();
                foreach (var item in allFT)
                {
                    bool isSelected = false;
                    for(int i = 0; i < FormatTags.Length; i++)
                    {
                        if (item.FormatID == Int32.Parse(FormatTags[i]))
                            isSelected = true;
                    }

                    if (db.WritingFormats.Where(w => w.WritingID == id).Where(w => w.FormatID == item.FormatID).ToList().Count > 0 && isSelected == false)
                    {
                        WritingFormat wf = db.WritingFormats.Where(w => w.WritingID == id).Where(w => w.FormatID == item.FormatID).First();
                        db.WritingFormats.Remove(wf);
                        db.SaveChanges();
                    }
                }

                //add any format tags that don't already exist
                foreach (var selection in FormatTags)
                {
                    int selected = Int32.Parse(selection);
                    if(db.WritingFormats.Where(w => w.WritingID == id).Where(w => w.FormatID == selected).ToList().Count == 0)
                    {
                        WritingFormat wf = new WritingFormat()
                        {
                            WritingID = id,
                            FormatID = selected
                        };
                        db.WritingFormats.Add(wf);
                        db.SaveChanges();
                    }
                }

                ICollection<Pseudonym> allPseudonyms = db.Pseudonyms.ToList();
                foreach (var item in allPseudonyms)
                {
                    bool isSelected = false;
                    for (int i = 0; i < Pseudonyms.Length; i++)
                    {
                        if (item.PseudonymID == Int32.Parse(Pseudonyms[i]))
                            isSelected = true;
                    }

                    if (db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == item.PseudonymID).ToList().Count > 0 && isSelected == false)
                    {
                        WritingPseudonym wp = db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == item.PseudonymID).First();
                        db.WritingPseudonyms.Remove(wp);
                        db.SaveChanges();
                    }
                }

                //add any pseudonyms that don't already exist
                foreach (var selection in Pseudonyms)
                {
                    int selected = Int32.Parse(selection);
                    if (db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == selected).ToList().Count == 0)
                    {
                        WritingPseudonym wp = new WritingPseudonym()
                        {
                            WritingID = id,
                            PseudonymID = selected
                        };
                        db.WritingPseudonyms.Add(wp);
                        db.SaveChanges();
                    }
                }

                //previouslySelectedPseudonyms = previouslySelectedPseudonyms.Distinct().ToList();
                //foreach(var item in previouslySelectedPseudonyms)
                //{
                //    if(db.WritingPseudonyms.Where(w => w.WritingID == id).Select(w => w.PseudonymID).ToList().Contains(item))
                //    {
                //        WritingPseudonym wp = db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == item).First();
                //        db.WritingPseudonyms.Remove(wp);
                //        db.SaveChanges();
                //    }
                //}

                return RedirectToAction("ViewWriting", "Writing", new { @id = id });
            }
            else
            {
                ViewBag.FileStatus = "Model Invalid";
                return View(db);
            }
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
            ICollection<WritingFormat> wfToDelete = db.WritingFormats.Where(w => w.WritingID == id).ToList();
            ICollection<WritingPseudonym> wpToDelete = db.WritingPseudonyms.Where(w => w.WritingID == id).ToList();

            foreach(var item in wfToDelete)
            {
                WritingFormat wf = db.WritingFormats.Where(w => w.WritingID == id).First();
                db.WritingFormats.Remove(wf);
                db.SaveChanges();
            }

            foreach (var item in wpToDelete)
            {
                WritingPseudonym wp = db.WritingPseudonyms.Where(w => w.WritingID == id).First();
                db.WritingPseudonyms.Remove(wp);
                db.SaveChanges();
            }

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