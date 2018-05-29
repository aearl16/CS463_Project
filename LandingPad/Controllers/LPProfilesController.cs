using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandingPad.DAL;
using LandingPad.Models;
using System.Data.Entity.Infrastructure;
using LandingPad.Repositories;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace LandingPad.Controllers
{
    [RequireHttps]
    [Authorize]
    public class LPProfilesController : AccessController
    {
        private LandingPadContext db = new LandingPadContext();
        ILProfilesRepository lprepo = new LProfilesRepository(new LandingPadContext());

        // GET: LPProfiles

        public ActionResult Index()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uid = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uid);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser(currentUser.Email);
            return View(lprepo.GetAll());
        }

        // GET: LPProfiles/Details/5
        public ActionResult Details()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uid = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uid);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser(currentUser.Email);
            //Get the current user's profile based on the user ID
            LPProfile lPProfile = lprepo.Get(lpCurrentUser.UserID);

            if (lPProfile == null)
            {
                return HttpNotFound();
            }
            return View(lPProfile);
        }

        // GET: LPProfiles/Edit/5
        public ActionResult Edit()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the user's ID
            string uid = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uid);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser(currentUser.Email);
            LPProfile lPProfile = lprepo.Get(lpCurrentUser.UserID);

            if (lpCurrentUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", lpCurrentUser.UserID);
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

                    //db.Entry(lPProfile).State = EntityState.Modified;
                    lprepo.SetModified(lPProfile);
                    lprepo.Save();
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
    }
}
