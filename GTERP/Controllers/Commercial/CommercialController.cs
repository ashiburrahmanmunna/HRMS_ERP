using GTERP.BLL;
using GTERP.Interfaces;
using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Controllers.Commercial
{
    [OverridableAuthorize]
    public class CommercialController : Controller
    {
        private readonly GTRDBContext _context;
        private readonly TransactionLogRepository _tranlog;
        private readonly ILogger<CommercialController> _logger;
        private readonly IBankAccountNoRepository _bankAccountNoRepository;
        private readonly IBuyerGroupsRepository _buyerGroupsRepository;
        private readonly IBuyerInformationsRepository _buyerInformationsRepository;
        private readonly IDestinationsRepository _destinationsRepository;
        private readonly ILienBanksRepository _lienBanksRepository;
        private readonly IOpeningBanksRepository _openingBanksRepository;
        private readonly IPortOfDischargesRepository _portOfDischargesRepository;
        private readonly IPortOfLoadingsRepository _portOfLoadingsRepository;
        private readonly ISisterConcernCompanyRepository _sisterConcernCompanyRepository;
        //private readonly IExportInvoiceMastersRepository _exportInvoiceMastersRepository;
        private readonly ICOM_ProformaInvoiceRepository _cOM_ProformaInvoiceRepository;
        public WebHelper _webHelper { get; }

        public CommercialController(
            GTRDBContext context,
            TransactionLogRepository tranlog,
            ILogger<CommercialController> logger,
            IBankAccountNoRepository bankAccountNoRepository,
            IBuyerGroupsRepository buyerGroupsRepository,
            IBuyerInformationsRepository buyerInformationsRepository,
            IDestinationsRepository destinationsRepository,
            ILienBanksRepository lienBanksRepository,
            IOpeningBanksRepository openingBanksRepository,
            IPortOfDischargesRepository portOfDischargesRepository,
            IPortOfLoadingsRepository portOfLoadingsRepository,
            ISisterConcernCompanyRepository sisterConcernCompanyRepository,
            //IExportInvoiceMastersRepository exportInvoiceMastersRepository,
            ICOM_ProformaInvoiceRepository cOM_ProformaInvoiceRepository,
            WebHelper webHelper
            )
        {
            _context = context;
            _tranlog = tranlog;
            _logger = logger;
            _bankAccountNoRepository = bankAccountNoRepository;
            _buyerGroupsRepository = buyerGroupsRepository;
            _buyerInformationsRepository = buyerInformationsRepository;
            _destinationsRepository = destinationsRepository;
            _lienBanksRepository = lienBanksRepository;
            _openingBanksRepository = openingBanksRepository;
            _portOfDischargesRepository = portOfDischargesRepository;
            _portOfLoadingsRepository = portOfLoadingsRepository;
            _sisterConcernCompanyRepository = sisterConcernCompanyRepository;
            //_exportInvoiceMastersRepository = exportInvoiceMastersRepository;
            _cOM_ProformaInvoiceRepository = cOM_ProformaInvoiceRepository;
            _webHelper = webHelper;
        }


        #region Bank Account No
        public ActionResult BankAccountNoList()
        {
            var bankAccountNos = _bankAccountNoRepository.GetBanAccNo();
            return View(bankAccountNos);
        }

        public ActionResult CreateBankAccountNo()
        {
            ViewBag.Title = "Create";
            ViewBag.SisterConcernCompanyId = _bankAccountNoRepository.SisterConcernCompanyId();
            ViewBag.OpeningBankId = _bankAccountNoRepository.OpeningBankId();
            return View();
        }

        [HttpPost]
        public ActionResult CreateBankAccountNo([Bind(/*Include =*/ "BankAccountId,BankAccountNumber,SisterConcernCompanyId,OpeningBankId")] BankAccountNo bankAccountNo)
        {
            var userid = HttpContext.Session.GetString("userid");
            try
            {
                if (bankAccountNo.BankAccountId > 0)
                {
                    bankAccountNo.UserId = userid;
                    bankAccountNo.UpdateByUserId = userid;

                    _bankAccountNoRepository.Update(bankAccountNo);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                }
                else
                {
                    _bankAccountNoRepository.Add(bankAccountNo);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                }

                ViewBag.SisterConcernCompanyId = _bankAccountNoRepository.SisterConcernCompanyId();
                ViewBag.OpeningBankId = _bankAccountNoRepository.OpeningBankId();

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                TempData["Status"] = "0";
            }
            return RedirectToAction("BankAccountNoList");
        }

        // GET: BankAccountNoes/Edit/5
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        public ActionResult EditBankAccountNo(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }


            ViewBag.Title = "Edit";
            BankAccountNo bankAccountNo = _context.BankAccountNos.Where(x => x.BankAccountId == id).FirstOrDefault();
            if (bankAccountNo == null)
            {
                return NotFound();
            }
            ViewBag.SisterConcernCompanyId = _bankAccountNoRepository.SisterConcernCompanyId();
            ViewBag.OpeningBankId = _bankAccountNoRepository.OpeningBankId();
            return View("CreateBankAccountNo", bankAccountNo);
        }

        // GET: BankAccountNoes/Delete/5
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        public ActionResult DeleteBankAccountNo(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            BankAccountNo bankAccountNo = _bankAccountNoRepository.FindByIdBankAccNo(id);
            if (bankAccountNo == null)
            {
                return NotFound();
            }
            ViewBag.SisterConcernCompanyId = _bankAccountNoRepository.SisterConcernCompanyId();
            ViewBag.OpeningBankId = _bankAccountNoRepository.OpeningBankId();
            return View("CreateBankAccountNo", bankAccountNo);
        }

        // POST: BankAccountNoes/Delete/5
        [HttpPost, ActionName("DeleteBankAccountNo")]
        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        ////[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteBankAccountNoConfirmed(int id)
        {
            try
            {
                BankAccountNo bankAccountNo = _bankAccountNoRepository.FindById(id);
                _bankAccountNoRepository.Delete(bankAccountNo);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";

                return Json(new { Success = 1, bankAccountNo.BankAccountId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }

        }
        #endregion

        #region Buyer Groups
        public ActionResult BuyerGroupsList()
        {
            var data = _buyerGroupsRepository.GetAll().Where(x => !x.IsDelete).ToList();
            return View(data);

        }

        public ActionResult testcallback()
        {
            Microsoft.Extensions.Primitives.StringValues originValues;
            Request.Headers.TryGetValue("Origin", out originValues);


            var callbackUrl = originValues;

            var redirectUrl = new Uri($"https://localhost:44330/Home/testResponse");
            var d = new req();
            d.OrderId = "OrderId_12345";
            d.Password = "Password_saasdf";
            string request = JsonConvert.SerializeObject(d);
            //string response = WebHelper.Post(redirectUrl, request);
            return Ok();
        }
        class req
        {
            public int Id { get; set; }
            public string OrderId { get; set; }
            public string Password { get; set; }
        }
        public IActionResult GenerateReport()
        {
            string callBackUrl = _buyerGroupsRepository.ReportGenerate();
            return Redirect(callBackUrl);
        }

        //  [OverridableAuthorize(typeof(object))]
        public ActionResult CreateBuyerGroups()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateBuyerGroups(BuyerGroup BuyerGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var comid = HttpContext.Session.GetString("comid");

                    if (BuyerGroup.BuyerGroupId > 0)
                    {
                        ViewBag.Title = "Edit";
                        BuyerGroup.ComId = comid;

                        _buyerGroupsRepository.Update(BuyerGroup);

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), BuyerGroup.BuyerGroupId.ToString(), "Create", BuyerGroup.BuyerGroupName.ToString());
                    }
                    else
                    {
                        ViewBag.Title = "Create";
                        BuyerGroup.ComId = comid;
                        _buyerGroupsRepository.Add(BuyerGroup);

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), BuyerGroup.BuyerGroupId.ToString(), "Create", BuyerGroup.BuyerGroupName.ToString());
                    }
                }

                return RedirectToAction("BuyerGroupsList");

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                BuyerGroup.BuyerGroupId = 0;
                TempData["Status"] = "0";

                return View(BuyerGroup);
                throw ex;
            }

        }
        // GET: BuyerGroups/Edit/5
        //[OverridableAuthorize]
        public ActionResult EditBuyerGroups(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return BadRequest();
            }
            BuyerGroup BuyerGroup = _buyerGroupsRepository.FindById(id);
            if (BuyerGroup == null)
            {
                return NotFound();
            }
            return View("CreateBuyerGroups", BuyerGroup);

        }
        // GET: BuyerGroups/Delete/5

        //[OverridableAuthorize]
        public ActionResult DeleteBuyerGroups(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return BadRequest();
            }
            BuyerGroup BuyerGroup = _buyerGroupsRepository.FindById(id);
            if (BuyerGroup == null)
            {
                return NotFound();
            }
            return View("CreateBuyerGroups", BuyerGroup);
        }

        // POST: BuyerGroups/Delete/5
        [HttpPost, ActionName("DeleteBuyerGroups")]
        //[OverridableAuthorize]

        public ActionResult DeleteBuyerGroupsConfirmed(int id)
        {
            try
            {
                BuyerGroup BuyerGroup = _buyerGroupsRepository.FindById(id);
                _buyerGroupsRepository.Delete(BuyerGroup);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "1";
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), BuyerGroup.BuyerGroupId.ToString(), "Delete", BuyerGroup.BuyerGroupName);

                return Json(new { Success = 1, id = BuyerGroup.BuyerGroupId, ex = "Data Delete Successfully" });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";

                return Json(new
                {
                    Success = 0,
                    ex = ex.Message.ToString()
                });

            }
        }
        #endregion

        #region Buyer Informations
        public ActionResult BuyerInformationsList()
        {
            var data = _buyerInformationsRepository.GetAll().Where(x => !x.IsDelete).ToList();
            return View(data);
        }
        // GET: BuyerInformations/Create
        //[OverridableAuthorize]
        public ActionResult CreateBuyerInformations()
        {
            ViewBag.Title = "Create";

            ViewBag.BuyerGroupId = _buyerInformationsRepository.BuyerGroupId();
            ViewBag.CountryId = _buyerInformationsRepository.CountryId();
            ViewBag.EmployeeIdImport = _buyerInformationsRepository.EmployeeIdImport();
            ViewBag.EmployeeIdExport = _buyerInformationsRepository.EmployeeIdExport();

            return View();
        }

        [HttpPost]

        public ActionResult CreateBuyerInformations(/*[Bind("BuyerId,BuyerGroupId,EmployeeIdExport,EmployeeIdImport,BuyerName , BuyerSearchName,CountryId,ContactPerson,Address,Address2,ShippingMarks,LocalOffice,LCMargin,DefferredPaymentDays,Addedby,Dateadded,Updatedby,Dateupdated,DiscountPercentage , CMPPercentage")]*/ BuyerInformation buyerInformation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (buyerInformation.BuyerId > 0)
                    {
                        buyerInformation.UserId = HttpContext.Session.GetString("userid");
                        buyerInformation.DateUpdated = DateTime.Now;
                        buyerInformation.ComId = HttpContext.Session.GetString("comid");

                        _buyerInformationsRepository.Update(buyerInformation);

                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                    }
                    else
                    {
                        buyerInformation.UserId = HttpContext.Session.GetString("userid");
                        buyerInformation.DateAdded = DateTime.Now;
                        buyerInformation.DateUpdated = DateTime.Now;
                        buyerInformation.ComId = HttpContext.Session.GetString("comid");

                        _buyerInformationsRepository.Add(buyerInformation);

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                    }
                    return RedirectToAction("BuyerInformationsList");
                }

                ViewBag.CountryId = _buyerInformationsRepository.CountryId();
                ViewBag.BuyerGroupId = _buyerInformationsRepository.BuyerGroupId();
                ViewBag.EmployeeIdImport = _buyerInformationsRepository.EmployeeIdImport();
                ViewBag.EmployeeIdExport = _buyerInformationsRepository.EmployeeIdExport();

                return View(buyerInformation);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                buyerInformation.BuyerId = 0;
                TempData["Status"] = "0";

                throw ex;
            }
        }
        // GET: BuyerInformations/Edit/5

        //[OverridableAuthorize]
        public ActionResult EditBuyerInformations(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";

            BuyerInformation buyerinformation = _buyerInformationsRepository.FindById(id);
            if (buyerinformation == null)
            {
                return NotFound();
            }
            ViewBag.CountryId = _buyerInformationsRepository.CountryId();
            ViewBag.BuyerGroupId = _buyerInformationsRepository.BuyerGroupId();
            ViewBag.EmployeeIdImport = _buyerInformationsRepository.EmployeeIdImport();
            ViewBag.EmployeeIdExport = _buyerInformationsRepository.EmployeeIdExport();
            return View("CreateBuyerInformations", buyerinformation);
        }

        //[OverridableAuthorize]
        public ActionResult DeleteBuyerInformations(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            BuyerInformation buyerinformation = _buyerInformationsRepository.FindById(id);
            if (buyerinformation == null)
            {
                return NotFound();
            }
            ViewBag.CountryId = _buyerInformationsRepository.CountryId();
            ViewBag.BuyerGroupId = _buyerInformationsRepository.BuyerGroupId();
            ViewBag.EmployeeIdImport = _buyerInformationsRepository.EmployeeIdImport();
            ViewBag.EmployeeIdExport = _buyerInformationsRepository.EmployeeIdExport();
            return View("CreateBuyerInformations", buyerinformation);
        }

        // POST: BuyerInformations/Delete/5
        [HttpPost, ActionName("DeleteBuyerInformations")]
        public JsonResult DeleteBuyerInformationsConfirmed(int id)
        {
            try
            {
                BuyerInformation buyerInformation = _buyerInformationsRepository.FindById(id);

                _buyerInformationsRepository.Delete(buyerInformation);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, ContactID = buyerInformation.BuyerId, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
        #endregion

        #region Destinations
        public ActionResult DestinationsList()
        {
            var data = _destinationsRepository.GetAll().Include(x => x.Countrys).Where(x => !x.IsDelete);
            return View(data);
        }
        // GET: Destinations/Create
        //[OverridableAuthorize]
        public ActionResult CreateDestinations()
        {
            ViewBag.Title = "Create";
            ViewBag.CountryId = _destinationsRepository.CountryId();

            return View();
        }
        [HttpPost]
        public ActionResult CreateDestinations([Bind(/*Include =*/ "DestinationID,DestinationCode,DestinationName,CountryId,DestinationNameSearch")] Destination destination)
        {
            if (destination.DestinationID > 0)
            {
                destination.DateAdded = DateTime.Now;
                destination.DateUpdated = DateTime.Now;

                _destinationsRepository.Update(destination);

                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
            }
            else
            {
                destination.DateAdded = DateTime.Now;
                destination.DateUpdated = DateTime.Now;

                _destinationsRepository.Add(destination);

                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
            }
            return RedirectToAction("DestinationsList");
        }
        // GET: Destinations/Edit/5
        //[OverridableAuthorize]
        public ActionResult EditDestinations(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            Destination destination = _destinationsRepository.FindById(id);
            ViewBag.CountryId = _destinationsRepository.CountryId();

            if (destination == null)
            {
                return NotFound();
            }
            return View("CreateDestinations", destination);
        }
        // GET: Destinations/Delete/5
        //[OverridableAuthorize]
        public ActionResult DeleteDestinations(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            Destination destination = _destinationsRepository.FindById(id);
            ViewBag.CountryId = _destinationsRepository.CountryId();
            if (destination == null)
            {
                return NotFound();
            }
            return View("CreateDestinations", destination);
        }
        [HttpPost, ActionName("DeleteDestinations")]
        public ActionResult DeleteDestinationsConfirmed(int id)
        {
            try
            {
                Destination Destinations = _destinationsRepository.FindById(id);

                _destinationsRepository.Delete(Destinations);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = Destinations.DestinationID, ex = TempData["Message"].ToString() });

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        #endregion

        #region Lien Banks
        public ActionResult LienBanksList()
        {
            var data = _lienBanksRepository.GetAll().Include(x => x.Country).Where(x => !x.IsDelete);
            return View(data);
        }

        public ActionResult CreateLienBanks()
        {
            ViewBag.Title = "Create";
            ViewBag.CountryId = _lienBanksRepository.CountryId();
            return View();
        }
        [HttpPost]
        public ActionResult CreateLienBanks(LienBank LienBank)
        {
            try
            {
                if (LienBank.LienBankId > 0)
                {
                    LienBank.DateUpdated = DateTime.Now;
                    LienBank.DateAdded = DateTime.Now;
                    LienBank.UserId = HttpContext.Session.GetString("userid");
                    LienBank.ComId = HttpContext.Session.GetString("comid");
                    _lienBanksRepository.Update(LienBank);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                }
                else
                {
                    LienBank.DateAdded = DateTime.Now;
                    LienBank.DateUpdated = DateTime.Now;
                    LienBank.UserId = HttpContext.Session.GetString("userid");
                    LienBank.ComId = HttpContext.Session.GetString("comid");
                    _lienBanksRepository.Add(LienBank);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                }
                return RedirectToAction("LienBanksList");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                TempData["Status"] = "0";
                throw ex;
            }
        }

        public ActionResult EditLienBanks(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            LienBank LienBank = _lienBanksRepository.FindById(id);
            ViewBag.CountryId = _lienBanksRepository.CountryId1(LienBank);

            if (LienBank == null)
            {
                return NotFound();
            }
            return View("CreateLienBanks", LienBank);
        }

        public ActionResult DeleteLienBanks(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            LienBank LienBank = _lienBanksRepository.FindById(id);
            ViewBag.CountryId = _lienBanksRepository.CountryId1(LienBank);
            if (LienBank == null)
            {
                return NotFound();
            }
            return View("CreateLienBanks", LienBank);
        }
        [HttpPost, ActionName("DeleteLienBanks")]
        public JsonResult DeleteLienBanksConfirmed(int id)
        {
            try
            {
                LienBank LienBank = _lienBanksRepository.FindById(id);
                _lienBanksRepository.Delete(LienBank);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = LienBank.LienBankId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() }); ;
            }
        }
        #endregion

        #region Opening Banks
        public ActionResult OpeningBanksList()
        {
            var data = _openingBanksRepository.GetAll().Include(x => x.Country).Where(x => !x.IsDelete);
            return View(data);
        }
        public ActionResult CreateOpeningBanks()
        {
            ViewBag.Title = "Create";
            ViewBag.CountryId = _openingBanksRepository.CountryId();
            return View();
        }
        [HttpPost]
        public ActionResult CreateOpeningBanks([Bind(/*Include =*/ "OpeningBankId,OpeningBankName,CountryId,SwiftCode,BranchAddress , BranchAddress2,PhoneNo,Remarks")] OpeningBank openingBank)
        {
            try
            {
                if (openingBank.OpeningBankId > 0)
                {
                    openingBank.DateUpdated = DateTime.Now;
                    openingBank.DateAdded = DateTime.Now;
                    openingBank.UserId = HttpContext.Session.GetString("userid");
                    openingBank.ComId = (HttpContext.Session.GetString("comid"));
                    _openingBanksRepository.Update(openingBank);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                }
                else
                {
                    openingBank.DateAdded = DateTime.Now;
                    openingBank.DateUpdated = DateTime.Now;
                    openingBank.UserId = HttpContext.Session.GetString("userid");
                    openingBank.ComId = (HttpContext.Session.GetString("comid"));
                    _openingBanksRepository.Add(openingBank);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                }
                return RedirectToAction("OpeningBanksList");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult EditOpeningBanks(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            OpeningBank openingBank = _openingBanksRepository.FindById(id);
            ViewBag.CountryId = _openingBanksRepository.CountryId1(openingBank);
            if (openingBank == null)
            {
                return NotFound();
            }
            return View("CreateOpeningBanks", openingBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOpeningBanks([Bind(/*Include =*/ "OpeningBankId,OpeningBankName,CountryId,SwiftCode,BranchAddress , BranchAddress2,PhoneNo,Remarks")] OpeningBank openingBank)
        {
            if (ModelState.IsValid)
            {
                _openingBanksRepository.Update(openingBank);
                return RedirectToAction("OpeningBanksList");
            }
            return View(openingBank);
        }

        public ActionResult DeleteOpeningBanks(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            OpeningBank openingBank = _openingBanksRepository.FindById(id);
            ViewBag.CountryId = _openingBanksRepository.CountryId1(openingBank);
            if (openingBank == null)
            {
                return NotFound();
            }
            return View("CreateOpeningBanks", openingBank);
        }

        [HttpPost, ActionName("DeleteOpeningBanks")]
        public JsonResult DeleteOpeningBanksConfirmed(int id)
        {
            try
            {
                OpeningBank openingBank = _openingBanksRepository.FindById(id);
                _openingBanksRepository.Delete(openingBank);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = openingBank.OpeningBankId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() }); ;
            }
        }
        #endregion

        #region Port Of Discharges
        public ActionResult PortOfDischargesList()
        {
            var data = _portOfDischargesRepository
                .GetAll()
                .Include(x => x.Countrys)
                .Where(x => !x.IsDelete);
            return View(data);
        }
        public ActionResult CreatePortOfDischarges()
        {
            ViewBag.Title = "Create";
            ViewBag.CountryId = _portOfDischargesRepository.CountryId();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePortOfDischarges([Bind(/*Include =*/ "PortOfDischargeId,PortCode,PortOfDischargeName,CountryId")] PortOfDischarge PortOfDischarge)
        {
            if (PortOfDischarge.PortOfDischargeId > 0)
            {
                PortOfDischarge.DateAdded = DateTime.Now;
                PortOfDischarge.DateUpdated = DateTime.Now;

                _portOfDischargesRepository.Update(PortOfDischarge);
                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
            }
            else
            {
                PortOfDischarge.DateAdded = DateTime.Now;
                PortOfDischarge.DateUpdated = DateTime.Now;
                _portOfDischargesRepository.Add(PortOfDischarge);
                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
            }
            return RedirectToAction("PortOfDischargesList");
        }

        public ActionResult EditPortOfDischarges(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            PortOfDischarge PortOfDischarge = _portOfDischargesRepository.FindById(id);
            ViewBag.CountryId = _portOfDischargesRepository.CountryId1(PortOfDischarge);

            if (PortOfDischarge == null)
            {
                return NotFound();
            }
            return View("CreatePortOfDischarges", PortOfDischarge);
        }
        public ActionResult DeletePortOfDischarges(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            PortOfDischarge PortOfDischarge = _portOfDischargesRepository.FindById(id);
            ViewBag.CountryId = _portOfDischargesRepository.CountryId1(PortOfDischarge);
            if (PortOfDischarge == null)
            {
                return NotFound();
            }
            return View("CreatePortOfDischarges", PortOfDischarge);
        }
        [HttpPost, ActionName("DeletePortOfDischarges")]
        public ActionResult DeletePortOfDischargesConfirmed(int id)
        {
            try
            {
                PortOfDischarge PortOfDischarges = _portOfDischargesRepository.FindById(id);
                _portOfDischargesRepository.Delete(PortOfDischarges);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = PortOfDischarges.PortOfDischargeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
        #endregion

        #region Port Of Loadings
        public ActionResult PortOfLoadingsList()
        {
            var data = _portOfLoadingsRepository.GetAll()
                .Include(x => x.Countrys).Where(x => !x.IsDelete);
            return View(data);
        }
        public ActionResult CreatePortOfLoadings()
        {
            ViewBag.Title = "Create";
            ViewBag.CountryId = _portOfLoadingsRepository.CountryId();

            return View();
        }
        [HttpPost]
        public ActionResult CreatePortOfLoadings([Bind(/*Include =*/ "PortOfLoadingId,PortCode,PortOfLoadingName,CountryId")] PortOfLoading PortOfLoading)
        {
            if (PortOfLoading.PortOfLoadingId > 0)
            {
                PortOfLoading.DateAdded = DateTime.Now;
                PortOfLoading.DateUpdated = DateTime.Now;
                _portOfLoadingsRepository.Update(PortOfLoading);
                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
            }
            else
            {
                PortOfLoading.DateAdded = DateTime.Now;
                PortOfLoading.DateUpdated = DateTime.Now;
                _portOfLoadingsRepository.Add(PortOfLoading);
                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
            }
            return RedirectToAction("PortOfLoadingsList");
        }

        public ActionResult EditPortOfLoadings(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            PortOfLoading PortOfLoading = _portOfLoadingsRepository.FindById(id);
            ViewBag.CountryId = _portOfLoadingsRepository.CountryId1(PortOfLoading);

            if (PortOfLoading == null)
            {
                return NotFound();
            }
            return View("CreatePortOfLoadings", PortOfLoading);
        }

        public ActionResult DeletePortOfLoadings(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            PortOfLoading PortOfLoading = _portOfLoadingsRepository.FindById(id);
            ViewBag.CountryId = _portOfLoadingsRepository.CountryId1(PortOfLoading);
            if (PortOfLoading == null)
            {
                return NotFound();
            }
            return View("CreatePortOfLoadings", PortOfLoading);
        }
        [HttpPost, ActionName("DeletePortOfLoadings")]
        public ActionResult DeletePortOfLoadingsConfirmed(int id)
        {
            try
            {
                PortOfLoading PortOfLoadings = _portOfLoadingsRepository.FindById(id);
                _portOfLoadingsRepository.Delete(PortOfLoadings);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = PortOfLoadings.PortOfLoadingId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }
        #endregion

        #region Sister Concern Company
        public ActionResult SisterConcernCompanyList()
        {
            //var ComId = HttpContext.Session.GetString("comid");
            //var data = db.SisterConcernCompany.Where(x => x.ComId == ComId);
            var data = _sisterConcernCompanyRepository.GetAll().Where(x => !x.IsDelete);
            return View(data);
        }
        public ActionResult CreateSisterConcernCompany()
        {
            ViewBag.Title = "Create";
            return View();
        }
        [HttpPost]
        public ActionResult CreateSisterConcernCompany(SisterConcernCompany SisterConcernCompany)
        {
            try
            {
                ViewBag.Title = "Create";
                if (ModelState.IsValid)
                {
                    if (SisterConcernCompany.SisterConcernCompanyId > 0)
                    {
                        SisterConcernCompany.ComId = (HttpContext.Session.GetString("comid"));
                        SisterConcernCompany.UserId = HttpContext.Session.GetString("userid");

                        _sisterConcernCompanyRepository.Update(SisterConcernCompany);

                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "1";
                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), SisterConcernCompany.ComId.ToString(), "Update", SisterConcernCompany.CompanyName.ToString());

                    }
                    else
                    {
                        SisterConcernCompany.ComId = HttpContext.Session.GetString("comid");
                        SisterConcernCompany.UserId = HttpContext.Session.GetString("userid");
                        _sisterConcernCompanyRepository.Add(SisterConcernCompany);

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), SisterConcernCompany.ComId.ToString(), "Save", SisterConcernCompany.CompanyName.ToString());
                    }
                    return RedirectToAction("SisterConcernCompanyList");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(SisterConcernCompany);
        }
        public ActionResult EditSisterConcernCompany(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";

            SisterConcernCompany SisterConcernCompany = _sisterConcernCompanyRepository.FindById(id);
            if (SisterConcernCompany == null)
            {
                return NotFound();
            }
            return View("CreateSisterConcernCompany", SisterConcernCompany);
        }

        public ActionResult DeleteSisterConcernCompany(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            SisterConcernCompany SisterConcernCompany = _sisterConcernCompanyRepository.FindById(id);
            if (SisterConcernCompany == null)
            {
                return NotFound();
            }
            return View("CreateSisterConcernCompany", SisterConcernCompany);
        }

        [HttpPost, ActionName("DeleteSisterConcernCompany")]
        public ActionResult DeleteSisterConcernCompanyConfirmed(int id)
        {
            try
            {
                SisterConcernCompany SisterConcernCompany = _sisterConcernCompanyRepository.FindById(id);
                _sisterConcernCompanyRepository.Delete(SisterConcernCompany);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                return Json(new { Success = 1, id = SisterConcernCompany.SisterConcernCompanyId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }
        #endregion

        #region Export Invoice Masters

        //private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        //public ActionResult Index(string UserList, string FromDate, string ToDate)
        //{
        //    _exportInvoiceMastersRepository.ExportInvoiceList(UserList, FromDate, ToDate);

        //    ViewBag.Userlist = _exportInvoiceMastersRepository.UserList();
        //    var x = _exportInvoiceMastersRepository.ExportInvoiceList(UserList, FromDate, ToDate);

        //    return View(x);
        //}


        #endregion

        #region COM_ProformaInvoice
        public ActionResult ProformaInvoiceList(int? supplierid, string UserList, string FromDate, string ToDate)
        {
            _cOM_ProformaInvoiceRepository.ProformaInvoiceList(supplierid, UserList, FromDate, ToDate);

            ViewBag.Userlist = _cOM_ProformaInvoiceRepository.AspNetUserList();

            ViewBag.SupplierId = _cOM_ProformaInvoiceRepository.SupplierId();

            var x = _cOM_ProformaInvoiceRepository.ProformaInvoiceList(supplierid, UserList, FromDate, ToDate);

            return View(x);
        }

        public ActionResult PIDailyReceiving(int? supplierid, string FromDate, string ToDate)
        {
            _cOM_ProformaInvoiceRepository.PIDailyReceiving(supplierid, FromDate, ToDate);

            ViewBag.SupplierId = _cOM_ProformaInvoiceRepository.SupplierId();

            ViewBag.PIDailyReceiving = _cOM_ProformaInvoiceRepository.ProductSerialresult();

            return View();
        }

        public ActionResult Download(string file)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "Download\\" + file;
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return response;
        }

        public ActionResult Create(int? supplierid, int? Flag)
        {
            try
            {
                ViewBag.SupplierId = supplierid;
                if (supplierid == null)
                {
                    supplierid = 0;
                }

                ViewBag.SisterConcernCompanyId = _cOM_ProformaInvoiceRepository.SisterConcernCompanyId();
                ViewBag.CurrencyId = _cOM_ProformaInvoiceRepository.CurrencyId();
                ViewBag.SupplierId = _cOM_ProformaInvoiceRepository.SupplierId();
                ViewBag.UnitMasterId = _cOM_ProformaInvoiceRepository.UnitMasterId();
                ViewBag.EmployeeId = _cOM_ProformaInvoiceRepository.EmployeeId();
                ViewBag.ItemGroupId = _cOM_ProformaInvoiceRepository.ItemGroupId();
                ViewBag.ItemDescId = _cOM_ProformaInvoiceRepository.ItemDescId();
                ViewBag.ItemDescArray = _cOM_ProformaInvoiceRepository.ItemDescArray();
                ViewBag.GroupLCId = _cOM_ProformaInvoiceRepository.GroupLCId();
                ViewBag.PaymentTermsId = _cOM_ProformaInvoiceRepository.PaymentTermsId();
                ViewBag.DayListId = _cOM_ProformaInvoiceRepository.DayListId();
                ViewBag.OpeningBankId = _cOM_ProformaInvoiceRepository.OpeningBankId();
                ViewBag.BankAccountId = _cOM_ProformaInvoiceRepository.BankAccountId();
                ViewBag.LienBankAccountId = _cOM_ProformaInvoiceRepository.LienBankAccountId();

                ViewBag.Title = "Create";
                #region excelupload
                if (Flag == 1)
                {
                    var userid = HttpContext.Session.GetString("userid");

                    if (userid.ToString() == "" || userid == null)
                    {

                        return BadRequest();
                    }
                    List<COM_ProformaInvoice> InvoiceListForView = new List<COM_ProformaInvoice>();

                    _cOM_ProformaInvoiceRepository.Create(supplierid, Flag);


                    return View("Create", InvoiceListForView);

                }
                #endregion
                else
                {
                    List<COM_ProformaInvoice> asdf = _context.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString())).ToList();
                    return View(asdf);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult Create(List<COM_ProformaInvoice> COM_ProformaInvoices)
        {
            try
            {
                if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
                {
                    return NotFound();
                }
                if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
                {
                    {
                        List<COM_ProformaInvoice_Sub> com_proforma_itemgrouplist = new List<COM_ProformaInvoice_Sub>();

                        _cOM_ProformaInvoiceRepository.CreatePost(COM_ProformaInvoices);
                        return Json(new { Success = 1, PIId = 0, ex = "Data Save Successfully" });
                    }
                    return Json(new { Success = 0, PIId = 0, ex = "Unable To Save.." });
                }
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

        public ActionResult UploadFiles()
        {
            try
            {
                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }

                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;

                return Json(new { Success = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            COM_ProformaInvoice cOM_ProformaInvoice = _cOM_ProformaInvoiceRepository.FindById(id);
            if (cOM_ProformaInvoice == null)
            {
                return NotFound();
            }
            ViewBag.SisterConcernCompanyId = _cOM_ProformaInvoiceRepository.SisterConcernCompanyId();
            ViewBag.CurrencyId = _cOM_ProformaInvoiceRepository.CurrencyId();
            ViewBag.SupplierId = _cOM_ProformaInvoiceRepository.SupplierId();
            ViewBag.UnitMasterId = _cOM_ProformaInvoiceRepository.UnitMasterId();
            ViewBag.EmployeeId = _cOM_ProformaInvoiceRepository.EmployeeId();
            ViewBag.ItemGroupId = _cOM_ProformaInvoiceRepository.ItemGroupId();
            ViewBag.ItemDescId = _cOM_ProformaInvoiceRepository.ItemDescId();
            ViewBag.GroupLCId = _cOM_ProformaInvoiceRepository.GroupLCId();
            ViewBag.PaymentTermsId = _cOM_ProformaInvoiceRepository.PaymentTermsId();
            ViewBag.DayListId = _cOM_ProformaInvoiceRepository.DayListId();
            ViewBag.OpeningBankId = _cOM_ProformaInvoiceRepository.OpeningBankId();
            //ViewBag.OpeningBankId = new SelectList(db.OpeningBanks.Where(x => x.OpeningBankId == cOM_ProformaInvoice.BankAccountNos.OpeningBankId), "OpeningBankId", "OpeningBankName");
            ViewBag.BankAccountId = _cOM_ProformaInvoiceRepository.BankAccountId();
            ViewBag.LienBankAccountId = _cOM_ProformaInvoiceRepository.LienBankAccountId();
            //ViewBag.PINatureId = new SelectList(db.PINature, "PINatureId", "PINatureName" , cOM_ProformaInvoice.PINatureId);

            string itemdesc = cOM_ProformaInvoice.ItemDescList;

            if (itemdesc == null)
            {
                ViewBag.ItemDescArray = _cOM_ProformaInvoiceRepository.ItemDescArray();
            }
            else
            {
                string[] split = itemdesc.Split(',');
                ViewBag.ItemDescArray = new MultiSelectList(_context.ItemDescs, "ItemDescId", "ItemDescName", split);
            }
            return View("Edit", cOM_ProformaInvoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COM_ProformaInvoice cOM_ProformaInvoice)
        {
            try
            {
                {
                    var text = "";
                    List<COM_ProformaInvoice_Sub> com_proforma_itemgrouplist = new List<COM_ProformaInvoice_Sub>();

                    var existitemdescid = _context.COM_ProformaInvoice_Subs.Where(x => x.PIId == cOM_ProformaInvoice.PIId);
                    foreach (COM_ProformaInvoice_Sub ss in existitemdescid)
                    {
                        _context.COM_ProformaInvoice_Subs.Remove(ss);
                    }
                    for (int i = 0; i < cOM_ProformaInvoice.ItemDescArray.Length; i++)
                    {
                        text += cOM_ProformaInvoice.ItemDescArray[i] + ",";
                        COM_ProformaInvoice_Sub itemgroupsingle = new COM_ProformaInvoice_Sub();
                        itemgroupsingle.PIId = cOM_ProformaInvoice.PIId;
                        itemgroupsingle.ItemDescId = int.Parse(cOM_ProformaInvoice.ItemDescArray[i]);
                        com_proforma_itemgrouplist.Add(itemgroupsingle);
                    }

                    _context.COM_ProformaInvoice_Subs.AddRange(com_proforma_itemgrouplist);

                    cOM_ProformaInvoice.ItemDescList = text.TrimEnd(',');

                    cOM_ProformaInvoice.DateUpdated = DateTime.Now;
                    cOM_ProformaInvoice.UpdateByUserId = HttpContext.Session.GetString("userid");

                    _cOM_ProformaInvoiceRepository.Update(cOM_ProformaInvoice);

                    return RedirectToAction("Index");
                }
                ViewBag.SisterConcernCompanyId = _cOM_ProformaInvoiceRepository.SisterConcernCompanyId();
                ViewBag.CurrencyId = _cOM_ProformaInvoiceRepository.CurrencyId();
                ViewBag.SupplierId = _cOM_ProformaInvoiceRepository.SupplierId();
                ViewBag.UnitMasterId = _cOM_ProformaInvoiceRepository.UnitMasterId();
                ViewBag.EmployeeId = _cOM_ProformaInvoiceRepository.EmployeeId();
                ViewBag.ItemGroupId = _cOM_ProformaInvoiceRepository.ItemGroupId();
                ViewBag.ItemDescId = _cOM_ProformaInvoiceRepository.ItemDescId();
                ViewBag.GroupLCId = _cOM_ProformaInvoiceRepository.GroupLCId();
                ViewBag.PaymentTermsId = _cOM_ProformaInvoiceRepository.PaymentTermsId();
                ViewBag.DayListId = _cOM_ProformaInvoiceRepository.DayListId();
                ViewBag.OpeningBankId = _cOM_ProformaInvoiceRepository.OpeningBankId();
                //ViewBag.OpeningBankId = new SelectList(db.OpeningBanks.Where(x => x.OpeningBankId == cOM_ProformaInvoice.BankAccountNos.OpeningBankId), "OpeningBankId", "OpeningBankName");
                ViewBag.BankAccountId = _cOM_ProformaInvoiceRepository.BankAccountId();
                ViewBag.LienBankAccountId = _cOM_ProformaInvoiceRepository.LienBankAccountId();
                //ViewBag.PINatureId = new SelectList(db.PINature, "PINatureId", "PINatureName" , cOM_ProformaInvoice.PINatureId);

                string itemdesc = cOM_ProformaInvoice.ItemDescList;

                if (itemdesc == null)
                {

                    ViewBag.ItemDescArray = _cOM_ProformaInvoiceRepository.ItemDescArray();
                }
                else
                {
                    string[] split = itemdesc.Split(',');
                    ViewBag.ItemDescArray = new MultiSelectList(_context.ItemDescs, "ItemDescId", "ItemDescName", split);
                }
                ViewBag.Title = "Edit";
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (HttpContext.Session.GetString("comid") == "0" || HttpContext.Session.GetString("comid") == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            COM_ProformaInvoice cOM_ProformaInvoice = _cOM_ProformaInvoiceRepository.FindById(id);
            if (cOM_ProformaInvoice == null)
            {
                return NotFound();
            }
            ViewBag.SisterConcernCompanyId = _cOM_ProformaInvoiceRepository.SisterConcernCompanyId();
            ViewBag.CurrencyId = _cOM_ProformaInvoiceRepository.CurrencyId();
            ViewBag.SupplierId = _cOM_ProformaInvoiceRepository.SupplierId();
            ViewBag.UnitMasterId = _cOM_ProformaInvoiceRepository.UnitMasterId();
            ViewBag.EmployeeId = _cOM_ProformaInvoiceRepository.EmployeeId();
            ViewBag.ItemGroupId = _cOM_ProformaInvoiceRepository.ItemGroupId();
            ViewBag.ItemDescId = _cOM_ProformaInvoiceRepository.ItemDescId();
            ViewBag.GroupLCId = _cOM_ProformaInvoiceRepository.GroupLCId();
            ViewBag.PaymentTermsId = _cOM_ProformaInvoiceRepository.PaymentTermsId();
            ViewBag.DayListId = _cOM_ProformaInvoiceRepository.DayListId();
            ViewBag.OpeningBankId = _cOM_ProformaInvoiceRepository.OpeningBankId();
            //ViewBag.OpeningBankId = new SelectList(db.OpeningBanks.Where(x => x.OpeningBankId == cOM_ProformaInvoice.BankAccountNos.OpeningBankId), "OpeningBankId", "OpeningBankName");
            ViewBag.BankAccountId = _cOM_ProformaInvoiceRepository.BankAccountId();
            ViewBag.LienBankAccountId = _cOM_ProformaInvoiceRepository.LienBankAccountId();
            //ViewBag.PINatureId = new SelectList(db.PINature, "PINatureId", "PINatureName" , cOM_ProformaInvoice.PINatureId);


            string itemdesc = cOM_ProformaInvoice.ItemDescList;

            if (itemdesc == null)
            {
                ViewBag.ItemDescArray = _cOM_ProformaInvoiceRepository.ItemDescArray();
            }
            else
            {
                string[] split = itemdesc.Split(',');
                ViewBag.ItemDescArray = new MultiSelectList(_context.ItemDescs, "ItemDescId", "ItemDescName", split);
            }
            return View("Edit", cOM_ProformaInvoice);
        }

        // POST: COM_ProformaInvoice/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                COM_ProformaInvoice cOM_ProformaInvoice = _cOM_ProformaInvoiceRepository.FindById(id);
                _cOM_ProformaInvoiceRepository.Delete(cOM_ProformaInvoice);
                return Json(new { Success = 1, TermsId = cOM_ProformaInvoice.PIId, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        public JsonResult PI_Duplicate_Check(string id)
        {
            try
            {
                int comid = int.Parse(HttpContext.Session.GetString("comid"));
                List<COM_ProformaInvoice> COM_ProformaInvoicelist = _cOM_ProformaInvoiceRepository.GetAll().ToList();

                List<SelectListItem> pilist = new List<SelectListItem>();

                if (COM_ProformaInvoicelist != null)
                {
                    foreach (COM_ProformaInvoice x in COM_ProformaInvoicelist)
                    {
                        pilist.Add(new SelectListItem { Text = x.PINo, Value = x.PIId.ToString() });
                    }
                }
                return Json(new SelectList(pilist, "Value", "Text"));
            }
            catch (Exception ex)
            {

                return Json(new { success = 0, values = ex.Message.ToString() });
            }
        }

        #endregion
    }
}
