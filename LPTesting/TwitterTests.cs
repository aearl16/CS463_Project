using NUnit.Framework;
using LandingPad.Controllers;
using Moq;
using LandingPad.Repositories;
using LandingPad.DAL;
using LandingPad.Models;
using Ninject;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LPTesting
{
    [TestFixture]
    public class TestTwitter
    {
        private List<Twitter> twitter;

        private Mock<ITwitterRepository> mockTwitterRepo;

        private TwittersController twittersController;

     //   private Twitter[] twitter = {
     //       new Twitter{	
     //   TwitterID = 1,
     //   UserID = 1,	
	    //Date = Convert.ToDateTime("4/29/2018 2:22:13 PM"),
     //   EndDate = Convert.ToDateTime("4/29/2018 3:22:13 PM"),
	    //TwName  = "SquareOne2018",
	    //TwTag = "SquareOne",
	    //TwOauth = "jkfapoienfad",
	    //TwVOauth = "dkalfjioekn"
     //       }
     //       };

        [SetUp]
        public void Setup()
        {
            twitter = new List<Twitter>()
                {
                   new Twitter()
                   {
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
        }

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

        [Test]
        public void TestingIfMoqWasCreated()
        {
            mockTwitterRepo = new Mock<ITwitterRepository>();

            mockTwitterRepo.Setup(m => m.GetAll()).Returns(twitter);

            twittersController = new TwittersController(mockTwitterRepo.Object);

            bool result = twittersController.CreatedMoq();

            Assert.IsTrue(result);

        }

        [Test]
        public void TestingSquareOneCreation()
        {
            mockTwitterRepo = new Mock<ITwitterRepository>();

            mockTwitterRepo.Setup(m => m.GetAll()).Returns(twitter);

            twittersController = new TwittersController(mockTwitterRepo.Object);

            bool result = twittersController.CheckSquareOne(1);

            Assert.IsTrue(result);
        }
    }
}
