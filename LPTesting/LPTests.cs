using NUnit.Framework;
using LandingPad.Controllers;

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
    }
}
