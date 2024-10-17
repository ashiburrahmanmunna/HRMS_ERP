using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GTCommercial.BL;
using GTCommercial.Wrappers;
using GTCommercial.Models;
using GTCommercial.Helpers;

namespace GTCommercial.Controllers
{
    public class UploadController : Controller
    {
        private UploadManager attachmentManager = new UploadManager();
        private GTRDBContext db = new GTRDBContext();

        //
        // GET: /Uploader/
        public ActionResult Index()
        {
            var Files = attachmentManager.GetAllMainAttachments();
            return View(Files);
        }
        //
        // GET: /Uploader/Upload
        public ActionResult Upload()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("UploadSingle");
            }
            else
            {
                return new HttpNotFoundResult();
            }

        }
        //
        // POST: /Uploader/Upload
        [HttpPost]
        public JsonResult Upload(AttachmentMain attachmentsub,HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.ContentLength > 0)
            {
                byte[] FileByteArray = new byte[uploadedFile.ContentLength];
                uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);
                AttachmentSub newAttchment = new AttachmentSub();
                //newAttchment.BasedOn = attachmentsub.BasedOn;
                //newAttchment.BasedId = attachmentsub.BasedId;

                newAttchment.FileName = uploadedFile.FileName;
                newAttchment.FileType = uploadedFile.ContentType;
                newAttchment.FileContent = FileByteArray;
                UploadOperationResult operationResult = attachmentManager.SaveAttachment(newAttchment);
                if (operationResult.Success)
                {
                    string HTMLString = CaptureHelper.RenderViewToString("_AttachmentItem", newAttchment, this.ControllerContext);
                    return Json(new
                    {
                        statusCode = 200,
                        status = operationResult.Message,
                        NewRow = HTMLString
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        statusCode = 400,
                        status = operationResult.Message,
                        file = uploadedFile.FileName
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            return Json(new
            {
                statusCode = 400,
                status = "Bad Request! Upload Failed",
                file = string.Empty
            }, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DownloadAttachment(int id)
        {
            AttachmentSub attachment = attachmentManager.GetAttachmentSub(id);
            return File(attachment.FileContent, attachment.FileType, attachment.FileName);
        }
        [HttpPost]
        public ActionResult DeleteAttachmentMain(int id)
        {
            UploadOperationResult OperationResult = new UploadOperationResult();
            OperationResult = attachmentManager.DeleteMain(id);
            if (OperationResult.Success)
            {
                return Json(new { ID = id });
            }
            else
            {
                return Json(new { ID = "", message = OperationResult.Message });
            }
        }
        [HttpPost]
        public ActionResult DeleteAttachmentSub(int id)
        {
            UploadOperationResult OperationResult = new UploadOperationResult();
            OperationResult = attachmentManager.DeleteSub(id);
            if (OperationResult.Success)
            {
                return Json(new { ID = id });
            }
            else
            {
                return Json(new { ID = "", message = OperationResult.Message });
            }
        }

        //
        // GET: /Uploader/Upload
        public ActionResult UploadMultiple()
        {
            //this.ViewBag.ProductSearch = db.Products.Where(c => c.ProductId > 0);
            this.ViewBag.Product = new SelectList(db.Products.Where(c => c.ProductId > 0), "ProductId", "ProductName");

            if (Request.IsAjaxRequest())
            {
                return PartialView("_UploadMultiple");
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }
        [HttpPost]
        public JsonResult UploadMultiple(AttachmentMain AttachmentMain, HttpPostedFileBase[] uploadedFiles)
        {
            AttachmentMain newAttachmentMain = new AttachmentMain();

            //newAttachmentMain.BasedOn = AttachmentMain.BasedOn;
            //newAttachmentMain.BasedId = AttachmentMain.BasedId;
            //newAttachmentMain.AttachmentDate = DateTime.Now.Date;
            //newAttachmentMain.AttachmentDesc = AttachmentMain.AttachmentDesc;
            Guid guid = Guid.NewGuid();

            newAttachmentMain.BasedOn = "Product";
            newAttachmentMain.BasedId = 101;
            newAttachmentMain.AttachmentDate = DateTime.Now.Date;
            newAttachmentMain.AttachmentDesc = "Test Desc.";
            newAttachmentMain.Wid = guid;

            List<AttachmentSub> newAttachmentList = new List<AttachmentSub>();
            foreach (var File in uploadedFiles)
            {
                if (File != null && File.ContentLength > 0)
                {
                    byte[] FileByteArray = new byte[File.ContentLength];
                    File.InputStream.Read(FileByteArray, 0, File.ContentLength);
                    AttachmentSub newAttchment = new AttachmentSub();
                    newAttchment.FileName = File.FileName;
                    newAttchment.FileType = File.ContentType;
                    newAttchment.FileContent = FileByteArray;
                    newAttachmentList.Add(newAttchment);
                }
            }
            newAttachmentMain.vAttachmentSub = newAttachmentList;


            UploadOperationResult operationResult = attachmentManager.SaveAttachments(newAttachmentMain);
            if (operationResult.Success)
            {
                string HTMLString = CaptureHelper.RenderViewToString("_AttachmentBulk", newAttachmentList, this.ControllerContext);
                return Json(new
                {
                    statusCode = 200,
                    status = operationResult.Message,
                    NewRow = HTMLString
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    statusCode = 400,
                    status = operationResult.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}