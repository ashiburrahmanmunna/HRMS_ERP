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
    public class CategoriesController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        public CategoriesController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;

        }

        // GET: catagory
        public async Task<IActionResult> Index()
        {

            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            var data = await db.Categories.Where(x => x.ComId == comid).ToListAsync();
            return View(data);
        }

        // GET: catagory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catagory = await db.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (catagory == null)
            {
                return NotFound();
            }

            return View(catagory);
        }

        // GET: catagory/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            //ViewBag.CountryId = new SelectList(db.Cat_Country, "CountryId", "CountryName");
            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CatName");
            return View();
        }

        // POST: catagory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind(" DistId ,DistName,DistNameShort,aId ,wId , LUserId ,PCName")]*/ Category catagory, IFormFile file, string imageDatatest)
        {
            var uploadlocation = "/Content/img/Categories/";
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });


            if (ModelState.IsValid)
            {
                catagory.UserId = HttpContext.Session.GetString("userid");
                catagory.ComId = HttpContext.Session.GetString("comid");

                if (catagory.CategoryId > 0)
                {
                    //if (file != null && file.Length > 0)
                    //{
                    //    fileName = Path.GetFileName(file.FileName);
                    //    Extension = Path.GetExtension(fileName);
                    //    catagory.CategoryImagePath = uploadlocation;
                    //    catagory.CategoryFileExtension = Extension;

                    //    _FileName = catagory.CategoryId + Extension;
                    //    _path = uploadlocation + _FileName;// Path.Combine(Server.MapPath("~/Content/img/Companies/comimageheader/"), _FileName);
                    //    byte[] fileData = null;
                    //    using (BinaryReader binaryreader = new BinaryReader(file.OpenReadStream()))
                    //    {
                    //        fileData = binaryreader.ReadBytes((int)file.Length);
                    //    }

                    //    Image cropimage = HandleImageUpload(fileData, _path);
                    //    MemoryStream ms = new MemoryStream();
                    //    cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //    byte[] imageData = ms.ToArray();

                    //    catagory.CategoryImage = imageData;
                    //}

                    catagory.DateUpdated = DateTime.Now;
                    catagory.UpdateByUserId = HttpContext.Session.GetString("userid");
                    db.Entry(catagory).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), catagory.CategoryId.ToString(), "Update", catagory.Name.ToString());


                    if (file != null && file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        catagory.CategoryImagePath = uploadlocation;// + SubCategory.ProductId.ToString() + Extension.ToString();
                        catagory.CategoryFileExtension = Extension;

                        _FileName = catagory.CategoryId.ToString() + Extension;
                        _path = uploadlocation + _FileName;
                        byte[] fileData = null;
                        using (BinaryReader binaryreader = new BinaryReader(file.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)file.Length);
                        }

                        Image cropimage = HandleImageUpload(fileData, "wwwroot/" + _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageData = ms.ToArray();


                        catagory.CategoryImage = imageData;

                        db.Entry(catagory).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                else
                {
                    db.Add(catagory);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), catagory.CategoryId.ToString(), "Create", catagory.Name.ToString());

                    db.Entry(catagory).GetDatabaseValues();
                    int id = catagory.CategoryId; // Yes it's here



                    if (file != null && file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);
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

                        catagory.CategoryImage = imageData;

                        catagory.CategoryImagePath = uploadlocation;
                        catagory.CategoryFileExtension = Extension;

                        db.Entry(catagory).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                return RedirectToAction(nameof(Index));
            }
            return View(catagory);
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
        // GET: catagory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            //Cat_Country  cat_Country  = db.Cat_Country.Where(m => m.BuyerId.ToString() == id.ToString()).FirstOrDefault(); //Find(id);// 
            var catagory = await db.Categories.FindAsync(id);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CatName", catagory.CategoryId);
            if (catagory == null)
            {
                return NotFound();
            }

            return View("Create", catagory);
        }

        // POST: catagory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,ComId,CatName,Remarks,SLNO,IsInactive,UserId, Pcname")] Category catagory)
        {
            if (id != catagory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(catagory);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatagoryExists(catagory.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(catagory);
        }

        // GET: catagory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catagory = await db.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (catagory == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CatName", catagory.CategoryId);
            return View("Create", catagory);
        }

        // POST: catagory/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            try
            {
                var catagory = await db.Categories.FindAsync(id);
                db.Categories.Remove(catagory);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), catagory.CategoryId.ToString(), "Delete", catagory.Name);

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, CategoryId = catagory.CategoryId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        private bool CatagoryExists(int id)
        {
            return db.Categories.Any(e => e.CategoryId == id);
        }
    }
}
