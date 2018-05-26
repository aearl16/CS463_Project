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
using System.Data.Entity.Infrastructure;
using LandingPad.Repositories;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace LandingPad.Controllers
{
    [RequireHttps]
    [Authorize]
    public class LPProfilesController : Controller
    {
        private LandingPadContext db = new LandingPadContext();
        ILProfilesRepository lprepo = new LProfilesRepository(new LandingPadContext());


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
