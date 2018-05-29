using LandingPad;
using LandingPad.DAL;
using LandingPad.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Controllers
{
    public abstract class AccessController : Controller
    {
        private LandingPadContext db = new LandingPadContext();
        private ApplicationUserManager _userManager;

        protected string GetCurrentIdentityID()
        {
            return User.Identity.GetUserId();
        }

        /// <summary>
        /// Helper method to access Identity to see if the current request comes from
        /// a logged in user.
        /// </summary>
        /// <returns>true if the user is authenticated, false otherwise</returns>
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Helper method that checks if a user is logged in
        /// </summary>
        /// <returns> tf if the user is logged in</returns>
        public bool CheckLogin()
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
        protected string GetUserID()
        {
            return User.Identity.GetUserId();
        }

        /// <summary>
        /// Gets the user object from the database
        /// </summary>
        /// <returns> ApplicationUser object of the current user </returns>
        protected ApplicationUser GetUser(string id)
        {
            return UserManager.FindById(id);
        }

        /// <summary>
        /// Gets the LP user object based on e-mail link
        /// Can also be used separately for obtaining the user object
        /// </summary>
        /// <param name="email"></param>
        /// <returns> LPUser object after ApplicationUser object</returns>
        protected LPUser GetLPUser(string email)
        {
            return db.LPUsers.Where(em => em.Email == email).SingleOrDefault();
        }

        /// <summary>
        /// Get the curent user's profile based on the LPUser id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>LPProfile object</returns>
        protected LPProfile GetLPProfile(int id)
        {
            return db.LPProfiles.Where(lid => lid.UserID == id).SingleOrDefault();
        }
    }
}