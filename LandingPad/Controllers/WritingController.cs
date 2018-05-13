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

        // GET: Pseudonym
        public ActionResult Index()
        {
            //Check if logged in ==> Should be caught by [Authorize] but just in case
            if(!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string id = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(id);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);
            //This operation didn't change ==> just gets the LPProfile with LPUser's ID
            ViewBag.Profiles = String.Join(",", db.LPProfiles.Find(lpCurrentUser.UserID));
            return View(db.LPProfiles.ToList());
        }

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

        [ChildActionOnly]
        public PartialViewResult _GetPermissionWritings(int id)
        {
            LPProfile lpProfile = db.LPProfiles.Find(id);

            return PartialView(GetAllWritingAvailable(id));
        }

        public ActionResult SearchByFormatTag(int id)
        {
            return View(db.Writings.Where(i => i.WritingFormats.Select(j => j.FormatTag.FormatID).ToList().Contains(id)).ToList());
        }

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

            if (wr.DocType == ".HTML")
            {
                doc = HTMLByteArrayToString(wr.Document);
            }

            ViewBag.Document = doc;

            ViewBag.Pseudonyms = String.Join(",", db.Pseudonyms.Where(i => i.ProfileID == wr.ProfileID).Select(j => j.PseudonymID));

            ViewBag.FormatTags = String.Join(",", db.FormatTags.Select(i => i.FormatID));

            return View(wr);
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

            return PartialView();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string[] FormatTags, string[] Pseudonyms, FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
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

        /*
         * Begin Helper method section
         */
        /// <summary>
        /// Helper method that checks if a user is logged in
        /// </summary>
        /// <returns> tf if the user is logged in</returns>
        private bool CheckLogin()
        {
            if(User.Identity.IsAuthenticated)
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