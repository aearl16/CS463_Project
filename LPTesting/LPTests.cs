using NUnit.Framework;
using LandingPad.Controllers;
using System.Web.Mvc;

namespace LPTesting
{
    [TestFixture]
    public class TestFileExtensions
    {
        [Test]
        public void TestUpload_FileExtensions_DOCX_Returns_True()
        {
            UploadController up = new UploadController();
            Assert.IsTrue(up.CheckExt(".DOCX"));
        }

        [Test]
        public void TestUpload_FileExtensions_DOC_Returns_True()
        {
            UploadController up = new UploadController();
            Assert.IsTrue(up.CheckExt(".DOC"));
        }

        [Test]
        public void TestUpload_FileExtensions_null_Returns_False()
        {
            UploadController up = new UploadController();
            Assert.IsFalse(up.CheckExt(null));
        }

        [Test]//Testing on Twitter??
        public void TestTwitter_Is_Valid_TokenNull()
        {
            HomeController tw = new HomeController();
            Assert.IsFalse(tw.CheckToken(null));
        }

        [Test]//Testing on Twitter??
        public void TestTwitter_Is_Valid_TokenNotNull()
        {
            HomeController tw = new HomeController();
            Assert.IsTrue(tw.VerifyToken());
        }
    }
}
