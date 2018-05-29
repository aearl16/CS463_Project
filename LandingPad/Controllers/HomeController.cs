using System;
using System.Linq;
using System.Web.Mvc;
using TweetSharp;
using LandingPad.DAL;
using LandingPad.Repositories;
using LandingPad.Models;
using Moq;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Diagnostics;

namespace LandingPad.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        private LandingPadContext db = new LandingPadContext();
        ITwitterRepository repository = new TwitterRepository(new LandingPadContext());
        IWritingRepository wrepo = new WritingRepository(new LandingPadContext());

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
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult TwitterAuth()
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
            try
            {
                repository.Remove(lpCurrentUser.UserID);
                repository.Save();
            }
            catch (Exception e)
            {
                //do nothing
                Debug.WriteLine(e.Message);
            }

            String sid = lpCurrentUser.UserID.ToString();

            string Key = System.Configuration.ConfigurationManager.AppSettings["twKey"];
            string Secret = System.Configuration.ConfigurationManager.AppSettings["twSecret"];
            TwitterService service = new TwitterService(Key, Secret);
           // OAuthRequestToken requestToken = service.GetRequestToken("https://landingpad.azurewebsites.net/Home/TwitterCallback" + "?id=" + sid); //For deployment
            OAuthRequestToken requestToken = service.GetRequestToken("https://localhost:44315/Home/TwitterCallback" + "?id=" + sid); //For testing purposes
            Uri uri = service.GetAuthenticationUrl(requestToken);
            if (CheckToken(Key))
            {
                if (CheckToken(Secret))
                {

                    return Redirect(uri.ToString());
                }
                else
                {
                    ViewBag.FileStatus = "Invalid Twitter Key";
                    return View();
                }
            }
            else
            {
                ViewBag.FileStatus = "Model Invalid";
                return View();
            }

        }

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier)
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
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);


            var requestToken = new OAuthRequestToken { Token = oauth_token };
            string Key = System.Configuration.ConfigurationManager.AppSettings["twKey"];
            string Secret = System.Configuration.ConfigurationManager.AppSettings["twSecret"];
            Twitter twitterUser = new Twitter { };
            try
            {
                TwitterService service = new TwitterService(Key, Secret);
                OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);
                service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
                VerifyCredentialsOptions option = new VerifyCredentialsOptions();
                TwitterUser user = service.VerifyCredentials(option);

                TempData["Token"] = oauth_token;
                TempData["VToken"] = oauth_verifier;
                TempData["UserTag"] = user.ScreenName;
                TempData["Name"] = user.Name;
                TempData["Userpic"] = user.ProfileImageUrl;

                int TwID = lpCurrentUser.UserID;
                var Token = oauth_token;
                var VToken = oauth_verifier;
                String TwName = user.ScreenName;
                String TagName = user.Name;
                twitterUser.UserID = lpCurrentUser.UserID;
                twitterUser.TwName = TwName;
                twitterUser.TwTag = TagName;
                twitterUser.TwOauth = Token;
                twitterUser.TwVOauth = VToken;
                twitterUser.Date = DateTime.Now;
                twitterUser.EndDate = DateTime.Now.AddMinutes(60);
                repository.Add(twitterUser);
                repository.Save();
                return RedirectToAction("Settings/" + lpCurrentUser.UserID);
        }
            catch
            {
                throw new System.InvalidOperationException("Twitter didnt like it");
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
            string uid = GetUserID();
            //Get ASP.NET User Object
            ApplicationUser currentUser = GetUser(uid);
            //Get the LPUser based on ASP.NET User's e-mail
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);

            try
            {
                //ViewBag.TwitterName = UnitOfWork.Twitter.GetTwitterEndTime(id);
                ViewBag.TwitterName = repository.GetTwitterTag(lpCurrentUser.UserID);
                DateTime EndTime = repository.GetTwitterEndTime(lpCurrentUser.UserID);
                ViewBag.EndTime = EndTime;
                //return View(db.Writings.ToList());
                return View(wrepo.GetAll());
            }
            catch
            {
               // return View(db.Writings.ToList());
                return View(wrepo.GetAll());
            }
            //}
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Settings()
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
            LPUser lpCurrentUser = GetLPUser((string)currentUser.Email);

            try
            {
                ViewBag.TwitterName = repository.GetTwitterTag(lpCurrentUser.UserID);
                DateTime EndTime = repository.GetTwitterEndTime(lpCurrentUser.UserID);
                ViewBag.EndTime = EndTime;
                return View();
            }
            catch
            {
                return View();
            }
            // return View();
            //}   
        }

        public bool CheckToken(String Token)
        {
            if (Token != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool VerifyToken()
        {
            string Key = System.Configuration.ConfigurationManager.AppSettings["twKey"];
            string Secret = System.Configuration.ConfigurationManager.AppSettings["twSecret"];
            if (String.IsNullOrEmpty(Key))
            {
                if (String.IsNullOrEmpty(Secret))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool TestTwitterAuth(int id)
        {
            return true;
        }

        //public bool TestUserIdInfor(int test, int id)
        //{
        //    return TestUserIdInfor(test, id, repository);
        //}
        // This is an entry point for testing.
        public bool TestUserIdInfor(int test, Mock<ITwitterRepository> mock)
        {
            string name = mock.Name;
            //int tid = repository.GetTwitterId(id.Value);

            if (test == 1)
            {
                return true;
            }
            else if (test == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create a LPUser, email passed in from Register
        /// </summary>
        /// <param name="Email"></param>
        /// <returns> ActionResult </returns>
        [AllowAnonymous]
        public ActionResult LPUserCreate(string Email)
        {
            //Create the User
            var lpUser = new LPUser();
            lpUser.Email = Email;
            //Used for testgin get Username
            string[] splitstring = Email.Split('@');
            lpUser.Username = splitstring[0];
            //Add to DB
            db.LPUsers.Add(lpUser);
            db.SaveChanges();

            //View Message from Register
            ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                                    + "before you can log in.";
            //Return the Info View
            return View("Info");
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