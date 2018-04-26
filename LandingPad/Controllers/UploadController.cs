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

        /*
        public ActionResult ViewDoc(int? id)
        {
            return View();
        }*/

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

        /// <summary>
        /// Helper method for Upload Post
        /// </summary>
        public bool CheckExt(String ext)
        {
            if(ext == ".DOCX" || ext == ".DOC")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}