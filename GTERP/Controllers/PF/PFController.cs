using DataTablesParser;
using GTERP.BLL;
using GTERP.Interfaces.Accounts;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.PF;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Accounts;
using GTERP.Repository.HR;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Controllers.PF
{

    public class PFController : Controller
    {
        #region Common Property

        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        public static string AccCodeFirst = "";
        public static string AccCodeSecound = "-0";
        public static string AccCodeThird = "-00";
        public static string AccCodeFourth = "-000";
        public static string AccCodeFifth = "-00000";
        public int isModelBase = 1;
        
        private readonly TransactionLogRepository _tranlog;
        private readonly ILogger<PFController> _logger;
        private readonly GTRDBContext _context;
        private readonly POSRepository POS;
        private readonly IConfiguration _configuration;
        public clsProcedure _clsProc { get; }
        private readonly IAccountProcessRepository _accountProcessRepository;
        private readonly IBankStatementBLRepository _bankStatementBLRepository;
        private readonly IFiscalYearRepository _fiscalYearRepository;
        private readonly IVoucherTypeRepository _voucherTypeRepository;
        private readonly IVoucherNoPrefixRepository _voucherNoPrefixRepository;
        private readonly IGovtScheduleEquityRepository _govtScheduleEquityRepository;
        private readonly IGovtScheduleForeignRepository _govtScheduleForeignRepository;
        private readonly IGovtScheduleJapanLoanRepository _govtScheduleJapanLoanRepository;
        private readonly IPFProcessRepository _PFProcessRepository;
        private readonly IProcessLockRepository _processLockRepository;
        private readonly IAccountChartRepository _accountChartRepository;
        private readonly IBankReconcilRepository _bankReconcilRepository;
        private readonly IVoucherCheckSubNoRepository _voucherCheckSubNoRepository;
        private readonly IAccountDashboardRepository _accountDashboard;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IAccPostVoucherRepository _accPostVoucherRepository;
        private readonly IShowVoucherRepository _showVoucherRepository;
        private readonly ISummaryReportRepository _summaryReportRepository;
        private readonly IGeneralReportRepository _generalReportRepository;
        private readonly IEmpReleaseRepository _empReleaseRepository;
        private readonly IBudgetReleaseRepository _budgetReleaseRepository;
        private readonly IPrdUnitRepository _prdUnitRepository;
        private readonly ICostAllocationRepository _costAllocationRepository;
        private readonly IBillManagementRepository _billManagementRepository;
        private readonly IBankClearingRepository _bankClearingRepository;
        private readonly IVoucherTranGroupRepository _voucherTranGroupRepository;
        private readonly IShareHoldingRepository _shareHoldingRepository;
        private readonly INoteDescriptionRepository _noteDescriptionRepository;
        private readonly IAcc_BudgetRepository _acc_BudgetRepository;
        private readonly IAccVoucherRepository _accVoucherRepository;

        #endregion

        #region Constructor
        public PFController(
            TransactionLogRepository tranlog,
            ILogger<PFController> logger,
            GTRDBContext context,
            clsProcedure clsProc,
            IConfiguration configuration,
            IBankStatementBLRepository bankStatementBLRepository,
            IFiscalYearRepository fiscalYearRepository,
            IVoucherTypeRepository voucherTypeRepository,
            IVoucherNoPrefixRepository voucherNoPrefixRepository,
            IGovtScheduleEquityRepository govtScheduleEquityRepository,
            IGovtScheduleForeignRepository govtScheduleForeignRepository,
            IGovtScheduleJapanLoanRepository govtScheduleJapanLoanRepository,
            IPFProcessRepository PFProcessRepository,
            POSRepository pos,
            IAccountChartRepository accountChartRepository,
            IBankReconcilRepository bankReconcilRepository,
            IVoucherCheckSubNoRepository voucherCheckSubNoRepository,
            IAccountDashboardRepository accountDashboardRepository,
            IBudgetRepository budgetRepository,
            IAccPostVoucherRepository accPostVoucherRepository,
            IShowVoucherRepository showVoucherRepository,
            ISummaryReportRepository summaryReportRepository,
            IGeneralReportRepository generalReportRepository,
            IEmpReleaseRepository empReleaseRepository,
            IBudgetReleaseRepository budgetReleaseRepository,
            IPrdUnitRepository prdUnitRepository,
            ICostAllocationRepository costAllocationRepository,
            IBillManagementRepository billManagementRepository,
            IBankClearingRepository bankClearingRepository,
            IVoucherTranGroupRepository voucherTranGroupRepository,
            IShareHoldingRepository shareHoldingRepository,
            INoteDescriptionRepository noteDescriptionRepository,
            IAcc_BudgetRepository acc_BudgetRepository,
            IAccVoucherRepository accVoucherRepository
,
            IAccountProcessRepository accountProcessRepository)
        {
            _tranlog = tranlog;
            _logger = logger;
            _context = context;
            _clsProc = clsProc;
            _configuration = configuration;
            _bankStatementBLRepository = bankStatementBLRepository;
            _fiscalYearRepository = fiscalYearRepository;
            _voucherTypeRepository = voucherTypeRepository;
            _voucherNoPrefixRepository = voucherNoPrefixRepository;
            _govtScheduleEquityRepository = govtScheduleEquityRepository;
            _govtScheduleForeignRepository = govtScheduleForeignRepository;
            _govtScheduleJapanLoanRepository = govtScheduleJapanLoanRepository;
            _PFProcessRepository = PFProcessRepository;
            POS = pos;
            _accountChartRepository = accountChartRepository;
            _bankReconcilRepository = bankReconcilRepository;
            _voucherCheckSubNoRepository = voucherCheckSubNoRepository;
            _accountDashboard = accountDashboardRepository;
            _budgetRepository = budgetRepository;
            _accPostVoucherRepository = accPostVoucherRepository;
            _showVoucherRepository = showVoucherRepository;
            _summaryReportRepository = summaryReportRepository;
            _generalReportRepository = generalReportRepository;
            _empReleaseRepository = empReleaseRepository;
            _budgetReleaseRepository = budgetReleaseRepository;
            _prdUnitRepository = prdUnitRepository;
            _costAllocationRepository = costAllocationRepository;
            _billManagementRepository = billManagementRepository;
            _bankClearingRepository = bankClearingRepository;
            _voucherTranGroupRepository = voucherTranGroupRepository;
            _shareHoldingRepository = shareHoldingRepository;
            _noteDescriptionRepository = noteDescriptionRepository;
            _acc_BudgetRepository = acc_BudgetRepository;
            _accVoucherRepository = accVoucherRepository;
            _accountProcessRepository = accountProcessRepository;
        }
        #endregion

        //#region Account Bank Statement Balance
        //public ActionResult BankStatementBLList()
        //{
        //    return View(_bankStatementBLRepository.GetAll());
        //}

        ////[Authorize]
        //// GET: Categories/Create
        //public ActionResult CreateBankStatementBL()
        //{

        //    ViewBag.AccId = _bankStatementBLRepository.AccId();
        //    ViewBag.Title = "Create";
        //    return View();
        //}

        //// POST: Categories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        ////[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateBankStatementBL(Acc_BankStatementBalance Acc_BankStatementBalances)
        //{
        //    var errors = ModelState.Where(x => x.Value.Errors.Any())
        //    .Select(x => new { x.Key, x.Value.Errors });

        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    try
        //    {

        //        if (Acc_BankStatementBalances.BankStatementBalanceId > 0)
        //        {
        //            Acc_BankStatementBalances.DateUpdated = DateTime.Now;
        //            Acc_BankStatementBalances.UpdateByUserId = userid;
        //            _bankStatementBLRepository.Update(Acc_BankStatementBalances);

        //            TempData["Message"] = "Data Update Successfully";
        //            TempData["Status"] = "2";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_BankStatementBalances.AccId.ToString(), "Update", Acc_BankStatementBalances.Acc_ChartOfAccount.ToString());

        //        }
        //        else
        //        {
        //            Acc_BankStatementBalances.ComId = comid;
        //            Acc_BankStatementBalances.UserId = userid;
        //            Acc_BankStatementBalances.DateAdded = DateTime.Now;
        //            _bankStatementBLRepository.Add(Acc_BankStatementBalances);
        //            TempData["Message"] = "Data Save Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_BankStatementBalances.AccId.ToString(), "Update", Acc_BankStatementBalances.Acc_ChartOfAccount.ToString());
        //            return RedirectToAction("BankStatementBLList");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        _logger.LogError(ex.InnerException.Message);
        //    }

        //    return RedirectToAction("BankStatementBLList");

        //}


        ////[Authorize]
        //// GET: Categories/Edit/5
        //public ActionResult EditBankStatementBL(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_BankStatementBalance Acc_BankStatementBalance = _bankStatementBLRepository.FindById(id);

        //    ViewBag.AccId = _bankStatementBLRepository.AccId();
        //    if (Acc_BankStatementBalance == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Title = "Edit";

        //    return View("CreateBankStatementBL", Acc_BankStatementBalance);

        //}


        ////[Authorize]
        //// GET: Categories/Delete/5
        //public ActionResult DeleteBankStatementBL(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }

        //    Acc_BankStatementBalance Acc_BankStatementBalance = _bankStatementBLRepository.FindById(id);
        //    ViewBag.AccId = _bankStatementBLRepository.AccId();

        //    if (Acc_BankStatementBalance == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.Title = "Delete";

        //    return View("CreateBankStatementBL", Acc_BankStatementBalance);
        //}

        ////        //[Authorize]
        //// POST: Categories/Delete/5
        //[HttpPost, ActionName("DeleteBankStatementBL")]
        ////      [ValidateAntiForgeryToken]
        //public JsonResult DeleteBankStatementBLConfirmed(int? id)
        //{
        //    try
        //    {

        //        Acc_BankStatementBalance Acc_BankStatementBalance = _bankStatementBLRepository.FindById(id);
        //        _bankStatementBLRepository.Delete(Acc_BankStatementBalance);

        //        TempData["Message"] = "Data Deleted Successfully";
        //        TempData["Status"] = "3";
        //        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_BankStatementBalance.AccId.ToString(), "Update", Acc_BankStatementBalance.Acc_ChartOfAccount.ToString());

        //        return Json(new { Success = 1, BankStatementBalanceId = Acc_BankStatementBalance.BankStatementBalanceId, ex = "" });

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.InnerException.Message);
        //        return Json(new { Success = 0, ex = ex.Message.ToString() });

        //    }
        //}
        //#endregion

      

        //#region Account Voucher Type
        //public ActionResult VoucherTypeList()
        //{
        //    return View(_voucherTypeRepository.GetVoucherType());
        //}

        //public ActionResult CreateVoucherType()
        //{
        //    ViewBag.Title = "Create";
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateVoucherType(Acc_VoucherType Acc_VoucherType)
        //{
        //    var errors = ModelState.Where(x => x.Value.Errors.Any())
        //    .Select(x => new { x.Key, x.Value.Errors });
        //    //if (ModelState.IsValid)
        //    {
        //        if (Acc_VoucherType.VoucherTypeId > 0)
        //        {
        //            _voucherTypeRepository.Update(Acc_VoucherType);
        //            TempData["Message"] = "Data Update Successfully";
        //            TempData["Status"] = "2";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherType.VoucherTypeId.ToString(), "Update", Acc_VoucherType.VoucherTypeName.ToString());
        //        }
        //        else
        //        {
        //            _voucherTypeRepository.SaveVoucherType(Acc_VoucherType);
        //            TempData["Message"] = "Data Update Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherType.VoucherTypeId.ToString(), "Update", Acc_VoucherType.VoucherTypeName.ToString());
        //            return RedirectToAction("VoucherTypeList");
        //        }
        //    }
        //    return RedirectToAction("VoucherTypeList");
        //}

        //public ActionResult EditVoucherType(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_VoucherType Acc_VoucherType = _voucherTypeRepository.FindById(id);
        //    if (Acc_VoucherType == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Title = "Edit";

        //    return View("CreateVoucherType", Acc_VoucherType);

        //}

        //public ActionResult DeleteVoucherType(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_VoucherType Acc_VoucherType = _voucherTypeRepository.FindById(id);

        //    if (Acc_VoucherType == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.Title = "Delete";

        //    return View("CreateVoucherType", Acc_VoucherType);
        //}

        //[HttpPost, ActionName("DeleteVoucherType")]
        ////      [ValidateAntiForgeryToken]
        //public JsonResult DeleteVoucherTypeConfirmed(int? id)
        //{
        //    try
        //    {
        //        Acc_VoucherType Acc_VoucherType = _voucherTypeRepository.FindById(id);
        //        _voucherTypeRepository.Delete(Acc_VoucherType);
        //        TempData["Message"] = "Data Update Successfully";
        //        TempData["Status"] = "3";
        //        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherType.VoucherTypeId.ToString(), "Update", Acc_VoucherType.VoucherTypeName.ToString());
        //        return Json(new { Success = 1, VoucherTypeId = Acc_VoucherType.VoucherTypeId, ex = "" });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = 0, ex = ex.Message.ToString() });
        //    }

        //}
        //#endregion

        //#region Account Voucher No Prefix
        //public ActionResult VoucherNoPrefixList()
        //{

        //    List<Acc_VoucherNoPrefix> vouchernoprefix = _voucherNoPrefixRepository.GetVoucherNo();

        //    //return View(db.Acc_VoucherNoPrefixes.Where(c => c.VoucherNoPrefixId > 0).ToList());
        //    return View(vouchernoprefix);

        //}

        ////[Authorize]
        //// GET: Categories/Create
        //public ActionResult CreateVoucherNoPrefix()
        //{
        //    ViewBag.Title = "Create";
        //    ViewBag.VoucherTypeId = _voucherNoPrefixRepository.VoucherTypeList();

        //    return View();
        //}

        //// POST: Categories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        ////[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateVoucherNoPrefix(Acc_VoucherNoPrefix Acc_VoucherNoPrefix)
        //{
        //    var errors = ModelState.Where(x => x.Value.Errors.Any())
        //    .Select(x => new { x.Key, x.Value.Errors });
        //    ViewBag.VoucherTypeId = _voucherNoPrefixRepository.VoucherTypeList();

        //    HttpContext.Session.GetString("userid");

        //    //if (ModelState.IsValid)
        //    {
        //        if (Acc_VoucherNoPrefix.VoucherNoPrefixId > 0)
        //        {

        //            Acc_VoucherNoPrefix.DateUpdated = DateTime.Now;
        //            Acc_VoucherNoPrefix.ComId = HttpContext.Session.GetString("comid");
        //            if (Acc_VoucherNoPrefix.UserId == null)
        //            {
        //                Acc_VoucherNoPrefix.UserId = HttpContext.Session.GetString("userid");
        //            }
        //            Acc_VoucherNoPrefix.UpdateByUserId = HttpContext.Session.GetString("userid");
        //            _voucherNoPrefixRepository.Update(Acc_VoucherNoPrefix);
        //            TempData["Message"] = "Data Update Successfully";
        //            TempData["Status"] = "2";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherNoPrefix.VoucherNoPrefixId.ToString(), "Update", Acc_VoucherNoPrefix.VoucherShortPrefix.ToString());

        //        }
        //        else
        //        {
        //            Acc_VoucherNoPrefix.UserId = HttpContext.Session.GetString("userid");
        //            Acc_VoucherNoPrefix.ComId = HttpContext.Session.GetString("comid");
        //            Acc_VoucherNoPrefix.DateAdded = DateTime.Now;

        //            _voucherNoPrefixRepository.Add(Acc_VoucherNoPrefix);
        //            TempData["Message"] = "Data Save Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherNoPrefix.VoucherNoPrefixId.ToString(), "Update", Acc_VoucherNoPrefix.VoucherShortPrefix.ToString());
        //            //int id = Acc_VoucherNoPrefix.VoucherNoPrefixId; // Yes it's here


        //            //db.Categories.Add(Acc_VoucherNoPrefix);
        //            return RedirectToAction("VoucherNoPrefixList");
        //        }
        //    }
        //    return RedirectToAction("VoucherNoPrefixList");

        //    //return View(Acc_VoucherNoPrefix);
        //}


        ////[Authorize]
        //// GET: Categories/Edit/5
        //public ActionResult EditVoucherNoPrefix(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_VoucherNoPrefix Acc_VoucherNoPrefix = _voucherNoPrefixRepository.FindByIdVoucherNo(id);
        //    ViewBag.VoucherTypeId = _voucherNoPrefixRepository.VoucherTypeList();

        //    if (Acc_VoucherNoPrefix == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Title = "Edit";

        //    return View("CreateVoucherNoPrefix", Acc_VoucherNoPrefix);

        //}


        ////[Authorize]
        //// GET: Categories/Delete/5
        //public ActionResult DeleteVoucherNoPrefix(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    string comid = HttpContext.Session.GetString("comid");
        //    Acc_VoucherNoPrefix Acc_VoucherNoPrefix = _voucherNoPrefixRepository.FindByIdVoucherNo(id);
        //    ViewBag.VoucherTypeId = _voucherNoPrefixRepository.VoucherTypeList();

        //    if (Acc_VoucherNoPrefix == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.Title = "Delete";

        //    return View("CreateVoucherNoPrefix", Acc_VoucherNoPrefix);
        //}
        ////        //[Authorize]
        //// POST: Categories/Delete/5
        //[HttpPost, ActionName("DeleteVoucherNoPrefix")]
        ////      [ValidateAntiForgeryToken]
        //public JsonResult DeleteVoucherNoPrefixConfirmed(int? id)
        //{
        //    try
        //    {

        //        Acc_VoucherNoPrefix Acc_VoucherNoPrefix = _voucherNoPrefixRepository.FindByIdVoucherNo(id);
        //        _voucherNoPrefixRepository.Delete(Acc_VoucherNoPrefix);
        //        TempData["Message"] = "Data Delete Successfully";
        //        TempData["Status"] = "3";
        //        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherNoPrefix.VoucherNoPrefixId.ToString(), "Update", Acc_VoucherNoPrefix.VoucherShortPrefix.ToString());
        //        return Json(new { Success = 1, VoucherNoPrefixId = Acc_VoucherNoPrefix.VoucherNoPrefixId, ex = "" });

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Success = 0, ex = ex.Message.ToString() });

        //    }



        //    //return RedirectToAction("Index");
        //}
        //#endregion

        //#region Account Government Schedule Equity
        //public ViewResult GovtScheduleEquityList(string FromDate, string ToDate, string criteria)
        //{

        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //    DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //    List<Acc_GovtSchedule_Equity> abcd = new List<Acc_GovtSchedule_Equity>();

        //    if (FromDate == null || FromDate == "")
        //    {
        //        abcd = _govtScheduleEquityRepository.GetAccGovtScheduleList();
        //    }
        //    else
        //    {
        //        dtFrom = Convert.ToDateTime(FromDate);
        //        abcd = _govtScheduleEquityRepository.GetAccGovtScheduleList();
        //    }

        //    return View(abcd);

        //}

        ////[Authorize]
        //// GET: Categories/Create
        //public ActionResult CreateGovtScheduleEquity()
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    Acc_GovtSchedule_Equity abc = new Acc_GovtSchedule_Equity();
        //    var x = _govtScheduleEquityRepository.GetAll().OrderByDescending(x => x.GovtScheduleId).FirstOrDefault();
        //    if (x != null)
        //    {
        //        ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();

        //        this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();
        //        abc.FromDate = x.FromDate.AddYears(1);
        //        abc.ToDate = x.ToDate.AddYears(1);
        //        abc.Loan = x.Loan;
        //        abc.Development = x.Development;
        //        abc.CDVAT = x.CDVAT;
        //        abc.Description = x.Description;
        //        abc.AccId = x.AccId;

        //    }
        //    else
        //    {
        //        ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();

        //        this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //        abc.FromDate = DateTime.Now.Date;
        //        abc.ToDate = DateTime.Now.Date;

        //    }

        //    ViewBag.Title = "Create";
        //    return View(abc);
        //}

        //// POST: Categories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        ////[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateGovtScheduleEquity(Acc_GovtSchedule_Equity Acc_GovtSchedulevar)
        //{
        //    var errors = ModelState.Where(x => x.Value.Errors.Any())
        //    .Select(x => new { x.Key, x.Value.Errors });

        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    //if (ModelState.IsValid)
        //    {
        //        if (Acc_GovtSchedulevar.GovtScheduleId > 0)
        //        {

        //            if (Acc_GovtSchedulevar.ComId == null || Acc_GovtSchedulevar.ComId == "")
        //            {
        //                Acc_GovtSchedulevar.ComId = comid;

        //            }

        //            Acc_GovtSchedulevar.DateUpdated = DateTime.Now;
        //            Acc_GovtSchedulevar.UpdateByUserId = userid;
        //            _govtScheduleEquityRepository.Update(Acc_GovtSchedulevar);
        //            TempData["Message"] = "Data Update Successfully";
        //            TempData["Status"] = "2";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedulevar.GovtScheduleId.ToString(), "Update", Acc_GovtSchedulevar.Criteria.ToString());

        //        }
        //        else
        //        {
        //            Acc_GovtSchedulevar.DateAdded = DateTime.Now;
        //            Acc_GovtSchedulevar.UserId = userid;
        //            Acc_GovtSchedulevar.ComId = comid;
        //            _govtScheduleEquityRepository.Add(Acc_GovtSchedulevar);
        //            TempData["Message"] = "Data Save Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedulevar.GovtScheduleId.ToString(), "Update", Acc_GovtSchedulevar.Criteria.ToString());


        //        }
        //    }
        //    return RedirectToAction("Create");
        //}
        ////[Authorize]
        //// GET: Categories/Edit/5
        //public ActionResult EditGovtScheduleEquity(int? id)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_GovtSchedule_Equity Acc_GovtSchedule_Equity = _govtScheduleEquityRepository.FindById(id);


        //    if (Acc_GovtSchedule_Equity == null)
        //    {
        //        return NotFound();
        //    }


        //    ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //    ViewBag.Title = "Edit";

        //    return View("CreateGovtScheduleEquity", Acc_GovtSchedule_Equity);

        //}

        //public JsonResult SetSessionAccountReport(string rptFormat, string FromDate, string ToDate, int? AccId)
        //{
        //    try
        //    {
        //        string comid = HttpContext.Session.GetString("comid");
        //        string userid = HttpContext.Session.GetString("userid");

        //        var reportname = "";
        //        var filename = "";
        //        string redirectUrl = "";
        //        string query = "";
        //        if (true)
        //        {
        //            reportname = "rptGovtEquitySchedule";
        //            filename = "GovtEquitySchedule_" + DateTime.Now.Date;
        //            query = "Exec Acc_rptGovtEquity_Schedule '" + comid + "', '" + FromDate + "' ,'" + ToDate + "' ,'" + AccId + "'  ";

        //            HttpContext.Session.SetString("reportquery", query);
        //            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
        //        }

        //        HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
        //        string DataSourceName = "DataSet1";

        //        clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
        //        clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
        //        clsReport.strDSNMain = DataSourceName;

        //        redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
        //        return Json(new { Url = redirectUrl });

        //    }

        //    catch (Exception ex)
        //    {

        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }

        //    return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });

        //}

        //public ActionResult DeleteGovtScheduleEquity(int? id)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_GovtSchedule_Equity Acc_GovtSchedule_Equity = _govtScheduleEquityRepository.FindById(id);

        //    if (Acc_GovtSchedule_Equity == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();
        //    ViewBag.Title = "Delete";

        //    return View("CreateGovtScheduleEquity", Acc_GovtSchedule_Equity);
        //}

        //[HttpPost, ActionName("DeleteGovtScheduleEquity")]
        //public JsonResult DeleteGovtScheduleEquityConfirmed(int? id)
        //{
        //    try
        //    {
        //        Acc_GovtSchedule_Equity Acc_GovtSchedule_Equity = _govtScheduleEquityRepository.FindById(id);
        //        _govtScheduleEquityRepository.Delete(Acc_GovtSchedule_Equity);
        //        TempData["Message"] = "Data Delete Successfully";
        //        TempData["Status"] = "3";
        //        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedule_Equity.GovtScheduleId.ToString(), "Update", Acc_GovtSchedule_Equity.Criteria.ToString());

        //        return Json(new { Success = 1, GovtScheduleId = Acc_GovtSchedule_Equity.GovtScheduleId, ex = "" });

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Success = 0, ex = ex.Message.ToString() });

        //    }
        //}
        //#endregion

        //#region Account Government Schedule Foreign Loan
        //public ViewResult GovtScheduleForeignList(string FromDate, string ToDate, string criteria)
        //{

        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //    DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //    List<Acc_GovtSchedule_ForeignLoan> abcd = new List<Acc_GovtSchedule_ForeignLoan>();

        //    if (FromDate == null || FromDate == "")
        //    {
        //        abcd = _govtScheduleForeignRepository.GetAccGovtScheduleForeignList();
        //    }
        //    else
        //    {
        //        dtFrom = Convert.ToDateTime(FromDate);
        //        abcd = _govtScheduleForeignRepository.GetAccGovtScheduleForeignList();

        //    }

        //    return View(abcd);

        //}

        ////[Authorize]
        //// GET: Categories/Create
        //public ActionResult CreateGovtScheduleForeign()
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    Acc_GovtSchedule_ForeignLoan abc = new Acc_GovtSchedule_ForeignLoan();


        //    var x = _govtScheduleForeignRepository.GetAll().OrderByDescending(x => x.GovtScheduleId).FirstOrDefault();
        //    if (x != null)
        //    {
        //        ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();

        //        this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //        abc.FromDate = x.FromDate.AddYears(1);
        //        abc.ToDate = x.ToDate.AddYears(1);
        //        abc.DateOfPayment = x.DateOfPayment;

        //        abc.GroupByName = x.GroupByName;

        //        abc.MilestonePortionPrincipleRMB = 0;
        //        abc.MilestonePortionPrincipleTaka = 0;
        //        abc.ExchangeRate = 0;
        //        abc.ExchangeFlucLossGain = 0;
        //        abc.Total = 0;
        //        abc.Interest = x.Interest;
        //        abc.Description = x.Description;
        //        abc.AccId = x.AccId;

        //    }
        //    else
        //    {
        //        ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //        this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //        abc.FromDate = DateTime.Now.Date;
        //        abc.ToDate = DateTime.Now.Date;
        //    }

        //    ViewBag.Title = "Create";


        //    return View(abc);
        //}

        //// POST: Categories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        ////[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateGovtScheduleForeign(Acc_GovtSchedule_ForeignLoan Acc_GovtSchedulevar)
        //{
        //    var errors = ModelState.Where(x => x.Value.Errors.Any())
        //    .Select(x => new { x.Key, x.Value.Errors });

        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    //if (ModelState.IsValid)
        //    {
        //        if (Acc_GovtSchedulevar.GovtScheduleId > 0)
        //        {

        //            if (Acc_GovtSchedulevar.ComId == null || Acc_GovtSchedulevar.ComId == "")
        //            {
        //                Acc_GovtSchedulevar.ComId = comid;

        //            }

        //            Acc_GovtSchedulevar.DateUpdated = DateTime.Now;
        //            Acc_GovtSchedulevar.UpdateByUserId = userid;
        //            _govtScheduleForeignRepository.Update(Acc_GovtSchedulevar);
        //            TempData["Message"] = "Data Update Successfully";
        //            TempData["Status"] = "2";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedulevar.GovtScheduleId.ToString(), "Update", Acc_GovtSchedulevar.Criteria.ToString());
        //        }
        //        else
        //        {

        //            Acc_GovtSchedulevar.DateAdded = DateTime.Now;
        //            Acc_GovtSchedulevar.UserId = userid;
        //            Acc_GovtSchedulevar.ComId = comid;
        //            _govtScheduleForeignRepository.Add(Acc_GovtSchedulevar);
        //            TempData["Message"] = "Data Save Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedulevar.GovtScheduleId.ToString(), "Update", Acc_GovtSchedulevar.Criteria.ToString());

        //        }
        //    }
        //    return RedirectToAction("CreateGovtScheduleForeign");

        //}

        ////[Authorize]
        //// GET: Categories/Edit/5
        //public ActionResult EditGovtScheduleForeign(int? id)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_GovtSchedule_ForeignLoan Acc_GovtSchedule_ForeignLoan = _govtScheduleForeignRepository.FindById(id);


        //    if (Acc_GovtSchedule_ForeignLoan == null)
        //    {
        //        return NotFound();
        //    }


        //    ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //    ViewBag.Title = "Edit";

        //    return View("CreateGovtScheduleForeign", Acc_GovtSchedule_ForeignLoan);

        //}

        //public JsonResult SetSessionAccountReportGovtScheduleForeign(string rptFormat, string FromDate, string ToDate, int? AccId)
        //{
        //    try
        //    {
        //        string comid = HttpContext.Session.GetString("comid");
        //        string userid = HttpContext.Session.GetString("userid");

        //        var reportname = "";
        //        var filename = "";
        //        string redirectUrl = "";
        //        string query = "";
        //        //return Json(new { Success = 1, TermsId = param, ex = "" });
        //        if (true)
        //        {
        //            //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
        //            reportname = "rptGovtForeignLoanSchedule";
        //            filename = "rptGovtForeignLoanSchedule_" + DateTime.Now.Date;
        //            query = "Exec Acc_rptForeignLoan_Schedule '" + comid + "', '" + FromDate + "' ,'" + ToDate + "' ,'" + AccId + "'  ";


        //            HttpContext.Session.SetString("reportquery", query);
        //            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
        //        }



        //        HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


        //        string DataSourceName = "DataSet1";

        //        //HttpContext.Session.SetObject("Acc_rptList", postData);

        //        //Common.Classes.clsMain.intHasSubReport = 0;
        //        clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
        //        clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
        //        clsReport.strDSNMain = DataSourceName;

        //        //var ConstrName = "ApplicationServices";
        //        //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
        //        //redirectUrl = callBackUrl;


        //        redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
        //        return Json(new { Url = redirectUrl });

        //    }

        //    catch (Exception ex)
        //    {
        //        // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }

        //    return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        //    //return RedirectToAction("Index");

        //}



        ////[Authorize]
        //// GET: Categories/Delete/5
        //public ActionResult DeleteGovtScheduleForeign(int? id)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_GovtSchedule_ForeignLoan Acc_GovtSchedule_ForeignLoan = _govtScheduleForeignRepository.FindById(id);


        //    if (Acc_GovtSchedule_ForeignLoan == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();


        //    ViewBag.Title = "Delete";

        //    return View("CreateGovtScheduleForeign", Acc_GovtSchedule_ForeignLoan);
        //}
        ////        //[Authorize]
        //// POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        ////      [ValidateAntiForgeryToken]
        //public JsonResult DeleteGovtScheduleForeignConfirmed(int? id)
        //{
        //    try
        //    {

        //        Acc_GovtSchedule_ForeignLoan Acc_GovtSchedule_ForeignLoan = _govtScheduleForeignRepository.FindById(id);
        //        _govtScheduleForeignRepository.Delete(Acc_GovtSchedule_ForeignLoan);
        //        TempData["Message"] = "Data Delete Successfully";
        //        TempData["Status"] = "3";
        //        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedule_ForeignLoan.GovtScheduleId.ToString(), "Update", Acc_GovtSchedule_ForeignLoan.Criteria.ToString());
        //        return Json(new { Success = 1, GovtScheduleId = Acc_GovtSchedule_ForeignLoan.GovtScheduleId, ex = "" });

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Success = 0, ex = ex.Message.ToString() });

        //    }
        //}

        //#endregion

        //#region Account Government Schedule Japan Loan
        ////[Authorize]
        //// GET: Categories
        //public ViewResult GovtJapanLoanList(string FromDate, string ToDate, string criteria)
        //{

        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //    DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //    List<Acc_GovtSchedule_JapanLoan> abcd = new List<Acc_GovtSchedule_JapanLoan>();

        //    if (FromDate == null || FromDate == "")
        //    {

        //        abcd = _govtScheduleJapanLoanRepository.GetAccGovtScheduleJapanLoanList();

        //    }
        //    else
        //    {
        //        dtFrom = Convert.ToDateTime(FromDate);
        //        abcd = _govtScheduleJapanLoanRepository.GetAccGovtScheduleJapanLoanList();

        //    }

        //    return View(abcd);

        //}

        ////[Authorize]
        //// GET: Categories/Create
        //public ActionResult CreateGovtJapanLoan()
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    Acc_GovtSchedule_JapanLoan abc = new Acc_GovtSchedule_JapanLoan();


        //    var x = _govtScheduleJapanLoanRepository.GetAll().OrderByDescending(x => x.GovtScheduleId).FirstOrDefault();
        //    if (x != null)
        //    {
        //        ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //        this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //        abc.FromDate = x.FromDate;
        //        abc.ToDate = x.ToDate;
        //        abc.DateOfPayment = x.DateOfPayment;

        //        abc.GroupByName = x.GroupByName;
        //        abc.PortionType = x.PortionType;



        //        abc.InterestPortionYen = 0;
        //        abc.MilestonePortionYen = 0;
        //        abc.SupplyPortionYen = 0;
        //        abc.PaymentPortionYen = 0;

        //        abc.TotalAmountYen = 0;

        //        abc.InterestPortionTaka = 0;
        //        abc.MilestonePortionTaka = 0;
        //        abc.SupplyPortionTaka = 0;
        //        abc.PaymentPortionTaka = 0;

        //        abc.TotalAmountTaka = 0;

        //        abc.ExchangeLossGainTaka = 0;
        //        abc.ExchangeRate = 0;


        //        abc.Description = x.Description;
        //        abc.AccId = x.AccId;

        //    }
        //    else
        //    {
        //        ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //        this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();
        //        abc.FromDate = DateTime.Now.Date;
        //        abc.ToDate = DateTime.Now.Date;
        //    }

        //    ViewBag.Title = "Create";


        //    return View(abc);
        //}

        //// POST: Categories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        ////[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateGovtJapanLoan(Acc_GovtSchedule_JapanLoan Acc_GovtSchedulevar)
        //{
        //    var errors = ModelState.Where(x => x.Value.Errors.Any())
        //    .Select(x => new { x.Key, x.Value.Errors });

        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");

        //    //if (ModelState.IsValid)
        //    {
        //        if (Acc_GovtSchedulevar.GovtScheduleId > 0)
        //        {

        //            if (Acc_GovtSchedulevar.ComId == null || Acc_GovtSchedulevar.ComId == "")
        //            {
        //                Acc_GovtSchedulevar.ComId = comid;

        //            }

        //            Acc_GovtSchedulevar.DateUpdated = DateTime.Now;
        //            Acc_GovtSchedulevar.UpdateByUserId = userid;
        //            _govtScheduleJapanLoanRepository.Update(Acc_GovtSchedulevar);
        //            TempData["Message"] = "Data Update Successfully";
        //            TempData["Status"] = "2";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedulevar.GovtScheduleId.ToString(), "Update", Acc_GovtSchedulevar.Criteria.ToString());
        //        }
        //        else
        //        {

        //            Acc_GovtSchedulevar.DateAdded = DateTime.Now;
        //            Acc_GovtSchedulevar.UserId = userid;
        //            Acc_GovtSchedulevar.ComId = comid;
        //            _govtScheduleJapanLoanRepository.Add(Acc_GovtSchedulevar);
        //            TempData["Message"] = "Data Save Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedulevar.GovtScheduleId.ToString(), "Update", Acc_GovtSchedulevar.Criteria.ToString());

        //        }
        //    }
        //    return RedirectToAction("CreateGovtJapanLoan");

        //}


        ////[Authorize]
        //// GET: Categories/Edit/5
        //public ActionResult EditGovtJapanLoan(int? id)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_GovtSchedule_JapanLoan Acc_GovtSchedule_JapanLoan = _govtScheduleJapanLoanRepository.FindById(id);

        //    if (Acc_GovtSchedule_JapanLoan == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();

        //    ViewBag.Title = "Edit";

        //    return View("CreateGovtJapanLoan", Acc_GovtSchedule_JapanLoan);

        //}




        //public JsonResult SetSessionAccountReportGovtJapanLoan(string rptFormat, string FromDate, string ToDate, int? AccId)
        //{
        //    try
        //    {
        //        string comid = HttpContext.Session.GetString("comid");
        //        string userid = HttpContext.Session.GetString("userid");

        //        var reportname = "";
        //        var filename = "";
        //        string redirectUrl = "";
        //        string query = "";
        //        //return Json(new { Success = 1, TermsId = param, ex = "" });
        //        if (true)
        //        {
        //            //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
        //            reportname = "rptJapanLoanSchedule";
        //            filename = "rptJapanLoanSchedule_" + DateTime.Now.Date;
        //            query = "Exec Acc_rptJapanLoan_Schedule '" + comid + "', '" + FromDate + "' ,'" + ToDate + "' ,'" + AccId + "'  ";


        //            HttpContext.Session.SetString("reportquery", query);
        //            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
        //        }

        //        HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

        //        string DataSourceName = "DataSet1";
        //        clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
        //        clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
        //        clsReport.strDSNMain = DataSourceName;

        //        redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
        //        return Json(new { Url = redirectUrl });

        //    }

        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }

        //    return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });

        //}

        ////[Authorize]
        //// GET: Categories/Delete/5
        //public ActionResult DeleteGovtJapanLoan(int? id)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Acc_GovtSchedule_JapanLoan Acc_GovtSchedule_JapanLoan = _govtScheduleJapanLoanRepository.FindById(id);


        //    if (Acc_GovtSchedule_JapanLoan == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //    this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();


        //    ViewBag.Title = "Delete";

        //    return View("CreateGovtJapanLoan", Acc_GovtSchedule_JapanLoan);
        //}
        ////        //[Authorize]
        //// POST: Categories/Delete/5
        //[HttpPost, ActionName("DeleteGovtJapanLoan")]
        ////      [ValidateAntiForgeryToken]
        //public JsonResult DeleteGovtJapanLoanConfirmed(int? id)
        //{
        //    try
        //    {

        //        Acc_GovtSchedule_JapanLoan Acc_GovtSchedule_JapanLoan = _govtScheduleJapanLoanRepository.FindById(id);
        //        _govtScheduleJapanLoanRepository.Delete(Acc_GovtSchedule_JapanLoan);
        //        TempData["Message"] = "Data Delete Successfully";
        //        TempData["Status"] = "3";
        //        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_GovtSchedule_JapanLoan.GovtScheduleId.ToString(), "Update", Acc_GovtSchedule_JapanLoan.Criteria.ToString());
        //        return Json(new { Success = 1, GovtScheduleId = Acc_GovtSchedule_JapanLoan.GovtScheduleId, ex = "" });

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Success = 0, ex = ex.Message.ToString() });

        //    }



        //    //return RedirectToAction("Index");
        //}
        //#endregion

        //#region Account Dashboard
        //[HttpGet]
        //public ActionResult BuyerWiseShipmentStatus(int? ComId, int? BuyerId, int? Year)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();
        //    //ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
        //    ViewBag.BuyerGroupId = _accountDashboard.BuyerGroupId();
        //    return View(); //p.ComId == AppData.AppData.intComId && 
        //}

        //[HttpGet]

        //public ActionResult GetOverDueShipment()
        //{
        //    var comid = HttpContext.Session.GetString("comid");

        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();
        //    ViewBag.BuyerGroupId = _accountDashboard.BuyerGroupId();
        //    return View();
        //}

        //[HttpGet]
        ////
        //public ActionResult MainDashboard(DateTime? FromDate, DateTime? ToDate)
        //{
        //    if (FromDate == null || ToDate == null)
        //    {
        //        FromDate = DateTime.Now.Date;
        //        ToDate = DateTime.Now.Date;

        //    }
        //    else
        //    {

        //        ViewBag.FromDate = FromDate;
        //        ViewBag.ToDate = ToDate;
        //    }
        //    var comid = HttpContext.Session.GetString("comid");

        //    //////1st 

        //    List<Dashboard1> CashPayment = _accountDashboard.CashPayment(FromDate, ToDate);
        //    ViewBag.CashPayment = CashPayment;

        //    /////2nd
        //    List<Dashboard1> CashReceipt = _accountDashboard.CashReceipt(FromDate, ToDate);
        //    ViewBag.CashReceipt = CashReceipt;

        //    //////  3  
        //    //SqlParameter[] parameters3 = new SqlParameter[4];
        //    List<Dashboard1> BankPayment = _accountDashboard.BankPayment(FromDate, ToDate);
        //    ViewBag.BankPayment = BankPayment;


        //    //////  4 
        //    //SqlParameter[] parameters4 = new SqlParameter[4];

        //    List<Dashboard1> BankReceipt = _accountDashboard.BankReceipt(FromDate, ToDate);
        //    ViewBag.BankReceipt = BankReceipt;


        //    //////  5 
        //    //SqlParameter[] parameters5 = new SqlParameter[4];


        //    List<Dashboard1> Contra = _accountDashboard.Contra(FromDate, ToDate);
        //    ViewBag.Contra = Contra;


        //    //////  6
        //    //SqlParameter[] parameters6 = new SqlParameter[4];

        //    List<Dashboard1> Journal = _accountDashboard.Journal(FromDate, ToDate);
        //    ViewBag.Journal = Journal;





        //    //////  88 s
        //    //SqlParameter[] parameters8 = new SqlParameter[4];

        //    List<Dashboard2> MonthWiseCashReceipt = _accountDashboard.MonthWiseCashReceipt(FromDate, ToDate);


        //    //////  9 s
        //    //SqlParameter[] parameters9 = new SqlParameter[4];


        //    List<Dashboard2> MonthWiseCashPayment = _accountDashboard.MonthWiseCashPayment(FromDate, ToDate);


        //    //////  10 s
        //    //SqlParameter[] parameters10 = new SqlParameter[4];

        //    List<Dashboard2> MonthWiseBankReceipt = _accountDashboard.MonthWiseBankReceipt(FromDate, ToDate);



        //    //////  11 s
        //    //SqlParameter[] parameters11 = new SqlParameter[4];

        //    List<Dashboard2> MonthWiseBankPayment = _accountDashboard.MonthWiseBankPayment(FromDate, ToDate);
        //    //////  12 s
        //    //SqlParameter[] parameters12 = new SqlParameter[4];
        //    List<Dashboard2> MonthWiseContra = _accountDashboard.MonthWiseContra(FromDate, ToDate);

        //    //////  12 s
        //    //SqlParameter[] parameters12 = new SqlParameter[4];


        //    List<Dashboard2> MonthWiseJournal = _accountDashboard.MonthWiseJournal(FromDate, ToDate);



        //    //SqlParameter[] parameters14 = new SqlParameter[4];


        //    List<Dashboard3> MonthUserWiseAllVoucher = _accountDashboard.MonthUserWiseAllVoucher(FromDate, ToDate);

        //    //////  14 s
        //    //SqlParameter[] parameters14 = new SqlParameter[4];


        //    List<Dashboard3> MonthUserWiseCashPayment = _accountDashboard.MonthUserWiseCashPayment(FromDate, ToDate);

        //    //////  15 s
        //    //SqlParameter[] parameters15 = new SqlParameter[4];

        //    List<Dashboard3> MonthUserWiseCashReceipt = _accountDashboard.MonthUserWiseCashReceipt(FromDate, ToDate);



        //    //////  16 s
        //    //SqlParameter[] parameters16 = new SqlParameter[4];


        //    List<Dashboard3> MonthUserWiseBankPayment = _accountDashboard.MonthUserWiseBankPayment(FromDate, ToDate);

        //    //////  17 s
        //    //SqlParameter[] parameters17 = new SqlParameter[4];

        //    List<Dashboard3> MonthUserWiseBankReceipt = _accountDashboard.MonthUserWiseBankReceipt(FromDate, ToDate);



        //    //////  18 s
        //    //SqlParameter[] parameters18 = new SqlParameter[4];


        //    List<Dashboard3> MonthUserWiseContra = _accountDashboard.MonthUserWiseContra(FromDate, ToDate);


        //    //SqlParameter[] parameters199 = new SqlParameter[4];


        //    List<Dashboard3> MonthUserWiseJournal = _accountDashboard.MonthUserWiseJournal(FromDate, ToDate);

        //    //////  19 s
        //    //SqlParameter[] parameters19 = new SqlParameter[4];

        //    List<TopTransaction> TopTransactionCP = _accountDashboard.TopTransaction(FromDate, ToDate);
        //    ViewBag.TopTransactionCashPayment = TopTransactionCP.ToList();


        //    //////  20 s
        //    //SqlParameter[] parameters20= new SqlParameter[4];


        //    List<TopTransaction> TopTransactionCR = _accountDashboard.TopTransactionCR(FromDate, ToDate);
        //    ViewBag.TopTransactionCashReceipt = TopTransactionCR;


        //    //////  21 s
        //    //SqlParameter[] parameters21 = new SqlParameter[4];


        //    List<TopTransaction> TopTransactionBP = _accountDashboard.TopTransactionBP(FromDate, ToDate);
        //    ViewBag.TopTransactionBankPayment = TopTransactionBP;

        //    //////  22 s
        //    //SqlParameter[] parameters22 = new SqlParameter[4];

        //    List<TopTransaction> TopTransactionBR = _accountDashboard.TopTransactionBR(FromDate, ToDate);
        //    ViewBag.TopTransactionBankReceipt = TopTransactionBR;


        //    //////  23 s
        //    //SqlParameter[] parameters23 = new SqlParameter[4];

        //    List<TopTransaction> TopTransactionContra = _accountDashboard.TopTransactionContra(FromDate, ToDate);
        //    ViewBag.TopTransactionContra = TopTransactionContra;


        //    //////  23 s
        //    //SqlParameter[] parameters233 = new SqlParameter[4];


        //    List<TopTransaction> TopTransactionJournal = _accountDashboard.TopTransactionJournal(FromDate, ToDate);
        //    ViewBag.TopTransactionJournal = TopTransactionJournal;





        //    //////  24 s
        //    //SqlParameter[] parameters24 = new SqlParameter[4];


        //    List<UserLogingInfoes> TopLogin = _accountDashboard.TopLogin(FromDate, ToDate);
        //    ViewBag.TopLogin = TopLogin;





        //    //////  25 s
        //    //SqlParameter[] parameters25 = new SqlParameter[4];

        //    List<UserCountDocumentLastTransaction> VoucherUserCount = _accountDashboard.VoucherUserCount(FromDate, ToDate);
        //    ViewBag.VoucherUserCount = VoucherUserCount;



        //    //////  26 s
        //    //SqlParameter[] parameters26 = new SqlParameter[4];


        //    List<TopTransaction> AllUserTransaction = _accountDashboard.AllUserTransaction(FromDate, ToDate);
        //    ViewBag.AllUserTransaction = AllUserTransaction;


        //    //////  7 s
        //    //SqlParameter[] parameters7 = new SqlParameter[4];


        //    List<UserLogingInfoes> UserLogingInfoes = _accountDashboard.UserLogingInfoes(FromDate, ToDate);
        //    ViewBag.UserLogingInfoe = UserLogingInfoes.ToList();


        //    //////  8
        //    //SqlParameter[] parameters66 = new SqlParameter[4];

        //    List<LastTransactions> LastTransactions = _accountDashboard.LastTransactions(FromDate, ToDate);
        //    ViewBag.LastTransactions = LastTransactions.ToList();


        //    //////  27 s

        //    List<Dashboard4> DayWiseVoucher = _accountDashboard.DayWiseVoucher(FromDate, ToDate);

        //    /////Post Voucher
        //    List<Dashboard1> PostVoucher = _accountDashboard.PostVoucher(FromDate, ToDate);
        //    ViewBag.PostVoucher = PostVoucher;


        //    /////UnPost Voucher
        //    List<Dashboard1> UnPostVoucher = _accountDashboard.UnPostVoucher(FromDate, ToDate);

        //    ViewBag.UnPostVoucher = UnPostVoucher;


        //    /////Total Debit
        //    List<Dashboard1> TotalDebit = _accountDashboard.TotalDebit(FromDate, ToDate);
        //    ViewBag.TotalDebit = TotalDebit;


        //    /////Total Credit
        //    List<Dashboard1> TotalCredit = _accountDashboard.TotalCredit(FromDate, ToDate);
        //    ViewBag.TotalCredit = TotalCredit;



        //    /////Total Cash Balance
        //    List<Dashboard1> TotalCashBalance = _accountDashboard.TotalCashBalance(FromDate, ToDate);
        //    ViewBag.TotalCashBalance = TotalCashBalance;

        //    /////Total Bank Balance
        //    List<Dashboard1> TotalBankBalance = _accountDashboard.TotalBankBalance(FromDate, ToDate);
        //    ViewBag.TotalBankBalance = TotalBankBalance;

        //    //////  19 s
        //    //SqlParameter[] parameters19 = new SqlParameter[4];


        //    List<LedgerBalance> CashBalanceList = _accountDashboard.CashBalanceList(FromDate, ToDate);
        //    ViewBag.CashBalanceList = CashBalanceList.ToList();


        //    //////  20 s
        //    //SqlParameter[] parameters20= new SqlParameter[4];


        //    List<LedgerBalance> BankBalanceList = _accountDashboard.BankBalanceList(FromDate, ToDate);
        //    ViewBag.BankBalanceList = BankBalanceList;


        //    //////  21 s
        //    //SqlParameter[] parameters21 = new SqlParameter[4];

        //    List<LedgerBalance> AccountsPayableBalanceList = _accountDashboard.AccountsPayableBalanceList(FromDate, ToDate);
        //    ViewBag.AccountsPayableBalanceList = AccountsPayableBalanceList;

        //    //////  22 s
        //    //SqlParameter[] parameters22 = new SqlParameter[4];


        //    List<LedgerBalance> AccountsReceivableBalanceList = _accountDashboard.AccountsReceivableBalanceList(FromDate, ToDate);
        //    ViewBag.AccountsReceivableBalanceList = AccountsReceivableBalanceList;


        //    //////  23 s
        //    //SqlParameter[] parameters23 = new SqlParameter[4];

        //    List<LedgerBalance> FixedDepostiBalanceList = _accountDashboard.FixedDepostiBalanceList(FromDate, ToDate);
        //    ViewBag.FixedDepostiBalanceList = FixedDepostiBalanceList;


        //    //////  23 s
        //    //SqlParameter[] parameters233 = new SqlParameter[4];

        //    List<LedgerBalance> FixedAssetBalanceList = _accountDashboard.FixedAssetBalanceList(FromDate, ToDate);
        //    ViewBag.FixedAssetBalanceList = FixedAssetBalanceList;


        //    ViewBag.DashboardData = new
        //    {
        //        TotalCashReceipt = MonthWiseCashReceipt,
        //        TotalCashPayment = MonthWiseCashPayment,
        //        TotalBankReceipt = MonthWiseBankReceipt,
        //        TotalBankPayment = MonthWiseBankPayment,
        //        TotalContra = MonthWiseContra,
        //        TotalJournal = MonthWiseJournal,

        //        MonthUserWiseAllVoucher = MonthUserWiseAllVoucher,
        //        MonthUserWiseCashPayment = MonthUserWiseCashPayment,
        //        MonthUserWiseCashReceipt = MonthUserWiseCashReceipt,
        //        MonthUserWiseBankPayment = MonthUserWiseBankPayment,
        //        MonthUserWiseBankReceipt = MonthUserWiseBankReceipt,
        //        MonthUserWiseContra = MonthUserWiseContra,
        //        MonthUserWiseJournal = MonthUserWiseJournal,

        //        DayWiseVoucher = DayWiseVoucher,

        //    };


        //    return View();
        //}

        //[HttpGet]

        //public ActionResult GatePassDetailsChart()
        //{
        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();
        //    return View();
        //}


        //[HttpGet]

        //public ActionResult GroupLCChart()
        //{
        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();
        //    return View();
        //}


        //[HttpGet]

        //public ActionResult rptDashboardMarginAlert()
        //{
        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();
        //    return View();
        //}

        //[HttpGet]

        //public ActionResult rptDashboardSupplierBillMaturityOverdue()
        //{
        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();

        //    return View();
        //}

        //[HttpGet]

        //public ActionResult rptDashboardBillCreatePending()
        //{
        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();

        //    return View();
        //}

        //[HttpGet]

        //public ActionResult rptForthComingShipment()
        //{
        //    ViewBag.SisterConcernCompanyId = _accountDashboard.SisterConcernCompanyId();

        //    return View();
        //}

        //#endregion

        #region PF Process
        public JsonResult GetMonthList(int? id)
        {


            string comid = HttpContext.Session.GetString("comid");

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;
            if (id == null)
            {
                List<PF_FiscalYear> fiscalyear = _PFProcessRepository.FiscalYear();
                id = fiscalyear.Max(p => p.FYId);
            }

            List<PF_FiscalMonth> PF_FiscalMonth = _PFProcessRepository.PF_FiscalMonth(id);
            //&& db.HR_ProcessLock.Contains(x.FiscalMonthId) && db.HR_ProcessLock.Where(p=> p.IsLock==true && p.LockType.Contains("Account Lock"))).ToList();
            List<fymonthclass> data = new List<fymonthclass>();

            int i = 0;
            foreach (PF_FiscalMonth item in PF_FiscalMonth)
            {
                fymonthclass asdf = new fymonthclass
                {
                    //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
                    isCheck = i++,
                    MonthId = item.MonthId,
                    MonthName = item.MonthName, //DateTime.Parse(item.InvoiceDate.ToString()).ToString("dd-MMM-yy");
                    dtFrom = item.dtFrom,
                    dtTo = item.dtTo
                };


                data.Add(asdf);
            }

            return Json(data);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetProcess(string[] monthid, string criteria, int? Currency, string FYId, string MinAccCode, string MaxAccCode)
        {

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            _PFProcessRepository.SetProcess(monthid, criteria, Currency, FYId, MinAccCode, MaxAccCode);

            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), "Set Process", criteria, "Update", criteria);

            if (AppData.AppData.globalException.Length > 0)
            {
                return Json(AppData.AppData.globalException);
            }

            var data = "abcd";
            return Json(data = "1");
            ////return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult PFProcessList()
        {
            //var gTRDBContext = db.HR_Emp_Info.Include(h => h.Cat_BloodGroup).Include(h => h.Cat_Department).Include(h => h.Cat_Designation).Include(h => h.Cat_Floor).Include(h => h.Cat_Grade).Include(h => h.Cat_Line).Include(h => h.Cat_Religion).Include(h => h.Cat_Section).Include(h => h.Cat_Shift).Include(h => h.Cat_SubSection).Include(h => h.Cat_Unit);
            string comid = HttpContext.Session.GetString("comid");
            int defaultcountry = _PFProcessRepository.DefaultCountry();
            ViewBag.CountryId = _PFProcessRepository.CountryId();

            PF_PFProcessViewModel model = new PF_PFProcessViewModel();
            List<PF_FiscalYear> fiscalyear = _PFProcessRepository.FiscalYear();
            int fiscalyid = fiscalyear.Max(p => p.FYId);

            List<PF_FiscalMonth> fiscalmonth = _PFProcessRepository.FiscalMonth();

            model.ProcessFYs = fiscalyear;
            model.ProcessMonths = fiscalmonth;
            model.CountryId = defaultcountry;

            TempData["Status"] = "2";
            TempData["Message"] = "PF Processing Successfully";
            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", ""); /// transaction log when enable it throw error

            return View(model);

        }

        // GET: Section


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PFProcessList(PF_PFProcessViewModel model, string criteria, string command)
        {
            try
            {
                if (command != "Save")
                {
                    if (command == "0")
                    {
                        string comid = HttpContext.Session.GetString("comid");
                        int defaultcountry = _PFProcessRepository.DefaultCountry();

                        ViewBag.CountryId = _PFProcessRepository.CountryId();

                        List<PF_FiscalYear> fiscalyear = _PFProcessRepository.FiscalYear();
                        int fiscalyid = fiscalyear.Max(p => p.FYId);

                        List<PF_FiscalMonth> fiscalmonth = _PFProcessRepository.FiscalMonth();

                        model.ProcessFYs = fiscalyear;
                        model.ProcessMonths = fiscalmonth;
                        model.CountryId = defaultcountry;
                    }
                    else
                    {
                        int fiscalyid = int.Parse(command);

                        List<PF_FiscalMonth> fiscalmonth = _PFProcessRepository.FiscalMonth();
                        ViewBag.CountryId = _PFProcessRepository.CountryId();

                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var values = prcDataSave(model, criteria);
                        return View(model);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string prcDataSave(PF_PFProcessViewModel model, string criteria)
        {

            try
            {
                _PFProcessRepository.prcDataSave(model, criteria);
                //clsCon.GTRSaveDataWithSQLCommand(arQuery);
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), "Process data", criteria, "Save", criteria);

                return "Process Completed Successfully";
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                //clsCon = null;
            }
        }
        #endregion

        //        #region Budget


        //        // GET: BudgetMains
        //        public IActionResult BudgetList()
        //        {
        //            TempData["Message"] = "Data Load Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
        //            return View(_budgetRepository.GetAll());
        //        }

        //        // GET: BudgetMains/Details/5
        //        public IActionResult BudgetDataLoadByParameter(int? Yearid, int? Monthid, int? ParentId)
        //        {
        //            try
        //            {
        //                var result = "";

        //                List<Acc_BudgetData> Acc_BudgetData = _budgetRepository.BudgetDataLoadByParameter(Yearid, Monthid, ParentId);

        //                return Json(new { Acc_BudgetData, ex = result });
        //            }
        //            catch (Exception ex)
        //            {

        //                throw ex;
        //            }
        //        }


        //        public ActionResult Print(int? id, string type)
        //        {

        //            string redirectUrl = _budgetRepository.Print(id, type);
        //            return Redirect(redirectUrl);

        //        }



        //        [HttpPost]
        //        public IActionResult GetMonthByYear(int? FYId)
        //        {
        //            var Month = _context.Acc_FiscalMonths.Where(m => m.FYId == FYId).ToList();
        //            List<SelectListItem> items = new List<SelectListItem>();
        //            if (Month != null)
        //            {
        //                foreach (var item in Month)
        //                {
        //                    items.Add(new SelectListItem { Value = item.FiscalMonthId.ToString(), Text = item.MonthName });
        //                }
        //            }
        //            return Json(new SelectList(items, "Value", "Text"));
        //        }



        //        // GET: BudgetMains/Create
        //        public IActionResult CreateBudget()
        //        {
        //            ViewBag.Title = "Create";
        //            var comid = HttpContext.Session.GetString("comid");


        //            var lastBudgetMainsdata = _context.BudgetMains.Take(1).Where(x => x.ComId == comid).OrderByDescending(x => x.BudgetMainId).FirstOrDefault();
        //            if (lastBudgetMainsdata != null)
        //            {
        //                var sampleBudgetMainsdata = new BudgetMain();

        //                ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", lastBudgetMainsdata.FiscalYearId);
        //                ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.FYId == lastBudgetMainsdata.FiscalYearId), "FiscalMonthId", "MonthName", lastBudgetMainsdata.FiscalMonthId);
        //                ViewBag.ParentId = new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.ComId == comid && x.AccType == "G"), "AccId", "AccName");

        //                return View();
        //            }

        //            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
        //            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
        //            ViewBag.ParentId = new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.ComId == comid && x.AccType == "G"), "AccId", "AccName");

        //            return View();
        //        }


        //        // POST: BudgetMains/Create
        //        [HttpPost]
        //        public IActionResult CreateBudget(BudgetMain BudgetMains)
        //        {
        //            try
        //            {
        //                var message = "";
        //                var result = "";
        //                var comid = HttpContext.Session.GetString("comid");
        //                var userid = HttpContext.Session.GetString("userid");


        //                var BudgetMainId = _context.BudgetMains.Where(x => x.ComId == comid && x.FiscalYearId == BudgetMains.FiscalYearId).Select(x => x.BudgetMainId).FirstOrDefault();
        //                BudgetMains.BudgetMainId = BudgetMainId;

        //                if (BudgetMains.BudgetMainId > 0)
        //                {

        //                    foreach (var item in BudgetMains.BudgetDetails)
        //                    {


        //                        if (item.BudgetDetailsId > 0)
        //                        {

        //                            item.DateUpdated = DateTime.Now;
        //                            item.UpdateByUserId = HttpContext.Session.GetString("userid");
        //                            _budgetRepository.Update(item);
        //                            TempData["Message"] = "Data Update Successfully";
        //                            TempData["Status"] = "2";
        //                            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), item.BudgetMainId.ToString(), "Update", item.AccId + " " + item.BudgetMainId);

        //                        }
        //                        else
        //                        {
        //                            _context.Add(item);
        //                            TempData["Message"] = "Data Save Successfully.";
        //                            TempData["Status"] = "1";
        //                        }
        //                        _context.SaveChanges();
        //                        TempData["Message"] = "Data Update Successfully";
        //                        TempData["Status"] = "2";
        //                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), item.BudgetMainId.ToString(), "Update", BudgetMains.FiscalYearId + " " + BudgetMains.FiscalMonthId);

        //                    }


        //                    var TotalDebitAmount = _context.BudgetDetails.Where(x => x.BudgetMainId == BudgetMainId).Sum(x => x.BudgetDebit);
        //                    var TotalCreditAmount = _context.BudgetDetails.Where(x => x.BudgetMainId == BudgetMainId).Sum(x => x.BudgetCredit);



        //                    BudgetMain abc = _context.BudgetMains.Where(x => x.BudgetMainId == BudgetMainId).FirstOrDefault();

        //                    abc.TotalBudgetDebit = TotalDebitAmount;
        //                    abc.TotalBudgetCredit = TotalCreditAmount;
        //                    abc.UpdateByUserId = userid;
        //                    abc.DateUpdated = DateTime.Now;


        //                    _context.BudgetMains.Attach(abc);
        //                    _context.Entry(abc).Property(x => x.TotalBudgetDebit).IsModified = true;
        //                    _context.Entry(abc).Property(x => x.TotalBudgetCredit).IsModified = true;
        //                    _context.Entry(abc).Property(x => x.DateUpdated).IsModified = true;
        //                    _context.Entry(abc).Property(x => x.UpdateByUserId).IsModified = true;

        //                    _context.SaveChanges();

        //                }
        //                else
        //                {
        //                    BudgetMains.UserId = userid;
        //                    BudgetMains.ComId = comid;
        //                    BudgetMains.DateAdded = DateTime.Now;


        //                    _context.BudgetMains.Add(BudgetMains);
        //                    _context.SaveChanges();

        //                }

        //                return Json(new { Success = 1, ex = TempData["Message"] });

        //            }
        //            catch (Exception ex)
        //            {

        //                //throw ex;
        //                return Json(new { Success = false, ex = ex });

        //            }

        //        }

        //        // GET: BudgetMains/Edit/5
        //        public async Task<IActionResult> Edit(int? id)
        //        {

        //            if (id == null)
        //            {
        //                return NotFound();
        //            }
        //            var comid = HttpContext.Session.GetString("comid");

        //            var BudgetMains = await _context.BudgetMains
        //                .Include(y => y.YearName)
        //                .Include(m => m.MonthName)
        //                .Where(b => b.BudgetMainId == id).FirstOrDefaultAsync();
        //            if (BudgetMains == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Edit";
        //            ViewBag.FiscalYearId = _budgetRepository.FiscalYearId(id);
        //            ViewBag.FiscalMonthId = _budgetRepository.FiscalMonthId(id);
        //            ViewBag.ParentId = _budgetRepository.ParentId();


        //            return View("CreateBudget", BudgetMains);
        //        }

        //        // POST: BudgetMains/Edit/5
        //        [HttpPost]
        //        public IActionResult Edit(int id, BudgetDetails BudgetDetails)
        //        {
        //            if (id != BudgetDetails.BudgetDetailsId)
        //            {
        //                return NotFound();
        //            }

        //            if (ModelState.IsValid)
        //            {
        //                try
        //                {
        //                    _budgetRepository.Edit(id, BudgetDetails);
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    if (!BudgetMainsExists(BudgetDetails.BudgetDetailsId))
        //                    {
        //                        return NotFound();
        //                    }
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //                return RedirectToAction(nameof(BudgetList));
        //            }
        //            return View(BudgetDetails);
        //        }

        //        // GET: BudgetMains/Delete/5
        //        public async Task<IActionResult> Delete(int? id)
        //        {

        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var BudgetMains = await _context.BudgetMains
        //                .Include(y => y.YearName)
        //                .Include(m => m.MonthName)
        //                .Where(b => b.BudgetMainId == id).FirstOrDefaultAsync();
        //            if (BudgetMains == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Delete";
        //            ViewBag.FiscalYearId = _budgetRepository.FiscalYearId(id);
        //            ViewBag.FiscalMonthId = _budgetRepository.FiscalMonthId(id);

        //            return View("Delete", BudgetMains);
        //        }

        //        // POST: BudgetMains/Delete/5
        //        [HttpPost, ActionName("Delete")]
        //        public IActionResult DeleteConfirmed(int id)
        //        {
        //            _budgetRepository.Delete(id);
        //            return RedirectToAction(nameof(BudgetList));
        //        }

        //        private bool BudgetMainsExists(int id)
        //        {
        //            return _context.BudgetMains.Any(e => e.BudgetMainId == id);
        //        }

        //        public IActionResult Get()
        //        {
        //            try
        //            {
        //                var query = _budgetRepository.Get();
        //                var parser = new Parser<BudgetMainsList>(Request.Form, query);

        //                return Json(parser.Parse());

        //            }
        //            catch (Exception ex)
        //            {

        //                throw ex;
        //            }
        //        }

        //        #endregion

        //        #region Bank Reconcile Info
        //        public ViewResult BankReconcileList(string FromDate, string ToDate, string criteria)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");

        //            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

        //            List<Acc_VoucherSubCheckno> abcd = new List<Acc_VoucherSubCheckno>();

        //            if (FromDate == null || FromDate == "")
        //            {
        //                abcd = _bankReconcilRepository.GetAccVoucherSub();
        //            }
        //            else
        //            {
        //                dtFrom = Convert.ToDateTime(FromDate);
        //                abcd = _bankReconcilRepository.GetAccVoucherSubElse();
        //            }

        //            return View(abcd);

        //        }

        //        public ActionResult CreateBankReconcile()
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");

        //            ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //            this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();
        //            ViewBag.Title = "Create";

        //            return View();
        //        }


        //        // POST: Categories/Create
        //        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //        //[Authorize]
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult CreateBankReconcile(Acc_VoucherSubCheckno Acc_VoucherSubChecknovar)
        //        {
        //            var errors = ModelState.Where(x => x.Value.Errors.Any())
        //            .Select(x => new { x.Key, x.Value.Errors });

        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");

        //            //if (ModelState.IsValid)
        //            {
        //                if (Acc_VoucherSubChecknovar.VoucherSubCheckId > 0)
        //                {

        //                    if (Acc_VoucherSubChecknovar.ComId == null || Acc_VoucherSubChecknovar.ComId == "")
        //                    {
        //                        Acc_VoucherSubChecknovar.ComId = comid;

        //                    }

        //                    Acc_VoucherSubChecknovar.DateUpdated = DateTime.Now;
        //                    Acc_VoucherSubChecknovar.UpdateByUserId = userid;
        //                    Acc_VoucherSubChecknovar.isManualEntry = true;
        //                    _voucherCheckSubNoRepository.Update(Acc_VoucherSubChecknovar);
        //                    TempData["Message"] = "Data Update Successfully";
        //                    TempData["Status"] = "2";
        //                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherSubChecknovar.VoucherId.ToString(), "Update", Acc_VoucherSubChecknovar.Criteria.ToString());

        //                }
        //                else
        //                {

        //                    Acc_VoucherSubChecknovar.DateAdded = DateTime.Now;
        //                    Acc_VoucherSubChecknovar.UserId = userid;
        //                    Acc_VoucherSubChecknovar.ComId = comid;
        //                    Acc_VoucherSubChecknovar.isManualEntry = true;
        //                    _voucherCheckSubNoRepository.Add(Acc_VoucherSubChecknovar);
        //                    TempData["Message"] = "Data Save Successfully";
        //                    TempData["Status"] = "1";
        //                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherSubChecknovar.VoucherId.ToString(), "Update", Acc_VoucherSubChecknovar.Criteria.ToString());

        //                    return RedirectToAction("BankReconcileList");
        //                }
        //            }
        //            return RedirectToAction("BankReconcileList");

        //        }


        //        //[Authorize]
        //        // GET: Categories/Edit/5
        //        public ActionResult EditBankReconcile(int? id)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }
        //            Acc_VoucherSubCheckno Acc_VoucherSubCheckno = _voucherCheckSubNoRepository.FindById(id);
        //            if (Acc_VoucherSubCheckno == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //            this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();
        //            ViewBag.Title = "Edit";

        //            return View("CreateBankReconcile", Acc_VoucherSubCheckno);

        //        }


        //        //[Authorize]
        //        // GET: Categories/Delete/5
        //        public ActionResult DeleteBankReconcile(int? id)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }
        //            Acc_VoucherSubCheckno Acc_VoucherSubCheckno = _voucherCheckSubNoRepository.FindById(id);


        //            if (Acc_VoucherSubCheckno == null)
        //            {
        //                return NotFound();
        //            }

        //            ViewData["Criteria"] = _govtScheduleEquityRepository.CriteriaList();
        //            this.ViewBag.AccId = _govtScheduleEquityRepository.AccId();
        //            ViewBag.Title = "Delete";

        //            return View("CreateBankReconcile", Acc_VoucherSubCheckno);
        //        }
        //        //        //[Authorize]
        //        // POST: Categories/Delete/5
        //        [HttpPost, ActionName("DeleteBankReconcile")]
        //        //      [ValidateAntiForgeryToken]
        //        public JsonResult DeleteBankReconcileConfirmed(int? id)
        //        {
        //            try
        //            {

        //                Acc_VoucherSubCheckno Acc_VoucherSubCheckno = _voucherCheckSubNoRepository.FindById(id);
        //                _voucherCheckSubNoRepository.Delete(Acc_VoucherSubCheckno);
        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Acc_VoucherSubCheckno.VoucherId.ToString(), "Update", Acc_VoucherSubCheckno.Criteria.ToString());

        //                return Json(new { Success = 1, VoucherSubCheckId = Acc_VoucherSubCheckno.VoucherSubCheckId, ex = "" });

        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }
        //        #endregion

        //        #region Post Voucher
        //        public ViewResult PostVoucherList(string FromDate, string ToDate, string criteria)
        //        {
        //            var transactioncomid = HttpContext.Session.GetString("comid");

        //            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //            if (criteria == null)
        //            {
        //                criteria = "UnPost";
        //            }

        //            if (FromDate == null || FromDate == "")
        //            {
        //            }
        //            else
        //            {
        //                dtFrom = Convert.ToDateTime(FromDate);
        //            }
        //            if (ToDate == null || ToDate == "")
        //            {
        //            }
        //            else
        //            {
        //                dtTo = Convert.ToDateTime(ToDate);
        //            }

        //            List<Acc_VoucherMain> modellist = new List<Acc_VoucherMain>();
        //            ViewBag.Title = criteria;

        //            if (criteria == "All")
        //            {
        //                modellist = _accPostVoucherRepository.ModelList1(FromDate, ToDate);
        //            }
        //            else if (criteria == "Post")
        //            {
        //                modellist = _accPostVoucherRepository.ModelList2(FromDate, ToDate);

        //            }
        //            else if (criteria == "UnPost")
        //            {
        //                modellist = _accPostVoucherRepository.ModelList3(FromDate, ToDate);

        //            }


        //            return View(modellist);
        //        }

        //        public IActionResult GetPostVoucher(string FromDate, string ToDate, string criteria, int isAll)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid"));

        //                DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
        //                DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

        //                if (FromDate == null || FromDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtFrom = Convert.ToDateTime(FromDate);

        //                }
        //                if (ToDate == null || ToDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtTo = Convert.ToDateTime(ToDate);

        //                }

        //                Microsoft.Extensions.Primitives.StringValues y = "";

        //                var x = Request.Form.TryGetValue("search[value]", out y);

        //                var query = _accPostVoucherRepository.Query();

        //                if (y.ToString().Length > 0)
        //                {
        //                    if (criteria == "Post")
        //                        query = query.Where(v => v.Comid == comid && v.isPosted == true);
        //                    else if (criteria == "UnPost")
        //                        query = query.Where(v => v.Comid == comid && v.isPosted == false);
        //                    else
        //                        query = query.Where(v => v.Comid == comid);

        //                    var parser = new Parser<VoucherView>(Request.Form, query);

        //                    return Json(parser.Parse());
        //                }
        //                else
        //                {
        //                    if (criteria == "Post")
        //                        query = query.Where(v => v.Comid == comid && v.VoucherDate >= dtFrom && v.VoucherDate <= dtTo && v.isPosted == true);
        //                    else if (criteria == "UnPost")
        //                        query = query.Where(v => v.Comid == comid && v.VoucherDate >= dtFrom && v.VoucherDate <= dtTo && v.isPosted == false);
        //                    else
        //                        query = query.Where(v => v.Comid == comid && v.VoucherDate >= dtFrom && v.VoucherDate <= dtTo);

        //                    var parser = new Parser<VoucherView>(Request.Form, query);

        //                    return Json(parser.Parse());
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = "0", error = ex.Message });
        //                //throw ex;
        //            }

        //        }
        //        public ActionResult PrintPostVoucher(int? id, string type)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            string callBackUrl = _accPostVoucherRepository.Print(id, type);
        //            var abcvouchermain = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(x => x.VoucherId == id && x.ComId == comid).FirstOrDefault();
        //            var reportname = "rptShowVoucher";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), abcvouchermain.VoucherId.ToString(), "Report", reportname);

        //            return Redirect(callBackUrl);

        //            ///return RedirectToAction("Index", "ReportViewer");


        //        }

        //        public JsonResult SetProcess(string[] voucherid, string criteria)
        //        {
        //            _accPostVoucherRepository.SetProcess(voucherid, criteria);
        //            var data = "";
        //            return Json(data = "1");
        //        }
        //        public ActionResult CreatePostVoucher()
        //        {
        //            return View();
        //        }
        //        public string prcSaveData(Acc_VoucherMain model)
        //        {
        //            ArrayList arQuery = new ArrayList();

        //            try
        //            {
        //                var sqlQuery = "";

        //                return "Data Posted Successfuly";
        //            }
        //            catch (Exception ex)
        //            {
        //                return ex.Message;
        //            }

        //            finally
        //            {
        //                //clsCon = null;
        //            }
        //        }
        //        #endregion

        //        #region Show Voucher
        //        public ActionResult VoucherReport()
        //        {
        //            TempData["Message"] = "Data Load Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

        //            string comid = HttpContext.Session.GetString("comid");
        //            int defaultcountry = _showVoucherRepository.DefaultCountry();

        //            var date = DateTime.Now.ToString("dd-MMM-yyyy");
        //            var date1 = DateTime.Now.ToString("dd-MMM-yyyy");

        //            ViewBag.date = date;
        //            ViewBag.date1 = date1;

        //            ViewBag.CountryId = new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName", defaultcountry);
        //            ViewBag.VoucherTypeId = _showVoucherRepository.VoucherTypeId();

        //            #region accid viewbag selectlist
        //            List<Acc_ChartOfAccount> acclistdb = POS.GetChartOfAccountLedger(comid).ToList();

        //            List<SelectListItem> accid = new List<SelectListItem>();
        //            accid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });

        //            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
        //            if (acclistdb != null)
        //            {
        //                foreach (Acc_ChartOfAccount x in acclistdb)
        //                {
        //                    accid.Add(new SelectListItem { Text = x.AccName, Value = x.AccId.ToString() });
        //                }
        //            }
        //            ViewBag.AccId = (accid);
        //            #endregion

        //            ViewBag.PrdUnitId = _showVoucherRepository.PrdUnitId();

        //            ShowVoucherViewModel model = new ShowVoucherViewModel();
        //            List<Acc_FiscalYear> fiscalyear = _showVoucherRepository.FiscalYear();
        //            int fiscalyid = fiscalyear.Max(p => p.FYId);

        //            List<Acc_FiscalMonth> fiscalmonth = _showVoucherRepository.FiscalMonth();

        //            model.FiscalYs = fiscalyear;
        //            model.ProcessMonths = fiscalmonth;
        //            model.CountryId = defaultcountry;

        //            return View(model);
        //        }


        //        //[ValidateAntiForgeryToken]
        //        public ActionResult PrintReport(int? id, string type)
        //        {
        //            try
        //            {

        //                return RedirectToAction("Index", "ReportViewer");

        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //            return null;
        //        }

        //        [HttpPost, ActionName("SetSession")]
        //        ////[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //        //[OverridableAuthorize]
        //        //[ValidateAntiForgeryToken]
        //        public ActionResult SetSession(string criteria, string rptFormat, string rpttype, string dtFrom, string dtTo,
        //            string VoucherFrom, string VoucherTo, int? Currency, int? isPosted, int? isOther, int? FYId, int? VoucherTypeId, int? AccId, int? PrdUnitId)
        //        {

        //            try
        //            {
        //                var callBackUrl = _showVoucherRepository.SetSession(criteria, rptFormat, rpttype, dtFrom, dtTo, VoucherFrom, VoucherTo, Currency, isPosted, isOther, FYId, VoucherTypeId, AccId, PrdUnitId);
        //                return Json(callBackUrl);
        //            }

        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);

        //            }
        //            return null;

        //        }
        //        #endregion

        //        #region Summary Report

        //        public JsonResult GetSummaryMonthList(int? id)
        //        {
        //            string comid = HttpContext.Session.GetString("comid");
        //            if (id == null)
        //            {
        //                List<Acc_FiscalYear> fiscalyear = _summaryReportRepository.FiscalYear();
        //                id = fiscalyear.Max(p => p.FYId);
        //            }

        //            List<Acc_FiscalMonth> Acc_FiscalMonth = _summaryReportRepository.FiscalMonth(id);
        //            var datamonth = _summaryReportRepository.Acc_FiscalMonth(id);

        //            List<Acc_FiscalHalfYear> Acc_FiscalHalfYear = _summaryReportRepository.FiscalHalfYear(id);
        //            var datahalfyear = _summaryReportRepository.Acc_FiscalHalfYear(id);

        //            List<Acc_FiscalQtr> FiscalQuarter = _summaryReportRepository.FiscalQuarter(id);
        //            var dataquarter = _summaryReportRepository.Acc_FiscalQtr(id);

        //            var data = new { datam = datamonth, datah = datahalfyear, dataq = dataquarter };
        //            return Json(data);

        //        }

        //        // GET: Section
        //        public ActionResult SummaryReport()
        //        {
        //            string comid = HttpContext.Session.GetString("comid");
        //            int defaultcountry = _showVoucherRepository.DefaultCountry();
        //            ViewBag.CountryId = _summaryReportRepository.CountryId();
        //            ViewBag.AccIdGroup = _summaryReportRepository.AccIdGroup();
        //            ViewBag.PrdUnitId = _showVoucherRepository.PrdUnitId();//&& c.ComId == (transactioncomid)

        //            var model = _summaryReportRepository.Model();

        //            return View(model);
        //        }

        //        public ActionResult trailReport()
        //        {
        //            try
        //            {

        //                List<TrialBalanceModel> TrialBalanceReport = _summaryReportRepository.TrialBalance();
        //                return View(TrialBalanceReport);

        //                //return Json(new { bookingDeliveryChallan, ex = result });
        //            }
        //            catch (Exception ex)
        //            {

        //                throw ex;
        //            }
        //        }
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult SummaryReport(string RptType, string criteria, string rptFormat, string command)
        //        {
        //            return View();
        //            ////return RedirectAndPostActionResult.RedirectAndPost("http://27.147.251.124/acs.mis/frmGeneratingReport.aspx", postData);
        //        }

        //        public ActionResult PrintReportSummary(int? id, string type)
        //        {
        //            try
        //            {
        //                return RedirectToAction("Index", "ReportViewer");
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //        }


        //        [HttpPost]
        //        public JsonResult SetSessionSummary(string criteria, string rptFormat, string rpttype, int? Currency, int? isCompare, int? isCumulative, int? isShowZero, int? isGroup, int? FYId, int? FYHId, int? FYQId, int? FYMId, string FromDate, string ToDate, int? AccIdGroup, int? PrdUnitId)
        //        {

        //            try
        //            {
        //                var reportname = "";
        //                var filename = "";
        //                _summaryReportRepository.SetSession(criteria, rptFormat, rpttype, Currency, isCompare, isCumulative, isShowZero, isGroup, FYId, FYHId, FYQId, FYMId, FromDate, ToDate, AccIdGroup, PrdUnitId);

        //                TempData["Status"] = "2";
        //                TempData["Message"] = "Summary Report";
        //                TempData["FileName"] = filename;
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), reportname, "Report", reportname); //not working 
        //                string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });
        //                return Json(new { Url = redirectUrl });

        //            }

        //            catch (Exception ex)
        //            {
        //                //throw ex;
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //        }
        //        #endregion

        //        #region General Report

        //        public ActionResult VoucherGeneralReport(int VoucherId)
        //        {
        //            try
        //            {
        //                Acc_VoucherMain Vouchermain = _generalReportRepository.VoucherMain(VoucherId);
        //                return View(Vouchermain);

        //                //return Json(new { bookingDeliveryChallan, ex = result });
        //            }
        //            catch (Exception ex)
        //            {

        //                throw ex;
        //            }

        //        }


        //        public ActionResult DemoGeneralReport(string AccId, string FYId, string dtFrom, string dtTo, string CountryId, string IsLocalCurrency, string SupplierId, string CustomerId, string EmployeeId)
        //        {
        //            try
        //            {
        //                List<LedgerDetailsModel> bookingDeliveryChallan = _generalReportRepository.BookingDeliveryChallan(AccId, FYId, dtFrom, dtTo, CountryId, IsLocalCurrency, SupplierId, CustomerId, EmployeeId);
        //                return View(bookingDeliveryChallan);
        //                //return Json(new { bookingDeliveryChallan, ex = result });
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //        }

        //        // GET: Section
        //        public ActionResult GeneralReport()
        //        {
        //            string comid = HttpContext.Session.GetString("comid");
        //            int defaultcountry = _showVoucherRepository.DefaultCountry();

        //            var date = DateTime.Now.ToString("dd-MMM-yyyy");
        //            var date1 = DateTime.Now.ToString("dd-MMM-yyyy");

        //            ViewBag.date = date;
        //            ViewBag.date1 = date1;

        //            ViewBag.CountryId = _generalReportRepository.CountryId();

        //            this.ViewBag.AccIdRecPay = _generalReportRepository.AccIdRecPay();
        //            this.ViewBag.AccIdLedger = _generalReportRepository.AccIdLedger();
        //            this.ViewBag.AccIdGroup = _generalReportRepository.AccIdGroup();

        //            this.ViewBag.AccIdNoteOneCT = _generalReportRepository.AccIdNoteOneCT();

        //            this.ViewBag.EmployeeId = _empReleaseRepository.EmpList();
        //            this.ViewBag.SupplierId = _generalReportRepository.SupplierList();
        //            this.ViewBag.CustomerId = _generalReportRepository.CustomerId();

        //            ViewBag.PrdUnitId = _generalReportRepository.PrdUnitId();
        //            ViewBag.VoucherTypeId = _generalReportRepository.VoucherTypeList();

        //            var model = _generalReportRepository.Model();
        //            return View(model);

        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult GeneralReport(string rpttype, string criteria, string rptFormat)
        //        {
        //            return View();
        //        }

        //        public ActionResult PrintGeneralReport(int? id, string type)
        //        {
        //            try
        //            {
        //                return RedirectToAction("Index", "ReportViewer");
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //        }

        //        [HttpPost, ActionName("GeneralSetSession")]
        //        public JsonResult GeneralSetSession(string criteria, string rptFormat, string rpttype, string dtFrom, string dtTo,
        //            int? Currency, int? isDetails, int? isLocalCurr, int? isMaterial, int? FYId, int? AccIdRecPay, int? AccIdLedger,
        //            int? AccIdGroup, int? PrdUnitId, int? AccVoucherTypeId,
        //            int? SupplierId, int? CustomerId, int? EmployeeId, string AccIdNoteOneCT, string MinAccCode, string MaxAccCode)
        //        {

        //            try
        //            {

        //                string redirectUrl = _generalReportRepository.GeneralSetSession(criteria, rptFormat, rpttype, dtFrom, dtTo, Currency, isDetails, isLocalCurr, isMaterial, FYId, AccIdRecPay, AccIdLedger, AccIdGroup, PrdUnitId, AccVoucherTypeId, SupplierId, CustomerId, EmployeeId, AccIdNoteOneCT, MinAccCode, MaxAccCode);
        //                return Json(new { Url = redirectUrl });
        //            }

        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }
        //        }


        //        #endregion

        //        #region Budget Release

        //        // GET: BudgetReleaseSetup
        //        public async Task<IActionResult> BudgetReleaseList()
        //        {
        //            TempData["Message"] = "Data Load Successfully";
        //            TempData["Status"] = "1";

        //            string comid = HttpContext.Session.GetString("comid");
        //            string userid = HttpContext.Session.GetString("userid");

        //            ViewBag.FiscalYearId = _budgetReleaseRepository.FiscalYearId();
        //            var filterAccount = _budgetReleaseRepository.FilterAccount();
        //            ViewBag.AccId = _budgetReleaseRepository.AccId();

        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

        //            return View(_budgetReleaseRepository.GetBudgetRelease());
        //        }


        //        // GET: BudgetReleaseSetup/Create
        //        public IActionResult CreateBudgetRelease()
        //        {
        //            ViewBag.Title = "Create";

        //            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName");
        //            ViewData["ProductId"] = _summaryReportRepository.PrdUnitId();

        //            string comid = HttpContext.Session.GetString("comid");
        //            string userid = HttpContext.Session.GetString("userid");

        //            var fiscalYear = _budgetReleaseRepository.FiscalYear();
        //            if (fiscalYear != null)
        //            {
        //                var fiscalMonth = _budgetReleaseRepository.FiscalMonth();

        //                ViewBag.FiscalYearId = _budgetReleaseRepository.FiscalYearId();
        //                ViewBag.FiscalMonthId = _budgetReleaseRepository.FiscalMonthId();

        //            }
        //            else
        //            {
        //                ViewBag.FiscalYearId = _budgetReleaseRepository.FiscalYearId();
        //                ViewBag.FiscalMonthId = _budgetReleaseRepository.FiscalMonthId();
        //            }

        //            var filterAccount = _budgetReleaseRepository.FilterAccount();

        //            ViewBag.AccId = _budgetReleaseRepository.AccId();

        //            ViewBag.EmpId = _empReleaseRepository.EmpList();

        //            return View();
        //        }

        //        // POST: BudgetReleaseSetup/Create
        //        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        //[ValidateAntiForgeryToken]
        //        public async Task<IActionResult> CreateBudgetRelease(Acc_BudgetRelease budgetRelease)
        //        {
        //            if (ModelState.IsValid)
        //            {

        //                string userid = HttpContext.Session.GetString("userid");
        //                string comid = HttpContext.Session.GetString("comid");
        //                if (budgetRelease.BudgetReleaseId > 0)
        //                {
        //                    budgetRelease.DateUpdated = DateTime.Now;
        //                    budgetRelease.UpdateByUserId = userid;
        //                    _budgetReleaseRepository.Update(budgetRelease);

        //                    TempData["Message"] = "Data Update Successfully";
        //                    TempData["Status"] = "2";
        //                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), budgetRelease.BudgetReleaseId.ToString(), "Update", budgetRelease.BudgetReleaseId.ToString());

        //                }
        //                else
        //                {
        //                    budgetRelease.DateAdded = DateTime.Now;
        //                    budgetRelease.UserId = userid;
        //                    budgetRelease.ComId = comid;
        //                    _budgetReleaseRepository.Add(budgetRelease);
        //                    TempData["Message"] = "Data Save Successfully";
        //                    TempData["Status"] = "1";
        //                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), budgetRelease.BudgetReleaseId.ToString(), "Create", budgetRelease.BudgetReleaseId.ToString());

        //                }
        //                return RedirectToAction(nameof(BudgetReleaseList));
        //            }
        //            return View(budgetRelease);
        //        }


        //        [HttpGet]
        //        public IActionResult GetFiscalMonth(int FiscalYearId)
        //        {
        //            string comid = HttpContext.Session.GetString("comid");
        //            var data = _budgetReleaseRepository.GetMonthData(FiscalYearId);

        //            var filterAccount = _budgetReleaseRepository.FilterAccount();
        //            var accHead = filterAccount.Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList();
        //            return Json(new { Month = data, AccHead = accHead });
        //        }

        //        // GET: BudgetReleaseSetup/Edit/5
        //        public async Task<IActionResult> EditBudgetRelease(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Edit";
        //            string comid = HttpContext.Session.GetString("comid");
        //            var budgetRelease = _budgetReleaseRepository.FindById(id);

        //            ViewBag.FiscalYearId = _budgetReleaseRepository.FiscalYearId();
        //            ViewBag.FiscalMonthId = _budgetReleaseRepository.FiscalMonthId();

        //            var filterAccount = _budgetReleaseRepository.FilterAccount();
        //            ViewBag.AccId = _budgetReleaseRepository.AccId();

        //            ViewBag.EmpId = _empReleaseRepository.EmpList();

        //            if (budgetRelease == null)
        //            {
        //                return NotFound();
        //            }
        //            return View("CreateBudgetRelease", budgetRelease);
        //        }

        //        // GET: BudgetReleaseSetup/Delete/5
        //        public async Task<IActionResult> DeleteBudgetRelease(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var budgetRelease = _budgetReleaseRepository.FindById(id);

        //            if (budgetRelease == null)
        //            {
        //                return NotFound();
        //            }

        //            ViewBag.Title = "Delete";
        //            string comid = HttpContext.Session.GetString("comid");

        //            ViewBag.FiscalYearId = _budgetReleaseRepository.FiscalYearId();
        //            ViewBag.FiscalMonthId = _budgetReleaseRepository.FiscalMonthId();

        //            var filterAccount = _budgetReleaseRepository.FilterAccount();
        //            ViewBag.AccId = _budgetReleaseRepository.AccId();

        //            ViewBag.EmpId = _empReleaseRepository.EmpList();

        //            return View("CreateBudgetRelease", budgetRelease);
        //        }

        //        // POST: BudgetReleaseSetup/Delete/5
        //        [HttpPost, ActionName("DeleteBudgetRelease")]
        //        //[ValidateAntiForgeryToken]
        //        public async Task<IActionResult> DeleteBudgetReleaseConfirmed(int id)
        //        {
        //            try
        //            {
        //                var budgetRelease = _budgetReleaseRepository.FindById(id);
        //                _budgetReleaseRepository.Delete(budgetRelease);

        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), budgetRelease.BudgetReleaseId.ToString(), "Delete", budgetRelease.BudgetReleaseId.ToString());

        //                //TempData["Message"] = "Data Delete Successfully";
        //                return Json(new { Success = 1, BudgetReleaseId = budgetRelease.BudgetReleaseId, ex = TempData["Message"].ToString() });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }
        //        }
        //        public IActionResult GetBalance(int AccId, int FiscalYearId, int BudgetReleaseId)
        //        {
        //            string comid = HttpContext.Session.GetString("comid");
        //            decimal balance = 0;
        //            var main = _budgetReleaseRepository.BudgetMain(FiscalYearId);
        //            if (main != null)
        //            {
        //                var sub = _budgetReleaseRepository.BudgetDetails(AccId, FiscalYearId);
        //                balance = sub != null ? sub.BudgetDebit : 0;
        //            }

        //            double release = _budgetReleaseRepository.BudgetRelease(AccId, FiscalYearId, BudgetReleaseId);

        //            //balance = release != null ? balance - (decimal)release : balance;
        //            return Json(balance - (decimal)release);
        //        }
        //        public JsonResult SetSessionAccountReport(string rptFormat, string FiscalYearId, string AccId)
        //        {
        //            try
        //            {
        //                string redirectUrl = _budgetReleaseRepository.SetSessionAccountReport(rptFormat, FiscalYearId, AccId);
        //                return Json(new { Url = redirectUrl });

        //            }

        //            catch (Exception ex)
        //            {
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        //            //return RedirectToAction("Index");

        //        }

        //        #endregion

        //        #region Prd Unit
        //        public ActionResult PrdUnitList()
        //        {
        //            TempData["Message"] = "Data Load Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

        //            //return View(db.PrdUnits.Where(c => c.PrdUnitId > 0).ToList());
        //            return View();

        //        }

        //        //[Authorize]
        //        // GET: Categories/Create
        //        public ActionResult CreatePrdUnit()
        //        {
        //            ViewBag.Title = "Create";
        //            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == (HttpContext.Session.GetString("comid"))).Where(c => c.CategoryId > 0), "CategoryId", "Name");

        //            return View();
        //        }

        //        // POST: Categories/Create
        //        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //        //[Authorize]
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult CreatePrdUnit(PrdUnit PrdUnit)
        //        {
        //            var errors = ModelState.Where(x => x.Value.Errors.Any())
        //            .Select(x => new { x.Key, x.Value.Errors });

        //            string comid = HttpContext.Session.GetString("comid");
        //            string userid = HttpContext.Session.GetString("userid");


        //            //if (ModelState.IsValid)
        //            //{
        //            if (PrdUnit.PrdUnitId > 0)
        //            {

        //                PrdUnit.DateUpdated = DateTime.Now;
        //                PrdUnit.ComId = comid;

        //                if (PrdUnit.UserId == null)
        //                {
        //                    PrdUnit.UserId = userid;
        //                }
        //                PrdUnit.UpdateByUserId = userid;


        //                _prdUnitRepository.Update(PrdUnit);

        //                TempData["Message"] = "Data Update Successfully";
        //                TempData["Status"] = "2";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PrdUnit.PrdUnitId.ToString(), "Update", PrdUnit.PrdUnitName.ToString());


        //            }
        //            else
        //            {
        //                PrdUnit.UserId = userid;
        //                PrdUnit.ComId = comid;
        //                PrdUnit.DateAdded = DateTime.Now;

        //                _prdUnitRepository.Add(PrdUnit);

        //                TempData["Message"] = "Data Save Successfully";
        //                TempData["Status"] = "1";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PrdUnit.PrdUnitId.ToString(), "Create", PrdUnit.PrdUnitName.ToString());

        //                return RedirectToAction("Index");
        //            }
        //            //}
        //            return RedirectToAction("Index");

        //            //return View(PrdUnit);
        //        }


        //        //[Authorize]
        //        // GET: Categories/Edit/5
        //        public ActionResult EditPrdUnit(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }
        //            var comid = HttpContext.Session.GetString("comid");

        //            PrdUnit PrdUnit = _prdUnitRepository.FindById(id);
        //            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == comid).Where(c => c.CategoryId > 0), "CategoryId", "Name");

        //            if (PrdUnit == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Edit";

        //            return View("CreatePrdUnit", PrdUnit);

        //        }


        //        //[Authorize]
        //        // GET: Categories/Delete/5
        //        public ActionResult DeletePrdUnit(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }

        //            var comid = (HttpContext.Session.GetString("comid")).ToString();

        //            PrdUnit PrdUnit = _prdUnitRepository.FindById(id);
        //            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == comid).Where(c => c.CategoryId > 0), "CategoryId", "Name");

        //            if (PrdUnit == null)
        //            {
        //                return NotFound();
        //            }

        //            ViewBag.Title = "Delete";

        //            return View("CreatePrdUnit", PrdUnit);
        //        }
        //        //        //[Authorize]
        //        // POST: Categories/Delete/5
        //        [HttpPost, ActionName("DeletePrdUnit")]
        //        //      [ValidateAntiForgeryToken]
        //        public JsonResult DeletePrdUnitConfirmed(int? id)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid")).ToString();
        //                PrdUnit PrdUnit = _prdUnitRepository.FindById(id);
        //                _prdUnitRepository.Delete(PrdUnit);
        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PrdUnit.PrdUnitId.ToString(), "Delete", PrdUnit.PrdUnitName);


        //                return Json(new { Success = 1, PrdUnitId = PrdUnit.PrdUnitId, ex = "" });

        //            }
        //            catch (Exception ex)
        //            {

        //                return Json(new { Success = 0, ex = ex.Message.ToString() });

        //            }
        //        }

        //        #endregion

        //        #region Cost Allocation
        //        public async Task<IActionResult> CostAllocationList()
        //        {
        //            string comid = HttpContext.Session.GetString("comid");

        //            var fiscalYear = _costAllocationRepository.FiscalYear();
        //            if (fiscalYear != null)
        //            {
        //                var fiscalMonth = _costAllocationRepository.FiscalMonth();

        //                ViewBag.FiscalYearId = _costAllocationRepository.FiscalYearId();
        //                ViewBag.FiscalMonthId = _costAllocationRepository.FiscalMonthId();

        //            }
        //            else
        //            {
        //                ViewBag.FiscalYearId = _costAllocationRepository.FiscalYearIdElse();
        //                ViewBag.FiscalMonthId = _costAllocationRepository.FiscalMonthIdElse();

        //            }

        //            ViewBag.CostAlloMainId = _costAllocationRepository.CostAlloMainId();

        //            return View(await _costAllocationRepository.GetCostAllocation());


        //            //return View();
        //        }

        //        public IActionResult GetCostAllocation(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid"));

        //                DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
        //                DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

        //                if (FromDate == null || FromDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtFrom = Convert.ToDateTime(FromDate);

        //                }
        //                if (ToDate == null || ToDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtTo = Convert.ToDateTime(ToDate);

        //                }

        //                Microsoft.Extensions.Primitives.StringValues y = "";

        //                var x = Request.Form.TryGetValue("search[value]", out y);

        //                if (y.ToString().Length > 0)
        //                {


        //                    var query = _costAllocationRepository.Query1();
        //                    var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, query);

        //                    return Json(parser.Parse());

        //                }
        //                else
        //                {


        //                    if (CustomerList != null && UserList != null)
        //                    {
        //                        var querytest = _costAllocationRepository.Query2(UserList);
        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());
        //                    }
        //                    else if (CustomerList != null && UserList == null)
        //                    {
        //                        var querytest = _costAllocationRepository.Query3();
        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());
        //                    }
        //                    else if (CustomerList == null && UserList != null)
        //                    {

        //                        var querytest = _costAllocationRepository.Query2(UserList);
        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());
        //                    }
        //                    else
        //                    {
        //                        var querytest = _costAllocationRepository.Query3();
        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());
        //                    }

        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                return Json(new { Success = "0", error = ex.Message });
        //                //throw ex;
        //            }

        //        }

        //        public ActionResult GetProductInfo(int id)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");

        //            decimal? lastpurchaseprice;
        //            lastpurchaseprice = _costAllocationRepository.LastPurchasePrice(id);
        //            if (lastpurchaseprice == null)
        //            {
        //                lastpurchaseprice = 0;
        //            }
        //            var ProductData = _costAllocationRepository.Product(id);// ToList();
        //            return Json(ProductData);
        //        }

        //        // GET: PurchaseRequisition/Create
        //        public IActionResult CreateCostAllocation()
        //        {

        //            InitViewBag("Create");
        //            return View();
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public IActionResult CreateCostAllocation(CostAllocation_Main CostAllocation_Main)
        //        {
        //            try
        //            {

        //                if (ModelState.IsValid)
        //                {
        //                    if (CostAllocation_Main.CostAlloMainId > 0)
        //                    {
        //                        _costAllocationRepository.UpdateCostAllocation(CostAllocation_Main);
        //                        TempData["Status"] = "2";
        //                        TempData["Message"] = "Data Update Successfully";

        //                        return Json(new { Success = 2, data = CostAllocation_Main.CostAlloMainId, ex = TempData["Message"].ToString() });
        //                    }
        //                    else
        //                    {
        //                        _costAllocationRepository.CreateCostAllocation(CostAllocation_Main);
        //                        TempData["Status"] = "1";
        //                        TempData["Message"] = "Data Save Successfully";

        //                        return Json(new { Success = 1, data = CostAllocation_Main.CostAlloMainId, ex = TempData["Message"].ToString() }); ;
        //                    }
        //                }
        //                else
        //                {
        //                    return Json(new { Success = 3, ex = "Model State Not Valid" });
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                return Json(new { Success = 3, ex = e.Message });

        //            }

        //        }

        //        public JsonResult ProductInfo(int id)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid"));
        //                var product = _costAllocationRepository.Prod(id);
        //                var unit = _costAllocationRepository.Unit(id);
        //                return Json(unit);

        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { success = false, values = ex.Message.ToString() });
        //            }

        //        }

        //        public JsonResult GetProducts(int? id)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            IEnumerable<object> product;
        //            if (id != null)
        //            {
        //                if (id == 0 || id == -1)
        //                {
        //                    //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
        //                    product = _costAllocationRepository.ProductList1();

        //                }
        //                else
        //                {
        //                    product = _costAllocationRepository.ProductList2(id);
        //                    //product = db.Products.Where(x => x.CategoryId == id).Select(x => new { x.ProductId, x.ProductName }).ToList();
        //                }
        //            }
        //            else
        //            {
        //                //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
        //                product = _costAllocationRepository.ProductList3();

        //            }
        //            return Json(new { item = product });
        //        }

        //        [HttpPost]
        //        public ActionResult DeletePrSub(int prsubid)
        //        {
        //            try
        //            {
        //                _costAllocationRepository.DeletePrSub(prsubid);
        //                return Json(new { error = 0, success = 1, message = "This record deleted successfully" });
        //            }
        //            catch (Exception ex)
        //            {
        //                var m = $" Message:{ex.Message}\nInner Exception:{ex.InnerException.Message}";
        //                return Json(new { error = 1, success = 0, message = m });
        //            }

        //        }

        //        // GET: PurchaseRequisition/Edit/5
        //        public async Task<IActionResult> EditCostAllocation(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            ViewBag.Title = "Edit";

        //            CostAllocation_Main CostAllocation_Main = await _costAllocationRepository.CostAllocation(id);

        //            if (CostAllocation_Main == null)
        //            {
        //                return NotFound();
        //            }
        //            InitViewBag("Edit", id, CostAllocation_Main);
        //            return View("CreateCostAllocation", CostAllocation_Main);
        //        }

        //        // GET: PurchaseRequisition/Delete/5
        //        public async Task<IActionResult> DeleteCostAllocation(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            ViewBag.Title = "Delete";

        //            CostAllocation_Main CostAllocation_Main = await _costAllocationRepository.CostAllocation(id);
        //            // PurchaseRequisitionMain purchaseRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
        //            if (CostAllocation_Main == null)
        //            {
        //                return NotFound();
        //            }
        //            InitViewBag("Delete", id, CostAllocation_Main);
        //            //return Json(new {data= purchaseRequisitionMain });
        //            return View("CreateCostAllocation", CostAllocation_Main);

        //        }

        //        // POST: PurchaseRequisition/Delete/5
        //        [HttpPost, ActionName("DeleteCostAllocation")]
        //        [ValidateAntiForgeryToken]
        //        public IActionResult DeleteCostAllocationConfirmed(int id)
        //        {
        //            try
        //            {
        //                var CostAllocation_Main = _costAllocationRepository.FindById(id);
        //                _costAllocationRepository.Delete(CostAllocation_Main);

        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), CostAllocation_Main.CostAlloMainId.ToString(), "Delete", CostAllocation_Main.CostAlloMainId.ToString());

        //                return Json(new { Success = 1, CostAlloMainId = CostAllocation_Main.CostAlloMainId, ex = "" });

        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new
        //                {
        //                    Success = 0,
        //                    ex = ex.Message.ToString()
        //                });
        //            }
        //        }
        //        private void InitViewBag(string title, int? id = null, CostAllocation_Main CostAllocationMian = null)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");

        //            ViewBag.Title = title;
        //            if (title == "Create")
        //            {
        //                var fiscalYear = _costAllocationRepository.FiscalYear();
        //                if (fiscalYear != null)
        //                {
        //                    var fiscalMonth = _costAllocationRepository.FiscalMonth();

        //                    ViewBag.FiscalYearId = _costAllocationRepository.FiscalYearId();
        //                    ViewBag.FiscalMonthId = _costAllocationRepository.FiscalMonthId();

        //                }
        //                else
        //                {
        //                    ViewBag.FiscalYearId = _costAllocationRepository.FiscalYearIdElse();
        //                    ViewBag.FiscalMonthId = _costAllocationRepository.FiscalMonthIdElse();

        //                }

        //                ViewBag.AccId = _costAllocationRepository.AccId();

        //                //ViewData["ProductId"] = new SelectList(db.Products.Where(c => c.comid == comid), "ProductId", "ProductName");

        //            }
        //            else if (title == "Edit" || title == "Delete")
        //            {

        //                ViewBag.FiscalYearId = _costAllocationRepository.FiscalYearIdCost(CostAllocationMian);
        //                ViewBag.FiscalMonthId = _costAllocationRepository.FiscalMonthIdCost(CostAllocationMian);

        //                ViewBag.AccId = _costAllocationRepository.AccId();
        //            }

        //        }
        //        public JsonResult SetSessionAccountReport(string rptFormat, string CostAlloMainId, string FiscalYearId, string FiscalMonthId)
        //        {
        //            try
        //            {
        //                string redirectUrl = _costAllocationRepository.SetSessionAccountReport(rptFormat, CostAlloMainId, FiscalYearId, FiscalMonthId);
        //                return Json(new { Url = redirectUrl });

        //            }

        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });

        //        }

        //        #endregion

        //        #region Bill Management
        //        public async Task<IActionResult> BillManagementList()
        //        {
        //            string comid = HttpContext.Session.GetString("comid");
        //            return View(await _billManagementRepository.GetBillManagement());

        //        }


        //        public IActionResult GetBillManagement(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid"));

        //                DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
        //                DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

        //                if (FromDate == null || FromDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtFrom = Convert.ToDateTime(FromDate);

        //                }
        //                if (ToDate == null || ToDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtTo = Convert.ToDateTime(ToDate);

        //                }

        //                Microsoft.Extensions.Primitives.StringValues y = "";

        //                var x = Request.Form.TryGetValue("search[value]", out y);

        //                if (y.ToString().Length > 0)
        //                {
        //                    var query = _billManagementRepository.Query1();
        //                    var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, query);
        //                    return Json(parser.Parse());
        //                }
        //                else
        //                {
        //                    if (CustomerList != null && UserList != null)
        //                    {
        //                        var querytest = _billManagementRepository.Query2(UserList);
        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());
        //                    }
        //                    else if (CustomerList != null && UserList == null)
        //                    {
        //                        var querytest = _billManagementRepository.Query3();
        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());
        //                    }
        //                    else if (CustomerList == null && UserList != null)
        //                    {
        //                        var querytest = _billManagementRepository.Query2(UserList);
        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());

        //                    }
        //                    else
        //                    {

        //                        var querytest = _billManagementRepository.Query3();

        //                        var parser = new Parser<PurchaseReQuisitionResult>(Request.Form, querytest);
        //                        return Json(parser.Parse());

        //                    }

        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = "0", error = ex.Message });
        //                //throw ex;
        //            }

        //        }

        //        public ActionResult GetProductInfoBill(int id)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            decimal? lastpurchaseprice;
        //            lastpurchaseprice = _billManagementRepository.LastPurchasePrice(id);
        //            if (lastpurchaseprice == null)
        //            {
        //                lastpurchaseprice = 0;
        //            }
        //            var ProductData = _billManagementRepository.Product(id);
        //            return Json(ProductData);
        //        }

        //        //GET: PurchaseRequisition/Create
        //        public IActionResult CreateBillManagement()
        //        {

        //            InitViewBagBill("Create");
        //            var data = _billManagementRepository.SingleBillData();
        //            var bill = new Bill_Main();
        //            bill.BillNo = "B-" + ++data;
        //            bill.BillDate = DateTime.Now;
        //            return View(bill); /// for new bill no set last primary key value increment
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public IActionResult CreateBillManagement(Bill_Main bill_Main)
        //        {
        //            try
        //            {
        //                var errors = ModelState.Where(x => x.Value.Errors.Any())
        //            .Select(x => new { x.Key, x.Value.Errors });

        //                if (ModelState.IsValid)
        //                {
        //                    if (bill_Main.BillMainId > 0)
        //                    {

        //                        _billManagementRepository.UpdateBillManagement(bill_Main);
        //                        TempData["Status"] = "2";
        //                        TempData["Message"] = "Data Update Successfully";

        //                        return Json(new { Success = 2, data = bill_Main.BillMainId, ex = TempData["Message"].ToString() });
        //                    }
        //                    else
        //                    {
        //                        _billManagementRepository.AddBillManagement(bill_Main);
        //                        //TempData["Status"] = "1";
        //                        TempData["Message"] = "Data Save Successfully";

        //                        return Json(new { Success = 1, data = bill_Main.BillMainId, ex = TempData["Message"].ToString() }); ;
        //                    }
        //                }
        //                else
        //                {
        //                    return Json(new { Success = 3, ex = "Model State Not Valid" });
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                _logger.LogError(e.InnerException.Message);
        //                return Json(new { Success = 3, ex = e.Message });

        //            }


        //        }

        //        public JsonResult ProductInfoBillManagement(int id)
        //        {
        //            try
        //            {


        //                var productName = _billManagementRepository.ProductName(id);
        //                return Json(productName);
        //                //return Json("tesst" );

        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                return Json(new { success = false, values = ex.Message.ToString() });
        //            }
        //            //return Json(new SelectList(product, "Value", "Text" ));
        //        }

        //        [HttpPost]
        //        public ActionResult DeletePrSubBill(int prsubid)
        //        {
        //            try
        //            {
        //                _billManagementRepository.DeletePrbBill(prsubid);
        //                return Json(new { error = 0, success = 1, message = "This record deleted successfully" });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                var m = $" Message:{ex.Message}\nInner Exception:{ex.InnerException.Message}";
        //                return Json(new { error = 1, success = 0, message = m });
        //            }

        //        }

        //        // GET: PurchaseRequisition/Edit/5
        //        public async Task<IActionResult> EditBillManagement(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            ViewBag.Title = "Edit";

        //            Bill_Main Bill = await _billManagementRepository.Bill(id);

        //            if (Bill == null)
        //            {
        //                return NotFound();
        //            }
        //            InitViewBagBill("Edit", Bill);
        //            //return Json(new {data= purchaseRequisitionMain });
        //            return View("CreateBillManagement", Bill);
        //        }


        //        public async Task<IActionResult> DeleteBillManagement(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            ViewBag.Title = "Delete";

        //            Bill_Main Bill = await _billManagementRepository.Bill(id);
        //            // PurchaseRequisitionMain purchaseRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
        //            if (Bill == null)
        //            {
        //                return NotFound();
        //            }
        //            InitViewBagBill("Delete", Bill);
        //            //return Json(new {data= purchaseRequisitionMain });
        //            return View("CreateBillManagement", Bill);

        //        }

        //        // POST: PurchaseRequisition/Delete/5
        //        [HttpPost, ActionName("DeleteBillManagement")]
        //        [ValidateAntiForgeryToken]
        //        public IActionResult DeleteBillManagementConfirmed(int id)
        //        {
        //            try
        //            {
        //                var bill =  _billManagementRepository.FindById(id);
        //                _billManagementRepository.Delete(bill);


        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), bill.BillMainId.ToString(), "Delete", bill.BillNo);

        //                return Json(new { Success = 1, BillMainId = bill.BillMainId, ex = "" });

        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new
        //                {
        //                    Success = 0,
        //                    ex = ex.Message.ToString()
        //                });
        //            }
        //        }
        //        private void InitViewBagBill(string title, Bill_Main Bill_Main = null)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");

        //            ViewBag.Title = title;
        //            if (title == "Create")
        //            {


        //                ViewBag.SupplierId = _billManagementRepository.SupplierIdIf();
        //                ViewBag.ProductId = _billManagementRepository.ProductIdIf();
        //                ViewBag.AccId = _billManagementRepository.AccIdIf();

        //                //ViewData["ProductId"] = new SelectList(db.Products.Where(c => c.comid == comid), "ProductId", "ProductName");

        //            }
        //            else if (title == "Edit" || title == "Delete")
        //            {
        //                ViewBag.AccId = _billManagementRepository.AccIdElse(Bill_Main);
        //                ViewBag.SupplierId = _billManagementRepository.SupplierIdElse(Bill_Main);
        //                ViewBag.ProductId = _billManagementRepository.ProductIdElse();
        //            }

        //        }


        //        public ActionResult Print(int id)
        //        {

        //            string callBackUrl = _billManagementRepository.PrintBillManagement(id);
        //            return Redirect(callBackUrl);

        //            ///return RedirectToAction("Index", "ReportViewer");


        //        }

        //        public JsonResult SetSessionAccountReportBill(string rptFormat, string CostAlloMainId, string FiscalYearId, string FiscalMonthId)
        //        {
        //            try
        //            {

        //                string redirectUrl = _billManagementRepository.SetSessionAccountReportBill(rptFormat, CostAlloMainId, FiscalYearId, FiscalMonthId);
        //                return Json(new { Url = redirectUrl });

        //            }

        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        //            //return RedirectToAction("Index");

        //        }


        //        [HttpPost, ActionName("PrintVatTax")]
        //        public JsonResult GrrDetailsReport(string rptFormat, string action, string FromDate, string ToDate)
        //        {
        //            try
        //            {

        //                string redirectUrl = _billManagementRepository.GrrDetailsReport(rptFormat, action, FromDate, ToDate);

        //                return Json(new { Url = redirectUrl });
        //            }

        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });


        //        }

        //        [HttpPost, ActionName("PrintSD")]
        //        public JsonResult PrintSDReport(string rptFormat, string action, string FromDate, string ToDate)
        //        {
        //            try
        //            {
        //                string redirectUrl = _billManagementRepository.PrintSDReport(rptFormat, action, FromDate, ToDate);
        //                return Json(new { Url = redirectUrl });
        //            }

        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex.InnerException.Message);
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        //        }

        //        [HttpPost, ActionName("PrintWelfare")]
        //        public JsonResult PrintWelfareReport(string rptFormat, string action, string FromDate, string ToDate)
        //        {
        //            try
        //            {
        //                string redirectUrl = _billManagementRepository.PrintWelfareReport(rptFormat, action, FromDate, ToDate);
        //                return Json(new { Url = redirectUrl });
        //            }

        //            catch (Exception ex)
        //            {
        //                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });


        //        }

        //        #endregion

        //        #region Bank Clearing
        //        public ViewResult BankClearingList(string FromDate, string ToDate, string criteria)
        //        {

        //            ViewBag.Title = criteria;
        //            var BankClearingList = _bankClearingRepository.GetBankClearing(FromDate, ToDate, criteria);
        //            TempData["Message"] = "Bank Clearing List";

        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), criteria, "psot/unpot", criteria);

        //            return View(BankClearingList);
        //        }

        //        public ActionResult PrintBankClearing(int? id, string type)
        //        {
        //            string callBackUrl = _bankClearingRepository.Print(id, type);
        //            return Redirect(callBackUrl);
        //        }

        //        public JsonResult SetProcess(List<BankClearing> BankClearinglist, string criteria)
        //        {

        //            _bankClearingRepository.SetProcess(BankClearinglist, criteria);
        //            var data = "";
        //            return Json(data = "1");
        //        }
        //        public ActionResult CreateBankClearing()
        //        {
        //            return View();
        //        }

        //        public string prcSaveDataBankClearing(Acc_VoucherMain model)
        //        {
        //            ArrayList arQuery = new ArrayList();

        //            try
        //            {
        //                var sqlQuery = "";
        //                return "Data Posted Successfuly";
        //            }
        //            catch (Exception ex)
        //            {
        //                return ex.Message;
        //            }

        //            finally
        //            {
        //                //clsCon = null;
        //            }
        //        }
        //        #endregion

        //        #region Account Chart of Accounts

        //        //public static string AccCodeFirst = "";
        //        //public static string AccCodeSecound = "-0";
        //        //public static string AccCodeThird = "-00";
        //        //public static string AccCodeFourth = "-000";
        //        //public static string AccCodeFifth = "-00000";

        //        public ActionResult ChartOfAccountList()
        //        {
        //            TempData["Message"] = "Data Load Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

        //            List<Acc_ChartOfAccount> chartofaccountlist = _accountChartRepository.ChartOfAccountList();
        //            return View(chartofaccountlist);
        //        }

        //        public ActionResult COATree()
        //        {
        //            var all = _accountChartRepository.All();
        //            return View(all);
        //        }
        //        public ActionResult COATreeEditable()
        //        {
        //            var all = _accountChartRepository.All();
        //            return View(all);
        //        }

        //        // [OverridableAuthorize]
        //        public ActionResult ChartOfAccountDetails(int Id)
        //        {
        //            return View();
        //        }

        //        // [OverridableAuthorize]
        //        public ActionResult CreateChartOfAccount()
        //        {
        //            try
        //            {
        //                string comid = HttpContext.Session.GetString("comid");
        //                var model = new Acc_ChartOfAccount();

        //                var lastacccoa = _accountChartRepository.LastAcccoa();

        //                ViewBag.Title = "Create";

        //                List<COAtemp> COAParent = _accountChartRepository.COAParent1();

        //                ViewBag.opFYId = _accountChartRepository.OpFYId();
        //                var defaultcurrency = _accountChartRepository.DefaultCurrency();
        //                model.CountryID = defaultcurrency;
        //                model.CountryIdLocal = defaultcurrency;
        //                model.AccType = "L";
        //                if (lastacccoa != null)
        //                {
        //                    ViewBag.ParentId = _accountChartRepository.ParentId();
        //                    ViewBag.AccumulatedDepId = _accountChartRepository.AccumulatedDepId();
        //                    ViewBag.DepExpenseId = _accountChartRepository.DepExpenseId();
        //                }
        //                else
        //                {
        //                    ViewBag.ParentId = _accountChartRepository.ParentIdElse();
        //                    ViewBag.AccumulatedDepId = _accountChartRepository.AccumulatedDepIdElse();
        //                    ViewBag.DepExpenseId = _accountChartRepository.DepExpenseIdElse();
        //                }

        //                ViewBag.CountryId = _accountChartRepository.CountryId();
        //                return View(model);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);
        //            }
        //        }        

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult CreateChartOfAccount(Acc_ChartOfAccount model, string title)
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");

        //            if (model.AccCode_Old == null)
        //            {
        //                model.AccCode_Old = "";
        //            }

        //            SqlParameter[] sqlParameter = new SqlParameter[1];
        //            sqlParameter[0] = new SqlParameter("@comid", comid);
        //            try
        //            {
        //                {
        //                    if (model.AccId > 0)
        //                    {
        //                        if (isModelBase == 1)
        //                        {
        //                            if (model.ParentID == model.PrevParentID && model.AccType == model.PrevAccType)
        //                            {
        //                                model.UpdateByUserId = userid;
        //                                model.DateUpdated = DateTime.Now;
        //                                _accountChartRepository.Update(model);

        //                                TempData["Message"] = "Data Update Successfully";
        //                                TempData["Status"] = "2";
        //                                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Update", model.AccName.ToString() + " " + model.AccCode);
        //                            }
        //                            else
        //                            {
        //                                var parentcodeinfo = _context.Acc_ChartOfAccounts.Where(x => x.AccId == model.ParentID && x.ComId == comid).FirstOrDefault();
        //                                string maxacccsubid = _context.Acc_ChartOfAccounts.Where(x => x.ParentID == model.ParentID && x.ComId == comid).Max(x => x.AccSubId).ToString();

        //                                if (model.AccType != model.PrevAccType)
        //                                {
        //                                    int ExistAccIdVoucher = _context.Acc_VoucherSubs.Where(x => x.AccId == model.AccId).Count();
        //                                    int ExistAccIdSubAccountsHead = _context.Acc_ChartOfAccounts.Where(x => x.ParentID == model.AccId && x.ComId == comid).Count();

        //                                    if (ExistAccIdVoucher > 0)
        //                                    {
        //                                        ModelState.AddModelError(string.Empty, "Not Possible to Change the Type. Cause It Already Contains Some Voucher.");
        //                                        ViewBag.Title = title;

        //                                        ViewBag.ParentId = _accountChartRepository.ParentId1(model);
        //                                        ViewBag.opFYId = _accountChartRepository.OpFYId1(model);
        //                                        ViewBag.CountryId = _accountChartRepository.CountryId1(model);

        //                                        return this.View(model);
        //                                    }
        //                                    else if (ExistAccIdSubAccountsHead > 0)
        //                                    {
        //                                        ModelState.AddModelError(string.Empty, "Not Possible to Change the Type. Cause It Already Contains Some Sub Accounts Head / Group");
        //                                        ViewBag.Title = title;

        //                                        ViewBag.ParentId = _accountChartRepository.ParentId1(model);
        //                                        ViewBag.opFYId = _accountChartRepository.OpFYId1(model);
        //                                        ViewBag.CountryId = _accountChartRepository.CountryId1(model);

        //                                        return this.View(model);
        //                                    }
        //                                }

        //                                if (maxacccsubid == null || maxacccsubid == "")
        //                                {
        //                                    int level = int.Parse(parentcodeinfo.Level.ToString());
        //                                    if (level == 0 || level == 1)///need to check the level if level 0 and 1 then "0" and  if level 2 then "00" if level 3 then "000"
        //                                    {
        //                                        maxacccsubid = "0";
        //                                    }
        //                                    else if (level == 2)
        //                                    {
        //                                        maxacccsubid = "10";
        //                                    }
        //                                    else if (level == 3)
        //                                    {
        //                                        maxacccsubid = "100";
        //                                    }
        //                                    else if (level == 4)
        //                                    {
        //                                        maxacccsubid = "10000";
        //                                    }
        //                                    else
        //                                    {
        //                                        ModelState.AddModelError(string.Empty, "Not Supportd Level. Maximum Level 5.");
        //                                        ViewBag.Title = title;

        //                                        ViewBag.ParentId = _accountChartRepository.ParentId1(model);
        //                                        ViewBag.opFYId = _accountChartRepository.OpFYId1(model);
        //                                        ViewBag.CountryId = _accountChartRepository.CountryId1(model);

        //                                        return this.View(model);
        //                                    }
        //                                }

        //                                string fahad = AccCodeMaker(model.AccType, int.Parse(parentcodeinfo.Level.ToString()), int.Parse(maxacccsubid) + 1, parentcodeinfo.AccCode);

        //                                model.ParentCode = parentcodeinfo.AccCode; //fahad test 
        //                                model.DateUpdated = DateTime.Now;
        //                                model.AccSubId = int.Parse(maxacccsubid) + 1;
        //                                model.Level = parentcodeinfo.Level + 1;
        //                                model.ComId = comid;
        //                                model.UpdateByUserId = HttpContext.Session.GetString("userid");
        //                                model.AccCode = fahad;// parentcodeinfo.ParentCode + "-" + model.AccSubId;
        //                                model.CountryIdLocal = model.CountryID;
        //                                model.IsCashItem = parentcodeinfo.IsCashItem;
        //                                model.IsBankItem = parentcodeinfo.IsBankItem;

        //                                if (model.AccType == "G")
        //                                {
        //                                    model.OpDebit = 0;
        //                                    model.OpCredit = 0;
        //                                    model.Rate = 0;
        //                                    model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
        //                                    model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());
        //                                }
        //                                else
        //                                {
        //                                    model.Rate = 1;
        //                                    model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
        //                                    model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());
        //                                }

        //                                _accountChartRepository.Update(model);

        //                                TempData["Message"] = "Data Update Successfully";
        //                                TempData["Status"] = "2";
        //                                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Update", model.AccName.ToString() + " " + model.AccCode);
        //                            }

        //                            if (title == "Delete")
        //                            {
        //                                var count = _context.Acc_ChartOfAccounts.Where(m => m.ParentID == model.AccId && m.ComId == comid).ToList();

        //                                if (count.Count > 0)
        //                                {
        //                                    ViewBag.ErrorMessage = "delete not possible";
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                        }

        //                        TempData["Message"] = "Data Update Successfully";
        //                        TempData["Status"] = "2";
        //                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Update", model.AccName.ToString() + " " + model.AccCode);
        //                    }
        //                    else
        //                    {
        //                        var parentcodeinfo = _context.Acc_ChartOfAccounts.Where(x => x.AccId == model.ParentID && x.ComId == comid).FirstOrDefault();
        //                        string maxacccsubid = _context.Acc_ChartOfAccounts.Where(x => x.ParentID == model.ParentID && x.ComId == comid).Max(x => x.AccSubId).ToString();
        //                        if (maxacccsubid == null || maxacccsubid == "")
        //                        {
        //                            int level = int.Parse(parentcodeinfo.Level.ToString());
        //                            if (level == 0 || level == 1)///need to check the level if level 0 and 1 then "0" and  if level 2 then "00" if level 3 then "000"
        //                            {
        //                                maxacccsubid = "0";
        //                            }
        //                            else if (level == 2)
        //                            {
        //                                maxacccsubid = "10";
        //                            }
        //                            else if (level == 3)
        //                            {
        //                                maxacccsubid = "100";
        //                            }
        //                            else if (level == 4)
        //                            {
        //                                maxacccsubid = "10000";
        //                            }
        //                            else
        //                            {
        //                                ModelState.AddModelError(string.Empty, "Not Supportd Level. Maximum Level 5.");
        //                                ViewBag.Title = title;

        //                                ViewBag.ParentId = _accountChartRepository.ParentId1(model);
        //                                ViewBag.opFYId = _accountChartRepository.OpFYId1(model);
        //                                ViewBag.CountryId = _accountChartRepository.CountryId1(model);

        //                                return this.View(model);
        //                            }
        //                        }

        //                        string fahad = "";

        //                        if (model.AccCode != null)
        //                        {
        //                            if (model.AccCode.Length > 9)
        //                            {
        //                                fahad = model.AccCode;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            fahad = AccCodeMaker(model.AccType, int.Parse(parentcodeinfo.Level.ToString()), int.Parse(maxacccsubid) + 1, parentcodeinfo.AccCode);
        //                        }

        //                        if (isModelBase == 1)
        //                        {
        //                            model.ParentCode = parentcodeinfo.AccCode;
        //                            model.DateAdded = DateTime.Now;
        //                            model.OpDate = DateTime.Now;
        //                            model.AccSubId = int.Parse(maxacccsubid) + 1;
        //                            model.Level = parentcodeinfo.Level + 1;
        //                            model.ComId = comid;
        //                            model.UserId = HttpContext.Session.GetString("userid");
        //                            model.AccCode = fahad;// parentcodeinfo.ParentCode + "-" + model.AccSubId;
        //                            model.CountryIdLocal = model.CountryID;
        //                            model.IsCashItem = parentcodeinfo.IsCashItem;
        //                            model.IsBankItem = parentcodeinfo.IsBankItem;

        //                            if (model.AccType == "G")
        //                            {
        //                                model.OpDebit = 0;
        //                                model.OpCredit = 0;
        //                                model.Rate = 0;
        //                                model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
        //                                model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());
        //                            }
        //                            else
        //                            {
        //                                model.Rate = 1;
        //                                model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
        //                                model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());
        //                            }
        //                            _accountChartRepository.Add(model);

        //                            TempData["Message"] = "Data Save Successfully";
        //                            TempData["Status"] = "1";
        //                            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Create", model.AccName.ToString() + "" + model.AccCode);
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                    return RedirectToAction("CreateChartOfAccount");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ViewBag.Title = title;

        //                ViewBag.ParentId = _accountChartRepository.ParentId1(model);
        //                ViewBag.opFYId = _accountChartRepository.OpFYId1(model);
        //                ViewBag.CountryId = _accountChartRepository.CountryId1(model);

        //                ModelState.AddModelError("CustomError", ex.Message);
        //                return View(model);
        //            }
        //        }

        //        public static string AccCodeMaker(string type, int level, int accsubid, string parentcode)
        //        {
        //            string FinalAccCode = "";
        //            string[] splitcode = parentcode.Split('-');
        //            if (level == 0)
        //            {
        //                FinalAccCode = accsubid.ToString() + AccCodeSecound + AccCodeThird + AccCodeFourth + AccCodeFifth;
        //            }
        //            else if (level == 1)
        //            {
        //                FinalAccCode = splitcode[0].ToString() + "-" + accsubid.ToString() + AccCodeThird + AccCodeFourth + AccCodeFifth;
        //            }
        //            else if (level == 2)
        //            {
        //                FinalAccCode = splitcode[0].ToString() + "-" + splitcode[1].ToString() + "-" + accsubid.ToString() + AccCodeFourth + AccCodeFifth;
        //            }
        //            else if (level == 3)
        //            {
        //                FinalAccCode = splitcode[0].ToString() + "-" + splitcode[1].ToString() + "-" + splitcode[2].ToString() + "-" + accsubid.ToString() + AccCodeFifth;
        //            }
        //            else if (level == 4)
        //            {
        //                FinalAccCode = splitcode[0].ToString() + "-" + splitcode[1].ToString() + "-" + splitcode[2].ToString() + "-" + splitcode[3].ToString() + "-" + accsubid.ToString(); ///+ splitcode[4].ToString() + "-"
        //            }
        //            else
        //            {
        //                FinalAccCode = "Maximum 5 Level Supported";
        //            }
        //            return FinalAccCode;
        //        }

        //        // [OverridableAuthorize]
        //        public ActionResult EditChartOfAccount(int? Id)
        //        {
        //            if (Id == null)
        //            {
        //                return BadRequest();
        //            }

        //            Acc_ChartOfAccount chartofaccount = _accountChartRepository.FindById(Id);

        //            if (chartofaccount == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Edit";

        //            chartofaccount.PrevParentID = chartofaccount.ParentID;
        //            chartofaccount.PrevAccType = chartofaccount.AccType;

        //            ViewBag.ParentId = _accountChartRepository.ParentId2(Id);
        //            ViewBag.AccumulatedDepId = _accountChartRepository.AccumulatedDepId2(Id);
        //            ViewBag.DepExpenseId = _accountChartRepository.DepExpenseId2(Id);
        //            ViewBag.opFYId = _accountChartRepository.OpFYId2(Id);
        //            ViewBag.CountryId = _accountChartRepository.CountryId2(Id);

        //            return View("CreateChartOfAccount", chartofaccount);
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult EditChartOfAccount(Acc_ChartOfAccount model)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    var values = prcUpdateData(model);
        //                    if (values == "Data Updated Sucessfully")
        //                    {
        //                        return RedirectToAction("ChartOfAccountList");
        //                    }
        //                    ModelState.AddModelError("CustomError", values);

        //                    return View(model);
        //                }
        //                return View(model);
        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError("CustomError", ex.Message);
        //                return View(model);
        //            }
        //        }

        //        [OverridableAuthorize]
        //        public ActionResult DeleteChartOfAccount(int Id)
        //        {
        //            if (Id == 0)
        //            {
        //                return BadRequest();
        //            }

        //            Acc_ChartOfAccount chartofaccount = _accountChartRepository.FindById(Id);

        //            if (chartofaccount == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Delete";

        //            ViewBag.ParentId = _accountChartRepository.ParentId2(Id);
        //            ViewBag.AccumulatedDepId = _accountChartRepository.AccumulatedDepId2(Id);
        //            ViewBag.DepExpenseId = _accountChartRepository.DepExpenseId2(Id);
        //            ViewBag.opFYId = _accountChartRepository.OpFYId2(Id);
        //            ViewBag.CountryId = _accountChartRepository.CountryId2(Id);

        //            return View("CreateChartOfAccount", chartofaccount);
        //        }

        //        [HttpPost, ActionName("DeleteChartOfAccount")]
        //        public JsonResult DeleteChartOfAccountConfirmed(int? id)
        //        {
        //            try
        //            {
        //                var comid = HttpContext.Session.GetString("comid");

        //                if (isModelBase == 1)
        //                {
        //                    Acc_ChartOfAccount coa = _accountChartRepository.FindById(id);

        //                    int ExistAccIdVoucher = _context.Acc_VoucherSubs.Where(x => x.AccId == coa.AccId).Count();
        //                    int ExistAccIdSubAccountsHead = _context.Acc_ChartOfAccounts.Where(x => x.ParentID == coa.AccId && x.ComId == comid).Count();

        //                    if (ExistAccIdVoucher > 0)
        //                    {
        //                        return Json(new { Success = 0, ex = new Exception("Unable to Delete . Not Possible to Change the Type. Cause It Already Contains Some Voucher.").Message.ToString() });
        //                    }
        //                    else if (ExistAccIdSubAccountsHead > 0)
        //                    {
        //                        return Json(new { Success = 0, ex = new Exception("Unable to Delete . Not Possible to Change the Type. Cause It Already Contains Some Sub Accounts Head / Group").Message.ToString() });
        //                    }
        //                    else
        //                    {
        //                        _accountChartRepository.Delete(coa);

        //                        TempData["Message"] = "Data Delete Successfully";
        //                        TempData["Status"] = "3";
        //                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), coa.AccId.ToString(), "Delete", coa.AccName + " " + coa.AccCode);
        //                        return Json(new { Success = 1, AccId = 1, ex = "Delete Done Successfully." });
        //                    }
        //                }
        //                else
        //                {
        //                }
        //                return Json(new { Success = 1, AccId = 1, ex = "" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }        

        //        public IActionResult GetChartOfAccount()
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid"));
        //                var query = from e in _context.Acc_ChartOfAccounts.Where(x => x.AccId > 0 && x.IsSysDefined == 0 && x.ComId == comid).OrderByDescending(x => x.AccId)
        //                            select new ChartOfAccountsabc
        //                            {
        //                                AccId = e.AccId,
        //                                AccCode = e.AccCode,
        //                                AccName = e.AccName,
        //                                AccType = e.AccType,
        //                                ParentName = e.ParentChartOfAccount.AccName,
        //                                Remarks = e.Remarks,
        //                                OpDebit = e.OpDebit,
        //                                OpCredit = e.OpCredit
        //                            };
        //                var parser = new Parser<ChartOfAccountsabc>(Request.Form, query);
        //                return Json(parser.Parse());
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        //public string prcDataSave(PF_ChartOfAccount model)
        //{
        //    try
        //    {
        //        return "Data Saved Sucessfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        //        public string prcUpdateData(Acc_ChartOfAccount model)
        //        {
        //            try
        //            {
        //                return "Data Updated Sucessfully";
        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);
        //            }
        //        }

        //        public string prcDeleteData(Acc_ChartOfAccount model)
        //        {
        //            try
        //            {
        //                return "Sucessfully Deleted.";
        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);
        //            }
        //        }

        //        ///call from json
        //        [HttpPost, ActionName("AccountsReport")]
        //        public JsonResult AccountsReport(string rptFormat, string action)
        //        {
        //            try
        //            {
        //                string comid = HttpContext.Session.GetString("comid");

        //                var reportname = "";
        //                var filename = "";
        //                string redirectUrl = "";
        //                if (action == "PrintAccCOA")
        //                {
        //                    reportname = "rptCOA";
        //                    filename = "rptCOA_List" + DateTime.Now.ToString();

        //                    HttpContext.Session.SetString("reportquery", "Exec [Acc_rptCOA] '" + comid + "'");
        //                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
        //                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
        //                }

        //                string DataSourceName = "DataSet1";
        //                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
        //                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
        //                clsReport.strDSNMain = DataSourceName;

        //                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }

        //                TempData["Message"] = "Chart Of Accounts Report Show";

        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), reportname, "Report", action);

        //                return Json(new { Url = redirectUrl });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        //        }

        //        public ActionResult AccountsReport()
        //        {
        //            try
        //            {
        //                string comid = HttpContext.Session.GetString("comid");

        //                var reportname = "";
        //                var filename = "";
        //                clsReport.rptList = null;

        //                reportname = "rptCOA";
        //                filename = "rptCOA_List" + DateTime.Now.Date.ToString();

        //                HttpContext.Session.SetString("reportquery", "Exec [Acc_rptCOA] '" + comid + "'");
        //                HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
        //                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
        //                HttpContext.Session.SetString("DataSourceName", "DataSet1");
        //                string DataSourceName = "DataSet1";

        //                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
        //                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
        //                clsReport.strDSNMain = DataSourceName;

        //                TempData["Message"] = "Chart Of Accounts Report Show";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), reportname, "Report", RouteData.Values["action"].ToString());

        //                return RedirectToAction("Index", "ReportViewer");
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }

        //            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        //        }

        //        [HttpPost, ActionName("SetSessionInd")]
        //        public JsonResult SetSessionInd(string reporttype)
        //        {
        //            try
        //            {
        //                HttpContext.Session.SetString("ReportType", reporttype);
        //                return Json(new { Success = 1 });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }
        //        }
        //        #endregion

        //        #region Voucher Tran Group
        //        public ActionResult VoucherTranGroupList()
        //        {
        //            var comid = HttpContext.Session.GetString("comid");
        //            var data = _voucherTranGroupRepository.GetAll().Where(x =>x.ComId == comid && !x.IsDelete).ToList();
        //            return View(data);
        //        }

        //        // GET: Categories/Create
        //        public ActionResult CreateVoucherTranGroup()
        //        {
        //            ViewBag.Title = "Create";
        //            return View();
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult CreateVoucherTranGroup(VoucherTranGroup VoucherTranGroups)
        //        {
        //            var errors = ModelState.Where(x => x.Value.Errors.Any())
        //            .Select(x => new { x.Key, x.Value.Errors });

        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");
        //            {
        //                if (VoucherTranGroups.VoucherTranGroupId > 0)
        //                {
        //                    _voucherTranGroupRepository.Update(VoucherTranGroups);
        //                }
        //                else
        //                {
        //                    VoucherTranGroups.ComId = comid;
        //                    VoucherTranGroups.UserId = userid;

        //                    _voucherTranGroupRepository.Add(VoucherTranGroups);

        //                    _context.Entry(VoucherTranGroups).GetDatabaseValues();
        //                    return RedirectToAction("VoucherTranGroupList");
        //                }
        //            }
        //            return RedirectToAction("VoucherTranGroupList");
        //        }

        //        // GET: Categories/Edit/5
        //        public ActionResult EditVoucherTranGroup(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }
        //            VoucherTranGroup VoucherTranGroup = _voucherTranGroupRepository.FindById(id);

        //            if (VoucherTranGroup == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Edit";
        //            return View("CreateVoucherTranGroup", VoucherTranGroup);
        //        }

        //        // GET: Categories/Delete/5
        //        public ActionResult DeleteVoucherTranGroup(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }
        //            VoucherTranGroup VoucherTranGroup = _voucherTranGroupRepository.FindById(id);

        //            if (VoucherTranGroup == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Delete";
        //            return View("CreateVoucherTranGroup", VoucherTranGroup);
        //        }

        //        // POST: Categories/Delete/5
        //        [HttpPost, ActionName("DeleteVoucherTranGroup")]
        //        public JsonResult DeleteVoucherTranGroupConfirmed(int? id)
        //        {
        //            try
        //            {
        //                VoucherTranGroup VoucherTranGroup = _voucherTranGroupRepository.FindById(id);
        //                _voucherTranGroupRepository.Delete(VoucherTranGroup);

        //                return Json(new { Success = 1, VoucherTranGroupId = VoucherTranGroup.VoucherTranGroupId, ex = "" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }
        //        #endregion

        //        #region Share Holding
        //        public ActionResult ShareHoldingList()
        //        {
        //            TempData["Message"] = "Data Load Successfully";
        //            TempData["Status"] = "1";
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

        //            var data = _shareHoldingRepository.GetAll()
        //                .Include(x => x.Acc_FiscalYears)
        //                .Where(c => c.ComId == HttpContext.Session.GetString("comid") && c.ShareHoldingId > 0 && !c.IsDelete).ToList();
        //            return View(data);
        //        }

        //        // GET: Categories/Create
        //        public ActionResult CreateShareHolding()
        //        {
        //            ViewBag.Title = "Create";
        //            ViewBag.FiscalyearId = _shareHoldingRepository.FiscalyearId();
        //            return View();
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult CreateShareHolding(ShareHolding ShareHolding)
        //        {
        //            var errors = ModelState.Where(x => x.Value.Errors.Any())
        //            .Select(x => new { x.Key, x.Value.Errors });

        //            string comid = HttpContext.Session.GetString("comid");
        //            string userid = HttpContext.Session.GetString("userid");

        //            if (ShareHolding.ShareHoldingId > 0)
        //            {
        //                ShareHolding.DateUpdated = DateTime.Now;
        //                ShareHolding.ComId = comid;

        //                if (ShareHolding.UserId == null)
        //                {
        //                    ShareHolding.UserId = userid;
        //                }
        //                ShareHolding.UpdateByUserId = userid;

        //                _shareHoldingRepository.Update(ShareHolding);

        //                TempData["Message"] = "Data Update Successfully";
        //                TempData["Status"] = "2";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ShareHolding.ShareHoldingId.ToString(), "Update", ShareHolding.ShareHolderName.ToString());
        //            }
        //            else
        //            {
        //                ShareHolding.UserId = userid;
        //                ShareHolding.ComId = comid;
        //                ShareHolding.DateAdded = DateTime.Now;

        //                _shareHoldingRepository.Add(ShareHolding);

        //                TempData["Message"] = "Data Save Successfully";
        //                TempData["Status"] = "1";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ShareHolding.ShareHoldingId.ToString(), "Create", ShareHolding.ShareHolderName.ToString());
        //                return RedirectToAction("ShareHoldingList");
        //            }
        //            return RedirectToAction("ShareHoldingList");
        //        }

        //        // GET: Categories/Edit/5
        //        public ActionResult EditShareHolding(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }

        //            ShareHolding ShareHolding = _shareHoldingRepository.FindById(id);
        //            ViewBag.FiscalyearId = _shareHoldingRepository.FiscalyearId();

        //            if (ShareHolding == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Edit";
        //            return View("CreateShareHolding", ShareHolding);
        //        }

        //        // GET: Categories/Delete/5
        //        public ActionResult DeleteShareHolding(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }

        //            ShareHolding ShareHolding = _shareHoldingRepository.FindById(id);
        //            ViewBag.FiscalyearId = _shareHoldingRepository.FiscalyearId();

        //            if (ShareHolding == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Delete";
        //            return View("CreateShareHolding", ShareHolding);
        //        }

        //        // POST: Categories/Delete/5
        //        [HttpPost, ActionName("DeleteShareHolding")]
        //        public JsonResult DeleteShareHoldingConfirmed(int? id)
        //        {
        //            try
        //            {
        //                ShareHolding ShareHolding = _shareHoldingRepository.FindById(id);

        //                _shareHoldingRepository.Delete(ShareHolding);

        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ShareHolding.ShareHoldingId.ToString(), "Delete", ShareHolding.ShareHolderName);

        //                return Json(new { Success = 1, ShareHoldingId = ShareHolding.ShareHoldingId, ex = "" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }
        //        #endregion

        //        #region Note Description

        //        public ActionResult NoteDescriptionList()
        //        {
        //            TempData["Message"] = "Data Load Successfully";
        //            TempData["Status"] = "1";

        //            ViewBag.FiscalYear = _shareHoldingRepository.FiscalyearId();
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

        //            return View();
        //        }     

        //        public IActionResult GetNoteDescription(int fiscalYearId)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid"));
        //                Microsoft.Extensions.Primitives.StringValues y = "";
        //                var x = Request.Form.TryGetValue("search[value]", out y);

        //                if (y.ToString().Length > 0)
        //                {
        //                    var query = from e in _context.NoteDescriptions.Include(x => x.Acc_FiscalYears)
        //                                .Where(p => p.ComId == comid && !p.IsDelete)
        //                                select new NotesView
        //                                {
        //                                    NoteDescriptionId = e.NoteDescriptionId,
        //                                    FYName = e.Acc_FiscalYears.FYName,
        //                                    SLNo = e.SLNo,
        //                                    NoteDetails = e.NoteDetails,
        //                                    NoteRemarks = e.NoteRemarks,
        //                                    NoteNo = e.NoteNo
        //                                };
        //                    var parser = new Parser<NotesView>(Request.Form, query);
        //                    return Json(parser.Parse());
        //                }
        //                else
        //                {
        //                    if (fiscalYearId != 0)
        //                    {
        //                        var query = from e in _context.NoteDescriptions.Include(x => x.Acc_FiscalYears)
        //                                .Where(p => p.ComId == comid && p.FiscalYearId == fiscalYearId && !p.IsDelete)
        //                                    select new NotesView
        //                                    {
        //                                        NoteDescriptionId = e.NoteDescriptionId,
        //                                        FYName = e.Acc_FiscalYears.FYName,
        //                                        SLNo = e.SLNo,
        //                                        NoteDetails = e.NoteDetails,
        //                                        NoteRemarks = e.NoteRemarks,
        //                                        NoteNo = e.NoteNo
        //                                    };
        //                        var parser = new Parser<NotesView>(Request.Form, query);
        //                        return Json(parser.Parse());
        //                    }
        //                    else
        //                    {
        //                        var query = from e in _context.NoteDescriptions.Include(x => x.Acc_FiscalYears)
        //                             .Where(p => p.ComId == comid && !p.IsDelete)
        //                                    select new NotesView
        //                                    {
        //                                        NoteDescriptionId = e.NoteDescriptionId,
        //                                        FYName = e.Acc_FiscalYears.FYName,
        //                                        SLNo = e.SLNo,
        //                                        NoteDetails = e.NoteDetails,
        //                                        NoteRemarks = e.NoteRemarks,
        //                                        NoteNo = e.NoteNo
        //                                    };

        //                        var parser = new Parser<NotesView>(Request.Form, query);
        //                        return Json(parser.Parse());
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = "0", error = ex.Message });
        //            }
        //        }

        //        // GET: Categories/Create
        //        public ActionResult CreateNoteDescription()
        //        {
        //            ViewBag.Title = "Create";
        //            ViewBag.FiscalyearId = _shareHoldingRepository.FiscalyearId();

        //            return View();
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult CreateNoteDescription(NoteDescription NoteDescription)
        //        {
        //            var errors = ModelState.Where(x => x.Value.Errors.Any())
        //            .Select(x => new { x.Key, x.Value.Errors });

        //            string comid = HttpContext.Session.GetString("comid");
        //            string userid = HttpContext.Session.GetString("userid");

        //            if (NoteDescription.NoteDescriptionId > 0)
        //            {
        //                NoteDescription.DateUpdated = DateTime.Now;
        //                NoteDescription.ComId = comid;

        //                if (NoteDescription.UserId == null)
        //                {
        //                    NoteDescription.UserId = userid;
        //                }
        //                NoteDescription.UpdateByUserId = userid;

        //                _noteDescriptionRepository.Update(NoteDescription);

        //                TempData["Message"] = "Data Update Successfully";
        //                TempData["Status"] = "2";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), NoteDescription.NoteDescriptionId.ToString(), "Update", NoteDescription.NoteDetails.ToString());
        //            }
        //            else
        //            {
        //                NoteDescription.UserId = userid;
        //                NoteDescription.ComId = comid;
        //                NoteDescription.DateAdded = DateTime.Now;

        //                _noteDescriptionRepository.Add(NoteDescription);

        //                TempData["Message"] = "Data Save Successfully";
        //                TempData["Status"] = "1";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), NoteDescription.NoteDescriptionId.ToString(), "Create", NoteDescription.NoteDetails.ToString());

        //                return RedirectToAction("NoteDescriptionList");
        //            }
        //            return RedirectToAction("NoteDescriptionList");
        //        }

        //        // GET: Categories/Edit/5
        //        public ActionResult EditNoteDescription(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }

        //            NoteDescription NoteDescription = _noteDescriptionRepository.FindById(id);
        //            ViewBag.FiscalyearId = _shareHoldingRepository.FiscalyearId();

        //            if (NoteDescription == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Edit";
        //            return View("CreateNoteDescription", NoteDescription);
        //        }


        //        //[Authorize]
        //        // GET: Categories/Delete/5
        //        public ActionResult DeleteNoteDescription(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return BadRequest();
        //            }

        //            NoteDescription NoteDescription = _noteDescriptionRepository.FindById(id);
        //            ViewBag.FiscalyearId = _shareHoldingRepository.FiscalyearId();

        //            if (NoteDescription == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewBag.Title = "Delete";
        //            return View("CreateNoteDescription", NoteDescription);
        //        }

        //        [HttpPost, ActionName("DeleteNoteDescription")]
        //        public JsonResult DeleteNoteDescriptionConfirmed(int? id)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid")).ToString();
        //                NoteDescription NoteDescription = _noteDescriptionRepository.FindById(id);

        //                _noteDescriptionRepository.Delete(NoteDescription);

        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), NoteDescription.NoteDescriptionId.ToString(), "Delete", NoteDescription.NoteDetails);

        //                return Json(new { Success = 1, NoteDescriptionId = NoteDescription.NoteDescriptionId, ex = "" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }
        //        #endregion

        //        #region ACC_Budget
        //        public ViewResult Acc_BudgetList(string FromDate, string ToDate)
        //        {
        //            var transactioncomid = HttpContext.Session.GetString("comid");
        //            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //            if (FromDate == null || FromDate == "")
        //            {
        //            }
        //            else
        //            {
        //                dtFrom = Convert.ToDateTime(FromDate);
        //            }
        //            if (ToDate == null || ToDate == "")
        //            {
        //            }
        //            else
        //            {
        //                dtTo = Convert.ToDateTime(ToDate);
        //            }
        //            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "Index", "");

        //            return View();
        //        }

        //        public ActionResult CreateAcc_Budget(int? FiscalYearId, int? FiscalMonthId, int? BudgetId = 0)
        //        {
        //            try
        //            {
        //                ViewBag.Title = "Entry";
        //                var transactioncomid = HttpContext.Session.GetString("comid");
        //                var Type = "VPC";

        //                Acc_BudgetMain Budgetsamplemodel = new Acc_BudgetMain();
        //                var transactioncompany = _context.Companys.Where(c => c.ComId.ToString() == transactioncomid).FirstOrDefault();
        //                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
        //                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

        //                ViewBag.PrdUnitId = _acc_BudgetRepository.PrdUnitId(); 
        //                ///////account head parent data for dropdown
        //                ViewBag.AccountParent = _acc_BudgetRepository.AccountParent();

        //                var ChartOfAccountsearch = _context.Acc_ChartOfAccounts.Where(c => c.AccId > 0 && c.AccType == "L" && c.ComId.ToString() == transactioncomid); 
        //                this.ViewBag.AccountSearch = ChartOfAccountsearch;

        //                if (Type == "VPC")
        //                {
        //                    ViewBag.Title = "Create";
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _acc_BudgetRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _acc_BudgetRepository.Account1();
        //                    }

        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId.ToString() == transactioncomid);

        //                    if (BudgetId == 0)
        //                    {
        //                        if (FiscalYearId != null && FiscalMonthId != null && FiscalMonthId > 0)
        //                        {
        //                            Acc_BudgetMain budgetMain = _context.Acc_BudgetMains.Where(m => m.FiscalYearId == FiscalYearId && m.FiscalMonthId == FiscalMonthId).FirstOrDefault();
        //                            if (budgetMain != null)
        //                            {
        //                                ViewBag.Title = "Edit";

        //                                this.ViewBag.Country = _acc_BudgetRepository.Country(FiscalYearId, FiscalMonthId);
        //                                this.ViewBag.AccountMain = _acc_BudgetRepository.AccountMain(FiscalYearId, FiscalMonthId);
        //                                this.ViewBag.FiscalYearId = _acc_BudgetRepository.FiscalYearId(FiscalYearId, FiscalMonthId);
        //                                this.ViewBag.FiscalMonthId = _acc_BudgetRepository.FiscalMonthId(FiscalYearId, FiscalMonthId); 

        //                                return View(budgetMain);
        //                            }
        //                        }
        //                        this.ViewBag.Country = _acc_BudgetRepository.Country1();
        //                        this.ViewBag.AccountMain = _acc_BudgetRepository.AccountMain1();
        //                        this.ViewBag.FiscalYearId = _acc_BudgetRepository.FiscalYearId1();
        //                        this.ViewBag.FiscalMonthId = _acc_BudgetRepository.FiscalMonthId1(); 
        //                        return View(Budgetsamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_BudgetMain budgetMain = _acc_BudgetRepository.FindById(BudgetId);

        //                        if (budgetMain.isPosted == true)
        //                        {
        //                            return NotFound();
        //                        }
        //                        if (budgetMain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = _acc_BudgetRepository.Country(FiscalYearId, FiscalMonthId);
        //                        this.ViewBag.AccountMain = _acc_BudgetRepository.AccountMain(FiscalYearId, FiscalMonthId);
        //                        this.ViewBag.FiscalYearId = _acc_BudgetRepository.FiscalYearId(FiscalYearId, FiscalMonthId);
        //                        this.ViewBag.FiscalMonthId = _acc_BudgetRepository.FiscalMonthId(FiscalYearId, FiscalMonthId);

        //                        return View(budgetMain);
        //                    }
        //                }
        //                return View();
        //            }
        //            catch (Exception ex)
        //            {

        //                throw ex;
        //            }
        //        }    

        //        public JsonResult CallComboSubSectionList()
        //        {
        //            try
        //            {
        //                var SubSectionList = _context.Cat_SubSection.Select(e => new
        //                {
        //                    value = e.SubSectId,
        //                    display = e.SubSectName
        //                }).ToList();
        //                return Json(SubSectionList);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }

        //        [HttpPost]
        //        public JsonResult AccountInfo(int id)
        //        {
        //            try
        //            {
        //                Acc_ChartOfAccount chartofaccount = _context.Acc_ChartOfAccounts.Where(y => y.AccId == id).SingleOrDefault();
        //                return Json(chartofaccount);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { success = false, values = ex.Message.ToString() });
        //            }
        //        }

        //        [HttpPost]
        //        public JsonResult FYMonthInfo(int id)
        //        {
        //            try
        //            {
        //                var comid = HttpContext.Session.GetString("comid");
        //                int fyid = (_context.Acc_FiscalYears.Where(x => x.FiscalYearId == id).Select(x => x.FYId).FirstOrDefault());

        //                List<Acc_FiscalMonth> fiscalmonthlist = _context.Acc_FiscalMonths.Where(x => x.FYId == fyid && x.ComId.ToString() == comid).ToList();

        //                List<SelectListItem> fiscalmonthselectlist = new List<SelectListItem>();
        //                if (fiscalmonthlist != null)
        //                {
        //                    foreach (Acc_FiscalMonth x in fiscalmonthlist)
        //                    {
        //                        fiscalmonthselectlist.Add(new SelectListItem { Text = x.MonthName, Value = x.FiscalMonthId.ToString() });
        //                    }
        //                }
        //                return Json(new SelectList(fiscalmonthselectlist, "Value", "Text"));
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { success = false, values = ex.Message.ToString() });
        //            }
        //        }

        //        // POST: /Budget/Create
        //        [HttpPost]
        //        public JsonResult CreateAcc_Budget(Acc_BudgetMain budgetMain)
        //        {
        //            try
        //            {
        //                {
        //                    if (budgetMain.BudgetId > 0)
        //                    {
        //                        var CurrentBudgetSub = _context.Acc_BudgetSubs.Where(p => p.BudgetId == budgetMain.BudgetId);
        //                        var CurrentBudgetSubSection = _context.Acc_BudgetSubSections.Where(p => p.BudgetId == budgetMain.BudgetId);

        //                        foreach (Acc_BudgetSub ss in CurrentBudgetSub)
        //                            _context.Acc_BudgetSubs.Remove(ss);
        //                        foreach (Acc_BudgetSub ss in budgetMain.BudgetSubs)
        //                        {
        //                            _context.Acc_BudgetSubs.Add(ss);
        //                            foreach (Acc_BudgetSubSection sss in ss.BudgetSubSections)
        //                            {
        //                                _context.Acc_BudgetSubSections.Add(sss);
        //                            }
        //                        }

        //                        _acc_BudgetRepository.Update(budgetMain);
        //                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), budgetMain.BudgetId.ToString(), "Update", budgetMain.BudgetId.ToString());
        //                    }
        //                    else
        //                    {
        //                        budgetMain.UserId = HttpContext.Session.GetString("userid");
        //                        budgetMain.ComId = HttpContext.Session.GetString("comid");
        //                        budgetMain.DateAdded = DateTime.Now;

        //                        var x = BudgetNoMaker(budgetMain.ComId.ToString(), budgetMain.FiscalYearId, budgetMain.FiscalMonthId); // nned to work..
        //                        budgetMain.BudgetNo = (x[0]);
        //                        budgetMain.BudgetSerialId = int.Parse(x[1]);

        //                        _acc_BudgetRepository.Add(budgetMain);
        //                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), budgetMain.BudgetId.ToString(), "Create", budgetMain.BudgetId.ToString());
        //                    }
        //                    return Json(new { Success = 1, BudgetID = budgetMain.BudgetId, ex = "" });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }
        //            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        //        }

        //        public string[] BudgetNoMaker(string comid, int? fiscalyearid = 0, int? fiscalmonthid = 0)
        //        {
        //            string[] FinalAccCode = new string[2];
        //            var input = 0;
        //            int length = 6;
        //            int maxBudgetid = 0;
        //            var maxnowithpadleftresult = "";
        //            string voucernocreatestyle = "";

        //            if (fiscalmonthid > 0)
        //            {
        //                maxBudgetid = _context.Acc_BudgetMains.Where(x => x.ComId.ToString() == comid && x.FiscalYearId == fiscalyearid && x.FiscalMonthId == fiscalmonthid).Max(x => x.BudgetSerialId);
        //                input = maxBudgetid + 1;
        //                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
        //                FinalAccCode[0] = "B-" + maxnowithpadleftresult.ToString();
        //                FinalAccCode[1] = input.ToString();
        //            }
        //            else
        //            {
        //                maxBudgetid = _context.Acc_BudgetMains.Where(x => x.ComId.ToString() == comid && x.FiscalYearId == fiscalyearid).Max(x => x.BudgetSerialId);
        //                input = maxBudgetid + 1;
        //                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
        //                FinalAccCode[0] = "B-" + maxnowithpadleftresult.ToString();
        //                FinalAccCode[1] = input.ToString();
        //            }
        //            return FinalAccCode;
        //        }

        //        // GET: /Budget/Edit/5
        //        public ActionResult EditAcc_Budget(string Type, int? BudgetId)
        //        {
        //            try
        //            {
        //                ViewBag.Title = "Entry";
        //                var transactioncomid = HttpContext.Session.GetString("comid");

        //                if (Type == null)
        //                {
        //                    Type = "VPC";
        //                }
        //                var Budgetsamplemodel = _context.BudgetMains.Include(x => x.BudgetDetails).Where(x => x.BudgetMainId == BudgetId).ToList();

        //                var transactioncompany = _context.Companys.Where(c => c.ComId.ToString() == transactioncomid).FirstOrDefault();
        //                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
        //                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);
        //                ViewBag.PrdUnitId = _acc_BudgetRepository.PrdUnitId();                                                                                                                                             ///////account head parent data for dropdown
        //                ViewBag.AccountParent = _acc_BudgetRepository.AccountParent();

        //                if (transactioncompany.isMultiDebitCredit == true)
        //                {
        //                    this.ViewBag.Account = _acc_BudgetRepository.Account();
        //                }
        //                else
        //                {
        //                    this.ViewBag.Account = _acc_BudgetRepository.Account1();
        //                }
        //                ///////only cash item when multi debit credit of then it enable
        //                this.ViewBag.AccountMain = _acc_BudgetRepository.AccountMain1();
        //                this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId.ToString() == transactioncomid);
        //                return View("Create", Budgetsamplemodel);
        //            }
        //            catch (Exception ex)
        //            {
        //                string abcd = ex.InnerException.InnerException.Message.ToString();
        //                throw ex;
        //            }
        //        }

        //        // GET: /Budget/Delete/5
        //        public ActionResult DeleteAcc_Budget(int? BudgetId)
        //        {
        //            try
        //            {
        //                var transactioncomid = HttpContext.Session.GetString("comid");
        //                if (BudgetId == null)
        //                {
        //                    return NotFound();
        //                }
        //                Acc_BudgetMain budgetMain = _acc_BudgetRepository.FindById(BudgetId);

        //                if (budgetMain.isPosted == true)
        //                {
        //                    return NotFound();
        //                }

        //                if (budgetMain == null)
        //                {
        //                    return NotFound();
        //                }
        //                ViewBag.Title = "Delete";

        //                var transactioncompany = _context.Companys.Where(c => c.ComId.ToString() == transactioncomid).FirstOrDefault();
        //                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
        //                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);
        //                ViewBag.PrdUnitId = _acc_BudgetRepository.PrdUnitId();
        //                ViewBag.AccountParent = _acc_BudgetRepository.AccountParent();

        //                if (transactioncompany.isMultiDebitCredit == true)
        //                {
        //                    this.ViewBag.Account = _acc_BudgetRepository.Account();
        //                }
        //                else
        //                {
        //                    this.ViewBag.Account = _acc_BudgetRepository.Account1();
        //                }
        //                ///////only cash item when multi debit credit of then it enable
        //                this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId.ToString() == transactioncomid);
        //                this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", budgetMain.CountryId);
        //                this.ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(p => p.ComId.ToString() == transactioncomid && p.FiscalYearId > 0), "FiscalYearId", "FYName", budgetMain.FiscalYearId).ToList(); 
        //                this.ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(p => p.ComId.ToString() == transactioncomid && p.FiscalMonthId == budgetMain.FiscalMonthId), "FiscalMonthId", "MonthName", budgetMain.FiscalMonthId).ToList();
        //                this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", budgetMain.CountryId);

        //                var ChartOfAccountsearch = _context.Acc_ChartOfAccounts.Where(c => c.AccId > 0 && c.AccType == "L" && c.ComId.ToString() == transactioncomid); 
        //                this.ViewBag.AccountSearch = ChartOfAccountsearch;
        //                return View("Create", budgetMain);
        //            }
        //            catch (Exception ex)
        //            {
        //                string abcd = ex.InnerException.InnerException.Message.ToString();
        //                throw ex;
        //            }
        //        }

        //        // POST: /Budget/Delete/5
        //        [HttpPost, ActionName("DeleteAcc_Budget")]
        //        public JsonResult DeleteAcc_BudgetConfirmed(int id)
        //        {
        //            try
        //            {
        //                Acc_BudgetMain budgetMain = _acc_BudgetRepository.FindById(id);
        //                _acc_BudgetRepository.Delete(budgetMain);
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), budgetMain.BudgetNo.ToString(), "Delete", budgetMain.BudgetNo.ToString());

        //                return Json(new { Success = 1, BudgetID = budgetMain.BudgetId, ex = "" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
        //        }

        //        [HttpPost]
        //        public JsonResult ProductInfo1(int id)
        //        {
        //            try
        //            {
        //                var product = _context.Products.Where(y => y.ProductId == id).SingleOrDefault();
        //                return Json(product);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { success = false, values = ex.Message.ToString() });
        //            }
        //        }
        //        #endregion

        //        #region ACC Voucher

        //        [HttpPost, ActionName("SetSessionInd")]
        //        //[ValidateAntiForgeryToken]
        //        public JsonResult SetSessionInd(string reporttype, string action, string reportid)
        //        {
        //            try
        //            {
        //                HttpContext.Session.SetString("ReportType", reporttype);
        //                return Json(new { Success = 1 });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //            }
        //            return Json(new { Success = 0, ex = new Exception("Unable to Set").Message.ToString() });
        //        }

        //        // GET: /Voucher/
        //        public ViewResult AccVoucherList(string FromDate, string ToDate, string UserList)
        //        {
        //            ViewBag.Userlist = _accVoucherRepository.UserList();

        //            var comid = HttpContext.Session.GetString("comid");
        //            var userid = HttpContext.Session.GetString("userid");

        //            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
        //            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

        //            if (FromDate == null || FromDate == "")
        //            {
        //            }
        //            else
        //            {
        //                dtFrom = Convert.ToDateTime(FromDate);
        //            }
        //            if (ToDate == null || ToDate == "")
        //            {
        //            }
        //            else
        //            {
        //                dtTo = Convert.ToDateTime(ToDate);
        //            }
        //            if (UserList == null)
        //            {
        //                UserList = userid;
        //            }

        //            ViewBag.Acc_VoucherNoPrefix = _context.Acc_VoucherNoPrefixes.Where(x => x.ComId == comid).Include(x => x.vVoucherTypes).Where(x => x.isVisible == true && x.vVoucherTypes.isSystem == false).ToList();

        //            var fiscalYear = _context.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
        //            if (fiscalYear != null)
        //            {
        //                ViewBag.FiscalYearId = _accVoucherRepository.FiscalYearId1();
        //                ViewBag.FiscalMonthId = _accVoucherRepository.FiscalMonthId1();
        //            }
        //            else
        //            {
        //                ViewBag.FiscalYearId = _accVoucherRepository.FiscalYearId();
        //                ViewBag.FiscalMonthId = _accVoucherRepository.FiscalMonthId();
        //            }

        //            ViewBag.IntegrationSettingMainId = _accVoucherRepository.IntegrationSettingMainId();

        //            if (UserList == null)
        //            {
        //                var X = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == comid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)).ToList();
        //                return View(X);
        //            }
        //            else
        //            {
        //                if (UserList == "1")
        //                {
        //                    var X = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == comid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)).ToList();

        //                    if (X.Count > 0)
        //                    {
        //                        return View(X);
        //                    }
        //                    else
        //                    {
        //                        X = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == comid).OrderByDescending(x => x.VoucherId).Take(5).ToList();
        //                        return View(X);
        //                    }
        //                }
        //                else
        //                {
        //                    var X = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == comid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo) && p.UserId == UserList).ToList();

        //                    if (X.Count > 0)
        //                    {
        //                        return View(X);
        //                    }
        //                    else
        //                    {
        //                        X = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).Where(p => p.ComId == comid && p.UserId == UserList).OrderByDescending(x => x.VoucherId).Take(5).ToList();
        //                        return View(X);
        //                    }
        //                }
        //            }
        //            return View();
        //        }     

        //        public IActionResult GetAccVoucher(string FromDate, string ToDate, string UserList, int isAll)
        //        {
        //            try
        //            {
        //                var comid = (HttpContext.Session.GetString("comid"));

        //                DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
        //                DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

        //                if (FromDate == null || FromDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtFrom = Convert.ToDateTime(FromDate);
        //                }
        //                if (ToDate == null || ToDate == "")
        //                {
        //                }
        //                else
        //                {
        //                    dtTo = Convert.ToDateTime(ToDate);
        //                }

        //                Microsoft.Extensions.Primitives.StringValues y = "";

        //                var x = Request.Form.TryGetValue("search[value]", out y);

        //                if (y.ToString().Length > 0)
        //                {
        //                    var query = from e in _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType)
        //                                .Where(p => p.ComId == comid)
        //                                select new VoucherView1
        //                                {
        //                                    VoucherId = e.VoucherId,
        //                                    VoucherNo = e.VoucherNo,
        //                                    VoucherDate = e.VoucherDate.ToString("dd-MMM-yy"),
        //                                    VoucherDesc = e.VoucherDesc,
        //                                    VAmount = e.VAmount,
        //                                    Currency = e.Acc_Currency.CurrencyShortName,
        //                                    isPosted = e.isPosted,
        //                                    VoucherTypeName = e.Acc_VoucherType.VoucherTypeName,
        //                                    VoucherTypeNameShort = e.Acc_VoucherType.VoucherTypeNameShort,
        //                                    Status = e.isPosted != false ? "Posted" : "Not Posted"
        //                                };

        //                    var parser = new Parser<VoucherView1>(Request.Form, query);
        //                    return Json(parser.Parse());
        //                }
        //                else
        //                {
        //                    if (UserList == null)
        //                    {
        //                        var query = from e in _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType)
        //                                   .Where(p => p.ComId == comid && p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)
        //                                    select new VoucherView1
        //                                    {
        //                                        VoucherId = e.VoucherId,
        //                                        VoucherNo = e.VoucherNo,
        //                                        VoucherDate = e.VoucherDate.ToString("dd-MMM-yy"),
        //                                        VoucherDesc = e.VoucherDesc,
        //                                        VAmount = e.VAmount,
        //                                        Currency = e.Acc_Currency.CurrencyShortName,
        //                                        isPosted = e.isPosted,
        //                                        VoucherTypeName = e.Acc_VoucherType.VoucherTypeName,
        //                                        VoucherTypeNameShort = e.Acc_VoucherType.VoucherTypeNameShort,
        //                                        Status = e.isPosted != false ? "Posted" : "Not Posted"
        //                                    };
        //                        var parser = new Parser<VoucherView1>(Request.Form, query);
        //                        return Json(parser.Parse());
        //                    }
        //                    else
        //                    {
        //                        if (UserList == "1")
        //                        {
        //                            var query = from e in _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType)
        //                                   .Where(p => p.ComId == comid && p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo)
        //                                        select new VoucherView1
        //                                        {
        //                                            VoucherId = e.VoucherId,
        //                                            VoucherNo = e.VoucherNo,
        //                                            VoucherDate = e.VoucherDate.ToString("dd-MMM-yy"),
        //                                            VoucherDesc = e.VoucherDesc,
        //                                            VAmount = e.VAmount,
        //                                            Currency = e.Acc_Currency.CurrencyShortName,
        //                                            isPosted = e.isPosted,
        //                                            VoucherTypeName = e.Acc_VoucherType.VoucherTypeName,
        //                                            VoucherTypeNameShort = e.Acc_VoucherType.VoucherTypeNameShort,
        //                                            Status = e.isPosted != false ? "Posted" : "Not Posted"
        //                                        };
        //                            var parser = new Parser<VoucherView1>(Request.Form, query);
        //                            return Json(parser.Parse());
        //                        }
        //                        else
        //                        {
        //                            var query = from e in _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType)
        //                                   .Where(p => p.ComId == comid && (p.VoucherDate >= dtFrom && p.VoucherDate <= dtTo) && p.UserId == UserList)
        //                                        select new VoucherView1
        //                                        {
        //                                            VoucherId = e.VoucherId,
        //                                            VoucherNo = e.VoucherNo,
        //                                            VoucherDate = e.VoucherDate.ToString("dd-MMM-yy"),
        //                                            VoucherDesc = e.VoucherDesc,
        //                                            VAmount = e.VAmount,
        //                                            Currency = e.Acc_Currency.CurrencyShortName,
        //                                            isPosted = e.isPosted,
        //                                            VoucherTypeName = e.Acc_VoucherType.VoucherTypeName,
        //                                            VoucherTypeNameShort = e.Acc_VoucherType.VoucherTypeNameShort,
        //                                            Status = e.isPosted != false ? "Posted" : "Not Posted"
        //                                        };
        //                            var parser = new Parser<VoucherView1>(Request.Form, query);
        //                            return Json(parser.Parse());
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = "0", error = ex.Message });
        //            }
        //        }

        //        [HttpGet]
        //        public IActionResult GetFiscalMonth1(int id)
        //        {
        //            var data = _context.Acc_FiscalMonths.OrderByDescending(y => y.MonthName).Where(m => m.FYId == id).ToList();
        //            return Json(data);
        //        }

        //        public ViewResult AccVoucherDetails(int id)
        //        {
        //            Acc_VoucherMain Vouchermain = _context.Acc_VoucherMains.Find(id);
        //            return View(Vouchermain);
        //        }

        //        public ViewResult PrintView(int id)
        //        {
        //            Acc_VoucherMain Vouchermain = _context.Acc_VoucherMains.Find(id);
        //            return View(Vouchermain);
        //        }

        //        public ActionResult asdf(int? id)
        //        {
        //            return RedirectToAction("ExportPdf", "Voucher", new { id = id });
        //        }

        //        public ActionResult PrintCheck(int? id, string type)
        //        {            
        //            string callBackUrl = _accVoucherRepository.PrintCheck(id, type);
        //            return Redirect(callBackUrl);
        //        }

        //        public ActionResult Print1(int? id, string type)
        //        {            
        //            string callBackUrl = _accVoucherRepository.Print1(id, type);
        //            return Redirect(callBackUrl);
        //        }

        //        public JsonResult CallComboSubSectionList1()
        //        {
        //            try
        //            {
        //                var SubSectionList = _context.Cat_SubSection.Select(e => new
        //                {
        //                    value = e.SubSectId,
        //                    display = e.SubSectName
        //                }).ToList();
        //                return Json(SubSectionList);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }

        //        struct MyObj
        //        {
        //            public int value { get; set; }
        //            public string display { get; set; }
        //        }

        //        public ActionResult CreateAccVoucher(string Type, int VoucherId = 0, int FiscalMonthId = 0, int? IntegrationSettingMainId = 0)
        //        {
        //            try
        //            {
        //                string comid = HttpContext.Session.GetString("comid");
        //                var transactioncomid = HttpContext.Session.GetString("comid");
        //                var lastvoucher = _context.Acc_VoucherMains.Where(x => x.Acc_VoucherType.VoucherTypeNameShort == Type && x.ComId == transactioncomid).OrderByDescending(x => x.VoucherId).FirstOrDefault();

        //                if (Type == null)
        //                {
        //                    Type = "VPC";
        //                }
        //                HttpContext.Session.SetString("vouchertype", Type);

        //                string vouchertypeid = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x => x.VoucherTypeId.ToString()).FirstOrDefault();
        //                Acc_VoucherMain vouchersamplemodel = new Acc_VoucherMain();

        //                if (lastvoucher != null)
        //                {
        //                    vouchersamplemodel.VoucherDate = lastvoucher.VoucherDate;
        //                    vouchersamplemodel.LastVoucherNo = lastvoucher.VoucherNo;
        //                }
        //                else
        //                {
        //                    vouchersamplemodel.VoucherDate = DateTime.Now.Date;
        //                }
        //                var transactioncompany = _context.Companys.Include(x => x.vCountryCompany).Where(c => c.CompanySecretCode == transactioncomid).FirstOrDefault();
        //                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
        //                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

        //                this.ViewBag.Acc_VoucherType = _accVoucherRepository.Acc_VoucherType(Type);
        //                vouchersamplemodel.VoucherTypeId = int.Parse(vouchertypeid.ToString());

        //                vouchersamplemodel.Acc_VoucherType = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

        //                ViewBag.PrdUnitId = _accVoucherRepository.PrdUnitId();
        //                ViewBag.Acc_FiscalYear = _accVoucherRepository.FiscalYearId();
        //                ViewBag.Acc_FiscalMonth = _accVoucherRepository.FiscalMonthId();
        //                ViewBag.EmpId = _accVoucherRepository.EmpId();
        //                ViewBag.CustomerId = _accVoucherRepository.CustomerId();
        //                ViewBag.SupplierId = _accVoucherRepository.SupplierId();
        //                ViewBag.VoucherTranGroupArray = _accVoucherRepository.VoucherTranGroupArray();
        //                ViewBag.VoucherTranGroupId = _accVoucherRepository.VoucherTranGroupId();
        //                ViewBag.VoucherTranGroupIdRow = _accVoucherRepository.VoucherTranGroupIdRow();

        //                if (Type == "VPC")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();

        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain(VoucherId);

        //                        if (Vouchermain.isPosted == true)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        var itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VRC")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();

        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain(VoucherId);

        //                        if (Vouchermain.isPosted == true)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VRB")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == true)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VPB")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        try
        //                        {
        //                            if (FiscalMonthId > 0 && IntegrationSettingMainId > 0)
        //                            {
        //                                _accVoucherRepository.vouchersamplemodel(FiscalMonthId, IntegrationSettingMainId);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw ex;
        //                        }
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == true)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VCR")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == true)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true || p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VJR")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        try
        //                        {
        //                            ///integration Part -- Need more work for issue and mrr /grr
        //                            if (FiscalMonthId > 0 && IntegrationSettingMainId > 0)
        //                            {
        //                                _accVoucherRepository.vouchersamplemodel(FiscalMonthId, IntegrationSettingMainId);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw ex;
        //                        }
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == true)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VLC")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();
        //                        if (Vouchermain.isPosted == true)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }

        //                        ViewBag.Title = "Edit";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                return View();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        public ActionResult ViewAccVoucher(string Type, int VoucherId = 0, int FiscalMonthId = 0, int? IntegrationSettingMainId = 0)
        //        {
        //            try
        //            {
        //                string comid = HttpContext.Session.GetString("comid");
        //                var transactioncomid = HttpContext.Session.GetString("comid");
        //                var lastvoucher = _context.Acc_VoucherMains.Where(x => x.Acc_VoucherType.VoucherTypeNameShort == Type && x.ComId == transactioncomid).OrderByDescending(x => x.VoucherId).FirstOrDefault();

        //                if (Type == null)
        //                {
        //                    Type = "VPC";
        //                }
        //                HttpContext.Session.SetString("vouchertype", Type);
        //                string vouchertypeid = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x => x.VoucherTypeId.ToString()).FirstOrDefault();
        //                Acc_VoucherMain vouchersamplemodel = new Acc_VoucherMain();

        //                if (lastvoucher != null)
        //                {
        //                    vouchersamplemodel.VoucherDate = lastvoucher.VoucherDate;
        //                    vouchersamplemodel.LastVoucherNo = lastvoucher.VoucherNo;
        //                }
        //                else
        //                {
        //                    vouchersamplemodel.VoucherDate = DateTime.Now.Date;
        //                }

        //                var transactioncompany = _context.Companys.Include(x => x.vCountryCompany).Where(c => c.CompanySecretCode == transactioncomid).FirstOrDefault();
        //                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
        //                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

        //                this.ViewBag.Acc_VoucherType = _accVoucherRepository.Acc_VoucherType(Type);
        //                vouchersamplemodel.VoucherTypeId = int.Parse(vouchertypeid.ToString());

        //                vouchersamplemodel.Acc_VoucherType = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

        //                ViewBag.PrdUnitId = _accVoucherRepository.PrdUnitId();
        //                ViewBag.Acc_FiscalYear = _accVoucherRepository.FiscalYearId();
        //                ViewBag.Acc_FiscalMonth = _accVoucherRepository.FiscalMonthId();
        //                ViewBag.EmpId = _accVoucherRepository.EmpId();
        //                ViewBag.CustomerId = _accVoucherRepository.CustomerId();
        //                ViewBag.SupplierId = _accVoucherRepository.SupplierId();
        //                ViewBag.VoucherTranGroupArray = _accVoucherRepository.VoucherTranGroupArray();
        //                ViewBag.VoucherTranGroupId = _accVoucherRepository.VoucherTranGroupId();
        //                ViewBag.VoucherTranGroupIdRow = _accVoucherRepository.VoucherTranGroupIdRow();

        //                if (Type == "VPC")
        //                {
        //                    ViewBag.Title = "View";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }

        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        ///////only cash item when multi debit credit of then it enable
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View("CreateAccVoucher", vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == false)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "View";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View("CreateAccVoucher", Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VRC")
        //                {
        //                    ViewBag.Title = "View";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View("CreateAccVoucher", vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == false)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "View";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View("CreateAccVoucher", Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VRB")
        //                {
        //                    ViewBag.Title = "View";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View("CreateAccVoucher", vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == false)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "View";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View("CreateAccVoucher", Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VPB")
        //                {
        //                    ViewBag.Title = "View";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        try
        //                        {
        //                            if (FiscalMonthId > 0 && IntegrationSettingMainId > 0)
        //                            {
        //                                _accVoucherRepository.vouchersamplemodel(FiscalMonthId,IntegrationSettingMainId);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw ex;
        //                        }
        //                        return View("CreateAccVoucher", vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == false)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "View";

        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View("CreateAccVoucher", Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VCR")
        //                {
        //                    ViewBag.Title = "View";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View("CreateAccVoucher", vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == false)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "View";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true || p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View("CreateAccVoucher", Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VJR")
        //                {
        //                    ViewBag.Title = "View";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        try
        //                        {
        //                            ///integration Part -- Need more work for issue and mrr /grr
        //                            if (FiscalMonthId > 0 && IntegrationSettingMainId > 0)
        //                            {
        //                                _accVoucherRepository.vouchersamplemodel(FiscalMonthId, IntegrationSettingMainId);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw ex;
        //                        }
        //                        return View("CreateAccVoucher", vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain.isPosted == false)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "View";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View("CreateAccVoucher", Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VLC")
        //                {
        //                    ViewBag.Title = "View";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L" && c.IsItemAccmulateddDep == false && c.IsItemDepExp == false); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View("CreateAccVoucher", vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();
        //                        if (Vouchermain.isPosted == false)
        //                        {
        //                            return BadRequest();
        //                        }
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }

        //                        ViewBag.Title = "View";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View("CreateAccVoucher", Vouchermain);
        //                    }
        //                }
        //                return View("CreateAccVoucher");
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }      

        //        [HttpPost]
        //        public JsonResult AccountInfo1(int id)
        //        {
        //            try
        //            {
        //                Acc_ChartOfAccount_view abc = new Acc_ChartOfAccount_view();
        //                Acc_ChartOfAccount chartofaccount = _context.Acc_ChartOfAccounts.Where(y => y.AccId == id).SingleOrDefault();

        //                if (chartofaccount != null)
        //                {
        //                    abc.AccId = chartofaccount.AccId;
        //                    abc.ParentId = chartofaccount.ParentID;
        //                    abc.AccountParent = _context.Acc_ChartOfAccounts.Where(y => y.AccId == chartofaccount.ParentID).SingleOrDefault().AccName;
        //                    abc.IsChkRef = chartofaccount.IsChkRef == true ? 1 : 0;
        //                    abc.IsBankItem = chartofaccount.IsBankItem == true ? 1 : 0;
        //                    abc.IsCashItem = chartofaccount.IsCashItem == true ? 1 : 0;
        //                    abc.CurrencyId = chartofaccount.CountryID;
        //                }
        //                return Json(abc);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { success = false, values = ex.Message.ToString() });
        //            }
        //        }

        //        public JsonResult AccountReferance(string query)
        //        {
        //            string ComId = HttpContext.Session.GetString("comid");

        //            var Referancedb = _context.Acc_VoucherMains
        //            .Where(x => x.ComId == ComId && (x.Referance).ToLower().Contains(query.ToLower()))
        //            .Select(m => new { Referance = m.Referance })
        //            .Distinct()
        //            .ToList();
        //            List<SelectListItem> Referance = new List<SelectListItem>();

        //            if (Referancedb != null)
        //            {
        //                foreach (var x in Referancedb)
        //                {
        //                    Referance.Add(new SelectListItem { Text = x.Referance, Value = "0" });
        //                }
        //            }
        //            return Json(Referance);
        //        }

        //        public JsonResult AccountReferanceTwo(string query)
        //        {
        //            string ComId = HttpContext.Session.GetString("comid");

        //            var Referancedb = _context.Acc_VoucherMains
        //               .Where(x => x.ComId == ComId && (x.ReferanceTwo).ToLower().Contains(query.ToLower()))
        //               .Select(m => new { Referance = m.ReferanceTwo })
        //               .Distinct()
        //               .ToList();

        //            List<SelectListItem> Referance = new List<SelectListItem>();
        //            if (Referancedb != null)
        //            {
        //                foreach (var x in Referancedb)
        //                {
        //                    Referance.Add(new SelectListItem { Text = x.Referance, Value = "0" });

        //                }
        //            }
        //            return Json(Referance);
        //        }

        //        public JsonResult AccountReferanceThree(string query)
        //        {
        //            string ComId = HttpContext.Session.GetString("comid");

        //            var Referancedb = _context.Acc_VoucherMains
        //               .Where(x => x.ComId == ComId && (x.ReferanceThree).ToLower().Contains(query.ToLower()))
        //               .Select(m => new { Referance = m.ReferanceThree })
        //               .Distinct()
        //               .ToList();

        //            List<SelectListItem> Referance = new List<SelectListItem>();
        //            if (Referancedb != null)
        //            {
        //                foreach (var x in Referancedb)
        //                {
        //                    Referance.Add(new SelectListItem { Text = x.Referance, Value = "0" });
        //                }
        //            }
        //            return Json(Referance);
        //        }

        //        public JsonResult NoteOne(string query)
        //        {
        //            string ComId = HttpContext.Session.GetString("comid");
        //            var Referancedb = _context.Acc_VoucherSubs
        //               .Where(x => x.Acc_VoucherMain.ComId == ComId && (x.Note1).ToLower().Contains(query.ToLower()))
        //               .Select(m => new { Referance = m.Note1 })
        //               .Distinct()
        //               .ToList();

        //            List<SelectListItem> Referance = new List<SelectListItem>();
        //            if (Referancedb != null)
        //            {
        //                foreach (var x in Referancedb)
        //                {
        //                    Referance.Add(new SelectListItem { Text = x.Referance, Value = "0" });
        //                }
        //            }
        //            return Json(Referance);
        //        }

        //        public JsonResult NoteTwo(string query)
        //        {
        //            string ComId = HttpContext.Session.GetString("comid");
        //            var Referancedb = _context.Acc_VoucherSubs
        //               .Where(x => x.Acc_VoucherMain.ComId == ComId && (x.Note2).ToLower().Contains(query.ToLower()))
        //               .Select(m => new { Referance = m.Note2 })
        //               .Distinct()
        //               .ToList();

        //            List<SelectListItem> Referance = new List<SelectListItem>();
        //            if (Referancedb != null)
        //            {
        //                foreach (var x in Referancedb)
        //                {
        //                    Referance.Add(new SelectListItem { Text = x.Referance, Value = "0" });
        //                }
        //            }
        //            return Json(Referance);
        //        }

        //        // POST: /Voucher/Create
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public IActionResult CreateAccVoucher(Acc_VoucherMain acc_voucherMain/*, List<HR_Loan_Data_HouseBuilding> details*/)
        //        {
        //            try
        //            {
        //                var text = "";
        //                var comid = HttpContext.Session.GetString("comid");
        //                var userid = HttpContext.Session.GetString("userid");

        //                var lockCheck = _context.HR_ProcessLock
        //                .Where(p => p.LockType.Contains("Account Lock") && p.DtDate.Date <= acc_voucherMain.VoucherDate.Date && p.DtToDate.Value.Date >= acc_voucherMain.VoucherDate.Date
        //                    && p.IsLock == true).FirstOrDefault();

        //                if (lockCheck != null)
        //                {
        //                    return Json(new { Success = 0, ex = "Account Lock this date!!!" });
        //                }

        //                var defaultcurrencyid = HttpContext.Session.GetInt32("defaultcurrencyid");
        //                var defaultcurrencyname = HttpContext.Session.GetString("defaultcurrencyname");
        //                acc_voucherMain.CountryIdLocal = int.Parse(defaultcurrencyid.ToString());
        //                acc_voucherMain.VAmount = double.Parse(_clsProc.GTRFormatCurrencyBDT(acc_voucherMain.VAmount.ToString()));
        //                acc_voucherMain.vAmountInWords = _clsProc.GTRInwordsFormatBD(acc_voucherMain.VAmount.ToString(), "", "");
        //                acc_voucherMain.vAmountLocalInWords = _clsProc.GTRInwordsFormatBD(acc_voucherMain.VAmountLocal.ToString(), "", "");
        //                acc_voucherMain.DateUpdated = DateTime.Now.Date;
        //                acc_voucherMain.UpdateByUserId = userid;

        //                if (acc_voucherMain.VoucherTypeId == 3)
        //                {
        //                    var abcsum = 0.00;
        //                    var bankacccode = "";
        //                    foreach (var item in acc_voucherMain.VoucherSubs)
        //                    {
        //                        bankacccode = _context.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.IsBankItem == true && p.AccId == item.AccId).Select(x => x.AccCode).FirstOrDefault();
        //                        if (bankacccode != null)
        //                        {
        //                            if (bankacccode.Contains("1-1-11"))
        //                            {
        //                                abcsum = +item.TKCredit;
        //                            }
        //                        }
        //                    }

        //                    acc_voucherMain.CountryIdLocal = int.Parse(defaultcurrencyid.ToString());
        //                    acc_voucherMain.vAmountInWords = _clsProc.GTRInwordsFormatBD(abcsum.ToString(), "", "");
        //                    acc_voucherMain.vAmountLocalInWords = _clsProc.GTRInwordsFormatBD(acc_voucherMain.VAmountLocal.ToString(), "", "");
        //                }

        //                string voucernocreatestylemain = _context.Companys.Include(x => x.vAcc_VoucherNoCreatedTypes).Where(c => c.CompanySecretCode == comid).Select(c => c.vAcc_VoucherNoCreatedTypes.VoucherNoCreatedTypeName).FirstOrDefault();

        //                if (voucernocreatestylemain == "Monthly")
        //                {
        //                    acc_voucherMain.VoucherInputDate = acc_voucherMain.VoucherDate;
        //                }
        //                else if (voucernocreatestylemain == "Yearly")
        //                {
        //                    acc_voucherMain.VoucherInputDate = DateTime.Now.Date;
        //                }
        //                else
        //                {
        //                    acc_voucherMain.VoucherInputDate = DateTime.Now.Date;
        //                }

        //                {
        //                    DateTime date = acc_voucherMain.VoucherInputDate;
        //                    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        //                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        //                    var activefiscalmonth = _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();

        //                    if (activefiscalmonth == null)
        //                    {
        //                        return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
        //                    }
        //                    var activefiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
        //                    if (activefiscalyear == null)
        //                    {
        //                        return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
        //                    }
        //                    if (acc_voucherMain.VoucherId > 0)
        //                    {
        //                        ViewBag.Title = "Edit";

        //                        var CurrentVoucherSub = _context.Acc_VoucherSubs.Include(x => x.VoucherSubChecnoes).Include(x => x.VoucherSubSections).Where(p => p.VoucherId == acc_voucherMain.VoucherId);
        //                        var CurrentVoucherSubcheckno = _context.Acc_VoucherSubChecnos.Where(p => p.VoucherId == acc_voucherMain.VoucherId);
        //                        acc_voucherMain.UpdateByUserId = userid;
        //                        acc_voucherMain.DateUpdated = DateTime.Now;
        //                        _context.Acc_VoucherSubChecnos.RemoveRange(CurrentVoucherSubcheckno);
        //                        _context.Acc_VoucherSubs.RemoveRange(CurrentVoucherSub);
        //                        _context.SaveChanges();

        //                        foreach (Acc_VoucherSub ss in acc_voucherMain.VoucherSubs)
        //                        {
        //                            ss.VoucherSubId = 0;
        //                            _context.Acc_VoucherSubs.Add(ss);                         
        //                        }
        //                        _context.SaveChanges();
        //                        acc_voucherMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
        //                        acc_voucherMain.FiscalYearId = activefiscalyear.FiscalYearId;
        //                        /////newly added for transaction group 
        //                        if (acc_voucherMain.VoucherTranGroupArray != null)
        //                        {
        //                            acc_voucherMain.VoucherTranGroupId = int.Parse(acc_voucherMain.VoucherTranGroupArray.FirstOrDefault().ToString());
        //                        }

        //                        _context.Entry(acc_voucherMain).State = EntityState.Modified;

        //                        List<Acc_VoucherTranGroup> com_proforma_itemgrouplist = new List<Acc_VoucherTranGroup>();

        //                        var asdf = _context.Acc_VoucherTranGroups.Where(x => x.VoucherId == acc_voucherMain.VoucherId);
        //                        _context.Acc_VoucherTranGroups.RemoveRange(asdf);

        //                        if (acc_voucherMain.VoucherTranGroupArray != null)
        //                        {
        //                            for (int i = 0; i < acc_voucherMain.VoucherTranGroupArray.Length; i++)
        //                            {
        //                                text += acc_voucherMain.VoucherTranGroupArray[i] + ",";
        //                                Acc_VoucherTranGroup itemgroupsingle = new Acc_VoucherTranGroup();
        //                                itemgroupsingle.VoucherId = acc_voucherMain.VoucherId;
        //                                itemgroupsingle.VoucherTranGroupId = int.Parse(acc_voucherMain.VoucherTranGroupArray[i]);
        //                                com_proforma_itemgrouplist.Add(itemgroupsingle);
        //                            }
        //                            _context.Acc_VoucherTranGroups.AddRange(com_proforma_itemgrouplist);
        //                            acc_voucherMain.VoucherTranGroupList = text.TrimEnd(',');
        //                        }

        //                        TempData["Message"] = "Data Update Successfully";
        //                        TempData["Status"] = "2";
        //                        _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), acc_voucherMain.VoucherId.ToString(), "Update", acc_voucherMain.VoucherNo.ToString());

        //                        _context.SaveChanges();
        //                    }
        //                    //Perform Save
        //                    else
        //                    {
        //                        text = "";
        //                        acc_voucherMain.Acc_VoucherTranGroups = new List<Acc_VoucherTranGroup>();
        //                        if (acc_voucherMain.VoucherTranGroupArray != null)
        //                        {
        //                            for (int i = 0; i < acc_voucherMain.VoucherTranGroupArray.Length; i++)
        //                            {
        //                                text += acc_voucherMain.VoucherTranGroupArray[i] + ",";
        //                                Acc_VoucherTranGroup COM_ProformaInvoice_Subs = new Acc_VoucherTranGroup { VoucherTranGroupId = int.Parse(acc_voucherMain.VoucherTranGroupArray[i]) }; 
        //                                acc_voucherMain.Acc_VoucherTranGroups.Add(COM_ProformaInvoice_Subs);
        //                            }
        //                            acc_voucherMain.VoucherTranGroupList = text.TrimEnd(',');
        //                        }
        //                        ViewBag.Title = "Create";

        //                        acc_voucherMain.UserId = userid;
        //                        acc_voucherMain.ComId = comid;
        //                        acc_voucherMain.DateAdded = DateTime.Now;
        //                        acc_voucherMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
        //                        acc_voucherMain.FiscalYearId = activefiscalyear.FiscalYearId;
        //                        var voucherfiscalyear = _context.Acc_FiscalYears.Where(x => x.ComId == comid && x.OpeningDate <= acc_voucherMain.VoucherDate && x.ClosingDate >= acc_voucherMain.VoucherDate).FirstOrDefault();
        //                        var x = VoucherNoMaker(acc_voucherMain.ComId, acc_voucherMain.VoucherTypeId, acc_voucherMain.VoucherDate, activefiscalyear.FiscalYearId, activefiscalmonth.FiscalMonthId);  
        //                        acc_voucherMain.VoucherNo = x[0];
        //                        acc_voucherMain.VoucherSerialId = int.Parse(x[1]);

        //                        _context.Acc_VoucherMains.Add(acc_voucherMain);
        //                        _context.SaveChanges();

        //                        _context.Entry(acc_voucherMain).GetDatabaseValues();
        //                        int id = acc_voucherMain.VoucherId; // Yes it's here

        //                        var abc = acc_voucherMain.VoucherSubs.ToList();
        //                        foreach (var itemabc in abc)
        //                        {
        //                            var xyz = itemabc.VoucherSubChecnoes.ToList();
        //                            xyz.ForEach(m => m.VoucherId = id);
        //                            _context.SaveChanges();
        //                        }
        //                    }

        //                    TempData["Message"] = "Data Save Successfully";
        //                    TempData["Status"] = "1";
        //                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), acc_voucherMain.VoucherId.ToString(), "Create", acc_voucherMain.VoucherNo.ToString());
        //                }
        //                return Json(new { Success = 1, VoucherID = acc_voucherMain.VoucherId, ex = "" });
        //            }
        //            catch (Exception ex)
        //            {
        //                if (acc_voucherMain.VoucherId > 0)
        //                {
        //                    ViewBag.Title = "Edit";
        //                }
        //                else
        //                {
        //                    ViewBag.Title = "Create";
        //                }
        //                if (ex.InnerException == null)
        //                {
        //                    return Json(new { Success = 0, ex = "Please Fill Necessary Data" });
        //                }
        //                else
        //                {
        //                    return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //                }
        //            }
        //        }

        //        ///	public IActionResult OnPostGetPDF() => PrepareReport("PDF", "pdf", "application/pdf");
        //        public IActionResult AccVoucherPrepareReport()
        //        {
        //            var WidgetPrice = 104.99m;
        //            var GizmoPrice = 1.41m;
        //            string renderFormat = "PDF";
        //            string extension = "pdf";
        //            string mimeType = "application/pdf";
        //            using var report = new LocalReport();
        //            Reports.Report.Load(report, WidgetPrice, GizmoPrice);
        //            var pdf = report.Render(renderFormat);
        //            return File(pdf, mimeType, "report." + extension);
        //        }

        //        public string[] VoucherNoMaker(string comid, int vouchertypeid, DateTime voucherdate, int fiscalyearid, int fiscalmonthid)
        //        {
        //            string[] FinalAccCode = new string[2];
        //            var input = 0;
        //            int length = 6;
        //            int maxvoucherid = 0;
        //            var maxnowithpadleftresult = "";

        //            string voucernocreatestyle = _context.Companys.Include(x => x.vAcc_VoucherNoCreatedTypes).Where(c => c.CompanySecretCode == comid).Select(c => c.vAcc_VoucherNoCreatedTypes.VoucherNoCreatedTypeName).FirstOrDefault();
        //            string vouchertypeShortPrefix = _context.Acc_VoucherNoPrefixes.Where(x => x.VoucherTypeId == vouchertypeid && x.ComId == comid).Select(x => x.VoucherShortPrefix).FirstOrDefault();

        //            length = _context.Acc_VoucherNoPrefixes.Where(x => x.VoucherTypeId == vouchertypeid && x.ComId == comid).Select(x => x.Length).FirstOrDefault();

        //            length = _context.Acc_VoucherNoPrefixes.Where(x => x.VoucherTypeId == vouchertypeid && x.ComId == comid).Select(x => x.Length).FirstOrDefault();

        //            if (voucernocreatestyle == "LifeTime")
        //            {
        //                Acc_FiscalYear fiscalyearname = _context.Acc_FiscalYears.Where(x => x.FiscalYearId == fiscalyearid).FirstOrDefault();
        //                var fiscalyearjoin = Convert.ToDateTime(fiscalyearname.OpDate).Year.ToString().Substring(2, 2) + "/" + Convert.ToDateTime(fiscalyearname.ClDate).Year.ToString().Substring(2, 2);
        //                var fiscalmonthname = _context.Acc_FiscalMonths.Where(x => x.FiscalMonthId == fiscalmonthid).Select(x => Convert.ToDateTime(x.dtFrom).Month).FirstOrDefault().ToString().PadLeft(2, '0');

        //                maxvoucherid = _context.Acc_VoucherMains.Where(x => x.ComId == comid && x.VoucherTypeId == vouchertypeid).Max(x => x.VoucherSerialId);
        //                input = maxvoucherid + 1;
        //                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
        //                FinalAccCode[1] = input.ToString();
        //            }
        //            else if (voucernocreatestyle == "Yearly")
        //            {
        //                Acc_FiscalYear fiscalyearname = _context.Acc_FiscalYears.Where(x => x.FiscalYearId == fiscalyearid).FirstOrDefault();
        //                var fiscalyearjoin = Convert.ToDateTime(fiscalyearname.OpDate).Year.ToString().Substring(2, 2) + "/" + Convert.ToDateTime(fiscalyearname.ClDate).Year.ToString().Substring(2, 2);
        //                var fiscalmonthname = _context.Acc_FiscalMonths.Where(x => x.FiscalMonthId == fiscalmonthid).Select(x => Convert.ToDateTime(x.dtFrom).Month).FirstOrDefault().ToString().PadLeft(2, '0');

        //                maxvoucherid = _context.Acc_VoucherMains.Where(x => x.ComId == comid && x.VoucherTypeId == vouchertypeid && x.FiscalYearId == fiscalyearid).DefaultIfEmpty().Max(p => p == null ? 0 : p.VoucherSerialId);
        //                input = maxvoucherid + 1;
        //                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
        //                FinalAccCode[1] = input.ToString();
        //            }
        //            else if (voucernocreatestyle == "Monthly")
        //            {
        //                Acc_FiscalYear fiscalyearname = _context.Acc_FiscalYears.Where(x => x.FiscalYearId == fiscalyearid).FirstOrDefault();
        //                var fiscalyearjoin = Convert.ToDateTime(fiscalyearname.OpeningDate).Year.ToString().Substring(2, 2) + "/" + Convert.ToDateTime(fiscalyearname.ClosingDate).Year.ToString().Substring(2, 2);

        //                if (Convert.ToDateTime(fiscalyearname.OpeningDate).Year.ToString().Substring(2, 2) == Convert.ToDateTime(fiscalyearname.ClosingDate).Year.ToString().Substring(2, 2))
        //                {
        //                    fiscalyearjoin = Convert.ToDateTime(fiscalyearname.ClosingDate).Year.ToString().Substring(2, 2);
        //                }

        //                var fiscalmonthname = _context.Acc_FiscalMonths.Where(x => x.FiscalMonthId == fiscalmonthid).Select(x => (x.OpeningdtFrom).Month).FirstOrDefault().ToString().PadLeft(2, '0');
        //                fiscalmonthname = _context.Acc_FiscalMonths.Where(x => x.FiscalMonthId == fiscalmonthid).Select(x => x.dtFrom.Substring(0, 3)).FirstOrDefault().ToString().PadLeft(2, '0');

        //                maxvoucherid = (_context.Acc_VoucherMains.Where(x => x.ComId == comid && x.VoucherTypeId == vouchertypeid && x.FiscalMonthId == fiscalmonthid).Max(x => (int?)x.VoucherSerialId)) ?? 0;
        //                maxvoucherid = input = maxvoucherid + 1;
        //                maxnowithpadleftresult = input.ToString().PadLeft(length, '0');
        //                FinalAccCode[1] = input.ToString();
        //            }
        //            else
        //            {
        //                FinalAccCode = null;
        //            }
        //            return FinalAccCode;
        //        }

        //        // GET: /Voucher/Edit/5
        //        public ActionResult EditAccVoucher(string Type, int? VoucherId)
        //        {
        //            try
        //            {
        //                ViewBag.Title = "Entry";
        //                var transactioncomid = HttpContext.Session.GetString("comid");

        //                if (Type == null)
        //                {
        //                    Type = "VPC";
        //                }
        //                HttpContext.Session.SetString("vouchertype", Type);
        //                string vouchertypeid = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x => x.VoucherTypeId.ToString()).FirstOrDefault();

        //                Acc_VoucherMain vouchersamplemodel = new Acc_VoucherMain();
        //                var transactioncompany = _context.Companys.Where(c => c.CompanySecretCode == transactioncomid).FirstOrDefault();
        //                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
        //                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

        //                this.ViewBag.Acc_VoucherType = _accVoucherRepository.Acc_VoucherType(Type);
        //                vouchersamplemodel.VoucherTypeId = int.Parse(vouchertypeid.ToString());

        //                vouchersamplemodel.Acc_VoucherType = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

        //                ViewBag.PrdUnitId = _accVoucherRepository.PrdUnitId(); 

        //                var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                if (transactioncompany.isMultiDebitCredit == true)
        //                {
        //                    this.ViewBag.Account = _accVoucherRepository.Account();
        //                }
        //                else
        //                {
        //                    this.ViewBag.Account = _accVoucherRepository.Account1();
        //                }
        //                ///////only cash item when multi debit credit of then it enable
        //                this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                return View();
        //            }
        //            catch (Exception ex)
        //            {
        //                string abcd = ex.InnerException.InnerException.Message.ToString();
        //                throw ex;
        //            }
        //        }

        //        public ActionResult CreateCopyAccVoucher(string Type, int? VoucherId)
        //        {
        //            try
        //            {
        //                ViewBag.Title = "Entry";
        //                var transactioncomid = HttpContext.Session.GetString("comid");

        //                if (Type == null)
        //                {
        //                    Type = "VPC";
        //                }
        //                string vouchertypeid = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeNameShort == Type).Select(x => x.VoucherTypeId.ToString()).FirstOrDefault();
        //                Acc_VoucherMain vouchersamplemodel = new Acc_VoucherMain();

        //                var transactioncompany = _context.Companys.Include(x => x.vCountryCompany).Where(c => c.CompanySecretCode == transactioncomid).FirstOrDefault();
        //                HttpContext.Session.SetInt32("defaultcurrencyid", transactioncompany.CountryId);
        //                HttpContext.Session.SetString("defaultcurrencyname", transactioncompany.vCountryCompany.CurrencyShortName);

        //                this.ViewBag.Acc_VoucherType = _accVoucherRepository.Acc_VoucherType(Type);
        //                vouchersamplemodel.VoucherTypeId = int.Parse(vouchertypeid.ToString());

        //                vouchersamplemodel.Acc_VoucherType = _context.Acc_VoucherTypes.Where(x => x.VoucherTypeId.ToString() == vouchertypeid).FirstOrDefault();

        //                ViewBag.PrdUnitId = _accVoucherRepository.PrdUnitId();
        //                ViewBag.Acc_FiscalYear = _accVoucherRepository.FiscalYearId();
        //                ViewBag.Acc_FiscalMonth = _accVoucherRepository.FiscalMonthId();
        //                ViewBag.EmpId = _accVoucherRepository.EmpId();
        //                ViewBag.CustomerId = _accVoucherRepository.CustomerId();
        //                ViewBag.SupplierId = _accVoucherRepository.SupplierId();
        //                ViewBag.VoucherTranGroupArray = _accVoucherRepository.VoucherTranGroupArray();
        //                ViewBag.VoucherTranGroupId = _accVoucherRepository.VoucherTranGroupId();
        //                ViewBag.VoucherTranGroupIdRow = _accVoucherRepository.VoucherTranGroupIdRow();

        //                if (Type == "VPC")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L");
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }

        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        ///////only cash item when multi debit credit of then it enable
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Create";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        string itemdesc = Vouchermain.VoucherTranGroupList;
        //                        var VoucherTranGroupList = _accVoucherRepository.VoucherTranGroupList();
        //                        if (itemdesc != null)
        //                        {
        //                            string[] split = itemdesc.Split(',');
        //                            ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                        }
        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VRC")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();
        //;
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Create";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VRB")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();
        //;
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Create";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VPB")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Create";
        //                        Vouchermain.VoucherNo = null;
        //                        Vouchermain.VoucherDate = DateTime.Now.Date;
        //                        Vouchermain.VoucherSubs.ToList().ForEach
        //                                    (c =>
        //                                    {
        //                                        c.VoucherId = 0;
        //                                    });

        //                        foreach (var item in Vouchermain.VoucherSubs)
        //                        {
        //                            item.VoucherSubId = 0;
        //                            item.VoucherSubChecnoes = null;
        //                            item.VoucherSubSections = null;
        //                        }

        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VCR")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();
        //;
        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Create";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true || p.IsBankItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VJR")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }
        //                        ViewBag.Title = "Create";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        return View(Vouchermain);
        //                    }
        //                }
        //                else if (Type == "VLC")
        //                {
        //                    ViewBag.Title = "Create";

        //                    var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L"); 
        //                    if (transactioncompany.isMultiDebitCredit == true)
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account();
        //                    }
        //                    else
        //                    {
        //                        this.ViewBag.Account = _accVoucherRepository.Account1();
        //                    }
        //                    ///////only cash item when multi debit credit of then it enable
        //                    this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                    this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);

        //                    if (VoucherId == 0)
        //                    {
        //                        this.ViewBag.Country = _accVoucherRepository.Country();
        //                        this.ViewBag.CountryIdVoucher = _accVoucherRepository.CountryIdVoucher();
        //                        this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                        return View(vouchersamplemodel);
        //                    }
        //                    else
        //                    {
        //                        Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                        if (Vouchermain == null)
        //                        {
        //                            return NotFound();
        //                        }

        //                        ViewBag.Title = "Create";
        //                        this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                        int AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                        this.ViewBag.AccountMain = new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == transactioncomid && p.AccType == "L" && p.IsCashItem == true).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", AccId);

        //                        return View(Vouchermain);
        //                    }
        //                }
        //                return View();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        //        // GET: /Voucher/Delete/5
        //        public ActionResult DeleteAccVoucher(int? VoucherId)
        //        {
        //            try
        //            {
        //                var transactioncomid = HttpContext.Session.GetString("comid");
        //                if (VoucherId == null)
        //                {
        //                    return BadRequest();
        //                }

        //                Acc_VoucherMain Vouchermain = _accVoucherRepository.GetVoucherMain();

        //                if (Vouchermain.isPosted == true)
        //                {
        //                    return BadRequest();
        //                }
        //                if (Vouchermain == null)
        //                {
        //                    return NotFound();
        //                }
        //                ViewBag.Title = "Delete";

        //                var transactioncompany = _context.Companys.Where(c => c.CompanySecretCode == transactioncomid).FirstOrDefault();
        //                this.ViewBag.Acc_VoucherType = new SelectList(_context.Acc_VoucherTypes, "VoucherTypeId", "VoucherTypeName", Vouchermain.VoucherTypeId);

        //                ViewBag.PrdUnitId = _accVoucherRepository.PrdUnitId(); 

        //                var Acc_ChartOfAccount = _context.Acc_ChartOfAccounts.Where(c => c.ComId == transactioncomid && c.AccId > 0 && c.AccType == "L");
        //                if (transactioncompany.isMultiDebitCredit == true) 
        //                {
        //                    this.ViewBag.Account = _accVoucherRepository.Account();
        //                }
        //                else
        //                {
        //                    this.ViewBag.Account = _accVoucherRepository.Account1();
        //                }
        //                ///////only cash item when multi debit credit of then it enable
        //                this.ViewBag.AccountMain = _accVoucherRepository.AccountMain();
        //                this.ViewBag.SubSectionList = _context.Cat_SubSection.Where(x => x.ComId == transactioncomid);
        //                this.ViewBag.Country = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                this.ViewBag.CountryIdVoucher = new SelectList(_context.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", Vouchermain.CountryId);
        //                Vouchermain.AccId = Vouchermain.VoucherSubs.Where(x => x.SRowNo < 0).Select(x => x.AccId).FirstOrDefault();
        //                ViewBag.EmpId = _accVoucherRepository.EmpId();
        //                ViewBag.CustomerId = _accVoucherRepository.CustomerId();
        //                ViewBag.SupplierId = _accVoucherRepository.SupplierId();

        //                var VoucherTranGroupList = _context.VoucherTranGroups.Where(x => x.ComId == transactioncomid).ToList();

        //                string itemdesc = Vouchermain.VoucherTranGroupList;
        //                if (itemdesc != null)
        //                {
        //                    string[] split = itemdesc.Split(',');
        //                    ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName", split);
        //                }
        //                else
        //                {
        //                    ViewBag.VoucherTranGroupArray = new MultiSelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName");
        //                }
        //                ViewBag.VoucherTranGroupIdRow = new SelectList(VoucherTranGroupList, "VoucherTranGroupId", "VoucherTranGroupName");

        //                return View("Create", Vouchermain);
        //            }
        //            catch (Exception ex)
        //            {
        //                string abcd = ex.InnerException.InnerException.Message.ToString();
        //                throw ex;
        //            }
        //        }

        //        // POST: /Voucher/Delete/5
        //        [HttpPost, ActionName("DeleteAccVoucher")]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> DeleteAccVoucherConfirmed(int id)
        //        {
        //            try
        //            {
        //                Acc_VoucherMain Vouchermain = await _context.Acc_VoucherMains.FindAsync(id);

        //                var CurrentVoucherSub = _context.Acc_VoucherSubs.Include(x => x.VoucherSubChecnoes).Include(x => x.VoucherSubSections).Where(p => p.VoucherId == Vouchermain.VoucherId);
        //                _context.Acc_VoucherSubs.RemoveRange(CurrentVoucherSub);
        //                _context.Acc_VoucherMains.Remove(Vouchermain);
        //                await _context.SaveChangesAsync();

        //                TempData["Message"] = "Data Delete Successfully";
        //                TempData["Status"] = "3";
        //                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Vouchermain.VoucherId.ToString(), "Delete", Vouchermain.VoucherNo);

        //                return Json(new { Success = 1, VoucherID = Vouchermain.VoucherId, ex = "" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Success = 0, ex = ex.Message.ToString() });
        //            }
        //        }

        //        public IActionResult VoucherTypeByIntegrationId(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return Json(new { Success = 1, VoucherTypeName = "VJR" });
        //            }
        //            var comid = HttpContext.Session.GetString("comid");
        //            var exist = _context.Cat_Integration_Setting_Mains.Where(x => x.IntegrationSettingMainId == id && x.Acc_VoucherType != null).FirstOrDefault();

        //            if (exist == null)
        //            {
        //                return Json(new { Success = 1, VoucherTypeName = "VJR" });
        //            }

        //            var VoucherTypeName = _context.Cat_Integration_Setting_Mains.Include(x => x.Acc_VoucherType).Where(x => x.IntegrationSettingMainId == id).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;

        //            if (VoucherTypeName == null)
        //            {
        //                return Json(new { Success = 2, VoucherTypeName = VoucherTypeName });
        //            }
        //            else
        //            {
        //                return Json(new { Success = 1, VoucherTypeName = VoucherTypeName });
        //            }
        //        }
        //        #endregion

        #region PF Fiscal Year
        public ActionResult FiscalYearList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            string comid = HttpContext.Session.GetString("comid");
            return View(_PFProcessRepository.GetFiscalYear());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FiscalYearList(PF_FiscalYear fiscalYear)
        {

            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);
            Helper.ExecProc("Pf_prcCloseFiscalYear", sqlParameter);

            TempData["Message"] = "Year Close Successfully.";
            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), fiscalYear.FYName, "Acc_prcCloseFiscalYear", fiscalYear.FYName);

            return View(_PFProcessRepository.GetFiscalYear());

        }

        public IActionResult CreateFiscalYear()
        {
            ViewBag.Title = "Create";
            return View();
        }
        [HttpPost]
        public IActionResult CreateFiscalYear(PF_FiscalYear acc)
        {
            PF_FiscalYear obj = new PF_FiscalYear();
            var comid = HttpContext.Session.GetString("comid");
            DateTime opDate = DateTime.ParseExact(acc.OpDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime clDate = DateTime.ParseExact(acc.ClDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            string opDateString = opDate.ToString("MMM d yyyy");
            string clDateString = clDate.ToString("MMM d yyyy");
            string fyName = $"{opDate.Year} ({opDate.ToString("MMMM")}) - {clDate.Year} ({clDate.ToString("MMMM")})";
            var prevData = _context.PF_FiscalYear.OrderByDescending(x => x.FiscalYearId).FirstOrDefault();

            obj.FYId = prevData.FYId + 1;
            obj.FYName = fyName;
            obj.OpDate = opDateString;
            obj.ClDate = clDateString;
            obj.isWorking = acc.isWorking;
            obj.isRunning = acc.isRunning;
            obj.ComId = comid;
            obj.OpeningDate = opDate;
            obj.ClosingDate = clDate;
            //obj.FYId = obj.FiscalYearId;
            _context.PF_FiscalYear.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("FiscalYearList", "PF");
        }
        public IActionResult EditFiscalYear(int? id)
        {

            //if (id == null)
            //{
            //    return NotFound();
            //}
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";            

            var data = _context.PF_FiscalYear.Find(id);
            return View("EditFiscalYear", data);

        }
        [HttpPost]
        public async Task<IActionResult> EditFiscalYear(PF_FiscalYear acc)
        {
            var data = _context.PF_FiscalYear.Find(acc.FiscalYearId);
            if (data == null)
            {
                return NotFound();
            }

            // Update the properties of the fiscal year record
            data.OpDate = DateTime.ParseExact(acc.OpDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
            data.ClDate = DateTime.ParseExact(acc.ClDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
            string fyName = $"{DateTime.ParseExact(acc.OpDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture):yyyy} ({DateTime.ParseExact(acc.OpDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture):MMM}) - {DateTime.ParseExact(acc.ClDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture):yyyy} ({DateTime.ParseExact(acc.ClDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture):MMM})";
            data.FYName = fyName;
            data.isWorking = acc.isWorking;
            data.isRunning = acc.isRunning;
            _context.PF_FiscalYear.Update(data);
            _context.SaveChanges();

            return RedirectToAction("FiscalYearList", "PF");
        }

        public IActionResult DeleteFiscalYear(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Delete";
          
            var DeleteFiscalYear = _context.PF_FiscalYear.Find(id);
            if (DeleteFiscalYear == null)
            {
                return NotFound();
            }
            return View("EditFiscalYear", DeleteFiscalYear);
        }
        [HttpPost, ActionName("DeleteFiscalYear")]
        public IActionResult DeleteFiscalYear(int id)
        {
            try
            {
                var fiscalyear = _context.PF_FiscalYear.Find(id);
                _context.PF_FiscalYear.Remove(fiscalyear);
                _context.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = fiscalyear.FiscalYearId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

    }
}