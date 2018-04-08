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
        public ActionResult FileUpload(HttpPostedFileBase file, [Bind(Include = "ProfileID, Title, Title, Document, AddDate, EditDate, LikesOn," +
                "CommentsOn, CritiqueOn, DocType, DescriptionText")] Writing doc)
        {

            String FileExt = Path.GetExtension(file.FileName).ToUpper();

            if (FileExt == ".DOCX" || FileExt == ".DOC")
            {
                if (ModelState.IsValid)
                {
                    //Stream str = file.InputStream;
                    //BinaryReader Br = new BinaryReader(str);
                    //Byte[] FileData = Br.ReadBytes((Int32)str.Length);
                    //This code is the same as above but shorter ==> Still throws the same error
                    // as the code above
                    Byte[] FileData = new byte[file.ContentLength];
                    Console.WriteLine(FileData);

                    Writing wr = new Writing();
                    wr.DocType = FileExt;
                    wr.AddDate = DateTime.Now;
                    wr.EditDate = DateTime.Now;
                    wr.Document = FileData;
                    wr.Title = doc.Title;
                    wr.DescriptionText = doc.DescriptionText;
                    wr.LikesOn = doc.LikesOn;
                    wr.CritiqueOn = doc.CritiqueOn;
                    wr.CommentsOn = doc.CommentsOn;
                    db.Writings.Add(wr);
                    db.SaveChanges();
                    return RedirectToAction("FileUpload");
                }
                else
                {
                    ViewBag.FileStatus = "Invalid file format.";
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