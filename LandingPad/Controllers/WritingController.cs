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
using System.Text.RegularExpressions;

namespace LandingPad.Controllers
{
    [RequireHttps]
    [Authorize]
    public class WritingController : Controller
    {
        LandingPadContext db = new LandingPadContext();

        // View displays a dropdown of ProfileIDs that changes the writings displayed to the ones written by/viewable by that profile
        public ActionResult Index()
        {
            ViewBag.Profiles = String.Join(",", db.LPProfiles.Select(i => i.ProfileID));
            return View(db.LPProfiles.ToList());
        }

        //View displays all the writing that the user with a ProfileID == id has access to
        public ActionResult AllWriting(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            LPProfile p = db.LPProfiles.Find(id);
            if(p == null)
            {
                return HttpNotFound();
            }

            return View(GetAllWritingAvailable(id.GetValueOrDefault()));
        }

        //Gets all writing that the user with a ProfileID of id has access to
        [ChildActionOnly]
        public PartialViewResult _GetPermissionWritings(int id)
        {
            return PartialView(GetAllWritingAvailable(id));
        }

        //Displays writings that have the format tag with a FormatID of id
        public ActionResult SearchByFormatTag(int id)
        {
            return View(db.Writings.Where(i => i.WritingFormats.Select(j => j.FormatTag.FormatID).ToList().Contains(id)).ToList());
        }

        //Displays writings that have the genre tag with a GenreID of id
        public ActionResult SearchByGenreTag(int id)
        {
            return View(db.Writings.Where(i => i.WritingGenres.Select(j => j.GenreTag.GenreID).ToList().Contains(id)).ToList());
        }

        //The view for editing existing writing; uses the id to edit the correct writing
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            //grabs the writing with the proper id
            Writing wr = db.Writings.Find(id);
            if (wr == null)
            {
                return HttpNotFound();
            }

            string doc = "";

            //if its DocType is HTML, set doc equal to the value of Document converted to an HTML string
            if (wr.DocType == ".HTML")
            {
                doc = HTMLByteArrayToString(wr.Document);
            }

            //puts doc in a form the view can access
            ViewBag.Document = doc;

            //passes an array of all the pseudonyms for this writing's author to the view
            ViewBag.Pseudonyms = String.Join(",", db.Pseudonyms.Where(i => i.ProfileID == wr.ProfileID).Select(j => j.PseudonymID));

            //passes an array of all the format tags to the view
            ViewBag.FormatTags = String.Join(",", db.FormatTags.Select(i => i.FormatID));

            //passes an array of all the genre tags to the view
            ViewBag.GenreTags = String.Join(",", db.GenreTags.Select(i => i.GenreID));

            return View(wr);
        }

        [HttpPost]
        public ActionResult Edit(string[] FormatTags, string[] Pseudonyms, string[] FictionOrNonfiction, string[] GenreTags, FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
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
                wr.UsePseudonymsInAdditionToUsername = form["UsePseudonymsInAdditionToUsername"] != null ? true : false;
                wr.DescriptionText = form["DescriptionText"];
                wr.Document = Encoding.Unicode.GetBytes(form["EditorContent"]);
                db.Entry(wr).State = EntityState.Modified;
                db.SaveChanges();

                AccessPermission ap = db.AccessPermissions.Find(wr.AccessPermissionID);
                ap.PublicAccess = form["PublicAccess"] != null ? true : false;
                ap.FriendAccess = form["FriendAccess"] != null ? true : false;
                ap.PublisherAccess = form["PublisherAccess"] != null ? true : false;
                ap.MinorAccess = form["MinorAccess"] != null ? true : false;
                db.Entry(ap).State = EntityState.Modified;
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
                if(FormatTags != null)
                {
                    foreach (var selection in FormatTags)
                    {
                        int selected = Int32.Parse(selection);
                        if (db.WritingFormats.Where(w => w.WritingID == id).Where(w => w.FormatID == selected).ToList().Count == 0)
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
                }

                ICollection<Pseudonym> allPseudonyms = db.Pseudonyms.ToList();
                foreach (var item in allPseudonyms)
                {
                    bool isSelected = false;
                    if(Pseudonyms != null)
                    {
                        for (int i = 0; i < Pseudonyms.Length; i++)
                        {
                            if (item.PseudonymID == Int32.Parse(Pseudonyms[i]))
                                isSelected = true;
                        }
                    }

                    if (db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == item.PseudonymID).ToList().Count > 0 && isSelected == false)
                    {
                        WritingPseudonym wp = db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == item.PseudonymID).First();
                        db.WritingPseudonyms.Remove(wp);
                        db.SaveChanges();
                    }
                }

                //add any pseudonyms that don't already exist
                if(Pseudonyms != null)
                {
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
                }

                ICollection<GenreTag> allGT = db.GenreTags.ToList();
                foreach (var item in allGT)
                {
                    bool isSelected = false;
                    for (int i = 0; i < FictionOrNonfiction.Length; i++)
                    {
                        if (item.GenreID == Int32.Parse(FictionOrNonfiction[i]))
                            isSelected = true;
                    }

                    if (db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == item.GenreID).ToList().Count > 0 && isSelected == false)
                    {
                        WritingGenre wg = db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == item.GenreID).First();
                        db.WritingGenres.Remove(wg);
                        db.SaveChanges();
                    }
                }

                //add any genre tags that don't already exist
                if (FictionOrNonfiction != null)
                {
                    foreach (var selection in FictionOrNonfiction)
                    {
                        int selected = Int32.Parse(selection);
                        if (db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == selected).ToList().Count == 0)
                        {
                            WritingGenre wg = new WritingGenre()
                            {
                                WritingID = id,
                                GenreID = selected
                            };
                            db.WritingGenres.Add(wg);
                            db.SaveChanges();
                        }
                    }
                }

                foreach (var item in allGT)
                {
                    bool isSelected = false;
                    for (int i = 0; i < GenreTags.Length; i++)
                    {
                        if (item.GenreID == Int32.Parse(GenreTags[i]))
                            isSelected = true;
                    }

                    if (db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == item.GenreID).ToList().Count > 0 && isSelected == false)
                    {
                        WritingGenre wg = db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == item.GenreID).First();
                        db.WritingGenres.Remove(wg);
                        db.SaveChanges();
                    }
                }

                //add any genre tags that don't already exist
                if (GenreTags != null)
                {
                    foreach (var selection in GenreTags)
                    {
                        int selected = Int32.Parse(selection);
                        if (db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == selected).ToList().Count == 0)
                        {
                            WritingGenre wg = new WritingGenre()
                            {
                                WritingID = id,
                                GenreID = selected
                            };
                            db.WritingGenres.Add(wg);
                            db.SaveChanges();
                        }
                    }
                }


                return RedirectToAction("ViewWriting", "Writing", new { @id = id });
            }
            else
            {
                ViewBag.FileStatus = "Model Invalid";
                return View();
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
            ICollection<WritingGenre> wgToDelete = db.WritingGenres.Where(w => w.WritingID == id).ToList();

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

            foreach (var item in wgToDelete)
            {
                WritingGenre wg = db.WritingGenres.Where(w => w.WritingID == id).First();
                db.WritingGenres.Remove(wg);
                db.SaveChanges();
            }

            Writing wr = db.Writings.Find(id);
            int aId = wr.AccessPermissionID;

            db.Writings.Remove(wr);
            db.SaveChanges();

            AccessPermission ap = db.AccessPermissions.Find(aId);
            db.AccessPermissions.Remove(ap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public PartialViewResult _WritingPreview(int id)
        {
            Writing wr = db.Writings.Find(id);

            return PartialView(wr);
        }

        [ChildActionOnly]
        public PartialViewResult Editor()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult _SelectAuthor()
        {
            List<LPProfile> pAuthor = db.LPProfiles.Where(i => i.ProfileRoles.Select(j => j.RoleID).ToList().Contains(1)).ToList();
            return PartialView(pAuthor);
        }

        [ChildActionOnly]
        public PartialViewResult _SelectFormat()
        {
            return PartialView(db.FormatTags.ToList());
        }

        [ChildActionOnly]
        public PartialViewResult _SelectGenre()
        {
            return PartialView(db.GenreTags.ToList());
        }

        [ChildActionOnly]
        public PartialViewResult _SelectPermissions()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult _Confirmation()
        {
            return PartialView();
        }
        
        [ChildActionOnly]
        public PartialViewResult _Menu()
        {
            ViewBag.Pseudonyms = String.Join(",", db.Pseudonyms.Select(i => i.PseudonymID));
            ViewBag.FormatTags = String.Join(",", db.FormatTags.Select(i => i.FormatID));
            ViewBag.GenreTags = String.Join(",", db.GenreTags.Select(i => i.GenreID));

            return PartialView();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string[] FormatTags, string[] Pseudonyms, string[] FictionOrNonfiction, string[] GenreTags, FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing ed)
        {
            if (!ModelState.IsValid)
            {
                AccessPermission ap = new AccessPermission()
                {
                    PublicAccess = form["PublicAccess"] != null ? true : false,
                    FriendAccess = form["FriendAccess"] != null ? true : false,
                    PublisherAccess = form["PublisherAccess"] != null ? true : false,
                    MinorAccess = form["MinorAccess"] != null ? true : false
                };
                db.AccessPermissions.Add(ap);
                db.SaveChanges();

                Writing wr = new Writing()
                {
                    ProfileID = Int32.Parse(form["ProfileID"]),
                    AccessPermissionID = ap.AccessPermissionID,
                    Title = form["Title"],
                    AddDate = DateTime.Now,
                    EditDate = null,
                    LikesOn = form["LikesOn"] != null ? true : false,
                    CommentsOn = form["CommentsOn"] != null ? true : false,
                    CritiqueOn = form["CritiqueOn"] != null ? true : false,
                    UsePseudonymsInAdditionToUsername = form["UsePseudonymsInAdditionToUsername"] != null ? true : false,
                    DocType = form["DocType"],
                    DescriptionText = form["DescriptionText"],
                    Document = Encoding.Unicode.GetBytes(form["EditorContent"]),
                    WritingFileName = form["WritingFileName"]
                };
                db.Writings.Add(wr);
                db.SaveChanges();

                int id = wr.WritingID;

                ap = db.AccessPermissions.Find(ap.AccessPermissionID);
                ap.WritingID = id;
                db.Entry(ap).State = EntityState.Modified;
                db.SaveChanges();

                if(FormatTags != null)
                {
                    foreach (var selection in FormatTags)
                    {
                        WritingFormat wf = new WritingFormat()
                        {
                            WritingID = id,
                            FormatID = Int32.Parse(selection)
                        };
                        db.WritingFormats.Add(wf);
                        db.SaveChanges();
                    }
                }

                if(Pseudonyms != null)
                {
                    foreach (var selection in Pseudonyms)
                    {
                        WritingPseudonym wp = new WritingPseudonym()
                        {
                            WritingID = id,
                            PseudonymID = Int32.Parse(selection)
                        };
                        db.WritingPseudonyms.Add(wp);
                        db.SaveChanges();
                    }
                }

                if(FictionOrNonfiction != null)
                {
                    foreach (var selection in FictionOrNonfiction)
                    {
                        WritingGenre wg = new WritingGenre()
                        {
                            WritingID = id,
                            GenreID = Int32.Parse(selection)
                        };
                        db.WritingGenres.Add(wg);
                        db.SaveChanges();
                    }
                }

                if (GenreTags != null)
                {
                    foreach (var selection in GenreTags)
                    {
                        WritingGenre wg = new WritingGenre()
                        {
                            WritingID = id,
                            GenreID = Int32.Parse(selection)
                        };
                        db.WritingGenres.Add(wg);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("ViewWriting", "Writing", new { @id = id });
            }
            else
            {
                ViewBag.FileStatus = "Model Invalid";
                return View();
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

            if (wr.DocType == ".HTML")
            {
                doc = HTMLByteArrayToString(wr.Document);
            }

            ViewBag.Document = doc;

            string byline = "";

            if(wr.WritingPseudonyms.Count > 0)
            {
                byline = " by " + wr.WritingPseudonyms.First().Pseudonym.Pseudonym1;
            }
            else if(wr.LPProfile.DisplayRealName == true && wr.LPProfile.LPUser.GivenName != null && wr.LPProfile.LPUser.Surname != null)
            {
                byline = " by " + wr.LPProfile.LPUser.GivenName + " " + wr.LPProfile.LPUser.Surname;
            }
            else
            {
                byline = " by " + wr.LPProfile.LPUser.Username;
            }

            string pageTitle = wr.Title + byline;

            ViewBag.Title = pageTitle;

            byline = byline.TrimStart();
            byline = byline[0].ToString().ToUpper() + byline.Substring(1);

            ViewBag.Byline = byline;

            return View(wr);
        }

        public string HTMLByteArrayToString(byte[] input)
        {
            if (input == null)
                return null;

            return Encoding.Unicode.GetString(input);
        }

        public List<Writing> GetAllWritingAvailable(int id)
        {
            LPProfile lpProfile = db.LPProfiles.Find(id);
            ICollection<Writing> w = db.Writings.ToList();
            ICollection<Writing> toRemove;
            bool isMinor = IsUserMinor(lpProfile);

            toRemove = w.Where(i => i.ProfileID == id).ToList();

            foreach(var item in toRemove)
            {
                w.Remove(item);
            }

            if(isMinor)
            {
                toRemove = w.Where(i => i.AccessPermission.MinorAccess == false).ToList();
                foreach(var item in toRemove)
                {
                    w.Remove(item);
                }
            }

            toRemove = w.Where(i => i.AccessPermission.IndividualAccessRevokes.Select(j => j.RevokeeID).ToList().Contains(id)).ToList();
            foreach(var item in toRemove)
            {
                w.Remove(item);
            }

            //get all writings from w where individual access has not been granted to the user
            toRemove = w.Except(w.Where(i => i.AccessPermission.IndividualAccessGrants.Select(j => j.GranteeID).ToList().Contains(id))).ToList();

            //remove the writings that are set to public access
            toRemove = toRemove.Except(toRemove.Where(i => i.AccessPermission.PublicAccess == true)).ToList();

            //if the user has the publisher role, remove all writings with publisher access
            if(lpProfile.ProfileRoles.Select(i => i.RoleID).ToList().Contains(2))
            {
                toRemove = toRemove.Except(toRemove.Where(i => i.AccessPermission.PublisherAccess == true)).ToList();
            }

            //remove the writings that have friend access and are by friends of the user
            toRemove = toRemove.Except(toRemove.Where(i => i.AccessPermission.FriendAccess == true).ToList().Where(i => i.LPProfile.Friends.Select(j => j.SecondFriendID).ToList().Contains(id))).ToList();

            foreach(var item in toRemove)
            {
                w.Remove(item);
            }

            return w.ToList();
        }

        public bool IsUserMinor(LPProfile p)
        {
            DateTime now = DateTime.Today;

            if(p.LPUser.Birthdate != null)
            {
                DateTime bDay = p.LPUser.Birthdate.Value;
                if(now.Year - bDay.Year > 17 ||
                    (now.Year - bDay.Year == 17 && (now.Month > bDay.Month || (now.Month == bDay.Month && now.Date >= bDay.Date))))
                {
                    return false;
                }
            }

            return true;
        }

        public List<FormatCategory> GetChildrenWithAltParents()
        {
            return db.FormatCategories
                .Where(i => i.SecondaryParentID == null)
                .ToList();
        }

        public List<GenreCategory> FictionOnly()
        {
            return db.GenreCategories
                .GroupBy(i => i.GenreID)
                .Where(j => j.Select(k => k.TertiaryParentID).ToList().Contains(2) == false)
                .Where(j => j.Select(k => k.SecondaryParentID).ToList().Contains(2) == false)
                .Where(j => j.Select(k => k.ParentID).ToList().Contains(2) == false)
                .SelectMany(r => r)
                .Where(r => r.ParentID == 1 || r.SecondaryParentID == 1 || r.TertiaryParentID == 1)
                .ToList();
        }

        public List<GenreCategory> NonfictionOnly()
        {
            return db.GenreCategories
                .GroupBy(i => i.GenreID)
                .Where(j => j.Select(k => k.TertiaryParentID).ToList().Contains(1) == false)
                .Where(j => j.Select(k => k.SecondaryParentID).ToList().Contains(1) == false)
                .Where(j => j.Select(k => k.ParentID).ToList().Contains(1) == false)
                .SelectMany(r => r)
                .Where(r => r.ParentID == 2 || r.SecondaryParentID == 2 || r.TertiaryParentID == 2)
                .ToList();
        }

        public List<GenreCategory> GetChildGenresWithAltParents()
        {
            return db.GenreCategories
                .Where(i => i.TertiaryParentID == null)
                .ToList();
        }

        public List<GenreFormat> GetGenreFormats()
        {
            return db.GenreFormats
                .ToList();
        }
    }
}