using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]

    public class ProductsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;
        // GET: Products


        [Authorize]
        public ActionResult Index()
        {
            //string cultureinfo = "bd-BD";
            //string cultureinfo = "th-TH";

            //CultureInfo culture = new CultureInfo(cultureinfo, false);
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            var products = db.Products
                .Include(p => p.vPrimaryCategory)
                .Where(p => p.ProductId > 1 && p.comid.ToString() == AppData.intComId.ToString());
            return View(products.ToList());
        }

        [Authorize]
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize]
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CurrencyShortName");
            ViewBag.UnitId = new SelectList(db.Unit.Where(u => u.UnitId > -1), "UnitId", "UnitName");

            ViewBag.Title = "Create";
            return View();
        }





        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ProductId,ProductName,ProductBarcode,CategoryId,CountryId,Description,CostPrice,SalePrice,vatPercentage , vatAmount,ProductImage,unitid,ImagePath,FileExtension")] Product product, HttpPostedFileBase file, string imageDatatest)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

            if (Request.Files.Count > 0)
            {
                var uploadedfile = Request.Files[0];
                //save file logic

            }
            product.comid = int.Parse(Session["comid"].ToString());


            if (ModelState.IsValid)
            {
                if (product.ProductId > 0)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        product.ImagePath = "uploads/Products/";// + product.ProductId.ToString() + Extension.ToString();
                        product.FileExtension = Extension;

                        _FileName = product.ProductId.ToString() + Extension;
                        _path = Path.Combine(Server.MapPath("~/uploads/products"), _FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }





                        Image cropimage = HandleImageUpload(fileData, _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();


                        product.ProductImage = imageData;
                        string imageUrls = "/uploads/Products/" + _FileName;



                        product.comid = int.Parse(Session["comid"].ToString());
                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                else
                {




                    //clsMain.Dimensions dimen = new clsMain.Dimensions();
                    //_currentBitmap = Image.FromStream(file.InputStream);
                    //Bitmap cropimage = clsMain.ConstrainProportions(_currentBitmap, 300, dimen);



                    //Image image = Image.FromStream(file.InputStream);


                    //var TestImages = Bitmap.FromStream(file.InputStream);
                    //Bitmap cropimagetest = clsMain.ConstrainProportions(_currentBitmap, 300, dimen);


                    //MemoryStream ms = new MemoryStream();
                    //cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //byte[] imageData = ms.ToArray();




                    //var dataurl = Request["image-data"];
                    //var data = dataurl.Substring(dataurl.IndexOf(",") + 1);
                    ///var newfile = Convert.FromBase64String(data);


                    //product.ProductImage = imageData; //new byte[file.ContentLength];
                    //file.InputStream.Read(product.ProductImage, 0, file.ContentLength);

                    product.comid = int.Parse(Session["comid"].ToString());
                    db.Products.Add(product);
                    db.SaveChanges();

                    db.Entry(product).GetDatabaseValues();
                    int id = product.ProductId; // Yes it's here

                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        Extension = Path.GetExtension(fileName);

                        product.ImagePath = "uploads/Products/";// + product.ProductId.ToString() + Extension.ToString();
                        product.FileExtension = Extension;


                        _FileName = id.ToString() + Extension;
                        _path = Path.Combine(Server.MapPath("~/uploads/Products"), _FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }





                        Image cropimage = HandleImageUpload(fileData, _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();




                        //byte[] imageDatatestbytes = Convert.FromBase64String(imageDatatest);
                        //product.ProductImage = imageDatatestbytes;



                        product.ProductImage = imageData;
                        string imageUrls = "/uploads/Products/" + _FileName;
                    }


               







                    //_FileName = Path.GetFileName(DateTime.Now.ToBinary() + "-" + file.FileName);
                    


                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    //if (file != null && file.ContentLength > 0)
                    //{
                    //    // extract only the fieldname



                    //    // store the file inside ~/App_Data/uploads folder
                    //    //var path = Path.Combine(Server.MapPath("~/uploads"), id.ToString() + Extension); //fileName
                    //    ////file.SaveAs(path);
                    //    //cropimage.Save(path);
                    //    //string filePath = "~/ProfilePic/" + imageData.ToString();
                    //    //file.WriteAllBytes(Server.MapPath(filePath), imgArray);

                    //    //db.Entry(product).State = EntityState.Modified;
                    //    //db.SaveChanges();

                    //}


                }
                return RedirectToAction("Index");

            }


            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CurrencyShortName", product.CountryId);

            return View(product);
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

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }


            //var files = Directory.GetFiles("~/uploads/", product.ProductId + ".*");
            //if (files.Length > 0)
            //{
            //    var Extension =  Path.GetExtension(files[0].ToString());
            //    // at least one matching file exists
            //    // file name is files[0]
            //}


            ViewBag.Title = "Edit";

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CurrencyShortName", product.CountryId);
            ViewBag.UnitId = new SelectList(db.Unit, "UnitId", "UnitName");


            //return View(product);
            return View("Create", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "ProductId,ProductName,ProductBarcode,CategoryId,CountryId,Description,CostPrice,SalePrice,vatPercentage,VatAmount,UnitId,ProductImage,ImagePath,FileExtension")] Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                //if (file == null)
                //{

                //}
                //else
                //{
                //    product.ProductImage = new byte[file.ContentLength];
                //    file.InputStream.Read(product.ProductImage, 0, file.ContentLength);
                //}




                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    Extension = Path.GetExtension(fileName);

                    product.ImagePath = "uploads/Products/";// + product.ProductId.ToString() + Extension.ToString();
                    product.FileExtension = Extension;

                    _FileName = product.ProductId.ToString() + Extension;
                    _path = Path.Combine(Server.MapPath("~/uploads/Products"), _FileName);
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }





                    Image cropimage = HandleImageUpload(fileData, _path);
                    MemoryStream ms = new MemoryStream();
                    cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageData = ms.ToArray();


                    product.ProductImage = imageData;
                    string imageUrls = "/uploads/Products/" + _FileName;




                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //byte[] imageDatatestbytes = Convert.FromBase64String(imageDatatest);
                //product.ProductImage = imageDatatestbytes;
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CurrencyShortName", product.CountryId);

            return View(product);
        }
        [Authorize]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return BadRequest();
            //}
            //Product product = db.Products.Find(id);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            //return View(product);

            if (id == null)
            {
                return BadRequest();
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.CategoryId > 0), "CategoryId", "Name");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CurrencyShortName");
            ViewBag.UnitId = new SelectList(db.Unit.Where(u => u.UnitId > -1), "UnitId", "UnitName");

            return View("Create", product);
        }

        // POST: Products/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {


                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();


                string fullPath = Request.MapPath("~/" + product.ImagePath + product.ProductId + product.FileExtension);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return Json(new { Success = 1, ProductId = product.ProductId, ex = "" });

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

        //public byte[] imageToByteArray(System.Drawing.Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //    return ms.ToArray();
        //}

        //public Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}
    }
}
