using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;

        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;
        public CategoryRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetCategoryList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.CategoryId.ToString(),
                Text = x.Name
            });
        }



        public void CreateCategory(Category catagory, IFormFile file, string imageDatatest)
        {
            var uploadlocation = "/Content/img/Categories/";
            catagory.UserId = _httpContext.HttpContext.Session.GetString("userid");
            catagory.ComId = _httpContext.HttpContext.Session.GetString("comid");
            if (catagory.CategoryId > 0)
            {
                catagory.DateUpdated = DateTime.Now;
                catagory.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
                _context.Entry(catagory).State = EntityState.Modified;
                _context.SaveChanges();

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

                    _context.Entry(catagory).State = EntityState.Modified;
                    _context.SaveChanges();
                }

            }
            else
            {
                _context.Add(catagory);
                _context.SaveChanges();
                _context.Entry(catagory).GetDatabaseValues();
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

                    _context.Entry(catagory).State = EntityState.Modified;
                    _context.SaveChanges();
                }


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
    }

}
