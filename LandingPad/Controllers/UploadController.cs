using LandingPad.DAL;
using LandingPad.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data.Entity;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Packaging;
using System.Drawing.Imaging;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LandingPad.Controllers
{
    [RequireHttps]
    [Authorize]
    public class UploadController : Controller
    {
        //LandingPad Context
        private LandingPadContext db = new LandingPadContext();
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
        }

        /// <summary>
        /// Gets upload edit page, contains partial views that build it
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadEdit()
        {
            return View();
        }

        /// <summary>
        /// User can upload a file given a specific type constraint, and it will be converted to html for use in our editor
        /// </summary>
        /// <param name="FormatTags"></param>
        /// <param name="Pseudonyms"></param>
        /// <param name="file"></param>
        /// <param name="form"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadEdit(string[] FormatTags, string[] Pseudonyms, HttpPostedFileBase file, FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing doc)
        {

            String FileExt = Path.GetExtension(file.FileName).ToUpper();

            if (CheckExtEdit(FileExt))
            {
                AccessPermission ap = new AccessPermission()
                {
                    PublicAccess = form["PublicAccess"] != null ? true : false,
                    FriendAccess = form["FriendAccess"] != null ? true : false,
                    PublisherAccess = form["PublisherAccess"] != null ? true : false,
                    MinorAccess = form["MinorAccess"] != null ? true : false
                };
                db.AccessPermissions.Add(ap);
                db.SaveChanges();

                if (!ModelState.IsValid)
                {
                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileData = Br.ReadBytes((Int32)str.Length);
                    string html = string.Empty;
                    string[] namestring = file.FileName.Split('.');

                    if (FileExt == ".PDF" || FileExt == "PDF")
                    {

                        SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                        f.OpenPdf(FileData);

                        if (f.PageCount > 0)
                        {
                            f.HtmlOptions.IncludeImageInHtml = true;
                            f.HtmlOptions.Title = "Simple text";
                            html = f.ToHtml();
                        }
                    }
                    else if (FileExt == ".DOCX" || FileExt == "DOCX" || FileExt == ".DOC" || FileExt == "DOC")
                    {
                        html = ConvertToHtml(Path.GetFullPath(file.FileName));
                    }
                    else
                    {
                        ViewBag.FileStatus = "Model Invalid";
                        return View();
                    }

                    Writing wr = new Writing()
                    {
                        ProfileID = Int32.Parse(form["ProfileID"]),
                        AccessPermissionID = ap.AccessPermissionID,
                        Title = form["Title"],
                        AddDate = DateTime.Now,
                        EditDate = null,
                        LikesOn = form["LikesOn"] != null ? true : false,
                        CommentsOn = form["CommentsOn"] != null ? true : false,
                        CritiqueOn = form["CritiqueOn"] != null ? true : false,
                        UsePseudonymsInAdditionToUsername = form["UsePseudonymsInAdditionToUsername"] != null ? true : false,
                        DocType = ".HTML",
                        DescriptionText = form["DescriptionText"],
                        Document = Encoding.Unicode.GetBytes(html),
                        WritingFileName = namestring[0] + ".html"
                    };
                    db.Writings.Add(wr);
                    db.SaveChanges();

                    int id = wr.WritingID;

                    ap = db.AccessPermissions.Find(ap.AccessPermissionID);
                    ap.WritingID = id;
                    db.Entry(ap).State = EntityState.Modified;
                    db.SaveChanges();

                    if (FormatTags != null)
                    {
                        foreach (var selection in FormatTags)
                        {
                            WritingFormat wf = new WritingFormat()
                            {
                                WritingID = id,
                                FormatID = Int32.Parse(selection)
                            };
                            db.WritingFormats.Add(wf);
                            db.SaveChanges();
                        }
                    }

                    if (Pseudonyms != null)
                    {
                        foreach (var selection in Pseudonyms)
                        {
                            WritingPseudonym wp = new WritingPseudonym()
                            {
                                WritingID = id,
                                PseudonymID = Int32.Parse(selection)
                            };
                            db.WritingPseudonyms.Add(wp);
                            db.SaveChanges();
                        }
                    }


                    return RedirectToAction("Index", "Home", new { @id = id });
                }
                else
                {
                    ViewBag.FileStatus = "File Type Not valid: Valid file type for edit is currently .PDF";
                    return View();
                }
            }
            else
            {
                ViewBag.FileStatus = "Invalid file format.";
                return View();
            }

        }

        /// <summary>
        /// Returns the Store view, built from partial views
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Store()
        {
            return View();
        }

        /// <summary>
        /// User can upload a file and store it in the original format
        /// </summary>
        /// <param name="FormatTags"></param>
        /// <param name="Pseudonyms"></param>
        /// <param name="file"></param>
        /// <param name="form"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Store(string[] FormatTags, string[] Pseudonyms, HttpPostedFileBase file, FormCollection form, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing doc)
        {

            String FileExt = Path.GetExtension(file.FileName).ToUpper();

            if (CheckExt(FileExt))
            {
                AccessPermission ap = new AccessPermission()
                {
                    PublicAccess = form["PublicAccess"] != null ? true : false,
                    FriendAccess = form["FriendAccess"] != null ? true : false,
                    PublisherAccess = form["PublisherAccess"] != null ? true : false,
                    MinorAccess = form["MinorAccess"] != null ? true : false
                };
                db.AccessPermissions.Add(ap);
                db.SaveChanges();

                if (!ModelState.IsValid)
                {
                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileData = Br.ReadBytes((Int32)str.Length);

                    Writing wr = new Writing()
                    {
                        ProfileID = Int32.Parse(form["ProfileID"]),
                        AccessPermissionID = ap.AccessPermissionID,
                        Title = form["Title"],
                        AddDate = DateTime.Now,
                        EditDate = null,
                        LikesOn = form["LikesOn"] != null ? true : false,
                        CommentsOn = form["CommentsOn"] != null ? true : false,
                        CritiqueOn = form["CritiqueOn"] != null ? true : false,
                        UsePseudonymsInAdditionToUsername = form["UsePseudonymsInAdditionToUsername"] != null ? true : false,
                        DocType = FileExt,
                        DescriptionText = form["DescriptionText"],
                        Document = FileData,
                        WritingFileName = file.FileName
                    };
                    db.Writings.Add(wr);
                    db.SaveChanges();

                    int id = wr.WritingID;

                    ap = db.AccessPermissions.Find(ap.AccessPermissionID);
                    ap.WritingID = id;
                    db.Entry(ap).State = EntityState.Modified;
                    db.SaveChanges();

                    if (FormatTags != null)
                    {
                        foreach (var selection in FormatTags)
                        {
                            WritingFormat wf = new WritingFormat()
                            {
                                WritingID = id,
                                FormatID = Int32.Parse(selection)
                            };
                            db.WritingFormats.Add(wf);
                            db.SaveChanges();
                        }
                    }

                    if (Pseudonyms != null)
                    {
                        foreach (var selection in Pseudonyms)
                        {
                            WritingPseudonym wp = new WritingPseudonym()
                            {
                                WritingID = id,
                                PseudonymID = Int32.Parse(selection)
                            };
                            db.WritingPseudonyms.Add(wp);
                            db.SaveChanges();
                        }
                    }


                    return RedirectToAction("Index", "Home", new { @id = id});
                }
                else
                {
                    ViewBag.FileStatus = "Model Invalid";
                    return View();
                }
            }
            else
            {
                ViewBag.FileStatus = "Invalid file format.";
                return View();
            }

        }

        /// <summary>
        /// Allows a user to download a file out of the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns> The file in the original format it was uploaded as</returns>
        public ActionResult Download(int? id)
        {
            Writing wr = db.Writings.Find(id);
            if (wr == null)
            {
                return HttpNotFound();
            }
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] contentAsBytes = wr.Document;

            if(wr.DocType == "HTML" || wr.DocType == ".HTML")
            {
                this.HttpContext.Response.ContentType = "application/force-download";
                this.HttpContext.Response.AddHeader("Content-Disposition", "filename=" + wr.WritingFileName);
            }
            else if(wr.DocType == "DOC" || wr.DocType == ".DOC")
            {
                this.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + wr.WritingFileName);
            }
            else if(wr.DocType == "ODT" || wr.DocType ==".ODT")
            {
                this.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + wr.WritingFileName);
            }
            else if(wr.DocType == "PDF" || wr.DocType == ".PDF")
            {
                this.HttpContext.Response.ContentType = "application/pdf";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + wr.WritingFileName);
            }
            else
            {
                this.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + wr.WritingFileName);
            }
            
            this.HttpContext.Response.Buffer = true;
            this.HttpContext.Response.Clear();
            this.HttpContext.Response.OutputStream.Write(contentAsBytes, 0, contentAsBytes.Length);
            this.HttpContext.Response.OutputStream.Flush();
            this.HttpContext.Response.End();

            return View();
            }

        /// <summary>
        /// Helper method for Upload Post
        /// </summary>
        public bool CheckExt(String ext)
        {
            if(ext == ".DOCX" || ext == ".DOC")
            {
                return true;
            }
            else if(ext == ".PDF" || ext == "PDF")
            {
                return true;
            }
            else if(ext == ".ODT" || ext == "ODT")
            {
                return true;
            }
            else if(ext == ".RTF" || ext == "RTF")
            {
                return true;
            }
            else if(ext == ".TXT" || ext == "TXT")
            {
                return true;
            }
            else if(ext == ".HTML" || ext == "HTML")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Helper method for Upload Post
        /// </summary>
        public bool CheckExtEdit(String ext)
        {
            if (ext == ".DOCX" || ext == ".DOC")
            {
                return false;
            }
            else if (ext == ".PDF" || ext == "PDF")
            {
                return true;
            }
            else if (ext == ".ODT" || ext == "ODT")
            {
                return false;
            }
            else if (ext == ".RTF" || ext == "RTF")
            {
                return false;
            }
            else if (ext == ".TXT" || ext == "TXT")
            {
                return false;
            }
            else if (ext == ".HTML" || ext == "HTML")
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Code obtained from CodeProject on GitHub
        /// https://github.com/zaagan/Docx-Html-Docx/blob/master/DocxToHTML.Converter/HTMLConverter.cs
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns>html of document</returns>
        public string ConvertToHtml(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath) || Path.GetExtension(fullFilePath) != ".docx")
                return "Unsupported format";

            FileInfo fileInfo = new FileInfo(fullFilePath);

            string htmlText = string.Empty;
            try
            {
                htmlText = ParseDOCX(fileInfo);
            }
            catch (OpenXmlPackageException e)
            {

                if (e.ToString().Contains("Invalid Hyperlink"))
                {
                    using (FileStream fs = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        UriFixer.FixInvalidUri(fs, brokenUri => FixUri(brokenUri));
                    }
                    htmlText = ParseDOCX(fileInfo);
                }
            }


            return htmlText;
        }

        /// <summary>
        /// Corrects uris as they are converted to HTML
        /// </summary>
        /// <param name="brokenUri"></param>
        /// <returns></returns>
        private static System.Uri FixUri(string brokenUri)
        {
            string newURI = string.Empty;

            if (brokenUri.Contains("mailto:"))
            {
                int mailToCount = "mailto:".Length;
                brokenUri = brokenUri.Remove(0, mailToCount);
                newURI = brokenUri;
            }
            else
            {
                newURI = " ";
            }
            return new Uri(newURI);
        }

        /// <summary>
        /// Partial view for the Upload Menu
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult _UploadMenu()
        {
            ViewBag.Pseudonyms = String.Join(",", db.Pseudonyms.Select(i => i.PseudonymID));
            ViewBag.FormatTags = String.Join(",", db.FormatTags.Select(i => i.FormatID));

            return PartialView();
        }

        /// <summary>
        /// Partial view for the upload edit menu
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult _UploadEditMenu()
        {
            ViewBag.Pseudonyms = String.Join(",", db.Pseudonyms.Select(i => i.PseudonymID));
            ViewBag.FormatTags = String.Join(",", db.FormatTags.Select(i => i.FormatID));

            return PartialView();
        }

        /// <summary>
        /// Partial view for upload confirmation
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult _UploadConfirmation()
        {
            return PartialView();
        }

        /// <summary>
        /// Helper method for converting DOCX to HTML. Parses file.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private string ParseDOCX(FileInfo fileInfo)
        {

            try
            {

                // Create a file stream to read the data from
                FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
                // Create a byte array that will have the contents of the file.
                byte[] byteArray = new byte[fileStream.Length];
                // read the bytes from the file stream to the byte array.
                fileStream.Read(byteArray, 0, byteArray.Length);
                // Create a memory stream to manipulate the file information.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(byteArray, 0, byteArray.Length);

                    using (WordprocessingDocument wDoc = WordprocessingDocument.Open(memoryStream, true))
                    {

                        int imageCounter = 0;

                        var pageTitle = fileInfo.FullName;
                        var part = wDoc.CoreFilePropertiesPart;
                        if (part != null)
                            pageTitle = (string)part.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? fileInfo.FullName;

                        WmlToHtmlConverterSettings settings = new WmlToHtmlConverterSettings()
                        {
                            AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                            PageTitle = pageTitle,
                            FabricateCssClasses = true,
                            CssClassPrefix = "pt-",
                            RestrictToSupportedLanguages = false,
                            RestrictToSupportedNumberingFormats = false,
                            ImageHandler = imageInfo =>
                            {
                                ++imageCounter;
                                string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                                ImageFormat imageFormat = null;
                                if (extension == "png") imageFormat = ImageFormat.Png;
                                else if (extension == "gif") imageFormat = ImageFormat.Gif;
                                else if (extension == "bmp") imageFormat = ImageFormat.Bmp;
                                else if (extension == "jpeg") imageFormat = ImageFormat.Jpeg;
                                else if (extension == "tiff")
                                {
                                    extension = "gif";
                                    imageFormat = ImageFormat.Gif;
                                }
                                else if (extension == "x-wmf")
                                {
                                    extension = "wmf";
                                    imageFormat = ImageFormat.Wmf;
                                }

                                if (imageFormat == null)
                                    return null;

                                string base64 = null;
                                try
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        imageInfo.Bitmap.Save(ms, imageFormat);
                                        var ba = ms.ToArray();
                                        base64 = System.Convert.ToBase64String(ba);
                                    }
                                }
                                catch (System.Runtime.InteropServices.ExternalException)
                                { return null; }


                                ImageFormat format = imageInfo.Bitmap.RawFormat;
                                ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == format.Guid);
                                string mimeType = codec.MimeType;

                                string imageSource = string.Format("data:{0};base64,{1}", mimeType, base64);

                                XElement img = new XElement(Xhtml.img,
                                    new XAttribute(NoNamespace.src, imageSource),
                                    imageInfo.ImgStyleAttribute,
                                    imageInfo.AltText != null ?
                                        new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                                return img;
                            }
                        };

                        XElement htmlElement = WmlToHtmlConverter.ConvertToHtml(wDoc, settings);

                        var html = new XDocument(new XDocumentType("html", null, null, null), htmlElement);
                        var htmlString = html.ToString(SaveOptions.DisableFormatting);
                        return htmlString;
                    }
                }
            }
            catch
            {
                return "File contains corrupt data";
            }

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