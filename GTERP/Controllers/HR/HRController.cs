using ClosedXML.Excel;
using DataTablesParser;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using ExcelDataReader;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GTERP.BLL;
using GTERP.Interfaces;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HRVariables;
using GTERP.Interfaces.Payroll;
using GTERP.Migrations.GTRDB;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.Email;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using HR_Emp_Info = GTERP.Models.HR_Emp_Info;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Vml.Office;
using System.Threading;
using System.Text.Json;
using DocumentFormat.OpenXml;
using System.Globalization;
using static StandaloneSDKDemo.SDKHelper;
using DocumentFormat.OpenXml.Wordprocessing;
using ZKTeco;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Drawing.Printing;
using System.Xml.Linq;
using QuickMailer;
using GTERP.Models.LeaveVM;
using Xamarin.Forms;
using HVFaceManagement.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DocumentFormat.OpenXml.VariantTypes;


namespace GTERP.Controllers.HR
{
    [OverridableAuthorize]
    public class HRController : Controller
    {
        #region Declarations 

        DataSet dsList;
        DataSet dsDetails;

        private readonly TransactionLogRepository tranlog;
        private readonly ILogger<HRController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IBusinessAllowanceRepository _businessAllowanceRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmpShiftInputRepository _empShiftInputRepository;
        private readonly GTRDBContext _context;
        private readonly IEmployeeArrearBill _employeeArrearBill;
        private readonly IGasChargeSettingRepository _gasChargeSettingRepository;
        private readonly IElectricChargeSettingRepository _electricChargeSettingRepository;
        private readonly IHRSettingRepository _hRSettingRepository;
        private readonly IHRExpSettingRepository _hRExpSettingRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IEmpInfoRepository _empInfoRepository;
        private readonly ICatAttStatusRepository _catAttStatusRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IFixAttRepository _fixAttRepository;
        private readonly IEmpReleaseRepository _empReleaseRepository;
        private readonly IPFChequeRepository _pfChequeRepository;
        private readonly IEmployeeBehaviourRepository _employeeBehaviourRepository;
        private readonly IIncrementAllRepository _incrementAllRepository;
        private readonly IAttendanceProcessRepository _attendanceProcessRepository;
        private readonly IRawDataViewRepository _rawDataViewRepository;
        private readonly IRecreationRepository _recreationRepository;
        private readonly ILoanReturnRepository _loanReturnRepository;
        private readonly HrRepository _repos;
        private readonly IBloodGroupRepository _bloodGroupRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IPoliceStationRepository _policeStationRepository;
        private readonly IPostOfficeRepository _postOfficeRepository;
        private readonly IBuildingTypeRepository _buildingTypeRepository;
        private readonly IEmpTypeRepository _empTypeRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ICat_UnitRepository _cat_UnitRepository;
        private readonly IReligionRepository _religionRepository;
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IGenderRepository _genderRepository;
        private readonly IHREmpInfoRepository _hREmpInfoRepository;
        private readonly ILeaveEntryRepository _leaveEntryRepository;
        private readonly IProcessLockRepository _processLockRepository;
        private readonly IOTAndNightShiftRepository _oTAndNightShiftRepository;
        private readonly IEmployeeSalaryRepository _employeeSalaryRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ISummerWinterAllowRepository _summerWinterAllowRepository;
        private readonly IStyleRepository _styleRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly ISupplimentRepository _supplimentRepository;
        private readonly ITotalFCRepository _totalFCRepository;
        private readonly ITotalNightRepository _totalNightRepository;
        private readonly IHRRepository _hRRepository;
        private readonly IIdCardRepository _idCardRepository;
        private readonly IHolidaySetupRepository _holidaySetupRepository;
        private readonly ILoanHaltRepository _loanHaltRepository;
        private readonly ILoanHouseBuilding _loanHouseBuilding;
        private readonly ILoanMotorCycleRepository _loanMotorCycleRepository;
        private readonly ILoanOtherRepository _loanOtherRepository;
        private readonly ILoanPFRepository _loanPFRepository;
        private readonly ILoanWelfareRepository _loanWFRepository;
        private readonly ILeaveEncashmentRepository _leaveEncashmentRepository;
        private readonly IIncrementRepository _incrementRepository;
        private readonly ISubSectionRepository _subSectionRepository;
        private readonly ISalaryAdditionRepository _salaryAdditionRepository;
        private readonly ITransferRepository _TransferRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<SMTPConfigModel> _smtpConfig;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor

        public HRController(
            IConfiguration configuration,
            TransactionLogRepository tran,
            GTRDBContext context,
            IWebHostEnvironment env,
            IBusinessAllowanceRepository businessAllowanceRepository,
            IEmployeeBehaviourRepository employeeBehaviourRepository,
            IShiftRepository shiftRepository,
            IEmpShiftInputRepository empShiftInputRepository,
            IEmployeeArrearBill employeeArrearBill,
            IGasChargeSettingRepository gasChargeSettingRepository,
            IElectricChargeSettingRepository electricChargeSettingRepository,
            IHRSettingRepository hRSettingRepository,
            IHRExpSettingRepository hRExpSettingRepository,
            ILeaveBalanceRepository leaveBalanceRepository,
            ISectionRepository sectionRepository,
            IEmpInfoRepository empInfoRepository,
            ICatAttStatusRepository catAttStatusRepository,
            IDesignationRepository designationRepository,
            IDepartmentRepository departmentRepository,
            ILineRepository lineRepository,
            IFixAttRepository fixAttRepository,
            IEmpReleaseRepository empReleaseRepository,
            IIncrementAllRepository incrementAllRepository,
            IAttendanceProcessRepository attendanceProcessRepository,
            IRawDataViewRepository rawDataViewRepository,
            IRecreationRepository recreationRepository,
            ILoanReturnRepository loanReturnRepository,
            HrRepository hrRepository,
            IBloodGroupRepository bloodGroupRepository,
            IDistrictRepository districtRepository,
            IPoliceStationRepository policeStationRepository,
            IPostOfficeRepository postOfficeRepository,
            IBuildingTypeRepository buildingTypeRepository,
            IEmpTypeRepository empTypeRepository,
            ISkillRepository skillRepository,
            ICat_UnitRepository cat_UnitRepository,
            IReligionRepository religionRepository,
            IBankBranchRepository bankBranchRepository,
            IBankRepository bankRepository,
            IFloorRepository floorRepository,
            IGradeRepository gradeRepository,
            IGenderRepository genderRepository,
            IHREmpInfoRepository hREmpInfoRepository,
            ILeaveEntryRepository leaveEntryRepository,
            IProcessLockRepository processLockRepository,
            IOTAndNightShiftRepository oTAndNightShiftRepository,
            IEmployeeSalaryRepository employeeSalaryRepository,
            IDocumentRepository documentRepository,
            ISummerWinterAllowRepository summerWinterAllowRepository,
            IStyleRepository styleRepository,
            IProductionRepository productionRepository,
            ISupplimentRepository supplimentRepository,
            ITotalFCRepository totalFCRepository,
            ITotalNightRepository totalNightRepository,
            IHRRepository hRRepository,
            ILogger<HRController> loggers,
            IIdCardRepository idCardRepository,
            IHolidaySetupRepository holidaySetupRepository,
            ILoanHouseBuilding loanHouseBuilding,
            ILoanHaltRepository loanHaltRepository,
            ILoanMotorCycleRepository loanMotorCycleRepository,
            ILoanOtherRepository loanOtherRepository,
            ILoanPFRepository loanPFRepository,
            ILoanWelfareRepository loanWFRepository,
            ILeaveEncashmentRepository leaveEncashmentRepository,
            IIncrementRepository incrementRepository,
            ISubSectionRepository subSectionRepository,
            IRawDataImportRepository rawDataImportRepository,
            ISalaryAdditionRepository salaryAdditionRepository,
            IWebHostEnvironment webHostEnvironment,
            ITransferRepository TransferRepository,
            IHttpContextAccessor httpContext,
            IOptions<SMTPConfigModel> smtpConfig
            )
        {
            _configuration = configuration;
            tranlog = tran;
            _context = context;
            _logger = loggers;
            _repos = hrRepository;
            _env = env;
            _businessAllowanceRepository = businessAllowanceRepository;
            _shiftRepository = shiftRepository;
            _empShiftInputRepository = empShiftInputRepository;
            _employeeArrearBill = employeeArrearBill;
            _electricChargeSettingRepository = electricChargeSettingRepository;
            _gasChargeSettingRepository = gasChargeSettingRepository;
            _hRExpSettingRepository = hRExpSettingRepository;
            _hRSettingRepository = hRSettingRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
            _sectionRepository = sectionRepository;
            _empInfoRepository = empInfoRepository;
            _catAttStatusRepository = catAttStatusRepository;
            _departmentRepository = departmentRepository;
            _lineRepository = lineRepository;
            _fixAttRepository = fixAttRepository;
            _designationRepository = designationRepository;
            _employeeBehaviourRepository = employeeBehaviourRepository;
            _incrementAllRepository = incrementAllRepository;
            _attendanceProcessRepository = attendanceProcessRepository;
            _rawDataViewRepository = rawDataViewRepository;
            _recreationRepository = recreationRepository;
            _loanReturnRepository = loanReturnRepository;
            _bloodGroupRepository = bloodGroupRepository;
            _districtRepository = districtRepository;
            _policeStationRepository = policeStationRepository;
            _postOfficeRepository = postOfficeRepository;
            _buildingTypeRepository = buildingTypeRepository;
            _empTypeRepository = empTypeRepository;
            _gradeRepository = gradeRepository;
            _religionRepository = religionRepository;
            _cat_UnitRepository = cat_UnitRepository;
            _bankBranchRepository = bankBranchRepository;
            _bankRepository = bankRepository;
            _floorRepository = floorRepository;
            _skillRepository = skillRepository;
            _genderRepository = genderRepository;
            _hREmpInfoRepository = hREmpInfoRepository;
            _empReleaseRepository = empReleaseRepository;
            _leaveEntryRepository = leaveEntryRepository;
            _processLockRepository = processLockRepository;
            _oTAndNightShiftRepository = oTAndNightShiftRepository;
            _employeeSalaryRepository = employeeSalaryRepository;
            _documentRepository = documentRepository;
            _summerWinterAllowRepository = summerWinterAllowRepository;
            _styleRepository = styleRepository;
            _productionRepository = productionRepository;
            _supplimentRepository = supplimentRepository;
            _totalFCRepository = totalFCRepository;
            _totalNightRepository = totalNightRepository;
            _hRRepository = hRRepository;
            _idCardRepository = idCardRepository;
            _holidaySetupRepository = holidaySetupRepository;
            _loanHaltRepository = loanHaltRepository;
            _loanHouseBuilding = loanHouseBuilding;
            _loanMotorCycleRepository = loanMotorCycleRepository;
            _loanOtherRepository = loanOtherRepository;
            _loanWFRepository = loanWFRepository;
            _loanPFRepository = loanPFRepository;
            _leaveEncashmentRepository = leaveEncashmentRepository;
            _incrementRepository = incrementRepository;
            _subSectionRepository = subSectionRepository;
            _salaryAdditionRepository = salaryAdditionRepository;
            _TransferRepository = TransferRepository;
            _httpContext = httpContext;
            _smtpConfig = smtpConfig;


        }

        #endregion

        #region Business Allowance
        public IActionResult BusinessAllowanceList(DateTime? todate)
        {

            var BusinessAllow = _businessAllowanceRepository.BusinessAllowanceList(todate);
            ViewBag.BusinessAllow = BusinessAllow;
            return View();
        }

        public IActionResult CreateBusinessAllowance(List<HR_Emp_BusinessAllow> BusinessAllows)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");
                foreach (var item in BusinessAllows)
                {
                    var exist = _businessAllowanceRepository.GetAll()
                        .Where(r => r.DtTo.Value.Month == item.DtTo.Value.Month && r.EmpId == item.EmpId)
                        .ToList();

                    if (exist.Count > 0)
                    {
                        _businessAllowanceRepository.RemoveRange(exist);

                    }

                    _businessAllowanceRepository.Add(item);

                }


                TempData["Message"] = "Business Allowance Save/Update Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Json(new { Success = 0, ex = TempData["Message"].ToString() });
            }

        }
        #endregion

        #region Employee Arrear Bill
        public IActionResult GetArrearBillList()
        {

            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");


            var gTRDBContext = _employeeArrearBill.GetAllEmpArrearBill();

            return View(gTRDBContext);
        }

        public IActionResult CreateEmpArrearBill()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            ViewData["EmpId"] = _employeeArrearBill.GetArrearBillList();
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Bind("SalaryId,ComId,EmpId,LId1,LId2,LId3,BId,PFLId,WelfareLId,MCLId,HBLId,BasicSalary,IsBS,HouseRent,IsHr,MadicalAllow,IsMa,ConveyanceAllow,IsConvAllow,DearnessAllow,IsDearAllow,GasAllow,IsGasAllow,PersonalPay,IsPersonalPay,ArrearBasic,IsArrearBasic,ArrearBonus,IsArrearBonus,WashingAllow,IsWashingAllow,SiftAllow,ChargeAllow,IsChargAllow,MiscAdd,IsMiscAdd,ContainSub,IsContainSub,ComPfCount,IsComPfcount,EduAllow,IsEduAllow,TiffinAllow,IsTiffinAllow,CanteenAllow,IsCanteenAllow,AttAllow,IsAttAllow,FestivalBonus,IsFestivalBonus,RiskAllow,IsRiskAllow,NightAllow,IsNightAllow,MobileAllow,IsMobileAllow,Pf,IsPf,PfAdd,IsPfAdd,HrExp,IsHrexp,FesBonusDed,IsFesBonus,Transportcharge,IsTrncharge,TeliphoneCharge,IsTelcharge,TAExpense,IsTAExp,SalaryAdv,IsSalaryAdv,PurchaseAdv,McloanDed,IsMcloan,HbloanDed,IsHbloan,PfloannDed,IsPfloann,WfloanLocal,IsWfloanLocal,WfloanOther,IsWfloanOther,WpfloanDed,IsWpfloanDed,MaterialLoanDed,IsMaterialLoan,MiscDed,IsMiscDed,AdvAgainstExp,IsAdvAgainstExp,AdvFacility,IsAdvFacility,ElectricCharge,IsElectricCharge,Gascharge,IsGascharge,HazScheme,IsHazScheme,Donaton,IsDonaton,Dishantenna,IsDishantenna,RevenueStamp,IsRevenueStamp,OwaSub,IsOwaSub,DedIncBns,IsDedIncBns,DapEmpClub,IsDapEmpClub,Moktab,IsMoktab,ChemicalForum,IsChemicalForum,DiplomaassoDed,IsDiplomaassoDed,EnggassoDed,IsEnggassoDed,Wfsub,IsWfsub,EduAlloDed,IsEduAlloDed,PurChange,IsPurChange,IncomeTax,IsIncomeTax,ArrearInTaxDed,IsArrearInTaxDed,OffWlfareAsso,IsOffWlfareAsso,OfficeclubDed,IsOfficeclubDed,IncBonusDed,IsIncBonusDed,Watercharge,IsWatercharge,ChemicalAsso,IsChemicalAsso,AdvInTaxDed,IsAdvInTaxDed,ConvAllowDed,IsConvAllowDed,DedIncBonusOf,IsDedIncBonusOf,UnionSubDed,IsUnionSubDed,EmpClubDed,IsEmpClubDed,MedicalExp,IsMedicalExp,WagesaAdv,IsWagesaAdv,MedicalLoanDed,IsMedicalLoanDed,AdvWagesDed,IsAdvWagesDed,WFL,IsWFL,CheForum,IsCheForum")]
        public IActionResult CreateEmpArrearBill(HR_Emp_ArrearBill HR_Emp_ArrearBill)
        {

            if (ModelState.IsValid)
            {
                if (HR_Emp_ArrearBill.ArrBillId > 0)
                {
                    _employeeArrearBill.Update(HR_Emp_ArrearBill);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_ArrearBill.ArrBillId.ToString(), "Update", HR_Emp_ArrearBill.Adv.ToString());

                }
                else
                {
                    _employeeArrearBill.Add(HR_Emp_ArrearBill);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_ArrearBill.ArrBillId.ToString(), "Create", HR_Emp_ArrearBill.Adv.ToString());

                }
                return RedirectToAction("GetArrearBillList", "HR");
            }
            return View(HR_Emp_ArrearBill);
        }

        // GET: hR_Emp_ArrearBill/Edit/5


        [HttpPost]
        public IActionResult AutoSalaryCalculation(int EmpId, int LId1, int BId, Double BS)
        {
            int? empTypeId = _employeeArrearBill.FindByEmpId(EmpId);
            Cat_HRSetting hr = null;
            Cat_HRExpSetting hrExp = null;

            if (empTypeId != null)
            {
                hr = _hRSettingRepository.GetAll()
                    .Where(h => h.EmpTypeId == empTypeId && h.LId == LId1 && h.BS <= BS && h.BId == BId)
                    .OrderByDescending(h => h.BS).FirstOrDefault();                    //.ToList();
                hrExp = _hRExpSettingRepository.GetAll()
                   .Where(h => h.EmpTypeId == empTypeId && h.LId == LId1 && h.BId == BId && h.BS <= BS)
                   .OrderByDescending(h => h.BS).FirstOrDefault();

            }

            var gasCharge = _gasChargeSettingRepository.GetAll()
             .Where(h => h.LId == LId1 && h.BId == BId).FirstOrDefault();

            var electricCharge = _electricChargeSettingRepository.GetAll()
             .Where(h => h.LId == LId1 && h.BId == BId).FirstOrDefault();

            return Json(new { HR = hr, HRExp = hrExp, GasCharge = gasCharge, ElectricCharge = electricCharge });

        }

        [HttpGet]
        public IActionResult FamilyOCalculation(int EmpId, int LId2, int BId, Double BS)
        {
            int? empTypeId = _employeeArrearBill.FindByEmpId(EmpId);
            Cat_HRSetting hr = null;
            Cat_HRExpSetting hrExp = null;

            if (empTypeId != null)
            {
                hr = _hRSettingRepository.GetAll()
                    .Where(h => h.EmpTypeId == empTypeId && h.LId == LId2 && h.BS <= BS && h.BId == BId)
                    .OrderByDescending(h => h.BS).FirstOrDefault();                    //.ToList();
                hrExp = _hRExpSettingRepository.GetAll()
                   .Where(h => h.EmpTypeId == empTypeId && h.LId == LId2 && h.BId == BId && h.BS <= BS)
                   .OrderByDescending(h => h.BS).FirstOrDefault();

            }

            return Json(new { HR = hr, HRExp = hrExp });

        }



        public IActionResult EditEmpArrearBill(int? id)
        {
            ViewData["EmpId"] = _employeeArrearBill.GetArrearBillList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var HR_Emp_ArrearBill = _employeeArrearBill.FindById(id);
            if (HR_Emp_ArrearBill == null)
            {
                return NotFound();
            }
            return View("CreateEmpArrearBill", HR_Emp_ArrearBill);
        }

        // GET: hR_Emp_ArrearBill/Delete/5
        //[Route("Delete")]
        public IActionResult DeleteEmpArrearBill(int? id)
        {
            ViewData["EmpId"] = _employeeArrearBill.GetArrearBillList();
            if (id == null)
            {
                return NotFound();
            }

            var HR_Emp_ArrearBill = _employeeArrearBill.FindById(id);

            if (HR_Emp_ArrearBill == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateEmpArrearBill", HR_Emp_ArrearBill);
        }

        [HttpPost, ActionName("DeleteEmpArrearBill")]
        public IActionResult DeleteEmpArrearBillConfirmed(int id)
        {
            ViewData["EmpId"] = _employeeArrearBill.GetArrearBillList();
            try
            {
                var HR_Emp_ArrearBill = _employeeArrearBill.FindById(id);
                _employeeArrearBill.Delete(HR_Emp_ArrearBill);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_ArrearBill.ArrBillId.ToString(), "Delete", HR_Emp_ArrearBill.Adv.ToString());

                return Json(new { Success = 1, ArrBillId = HR_Emp_ArrearBill.ArrBillId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        #endregion

        #region Employee Behaviour
        public IActionResult EmployeeBehaviourList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_employeeBehaviourRepository.GetBehaveAll());

        }

        // GET: EmployeeBehaviorController/Create
        public IActionResult CreateEmployeeBehaviour()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Create";

            ViewBag.EmpId = _empReleaseRepository.EmpList();

            ViewBag.NoticeType = _employeeBehaviourRepository.CatVariableList();
            return View();
        }

        // POST: EmployeeBehaviorController/Create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult CreateEmployeeBehaviour(HR_Emp_Behave HR_Emp_Behave)
        {
            ViewBag.EmpId = _empReleaseRepository.EmpList();

            ViewBag.NoticeType = _empReleaseRepository.EmpList();

            if (ModelState.IsValid)
            {
                if (HR_Emp_Behave.BehaveId > 0)
                {
                    _employeeBehaviourRepository.Update(HR_Emp_Behave);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Behave.BehaveId.ToString(), "Update", HR_Emp_Behave.Decision.ToString());

                }
                else
                {
                    _employeeBehaviourRepository.Add(HR_Emp_Behave);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Behave.BehaveId.ToString(), "Create", HR_Emp_Behave.Decision.ToString());

                }
                return RedirectToAction("EmployeeBehaviourList", "HR");
            }
            return View(HR_Emp_Behave);
        }

        // GET: EmployeeBehaviorController/Edit/5
        public IActionResult EditEmployeeBehaviour(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            var HR_Emp_Behave = _employeeBehaviourRepository.FindById(id);

            ViewBag.EmpId = _empReleaseRepository.EmpList();

            ViewBag.NoticeType = _employeeBehaviourRepository.CatVariableList();

            if (HR_Emp_Behave == null)
            {
                return NotFound();
            }

            return View("CreateEmployeeBehaviour", HR_Emp_Behave);
        }



        // GET: EmployeeBehaviorController/Delete/5
        public IActionResult DeleteEmployeeBehaviour(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Delete";
            var HR_Emp_Behave = _employeeBehaviourRepository.FindById(id);

            ViewBag.EmpId = _empReleaseRepository.EmpList();

            ViewBag.NoticeType = _employeeBehaviourRepository.CatVariableList();

            if (HR_Emp_Behave == null)
            {
                return NotFound();
            }

            return View("CreateEmployeeBehaviour", HR_Emp_Behave);
        }

        [HttpPost, ActionName("DeleteEmployeeBehaviour")]
        public IActionResult DeleteEmployeeBehaviourConfirmed(int id)
        {

            try
            {
                var HR_Emp_Behave = _employeeBehaviourRepository.FindById(id);
                _employeeBehaviourRepository.Delete(HR_Emp_Behave);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Behave.BehaveId.ToString(), "Delete", HR_Emp_Behave.Decision);

                return Json(new { Success = 1, BehaveId = HR_Emp_Behave.BehaveId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion

        #region Document
        // GET: Document List..
        [HttpGet]
        public IActionResult DocumentList()
        {
            var data = _documentRepository.GetDocumentAll();
            char[] spearator = { '!' };

            List<HR_Emp_DocVM> person = new List<HR_Emp_DocVM>();
            foreach (var a in data)
            {
                HR_Emp_DocVM index = new HR_Emp_DocVM();
                if (a.FileName == null)
                {
                    a.FileName = "";
                }
                String[] strlist = a.FileName.Split(spearator);
                strlist = strlist.SkipLast(1).ToArray();
                List<string> ls = new();

                foreach (var nam in strlist)
                {
                    string path = Path.Combine(this._env.WebRootPath, "EmpDocument/") + nam;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exsit or not  
                    {
                        ls.Add(nam);
                    }

                }
                index.Id = a.Id;
                index.RefCode = a.RefCode;
                index.Title = a.Title;
                index.VarType = a.VarType;
                index.Remarks = a.Remarks;
                index.DtInput = a.DtInput;
                index.imageName = ls;
                person.Add(index);
            }

            ViewBag.filename = person;
            return View(person);
        }
        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(this._env.WebRootPath, "EmpDocument/") + filename;
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public IActionResult btnDelete_Click(string fileName)
        {
            string path = Path.Combine(this._env.WebRootPath, "EmpDocument/") + fileName;
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }
            return RedirectToAction("DocumentList");
        }

        // GET: Document/Create
        public IActionResult CreateDocument()
        {
            ViewBag.VarType = _documentRepository.CatVariableList();
            ViewBag.Title = "Create";
            return View();
        }

        // POST: Document/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> CreateDocument(HR_Emp_DocVM model)
        {

            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            string wwwPath = this._env.WebRootPath;
            string contentPath = this._env.ContentRootPath;
            string path = Path.Combine(this._env.WebRootPath, "EmpDocument");
            if (model == null ||
                model.FileToUpload == null)
                return Content("File not selected");

            //var path = _env.WebRootPath + "\\EmpDocument\\";
            HR_Emp_Document data = new HR_Emp_Document
            {
                RefCode = model.RefCode,
                Title = model.Title,
                VarType = model.VarType,
                Remarks = model.Remarks,
                ComId = comid,
                UserId = userid,
                DtInput = model.DtInput,
            };
            data.FileName = "";
            data.FilePath = "";
            foreach (var item in model.FileToUpload)
            {
                string FileNameUrl = UploadFile(item, model.Title);
                data.FileName += FileNameUrl + "!";
                data.FilePath += FileNameUrl + "!";
            };
            _context.Add(data);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Data Save Successfully";
            TempData["Status"] = "1";
            return RedirectToAction("DocumentList");
        }
        private string UploadFile(IFormFile file, string title)
        {
            string FileName = null;
            if (file != null)
            {
                //string folder = "book/Gallery/";
                string serverFolder = _env.WebRootPath + "\\EmpDocument\\";
                FileName = title.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(serverFolder, FileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

            }
            return FileName;
        }

        public async Task<IActionResult> EditEmpFiles(int? id)
        {
            var item = await _context.HR_Emp_Document.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            var temp = new HR_Emp_DocVM()
            {
                Id = item.Id,
                RefCode = item.RefCode,
                Title = item.Title,
                VarType = item.VarType,
                Remarks = item.Remarks,
                DtInput = item.DtInput
            };

            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmpFiles(int id, HR_Emp_DocVM model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.HR_Emp_Document.FindAsync(model.Id);
                customer.RefCode = model.RefCode;
                customer.Title = model.Title;
                customer.VarType = model.VarType;
                customer.Remarks = model.Remarks;
                customer.DtInput = model.DtInput;
                if (model.FileToUpload != null)
                {
                    foreach (var item in model.FileToUpload)
                    {
                        string FileNameUrl = UploadFile(item, model.Title);
                        customer.FileName += FileNameUrl + "!";
                        customer.FilePath += FileNameUrl + "!";
                    };
                }
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(DocumentList));
        }
        //[Obsolete]
        //public IActionResult Download(int id)
        //{
        //    var document = _context.HR_Emp_Document.Find(id);


        //    if (document == null)
        //    {
        //        return Content("Document not found");
        //    }

        //    string filepath = _env.WebRootPath + "\\EmpDocument\\" + document.FilePath;
        //    if (!System.IO.File.Exists(filepath))
        //    {
        //        return NotFound();
        //    }

        //    var fileBytes = System.IO.File.ReadAllBytes(filepath);
        //    var response = new FileContentResult(fileBytes, "application/octet-stream")
        //    {
        //        FileDownloadName = document.FilePath
        //    };
        //    return response;
        //}


        //private string GetContentType(string path)
        //{
        //    var types = GetMimeTypes();
        //    var ext = Path.GetExtension(path).ToLowerInvariant();
        //    return types[ext];
        //}

        //private Dictionary<string, string> GetMimeTypes()
        //{
        //    return new Dictionary<string, string>
        //    {
        //        {".txt", "text/plain"},
        //        {".pdf", "application/pdf"},
        //        {".doc", "application/vnd.ms-word"},
        //        {".docx", "application/vnd.ms-word"},
        //        {".xls", "application/vnd.ms-excel"},
        //        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        //        {".png", "image/png"},
        //        {".jpg", "image/jpeg"},
        //        {".jpeg", "image/jpeg"},
        //        {".gif", "image/gif"},
        //        {".csv", "text/csv"},
        //        {".mp4", "video/mp4"}
        //    };
        //}



        string GetFilename(string code, IFormFile file)
        {
            var name = ContentDispositionHeaderValue.Parse(
                            file.ContentDisposition).FileName.ToString().Trim('"');
            return code + "_" + name;
        }

        // GET: Document/Edit/5
        public IActionResult EditDocument(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";
            var hR_Emp_Document = _context.HR_Emp_Document.Find(id);
            if (hR_Emp_Document == null)
            {
                return NotFound();
            }

            ViewBag.VarType = _documentRepository.CatVariableList();

            return View("CreateDocument", hR_Emp_Document);
        }


        // GET: Document/Delete/5
        public IActionResult DeleteDocument(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hR_Emp_Document = _context.HR_Emp_Document
                .FirstOrDefault(m => m.Id == id);
            if (hR_Emp_Document == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            ViewBag.VarType = _documentRepository.CatVariableList();
            return View("CreateDocument", hR_Emp_Document);
        }

        // POST: Document/Delete/5
        [HttpPost, ActionName("DeleteDocument")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteDocumentConfirmed(int id)
        {
            var hR_Emp_Document = _documentRepository.FindById(id);
            _documentRepository.Remove(hR_Emp_Document);

            if (System.IO.File.Exists(hR_Emp_Document.FilePath))
            {
                System.IO.File.Delete(hR_Emp_Document.FilePath);
            }
            _context.SaveChanges();
            TempData["Message"] = "Data Delete Successfully";
            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
        }
        #endregion

        #region SummerWinterAllow
        public IActionResult SWAllowList(int? SWAId)
        {
            ViewBag.ProssType = _summerWinterAllowRepository.SWAllowList();

            ViewBag.Recreation = _summerWinterAllowRepository.GetSummerWinterAllowAll(SWAId);
            ViewBag.SWAId = SWAId;

            if (SWAId != null)
            {
                _summerWinterAllowRepository.GetSummerWinterAllowAll(SWAId);
            }

            ViewBag.Recreation = null;
            return View();
        }

        public IActionResult CreateSWAllow(List<HR_SummerWinterAllowance> summerWinterAllows)
        {
            try
            {
                var swId = summerWinterAllows.FirstOrDefault().SWAllowanceId;
                var setting = _context.Cat_SummerWinterAllowanceSetting.Where(s => s.SWAllowanceId == swId).FirstOrDefault();
                if (setting != null)
                {
                    foreach (var item in summerWinterAllows)
                    {
                        item.Stamp = 10; // default 10 tk
                        var exist = _context.HR_SummerWinterAllowance
                            .Where(s => s.SWAllowanceId == item.SWAllowanceId && s.EmpId == item.EmpId).FirstOrDefault();
                        if (exist != null)
                        {
                            _context.HR_SummerWinterAllowance.Remove(exist);
                        }

                        if (item.IsRaincoat || item.IsSummer || item.IsWinter)
                        {
                            _context.Add(AllowCalculation(item, setting));
                        }
                    }
                    _context.SaveChanges();
                    TempData["Message"] = "Summer Winter allowance Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                else
                {
                    TempData["Message"] = "Settinge not found the Month";
                    return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        HR_SummerWinterAllowance AllowCalculation(HR_SummerWinterAllowance item, Cat_SummerWinterAllowanceSetting setting)
        {
            return _summerWinterAllowRepository.AllowCalculation(item, setting);
        }
        #endregion

        #region Leave Balance
        public IActionResult LeaveBalanceList()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.OpeningYear = _leaveBalanceRepository.GetOpeningYear();
            ViewBag.EmpId = _empReleaseRepository.EmpList();
            ViewBag.SectId = _sectionRepository.GetSectionList();
            ViewBag.DeptId = _departmentRepository.GetDepartmentList();
            ViewBag.LineId = _lineRepository.GetLineList();
            ViewBag.FloorId = _floorRepository.GetFloorList();
            ViewBag.LeaveBalanceList = new List<HR_Leave_Balance>();
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<JsonResult> GetLeaveBalance(string Criteria, int EmpId, int SectId,int DeptId,int LineId,int FloorId, string DtOpBal)
        //{

        //    var ProductSerialresult = await _leaveBalanceRepository.GetLeaveBalance(Criteria, EmpId, SectId,DeptId,LineId,FloorId, DtOpBal);
        //    return Json(ProductSerialresult);
        //}
        [HttpGet]
        public async Task<IActionResult> GetLeaveBalances(string Criteria, int EmpId, int SectId, int DeptId, int LineId, int FloorId, string DtOpBal)
        {
            try
            {
                var ProductSerialresult = await _leaveBalanceRepository.GetLeaveBalance(Criteria, EmpId, SectId, DeptId, LineId, FloorId, DtOpBal);
                TempData["Message"] = "Data Load Successfully";
                return Json(new { Success = 1, data = ProductSerialresult, message = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });

            }
        }
        //[HttpPost]
        //[RequestSizeLimit(100000000)]
        //public async Task<ActionResult> SaveLeaveBalance(string? LeaveBalancestring)

        //{
        //    try
        //    {
        //        List<HR_Leave_Balance> LeaveBalance = JsonConvert.DeserializeObject<List<HR_Leave_Balance>>(LeaveBalancestring);
        //        await _leaveBalanceRepository.SaveLeaveBalance(LeaveBalance);

        //        TempData["Message"] = "Leave Balance created/updated Successfully";
        //        return Json(new { Success = 1, ex = TempData["Message"].ToString() });
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        [HttpPost]
        //[RequestSizeLimit(100000000)]
        public async Task<IActionResult> SaveLeaveBalance([FromBody] List<HR_Leave_Balance> LeaveBalance)

        {
            try
            {
                //List<HR_Leave_Balance> LeaveBalance = JsonConvert.DeserializeObject<List<HR_Leave_Balance>>(LeaveBalancestring);
                await _leaveBalanceRepository.SaveLeaveBalance(LeaveBalance);

                TempData["Message"] = "Leave Balance created/updated Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<IActionResult> LeaveBalanceUploadFile(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    _leaveBalanceRepository.FileUploadDirectory(file);
                    var leavebl = this.GetLeaveBL(file.FileName);
                    if (leavebl.Count() > 0)
                    {
                        // for temporary table data save

                        await _context.HR_TempLeaveBalanceExcels.AddRangeAsync(leavebl);
                        await _context.SaveChangesAsync();

                        // for update leave balance procedure
                        _leaveBalanceRepository.SaveUploadedData();

                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

                }

            }
            catch (Exception e)
            {
                throw e;
                _logger.LogError(e.InnerException.Message);
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("LeaveBalanceList");
        }

        private List<HR_TempLeaveBalanceExcel> GetLeaveBL(string fName)
        {

            var data = _leaveBalanceRepository.GetLeaveBalanceExcel(fName);
            return data;
        }
        public ActionResult DownloadSampleFile(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }
            var response = _leaveBalanceRepository.DownloadSampleFile(file);
            return response;
        }
        #endregion

        #region Employee Released
        public IActionResult EmployeeReleasedList()
        {
            //TempData["Message"] = "Data Load Successfully";
            //TempData["Status"] = "1";
            //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_empReleaseRepository.GetReleasedAll());
        }

        public IActionResult CreateEmpReleased()
        {
            ViewBag.EmpId = _empReleaseRepository.EmpList();

            ViewBag.RelType = _empReleaseRepository.CatVariableList();

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");


            ViewBag.Title = "Create";
            return View();

        }

        [HttpPost]
        public IActionResult CreateEmpReleased(HR_Emp_Released HR_Emp_Released)
        {
            ViewBag.EmpId = _empReleaseRepository.EmpList();

            ViewBag.RelType = _empReleaseRepository.CatVariableList();
            var empinfo = _context.HR_Emp_Info.Where(x => x.EmpId == HR_Emp_Released.EmpId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (HR_Emp_Released.RelId > 0)
                {

                    _empReleaseRepository.Update(HR_Emp_Released);
                    if (HR_Emp_Released.RelType == "Salary Halt")
                    {
                        empinfo.IsInactive = false;
                    }
                    else
                        empinfo.IsInactive = true;
                    _context.Entry(empinfo).State = EntityState.Modified;
                    _context.SaveChanges();
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Released.RelId.ToString(), "Update", HR_Emp_Released.RelType.ToString());

                }
                else
                {
                    _empReleaseRepository.ApproveSet(HR_Emp_Released);
                    _empReleaseRepository.Add(HR_Emp_Released);
                    if (HR_Emp_Released.RelType == "Salary Halt")
                    {
                        empinfo.IsInactive = false;
                    }
                    else
                        empinfo.IsInactive = true;
                    _context.Entry(empinfo).State = EntityState.Modified;
                    _context.SaveChanges();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Released.RelId.ToString(), "Create", HR_Emp_Released.RelType.ToString());

                }
                return RedirectToAction("EmployeeReleasedList", "HR");
            }
            return View(HR_Emp_Released);
        }

        public IActionResult EditEmpReleased(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            // ViewBag.EmpId = _empReleaseRepository.EmpList();

            ViewBag.RelType = _empReleaseRepository.CatVariableList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var HR_Emp_Released = _empReleaseRepository.FindById(id);
            if (HR_Emp_Released == null)
            {
                return NotFound();
            }

            ViewBag.EmpId = _empReleaseRepository.EmpListEdit(HR_Emp_Released.EmpId);

            return View("CreateEmpReleased", HR_Emp_Released);
        }

        public IActionResult DeleteEmpReleased(int? id)
        {
            //ViewBag.EmpId = _empReleaseRepository.EmpListEdit();
            ViewBag.RelType = _empReleaseRepository.CatVariableList();

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            if (id == null)
            {
                return NotFound();
            }

            var HR_Emp_Released = _empReleaseRepository.FindById(id);

            if (HR_Emp_Released == null)
            {
                return NotFound();
            }
            ViewBag.EmpId = _empReleaseRepository.EmpListEdit(HR_Emp_Released.EmpId);
            ViewBag.Title = "Delete";

            return View("CreateEmpReleased", HR_Emp_Released);

        }
        [HttpPost, ActionName("DeleteEmpReleased_p")]
        public IActionResult DeleteEmpReleasedConfirmed(int id)
        {
            try
            {
                var HR_Emp_Released = _empReleaseRepository.FindById(id);
                var empinfo = _context.HR_Emp_Info.Where(x => x.EmpId == HR_Emp_Released.EmpId).FirstOrDefault();

                _empReleaseRepository.Delete(HR_Emp_Released);
                empinfo.IsInactive = false;
                _context.Entry(empinfo).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Released.RelId.ToString(), "Delete", HR_Emp_Released.RelType);

                return Json(new { Success = 1, RelId = HR_Emp_Released.RelId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EmpReleasedUploadFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/EmpReleased/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/EmpReleased/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetEmpReleaseExcel(file.FileName);
                    if (addition.Count() > 0)
                    {
                        await _context.HR_TempReleasedExcel.AddRangeAsync(addition);
                        await _context.SaveChangesAsync();

                        var Query = $"[dbo].[HR_prcProcessEmpReleasedExcel] '{comid}',{userid}";
                        SqlParameter[] sqlParameter = new SqlParameter[2];
                        sqlParameter[0] = new SqlParameter("@ComId", comid);
                        sqlParameter[1] = new SqlParameter("@UserId ", userid);
                        Helper.ExecProc("HR_prcProcessEmpReleasedExcel", sqlParameter);

                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("EmployeeReleasedList");
        }

        private List<HR_TempReleasedExcel> GetEmpReleaseExcel(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/EmpReleased/" + comid + "/" + userid + "/" + fName);

            List<HR_TempReleasedExcel> Released = new List<HR_TempReleasedExcel>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                Released.Add(new HR_TempReleasedExcel()
                                {
                                    ComId = HttpContext.Session.GetString("comid"),
                                    EmpCode = reader.GetValue(1).ToString(),
                                    dtReleased = DateTime.Parse(reader.GetValue(2).ToString()),
                                    dtPresentLast = DateTime.Parse(reader.GetValue(3).ToString()),
                                    dtSubmit = DateTime.Parse(reader.GetValue(4).ToString()),
                                    Remarks = reader.GetValue(5).ToString(),
                                    RelType = reader.GetValue(6).ToString(),

                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return Released;
        }
        public ActionResult DownloadSampleFormatReleased(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
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

        #endregion


        #region PF Cheque info
        public IActionResult PFChequeList()
        {

            var comid = HttpContext.Session.GetString("comid");
            var data = _context.HR_PFCheque.Where(x => x.ComId == comid).ToList();

            return View(data);
        }

        public IActionResult CreatePFCheque()
        {
            ViewBag.EmpId = _empReleaseRepository.EmpList();

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");


            ViewBag.Title = "Create";
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> CreatePFCheque(HR_PFCheque hR_PFCheque)
        {
            ViewBag.EmpId = _empReleaseRepository.EmpList();
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            hR_PFCheque.ComId = comid;
            if (ModelState.IsValid)
            {
                _context.Add(hR_PFCheque);
                await _context.SaveChangesAsync();
                return RedirectToAction("PFChequeList", "HR");
            }
            return View(hR_PFCheque);
        }

        public IActionResult EditPFCheque(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var HR_PFCheque = _context.HR_PFCheque.Find(id);
            if (HR_PFCheque == null)
            {
                return NotFound();
            }
            ViewBag.EmpId = _empReleaseRepository.EmpListEdit(HR_PFCheque.EmpId);

            return View(HR_PFCheque);
        }

        [HttpPost]
        public async Task<IActionResult> EditPFCheque(HR_PFCheque hR_PFCheque)
        {
            if (ModelState.IsValid)
            {
                _context.Update(hR_PFCheque);
                await _context.SaveChangesAsync();
                return RedirectToAction("PFChequeList", "HR");
            }
            return View(hR_PFCheque);
        }
        [HttpGet]
        public IActionResult DeletePFCheque(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            var HR_PFCheque = _context.HR_PFCheque.Find(id);
            if (HR_PFCheque == null)
            {
                return NotFound();
            }
            ViewBag.EmpId = _empReleaseRepository.EmpListEdit(HR_PFCheque.EmpId);

            return View(HR_PFCheque);

        }
        [HttpPost]
        public IActionResult DeletePFChequePost(HR_PFCheque model)
        {
            ViewBag.Title = "Delete";
            var obj = _context.HR_PFCheque.Find(model.PFId);
            if (ModelState.IsValid)
            {
                _context.Remove(obj);
                _context.SaveChanges();
                return RedirectToAction("PFChequeList", "HR");
            }
            return View(obj);
        }
        //[HttpPost]
        //public async Task<IActionResult> PFChequeUploadFile(IFormFile file)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    try
        //    {

        //        if (file != null)
        //        {
        //            string fileLocation = Path.GetFullPath("wwwroot/Content/EmpReleased/" + comid + "/" + userid);
        //            if (Directory.Exists(fileLocation))
        //            {
        //                Directory.Delete(fileLocation, true);
        //            }
        //            string uploadlocation = Path.GetFullPath("wwwroot/Content/EmpReleased/" + comid + "/" + userid + "/");

        //            if (!Directory.Exists(uploadlocation))
        //            {
        //                Directory.CreateDirectory(uploadlocation);
        //            }

        //            string filePath = uploadlocation + Path.GetFileName(file.FileName);

        //            string extension = Path.GetExtension(file.FileName);
        //            var fileStream = new FileStream(filePath, FileMode.Create);
        //            file.CopyTo(fileStream);
        //            fileStream.Close();

        //            var addition = this.GetPFChequeExcel(file.FileName);
        //            if (addition.Count() > 0)
        //            {
        //                await _context.HR_PFChequeExcel.AddRangeAsync(addition);
        //                await _context.SaveChangesAsync();

        //                var Query = $"[dbo].[HR_prcProcessEmpReleasedExcel] '{comid}',{userid}";
        //                SqlParameter[] sqlParameter = new SqlParameter[2];
        //                sqlParameter[0] = new SqlParameter("@ComId", comid);
        //                sqlParameter[1] = new SqlParameter("@UserId ", userid);
        //                Helper.ExecProc("HR_prcProcessEmpReleasedExcel", sqlParameter);

        //                TempData["Message"] = "Data Upload Successfully";
        //                TempData["Status"] = "1";
        //            }
        //            else
        //            {
        //                TempData["Message"] = "Something is wrong!";
        //                TempData["Status"] = "3";
        //            }
        //        }
        //        else
        //        {
        //            TempData["Message"] = "Please, Select a excel file!";
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //        ViewData["Message"] = "Error Occured";
        //    }
        //    //Process();
        //    return RedirectToAction("PFChequeList");
        //}

        //private List<HR_PFChequeExcel> GetPFChequeExcel(string fName)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    var filename = Path.GetFullPath("wwwroot/Content/EmpReleased/" + comid + "/" + userid + "/" + fName);

        //    List<HR_PFChequeExcel> Released = new List<HR_PFChequeExcel>();

        //    try
        //    {

        //        using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
        //        {
        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //            {
        //                var startSl = 0;
        //                while (reader.Read())
        //                {
        //                    startSl++;
        //                    if (startSl == 1)
        //                    {
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        Released.Add(new HR_PFChequeExcel()
        //                        {
        //                            EmpId = (int)HttpContext.Session.GetInt32("EmpId"),
        //                            EmpName = reader.GetValue(1).ToString(),
        //                            ChequeDate = DateTime.Parse(reader.GetValue(2).ToString()),
        //                            ChequeNo = reader.GetValue(3).ToString(),
        //                            ReturnChequeDate = DateTime.Parse(reader.GetValue(4).ToString()),
        //                            ReturnChequeNo = reader.GetValue(5).ToString(),
        //                            ReturnChequeAmt = reader.GetValue(6).ToString(),

        //                        });
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //        //_logger.LogError(e.Message);
        //    }

        //    return Released;
        //}
        public ActionResult DownloadSampleFormatPFCheque(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
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

        #endregion

        public IActionResult GetLastProcessDate()
        {
            var model = _attendanceProcessRepository.GetAttProcess("");
            return Ok(model.dtLast.ToString("dd-MMM-yyyy"));
        }
        #region Attendance Process
        public IActionResult AttendanceProcessList(string msg)
        {
            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            var model = _attendanceProcessRepository.GetAttProcess(msg);
            var data = _context.ButtonPermissions.FirstOrDefault(x => x.ComId == comid);

            if (msg == null)
            {

            }
            else
            {
                ModelState.AddModelError("CustomError", "Process complete");
                ViewBag.loadMsg = msg;
            }
            ViewBag.ButtonPermission = data;
            ViewBag.Section = _sectionRepository.GetSectionList();
            ViewBag.Employee = _attendanceProcessRepository.GetEmpSelectList();
            ViewBag.EmpData = _empInfoRepository.GetEmp();

            string username = HttpContext.Session.GetString("username");

            ViewBag.UserName = false;

            if (username != null)
            {
                if (username.Contains("gtrbd"))
                {
                    ViewBag.UserName = true;
                }
                else
                {
                    ViewBag.UserName = false;
                }
            }

            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> AttendanceProcessList(HR_AttendanceProcess model/*, string optSts, string optCriteria*/)
        {
            string er1 = " ";
            try
            {

                string optSts = model.dayType;
                string optCriteria = model.optCriteria;

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                //var values = prcProcessData(model, optSts, optCriteria,comid,userid);
                string values = "";

                string pcName = "AHSAN-PC";


                string sqlQuery = "";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string procedureName = "HR_getautoNotification"; // Replace with your actual stored procedure name

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ComId", comid);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        foreach (var item in reader)
                        {
                            var token = reader["Token"].ToString();
                            var title = reader["Title"].ToString();
                            var body = reader["Body"].ToString();
                            var empid = reader["EmpId"].ToString();
                            await SendNotificationAsync(token, title, body);


                            var entityToUpdate = _context.HR_Notify.Where(x => x.EmpId == int.Parse(empid)).FirstOrDefault();
                            if (entityToUpdate != null)
                            {
                                entityToUpdate.IsMobileApp = 1;

                                _context.SaveChanges();
                            }


                        }

                        connection.Close();

                    }
                }

                dsDetails = new DataSet();

                Int64 ChkLock = 0;

                sqlQuery = "Select dbo.HR_fncProcessLock ('" + comid + "', 'Attendance Lock','" + Helper.GTRDate(model.dtProcess.ToString()) + "')";
                ChkLock = Helper.GTRCountingDataLarge(sqlQuery);


                if (ChkLock == 1)
                {
                    TempData["Message"] = "Process Locked. Please communicate with Administrator";
                    return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                }


                if (model.Monthly == true)
                {
                    int X = 0, Y = 0;
                    var m = model.dtLast;
                    X = DateTime.Parse(model.dtProcess.ToString()).Day;
                    Y = DateTime.Parse(model.dtTo.ToString()).Day;

                    while (X <= Y)
                    {
                        dsDetails = new DataSet();
                        {
                            if (optSts == "H" || optSts == "R" || optSts == "W" || optSts == "S")
                            {

                                _attendanceProcessRepository.RemoveProssType(model);
                                _attendanceProcessRepository.SaveAtt(model);

                            }
                            prcInsertEmp(model, model.optCriteria);


                            SqlParameter[] parameter = new SqlParameter[2];
                            parameter[0] = new SqlParameter("@ComID", comid);
                            parameter[1] = new SqlParameter("@Date", Helper.GTRDate(model.dtProcess.ToString()));

                            er1 = Helper.ExecProc("HR_PrcProcessAttend", parameter);
                            // er1 = Helper.ExecProc("HR_PrcProcessAttend_new_18_feb_24", parameter);
                            values = er1;
                        }
                        model.dtProcess = DateTime.Parse(model.dtProcess.ToString()).AddDays(1);
                        X++;
                    }

                    if (values == "Process Successfull")
                    {
                        ModelState.AddModelError("CustomError", values);
                        ViewBag.loadMsg = "save";
                        TempData["Message"] = values;
                        return Json(new { Success = 1, ex = TempData["Message"] });
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", values);
                        ViewBag.msgErr = "error";
                        return Json(new { Success = 2, ex = TempData["msg1"] });
                    }
                }
                else
                {
                    var queueid = 0;
                    try
                    {
                        var path = "";
                        if (path == null || path.Length == 0)
                        {
                            path = "AttendanceProcess";
                        }

                        DateTime dt1 = model.dtProcess;
                        DateTime dt2 = model.dtLast;

                        TimeSpan ts = dt1 - dt2;
                        int days = ts.Days;
                        if (days > 1)
                        {
                            TempData["Message"] = "Please Run The Process For " + Helper.GTRDate(model.dtLast.AddDays(1).ToString());
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                        if (optSts == "H" || optSts == "R" || optSts == "W" || optSts == "S")
                        {
                            _attendanceProcessRepository.RemoveProssType(model);
                            _attendanceProcessRepository.SaveAtt(model);

                        }

                        SqlParameter[] parameter = new SqlParameter[2];
                        parameter[0] = new SqlParameter("@ComID", comid);
                        parameter[1] = new SqlParameter("@Date", Helper.GTRDate(model.dtProcess.ToString()));
                        //var query = $"Exec HR_PrcProcessAttend_new_18_feb_24 '{comid}','{Helper.GTRDate(model.dtProcess.ToString())}'";
                        var query = $"Exec HR_PrcProcessAttend '{comid}','{Helper.GTRDate(model.dtProcess.ToString())}'";
                        var queue = new QueryProcessQueue
                        {
                            ComId = comid,
                            Query = query,
                            ExcuteById = userid,
                            Type = "att",
                            RequestFrom = model.dtProcess,
                            RequestTo = model.dtTo,
                        };

                        await _context.QueryProcessQueues.AddAsync(queue);
                        await _context.SaveChangesAsync();
                        queueid = queue.Id;
                        // await _context.Database.ExecuteSqlRawAsync($"Exec ExecuteProcess '{queue.Id}'");
                        await Helper.ExecCommand($"Exec ExecuteProcess '{queue.Id}'");
                        //TempData["Message"] = "Process Complete";
                        return Json(new { Success = 1, ex = "Process Complete" });
                    }
                    catch (Exception ex)
                    {
                        var errorqueue = await _context.QueryProcessQueues.SingleOrDefaultAsync(x => x.Id == queueid);
                        errorqueue.ErrorLog = ex.Message;
                        errorqueue.IsExecuted = 4;
                        await _context.SaveChangesAsync();
                        //TempData["Message"] = "Process Fail";
                        return Json(new { Success = 0, ex = "Process Fail" });
                    }


                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                ViewBag.msgErr = "error";
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });

            }
        }

        public async Task<IActionResult> PendingProcessList(string type)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var prcList = await _context.QueryProcessQueues.Where(x => x.ComId == comid && x.Type == type).OrderByDescending(x => x.RequestedAt).Select(x =>
                new QueryProcessQueueVm
                {
                    Id = x.Id,
                    IsExecuted = x.IsExecuted,
                    RequestFrom = x.RequestFrom == null ? "-" : Convert.ToDateTime(x.RequestFrom).ToShortDateString(),
                    RequestTo = x.RequestTo == null ? "-" : Convert.ToDateTime(x.RequestTo).ToShortDateString(),
                    Requesteddate = x.RequestedAt.ToShortDateString(),
                    Requestetime = x.RequestedAt.ToShortTimeString(),
                    PrcstDate = x.PrcStarted == null ? "-" : Convert.ToDateTime(x.PrcStarted).ToShortDateString(),
                    PrcstTime = x.PrcStarted == null ? "-" : Convert.ToDateTime(x.PrcStarted).ToShortTimeString(),
                    PrcedDate = x.PrcEnd == null ? "-" : Convert.ToDateTime(x.PrcEnd).ToShortDateString(),
                    PrcedTime = x.PrcEnd == null ? "-" : Convert.ToDateTime(x.PrcEnd).ToShortTimeString(),
                }).Take(5).ToListAsync();

                return Ok(prcList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        public async Task<IActionResult> RetryProcess(int prcId)
        {
            try
            {
                await Helper.ExecCommand($"Exec ExecuteProcess '{prcId}'");
                return Ok();
            }
            catch (Exception ex)
            {
                var errorqueue = await _context.QueryProcessQueues.SingleOrDefaultAsync(x => x.Id == prcId);
                errorqueue.ErrorLog = ex.Message;
                errorqueue.IsExecuted = 4;
                await _context.SaveChangesAsync();
                //TempData["Message"] = "Process Fail";
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        public IActionResult EmailAbsentList(string processDate)
        {

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var msg = "";


            SqlParameter[] parameter1 = new SqlParameter[3];
            parameter1[0] = new SqlParameter("@ComID", comid);
            parameter1[1] = new SqlParameter("@dtFrom", Helper.GTRDate(processDate.ToString()));
            parameter1[2] = new SqlParameter("@dtTo", Helper.GTRDate(processDate.ToString()));



            var absentList = Helper.GetDataTable("HR_RptAttendunnilever", parameter1);
            string FileLocation = @".\wwwroot\Content\AbsentList\" + comid + "\\" + userid + "\\";
            string FileName = "absentList";
            var filePath = FileLocation + FileName + ".xlsx";
            //if (!System.IO.File.Exists(FileLocation + FileName + ".xlsx"))
            //{
            //    System.IO.File.Create(FileLocation + FileName + ".xlsx").Close();
            //}
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            //string filePath = Path.Combine(_env.WebRootPath, "absentListExcel/absentList.xlsx");
            //var filePath = Path.GetFullPath(FileName + ".xlsx");

            using (var workbook = new XLWorkbook())

            {
                var worksheet = workbook.Worksheets.Add("AbsentList");

                for (int j = 0; j < absentList.Columns.Count; j++)
                {
                    worksheet.Cell(1, j + 1).Value = absentList.Columns[j].ColumnName;

                }

                for (int i = 0; i < absentList.Rows.Count; i++)
                {
                    for (int j = 0; j < absentList.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = absentList.Rows[i][j];
                    }
                }
                workbook.SaveAs(filePath);
                //var content = stream.ToArray();
                //Response.Clear();
                //Response.Headers.Add("content-disposition", "attachment;filename=AbsentList.xls");
                //Response.ContentType = "application/xls";
                //Response.Body.WriteAsync(content);
                //Response.Body.Flush();

            }
            try
            {
                if (comid == "C2D5D66E-6406-4569-B7B5-BD272A5B520C")
                {
                    var emailto = "Md.Imranul-Hoque@unilever.com";
                    var cc = new List<string> {
                            "halim@gtrbd.com",
                     "sharmin.akter-borsha@unilever.com",
                      "Rezaul.Karim-WPS@unilever.com",
                      "Mihir-Kanti.Nath@unilever.com"
                     };

                    string subject = $"Care Customer Service";
                    string body = $"Dear Sir,<br/><br/>Greetings from GTR!<br/><br/>" +
                        $"The absent List of employees is attached below" +
                        $"<br/>Visit care support center for your support. <br/><br/> " +
                        $"Thank you for being connected with us. <br/> <br/> Sincerely,<br/> " +
                        $"Care Service<br/> Genuine Technology & Research Ltd.";
                    SendEmail(emailto, cc, subject, body, filePath);
                }

                msg = "Mail Send successfully";
                TempData["Message"] = msg;
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            catch
            {
                msg = "operation failed";
                return Json(new { Success = 0, ex = TempData["Message"].ToString() });
            }



        }

        public void SendEmail(string emailTo, List<string> emailCC, string subject, string body, string attchment)
        {
            var comid = HttpContext.Session.GetString("comid");

            try
            {

                var message = new MailMessage();
                message.From = new MailAddress(_smtpConfig.Value.SenderAddress, "Genuine Technology & Research Ltd.");

                message.To.Add(new MailAddress(emailTo));

                if (emailCC != null)
                {

                    foreach (var mailCC in emailCC)
                    {
                        message.CC.Add(new MailAddress(mailCC));
                    }
                }
                message.Subject = subject;
                message.Body = body;
                message.Attachments.Add(new System.Net.Mail.Attachment(attchment));
                message.IsBodyHtml = _smtpConfig.Value.IsBodyHTML; //true;

                using (var client = new SmtpClient())
                {
                    client.Host = _smtpConfig.Value.Host; //"smtp.gmail.com";
                    client.Port = _smtpConfig.Value.Port;//587;
                    client.EnableSsl = _smtpConfig.Value.EnableSSL;// true;
                                                                   //client.Credentials = new NetworkCredential(config.GetSection("CredentialMail").Value, config.GetSection("CredentialPassword").Value);
                    client.Credentials = new NetworkCredential(_smtpConfig.Value.UserName, _smtpConfig.Value.Password);
                    client.Send(message);
                }


                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}
            }
            catch (Exception)
            {

                throw;
            }

        }

        public String prcProcessData(HR_AttendanceProcess model, string optSts, string optCriteria, string comId, string userId)
        {
            string comid = comId;
            string userid = userId;

            string pcName = "Ahsan Test Pc";
            ArrayList arQuery = new ArrayList();

            string sqlQuery = "";
            dsDetails = new DataSet();
            Int64 ChkLock = 0;

            sqlQuery = "Select dbo.HR_fncProcessLock ('" + comid + "', 'Attendance Lock','" + Helper.GTRDate(model.dtProcess.ToString()) + "')";
            ChkLock = Helper.GTRCountingDataLarge(sqlQuery);


            if (ChkLock == 1)
            {
                return "Process Locked. Please communicate with Administrator";
            }
            try
            {

                var path = "";
                if (path == null || path.Length == 0)
                {
                    path = "AttendanceProcess";
                }

                DateTime dt1 = model.dtProcess;
                DateTime dt2 = model.dtLast;

                TimeSpan ts = dt1 - dt2;
                int days = ts.Days;
                if (days > 1)
                {
                    return "Please Run The Process For " + Helper.GTRDate(model.dtLast.AddDays(1).ToString());
                }

                if (model.Monthly == true)
                {
                    int X = 0, Y = 0;
                    var m = model.dtLast;
                    X = DateTime.Parse(model.dtProcess.ToString()).Day;
                    Y = DateTime.Parse(model.dtTo.ToString()).Day;

                    while (X <= Y)
                    {

                        {
                            if (optSts == "H" || optSts == "R" || optSts == "W" || optSts == "S")
                            {
                                sqlQuery = "delete HR_ProssType where ComId = " + comid + " and ProssDt =  '" + Helper.GTRDate(model.dtProcess.ToString()) + "' ";
                                arQuery.Add(sqlQuery);
                                sqlQuery = "insert into HR_ProssType(ComId, ProssDt, DaySts, DayStsB, IsLock) values(" + comid + ", '" + Helper.GTRDate(model.dtProcess.ToString()) + "', '" + optSts + "', '" + optSts + "', 0)";
                                arQuery.Add(sqlQuery);

                            }
                            prcInsertEmp(model, optCriteria);

                            sqlQuery = "Exec HR_PrcProcessAttend " + comid + ",'" + Helper.GTRDate(model.dtProcess.ToString()) + "'";
                            arQuery.Add(sqlQuery);

                        }
                        model.dtProcess = DateTime.Parse(model.dtProcess.ToString()).AddDays(1);
                        X++;

                    }
                }
                else
                {
                    if (optSts == "H" || optSts == "R" || optSts == "W" || optSts == "S")
                    {
                        sqlQuery = "delete HR_ProssType where ComId = " + comid + " and ProssDt =  '" + Helper.GTRDate(model.dtProcess.ToString()) + "' ";
                        arQuery.Add(sqlQuery);
                        sqlQuery = "insert into HR_ProssType(ComId, ProssDt, DaySts, DayStsB, IsLock) values(" + comid + ", '" + Helper.GTRDate(model.dtProcess.ToString()) + "', '" + optSts + "', '" + optSts + "', 0)";
                        arQuery.Add(sqlQuery);

                    }
                    sqlQuery = "Exec HR_PrcProcessAttend " + comid + ",'" + Helper.GTRDate(model.dtProcess.ToString()) + "'";
                    arQuery.Add(sqlQuery);

                }

                Helper.GTRSaveDataWithSQLCommand(arQuery);
                return "Process complete";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        private void prcInsertEmp(HR_AttendanceProcess model, string optCriteria)
        {
            _attendanceProcessRepository.prcInsertEmp(model, optCriteria);
        }
        #endregion

        #region Raw Data View
        public IActionResult RawDataViewList(DateTime? From, DateTime? To, string Emp, string act = "")
        {

            string comid = HttpContext.Session.GetString("comid");
            //ViewData["EmpId"] = _rawDataViewRepository.GetEmpList();
            ViewBag.empList = _rawDataViewRepository.GetEmpList();

            //var dateFrom = From.Value.Date;
            //var dateTo = To.Value.Date;
            if (Emp == null)
            {
                var dateFrom1 = From.HasValue ? From.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var dateTo1 = To.HasValue ? To.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                ViewBag.RawDataViewModel = new List<RawDataVM>();
                ViewBag.DateFrom = dateFrom1;
                ViewBag.DateTo = dateTo1;

                SqlParameter p5 = new SqlParameter("@DtFrom", dateFrom1);
                SqlParameter p6 = new SqlParameter("@DtTo", dateTo1);
                SqlParameter p7 = new SqlParameter("@ComId", comid);
                var data1 = Helper.ExecProcMapTList<RawDataVM>("dbo.Hr_prcGetRawData", new SqlParameter[] { p5, p6, p7 });
                if (act == "view")
                    return View(data1);
                else
                    return View();
            }
            else
            {
                var dateFrom = From.HasValue ? From.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var dateTo = To.HasValue ? To.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                ViewBag.RawDataViewModel = new List<RawDataVM>();
                ViewBag.DateFrom = dateFrom;
                ViewBag.DateTo = dateTo;
                //ViewBag.empList = new SelectList(db.HR_Emp_Info.FromSqlRaw("dbo.prcGetEmployeeList"), "EmpName", "EmpName");


                SqlParameter p1 = new SqlParameter("@DtFrom", dateFrom);
                SqlParameter p2 = new SqlParameter("@DtTo", dateTo);
                SqlParameter p3 = new SqlParameter("@EmpName", Emp);
                SqlParameter p4 = new SqlParameter("@ComId", comid);
                var data = Helper.ExecProcMapTList<RawDataVM>("dbo.Hr_prcGetRawDataTest", new SqlParameter[] { p1, p2, p3, p4 });
                if (act == "view")
                    return View(data);

                else
                    return View();
            }
        }
        #endregion

        #region Recreation
        public IActionResult RecreationList()
        {
            var reCreation = _recreationRepository.prcRecationList();
            ViewBag.Recreation = reCreation;
            return View();
        }
        public IActionResult CreateRecreation(List<HR_Emp_Recreation> recreations)
        {
            try
            {
                _recreationRepository.CreateRecreation(recreations);
                TempData["Message"] = "Re-Creation Save/Update Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }

        }
        #endregion

        #region Employee Shift Input
        public IActionResult EmpShiftInputList()
        {
            string comid = HttpContext.Session.GetString("comid");

            ViewBag.EmployeeInfo = _empShiftInputRepository.GetEmpShiftAll();
            ViewBag.Shift = _empShiftInputRepository.GetShiftAll();

            return View();
        }


        [HttpGet]
        public IActionResult ShiftInfo()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            string companyrole = HttpContext.Session.GetString("companyRole");

            ViewBag.Role = companyrole;

            return View();
        }

      
        public IActionResult GetShiftInfo(int? emText, string From, string To, string? DeptName, string? DesigName,
            string? SectName, string? EmpName, string? EmpCode, string? DtPunchDate, string? DtPunchTime, int page = 1, int size = 5
            )
        {
            if(emText == null)
            {
                emText = 0;
            }
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            string companyrole = HttpContext.Session.GetString("companyRole");

            ViewBag.Role = companyrole;

            var query = $"EXEC HR_PrcGetShiftInfo '{comid}', {emText}, '{From}', '{To}'";

            SqlParameter[] sqlParameters = new SqlParameter[4];
            sqlParameters[0] = new SqlParameter("@ComId", comid);
            sqlParameters[1] = new SqlParameter("@EmpId", emText);
            sqlParameters[2] = new SqlParameter("@dtfrom", From);
            sqlParameters[3] = new SqlParameter("@dtTo", To);

            var listOfShiftInfo = Helper.ExecProcMapTList<ShiftInfoViewModel>("HR_PrcGetShiftInfo", sqlParameters);
            //return listOfShiftInfo;

            //ShiftInfoViewModel
            return Json(new { Success = 1, error = false, EmployeeList = listOfShiftInfo});       
        }


        //kamrul Edit/Delete manual shifting data
       
        public IActionResult DeleteShiftData(int id)
        {
            var data = _context.HR_Emp_ShiftInput.Where(x => x.ShiftInputId == id).FirstOrDefault();
            if (data == null)
            {
                var data1 = _context.HR_Emp_ShiftInput.Where(x => x.ShiftInputId == id).FirstOrDefault();
                data1.IsDelete = true;
                _context.Update(data1);
            }
            else
            {
                data.IsDelete = true;
                _context.Update(data);
            }
           _context.SaveChanges();

            return RedirectToAction("ShiftInfo");
        }

        public IActionResult Delete(int id)
        {
            var data = _context.HR_Emp_ShiftInput.Where(x => x.ShiftInputId == id).FirstOrDefault();
            if (data == null)
            {
                var data1 = _context.HR_Emp_ShiftInput.Where(x => x.ShiftInputId == id).FirstOrDefault();
                data1.IsDelete = true;
                _context.Update(data1);
            }
            else
            {
                data.IsDelete = true;
                _context.Update(data);
            }
            _context.SaveChanges();

            return RedirectToAction("ShiftInfo");
        }

        public IActionResult EditEmpShift(int id)
        {
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            var result = (from a in _context.HR_Emp_ShiftInput
                          join i in _context.HR_Emp_Info on a.EmpId equals i.EmpId
                          join dp in _context.Cat_Department on i.DeptId equals dp.DeptId
                          join ds in _context.Cat_Designation on i.DesigId equals ds.DesigId
                          join cs in _context.Cat_Section on i.SectId equals cs.SectId
                          join s in _context.Cat_Shift on a.ShiftId equals s.ShiftId
                          join ms in _context.Cat_Shift on i.ShiftId equals ms.ShiftId
                          join cf in _context.Cat_Floor on i.FloorId equals cf.FloorId
                          join cl in _context.Cat_Line on i.LineId equals cl.LineId
                          where a.ShiftInputId == id
                          select new ShiftInfoViewModel
                          {
                              Id = a.ShiftInputId,
                              ShiftId =a.ShiftId,
                              EmpCode = i.EmpCode,
                              EmpName = i.EmpName,
                              DeptName = dp.DeptName,
                              SectName = cs.SectName,
                              DesigName = ds.DesigName,
                              Main_Shift = ms.ShiftName,
                              Assigned_Shift = s.ShiftName,
                              FloorName = cf.FloorName,
                              LineName = cl.LineName,
                              dtFrom = a.FromDate.ToString("dd MMMM yyyy"),
                              dtTo = a.ToDate.ToString("dd MMMM yyyy")
                          }).FirstOrDefault();

            if (result != null)
            {
                return View(result);
            }

           
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(ShiftInfoViewModel obj)
        {
            var RawData = obj;
            var data = _context.HR_Emp_ShiftInput.Where(x => x.ShiftInputId == obj.Id).FirstOrDefault();
           
            if (data != null)
            {
                data.ShiftId = obj.ShiftId;
                //data.ToDate = obj.dtTo;
                //data.FromDate = obj.dtFrom;

                DateTime toDate;
                if (DateTime.TryParse(obj.dtTo, out toDate))
                {
                    data.ToDate = toDate;
                }
                else
                {
                    
                }

                // Parse string representation of date to DateTime
                DateTime fromDate;
                if (DateTime.TryParse(obj.dtFrom, out fromDate))
                {
                    data.FromDate = fromDate;
                }
                else
                {
                    
                }
                _context.Update(data);
            }
           
            _context.SaveChanges();
            return RedirectToAction("ShiftInfo");
           
        }


        [HttpPost]
        public IActionResult DeleteShiftDataList([FromBody] List<ShiftInfoViewModel> itemList)
        {
            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    var data = _context.HR_Emp_ShiftInput.Where(x => x.ShiftInputId == item.Id).FirstOrDefault();
                    if (data != null)
                    {
                        data.IsDelete = true;
                        _context.Update(data);
                    }
                   
                    _context.SaveChanges();
                }
                return Json(true);
            }
            return Json(false);
        }




        [HttpPost]
        public async Task<ActionResult> EmpShiftPost(string empShiftsString)
        {
            try
            {
                //List<HR_Emp_ShiftInput> empShifts 

                List<HR_Emp_ShiftInput> empShifts = JsonConvert.DeserializeObject<List<HR_Emp_ShiftInput>>(empShiftsString);

                await _empShiftInputRepository.EmpShiftPost(empShifts);
                TempData["Message"] = "Employee shift create/update Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });

            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        /// <summary>
        /// Excel Shift Upload
        /// </summary>
        public async Task<IActionResult> ShiftUploadFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/Shift/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/Shift/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetShiftExcel(file.FileName);
                    if (addition.Count() > 0)
                    {
                        foreach (var item in addition)
                        {
                            var exist = _context.HR_Emp_ShiftInput.Where(w => w.ComId == comid && w.DtDate.Date == item.DtDate.Date && w.EmpId == item.EmpId).Select(s => s).FirstOrDefault();
                            if (exist != null)
                            {
                                _context.HR_Emp_ShiftInput.Remove(exist);
                            }
                        }

                        await _context.HR_Emp_ShiftInput.AddRangeAsync(addition);
                        await _context.SaveChangesAsync();

                        //var Query = $"Exec HR_prcProcessIncrementExcel '{comid}'";
                        //SqlParameter[] sqlParameter = new SqlParameter[1];
                        //sqlParameter[0] = new SqlParameter("@ComId", comid);
                        //Helper.ExecProc("HR_prcProcessIncrementExcel", sqlParameter);

                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("EmpShiftInputList");
        }


        public async Task<IActionResult> LeaveUploadFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/LeaveExcel/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/LeaveExcel/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetLeaveExcel(file.FileName);
                    if (addition.Count() > 0)
                    {
                        await _context.TempLeaveEntryExcel.AddRangeAsync(addition);
                        await _context.SaveChangesAsync();

                        long empId = 0; 
                        long leaveId = 0; 
                        DateTime dtFrom = DateTime.Now; 
                        DateTime dtTo = DateTime.Now.AddDays(1);

                        var query = $"EXEC HR_prcProcessLeave '{comid}', {empId}, {leaveId}, '{dtFrom}','{dtTo}'";
                        SqlParameter[] sqlParameters = new SqlParameter[5];

                     
                        sqlParameters[0] = new SqlParameter("@ComId", comid);
                        sqlParameters[1] = new SqlParameter("@EmpID", empId);
                        sqlParameters[2] = new SqlParameter("@LeaveID", leaveId);
                        sqlParameters[3] = new SqlParameter("@dtFrom", dtFrom);
                        sqlParameters[4] = new SqlParameter("@dtTo", dtTo);

                        Helper.ExecProc("HR_prcProcessLeave", sqlParameters);

                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("LeaveEntryList");
        }


        private List<TempLeaveEntryExcel> GetLeaveExcel(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/LeaveExcel/" + comid + "/" + userid + "/" + fName);

           // List<HR_Emp_ShiftInput> shift = new List<HR_Emp_ShiftInput>();

            List<TempLeaveEntryExcel> EmpLeaveList = new List<TempLeaveEntryExcel>();

            try
            {
                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader != null && reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                // Check if all values in the row are null
                                bool allValuesNull = true;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (reader.GetValue(i) != null)
                                    {
                                        allValuesNull = false;
                                        break;
                                    }
                                }

                                if (!allValuesNull)
                                {
                                    EmpLeaveList.Add(new TempLeaveEntryExcel()
                                    {
                                        EmpCode = reader.GetValue(0)?.ToString(),
                                        DtInput = DateTime.Parse(reader.GetValue(1)?.ToString()),
                                        DtFrom = DateTime.Parse(reader.GetValue(2)?.ToString()),
                                        DtTo = DateTime.Parse(reader.GetValue(3)?.ToString()),
                                        LvType = reader.GetValue(4)?.ToString(),
                                        TotalDay = int.TryParse(reader.GetValue(5)?.ToString(), out int totalDay) ? totalDay : 0,
                                        Remarks = reader.GetValue(6)?.ToString(),
                                        //ComId = HttpContext.Session.GetString("comid")
                                        ComId = comid
                                    });


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return EmpLeaveList;
        }

        public async Task<IActionResult> RateUploadFileUBL(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/Rate/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/Rate/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetRateExcelUBL(file.FileName);
                    var updatelist = new List<HR_Emp_Info>();
                    var updatelistP = new List<HR_Emp_PersonalInfo>();
                    if (addition.Count() > 0)
                    {
                        foreach (var item in addition)
                        {
                            var exist = _context.HR_Emp_Info.Where(w => w.ComId == comid && w.EmpCode == item.EmpCode).Select(s => s).FirstOrDefault();
                            if (exist != null)
                            {
                                if (item.Rate != 0.0)
                                {
                                    exist.Rate = item.Rate;
                                }
                                if (item.Category != null)
                                {
                                    var categoryId = _context.Categories.Where(x => x.Name == item.Category).Select(s => s.CategoryId).FirstOrDefault();
                                    exist.CategoryId = categoryId;
                                }
                                if (item.DtJoin != DateTime.MinValue)
                                {
                                    exist.DtJoin = item.DtJoin;
                                }
                                if (item.Gs != 0.0)
                                {
                                    exist.GrossSal = item.Gs;
                                }

                                updatelist.Add(exist);
                            }

                            var exist2 = _context.HR_Emp_PersonalInfo.Where(w => w.ComId == comid && w.EmpId == exist.EmpId).Select(s => s).FirstOrDefault();
                            if (exist2 != null)
                            {
                                if (item.WeekDay != 0)
                                {
                                    exist2.WeekDay = item.WeekDay;
                                }
                                if (item.WeekDay2 != 0)
                                {
                                    exist2.WeekDay2 = item.WeekDay2;
                                }
                                updatelistP.Add(exist2);
                            }
                        }

                        _context.HR_Emp_Info.UpdateRange(updatelist);
                        await _context.SaveChangesAsync();
                        _context.HR_Emp_PersonalInfo.UpdateRange(updatelistP);
                        await _context.SaveChangesAsync();

                        //var Query = $"Exec HR_prcProcessIncrementExcel '{comid}'";
                        //SqlParameter[] sqlParameter = new SqlParameter[1];
                        //sqlParameter[0] = new SqlParameter("@ComId", comid);
                        //Helper.ExecProc("HR_prcProcessIncrementExcel", sqlParameter);

                        TempData["Message"] = "Data Updated Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("VendorInfoList");

        }
        private List<VendorUploadXL> GetRateExcelUBL(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/Rate/" + comid + "/" + userid + "/" + fName);

            List<VendorUploadXL> employee = new List<VendorUploadXL>();

            try
            {
                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader != null && reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                //var d = DateTime.Parse(reader.GetValue(1).ToString());
                                VendorUploadXL temp = new VendorUploadXL();
                                temp.EmpCode = reader.GetValue(0).ToString();
                                temp.Category = reader.GetValue(1)?.ToString();
                                string dtValue = reader.GetValue(2)?.ToString();
                                temp.DtJoin = !string.IsNullOrEmpty(dtValue) ? DateTime.Parse(dtValue) : DateTime.MinValue;
                                temp.Gs = reader.GetValue(3) != null ? (double)reader.GetValue(3) : 0.0;
                                temp.Rate = reader.GetValue(4) != null ? (double)reader.GetValue(4) : 0.0;
                                temp.WeekDay = MapWeekdayNameToNumber(reader.GetValue(5)?.ToString() ?? null);
                                temp.WeekDay2 = MapWeekdayNameToNumber(reader.GetValue(6)?.ToString() ?? null);
                                employee.Add(temp);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return employee;

        }

        #endregion
        //kamrul Bank data uplod for vendor
        #region bankdata



        public async Task<IActionResult> BankDataUpload(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/Bank/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/Bank/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetBankExcel(file.FileName);
                    var updatelist = new List<HR_Emp_BankInfo>();
                    //var updatelistP = new List<HR_Emp_PersonalInfo>();
                    if (addition.Count() > 0)
                    {

                        foreach (var item in addition)
                        {
                            var exist = _context.HR_Emp_BankInfo.Where(w => w.ComId == comid && w.HR_Emp_Info.EmpCode == item.EmpCode).Select(s => s).FirstOrDefault();
                            if (exist != null)
                            {
                                ////EmpCode = reader.GetValue(0).ToString(),                                                                                                                                                                                                  
                                //exist.PayModeId = GetPayModeIdByPayModeName(item.PayMode);
                                //exist.AccountNumber = item.AccountNumber;
                                //exist.AccountName = item.AccountName;
                                //exist.RoutingNumber = item.RoutingNumber;
                                //exist.BankId = GetBankIdByBankName(item.BankName);                           
                                //exist.AccTypeId = GetAcTypeACTypeName(item.AccountType);
                                //exist.BranchId = GetBranchIdByBranchName(item.BranchName);

                                // Update each field only if the corresponding item property is not null
                                exist.PayModeId = UpdateFieldIfNotNull(GetPayModeIdByPayModeName(item.PayMode), exist.PayModeId);
                                exist.AccountNumber = UpdateFieldIfNotNull(item.AccountNumber, exist.AccountNumber);
                                exist.AccountName = UpdateFieldIfNotNull(item.AccountName, exist.AccountName);
                                exist.RoutingNumber = UpdateFieldIfNotNull(item.RoutingNumber, exist.RoutingNumber);
                                exist.BankId = UpdateFieldIfNotNull(GetBankIdByBankName(item.BankName), exist.BankId);
                                exist.AccTypeId = UpdateFieldIfNotNull(GetAcTypeACTypeName(item.AccountType), exist.AccTypeId);
                                exist.BranchId = UpdateFieldIfNotNull(GetBranchIdByBranchName(item.BranchName), exist.BranchId);


                                updatelist.Add(exist);
                                _context.HR_Emp_BankInfo.UpdateRange(updatelist);
                            }
                            else
                            {
                                var existingBankInfo = _context.HR_Emp_Info
                               .FirstOrDefault(w => w.ComId == comid && w.EmpCode == item.EmpCode);

                                int empId = existingBankInfo.EmpId;
                                var newBankInfo = new HR_Emp_BankInfo
                                {

                                    ComId = comid,
                                    EmpId = empId,

                                };

                                newBankInfo.PayModeId = GetPayModeIdByPayModeName(item.PayMode);
                                newBankInfo.AccountNumber = item.AccountNumber;
                                newBankInfo.AccountName = item.AccountName;
                                newBankInfo.RoutingNumber = item.RoutingNumber;
                                newBankInfo.BankId = GetBankIdByBankName(item.BankName);
                                newBankInfo.AccTypeId = GetAcTypeACTypeName(item.AccountType);
                                newBankInfo.BranchId = GetBranchIdByBranchName(item.BranchName);

                                _context.HR_Emp_BankInfo.Add(newBankInfo);
                            }
                        }


                        await _context.SaveChangesAsync();


                        TempData["Message"] = "Data Updated Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("VendorInfoList");

        }
        private List<BankinfoUpdateExcelModel> GetBankExcel(string fName)
        {

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/Bank/" + comid + "/" + userid + "/" + fName);

            List<BankinfoUpdateExcelModel> employee = new List<BankinfoUpdateExcelModel>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader != null && reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                //var d = DateTime.Parse(reader.GetValue(1).ToString());

                                employee.Add(new BankinfoUpdateExcelModel()
                                {
                                    EmpCode = reader.GetValue(0).ToString(),
                                    PayMode = reader.GetValue(1)?.ToString(),
                                    BankName = reader.GetValue(2)?.ToString(),
                                    BranchName = reader.GetValue(3)?.ToString(),
                                    RoutingNumber = reader.GetValue(4)?.ToString(),
                                    AccountNumber = reader.GetValue(5)?.ToString(),
                                    AccountName = reader.GetValue(6)?.ToString(),
                                    AccountType = reader.GetValue(7)?.ToString(),
                                });



                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return employee;

        }

        #endregion

        #region Kamrul for UBL
        //checking null
        private T UpdateFieldIfNotNull<T>(T newValue, T existingValue)
        {
            return newValue != null ? newValue : existingValue;
        }
        //kamrul get branch Id 
        public int? GetPayModeIdByPayModeName(string payModeName)
        {
            var payMode = _context.Cat_PayMode
                               .FirstOrDefault(p => p.PayModeName == payModeName);

            return payMode?.PayModeId;
        }

        //kamrul get bank id
        public int GetBankIdByBankName(string bankName)
        {
            var payMode = _context.Cat_Bank
                               .FirstOrDefault(p => p.BankName == bankName);

            return payMode?.BankId ?? 29;
        }

        public int GetBranchIdByBranchName(string bankName)
        {
            var payMode = _context.Cat_BankBranch
                               .FirstOrDefault(p => p.BranchName == bankName);

            return payMode?.BranchId ?? 33;
        }

        public int? GetAcTypeACTypeName(string Actype)
        {
            var payMode = _context.Cat_AccountType
                               .FirstOrDefault(p => p.AccTypeName == Actype);

            return payMode?.AccTypeId;
        }

        //kamrul
        public int GetCategoryIdByName(string categoryName)
        {
            var categoryId = _context.Cat_Variable
                .Where(x => x.VarType == "CatagoryType" && x.VarName == categoryName)
                .Select(x => x.VarId)
                .FirstOrDefault();

            return categoryId;
        }

        //Kamrul Islam
        private int MapWeekdayNameToNumber(string weekdayName)
        {
            if (weekdayName != null)
            {
                switch (weekdayName.ToUpper())
                {
                    case "SUNDAY":
                        return 1;
                    case "MONDAY":
                        return 2;
                    case "TUESDAY":
                        return 3;
                    case "WEDNESDAY":
                        return 4;
                    case "THURSDAY":
                        return 5;
                    case "FRIDAY":
                        return 6;
                    case "SATURDAY":
                        return 7;
                    default:

                        throw new ArgumentException($"Invalid weekday name: {weekdayName}");
                }
            }
            else
                return 0;
        }


        public async Task<IActionResult> RateUploadFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/Rate/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/Rate/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetRateExcel(file.FileName);
                    var updatelist = new List<HR_Emp_Info>();
                    if (addition.Count() > 0)
                    {

                        foreach (var item in addition)
                        {
                            var exist = _context.HR_Emp_Info.Where(w => w.ComId == comid && w.EmpCode == item.EmpCode).Select(s => s).FirstOrDefault();
                            if (exist != null)
                            {
                                exist.Rate = item.Rate;
                                updatelist.Add(exist);
                            }
                        }

                        _context.HR_Emp_Info.UpdateRange(updatelist);
                        await _context.SaveChangesAsync();

                        //var Query = $"Exec HR_prcProcessIncrementExcel '{comid}'";
                        //SqlParameter[] sqlParameter = new SqlParameter[1];
                        //sqlParameter[0] = new SqlParameter("@ComId", comid);
                        //Helper.ExecProc("HR_prcProcessIncrementExcel", sqlParameter);

                        TempData["Message"] = "Data Updated Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("VendorInfoList");
        }
        private List<HR_Emp_Info> GetRateExcel(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/Rate/" + comid + "/" + userid + "/" + fName);

            List<HR_Emp_Info> employee = new List<HR_Emp_Info>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader != null && reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                //var d = DateTime.Parse(reader.GetValue(1).ToString());

                                employee.Add(new HR_Emp_Info()
                                {
                                    EmpCode = reader.GetValue(0).ToString(),
                                    Rate = (double)reader.GetValue(1),
                                });



                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return employee;
        }
        private List<HR_Emp_ShiftInput> GetShiftExcel(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/Shift/" + comid + "/" + userid + "/" + fName);

            List<HR_Emp_ShiftInput> shift = new List<HR_Emp_ShiftInput>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader != null && reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                var d = DateTime.Parse(reader.GetValue(1).ToString());


                                for (int i = 0; i < 31; i++)
                                {
                                    if (reader.GetValue(4 + i) == null)
                                    {
                                        continue;
                                    }
                                    var exshift = reader.GetValue(4 + i).ToString();
                                    var shiftId = _context.Cat_Shift.Where(w => w.ComId == comid && w.ShiftName == exshift).Select(s => s.ShiftId).FirstOrDefault();

                                    shift.Add(new HR_Emp_ShiftInput()
                                    {
                                        ComId = comid,
                                        DtDate = d.AddDays(i),
                                        FromDate = d.AddDays(i),
                                        ToDate = d.AddDays(i),
                                        EmpId = _context.HR_Emp_Info.Where(w => w.ComId == comid && w.EmpCode == (reader.GetValue(2).ToString())).Select(s => s.EmpId).FirstOrDefault(),
                                        ShiftId = shiftId,
                                        UserId = userid,
                                        DtTran = d,
                                        IsDelete = false,

                                    }); ; ;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return shift;
        }


        public ActionResult DownloadShiftFile(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
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
        public ActionResult DownloadShiftFiles(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
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


        #endregion

        #region Employee Shift Input Ex
        public IActionResult EmpShiftInputListEx()
        {
            string comid = HttpContext.Session.GetString("comid");

            ViewBag.EmployeeInfo = _empShiftInputRepository.GetEmpShiftAll();
            ViewBag.Shift = _empShiftInputRepository.GetShiftAll();

            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> EmpShiftPostEx(string empShiftsString)
        //{
        //    try
        //    {
        //        //List<HR_Emp_ShiftInput> empShifts 

        //        List<HR_Emp_ShiftInput> empShifts = JsonConvert.DeserializeObject<List<HR_Emp_ShiftInput>>(empShiftsString);

        //        await _empShiftInputRepository.EmpShiftPost(empShifts);


        //        TempData["Message"] = "Employee shift create/update Successfully";
        //        return Json(new { Success = 1, ex = TempData["Message"].ToString() });


        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }
        //}

        /// <summary>
        /// Excel Shift Upload
        /// </summary>



        #endregion

        #region Loan Return
        public IActionResult LoanReturnList()
        {
            var comid = HttpContext.Session.GetString("comid");

            var empData = _loanReturnRepository.EmpData()
                .Select(s => new
                {
                    EmpId = s.EmpId,
                    EmpCode = s.EmpCode,
                    EmpName = s.EmpName,
                    DtJoin = s.DtJoin
                })
                .ToList();

            ViewBag.EmpInfo = empData;
            ViewData["EmpId"] = _rawDataViewRepository.GetEmpList();
            ViewBag.LoanType = _loanReturnRepository.CatVariableList();
            return View();
        }

        [HttpPost]
        public JsonResult CreateLoanReturn(HR_Loan_Return LoanReturn)
        {
            if (ModelState.IsValid)
            {
                var check = _loanReturnRepository.CheckData(LoanReturn);
                if (check != null)
                {
                    TempData["Message"] = "Data Already Exist";
                    return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                }

                string comid = HttpContext.Session.GetString("comid");
                LoanReturn.ComId = comid;
                LoanReturn.UserId = HttpContext.Session.GetString("userid");
                if (LoanReturn.LoanReturnId > 0)
                {
                    LoanReturn.DateAdded = DateTime.Now;
                    _loanReturnRepository.Update(LoanReturn);
                    TempData["Message"] = "Data Update Successfully";
                }
                else
                {
                    LoanReturn.DateUpdated = DateTime.Now;
                    LoanReturn.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _loanReturnRepository.Add(LoanReturn);
                    TempData["Message"] = "Data Save Successfully";
                }
                _context.SaveChanges();
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });

            }
            else
            {
                TempData["Message"] = "Models state is not valid";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        [HttpPost]
        public JsonResult DeleteLoanReturnAjax(int DedId)
        {
            var LoanReturn = _loanReturnRepository.FindById(DedId);
            if (LoanReturn != null)
            {
                _loanReturnRepository.Remove(LoanReturn);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }

            TempData["Message"] = "Data Not Found";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }

        public ActionResult LoadLoanReturnPartial(DateTime date)
        {
            var LoanReturns = _loanReturnRepository.LoadLoanReturnPartial(date);
            return Json(LoanReturns);
        }

        #endregion

        #region Employee Info table fill

        #endregion

        #region Employee Info
        public IActionResult EmployeeInfoList()
        {

            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            //Return Employee Value
            return View(_empInfoRepository.GetEmpInfoAll());
        }
        #region commented code
        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        var comid = (HttpContext.Session.GetString("comid"));

        //        Microsoft.Extensions.Primitives.StringValues y = "";

        //        var x = Request.Form.TryGetValue("search[value]", out y);

        //        //if (y.ToString().Length > 0)
        //        //{

        //        var query = _empInfoRepository.EmpInfo();

        //        var parser = new Parser<EmployeeInfo>(Request.Form, query);

        //        return Json(parser.Parse());

        //        //}
        //        //return Json("");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = "0", error = ex.Message });
        //        //throw ex;
        //    }

        //}

        #endregion

        public IActionResult EmpCodeExist(string code)
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _empInfoRepository.GetEmp().Any(e => e.EmpCode == code && e.ComId == comid && e.IsDelete == false);
            return Json(_empInfoRepository.GetEmp().Any(e => e.EmpCode == code && e.ComId == comid && e.IsDelete == false));
        }

        public IActionResult CreateEmpInfo()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6", true));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            ViewData["WeekDay"] = WeekDaylist;

            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
            ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

            ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
            ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();

            ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
            ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();

            ViewData["BId"] = _buildingTypeRepository.GetBuildingType();

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();

            ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
            ViewData["BankId"] = _bankRepository.GetBankList();
            ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
            ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();

            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SubCategoryId"] = _hREmpInfoRepository.SubCategoryList();
            ViewData["CategoryId"] = _hREmpInfoRepository.CategoryList();
            var atttype = _context.Cat_Variable.Where(x => x.VarType == "AttendenceType").OrderBy(x => x.SLNo).Select(x => new { x.VarName }).ToList();
            ViewBag.MobileAttendence = new SelectList(atttype, "VarName", "VarName");
            var iscorporate = _context.Companys.Where(x => x.CompanyCode == comid).Select(y => y.isCorporate).FirstOrDefault();
            if (iscorporate == null || iscorporate == false)
            {
                ViewData["IsCorporate"] = 0;
            }
            else
            {
                ViewData["IsCorporate"] = 1;
            }
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });

            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            ViewData["SkillId"] = _skillRepository.GetSkillList();

            int status = 0, firstaprv = 0;

            var data = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.IsDelete == false && x.ApprovalType == 1175).FirstOrDefault();
            if (data != null)
            {
                if (data.IsApprove == true)
                {
                    if (data.IsFirstLeaveApprove == true)
                    {
                        status = 1;
                        firstaprv = 1;
                    }
                    else
                    {
                        status = 1;
                    }
                }
            }
            ViewData["Status"] = status;
            ViewData["FirstApprove"] = firstaprv;

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> CreateEmpInfo(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var dtConfirm = hrEmpInfo.DtConfirm;
            //var errors = ModelState.Where(x => x.Value.Errors.Any())
            //   .Select(x => new { x.Key, x.Value.Errors });
            string comid = HttpContext.Session.GetString("comid");
            if (true)
            {
                hrEmpInfo.UserId = HttpContext.Session.GetString("userid");
                hrEmpInfo.ComId = HttpContext.Session.GetString("comid");
                if (hrEmpInfo.EmpId > 0)
                {
                    _empInfoRepository.EmpInfoPost(hrEmpInfo, file, signFile);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hrEmpInfo.EmpId.ToString(), "Update", hrEmpInfo.EmpName.ToString());
                }
                else
                {

                    _empInfoRepository.EmpInfoPostElse(hrEmpInfo, file, signFile);
                    await _context.SaveChangesAsync();

                    SqlParameter[] sqlParemeter = new SqlParameter[3];
                    sqlParemeter[0] = new SqlParameter("@ComID", comid);
                    sqlParemeter[1] = new SqlParameter("@EmpID", hrEmpInfo.EmpId);
                    sqlParemeter[2] = new SqlParameter("@dtJoin", hrEmpInfo.DtJoin);

                    string query = $"Exec HR_prcProcessLeaveInput '{comid}', {hrEmpInfo.EmpId}, '{hrEmpInfo.DtJoin}'";
                    Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeaveInput", sqlParemeter);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";

                }
                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hrEmpInfo.EmpId.ToString(), "Create", hrEmpInfo.EmpName.ToString());

                return RedirectToAction(nameof(EmployeeInfoList));
            }
            else
            {
                TempData["Message"] = "Something is wrong!";
                TempData["Status"] = "3";
            }

            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
            ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

            ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
            ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();

            ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
            ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();

            ViewData["BId"] = _buildingTypeRepository.GetBuildingType();

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();

            ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
            ViewData["BankId"] = _bankRepository.GetBankList();
            ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
            ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();

            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            var atttype = _context.Cat_Variable.Where(x => x.VarType == "AttendenceType").OrderBy(x => x.SLNo).Select(x => new { x.VarName }).ToList();
            ViewBag.MobileAttendence = new SelectList(atttype, "VarName", "VarName");
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });

            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6", true));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));

            ViewData["WeekDay"] = WeekDaylist;
            return View(hrEmpInfo);
        }
        public JsonResult SearchEmployeesForHOD(string term)
        {
            string comid = HttpContext.Session.GetString("comid");
            var employees = _context.HR_Emp_Info
                .Where(e => (e.EmpName.Contains(term) || e.EmpCode.Contains(term)) && e.ComId == comid && e.IsHOD == true && e.IsDelete == false && e.IsInactive == false)
                .Select(e => new { label = e.EmpCode + " " + e.EmpName, value = e.EmpId })
                .Take(10)
                .ToList();

            return new JsonResult(employees);
        }

        // Emp info edit 
        public async Task<IActionResult> EditEmpInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hrEmpInfo = await _empInfoRepository.EmpInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {

                ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                ViewBag.permission = true;
            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1187))
            {
                if (hrEmpInfo.EmpTypeId == 1 || hrEmpInfo.EmpTypeId == 2)
                {
                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = true;
                }
                else
                {

                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = false;

                }

            }
            else if (approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                if (hrEmpInfo.EmpTypeId == 2 || hrEmpInfo.EmpTypeId == 3)
                {
                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = true;
                }
                else
                {

                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = false;

                }

            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1257))
            {
                if (hrEmpInfo.EmpTypeId == 1 || hrEmpInfo.EmpTypeId == 3)
                {
                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = true;
                }
                else
                {

                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = false;

                }

            }
            else if (approvetype.Contains(1186))
            {
                if (hrEmpInfo.EmpTypeId == 1)
                {
                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = true;
                }
                else
                {

                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = false;

                }

            }
            else if (approvetype.Contains(1187))
            {
                if (hrEmpInfo.EmpTypeId == 2)
                {
                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = true;
                }
                else
                {

                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = false;

                }

            }
            else if (approvetype.Contains(1257))
            {
                if (hrEmpInfo.EmpTypeId == 3)
                {
                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = true;
                }
                else
                {

                    ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                    ViewBag.permission = false;

                }

            }

            else
            {
                ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();
                ViewBag.permission = true;

            }
            var iscorporate = _context.Companys.Where(x => x.CompanyCode == comid).Select(y => y.isCorporate).FirstOrDefault();

            if (iscorporate == null || iscorporate == false)
            {
                ViewData["IsCorporate"] = 0;
            }
            else
            {
                ViewData["IsCorporate"] = 1;
            }





            //ViewBag.DtJoin = DateTime.Parse(hrEmpInfo.DtJoin).ToString("dd-MMM-yy");
            ViewBag.Title = "Edit";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo?.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }

            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }

            ViewData["GenderId"] = _genderRepository.GenderList();



            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpAccTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["SubCategoryId"] = _hREmpInfoRepository.SubCategoryList();
            ViewData["CategoryId"] = _hREmpInfoRepository.CategoryList();
            var atttype = _context.Cat_Variable.Where(x => x.VarType == "AttendenceType").OrderBy(x => x.SLNo).Select(x => new { x.VarName }).ToList();
            ViewBag.MobileAttendence = new SelectList(atttype, "VarName", "VarName");
            if (hrEmpInfo.FirstAprvId != null)
            {
                var empCode = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FirstAprvId).Select(y => y.EmpCode).FirstOrDefault();
                var empName = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FirstAprvId).Select(y => y.EmpName).FirstOrDefault();
                hrEmpInfo.FirstAprvName = empCode + " " + empName;
            }
            if (hrEmpInfo.FinalAprvId != null)
            {
                var empCode = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FinalAprvId).Select(y => y.EmpCode).FirstOrDefault();
                var empName = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FinalAprvId).Select(y => y.EmpName).FirstOrDefault();
                hrEmpInfo.FinalAprvName = empCode + " " + empName;
            }
            int status = 0, firstaprv = 0;

            var data = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.IsDelete == false && x.ApprovalType == 1175).FirstOrDefault();
            if (data != null)
            {
                if (data.IsApprove == true)
                {
                    if (data.IsFirstLeaveApprove == true)
                    {
                        status = 1;
                        firstaprv = 1;
                    }
                    else
                    {
                        status = 1;
                    }
                }
            }
            ViewData["Status"] = status;
            ViewData["FirstApprove"] = firstaprv;
            return View("CreateEmpInfo", hrEmpInfo);
        }

        // GET: EmpInfoTemp/Delete/5
        public async Task<IActionResult> DeleteEmpInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var hrEmpInfo = await _empInfoRepository.EmpInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }


            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }
            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpAccTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["SubCategoryId"] = _hREmpInfoRepository.SubCategoryList();
            ViewData["CategoryId"] = _hREmpInfoRepository.CategoryList();

            if (hrEmpInfo.FirstAprvId != null)
            {
                var empCode = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FirstAprvId).Select(y => y.EmpCode).FirstOrDefault();
                var empName = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FirstAprvId).Select(y => y.EmpName).FirstOrDefault();
                hrEmpInfo.FirstAprvName = empCode + " " + empName;
            }
            if (hrEmpInfo.FinalAprvId != null)
            {
                var empCode = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FinalAprvId).Select(y => y.EmpCode).FirstOrDefault();
                var empName = _context.HR_Emp_Info.Where(x => x.EmpId == hrEmpInfo.FinalAprvId).Select(y => y.EmpName).FirstOrDefault();
                hrEmpInfo.FinalAprvName = empCode + " " + empName;
            }
            return View("CreateEmpInfo", hrEmpInfo);
        }

        // POST: EmpInfoTemp/Delete/5
        [HttpPost, ActionName("DeleteEmpInfo")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteEmpInfoConfirmed(int id)
        {

            try
            {
                var HR_Emp_Info = _empInfoRepository.FindById(id);
                _empInfoRepository.Delete(HR_Emp_Info);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Info.EmpId.ToString(), "Delete", HR_Emp_Info.EmpName);

                return Json(new { Success = 1, EmpId = HR_Emp_Info.EmpId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }


        [HttpGet]
        public ActionResult GetPoliceStation(int id)
        {
            string comid = HttpContext.Session.GetString("comid");

            var policeStation = _context.Cat_PoliceStation
                .Select(p => new
                {
                    DistId = p.DistId,
                    Id = p.PStationId,
                    Name = p.PStationName,
                    ComId = p.ComId
                })
                .Where(p => p.DistId == id).ToList();
            return Json(new { PoliceStation = policeStation });
        }
        [HttpGet]
        public ActionResult GetPostOffice(int id)
        {
            string comid = HttpContext.Session.GetString("comid");
            var postOffice = _context.Cat_PostOffice
               .Select(p => new
               {
                   PStationId = p.PStationId,
                   Id = p.POId,
                   Name = p.POName,
                   ComId = p.ComId
               })
               .Where(p => p.PStationId == id).ToList();
            //.Where(p => p.DistId == id && p.ComId == comid).ToList();
            return Json(postOffice);
        }

        //---------------------For Education-------------------------------//

        [HttpPost]
        //[IgnoreAntiforgeryToken]
        public ActionResult EmpEducation(string HR_Emp_Educations)
        {
            try
            {

                _hREmpInfoRepository.EmpEducationDelete(HR_Emp_Educations);
                TempData["Message"] = "Data Update Successfully";
                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        public async Task<IActionResult> EmpEducationUpdate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hrEmpInfo = await _empInfoRepository.EmpInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            //ViewBag.DtJoin = DateTime.Parse(hrEmpInfo.DtJoin).ToString("dd-MMM-yy");
            ViewBag.Title = "Edit";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }
            var examlist = _context.Cat_Variable.Where(x => x.VarType == "ExamType").ToList();
            List<SelectListItem> Examlist = examlist
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();


            ViewData["ExamList"] = new SelectList(Examlist, "Value", "Text");

            List<SelectListItem> Boardlist = new List<SelectListItem>();
            Boardlist.Add(new SelectListItem("Dhaka", "Dhaka"));
            Boardlist.Add(new SelectListItem("Chattogram", "Chattogram"));
            Boardlist.Add(new SelectListItem("Cumilla", "Cumilla"));
            Boardlist.Add(new SelectListItem("Barishal", "Barishal"));
            Boardlist.Add(new SelectListItem("Jessore", "Jessore"));
            Boardlist.Add(new SelectListItem("Shylhet", "Shylhet"));
            Boardlist.Add(new SelectListItem("Rajshahi", "Rajshahi"));
            Boardlist.Add(new SelectListItem("Dinajpur", "Dinajpur"));
            Boardlist.Add(new SelectListItem("Madrasha", "Madrasha"));

            ViewData["Boardlist"] = new SelectList(Boardlist, "Value", "Text");

            List<SelectListItem> Statuslist = new List<SelectListItem>();
            Statuslist.Add(new SelectListItem("Complete", "Complete"));
            Statuslist.Add(new SelectListItem("Open", "Open"));

            ViewData["Statuslist"] = new SelectList(Statuslist, "Value", "Text");


            var subjectlist = _context.Cat_Variable.Where(x => x.VarType == "Subject").ToList();
            List<SelectListItem> Subjectlist = subjectlist
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();
            ViewData["Subjectlist"] = new SelectList(Subjectlist, "Value", "Text");

            var institute = _context.Cat_Variable.Where(x => x.VarType == "Institute").ToList();
            List<SelectListItem> Institutelist = institute
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();

            ViewData["Institutelist"] = new SelectList(Institutelist, "Value", "Text");

            var comid = HttpContext.Session.GetString("comid");
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _hREmpInfoRepository.EmpBankList(hrEmpInfo);
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            return View(nameof(EmpEducation), hrEmpInfo);
        }
        public IActionResult UploadCertificate(IFormFile Certificate)
        {
            if (Certificate != null)
            {
                string filename = $"{_env.WebRootPath}\\EmpDocument\\Certificates\\{Certificate.FileName}";
                using (FileStream stream = System.IO.File.Create(filename))
                {
                    Certificate.CopyTo(stream);
                    stream.Flush();
                }
            }
            return View("EmpEducation");
        }
        //-------------------------End of Education-----------------------------//



        //---------------------For Employee Devices-------------------------------//

        [HttpPost]
        //[IgnoreAntiforgeryToken]
        public ActionResult EmpDevice(string HR_Emp_Devices)
        {
            try
            {

                _hREmpInfoRepository.EmpDeviceDelete(HR_Emp_Devices);
                TempData["Message"] = "Data Update Successfully";
                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        public async Task<IActionResult> EmpDeviceUpdate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hrEmpInfo = await _empInfoRepository.EmpInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            //ViewBag.DtJoin = DateTime.Parse(hrEmpInfo.DtJoin).ToString("dd-MMM-yy");
            ViewBag.Title = "Edit";
            ViewBag.Title1 = "Create";

            var comid = HttpContext.Session.GetString("comid");
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            var employee = _context.HR_Emp_Info.Where(e => e.ComId == comid && e.IsDelete == false && e.IsInactive == false && e.EmpId == id).ToList();
            List<SelectListItem> employeelist = employee
            .Select(item => new SelectListItem
            {
                Value = item.EmpId.ToString(),
                Text = item.EmpCode + "_" + item.EmpName
            })
            .ToList();

            ViewData["EmpId"] = new SelectList(employeelist, "Value", "Text");

            var deviceType = _context.Cat_Variable.Where(x => x.VarType == "DeviceType").ToList();
            List<SelectListItem> devicelist = deviceType
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();
            ViewData["DeviceList"] = new SelectList(devicelist, "Value", "Text");

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _hREmpInfoRepository.EmpBankList(hrEmpInfo);
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            return View(nameof(EmpDevice), hrEmpInfo);
        }
        public IActionResult UploadCertificate1(IFormFile Certificate)
        {
            if (Certificate != null)
            {
                string filename = $"{_env.WebRootPath}\\EmpDocument\\Certificates\\{Certificate.FileName}";
                using (FileStream stream = System.IO.File.Create(filename))
                {
                    Certificate.CopyTo(stream);
                    stream.Flush();
                }
            }
            return View("EmpDevice");
        }
        //-------------------------End of Employee Device-----------------------------//






        //---------------------For Project-------------------------------//

        [HttpPost]
        //[IgnoreAntiforgeryToken]
        public ActionResult EmpProject(string HR_Emp_Projects)
        {
            try
            {

                _hREmpInfoRepository.EmpProjectDelete(HR_Emp_Projects);
                TempData["Message"] = "Data Update Successfully";
                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        public async Task<IActionResult> EmpProjectUpdate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hrEmpInfo = await _empInfoRepository.EmpInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            //ViewBag.DtJoin = DateTime.Parse(hrEmpInfo.DtJoin).ToString("dd-MMM-yy");
            ViewBag.Title = "Edit";
            ViewBag.Title1 = "Create";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }
            var examlist = _context.Cat_Variable.Where(x => x.VarType == "ExamType").ToList();
            List<SelectListItem> Examlist = examlist
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();


            ViewData["ExamList"] = new SelectList(Examlist, "Value", "Text");

            List<SelectListItem> Boardlist = new List<SelectListItem>();
            Boardlist.Add(new SelectListItem("Dhaka", "Dhaka"));
            Boardlist.Add(new SelectListItem("Chittagong", "Chittagong"));
            Boardlist.Add(new SelectListItem("Cumilla", "Cumilla"));
            Boardlist.Add(new SelectListItem("Barishal", "Barishal"));
            Boardlist.Add(new SelectListItem("Jessore", "Jessore"));
            Boardlist.Add(new SelectListItem("Shylhet", "Shylhet"));

            ViewData["Boardlist"] = new SelectList(Boardlist, "Value", "Text");


            var subjectlist = _context.Cat_Variable.Where(x => x.VarType == "Subject").ToList();
            List<SelectListItem> Subjectlist = subjectlist
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();
            ViewData["Subjectlist"] = new SelectList(Subjectlist, "Value", "Text");



            var ClientName = _context.Cat_Variable.Where(x => x.VarType == "ClientInfo").ToList();
            List<SelectListItem> clientinfo = ClientName
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();
            ViewData["ClientName"] = new SelectList(clientinfo, "Value", "Text");



            var institute = _context.Cat_Variable.Where(x => x.VarType == "Institute").ToList();
            List<SelectListItem> Institutelist = institute
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();
            ViewData["Institutelist"] = new SelectList(Institutelist, "Value", "Text");

            var comid = HttpContext.Session.GetString("comid");
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();


            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _hREmpInfoRepository.EmpBankList(hrEmpInfo);
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            return View(nameof(EmpProject), hrEmpInfo);
        }
        public IActionResult UploadCertificate2(IFormFile Certificate)
        {
            if (Certificate != null)
            {
                string filename = $"{_env.WebRootPath}\\EmpDocument\\Certificates\\{Certificate.FileName}";
                using (FileStream stream = System.IO.File.Create(filename))
                {
                    Certificate.CopyTo(stream);
                    stream.Flush();
                }
            }
            return View("EmpProject");
        }
        //-------------------------End of Employee Devices-----------------------------//






        // for download file
        public IActionResult DownloadEducationFile(string file)
        {
            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\EmpDocument\Certificates"}" + "\\" + file;

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return Ok(response);
        }
        //[Obsolete]
        //public async Task<IActionResult> DownloadEducationFile(string file)
        //{ 
        //    if (file == null)
        //        return Content("filename is not availble");

        //    var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\EmpDocument\Certificates"}" + "\\" + file;

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, GetContentType(path), Path.GetFileName(path));
        //}
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip", "application/zip" },
                {".rar" , "application/vnd.rar"}
            };
        }
        //-------------------------For Experience------------------------------//

        [HttpPost]
        public ActionResult EmpExperience(string HR_Emp_Experiences)
        {
            try
            {
                _hREmpInfoRepository.EmpExperienceDelete(HR_Emp_Experiences);
                TempData["Message"] = "Data Update Successfully";
                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        public async Task<IActionResult> EmpExperienceUpdate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hrEmpInfo = await _empInfoRepository.EmpInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            //ViewBag.DtJoin = DateTime.Parse(hrEmpInfo.DtJoin).ToString("dd-MMM-yy");
            ViewBag.Title = "Edit";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _hREmpInfoRepository.EmpBankList(hrEmpInfo);
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            return View(nameof(EmpExperience), hrEmpInfo);
        }
        //------------------------End of Experience----------------------------//
        #endregion
        #region Create Cat_Leave_Type
        public IActionResult CreateLeaveType()
        {
            ViewBag.Title = "Create";
            return View();
        }


        [HttpPost]
        public IActionResult CreateLeaveType(Cat_Leave_Type model)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (model.LTypeId > 0)
            {
                _context.Update(model);
            }
            else
            {
                model.ComId = comid;
                model.IsDelete = false;
                _context.Cat_Leave_Type.Add(model);
            }

            _context.SaveChanges();
            return RedirectToAction("CatLeaveTypeList");
            return View(model);

        }

        [HttpGet]
        public IActionResult CatLeaveTypeList() //Hossain Naim
        {
            var data = _leaveEntryRepository.GetLeaveAll();
            return View(data); //_leaveEntryRepository.LeaveTypeList
        }


        [HttpGet]
        public IActionResult EditLeaveType(int id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            var HR_LeaveType = _leaveEntryRepository.FindById(id);

            ViewBag.LTypeName = new SelectList(_context.Cat_Leave_Type
                .Where(v => v.LTypeName == "ProcessLeaveType")
                .OrderBy(v => v.ComId).ToList(), "VarName", "VarName", HR_LeaveType.LTypeName);
            if (HR_LeaveType == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            return View("CreateLeaveType", HR_LeaveType);

        }



        [HttpGet]
        public IActionResult DeleteLeaveType(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Check if the leave type exists in your repository or database
            var comid = HttpContext.Session.GetString("comid");
            var HR_LeaveType = _leaveEntryRepository.FindById(id);

            if (HR_LeaveType == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            return View("CreateLeaveType", HR_LeaveType);
        }


        [HttpPost]
        public IActionResult DeleteLeaveTypePost(int id)
        {
            string comid = HttpContext.Session.GetString("comid");


            try
            {
                var HR_LeaveType = _leaveEntryRepository.FindById(id);
                HR_LeaveType.IsDelete = true;
                _context.Update(HR_LeaveType);
                _context.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = HR_LeaveType.LTypeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }


            return RedirectToAction("CatLeaveTypeList"); ;

        }
        #endregion

        #region Leave Entry
        public IActionResult LeaveEntryList()
        {
            var comid = HttpContext.Session.GetString("comid");
            HR_Leave_Avail LeaveData = _leaveEntryRepository.LeaveAvail();
            ViewBag.LeaveBalance = _leaveEntryRepository.LeaveBalance();
            ViewBag.EmpId = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeList();

            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();


            if (approveData == null)
            {
                ViewBag.ApprovalSetting = 0;
            }
            //else
            //{
            //    ViewBag.ApprovalSetting = 0;
            //}

            return View();
        }


        public IActionResult LeaveEntryListForUBL()
        {
            var comid = HttpContext.Session.GetString("comid");
            HR_Leave_Avail LeaveData = _leaveEntryRepository.LeaveAvail();
            ViewBag.LeaveBalance = _leaveEntryRepository.LeaveBalance();
            ViewBag.EmpId = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeList();

            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();


            if (approveData != null)
            {
                ViewBag.ApprovalSetting = 0;
            }
            else
            {
                ViewBag.ApprovalSetting = 0;
            }

            return View();
        }
        [HttpPost]
        public IActionResult CreateLeaveEntryUBL(HR_Leave_Avail hR_Leave_Avail)
        {
            hR_Leave_Avail.IsApprove = (hR_Leave_Avail.Status == 1) ? true : false;
            ViewData["EmpId"] = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeList();

            hR_Leave_Avail.PcName = HttpContext.Session.GetString("pcname");
            hR_Leave_Avail.UserId = HttpContext.Session.GetString("userid");
            hR_Leave_Avail.ComId = HttpContext.Session.GetString("comid");
            hR_Leave_Avail.DtInput = DateTime.Today;

            HR_Leave_Balance LeaveBalance = _leaveEntryRepository.CreateLeaveBalance(hR_Leave_Avail);
            Cat_Leave_Type LeaveType = _leaveEntryRepository.CreateLeaveType(hR_Leave_Avail);
            float AvailCL = 0;
            float AvailSL = 0;
            float AvailEL = 0;
            float AvailML = 0;
            var Success = "";
            AvailCL = (float)(LeaveBalance.CL - LeaveBalance.ACL);
            AvailSL = (float)(LeaveBalance.SL - LeaveBalance.ASL);
            AvailEL = (float)(LeaveBalance.EL - LeaveBalance.AEL);
            AvailML = (float)(LeaveBalance.ML - LeaveBalance.AML);

            var allLeaveData = _context.HR_Leave_Avail
                .Where(x => x.ComId == hR_Leave_Avail.ComId
                         && x.EmpId == hR_Leave_Avail.EmpId
                         && (hR_Leave_Avail.DtFrom).Date >= x.DtFrom.Date
                         && (hR_Leave_Avail.DtFrom).Date <= x.DtTo.Date)
                .ToList();
            if (allLeaveData.Count > 0)
            {
                return Json(new { Success = 0, ex = "Leave already existed." });
            }
            var message = "Leave Balance Over.Please Correction Leave Day";
            Success = "Data Save Successfully";
            var gender = _context.HR_Emp_Info.Where(x => x.ComId == hR_Leave_Avail.ComId && x.EmpId == hR_Leave_Avail.EmpId).FirstOrDefault();
            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == hR_Leave_Avail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            try
            {
                try
                {
                    if (approveData == null)
                    {

                        if (LeaveType.LTypeNameShort == "CL" || LeaveType.LTypeNameShort == "CLH")
                        {
                            if (AvailCL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.ACL;
                                LeaveBalance.ACL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "EL" || LeaveType.LTypeNameShort == "ELH")
                        {
                            if (AvailEL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.AEL;
                                LeaveBalance.AEL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "SL" || LeaveType.LTypeNameShort == "SLH")
                        {
                            if (AvailSL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.ASL;
                                LeaveBalance.ASL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "ML")
                        {
                            if (gender.GenderId == 3)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.AML;
                                LeaveBalance.AML = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = "ML is not Applicable for Male. Please Select Correct Gender" });

                            }
                        }
                    }

                    _leaveEntryRepository.CreateLeaveEntryPost(hR_Leave_Avail);

                }
                catch (Exception ex)
                {
                    //throw ex;
                    return Json(new { Success = 0, ex = ex });

                }
                _leaveEntryRepository.CreateLeaveEntryPost2(hR_Leave_Avail);

                SqlParameter[] sqlParemeter = new SqlParameter[5];
                sqlParemeter[0] = new SqlParameter("@ComID", hR_Leave_Avail.ComId);
                sqlParemeter[1] = new SqlParameter("@EmpID", hR_Leave_Avail.EmpId);
                sqlParemeter[2] = new SqlParameter("@LeaveID", hR_Leave_Avail.LvId);
                sqlParemeter[3] = new SqlParameter("@dtFrom", hR_Leave_Avail.DtFrom);
                sqlParemeter[4] = new SqlParameter("@dtTo", hR_Leave_Avail.DtTo);

                string query = $"Exec HR_prcProcessLeave '{hR_Leave_Avail.ComId}', {hR_Leave_Avail.EmpId}, {hR_Leave_Avail.LvId}, '{hR_Leave_Avail.DtFrom}', '{hR_Leave_Avail.DtTo}'";
                Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                // hR_Leave_Avail = null;
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex });

                //throw ex;
            }

            return Json(new { Success = 1, ex = Success });

        }

        public JsonResult LoadEmployeeLeaveData(int? empId, DateTime? date)
        {
            DateTime asdf = DateTime.Now.Date;
            if (date is null)
            {
                var adate = DateTime.Now.Year;
                date = DateTime.Now;
            }
            else
            {
                //asdf = date;
            }

            var comid = HttpContext.Session.GetString("comid");


            var year = date.Value.Year;


            var CL = (from emp in _context.HR_Leave_Avail
                      where emp.IsApprove == true && emp.IsDelete == false &&  emp.ComId == comid && emp.LvType == "CL" && emp.EmpId == empId && emp.DtFrom.Year == year
                      select emp).Sum(x => x.TotalDay);

            var CLH = (from emp in _context.HR_Leave_Avail
                       where emp.IsApprove == true && emp.IsDelete == false && emp.ComId == comid && emp.LvType == "CLH" && emp.EmpId == empId && emp.DtFrom.Year == year
                       select emp).Sum(x => x.TotalDay);

            var TotalCHL = CL + CLH;

            var SL = (from emp in _context.HR_Leave_Avail
                      where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "SL" && emp.IsDelete == false && emp.EmpId == empId && emp.DtFrom.Year == year
                      select emp).Sum(x => x.TotalDay);

            var SLH = (from emp in _context.HR_Leave_Avail
                       where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "SLH" && emp.EmpId == empId && emp.IsDelete == false && emp.DtFrom.Year == year
                       select emp).Sum(x => x.TotalDay);

            var TotalSHL = SL + SLH;
            var EL = (from emp in _context.HR_Leave_Avail
                      where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "EL" && emp.EmpId == empId && emp.IsDelete == false && emp.DtFrom.Year == year
                      select emp).Sum(x => x.TotalDay);

            var ELH = (from emp in _context.HR_Leave_Avail
                       where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "ELH" && emp.EmpId == empId && emp.IsDelete == false && emp.DtFrom.Year == year
                       select emp).Sum(x => x.TotalDay);

            var TotalEHL = EL + ELH;

            var ML = (from emp in _context.HR_Leave_Avail
                      where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "ML" && emp.EmpId == empId && emp.IsDelete == false && emp.DtFrom.Year == year
                      select emp).Sum(x => x.TotalDay);

            var MLH = (from emp in _context.HR_Leave_Avail
                       where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "MLH" && emp.EmpId == empId && emp.IsDelete == false && emp.DtFrom.Year == year
                       select emp).Sum(x => x.TotalDay);

            var TotaMHL = ML + MLH;

            UpdateLeaveBalance(TotalCHL, TotalSHL, TotalEHL, TotaMHL, empId, date);
            var LeaveBalance = _leaveEntryRepository.LeaveEntry(empId, date)



                .Select(d => new
                {
                    LeaveId = d.LvBalId,
                    Code = d.HR_Emp_Info.EmpCode,
                    EmployeeName = d.HR_Emp_Info.EmpName,
                    Year = year,
                    CLTotal = d.CL,
                    CLEnjoyed = d.ACL,//TotaCHL,d.ACL + CLH,
                    CLBalance = d.CL - d.ACL,
                    SLTotal = d.SL,
                    SLEnjoyed = d.ASL,//TotaSHL,d.ASL + SLH,
                    SLBalance = d.SL - d.ASL,
                    ELTotal = d.EL,
                    ELEnjoyed = d.AEL,
                    ELBalance = d.EL - d.AEL,
                    MLTotal = d.ML,
                    MLEnjoyed = d.AML,
                    MLBalance = d.ML - d.AML

                });

            // ViewBag.Leaveavoail = LeaveBalance;
            return Json(LeaveBalance);
        }

        public void UpdateLeaveBalance(float? CL, float? SL, float? EL, float? ML, int? empId, DateTime? date)
        {

            string year = date.Value.Year.ToString();
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = _context.HR_Leave_Balance
                .Include(lb => lb.HR_Emp_Info)
                .Where(l => l.ComId == comid && l.EmpId == empId && l.DtOpeningBalance.ToString() == year.ToString())
                .FirstOrDefault();
            if (data != null)
            {
                data.ACL = CL;
                data.ASL = SL;
                data.AML = ML;
                data.AEL = EL;
                _context.Update(data);
                _context.SaveChanges();
            }

        }

        public JsonResult LoadLeaveEntryPartial(int empId, DateTime? date)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                char[] spearator = { '!' };
                var data = _leaveEntryRepository.LoadLeaveEntryPartial(empId, date);
                foreach (var a in data)
                {
                    if (a.FileName == null || a.FileName == "")
                    {
                        a.FileName = "";
                    }
                    else
                    {
                        String[] strlist = a.FileName.Split(spearator);
                        strlist = strlist.SkipLast(1).ToArray();
                        a.FileName = strlist[0];
                    }
                    var temp = _context.HR_Leave_Avail.Where(x => x.LvId == a.LvId && x.IsDelete == false && x.DtFrom.Year >= date.Value.Year).FirstOrDefault();
                    var data1 = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.IsDelete == false && x.ApprovalType == 1175).FirstOrDefault();

                    if (data1 != null)
                    {
                        if (data1.IsApprove == true && data1.IsFirstLeaveApprove == true)
                        {

                            if (temp != null)
                            {
                                if (temp.Status == 1)
                                {
                                    if (temp.IsApprove == true)
                                    {
                                        a.Status = "Approved";
                                    }
                                    else
                                    {
                                        a.Status = "First Approved";
                                    }
                                }
                                else
                                {
                                    if (temp.Status >= 2)
                                    {
                                        a.Status = "Disapproved";
                                    }
                                    else
                                    {
                                        a.Status = "Pending";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (data1.IsApprove == true && data1.IsFirstLeaveApprove == false)
                            {
                                if (temp.Status == 1)
                                {
                                    if (temp.IsApprove == true)
                                    {
                                        a.Status = "Approved";
                                    }
                                    else
                                    {
                                        a.Status = "First Approved";
                                    }
                                }
                                else
                                {
                                    if (temp.Status >= 2)
                                    {
                                        a.Status = "Disapproved";
                                    }
                                    else
                                    {
                                        a.Status = "Pending";
                                    }
                                }
                            }
                        }
                    }


                }
                return Json(data);
            }
            catch (Exception)
            {
                return Json(new { Success = 0, InvoiceId = 0, ex = "Unable to Load the Data" });

            }
        }

        public JsonResult GetToDate(DateTime? DtFrom, double TotalDay)
        {
            double day = 1;
            if (TotalDay < 1)
            {
                day = 1;
            }
            else
            {
                day = TotalDay;
            }

            DateTime DtTo = DtFrom.Value.AddDays(day).AddDays(-1);
            string dtto = DtTo.ToString("dd-MMM-yy");
            return Json(dtto);
        }
        public JsonResult ChangeLeaveOption(int id, float TotalDay) // Hossain Naim
        {
            var cm = HttpContext.Session.GetString("Comid");
            var check = _context.Cat_Leave_Type.Where(y => y.LTypeId == id).Select(y => y.IsAllowHalfLeave).FirstOrDefault();


            if (check != null && check == true && TotalDay < 1)
            {

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public JsonResult LeaveBalanceCheck(int empid)
        {
            var balance = _leaveEntryRepository.GetBalance(empid);
            var leaveType = _leaveEntryRepository.GetLeaveType();
            return Json(new { balance, leaveType });
        }

        public IActionResult CreateLeaveEntry(int empid)
        {
            HR_Leave_Avail LeaveData = _leaveEntryRepository.LeaveAvail();
            ViewBag.LeaveBalance = _leaveEntryRepository.LeaveBalance();
            ViewData["EmpId"] = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeList();
            return View();

        }
        [HttpPost]
        public IActionResult CreateLeaveEntry(HR_Leave_Avail hR_Leave_Avail)
        {
            ViewData["EmpId"] = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeList();

            hR_Leave_Avail.PcName = HttpContext.Session.GetString("pcname");
            hR_Leave_Avail.UserId = HttpContext.Session.GetString("userid");
            hR_Leave_Avail.ComId = HttpContext.Session.GetString("comid");
            hR_Leave_Avail.DtInput = DateTime.Today;

            HR_Leave_Balance LeaveBalance = _leaveEntryRepository.CreateLeaveBalance(hR_Leave_Avail);
            Cat_Leave_Type LeaveType = _leaveEntryRepository.CreateLeaveType(hR_Leave_Avail);
            float AvailCL = 0;
            float AvailSL = 0;
            float AvailEL = 0;
            float AvailML = 0;
            var Success = "";
            AvailCL = (float)(LeaveBalance.CL - LeaveBalance.ACL);
            AvailSL = (float)(LeaveBalance.SL - LeaveBalance.ASL);
            AvailEL = (float)(LeaveBalance.EL - LeaveBalance.AEL);
            AvailML = (float)(LeaveBalance.ML - LeaveBalance.AML);

            var allLeaveData = _context.HR_Leave_Avail
                .Where(x => x.ComId == hR_Leave_Avail.ComId && x.IsDelete == false
                         && x.EmpId == hR_Leave_Avail.EmpId
                         && (hR_Leave_Avail.DtFrom).Date >= x.DtFrom.Date
                         && (hR_Leave_Avail.DtFrom).Date <= x.DtTo.Date)
                .ToList();
            if (allLeaveData.Count > 0)
            {
                return Json(new { Success = 0, ex = "Leave already existed." });
            }
            var message = "Leave Balance Over.Please Correction Leave Day";
            Success = "Data Save Successfully";
            var gender = _context.HR_Emp_Info.Where(x => x.ComId == hR_Leave_Avail.ComId && x.EmpId == hR_Leave_Avail.EmpId).FirstOrDefault();
            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == hR_Leave_Avail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            try
            {
                try
                {
                    if (approveData == null)
                    {

                        if (LeaveType.LTypeNameShort == "CL" || LeaveType.LTypeNameShort == "CLH")
                        {
                            if (AvailCL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.ACL;
                                LeaveBalance.ACL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "EL" || LeaveType.LTypeNameShort == "ELH")
                        {
                            if (AvailEL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.AEL;
                                LeaveBalance.AEL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "SL" || LeaveType.LTypeNameShort == "SLH")
                        {
                            if (AvailSL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.ASL;
                                LeaveBalance.ASL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "ML")
                        {
                            if (gender.GenderId == 3)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.AML;
                                LeaveBalance.AML = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = "ML is not Applicable for Male. Please Select Correct Gender" });

                            }
                        }
                    }

                    _leaveEntryRepository.CreateLeaveEntryPost(hR_Leave_Avail);

                }
                catch (Exception ex)
                {
                    //throw ex;
                    return Json(new { Success = 0, ex = ex });

                }
                _leaveEntryRepository.CreateLeaveEntryPost2(hR_Leave_Avail);

                SqlParameter[] sqlParemeter = new SqlParameter[5];
                sqlParemeter[0] = new SqlParameter("@ComID", hR_Leave_Avail.ComId);
                sqlParemeter[1] = new SqlParameter("@EmpID", hR_Leave_Avail.EmpId);
                sqlParemeter[2] = new SqlParameter("@LeaveID", hR_Leave_Avail.LvId);
                sqlParemeter[3] = new SqlParameter("@dtFrom", hR_Leave_Avail.DtFrom);
                sqlParemeter[4] = new SqlParameter("@dtTo", hR_Leave_Avail.DtTo);

                string query = $"Exec HR_prcProcessLeave '{hR_Leave_Avail.ComId}', {hR_Leave_Avail.EmpId}, {hR_Leave_Avail.LvId}, '{hR_Leave_Avail.DtFrom}', '{hR_Leave_Avail.DtTo}'";
                Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                // hR_Leave_Avail = null;
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex });

                //throw ex;
            }

            return Json(new { Success = 1, ex = Success });
        }

        public JsonResult LoadGridData(int lvid)
        {
            HR_Leave_Avail LeaveData = (HR_Leave_Avail)_leaveEntryRepository.LoadGridLeaveData(lvid);


            var data = _leaveEntryRepository.GridData(lvid);


            //var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveData.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            //var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveData.ComId && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            //if (approveData != null)
            //{
            //    ViewBag.ApprovalSetting = 1;
            //}
            //else
            //{
            //    ViewBag.ApprovalSetting = 0;
            //}

            return Json(data);

        }

        public JsonResult UpdateLeaveEntry(HR_Leave_Avail LeaveAvail)
        {
            LeaveAvail.IsDelete = false;
            LeaveAvail.UserId = HttpContext.Session.GetString("userid");
            LeaveAvail.ComId = HttpContext.Session.GetString("comid");
            LeaveAvail.DtInput = DateTime.Today;

            var message = "Leave Balance Over.Please Correction Leave Day";
            HR_Leave_Balance lb = (HR_Leave_Balance)_leaveEntryRepository.UpdateLB(LeaveAvail);
            HR_Leave_Balance previouslb = (HR_Leave_Balance)_leaveEntryRepository.PreviousLB(LeaveAvail);

            Cat_Leave_Type LeaveType = _leaveEntryRepository.CreateLeaveType(LeaveAvail);
            LeaveAvail.LvType = LeaveType.LTypeNameShort;
            HR_Leave_Avail PreviousLeave = (HR_Leave_Avail)_leaveEntryRepository.PreviousLA(LeaveAvail);
            float ACL = 0;
            float AEL = 0;
            float ASL = 0;
            float AML = 0;
            LeaveAvail.PreviousDay = PreviousLeave.TotalDay;
            var Success = "";

            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveAvail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveAvail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (approveData != null)
            {
                Success = "Data Update Successully";

                if (LeaveType.LTypeNameShort == "CL") //|| LeaveType.LTypeNameShort == "CLH")
                {

                    if (LeaveAvail.TotalDay <= lb.CL)
                    {

                        _leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "CLH")
                {
                    if (LeaveAvail.TotalDay <= lb.CL)
                    {

                        _leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }


                else if (LeaveType.LTypeNameShort == "SL")
                {
                    if (LeaveAvail.TotalDay <= lb.SL)
                    {

                        _leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }



                else if (LeaveType.LTypeNameShort == "SLH")
                {
                    if (LeaveAvail.TotalDay <= lb.SL)
                    {

                        _leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);
                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "EL")
                {
                    if (LeaveAvail.TotalDay <= lb.EL)
                    {

                        _leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        ////return Json(message);
                    }
                }



                else if (LeaveType.LTypeNameShort == "ML")
                {
                    if (LeaveAvail.TotalDay <= lb.ML)
                    {

                        _leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        ////return Json(message);
                    }
                }
                return Json(new { Success = 2, ex = message });

            }
            else
            {

                if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CL")// || LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);

                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CLH")// || LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);

                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SL")// || LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SLH")// || LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "EL" || LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL - LeaveAvail.PreviousDay);
                    lb.AEL = AEL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML - LeaveAvail.PreviousDay);
                    lb.AML = AML;
                }

                //db.Update(lb);
                //db.SaveChanges();



                if (LeaveType.LTypeNameShort == "CL") //|| LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL + LeaveAvail.TotalDay);
                    lb.ACL = ACL;

                    if (ACL > lb.CL)
                    {
                        //db.Update(previouslb);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "CLH") //|| LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL + LeaveAvail.TotalDay);
                    lb.ACL = ACL;

                    if (ACL > lb.CL)
                    {
                        //db.Update(previouslb);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                        //return Json(message);
                    }
                }
                else if (LeaveType.LTypeNameShort == "SL") //|| LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL + LeaveAvail.TotalDay);
                    lb.ASL = ASL;
                    if (ASL > lb.SL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "SLH") //|| LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL + LeaveAvail.TotalDay);
                    lb.ASL = ASL;
                    if (ASL > lb.SL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "EL") //|| LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL + LeaveAvail.TotalDay);
                    lb.AEL = AEL;
                    if (AEL > lb.EL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }

                else if (LeaveType.LTypeNameShort == "ELH") //|| LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL + LeaveAvail.TotalDay);
                    lb.AEL = AEL;
                    if (AEL > lb.EL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML + LeaveAvail.TotalDay);
                    lb.AML = AML;
                    if (AML > lb.ML)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                _leaveEntryRepository.UpdateLAB(LeaveAvail);
                _leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);

                SqlParameter[] sqlParemeter = new SqlParameter[5];
                sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

            }
            Success = "Data Update Successully";
            return Json(new { Success = 2, ex = Success });

        }

        public JsonResult UpdateLeaveEntryUBL(HR_Leave_Avail LeaveAvail)
        {
            LeaveAvail.UserId = HttpContext.Session.GetString("userid");
            LeaveAvail.ComId = HttpContext.Session.GetString("comid");
            LeaveAvail.DtInput = DateTime.Today;

            var message = "Leave Balance Over.Please Correction Leave Day";
            HR_Leave_Balance lb = (HR_Leave_Balance)_leaveEntryRepository.UpdateLB(LeaveAvail);
            HR_Leave_Balance previouslb = (HR_Leave_Balance)_leaveEntryRepository.PreviousLB(LeaveAvail);

            Cat_Leave_Type LeaveType = _leaveEntryRepository.CreateLeaveType(LeaveAvail);
            LeaveAvail.LvType = LeaveType.LTypeNameShort;
            HR_Leave_Avail PreviousLeave = (HR_Leave_Avail)_leaveEntryRepository.PreviousLA(LeaveAvail);
            float ACL = 0;
            float AEL = 0;
            float ASL = 0;
            float AML = 0;
            LeaveAvail.PreviousDay = PreviousLeave.TotalDay;
            var Success = "";

            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveAvail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveAvail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (approveData != null)
            {
                Success = "Data Update Successully";

                if (LeaveType.LTypeNameShort == "CL") //|| LeaveType.LTypeNameShort == "CLH")
                {

                    if (LeaveAvail.TotalDay <= lb.CL)
                    {

                        UpdateLeaveAvailUBL(LeaveAvail);

                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "CLH")
                {
                    if (LeaveAvail.TotalDay <= lb.CL)
                    {

                        UpdateLeaveAvailUBL(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }


                else if (LeaveType.LTypeNameShort == "SL")
                {
                    if (LeaveAvail.TotalDay <= lb.SL)
                    {

                        UpdateLeaveAvailUBL(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }



                else if (LeaveType.LTypeNameShort == "SLH")
                {
                    if (LeaveAvail.TotalDay <= lb.SL)
                    {

                        UpdateLeaveAvailUBL(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);
                        return Json(new { Success = 2, ex = Success });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "EL")
                {
                    if (LeaveAvail.TotalDay <= lb.EL)
                    {

                        UpdateLeaveAvailUBL(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        ////return Json(message);
                    }
                }



                else if (LeaveType.LTypeNameShort == "ML")
                {
                    if (LeaveAvail.TotalDay <= lb.ML)
                    {

                        UpdateLeaveAvailUBL(LeaveAvail);
                        //db.SaveChanges();
                        SqlParameter[] sqlParemeter = new SqlParameter[5];
                        sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                        sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                        sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                        sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                        sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                        string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                        Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

                        return Json(new { Success = 2, ex = Success });

                        ////return Json(message);
                    }
                }
                return Json(new { Success = 2, ex = message });

            }
            else
            {

                if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CL")// || LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);

                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CLH")// || LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);

                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SL")// || LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SLH")// || LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "EL" || LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL - LeaveAvail.PreviousDay);
                    lb.AEL = AEL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML - LeaveAvail.PreviousDay);
                    lb.AML = AML;
                }

                //db.Update(lb);
                //db.SaveChanges();



                if (LeaveType.LTypeNameShort == "CL") //|| LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL + LeaveAvail.TotalDay);
                    lb.ACL = ACL;

                    if (ACL > lb.CL)
                    {
                        //db.Update(previouslb);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "CLH") //|| LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL + LeaveAvail.TotalDay);
                    lb.ACL = ACL;

                    if (ACL > lb.CL)
                    {
                        //db.Update(previouslb);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                        //return Json(message);
                    }
                }
                else if (LeaveType.LTypeNameShort == "SL") //|| LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL + LeaveAvail.TotalDay);
                    lb.ASL = ASL;
                    if (ASL > lb.SL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "SLH") //|| LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL + LeaveAvail.TotalDay);
                    lb.ASL = ASL;
                    if (ASL > lb.SL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "EL") //|| LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL + LeaveAvail.TotalDay);
                    lb.AEL = AEL;
                    if (AEL > lb.EL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }

                else if (LeaveType.LTypeNameShort == "ELH") //|| LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL + LeaveAvail.TotalDay);
                    lb.AEL = AEL;
                    if (AEL > lb.EL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML + LeaveAvail.TotalDay);
                    lb.AML = AML;
                    if (AML > lb.ML)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                _leaveEntryRepository.UpdateLAB(LeaveAvail);
                UpdateLeaveAvailUBL(LeaveAvail);

                SqlParameter[] sqlParemeter = new SqlParameter[5];
                sqlParemeter[0] = new SqlParameter("@ComID", LeaveAvail.ComId);
                sqlParemeter[1] = new SqlParameter("@EmpID", LeaveAvail.EmpId);
                sqlParemeter[2] = new SqlParameter("@LeaveID", LeaveAvail.LvId);
                sqlParemeter[3] = new SqlParameter("@dtFrom", LeaveAvail.DtFrom);
                sqlParemeter[4] = new SqlParameter("@dtTo", LeaveAvail.DtTo);

                string query = $"Exec HR_prcProcessLeave '{LeaveAvail.ComId}', {LeaveAvail.EmpId}, {LeaveAvail.LvId}, '{LeaveAvail.DtFrom}', '{LeaveAvail.DtTo}'";
                Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeave", sqlParemeter);

            }
            Success = "Data Update Successully";
            return Json(new { Success = 2, ex = Success });

        }

        public void UpdateLeaveAvailUBL(HR_Leave_Avail LeaveAvail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            LeaveAvail.DateUpdated = DateTime.Now;
            //var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            //var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            LeaveAvail.IsApprove = (LeaveAvail.Status == 1) ? true : false;
            _context.Update(LeaveAvail);
            _context.SaveChanges();
        }


        [HttpPost, ActionName("DeleteLeaveEntry")]
        public IActionResult DeleteLeaveEntryConfirmed(HR_Leave_Avail LeaveAvail)
        {
            var Success = "";
            try
            {
                _leaveEntryRepository.DeleteLeaveEntry(LeaveAvail);
                Success = "Data Delete Successully";
                return Json(new { Success = 1, ex = Success });

            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        #endregion 

        #region Process Lock
        public IActionResult ProcessLockList()
        {
            var data = _processLockRepository.GetProcessLockData();
            return View(data);
        }

        // GET: ProcessLock/Create
        public IActionResult CreateProcessLock()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Create";

            ViewBag.LockType = _processLockRepository.LockTypeList();


            var fiscalYear = _processLockRepository.FiscalYear();
            if (fiscalYear != null)
            {
                var fiscalMonth = _processLockRepository.FiscalMonth();

                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
                ViewBag.FiscalMonthId = _processLockRepository.FiscalMonthID();

            }
            else
            {
                ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
                ViewBag.FiscalMonthId = _processLockRepository.FiscalMonthID2();
            }

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ProcessId,ComId,LockType,DtDate,IsLock,PcName,UserId,DtTran")] HR_ProcessLock hR_ProcessLock)
        public async Task<IActionResult> CreateProcessLock(HR_ProcessLock HR_ProcessLock)
        {

            string comid = HttpContext.Session.GetString("comid");

            if (ModelState.IsValid)
            {

                HR_ProcessLock.ComId = HttpContext.Session.GetString("comid");

                if (HR_ProcessLock.ProcessId > 0)
                {
                    HR_ProcessLock.DateUpdated = DateTime.Now;
                    HR_ProcessLock.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _processLockRepository.Update(HR_ProcessLock);

                }
                else
                {
                    HR_ProcessLock.UserId = HttpContext.Session.GetString("userid");
                    HR_ProcessLock.DateAdded = DateTime.Now;
                    _processLockRepository.Add(HR_ProcessLock);

                    string connectionString = _configuration.GetConnectionString("DefaultConnection");
                    string procedureName = "HR_prcGetMessage"; // Replace with your actual stored procedure name
                    string message = "";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(procedureName, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@ComId", comid); // Replace with your actual input parameter values
                            command.Parameters.AddWithValue("@empId", "");
                            command.Parameters.AddWithValue("@Type", HR_ProcessLock.LockType);
                            command.Parameters.Add("@Message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output; // Add the output parameter

                            connection.Open();
                            command.ExecuteNonQuery();
                            message += command.Parameters["@Message"].Value.ToString(); // Retrieve the returned string value
                            connection.Close();

                        }
                    }
                    HR_Notification hR_Notification = new HR_Notification();

                    hR_Notification.Body = message;
                    hR_Notification.Title = "ERP Notification";
                    hR_Notification.execTime = DateTime.Now;
                    hR_Notification.ComId = comid;
                    _context.HR_Notification.Add(hR_Notification);
                    _context.SaveChanges();

                    var ntfid = _context.HR_Notification.Where(x => x.execTime == hR_Notification.execTime).ToList();
                    int id = ntfid.FirstOrDefault().NtfId;
                    var result = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.Token != null).ToList();
                    foreach (var data in result)
                    {
                        HR_Notify hR_Notify = new HR_Notify();
                        hR_Notify.EmpId = data.EmpId;
                        hR_Notify.ComId = comid;
                        hR_Notify.IsMobileApp = 0;
                        hR_Notify.MobileAppText = message;
                        hR_Notify.NtfId = id;
                        _context.HR_Notify.Add(hR_Notify);
                        _context.SaveChanges();

                        if (data.Token != null)
                        {
                            await SendNotificationAsync(data.Token, "ERP Notification", message);
                            hR_Notify.IsMobileApp = 1;
                            _context.SaveChanges();
                        }

                    }
                }
                return RedirectToAction(nameof(ProcessLockList));
            }




            ViewBag.LockType = _processLockRepository.LockTypeList();

            return View(HR_ProcessLock);
        }

        public async Task SendNotificationAsync(string tkn, string title, string body)
        {
            // Initialize the Firebase Admin SDK
            var path = _env.ContentRootPath;
            path = path + "\\auth.json";
            FirebaseApp app = null;
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "myApp");
            }
            catch (Exception ex)
            {
                app = FirebaseApp.GetInstance("myApp");
            }

            var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);

            var token = tkn;
            Message message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                 {
                     { "AdditionalData1", "This is Additional data" },
                     { "AdditionalData2", "data 2" },
                     //{ "image", "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80"},
                 },
                Token = token
            };
            var result = await fcm.SendAsync(message);
        }

        public IActionResult EditProcessLock(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            var HR_ProcessLock = _processLockRepository.FindById(id);

            ViewBag.LockType = new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "ProcessLock")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName", HR_ProcessLock.LockType);
            if (HR_ProcessLock == null)
            {
                return NotFound();
            }

            ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            ViewBag.FiscalMonthId = _processLockRepository.FiscalMonthID();

            return View("CreateProcessLock", HR_ProcessLock);

        }

        public IActionResult DeleteProcessLock(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var HR_ProcessLock = _processLockRepository.FindById(id);
            if (HR_ProcessLock == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";


            //ViewBag.LockType = _processLockRepository.LockTypeList();
            ViewBag.LockType = new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "ProcessLock")
                .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName", HR_ProcessLock.LockType);

            ViewBag.FiscalYearId = _processLockRepository.FiscalYearID();
            ViewBag.FiscalMonthId = _processLockRepository.FiscalMonthID();

            return View("CreateProcessLock", HR_ProcessLock);

        }

        // POST: ProcessLock/Delete/5
        [HttpPost, ActionName("DeleteProcessLock")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteProcessConfirmed(int id)
        {

            try
            {
                var HR_ProcessLock = _processLockRepository.FindById(id);
                _processLockRepository.Delete(HR_ProcessLock);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, RelId = HR_ProcessLock.ProcessId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult GetFiscalMonth(int FiscalYearId)
        {
            string comid = HttpContext.Session.GetString("comid");
            var data = _processLockRepository.GetFiscalMonth(FiscalYearId);
            return Json(new { Month = data });
        }
        #endregion

        #region OT And Night Shift
        public IActionResult OTAndNightShiftList()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.ProssType = _oTAndNightShiftRepository.ProssType();
            ViewBag.OTFCList = new List<OTFC>();
            return View();
        }


        [HttpPost]
        public IActionResult CreateOTAndNightShift(List<HR_OT_FC> hR_OT_FCs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _oTAndNightShiftRepository.ExistingData(hR_OT_FCs);
                    TempData["Message"] = "Total OT & OT Night Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
        }

        public IActionResult Search(string prossType)
        {

            var OTFCList = _oTAndNightShiftRepository.SearchOTAndNight(prossType);
            return Json(new { result = OTFCList, Success = "1" });
        }

        #endregion

        #region Production
        public ActionResult ProductionList()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.EmpId = _empInfoRepository.GetEmpInfoAllList();
            //ViewBag.Band = new SelectList(db.Cat_Variable.Where(x => x.ComId == comid && x.VarType=="Band").ToList(), "varID", "varName");
            ViewBag.Sections = _sectionRepository.GetSectionList();
            ViewBag.Designation = _designationRepository.GetDesignationList();
            ViewBag.Style = _styleRepository.GetStyleList();
            ViewBag.Color = new SelectList(_context.Cat_Colors.ToList(), "ColorId", "ColorName");
            ViewBag.Size = new SelectList(_context.Cat_Sizes.ToList(), "SizeId", "SizeName");

            return View("MyView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductionList(string DtFrom, string DtTo, string criteria, string value)
        {
            try
            {
                var listOfProd = _productionRepository.GetProduction(DtFrom, DtTo, criteria, value);
                TempData["Message"] = "Data Load Successfully";
                return Json(new { Success = 1, Result = listOfProd, message = TempData["Message"].ToString() });

            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduction(string GridDataList)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var pcName = HttpContext.Session.GetString("pcname");
                var userid = HttpContext.Session.GetString("userid");
                if (GridDataList != null)
                {
                    _productionRepository.CreateProduction(GridDataList);
                    TempData["Message"] = "Production Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Something wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something wrong,Please check data";
                return Json(new { Success = 3, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduction(string GridDataLists)
        {
            var comid = HttpContext.Session.GetString("comid");
            var pcName = HttpContext.Session.GetString("pcname");
            var userid = HttpContext.Session.GetString("userid");
            if (GridDataLists != null)
            {
                var jobject = new JObject();
                var d = JObject.Parse(GridDataLists);
                string objct = d["GridDataList"].ToString();
                var model = JsonConvert.DeserializeObject<List<ProdGrid>>(objct);

                _productionRepository.UpdateProduction(GridDataLists);

                return Json(new { Success = 1, message = "Data Updated Successfully" });
            }

            return null;
        }
        public JsonResult StyleListFromMVC()
        {
            var JObject = new JObject();
            var GridDataList = _styleRepository.GetStyleList();
            var output = JsonConvert.SerializeObject(GridDataList);
            return Json(output);
        }
        #endregion

        #region Suppliment
        public IActionResult SupplimentList(DateTime? dtInput, int? sectId)
        {
            string comid = HttpContext.Session.GetString("comid");

            ViewBag.SectId = _sectionRepository.GetSectionList();
            ViewBag.Suppliments = null;

            if (dtInput == null && sectId == 0)
            {
                var data = _supplimentRepository.HREmpSuppliment1(dtInput, sectId);
                ViewBag.Suppliments = data;
            }
            if (dtInput == null && sectId > 0)
            {
                var data = _supplimentRepository.HREmpSuppliment2(dtInput, sectId);
                ViewBag.Suppliments = data;
            }
            else if (dtInput != null && sectId == 0)
            {
                var data = _supplimentRepository.HREmpSuppliment3(dtInput, sectId);
                ViewBag.Suppliments = data;
            }
            else if (dtInput != null && sectId > 0)
            {
                var data = _supplimentRepository.HREmpSuppliment4(dtInput, sectId);
                ViewBag.Suppliments = data;
            }


            return View();
        }

        [HttpPost]
        public IActionResult CreateSuppliment(List<HR_Emp_Suppliment> suppliments)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
               .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    _supplimentRepository.CreateSuppliment(suppliments);
                    TempData["Message"] = "Suppliment Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
        }

        #endregion

        #region Total FC
        public IActionResult TotalFCList()
        {
            string comid = HttpContext.Session.GetString("comid");

            var prossType = _totalFCRepository.TotalFCList();
            ViewBag.ProssType = new SelectList(prossType, "ProssType", "ProssType");
            ViewBag.OTFCList = new List<OTFC>();
            return View();
        }


        [HttpPost]
        public IActionResult CreateTotalFC(List<HR_OT_FC> hR_OT_FCs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _totalFCRepository.CreateTotalFC(hR_OT_FCs);
                    TempData["Message"] = "Total FC Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
        }


        public IActionResult SearchTotalFC(string prossType)
        {

            var OTFCList = _totalFCRepository.SearchTotalFC(prossType);

            //ViewBag.OTFCList = OTFCList;
            return Json(new { result = OTFCList, Success = "1" });
        }
        #endregion

        #region Total Night
        public IActionResult TotalNightList()
        {
            var prossType = _totalNightRepository.TotalNightList();
            ViewBag.ProssType = new SelectList(prossType, "ProssType", "ProssType");
            ViewBag.OTFCList = new List<OTFC>();
            return View();
        }


        [HttpPost]
        public IActionResult CreateTotalNight(List<HR_OT_FC> hR_OT_FCs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _totalNightRepository.CreateTotalNight(hR_OT_FCs);
                    TempData["Message"] = "Total Night Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
        }


        public IActionResult SearchTotalNight(string prossType)
        {

            var OTFCList = _totalNightRepository.SearchTotalNight(prossType);

            //ViewBag.OTFCList = OTFCList;
            return Json(new { result = OTFCList, Success = "1" });
        }
        #endregion

        #region Employee List
        public IActionResult EmployeeList()
        {
            try
            {
                int offset = 0, fetch = 10;
                string criteria = "All";
                string EmpCode = null;
                string EmpName = null;
                string dateString = "1-Jan-1950";
                DateTime to = DateTime.Parse(dateString);
                DateTime from = DateTime.Parse(dateString);
                DateTime dateValue1;
                DateTime dateValue2;
                if (DateTime.TryParseExact(dateString, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue1))
                {
                    to = DateTime.ParseExact(dateValue1.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }
                if (DateTime.TryParseExact(dateString, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue2))
                {
                    from = DateTime.ParseExact(dateValue2.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }


                //var data = _hRRepository.EmpListIndex(criteria, offset, fetch, EmpCode, EmpName, to, from).ToList();
                List<string> myList = new List<string> { "This Month", "This Week", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
                IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
                List<SelectListItem> mySelectListAsList = mySelectList.ToList();
                ViewBag.period = mySelectListAsList;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw ex;
            }

        }

        public IActionResult EmployeeListModern()
        {
            try
            {
                List<string> myList = new List<string> { "This Month", "This Week", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
                IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
                List<SelectListItem> mySelectListAsList = mySelectList.ToList();
                ViewBag.period = mySelectListAsList;
                return View("ModernReportView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw ex;
            }
        }
        [AllowAnonymous]
        public JsonResult GetEmployees(int pageIndex, int pageSize, string from, string to, string criteria, string searchquery = "")
        {
            try
            {
                if (pageIndex == 0)
                    pageIndex = 1;
                int Offset = (pageIndex - 1) * pageSize;
                int fetch = Offset + pageSize;
                string dateString = "1950-01-01 00:00:00.0000000";
                DateTime startDate = DateTime.Parse(dateString);
                DateTime endDate = DateTime.Parse(dateString);
                DateTime dateValue1;
                DateTime dateValue2;
                if (DateTime.TryParseExact(from, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue1))
                {
                    startDate = DateTime.ParseExact(dateValue1.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }
                if (DateTime.TryParseExact(to, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue2))
                {
                    endDate = DateTime.ParseExact(dateValue2.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }
                if (searchquery == null)
                {
                    searchquery = "{\"EmpCode\":\"\",\"EmpName\":\"\",\"EmpNameB\":\"\",\"EmpSpouse\":\"\",\"EmpSpouseB\":\"\",\"EmpFather\":\"\",\"EmpFatherB\":\"\",\"EmpMother\":\"\",\"EmpMotherB\":\"\",\"BankAccNo\":\"\",\"ReligionName\":\"\",\"BloodName\":\"\",\"UnitName\":\"\",\"LineName\":\"\",\"FloorName\":\"\",\"CategoryName\":\"\",\"DeptName\":\"\",\"ShiftName\":\"\",\"Skill\":\"\",\"EmpCurrCityVill\":\"\",\"PerVillName\":\"\",\"Educations\":\"\",\"Experiences\":\"\",\"DesigName\":\"\",\"SectName\":\"\",\"SubSectName\":\"\",\"DtBirth\":\"\",\"DtJoin\":\"\",\"DtIncrement\":\"\",\"DtConfirm\":\"\",\"DtPf\":\"\",\"EmpTypeName\":\"\",\"GenderName\":\"\",\"NID\":\"\",\"FingerId\":\"\",\"EmpPhone1\":\"\",\"EmpPhone2\":\"\",\"CurrDistName\":\"\",\"PerDistName\":\"\",\"EmpEmail\":\"\",\"EmpRemarks\":\"\",\"GradeName\":\"\",\"EmpNomineeName1\":\"\",\"EmpNomineeMobile1\":\"\",\"EmpNomineeNID1\":\"\",\"EmpNomineeRelation1\":\"\",\"EmpNomineeAddress1\":\"\",\"EmpNomineeName2\":\"\",\"BirthCertificate\":\"\",\"Salary\":\"\",\"BusStopName\":\"\",\"EmpNomineeMobile2\":\"\",\"EmpNomineeNID2\":\"\",\"EmpNomineeRelation2\":\"\",\"EmpNomineeAddress2\":\"\",\"EmpId\":\"\",\"pageIndex\":1,\"pageSize\":10}";
                }
                string SearchColumns = "";
                string SearchKeywords = "";
                if (searchquery?.Length > 1)
                {

                    JObject jsonObject = JObject.Parse(searchquery);


                    foreach (JProperty property in jsonObject.Properties())
                    {
                        string columnName = property.Name;
                        string value = property.Value.ToString();

                        if (columnName == "BankAccNo")
                        {
                            columnName = "AccountNumber";
                        }
                        if (columnName == "Skill")
                        {
                            columnName = "SkillName";
                        }
                        if (columnName == "JoinDate")
                        {
                            columnName = "DtJoin";
                        }
                        if (columnName == "Email")
                        {
                            columnName = "EmpEmail";
                        }
                        if (!string.IsNullOrEmpty(value))
                        {
                            SearchColumns += columnName + ",";
                            SearchKeywords += value + ",";
                        }
                    }

                    string toremove = pageIndex.ToString() + "," + pageSize.ToString() + ",";
                    string toremove1 = "pageIndex,pageSize,";
                    int lastIndex = SearchKeywords.LastIndexOf(toremove);
                    int lastIndex1 = SearchColumns.LastIndexOf(toremove1);
                    if (lastIndex1 == -1)
                    {
                        lastIndex1 = 0;
                    }
                    if (lastIndex == -1)
                    {
                        lastIndex = 0;
                    }
                    SearchKeywords = SearchKeywords.Substring(0, lastIndex) + SearchKeywords.Substring(lastIndex + toremove.Length);
                    SearchColumns = SearchColumns.Substring(0, lastIndex1) + SearchColumns.Substring(lastIndex1 + toremove1.Length);
                    SearchColumns = SearchColumns.TrimEnd(',', ' ');

                }
                var temp = criteria;
                criteria += "Count";
                var totalRow = _hRRepository.EmpListIndexCount(criteria, Offset, fetch, SearchKeywords, SearchColumns, startDate, endDate).ToList();
                decimal TotalRecordCount = totalRow[0].TotalRecord;
                var PageCountabc = decimal.Parse((TotalRecordCount / pageSize).ToString());
                var PageCount = Math.Ceiling(PageCountabc);


                decimal skip = (pageIndex - 1) * pageSize;
                // Get total number of records
                int total = totalRow[0].TotalRecord;
                var employeelist = _hRRepository.EmpListIndex(temp, Offset, fetch, SearchKeywords, SearchColumns, startDate, endDate).ToList();
                //var abcd = employeelist.OrderByDescending(x => x.EmpCode).Skip(int.Parse(skip.ToString())).Take(int.Parse(pageSize.ToString())).ToList();// Take(50);
                var pageinfo = new PagingInfo();
                pageinfo.PageCount = int.Parse(PageCount.ToString());
                pageinfo.PageNo = pageIndex;
                pageinfo.PageSize = int.Parse(pageSize.ToString());
                pageinfo.TotalRecordCount = int.Parse(TotalRecordCount.ToString());

                //return  abcd;
                return Json(new { Success = 1, error = false, EmployeeList = employeelist, PageInfo = pageinfo });


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class PagingInfo
        {
            public int PageCount { get; set; }
            public int PageNo { get; set; }
            public int PageSize { get; set; }
            public int TotalRecordCount { get; set; }
        }
        public ActionResult Print(int? id, string type = "pdf")
        {

            var callBackUrl = _hRRepository.EmpListPrint(id, type);
            return Redirect(callBackUrl);

        }

        public JsonResult GetEmployeesAll(string From, string To, string criteria, string searchquery = "")
        {
            criteria += "Excel";
            string dateString = "1950-01-01 00:00:00.0000000";
            DateTime startDate = DateTime.Parse(dateString);
            DateTime endDate = DateTime.Parse(dateString);
            DateTime dateValue1;
            DateTime dateValue2;
            if (DateTime.TryParseExact(From, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue1))
            {
                startDate = DateTime.ParseExact(dateValue1.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            }
            if (DateTime.TryParseExact(To, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue2))
            {
                endDate = DateTime.ParseExact(dateValue2.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            }

            string SearchColumns = "";
            string SearchKeywords = "";
            if (searchquery?.Length > 1)
            {

                JObject jsonObject = JObject.Parse(searchquery);


                foreach (JProperty property in jsonObject.Properties())
                {
                    string columnName = property.Name;
                    string value = property.Value.ToString();
                    if (columnName == "BankAccNo")
                    {
                        columnName = "AccountNumber";
                    }
                    if (columnName == "Skill")
                    {
                        columnName = "SkillName";
                    }
                    if (columnName == "JoinDate")
                    {
                        columnName = "DtJoin";
                    }
                    if (columnName == "Email")
                    {
                        columnName = "EmpEmail";
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        SearchColumns += columnName + ",";
                        SearchKeywords += value + ",";
                    }
                }

                string toremove = "1,10,";
                string toremove1 = "pageIndex,pageSize,";
                int lastIndex = SearchKeywords.LastIndexOf(toremove);
                int lastIndex1 = SearchColumns.LastIndexOf(toremove1);
                if (lastIndex1 == -1)
                {
                    lastIndex1 = 0;
                }
                if (lastIndex == -1)
                {
                    lastIndex = 0;
                }
                SearchKeywords = SearchKeywords.Substring(0, lastIndex) + SearchKeywords.Substring(lastIndex + toremove.Length);
                SearchColumns = SearchColumns.Substring(0, lastIndex1) + SearchColumns.Substring(lastIndex1 + toremove1.Length);
                SearchColumns = SearchColumns.TrimEnd(',', ' ');

            }
            var data = _hRRepository.EmpListIndex(criteria, 0, 0, SearchKeywords, SearchColumns, startDate, endDate).ToList();
            var total = data.Count();
            return Json(new { Success = 1, error = false, EmployeeList = data, Total = total });
        }

        public IActionResult EmployeeListTry()
        {
            try
            {
                List<string> myList = new List<string> { "This Month", "This Week", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
                IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
                List<SelectListItem> mySelectListAsList = mySelectList.ToList();
                ViewBag.period = mySelectListAsList;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw ex;
            }
        }
        #endregion

        #region ID Card
        public IActionResult IDCardList()
        {
            @ViewBag.Dt = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.EmpList = _empInfoRepository.GetAll();

            ViewBag.Departments = _departmentRepository.GetAll();

            ViewBag.Sections = _sectionRepository.GetAll();

            ViewBag.Designations = _designationRepository.GetAll();


            List<IdcardGreadData> IdcardGreadData = _idCardRepository.GetIdcard();
            return View(IdcardGreadData.ToList());
        }




        [HttpPost]
        public IActionResult IDCardList(IdCard idCard)
        {
            _idCardRepository.SaveIdcard(idCard);
            return Json("");
        }


        public IActionResult IdcardViewReport(DateTime fromDate, DateTime toDate, IdCard IdCard)
        {
            var callBackUrl = _idCardRepository.PrintIdcardReport(fromDate, toDate, IdCard);
            return Redirect(callBackUrl);
        }



        [HttpPost]
        public IActionResult EmployeeDataByDate(string FromDate, string ToDate)
        {
            List<IdcardGreadData> IdcardGreadData = _idCardRepository.loadDataByDate(FromDate, ToDate);
            return Json(IdcardGreadData);
        }




        public IActionResult IDCardListPrint()
        {
            @ViewBag.Dt = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.EmpList = _empInfoRepository.GetAll();

            ViewBag.Departments = _departmentRepository.GetAll();

            ViewBag.Sections = _sectionRepository.GetAll();

            ViewBag.Designations = _designationRepository.GetAll();


            List<IdcardGreadData> IdcardGreadData = _idCardRepository.GetIdcard();
            return View(IdcardGreadData.ToList());
        }

        [HttpPost]
        public IActionResult IDCardListPrint(IdCard idCard)
        {
            _idCardRepository.SaveIdcard(idCard);
            return Json("");
        }
        public IActionResult IdcardViewReportPrint(DateTime fromDate, DateTime toDate, IdCard IdCard)
        {
            var callBackUrl = _idCardRepository.PrintIdcardReport(fromDate, toDate, IdCard);
            return Redirect(callBackUrl);

        }




        [HttpPost]
        public IActionResult EmployeeDataByDatePrint(string FromDate, string ToDate)
        {
            List<IdcardGreadData> IdcardGreadData = _idCardRepository.loadDataByDate(FromDate, ToDate);
            return Json(IdcardGreadData);
        }


        #endregion

        #region Holiday Setup
        public IActionResult HoliDaySetupList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreateHoliDaySetup(HR_ProssType_WHDay WHProssType)
        {
            var comid = HttpContext.Session.GetString("comid");
            if (ModelState.IsValid)
            {
                if (WHProssType.WHId > 0)
                {
                    _holidaySetupRepository.UpdateHoliDaySetUp(WHProssType);
                    TempData["Message"] = "Data Update Successfully";
                }
                else
                {
                    var data = _context.HR_ProssType_WHDay.Where(x => x.ComId == comid && !x.IsDelete).Any(x => x.dtPunchDate == WHProssType.dtPunchDate);
                    if (data == true)
                    {
                        TempData["Message"] = "Please Select Another Date. This Date is Exists.";
                        return Json(new { Success = 3, ex = TempData["Message"].ToString() });
                    }
                    else
                    {
                        _holidaySetupRepository.CreateHoliDaySetUp(WHProssType);
                        TempData["Message"] = "Data Save Successfully";
                    }

                }
                _context.SaveChanges();
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });

            }
            else
            {
                TempData["Message"] = "Models state is not valid";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        [HttpPost]
        public JsonResult DeleteWHProssTypeAjax(int addId)
        {
            var WHProssType = _holidaySetupRepository.FindById(addId);
            if (WHProssType != null)
            {
                _holidaySetupRepository.Remove(WHProssType);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }

            TempData["Message"] = "No Data Found";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }

        [HttpPost]
        public ActionResult LoadWHProssTypePartial(DateTime date)
        {
            string comid = HttpContext.Session.GetString("comid");
            var WHProssTypes = _holidaySetupRepository.GetAll()
                .Where(s => s.dtPunchDate.Value.Date.Year == date.Year && s.ComId == comid)
                .ToList();

            return Json(WHProssTypes);
        }

        #endregion

        #region Leave adjust
        public IActionResult LeaveAdjustList()
        {
            var leavelist = _context.Cat_Variable.Where(x => x.VarType == "LeaveAdjust").ToList();
            List<SelectListItem> Leavelist = leavelist
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();


            ViewData["Leavelist"] = new SelectList(Leavelist, "Value", "Text");
            return View();
        }

        public IActionResult LeaveAdjustListUBL()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var leavelist = _context.Cat_Variable.Where(x => x.VarType == "LeaveAdjust").ToList();
            List<SelectListItem> Leavelist = leavelist
            .Select(item => new SelectListItem
            {
                Value = item.VarName,
                Text = item.VarName
            })
            .ToList();

            //var employeeList = _context.HR_Emp_Info.Where(x => x.ComId == comid).ToList();

            //List<SelectListItem> selectLists = employeeList
            //.Select(item => new SelectListItem
            //{
            //    Value = item.EmpCode,
            //    Text = item.EmpName
            //})
            //.ToList();


            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid && emp.IsDelete == false && emp.IsInactive == false
                           orderby emp.EmpId
                           select new SelectListItem
                           {
                               Value = emp.EmpCode,
                               Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            //ViewData["Employee"] = empInfo;

            ViewData["EmpCode"] = new SelectList(empInfo, "Value", "Text");
            return View();
        }

        [HttpPost]
        public JsonResult CreateLeaveAdjustUBL(HR_Leave_AdjustUbL model)
        {
            var comid = HttpContext.Session.GetString("comid");
            var pcname = HttpContext.Session.GetString("pcname");
            var userid = HttpContext.Session.GetString("userid");

            //var isexist = _context.HR_Leave_Adjust.Where(x => x.DtFrom == model.DtFrom && x.ComId == comid && x.Remark.Contains("Adjust")).ToList();

            //var isexist = _context.HR_Leave_AdjustUbL.Where(x => x.DutyDate.Date.Day == model.DutyDate.Date.Day && x.ComId == comid && x.Remark.Contains("Adjust")).FirstOrDefault();
            var isexist = _context.HR_Leave_AdjustUbL.Where(x => x.DutyDate.Date.Day == model.DutyDate.Date.Day || x.ReplaceDate.Date.Day == model.ReplaceDate.Date.Day && x.ComId == comid && x.EmpCode == model.EmpCode).FirstOrDefault();
            //  if (isexist.Count > 0)
            if (model.Id != 0 || isexist != null)
            {
                var exist = _context.HR_Leave_AdjustUbL.Where(x => x.Id == model.Id && x.ComId == comid).FirstOrDefault();
                // ComId = comid.ToString(),
                exist.ReplaceDate = model.ReplaceDate;
                if (model.DutyDate != new DateTime(0001, 01, 01))
                {
                    // Your code here
                    exist.DutyDate = model.DutyDate;
                }
                exist.Remark = model.Remark;
                //EmpCode = model.EmpCode,
                exist.UpdateByUserId = userid.ToString();
                // IsDelete = false,
                exist.DateAdded = DateTime.Now;
                exist.DateUpdated = DateTime.Now;

                _context.HR_Leave_AdjustUbL.Update(exist);
                _context.SaveChanges();
                TempData["Message"] = "Update Successfull..!!";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            else
            {
                var lvadjust = new HR_Leave_AdjustUbL
                {
                    ComId = comid.ToString(),
                    ReplaceDate = model.ReplaceDate,
                    DutyDate = model.DutyDate,
                    Remark = model.Remark,
                    EmpCode = model.EmpCode,
                    UpdateByUserId = userid.ToString(),
                    IsDelete = false,
                    DateAdded = DateTime.Now,
                    DateUpdated = DateTime.Now
                };

                // Now 'lvadjust' is an instance of HR_Leave_AdjustUbL with default values.


                _context.HR_Leave_AdjustUbL.Add(lvadjust);
                _context.SaveChanges();

                //if (pcname == null)
                //{
                //    pcname = "HR PC";
                //}
                //SqlParameter p1 = new SqlParameter("@Id", 1);
                //SqlParameter p2 = new SqlParameter("@ComId", comid);
                //SqlParameter p3 = new SqlParameter("@dtJoin", model.dtJoin);
                //SqlParameter p4 = new SqlParameter("@dtLeave", model.DtFrom);
                //SqlParameter p5 = new SqlParameter("@Remarks", model.Remark);
                //SqlParameter p6 = new SqlParameter("@PCName", pcname);
                //SqlParameter p7 = new SqlParameter("@LUserId", 2);
                //var data = Helper.ExecProcMapTList<HR_Leave_Avail>("dbo.Hr_prcProcessLeaveAdjust", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7 });
                TempData["Message"] = "Successfully Created..!!";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            //if (isexist != null)
            //{
            //    TempData["Message"] = "Same Date Already Exists";
            //    return Json(new { Success = 2, ex = TempData["Message"].ToString() });
            //}
            //else
            //{

            //}




        }


        [HttpGet]
        public ActionResult LoadLeaveAdjustByEmpCode(string empcode)
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _context.HR_Leave_AdjustUbL.Where(x => x.ComId == comid && x.EmpCode == empcode).ToList();
            return Json(data);
        }


        [HttpPost]
        public JsonResult CreateLeaveAdjust(HR_Leave_Avail model)
        {
            var comid = HttpContext.Session.GetString("comid");
            var pcname = HttpContext.Session.GetString("pcname");

            //var isexist = _context.HR_Leave_Adjust.Where(x => x.DtFrom == model.DtFrom && x.ComId == comid && x.Remark.Contains("Adjust")).ToList();

            var isexist = _context.HR_Leave_Adjust.Where(x => x.DtFrom.Date.Day == model.DtFrom.Date.Day && x.ComId == comid && x.Remark.Contains("Adjust")).FirstOrDefault();
            //  if (isexist.Count > 0)
            if (isexist != null)
            {
                TempData["Message"] = "Same Date Already Exists";
                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
            }

            var lvadjust = new HR_Leave_Adjust();
            lvadjust.ComId = comid;
            lvadjust.dtJoin = model.dtJoin ?? DateTime.Now;
            lvadjust.DtFrom = model.DtFrom;
            lvadjust.Remark = model.Remark;

            _context.Add(lvadjust);
            _context.SaveChangesAsync();

            if (pcname == null)
            {
                pcname = "HR PC";
            }
            SqlParameter p1 = new SqlParameter("@Id", 1);
            SqlParameter p2 = new SqlParameter("@ComId", comid);
            SqlParameter p3 = new SqlParameter("@dtJoin", model.dtJoin);
            SqlParameter p4 = new SqlParameter("@dtLeave", model.DtFrom);
            SqlParameter p5 = new SqlParameter("@Remarks", model.Remark);
            SqlParameter p6 = new SqlParameter("@PCName", pcname);
            SqlParameter p7 = new SqlParameter("@LUserId", 2);
            var data = Helper.ExecProcMapTList<HR_Leave_Avail>("dbo.Hr_prcProcessLeaveAdjust", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7 });
            return Json(new { Success = 1, ex = TempData["Message"].ToString() });


        }

        [HttpPost]
        public JsonResult DeleteLeaveAdjust(DateTime dtFrom, string remark)
        {
            string comid = HttpContext.Session.GetString("comid");

            // var model = _context.HR_Leave_Adjust.Where(x => x.ComId == comid && x.DtFrom == dtFrom && x.Remark == remark).FirstOrDefault();
            var model = _context.HR_Leave_Adjust.Where(x => x.DtFrom.Date.Day == dtFrom.Date.Day && x.ComId == comid && x.Remark.Contains("Adjust")).FirstOrDefault();
            _context.Remove(model);
            _context.SaveChangesAsync();

            SqlParameter p1 = new SqlParameter("@Id", 2);
            SqlParameter p2 = new SqlParameter("@ComId", comid);
            SqlParameter p3 = new SqlParameter("@dtJoin", "");
            SqlParameter p4 = new SqlParameter("@dtLeave", dtFrom);
            SqlParameter p5 = new SqlParameter("@Remarks", "");
            SqlParameter p6 = new SqlParameter("@PCName", "");
            SqlParameter p7 = new SqlParameter("@LUserId", "");
            var data = Helper.ExecProcMapTList<HR_Leave_Avail>("dbo.Hr_prcProcessLeaveAdjust", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7 });

            //TempData["Message"] = "No Data Found";
            TempData["Message"] = "Data Delete Successfully";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }

        public JsonResult DeleteLeaveAdjustUBL(HR_Leave_AdjustUbL model)
        {
            string comid = HttpContext.Session.GetString("comid");

            // var model = _context.HR_Leave_Adjust.Where(x => x.ComId == comid && x.DtFrom == dtFrom && x.Remark == remark).FirstOrDefault();
            var data = _context.HR_Leave_AdjustUbL.Where(x => x.Id == model.Id && x.ComId == comid).FirstOrDefault();
            _context.Remove(data);
            _context.SaveChanges();

            //SqlParameter p1 = new SqlParameter("@Id", 2);
            //SqlParameter p2 = new SqlParameter("@ComId", comid);
            //SqlParameter p3 = new SqlParameter("@dtJoin", "");
            //SqlParameter p4 = new SqlParameter("@dtLeave", dtFrom);
            //SqlParameter p5 = new SqlParameter("@Remarks", "");
            //SqlParameter p6 = new SqlParameter("@PCName", "");
            //SqlParameter p7 = new SqlParameter("@LUserId", "");
            //var data = Helper.ExecProcMapTList<HR_Leave_Avail>("dbo.Hr_prcProcessLeaveAdjust", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7 });

            //TempData["Message"] = "No Data Found";
            TempData["Message"] = "Data Delete Successfully";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }


        [HttpPost]
        public JsonResult EditLeaveAdjust(DateTime dtFrom, DateTime dtJoin, string remark)
        {
            string comid = HttpContext.Session.GetString("comid");
            var pcname = HttpContext.Session.GetString("pcname");

            var model = _context.HR_Leave_Adjust.Where(x => x.ComId == comid && x.DtFrom == dtFrom && x.dtJoin == dtJoin).FirstOrDefault();
            _context.Remove(model);
            _context.SaveChangesAsync();

            SqlParameter p1 = new SqlParameter("@Id", 2);
            SqlParameter p2 = new SqlParameter("@ComId", comid);
            SqlParameter p3 = new SqlParameter("@dtJoin", "");
            SqlParameter p4 = new SqlParameter("@dtLeave", dtFrom);
            SqlParameter p5 = new SqlParameter("@Remarks", "");
            SqlParameter p6 = new SqlParameter("@PCName", "");
            SqlParameter p7 = new SqlParameter("@LUserId", "");
            var data = Helper.ExecProcMapTList<HR_Leave_Avail>("dbo.Hr_prcProcessLeaveAdjust", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7 });

            var lvadjust = new HR_Leave_Adjust();
            lvadjust.ComId = comid;
            lvadjust.dtJoin = dtJoin;
            lvadjust.DtFrom = dtFrom;
            lvadjust.Remark = remark;

            _context.Add(lvadjust);
            _context.SaveChangesAsync();

            if (pcname == null)
            {
                pcname = "HR PC";
            }
            SqlParameter p8 = new SqlParameter("@Id", 1);
            SqlParameter p9 = new SqlParameter("@ComId", comid);
            SqlParameter p10 = new SqlParameter("@dtJoin", dtJoin);
            SqlParameter p11 = new SqlParameter("@dtLeave", dtFrom);
            SqlParameter p12 = new SqlParameter("@Remarks", remark);
            SqlParameter p13 = new SqlParameter("@PCName", pcname);
            SqlParameter p14 = new SqlParameter("@LUserId", 2);
            var data1 = Helper.ExecProcMapTList<HR_Leave_Avail>("dbo.Hr_prcProcessLeaveAdjust", new SqlParameter[] { p8, p9, p10, p11, p12, p13, p14 });



            TempData["Message"] = "Data Updated Successfully";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }

        [HttpPost]
        public ActionResult LoadLeaveAdjustPartial(DateTime date)
        {
            string comid = HttpContext.Session.GetString("comid");
            SqlParameter p1 = new SqlParameter("@Id", 0);
            SqlParameter p2 = new SqlParameter("@ComId", comid);
            SqlParameter p3 = new SqlParameter("@dtJoin", "");
            SqlParameter p4 = new SqlParameter("@dtLeave", date);
            SqlParameter p5 = new SqlParameter("@Remarks", "");
            SqlParameter p6 = new SqlParameter("@PCName", "");
            SqlParameter p7 = new SqlParameter("@LUserId", "");
            var data = Helper.ExecProcMapTList<HR_Leave_Avail>("dbo.Hr_prcProcessLeaveAdjust", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7 });


            return Json(data);
        }


        [HttpPost]
        public IActionResult getDtjoinubl(DateTime dtFrom, string remark)
        {
            string comid = HttpContext.Session.GetString("comid");

            var data = _context.HR_Leave_AdjustUbL.Where(x => x.ComId == comid && x.DutyDate == dtFrom && x.Remark == remark).Select(y => y.ReplaceDate).FirstOrDefault();

            return Json(data);
        }

        [HttpPost]
        public IActionResult getDtjoin(DateTime dtFrom, string remark)
        {
            string comid = HttpContext.Session.GetString("comid");

            var data = _context.HR_Leave_Adjust.Where(x => x.ComId == comid && x.DtFrom == dtFrom && x.Remark == remark).Select(y => y.dtJoin).FirstOrDefault();

            return Json(data);
        }

        #endregion



        #region Loan Halt
        public IActionResult LoanHaltList()
        {

            ViewBag.LoanTypes = _loanHaltRepository.LoanTypeCat_Variable();

            ViewBag.OtherLoanType = _loanHaltRepository.OtherTypeCat_Variable();

            ViewBag.Employees = _loanHaltRepository.GetEmployee();

            return View();
        }
        public IActionResult CreateLoanHalt(LoanHalt loanHalt)
        {
            try
            {
                _loanHaltRepository.LoanCreate(loanHalt);
                TempData["Message"] = "Data Save Successfully";

                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }


            catch (Exception e)
            {
                TempData["Message"] = e.Message;
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }
        #endregion

        #region Loan House Building
        public IActionResult LoanHouseBuildingList()
        {
            var loanData = _loanHouseBuilding.GetLoanHouseDataList();
            return View(loanData);

        }
        [HttpGet]
        public IActionResult CreateLoanHouseBuilding()
        {

            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";

            var empInfo = _loanHouseBuilding.GetEmpHouseList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");

            ViewBag.Compound = _loanHouseBuilding.CompoundHouseList();

            ViewBag.PayBack = _loanHouseBuilding.PayBackHouseList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateLoanHouseBuilding(HR_Loan_HouseBuilding hR_Loan_HouseBuilding, bool newCalculation)
        {
            try
            {
                //  var errors = ModelState.Where(x => x.Value.Errors.Any())
                //.Select(x => new { x.Key, x.Value.Errors });
                if (ModelState.IsValid)
                {
                    string comid = HttpContext.Session.GetString("comid");
                    string userid = HttpContext.Session.GetString("userid");
                    if (hR_Loan_HouseBuilding.LoanHouseId > 0)
                    {
                        _loanHouseBuilding.SaveLoanHouseBuilding(hR_Loan_HouseBuilding, newCalculation);
                    }
                    else
                    {
                        var unPaidLoan = _loanHouseBuilding.unPaidHouse(hR_Loan_HouseBuilding);

                        if (unPaidLoan != null)
                        {
                            TempData["Message"] = "This Employee has unpaide Loan";
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                        hR_Loan_HouseBuilding.DtTran = DateTime.Now.Date;
                        _context.Add(hR_Loan_HouseBuilding);
                    }
                    _context.SaveChanges();

                    TempData["Message"] = "Loan Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        public IActionResult CalcualteLoanHouseBuildingPartial(decimal lAmount, decimal interest, int period, DateTime startDate,
            decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
        {

            var loanCal = _loanHouseBuilding.
                CalcualteLoanHousePartial(lAmount, interest, period, startDate, ttlLoanAmt, ttlInterest, monthlyLoanAmt, loanType);
            var loanData = loanCal.Select(l => new
            {
                Period = l.PERIOD,
                DtLoanMonth = String.Format("{0:dd-MMM-yyyy}", l.PAYDATE),
                MonthlyLoanAmount = l.PAYMENT,
                InterestAmount = l.INTEREST,
                PrincipalAmount = l.PRINCIPAL,
                BeginningLoanBalance = l.BeginningBalance,
                EndingBalance = l.EndingBalance
            }).ToList();
            //ViewBag.LoanCalculation = loanData;
            //return PartialView("~/Views/LoanHouseBuilding/_LoanHouseBuildingGrid.cshtml", new HR_Loan_Data_HouseBuilding());
            return Json(loanData);
        }

        [HttpGet]
        public IActionResult GetEmpLoanHouseBuilding(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanHouseBuilding.loanHouseMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanHouseBuilding.GetEmpHouseList();

                ViewBag.Compound = _loanHouseBuilding.CompoundHouseList();

                ViewBag.PayBack = _loanHouseBuilding.PayBackHouseList();

                ViewBag.LoanCalculation = _loanHouseBuilding.LoanHouseCalc(id);

                return View("Create", loandMaster);
            }

            ViewBag.Title = "Create";
            HR_Loan_HouseBuilding loan = new HR_Loan_HouseBuilding();

            loan.HR_Emp_Info = _context.HR_Emp_Info
                .Include(l => l.Cat_Department)
                .Include(l => l.Cat_Designation)
                .Include(l => l.HR_Emp_Image)
                .Where(l => l.EmpId == id).FirstOrDefault();

            ViewBag.EmpId = _loanHouseBuilding.GetEmpList(id);

            ViewBag.Compound = _loanHouseBuilding.CompoundHouseList();

            ViewBag.PayBack = _loanHouseBuilding.PayBackHouseList();

            return View("CreateLoanHouseBuilding", loan);
        }
        [HttpGet]
        public IActionResult EditLoanHouseBuilding(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanHouseBuilding.loanHouseMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanHouseBuilding.GetEmpHouseList();

                ViewBag.Compound = _loanHouseBuilding.CompoundHouseList();

                ViewBag.PayBack = _loanHouseBuilding.PayBackHouseList();
                ViewBag.LoanCalculation = _loanHouseBuilding.LoanHouseCalc(id);
                return View("CreateLoanHouseBuilding", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanHouseBuilding.GetEmpHouseList();

            ViewBag.Compound = _loanHouseBuilding.CompoundHouseList();
            ViewBag.PayBack = _loanHouseBuilding.PayBackHouseList();

            return View("CreateLoanHouseBuilding", loandMaster);
        }


        [HttpGet]
        public IActionResult DeleteLoanHouseBuilding(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanHouseBuilding.loanHouseMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Delete";

                ViewBag.EmpId = _loanHouseBuilding.GetEmpHouseList();

                ViewBag.Compound = _loanHouseBuilding.CompoundHouseList();

                ViewBag.PayBack = _loanHouseBuilding.PayBackHouseList();

                ViewBag.LoanCalculation = _loanHouseBuilding.LoanHouseCalc(id);

                return View("CreateLoanHouseBuilding", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanHouseBuilding.GetEmpHouseList();

            ViewBag.Compound = _loanHouseBuilding.CompoundHouseList();

            ViewBag.PayBack = _loanHouseBuilding.PayBackHouseList();

            return View("CreateLoanHouseBuilding", loandMaster);
        }


        [HttpPost, ActionName("DeleteLoanHouseBuilding")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteLoanHouseBuildingConfirmed(int id)
        {

            try
            {
                var master = _loanHouseBuilding.FindById(id);
                _loanHouseBuilding.Delete(master);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, LoanHouseId = master.LoanHouseId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion

        #region Loan Motor Cycle 
        public IActionResult LoanMotorCycleList()
        {
            var loanData = _loanMotorCycleRepository.GetLoanMotorDataList();
            return View(loanData);
        }

        [HttpGet]
        public IActionResult CreateLoanMotor()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";


            ViewData["EmpId"] = _loanMotorCycleRepository.GetEmpMotorList();

            ViewBag.Compound = _loanMotorCycleRepository.CompoundMotorList();

            ViewBag.PayBack = _loanMotorCycleRepository.PayBackMotorList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateLoanMotor(HR_Loan_MotorCycle hR_Loan_MotorCycle, bool newCalculation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string comid = HttpContext.Session.GetString("comid");
                    string userid = HttpContext.Session.GetString("userid");
                    if (hR_Loan_MotorCycle.LoanMotorId > 0)
                    {
                        _loanMotorCycleRepository.SaveLoanMotorCycle(hR_Loan_MotorCycle, newCalculation);
                    }
                    else
                    {
                        var unPaidLoan = _loanMotorCycleRepository.unPaidMotor(hR_Loan_MotorCycle);

                        if (unPaidLoan != null)
                        {
                            TempData["Message"] = "This Employee has unpaide Loan";
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                        hR_Loan_MotorCycle.DtTran = DateTime.Now.Date;
                        _context.Add(hR_Loan_MotorCycle);
                    }
                    _context.SaveChanges();

                    TempData["Message"] = "Loan Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        public IActionResult CalcualteLoanMotorPartial(decimal lAmount, decimal interest, int period, DateTime startDate,
          decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
        {

            List<CalculateData> loanCal = _loanMotorCycleRepository.CalcualteLoanMotorPartial(lAmount, interest, period, startDate,
                ttlLoanAmt, ttlInterest, monthlyLoanAmt, loanType);
            var loanData = loanCal.Select(l => new
            {
                Period = l.PERIOD,
                DtLoanMonth = String.Format("{0:dd-MMM-yyyy}", l.PAYDATE),
                MonthlyLoanAmount = l.PAYMENT,
                InterestAmount = l.INTEREST,
                PrincipalAmount = l.PRINCIPAL,
                BeginningLoanBalance = l.BeginningBalance,
                EndingBalance = l.EndingBalance
            }).ToList();
            //ViewBag.LoanCalculation = loanData;
            //return PartialView("~/Views/LoanMotorCycle/_LoanMotorCycleGrid.cshtml", new HR_Loan_Data_MotorCycle());
            return Json(loanData);
        }

        [HttpGet]
        public IActionResult GetEmpLoanMotor(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanMotorCycleRepository.loanMotorMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanMotorCycleRepository.GetEmpMotorList();

                ViewBag.Compound = _loanMotorCycleRepository.CompoundMotorList();

                ViewBag.PayBack = _loanMotorCycleRepository.PayBackMotorList();

                ViewBag.LoanCalculation = _loanMotorCycleRepository.LoanMotorCalc(id);

                return View("CreateLoanMotor", loandMaster);
            }

            ViewBag.Title = "Create";
            HR_Loan_MotorCycle loan = new HR_Loan_MotorCycle();

            loan.HR_Emp_Info = _context.HR_Emp_Info
                .Include(l => l.Cat_Department)
                .Include(l => l.Cat_Designation)
                .Include(l => l.HR_Emp_Image)
                .Where(l => l.EmpId == id).FirstOrDefault();

            ViewBag.EmpId = _loanHouseBuilding.GetEmpList(id);

            ViewBag.Compound = _loanMotorCycleRepository.CompoundMotorList();

            ViewBag.PayBack = _loanMotorCycleRepository.PayBackMotorList();

            return View("CreateLoanMotor", loan);
        }

        [HttpGet]
        public IActionResult EditLoanMotor(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanMotorCycleRepository.loanMotorMaster(id);
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanMotorCycleRepository.GetEmpMotorList();

                ViewBag.Compound = _loanMotorCycleRepository.CompoundMotorList();

                ViewBag.PayBack = _loanMotorCycleRepository.PayBackMotorList();

                ViewBag.LoanCalculation = _loanMotorCycleRepository.LoanMotorCalc(id);

                return View("CreateLoanMotor", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanMotorCycleRepository.GetEmpMotorList();

            ViewBag.Compound = _loanMotorCycleRepository.CompoundMotorList();

            ViewBag.PayBack = _loanMotorCycleRepository.PayBackMotorList();
            return View("CreateLoanMotor", loandMaster);
        }

        [HttpGet]
        public IActionResult DeleteLoanMotor(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanMotorCycleRepository.loanMotorMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Delete";

                ViewBag.EmpId = _loanMotorCycleRepository.GetEmpMotorList();

                ViewBag.Compound = _loanMotorCycleRepository.CompoundMotorList();

                ViewBag.PayBack = _loanMotorCycleRepository.PayBackMotorList();

                ViewBag.LoanCalculation = _loanMotorCycleRepository.LoanMotorCalc(id);

                return View("CreateLoanMotor", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanMotorCycleRepository.GetEmpMotorList();

            ViewBag.Compound = _loanMotorCycleRepository.CompoundMotorList();

            ViewBag.PayBack = _loanMotorCycleRepository.PayBackMotorList();

            return View("CreateLoanMotor", loandMaster);
        }

        [HttpPost, ActionName("DeleteLoanMotor")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteLoanMotorConfirmed(int id)
        {

            try
            {
                var master = _loanMotorCycleRepository.FindById(id);
                _loanMotorCycleRepository.DeleteMotorCycle(id);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, LoanMotorId = master.LoanMotorId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Loan Other
        public IActionResult LoanOtherList()
        {
            var loanData = _loanOtherRepository.GetLoanOtherDataList();
            return View(loanData);
        }
        [HttpGet]
        public IActionResult CreateLoanOther()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";


            ViewData["EmpId"] = _loanOtherRepository.GetEmpOtherList();

            ViewBag.Compound = _loanOtherRepository.CompoundOtherList();


            ViewBag.LoanType = _loanOtherRepository.LoanTypeList();

            ViewBag.PayBack = _loanOtherRepository.PayBackOtherList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateLoanOther(HR_Loan_Other hR_Loan_Other, bool newCalculation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string comid = HttpContext.Session.GetString("comid");
                    string userid = HttpContext.Session.GetString("userid");
                    if (hR_Loan_Other.LoanOtherId > 0)
                    {
                        var master = _context.HR_Loan_Other.Where(l => l.EmpId == hR_Loan_Other.EmpId && l.LoanType == hR_Loan_Other.LoanType
                                && l.LoanOtherId != hR_Loan_Other.LoanOtherId && l.ComId == comid).FirstOrDefault();
                        if (master != null)
                        {
                            var unPaidLoan = _loanOtherRepository.unPaidOther1(hR_Loan_Other);
                            if (unPaidLoan != null)
                            {
                                TempData["Message"] = "This Employee has unpaide Loan";
                                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                            }
                        }
                        _context.Update(hR_Loan_Other);
                    }
                    else
                    {
                        var master = _context.HR_Loan_Other.Where(l => l.EmpId == hR_Loan_Other.EmpId
                                      && l.LoanType == hR_Loan_Other.LoanType && l.ComId == comid).FirstOrDefault();
                        if (master != null)
                        {
                            var unPaidLoan = _loanOtherRepository.unPaidOther2(hR_Loan_Other);
                            if (unPaidLoan != null)
                            {
                                TempData["Message"] = "This Employee has unpaide Loan";
                                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                            }
                        }

                        hR_Loan_Other.DtTran = DateTime.Now.Date;
                        _context.Add(hR_Loan_Other);
                    }
                    _context.SaveChanges();

                    TempData["Message"] = "Loan Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        public IActionResult CalcualteLoanOtherPartial(decimal lAmount, decimal interest, int period, DateTime startDate,
            decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
        {

            List<CalculateData> loanCal = _loanOtherRepository.CalcualteLoanOtherPartial(lAmount, interest, period, startDate, ttlLoanAmt, ttlInterest, monthlyLoanAmt, loanType);
            var loanData = loanCal.Select(l => new
            {
                Period = l.PERIOD,
                DtLoanMonth = String.Format("{0:dd-MMM-yyyy}", l.PAYDATE),
                MonthlyLoanAmount = l.PAYMENT,
                InterestAmount = l.INTEREST,
                PrincipalAmount = l.PRINCIPAL,
                BeginningLoanBalance = l.BeginningBalance,
                EndingBalance = l.EndingBalance
            }).ToList();
            //ViewBag.LoanCalculation = loanData;
            //return PartialView("~/Views/LoanOther/_LoanOtherGrid.cshtml", new HR_Loan_Data_Other());
            return Json(loanData);
        }

        [HttpGet]
        public IActionResult GetEmpLoanOther(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanOtherRepository.loanOtherMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanOtherRepository.GetEmpOtherList();

                ViewBag.Compound = _loanOtherRepository.CompoundOtherList();

                ViewBag.LoanType = _loanOtherRepository.LoanTypeList();

                ViewBag.PayBack = _loanOtherRepository.PayBackOtherList();

                ViewBag.LoanCalculation = _loanOtherRepository.LoanOtherCalc(id);
                return View("Create", loandMaster);
            }

            ViewBag.Title = "Create";
            HR_Loan_Other loan = new HR_Loan_Other();

            loan.HR_Emp_Info = _context.HR_Emp_Info
                .Include(l => l.Cat_Department)
                .Include(l => l.Cat_Designation)
                .Include(l => l.HR_Emp_Image)
                .Where(l => l.EmpId == id).FirstOrDefault();

            ViewBag.EmpId = _loanHouseBuilding.GetEmpList(id);

            ViewBag.Compound = _loanOtherRepository.CompoundOtherList();

            ViewBag.LoanType = _loanOtherRepository.LoanTypeList();

            ViewBag.PayBack = _loanOtherRepository.PayBackOtherList();

            return View("CreateLoanOther", loan);
        }
        [HttpGet]
        public IActionResult EditLoanOther(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanOtherRepository.loanOtherMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanOtherRepository.GetEmpOtherList();

                ViewBag.Compound = _loanOtherRepository.CompoundOtherList();

                ViewBag.LoanType = _loanOtherRepository.LoanTypeList();

                ViewBag.PayBack = _loanOtherRepository.PayBackOtherList();

                ViewBag.LoanCalculation = _loanOtherRepository.LoanOtherCalc(id);

                return View("CreateLoanOther", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanOtherRepository.GetEmpOtherList();

            ViewBag.Compound = _loanOtherRepository.CompoundOtherList();

            ViewBag.LoanType = _loanOtherRepository.LoanTypeList();

            ViewBag.PayBack = _loanOtherRepository.PayBackOtherList();

            return View("CreateLoanOther", loandMaster);
        }


        [HttpGet]
        public IActionResult DeleteLoanOther(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanOtherRepository.loanOtherMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Delete";

                ViewBag.EmpId = _loanOtherRepository.GetEmpOtherList();

                ViewBag.Compound = _loanOtherRepository.CompoundOtherList();

                ViewBag.LoanType = _loanOtherRepository.LoanTypeList();

                ViewBag.PayBack = _loanOtherRepository.PayBackOtherList();

                ViewBag.LoanCalculation = _loanOtherRepository.LoanOtherCalc(id);

                return View("CreateLoanOther", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanOtherRepository.GetEmpOtherList();

            ViewBag.Compound = _loanOtherRepository.CompoundOtherList();

            ViewBag.LoanType = _loanOtherRepository.LoanTypeList();

            ViewBag.PayBack = _loanOtherRepository.PayBackOtherList();

            return View("Create", loandMaster);
        }


        [HttpPost, ActionName("DeleteLoanOther")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteLoanOtherConfirmed(int id)
        {

            try
            {

                var master = _loanOtherRepository.FindById(id);
                _loanOtherRepository.DeleteOtherCycle(id);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, LoanOtherId = master.LoanOtherId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Loan PF
        public IActionResult LoanPFList()
        {
            var loanData = _loanPFRepository.GetLoanPFDataList();
            return View(loanData);
        }
        [HttpGet]
        public IActionResult CreateLoanPF()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";

            ViewData["EmpId"] = _loanPFRepository.GetEmpPFList();

            ViewBag.Compound = _loanPFRepository.CompoundPFList();

            ViewBag.PayBack = _loanPFRepository.PayBackPFList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateLoanPF(HR_Loan_PF hR_Loan_PF, bool newCalculation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string comid = HttpContext.Session.GetString("comid");
                    string userid = HttpContext.Session.GetString("userid");
                    if (hR_Loan_PF.LoanPFId > 0)
                    {
                        var master = _context.HR_Loan_PF.Where(l => l.EmpId == hR_Loan_PF.EmpId && l.LoanType == hR_Loan_PF.LoanType
                                && l.LoanPFId != hR_Loan_PF.LoanPFId && l.ComId == comid).FirstOrDefault();
                        if (master != null)
                        {
                            var unPaidLoan = _loanPFRepository.unPaidPF1(hR_Loan_PF);
                            if (unPaidLoan != null)
                            {
                                TempData["Message"] = "This Employee has unpaide Loan";
                                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                            }
                        }

                    }
                    else
                    {
                        var master = _context.HR_Loan_PF.Where(l => l.EmpId == hR_Loan_PF.EmpId
                                      && l.LoanType == hR_Loan_PF.LoanType && l.ComId == comid).FirstOrDefault();
                        if (master != null)
                        {
                            var unPaidLoan = _loanPFRepository.unPaidPF2(hR_Loan_PF);
                            if (unPaidLoan != null)
                            {
                                TempData["Message"] = "This Employee has unpaide Loan";
                                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                            }
                        }

                        hR_Loan_PF.DtTran = DateTime.Now.Date;
                        _context.Add(hR_Loan_PF);
                    }
                    _context.SaveChanges();

                    TempData["Message"] = "Loan Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        public IActionResult CalcualteLoanPFPartial(decimal lAmount, decimal interest, int period, DateTime startDate,
            decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
        {

            List<CalculateData> loanCal = _loanPFRepository.CalcualteLoanPFPartial(lAmount, interest, period, startDate, ttlLoanAmt, ttlInterest, monthlyLoanAmt, loanType);
            var loanData = loanCal.Select(l => new
            {
                Period = l.PERIOD,
                DtLoanMonth = String.Format("{0:dd-MMM-yyyy}", l.PAYDATE),
                MonthlyLoanAmount = l.PAYMENT,
                InterestAmount = l.INTEREST,
                PrincipalAmount = l.PRINCIPAL,
                BeginningLoanBalance = l.BeginningBalance,
                EndingBalance = l.EndingBalance
            }).ToList();
            //ViewBag.LoanCalculation = loanData;
            //return PartialView("~/Views/LoanPF/_LoanPFGrid.cshtml", new HR_Loan_Data_PF());
            return Json(loanData);
        }

        [HttpGet]
        public IActionResult GetEmpLoanPF(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanPFRepository.loanPFMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanPFRepository.GetEmpPFList();

                ViewBag.Compound = _loanPFRepository.CompoundPFList();

                ViewBag.PayBack = _loanPFRepository.PayBackPFList();

                ViewBag.LoanCalculation = _loanPFRepository.LoanPFCalc(id);

                return View("Create", loandMaster);
            }

            ViewBag.Title = "Create";
            HR_Loan_PF loan = new HR_Loan_PF();

            loan.HR_Emp_Info = _context.HR_Emp_Info
                .Include(l => l.Cat_Department)
                .Include(l => l.Cat_Designation)
                .Include(l => l.HR_Emp_Image)
                .Where(l => l.EmpId == id).FirstOrDefault();

            ViewBag.EmpId = _loanHouseBuilding.GetEmpList(id);

            ViewBag.Compound = _loanPFRepository.CompoundPFList();

            ViewBag.PayBack = _loanPFRepository.PayBackPFList();

            return View("CreateLoanPF", loan);
        }
        [HttpGet]
        public IActionResult EditLoanPF(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanPFRepository.loanPFMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewData["EmpId"] = _loanPFRepository.GetEmpPFList();

                ViewBag.Compound = _loanPFRepository.CompoundPFList();

                ViewBag.PayBack = _loanPFRepository.PayBackPFList();

                ViewBag.LoanCalculation = _loanPFRepository.LoanPFCalc(id);

                return View("CreateLoanPF", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewData["EmpId"] = _loanPFRepository.GetEmpPFList();

            ViewBag.Compound = _loanPFRepository.CompoundPFList();

            ViewBag.PayBack = _loanPFRepository.PayBackPFList();

            return View("CreateLoanPF", loandMaster);
        }


        [HttpGet]
        public IActionResult DeleteLoanPF(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanPFRepository.loanPFMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Delete";


                ViewData["EmpId"] = _loanPFRepository.GetEmpPFList();

                ViewBag.Compound = _loanPFRepository.CompoundPFList();

                ViewBag.PayBack = _loanPFRepository.PayBackPFList();

                ViewBag.LoanCalculation = _loanPFRepository.LoanPFCalc(id);

                return View("CreateLoanPF", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewData["EmpId"] = _loanPFRepository.GetEmpPFList();

            ViewBag.Compound = _loanPFRepository.CompoundPFList();

            ViewBag.PayBack = _loanPFRepository.PayBackPFList();

            return View("CreateLoanPF", loandMaster);
        }


        [HttpPost, ActionName("DeleteLoanPF")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteLoanPFConfirmed(int id)
        {

            try
            {
                var master = _loanPFRepository.FindById(id);
                _loanPFRepository.Delete(master);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, LoanPFId = master.LoanPFId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Loan WF
        public IActionResult LoanWelfareList()
        {
            var loanData = _loanWFRepository.GetLoanWelfareDataList();
            return View(loanData);
        }
        [HttpGet]
        public IActionResult CreateLoanWelfare()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";

            ViewData["EmpId"] = _loanWFRepository.GetEmpWelfareList();

            ViewBag.Compound = _loanWFRepository.CompoundWelfareList();
            ViewBag.PayBack = _loanWFRepository.PayBackWelfareList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateLoanWelfare(HR_Loan_Welfare hR_Loan_Welfare, bool newCalculation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string comid = HttpContext.Session.GetString("comid");
                    string userid = HttpContext.Session.GetString("userid");
                    if (hR_Loan_Welfare.LoanWelId > 0)
                    {
                        hR_Loan_Welfare.UpdateByUserId = comid;
                        hR_Loan_Welfare.DateUpdated = DateTime.Now.Date;
                        _loanWFRepository.Update(hR_Loan_Welfare);
                        _loanWFRepository.SaveLoanWelfare(hR_Loan_Welfare, newCalculation);
                    }
                    else
                    {
                        var unPaidLoan = _loanWFRepository.unPaidWelfare2(hR_Loan_Welfare);

                        if (unPaidLoan != null)
                        {
                            TempData["Message"] = "This Employee has unpaide Loan";
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                        hR_Loan_Welfare.DtTran = DateTime.Now.Date;
                        _context.Add(hR_Loan_Welfare);
                    }
                    _context.SaveChanges();

                    TempData["Message"] = "Loan Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }

        public IActionResult CalcualteLoanWelfarePartial(decimal lAmount, decimal interest, int period, DateTime startDate,
            decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType)
        {

            List<CalculateData> loanCal = _loanWFRepository.CalcualteLoanWelfarePartial(lAmount, interest, period, startDate, ttlLoanAmt, ttlInterest, monthlyLoanAmt, loanType);
            var loanData = loanCal.Select(l => new
            {
                Period = l.PERIOD,
                DtLoanMonth = String.Format("{0:dd-MMM-yyyy}", l.PAYDATE),
                MonthlyLoanAmount = l.PAYMENT,
                InterestAmount = l.INTEREST,
                PrincipalAmount = l.PRINCIPAL,
                BeginningLoanBalance = l.BeginningBalance,
                EndingBalance = l.EndingBalance
            }).ToList();
            //ViewBag.LoanCalculation = loanData;
            //return PartialView("~/Views/LoanWelfare/_LoanWelfareGrid.cshtml", new HR_Loan_Data_Welfare());
            return Json(loanData);
        }

        [HttpGet]
        public IActionResult GetEmpLoanWelfare(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanWFRepository.loanWelfareMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanWFRepository.GetEmpWelfareList();

                ViewBag.Compound = _loanWFRepository.CompoundWelfareList();

                ViewBag.PayBack = _loanWFRepository.PayBackWelfareList();

                ViewBag.LoanCalculation = _loanWFRepository.LoanWelfareCalc(id);

                return View("CreateLoanWelfare", loandMaster);
            }

            ViewBag.Title = "Create";
            HR_Loan_Welfare loan = new HR_Loan_Welfare();

            loan.HR_Emp_Info = _context.HR_Emp_Info
                .Include(l => l.Cat_Department)
                .Include(l => l.Cat_Designation)
                .Include(l => l.HR_Emp_Image)
                .Where(l => l.EmpId == id).FirstOrDefault();

            ViewBag.EmpId = _loanHouseBuilding.GetEmpList(id);

            ViewBag.Compound = _loanWFRepository.CompoundWelfareList();

            ViewBag.PayBack = _loanWFRepository.PayBackWelfareList();

            return View("CreateLoanWelfare", loan);
        }
        [HttpGet]
        public IActionResult EditLoanWelfare(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanWFRepository.loanWelfareMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Edit";

                ViewBag.EmpId = _loanWFRepository.GetEmpWelfareList();

                ViewBag.Compound = _loanWFRepository.CompoundWelfareList();

                ViewBag.PayBack = _loanWFRepository.PayBackWelfareList();

                ViewBag.LoanCalculation = _loanWFRepository.LoanWelfareCalc(id);

                return View("Create", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanWFRepository.GetEmpWelfareList();

            ViewBag.Compound = _loanWFRepository.CompoundWelfareList();

            ViewBag.PayBack = _loanWFRepository.PayBackWelfareList();
            return View("CreateLoanWelfare", loandMaster);
        }


        [HttpGet]
        public IActionResult DeleteLoanWelfare(int id)
        {
            if (id == 0)
                return NotFound();

            var loandMaster = _loanWFRepository.loanWelfareMaster(id);
            string comid = HttpContext.Session.GetString("comid");
            if (loandMaster != null)
            {
                ViewBag.Title = "Delete";

                ViewBag.EmpId = _loanWFRepository.GetEmpWelfareList();

                ViewBag.Compound = _loanWFRepository.CompoundWelfareList();

                ViewBag.PayBack = _loanWFRepository.PayBackWelfareList();

                ViewBag.LoanCalculation = _loanWFRepository.LoanWelfareCalc(id);

                return View("Create", loandMaster);
            }

            ViewBag.Title = "Create";
            ViewBag.EmpId = _loanWFRepository.GetEmpWelfareList();

            ViewBag.Compound = _loanWFRepository.CompoundWelfareList();

            ViewBag.PayBack = _loanWFRepository.PayBackWelfareList();

            return View("CreateLoanWelfare", loandMaster);
        }


        [HttpPost, ActionName("DeleteLoanWelfare")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLoanWelfareConfirmed(int id)
        {

            try
            {

                var master = await _context.HR_Loan_Welfare.Where(l => l.LoanWelId == id).FirstOrDefaultAsync();
                _loanWFRepository.Delete(master);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, LoanWelId = master.LoanWelId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Leave Encashment
        public IActionResult LvEncashmentList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_leaveEncashmentRepository.GetLvEncashments());
        }


        // GET: lvEncashment/Create
        public IActionResult CreateLvEncashment()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            ViewData["EmpId"] = _leaveEncashmentRepository.GetEmpList();

            return View();
        }



        // POST: lvEncashment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateLvEncashment(HR_LvEncashment lvEncashment)
        {
            if (ModelState.IsValid)
            {
                //lvEncashment.UserId = HttpContext.Session.GetString("userid");
                lvEncashment.ComId = HttpContext.Session.GetString("comid");

                string comid = HttpContext.Session.GetString("comid");



                var elBalance = _leaveEncashmentRepository.CreateLvEncashment(lvEncashment);
                if (elBalance != null)
                {

                    var basic = _leaveEncashmentRepository.prcBasic(lvEncashment);

                    if (basic.Count > 0 || basic != null)
                    {
                        var bs = basic.FirstOrDefault().BS;

                        //float round = (float)Math.Round((decimal)((bs * lvEncashment.TotalDays * 12) / 365), 0);
                        float a = (float)(bs * lvEncashment.TotalDays * 12) / 365;
                        lvEncashment.Amount = (float)Math.Round(a, 0);
                        lvEncashment.Stamp = 10;
                        lvEncashment.NetAmount = lvEncashment.Amount - 10;
                    }

                    if (lvEncashment.LvEncashmentId > 0)
                    {
                        if (lvEncashment.IsELEnjoy)
                            elBalance.EL = 0;
                        else
                            elBalance.EL = lvEncashment.ELBalance - lvEncashment.TotalDays;

                        _context.Entry(elBalance).State = EntityState.Modified;

                        lvEncashment.DateUpdated = DateTime.Now;
                        lvEncashment.UpdateByUserId = HttpContext.Session.GetString("userid");
                        lvEncashment.ProssType = GetLvEncashmentProssType(Convert.ToDateTime(lvEncashment.DtInput));
                        _leaveEncashmentRepository.Update(lvEncashment);

                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), lvEncashment.LvEncashmentId.ToString(), "Update", lvEncashment.LvEncashmentId.ToString());

                    }
                    else
                    {

                        if (lvEncashment.IsELEnjoy)
                            elBalance.EL = 0;
                        else
                            elBalance.EL = elBalance.EL - lvEncashment.TotalDays;



                        _context.Entry(elBalance).State = EntityState.Modified;

                        lvEncashment.UserId = HttpContext.Session.GetString("userid");
                        lvEncashment.DateAdded = DateTime.Now;
                        lvEncashment.ProssType = GetLvEncashmentProssType(Convert.ToDateTime(lvEncashment.DtInput));
                        _leaveEncashmentRepository.Add(lvEncashment);

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), lvEncashment.LvEncashmentId.ToString(), "Create", lvEncashment.LvEncashmentId.ToString());

                    }
                    return RedirectToAction(nameof(Index));
                }
                TempData["Message"] = "The Employee has not EL Balance";
                TempData["Status"] = "2";

            }
            return View(lvEncashment);
        }

        string GetLvEncashmentProssType(DateTime date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            return date.ToString("MMMM") + "-" + date.Year.ToString();
        }

        // GET: lvEncashment/Edit/5
        public IActionResult EditLvEncashment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            var lvEncashment = _leaveEncashmentRepository.FindById(id);

            ViewData["EmpId"] = _leaveEncashmentRepository.GetEmpList();

            if (lvEncashment == null)
            {
                return NotFound();
            }
            var elBalance = _leaveEncashmentRepository.CreateLvEncashment(lvEncashment);

            if (elBalance != null)
            {
                if (lvEncashment.IsELEnjoy)// current and previous add
                {
                    lvEncashment.ELBalance = (lvEncashment.TotalDays * 2) + (int)elBalance.EL;
                }
                else
                {
                    lvEncashment.ELBalance = lvEncashment.TotalDays + (int)elBalance.EL;
                }

            }
            else
            {
                lvEncashment.ELBalance = 0;
            }

            return View("CreateLvEncashment", lvEncashment);
        }

        // GET: lvEncashment/Delete/5
        public IActionResult DeleteLvEncashment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string comid = HttpContext.Session.GetString("comid");
            var lvEncashment = _leaveEncashmentRepository.FindById(id);
            ViewData["EmpId"] = _leaveEncashmentRepository.GetEmpList();
            if (lvEncashment == null)
            {
                return NotFound();
            }
            var elBalance = _leaveEncashmentRepository.CreateLvEncashment(lvEncashment);

            if (elBalance != null)
            {
                if (lvEncashment.IsELEnjoy)// current and previous add
                {
                    lvEncashment.ELBalance = (lvEncashment.TotalDays * 2) + (int)elBalance.EL;
                }
                else
                {
                    lvEncashment.ELBalance = lvEncashment.TotalDays + (int)elBalance.EL;
                }
            }
            else
            {
                lvEncashment.ELBalance = 0;
            }

            ViewBag.Title = "Delete";
            //ViewBag.DeptId = new SelectList(db.HR_LvEncashment, "DeptId", "DeptName", lvEncashment.ParentId);
            return View("CreateLvEncashment", lvEncashment);
        }

        // POST: lvEncashment/Delete/5
        [HttpPost, ActionName("DeleteLvEncashment")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteLvEncashmentConfirmed(int id)
        {
            try
            {
                var lvEncashment = _leaveEncashmentRepository.FindById(id);
                var elBalance = _leaveEncashmentRepository.CreateLvEncashment(lvEncashment);
                if (elBalance != null)
                {
                    if (lvEncashment.IsELEnjoy)// current and previous add
                    {
                        elBalance.EL = elBalance.EL + (lvEncashment.TotalDays * 2);
                    }
                    else
                    {
                        elBalance.EL = elBalance.EL + lvEncashment.TotalDays;
                    }
                }
                _context.Entry(elBalance).State = EntityState.Modified;
                _leaveEncashmentRepository.Delete(lvEncashment);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), lvEncashment.LvEncashmentId.ToString(), "Delete", lvEncashment.LvEncashmentId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, LvEncashmentId = lvEncashment.LvEncashmentId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        //[HttpGet]
        //public IActionResult GetLeaveBalance(int empId, int year)
        //{
        //    var data = _leaveEncashmentRepository.GetLeaveBalance(empId, year);
        //    return Json(data);
        //}
        [HttpGet]
        public IActionResult GetLeaveBalance(int empId, int year)
        {
            string comid = HttpContext.Session.GetString("comid");
            var data = _context.HR_Leave_Balance
                .Where(l => l.DtOpeningBalance == year && l.ComId == comid).FirstOrDefault();
            return Json(data);
        }


        #endregion

        #region Increment
        public async Task<IActionResult> IncrementList()
        {
            string comid = HttpContext.Session.GetString("comid");

            string userId = HttpContext.Session.GetString("userid");
            ViewBag.increment = new List<HR_Emp_Increment>();
            ViewBag.NewDeptId = _departmentRepository.GetDepartmentList();
            ViewBag.OldDeptId = _departmentRepository.GetDepartmentList();
            ViewBag.NewDesigId = _designationRepository.GetDesignationList();
            ViewBag.OldDesigId = _designationRepository.GetDesignationList();
            ViewBag.NewSectId = _sectionRepository.GetSectionList();
            ViewBag.OldSectId = _sectionRepository.GetSectionList();
            ViewBag.NewUnitId = _cat_UnitRepository.GetCat_UnitList();
            ViewBag.OldUnitId = _cat_UnitRepository.GetCat_UnitList();
            ViewBag.NewGradeId = _gradeRepository.GradeSelectList();
            ViewBag.OldGradeId = _gradeRepository.GradeSelectList();
            ViewBag.EmpId = _incrementRepository.GetEmpList1(comid, userId);
            //ViewData["EmpId"] = _incrementRepository.GetEmpList2();
            ViewBag.IncTypeId = _incrementRepository.IncTypeList();
            ViewBag.OldEmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.NewEmpTypeId = _empTypeRepository.GetEmpTypeList();

            var DollerData = _context.Companys.Where(x => x.CompanyCode == comid && x.IsDoller == true).FirstOrDefault();



            if (DollerData == null)
            {
                ViewBag.IsDoller = 0;
            }
            else
            {
                ViewBag.IsDoller = 1;
            }
            return View();
        }
        #endregion

        #region Transfer
        public async Task<IActionResult> TransferList()
        {
            string comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            ViewBag.transfer = new List<HR_Emp_Increment>();
            ViewBag.NewDeptId = _departmentRepository.GetDepartmentList();
            ViewBag.OldDeptId = _departmentRepository.GetDepartmentList();
            ViewBag.NewDesigId = _designationRepository.GetDesignationList();
            ViewBag.OldDesigId = _designationRepository.GetDesignationList();
            ViewBag.NewSectId = _sectionRepository.GetSectionList();
            ViewBag.OldSectId = _sectionRepository.GetSectionList();
            ViewBag.NewUnitId = _cat_UnitRepository.GetCat_UnitList();
            ViewBag.OldUnitId = _cat_UnitRepository.GetCat_UnitList();
            ViewBag.EmpId = _incrementRepository.GetEmpList1(comid, userId);
            ViewData["EmpId"] = _incrementRepository.GetEmpList2();
            //ViewBag.IncTypeId = _TransferRepository.IncTypeList();
            ViewBag.IncTypeId = _TransferRepository.IncTypeList();
            ViewBag.OldEmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.NewEmpTypeId = _empTypeRepository.GetEmpTypeList();
            return View();
        }



        [HttpPost]
        public JsonResult GetEmployeeInfo(int empid)
        {
            var comid = HttpContext.Session.GetString("comid");
            var dtincrement = DateTime.Now.ToString("dd-MMM-yyyy");
            var date = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1).ToString("dd-MMM-yyyy");
            var EmpInfo = _context.HR_Emp_Info.Select(e => new
            {
                e.EmpId,
                e.EmpCode,
                e.EmpName,
                DtJoin = Convert.ToDateTime(e.DtJoin).ToString("dd-MMM-yyyy"),
                DesigName = e.Cat_Designation.DesigName,
                DeptName = e.Cat_Department.DeptName,
                SectName = e.Cat_Section.SectName,
                ShiftName = e.Cat_Shift.ShiftName,
                UnitName = e.Cat_Unit.UnitName,
                DtConfirm = Convert.ToDateTime(e.DtConfirm).ToString("dd-MMM-yyyy"),
                EmpTypeName = e.Cat_Emp_Type.EmpTypeName,
                GradeName = e.Cat_Grade.GradeName,
                DesigNameNew = e.Cat_Designation.DesigName,
                DeptNameNew = e.Cat_Department.DeptName,
                LineName = e.Cat_Line.LineName,
                FloorName = e.Cat_Floor.FloorName,
                FingerId = e.FingerId,
                SectNameNew = e.Cat_Section.SectName,
                EmpTypeNameNew = e.Cat_Emp_Type.EmpTypeName,
                DesigNameOld = e.HR_Emp_Increment.Cat_DesignationOld.DesigName,
                DeptNameOld = e.HR_Emp_Increment.Cat_DepartmentOld.DeptName,
                SectNameOld = e.HR_Emp_Increment.Cat_SectionOld.SectName,
                EmpTypeNameOld = e.HR_Emp_Increment.Cat_Emp_TypeOld.EmpTypeName,
                DesigId = e.DesigId,
                SectId = e.SectId,
                EmpTypeId = e.EmpTypeId,
                GradeId = e.GradeId,
                e.ComId,
                // DtIncrement = (Convert.ToDateTime(e.DtIncrement).ToString("dd-MMM-yyyy") == "01-Jan-0001") ? firstDayOfMonth : Convert.ToDateTime(e.DtIncrement).ToString("dd-MMM-yyyy"),
                DtIncrement = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MMM-yyyy"),
                IncTypeId = e.HR_Emp_Increment.IncTypeId,
                DeptID = e.DeptId

            }).Where(e => e.EmpId == empid && e.ComId == comid).ToList();

            var SalaryInfo = _context.HR_Emp_Salary.Select(s => new
            {
                s.PersonalPay,
                s.BasicSalary,
                s.HouseRent,
                s.HrExp,
                s.HRExpensesOther,
                s.CanteenAllow,
                s.MadicalAllow,
                s.ConveyanceAllow,
                s.EmpId
            }).Where(s => s.EmpId == empid).ToList();

            return Json(new { EmpInfo, SalaryInfo });
        }

        [HttpPost]  // Get Salary info for auto calculation
        public IActionResult GetSalaryInfo(int empId, float BS)
        {
            var comid = HttpContext.Session.GetString("comid");
            var empTypeId = _context.HR_Emp_Info.Find(empId).EmpTypeId;
            var empSalary = _context.HR_Emp_Salary.Where(s => s.EmpId == empId).FirstOrDefault();

            Cat_HRSetting hr = null;
            Cat_HRExpSetting hrExp = null;

            if (empSalary.BId == 11)
            {
                if (empTypeId != null)
                {
                    hr = _context.Cat_HRSetting
                            .Include(x => x.Cat_Emp_Type)
                            .Where(x => x.CompanyCode == comid && x.Cat_Emp_Type.EmpTypeId == empTypeId).FirstOrDefault();



                    //_context.Cat_HRSetting
                    //.Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId2 && h.BS <= BS && h.BId == empSalary.BId)
                    //.OrderByDescending(h => h.BS).FirstOrDefault();                    //.ToList();
                    hrExp = _context.Cat_HRExpSetting
                       .Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId2 && h.BId == empSalary.BId && h.BS <= BS)
                       .OrderByDescending(h => h.BS).FirstOrDefault();
                }

            }
            else
            {
                if (empTypeId != null && empSalary != null)
                {
                    hr = _context.Cat_HRSetting
                            .Include(x => x.Cat_Emp_Type)
                            .Where(x => x.CompanyCode == comid && x.Cat_Emp_Type.EmpTypeId == empTypeId).FirstOrDefault();



                    //_context.Cat_HRSetting
                    //.Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId1 && h.BS <= BS && h.BId == empSalary.BId)
                    //.OrderByDescending(h => h.BS).FirstOrDefault();                    //.ToList();
                    hrExp = _context.Cat_HRExpSetting
                       .Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId1 && h.BId == empSalary.BId && h.BS <= BS)
                       .OrderByDescending(h => h.BS).FirstOrDefault();
                }
            }


            return Json(new { HR = hr, HRExp = hrExp, EmpSalary = empSalary });
        }


        [HttpPost]
        public JsonResult LoadIncrementGrid(int empid)
        {
            var comid = HttpContext.Session.GetString("comid");

            var increment = _context.HR_Emp_Increment
                .Include(i => i.HR_IncType)
                .Where(n => !n.HR_IncType.IncType.Contains("Transfer"))
                .Include(i => i.HR_Emp_Info)
                .Where(i => i.EmpId == empid && i.ComId == comid)
                .Select(ei => new
                {
                    IncrementId = ei.IncId,
                    EmpCode = ei.HR_Emp_Info.EmpCode,
                    IncrementType = ei.HR_IncType.IncType,
                    DtIncrement = Convert.ToDateTime(ei.DtIncrement).ToString("dd-MMM-yyyy"),
                    Amount = ei.Amount,
                    ei.NewBS
                }).ToList().OrderBy(i => i.IncrementId);
            return Json(increment);
        }

        public JsonResult LoadTransferGrid(int empid)
        {
            var comid = HttpContext.Session.GetString("comid");
            var increment = _context.HR_Emp_Increment
                .Include(i => i.HR_IncType)
                .Where(n => n.HR_IncType.IncType.Contains("Transfer"))
                .Include(i => i.HR_Emp_Info)
                .Where(i => i.EmpId == empid && i.ComId == comid)
                .Select(ei => new
                {
                    IncrementId = ei.IncId,
                    EmpCode = ei.HR_Emp_Info.EmpCode,
                    EmpName = ei.HR_Emp_Info.EmpName,
                    IncrementType = ei.HR_IncType.IncType,
                    DtIncrement = Convert.ToDateTime(ei.DtIncrement).ToString("dd-MMM-yyyy"),
                    OldDesigId = ei.Cat_DesignationOld.DesigName,
                    NewDesigId = ei.Cat_DesignationNew.DesigName,
                    OldDeptId = ei.Cat_DepartmentOld.DeptName,
                    NewDeptId = ei.Cat_DepartmentNew.DeptName,
                    OldSectId = ei.Cat_SectionOld.SectName,
                    NewSectId = ei.Cat_SectionNew.SectName,
                    OldEmpTypeId = ei.Cat_Emp_TypeOld.EmpTypeName,
                    NewEmpTypeId = ei.Cat_Emp_TypeNew.EmpTypeName

                }).ToList().OrderBy(i => i.IncrementId);
            return Json(increment);
        }

        [HttpPost]
        public ActionResult SaveIncrementInfo(HR_Emp_Increment hR_Emp_Increment)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");

                hR_Emp_Increment.UserId = userid;

                hR_Emp_Increment.ComId = comid;

                hR_Emp_Increment.DtInput = DateTime.Now;

                //Update Information
                var newdesigid = Convert.ToInt32(hR_Emp_Increment.NewDesigId);
                var newsectid = Convert.ToInt32(hR_Emp_Increment.NewSectId);
                var newemptypeid = Convert.ToInt32(hR_Emp_Increment.NewEmpTypeId);
                var newgradeid = Convert.ToInt32(hR_Emp_Increment.NewGradeId);

                HR_Emp_Salary salary = _context.HR_Emp_Salary.Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == hR_Emp_Increment.ComId).FirstOrDefault();
                hR_Emp_Increment.OldSalary = hR_Emp_Increment.OldSalary; //+ salary.HouseRent + salary.MadicalAllow + salary.ConveyanceAllow + salary.FoodAllow;
                hR_Emp_Increment.NewSalary = (hR_Emp_Increment.NewSalary);// + hR_Emp_Increment.NewHR + hR_Emp_Increment.NewMA + hR_Emp_Increment.NewTA + hR_Emp_Increment.NewFA );

                if (hR_Emp_Increment.NewSalary == 0)
                {
                    hR_Emp_Increment.NewSalary = hR_Emp_Increment.OldSalary;
                }

                if (hR_Emp_Increment.IncId > 0)
                {
                    hR_Emp_Increment.BSDiff = (float)(hR_Emp_Increment.NewBS - hR_Emp_Increment.OldBS);
                    hR_Emp_Increment.HRDiff = (float)(hR_Emp_Increment.NewHR - hR_Emp_Increment.OldHR);
                    hR_Emp_Increment.HRExpDiff = (float)(hR_Emp_Increment.NewHRExp - hR_Emp_Increment.OldHRExp);
                    hR_Emp_Increment.HRExpOtherDiff = (float)(hR_Emp_Increment.NewHRExpOther - hR_Emp_Increment.OldHRExpOther);
                    hR_Emp_Increment.MADiff = (float)(hR_Emp_Increment.NewMA - hR_Emp_Increment.OldMA);

                    //var oldsalary = hR_Emp_Increment.OldSalary;
                    //var newsalary = hR_Emp_Increment.NewSalary;

                    //hR_Emp_Increment.Amount = (newsalary - oldsalary);

                    //var incamount = hR_Emp_Increment.Amount;

                    //hR_Emp_Increment.Percentage = ((float)((incamount / oldsalary) * 100));
                    HR_Emp_Salary empsal = _context.HR_Emp_Salary
                        .Include(e => e.HR_Emp_Info)
                        .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();
                    if (empsal != null)
                    {
                        empsal.PersonalPay = (float)hR_Emp_Increment.NewSalary;
                        empsal.BasicSalary = (float)hR_Emp_Increment.NewBS;
                        empsal.HouseRent = (float)hR_Emp_Increment.NewHR;
                        empsal.HrExp = (float)hR_Emp_Increment.NewHRExp;
                        empsal.HRExpensesOther = (float)hR_Emp_Increment.NewHRExpOther;
                        empsal.MadicalAllow = (float)hR_Emp_Increment.NewMA;
                        empsal.CanteenAllow = (float)hR_Emp_Increment.NewFA;
                        empsal.ConveyanceAllow = (float)hR_Emp_Increment.NewTA;

                        if (newdesigid > 0)
                        {
                            empsal.HR_Emp_Info.DesigId = newdesigid;
                        }
                        if (newsectid > 0)
                        {
                            empsal.HR_Emp_Info.SectId = newsectid;
                        }
                        if (newemptypeid > 0)
                        {
                            empsal.HR_Emp_Info.EmpTypeId = newemptypeid;
                        }
                        if (newgradeid > 0)
                        {
                            empsal.HR_Emp_Info.GradeId = newgradeid;
                        }
                        empsal.HR_Emp_Info.DtIncrement = hR_Emp_Increment.DtIncrement;
                        _context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                        _context.Entry(empsal).State = EntityState.Modified;
                    }

                    hR_Emp_Increment.UpdateByUserId = userid;
                    hR_Emp_Increment.DateUpdated = DateTime.Now;

                    _context.Entry(hR_Emp_Increment).State = EntityState.Modified;
                }
                else
                {
                    var check = _context.HR_Emp_Increment.Where(i => i.EmpId == hR_Emp_Increment.EmpId
                            && i.DtIncrement == hR_Emp_Increment.DtIncrement
                            && i.IncTypeId == hR_Emp_Increment.IncTypeId).FirstOrDefault();

                    if (check != null)
                    {
                        return Json(new { Success = 2, ex = "The employee has Incremen or Promotion this date" });
                    }
                    hR_Emp_Increment.BSDiff = (float)(hR_Emp_Increment.NewBS - hR_Emp_Increment.OldBS);
                    hR_Emp_Increment.HRDiff = (float)(hR_Emp_Increment.NewHR - hR_Emp_Increment.OldHR);
                    hR_Emp_Increment.HRExpDiff = (float)(hR_Emp_Increment.NewHRExp - hR_Emp_Increment.OldHRExp);
                    hR_Emp_Increment.HRExpOtherDiff = (float)(hR_Emp_Increment.NewHRExpOther - hR_Emp_Increment.OldHRExpOther);
                    hR_Emp_Increment.MADiff = (float)(hR_Emp_Increment.NewMA - hR_Emp_Increment.OldMA);


                    var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1176 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
                    if (approveData == null)
                    {
                        hR_Emp_Increment.IsApprove = true;
                    }
                    else if (approveData.IsApprove == true)
                    {
                        hR_Emp_Increment.IsApprove = false;
                    }
                    else
                    {
                        hR_Emp_Increment.IsApprove = true;
                    }

                    HR_Emp_Salary empsal = _context.HR_Emp_Salary
                            .Include(e => e.HR_Emp_Info)
                            .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();
                    if (newdesigid > 0)
                    {
                        empsal.HR_Emp_Info.DesigId = newdesigid;
                    }
                    if (newgradeid > 0)
                    {
                        empsal.HR_Emp_Info.GradeId = newgradeid;
                    }
                    if (newsectid > 0)
                    {
                        empsal.HR_Emp_Info.SectId = newsectid;
                    }
                    if (newemptypeid > 0)
                    {
                        empsal.HR_Emp_Info.EmpTypeId = newemptypeid;
                    }
                    _context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                    #region Salary Update
                    if (hR_Emp_Increment.IsApprove == true)
                    {

                        if (empsal != null)
                        {
                            empsal.PersonalPay = (float)hR_Emp_Increment.NewSalary;
                            empsal.BasicSalary = (float)hR_Emp_Increment.NewBS;
                            empsal.HouseRent = (float)hR_Emp_Increment.NewHR;
                            empsal.HrExp = (float)hR_Emp_Increment.NewHRExp;
                            empsal.HRExpensesOther = (float)hR_Emp_Increment.NewHRExpOther;
                            empsal.MadicalAllow = (float)hR_Emp_Increment.NewMA;
                            empsal.CanteenAllow = (float)hR_Emp_Increment.NewFA;
                            empsal.ConveyanceAllow = (float)hR_Emp_Increment.NewTA;

                            if (newdesigid > 0)
                            {
                                empsal.HR_Emp_Info.DesigId = newdesigid;
                            }
                            if (newsectid > 0)
                            {
                                empsal.HR_Emp_Info.SectId = newsectid;
                            }
                            if (newemptypeid > 0)
                            {
                                empsal.HR_Emp_Info.EmpTypeId = newemptypeid;
                            }
                            if (newgradeid > 0)
                            {
                                empsal.HR_Emp_Info.GradeId = newgradeid;
                            }

                            empsal.HR_Emp_Info.DtIncrement = hR_Emp_Increment.DtIncrement;
                            _context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                            _context.Entry(empsal).State = EntityState.Modified;
                            // db.SaveChanges();
                        }

                    }


                    #endregion

                    hR_Emp_Increment.DateAdded = DateTime.Now;
                    _context.Add(hR_Emp_Increment);
                }
                _context.SaveChanges();
                TempData["Message"] = "Data Saved / Update Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
            }

        }
        [HttpPost]
        public ActionResult SaveTransferInfo(HR_Emp_Increment hR_Emp_Increment)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");

                hR_Emp_Increment.UserId = userid;

                hR_Emp_Increment.ComId = comid;

                hR_Emp_Increment.DtInput = DateTime.Now;
                if (hR_Emp_Increment.IncId > 0)
                {



                    HR_Emp_Info empsal = _context.HR_Emp_Info
                        //.Include(e => e.HR_Emp_Info)
                        .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();
                    if (empsal != null)
                    {

                        var newdesigid = Convert.ToInt32(hR_Emp_Increment.NewDesigId);
                        if (newdesigid > 0)
                        {
                            empsal.DesigId = newdesigid;
                        }
                        var newdeptid = Convert.ToInt32(hR_Emp_Increment.NewDeptId);
                        if (newdeptid > 0)
                        {
                            empsal.DeptId = newdeptid;
                        }

                        var newsectid = Convert.ToInt32(hR_Emp_Increment.NewSectId);
                        if (newsectid > 0)
                        {
                            empsal.SectId = newsectid;
                        }
                        var newemptypeid = Convert.ToInt32(hR_Emp_Increment.NewEmpTypeId);
                        if (newemptypeid > 0)
                        {
                            empsal.EmpTypeId = newemptypeid;
                        }
                        empsal.DtIncrement = hR_Emp_Increment.DtIncrement;
                        _context.Entry(empsal).State = EntityState.Modified;
                        //_context.Entry(empsal).State = EntityState.Modified;
                    }

                    hR_Emp_Increment.UpdateByUserId = userid;
                    hR_Emp_Increment.DateUpdated = DateTime.Now;

                    _context.Entry(hR_Emp_Increment).State = EntityState.Modified;
                }
                else
                {


                    var check = _context.HR_Emp_Increment.Where(i => i.EmpId == hR_Emp_Increment.EmpId
                            && i.DtIncrement == hR_Emp_Increment.DtIncrement
                            && i.IncTypeId == hR_Emp_Increment.IncTypeId).FirstOrDefault();

                    if (check != null)
                    {
                        return Json(new { Success = 2, ex = "The employee has Transfer this date" });
                    }

                    #region Salary Update
                    // if (hR_Emp_Increment.IsApprove == true)
                    //{
                    HR_Emp_Info empsal = _context.HR_Emp_Info
                       .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();

                    //HR_Emp_Salary empsal = _context.HR_Emp_Salary
                    //    .Include(e => e.HR_Emp_Info)
                    //    .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();
                    if (empsal != null)
                    {

                        var newdesigid = Convert.ToInt32(hR_Emp_Increment.NewDesigId);
                        if (newdesigid > 0)
                        {
                            empsal.DesigId = newdesigid;
                        }
                        var newdeptid = Convert.ToInt32(hR_Emp_Increment.NewDeptId);
                        if (newdeptid > 0)
                        {
                            empsal.DeptId = newdeptid;
                        }

                        var newsectid = Convert.ToInt32(hR_Emp_Increment.NewSectId);
                        if (newsectid > 0)
                        {
                            empsal.SectId = newsectid;
                        }

                        var newemptypeid = Convert.ToInt32(hR_Emp_Increment.NewEmpTypeId);
                        if (newemptypeid > 0)
                        {
                            empsal.EmpTypeId = newemptypeid;
                        }

                        empsal.DtIncrement = hR_Emp_Increment.DtIncrement;
                        _context.Entry(empsal).State = EntityState.Modified;

                        // db.SaveChanges();
                    }

                    // }

                    #endregion

                    //// for isapprove field
                    //var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1176 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
                    //if (approveData == null)
                    //{
                    //    hR_Emp_Increment.IsApprove = true;
                    //}
                    //else if (approveData.IsApprove == true)
                    //{
                    //    hR_Emp_Increment.IsApprove = false;
                    //}
                    //else
                    //{
                    //    hR_Emp_Increment.IsApprove = true;
                    //}
                    hR_Emp_Increment.DateAdded = DateTime.Now;
                    _context.Add(hR_Emp_Increment);
                }
                _context.SaveChanges();
                TempData["Message"] = "Data Saved / Update Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
            }

        }

        #region Increment with Excel File Upload
        public async Task<IActionResult> IncrementUploadFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/Increment/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/Increment/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetIncrementExcel(file.FileName);
                    if (addition.Count() > 0)
                    {
                        await _context.HR_TempIncrementExcel.AddRangeAsync(addition);
                        await _context.SaveChangesAsync();

                        var Query = $"Exec HR_prcProcessIncrementExcel '{comid}'";
                        SqlParameter[] sqlParameter = new SqlParameter[1];
                        sqlParameter[0] = new SqlParameter("@ComId", comid);
                        Helper.ExecProc("HR_prcProcessIncrementExcel", sqlParameter);

                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("IncrementList");
        }

        private List<HR_TempIncrementExcel> GetIncrementExcel(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/Increment/" + comid + "/" + userid + "/" + fName);

            List<HR_TempIncrementExcel> increment = new List<HR_TempIncrementExcel>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                increment.Add(new HR_TempIncrementExcel()
                                {
                                    ComId = HttpContext.Session.GetString("comid"),
                                    EmpCode = reader.GetValue(1).ToString(),
                                    EmpName = reader.GetValue(2).ToString(),
                                    DtJoin = DateTime.Parse(reader.GetValue(3).ToString()),
                                    OldSalary = float.Parse(reader.GetValue(4).ToString()),
                                    Amount = float.Parse(reader.GetValue(5).ToString()),
                                    NewSalary = float.Parse(reader.GetValue(6).ToString()),
                                    NewBS = float.Parse(reader.GetValue(7).ToString()),
                                    NewHR = float.Parse(reader.GetValue(8).ToString()),
                                    NewMA = float.Parse(reader.GetValue(9).ToString()),
                                    NewFA = float.Parse(reader.GetValue(10).ToString()),
                                    NewTA = float.Parse(reader.GetValue(11).ToString()),
                                    NewDeptName = reader.GetValue(12).ToString(),
                                    NewSectName = reader.GetValue(13).ToString(),
                                    NewDesigName = reader.GetValue(14).ToString(),
                                    NewGradeName = reader.GetValue(15).ToString(),
                                    DtIncrement = DateTime.Parse(reader.GetValue(16).ToString()),
                                    IncType = reader.GetValue(17).ToString()
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return increment;
        }

        public ActionResult DownloadIncrementFile(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
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

        #endregion


        public ActionResult GetIncrementId(int incId)
        {
            var inc = _context.HR_Emp_Increment.Where(i => i.IncId == incId);
            _context.Remove(inc);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetIncrementInfo(int IncId)
        {
            var IncrementInfo = _context.HR_Emp_Increment.Select(i => new
            {
                IncId = i.IncId,
                EmpCode = i.HR_Emp_Info.EmpCode,
                EmpName = i.HR_Emp_Info.EmpName,
                SectName = i.HR_Emp_Info.Cat_Section.SectName,
                DeptName = i.Cat_DepartmentOld.DeptName,
                DesigName = i.HR_Emp_Info.Cat_Designation.DesigName,
                GradeNameOld = i.Cat_GradeOld.GradeName,
                DesigNameOld = i.Cat_DesignationOld.DesigName,
                SectNameOld = i.Cat_SectionOld.SectName,
                EmpTypeNameOld = i.Cat_Emp_TypeOld.EmpTypeName,
                OldEmpTypeId = i.OldEmpTypeId,
                NewEmpTypeId = i.NewEmpTypeId,
                NewGradeId = i.NewGradeId,
                OldGradeId = i.OldGradeId,
                IncType = i.HR_IncType.IncType,
                DtIncrement = Convert.ToDateTime(i.DtIncrement).ToString("dd-MMM-yyyy"),
                DtInput = i.DtInput,
                IncTypeId = i.IncTypeId,
                NewBS = i.NewBS,
                OldBS = i.OldBS,
                OldHR = i.OldHR,
                OldHRExp = i.OldHRExp,
                OldHRExpOther = i.OldHRExpOther,
                OldMA = i.OldMA,
                OldTA = i.OldTA,
                OldFA = i.OldFA,
                Amount = i.Amount,
                Percentage = i.Percentage,
                OldSalary = i.OldSalary,
                NewSalary = i.NewSalary,
                OldPA = i.OldPA,
                NewPA = i.NewPA,
                OldDA = i.OldDA,
                NewDA = i.NewDA,
                NewHR = i.NewHR,
                NewHRExp = i.NewHRExp,
                NewHRExpOther = i.NewHRExpOther,
                NewMA = i.NewMA,
                NewFA = i.NewFA,
                NewTA = i.NewTA
            }).Where(i => i.IncId == IncId);
            return Json(IncrementInfo);
        }

        //New Nurjahan
        public JsonResult GetTransferInfo(int IncId)
        {
            var IncrementInfo = _context.HR_Emp_Increment.Select(i => new
            {
                IncId = i.IncId,
                EmpCode = i.HR_Emp_Info.EmpCode,
                EmpName = i.HR_Emp_Info.EmpName,
                SectName = i.HR_Emp_Info.Cat_Section.SectName,
                DeptName = i.HR_Emp_Info.Cat_Department.DeptName,
                DesigName = i.HR_Emp_Info.Cat_Designation.DesigName,

                DesigNameOld = i.Cat_DesignationOld.DesigName,
                DeptNameOld = i.Cat_DepartmentOld.DeptName,
                SectNameOld = i.Cat_SectionOld.SectName,
                EmpTypeNameOld = i.Cat_Emp_TypeOld.EmpTypeName,

                OldEmpTypeId = i.OldEmpTypeId,
                NewEmpTypeId = i.NewEmpTypeId,
                OldSecctId = i.OldSectId,
                NewSectId = i.NewSectId,
                //?New Add
                OldDesigId = i.OldDesigId,
                NewDesigId = i.NewDesigId,
                OldDeptId = i.OldDeptId,
                NewDeptId = i.NewDeptId,
                IncType = i.HR_IncType.IncType,
                DtIncrement = Convert.ToDateTime(i.DtIncrement).ToString("dd-MMM-yyyy"),
                DtInput = i.DtInput,
                IncTypeId = i.IncTypeId
            }).Where(i => i.IncId == IncId);
            return Json(IncrementInfo);
        }

        [HttpPost, ActionName("DeleteTransfer")]
        public async Task<IActionResult> DeleteTransferConfirmed(int id)
        {
            string comid = HttpContext.Session.GetString("comid");
            var hR_Emp_Increment = await _context.HR_Emp_Increment.FindAsync(id);

            HR_Emp_Info empsal = await _context.HR_Emp_Info
                        //.Include(e => e.HR_Emp_Info)
                        .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefaultAsync();
            if (empsal != null)
            {

                var oldDesigId = Convert.ToInt32(hR_Emp_Increment.OldDesigId);
                if (oldDesigId > 0)
                {
                    empsal.DesigId = oldDesigId;
                }
                var oldDeptId = Convert.ToInt32(hR_Emp_Increment.OldDeptId);
                if (oldDeptId > 0)
                {
                    empsal.DeptId = oldDeptId;
                }


                var oldSectId = Convert.ToInt32(hR_Emp_Increment.OldSectId);
                if (oldSectId > 0)
                {
                    empsal.SectId = oldSectId;
                }

                var oldEmpTypeId = Convert.ToInt32(hR_Emp_Increment.OldEmpTypeId);
                if (oldEmpTypeId > 0)
                {
                    empsal.EmpTypeId = oldEmpTypeId;
                }

                empsal.DtIncrement = hR_Emp_Increment.DtIncrement;
                _context.Entry(empsal).State = EntityState.Modified;
                //_context.Entry(empsal).State = EntityState.Modified;
                //_context.SaveChanges();
            }

            _context.HR_Emp_Increment.Remove(hR_Emp_Increment);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Data Delete Successfully";
            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
        }


        [HttpPost, ActionName("DeleteIncrement")]
        public async Task<IActionResult> DeleteIncrementConfirmed(int id)
        {
            string comid = HttpContext.Session.GetString("comid");
            var hR_Emp_Increment = await _context.HR_Emp_Increment.FindAsync(id);

            HR_Emp_Salary empsal = await _context.HR_Emp_Salary
                        .Include(e => e.HR_Emp_Info)
                        .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefaultAsync();
            if (empsal != null)
            {
                empsal.BasicSalary = (float)hR_Emp_Increment.OldBS;
                empsal.PersonalPay = (float)hR_Emp_Increment.OldSalary;
                empsal.HouseRent = (float)hR_Emp_Increment.OldHR;
                //empsal.HrExp = (float)hR_Emp_Increment.OldHRExp;
                //empsal.HRExpensesOther = (float)hR_Emp_Increment.OldHRExpOther;
                empsal.MadicalAllow = (float)hR_Emp_Increment.OldMA;
                empsal.CanteenAllow = (float)hR_Emp_Increment.OldFA;
                empsal.ConveyanceAllow = (float)hR_Emp_Increment.OldTA;
                var oldDesigId = Convert.ToInt32(hR_Emp_Increment.OldDesigId);
                if (oldDesigId > 0)
                {
                    empsal.HR_Emp_Info.DesigId = oldDesigId;
                }

                var oldSectId = Convert.ToInt32(hR_Emp_Increment.OldSectId);
                if (oldSectId > 0)
                {
                    empsal.HR_Emp_Info.SectId = oldSectId;
                }

                var oldEmpTypeId = Convert.ToInt32(hR_Emp_Increment.OldEmpTypeId);
                if (oldEmpTypeId > 0)
                {
                    empsal.HR_Emp_Info.EmpTypeId = oldEmpTypeId;
                }

                empsal.HR_Emp_Info.DtIncrement = hR_Emp_Increment.DtIncrement;
                _context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                _context.Entry(empsal).State = EntityState.Modified;
                //db.SaveChanges();
            }

            _context.HR_Emp_Increment.Remove(hR_Emp_Increment);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Data Delete Successfully";
            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
        }

        #endregion

        #region Raw Data Import
        public IActionResult UploadFiles()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {

            try
            {
                if (file != null)
                {

                    var comid = HttpContext.Session.GetString("comid");
                    var userid = HttpContext.Session.GetString("userid");
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/Upload/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    string path = filePath;
                    string[] readText = System.IO.File.ReadAllLines(path);

                    //var DeviceNo = readText[0][1];
                    List<string> newList = readText.ToList();
                    List<Hr_RawData> hr_RawDataList = new List<Hr_RawData>();
                    foreach (var item in newList)
                    {

                        var data = item.Split(':');
                        var deviceNo = data[0];
                        var cardNo = data[1];
                        var punchDate = data[2];
                        var punchTime = data[3] + ":" + data[4] + ":" + data[5];
                        Hr_RawData hr_RawData = new Hr_RawData
                        {

                            DeviceNo = deviceNo,
                            CardNo = cardNo,
                            DtPunchDate = DateTime.Parse(punchDate),
                            DtPunchTime = DateTime.Parse(punchTime),
                            ComId = comid
                        };

                        hr_RawDataList.Add(hr_RawData);
                    }
                    await _context.Hr_RawData.AddRangeAsync(hr_RawDataList);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Data Import Successfully";
                    TempData["Status"] = "1";
                }

                else
                {
                    TempData["Message"] = "Please, Select a file!";
                    TempData["Status"] = "3";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);

            }

            return View();
        }
        #endregion

        #region Increment ALl Fill

        public int reportempFill()
        {

            int fun = 0;
            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                fun = 123;

            }

            else if (approvetype.Contains(1186) && approvetype.Contains(1187))
            {

                fun = 12;


            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1257))
            {
                fun = 13;

            }
            else if (approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                fun = 23;



            }

            else if (approvetype.Contains(1186))
            {
                fun = 1;
            }
            else if (approvetype.Contains(1187))
            {
                fun = 2;
            }
            else if (approvetype.Contains(1257))
            {
                fun = 3;
            }

            return fun;
        }
        #endregion

        #region Increment All

        public IActionResult GetIncrementAll(DateTime? from, string act = "")
        {
            int aproval = 0;
            ViewBag.worker = _incrementAllRepository.ForWorker();
            ViewBag.staff = _incrementAllRepository.ForStaff();
            var dateFrom = from.HasValue ? from.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dateTo = dateFrom.AddMonths(1).AddSeconds(-1).Date;


            aproval = reportempFill();

            var data = _incrementAllRepository.GetIncrementList(aproval, from, act);
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            if (act == "view")
                return View(data);
            else
                return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetIncrementAllJson(DateTime? from, string act = "")
        {
            int aproval = 0;
            ViewBag.worker = _incrementAllRepository.ForWorker();
            ViewBag.staff = _incrementAllRepository.ForStaff();
            var dateFrom = from.HasValue ? from.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dateTo = dateFrom.AddMonths(1).AddSeconds(-1).Date;


            aproval = reportempFill();

            var data = _incrementAllRepository.GetIncrementList(aproval, from, act);
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;

            return Json(data);

        }

        [HttpPost]
        public IActionResult CreateIncrementAll(List<HR_Emp_Increment> Increments)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _incrementAllRepository.CreateSalary(Increments);
                    TempData["Message"] = "Increment Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {

                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
                _logger.LogInformation(e.InnerException.Message);
            }
        }


        [HttpPost]
        public IActionResult UpdateSalary(List<HR_Emp_Salary> Salaries)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _incrementAllRepository.UpdateSalary(Salaries);
                    TempData["Message"] = "Increment Save/Update Successfully";
                    return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                }
                TempData["Message"] = "Some thing wrong, Check data";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
            catch (Exception e)
            {
                TempData["Message"] = "Please contact software authority";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }
        }


        #endregion

        #region Employee Info With Vendor type
        public IActionResult VendorInfoList()
        {
            string userid = HttpContext.Session.GetString("userid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), userid, "List Show", "");

            return View(_empInfoRepository.GetVendorInfoAll());
        }

        public IActionResult VendorInfoListUBL()
        {
            string userid = HttpContext.Session.GetString("userid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), userid, "List Show", "");

            return View(_empInfoRepository.GetVendorInfoAll());
        }

        #region commented code
        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        var comid = (HttpContext.Session.GetString("comid"));

        //        Microsoft.Extensions.Primitives.StringValues y = "";

        //        var x = Request.Form.TryGetValue("search[value]", out y);

        //        //if (y.ToString().Length > 0)
        //        //{

        //        var query = _empInfoRepository.EmpInfo();

        //        var parser = new Parser<EmployeeInfo>(Request.Form, query);

        //        return Json(parser.Parse());

        //        //}
        //        //return Json("");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = "0", error = ex.Message });
        //        //throw ex;
        //    }

        //}

        #endregion

        public IActionResult VendorCodeExist(string code)
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _empInfoRepository.GetEmp().Any(e => e.EmpCode == code && e.ComId == comid && e.IsDelete == false);
            return Json(_empInfoRepository.GetEmp().Any(e => e.EmpCode == code && e.ComId == comid && e.IsDelete == false));
        }

        public IActionResult CreateVendorInfo()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6", true));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            ViewData["WeekDay"] = WeekDaylist;

            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
            ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

            ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
            ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();

            ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
            ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();

            ViewData["BId"] = _buildingTypeRepository.GetBuildingType();

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();

            ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
            ViewData["BankId"] = _bankRepository.GetBankList();
            ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
            ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();

            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();

            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });

            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();
            ViewData["SubCategoryId"] = _empInfoRepository.VendorCategory();
            ViewData["RelayId"] = _empInfoRepository.VendorRelay();

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Bind("ComId,EmpId,EmpCode,EmpName,EmpFather,EmpMother,EmpSpouse,EmpCurrAdd,EmpCurrVill,EmpCurrPo,EmpCurrPs,EmpCurrDistId,EmpPerAdd,EmpPerVill,EmpPerPo,EmpPerPs,EmpPerCity,EmpPerDistId,EmpPerZip,EmpPhone,EmpMobile,EmpEmail,EmpPicLocation,EmpRemarks,Sex,RelgionId,Caste,BloodId,MaritalSts,DtBirth,DtJoin,DtReleased,DtIncrement,IsConfirm,DtConfirm,ConfDay,DeptId,SectId,SubSectId,DesigId,GradeId,FloorId,LineId,EmpCategory,WorkPlace,ShiftId,Nationality,PassportNo,VoterNo,BirthCertNo,IsAllowPf,DtPf,IsAllowOt,PaySource,PayMode,EmpType,EmpSts,CardNo,BankId,BankAcNo,Fpid,WeekDayId,OldEmpId,Approved,NickName,DtApprove,ChildNo,EmpCurrDist,EmpPerDist,EduLvl,EmpRef,EmpRefMob,IsTax,IsAcc,EmpCurrZip,EmpCurrCity,DtTransport,IsDisabled,EmpNomineeName,EmpNomineeMob,EmergencyName,EmergencyMob,EmployementType,EmpCatagory,Title,DtCardAssign,IsContract,WorkType,DtMarrige,CardNoOld,IsNid,ChildM,ChildF,EmpPflocation,EmpMclocation,EmpHblocation,EmpWflocation,IsInactive,PcName,UserId,DateAdded,UpdateByUserId,DateUpdated")]
        public async Task<IActionResult> CreateVendorInfo(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var dtConfirm = hrEmpInfo.DtConfirm;
            //var errors = ModelState.Where(x => x.Value.Errors.Any())
            //   .Select(x => new { x.Key, x.Value.Errors });
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay != null)
            {
                string relayId = GetRelayId(hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
                int? varId = GetVarId(relayId);
                hrEmpInfo.RelayId = varId.ToString();

            }


            string comid = HttpContext.Session.GetString("comid");
            if (ModelState.IsValid)
            {
                hrEmpInfo.UserId = HttpContext.Session.GetString("userid");
                hrEmpInfo.ComId = HttpContext.Session.GetString("comid");
                if (hrEmpInfo.EmpId != 0)
                {

                    _empInfoRepository.VendorInfoPost(hrEmpInfo, file, signFile);
                    if (hrEmpInfo.GrossSal != null)
                    {
                        //var val = _context.HR_Emp_Info.FirstOrDefault(x => x.ComId == comid && x.EmpCode == hrEmpInfo.EmpCode);
                        var sal = _context.HR_Emp_Salary.FirstOrDefault(x => x.ComId == comid && x.EmpId == hrEmpInfo.EmpId);
                        if (sal == null)
                        {
                            HR_Emp_Salary salary = new HR_Emp_Salary
                            {
                                EmpId = hrEmpInfo.EmpId,
                                BasicSalary = 0,
                                IsBS = true,
                                IsHr = true,
                                IsMa = true,
                                IsApprove = true,
                                IsDelete = false,
                                ComId = HttpContext.Session.GetString("comid"),
                                PersonalPay = (float)hrEmpInfo.GrossSal,
                                UserId = HttpContext.Session.GetString("userid"),
                                UpdateByUserId = HttpContext.Session.GetString("userid"),

                                //BId = 0,  
                                //PFLId = 0, 
                                //WelfareLId = 0,  
                                //MCLId = 0,  
                                //HBLId = 0, 
                                //HBLId2 = 0,  
                                //HBLId3 = 0, 
                                //PFLLId = 0,  
                                //PFLLId2 = 0,  
                                //PFLLId3 = 0, 
                                //GLId = 0, 
                                //PcName = "PC001",                          
                                //ApprovedBy = "Admin", 
                                //Remarks = "Initial entry", 
                            };
                            _context.HR_Emp_Salary.Add(salary);
                        }
                        else
                        {
                            sal.PersonalPay = (float)hrEmpInfo.GrossSal;
                        }



                        _context.SaveChanges();

                    }

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hrEmpInfo.EmpId.ToString(), "Update", hrEmpInfo.EmpName.ToString());

                }
                else
                {

                    _empInfoRepository.VendorInfoPostElse(hrEmpInfo, file, signFile);
                    _context.SaveChanges();

                    if (hrEmpInfo.GrossSal != null)
                    {
                        var sal = _context.HR_Emp_Salary.FirstOrDefault(x => x.ComId == comid && x.EmpId == hrEmpInfo.EmpId);
                        if (sal == null)
                        {
                            var val = _context.HR_Emp_Info.FirstOrDefault(x => x.ComId == comid && x.EmpCode == hrEmpInfo.EmpCode);


                            HR_Emp_Salary salary = new HR_Emp_Salary
                            {
                                EmpId = val.EmpId,
                                BasicSalary = 0,
                                IsBS = true,
                                IsHr = true,
                                IsMa = true,
                                IsApprove = true,
                                IsDelete = false,
                                ComId = HttpContext.Session.GetString("comid"),
                                PersonalPay = (float)hrEmpInfo.GrossSal,
                                UserId = HttpContext.Session.GetString("userid"),
                                UpdateByUserId = HttpContext.Session.GetString("userid"),

                                //BId = 0,  
                                //PFLId = 0, 
                                //WelfareLId = 0,  
                                //MCLId = 0,  
                                //HBLId = 0, 
                                //HBLId2 = 0,  
                                //HBLId3 = 0, 
                                //PFLLId = 0,  
                                //PFLLId2 = 0,  
                                //PFLLId3 = 0, 
                                //GLId = 0, 
                                //PcName = "PC001",                          
                                //ApprovedBy = "Admin", 
                                //Remarks = "Initial entry", 
                            };
                            _context.HR_Emp_Salary.Add(salary);
                        }
                        else
                        {
                            sal.PersonalPay = (float)hrEmpInfo.GrossSal;
                            _context.HR_Emp_Salary.Add(sal);
                            _context.SaveChanges();
                        }



                        _context.SaveChanges();

                    }
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";

                }

                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hrEmpInfo.EmpId.ToString(), "Create", hrEmpInfo.EmpName.ToString());

                return RedirectToAction(nameof(VendorInfoList));
            }
            else
            {
                TempData["Message"] = "Something is wrong!";
                TempData["Status"] = "3";
            }

            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
            ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

            ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
            ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();

            ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
            ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();

            ViewData["BId"] = _buildingTypeRepository.GetBuildingType();

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();
            ViewData["SubCategoryId"] = _empInfoRepository.VendorCategory();
            ViewData["RelayId"] = _empInfoRepository.VendorRelay();



            return View(hrEmpInfo);
        }

        // Emp info edit 
        public async Task<IActionResult> EditVendorInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hrEmpInfo = await _empInfoRepository.VendorInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            //ViewBag.DtJoin = DateTime.Parse(hrEmpInfo.DtJoin).ToString("dd-MMM-yy");
            ViewBag.Title = "Edit";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpAccTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();
            ViewData["SubCategoryId"] = _empInfoRepository.VendorCategory();
            ViewData["RelayId"] = _empInfoRepository.VendorRelay();

            return View("CreateVendorInfo", hrEmpInfo);
        }

        //Kamrul Relay
        private string GetRelayId(int? weekDay)
        {
            if (weekDay.HasValue)
            {
                switch (weekDay.Value)
                {
                    case 1: // Sunday
                        return "Q";
                    case 2: // Monday
                        return "R";
                    case 3: // Tuesday
                        return "S";
                    case 4: // Wednesday
                        return "T";
                    case 5: // Thursday
                        return "U";
                    case 6: // Friday
                        return "V";
                    case 7: // Saturday
                        return "P";
                    default:
                        break;
                }
            }
            return string.Empty;
        }

        //Kamrul Get Relay 

        private int? GetVarId(string relayName)
        {

            var catVariable = _context.Cat_Variable
                .FirstOrDefault(v => v.VarName == relayName && v.VarType == "Relay");

            return catVariable?.VarId;
        }

        // GET: EmpInfoTemp/Delete/5
        public async Task<IActionResult> DeleteVendorInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var hrEmpInfo = await _empInfoRepository.EmpInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }


            //var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            //ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            //ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            //ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            //ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            //ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            //ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }
            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();

            return View("CreateVendorInfo", hrEmpInfo);
        }

        // POST: EmpInfoTemp/Delete/5
        [HttpPost, ActionName("DeleteVendorInfo")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteVendorInfoConfirmed(int id)
        {

            try
            {
                var HR_Emp_Info = _empInfoRepository.FindById(id);
                _empInfoRepository.Delete(HR_Emp_Info);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Info.EmpId.ToString(), "Delete", HR_Emp_Info.EmpName);

                return Json(new { Success = 1, EmpId = HR_Emp_Info.EmpId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }



        #endregion

        #region Student Info (Employee Info With Student type)

        public IActionResult StudentInfoList()
        {
            string userid = HttpContext.Session.GetString("userid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), userid, "List Show", "");

            return View(_empInfoRepository.GetStudentInfoAll());
        }



        public IActionResult StudentCodeExist(string code)
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _empInfoRepository.GetEmp().Any(e => e.EmpCode == code && e.ComId == comid && e.IsDelete == false);
            return Json(_empInfoRepository.GetEmp().Any(e => e.EmpCode == code && e.ComId == comid && e.IsDelete == false));
        }

        public IActionResult CreateStudentInfo()
        {
            string comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Create";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6", true));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            ViewData["WeekDay"] = WeekDaylist;


            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
            ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

            ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
            ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();

            ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
            ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();

            ViewData["BId"] = _buildingTypeRepository.GetBuildingType();

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();

            ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
            ViewData["BankId"] = _bankRepository.GetBankList();
            ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
            ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();

            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();

            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });

            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentInfo(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var dtConfirm = hrEmpInfo.DtConfirm;
            //var errors = ModelState.Where(x => x.Value.Errors.Any())
            //   .Select(x => new { x.Key, x.Value.Errors });
            string comid = HttpContext.Session.GetString("comid");
            if (ModelState.IsValid)
            {
                hrEmpInfo.UserId = HttpContext.Session.GetString("userid");
                hrEmpInfo.ComId = HttpContext.Session.GetString("comid");
                if (hrEmpInfo.EmpId > 0)
                {
                    //SqlParameter[] sqlParemeter = new SqlParameter[3];
                    //sqlParemeter[0] = new SqlParameter("@ComID", comid);
                    //sqlParemeter[1] = new SqlParameter("@EmpID", hrEmpInfo.EmpId);
                    //sqlParemeter[2] = new SqlParameter("@dtJoin", hrEmpInfo.DtJoin);

                    //string query = $"Exec HR_prcProcessLeaveInput '{comid}', {hrEmpInfo.EmpId}, '{hrEmpInfo.DtJoin}'";
                    //Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeaveInput", sqlParemeter);

                    _empInfoRepository.StudentInfoPost(hrEmpInfo, file, signFile);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hrEmpInfo.EmpId.ToString(), "Update", hrEmpInfo.EmpName.ToString());

                }
                else
                {
                    _empInfoRepository.StudentInfoPostElse(hrEmpInfo, file, signFile);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";

                }

                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hrEmpInfo.EmpId.ToString(), "Create", hrEmpInfo.EmpName.ToString());

                return RedirectToAction(nameof(StudentInfoList));
            }
            else
            {
                TempData["Message"] = "Something is wrong!";
                TempData["Status"] = "3";
            }

            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
            ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

            ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
            ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();

            ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
            ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();

            ViewData["BId"] = _buildingTypeRepository.GetBuildingType();

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();




            return View(hrEmpInfo);
        }

        // Student info edit 
        public async Task<IActionResult> EditStudentInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hrEmpInfo = await _empInfoRepository.StudentInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            //ViewBag.DtJoin = DateTime.Parse(hrEmpInfo.DtJoin).ToString("dd-MMM-yy");
            ViewBag.Title = "Edit";
            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }
            var comid = HttpContext.Session.GetString("comid");
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();
            var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }

            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();

            return View("CreateStudentInfo", hrEmpInfo);
        }

        // GET: StudentInfoTemp/Delete/5
        public async Task<IActionResult> DeleteStudentInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var hrEmpInfo = await _empInfoRepository.StudentInfoEdit(id);

            if (hrEmpInfo == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            List<SelectListItem> WeekDaylist = new List<SelectListItem>();
            WeekDaylist.Add(new SelectListItem("SUNDAY", "1"));
            WeekDaylist.Add(new SelectListItem("MONDAY", "2"));
            WeekDaylist.Add(new SelectListItem("TUESDAY", "3"));
            WeekDaylist.Add(new SelectListItem("WEDNESDAY", "4"));
            WeekDaylist.Add(new SelectListItem("THURSDAY", "5"));
            WeekDaylist.Add(new SelectListItem("FRIDAY", "6"));
            WeekDaylist.Add(new SelectListItem("SATURDAY", "7"));
            if (hrEmpInfo.HR_Emp_PersonalInfo.WeekDay == null)
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", 6);
            }
            else
            {
                ViewData["WeekDay"] = new SelectList(WeekDaylist, "Value", "Text", hrEmpInfo.HR_Emp_PersonalInfo.WeekDay);
            }


            //var year = _context.Acc_FiscalYears.Where(f => f.ComId == comid).Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            //ViewData["PFFinalYId"] = _hREmpInfoRepository.PFFinalYList();
            //ViewData["PFFundTransferYId"] = _hREmpInfoRepository.PFFundTransferYList();
            //ViewData["WFFinalYId"] = _hREmpInfoRepository.WFFinalYList();
            //ViewData["WFFundTransferYId"] = _hREmpInfoRepository.WFFundTransferYList();
            //ViewData["GratuityFinalYId"] = _hREmpInfoRepository.GratuityFinalYList();
            //ViewData["GratuityFundTransferYId"] = _hREmpInfoRepository.GratuityFundTransferYList();
            ViewData["BloodId"] = _bloodGroupRepository.BloodGroupSelectList();
            ViewData["DeptId"] = _departmentRepository.GetDepartmentList();
            ViewData["DesigId"] = _designationRepository.GetDesignationList();

            if (hrEmpInfo.HR_Emp_Address != null)
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _hREmpInfoRepository.EmpCurrPSList(hrEmpInfo);
                ViewData["EmpPerPSId"] = _hREmpInfoRepository.EmpPerPSList(hrEmpInfo);


                ViewData["EmpCurrPOId"] = _hREmpInfoRepository.EmpCurrPOList(hrEmpInfo);
                ViewData["EmpPerPOId"] = _hREmpInfoRepository.EmpPerPOList(hrEmpInfo);
            }
            else
            {
                ViewData["EmpCurrDistId"] = _districtRepository.GetDistrictList();
                ViewData["EmpPerDistId"] = _districtRepository.GetDistrictList();

                ViewData["EmpCurrPSId"] = _policeStationRepository.GetPoliceStationList();
                ViewData["EmpPerPSId"] = _policeStationRepository.GetPoliceStationList();


                ViewData["EmpCurrPOId"] = _postOfficeRepository.GetPostOfficeList();
                ViewData["EmpPerPOId"] = _postOfficeRepository.GetPostOfficeList();
            }
            if (hrEmpInfo.HR_Emp_PersonalInfo != null)
            {
                ViewData["BId"] = _hREmpInfoRepository.EmpBuildingTypeList(hrEmpInfo);
            }
            else
            {
                ViewData["BId"] = _buildingTypeRepository.GetBuildingType();
            }
            ViewData["GenderId"] = _genderRepository.GenderList();

            ViewData["EmpTypeId"] = _empTypeRepository.GetEmpTypeList();


            if (hrEmpInfo.HR_Emp_BankInfo != null)
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.EmpPayModeList(hrEmpInfo);
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
                ViewData["AccTypeId"] = _hREmpInfoRepository.EmpBranchList(hrEmpInfo);
            }
            else
            {
                ViewData["PayModeId"] = _hREmpInfoRepository.PayModeList();
                ViewData["BankId"] = _bankRepository.GetBankList();
                ViewData["BranchId"] = _bankBranchRepository.GetBankBranchList();
                ViewData["AccTypeId"] = _hREmpInfoRepository.AccTypeList();
            }

            HttpContext.Session.SetInt32("empid", (int)id);
            ViewData["FloorId"] = _floorRepository.GetFloorList();
            ViewData["GradeId"] = _gradeRepository.GradeSelectList();
            ViewData["LineId"] = _lineRepository.GetLineList();
            ViewData["RelgionId"] = _religionRepository.ReligionSelectList();
            ViewData["SectId"] = _sectionRepository.GetSectionList();
            ViewData["ShiftId"] = _shiftRepository.GetShiftList();
            ViewData["SubSectId"] = _subSectionRepository.GetSubSectionList();
            ViewData["UnitId"] = _cat_UnitRepository.GetCat_UnitList();
            ViewData["SkillId"] = _skillRepository.GetSkillList();
            ViewData["VendorType"] = _empInfoRepository.VendorType();
            ViewData["JobNatureType"] = _empInfoRepository.JobNatureType();
            ViewData["AltitudeType"] = _empInfoRepository.AltitudeType();

            return View("CreateStudentInfo", hrEmpInfo);
            //return RedirectToAction("DeleteStudentInfoConfirmed","HR");
        }

        public class idpass
        {
            public string id { get; set; }

        }
        // POST: EmpInfoTemp/Delete/5
        [HttpPost, ActionName("DeleteStudentInfo")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteStudentInfoConfirmed(int id)
        {

            try
            {
                var HR_Stu_Info = _empInfoRepository.FindById(id);
                _empInfoRepository.Delete(HR_Stu_Info);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Stu_Info.EmpId.ToString(), "Delete", HR_Stu_Info.EmpName);

                return Json(new { Success = 1, EmpId = HR_Stu_Info.EmpId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }



        #endregion

        #region Leave Info Entry Single
        public IActionResult LeaveInfoEntryList()
        {
            var comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            string email = HttpContext.Session.GetString("username");
            ViewBag.userid = (from emp in _context.HR_Emp_Info
                              join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                              join s in _context.Cat_Section on emp.SectId equals s.SectId
                              join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                              join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                              where emp.ComId == comid && emp.EmpEmail == email && emp.IsDelete == false && emp.IsInactive == false

                              select emp.EmpId).FirstOrDefault();
            HR_Leave_Avail LeaveData = _leaveEntryRepository.LeaveAvailInfo();
            ViewBag.LeaveBalance = _leaveEntryRepository.LeaveBalanceInfo();
            ViewBag.EmpId = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeListInfo();
            var timeOfDayList = new SelectList(new[]
            {
                new { Value = "Fullday", Text = "Full Day" },
                new { Value = "Morning", Text = "Morning" },
                new { Value = "Evening", Text = "Evening" }
            }, "Value", "Text");

            ViewData["LeaveOption"] = timeOfDayList;
            var x = (from emp in _context.HR_Emp_Info
                     join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                     join s in _context.Cat_Section on emp.SectId equals s.SectId
                     join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                     join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                     where emp.ComId == comid && emp.EmpEmail == email && emp.IsDelete == false && emp.IsInactive == false

                     select emp.EmpCode + " - [ " + emp.EmpName + " ]  ").FirstOrDefault();

            ViewBag.empname = x;

            //ViewBag.SingleEmp = _context.HR_ApprovalSettings.Where(x => x.UserId == userid && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();

            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();


            if (approveData != null)
            {
                ViewBag.ApprovalSetting = 1;
            }
            else
            {
                ViewBag.ApprovalSetting = 0;
            }

            return View();
        }

        [HttpPost]
        public ActionResult RefreshFileForLeave(string empId, string date)
        {
            // Code to refresh the file content on the server
            // You can update the file content based on empId and date parameters here

            // Return a success response
            return Json(new { success = true });
        }

        public FileResult DownloadFileForLeave(string filename)
        {
            string path = Path.Combine(this._env.WebRootPath, "LeaveDocument/") + filename;
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public JsonResult LoadInfoEmployeeLeaveData(int? empId, DateTime? date)
        {
            DateTime asdf = DateTime.Now.Date;
            if (date is null)
            {
                var adate = DateTime.Now.Year;
            }
            else
            {
                //asdf = date;
            }

            var comid = HttpContext.Session.GetString("comid");


            string year = date.Value.Year.ToString();


            var CL = (from emp in _context.HR_Leave_Avail
                      where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "CL" && emp.EmpId == empId && emp.DtFrom.Year.ToString() == year
                      select emp).Sum(x => x.TotalDay);

            var CLH = (from emp in _context.HR_Leave_Avail
                       where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "CLH" && emp.EmpId == empId && emp.DtFrom.Year.ToString() == year
                       select emp).Sum(x => x.TotalDay);

            var TotaCHL = CL + CLH;

            var SL = (from emp in _context.HR_Leave_Avail
                      where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "SL" && emp.EmpId == empId && emp.DtFrom.Year.ToString() == year
                      select emp).Sum(x => x.TotalDay);

            var SLH = (from emp in _context.HR_Leave_Avail
                       where emp.IsApprove == true && emp.ComId == comid && emp.LvType == "SLH" && emp.EmpId == empId && emp.DtFrom.Year.ToString() == year
                       select emp).Sum(x => x.TotalDay);

            var TotaSHL = SL + SLH;

            var LeaveBalance = _leaveEntryRepository.LeaveInfoEntry(empId, date)


                .Select(d => new
                {
                    LeaveId = d.LvBalId,
                    Code = d.HR_Emp_Info.EmpCode,
                    EmployeeName = d.HR_Emp_Info.EmpName,
                    Year = year,
                    CLTotal = d.CL,
                    CLEnjoyed = TotaCHL,//d.ACL + CLH,
                    SLTotal = d.SL,
                    SLEnjoyed = TotaSHL,//d.ASL + SLH,
                    ELTotal = d.EL,
                    ELEnjoyed = d.AEL,
                    MLTotal = d.ML,
                    MLEnjoyed = d.AML
                });

            // ViewBag.Leaveavoail = LeaveBalance;
            return Json(LeaveBalance);
        }

        public JsonResult LoadLeaveInfoEntryPartial(int empId)
        {
            try
            {
                var data = _leaveEntryRepository.LoadLeaveInfoEntryPartial(empId);
                return Json(data);
            }
            catch (Exception)
            {
                return Json(new { Success = 0, InvoiceId = 0, ex = "Unable to Load the Data" });

            }
        }

        public JsonResult GetInfoToDate(DateTime? DtFrom, double TotalDay)
        {
            double day = 1;
            if (TotalDay < 1)
            {
                day = 1;
            }
            else
            {
                day = TotalDay;
            }

            DateTime DtTo = DtFrom.Value.AddDays(day).AddDays(-1);
            string dtto = DtTo.ToString("dd-MMM-yy");
            return Json(dtto);
        }


        public JsonResult LeaveInfoBalanceCheck(int empid)
        {
            var balance = _leaveEntryRepository.GetBalanceInfo(empid);
            var leaveType = _leaveEntryRepository.GetLeaveTypeInfo();
            return Json(new { balance, leaveType });
        }

        public IActionResult CreateLeaveInfoEntry(int empid)
        {
            HR_Leave_Avail LeaveData = _leaveEntryRepository.LeaveAvailInfo();
            ViewBag.LeaveBalance = _leaveEntryRepository.LeaveBalanceInfo();
            ViewData["EmpId"] = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeListInfo();
            return View();

        }
        [HttpPost]
        public IActionResult CreateLeaveInfoEntry(HR_Leave_Avail hR_Leave_Avail)
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewData["EmpId"] = _empReleaseRepository.EmpList();
            ViewBag.LTypeId = _leaveEntryRepository.LeaveTypeListInfo();

            hR_Leave_Avail.PcName = HttpContext.Session.GetString("pcname");
            hR_Leave_Avail.UserId = HttpContext.Session.GetString("userid");
            hR_Leave_Avail.ComId = HttpContext.Session.GetString("comid");
            hR_Leave_Avail.DtInput = DateTime.Today;
            //var temp = hR_Leave_Avail.EmpId.ToString();
            Guid guidValue = Guid.NewGuid();
            var temp = guidValue.ToString();
            hR_Leave_Avail.FileName = "";
            if (hR_Leave_Avail.FileToUpload != null)
            {
                foreach (var item in hR_Leave_Avail.FileToUpload)
                {
                    string FileNameUrl = UploadFileForLeave(item, temp);
                    hR_Leave_Avail.FileName += FileNameUrl + "!";
                };
            }


            string wwwPath = this._env.WebRootPath;
            string contentPath = this._env.ContentRootPath;
            string path = Path.Combine(this._env.WebRootPath, "LeaveDocument");


            HR_Leave_Balance LeaveBalance = _leaveEntryRepository.CreateLeaveBalanceInfo(hR_Leave_Avail);
            Cat_Leave_Type LeaveType = _leaveEntryRepository.CreateLeaveTypeInfo(hR_Leave_Avail);
            float AvailCL = 0;
            float AvailSL = 0;
            float AvailEL = 0;
            float AvailML = 0;
            var Success = "";
            AvailCL = (float)(LeaveBalance.CL - LeaveBalance.ACL);
            AvailSL = (float)(LeaveBalance.SL - LeaveBalance.ASL);
            AvailEL = (float)(LeaveBalance.EL - LeaveBalance.AEL);
            AvailML = (float)(LeaveBalance.ML - LeaveBalance.AML);
            var message = "Leave Balance Over.Please Correction Leave Day";
            var message1 = "LL have to be applied within ";
            Success = "Data Save Successfully";

            var allLeaveData = _context.HR_Leave_Avail
                            .Where(x => x.ComId == hR_Leave_Avail.ComId
                                     && x.EmpId == hR_Leave_Avail.EmpId
                                     && (hR_Leave_Avail.DtFrom).Date >= x.DtFrom.Date
                                     && (hR_Leave_Avail.DtFrom).Date <= x.DtTo.Date)
                            .ToList();
            if (allLeaveData.Count > 0)
            {
                return Json(new { Success = 0, ex = "Leave already existed." });
            }

            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == hR_Leave_Avail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            try
            {
                try //Hossain Naim
                {
                    var leavevalidation = _context.Cat_Leave_Type.Where(x => x.LTypeId == hR_Leave_Avail.LTypeId).Select(s => s.IsValidation).FirstOrDefault();
                    if (hR_Leave_Avail.dtWork != null || leavevalidation == true)
                    {
                        var validDays = _context.Cat_Leave_Type.Where(x => x.LTypeId == hR_Leave_Avail.LTypeId).Select(s => s.ValidDays).FirstOrDefault();

                        message1 += validDays.ToString() + " " + "Days";
                        var timeSpan = hR_Leave_Avail.DtFrom - hR_Leave_Avail.dtWork;
                        if (timeSpan.HasValue)
                        {
                            int daysDifference = timeSpan.Value.Days;
                            if (daysDifference > validDays)
                            {
                                return Json(new { Success = 0, ex = message1 });
                            }
                        }

                    }
                    if (approveData == null)
                    {
                        if (LeaveType.LTypeNameShort == "CL" || LeaveType.LTypeNameShort == "CLH")
                        {
                            if (AvailCL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.ACL;
                                LeaveBalance.ACL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "EL" || LeaveType.LTypeNameShort == "ELH")
                        {
                            if (AvailEL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.AEL;
                                LeaveBalance.AEL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "SL" || LeaveType.LTypeNameShort == "SLH")
                        {
                            if (AvailSL >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.ASL;
                                LeaveBalance.ASL = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                        else if (LeaveType.LTypeNameShort == "ML")
                        {
                            if (AvailML >= hR_Leave_Avail.TotalDay)
                            {
                                LeaveBalance.PreviousLeave = LeaveBalance.AML;
                                LeaveBalance.AML = (LeaveBalance.PreviousLeave + hR_Leave_Avail.TotalDay);
                            }
                            else
                            {
                                return Json(new { Success = 0, ex = message });

                            }
                        }
                    }

                    _leaveEntryRepository.CreateLeaveInfoEntryPost(hR_Leave_Avail);

                }
                catch (Exception ex)
                {
                    //throw ex;
                    return Json(new { Success = 0, ex = ex });

                }
                _leaveEntryRepository.CreateLeaveInfoEntryPost2(hR_Leave_Avail);

                if (approveData != null)
                {
                    var empData = _context.HR_Emp_Info.Where(x => x.EmpId == hR_Leave_Avail.EmpId).FirstOrDefault();
                    var emailto = empData.EmpEmail;
                    SendEmailForLeave(emailto, true, false, false, hR_Leave_Avail);
                    if (approveData.IsFirstLeaveApprove == true)
                    {
                        var firstData = _context.HR_Emp_Info.Where(x => x.EmpId == empData.FirstAprvId).FirstOrDefault();

                        if (firstData != null)
                        {
                            var emailtoHod = firstData.EmpEmail;
                            SendEmailForLeave(emailtoHod, false, true, false, hR_Leave_Avail);
                        }

                    }
                    else
                    {
                        var firstData = _context.HR_Emp_Info.Where(x => x.EmpId == empData.FinalAprvId).FirstOrDefault();

                        if (firstData != null)
                        {
                            var emailtoHod = firstData.EmpEmail;
                            SendEmailForLeave(emailtoHod, false, true, false, hR_Leave_Avail);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex });

                //throw ex;
            }

            return Json(new { Success = 1, ex = Success });
        }

        private string UploadFileForLeave(IFormFile file, string title)
        {
            string FileName = null;
            if (file != null)
            {
                //string folder = "book/Gallery/";
                string serverFolder = _env.WebRootPath + "\\LeaveDocument\\";
                FileName = title.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(serverFolder, FileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

            }
            return FileName;
        }

        public void SendEmailForLeave(string emailTo, bool IsApplicant, bool HOD, bool IsFirstApprvd, HR_Leave_Avail model)
        {
            var link = "<br><br>Please follow the link to proceed.<a href=\"https://gtrbd.net/erp/PostDocument/Index?DocType=Leave&criteria=Pending\"> Click here to approve or disapprove</a>";
            var comid = HttpContext.Session.GetString("comid");
            string subject = "", body = "Dear ", senderAddress = "", host = "", userName = "", password = "", title = "";

            var leave = _context.Cat_Leave_Type.Where(x => x.LTypeId == model.LTypeId).Select(y => y.LTypeName).FirstOrDefault();
            var leaveName = model.LvType + "[ " + leave + " ]";

            string[] substrings1 = model.DtLvInput.ToString().Split(' ');
            var dtinput = substrings1[0];

            string[] substrings2 = model.DtTo.ToString().Split(' ');
            var dtTo = substrings2[0];

            string[] substrings3 = model.DtFrom.ToString().Split(' ');
            var dtFrom = substrings3[0];

            int port = 0;
            try
            {

                if (IsApplicant == true && IsFirstApprvd == false)
                {
                    var empName = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var mailData = _context.Cat_MailSettings.Where(x => x.IsApplicant == true && x.IsFirstApprvd == false && x.IsRejected == false).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>&nbsp;&nbsp;&nbsp;:" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtTo + " To " + dtFrom + "<br><b>Reason</b>      :" + model.Remark;


                    //body = body + empName + mailData.MailBody + model.DtLvInput + "<br><b>Leave Type</b>       : " + model.LvType + "<br><b>Leave Day(s)</b>      : " + model.TotalDay.ToString()
                    //       + "<br><b>Leave Date</b>        : " + model.DtTo.ToString() + " To " + model.DtFrom.ToString() + "<br><b>Reason</b>      : " + model.Remark;



                    title = mailData.CompanyTitle;


                }

                if (HOD == true)
                {
                    var empName = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var empCode = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpCode).FirstOrDefault();

                    var mailData = _context.Cat_MailSettings.Where(x => x.IsHOD == true).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;


                    body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtTo + " To " + dtFrom + "<br><b>Reason</b>      :" + model.Remark;

                    body += link;

                    title = mailData.CompanyTitle;
                }
                //emailTo ="Future03@gtrbd.com";
                var message = new MailMessage();
                message.From = new MailAddress(senderAddress, title);

                message.To.Add(new MailAddress(emailTo));


                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = _smtpConfig.Value.IsBodyHTML; //true;

                using (var client = new SmtpClient())
                {
                    client.Host = host; //"smtp.gmail.com";
                    client.Port = port;//587;
                    client.EnableSsl = _smtpConfig.Value.EnableSSL;// true;
                                                                   //client.Credentials = new NetworkCredential(config.GetSection("CredentialMail").Value, config.GetSection("CredentialPassword").Value);
                    client.Credentials = new NetworkCredential(userName, password);
                    client.Send(message);
                }


                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}
            }
            catch (Exception)
            {

                throw;
            }

        }

        public JsonResult LoadGridInfoData(int lvid)
        {
            HR_Leave_Avail LeaveData = (HR_Leave_Avail)_leaveEntryRepository.LoadGridLeaveInfoData(lvid);
            var data = _leaveEntryRepository.GridDataInfo(lvid);

            //var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveData.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            //var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveData.ComId && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            //if (approveData != null)
            //{
            //    ViewBag.ApprovalSetting = 1;
            //}
            //else
            //{
            //    ViewBag.ApprovalSetting = 0;
            //}

            return Json(data);

        }

        public JsonResult UpdateLeaveInfoEntry(HR_Leave_Avail LeaveAvail)
        {
            LeaveAvail.UserId = HttpContext.Session.GetString("userid");
            LeaveAvail.ComId = HttpContext.Session.GetString("comid");
            LeaveAvail.DtInput = DateTime.Today;

            var message = "Leave Balance Over.Please Correction Leave Day";
            HR_Leave_Balance lb = (HR_Leave_Balance)_leaveEntryRepository.UpdateLBInfo(LeaveAvail);
            HR_Leave_Balance previouslb = (HR_Leave_Balance)_leaveEntryRepository.PreviousLBInfo(LeaveAvail);

            Cat_Leave_Type LeaveType = _leaveEntryRepository.CreateLeaveTypeInfo(LeaveAvail);
            LeaveAvail.LvType = LeaveType.LTypeNameShort;
            HR_Leave_Avail PreviousLeave = (HR_Leave_Avail)_leaveEntryRepository.PreviousLAInfo(LeaveAvail);
            float ACL = 0;
            float AEL = 0;
            float ASL = 0;
            float AML = 0;
            LeaveAvail.PreviousDay = PreviousLeave.TotalDay;
            var Success = "";

            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveAvail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == LeaveAvail.ComId && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (approveData != null)
            {


                if (LeaveType.LTypeNameShort == "CL") //|| LeaveType.LTypeNameShort == "CLH")
                {

                    if (LeaveAvail.TotalDay <= lb.CL)
                    {

                        //_leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        ////db.SaveChanges();

                        //return Json(new { Success = 2, ex = message });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "CLH")
                {
                    if (LeaveAvail.TotalDay <= lb.CL)
                    {

                        //_leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        ////db.SaveChanges();

                        //return Json(new { Success = 2, ex = message });

                        //return Json(message);
                    }
                }


                else if (LeaveType.LTypeNameShort == "SL")
                {
                    if (LeaveAvail.TotalDay <= lb.SL)
                    {

                        //_leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        ////db.SaveChanges();

                        //return Json(new { Success = 2, ex = message });

                        //return Json(message);
                    }
                }



                else if (LeaveType.LTypeNameShort == "SLH")
                {
                    if (LeaveAvail.TotalDay <= lb.SL)
                    {

                        //_leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        //db.SaveChanges();

                        //  return Json(new { Success = 2, ex = message });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "EL")
                {
                    if (LeaveAvail.TotalDay <= lb.EL)
                    {

                        //_leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        ////db.SaveChanges();

                        //return Json(new { Success = 2, ex = message });

                        ////return Json(message);
                    }
                }



                else if (LeaveType.LTypeNameShort == "ML")
                {
                    if (LeaveAvail.TotalDay <= lb.ML)
                    {

                        //_leaveEntryRepository.UpdateLeaveAvail(LeaveAvail);
                        ////db.SaveChanges();

                        //return Json(new { Success = 2, ex = message });

                        ////return Json(message);
                    }
                }

                _leaveEntryRepository.UpdateLeaveAvailInfo(LeaveAvail);
                // db.SaveChanges();

                return Json(new { Success = 2, ex = message });

            }
            else
            {

                if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CL")// || LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);

                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CLH")// || LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);

                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SL")// || LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SLH")// || LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "EL" || LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL - LeaveAvail.PreviousDay);
                    lb.AEL = AEL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML - LeaveAvail.PreviousDay);
                    lb.AML = AML;
                }

                //db.Update(lb);
                //db.SaveChanges();



                if (LeaveType.LTypeNameShort == "CL") //|| LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL + LeaveAvail.TotalDay);
                    lb.ACL = ACL;

                    if (ACL > lb.CL)
                    {
                        //db.Update(previouslb);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                        //return Json(message);
                    }
                }

                else if (LeaveType.LTypeNameShort == "CLH") //|| LeaveType.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL + LeaveAvail.TotalDay);
                    lb.ACL = ACL;

                    if (ACL > lb.CL)
                    {
                        //db.Update(previouslb);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                        //return Json(message);
                    }
                }
                else if (LeaveType.LTypeNameShort == "SL") //|| LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL + LeaveAvail.TotalDay);
                    lb.ASL = ASL;
                    if (ASL > lb.SL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "SLH") //|| LeaveType.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL + LeaveAvail.TotalDay);
                    lb.ASL = ASL;
                    if (ASL > lb.SL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "EL") //|| LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL + LeaveAvail.TotalDay);
                    lb.AEL = AEL;
                    if (AEL > lb.EL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }

                else if (LeaveType.LTypeNameShort == "ELH") //|| LeaveType.LTypeNameShort == "ELH")
                {
                    AEL = (float)(lb.AEL + LeaveAvail.TotalDay);
                    lb.AEL = AEL;
                    if (AEL > lb.EL)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }
                else if (LeaveType.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML + LeaveAvail.TotalDay);
                    lb.AML = AML;
                    if (AML > lb.ML)
                    {
                        //db.Update(PreviousLeave);
                        //db.SaveChanges();

                        return Json(new { Success = 0, ex = message });

                    }
                }




                _leaveEntryRepository.UpdateLABInfo(LeaveAvail);
                _leaveEntryRepository.UpdateLeaveAvailInfo(LeaveAvail);


            }
            Success = "Data Update Successully";
            return Json(new { Success = 2, ex = Success });

        }

        [HttpPost, ActionName("DeleteLeaveInfoEntry")]
        public IActionResult DeleteLeaveInfoEntryConfirmed(HR_Leave_Avail LeaveAvail)
        {
            var Success = "";
            try
            {
                _leaveEntryRepository.DeleteLeaveInfoEntry(LeaveAvail);
                Success = "Data Delete Successully";
                return Json(new { Success = 1, ex = Success });

            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }


        #endregion 

        #region GatePass Management System
        public IActionResult GetGatePassMsAll()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            // _employeeBehaviourRepository.GetBehaveAll()
            return View();

        }

        // GET: EmployeeBehaviorController/Create
        public IActionResult CreateGatePassMs()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Create";

            //ViewBag.EmpId = _empReleaseRepository.EmpList();

            //ViewBag.NoticeType = _employeeBehaviourRepository.CatVariableList();
            return View();
        }

        // POST: EmployeeBehaviorController/Create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult CreateGatePassMs(GatePassMs gatePassMs)
        {
            //ViewBag.EmpId = _empReleaseRepository.EmpList();

            //ViewBag.NoticeType = _empReleaseRepository.EmpList();

            //if (ModelState.IsValid)
            //{
            //    if (HR_Emp_Behave.BehaveId > 0)
            //    {
            //        _employeeBehaviourRepository.Update(HR_Emp_Behave);

            //        TempData["Message"] = "Data Update Successfully";
            //        TempData["Status"] = "2";
            //        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Behave.BehaveId.ToString(), "Update", HR_Emp_Behave.Decision.ToString());

            //    }
            //    else
            //    {
            //        _employeeBehaviourRepository.Add(HR_Emp_Behave);

            //        TempData["Message"] = "Data Save Successfully";
            //        TempData["Status"] = "1";
            //        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_Emp_Behave.BehaveId.ToString(), "Create", HR_Emp_Behave.Decision.ToString());

            //    }
            //    return RedirectToAction("EmployeeBehaviourList", "HR");
            //}
            return View();
        }

        // GET: EmployeeBehaviorController/Edit/5
        public IActionResult EditGatePassMs(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //var comid = HttpContext.Session.GetString("comid");
            //ViewBag.Title = "Edit";
            //var HR_Emp_Behave = _employeeBehaviourRepository.FindById(id);

            //ViewBag.EmpId = _empReleaseRepository.EmpList();

            //ViewBag.NoticeType = _employeeBehaviourRepository.CatVariableList();


            return View();
        }
        // GET: EmployeeBehaviorController/Delete/5
        public IActionResult DeleteGatePassMs(int id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //var comid = HttpContext.Session.GetString("comid");
            //ViewBag.Title = "Delete";
            //var HR_Emp_Behave = _employeeBehaviourRepository.FindById(id);

            //ViewBag.EmpId = _empReleaseRepository.EmpList();

            //ViewBag.NoticeType = _employeeBehaviourRepository.CatVariableList();

            //if (HR_Emp_Behave == null)
            //{
            //    return NotFound();
            //}
            //"CreateEmployeeBehaviour", HR_Emp_Behave
            return View();
        }
        #endregion

        #region Unilever Daily requisition
        public IActionResult dailyReq()
        {

            return View();
        }
        public IActionResult dailyReqData(string searchDate)
        {


            return Json(_empInfoRepository.GetRequisitionInfo(searchDate));
        }
        [HttpPost]
        public IActionResult SavedailyReqData(List<Models.Daily_req_entry> requisition, string searchDate)
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            foreach (var t in requisition)
            {
                if (t.ID != 0)
                {
                    var exist = _context.Daily_req_entry.Where(w => w.ID == t.ID && w.Comid == ComId && w.dateTime == DateTime.Parse(searchDate)).Select(s => s).FirstOrDefault();
                    if (exist != null)
                    {
                        TempData["Message"] = "data updated";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), t.ID.ToString(), "Updated", DateTime.Now.ToString());
                    }
                    else
                    {
                        TempData["Message"] = "data Saved";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), t.ID.ToString(), "Saved", DateTime.Now.ToString());

                    }
                }
                else  /// for checking duplicate upload from excel
                {
                    var exist = _context.Daily_req_entry.Where(w => w.Comid == ComId && w.dateTime == DateTime.Parse(searchDate) && w.unitid == t.unitid).Select(s => s).FirstOrDefault();
                    if (exist != null)
                    {
                        TempData["Message"] = "data updated";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), t.ID.ToString(), "Updated", DateTime.Now.ToString());
                    }
                    else
                    {
                        TempData["Message"] = "data Saved";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), t.ID.ToString(), "Saved", DateTime.Now.ToString());

                    }
                }

            }
            _empInfoRepository.SaveRequisitionInfo(requisition, searchDate);
            return Json(TempData["Message"]);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile file, string fromDate)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/Vendor/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/Vendor/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var data = this.GetVendorDailyReq(file.FileName, fromDate);
                    if (data.Count() > 0)
                    {

                        SavedailyReqData(data, fromDate);

                        //await _context.Daily_req_entry.AddRangeAsync(data);
                        //await _context.SaveChangesAsync();

                        // for import database data from temporary table

                        //SqlParameter[] sqlParameter1 = new SqlParameter[2];
                        //sqlParameter1[0] = new SqlParameter("@ComId", comid);
                        //sqlParameter1[1] = new SqlParameter("@InputType", "Update");

                        //string query = $"Exec HR_prcProcessEmpInfoExcel '{comid}','Update'";
                        //Helper.ExecProc("HR_prcProcessEmpInfoExcel", sqlParameter1);

                        //TempData["Message"] = "Data Upload Successfully";
                        //TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }

            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("dailyReq");
        }

        private List<Models.Daily_req_entry> GetVendorDailyReq(string fName, string fromDate)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/Vendor/" + comid + "/" + userid + "/" + fName);

            List<Models.Daily_req_entry> empdata = new List<Models.Daily_req_entry>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }

                            else
                            {

                                empdata.Add(new Models.Daily_req_entry()
                                {

                                    //dateTime= DateTime.Parse(fromDate),
                                    deptid = reader.GetValue(1) == null ? 0 : int.Parse(reader.GetValue(1).ToString()),
                                    DeptName = reader.GetValue(2) == null ? "N/A" : reader.GetValue(2).ToString(),
                                    unitid = int.Parse(reader.GetValue(3).ToString()),
                                    Job_Loc = reader.GetValue(4) == null ? "N/A" : reader.GetValue(4).ToString(),
                                    desigid = int.Parse(reader.GetValue(5).ToString()),
                                    Job_Nat = reader.GetValue(6) == null ? "N/A" : reader.GetValue(6).ToString(),
                                    SectId = int.Parse(reader.GetValue(7).ToString()),
                                    Cost_head = reader.GetValue(8).ToString(),

                                    Sup_A = int.Parse(reader.GetValue(9).ToString()),
                                    Exc_A = int.Parse(reader.GetValue(10).ToString()),
                                    Wor_A = int.Parse(reader.GetValue(11).ToString()),

                                    Sup_G = int.Parse(reader.GetValue(12).ToString()),
                                    Exc_G = int.Parse(reader.GetValue(13).ToString()),
                                    Wor_G = int.Parse(reader.GetValue(14).ToString()),

                                    Sup_B = int.Parse(reader.GetValue(15).ToString()),
                                    Exc_B = int.Parse(reader.GetValue(16).ToString()),
                                    Wor_B = int.Parse(reader.GetValue(17).ToString()),

                                    Sup_C = int.Parse(reader.GetValue(18).ToString()),
                                    Exc_C = int.Parse(reader.GetValue(19).ToString()),
                                    Wor_C = int.Parse(reader.GetValue(20).ToString()),
                                    Comid = comid

                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                _logger.LogError(e.InnerException.Message);
            }

            return empdata;
        }

        public IActionResult Emp_Details()
        {
            return View();
        }

        #endregion

        #region Employee Profile

        public JsonResult SearchEmployeesForDevice(string term)
        {
            var employees = _context.HR_Emp_Info
                .Where(e => e.EmpName.Contains(term) || e.EmpCode.Contains(term))
                .Select(e => new { label = e.EmpCode + " " + e.EmpName, value = e.EmpId })
                .Take(10)
                .ToList();

            return new JsonResult(employees);
        }

        public IActionResult Emp_Profile()
        {
            ViewBag.empList = _rawDataViewRepository.GetEmpList();
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;
            EmpProfileVM emp = new EmpProfileVM();
            return View(emp);
        }
        [HttpPost]
        public IActionResult Emp_Profile(string Period, DateTime? From, DateTime? To, int Emp)
        {
            //int id = int.Parse(Emp);
            int id = Emp;
            string comid = HttpContext.Session.GetString("comid");
            List<string> myList = new List<string> { "This Week", "This Month", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();

            ViewBag.period = mySelectListAsList;
            ViewBag.empList = _rawDataViewRepository.GetEmpList();

            var dateFrom = From.HasValue ? From.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dateTo = To.HasValue ? To.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            var data = _hRRepository.GetEmpProfile(dateFrom, dateTo, id).ToList();
            var attendanceDetails = _hRRepository.GetAttendanceDetails(Period, dateFrom, dateTo, id).ToList();

            int year = DateTime.Now.Year;
            var leaveDetails = _hRRepository.GetLeaveDetails(year, id).ToList();

            var paymentDetails = _hRRepository.GetPaymentDetails(id).ToList();
            var nomineeDetails = _hRRepository.GetNomineeDetails(id).ToList();
            var showcauseDetails = _hRRepository.GetShowCauseDetails(id).ToList();
            foreach (var item in showcauseDetails)
            {
                DateTime date;
                if (DateTime.TryParseExact(item.dtNotice, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    string formattedDate = date.ToString("dd-MMM-yyyy");
                    item.dtNotice = formattedDate;
                }
                if (DateTime.TryParseExact(item.dtEvent, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    string formattedDate = date.ToString("dd-MMM-yyyy");
                    item.dtEvent = formattedDate;
                }
            }
            if (showcauseDetails.Count() == 0)
            {
                var datam = new ShowCauseDetails();
                datam.dtNotice = " ";
                datam.dtEvent = " ";
                datam.Decision = " ";
                showcauseDetails.Add(datam);
            }

            var loanDetails = _hRRepository.GetLoanDetails(id).ToList();
            foreach (var item in loanDetails)
            {
                DateTime date1 = DateTime.ParseExact(item.DtLoanStart, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime currentDate = DateTime.Now;
                int monthDifference = (currentDate.Year - date1.Year) * 12 + (currentDate.Month - date1.Month);
                int lnAmount = (int)decimal.Parse(item.LoanAmount);
                int mnthlyAmount = (int)decimal.Parse(item.MonthlyLoanAmount);
                int temp = lnAmount - mnthlyAmount * monthDifference;
                if (temp > 0)
                {
                    item.DueAmount = temp.ToString();
                }
                else
                {
                    item.DueAmount = "Paid";
                }

                DateTime date;
                if (DateTime.TryParseExact(item.DtLoanStart, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    string formattedDate = date.ToString("dd-MMM-yyyy");
                    item.DtLoanStart = formattedDate;

                }
                DateTime date2;
                if (DateTime.TryParseExact(item.DtLoanEnd, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date2))
                {
                    string formattedDate = date2.ToString("dd-MMM-yyyy");
                    item.DtLoanEnd = formattedDate;

                }
            }
            if (loanDetails.Count() == 0)
            {
                var datam = new LoanDetails();
                datam.DtLoanStart = " ";
                datam.DueAmount = " ";
                datam.DtLoanEnd = " ";
                datam.DueAmount = " ";
                datam.LoanType = " ";
                loanDetails.Add(datam);
            }

            var strucDetails = _hRRepository.GetSalStrucDetails(id).ToList();
            var salList = new List<SalaryStructure>();
            foreach (var str in strucDetails)
            {
                var list = new SalaryStructure();
                list = EraseAfterFloating(str);
                salList.Add(list);
            }

            var taxDetails = _hRRepository.GetTaxDetails(id).ToList();


            EmpProfileVM empProfileVM = new EmpProfileVM();
            empProfileVM.EmpProfile = data;
            empProfileVM.AttendanceDetails = attendanceDetails;
            empProfileVM.LeaveDetails = leaveDetails;
            empProfileVM.PaymentDetails = paymentDetails;
            empProfileVM.NomineeDetails = nomineeDetails;
            empProfileVM.ShowCauseDetails = showcauseDetails;
            empProfileVM.LoanDetails = loanDetails;
            empProfileVM.SalaryStructure = salList;
            empProfileVM.TaxDetails = taxDetails;
            return View("_EmpProfilePartial", empProfileVM);
        }

        public SalaryStructure EraseAfterFloating(SalaryStructure obj)
        {
            int bsInInt = obj.BS.IndexOf('.');
            obj.BS = obj.BS.Substring(0, bsInInt) + "%";

            int hrInInt = obj.HR.IndexOf('.');
            obj.HR = obj.HR.Substring(0, hrInInt) + "%";

            int maInInt = obj.MA.IndexOf('.');
            obj.MA = obj.MA.Substring(0, maInInt) + "%";

            int caInInt = obj.CA.IndexOf('.');
            obj.CA = obj.CA.Substring(0, caInInt) + "%";

            int faInInt = obj.FA.IndexOf('.');
            obj.FA = obj.FA.Substring(0, faInInt) + "%";
            return obj;
        }

        #endregion

        #region Raw_Data_Transfer
        public IActionResult Raw_DataTransfer()
        {
            ViewBag.Title = "Raw Data Transfer";
            return View();
        }
        [HttpPost]
        public IActionResult Raw_DataTransfer(DateTime From, DateTime to)
        {
            _hRRepository.Raw_DataTransfer(From, to);
            ViewBag.Title = "Raw Data Transfer";
            return View();
        }
        #endregion

        #region ubl new module(kamrul Islam)

        public async Task<IActionResult> CreateCostEntry()
        {

            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = _context.Cat_Variable.Where(w => w.VarType == "CatagoryType").Select(x => new SelectListItem
            {
                Value = x.VarId.ToString(),
                Text = x.VarName
            }).ToList();


            var employeeList = _context.Cat_Unit
                .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(e => new SelectListItem
                {
                    Value = e.UnitId.ToString(),
                    Text = e.UnitName
                })
                .ToList();


            ViewData["Employee"] = employeeList;
            ViewData["CategoryId"] = data;
            return View();
        }



        [HttpPost]
        public IActionResult CreateCostEntry(PRcostEntry model)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            if (ModelState.IsValid)
            {
                if (model.id == null || model.id == 0)
                {
                    var exist = _context.PRcostEntry.Where(x => x.ComId == comid && x.UnitId == model.UnitId && x.CategoryId == model.CategoryId && x.IsDelete == false).FirstOrDefault();
                    if (exist == null)
                    {
                        model.ComId = comid;
                        model.UserId = userid;
                        model.DateAdded = DateTime.Now;
                        model.IsDelete = false;
                        model.UpdateByUserId = userid;

                        _context.PRcostEntry.Add(model);
                        _context.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Vendor already exists in this category.";
                        return RedirectToAction("CreateCostEntry");

                    }


                }
                else
                {
                    var exist = _context.PRcostEntry.Where(x => x.ComId == comid && x.id == model.id).FirstOrDefault();
                    if (exist != null)
                    {
                        exist.MedicalCost = model.MedicalCost;
                        //exist.CategoryId = model.CategoryId;
                        exist.DateUpdated = DateTime.Now;
                        exist.UpdateByUserId = userid;
                        //exist.UnitId = model.UnitId;
                        exist.Uniform = model.Uniform;
                        exist.ServiceComission = model.ServiceComission;
                        exist.SafetyShoe = model.SafetyShoe;

                    }

                    _context.PRcostEntry.Update(exist);
                    _context.SaveChanges();
                }

                return RedirectToAction("CostEntryList");
            }
            return View();
        }

        public IActionResult CostEntryDetails(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.PRcostEntry
                .Where(x => x.ComId == comid && x.id == id)
                .FirstOrDefault();

            var data = _context.Cat_Variable.Where(w => w.VarType == "CatagoryType").Select(x => new SelectListItem
            {
                Value = x.VarId.ToString(),
                Text = x.VarName
            }).ToList();


            var employeeList = _context.Cat_Unit
                .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(e => new SelectListItem
                {
                    Value = e.UnitId.ToString(),
                    Text = e.UnitName
                })
                .ToList();


            ViewData["Employee"] = employeeList;
            ViewData["CategoryId"] = data;

            return View("CreateCostEntry", exist);
        }


        public IActionResult CostEntryDelete(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.PRcostEntry.Where(x => x.ComId == comid && x.id == id).FirstOrDefault();
            if (exist != null)
            {
                exist.IsDelete = true;

            }
            _context.PRcostEntry.Update(exist);
            _context.SaveChanges();

            return RedirectToAction("CostEntryList");
        }

        public IActionResult CostEntryList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var data = (from pr in _context.PRcostEntry
                        join d in _context.Cat_Unit on pr.UnitId equals d.UnitId
                        join s in _context.Cat_Variable on pr.CategoryId equals s.VarId

                        where pr.ComId == comid && pr.IsDelete == false
                        orderby pr.id
                        select new PRcostEntryVM
                        {
                            id = pr.id,
                            UnitId = pr.UnitId,
                            Cat_Unit = d,
                            EmpName = d.UnitName,
                            CategoryId = pr.CategoryId,
                            CategoryName = s.VarName,
                            SafetyShoe = pr.SafetyShoe,
                            Uniform = pr.Uniform,
                            ServiceComission = pr.ServiceComission,
                            MedicalCost = pr.MedicalCost,

                        })
             .ToList();





            //var data = _context.PRcostEntry
            //    .Include(x => x.Cat_Unit) 
            //    .Include(x=> x.)
            //    .Where(x => x.ComId == comid && x.IsDelete == false)
            //    .ToList();

            return View(data);
        }
        public async Task<IActionResult> BOFuploader()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var employeeList = _context.HR_Emp_Info
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                 .Select(e => new SelectListItem
                 {
                     Value = e.EmpCode.ToString(),
                     Text = e.EmpName
                 })
                 .ToList();
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid && emp.IsDelete == false && emp.IsInactive == false
                           orderby emp.EmpId
                           select new SelectListItem
                           {
                               Value = emp.EmpCode,
                               Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["Employee"] = empInfo;

            // Rest of your code


            //ViewData["Employee"] = empInfo;

            // For Department



            var departmentList = _context.Cat_Department
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                })
                .ToList();
            ViewData["DeptId"] = departmentList;

            // For Designation

            // var comid = _httpContext.HttpContext.Session.GetString("comid");
            //var userid = _httpContext.HttpContext.Session.GetString("userid");
            //var data = _context.Cat_Variable.Where(w => w.ComId == comid && w.VarType == "JobNatureType" && w.IsDelete == false).Select(x => new SelectListItem
            //{
            //    Value = x.VarId.ToString(),
            //    Text = x.VarName
            //}).ToList();



            var designationList = _context.Cat_Designation
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(desig => new SelectListItem
                {
                    Value = desig.DesigId.ToString(),
                    Text = desig.DesigName
                })
                .ToList();
            ViewData["DesigId"] = designationList;
            return View();
        }

        [HttpPost]
        public IActionResult BOFuploader(Hr_BOFuploader model)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid").ToString();
            string userid = HttpContext.Session.GetString("userid").ToString();
            if (ModelState.IsValid)
            {
                if (model.id == 0 || model.id == null)
                {
                    var exist = _context.Hr_BOFuploader
                        .Where(x => x.ComId == comid && x.EmpCode == model.EmpCode
                        && x.DateAdded == model.DateAdded
                        && x.IsDelete == false)
                        .FirstOrDefault();

                    if (exist == null)
                    {
                        model.ComId = comid;
                        model.UserId = userid;
                        //model.DateAdded = DateTime.Now;
                        model.IsDelete = false;
                        model.UpdateByUserId = userid;
                        _context.Hr_BOFuploader.Add(model);
                        _context.SaveChanges();
                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Data Already Exist in this Date";
                        TempData["Status"] = "2";
                    }

                }
                else
                {
                    var exist = _context.Hr_BOFuploader
                        .Where(x => x.ComId == comid && x.id == model.id).FirstOrDefault();
                    if (exist != null)
                    {
                        exist.DeptId = model.DeptId;
                        exist.DesigId = model.DesigId;
                        exist.DateAdded = model.DateAdded;
                        exist.DateUpdated = DateTime.Now;
                        exist.UpdateByUserId = userid;
                        exist.FgDispatch1st = model.FgDispatch1st;
                        exist.FgDispatch2nd = model.FgDispatch2nd;
                        exist.TotalEarnde = model.TotalEarnde;
                        exist.Glycerin = model.Glycerin;
                        exist.Unloading = model.Unloading;

                    }
                    _context.Hr_BOFuploader.Update(exist);
                    _context.SaveChanges();
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "1";
                }


                //_yourRepository.Save();

                return RedirectToAction("BOFuploaderList");
            }


            //ViewData["CategoryId"] = _empInfoRepository.VendorCategory();
            //ViewData["Employee"] = _cat_UnitRepository.GetAll();
            return View();
        }

        public IActionResult BOFuploaderDetails(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.Hr_BOFuploader
                .Where(x => x.ComId == comid && x.id == id)
                .FirstOrDefault();

            var employeeList = _context.HR_Emp_Info
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                 .Select(e => new SelectListItem
                 {
                     Value = e.EmpCode.ToString(),
                     Text = e.EmpName
                 })
                 .ToList();


            ViewData["Employee"] = employeeList;

            // For Department
            var departmentList = _context.Cat_Department
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                })
                .ToList();
            ViewData["DeptId"] = departmentList;

            // For Designation
            var designationList = _context.Cat_Designation
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(desig => new SelectListItem
                {
                    Value = desig.DesigId.ToString(),
                    Text = desig.DesigName
                })
                .ToList();
            ViewData["DesigId"] = designationList;

            return View("BOFuploader", exist);
        }


        public IActionResult BOFuploaderDelete(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.Hr_BOFuploader.Where(x => x.ComId == comid && x.id == id).FirstOrDefault();
            if (exist != null)
            {
                exist.IsDelete = true;

            }
            _context.Hr_BOFuploader.Update(exist);
            _context.SaveChanges();

            return RedirectToAction("BOFuploaderList");
        }

        public IActionResult BOFuploaderList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            //var data = _context.Hr_BOFuploader            
            // .Include(x => x.Cat_Department)  
            // .Include(x => x.Cat_Designation)
            // .Where(x => x.ComId == comid && x.IsDelete == false)
            // .ToList();
            var data = (from uploader in _context.Hr_BOFuploader
                        join empInfo in _context.HR_Emp_Info on new { uploader.ComId, uploader.EmpCode } equals new { empInfo.ComId, empInfo.EmpCode }
                        where uploader.ComId == comid && uploader.IsDelete == false
                        select new Hr_BOFuploader
                        {
                            id = uploader.id,
                            EmpCode = uploader.EmpCode,
                            EmpName = empInfo.EmpName,
                            DeptId = uploader.DeptId,
                            Cat_Department = uploader.Cat_Department,
                            DesigId = uploader.DesigId,
                            Cat_Designation = uploader.Cat_Designation,
                            FgDispatch1st = uploader.FgDispatch1st,
                            FgDispatch2nd = uploader.FgDispatch2nd,
                            Glycerin = uploader.Glycerin,
                            Unloading = uploader.Unloading,
                            TotalEarnde = uploader.TotalEarnde,
                            DateAdded = uploader.DateAdded
                        })
             .ToList();



            return View(data);
        }


        [HttpPost]
        public async Task<IActionResult> BOFuploaderFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/BOFuploaderFile/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/BOFuploaderFile/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetBOFuploader(file.FileName);
                    if (addition.Count() > 0)
                    {
                        foreach (var newItem in addition)
                        {

                            var exist = _context.Hr_BOFuploader
                                            .FirstOrDefault(e =>
                                                e.EmpCode == newItem.EmpCode &&
                                                e.ComId == newItem.ComId && e.DateAdded == newItem.DateAdded && e.IsDelete == false);
                            if (exist != null)
                            {
                                exist.DeptId = newItem.DeptId;
                                exist.DesigId = newItem.DesigId;
                                exist.DateUpdated = newItem.DateUpdated;
                                exist.DateAdded = newItem.DateAdded;
                                exist.UpdateByUserId = userid;
                                exist.FgDispatch1st = newItem.FgDispatch1st;
                                exist.FgDispatch2nd = newItem.FgDispatch2nd;
                                exist.TotalEarnde = newItem.TotalEarnde;
                                exist.Glycerin = newItem.Glycerin;
                                exist.Unloading = newItem.Unloading;

                                _context.Hr_BOFuploader.Update(exist);
                            }
                            else
                            {

                                _context.Hr_BOFuploader.Add(newItem);
                            }


                        }

                        await _context.SaveChangesAsync();


                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            //return RedirectToAction("Index");
            return RedirectToAction("BOFuploaderList");
        }


        private List<Hr_BOFuploader> GetBOFuploader(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/BOFuploaderFile/" + comid + "/" + userid + "/" + fName);

            List<Hr_BOFuploader> transferVariables = new List<Hr_BOFuploader>();

            try
            {
                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader.Read() && reader != null)
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                // Check if all values in the row are null
                                bool allValuesNull = true;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (reader.GetValue(i) != null)
                                    {
                                        allValuesNull = false;
                                        break;
                                    }
                                }

                                if (!allValuesNull)
                                {
                                    Hr_BOFuploader transferVariable = new Hr_BOFuploader()
                                    {
                                        DateAdded = ParseDateTime(reader.GetValue(0)) ?? DateTime.Now,
                                        EmpCode = reader.GetValue(1)?.ToString(),
                                        DeptId = GetDepartmentId(reader.GetValue(4)?.ToString()) ?? 0,
                                        DesigId = GetDesignationId(reader.GetValue(3)?.ToString()) ?? 0,
                                        FgDispatch1st = ParseDouble(reader.GetValue(5)?.ToString()) ?? 0.0,
                                        FgDispatch2nd = ParseDouble(reader.GetValue(6)?.ToString()) ?? 0.0,
                                        Glycerin = ParseDouble(reader.GetValue(7)?.ToString()) ?? 0.0,
                                        Unloading = ParseDouble(reader.GetValue(8)?.ToString()) ?? 0.0,
                                        TotalEarnde = ParseDouble(reader.GetValue(9)?.ToString()) ?? 0.0,
                                        UserId = "DefaultUserId",
                                        ComId = HttpContext.Session.GetString("comid"),
                                        UpdateByUserId = HttpContext.Session.GetString("userid") ?? "DefaultUpdateByUserId",
                                        IsDelete = false,
                                        DateUpdated = DateTime.Now
                                    };

                                    transferVariables.Add(transferVariable);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }


            return transferVariables;
        }


        private double? ParseDouble(string value)
        {
            if (double.TryParse(value, out double result))
            {
                return result;
            }
            return null;
        }
        private int? GetDepartmentId(string departmentName)
        {
            var comid = HttpContext.Session.GetString("comid").ToString();
            var department = _context.Cat_Department
                .Where(x => x.ComId == comid && x.DeptName == departmentName)
                .FirstOrDefault();
            return department?.DeptId ?? 0;
        }


        private int? GetDesignationId(string designationName)
        {
            var comid = HttpContext.Session.GetString("comid").ToString();
            var designation = _context.Cat_Designation
                .Where(x => x.ComId == comid && x.DesigName == designationName)
                .FirstOrDefault();
            return designation?.DesigId ?? 0;
        }



        private DateTime? ParseDateTime(object value)
        {
            if (value != null && DateTime.TryParse(value.ToString(), out DateTime result))
            {
                return result;
            }
            return null;
        }




        public async Task<IActionResult> ProductionConstEntry()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            //var employeeList = _context.HR_Emp_Info
            //     .Where(x => x.ComId == comid && x.IsDelete == false)
            //     .Select(e => new SelectListItem
            //     {
            //         Value = e.EmpCode.ToString(),
            //         Text = e.EmpName
            //     })
            //     .ToList();
            //var empInfo = (from emp in _context.HR_Emp_Info
            //               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
            //               join s in _context.Cat_Section on emp.SectId equals s.SectId
            //               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
            //               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

            //               where emp.ComId == comid && emp.IsDelete == false && emp.IsInactive == false
            //               orderby emp.EmpId
            //               select new SelectListItem
            //               {
            //                   Value = emp.EmpCode,
            //                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
            //               }).ToList();

            //ViewData["Employee"] = empInfo;

            //// Rest of your code


            ////ViewData["Employee"] = empInfo;

            //// For Department
            //var departmentList = _context.Cat_Department
            //     .Where(x => x.ComId == comid && x.IsDelete == false)
            //    .Select(d => new SelectListItem
            //    {
            //        Value = d.DeptId.ToString(),
            //        Text = d.DeptName
            //    })
            //    .ToList();
            //ViewData["DeptId"] = departmentList;

            //// For Designation
            //var designationList = _context.Cat_Designation
            //     .Where(x => x.ComId == comid && x.IsDelete == false)
            //    .Select(desig => new SelectListItem
            //    {
            //        Value = desig.DesigId.ToString(),
            //        Text = desig.DesigName
            //    })
            //    .ToList();
            //ViewData["DesigId"] = designationList;
            return View();
        }

        [HttpPost]
        public IActionResult ProductionConstEntry(ProductionCostEntry model)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid").ToString();
            string userid = HttpContext.Session.GetString("userid").ToString();
            if (ModelState.IsValid)
            {
                if (model.Id == 0 || model.Id == null)
                {
                    var exist = _context.ProductionCostEntry.Where(x => x.ComId == comid && x.CreateDate == model.CreateDate && x.IsDelete == false).FirstOrDefault();
                    if (exist == null)
                    {
                        model.ComId = comid;
                        model.UserId = userid;
                        model.DateAdded = DateTime.Now;
                        model.IsDelete = false;
                        model.UpdateByUserId = userid;
                        _context.ProductionCostEntry.Add(model);
                        _context.SaveChanges();

                        TempData["Message"] = "Create Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Already Exist in this Date";
                        TempData["Status"] = "2";
                    }

                }
                else
                {
                    var exist = _context.ProductionCostEntry
                        .Where(x => x.ComId == comid && x.Id == model.Id).FirstOrDefault();
                    if (exist != null)
                    {
                        // exist.CreateDate = model.CreateDate;
                        exist.Boundle = model.Boundle;
                        exist.BoxPacketReel = model.BoxPacketReel;
                        exist.Bags = model.Bags;
                        exist.Drum = model.Drum;


                    }
                    _context.ProductionCostEntry.Update(exist);
                    _context.SaveChanges();
                }


                //_yourRepository.Save();

                return RedirectToAction("ProductionConstEntryList");
            }


            //ViewData["CategoryId"] = _empInfoRepository.VendorCategory();
            //ViewData["Employee"] = _cat_UnitRepository.GetAll();
            return View();
        }

        public IActionResult ProductionConstEntryDetails(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.ProductionCostEntry
                .Where(x => x.ComId == comid && x.Id == id)
                .FirstOrDefault();

            var employeeList = _context.HR_Emp_Info
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                 .Select(e => new SelectListItem
                 {
                     Value = e.EmpCode.ToString(),
                     Text = e.EmpName
                 })
                 .ToList();


            ViewData["Employee"] = employeeList;

            // For Department
            var departmentList = _context.Cat_Department
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                })
                .ToList();
            ViewData["DeptId"] = departmentList;

            // For Designation
            var designationList = _context.Cat_Designation
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(desig => new SelectListItem
                {
                    Value = desig.DesigId.ToString(),
                    Text = desig.DesigName
                })
                .ToList();
            ViewData["DesigId"] = designationList;

            return View("ProductionConstEntry", exist);
        }


        public IActionResult ProductionConstEntryDelete(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.ProductionCostEntry.Where(x => x.ComId == comid && x.Id == id).FirstOrDefault();
            if (exist != null)
            {
                exist.IsDelete = true;

            }
            _context.ProductionCostEntry.Update(exist);
            _context.SaveChanges();

            return RedirectToAction("ProductionConstEntryList");
        }

        public IActionResult ProductionConstEntryList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var data = _context.ProductionCostEntry
             .Where(x => x.ComId == comid && x.IsDelete == false)
             .ToList();
            //var data = (from uploader in _context.ProductionCostEntry
            //            join empInfo in _context.HR_Emp_Info on new { uploader.ComId, uplo } equals new { empInfo.ComId, empInfo.EmpCode }
            //            where uploader.ComId == comid && uploader.IsDelete == false
            //            select new ProductionCostEntry
            //            {
            //                Id = uploader.Id,
            //                CreateDate=uploader.CreateDate,
            //                Boundle=uploader.Boundle,
            //                BoxPacketReel=uploader.BoxPacketReel,
            //                Bags=uploader.Bags,
            //                Drum=uploader.Drum,
            //            })
            // .ToList();



            return View(data);
        }




        //DailyProductionRules


        public async Task<IActionResult> DailyProductionRules()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            //var employeeList = _context.HR_Emp_Info
            //     .Where(x => x.ComId == comid && x.IsDelete == false)
            //     .Select(e => new SelectListItem
            //     {
            //         Value = e.EmpCode.ToString(),
            //         Text = e.EmpName
            //     })
            //     .ToList();
            //var empInfo = (from emp in _context.HR_Emp_Info
            //               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
            //               join s in _context.Cat_Section on emp.SectId equals s.SectId
            //               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
            //               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

            //               where emp.ComId == comid && emp.IsDelete == false && emp.IsInactive == false
            //               orderby emp.EmpId
            //               select new SelectListItem
            //               {
            //                   Value = emp.EmpCode,
            //                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
            //               }).ToList();

            //ViewData["Employee"] = empInfo;

            //// Rest of your code


            ////ViewData["Employee"] = empInfo;

            //// For Department
            //var departmentList = _context.Cat_Department
            //     .Where(x => x.ComId == comid && x.IsDelete == false)
            //    .Select(d => new SelectListItem
            //    {
            //        Value = d.DeptId.ToString(),
            //        Text = d.DeptName
            //    })
            //    .ToList();
            //ViewData["DeptId"] = departmentList;

            //// For Designation
            //var designationList = _context.Cat_Designation
            //     .Where(x => x.ComId == comid && x.IsDelete == false)
            //    .Select(desig => new SelectListItem
            //    {
            //        Value = desig.DesigId.ToString(),
            //        Text = desig.DesigName
            //    })
            //    .ToList();
            //ViewData["DesigId"] = designationList;
            return View();
        }

        [HttpPost]
        public IActionResult DailyProductionRules(DailyProductionRules model)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid").ToString();
            string userid = HttpContext.Session.GetString("userid").ToString();
            if (ModelState.IsValid)
            {
                if (model.Id == 0 || model.Id == null)
                {
                    model.ComId = comid;
                    model.UserId = userid;
                    model.DateAdded = DateTime.Now;
                    model.IsDelete = false;
                    model.UpdateByUserId = userid;
                    _context.DailyProductionRules.Add(model);
                    _context.SaveChanges();
                }
                else
                {
                    var exist = _context.DailyProductionRules
                        .Where(x => x.ComId == comid && x.Id == model.Id).FirstOrDefault();
                    if (exist != null)
                    {
                        exist.CreateDate = model.CreateDate;
                        exist.FgDisPatch1st = model.FgDisPatch1st;
                        exist.FgDispatch2nd = model.FgDispatch2nd;
                        exist.Boo2 = model.Boo2;
                        exist.unloading = model.unloading;
                        exist.OtherCost = model.OtherCost;


                    }
                    _context.DailyProductionRules.Update(exist);
                    _context.SaveChanges();
                }


                //_yourRepository.Save();

                return RedirectToAction("DailyProductionRulesList");
            }


            //ViewData["CategoryId"] = _empInfoRepository.VendorCategory();
            //ViewData["Employee"] = _cat_UnitRepository.GetAll();
            return View();
        }

        public IActionResult DailyProductionRulesDetails(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.DailyProductionRules
                .Where(x => x.ComId == comid && x.Id == id)
                .FirstOrDefault();

            var employeeList = _context.HR_Emp_Info
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                 .Select(e => new SelectListItem
                 {
                     Value = e.EmpCode.ToString(),
                     Text = e.EmpName
                 })
                 .ToList();


            ViewData["Employee"] = employeeList;

            // For Department
            var departmentList = _context.Cat_Department
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                })
                .ToList();
            ViewData["DeptId"] = departmentList;

            // For Designation
            var designationList = _context.Cat_Designation
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(desig => new SelectListItem
                {
                    Value = desig.DesigId.ToString(),
                    Text = desig.DesigName
                })
                .ToList();
            ViewData["DesigId"] = designationList;

            return View("DailyProductionRules", exist);
        }


        public IActionResult DailyProductionRulesDelete(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.DailyProductionRules.Where(x => x.ComId == comid && x.Id == id).FirstOrDefault();
            if (exist != null)
            {
                exist.IsDelete = true;

            }
            _context.DailyProductionRules.Update(exist);
            _context.SaveChanges();

            return RedirectToAction("DailyProductionRulesList");
        }

        public IActionResult DailyProductionRulesList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var data = _context.DailyProductionRules
             .Where(x => x.ComId == comid && x.IsDelete == false)
             .ToList();
            //var data = (from uploader in _context.ProductionCostEntry
            //            join empInfo in _context.HR_Emp_Info on new { uploader.ComId, uplo } equals new { empInfo.ComId, empInfo.EmpCode }
            //            where uploader.ComId == comid && uploader.IsDelete == false
            //            select new ProductionCostEntry
            //            {
            //                Id = uploader.Id,
            //                CreateDate=uploader.CreateDate,
            //                Boundle=uploader.Boundle,
            //                BoxPacketReel=uploader.BoxPacketReel,
            //                Bags=uploader.Bags,
            //                Drum=uploader.Drum,
            //            })
            // .ToList();



            return View(data);
        }


        //PsUploader


        public async Task<IActionResult> PSUploader()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var employeeList = _context.HR_Emp_Info
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                 .Select(e => new SelectListItem
                 {
                     Value = e.EmpCode.ToString(),
                     Text = e.EmpName
                 })
                 .ToList();
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid && emp.IsDelete == false && emp.IsInactive == false
                           orderby emp.EmpId
                           select new SelectListItem
                           {
                               Value = emp.EmpCode,
                               Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["Employee"] = empInfo;

            // Rest of your code


            //ViewData["Employee"] = empInfo;

            // For Department
            var departmentList = _context.Cat_Department
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                })
                .ToList();
            ViewData["DeptId"] = departmentList;

            // For Designation
            var designationList = _context.Cat_Designation
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(desig => new SelectListItem
                {
                    Value = desig.DesigId.ToString(),
                    Text = desig.DesigName
                })
                .ToList();
            ViewData["DesigId"] = designationList;
            return View();
        }

        [HttpPost]
        public IActionResult PSUploader(PSuploader model)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid").ToString();
            string userid = HttpContext.Session.GetString("userid").ToString();
            if (ModelState.IsValid)
            {
                if (model.Id == 0 || model.Id == null)
                {
                    var exist = _context.PSuploader
                                            .Where(e => e.ComId == comid &&
                                                e.EmpCode == model.EmpCode &&
                                                e.DateAdded == model.DateAdded && e.IsDelete == false).FirstOrDefault();
                    if (exist == null)
                    {
                        model.ComId = comid;
                        model.UserId = userid;
                        model.DateAdded = model.DateAdded;
                        model.IsDelete = false;
                        model.UpdateByUserId = userid;
                        _context.PSuploader.Add(model);
                        _context.SaveChanges();
                        TempData["Message"] = "Create Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Data Already Exist in this Date";
                        TempData["Status"] = "2";
                    }


                }
                else
                {
                    var exist = _context.PSuploader
                        .Where(x => x.ComId == comid && x.Id == model.Id).FirstOrDefault();
                    if (exist != null)
                    {
                        exist.DeptId = model.DeptId;
                        exist.DesigId = model.DesigId;
                        // exist.DateAdded = model.DateAdded;
                        exist.DateUpdated = DateTime.Now;
                        exist.UpdateByUserId = userid;
                        exist.EmpCode = model.EmpCode;
                        exist.DtJoin = model.DtJoin;
                        exist.TotalPresent = model.TotalPresent;
                        exist.TotalAbsent = model.TotalAbsent;
                        exist.BoxPacketReels = model.BoxPacketReels;

                        exist.Drums = model.Drums;
                        exist.Bags = model.Bags;
                        exist.Unloads = model.Unloads;
                        exist.GsWages = model.GsWages;

                    }
                    _context.PSuploader.Update(exist);
                    _context.SaveChanges();
                    TempData["Message"] = "Update Successfully";
                    TempData["Status"] = "1";
                }


                //_yourRepository.Save();

                return RedirectToAction("PSUploaderList");
            }


            //ViewData["CategoryId"] = _empInfoRepository.VendorCategory();
            //ViewData["Employee"] = _cat_UnitRepository.GetAll();
            return View();
        }

        public IActionResult PSUploaderDetails(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.PSuploader
                .Where(x => x.ComId == comid && x.Id == id)
                .FirstOrDefault();

            var employeeList = _context.HR_Emp_Info
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                 .Select(e => new SelectListItem
                 {
                     Value = e.EmpCode.ToString(),
                     Text = e.EmpName
                 })
                 .ToList();


            ViewData["Employee"] = employeeList;

            // For Department
            var departmentList = _context.Cat_Department
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                })
                .ToList();
            ViewData["DeptId"] = departmentList;

            // For Designation
            var designationList = _context.Cat_Designation
                 .Where(x => x.ComId == comid && x.IsDelete == false)
                .Select(desig => new SelectListItem
                {
                    Value = desig.DesigId.ToString(),
                    Text = desig.DesigName
                })
                .ToList();
            ViewData["DesigId"] = designationList;

            return View("PSUploader", exist);
        }


        public IActionResult PSUploaderDelete(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid").ToString();
            var exist = _context.PSuploader.Where(x => x.ComId == comid && x.Id == id).FirstOrDefault();
            if (exist != null)
            {
                exist.IsDelete = true;

            }
            _context.PSuploader.Update(exist);
            _context.SaveChanges();

            return RedirectToAction("PSUploaderList");
        }

        public IActionResult PSUploaderList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            //var data = _context.Hr_BOFuploader            
            // .Include(x => x.Cat_Department)  
            // .Include(x => x.Cat_Designation)
            // .Where(x => x.ComId == comid && x.IsDelete == false)
            // .ToList();
            var data = (from uploader in _context.PSuploader
                        join empInfo in _context.HR_Emp_Info on new { uploader.ComId, uploader.EmpCode } equals new { empInfo.ComId, empInfo.EmpCode }
                        where uploader.ComId == comid && uploader.IsDelete == false
                        select new PSuploader
                        {
                            Id = uploader.Id,
                            EmpCode = uploader.EmpCode,
                            EmpName = empInfo.EmpName,
                            DtJoin = empInfo.DtJoin,
                            DeptId = uploader.DeptId,
                            Cat_Department = uploader.Cat_Department,
                            DesigId = uploader.DesigId,
                            Cat_Designation = uploader.Cat_Designation,
                            TotalAbsent = uploader.TotalAbsent,
                            TotalPresent = uploader.TotalPresent,
                            BoxPacketReels = uploader.BoxPacketReels,
                            Drums = uploader.Drums,
                            Bags = uploader.Bags,
                            Unloads = uploader.Unloads,
                            GsWages = uploader.GsWages,
                            DateAdded = uploader.DateAdded,

                        })
             .ToList();



            return View(data);
        }



        [HttpPost]
        public async Task<IActionResult> PSUploaderFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/PSuploaderFile/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/PSuploaderFile/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetPSUploader(file.FileName);
                    if (addition.Count() > 0)
                    {
                        foreach (var newItem in addition)
                        {

                            var exist = _context.PSuploader
                                            .FirstOrDefault(e =>
                                                e.EmpCode == newItem.EmpCode &&
                                                e.ComId == newItem.ComId && e.DateAdded == newItem.DateAdded && e.IsDelete == false);
                            if (exist != null)
                            {
                                exist.DeptId = newItem.DeptId;
                                exist.DesigId = newItem.DesigId;
                                exist.DateUpdated = newItem.DateUpdated;
                                // exist.DateAdded = newItem.DateAdded;
                                exist.UpdateByUserId = userid;

                                exist.DtJoin = newItem.DtJoin;
                                exist.TotalPresent = newItem.TotalPresent;
                                exist.TotalAbsent = newItem.TotalAbsent;
                                exist.BoxPacketReels = newItem.BoxPacketReels;

                                exist.Drums = newItem.Drums;
                                exist.Bags = newItem.Bags;
                                exist.Unloads = newItem.Unloads;
                                exist.GsWages = newItem.GsWages;

                                _context.PSuploader.Update(exist);
                            }
                            else
                            {

                                await _context.PSuploader.AddRangeAsync(newItem);
                            }


                        }

                        await _context.SaveChangesAsync();


                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            //return RedirectToAction("Index");
            return RedirectToAction("PSUploaderList");
        }


        private List<PSuploader> GetPSUploader(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/PSuploaderFile/" + comid + "/" + userid + "/" + fName);

            List<PSuploader> transferVariables = new List<PSuploader>();

            try
            {
                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader.Read() && reader != null)
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                // Check if all values in the row are null
                                bool allValuesNull = true;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (reader.GetValue(i) != null)
                                    {
                                        allValuesNull = false;
                                        break;
                                    }
                                }

                                if (!allValuesNull)
                                {
                                    PSuploader transferVariable = new PSuploader()
                                    {
                                        DateAdded = ParseDateTime(reader.GetValue(0)) ?? DateTime.Now,
                                        EmpCode = reader.GetValue(1)?.ToString(),
                                        DtJoin = ParseDateTime(reader.GetValue(3)) ?? DateTime.Now,
                                        DeptId = GetDepartmentId(reader.GetValue(4)?.ToString()) ?? 0,
                                        DesigId = GetDesignationId(reader.GetValue(5)?.ToString()) ?? 0,
                                        TotalPresent = ParseDouble(reader.GetValue(6)?.ToString()) ?? 0.0,
                                        TotalAbsent = ParseDouble(reader.GetValue(7)?.ToString()) ?? 0.0,
                                        BoxPacketReels = ParseDouble(reader.GetValue(8)?.ToString()) ?? 0.0,
                                        Drums = ParseDouble(reader.GetValue(9)?.ToString()) ?? 0.0,
                                        Bags = ParseDouble(reader.GetValue(10)?.ToString()) ?? 0.0,
                                        Unloads = ParseDouble(reader.GetValue(11)?.ToString()) ?? 0.0,
                                        GsWages = ParseDouble(reader.GetValue(12)?.ToString()) ?? 0.0,
                                        UserId = "DefaultUserId",
                                        ComId = HttpContext.Session.GetString("comid"),
                                        UpdateByUserId = HttpContext.Session.GetString("userid") ?? "DefaultUpdateByUserId",
                                        IsDelete = false,
                                        DateUpdated = DateTime.Now
                                    };

                                    transferVariables.Add(transferVariable);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }


            return transferVariables;
        }

        #endregion
        #region transferEmployee (asad_iiuc)
        
        public IActionResult EmployeeTransfer()
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.comid = comid;
            return View();
        }
        public IActionResult GetDepartment(string companyId)
        {
            var deptList = _departmentRepository.GetDepartmentByCompany(companyId);
            return Json(deptList);
        }
        public IActionResult GetDesignation(string companyId)
        {
            var desigList = _designationRepository.GetDesignationsByCompany(companyId);
            return Json(desigList);
        }
        public IActionResult GetSection(string companyId)
        {
            var sectionList = _sectionRepository.GetSectionByCompany(companyId);
            return Json(sectionList);
        }
        public IActionResult GetShift(string companyId)
        {
            var shiftList = _shiftRepository.GetShiftByCompany(companyId);
            return Json(shiftList);
        }
        public IActionResult GetUnit(string companyId)
        {
            var unitList = _cat_UnitRepository.GetAllUnitbyCompany(companyId);
            return Json(unitList);
        }
        public IActionResult GetEmpType(string companyId)
        {
            var typeList = _context.Cat_Emp_Type.ToList();
            return Json(typeList);
        }
        public IActionResult GetLine(string companyId)
        {
            var LineList = _context.Cat_Line.Where(x => x.ComId == companyId).ToList();
            return Json(LineList);
        }
        public IActionResult GetFloor(string companyId)
        {
            var floorList = _context.Cat_Floor.Where(x => x.ComId == companyId).ToList();
            return Json(floorList);
        }
        public IActionResult SaveEmpTransfer(EmployeeTransferViewModel empDetails)
        {
            var comid = HttpContext.Session.GetString("comid");
            if (empDetails != null)
            {
                var Employee_Transfer = new HR_Employee_Transfer();
                Employee_Transfer.OldComId = comid.ToString();
                Employee_Transfer.NewComId = empDetails.NewComId;
                Employee_Transfer.EmpId = empDetails.EmpId;
                Employee_Transfer.EmpName = empDetails.EmpName;
                Employee_Transfer.NewEmpCode = empDetails.NewEmpCode;
                Employee_Transfer.NewDeptId = empDetails.NewDeptId;
                Employee_Transfer.NewDesigId = empDetails.NewDesigId;
                Employee_Transfer.NewSectId = empDetails.NewSectId;
                Employee_Transfer.NewShiftId = empDetails.NewShiftId;
                Employee_Transfer.NewLineId = empDetails.NewLineId;
                Employee_Transfer.NewFloorId = empDetails.NewFloorId;
                Employee_Transfer.NewEmpTypeId = empDetails.NewEmpTypeId;
                Employee_Transfer.NewUnitId = empDetails.NewUnitId;
                Employee_Transfer.FingerId = empDetails.FingerId;
                Employee_Transfer.TransferDate = empDetails.TransferDate;

                _context.HR_Employee_Transfer.Add(Employee_Transfer);
                _context.SaveChanges();
                //SqlParameter[] sqlParemeter = new SqlParameter[3];
                //SqlParameter[0] = new SqlParameter("@OldComId", comid.ToString());
                //SqlParameter p2 = new SqlParameter("@NewComid", empDetails.NewComId);
                //SqlParameter p3 = new SqlParameter("@EmpId", empDetails.EmpId);
                //SqlParameter p4 = new SqlParameter("@NewDeptId", empDetails.NewDeptId);
                //SqlParameter p5 = new SqlParameter("@NewSectId", empDetails.NewSectId);
                //SqlParameter p6 = new SqlParameter("@NewDesigId", empDetails.NewDesigId);
                //SqlParameter p7 = new SqlParameter("@NewShiftId", empDetails.NewShiftId);
                //SqlParameter p8 = new SqlParameter("@NewLineId", empDetails.NewLineId);
                //SqlParameter p9 = new SqlParameter("@NewFloorId", empDetails.NewFloorId);
                //SqlParameter p10 = new SqlParameter("@NewEmpTypeId", empDetails.NewEmpTypeId);
                //SqlParameter p11 = new SqlParameter("@NewUnitId", empDetails.NewUnitId);
                //SqlParameter p12 = new SqlParameter("@FingerId", empDetails.FingerId);
                //SqlParameter p13 = new SqlParameter("@TransferId", empDetails.TransferDate);


                //kamrul
                SqlParameter[] sqlParameters = new SqlParameter[12];

                // Initialize SqlParameter objects with values
                sqlParameters[0] = new SqlParameter("@ComId", comid.ToString());
                sqlParameters[1] = new SqlParameter("@NewComId", empDetails.NewComId);
                sqlParameters[2] = new SqlParameter("@EmpId", empDetails.EmpId);
                sqlParameters[3] = new SqlParameter("@DeptId", empDetails.NewDeptId);
                sqlParameters[4] = new SqlParameter("@SectId", empDetails.NewSectId);
                sqlParameters[5] = new SqlParameter("@DesigId", empDetails.NewDesigId);
                sqlParameters[6] = new SqlParameter("@ShiftId", empDetails.NewShiftId);
                sqlParameters[7] = new SqlParameter("@LineId", empDetails.NewLineId);
                sqlParameters[8] = new SqlParameter("@FloorId", empDetails.NewFloorId);
                sqlParameters[9] = new SqlParameter("@EmpTypeId", empDetails.NewEmpTypeId);
                sqlParameters[10] = new SqlParameter("@FingerId", empDetails.FingerId);
                sqlParameters[11] = new SqlParameter("@dtTransfer", empDetails.TransferDate);

                string query = $"Exec HR_PrcProcessTransfer '{comid}', '{empDetails.NewComId}', {empDetails.EmpId}, {empDetails.NewDeptId}, " +
               $"{empDetails.NewSectId}, {empDetails.NewDesigId}, {empDetails.NewShiftId}, {empDetails.NewLineId}, " +
               $"{empDetails.NewFloorId}, {empDetails.NewEmpTypeId}, '{empDetails.FingerId}', '{empDetails.TransferDate}'";

                // Call the ExecProc method with the commandText and sqlParameters
                string result = Helper.ExecProc("HR_PrcProcessTransfer", sqlParameters);




                //HR_PrcProcessTransfer
                //SqlParameter[] sqlParemeter = new SqlParameter[3];
                //sqlParemeter[0] = new SqlParameter("@ComID", comid);
                //sqlParemeter[1] = new SqlParameter("@EmpID", hrEmpInfo.EmpId);
                //sqlParemeter[2] = new SqlParameter("@dtJoin", hrEmpInfo.DtJoin);

                //string query = $"Exec HR_prcProcessLeaveInput '{comid}', {hrEmpInfo.EmpId}, '{hrEmpInfo.DtJoin}'";
                //Helper.ExecProcMapT<EmployeeInfo>("HR_prcProcessLeaveInput", sqlParemeter);

                return Ok(true);
            }

            return Ok(false);
        }
        #endregion

    }
    public class EmployeeTransferViewModel
    {
        public string NewComId { get; set; }
        public int EmpId { get; set; }
        public string NewEmpCode { get; set; }
        public string EmpName { get; set; }
        public int? NewUnitId { get; set; }
        public int? NewDeptId { get; set; }
        public int? NewSectId { get; set; }
        public int? NewDesigId { get; set; }
        public int? NewShiftId { get; set; }
        public int? NewEmpTypeId { get; set; }
        public int? NewFloorId { get; set; }
        public int? NewLineId { get; set; }
        public int? FingerId { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;
    }
}
