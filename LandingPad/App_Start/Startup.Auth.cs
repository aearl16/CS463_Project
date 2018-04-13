using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin.Security.Providers.LinkedIn;
using Owin;
using LandingPad.Models;
using Microsoft.Owin.Security.Twitter;
using Owin.Security.Providers.Instagram;
using Owin.Security.Providers.Dropbox;
using Owin.Security.Providers.Orcid;
using Owin.Security.Providers.DeviantArt;

namespace LandingPad
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //Get config values
            string fbID = System.Configuration.ConfigurationManager.AppSettings["fbID"];
            string fbSecret = System.Configuration.ConfigurationManager.AppSettings["fbSecret"];
            string gcID = System.Configuration.ConfigurationManager.AppSettings["gcID"];
            string gcSecret = System.Configuration.ConfigurationManager.AppSettings["gcSecret"];
            string lic = System.Configuration.ConfigurationManager.AppSettings["lic"];
            string liSecret = System.Configuration.ConfigurationManager.AppSettings["liSecret"];
            string twKey = System.Configuration.ConfigurationManager.AppSettings["twKey"];
            string twSecret = System.Configuration.ConfigurationManager.AppSettings["twSecret"];
            string inKey = System.Configuration.ConfigurationManager.AppSettings["inKey"];
            string inSecret = System.Configuration.ConfigurationManager.AppSettings["inSecret"];
            string DeviantKey = System.Configuration.ConfigurationManager.AppSettings["DeviantKey"];
            string DeviantSecret = System.Configuration.ConfigurationManager.AppSettings["DeviantSecret"];
            string ORCIDKey = System.Configuration.ConfigurationManager.AppSettings["ORCIDKey"];
            string ORCIDSecret = System.Configuration.ConfigurationManager.AppSettings["ORCIDSecret"];
            string DBKey = System.Configuration.ConfigurationManager.AppSettings["DBKey"];
            string DBSecret = System.Configuration.ConfigurationManager.AppSettings["DBSecret"];


            app.UseFacebookAuthentication(
               appId: fbID,
               appSecret: fbSecret);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = gcID,
                ClientSecret = gcSecret
            });

            app.UseLinkedInAuthentication(lic, liSecret);

            app.UseTwitterAuthentication(
               consumerKey: twKey,
               consumerSecret: twSecret);

            app.UseInstagramInAuthentication(inKey, inSecret);

            app.UseDropboxAuthentication(
                appKey: DBKey,
                appSecret: DBSecret);

            app.UseOrcidAuthentication(ORCIDKey, ORCIDSecret);

            app.UseDeviantArtAuthentication(DeviantKey, DeviantSecret);

        }
    }
}