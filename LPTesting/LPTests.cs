using NUnit.Framework;
using LandingPad.Controllers;

namespace LPTesting
{
    [TestFixture]
    public class TestFileExtensions
    {
        byte[] testFullByteArray = new byte[] { 0x26, 0x00, 0x6C, 0x00, 0x74, 0x00, 0x3B, 0x00, 0x70, 0x00, 0x26, 0x00, 0x67, 0x00, 0x74, 0x00, 0x3B, 0x00, 0x26, 0x00, 0x6C, 0x00, 0x74, 0x00, 0x3B, 0x00, 0x73, 0x00, 0x70, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x20, 0x00, 0x63, 0x00, 0x6C, 0x00, 0x61, 0x00, 0x73, 0x00, 0x73, 0x00, 0x3D, 0x00, 0x22, 0x00, 0x71, 0x00, 0x6C, 0x00, 0x2D, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x2D, 0x00, 0x74, 0x00, 0x69, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x73, 0x00, 0x22, 0x00, 0x26, 0x00, 0x67, 0x00, 0x74, 0x00, 0x3B, 0x00, 0x09, 0x00, 0x26, 0x00, 0x6C, 0x00, 0x74, 0x00, 0x3B, 0x00, 0x2F, 0x00, 0x73, 0x00, 0x70, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x26, 0x00, 0x67, 0x00, 0x74, 0x00, 0x3B, 0x00, 0x26, 0x00, 0x6C, 0x00, 0x74, 0x00, 0x3B, 0x00, 0x73, 0x00, 0x70, 0x00, 0x61, 0x00 };
        byte[] testEmptyByteArray = new byte[] { };
        byte[] testByteArrayLengthOne = new byte[] { 0x6C, 0x00 };
        byte[] testByteArrayWithConversion = new byte[] { 0x26, 0x00, 0x6C, 0x00, 0x74, 0x00, 0x3B, 0x00 };
        byte[] testByteArrayLengthOneLowerCase = new byte[] { 0x6c, 0x00 };
        string expectedStringResultForFullArray = "<p><span class=\"ql-font-times\">	</span><spa";
        string expectedStringResultForByteArrayWithLengthOne = "l";
        string expectedStringResultForByteArrayWithConversion = "<";
        WritingController wr = new WritingController();

        [Test]
        public void TestHTMLByteArrayToString_FullByteArray_Returns_CorrectResult()
        {
            StringAssert.AreEqualIgnoringCase(expectedStringResultForFullArray, wr.HTMLByteArrayToString(testFullByteArray));
        }

        [Test]
        public void TestHTMLByteArrayToString_ByteArrayWithLengthOfOneAllCaps_Returns_CorrectResult()
        {
            StringAssert.AreEqualIgnoringCase(expectedStringResultForByteArrayWithLengthOne, wr.HTMLByteArrayToString(testByteArrayLengthOne));
        }

        [Test]
        public void TestHTMLByteArrayToString_ByteArrayWithLengthOfOneWithLower_Returns_CorrectResult()
        {
            StringAssert.AreEqualIgnoringCase(expectedStringResultForByteArrayWithLengthOne, wr.HTMLByteArrayToString(testByteArrayLengthOneLowerCase));
        }

        [Test]
        public void TestHTMLByteArrayToString_HTMLEscapeCharacter_Returns_CharacterInPlainText()
        {
            StringAssert.AreEqualIgnoringCase(expectedStringResultForByteArrayWithConversion, wr.HTMLByteArrayToString(testByteArrayWithConversion));
        }

        [Test]
        public void TestHTMLByteArrayToString_EmptyByteArray_Returns_EmptyString()
        {
            StringAssert.AreEqualIgnoringCase("", wr.HTMLByteArrayToString(testEmptyByteArray));
        }

        [Test]
        public void TestHTMLByteArrayToString_null_Returns_null()
        {
            StringAssert.AreEqualIgnoringCase(null, wr.HTMLByteArrayToString(null));
        }

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
        public void Test_CheckToken_Null()
        {
            HomeController tw = new HomeController();
            Assert.IsFalse(tw.CheckToken(null));
        }
        [Test]//Testing on Twitter??
        public void Test_CheckToken_NotNull()
        {
            HomeController tw = new HomeController();
            Assert.IsTrue(tw.CheckToken("secretkey"));
        }
        [Test]//Testing on Twitter??
        public void TestVerifyToken_TokenAvailable()
        {
            HomeController tw = new HomeController();
            Assert.IsTrue(tw.VerifyToken());
        }
    }
}
