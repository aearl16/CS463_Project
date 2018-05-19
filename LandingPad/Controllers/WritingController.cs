using LandingPad.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandingPad.Models;
using System.Text;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace LandingPad.Controllers
{
    [RequireHttps]
    [Authorize]
    public class WritingController : Controller
    {
        private LandingPadContext db = new LandingPadContext();
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Used to get the user manager for helper methods
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ActionResult Index()
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string id = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(id);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            return View(db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).FirstOrDefault());
        }

        public List<Writing> OrderByNewest(List<Writing> wr)
        {
            List<Writing> ordered = new List<Writing>();
            List<DateTime> comparable = new List<DateTime>();

            foreach (var item in wr)
            {
                if (item.EditDate != null)
                {
                    comparable.Add(item.EditDate.Value);
                }
                else
                {
                    comparable.Add(item.AddDate);
                }
            }

            comparable = comparable.OrderByDescending(i => i).ToList();

            foreach (var item in comparable)
            {
                List<Writing> wCopy = wr;

                foreach (var wItem in wCopy)
                {
                    if (wItem.EditDate != null)
                    {
                        if (wItem.EditDate.Value == item)
                        {
                            ordered.Add(wItem);
                            wr.Remove(wItem);
                            break;
                        }
                    }
                    else if (wItem.AddDate == item)
                    {
                        ordered.Add(wItem);
                        wr.Remove(wItem);
                        break;
                    }
                }
            }

            return ordered;
        }

        //Gets all writing that the user with a ProfileID of id has access to
        [ChildActionOnly]
        public PartialViewResult _GetPermissionWritings()
        {
            //Get the user's ID
            string id = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(id);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);

            List<LandingPad.Models.Writing> w = GetAllWritingAvailable(db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault());

            return PartialView(OrderByNewest(w));
        }

        //Displays writings that have the format tag with a FormatID of id
        public ActionResult SearchByFormatTag(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            List<LandingPad.Models.Writing> w = GetAllWritingAvailable(pID);
            List<LandingPad.Models.Writing> uw = db.Writings.Where(i => i.ProfileID == pID).ToList();

            foreach (var item in uw)
            {
                w.Add(item);
            }

            w = OrderByNewest(w);

            return View(w.Where(i => i.WritingFormats.Select(j => j.FormatTag.FormatID).ToList().Contains(id)).ToList());
        }

        //Displays writings that have the genre tag with a GenreID of id
        public ActionResult SearchByGenreTag(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            List<LandingPad.Models.Writing> w = GetAllWritingAvailable(pID);
            List<LandingPad.Models.Writing> uw = db.Writings.Where(i => i.ProfileID == pID).ToList();

            foreach (var item in uw)
            {
                w.Add(item);
            }

            w = OrderByNewest(w);

            return View(w.Where(i => i.WritingGenres.Select(j => j.GenreTag.GenreID).ToList().Contains(id)).ToList());
        }

        public ActionResult TestView()
        {
            return View(OrderByNewest(db.Writings.ToList()));
        }

        //The view for editing existing writing; uses the id to edit the correct writing
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uid = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uid);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            //Get the LPProfile
            LPProfile lpProfile = GetLPProfile(lpCurrentUser.UserID);

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

            //If the ProfileIDs don't match redirect to an error page
            if (wr.ProfileID != lpProfile.ProfileID)
            {
                return RedirectToAction("EditError", "Error");
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
                    for (int i = 0; i < FormatTags.Length; i++)
                    {
                        if (item.FormatID == Int32.Parse(FormatTags[i]))
                            isSelected = true;
                    }

                    if (db.WritingFormats.Where(w => w.WritingID == id).Where(w => w.FormatID == item.FormatID).ToList().Count > 0 && isSelected == false)
                    {
                        WritingFormat wf = db.WritingFormats.Where(w => w.WritingID == id).Where(w => w.FormatID == item.FormatID).FirstOrDefault();
                        db.WritingFormats.Remove(wf);
                        db.SaveChanges();
                    }
                }

                //add any format tags that don't already exist
                if (FormatTags != null)
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
                    if (Pseudonyms != null)
                    {
                        for (int i = 0; i < Pseudonyms.Length; i++)
                        {
                            if (item.PseudonymID == Int32.Parse(Pseudonyms[i]))
                                isSelected = true;
                        }
                    }

                    if (db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == item.PseudonymID).ToList().Count > 0 && isSelected == false)
                    {
                        WritingPseudonym wp = db.WritingPseudonyms.Where(w => w.WritingID == id).Where(w => w.PseudonymID == item.PseudonymID).FirstOrDefault();
                        db.WritingPseudonyms.Remove(wp);
                        db.SaveChanges();
                    }
                }

                //add any pseudonyms that don't already exist
                if (Pseudonyms != null)
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
                        WritingGenre wg = db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == item.GenreID).FirstOrDefault();
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
                        WritingGenre wg = db.WritingGenres.Where(w => w.WritingID == id).Where(w => w.GenreID == item.GenreID).FirstOrDefault();
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
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uid = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uid);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            //Get the LPProfile
            LPProfile lpProfile = GetLPProfile(lpCurrentUser.UserID);

            if (id == null)
            {
                return HttpNotFound();
            }
            Writing wr = db.Writings.Find(id);

            if (wr == null)
            {
                return HttpNotFound();
            }

            //If the ProfileIDs don't match redirect to an error page
            if (wr.ProfileID != lpProfile.ProfileID)
            {
                return RedirectToAction("DeleteError", "Error");
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

            foreach (var item in wfToDelete)
            {
                WritingFormat wf = db.WritingFormats.Where(w => w.WritingID == id).FirstOrDefault();
                db.WritingFormats.Remove(wf);
                db.SaveChanges();
            }

            foreach (var item in wpToDelete)
            {
                WritingPseudonym wp = db.WritingPseudonyms.Where(w => w.WritingID == id).FirstOrDefault();
                db.WritingPseudonyms.Remove(wp);
                db.SaveChanges();
            }

            foreach (var item in wgToDelete)
            {
                WritingGenre wg = db.WritingGenres.Where(w => w.WritingID == id).FirstOrDefault();
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
            //Get the user's ID
            string id = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(id);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);

            LPProfile pAuthor = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).FirstOrDefault();

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
        public ActionResult Friends()
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            ViewBag.ProfileID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            return View(db.LPProfiles.ToList());
        }

        [HttpPost]
        public ActionResult CreateProfileFriendRequest(int id, FormCollection form)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            if (Int32.Parse(form["ProfileID-" + id]) == 0)
            {
                FriendRequest fr = new FriendRequest()
                {
                    RequesterProfileID = pID,
                    RequesteeProfileID = id,
                    RequesterPseudonymID = null,
                    RequesteePseudonymID = null,
                    RequestDate = DateTime.Now
                };
                db.FriendRequests.Add(fr);
                db.SaveChanges();
            }
            else
            {
                FriendRequest fr = new FriendRequest()
                {
                    RequesterProfileID = pID,
                    RequesteeProfileID = id,
                    RequesterPseudonymID = Int32.Parse(form["ProfileID-" + id]),
                    RequesteePseudonymID = null,
                    RequestDate = DateTime.Now
                };
                db.FriendRequests.Add(fr);
                db.SaveChanges();
            }

            //create the new database object here
            return RedirectToAction("Friends");
        }

        [HttpPost]
        public ActionResult CreatePseudonymFriendRequest(int id, FormCollection form)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            if (Int32.Parse(form["PseudonymID-" + id]) == 0)
            {
                FriendRequest fr = new FriendRequest()
                {
                    RequesterProfileID = pID,
                    RequesteeProfileID = db.Pseudonyms.Where(i => i.PseudonymID == id).FirstOrDefault().ProfileID,
                    RequesterPseudonymID = null,
                    RequesteePseudonymID = id,
                    RequestDate = DateTime.Now
                };
                db.FriendRequests.Add(fr);
                db.SaveChanges();
            }
            else
            {
                FriendRequest fr = new FriendRequest()
                {
                    RequesterProfileID = pID,
                    RequesteeProfileID = db.Pseudonyms.Where(i => i.PseudonymID == id).FirstOrDefault().ProfileID,
                    RequesterPseudonymID = Int32.Parse(form["PseudonymID-" + id]),
                    RequesteePseudonymID = id,
                    RequestDate = DateTime.Now
                };
                db.FriendRequests.Add(fr);
                db.SaveChanges();
            }

            //create the new database object here
            return RedirectToAction("Friends");
        }

        [HttpPost]
        public ActionResult AcceptFriendRequest(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            FriendRequest fr = db.FriendRequests.Find(id);

            Friendship f1 = new Friendship()
            {
                FirstFriendID = fr.RequesterProfileID,
                SecondFriendID = fr.RequesteeProfileID,
                FirstPseudonymID = fr.RequesterPseudonymID,
                SecondPseudonymID = fr.RequesteePseudonymID,
                AcceptDate = DateTime.Now
            };
            db.Friendships.Add(f1);
            db.SaveChanges();

            Friendship f2 = new Friendship()
            {
                FirstFriendID = fr.RequesteeProfileID,
                SecondFriendID = fr.RequesterProfileID,
                FirstPseudonymID = fr.RequesteePseudonymID,
                SecondPseudonymID = fr.RequesterPseudonymID,
                AcceptDate = DateTime.Now
            };
            db.Friendships.Add(f2);
            db.SaveChanges();

            db.FriendRequests.Remove(fr);
            db.SaveChanges();

            return RedirectToAction("Friends");
        }

        [HttpPost]
        public ActionResult DeleteFriendRequest(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            FriendRequest fr = db.FriendRequests.Find(id);

            db.FriendRequests.Remove(fr);
            db.SaveChanges();

            return RedirectToAction("Friends");
        }

        [HttpPost]
        public ActionResult RemoveFriend(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            Friendship f1 = db.Friendships.Find(id);
            Friendship f2 = db.Friendships.Where(i => i.FirstFriendID == f1.SecondFriendID && i.SecondFriendID == f1.FirstFriendID && i.FirstPseudonymID == f1.SecondPseudonymID && i.SecondPseudonymID == f1.FirstPseudonymID).FirstOrDefault();

            db.Friendships.Remove(f1);
            db.SaveChanges();

            db.Friendships.Remove(f2);
            db.SaveChanges();

            //delete database object here
            return RedirectToAction("Friends");
        }

        [HttpGet]
        public ActionResult Settings()
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);

            return View(db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult AddProfileRole(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            ProfileRole pr = new ProfileRole()
            {
                ProfileID = pID,
                RoleID = id
            };
            db.ProfileRoles.Add(pr);
            db.SaveChanges();

            //create new database object here
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult RemoveProfileRole(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            ProfileRole pr = db.ProfileRoles.Where(i => i.ProfileID == pID && i.RoleID == 1).FirstOrDefault();
            db.ProfileRoles.Remove(pr);
            db.SaveChanges();

            //delete database object here
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult ChangeDateOfBirth(FormCollection form)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            lpCurrentUser.Birthdate = new DateTime(long.Parse(form["Birthday"]));
            db.Entry(lpCurrentUser).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult ChangeName(FormCollection form)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            LPProfile lpp = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).FirstOrDefault();

            lpCurrentUser.GivenName = form["GivenName"];
            lpCurrentUser.Surname = form["Surname"];
            db.Entry(lpCurrentUser).State = EntityState.Modified;
            db.SaveChanges();

            lpp.DisplayRealName = form["DisplayName"] != null ? true : false;
            db.Entry(lpp).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult AddPseudonym(FormCollection form)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            AccessPermission ap = new AccessPermission()
            {
                PublicAccess = true,
                FriendAccess = true,
                PublisherAccess = true,
                MinorAccess = true
            };

            int apID = ap.AccessPermissionID;

            db.AccessPermissions.Add(ap);
            db.SaveChanges();

            Pseudonym p = new Pseudonym()
            {
                ProfileID = pID,
                AccessPermissionID = apID,
                Pseudonym1 = form["Pseudonym"]
            };

            ap = db.AccessPermissions.Where(i => i.AccessPermissionID == apID).FirstOrDefault();
            ap.PseudonymID = p.PseudonymID;
            db.Entry(ap).State = EntityState.Modified;
            db.SaveChanges();

            db.Pseudonyms.Add(p);
            db.SaveChanges();

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult DeletePseudonym(int id)
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uID = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uID);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            int pID = db.LPProfiles.Where(i => i.UserID == lpCurrentUser.UserID).Select(i => i.ProfileID).FirstOrDefault();

            List<WritingPseudonym> wp = db.WritingPseudonyms.Where(i => i.PseudonymID == id).ToList();

            foreach (var item in wp)
            {
                int wID = item.WritingID;
                WritingPseudonym toRemove = item;
                db.WritingPseudonyms.Remove(toRemove);
                db.SaveChanges();

                Writing wr = db.Writings.Where(i => i.WritingID == wID).FirstOrDefault();

                if (wr.WritingPseudonyms.Count() == 0)
                {
                    if (wr.UsePseudonymsInAdditionToUsername == true)
                    {
                        wr.UsePseudonymsInAdditionToUsername = false;
                        db.Entry(wr).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            Pseudonym p = db.Pseudonyms.Where(i => i.PseudonymID == id).FirstOrDefault();
            db.Pseudonyms.Remove(p);
            db.SaveChanges();

            return RedirectToAction("Settings");
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

                if (FormatTags != null)
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

                if (Pseudonyms != null)
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

                if (FictionOrNonfiction != null)
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

            if (wr.WritingPseudonyms.Count > 0)
            {
                byline = " by " + wr.WritingPseudonyms.FirstOrDefault().Pseudonym.Pseudonym1;
            }
            else if (wr.LPProfile.DisplayRealName == true && wr.LPProfile.LPUser.GivenName != null && wr.LPProfile.LPUser.Surname != null)
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

            string output = Encoding.Unicode.GetString(input);
            output = output.Replace("&lt;", "<").Replace("&gt;", ">").Replace("'", "&#39;").Replace('"'.ToString(), "&#34;");


            return output;
        }

        public List<Writing> GetAllWritingAvailable(int id)
        {
            LPProfile lpProfile = db.LPProfiles.Find(id);
            ICollection<Writing> w = db.Writings.ToList();
            ICollection<Writing> toRemove;
            bool isMinor = IsUserMinor(lpProfile);

            toRemove = w.Where(i => i.ProfileID == id).ToList();

            foreach (var item in toRemove)
            {
                w.Remove(item);
            }

            if (isMinor)
            {
                toRemove = w.Where(i => i.AccessPermission.MinorAccess == false).ToList();
                foreach (var item in toRemove)
                {
                    w.Remove(item);
                }
            }

            toRemove = w.Where(i => i.AccessPermission.IndividualAccessRevokes.Select(j => j.RevokeeID).ToList().Contains(id)).ToList();
            foreach (var item in toRemove)
            {
                w.Remove(item);
            }

            //get all writings from w where individual access has not been granted to the user
            toRemove = w.Except(w.Where(i => i.AccessPermission.IndividualAccessGrants.Select(j => j.GranteeID).ToList().Contains(id))).ToList();

            //remove the writings that are set to public access
            toRemove = toRemove.Except(toRemove.Where(i => i.AccessPermission.PublicAccess == true)).ToList();

            //if the user has the publisher role, remove all writings with publisher access
            if (lpProfile.ProfileRoles.Select(i => i.RoleID).ToList().Contains(2))
            {
                toRemove = toRemove.Except(toRemove.Where(i => i.AccessPermission.PublisherAccess == true)).ToList();
            }

            //remove the writings that have friend access and are by friends of the user
            toRemove = toRemove.Except(toRemove.Where(i => i.AccessPermission.FriendAccess == true).ToList().Where(i => i.LPProfile.Friends.Select(j => j.SecondFriendID).ToList().Contains(id))).ToList();

            foreach (var item in toRemove)
            {
                w.Remove(item);
            }

            return w.ToList();
        }

        public bool IsUserMinor(LPProfile p)
        {
            DateTime now = DateTime.Today;

            if (p.LPUser.Birthdate != null)
            {
                DateTime bDay = p.LPUser.Birthdate.Value;
                if (now.Year - bDay.Year > 17 ||
                    (now.Year - bDay.Year == 17 && (now.Month > bDay.Month || (now.Month == bDay.Month && now.Date >= bDay.Date))))
                {
                    return false;
                }
            }

            return true;
        }

        //in use
        public List<FormatCategory> GetChildrenWithAltParents()
        {
            return db.FormatCategories
                .Where(i => i.SecondaryParentID == null)
                .ToList();
        }

        //in use
        public List<GenreFormat> GetFictionOnly()
        {
            return db.GenreFormats
                .Where(i => i.GenreID == 1 || i.ParentGenreID == 1)
                .ToList();
        }

        //in use
        public List<GenreFormat> GetNonfictionOnly()
        {
            return db.GenreFormats
                .Where(i => i.GenreID == 2 || i.ParentGenreID == 2)
                .ToList();
        }

        //in use
        public List<GenreCategory> FictionOrNonfictionOnly()
        {
            return db.GenreCategories
                .GroupBy(i => i.GenreID)
                .Where(j => ((j.Select(k => k.TertiaryParentID).ToList().Contains(1) == false && j.Select(k => k.TertiaryParentID).ToList().Contains(2) == true) || (j.Select(k => k.TertiaryParentID).ToList().Contains(2) == false && j.Select(k => k.TertiaryParentID).ToList().Contains(1) == true)) || ((j.Select(k => k.SecondaryParentID).ToList().Contains(1) == false && j.Select(k => k.SecondaryParentID).ToList().Contains(2) == true) || (j.Select(k => k.SecondaryParentID).ToList().Contains(2) == false && j.Select(k => k.SecondaryParentID).ToList().Contains(1) == true)) || ((j.Select(k => k.ParentID).ToList().Contains(1) == false && j.Select(k => k.ParentID).ToList().Contains(2) == true) || (j.Select(k => k.ParentID).ToList().Contains(2) == false && j.Select(k => k.ParentID).ToList().Contains(1) == true)))
                .SelectMany(r => r)
                .Where(r => r.ParentID == 1 || r.ParentID == 2 || r.SecondaryParentID == 1 || r.SecondaryParentID == 2 || r.TertiaryParentID == 1 || r.TertiaryParentID == 2)
                .ToList();
        }

        //in use
        public string IsFictionOrNonfictionOnlyForGenre(int id)
        {
            GenreTag gt = db.GenreTags.Find(id);

            if ((gt.ParentGenres.Select(i => i.ParentID).ToList().Contains(1) && gt.ParentGenres.Select(i => i.ParentID).ToList().Contains(2) == false) || gt.ParentGenres.Select(i => i.SecondaryParentID).ToList().Contains(1) || gt.ParentGenres.Select(i => i.TertiaryParentID).ToList().Contains(1))
            {
                return "Fiction";
            }
            else if ((gt.ParentGenres.Select(i => i.ParentID).ToList().Contains(1) == false && gt.ParentGenres.Select(i => i.ParentID).ToList().Contains(2)) || gt.ParentGenres.Select(i => i.SecondaryParentID).ToList().Contains(2) || gt.ParentGenres.Select(i => i.TertiaryParentID).ToList().Contains(2))
            {
                return "Nonfiction";
            }
            else
                return null;
        }

        //in use
        public int GetFictionOrNonfiction(int id)
        {
            List<GenreCategory> ForN = FictionOrNonfictionOnly();

            //if the GenreID passed in is fiction or nonfiction only
            if (ForN.Select(i => i.GenreID).ToList().Contains(id))
            {
                GenreCategory FicOrNon = ForN.Where(i => i.GenreID == id).FirstOrDefault();

                //if FicOrNon only has a Parent ID
                if (FicOrNon.SecondaryParentID == null)
                {
                    return FicOrNon.ParentID;
                } //if FicOrNon has a SecondaryParentID but not a TertiaryParentID
                else if (FicOrNon.TertiaryParentID == null)
                {
                    return FicOrNon.SecondaryParentID.Value;
                } //if FicOrNon has a TertiaryParentID
                else
                {
                    return FicOrNon.TertiaryParentID.Value;
                }
            }
            else
            {
                return 0;
            }
        }

        //in use
        public List<int> GetFictionOrNonfictionForGenre(int id, string name)
        {
            List<GenreCategory> ForN = FictionOrNonfictionOnly();


            if (String.Compare(name, "genre") == 0)
            {
                int fnGenreID = GetFictionOrNonfiction(id);
                if (fnGenreID != 0)
                {
                    if (fnGenreID == 1)
                    {
                        return ForN.Where(i => i.ParentID == 2 || i.SecondaryParentID == 2 || i.TertiaryParentID == 2).Select(i => i.GenreID).Distinct().ToList();
                    }
                    else
                    {
                        return ForN.Where(i => i.ParentID == 1 || i.SecondaryParentID == 1 || i.TertiaryParentID == 1).Select(i => i.GenreID).Distinct().ToList();
                    }
                }
                else
                {
                    return new List<int>();
                }
            }
            else
            {
                if (id == 1)
                {
                    return db.GenreFormats.Where(i => i.GenreID == 2).Select(i => i.ParentFormatID).ToList();
                }
                else if (id == 2)
                {
                    return db.GenreFormats.Where(i => i.GenreID == 1).Select(i => i.ParentFormatID).ToList();
                }
                else
                {
                    return new List<int>();
                }
            }
        }

        //in use
        public List<int> GetFictionOrNonfictionForFormat(int id)
        {
            //if this format tag is fiction only
            if (db.FormatTags.Find(id).ChildGenres.Select(i => i.GenreID).ToList().Contains(1))
            {
                return db.GenreFormats.Where(i => i.GenreID == 2).Select(i => i.ParentFormatID).ToList();
            } //if the format tag is nonfiction only
            else if (db.FormatTags.Find(id).ChildGenres.Select(i => i.GenreID).ToList().Contains(2))
            {
                return db.GenreFormats.Where(i => i.GenreID == 1).Select(i => i.ParentFormatID).ToList();
            }
            else //if the format tag can be either fiction or nonfiction
            {
                return new List<int>();
            }
        }

        //in use
        public List<GenreCategory> GetChildGenresWithAltParents()
        {
            return db.GenreCategories
                .Where(i => i.TertiaryParentID == null)
                .ToList();
        }

        /*
         * Begin Helper method section
         */
        /// <summary>
        /// Helper method that checks if a user is logged in
        /// </summary>
        /// <returns> tf if the user is logged in</returns>
        private bool CheckLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the currently logged in user's ID
        /// </summary>
        /// <returns> string id of the current user</returns>
        private string GetUserID()
        {
            return User.Identity.GetUserId();
        }

        /// <summary>
        /// Gets the user object from the database
        /// </summary>
        /// <returns> ApplicationUser object of the current user </returns>
        private ApplicationUser GetUser(string id)
        {
            return UserManager.FindById(id);
        }

        /// <summary>
        /// Gets the LP user object based on e-mail link
        /// Can also be used separately for obtaining the user object
        /// </summary>
        /// <param name="email"></param>
        /// <returns> LPUser object after ApplicationUser object</returns>
        private LPUser GetLPUser(string email)
        {
            return db.LPUsers.Where(em => em.Email == email).SingleOrDefault();
        }

        /// <summary>
        /// Get the curent user's profile based on the LPUser id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>LPProfile object</returns>
        private LPProfile GetLPProfile(int id)
        {
            return db.LPProfiles.Where(lid => lid.UserID == id).SingleOrDefault();
        }
    }
}