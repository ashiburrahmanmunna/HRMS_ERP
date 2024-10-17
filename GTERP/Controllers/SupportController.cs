using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class SupportController : Controller
    {
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        private GTRDBContext db = new GTRDBContext();

        //
        // GET: /Support/
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            return View(db.Supports.ToList());
        }

        //
        // GET: /Support/Create
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Support support)
        {
            support.ShipViewId = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                
                db.Supports.Add(support);
                db.SaveChanges();
                int SupportId = support.SupportId;

                byte[] fileData = null;
                List<FileDetail> fileDetails = new List<FileDetail>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
         

                        var fileName = Path.GetFileName(file.FileName);

                    

                        FileDetail fileDetail = new FileDetail()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid(),
                            //DBFile = fileData
                        };

                        //fileDetails.Add(fileDetail);
                        var Folderpath = Path.Combine(Server.MapPath("~/App_Data/Upload/" + SupportId + "/"));

                        var path = Path.Combine(Server.MapPath("~/App_Data/Upload/"+ SupportId+"/"), fileDetail.Id + fileDetail.Extension);

                        if (!Directory.Exists(Folderpath))
                        {
                            Directory.CreateDirectory(Folderpath);
                        }



                        file.SaveAs(path);


                        if (ImageExtensions.Contains(Path.GetExtension(file.FileName).ToUpperInvariant()))
                        {
                            // process image
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(file.ContentLength);


                                Image cropimage = HandleImageUpload(fileData, path , 300 , 300);
                                MemoryStream ms = new MemoryStream();
                                cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] imageData = ms.ToArray();

                                fileDetail.DBFile = imageData;
                                fileDetails.Add(fileDetail);
                            }
                        }
                        else
                        {
                            fileDetails.Add(fileDetail);
                        }

                        
                    }
                }

                Support supportExist = db.Supports.Include(s => s.FileDetails).SingleOrDefault(x => x.SupportId == SupportId);

                supportExist.FileDetails = fileDetails;
                ///db.Supports.Add(support);
                db.Entry(support).State = EntityState.Modified;
                db.SaveChanges();
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(support);
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

        private Image HandleImageUpload(byte[] binaryImage, string path ,int width ,int height)
        {
            Image img = RezizeImage(Image.FromStream(BytearrayToStream(binaryImage)), width, height);
            //img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            return img;
        }

        //
        // GET: /Support/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit";

            if (id == null)
            {
                return BadRequest();
            }
            Support support = db.Supports.Include(s => s.FileDetails).SingleOrDefault(x => x.SupportId == id);
            if (support == null)
            {
                return NotFound();
            }
            return View(support);
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Details(int? id)
        {
            ViewBag.Title=  "Details";
            if (id == null)
            {
                return BadRequest();
            }
            Support support = db.Supports.Include(s => s.FileDetails).SingleOrDefault(x => x.SupportId == id);
            if (support == null)
            {
                return NotFound();
            }
            return View("Edit", support);
        }
        public ActionResult ViewShipDetails(string id)
        {
            ViewBag.Title = "Details";
            if (id.ToString() == null || id.ToString() == "")
            {
                return BadRequest();
            }
            Support support = db.Supports.Include(s => s.FileDetails).SingleOrDefault(x => x.ShipViewId.ToString() == id.ToString());
            if (support == null)
            {
                return NotFound();
            }
            string url = HttpContext.Request.Url.AbsoluteUri;
            //string url = this.Url.Action("ViewShipDetails", "Support", null);
            //            string shortenaddress = Shorten("http://103.106.236.213/MVCEF/Support/ViewShipDetails/f15903e1-e07f-4842-bc1d-53fa2a13f7e9", "o_2k8ualoj6n", "R_5809e48aeb9c48c88f6f432e6f879410");
            string shortenaddress = Shorten(url, "o_2k8ualoj6n", "R_5809e48aeb9c48c88f6f432e6f879410");

            ViewBag.shortenaddress = shortenaddress;

            return View("Edit", support);
        }


        public FileResult Download(String p, String d, int i)
        {
            return File(Path.Combine(Server.MapPath("~/App_Data/Upload/"+ i+"/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }

        public string Shorten(string longUrl, string login, string apikey)
        {
            var url = string.Format("http://api.bit.ly/shorten?format=json&version=2.0.1&longUrl={0}&login={1}&apiKey={2}", HttpUtility.UrlEncode(longUrl), login, apikey);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    dynamic jsonResponse = js.Deserialize<dynamic>(reader.ReadToEnd());
                    string s = jsonResponse["results"][longUrl]["shortUrl"];
                    return s;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }


        //
        // POST: /Support/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Support support)
        {
            if (ModelState.IsValid)
            {

                //New Files
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] fileData = null;
                        var fileName = Path.GetFileName(file.FileName);
                        FileDetail fileDetail = new FileDetail()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid(),
                            SupportId = support.SupportId
                            
                        };

                        var Folderpath = Path.Combine(Server.MapPath("~/App_Data/Upload/" + support.SupportId + "/"));

                        var path = Path.Combine(Server.MapPath("~/App_Data/Upload/" + support.SupportId + "/"), fileDetail.Id + fileDetail.Extension);

                        if (!Directory.Exists(Folderpath))
                        {
                            Directory.CreateDirectory(Folderpath);
                        }
                        //var path = Path.Combine(Server.MapPath("~/App_Data/Upload/"), fileDetail.Id + fileDetail.Extension);
                        file.SaveAs(path);

                        if (ImageExtensions.Contains(Path.GetExtension(file.FileName).ToUpperInvariant()))
                        {
                            // process image
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(file.ContentLength);


                                Image cropimage = HandleImageUpload(fileData, path, 300, 300);
                                MemoryStream ms = new MemoryStream();
                                cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] imageData = ms.ToArray();

                                fileDetail.DBFile = imageData;
                                //fileDetails.Add(fileDetail);
                                db.Entry(fileDetail).State = EntityState.Added;
                            }
                        }

                        
                    }
                }

                db.Entry(support).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(support);
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public JsonResult DeleteFile(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Guid guid = new Guid(id);
                FileDetail fileDetail = db.FileDetails.Find(guid);
                if (fileDetail == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                db.FileDetails.Remove(fileDetail);
                db.SaveChanges();

                //Delete file from the file system
                var path = Path.Combine(Server.MapPath("~/App_Data/Upload/"+ fileDetail.SupportId+"/"), fileDetail.Id + fileDetail.Extension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }





        //
        // POST: /Support/Delete/5

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult Delete(int id)
        {
            try
            {
                Support support = db.Supports.Find(id);
                if (support == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //delete files from the file system

                //foreach (var item in support.FileDetails)
                //{
                //    String path = Path.Combine(Server.MapPath("~/App_Data/Upload/"), item.Id + item.Extension);
                //    if (System.IO.File.Exists(path))
                //    {
                //        System.IO.File.Delete(path);
                //    }
                //}

                String path = Path.Combine(Server.MapPath("~/App_Data/Upload/"), support.SupportId + "");

                //string mappedPath1 = Server.MapPath(@"~/Content/Essential_Folder/attachments_AR/" + idnumber);

                //DirectoryInfo attachments_AR = new DirectoryInfo(path);

                //EmptyFolder(attachments_AR);
                //Directory.Delete(path);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                //Directory.Delete(path, true);

                //if (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}


                db.Supports.Remove(support);
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}