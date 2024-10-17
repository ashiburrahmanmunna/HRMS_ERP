using DataTablesParser;
using GTERP.BLL;
using GTERP.Interfaces.ControllerFolder;
using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using ZXing;
using ZXing.QrCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.ViewModels.ControllerFolderVM;
using Microsoft.AspNetCore.Http.Extensions;
using GTERP.Models.ViewModels;

namespace GTERP.Controllers.ControllerFolder
{
    public class ControllerFolderController : Controller
    {
        private readonly GTRDBContext _context;
        private readonly TransactionLogRepository _tranlog;
        private readonly ILogger<ControllerFolderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly PermissionLevel _PL;
        private readonly IAddToCartRepository _addToCartRepository;
        private readonly IAssetsRepository _assetsRepository;
        private readonly ICompanyDetailsRepository _companyDetailsRepository;
        private readonly ICurrenciesRepository _currenciesRepository;
        private readonly ICompaniesRepository _companiesRepository;
        private readonly ICompanyPermissionsRepository _companyPermissionsRepository;
        private readonly IModuleGroupsRepository _moduleGroupsRepository;
        private readonly IModuleMenusRepository _moduleMenusRepository;
        private readonly IModulesRepository _modulesRepository;
        private readonly IUnitMastersRepository _unitMastersRepository;
        private readonly IReportPermissionsRepository _reportPermissionsRepository;
        private readonly ISystemAdminRepository _systemAdminRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IEmpReleaseRepository _empReleaseRepository;
        private readonly IReportTypesRepository _reportTypesRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WebHelper _webHelper { get; }

        public ControllerFolderController(
            GTRDBContext context,
            TransactionLogRepository tranlog,
            ILogger<ControllerFolderController> logger,
            IConfiguration configuration,
            PermissionLevel pl,
            IAddToCartRepository addToCartRepository,
            IAssetsRepository assetsRepository,
            ICompanyDetailsRepository companyDetailsRepository,
            ICurrenciesRepository currenciesRepository,
            ICompaniesRepository companiesRepository,
            ICompanyPermissionsRepository companyPermissionsRepository,
            IModuleGroupsRepository moduleGroupsRepository,
            IModuleMenusRepository moduleMenusRepository,
            IModulesRepository modulesRepository,
            IUnitMastersRepository unitMastersRepository,
            IReportPermissionsRepository reportPermissionsRepository,
            ISystemAdminRepository systemAdminRepository,
            IUserPermissionRepository userPermissionRepository,
            IEmpReleaseRepository empReleaseRepository,
            IReportTypesRepository reportTypesRepository,
            IWebHostEnvironment webHostEnvironment,
            WebHelper webHelper
            )
        {
            _context = context;
            _tranlog = tranlog;
            _logger = logger;
            _configuration = configuration;
            _PL = pl;
            _addToCartRepository = addToCartRepository;
            _assetsRepository = assetsRepository;
            _companyDetailsRepository = companyDetailsRepository;
            _currenciesRepository = currenciesRepository;
            _companiesRepository = companiesRepository;
            _companyPermissionsRepository = companyPermissionsRepository;
            _moduleGroupsRepository = moduleGroupsRepository;
            _moduleMenusRepository = moduleMenusRepository;
            _modulesRepository = modulesRepository;
            _unitMastersRepository = unitMastersRepository;
            _reportPermissionsRepository = reportPermissionsRepository;
            _systemAdminRepository = systemAdminRepository;
            _userPermissionRepository = userPermissionRepository;
            _empReleaseRepository = empReleaseRepository;
            _reportTypesRepository = reportTypesRepository;
            _webHelper = webHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Add To Cart
        [HttpPost]
        public JsonResult Add(CartOrderMain cartordermain)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                {
                    _addToCartRepository.AddToCartList(cartordermain);
                    return Json(new { Success = 1, SalesID = cartordermain.CartOrderId, ex = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        public ActionResult MyOrderList()
        {
            var myorderabc = _addToCartRepository.MyOrder();
            return View(myorderabc);
        }

        public ActionResult Remove(CartOrderDetails mob)
        {
            _addToCartRepository.RemoveToCartList(mob);
            return RedirectToAction("MyOrderList", "ControllerFolder");
        }
        #endregion

        #region Assets
        public async Task<IActionResult> AssetsList()
        {
            var gTRDBContext = _context.Asset
                .Include(a => a.Location);
            return View(await gTRDBContext.ToListAsync());
        }

        public async Task<IActionResult> AssetsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Asset
                .Include(a => a.Location)
                .FirstOrDefaultAsync(m => m.AssetId == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        public IActionResult CreateAssets()
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewData["Items"] = _assetsRepository.Items();
            ViewData["AssetCategoryId"] = _assetsRepository.AssetCategoryId();
            ViewData["ComId"] = _assetsRepository.ComId();
            ViewData["LocationId"] = _assetsRepository.LocationId();
            ViewData["Custodian"] = _assetsRepository.Custodian();
            ViewData["CategoryId"] = _assetsRepository.CategoryId();
            ViewData["PurchaseType"] = _assetsRepository.PurchaseType();
            ViewData["Supplier"] = _assetsRepository.Supplier();
            ViewData["DepreciationMethod"] = _assetsRepository.DepreciationMethod();
            ViewData["Department"] = _assetsRepository.Department();
            ViewData["AssetName"] = _assetsRepository.AssetName();
            ViewData["FoDId"] = _assetsRepository.FoDId();
            ViewData["Employees"] = _assetsRepository.Employees();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAssets([Bind("AssetId,AssetName,AssetCategoryId,ComId,LocationId,PurchaseDate")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                _assetsRepository.Add(asset);
                return RedirectToAction(nameof(AssetsList));
            }
            ViewData["AssetCategoryId"] = _assetsRepository.AssetCategoryId();
            ViewData["ComId"] = _assetsRepository.ComId();
            ViewData["LocationId"] = _assetsRepository.LocationId();
            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DepreiationMethod(DepreciationMethod input)
        {
            if (input.DMId > 0)
            {
                _context.Entry(input).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(input).State = EntityState.Added;

            }
            _context.SaveChanges();
            return Json(new { success = 1 });
        }

        public IActionResult EditAssets(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var asset = _assetsRepository.FindById(id);
            if (asset == null)
            {
                return NotFound();
            }
            ViewData["AssetCategoryId"] = _assetsRepository.AssetCategoryId();
            ViewData["ComId"] = _assetsRepository.ComId();
            ViewData["LocationId"] = _assetsRepository.LocationId();
            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAssets(int id, [Bind("AssetId,AssetName,AssetCategoryId,ComId,LocationId,PurchaseDate")] Asset asset)
        {
            if (id != asset.AssetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _assetsRepository.Update(asset);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(asset.AssetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AssetsList));
            }
            ViewData["AssetCategoryId"] = _assetsRepository.AssetCategoryId();
            ViewData["ComId"] = _assetsRepository.ComId();
            ViewData["LocationId"] = _assetsRepository.LocationId();
            return View(asset);
        }

        private bool AssetExists(int id)
        {
            return _context.Asset.Any(e => e.AssetId == id);
        }

        public IActionResult DeleteAssets(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = _context.Asset
                .Include(a => a.Location)
                .FirstOrDefaultAsync(m => m.AssetId == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        [HttpPost, ActionName("DeleteAssets")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAssetsConfirmed(int id)
        {
            var asset = _assetsRepository.FindById(id);
            _assetsRepository.Delete(asset);
            return RedirectToAction(nameof(AssetsList));
        }
        #endregion

        #region Company Details
        public ActionResult CompanyDetailsList()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            string username = HttpContext.Session.GetString("username");

            if (username != null)
            {
                if (username.Contains("gtrbd"))
                {
                    return View(_context.CompanyDetails.Where(x => !x.IsDelete).ToList());
                }
                else
                {
                    return View(_context.CompanyDetails.Where(x => x.ComId == comid && !x.IsDelete).ToList());
                }
            }
            return View("CompanyDetailsList");
        }

        public ActionResult CreateCompanyDetails()
        {
            ViewBag.Title = "Create";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                if (username.Contains("gtrbd"))
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode();
                }
                else
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode1();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateCompanyDetails(CompanyDetails collection)
        {
            ViewBag.Title = "Create";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                if (username.Contains("gtrbd"))
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode();
                }
                else
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode1();
                }
            }
            collection.ComId = comid;
            collection.CompanyName = _context.Companys.Where(r => r.ComId == collection.CompanyCode).Select(x => x.CompanyName).FirstOrDefault();
            if (collection.ComDetailsId > 0)
            {
                _companyDetailsRepository.Update(collection);
                ViewBag.Success = true;

                return RedirectToAction("CompanyDetailsList");
            }
            else
            {
                ViewBag.Success = true;
                _companyDetailsRepository.Add(collection);

                return RedirectToAction("CompanyDetailsList");
            }
            return View(collection);
        }

        public ActionResult EditCompanyDetails(int id)
        {
            ViewBag.Title = "Edit";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                if (username.Contains("gtrbd"))
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode();
                }
                else
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode1();
                }
            }
            var data = _companyDetailsRepository.FindById(id);

            return View("CreateCompanyDetails", data);
        }

        public async Task<IActionResult> DeleteCompanyDetails(int? id)
        {
            ViewBag.Title = "Delete";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                if (username.Contains("gtrbd"))
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode();
                }
                else
                {
                    ViewBag.CompanyCode = _companyDetailsRepository.CompanyCode1();
                }
            }
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.CompanyDetails.FirstOrDefaultAsync(x => x.ComDetailsId == id);
            ViewBag.Title = "Delete";
            return View("CreateCompanyDetails", data);
        }


        [HttpPost]
        public IActionResult DeleteCompanyDetails(int id)
        {
            try
            {
                var data = _companyDetailsRepository.FindById(id);
                _companyDetailsRepository.Delete(data);

                ViewBag.Message = "Data Delete Successfully";
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ComDetailsId = data.ComDetailsId });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        public IActionResult CompanyQr()
        {


            var userId = HttpContext.Session.GetString("userid");
            var comid = HttpContext.Session.GetString("comid");
            var fullName = HttpContext.Session.GetString("fullname");
            var CompanyName = _context.Companys.Where(r => r.CompanyCode == comid).Select(x => new { x.ComLogo, x.CompanyName }).FirstOrDefault();

            ViewBag.CompanyName = CompanyName.CompanyName;

            string base64logo = Convert.ToBase64String(CompanyName.ComLogo);
            ViewBag.Companylogo = base64logo;
            try
            {



                var QRCodeText = comid;
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = 200,
                        Height = 200
                    }
                };
                var result = writer.Write(QRCodeText);
                var bitmap = new Bitmap(result);
                var stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                var base64String = Convert.ToBase64String(stream.ToArray());


                string path = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode/" + comid + ".png");
                bitmap.Save(filePath, ImageFormat.Png);
                //byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
                //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                //string fileName = Path.GetFileName(filePath);
                //string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
                //ViewBag.QrCodeUri = imageUrl;

                ViewBag.barcode = base64String;



                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return View();

        }

        #endregion

        #region Currencies
        public ActionResult CurrenciesList()
        {
            var data = _currenciesRepository.All().Where(x => !x.IsDelete);
            return View(data);
        }

        public ActionResult CreateCurrencies()
        {
            ViewBag.Title = "Create";
            return View();
        }
        [HttpPost]
        public ActionResult CreateCurrencies([Bind(/*Include =*/ "CurrencyId,CurCode,Rate,EffectDate")] Currency Currency)
        {
            try
            {
                if (Currency.CurrencyId > 0)
                {
                    Currency.DateAdded = DateTime.Now;
                    _currenciesRepository.Update(Currency);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                }
                else
                {
                    Currency.DateAdded = DateTime.Now;
                    _currenciesRepository.Add(Currency);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                }
                return RedirectToAction("CurrenciesList");

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                TempData["Status"] = "0";
                throw ex;
            }
        }

        public ActionResult EditCurrencies(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            Currency Currency = _currenciesRepository.FindById(id);
            if (Currency == null)
            {
                return NotFound();
            }
            return View("CreateCurrencies", Currency);
        }

        public ActionResult DeleteCurrencies(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            Currency Currency = _currenciesRepository.FindById(id);
            if (Currency == null)
            {
                return NotFound();
            }
            return View("CreateCurrencies", Currency);
        }

        [HttpPost, ActionName("DeleteCurrencies")]
        public JsonResult DeleteCurrenciesConfirmed(int id)
        {
            try
            {
                Currency Currency = _currenciesRepository.FindById(id);
                _currenciesRepository.Delete(Currency);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = Currency.CurrencyId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() }); ;
            }
        }
        #endregion

        #region Companies
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        // GET: Companies
        public ActionResult CompaniesList()
        {
            var company = _companiesRepository.GetCompanyList();
            return View(company);
        }

        public ActionResult CompaniesDetails(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Models.Company company = _companiesRepository.FindById(id);
            if (company == null)
            {
                return NotFound();
            }

            ViewBag.BusinessTypeId = _companiesRepository.BusinessTypeId();
            ViewBag.BaseComId = _companiesRepository.BaseComId();
            ViewBag.CountryId = _companiesRepository.CountryId();

            ViewBag.Title = "Details";
            return View("CreateCompanies", company);
        }

        // GET: Companies/Create
        public ActionResult CreateCompanies()
        {
            ViewBag.VoucherNoCreatedTypeId = _companiesRepository.VoucherNoCreatedTypeId();
            ViewBag.BusinessTypeId = _companiesRepository.BusinessTypeId();
            ViewBag.BaseComId = _companiesRepository.BaseComId();
            ViewBag.CountryId = _companiesRepository.CountryId();

            ViewBag.Title = "Create";
            return View();
        }
        [HttpPost]

        public ActionResult CreateCompanies(GTERP.Models.Company company, IFormFile fileImageHeader, IFormFile fileLogo, IFormFile comSign)
        {
            var comScode = company.CompanySecretCode;
            var comCode = company.CompanyCode;
            var appkey = company.AppKey;
            var errors = ModelState.Where(x => x.Value.Errors.Any())
                  .Select(x => new { x.Key, x.Value.Errors });

            if (ModelState.IsValid)
            {
                if (company.ComId > 0)
                {
                    if (fileImageHeader != null && fileImageHeader.Length > 0)
                    {
                        fileName = Path.GetFileName(fileImageHeader.FileName);
                        Extension = Path.GetExtension(fileName);
                        var uploadlocation = "Content/img/Companies/comimageheader/";
                        company.HeaderImagePath = uploadlocation;
                        company.HeaderFileExtension = Extension;

                        _FileName = company.ComId + Extension;
                        _path = uploadlocation + _FileName;// Path.Combine(Server.MapPath("~/Content/img/Companies/comimageheader/"), _FileName);
                        byte[] fileData = null;
                        using (BinaryReader binaryreader = new BinaryReader(fileImageHeader.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)fileImageHeader.Length);
                        }

                        Image cropimage = HandleImageUpload(fileData, "wwwroot/" + _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageData = ms.ToArray();

                        company.ComImageHeader = imageData;

                    }

                    if (fileLogo != null && fileLogo.Length > 0)
                    {
                        fileName = Path.GetFileName(fileLogo.FileName);
                        Extension = Path.GetExtension(fileName);
                        var uploadlocation = "Content/img/Companies/comlogo/";
                        company.LogoImagePath = uploadlocation;
                        company.LogoFileExtension = Extension;

                        _FileName = company.ComId + Extension;

                        string _path = uploadlocation + _FileName;
                        byte[] fileData = null;

                        using (BinaryReader binaryreader = new BinaryReader(fileLogo.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)fileLogo.Length);
                        }
                        Image cropimage = HandleImageUpload(fileData, "wwwroot/" + _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, ImageFormat.Png);
                        byte[] imageData = ms.ToArray();

                        company.ComLogo = imageData;
                    }

                    if (comSign != null && comSign.Length > 0)
                    {
                        fileName = Path.GetFileName(comSign.FileName);
                        Extension = Path.GetExtension(fileName);
                        var uploadlocation = "Content/img/Companies/cSign/";
                        company.SignImagePath = uploadlocation;
                        company.SignFileExtension = Extension;

                        _FileName = company.ComId + Extension;

                        string _path = uploadlocation + _FileName;

                        byte[] fileData = null;
                        using (BinaryReader binaryreader = new BinaryReader(comSign.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)comSign.Length);
                        }

                        Image cropimage = HandleImageUpload(fileData, "wwwroot/" + _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, ImageFormat.Png);
                        byte[] imageData = ms.ToArray();
                        company.ComSign = imageData;

                    }
                    _companiesRepository.Update(company);
                    return RedirectToAction("CompaniesList");
                }

                else
                {
                    _companiesRepository.Add(company);

                    _context.Entry(company).GetDatabaseValues();
                    string id = company.ComId.ToString(); // Yes it's here

                    if (fileImageHeader != null && fileImageHeader.Length > 0)
                    {
                        var uploadlocation = "Content/img/Companies/comimageheader/";
                        fileName = Path.GetFileName(fileImageHeader.FileName);
                        Extension = Path.GetExtension(fileName);

                        company.HeaderImagePath = uploadlocation;
                        company.HeaderFileExtension = Extension;

                        _FileName = id.ToString() + Extension;
                        _path = uploadlocation;// Path.Combine(Server.MapPath("~/Content/img/Companies/comimageheader/"), _FileName);
                        byte[] fileData = null;

                        using (BinaryReader binaryreader = new BinaryReader(fileImageHeader.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)fileImageHeader.Length);
                        }

                        Image cropimage = HandleImageUpload(fileData, _path);
                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageData = ms.ToArray();

                        company.ComImageHeader = imageData;
                        string imageUrls = "/Content/img/companies/comimageheader/" + _FileName;
                    }
                    if (fileLogo != null && fileLogo.Length > 0)
                    {
                        var uploadlocation = "Content/img/Companies/comlogo/";
                        fileName = Path.GetFileName(fileLogo.FileName);
                        Extension = Path.GetExtension(fileName);

                        company.LogoImagePath = uploadlocation;
                        company.LogoFileExtension = Extension;

                        _FileName = id.ToString() + Extension;
                        _path = uploadlocation;// Path.Combine(Server.MapPath("~/Content/img/Companies/comlogo/"), _FileName);
                        byte[] fileData = null;

                        using (BinaryReader binaryreader = new BinaryReader(fileLogo.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)fileLogo.Length);
                        }
                        Image cropimage = HandleImageUpload(fileData, _path);

                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, cropimage.RawFormat);
                        byte[] imageData = ms.ToArray();

                        company.ComLogo = imageData;
                        string imageUrls = "/Content/img/companies/comlogo/" + _FileName;
                    }

                    if (comSign != null && comSign.Length > 0)
                    {
                        var uploadlocation = "Content/img/Companies/cSign/";
                        fileName = Path.GetFileName(comSign.FileName);
                        Extension = Path.GetExtension(fileName);

                        company.SignImagePath = uploadlocation;
                        company.SignFileExtension = Extension;

                        _FileName = id.ToString() + Extension;
                        _path = uploadlocation;// Path.Combine(Server.MapPath("~/Content/img/Companies/comlogo/"), _FileName);
                        byte[] fileData = null;

                        using (BinaryReader binaryreader = new BinaryReader(comSign.OpenReadStream()))
                        {
                            fileData = binaryreader.ReadBytes((int)comSign.Length);
                        }
                        Image cropimage = HandleImageUpload(fileData, _path);

                        MemoryStream ms = new MemoryStream();
                        cropimage.Save(ms, cropimage.RawFormat);
                        byte[] imageData = ms.ToArray();

                        company.ComSign = imageData;
                        string imageUrls = "/Content/img/Companies/cSign/" + _FileName;
                    }
                    _companiesRepository.Update(company);
                }
                return RedirectToAction("CompaniesList");
            }
            ViewBag.BusinessTypeId = _companiesRepository.BusinessTypeId();
            ViewBag.BaseComId = _companiesRepository.BaseComId();
            ViewBag.CountryId = _companiesRepository.CountryId();

            ViewBag.Title = "Create";
            return View("CreateCompanies", company);
        }


        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        // GET: Companies/Edit/5
        public ActionResult EditCompanies(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Models.Company company = _companiesRepository.FindById(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewBag.comUpdate = "Edit";

            ViewBag.VoucherNoCreatedTypeId = _companiesRepository.VoucherNoCreatedTypeId();
            ViewBag.BusinessTypeId = _companiesRepository.BusinessTypeId();
            ViewBag.BaseComId = _companiesRepository.BaseComId();
            ViewBag.CountryId = _companiesRepository.CountryId();

            ViewBag.Title = "Edit";
            return View("CreateCompanies", company);
        }

        private Image HandleImageUpload(byte[] binaryImage, string path)
        {
            Image img = ResizeImage(Image.FromStream(BytearrayToStream(binaryImage)), 300, 300);
            img.Save(path, ImageFormat.Png);
            return img;
        }

        private Image ResizeImage(Image img, int maxWidth, int maxHeight)
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
        public ActionResult DeleteCompanies(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Models.Company company = _companiesRepository.FindById(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewBag.VoucherNoCreatedTypeId = _companiesRepository.VoucherNoCreatedTypeId();
            ViewBag.BusinessTypeId = _companiesRepository.BusinessTypeId();
            ViewBag.BaseComId = _companiesRepository.BaseComId();
            ViewBag.CountryId = _companiesRepository.CountryId();

            ViewBag.Title = "Delete";
            return View("CreateCompanies", company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("DeleteCompanies")]
        public JsonResult DeleteCompaniesConfirmed(int id)
        {
            try
            {
                Models.Company Company = _companiesRepository.FindById(id);
                _companiesRepository.Delete(Company);

                string fullPath = ("~/" + Company.HeaderImagePath + Company.ComId + Company.HeaderFileExtension);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                string fullPathLogo = ("~/" + Company.LogoImagePath + Company.ComId + Company.LogoFileExtension);
                if (System.IO.File.Exists(fullPathLogo))
                {
                    System.IO.File.Delete(fullPathLogo);
                }

                string fullPathSign = ("~/" + Company.ComSign + Company.ComId + Company.SignFileExtension);
                if (System.IO.File.Exists(fullPathSign))
                {
                    System.IO.File.Delete(fullPathSign);
                }
                return Json(new { Success = 1, CompanyId = Company.ComId, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
        #endregion

        #region Company Permissions
        public ActionResult CompanyPermissionsList()
        {
            var data = _companyPermissionsRepository.GetAll().Where(x => !x.IsDelete);
            return View(data);
        }

        public JsonResult getUserCompany(string UserId)
        {
            List<CompanyUser> abc = _companyPermissionsRepository.CompanyUserList(UserId);

            return Json(abc);
        }

        // GET: Countries/Details/5
        public ActionResult CompanyPermissionsDetails(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = _context.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }



        // GET: Countries/Create
        public ActionResult CreateCompanyPermissions()
        {
            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);

            var data = new CompanyPermissionVM();

            if (res.Companies != null || res.MyUsers != null)
            {
                List<AspnetUserList> aspNetUser = new List<AspnetUserList>();
                foreach (var user in res.MyUsers)
                {
                    var u = new AspnetUserList();
                    u.UserId = user.UserID;
                    u.UserName = user.UserName;
                    aspNetUser.Add(u);
                }
                ViewBag.UserId = new SelectList(aspNetUser, "UserId", "UserName");

                string comid = HttpContext.Session.GetString("comid");
                var comName = _context.Companys.Where(x => x.CompanyCode == comid).Select(x => x.CompanyName).FirstOrDefault();
                var cp = _context.CompanyPermissions.Where(x => x.ComId == comid).Select(x => new { x.ComId, x.UserId, x.CompanyPermissionId, x.isChecked, x.isDefault }).FirstOrDefault();

                data = new CompanyPermissionVM();
                data.UserId = cp.UserId;
                data.CompanyName = comName;
                data.ComId = cp.ComId;
                data.isChecked = cp.isChecked;
                data.isDefault = cp.isDefault;
                data.CompanyPermissionId = cp.CompanyPermissionId;
            }
            else
            {

            }
            var comPermision = new CompanyPermission();

            return View(data);
        }

        public class GetUserModel
        {
            public Guid AppKey { get; set; }
        }

        [HttpPost]
        public IActionResult Save(List<CompanyPermission> CompanyPermission) //List<player> list
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var userid = HttpContext.Session.GetString("userid");

                    // Perform Update
                    foreach (CompanyPermission cp in CompanyPermission)
                    {
                        if (cp.CompanyPermissionId > 0)
                        {
                            if (cp.isChecked == 1)
                            {
                                _companyPermissionsRepository.Update(cp);
                            }
                            else
                            {
                                var x = _context.CompanyPermissions.Where(p => p.CompanyPermissionId == cp.CompanyPermissionId).FirstOrDefault();
                                _companyPermissionsRepository.Delete(x);
                            }
                        }
                        else
                        {
                            if (cp.isChecked == 1)
                            {
                                _companyPermissionsRepository.Add(cp);
                            }
                        }
                    }
                    return Json(new { Success = 1, ex = "" });
                }
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        // GET: Countries/Edit/5
        public ActionResult EditCompanyPermissions(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = _context.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompanyPermissions([Bind(/*Include =*/ "CountryCode,CountryName,CurrencyName,CountryShortName,CultureInfo,CurrencySymbol,CurrencyShortName,flagclass,DialCode")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(country).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("CompanyPermissionsList");
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public ActionResult DeleteCompanyPermissions(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = _context.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("DeleteCompanyPermissions")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCompanyPermissionsConfirmed(int id)
        {
            _companyPermissionsRepository.CompanyPermissionDelete(id);
            return RedirectToAction("CompanyPermissionsList");
        }
        #endregion

        #region Module Groups
        //public Image _currentBitmap;
        //string _FileName = "";
        //string _path = "";
        //string fileName = null;
        //string Extension = null;

        // GET: ModuleGroups
        public ActionResult ModuleGroupsList()
        {
            var data = _moduleGroupsRepository.All()
                .Include(m => m.vModule).Where(x => !x.IsDelete).ToList();
            return View(data);
        }

        // GET: ModuleGroups/Details/5
        public ActionResult ModuleGroupsDetails(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ModuleGroup moduleGroup = _moduleGroupsRepository.FindById(id);
            if (moduleGroup == null)
            {
                return NotFound();
            }
            return View(moduleGroup);
        }

        // GET: ModuleGroups/Create
        public ActionResult CreateModuleGroups()
        {
            ViewBag.Title = "Create";
            ViewBag.ModuleId = _moduleGroupsRepository.ModuleId();
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateModuleGroups(ModuleGroup moduleGroup, /*HttpPostedFileBase file,*/ string imageDatatest)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
           .Select(x => new { x.Key, x.Value.Errors });

            if (ModelState.IsValid)
            {
                if (moduleGroup.ModuleGroupId > 0)
                {
                    _moduleGroupsRepository.Update(moduleGroup);

                }
                else
                {
                    _moduleGroupsRepository.Add(moduleGroup);

                    _context.Entry(moduleGroup).GetDatabaseValues();
                    int id = moduleGroup.ModuleGroupId; // Yes it's here

                }
                return RedirectToAction("ModuleGroupsList");

            }
            ViewBag.ModuleId = _moduleGroupsRepository.ModuleId1(moduleGroup);

            return View(moduleGroup);

        }

        private MemoryStream BytearrayToStream1(byte[] arr)
        {
            return new MemoryStream(arr, 0, arr.Length);
        }

        private Image HandleImageUpload1(byte[] binaryImage, string path)
        {
            Image img = RezizeImage(Image.FromStream(BytearrayToStream1(binaryImage)), 300, 300);
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

        // GET: ModuleGroups/Edit/5
        public ActionResult EditModuleGroups(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ModuleGroup moduleGroup = _moduleGroupsRepository.FindById(id);
            if (moduleGroup == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.ModuleId = _moduleGroupsRepository.ModuleId1(moduleGroup);
            return View("CreateModuleGroups", moduleGroup);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditModuleGroups([Bind(/*Include =*/ "ModuleGroupId,ModuleGroupName,ModuleGroupCaption,ModuleId,ModuleGroupImage,ImagePath,ImageExtension,SLNo")] ModuleGroup moduleGroup)
        {
            if (ModelState.IsValid)
            {
                _moduleGroupsRepository.Update(moduleGroup);
                return RedirectToAction("ModuleGroupsList");
            }
            ViewBag.ModuleId = _moduleGroupsRepository.ModuleId1(moduleGroup);
            return View(moduleGroup);
        }

        // GET: ModuleGroups/Delete/5
        public ActionResult DeleteModuleGroups(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ModuleGroup moduleGroup = _moduleGroupsRepository.FindById(id);
            if (moduleGroup == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            ViewBag.ModuleId = _moduleGroupsRepository.ModuleId1(moduleGroup);
            return View("CreateModuleGroups", moduleGroup);
        }

        // POST: ModuleGroups/Delete/5
        [HttpPost, ActionName("DeleteModuleGroups")]
        public JsonResult DeleteModuleGroupsConfirmed(int? id)
        {
            try
            {
                ModuleGroup moduleGroup = _moduleGroupsRepository.FindById(id);
                _moduleGroupsRepository.Delete(moduleGroup);

                return Json(new { Success = 1, ModuleGroupId = moduleGroup.ModuleGroupId, ex = "" });

            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
        #endregion

        #region Module Menus
        //public Image _currentBitmap;
        //string _FileName = "";
        //string _path = "";
        //string fileName = null;
        //string Extension = null;

        // GET: ModuleGroups
        public ActionResult ModuleMenusList()
        {
            var moduleMenus = _moduleMenusRepository.All()
                .Include(m => m.vModuleGroup)
                .Include(m => m.vModule)
                .Where(x => !x.IsDelete);
            return View(moduleMenus.ToList());
        }

        // GET: ModuleGroups/Details/5
        public ActionResult ModuleMenusDetails(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ModuleGroup moduleMenu = _moduleGroupsRepository.FindById(id);
            if (moduleMenu == null)
            {
                return NotFound();
            }
            return View(moduleMenu);
        }

        // GET: ModuleGroups/Create
        public ActionResult CreateModuleMenus()
        {
            ViewBag.Title = "Create";
            ViewBag.ModuleId = _moduleMenusRepository.ModuleId();
            ViewBag.ModuleGroupId = _moduleMenusRepository.ModuleGroupId();
            ViewBag.ImageCriteriaId = _moduleMenusRepository.ImageCriteriaId();

            //Module Menu Action

            ViewBag.ModuleMenuAction = new Module_Menu_Action();
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateModuleMenus(ModuleMenu moduleMenu, /*HttpPostedFileBase file,*/ string imageDatatest)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
               .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    if (moduleMenu.ModuleMenuId > 0)
                    {
                        _context.Entry(moduleMenu).State = EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToAction("ModuleMenusList");
                    }
                    else
                    {
                        if (moduleMenu.ParentId == -1)
                        {
                            moduleMenu.ParentId = 0;
                        }

                        _moduleMenusRepository.Add(moduleMenu);
                        var error = ModelState.Where(x => x.Value.Errors.Any())
                        .Select(x => new { x.Key, x.Value.Errors });
                        //db.SaveChanges();

                        _context.Entry(moduleMenu).GetDatabaseValues();
                        int id = moduleMenu.ModuleMenuId; // Yes it's here                       
                    }
                    return RedirectToAction("ModuleMenusList");
                }
                ViewBag.ModuleMenuId = _moduleMenusRepository.ModuleMenuId(moduleMenu);

                return View(moduleMenu);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region Module Menu Action

        public ActionResult CreateModuleMenuAction(int id)
        {
            var data = _context.Module_Menu_Action.Where(x => x.ModuleMenuId == id && !x.IsDelete).ToList();
            if (data.Count > 0)
            {
                var editModuleMenuAction = _moduleMenusRepository.FindById(id);
                var ModuleMenuActions = _context.Module_Menu_Action.Include(x => x.ModuleMenu).Where(x => x.ModuleMenuId == id).ToList();
                ViewBag.Title = "Edit";
                ViewBag.EditData = ModuleMenuActions;
                ViewBag.ModuleName = editModuleMenuAction.ModuleMenuCaption;
                ViewBag.ModuleMenuId = id;
            }
            else
            {
                ModuleMenu moduleMenu = _moduleMenusRepository.FindById(id);

                ViewBag.ModuleId = moduleMenu.ModuleMenuCaption;
                ViewBag.ModuleMenuId = id;
                ViewBag.Title = "Create";
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateModuleMenuAction(List<Module_Menu_Action> mmAction)
        {

            foreach (var m in mmAction)
            {
                var existing = _context.Module_Menu_Action.Where(x => x.ModuleMenuId == m.ModuleMenuId).ToList();
                if (existing.Count > 0)
                {
                    var removeData = _context.Module_Menu_Action.Where(x => x.ModuleMenuId == m.ModuleMenuId).ToList();
                    _context.Module_Menu_Action.RemoveRange(removeData);
                    _context.SaveChanges();
                }
            }
            _context.Module_Menu_Action.AddRange(mmAction);
            _context.SaveChanges();
            ViewBag.Title = "Create";

            return Json(new { Success = 1 });
        }

        //public IActionResult DeleteModuleMenuAction(int id)
        //{
        //    var data = _context.Module_Menu_Action.Find(id);
        //    ModuleMenu moduleMenu = _moduleMenusRepository.FindById(id);
        //    ViewBag.ModuleId = moduleMenu.ModuleMenuCaption;
        //    ViewBag.ModuleMenuId = id;

        //    ViewBag.Title = "Delete";
        //    return View("CreateModuleMenuAction", data);
        //}

        //[HttpPost, ActionName("DeleteModuleMenuAction")]
        //public IActionResult DeleteModuleMenuActionConfirmed(int id)
        //{
        //    ViewBag.Title = "Delete";

        //    try
        //    {
        //        var data = _context.Module_Menu_Action.Find(id);
        //        ModuleMenu moduleMenu = _moduleMenusRepository.FindById(id);
        //        ViewBag.ModuleId = moduleMenu.ModuleMenuCaption;
        //        ViewBag.ModuleMenuId = id;

        //        data.IsDelete = true;
        //        _context.Update(data);
        //        _context.SaveChanges();

        //        TempData["Message"] = "Data Delete Successfully";
        //        TempData["Status"] = "3";

        //        return Json(new { Success = 1, DeptId = data.Id, ex = TempData["Message"].ToString() });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }
        //}

        #endregion

        //private MemoryStream BytearrayToStream(byte[] arr)
        //{
        //    return new MemoryStream(arr, 0, arr.Length);
        //}

        //private Image HandleImageUpload(byte[] binaryImage, string path)
        //{
        //    Image img = RezizeImage(Image.FromStream(BytearrayToStream(binaryImage)), 300, 300);
        //    img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return img;
        //}

        //private Image RezizeImage(Image img, int maxWidth, int maxHeight)
        //{
        //    if (img.Height < maxHeight && img.Width < maxWidth) return img;
        //    using (img)
        //    {
        //        Double xRatio = (double)img.Width / maxWidth;
        //        Double yRatio = (double)img.Height / maxHeight;
        //        Double ratio = Math.Max(xRatio, yRatio);
        //        int nnx = (int)Math.Floor(img.Width / ratio);
        //        int nny = (int)Math.Floor(img.Height / ratio);
        //        Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
        //        using (Graphics gr = Graphics.FromImage(cpy))
        //        {
        //            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //            gr.DrawImage(img,
        //                new Rectangle(0, 0, nnx, nny),
        //                new Rectangle(0, 0, img.Width, img.Height),
        //                GraphicsUnit.Pixel);
        //        }
        //        return cpy;
        //    }
        //}

        // GET: ModuleGroups/Edit/5
        public ActionResult EditModuleMenus(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ModuleMenu moduleMenu = _moduleMenusRepository.FindById(id);
            if (moduleMenu == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.ModuleId = _moduleMenusRepository.ModuleId1(moduleMenu);
            ViewBag.ImageCriteriaId = _moduleMenusRepository.ImageCriteriaId1(moduleMenu);
            ViewBag.ModuleGroupId = _moduleMenusRepository.ModuleGroupId1(moduleMenu);
            ViewBag.ParentId = _moduleMenusRepository.ParentId(moduleMenu);
            return View("CreateModuleMenus", moduleMenu);
        }

        [HttpPost]
        public ActionResult EditModuleMenus(ModuleGroup moduleMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(moduleMenu).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("ModuleMenusList");
            }
            ViewBag.ModuleMenuId = new SelectList(_context.ModuleGroups, "ModuleMenuId", "ModuleGroupCaption", moduleMenu.ModuleGroupId);
            return View(moduleMenu);
        }

        // GET: ModuleGroups/Delete/5
        public ActionResult DeleteModuleMenus(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ModuleMenu moduleMenu = _moduleMenusRepository.FindById(id);
            if (moduleMenu == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            ViewBag.ModuleId = _moduleMenusRepository.ModuleId1(moduleMenu);
            ViewBag.ImageCriteriaId = _moduleMenusRepository.ImageCriteriaId1(moduleMenu);
            ViewBag.ModuleGroupId = _moduleMenusRepository.ModuleGroupId1(moduleMenu);
            ViewBag.ParentId = _moduleMenusRepository.ParentId(moduleMenu);
            return View("CreateModuleMenus", moduleMenu);
        }

        // POST: ModuleGroups/Delete/5
        [HttpPost, ActionName("DeleteModuleMenus")]
        public JsonResult DeleteModuleMenusConfirmed(int? id)
        {
            try
            {
                ModuleMenu moduleMenu = _moduleMenusRepository.FindById(id);
                _moduleMenusRepository.Delete(moduleMenu);

                return Json(new { Success = 1, ModuleMenuId = moduleMenu.ModuleMenuId, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        public JsonResult GetModuleGroup(int id)
        {
            List<ModuleGroup> ModuleGroupList = _context.ModuleGroups.Where(m => m.ModuleId == id).ToList();

            List<SelectListItem> ListOfModuleGroup = new List<SelectListItem>();
            if (ModuleGroupList != null)
            {
                foreach (ModuleGroup x in ModuleGroupList)
                {
                    ListOfModuleGroup.Add(new SelectListItem { Text = x.ModuleGroupName, Value = x.ModuleGroupId.ToString() });
                }
            }
            return Json(new SelectList(ListOfModuleGroup, "Value", "Text"));
        }

        public JsonResult GetModuleMenu(int id)
        {
            List<ModuleMenu> ParentMenuList = _context.ModuleMenus.Where(m => m.ModuleGroupId == id && m.isParent == 1).ToList();

            List<SelectListItem> listparentmenu = new List<SelectListItem>();
            if (ParentMenuList != null)
            {
                foreach (ModuleMenu x in ParentMenuList)
                {
                    listparentmenu.Add(new SelectListItem { Text = x.ModuleMenuCaption, Value = x.ModuleMenuId.ToString() });
                }
            }
            return Json(new SelectList(listparentmenu, "Value", "Text"));
        }
        #endregion

        #region Modules
        //private Image _currentBitmap;
        //string _FileName = "";
        //string _path = "";
        //string fileName = null;
        //string Extension = null;

        // GET: Modules
        public ActionResult ModulesList()
        {
            var data = _modulesRepository.All().Where(x => !x.IsDelete).ToList();
            return View(data);
        }

        // GET: Modules/Details/5
        public ActionResult ModulesDetails(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Module module = _modulesRepository.FindById(id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        public ActionResult CreateModules()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public ActionResult CreateModules([Bind(/*Include =*/ "ModuleId,ModuleCode,ModuleName,ModuleCaption,ModuleDescription,ModuleLink,ModuleImage,ModuleImagePath,ModuleImageExtension,isInactive,SLNo")] Module module,/* HttpPostedFileBase file,*/ string imageDatatest)
        {
            module.isInactive = 0;
            var errors = ModelState.Where(x => x.Value.Errors.Any())
          .Select(x => new { x.Key, x.Value.Errors });

            if (ModelState.IsValid)
            {
                if (module.ModuleId > 0)
                {
                    _modulesRepository.Update(module);
                }
                else
                {
                    _modulesRepository.Add(module);

                    _context.Entry(module).GetDatabaseValues();
                    int id = module.ModuleId; // Yes it's here

                    return RedirectToAction("ModulesList");
                }
            }
            return RedirectToAction("ModulesList");
        }
        //private Image HandleImageUpload(byte[] binaryImage, string path)
        //{
        //    Image img = RezizeImage(Image.FromStream(BytearrayToStream(binaryImage)), 300, 300);
        //    img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return img;
        //}
        //private static Image RezizeImage(Image img, int maxWidth, int maxHeight)
        //{
        //    if (img.Height < maxHeight && img.Width < maxWidth) return img;
        //    using (img)
        //    {
        //        Double xRatio = (double)img.Width / maxWidth;
        //        Double yRatio = (double)img.Height / maxHeight;
        //        Double ratio = Math.Max(xRatio, yRatio);
        //        int nnx = (int)Math.Floor(img.Width / ratio);
        //        int nny = (int)Math.Floor(img.Height / ratio);
        //        Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
        //        using (Graphics gr = Graphics.FromImage(cpy))
        //        {
        //            //gr.Clear(Color.Transparent);

        //            // This is said to give best quality when resizing images
        //            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //            gr.DrawImage(img,
        //                new Rectangle(0, 0, nnx, nny),
        //                new Rectangle(0, 0, img.Width, img.Height),
        //                GraphicsUnit.Pixel);
        //        }
        //        return cpy;
        //    }

        //}
        //private MemoryStream BytearrayToStream(byte[] arr)
        //{
        //    return new MemoryStream(arr, 0, arr.Length);
        //}

        // GET: Modules/Edit/5
        public ActionResult EditModules(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Module module = _modulesRepository.FindById(id);
            if (module == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("CreateModules", module);
        }

        [HttpPost]
        public ActionResult EditModules([Bind(/*Include =*/ "ModuleId,ModuleCode,ModuleName,ModuleCaption,ModuleDescription,ModuleLink,ModuleImage,ModuleImagePath,ModuleImageExtension,isInactive,SLNo")] Module module)
        {
            if (ModelState.IsValid)
            {
                _modulesRepository.Update(module);
                return RedirectToAction("ModulesList");
            }
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult DeleteModules(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Module module = _modulesRepository.FindById(id);
            if (module == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";

            return View("CreateModules", module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("DeleteModules")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteModulesConfirmed(int id)
        {
            try
            {
                Module module = _modulesRepository.FindById(id);
                _modulesRepository.Delete(module);

                return Json(new { Success = 1, ModuleId = module.ModuleId, ex = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = 0,
                    ex = ex.InnerException.InnerException.Message.ToString()
                });
            }
        }
        #endregion

        #region Unit Masters
        public ActionResult UnitMastersList()
        {
            var unitMasters = _unitMastersRepository.All()
                .Include(u => u.UnitGroup)
                .Where(x => !x.IsDelete);
            return View(unitMasters.ToList());
        }
        // GET: UnitMasters/Create
        //[OverridableAuthorize]
        public ActionResult CreateUnitMasters()
        {
            ViewBag.Title = "Create";
            ViewBag.UnitGroupId = _unitMastersRepository.UnitGroupId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUnitMasters([Bind(/*Include =*/ "UMId,UnitMasterId,UnitGroupId,UnitName,RelativeFactor,IsBaseUOM")] UnitMaster unitMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (unitMaster.UMId != null)
                    {
                        unitMaster.DateAdded = DateTime.Now;
                        unitMaster.DateUpdated = DateTime.Now;

                        if (unitMaster.UMId == unitMaster.UnitMasterId)
                        {
                            _unitMastersRepository.Update(unitMaster);
                        }
                        else
                        {
                            UnitMaster abcd = unitMaster;
                            _unitMastersRepository.Add(abcd);

                            UnitMaster deleteunit = _context.UnitMasters.Where(m => m.UnitMasterId == abcd.UMId).FirstOrDefault();

                            _unitMastersRepository.Delete(deleteunit);
                        }
                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                    }
                    else
                    {
                        unitMaster.DateAdded = DateTime.Now;
                        unitMaster.DateUpdated = DateTime.Now;

                        _unitMastersRepository.Add(unitMaster);
                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                    }
                    return RedirectToAction("UnitMastersList");
                }

                ViewBag.UnitGroupId = _unitMastersRepository.UnitGroupId1(unitMaster);
                return View(unitMaster);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                TempData["Status"] = "0";
                throw ex;
            }
        }

        public ActionResult EditUnitMasters(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.Title = "Edit";
            UnitMaster unitMaster = _unitMastersRepository.Find(id);
            unitMaster.UMId = unitMaster.UnitMasterId;

            if (unitMaster == null)
            {
                return NotFound();
            }
            ViewBag.UnitGroupId = _unitMastersRepository.UnitGroupId1(unitMaster);
            return View("CreateUnitMasters", unitMaster);
        }

        public ActionResult DeleteUnitMasters(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.Title = "Delete";
            UnitMaster unitMaster = _unitMastersRepository.Find(id);
            unitMaster.UMId = unitMaster.UnitMasterId;

            if (unitMaster == null)
            {
                return NotFound();
            }
            ViewBag.UnitGroupId = _unitMastersRepository.UnitGroupId1(unitMaster);
            return View("CreateUnitMasters", unitMaster);
        }

        [HttpPost, ActionName("DeleteUnitMasters")]
        public JsonResult DeleteUnitMastersConfirmed(string id)
        {
            try
            {
                UnitMaster unitMaster = _unitMastersRepository.Find(id);
                _unitMastersRepository.Delete(unitMaster);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = unitMaster.UnitMasterId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 1, ex = ex.Message.ToString() });
            }
        }
        #endregion

        #region Report Permissions
        public ActionResult ReportPermissionsList()
        {
            var data = _reportPermissionsRepository.GetAll().Where(x => !x.IsDelete).ToList();
            return View(data);
        }

        public JsonResult getUserReport(string userid)
        {
            var ListOfReportPermission = _reportPermissionsRepository.GetReportPermissionsList(userid);
            return Json(ListOfReportPermission);
        }

        public ActionResult ReportPermissionsDetails(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = _context.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        public class GetUserModel1
        {
            public Guid AppKey { get; set; }
        }

        // GET: Countries/Create
        public ActionResult CreateReportPermissions()
        {
            //need to have all users and company list fom chitra api

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel1();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetAllUsers")));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);


            if (res.Companies != null || res.MyUsers != null)
            {
                List<AspnetUserList> aspNetUser = new List<AspnetUserList>();
                foreach (var user in res.MyUsers)
                {
                    var u = new AspnetUserList();
                    u.UserId = user.UserID;
                    u.UserName = user.UserName;
                    aspNetUser.Add(u);
                }
                ViewBag.UserId = new SelectList(aspNetUser, "UserId", "UserName");

                return View(new List<ReportPermissionsVM>());
            }
            return View(new List<ReportPermissionsVM>());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        ////[Authorize]
        //public string Create(List<CompanyPermission> companypermission)
        public JsonResult CreateReportPermissions(List<ReportPermissions> ReportPermissions, string UserId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userid = UserId; //HttpContext.Session.GetString("userid");
                    var comid = HttpContext.Session.GetString("comid");

                    var exist = _context.ReportPermissions.Where(c => c.ComId == comid && c.UserId == UserId).ToList();
                    if (exist.Count > 0)
                    {
                        _context.RemoveRange(exist);
                        _context.SaveChanges();
                    }

                    if (ReportPermissions != null)
                    {
                        ReportPermissions.ForEach(x =>
                        {
                            x.ComId = comid;
                            x.UserId = userid.ToString();
                        });

                        _context.ReportPermissions.AddRange(ReportPermissions);
                        _context.SaveChanges();

                        return Json(new { Success = 1, ex = "" });
                    }
                    return Json(new { Success = 1, ex = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }

        // GET: Countries/Edit/5
        public ActionResult EditReportPermissions(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = _context.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReportPermissions([Bind(/*Include =*/ "CountryCode,CountryName,CurrencyName,CountryShortName,CultureInfo,CurrencySymbol,CurrencyShortName,flagclass,DialCode")] Country country)
        {
            if (ModelState.IsValid)
            {

                _reportPermissionsRepository.UpdateCountry(country);

                return RedirectToAction("ReportPermissionsList");
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public ActionResult DeleteReportPermissions(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = _context.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("DeleteReportPermissions")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReportPermissionsConfirmed(int id)
        {
            _reportPermissionsRepository.DeleteCountry(id);
            return RedirectToAction("ReportPermissionsList");
        }
        #endregion

        #region System Admin

        public async Task<IActionResult> SystemAdminUserLogingList()
        {
            var comid = HttpContext.Session.GetString("comid");
            var getCompanyUsersURL = $"{_configuration.GetValue<string>("API:GetAllCompnayUsers")}/{comid}";
            var companyUserList = await APIHelper.GetRequest<List<CompanyUserVM>>(getCompanyUsersURL, false);


            ViewBag.Userlist = new SelectList(companyUserList, "UserId", "UserName");
            return View();
        }

        public JsonResult Get(string UserList, string FromDate, string ToDate)
        {
            try
            {
                string comid = (HttpContext.Session.GetString("comid"));

                DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
                DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

                if (FromDate == null || FromDate == "")
                {
                }
                else
                {
                    dtFrom = Convert.ToDateTime(FromDate);
                }
                if (ToDate == null || ToDate == "")
                {
                }
                else
                {
                    dtTo = Convert.ToDateTime(ToDate);
                }

                Microsoft.Extensions.Primitives.StringValues y = "";
                var x = Request.Form.TryGetValue("search[value]", out y);
                UserPermission permission = HttpContext.Session.GetObject<UserPermission>("userpermission");

                var userlogininfodata = _context.UserLogingInfos.Where(x => x.comid == comid).OrderByDescending(x => x.UserLogingInfoId);

                if (y.ToString().Length > 0)
                {
                }
                else
                {
                    if (UserList == null)
                    {
                        userlogininfodata = _context.UserLogingInfos.Where(x => x.comid == comid && x.LoginDate >= dtFrom && x.LoginDate <= dtTo).OrderByDescending(x => x.UserLogingInfoId);
                    }
                    else
                    {
                        userlogininfodata = _context.UserLogingInfos.Where(x => x.comid == comid && x.userid == UserList && x.LoginDate >= dtFrom && x.LoginDate <= dtTo).OrderByDescending(x => x.UserLogingInfoId);
                    }

                }

                var query = (from e in userlogininfodata
                             select new UserLogingInfoVM
                             {
                                 UserLogingInfoId = e.UserLogingInfoId,
                                 WebLink = e.WebLink,
                                 LoginDate = e.LoginDate.Value.ToString("dd-MMM-yyyy"),
                                 LoginTime = e.LoginTime.Value.ToString("hh:mm:ss"),
                                 PcName = e.PcName,
                                 MacAddress = e.MacAddress,
                                 IPAddress = e.IPAddress,
                                 DeviceType = e.DeviceType,
                                 Platform = e.Platform,
                                 WebBrowserName = e.WebBrowserName,
                                 LongString = e.LongString,
                                 Status = e.Status,
                                 UserName = e.UserName
                             });
                var parser = new Parser<UserLogingInfoVM>(Request.Form, query);

                return Json(parser.Parse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region User Permission
        public IActionResult UserPermissionList()
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _userPermissionRepository.GetAll()
                .Where(x => x.ComId == comid && !x.IsDelete)
                .Include(x => x.HR_Emp_Info).ToList();
            return View(data);
        }

        // GET: UserPermission/Create
        public IActionResult CreateUserPermission()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            ViewBag.Title = "Create";

            ViewData["EmpId"] = _empReleaseRepository.EmpList();

            ViewBag.Userlist = _userPermissionRepository.Userlist();

            return View();
        }

        [HttpPost]
        public IActionResult CreateUserPermission(/*[Bind("PermissionId,UserPermissionName,UserPermissionhortName")]*/ UserPermission UserPermission)
        {
            if (ModelState.IsValid)
            {
                UserPermission.UserId = HttpContext.Session.GetString("userid");
                UserPermission.ComId = HttpContext.Session.GetString("comid");
                if (UserPermission.PermissionId > 0)
                {
                    _userPermissionRepository.Update(UserPermission);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), UserPermission.PermissionId.ToString(), "Update", UserPermission.EmpId.ToString());
                }
                else
                {
                    _userPermissionRepository.Add(UserPermission);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), UserPermission.PermissionId.ToString(), "Create", UserPermission.EmpId.ToString());
                }
                return RedirectToAction(nameof(UserPermissionList));
            }
            return View(UserPermission);
        }

        // GET: UserPermission/Edit/5
        public IActionResult EditUserPermission(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Edit";
            var UserPermission = _userPermissionRepository.FindById(id);

            if (UserPermission == null)
            {
                return NotFound();
            }
            ViewData["EmpId"] = _empReleaseRepository.EmpList();

            ViewBag.Userlist = _userPermissionRepository.Userlist();

            return View("CreateUserPermission", UserPermission);
        }

        // GET: UserPermission/Delete/5
        public IActionResult DeleteUserPermission(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            var UserPermission = _userPermissionRepository.FindById(id);

            if (UserPermission == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            ViewData["EmpId"] = _empReleaseRepository.EmpList();

            ViewBag.Userlist = _userPermissionRepository.Userlist();

            return View("CreateUserPermission", UserPermission);
        }

        [HttpPost, ActionName("DeleteUserPermission")]
        public IActionResult DeleteUserPermissionConfirmed(int id)
        {
            try
            {
                var UserPermission = _userPermissionRepository.FindById(id);
                _userPermissionRepository.Delete(UserPermission);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), UserPermission.PermissionId.ToString(), "Delete", UserPermission.EmpId.ToString());
                return Json(new { Success = 1, PermissionId = UserPermission.PermissionId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        private bool UserPermissionExists(int id)
        {
            return _context.UserPermission.Any(e => e.PermissionId == id);
        }
        #endregion

        #region Report Types
        public IActionResult ReportTypesList()
        {
            return View("CreateReportTypes");
        }
        public IActionResult GetReportTypes()
        {
            return Json(new { data = _reportTypesRepository.All().Where(x => !x.IsDelete).ToList() });
        }

        // GET: ReportTypes/Details/5
        public IActionResult ReportTypesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportType = _reportTypesRepository.FindById(id);
            if (reportType == null)
            {
                return NotFound();
            }

            return View(reportType);
        }

        // GET: ReportTypes/Create
        public IActionResult CreateReportTypes()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(ApprovalRole model)
        {
            string errorMessage = "Model State is not valid";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.ApprovalRoles.Add(model);
                    _context.SaveChanges();
                    return Json(new { success = 1 });
                }
                catch (Exception ex)
                {

                    errorMessage = ex.Message;
                }

            }
            return Json(new { error = errorMessage });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReportTypes(ReportType reportType)
        {
            string errorMessage = "Model State is not valid";
            if (ModelState.IsValid)
            {
                try
                {
                    _reportTypesRepository.Add(reportType);
                    return Json(new { success = 1 });
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            return Json(new { error = errorMessage });
        }

        // GET: ReportTypes/Edit/5
        public IActionResult EditReportTypes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reportType = _reportTypesRepository.FindById(id);
            if (reportType == null)
            {
                return NotFound();
            }
            return Json(new { data = reportType });
        }
        public IActionResult EditRole(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reportType = _context.ApprovalRoles.Find(id);
            if (reportType == null)
            {
                return NotFound();
            }
            return Json(new { result = reportType });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditReportTypes(int id, [Bind("Id,TypeTitle,Description")] ReportType reportType)
        {
            if (id != reportType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _reportTypesRepository.Update(reportType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportTypeExists(reportType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = 1 });
            }
            return Json(new { error = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(int id, [Bind("Id,RoleTitle,RoleDescription")] ApprovalRole approvalRole)
        {
            if (id != approvalRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportTypeExists(approvalRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = 1 });
            }
            return Json(new { error = 0 });
        }

        // GET: ReportTypes/Delete/5
        public IActionResult DeleteReportTypes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reportType = _reportTypesRepository.FindById(id);
            if (reportType == null)
            {
                return NotFound();
            }
            return View(reportType);
        }

        // POST: ReportTypes/Delete/5
        [HttpPost, ActionName("DeleteReportTypes")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReportTypesConfirmed(int id)
        {
            try
            {
                var reportType = _reportTypesRepository.FindById(id);
                _reportTypesRepository.Delete(reportType);
                return Json(new { success = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _context.ApprovalRoles.FindAsync(id);
                _context.ApprovalRoles.Remove(role);
                _context.SaveChanges();
                return Json(new { success = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public ActionResult GetRoleAndUsers()
        {
            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);
            if (res.MyUsers != null)
            {
                var list = res.MyUsers.ToList();
                var UserListM = new List<AspnetUserList>();
                foreach (var c in list)
                {
                    var le = new AspnetUserList();
                    le.Email = c.UserName;
                    le.UserId = c.UserID;
                    le.UserName = c.UserName;
                    UserListM.Add(le);
                }
                var UserList = new List<users>();
                var RoleList = _context.ApprovalRoles.ToList();

                foreach (var item in UserListM)
                {
                    var users = new users();
                    users.UserId = item.UserId;
                    users.Email = item.Email;
                    UserList.Add(users);

                }
                return Json(new { roleList = RoleList, userList = UserList });
            }
            return Json(new { error = "0" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveApproval(string assignApprovals)
        {
            if (ModelState.IsValid)
            {
                var JObject = new JObject();
                var da = JObject.Parse(assignApprovals);
                string d = da["userRoleList"].ToString();
                var objList = JsonConvert.DeserializeObject<List<AssignApproval>>(d);
                using (var tr = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in objList)
                        {
                            var panel = new ApprovalPanel();
                            panel.ApprovalRoleId = item.RoleId;
                            panel.ReportTypeId = item.RptTypeId;
                            panel.CreatedUserId = item.CreatedUserId;
                            panel.ApprovedUserId = item.ApprovalUserId;
                            panel.ComId = HttpContext.Session.GetString("comid");
                            _context.ApprovalPanels.Add(panel);
                            _context.SaveChanges();
                        }
                        tr.Commit();
                        return Json(new { success = 1 });
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return Json(new { error = 0 });
        }

        public JsonResult GetCreateUsersApprovals(string userId)
        {
            var approvalList = _reportTypesRepository.ApprovalList(userId);
            return Json(new { approvalList });
        }
        public JsonResult GetCreateUsersApprovalsRoles(string userId, int reportTypeId)
        {
            var approvalList = _reportTypesRepository.ApprovalList1(userId, reportTypeId);
            return Json(new { approvalList });
        }

        public JsonResult GetApprovalList(string userId, int reportTypeId)
        {
            var approvalUserList = _reportTypesRepository.ApprovalList2(userId, reportTypeId);
            return Json(new { approvalUserList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovalTransfer(string fromUserId, string toUserId)
        {
            try
            {
                _reportTypesRepository.ApprovalTransfer(fromUserId, toUserId);
                return Json(new { success = 1 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { error = ex.Message });
            }
        }

        private bool ReportTypeExists(int id)
        {
            return _context.ReportTypes.Any(e => e.Id == id);
        }
    }
    #endregion
}
