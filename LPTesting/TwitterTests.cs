using NUnit.Framework;
using LandingPad.Controllers;
using Moq;
using LandingPad.Repositorys;
using LandingPad.DAL;
using LandingPad.Models;
using Ninject;
using System.Linq;
using System;

namespace LPTesting
{
    [TestFixture]
    public class TestTwitter
    {
        //private Mock<ITwitterRepository> tmocker;
        //ITwitterRepository repository = new TwitterRepository(new LandingPadContext());

        private Twitter[] twitter = {
            new Twitter{	
        TwitterID = 1,
        UserID = 1,	
	    Date = Convert.ToDateTime("4/29/2018 2:22:13 PM"),
        EndDate = Convert.ToDateTime("4/29/2018 3:22:13 PM"),
	    TwName  = "SquareOne2018",
	    TwTag = "SquareOne",
	    TwOauth = "jkfapoienfad",
	    TwVOauth = "dkalfjioekn"
            }
            };

        //[Test]//Testing on Twitter
        //public void TestVerifyToken_TokenAvailable()
        //{
        //    HomeController tw = new HomeController();
        //    Assert.IsTrue(tw.VerifyToken());
        //}
        [Test]
        public void TestUser1Info_TagName()
        {
            var mock = new Mock<ITwitterRepository>();
            mock.Setup(p => p.GetTwitterTag(1)).Returns("SquareOne2018");
            var lift = new HomeController();
            var result = lift.TestUserIdInfor(1, mock);

            Assert.IsTrue(result, "SquareOne2018");
        }

        [Test]
        public void TestUser1Info_DateTime()
        {
            var mock = new Mock<ITwitterRepository>();
            mock.Setup(p => p.GetTwitterEndTime(1)).Returns(Convert.ToDateTime("4/29/2018 3:22:13 PM"));
            var lift = new HomeController();
            var result = lift.TestUserIdInfor(2, mock);
            DateTime comp = Convert.ToDateTime("4/29/2018 3:22:13 PM");
            Assert.IsTrue(result, "4/29/2018 3:22:13 PM");
        }
    }
}
