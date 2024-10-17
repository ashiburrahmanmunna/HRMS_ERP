using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class SoftwarePackagesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        [Authorize]
        // GET: SoftwarePackages
        public ActionResult Index()
        {
            return View(db.SoftwarePackages.ToList());
        }
        [Authorize]
        // GET: SoftwarePackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SoftwarePackage softwarePackage = db.SoftwarePackages.Find(id);
            if (softwarePackage == null)
            {
                return NotFound();
            }
            return View(softwarePackage);
        }
        [Authorize]
        // GET: SoftwarePackages/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            return View();
        }

        // POST: SoftwarePackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "SoftwarePackageId,SoftwarePackageCode,Name,SoftwarePackageDescription,LinkAdd,SoftwarePackageImage,SoftwarePackageImagePath,SoftwarePackageFileExtension")] SoftwarePackage softwarePackage, HttpPostedFileBase file, string imageDatatest)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
                        .Select(x => new { x.Key, x.Value.Errors });

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase uploadedfile = Request.Files[0];
                //save file logic

            }

            if (ModelState.IsValid)
            {
                if (softwarePackage.SoftwarePackageId > 0)
                {
                    db.Entry(softwarePackage).State = EntityState.Modified;
                    db.SaveChanges();

                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        softwarePackage.SoftwarePackageImagePath = "uploads/SoftwarePackages/";// + softwarepackage.ProductId.ToString() + Extension.ToString();
                        softwarePackage.SoftwarePackageFileExtension = Extension;

                        _FileName = softwarePackage.SoftwarePackageId.ToString() + Extension;
                        _path = Path.Combine(Server.MapPath("~/uploads/SoftwarePackages"), _FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        Image cropimage = HandleImageUpload(fileData, _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();


                        softwarePackage.SoftwarePackageImage = imageData;
                        string imageUrls = "/uploads/SoftwarePackages/" + _FileName;




                        db.Entry(softwarePackage).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                else
                {


                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);
                        
                        softwarePackage.SoftwarePackageImagePath = "uploads/SoftwarePackages/";// + product.ProductId.ToString() + Extension.ToString();
                        softwarePackage.SoftwarePackageFileExtension = Extension;
                    }



                    db.SoftwarePackages.Add(softwarePackage);
                    db.SaveChanges();


                    db.Entry(softwarePackage).GetDatabaseValues();
                    int id = softwarePackage.SoftwarePackageId; // Yes it's here




                    //_FileName = Path.GetFileName(DateTime.Now.ToBinary() + "-" + file.FileName);
                    _FileName = id.ToString() + Extension;
                    _path = Path.Combine(Server.MapPath("~/uploads/SoftwarePackages"), _FileName);
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    Image cropimage = HandleImageUpload(fileData, _path);
                    MemoryStream ms = new MemoryStream();
                    cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageData = ms.ToArray();

                    softwarePackage.SoftwarePackageImage = imageData;
                    string imageUrls = "/uploads/SoftwarePackages/" + _FileName;


                    db.Entry(softwarePackage).State = EntityState.Modified;
                    db.SaveChanges();


                    //db.SoftwarePackage.Add(softwarepackage);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        private Image RezizeImage(Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(cpy))
                {
                    //gr.Clear(Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new Rectangle(0, 0, nnx, nny),
                        new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                }
                return cpy;
            }

        }

        private MemoryStream BytearrayToStream(byte[] arr)
        {
            return new MemoryStream(arr, 0, arr.Length);
        }

        private Image HandleImageUpload(byte[] binaryImage, string path)
        {
            Image img = RezizeImage(Image.FromStream(BytearrayToStream(binaryImage)), 300, 300);
            img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            return img;
        }

        [Authorize]
        // GET: SoftwarePackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SoftwarePackage softwarePackage = db.SoftwarePackages.Find(id);
            if (softwarePackage == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("Create", softwarePackage);
        }

        // POST: SoftwarePackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "SoftwarePackageId,SoftwarePackageCode,Name,SoftwarePackageDescription,LinkAdd,SoftwarePackageImage,SoftwarePackageImagePath,SoftwarePackageFileExtension")] SoftwarePackage softwarePackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(softwarePackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(softwarePackage);
        }
        [Authorize]
        // GET: SoftwarePackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SoftwarePackage softwarePackage = db.SoftwarePackages.Find(id);
            if (softwarePackage == null)
            {
                return NotFound();
            }
            return View(softwarePackage);
        }
        [Authorize]
        // POST: SoftwarePackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                SoftwarePackage softwarepackage = db.SoftwarePackages.Find(id);
                db.SoftwarePackages.Remove(softwarepackage);
                db.SaveChanges();

                string fullPath = Request.MapPath("~/" + softwarepackage.SoftwarePackageImagePath + softwarepackage.SoftwarePackageId + softwarepackage.SoftwarePackageFileExtension);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return Json(new { Success = 1, softwarepackageId = softwarepackage.SoftwarePackageId, ex = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });

            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
