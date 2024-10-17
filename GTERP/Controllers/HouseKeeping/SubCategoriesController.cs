using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    [OverridableAuthorize]
    public class SubCategoriesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        private TransactionLogRepository tranlog;
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;


        public SubCategoriesController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {

            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");


            //return View(db.SubCategory.Where(c => c.SubCategoryId > 0).ToList());
            return View(await db.SubCategory.Where(c => c.comid == HttpContext.Session.GetString("comid")).Include(x => x.Category).Where(c => c.SubCategoryId > 0).ToListAsync());

        }


        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.ComId == HttpContext.Session.GetString("comid")).Where(c => c.CategoryId > 0), "CategoryId", "Name");

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SubCategoryId,SubCategoryCode,CategoryId,SubCategoryName,SubCategoryDescription,SubCategoryImage,SubCategoryImagePath,SubCategoryFileExtension,LinkAdd,comid,userid,useridUpdate,DateAdded,DateUpdated")] SubCategory SubCategory, IFormFile file, string imageDatatest)
        {
            var uploadlocation = "/Content/img/SubCategories/";
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });


            if (ModelState.IsValid)
            {
                if (SubCategory.SubCategoryId > 0)
                {

                    SubCategory.DateUpdated = DateTime.Now;
                    SubCategory.comid = HttpContext.Session.GetString("comid");

                    if (SubCategory.userid == null)
                    {
                        SubCategory.userid = HttpContext.Session.GetString("userid");
                    }
                    SubCategory.useridUpdate = HttpContext.Session.GetString("userid");



                    db.Entry(SubCategory).State = EntityState.Modified;
                    db.SaveChanges();

                    if (file != null && file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        SubCategory.SubCategoryImagePath = uploadlocation;// + SubCategory.ProductId.ToString() + Extension.ToString();
                        SubCategory.SubCategoryFileExtension = Extension;

                        _FileName = SubCategory.SubCategoryId.ToString() + Extension;
                        _path = uploadlocation + _FileName;
                        byte[] fileData = null;
                        using (BinaryReader binaryreader = new BinaryReader(file.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)file.Length);
                        }

                        Image cropimage = HandleImageUpload(fileData, "wwwroot" + _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageData = ms.ToArray();


                        SubCategory.SubCategoryImage = imageData;

                        db.Entry(SubCategory).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                else
                {
                    SubCategory.userid = HttpContext.Session.GetString("userid");
                    SubCategory.comid = HttpContext.Session.GetString("comid");
                    SubCategory.DateAdded = DateTime.Now;

                    if (file != null && file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        SubCategory.SubCategoryImagePath = uploadlocation;// + product.ProductId.ToString() + Extension.ToString();
                        SubCategory.SubCategoryFileExtension = Extension;
                    }



                    db.SubCategory.Add(SubCategory);
                    db.SaveChanges();


                    db.Entry(SubCategory).GetDatabaseValues();
                    int id = SubCategory.SubCategoryId; // Yes it's here



                    if (file != null && file.Length > 0)
                    {
                        //_FileName = Path.GetFileName(DateTime.Now.ToBinary() + "-" + file.FileName);
                        _FileName = id.ToString() + Extension;
                        _path = uploadlocation + _FileName;
                        byte[] fileData = null;
                        using (BinaryReader binaryreader = new BinaryReader(file.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)file.Length);
                        }

                        Image cropimage = HandleImageUpload(fileData, "wwwroot/" + _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();

                        SubCategory.SubCategoryImage = imageData;



                        db.Entry(SubCategory).State = EntityState.Modified;
                        db.SaveChanges();
                    }

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


        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SubCategory SubCategory = await db.SubCategory.Where(c => c.comid == HttpContext.Session.GetString("comid")).Where(c => c.SubCategoryId == id).FirstOrDefaultAsync();
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.ComId == HttpContext.Session.GetString("comid")).Where(c => c.CategoryId > 0), "CategoryId", "Name");

            if (SubCategory == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("Create", SubCategory);

        }


        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SubCategory SubCategory = await db.SubCategory.Where(c => c.comid == HttpContext.Session.GetString("comid")).Where(c => c.SubCategoryId == id).FirstOrDefaultAsync();
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.ComId == HttpContext.Session.GetString("comid")).Where(c => c.CategoryId > 0), "CategoryId", "Name");

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

                SubCategory SubCategory = db.SubCategory.Where(c => c.comid == HttpContext.Session.GetString("comid")).Where(c => c.SubCategoryId == id).FirstOrDefault();

                db.SubCategory.Remove(SubCategory);
                db.SaveChanges();

                string fullPath = ("~/" + SubCategory.SubCategoryImagePath + SubCategory.SubCategoryId + SubCategory.SubCategoryFileExtension);
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
