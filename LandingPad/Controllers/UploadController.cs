using LandingPad.DAL;
using LandingPad.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            if (FileExt == ".DOCX" || FileExt == ".DOC")
            {
                if (!ModelState.IsValid)
                {
                    Stream str = file.InputStream;
                    //BinaryReader Br = new BinaryReader(str);
                    //Byte[] FileData = Br.ReadBytes((Int32)str.Length);
                    //This code is the same as above but shorter ==> No longer throwing
                    //an error but is not uploading to server
                    Byte[] FileData = new byte[file.ContentLength];

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
    }
}