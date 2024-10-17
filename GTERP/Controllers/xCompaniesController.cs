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
using MasterDetail.Models;

namespace MasterDetail.Controllers
{
    public class CompaniesController : Controller
    {
        private MasterDetailContext db = new MasterDetailContext();
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Company.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            XCompany company = db.Company.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "CompanyId,CompanyCode,CompanyName,CompanyShortName,PrimaryAddress,SecoundaryAddress,comPhone,comPhone2,comFax,comEmail,comWeb,ContPerson,ContDesig,IsInActive,ComImageHeader,ComLogo")] XCompany company, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Company.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            XCompany company = db.Company.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "CompanyId,CompanyCode,CompanyName,CompanyShortName,PrimaryAddress,SecoundaryAddress,comPhone,comPhone2,comFax,comEmail,comWeb,ContPerson,ContDesig,IsInActive,ComImageHeader,ComLogo")] XCompany company , HttpPostedFileBase[] file)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(company).State = EntityState.Modified;
                //db.SaveChanges();
                int i = 0;
                foreach (var item in file)
                {

                    
                    if (file != null && item.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(item.FileName);
                        Extension = Path.GetExtension(fileName);

                        //company.ImagePath = "uploads/";// + product.ProductId.ToString() + Extension.ToString();
                        //company.FileExtension = Extension;

                        _FileName = company.CompanyId.ToString() + Extension;
                        _path = Path.Combine(Server.MapPath("~/uploads/Companies"), _FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(item.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(item.ContentLength);
                        }




                        

                        //Image cropimage = HandleImageUpload(fileData, _path);
                        MemoryStream ms = new MemoryStream();
                        //cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                        //byte[] imageData = ms.ToArray();


                        if (i == 0)
                        {
                            company.ComImageHeader = fileData;
                            string imageUrls = "/uploads/companies/comimageheader/" + _FileName;
                        }
                        else if (i == 1)
                        {
                            company.ComLogo = fileData;
                            string imageUrls = "/uploads/companies/comlogo/" + _FileName;

                        }
                       


                        db.Entry(company).State = EntityState.Modified;
                        db.SaveChanges();

                        i++;
                    }
                }


                return RedirectToAction("Index");
            }
            return View(company);
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
                    gr.Clear(Color.Transparent);

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


        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            XCompany company = db.Company.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            XCompany company = db.Company.Find(id);
            db.Company.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
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
