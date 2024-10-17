using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using GTERP.Controllers.HR;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HRVariables;
using GTERP.Interfaces.Tax;
using GTERP.Interfaces.Tax_Report;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.ViewModels;
using GTERP.Repository.HR;
using GTERP.Repository.HRVariables;
using GTERP.Repository.Tax;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.Tax
{
    public class TaxController : Controller
    {
        private readonly ILogger<HRController> _logger;
        private readonly GTRDBContext _context;
        private readonly ITaxRepository _taxRepository;
        private readonly IProcessLockRepository _processLockRepository;
        private readonly IClientContractPayment _clientContractPayment;
        private readonly IClientCompany _clientCompany;
        private readonly IClientTaxInfo _clientTaxInfo;
        private readonly IClientTaxPayment _clientTaxPayment;
        private readonly IClientTaxCosting _clientTaxCosting;
        private readonly IDesignationRepository _designationRepository;
        private readonly IClientContactPayment _clientContactPayment;
        private readonly IDocumentInfo _documentInfo;
        private readonly IWebHostEnvironment _env;
        public readonly ITaxReportRepository _taxReportRepository;
        private readonly IDocumentRepository _documentRepository;


        public TaxController(ILogger<HRController> loggers, GTRDBContext context, IDocumentRepository documentRepository, ITaxRepository taxRepository, IClientCompany clientCompany, IClientContractPayment clientContractPayment, IClientTaxInfo clientTaxInfo, IClientTaxPayment clientTaxPayment, IClientTaxCosting clientTaxCosting, IDesignationRepository designationRepository, IClientContactPayment clientContactPayment, IProcessLockRepository processLockRepository, IDocumentInfo documentInfo,IWebHostEnvironment env, ITaxReportRepository taxReportRepository)
        {
            _logger = loggers;
            _context = context;
            _taxRepository = taxRepository;
            _processLockRepository = processLockRepository;
            _clientCompany = clientCompany;
            _clientContractPayment = clientContractPayment;
            _clientTaxInfo = clientTaxInfo;
            _clientTaxPayment = clientTaxPayment;
            _clientTaxCosting = clientTaxCosting;
            _designationRepository = designationRepository;
            _clientContactPayment = clientContactPayment;
            _documentInfo = documentInfo;
            _env=env;
            _taxReportRepository=taxReportRepository;
            _documentRepository = documentRepository;
            _env =env;
        }


        #region Tax_ClientInfo
        //Tax_ClientInfo List
        public IActionResult ClientInfoList()
        {
            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            var comName = _context.Tax_ClientCompany.Select(x => x.ClientComName).ToList();
            ViewBag.comName = comName;
            var entities = _taxRepository.GetAll();
            return View(entities);
        }
        //Tax_ClientInfo  Create view
        public IActionResult CreateClientInfo()
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            ViewBag.comName = _taxRepository.ComList();
            ViewBag.Designation = _designationRepository.GetDesignationList();
            return View();
        }
        //Tax_ClientInfo  Create Post
        [HttpPost]
        public IActionResult CreateClientInfo(Tax_ClientInfo obj)
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            obj.ComId = comid;
            if (obj.ClientId > 0)
            {
                _taxRepository.Update(obj);
            }
            else
            {
                _taxRepository.Add(obj);
            }
            return RedirectToAction(nameof(ClientInfoList));
        }
        public IActionResult EditClientInfo(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.comName = _taxRepository.ComList();
            if (id == null)
            {
                return NotFound();
            }
            
            ViewBag.Title = "Edit";
            
            ViewBag.Designation = _designationRepository.GetDesignationList();
            var Tax_ClientInfo = _taxRepository.FindById(id);
            if (Tax_ClientInfo == null)
            {
                return NotFound();
            }

            // Find the corresponding SelectListItem and set the Selected property to true
            foreach (var item in ViewBag.comName)
            {
                if (item.Value == Tax_ClientInfo.ClientCompId.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

            return View("CreateClientInfo", Tax_ClientInfo);

        }
        public IActionResult DeleteClientInfo(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.comName = _taxRepository.ComList();
            if (id == null)
            {
                return NotFound();
            }
            
            ViewBag.Title = "Delete";
            
            ViewBag.Designation = _designationRepository.GetDesignationList();

            var Tax_ClientInfo = _taxRepository.FindById(id);
            if (Tax_ClientInfo == null)
            {
                return NotFound();
            }
            return View("CreateClientInfo", Tax_ClientInfo);
        }
        [HttpPost, ActionName("DeleteClientInfo")]
        public IActionResult DeleteClientInfoConfirmed(int id)
        {
            try
            {
                var Tax_ClientCompany = _taxRepository.FindById(id);

                _taxRepository.Delete(Tax_ClientCompany);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = Tax_ClientCompany.ClientId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion


        #region Tax_ClientCompany
        public IActionResult ClientCompanyList()
        {
            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            var comName = _context.Tax_ClientCompany.Where(x => x.IsDelete == false && x.ComId == comid).Select(x => x.ClientComName).ToList();
            ViewBag.comName = comName;
            var entities = _clientCompany.GetAll();
            return View(entities);
        }
        public IActionResult CreateClientCompany()
        {
            ViewBag.Title = "Create";
            ViewBag.comName = _taxRepository.ComList();
            return View();
        }
        [HttpPost]
        public IActionResult CreateClientCompany(Tax_ClientCompany obj)
        {

            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            obj.ComId = comid;

            if (obj.ClientComId > 0)
            {
                _clientCompany.Update(obj);
            }
            else
            {
                _clientCompany.Add(obj);
            }

            return RedirectToAction(nameof(ClientCompanyList));
        }


        public IActionResult EditClientCompany(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            ViewBag.comName = _taxRepository.ComList();
            var Tax_ClientCompany = _clientCompany.FindById(id);
            if (Tax_ClientCompany == null)
            {
                return NotFound();
            }
            return View("CreateClientCompany", Tax_ClientCompany);

        }

        public IActionResult DeleteClientCompany(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Delete";
            ViewBag.comName = _taxRepository.ComList();
            var Tax_ClientCompany = _clientCompany.FindById(id);
            if (Tax_ClientCompany == null)
            {
                return NotFound();
            }
            return View("CreateClientCompany", Tax_ClientCompany);
        }
        [HttpPost, ActionName("DeleteClientCompany")]
        public IActionResult DeleteClientCompanyConfirmed(int id)
        {
            try
            {
                var Tax_ClientCompany = _clientCompany.FindById(id);

                _clientCompany.Delete(Tax_ClientCompany);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = Tax_ClientCompany.ClientComId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion

        


        #region Tax_ClientTaxInfo
        public IActionResult ClientTaxInfoList()
        {
            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            //var clients = _context.Tax_ClientInfo.ToList();
            //ViewBag.clients = clients;
            var entities = _context.Tax_ClientTaxInfos.Where(x => x.IsDelete == false && x.ComId == comid).ToList();
            foreach (var data in entities)
            {
                data.ClientCode = _context.Tax_ClientInfo.Where(x => x.ClientId == data.ClientId).Select(x => x.ClientCode).FirstOrDefault();
                data.ClientName = _context.Tax_ClientInfo.Where(x => x.ClientId == data.ClientId).Select(x => x.ClientName).FirstOrDefault();
            }
            return View(entities);
        }
        public IActionResult CreateClientTaxInfo()
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            ViewBag.ClientId = _taxRepository.ClientList();
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
           
            return View();
        }
        [HttpPost]
        public IActionResult CreateClientTaxInfo(Tax_ClientTaxInfo obj)
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            obj.ComId = comid;
            if (obj.ClientTaxInfoId > 0)
            {
                _clientTaxInfo.Update(obj);
            }
            else
            {
                _clientTaxInfo.Add(obj);
            }
            return RedirectToAction(nameof(ClientTaxInfoList));
        }
        public IActionResult EditClientTaxInfo(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            //ViewBag.ClientId = _taxRepository.ClientList();
            if (id == null)
            {
                return NotFound();
            }
            
            ViewBag.Title = "Edit";
            
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            var Tax_ClientInfo = _clientTaxInfo.FindById(id);
            if (Tax_ClientInfo == null)
            {
                return NotFound();
            }

            // Get the list of clients and set the selected value
            var clientList = _taxRepository.ClientList();
            foreach (var item in clientList)
            {
                if (item.Value == Tax_ClientInfo.ClientId.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
            ViewBag.ClientId = clientList;

            return View("CreateClientTaxInfo", Tax_ClientInfo);
        }
        public IActionResult DeleteClientTaxInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Delete";
            ViewBag.ClientId = _taxRepository.ClientList();
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            var Tax_ClientTaxInfo = _clientTaxInfo.FindById(id);
            if (Tax_ClientTaxInfo == null)
            {
                return NotFound();
            }
            return View("CreateClientTaxInfo", Tax_ClientTaxInfo);
        }
        [HttpPost, ActionName("DeleteClientTaxInfo")]
        public IActionResult DeleteClientTaxInfoConfirmed(int id)
        {
            try
            {
                var tax_ClientCompany = _clientTaxInfo.FindById(id);

                _clientTaxInfo.Delete(tax_ClientCompany);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = tax_ClientCompany.ClientTaxInfoId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion

        #region Tax_ClientContactPayment
        public IActionResult ClientContactPayment()
        {
            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            
            var entities = _context.Tax_ClientContactPayment.Where(x=>x.IsDelete==false && x.ComId == comid).ToList();
            foreach(var data in entities)
            {
                data.ClientName = _context.Tax_ClientInfo.Where(x=>x.ClientId == data.ClientId).Select(x=>x.ClientName).FirstOrDefault();
                data.ClientCode = _context.Tax_ClientInfo.Where(x => x.ClientId == data.ClientId).Select(x => x.ClientCode).FirstOrDefault();               
                data.FiscalYearId = _context.Acc_FiscalYears.Where(w => w.FiscalYearId == int.Parse(data.FiscalYearId) && w.IsDelete==false && w.ComId==comid).Select(s => s.FYName).FirstOrDefault();                  
            }
            
            return View(entities);
        }


        public IActionResult CreateContactPayment()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            ViewBag.Title = "Create";
            ViewBag.ClientId = _taxRepository.ClientList();
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            return View();
        }
        [HttpPost]
        public IActionResult CreateContactPayment(Tax_ClientContactPayment obj)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            ViewBag.Title = "Create";
            ViewBag.ClientId = _taxRepository.ClientList();
            obj.ComId = comid;
            if (obj.ClientPaymentId > 0)
            {
                _clientContactPayment.Update(obj);
            }
            else
            {
                _clientContactPayment.Add(obj);
            }
            return RedirectToAction(nameof(ClientContactPayment));

        }
        public IActionResult EditClientContractPayment(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            ViewBag.ClientId = _taxRepository.ClientList();
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            var Tax_ClientInfo = _clientContactPayment.FindById(id);
            if (Tax_ClientInfo == null)
            {
                return NotFound();
            }
            return View("CreateContactPayment", Tax_ClientInfo);
        }
        public IActionResult DeleteClientContractPayment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Delete";
            ViewBag.ClientId = _taxRepository.ClientList();
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            var Tax_ClientTaxInfo = _clientContactPayment.FindById(id);
            if (Tax_ClientTaxInfo == null)
            {
                return NotFound();
            }
            return View("CreateContactPayment", Tax_ClientTaxInfo);
        }




        [HttpPost, ActionName("DeleteClientContractPayment")]
        public IActionResult DeleteClientContractPaymentConfirmed(int id)
        {
            try
            {
                var tax_ClientCompany = _clientContactPayment.FindById(id);

                _clientContactPayment.Delete(tax_ClientCompany);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = tax_ClientCompany.ClientPaymentId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion

        #region Tax_DocumentInfo
        public IActionResult DocumentList()
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _context.Tax_DocumentInfos.Where(x => x.IsDelete == false && x.ComId == comid).Include(x=>x.Tax_ClientInfo).ToList();
            char[] spearator = { '!' };

            List<Tax_DocumentInfo> person = new List<Tax_DocumentInfo>();
            foreach (var a in data)
            {
                a.ClientName = _context.Tax_ClientInfo.Where(x => x.ClientId == a.ClientId).Select(x => x.ClientName).FirstOrDefault();
                Tax_DocumentInfo index = new Tax_DocumentInfo();
                if (a.FileName == null)
                {
                    a.FileName = "";
                }
                String[] strlist = a.FileName.Split(spearator);
                strlist = strlist.SkipLast(1).ToArray();
                List<string> ls = new();

                foreach (var nam in strlist)
                {
                    string path = Path.Combine(this._env.WebRootPath, "TaxDocument/") + nam;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exsit or not  
                    {
                        ls.Add(nam);
                    }

                }
                index.DocumentInfoId = a.DocumentInfoId;
                index.FiscalYearId = a.FiscalYearId;
                index.VarType = a.VarType;
                index.Title = a.Title;
                index.ClientId = a.ClientId;
                index.ClientName = a.ClientName;
                index.imageName = ls;
                person.Add(index);
            }

            ViewBag.filename = person;
            return View(person);
        }
        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(this._env.WebRootPath, "TaxDocument/") + filename;
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public IActionResult btnDelete_Click(string fileName)
        {
            string path = Path.Combine(this._env.WebRootPath, "TaxDocument/") + fileName;
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }
            return RedirectToAction("DocumentList");
        }
        public IActionResult CreateDocument()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.VarType = _documentRepository.CatVariableList();
            ViewBag.Title = "Create";
            ViewBag.ClientId = _taxRepository.ClientList();
           
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> CreateDocument(Tax_DocumentInfo model)
        {

            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            string wwwPath = this._env.WebRootPath;
            string contentPath = this._env.ContentRootPath;
            string path = Path.Combine(this._env.WebRootPath, "TaxDocument");
            model.ComId = comid;
            if (model == null ||
                model.FileToUpload == null)
                return Content("File not selected");

            

            //var path = _env.WebRootPath + "\\EmpDocument\\";
            Tax_DocumentInfo data = new Tax_DocumentInfo
            {
                ClientId = model.ClientId,
                FiscalYearId=model.FiscalYearId,
                VarType = model.VarType,
                Title = model.Title,
                FileName = model.FileName,
                ComId = model.ComId

            };
            data.FileName = "";
            
            foreach (var item in model.FileToUpload)
            {
                string FileNameUrl = UploadFile(item, model.ClientId.ToString());
                data.FileName += FileNameUrl + "!";
                
            };

            // Get the existing document, if it exists
            var exist = _context.Tax_DocumentInfos
                .Where(x => x.DocumentInfoId == model.DocumentInfoId)
                .FirstOrDefault();

            if (exist == null) 
            {
                _context.Add(data);
            }
            else 
            {
                
                exist.ClientId = model.ClientId;
                exist.FiscalYearId = model.FiscalYearId;
                exist.VarType = model.VarType;
                exist.Title = model.Title;

                
                if (model.FileToUpload != null && model.FileToUpload.Count > 0)
                {
                    string[] fileNames = exist.FileName.Split('!'); 
                    foreach (var item in model.FileToUpload)
                    {
                        string fileNameUrl = UploadFile(item, model.Title);
                        exist.FileName += fileNameUrl + "!";
                        
                    }
                }

                _context.Update(exist);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Data Save Successfully";
            TempData["Status"] = "1";
            return RedirectToAction("DocumentList");
        }

        
        public async Task<IActionResult> EditDocument(int? id)
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.VarType = _documentRepository.CatVariableList();

            ViewBag.Title = "Edit";
            ViewBag.ClientId = _taxRepository.ClientList();
            
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }

            var item = await _context.Tax_DocumentInfos.FindAsync(id);          
            var temp = new Tax_DocumentInfo()
            {

                ClientId = item.ClientId,
                FiscalYearId = item.FiscalYearId,
                Title = item.Title,
                VarType = item.VarType,
                FileName = item.FileName,

            };

            if (item == null)
            {
                return NotFound();
            }

            return View("CreateDocument", item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> EditDocument(Tax_DocumentInfo model)
        {

            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            if (model == null ||
                model.FileToUpload == null)
                return Content("File not selected");

            TempData["Message"] = "Data Save Successfully";
            TempData["Status"] = "1";
            return RedirectToAction("DocumentList");
        }


        public IActionResult DeleteDocument(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.VarType = _documentRepository.CatVariableList();
            if (id == null)
            {
                return NotFound();
            }
            
            ViewBag.Title = "Delete";
            ViewBag.ClientId = _taxRepository.ClientList();
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            var file = _context.Tax_DocumentInfos.Where(u => u.DocumentInfoId == id).Select(u => u.FileName);
            ViewBag.file = file;
            var Tax_ClientTaxInfo = _documentInfo.FindById(id);
            if (Tax_ClientTaxInfo == null)
            {
                return NotFound();
            }
            return View("CreateDocument", Tax_ClientTaxInfo);
        }




        [HttpPost, ActionName("DeleteDocument")]
        public IActionResult DeleteDocumentPost(int id)
        {
            try
            {
                var tax_Document = _context.Tax_DocumentInfos.Find(id);
                _context.Tax_DocumentInfos.Remove(tax_Document);
                _context.SaveChanges();
                //_documentInfo.Delete(tax_Document);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = tax_Document.DocumentInfoId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }



        private string UploadFile(IFormFile file, string title)
        {
            string FileName = null;
            if (file != null)
            {
                //string folder = "book/Gallery/";
                string serverFolder = _env.WebRootPath + "\\TaxDocument\\";
                FileName = title.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(serverFolder, FileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

            }
            return FileName;
        }


        #endregion



        #region Tax_PaymentReceived
        public IActionResult TaxPaymentList()
        {

            var comid = HttpContext.Session.GetString("comid");
            var data = _context.Tax_PaymentReceiveds.Include(x => x.Tax_ClientInfo).Where(x => x.IsDelete == false && x.ComId == comid).ToList();

            return View(data);
        }

        public IActionResult CreateTaxPayment()
        {
            ViewBag.ClientId = _taxRepository.ClientList();

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Create";

            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> CreateTaxPayment(Tax_PaymentReceived Tax_PaymentReceived)
        {
            ViewBag.ClientId = _taxRepository.ClientList();
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            Tax_PaymentReceived.ComId = comid;
            if (ModelState.IsValid)
            {
                _context.Add(Tax_PaymentReceived);
                await _context.SaveChangesAsync();
                return RedirectToAction("TaxPaymentList", "Tax");
            }
            return View(Tax_PaymentReceived);
        }

        public IActionResult EditTaxPayment(int? id)
        {
            ViewBag.ClientId = _taxRepository.ClientList();
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Tax_PaymentReceived = _context.Tax_PaymentReceiveds.Find(id);
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            if (Tax_PaymentReceived == null)
            {
                return NotFound();
            }


            return View(Tax_PaymentReceived);
        }

        [HttpPost]
        public async Task<IActionResult> EditTaxPayment(Tax_PaymentReceived Tax_PaymentReceived)
        {
            if (ModelState.IsValid)
            {
                _context.Update(Tax_PaymentReceived);
                await _context.SaveChangesAsync();
                return RedirectToAction("TaxPaymentList", "Tax");
            }
            return View(Tax_PaymentReceived);
        }
        [HttpGet]
        public IActionResult DeleteTaxPayment(int? id)
        {
            ViewBag.ClientId = _taxRepository.ClientList();
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            
            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();

            }
            var Tax_PaymentReceived = _clientTaxPayment.FindById(id);
            if (Tax_PaymentReceived == null)
            {
                return NotFound();
            }


            return View(Tax_PaymentReceived);

        }
        [HttpPost, ActionName("DeleteTaxPayment")]
        public IActionResult DeleteTaxPaymentPost(int id)
        {
            try
            {
                var tax_ClientPayment = _clientTaxPayment.FindById(id);
                _clientTaxPayment.Delete(tax_ClientPayment);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = tax_ClientPayment.PaymentReceiveId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion


        #region Tax_Costings
        public IActionResult TaxCostingList()
        {

            var comid = HttpContext.Session.GetString("comid");
            var data = _context.Tax_Costings.Where(x => x.IsDelete == false && x.ComId == comid).ToList();

            return View(data);
        }

        public IActionResult CreateTaxCosting()
        {
            ViewBag.ClientId = _taxRepository.ClientList();

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Create";

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> CreateTaxCosting(Tax_Costing Tax_Costing)
        {
            ViewBag.ClientId = _taxRepository.ClientList();
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            Tax_Costing.ComId = comid;
            //if (ModelState.IsValid)
            //{
            _context.Add(Tax_Costing);
            await _context.SaveChangesAsync();
            return RedirectToAction("TaxCostingList", "Tax");
            //}
            return View(Tax_Costing);
        }

        public IActionResult EditTaxCosting(int? id)
        {
            ViewBag.ClientId = _taxRepository.ClientList();
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Tax_Costing = _context.Tax_Costings.Find(id);
            if (Tax_Costing == null)
            {
                return NotFound();
            }


            return View(Tax_Costing);
        }

        [HttpPost]
        public async Task<IActionResult> EditTaxCosting(Tax_Costing Tax_Costing)
        {
            //if (ModelState.IsValid)
            //{
            _context.Update(Tax_Costing);
            await _context.SaveChangesAsync();
            return RedirectToAction("TaxCostingList", "Tax");
            //}
            return View(Tax_Costing);
        }
        [HttpGet]
        public IActionResult DeleteTaxCosting(int? id)
        {
            ViewBag.ClientId = _taxRepository.ClientList();
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            var Tax_Costing = _clientTaxCosting.FindById(id);
            if (Tax_Costing == null)
            {
                return NotFound();
            }


            return View(Tax_Costing);

        }
        [HttpPost, ActionName("DeleteTaxCosting")]
        public IActionResult DeleteTaxCostingPost(int id)
        {
            try
            {
                var tax_ClientCosting = _clientTaxCosting.FindById(id);
                _clientTaxCosting.Delete(tax_ClientCosting);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = tax_ClientCosting.CostingID, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }



        #endregion
        public async Task<IActionResult> TaxReport()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var companyRole = HttpContext.Session.GetString("companyRole");

            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;

            ViewBag.DesignationList = _context.Cat_Designation.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.ClientList = _context.Tax_ClientInfo.Where(x => x.ComId == comid && !x.IsDelete).OrderBy(o => o.ClientCode).ToList();

            ViewBag.FiscalYearList = _context.Acc_FiscalYears.Where(x => x.ComId == comid && !x.IsDelete).ToList();

            ViewBag.ReportList = _context.HR_ReportType.Where(x => x.ReportType == "Tax Report").OrderBy(x => x.SLNo).ToList();
            var AppKey = HttpContext.Session.GetString("appkey");
            LoginResponse res = HttpContext.Session.GetObject<LoginResponse>("ResponseData");

            int softwareId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.SoftwareId).FirstOrDefault();
            int versionId = res.Products.Where(a => a.AppKey == AppKey).Select(a => a.VersionId).FirstOrDefault();
            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            sqlParameter[1] = new SqlParameter("@UserId", userid);
            sqlParameter[2] = new SqlParameter("@UserRole", companyRole);
            sqlParameter[3] = new SqlParameter("@SoftwareId", softwareId);
            sqlParameter[4] = new SqlParameter("@VersionId", versionId);
            sqlParameter[5] = new SqlParameter("@ReportType", "Tax Report");

            string query = $"Exec PrcGetShowReport '{comid}','{userid}','{companyRole}'," +
                            $"'{softwareId}', '{versionId}', 'Salary Report'";

            var permission = Helper.ExecProcMapTList<VersionReport>("PrcGetShowReport", sqlParameter);

            if (permission.Count > 0)
            {
                var reports = new List<HR_ReportType>();
                foreach (var item in permission)
                {
                    var report = _context.HR_ReportType.Where(r => r.ReportType == "Tax Report" && r.ReportId == item.ReportId).FirstOrDefault();
                    if (report != null)
                    {
                        reports.Add(report);
                    }

                }
                ViewBag.ReportList = reports;
            }
            else
            {
                List<HR_ReportType> Salaryreport = _context.HR_ReportType.Where(a => a.ReportType == "Tax Report").OrderBy(a => a.SLNo).ToList();
                ViewBag.ReportList = Salaryreport;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaxReport(TaxDto taxDto)
        {
            string callBackUrl = _taxReportRepository.ClientInfo(taxDto);
            return Redirect(callBackUrl);
        }

    }
}


