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
using LandingPad.Models;


namespace LandingPad.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private LandingPadContext db = new LandingPadContext();

        //[Authorize]
        [HttpGet]
        public ActionResult TwitterAuth()
        {
            string Key = System.Configuration.ConfigurationManager.AppSettings["twKey"];
            string Secret = System.Configuration.ConfigurationManager.AppSettings["twSecret"];
            TwitterService service = new TwitterService(Key, Secret);
            OAuthRequestToken requestToken = service.GetRequestToken("https://localhost:44315/Home/TwitterCallback");
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

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier, int? id)
        {
            var requestToken = new OAuthRequestToken { Token = oauth_token };
            string Key = System.Configuration.ConfigurationManager.AppSettings["twKey"];
            string Secret = System.Configuration.ConfigurationManager.AppSettings["twSecret"];
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

                int? TwID = id;
                var Token = oauth_token;
                var VToken = oauth_verifier;
                String TwName = user.ScreenName;
                String TagName = user.Name;


                




                return RedirectToAction("Settings");
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

        public ActionResult Index()
        {      
            return View(db.Writings.ToList());
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Settings()
        {
            return View();
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
            if(String.IsNullOrEmpty(Key))
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
    }
}