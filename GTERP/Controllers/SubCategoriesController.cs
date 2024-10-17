using GTCommercial.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class SubCategoriesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        [Authorize]
        // GET: Categories
        public ActionResult Index()
        {
            return View(db.SubCategory.Where(c => c.SubCategoryId > 0).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SubCategory SubCategory = db.SubCategory.Find(id);
            if (SubCategory == null)
            {
                return NotFound();
            }
            return View(SubCategory);
        }
        [Authorize]
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "SubCategoryId,SubCategoryCode,CategoryId,SubCategoryName,SubCategoryDescription,SubCategoryImage,SubCategoryImagePath,SubCategoryFileExtension,LinkAdd")] SubCategory SubCategory, HttpPostedFileBase file, string imageDatatest)
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
                if (SubCategory.SubCategoryId > 0)
                {
                    db.Entry(SubCategory).State = EntityState.Modified;
                    db.SaveChanges();

                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        SubCategory.SubCategoryImagePath = "uploads/SubCategories/";// + SubCategory.ProductId.ToString() + Extension.ToString();
                        SubCategory.SubCategoryFileExtension = Extension;

                        _FileName = SubCategory.SubCategoryId.ToString() + Extension;
                        _path = Path.Combine(Server.MapPath("~/uploads/SubCategories"), _FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }





                        Image cropimage = HandleImageUpload(fileData, _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();


                        SubCategory.SubCategoryImage = imageData;
                        string imageUrls = "/uploads/SubCategories/" + _FileName;




                        db.Entry(SubCategory).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                else
                {


                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        SubCategory.SubCategoryImagePath = "uploads/SubCategories/";// + product.ProductId.ToString() + Extension.ToString();
                        SubCategory.SubCategoryFileExtension = Extension;
                    }



                    db.SubCategory.Add(SubCategory);
                    db.SaveChanges();


                    db.Entry(SubCategory).GetDatabaseValues();
                    int id = SubCategory.SubCategoryId; // Yes it's here




                    //_FileName = Path.GetFileName(DateTime.Now.ToBinary() + "-" + file.FileName);
                    _FileName = id.ToString() + Extension;
                    _path = Path.Combine(Server.MapPath("~/uploads/SubCategories"), _FileName);
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    Image cropimage = HandleImageUpload(fileData, _path);
                    MemoryStream ms = new MemoryStream();
                    cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageData = ms.ToArray();

                    SubCategory.SubCategoryImage = imageData;
                    string imageUrls = "/uploads/SubCategories/" + _FileName;


                    db.Entry(SubCategory).State = EntityState.Modified;
                    db.SaveChanges();


                    //db.Categories.Add(SubCategory);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

            //return View(SubCategory);
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
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SubCategory SubCategory = db.SubCategory.Find(id);
            if (SubCategory == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("Create", SubCategory);

        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "SubCategoryId,SubCategoryCode,Name")] SubCategory SubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(SubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(SubCategory);
        }
        [Authorize]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SubCategory SubCategory = db.SubCategory.Find(id);
            if (SubCategory == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("Create", SubCategory);
        }
//        [Authorize]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
  //      [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                SubCategory SubCategory = db.SubCategory.Find(id);
                db.SubCategory.Remove(SubCategory);
                db.SaveChanges();

                string fullPath = Request.MapPath("~/" + SubCategory.SubCategoryImagePath + SubCategory.SubCategoryId + SubCategory.SubCategoryFileExtension);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return Json(new { Success = 1, SubCategoryId = SubCategory.SubCategoryId, ex = "" });

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
