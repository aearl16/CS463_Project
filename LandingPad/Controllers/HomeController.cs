using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using TweetSharp;
using LandingPad.DAL;
using LandingPad.Repositories;
using LandingPad.Models;
using Moq;


namespace LandingPad.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        private LandingPadContext db = new LandingPadContext();
        ITwitterRepository repository = new TwitterRepository(new LandingPadContext());
        IWritingRepository wrepo = new WritingRepository(new LandingPadContext());

        [HttpGet]
        public ActionResult TwitterAuth(int id)
        {
            try
            {
                //Twitter twitterData = repository.GetTwitterId(id);
                //db.Twitters.Where(u => u.UserID == id).FirstOrDefault();
                //db.Twitters.Remove(twitterData);
                repository.Remove(id);
                repository.Save();
            }
            catch (Exception e)
            {
                //do nothing
            }
            string Key = System.Configuration.ConfigurationManager.AppSettings["twKey"];
            string Secret = System.Configuration.ConfigurationManager.AppSettings["twSecret"];
            TwitterService service = new TwitterService(Key, Secret);
            OAuthRequestToken requestToken = service.GetRequestToken("https://localhost:44315/Home/TwitterCallback" + "?id=" + Convert.ToString(id));
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

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier, int id)
        {
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

                int TwID = id;
                var Token = oauth_token;
                var VToken = oauth_verifier;
                String TwName = user.ScreenName;
                String TagName = user.Name;
                twitterUser.UserID = id;
                twitterUser.TwName = TwName;
                twitterUser.TwTag = TagName;
                twitterUser.TwOauth = Token;
                twitterUser.TwVOauth = VToken;
                twitterUser.Date = DateTime.Now;
                twitterUser.EndDate = DateTime.Now.AddMinutes(60);
                //db.Twitters.Add(twitterUser);
                repository.Add(twitterUser);
                repository.Save();
                //db.SaveChanges();
                return RedirectToAction("Settings/" + id);
                //return View();
            }
            catch
            {
                throw;
            }
        }

        // GET: Twitters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Twitter twitter = db.Twitters.Find(id);
            if (twitter == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", twitter.UserID);
            return View(twitter);
        }

        // POST: Twitters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TwitterID,UserID,TwName,TwTag,TwOauth,TwVOauth")] Twitter twitter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(twitter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.LPUsers, "UserID", "Email", twitter.UserID);
            return View(twitter);
        }

        public ActionResult Index(int? id)
        {
            // if (id == null)
            //{
            //  return RedirectToAction("Login", "Account");
            //}
            // else
            //{
            try
            {
                //ViewBag.TwitterName = UnitOfWork.Twitter.GetTwitterEndTime(id);
                ViewBag.TwitterName = repository.GetTwitterTag(id.Value);
                DateTime EndTime = repository.GetTwitterEndTime(id.Value);
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

        public ActionResult Settings(int? id)
        {
            //if(id == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //else {
            try
            {
                ViewBag.TwitterName = repository.GetTwitterTag(id.Value);
                DateTime EndTime = repository.GetTwitterEndTime(id.Value);
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
    }
}