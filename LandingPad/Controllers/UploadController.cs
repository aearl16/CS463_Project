using LandingPad.DAL;
using LandingPad.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mammoth;
using System.Text;
using System.Data.Entity;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Packaging;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace LandingPad.Controllers
{
    public class UploadController : Controller
    {
        private LandingPadContext db = new LandingPadContext();

        public ActionResult FileUpload()
        {
            return View();
        }


        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing doc)
        {

            String FileExt = Path.GetExtension(file.FileName).ToUpper();

            if (CheckExt(FileExt))
            {
                if (!ModelState.IsValid)
                {
                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileData = Br.ReadBytes((Int32)str.Length);

                    Writing wr = new Writing()
                    {
                        ProfileID = doc.ProfileID,
                        DocType = FileExt,
                        AddDate = DateTime.Now,
                        EditDate = DateTime.Now,
                        Document = FileData,
                        Title = doc.Title,
                        DescriptionText = doc.DescriptionText,
                        LikesOn = doc.LikesOn,
                        CritiqueOn = doc.CritiqueOn,
                        CommentsOn = doc.CommentsOn
                        //AccessPermission ac = new AccessPermission() //Later Feature
                    };
                    db.Writings.Add(wr);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
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

        public ActionResult UploadEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadEdit(HttpPostedFileBase file, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing doc)
        {
            
            String FileExt = Path.GetExtension(file.FileName).ToUpper();

            if (CheckExt(FileExt))
            {
                if (!ModelState.IsValid)
                {
                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileData = Br.ReadBytes((Int32)str.Length);
                    string html = string.Empty;

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
                    else
                    {
                        ViewBag.FileStatus = "Model Invalid";
                        return View();
                    }

                    Writing wr = new Writing()
                    {
                        ProfileID = doc.ProfileID,
                        DocType = ".HTML",
                        AddDate = DateTime.Now,
                        EditDate = DateTime.Now,
                        Document = Encoding.Unicode.GetBytes(html),
                        Title = doc.Title,
                        DescriptionText = doc.DescriptionText,
                        LikesOn = doc.LikesOn,
                        CritiqueOn = doc.CritiqueOn,
                        CommentsOn = doc.CommentsOn
                        //AccessPermission ac = new AccessPermission() //Later Feature
                    };
                    db.Writings.Add(wr);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
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



    public FileResult ViewDOC(int? id)
        {
            Writing wr = db.Writings.Find(id);

            var DocByte = wr.Document;

            return File(DocByte, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

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
                this.HttpContext.Response.AddHeader("Content-Disposition", "filename=" + "yourdocument.html");
            }
            else if(wr.DocType == "DOC" || wr.DocType == ".DOC")
            {
                this.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + "yourdocument.doc");
            }
            else if(wr.DocType == "ODT" || wr.DocType ==".ODT")
            {
                this.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + "yourdocument.odt");
            }
            else if(wr.DocType == "PDF" || wr.DocType == ".PDF")
            {
                this.HttpContext.Response.ContentType = "application/pdf";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + "yourdocument.pdf");
            }
            else
            {
                this.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                this.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + "yourdocument.docx");
            }
            
            this.HttpContext.Response.Buffer = true;
            this.HttpContext.Response.Clear();
            this.HttpContext.Response.OutputStream.Write(contentAsBytes, 0, contentAsBytes.Length);
            this.HttpContext.Response.OutputStream.Flush();
            this.HttpContext.Response.End();

            return View();
        }

        public ActionResult Edit(int? id)
        {
            Writing wr = db.Writings.Find(id);
            return View(wr);
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "ProfileID, Title, LikesOn," +
                "CommentsOn, CritiqueOn, DescriptionText")] Writing doc)
        {
            String FileExt = Path.GetExtension(file.FileName).ToUpper();

            if (CheckExt(FileExt))
            {
                if (!ModelState.IsValid)
                {
                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileData = Br.ReadBytes((Int32)str.Length);
                    //doc.Document = FileData;
                    //doc.EditDate = DateTime.Now;

                    Writing wr = doc;
                    doc.Document = FileData;
                    doc.EditDate = DateTime.Now;

                    /*
                    Writing wr = new Writing()
                    {
                        ProfileID = doc.ProfileID,
                        DocType = FileExt,
                        AddDate = doc.AddDate,
                        EditDate = DateTime.Now,
                        Document = FileData,
                        Title = doc.Title,
                        DescriptionText = doc.DescriptionText,
                        LikesOn = doc.LikesOn,
                        CritiqueOn = doc.CritiqueOn,
                        CommentsOn = doc.CommentsOn
                        //AccessPermission ac = new AccessPermission() //Later Feature
                    };*/
                    
                    db.Entry(wr).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
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


        private string ParseDOCX(FileInfo fileInfo)
        {

            try
            {
                byte[] byteArray = File.ReadAllBytes(fileInfo.FullName);
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
    }

}