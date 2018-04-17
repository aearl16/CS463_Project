using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using TweetSharp;


namespace LandingPad.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
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

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier)
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
                return View();
            }
            catch
            {
                throw;
            }

        }
        public ActionResult Index()
        {
           // TempData["UserTag"] = UserTag;
            return View();
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