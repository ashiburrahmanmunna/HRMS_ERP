using GTERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class CategoriesController : Controller
    {
        private GTRDBContext db;
        public CategoriesController(GTRDBContext context)
        {
            db = context;
        }
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        [Authorize]
        // GET: Categories
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            return View(db.Categories.Where(c => c.CategoryId > 0).ToList());
        }

        // GET: Categories/Details/5
        //[OverridableAuthorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [Authorize]
        //[OverridableAuthorize]
        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "CategoryId,CategoryCode,Name,CategoryDescription,CategoryImage,CategoryImagePath,CategoryFileExtension,LinkAdd")] Category category, HttpPostedFileBase file, string imageDatatest)
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
                if (category.CategoryId > 0)
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();

                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        category.CategoryImagePath = "uploads/Categories/";// + category.ProductId.ToString() + Extension.ToString();
                        category.CategoryFileExtension = Extension;

                        _FileName = category.CategoryId.ToString() + Extension;
                        _path = Path.Combine(Server.MapPath("~/uploads/Categories"), _FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        Image cropimage = HandleImageUpload(fileData, _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();


                        category.CategoryImage = imageData;
                        string imageUrls = "/uploads/Categories/" + _FileName;




                        db.Entry(category).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                else
                {


                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        category.CategoryImagePath = "uploads/Categories/";// + product.ProductId.ToString() + Extension.ToString();
                        category.CategoryFileExtension = Extension;
                    }



                    db.Categories.Add(category);
                    db.SaveChanges();


                    db.Entry(category).GetDatabaseValues();
                    int id = category.CategoryId; // Yes it's here




                    //_FileName = Path.GetFileName(DateTime.Now.ToBinary() + "-" + file.FileName);
                    _FileName = id.ToString() + Extension;
                    _path = Path.Combine(Server.MapPath("~/uploads/Categories"), _FileName);
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    Image cropimage = HandleImageUpload(fileData, _path);
                    MemoryStream ms = new MemoryStream();
                    cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageData = ms.ToArray();

                    category.CategoryImage = imageData;
                    string imageUrls = "/uploads/Categories/" + _FileName;


                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();


                    //db.Categories.Add(category);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

            //return View(category);
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
        //[OverridableAuthorize]
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("Create", category);

        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "CategoryId,CategoryCode,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [Authorize]
        //[OverridableAuthorize]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("Create", category);
        }
//        [Authorize]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        //[OverridableAuthorize]
        //      [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();

                string fullPath = Request.MapPath("~/" + category.CategoryImagePath + category.CategoryId + category.CategoryFileExtension);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return Json(new { Success = 1, CategoryId = category.CategoryId, ex = "" });

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
