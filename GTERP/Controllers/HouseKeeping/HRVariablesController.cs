#region Assembly
using GTERP.BLL;
using GTERP.Interfaces;
using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using static GTERP.Controllers.COM_ProformaInvoiceController;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using GTERP.ViewModels;
using Nancy.Extensions;
using Microsoft.Data.SqlClient;
using GTERP.Models.Common;
using DocumentFormat.OpenXml.Bibliography;
using System.Threading.Tasks;
using System.IO;
using ExcelDataReader;
using System.Data;
using ClosedXML.Excel;
using System.Globalization;
#endregion

namespace GTERP.Controllers.HouseKeeping
{
    //[OverridableAuthorize(typeof(OverridableAuthorize))]
    //[TypeFilter(typeof(OverridableAuthorize))] --- working for onoverrride 
    [OverridableAuthorize]
    public class HRVariablesController : Controller
    {
        #region Common Property
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IBloodGroupRepository _bloodGroupRepository;
        private readonly IReligionRepository _religionRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly ICat_UnitRepository _catUnitRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly IBuildingTypeRepository _buildingTypeRepository;
        private readonly IEmpTypeRepository _empTypeRepository;
        private readonly IBusStopRepository _busStopRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IPoliceStationRepository _policeStationRepository;
        private readonly IPostOfficeRepository _postOfficeRepository;
        private readonly IStyleRepository _styleRepository;
        private readonly IWareHouseRepository _wareHouseRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ISummerWinterAllowanceRepository _summerWinterAllowanceRepository;
        private readonly IComputationSettingRepository _iTComputationSettingRepository;
        private readonly IGasChargeSettingRepository _gasChargeSettingRepository;
        private readonly ITaxBankRepository _taxBankRepository;
        private readonly IIncenBandRepository _incenBandRepository;
        private readonly IInsureGradeRepository _insureGradeRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IElectricChargeSettingRepository _electricChargeSettingRepository;
        private readonly IDiagnosisRepository _diagnosisRepository;
        private readonly ISignatoryRepository _signatoryRepository;
        private readonly IHRExpSettingRepository _hRExpSettingRepository;
        private readonly IHRSettingRepository _hRSettingRepository;
        private readonly IHRReportRepository _hRReportRepository;
        private readonly IHRCustomReportRepository _hRCustomReportRepository;
        private readonly IStrengthRepository _strengthRepository;
        private readonly ICatVariableRepository _catVariableRepository;
        private readonly ICat_AttBonusSetting _Cat_AttBonusSetting;
        private readonly ICat_Stamp _Cat_Stamp;

        private readonly ISubSectionRepository _subSectionRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IHROvertimeSettingRepository _hROvertimeSettingRepository;
        private readonly ITaxAmountSettingRepository _taxAmountSettingRepository;
        private readonly IHR_ApprovalSettingRepository _hR_ApprovalSettingRepository;
        private readonly IConfiguration _configuration;
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext _context;
        private readonly IIncomeTaxRepository _incomeTaxRepository;
        private readonly ISalaryMonthRepository _salaryMonthRepository;
        #endregion

        #region Constructor
        public HRVariablesController(TransactionLogRepository tran,
            IDepartmentRepository departmentRepository,
            ISectionRepository sectionRepository,
            IDesignationRepository designationRepository,
            IGradeRepository gradeRepository,
            IBloodGroupRepository bloodGroupRepository,
            IReligionRepository religionRepository,
            ILineRepository lineRepository,
            IShiftRepository shiftRepository,
            ISkillRepository skillRepository,
            IFloorRepository floorRepository,
            ICat_UnitRepository catUnitRepository,
            IBankRepository bankRepository,
            IBankBranchRepository bankBranchRepository,
            IBuildingTypeRepository buildingTypeRepository,
            IEmpTypeRepository empTypeRepository,
            IBusStopRepository busStopRepository,
            IMeetingRepository meetingRepository,
            ILocationRepository locationRepository,
            IDistrictRepository districtRepository,
            IPoliceStationRepository policeStationRepository,
            IPostOfficeRepository postOfficeRepository,
            IStyleRepository styleRepository,
            IUnitRepository unitRepository,
            ISupplierRepository supplierRepository,
            ICountryRepository countryRepository,
            ISummerWinterAllowanceRepository summerWinterAllowanceRepository,
            IComputationSettingRepository iTComputationSettingRepository,
            IGasChargeSettingRepository gasChargeSettingRepository,
            ITaxBankRepository taxBankRepository,
            IIncenBandRepository incenBandRepository,
            IInsureGradeRepository insureGradeRepository,
            IExchangeRateRepository exchangeRateRepository,
            IElectricChargeSettingRepository electricChargeSettingRepository,
            IDiagnosisRepository diagnosisRepository,
            ISignatoryRepository signatoryRepository,
            IWareHouseRepository wareHouseRepository,
            IHRExpSettingRepository hRExpSettingRepository,
            IHRSettingRepository hRSettingRepository,
            IHRReportRepository hRReportRepository,
            IHRCustomReportRepository hRCustomReportRepository,
            IStrengthRepository strengthRepository,
            ICatVariableRepository catVariableRepository,
            ISubSectionRepository subSectionRepository,
            ISizeRepository sizeRepository,
            IColorRepository colorRepository,
            GTRDBContext db,
            IHROvertimeSettingRepository hROvertimeSettingRepository,
            ITaxAmountSettingRepository taxAmountSettingRepository,
            IHR_ApprovalSettingRepository hR_ApprovalSettingRepository,
            IConfiguration configuration,
            IIncomeTaxRepository incomeTaxRepository,
            ISalaryMonthRepository salaryMonthRepository,
            ICat_AttBonusSetting Cat_AttBonusSetting,
            ICat_Stamp Cat_Stamp

            )
        {
            _context = db;
            tranlog = tran;
            _departmentRepository = departmentRepository;
            _sectionRepository = sectionRepository;
            _designationRepository = designationRepository;
            _gradeRepository = gradeRepository;
            _bloodGroupRepository = bloodGroupRepository;
            _religionRepository = religionRepository;
            _lineRepository = lineRepository;
            _shiftRepository = shiftRepository;
            _skillRepository = skillRepository;
            _floorRepository = floorRepository;
            _catUnitRepository = catUnitRepository;
            _bankRepository = bankRepository;
            _bankBranchRepository = bankBranchRepository;
            _buildingTypeRepository = buildingTypeRepository;
            _empTypeRepository = empTypeRepository;
            _busStopRepository = busStopRepository;
            _meetingRepository = meetingRepository;
            _locationRepository = locationRepository;
            _districtRepository = districtRepository;
            _policeStationRepository = policeStationRepository;
            _postOfficeRepository = postOfficeRepository;
            _styleRepository = styleRepository;
            _unitRepository = unitRepository;
            _supplierRepository = supplierRepository;
            _countryRepository = countryRepository;
            _summerWinterAllowanceRepository = summerWinterAllowanceRepository;
            _iTComputationSettingRepository = iTComputationSettingRepository;
            _gasChargeSettingRepository = gasChargeSettingRepository;
            _taxBankRepository = taxBankRepository;
            _incenBandRepository = incenBandRepository;
            _insureGradeRepository = insureGradeRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _electricChargeSettingRepository = electricChargeSettingRepository;
            _diagnosisRepository = diagnosisRepository;
            _signatoryRepository = signatoryRepository;
            _wareHouseRepository = wareHouseRepository;
            _hRExpSettingRepository = hRExpSettingRepository;
            _hRSettingRepository = hRSettingRepository;
            _hRReportRepository = hRReportRepository;
            _hRCustomReportRepository = hRCustomReportRepository;
            _strengthRepository = strengthRepository;
            _catVariableRepository = catVariableRepository;
            _subSectionRepository = subSectionRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
            _hROvertimeSettingRepository = hROvertimeSettingRepository;
            _taxAmountSettingRepository = taxAmountSettingRepository;
            _hR_ApprovalSettingRepository = hR_ApprovalSettingRepository;
            _configuration = configuration;
            _incomeTaxRepository = incomeTaxRepository;
            _salaryMonthRepository = salaryMonthRepository;
            _Cat_AttBonusSetting = Cat_AttBonusSetting;
            _Cat_Stamp = Cat_Stamp;
        }
        #endregion


        #region Department

        public IActionResult DepartmentList()
        {
            string comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            var data = _departmentRepository.GetAll();
            foreach (var item in data)
            {
                var hodinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.EmpId == item.DeptHODId && x.IsDelete == false && x.IsInactive == false).FirstOrDefault();
                if (hodinfo != null)
                {
                    item.DeptHODName = hodinfo.EmpCode + "_" + hodinfo.EmpName;
                }
            }
            return View(data);
        }



        public IActionResult CreateDepartment(int Id = 0)
        {
            ViewBag.Title = "Create";

            return View(new Cat_Department());
        }

        public ActionResult CreateDepartmentPartial()
        {
            var department = new Cat_Department();
            return PartialView("_CreateDepartment", department);
        }

        public ActionResult CreateDe(Cat_Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRepository.Add(department);
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public IActionResult CreateDepartment(Cat_Department department)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (ModelState.IsValid)
            {
                if (department.DeptId > 0)
                {
                    _departmentRepository.Update(department);

                    var emplist = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.DeptId == department.DeptId).ToList();
                    if (emplist != null)
                    {
                        foreach (var emp in emplist)
                        {
                            emp.FinalAprvId = department.DeptHODId;

                        }
                        _context.SaveChanges();
                    }


                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), department.DeptId.ToString(), "Update", department.DeptName.ToString());

                }
                else
                {
                    _departmentRepository.Add(department);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), department.DeptId.ToString(), "Create", department.DeptName.ToString());

                }
                return RedirectToAction("DepartmentList", "HRVariables");
            }
            return View(department);
        }

        public IActionResult EditDepartment(int? id)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }

            var department = _departmentRepository.FindById(id);
            if (department.DeptHODId != null)
            {

                var hodinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.EmpId == department.DeptHODId && x.IsDelete == false && x.IsInactive == false).FirstOrDefault();
                if (hodinfo != null)
                {
                    department.DeptHODName = hodinfo.EmpCode + "_" + hodinfo.EmpName;
                }
            }
            if (department.DeptCLevelId != null)
            {
                var clevelinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.EmpId == department.DeptCLevelId && x.IsDelete == false && x.IsInactive == false).FirstOrDefault();
                if (clevelinfo != null)
                {
                    department.DeptCLevelName = clevelinfo.EmpCode + "_" + clevelinfo.EmpName;
                }

            }

            if (department == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";
            return View("CreateDepartment", department);
        }

        public IActionResult DeleteDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = _departmentRepository.FindById(id);

            if (department == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            return View("CreateDepartment", department);
        }

        [HttpPost, ActionName("DeleteDepartment")]
        public IActionResult DeleteDepartmentConfirmed(int id)
        {
            try
            {
                var department = _departmentRepository.FindById(id);
                _departmentRepository.Delete(department);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), department.DeptId.ToString(), "Delete", department.DeptName);

                return Json(new { Success = 1, DeptId = department.DeptId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        public JsonResult SearchEmployeesForCLevel(string term)
        {
            string comid = HttpContext.Session.GetString("comid");
            var employees = _context.HR_Emp_Info
                .Where(e => (e.EmpName.Contains(term) || e.EmpCode.Contains(term)) && e.ComId == comid && e.IsDelete == false && e.IsInactive == false)
                .Select(e => new { label = e.EmpCode + " " + e.EmpName, value = e.EmpId })
                .Take(10)
                .ToList();

            return new JsonResult(employees);
        }

        #endregion Department

        #region Section 

        public IActionResult SectionList()
        {
            string comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            var data = _sectionRepository.GetAllSection();
            foreach (var item in data)
            {
                var hodinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.EmpId == item.SectHODId && x.IsDelete == false && x.IsInactive == false).FirstOrDefault();
                if (hodinfo != null)
                {
                    item.SectHODName = hodinfo.EmpCode + "_" + hodinfo.EmpName;
                }
            }
            return View(data.ToList());
        }

        public IActionResult CreateSection()
        {
            ViewBag.Title = "Create";
            ViewBag.DeptId = _departmentRepository.GetDepartmentList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateSection(Cat_Section cat_Section)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (ModelState.IsValid)
            {

                if (cat_Section.SectId > 0)
                {
                    _sectionRepository.Update(cat_Section);
                    var emplist = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.SectId == cat_Section.SectId).ToList();
                    if (emplist != null)
                    {
                        foreach (var emp in emplist)
                        {
                            emp.FirstAprvId = cat_Section.SectHODId;

                        }
                        _context.SaveChanges();
                    }
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Section.SectId.ToString(), "Update", cat_Section.SectName.ToString());

                }
                else
                {
                    _sectionRepository.Add(cat_Section);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Section.SectId.ToString(), "Create", cat_Section.SectName.ToString());

                }
                return RedirectToAction("SectionList", "HRVariables");
            }
            ViewBag.DeptId = new SelectList(_departmentRepository.GetDepartmentList(), "Value", "Text");
            return View(cat_Section);
        }

        public IActionResult EditSection(int? Id)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (Id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var section = _sectionRepository.FindById(Id);
            if (section.SectHODId != null)
            {

                var hodinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.EmpId == section.SectHODId && x.IsDelete == false && x.IsInactive == false).FirstOrDefault();
                if (hodinfo != null)
                {
                    section.SectHODName = hodinfo.EmpCode + "_" + hodinfo.EmpName;
                }
            }
            if (section.SectCLevelId != null)
            {
                var clevelinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.EmpId == section.SectCLevelId && x.IsDelete == false && x.IsInactive == false).FirstOrDefault();
                if (clevelinfo != null)
                {
                    section.SectCLevelName = clevelinfo.EmpCode + "_" + clevelinfo.EmpName;
                }

            }
            ViewBag.DeptId = new SelectList(_departmentRepository.GetDepartmentList(), "Value", "Text", section.DeptId);
            if (section == null)
            {
                return NotFound();
            }

            return View("CreateSection", section);
        }

        public IActionResult DeleteSection(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = _sectionRepository.FindById(id);
            ViewBag.DeptId = new SelectList(_departmentRepository.GetDepartmentList(), "Value", "Text", section.DeptId);

            if (section == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            return View("CreateSection", section);
        }

        [HttpPost, ActionName("DeleteSection")]
        public IActionResult DeleteSectionConfirmed(int id)
        {
            try
            {
                var section = _sectionRepository.FindById(id);
                _sectionRepository.Delete(section);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), section.SectId.ToString(), "Delete", section.SectName);

                return Json(new { Success = 1, SectId = section.SectId.ToString(), ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion Section

        #region Designation

        public IActionResult DesignationList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            var data = _designationRepository.GetAllDesignations();
            return View(data.ToList());
        }

        public IActionResult CreateDesignation()
        {
            ViewBag.Title = "Create";
            ViewBag.GradeId = new SelectList(_gradeRepository.GradeSelectList(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult CreateDesignation(Cat_Designation cat_Designation)
        {
            if (ModelState.IsValid)
            {
                if (cat_Designation.DesigId > 0)
                {
                    _designationRepository.Update(cat_Designation);
                    _designationRepository.UpdateDesignation(cat_Designation);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Designation.DesigId.ToString(), "Update", cat_Designation.DesigName.ToString());

                }
                else
                {
                    _designationRepository.Add(cat_Designation);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Designation.DesigId.ToString(), "Create", cat_Designation.DesigName.ToString());

                }
                return RedirectToAction("DesignationList", "HRVariables");
            }
            return View(cat_Designation);
        }

        public IActionResult EditDesignation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var cat_Designation = _designationRepository.FindById(id);
            ViewBag.GradeId = new SelectList(_gradeRepository.GradeSelectList(), "Value", "Text", cat_Designation.GradeId);
            if (cat_Designation == null)
            {
                return NotFound();
            }

            return View("CreateDesignation", cat_Designation);
        }

        public IActionResult DeleteDesignation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat_Designation = _designationRepository.FindById(id);
            if (cat_Designation == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.GradeId = new SelectList(_gradeRepository.GradeSelectList(), "Value", "Text", cat_Designation.GradeId);

            return View("CreateDesignation", cat_Designation);

        }

        [HttpPost, ActionName("DeleteDesignation")]
        public IActionResult DeleteDesignationConfirmed(int id)
        {
            try
            {
                var Cat_Designation = _designationRepository.FindById(id);
                _designationRepository.Delete(Cat_Designation);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Designation.DesigId.ToString(), "Delete", Cat_Designation.DesigName);
                return Json(new { Success = 1, DesigId = Cat_Designation.DesigId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        #endregion

        #region Grade

        public IActionResult GradeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            var data = _gradeRepository.GradeAll();
            return View(data.ToList());

        }

        public IActionResult CreateGrade()
        {

            ViewBag.Title = "Create";
            //ViewBag.DeptId = new SelectList(db.Cat_Department, "DeptId", "DeptName");
            return View();
        }

        [HttpPost]
        public IActionResult CreateGrade(Cat_Grade cat_Grade)
        {
            if (ModelState.IsValid)
            {

                if (cat_Grade.GradeId > 0)
                {
                    _gradeRepository.Update(cat_Grade);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Grade.GradeId.ToString(), "Update", cat_Grade.GradeName.ToString());

                }
                else
                {
                    _gradeRepository.Add(cat_Grade);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Grade.GradeId.ToString(), "Create", cat_Grade.GradeName.ToString());

                }
                return RedirectToAction("GradeList", "HRVariables");
            }
            return View(cat_Grade);
        }
        public IActionResult EditGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var cat_Grade = _gradeRepository.FindById(id);
            if (cat_Grade == null)
            {
                return NotFound();
            }
            return View("CreateGrade", cat_Grade);
        }


        public IActionResult DeleteGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Grade = _gradeRepository.FindById(id);
            if (Cat_Grade == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            return View("CreateGrade", Cat_Grade);
        }

        [HttpPost, ActionName("DeleteGrade")]
        public IActionResult DeleteGradeConfirmed(int id)
        {
            try
            {
                var Cat_Grade = _gradeRepository.FindById(id);
                _gradeRepository.Delete(Cat_Grade);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Grade.GradeId.ToString(), "Delete", Cat_Grade.GradeName);

                return Json(new { Success = 1, GradeId = Cat_Grade.GradeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion

        #region BloodGroup
        public IActionResult BloodGroupList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_bloodGroupRepository.GetBloodGroup().ToList());
        }

        public IActionResult CreateBloodGroup()
        {
            ViewBag.Title = "Create";
            ViewBag.BloodId = new SelectList(_bloodGroupRepository.BloodGroupSelectList(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult CreateBloodGroup(Cat_BloodGroup Cat_BloodGroup)
        {
            if (ModelState.IsValid)
            {
                if (Cat_BloodGroup.BloodId > 0)
                {
                    _bloodGroupRepository.Update(Cat_BloodGroup);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BloodGroup.BloodId.ToString(), "Update", Cat_BloodGroup.BloodName.ToString());

                }
                else
                {
                    _bloodGroupRepository.Add(Cat_BloodGroup);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BloodGroup.BloodId.ToString(), "Create", Cat_BloodGroup.BloodName.ToString());

                }
                return RedirectToAction("BloodGroupList", "HRVariables");
            }
            return View(Cat_BloodGroup);
        }

        public IActionResult EditBloodGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_BloodGroup = _bloodGroupRepository.FindById(id);
            ViewBag.BloodId = new SelectList(_bloodGroupRepository.BloodGroupSelectList(), "Value", "Text", Cat_BloodGroup.BloodId);
            if (Cat_BloodGroup == null)
            {
                return NotFound();
            }
            return View("CreateBloodGroup", Cat_BloodGroup);
        }

        public IActionResult DeleteBloodGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_BloodGroup = _bloodGroupRepository.FindById(id);
            if (Cat_BloodGroup == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.DeptId = new SelectList(_bloodGroupRepository.BloodGroupSelectList(), "Value", "Text", Cat_BloodGroup.BloodId);
            return View("CreateBloodGroup", Cat_BloodGroup);
        }


        [HttpPost, ActionName("DeleteBloodGroup")]
        public IActionResult DeleteBloodGroupConfirmed(int id)
        {
            try
            {
                var Cat_BloodGroup = _bloodGroupRepository.FindById(id);
                _bloodGroupRepository.Delete(Cat_BloodGroup);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BloodGroup.BloodId.ToString(), "Delete", Cat_BloodGroup.BloodName);

                return Json(new { Success = 1, BloodId = Cat_BloodGroup.BloodId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        #endregion

        #region Religion

        public IActionResult ReligionList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_religionRepository.GetReligionList().ToList());
        }

        public IActionResult CreateReligion()
        {
            ViewBag.Title = "Create";
            ViewBag.RelgionId = new SelectList(_religionRepository.ReligionSelectList(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult CreateReligion(Cat_Religion Cat_Religion)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Religion.RelgionId > 0)
                {
                    _religionRepository.Update(Cat_Religion);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Religion.RelgionId.ToString(), "Update", Cat_Religion.ReligionName.ToString());

                }
                else
                {
                    _religionRepository.Add(Cat_Religion);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Religion.RelgionId.ToString(), "Create", Cat_Religion.ReligionName.ToString());

                }
                return RedirectToAction("ReligionList", "HRVariables");
            }
            return View(Cat_Religion);
        }

        public IActionResult EditReligion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Religion = _religionRepository.FindById(id);
            ViewBag.RelgionId = new SelectList(_religionRepository.ReligionSelectList(), "Value", "Text", Cat_Religion.RelgionId);
            if (Cat_Religion == null)
            {
                return NotFound();
            }
            return View("CreateReligion", Cat_Religion);
        }

        public IActionResult DeleteReligion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Religion = _religionRepository.FindById(id);

            if (Cat_Religion == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.DeptId = new SelectList(_religionRepository.ReligionSelectList(), "Value", "Text", Cat_Religion.RelgionId);
            return View("CreateReligion", Cat_Religion);
        }

        [HttpPost, ActionName("DeleteReligion")]
        public IActionResult DeleteReligionConfirmed(int id)
        {
            try
            {
                var Cat_Religion = _religionRepository.FindById(id);
                _religionRepository.Delete(Cat_Religion);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Religion.RelgionId.ToString(), "Delete", Cat_Religion.ReligionName);

                return Json(new { Success = 1, RelgionId = Cat_Religion.RelgionId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Line
        public IActionResult LineList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_lineRepository.GetAll().ToList());
        }

        public IActionResult CreateLine()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateLine(Cat_Line Cat_Line)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Line.LineId > 0)
                {
                    _lineRepository.Update(Cat_Line);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Line.LineId.ToString(), "Update", Cat_Line.LineName.ToString());

                }
                else
                {
                    _lineRepository.Add(Cat_Line);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Line.LineId.ToString(), "Create", Cat_Line.LineName.ToString());

                }
                return RedirectToAction("LineList", "HRVariables");
            }
            return View(Cat_Line);
        }

        public IActionResult EditLine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Line = _lineRepository.FindById(id);
            if (Cat_Line == null)
            {
                return NotFound();
            }
            return View("CreateLine", Cat_Line);
        }

        public IActionResult DeleteLine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Line = _lineRepository.FindById(id);

            if (Cat_Line == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.LineId = new SelectList(_lineRepository.GetLineList(), "Value", "Text", Cat_Line.LineId);
            return View("CreateLine", Cat_Line);
        }

        [HttpPost, ActionName("DeleteLine")]
        public IActionResult DeleteLineConfirmed(int id)
        {
            try
            {
                var Cat_Line = _lineRepository.FindById(id);
                _lineRepository.Delete(Cat_Line);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Line.LineId.ToString(), "Delete", Cat_Line.LineName);

                return Json(new { Success = 1, LineId = Cat_Line.LineId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Cat Variable
        public IActionResult CatVariableList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_catVariableRepository.GetVariableList());
        }

        public IActionResult CreateCatVariable()
        {
            ViewBag.VarType = _catVariableRepository.GetCatVariableList();
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateCatVariable(Cat_Variable Cat_Variable)
        {
            ViewBag.VarType = _catVariableRepository.GetCatVariableList();
            if (ModelState.IsValid)
            {
                if (Cat_Variable.VarId > 0)
                {
                    _catVariableRepository.Update(Cat_Variable);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Variable.VarId.ToString(), "Update", Cat_Variable.VarName.ToString());

                }
                else
                {
                    _catVariableRepository.Add(Cat_Variable);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Variable.VarId.ToString(), "Create", Cat_Variable.VarName.ToString());

                }
                return RedirectToAction("CatVariableList", "HRVariables");
            }
            return View(Cat_Variable);
        }

        public IActionResult EditCatVariable(int? id)
        {
            ViewBag.VarType = _catVariableRepository.GetCatVariableList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Variable = _catVariableRepository.FindById(id);
            if (Cat_Variable == null)
            {
                return NotFound();
            }
            return View("CreateCatVariable", Cat_Variable);
        }

        public IActionResult DeleteCatVariable(int? id)
        {
            ViewBag.VarType = _catVariableRepository.GetCatVariableList();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Variable = _catVariableRepository.FindById(id);

            if (Cat_Variable == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateCatVariable", Cat_Variable);
        }

        [HttpPost, ActionName("DeleteCatVariable")]
        public IActionResult DeleteCatVariableConfirmed(int id)
        {
            ViewBag.VarType = _catVariableRepository.GetCatVariableList();
            try
            {
                var Cat_Variable = _catVariableRepository.FindById(id);
                _catVariableRepository.Delete(Cat_Variable);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Variable.VarId.ToString(), "Delete", Cat_Variable.VarName);

                return Json(new { Success = 1, VarId = Cat_Variable.VarId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Shift
        public IActionResult ShiftList()
        {

            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_shiftRepository.GetAll().ToList());
        }

        public IActionResult CreateShift()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateShift(Cat_Shift Cat_Shift)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Shift.ShiftId > 0)
                {

                    _shiftRepository.ShiftDefalutDate(Cat_Shift);
                    _shiftRepository.Update(Cat_Shift);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Shift.ShiftId.ToString(), "Update", Cat_Shift.ShiftName.ToString());

                }
                else
                {
                    _shiftRepository.ShiftDefalutDate(Cat_Shift);
                    _shiftRepository.Add(Cat_Shift);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Shift.ShiftId.ToString(), "Create", Cat_Shift.ShiftName.ToString());

                }
                return RedirectToAction("ShiftList", "HRVariables");
            }
            return View(Cat_Shift);
        }

        public IActionResult EditShift(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Shift = _shiftRepository.FindById(id);
            TimeSpan time = Cat_Shift.TiffinIn.TimeOfDay;
            if (Cat_Shift == null)
            {
                return NotFound();
            }
            return View("CreateShift", Cat_Shift);
        }

        public IActionResult DeleteShift(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Shift = _shiftRepository.FindById(id);

            if (Cat_Shift == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.ShiftId = new SelectList(_shiftRepository.GetShiftList(), "Value", "Text", Cat_Shift.ShiftId);
            return View("CreateShift", Cat_Shift);
        }

        [HttpPost, ActionName("DeleteShift")]
        public IActionResult DeleteShiftConfirmed(int id)
        {
            try
            {
                var Cat_Shift = _shiftRepository.FindById(id);
                _shiftRepository.Delete(Cat_Shift);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Shift.ShiftId.ToString(), "Delete", Cat_Shift.ShiftName);

                return Json(new { Success = 1, ShiftId = Cat_Shift.ShiftId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Skill
        public IActionResult SkillList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_skillRepository.SkillAll());
        }

        public IActionResult CreateSkill()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateSkill(Cat_Skill Cat_Skill)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Skill.SkillId > 0)
                {
                    _skillRepository.Update(Cat_Skill);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Skill.SkillId.ToString(), "Update", Cat_Skill.SkillName.ToString());

                }
                else
                {
                    _skillRepository.Add(Cat_Skill);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Skill.SkillId.ToString(), "Create", Cat_Skill.SkillName.ToString());

                }
                return RedirectToAction("SkillList", "HRVariables");
            }
            return View(Cat_Skill);
        }

        public IActionResult EditSkill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Skill = _skillRepository.FindById(id);
            if (Cat_Skill == null)
            {
                return NotFound();
            }
            return View("CreateSkill", Cat_Skill);
        }

        public IActionResult DeleteSkill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Skill = _skillRepository.FindById(id);

            if (Cat_Skill == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.SkillId = new SelectList(_skillRepository.GetSkillList(), "Value", "Text", Cat_Skill.SkillId);
            return View("CreateSkill", Cat_Skill);
        }

        [HttpPost, ActionName("DeleteSkill")]
        public IActionResult DeleteSkillConfirmed(int id)
        {
            try
            {
                var Cat_Skill = _skillRepository.FindById(id);
                _skillRepository.Delete(Cat_Skill);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Skill.SkillId.ToString(), "Delete", Cat_Skill.SkillName);

                return Json(new { Success = 1, SkillId = Cat_Skill.SkillId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Floor
        public IActionResult FloorList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_floorRepository.GetAll().ToList());
        }

        public IActionResult CreateFloor()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateFloor(Cat_Floor Cat_Floor)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Floor.FloorId > 0)
                {
                    _floorRepository.Update(Cat_Floor);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Floor.FloorId.ToString(), "Update", Cat_Floor.FloorName.ToString());

                }
                else
                {
                    _floorRepository.Add(Cat_Floor);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Floor.FloorId.ToString(), "Create", Cat_Floor.FloorName.ToString());

                }
                return RedirectToAction("FloorList", "HRVariables");
            }
            return View(Cat_Floor);
        }

        public IActionResult EditFloor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Floor = _floorRepository.FindById(id);
            if (Cat_Floor == null)
            {
                return NotFound();
            }
            return View("CreateFloor", Cat_Floor);
        }

        public IActionResult DeleteFloor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Floor = _floorRepository.FindById(id);

            if (Cat_Floor == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateFloor", Cat_Floor);
        }

        [HttpPost, ActionName("DeleteFloor")]
        public IActionResult DeleteFloorConfirmed(int id)
        {
            try
            {
                var Cat_Floor = _floorRepository.FindById(id);
                _floorRepository.Delete(Cat_Floor);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Floor.FloorId.ToString(), "Delete", Cat_Floor.FloorName);

                return Json(new { Success = 1, FloorId = Cat_Floor.FloorId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Unit
        public IActionResult CatUnitList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_catUnitRepository.GetAll().ToList());
        }

        public IActionResult CreateCatUnit()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateCatUnit(Cat_Unit Cat_Unit)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Unit.UnitId > 0)
                {
                    _catUnitRepository.Update(Cat_Unit);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Unit.UnitId.ToString(), "Update", Cat_Unit.UnitName.ToString());

                }
                else
                {
                    _catUnitRepository.Add(Cat_Unit);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Unit.UnitId.ToString(), "Create", Cat_Unit.UnitName.ToString());

                }
                return RedirectToAction("CatUnitList", "HRVariables");
            }
            return View(Cat_Unit);
        }

        public IActionResult EditCatUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Unit = _catUnitRepository.FindById(id);
            if (Cat_Unit == null)
            {
                return NotFound();
            }
            return View("CreateCatUnit", Cat_Unit);
        }

        public IActionResult DeleteCatUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Unit = _catUnitRepository.FindById(id);

            if (Cat_Unit == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateCatUnit", Cat_Unit);
        }

        [HttpPost, ActionName("DeleteCatUnit")]
        public IActionResult DeleteCatUnitConfirmed(int id)
        {
            try
            {
                var Cat_Unit = _catUnitRepository.FindById(id);
                _catUnitRepository.Delete(Cat_Unit);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Unit.UnitId.ToString(), "Delete", Cat_Unit.UnitName);

                return Json(new { Success = 1, UnitId = Cat_Unit.UnitId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Bank
        public IActionResult BankList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_bankRepository.GetBankInfo());
        }

        public IActionResult CreateBank()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateBank(Cat_Bank Cat_Bank)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Bank.BankId > 0)
                {
                    _bankRepository.Update(Cat_Bank);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Bank.BankId.ToString(), "Update", Cat_Bank.BankName.ToString());

                }
                else
                {
                    _bankRepository.Add(Cat_Bank);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Bank.BankId.ToString(), "Create", Cat_Bank.BankName.ToString());

                }
                return RedirectToAction("BankList", "HRVariables");
            }
            return View(Cat_Bank);
        }

        public IActionResult EditBank(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Bank = _bankRepository.FindById(id);
            if (Cat_Bank == null)
            {
                return NotFound();
            }
            return View("CreateBank", Cat_Bank);
        }

        public IActionResult DeleteBank(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Bank = _bankRepository.FindById(id);

            if (Cat_Bank == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateBank", Cat_Bank);
        }

        [HttpPost, ActionName("DeleteBank")]
        public IActionResult DeleteBankConfirmed(int id)
        {
            try
            {
                var Cat_Bank = _bankRepository.FindById(id);
                _bankRepository.Delete(Cat_Bank);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Bank.BankId.ToString(), "Delete", Cat_Bank.BankName);

                return Json(new { Success = 1, BankId = Cat_Bank.BankId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Bank Branch
        public IActionResult BankBranchList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_bankBranchRepository.GetBankBranchInfo());
        }

        public IActionResult CreateBankBranch()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateBankBranch(Cat_BankBranch Cat_BankBranch)
        {
            if (ModelState.IsValid)
            {
                if (Cat_BankBranch.BranchId > 0)
                {
                    _bankBranchRepository.Update(Cat_BankBranch);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BankBranch.BranchId.ToString(), "Update", Cat_BankBranch.BranchName.ToString());

                }
                else
                {
                    _bankBranchRepository.Add(Cat_BankBranch);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BankBranch.BranchId.ToString(), "Create", Cat_BankBranch.BranchName.ToString());

                }
                return RedirectToAction("BankBranchList", "HRVariables");
            }
            return View(Cat_BankBranch);
        }

        public IActionResult EditBankBranch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_BankBranch = _bankBranchRepository.FindById(id);
            if (Cat_BankBranch == null)
            {
                return NotFound();
            }
            return View("CreateBankBranch", Cat_BankBranch);
        }

        public IActionResult DeleteBankBranch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_BankBranch = _bankBranchRepository.FindById(id);

            if (Cat_BankBranch == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateBankBranch", Cat_BankBranch);
        }

        [HttpPost, ActionName("DeleteBankBranch")]
        public IActionResult DeleteBankBranchConfirmed(int id)
        {
            try
            {
                var Cat_BankBranch = _bankBranchRepository.FindById(id);
                _bankBranchRepository.Delete(Cat_BankBranch);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BankBranch.BranchId.ToString(), "Delete", Cat_BankBranch.BranchName);

                return Json(new { Success = 1, BankBranchId = Cat_BankBranch.BranchId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Building Type
        public IActionResult BuildingTypeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_buildingTypeRepository.GetAll().ToList());
        }

        public IActionResult CreateBuildingType()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateBuildingType(Cat_BuildingType Cat_BuildingType)
        {
            if (ModelState.IsValid)
            {
                if (Cat_BuildingType.BId > 0)
                {
                    _buildingTypeRepository.Update(Cat_BuildingType);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BuildingType.BId.ToString(), "Update", Cat_BuildingType.BId.ToString());

                }
                else
                {
                    _buildingTypeRepository.Add(Cat_BuildingType);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BuildingType.BId.ToString(), "Create", Cat_BuildingType.BuildingName.ToString());

                }
                return RedirectToAction("BuildingTypeList", "HRVariables");
            }
            return View(Cat_BuildingType);
        }

        public IActionResult EditBuildingType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_BuildingType = _buildingTypeRepository.FindById(id);
            if (Cat_BuildingType == null)
            {
                return NotFound();
            }
            return View("CreateBuildingType", Cat_BuildingType);
        }

        public IActionResult DeleteBuildingType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_BuildingType = _buildingTypeRepository.FindById(id);

            if (Cat_BuildingType == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateBuildingType", Cat_BuildingType);
        }

        [HttpPost, ActionName("DeleteBuildingType")]
        public IActionResult DeleteBuildingTypeConfirmed(int id)
        {
            try
            {
                var Cat_BuildingType = _buildingTypeRepository.FindById(id);
                _buildingTypeRepository.Delete(Cat_BuildingType);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BuildingType.BId.ToString(), "Delete", Cat_BuildingType.BuildingName);

                return Json(new { Success = 1, BId = Cat_BuildingType.BId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Emp Type
        public IActionResult EmpTypeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_empTypeRepository.All().ToList());
        }

        public IActionResult CreateEmpType()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateEmpType(Cat_Emp_Type Cat_Emp_Type)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Emp_Type.EmpTypeId > 0)
                {
                    _empTypeRepository.Update(Cat_Emp_Type);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Emp_Type.EmpTypeId.ToString(), "Update", Cat_Emp_Type.EmpTypeName.ToString());

                }
                else
                {
                    _empTypeRepository.Add(Cat_Emp_Type);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Emp_Type.EmpTypeId.ToString(), "Create", Cat_Emp_Type.EmpTypeName.ToString());

                }
                return RedirectToAction("EmpTypeList", "HRVariables");
            }
            return View(Cat_Emp_Type);
        }

        public IActionResult EditEmpType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Emp_Type = _empTypeRepository.FindById(id);
            if (Cat_Emp_Type == null)
            {
                return NotFound();
            }
            return View("CreateEmpType", Cat_Emp_Type);
        }

        public IActionResult DeleteEmpType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Emp_Type = _empTypeRepository.FindById(id);

            if (Cat_Emp_Type == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateEmpType", Cat_Emp_Type);
        }

        [HttpPost, ActionName("DeleteEmpType")]
        public IActionResult DeleteEmpTypeConfirmed(int id)
        {
            try
            {
                var Cat_Emp_Type = _empTypeRepository.FindById(id);
                _empTypeRepository.Delete(Cat_Emp_Type);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Emp_Type.EmpTypeId.ToString(), "Delete", Cat_Emp_Type.EmpTypeName);

                return Json(new { Success = 1, EmpTypeId = Cat_Emp_Type.EmpTypeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Bus Stop
        public IActionResult BusStopList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_busStopRepository.GetAll().ToList());
        }

        public IActionResult CreateBusStop()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateBusStop(Cat_BusStop Cat_BusStop)
        {
            if (ModelState.IsValid)
            {
                if (Cat_BusStop.BusStopId > 0)
                {
                    _busStopRepository.Update(Cat_BusStop);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BusStop.BusStopId.ToString(), "Update", Cat_BusStop.BusStopName.ToString());

                }
                else
                {
                    _busStopRepository.Add(Cat_BusStop);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BusStop.BusStopId.ToString(), "Create", Cat_BusStop.BusStopName.ToString());

                }
                return RedirectToAction("BusStopList", "HRVariables");
            }
            return View(Cat_BusStop);
        }

        public IActionResult EditBusStop(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_BusStop = _busStopRepository.FindById(id);
            if (Cat_BusStop == null)
            {
                return NotFound();
            }
            return View("CreateBusStop", Cat_BusStop);
        }

        public IActionResult DeleteBusStop(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_BusStop = _busStopRepository.FindById(id);

            if (Cat_BusStop == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateBusStop", Cat_BusStop);
        }

        [HttpPost, ActionName("DeleteBusStop")]
        public IActionResult DeleteBusStopConfirmed(int id)
        {
            try
            {
                var Cat_BusStop = _busStopRepository.FindById(id);
                _busStopRepository.Delete(Cat_BusStop);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_BusStop.BusStopId.ToString(), "Delete", Cat_BusStop.BusStopName);

                return Json(new { Success = 1, BusStopId = Cat_BusStop.BusStopId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Meeting
        public IActionResult MeetingList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_meetingRepository.GetAll().ToList());
        }

        public IActionResult CreateMeeting()
        {
            ViewBag.Title = "Create";
            // this variable data is changed when add variable controller.
            ViewBag.MeetingType = _meetingRepository.GetMeetingList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateMeeting(Cat_Meeting Cat_Meeting)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Meeting.MeetingId > 0)
                {
                    _meetingRepository.Update(Cat_Meeting);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Meeting.MeetingId.ToString(), "Update", Cat_Meeting.Meeting.ToString());

                }
                else
                {
                    _meetingRepository.Add(Cat_Meeting);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Meeting.MeetingId.ToString(), "Create", Cat_Meeting.Meeting.ToString());

                }
                // this variable data is changed when add variable controller.
                ViewBag.MeetingType = _meetingRepository.GetMeetingList();
                return RedirectToAction("MeetingList", "HRVariables");
            }
            return View(Cat_Meeting);
        }

        public IActionResult EditMeeting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            // this variable data is changed when add variable controller.
            ViewBag.MeetingType = _meetingRepository.GetMeetingList();
            var Cat_Meeting = _meetingRepository.FindById(id);
            if (Cat_Meeting == null)
            {
                return NotFound();
            }
            return View("CreateMeeting", Cat_Meeting);
        }

        public IActionResult DeleteMeeting(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var Cat_Meeting = _meetingRepository.FindById(id);

            if (Cat_Meeting == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            // this variable data is changed when add variable controller.
            ViewBag.MeetingType = _meetingRepository.GetMeetingList();
            return View("CreateMeeting", Cat_Meeting);
        }

        [HttpPost, ActionName("DeleteMeeting")]
        public IActionResult DeleteMeetingConfirmed(int id)
        {
            ViewBag.MeetingType = _meetingRepository.GetMeetingList();
            try
            {
                var Cat_Meeting = _meetingRepository.FindById(id);
                _meetingRepository.Delete(Cat_Meeting);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Meeting.MeetingId.ToString(), "Delete", Cat_Meeting.Meeting);

                return Json(new { Success = 1, MeetingId = Cat_Meeting.MeetingId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Location
        public IActionResult LocationList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_locationRepository.GetAll().ToList());
        }

        public IActionResult CreateLocation()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateLocation(Cat_Location Cat_Location)
        {
            if (ModelState.IsValid)
            {
                if (Cat_Location.LId > 0)
                {
                    _locationRepository.Update(Cat_Location);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Location.LId.ToString(), "Update", Cat_Location.LocationName.ToString());

                }
                else
                {
                    _locationRepository.Add(Cat_Location);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Location.LId.ToString(), "Create", Cat_Location.LocationName.ToString());

                }
                return RedirectToAction("LocationList", "HRVariables");
            }
            return View(Cat_Location);
        }

        public IActionResult EditLocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Location = _locationRepository.FindById(id);
            if (Cat_Location == null)
            {
                return NotFound();
            }
            return View("CreateLocation", Cat_Location);
        }

        public IActionResult DeleteLocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Location = _locationRepository.FindById(id);

            if (Cat_Location == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateLocation", Cat_Location);
        }

        [HttpPost, ActionName("DeleteLocation")]
        public IActionResult DeleteLocationConfirmed(int id)
        {
            try
            {
                var Cat_Location = _locationRepository.FindById(id);
                _locationRepository.Delete(Cat_Location);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Location.LId.ToString(), "Delete", Cat_Location.LocationName);

                return Json(new { Success = 1, LocationId = Cat_Location.LId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region District
        public IActionResult DistrictList()
        {
            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_districtRepository.GetAllDistrict().ToList());
        }

        public IActionResult CreateDistrict()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateDistrict(Cat_District Cat_District)
        {
            if (ModelState.IsValid)
            {
                if (Cat_District.DistId > 0)
                {
                    _districtRepository.Update(Cat_District);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_District.DistId.ToString(), "Update", Cat_District.DistName.ToString());

                }
                else
                {
                    _districtRepository.Add(Cat_District);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_District.DistId.ToString(), "Create", Cat_District.DistName.ToString());

                }
                return RedirectToAction("DistrictList", "HRVariables");
            }
            return View(Cat_District);
        }

        public IActionResult EditDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_District = _districtRepository.FindById(id);
            if (Cat_District == null)
            {
                return NotFound();
            }
            return View("CreateDistrict", Cat_District);
        }

        public IActionResult DeleteDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_District = _districtRepository.FindById(id);

            if (Cat_District == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateDistrict", Cat_District);
        }

        [HttpPost, ActionName("DeleteDistrict")]
        public IActionResult DeleteDistrictConfirmed(int id)
        {
            try
            {
                var Cat_District = _districtRepository.FindById(id);
                _districtRepository.Delete(Cat_District);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_District.DistId.ToString(), "Delete", Cat_District.DistName);

                return Json(new { Success = 1, DistId = Cat_District.DistId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Police Station
        public IActionResult PoliceStationList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_policeStationRepository.GetPSList().ToList());
        }

        public IActionResult CreatePoliceStation()
        {
            ViewBag.Title = "Create";

            ViewBag.DistId = _districtRepository.GetDistrictList();
            return View();
        }

        [HttpPost]
        public IActionResult CreatePoliceStation(Cat_PoliceStation Cat_PoliceStation)
        {
            if (ModelState.IsValid)
            {
                if (Cat_PoliceStation.PStationId > 0)
                {
                    _policeStationRepository.Update(Cat_PoliceStation);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_PoliceStation.PStationId.ToString(), "Update", Cat_PoliceStation.PStationName.ToString());

                }
                else
                {
                    _policeStationRepository.Add(Cat_PoliceStation);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_PoliceStation.PStationId.ToString(), "Create", Cat_PoliceStation.PStationName.ToString());

                }
                return RedirectToAction("PoliceStationList", "HRVariables");

            }
            ViewBag.DistId = _districtRepository.GetDistrictList();
            return View(Cat_PoliceStation);
        }

        public IActionResult EditPoliceStation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.DistId = _districtRepository.GetDistrictList();
            var Cat_PoliceStation = _policeStationRepository.FindById(id);
            if (Cat_PoliceStation == null)
            {
                return NotFound();
            }
            return View("CreatePoliceStation", Cat_PoliceStation);
        }

        public IActionResult DeletePoliceStation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_PoliceStation = _policeStationRepository.FindById(id);
            ViewBag.DistId = _districtRepository.GetDistrictList();
            if (Cat_PoliceStation == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreatePoliceStation", Cat_PoliceStation);
        }

        [HttpPost, ActionName("DeletePoliceStation")]
        public IActionResult DeletePoliceStationConfirmed(int id)
        {
            ViewBag.DistId = _districtRepository.GetDistrictList();
            try
            {
                var Cat_PoliceStation = _policeStationRepository.FindById(id);
                _policeStationRepository.Delete(Cat_PoliceStation);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_PoliceStation.PStationId.ToString(), "Delete", Cat_PoliceStation.PStationName);

                return Json(new { Success = 1, PSId = Cat_PoliceStation.PStationId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Post Office
        public IActionResult PostOfficeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_postOfficeRepository.GetPOList().ToList());
        }
        [HttpGet]
        public IActionResult GetPoliceStation(int id)
        {
            string comid = HttpContext.Session.GetString("comid");
            var data = _policeStationRepository.GetPSList().Where(p => p.DistId == id).ToList();
            return Json(data);
        }

        public IActionResult CreatePostOffice()
        {
            ViewBag.Title = "Create";

            ViewBag.DistId = _districtRepository.GetDistrictList();
            return View();
        }

        [HttpPost]
        public IActionResult CreatePostOffice(Cat_PostOffice Cat_PostOffice)
        {
            if (ModelState.IsValid)
            {
                if (Cat_PostOffice.POId > 0)
                {
                    _postOfficeRepository.Update(Cat_PostOffice);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_PostOffice.POId.ToString(), "Update", Cat_PostOffice.POName.ToString());

                }
                else
                {
                    _postOfficeRepository.Add(Cat_PostOffice);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_PostOffice.POId.ToString(), "Create", Cat_PostOffice.POName.ToString());

                }
                return RedirectToAction("PostOfficeList", "HRVariables");

            }
            ViewBag.DistId = _districtRepository.GetDistrictList();
            return View(Cat_PostOffice);
        }

        public IActionResult EditPostOffice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.DistId = _districtRepository.GetDistrictList();
            var Cat_PostOffice = _postOfficeRepository.FindById(id);
            if (Cat_PostOffice == null)
            {
                return NotFound();
            }
            return View("CreatePostOffice", Cat_PostOffice);
        }

        public IActionResult DeletePostOffice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_PostOffice = _postOfficeRepository.FindById(id);
            ViewBag.DistId = _districtRepository.GetDistrictList();
            if (Cat_PostOffice == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreatePostOffice", Cat_PostOffice);
        }

        [HttpPost, ActionName("DeletePostOffice")]
        public IActionResult DeletePostOfficeConfirmed(int id)
        {
            ViewBag.DistId = _districtRepository.GetDistrictList();
            try
            {
                var Cat_PostOffice = _postOfficeRepository.FindById(id);
                _postOfficeRepository.Delete(Cat_PostOffice);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_PostOffice.POId.ToString(), "Delete", Cat_PostOffice.POName);

                return Json(new { Success = 1, POId = Cat_PostOffice.POId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Style
        public IActionResult StyleList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_styleRepository.GetAll().ToList());
        }

        public IActionResult CreateStyle()
        {
            ViewBag.Color = _styleRepository.GetColor();
            ViewBag.Size = _styleRepository.GetSize();
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateStyle(Cat_Style Cat_Style)
        {
            ViewBag.StyleDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (Cat_Style.StyleId > 0)
                {
                    _styleRepository.Update(Cat_Style);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Style.StyleId.ToString(), "Update", Cat_Style.StyleName.ToString());

                }
                else
                {
                    _styleRepository.Add(Cat_Style);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Style.StyleId.ToString(), "Create", Cat_Style.StyleName.ToString());

                }
                return RedirectToAction("StyleList", "HRVariables");
            }
            return View(Cat_Style);
        }

        public IActionResult EditStyle(int? id)
        {
            ViewBag.StyleDate = DateTime.Now;
            ViewBag.Color = _styleRepository.GetColor();
            ViewBag.Size = _styleRepository.GetSize();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Style = _styleRepository.FindById(id);
            if (Cat_Style == null)
            {
                return NotFound();
            }
            return View("CreateStyle", Cat_Style);
        }

        public IActionResult DeleteStyle(int? id)
        {
            ViewBag.StyleDate = DateTime.Now;
            ViewBag.Color = _styleRepository.GetColor();
            ViewBag.Size = _styleRepository.GetSize();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Style = _styleRepository.FindById(id);

            if (Cat_Style == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateStyle", Cat_Style);
        }

        [HttpPost, ActionName("DeleteStyle")]
        public IActionResult DeleteStyleConfirmed(int id)
        {

            try
            {
                var Cat_Style = _styleRepository.FindById(id);
                _styleRepository.Delete(Cat_Style);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Style.StyleId.ToString(), "Delete", Cat_Style.StyleName);

                return Json(new { Success = 1, StyleId = Cat_Style.StyleId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Unit
        public IActionResult UnitList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_unitRepository.All().ToList());
        }

        public IActionResult CreateUnit()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateUnit(Unit Unit)
        {
            if (ModelState.IsValid)
            {
                if (Unit.UnitId > 0)
                {
                    _unitRepository.Update(Unit);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Unit.UnitId.ToString(), "Update", Unit.UnitName.ToString());

                }
                else
                {
                    _unitRepository.Add(Unit);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Unit.UnitId.ToString(), "Create", Unit.UnitName.ToString());

                }
                return RedirectToAction("UnitList", "HRVariables");
            }
            return View(Unit);
        }

        public IActionResult EditUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Unit = _unitRepository.FindById(id);
            if (Unit == null)
            {
                return NotFound();
            }
            return View("CreateUnit", Unit);
        }

        public IActionResult DeleteUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Unit = _unitRepository.FindById(id);

            if (Unit == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateUnit", Unit);
        }

        [HttpPost, ActionName("DeleteUnit")]
        public IActionResult DeleteUnitConfirmed(int id)
        {
            try
            {
                var Unit = _unitRepository.FindById(id);
                _unitRepository.Delete(Unit);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Unit.UnitId.ToString(), "Delete", Unit.UnitName);

                return Json(new { Success = 1, UnitId = Unit.UnitId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Supplier
        public IActionResult SupplierList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_supplierRepository.GetAll().ToList());
        }


        public JsonResult getPoliceStation(int id)
        {
            List<Cat_PoliceStation> PStation = _policeStationRepository.GetPSList().Where(x => x.DistId == id).ToList();

            List<SelectListItem> lipstation = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (PStation != null)
            {
                foreach (Cat_PoliceStation x in PStation)
                {
                    lipstation.Add(new SelectListItem { Text = x.PStationName, Value = x.PStationId.ToString() });
                }
            }
            return Json(new SelectList(lipstation, "Value", "Text"));
        }

        public IActionResult CreateSupplier()
        {
            ViewBag.CountryId = _countryRepository.GetCountryList();
            ViewBag.DistId = _districtRepository.GetDistrictList();
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateSupplier(Supplier Supplier)
        {

            ViewBag.CountryId = _countryRepository.GetCountryList();
            ViewBag.DistId = _districtRepository.GetDistrictList();
            if (ModelState.IsValid)
            {
                if (Supplier.SupplierId > 0)
                {
                    _supplierRepository.Update(Supplier);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Supplier.SupplierId.ToString(), "Update", Supplier.SupplierName.ToString());

                }
                else
                {
                    _supplierRepository.Add(Supplier);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Supplier.SupplierId.ToString(), "Create", Supplier.SupplierName.ToString());

                }
                return RedirectToAction("SupplierList", "HRVariables");
            }
            return View(Supplier);
        }

        public IActionResult EditSupplier(int? id)
        {
            ViewBag.CountryId = _countryRepository.GetCountryList();
            ViewBag.DistId = _districtRepository.GetDistrictList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Supplier = _supplierRepository.FindById(id);
            if (Supplier == null)
            {
                return NotFound();
            }
            return View("CreateSupplier", Supplier);
        }

        public IActionResult DeleteSupplier(int? id)
        {
            ViewBag.CountryId = _countryRepository.GetCountryList();
            ViewBag.DistId = _districtRepository.GetDistrictList();
            if (id == null)
            {
                return NotFound();
            }

            var Supplier = _supplierRepository.FindById(id);

            if (Supplier == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateSupplier", Supplier);
        }

        [HttpPost, ActionName("DeleteSupplier")]
        public IActionResult DeleteSupplierConfirmed(int id)
        {
            ViewBag.CountryId = _countryRepository.GetCountryList();
            ViewBag.DistId = _districtRepository.GetDistrictList();
            try
            {
                var Supplier = _supplierRepository.FindById(id);
                _supplierRepository.Delete(Supplier);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Supplier.SupplierId.ToString(), "Delete", Supplier.SupplierName);

                return Json(new { Success = 1, SupplierId = Supplier.SupplierId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Summer Winter Allowance
        public IActionResult SWAllowanceList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_summerWinterAllowanceRepository.GetAll().ToList());
        }

        public IActionResult CreateSWAllowance()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateSWAllowance(Cat_SummerWinterAllowanceSetting SWSetting)
        {
            if (ModelState.IsValid)
            {
                if (SWSetting.SWAllowanceId > 0)
                {
                    var check = _summerWinterAllowanceRepository.GetAll().Where(s => s.ProssType == SWSetting.ProssType && s.SWAllowanceId != SWSetting.SWAllowanceId).FirstOrDefault();
                    if (check != null)
                    {
                        _summerWinterAllowanceRepository.Update(SWSetting);

                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), SWSetting.SWAllowanceId.ToString(), "Update", SWSetting.ProssType.ToString());

                    }
                }

                else
                {
                    var check = _summerWinterAllowanceRepository.GetAll().Where(s => s.ProssType == SWSetting.ProssType && s.SWAllowanceId != SWSetting.SWAllowanceId).FirstOrDefault();
                    if (check != null)
                    {
                        _summerWinterAllowanceRepository.Add(SWSetting);

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), SWSetting.SWAllowanceId.ToString(), "Create", SWSetting.ProssType.ToString());
                    }

                }
                return RedirectToAction("SWAllowanceList", "HRVariables");
            }
            return View(SWSetting);
        }

        public IActionResult EditSWAllowance(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var SWSetting = _summerWinterAllowanceRepository.FindById(id);
            if (SWSetting == null)
            {
                return NotFound();
            }
            return View("CreateSWAllowance", SWSetting);
        }

        public IActionResult DeleteSWAllowance(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SWSetting = _summerWinterAllowanceRepository.FindById(id);

            if (SWSetting == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateSWAllowance", SWSetting);
        }

        [HttpPost, ActionName("DeleteSWAllowance")]
        public IActionResult DeleteSWAllowanceConfirmed(int id)
        {
            try
            {
                var SWSetting = _summerWinterAllowanceRepository.FindById(id);
                _summerWinterAllowanceRepository.Delete(SWSetting);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), SWSetting.SWAllowanceId.ToString(), "Delete", SWSetting.ProssType);

                return Json(new { Success = 1, SWSettingId = SWSetting.SWAllowanceId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region IT Computation Setting
        public IActionResult ITComputationList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_iTComputationSettingRepository.GetAll().ToList());
        }

        public IActionResult CreateITComputation()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateITComputation(Cat_ITComputationSetting Cat_ITComputation)
        {
            if (ModelState.IsValid)
            {
                if (Cat_ITComputation.Id > 0)
                {
                    _iTComputationSettingRepository.Update(Cat_ITComputation);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ITComputation.Id.ToString(), "Update", Cat_ITComputation.TaxComputation.ToString());

                }
                else
                {
                    _iTComputationSettingRepository.Add(Cat_ITComputation);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ITComputation.Id.ToString(), "Create", Cat_ITComputation.TaxComputation.ToString());

                }
                return RedirectToAction("ITComputationList", "HRVariables");
            }
            return View(Cat_ITComputation);
        }

        public IActionResult EditITComputation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_ITComputation = _iTComputationSettingRepository.FindById(id);
            if (Cat_ITComputation == null)
            {
                return NotFound();
            }
            return View("CreateITComputation", Cat_ITComputation);
        }

        public IActionResult DeleteITComputation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_ITComputation = _iTComputationSettingRepository.FindById(id);

            if (Cat_ITComputation == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateStyle", Cat_ITComputation);
        }

        [HttpPost, ActionName("DeleteITComputation")]
        public IActionResult DeleteITComputationConfirmed(int id)
        {
            try
            {
                var Cat_ITComputation = _iTComputationSettingRepository.FindById(id);
                _iTComputationSettingRepository.Delete(Cat_ITComputation);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ITComputation.Id.ToString(), "Delete", Cat_ITComputation.TaxComputation.ToString());

                return Json(new { Success = 1, StyleId = Cat_ITComputation.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Gas Charge Setting
        public IActionResult GasChargeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_gasChargeSettingRepository.GetAll().ToList());
        }

        public IActionResult CreateGasCharge()
        {
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateGasCharge(Cat_GasChargeSetting GasCharge)
        {
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (ModelState.IsValid)
            {
                if (GasCharge.Id > 0)
                {
                    _gasChargeSettingRepository.Update(GasCharge);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), GasCharge.Id.ToString(), "Update", GasCharge.Cat_Location.ToString());

                }
                else
                {
                    _gasChargeSettingRepository.Add(GasCharge);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), GasCharge.Id.ToString(), "Create", GasCharge.Cat_Location.ToString());

                }
                return RedirectToAction("GasChargeList", "HRVariables");
            }
            return View(GasCharge);
        }

        public IActionResult EditGasCharge(int? id)
        {
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var GasCharge = _gasChargeSettingRepository.FindById(id);
            if (GasCharge == null)
            {
                return NotFound();
            }
            return View("CreateGasCharge", GasCharge);
        }

        public IActionResult DeleteGasCharge(int? id)
        {
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (id == null)
            {
                return NotFound();
            }

            var GasCharge = _gasChargeSettingRepository.FindById(id);

            if (GasCharge == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateStyle", GasCharge);
        }

        [HttpPost, ActionName("DeleteGasCharge")]
        public IActionResult DeleteGasChargeConfirmed(int id)
        {
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            try
            {
                var GasCharge = _gasChargeSettingRepository.FindById(id);
                _gasChargeSettingRepository.Delete(GasCharge);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), GasCharge.Id.ToString(), "Delete", GasCharge.Cat_Location.ToString());

                return Json(new { Success = 1, Id = GasCharge.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Tax Bank
        public IActionResult TaxBankList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_taxBankRepository.GetAll().ToList());
        }

        public IActionResult CreateTaxBank()
        {
            ViewBag.BankId = _bankRepository.GetBankList();
            ViewBag.BranchId = _bankBranchRepository.GetBankBranchList();
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateTaxBank(Cat_ITaxBank Cat_TaxBank)
        {
            ViewBag.BankId = _bankRepository.GetBankList();
            ViewBag.BranchId = _bankBranchRepository.GetBankBranchList();
            if (ModelState.IsValid)
            {
                if (Cat_TaxBank.Id > 0)
                {
                    _taxBankRepository.Update(Cat_TaxBank);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_TaxBank.Id.ToString(), "Update", Cat_TaxBank.Cat_Bank.ToString());

                }
                else
                {
                    _taxBankRepository.Add(Cat_TaxBank);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_TaxBank.Id.ToString(), "Create", Cat_TaxBank.Cat_Bank.ToString());

                }
                return RedirectToAction("TaxBankList", "HRVariables");
            }
            return View(Cat_TaxBank);
        }

        public IActionResult EditTaxBank(int? id)
        {
            ViewBag.BankId = _bankRepository.GetBankList();
            ViewBag.BranchId = _bankBranchRepository.GetBankBranchList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_TaxBank = _taxBankRepository.FindById(id);
            if (Cat_TaxBank == null)
            {
                return NotFound();
            }
            return View("CreateTaxBank", Cat_TaxBank);
        }

        public IActionResult DeleteTaxBank(int? id)
        {
            ViewBag.BankId = _bankRepository.GetBankList();
            ViewBag.BranchId = _bankBranchRepository.GetBankBranchList();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_TaxBank = _taxBankRepository.FindById(id);

            if (Cat_TaxBank == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateTaxBank", Cat_TaxBank);
        }

        [HttpPost, ActionName("DeleteTaxBank")]
        public IActionResult DeleteTaxBankConfirmed(int id)
        {
            ViewBag.BankId = _bankRepository.GetBankList();
            ViewBag.BranchId = _bankBranchRepository.GetBankBranchList();
            try
            {
                var Cat_TaxBank = _taxBankRepository.FindById(id);
                _taxBankRepository.Delete(Cat_TaxBank);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_TaxBank.Id.ToString(), "Delete", Cat_TaxBank.Cat_Bank.ToString());

                return Json(new { Success = 1, StyleId = Cat_TaxBank.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Incen Band
        public IActionResult IncenBandList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_incenBandRepository.GetAll().ToList());
        }

        public IActionResult CreateIncenBand()
        {
            ViewBag.InBId = _incenBandRepository.GetIncenBandList();
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateIncenBand(Cat_IncenBand Cat_IncenBand)
        {
            ViewBag.InBId = _incenBandRepository.GetIncenBandList();
            if (ModelState.IsValid)
            {
                if (Cat_IncenBand.InBId > 0)
                {
                    _incenBandRepository.Update(Cat_IncenBand);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_IncenBand.InBId.ToString(), "Update", Cat_IncenBand.IncenBandName.ToString());

                }
                else
                {
                    _incenBandRepository.Add(Cat_IncenBand);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_IncenBand.InBId.ToString(), "Create", Cat_IncenBand.IncenBandName.ToString());

                }
                return RedirectToAction("IncenBandList", "HRVariables");
            }
            return View(Cat_IncenBand);
        }

        public IActionResult EditIncenBand(int? id)
        {
            ViewBag.InBId = _incenBandRepository.GetIncenBandList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_IncenBand = _incenBandRepository.FindById(id);
            if (Cat_IncenBand == null)
            {
                return NotFound();
            }
            return View("CreateIncenBand", Cat_IncenBand);
        }

        public IActionResult DeleteIncenBand(int? id)
        {
            ViewBag.InBId = _incenBandRepository.GetIncenBandList();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_IncenBand = _incenBandRepository.FindById(id);

            if (Cat_IncenBand == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateIncenBand", Cat_IncenBand);
        }

        [HttpPost, ActionName("DeleteIncenBand")]
        public IActionResult DeleteIncenBandConfirmed(int id)
        {
            ViewBag.InBId = _incenBandRepository.GetIncenBandList();
            try
            {
                var Cat_IncenBand = _incenBandRepository.FindById(id);
                _incenBandRepository.Delete(Cat_IncenBand);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_IncenBand.InBId.ToString(), "Delete", Cat_IncenBand.IncenBandName.ToString());

                return Json(new { Success = 1, InBId = Cat_IncenBand.InBId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Insure Grade
        public IActionResult InsureGradeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_insureGradeRepository.GetAll().ToList());
        }

        public IActionResult CreateInsureGrade()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateInsureGrade(Cat_InsurGrade Cat_InsurGrade)
        {
            if (ModelState.IsValid)
            {
                if (Cat_InsurGrade.InGId > 0)
                {
                    _insureGradeRepository.Update(Cat_InsurGrade);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_InsurGrade.InGId.ToString(), "Update", Cat_InsurGrade.InSurGrade.ToString());

                }
                else
                {
                    _insureGradeRepository.Add(Cat_InsurGrade);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_InsurGrade.InGId.ToString(), "Create", Cat_InsurGrade.InSurGrade.ToString());

                }
                return RedirectToAction("InsureGradeList", "HRVariables");
            }
            return View(Cat_InsurGrade);
        }

        public IActionResult EditInsureGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_InsurGrade = _insureGradeRepository.FindById(id);
            if (Cat_InsurGrade == null)
            {
                return NotFound();
            }
            return View("CreateInsureGrade", Cat_InsurGrade);
        }

        public IActionResult DeleteInsureGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_InsurGrade = _insureGradeRepository.FindById(id);

            if (Cat_InsurGrade == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateInsureGrade", Cat_InsurGrade);
        }

        [HttpPost, ActionName("DeleteInsureGrade")]
        public IActionResult DeleteInsureGradeConfirmed(int id)
        {
            try
            {
                var Cat_InsurGrade = _insureGradeRepository.FindById(id);
                _insureGradeRepository.Delete(Cat_InsurGrade);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_InsurGrade.InGId.ToString(), "Delete", Cat_InsurGrade.InSurGrade);

                return Json(new { Success = 1, InGId = Cat_InsurGrade.InGId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Exchange Rate
        public IActionResult ExchangeRateList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_exchangeRateRepository.GetAll().ToList());
        }

        public IActionResult CreateExchangeRate()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateExchangeRate(Cat_ExchangeRate Cat_ExchangeRate)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            Cat_ExchangeRate.ComId = comid;

            if (ModelState.IsValid)
            {
                if (Cat_ExchangeRate.ExChangeId > 0)
                {
                    Cat_ExchangeRate.UpdateByUserId = userId;
                    Cat_ExchangeRate.DateUpdated = DateTime.Now;
                    _exchangeRateRepository.Update(Cat_ExchangeRate);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ExchangeRate.ExChangeId.ToString(), "Update", Cat_ExchangeRate.ExchangeRate.ToString());

                }
                else
                {
                    Cat_ExchangeRate.UserId = userId;
                    Cat_ExchangeRate.DtInput = DateTime.Now;
                    _exchangeRateRepository.Add(Cat_ExchangeRate);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ExchangeRate.ExChangeId.ToString(), "Create", Cat_ExchangeRate.ExchangeRate.ToString());

                }
                return RedirectToAction("ExchangeRateList", "HRVariables");
            }
            return View(Cat_ExchangeRate);
        }

        public IActionResult EditExchangeRate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_ExchangeRate = _exchangeRateRepository.FindById(id);
            if (Cat_ExchangeRate == null)
            {
                return NotFound();
            }
            return View("CreateExchangeRate", Cat_ExchangeRate);
        }

        public IActionResult DeleteExchangeRate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_ExchangeRate = _exchangeRateRepository.FindById(id);

            if (Cat_ExchangeRate == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateExchangeRate", Cat_ExchangeRate);
        }

        [HttpPost, ActionName("DeleteExchangeRate")]
        public IActionResult DeleteExchangeRateConfirmed(int id)
        {
            try
            {
                var Cat_ExchangeRate = _exchangeRateRepository.FindById(id);
                _exchangeRateRepository.Delete(Cat_ExchangeRate);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ExchangeRate.ExChangeId.ToString(), "Delete", Cat_ExchangeRate.ExchangeRate.ToString());

                return Json(new { Success = 1, StyleId = Cat_ExchangeRate.ExChangeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Electric Charge Setting
        public IActionResult ECSettingList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_electricChargeSettingRepository.GetAll().ToList());
        }

        public IActionResult CreateECSetting()
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateECSetting(Cat_ElectricChargeSetting Cat_ElectricChargeSetting)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (ModelState.IsValid)
            {
                if (Cat_ElectricChargeSetting.Id > 0)
                {
                    _electricChargeSettingRepository.Update(Cat_ElectricChargeSetting);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ElectricChargeSetting.Id.ToString(), "Update", Cat_ElectricChargeSetting.ElectricCharge.ToString());

                }
                else
                {
                    _electricChargeSettingRepository.Add(Cat_ElectricChargeSetting);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ElectricChargeSetting.Id.ToString(), "Create", Cat_ElectricChargeSetting.ElectricCharge.ToString());

                }
                return RedirectToAction("ECSettingList", "HRVariables");
            }
            return View(Cat_ElectricChargeSetting);
        }

        public IActionResult EditECSetting(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_ElectricChargeSetting = _electricChargeSettingRepository.FindById(id);
            if (Cat_ElectricChargeSetting == null)
            {
                return NotFound();
            }
            return View("CreateECSetting", Cat_ElectricChargeSetting);
        }

        public IActionResult DeleteECSetting(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_ElectricChargeSetting = _electricChargeSettingRepository.FindById(id);

            if (Cat_ElectricChargeSetting == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateECSetting", Cat_ElectricChargeSetting);
        }

        [HttpPost, ActionName("DeleteECSetting")]
        public IActionResult DeleteECSettingConfirmed(int id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            try
            {
                var Cat_ElectricChargeSetting = _electricChargeSettingRepository.FindById(id);
                _electricChargeSettingRepository.Delete(Cat_ElectricChargeSetting);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ElectricChargeSetting.Id.ToString(), "Delete", Cat_ElectricChargeSetting.ElectricCharge.ToString());

                return Json(new { Success = 1, ECSettingId = Cat_ElectricChargeSetting.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Diagnosis
        public IActionResult DiagnosisList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_diagnosisRepository.GetAll().ToList());
        }

        public IActionResult CreateDiagnosis()
        {
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateDiagnosis(Cat_MedicalDiagnosis Cat_MedicalDiagnosis)
        {
            if (ModelState.IsValid)
            {
                if (Cat_MedicalDiagnosis.DiagId > 0)
                {
                    _diagnosisRepository.Update(Cat_MedicalDiagnosis);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_MedicalDiagnosis.DiagId.ToString(), "Update", Cat_MedicalDiagnosis.DiagName.ToString());

                }
                else
                {
                    _diagnosisRepository.Add(Cat_MedicalDiagnosis);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_MedicalDiagnosis.DiagId.ToString(), "Create", Cat_MedicalDiagnosis.DiagName.ToString());

                }
                return RedirectToAction("DiagnosisList", "HRVariables");
            }
            return View(Cat_MedicalDiagnosis);
        }

        public IActionResult EditDiagnosis(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_MedicalDiagnosis = _diagnosisRepository.FindById(id);
            if (Cat_MedicalDiagnosis == null)
            {
                return NotFound();
            }
            return View("CreateDiagnosis", Cat_MedicalDiagnosis);
        }

        public IActionResult DeleteDiagnosis(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cat_MedicalDiagnosis = _diagnosisRepository.FindById(id);

            if (Cat_MedicalDiagnosis == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateDiagnosis", Cat_MedicalDiagnosis);
        }

        [HttpPost, ActionName("DeleteDiagnosis")]
        public IActionResult DeleteDiagnosisConfirmed(int id)
        {
            try
            {
                var Cat_MedicalDiagnosis = _diagnosisRepository.FindById(id);
                _diagnosisRepository.Delete(Cat_MedicalDiagnosis);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_MedicalDiagnosis.DiagId.ToString(), "Delete", Cat_MedicalDiagnosis.DiagName);

                return Json(new { Success = 1, DiagId = Cat_MedicalDiagnosis.DiagId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Signatory
        public IActionResult SignatoryList()
        {
            ViewBag.ReportName = _signatoryRepository.ReportNames();
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_signatoryRepository.GetAll());
        }

        public IActionResult CreateSignatory()
        {
            ViewBag.ReportName = _signatoryRepository.ReportNames();
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateSignatory(Cat_ReportSignatory Cat_ReportSignatory)
        {
            if (ModelState.IsValid)
            {
                if (Cat_ReportSignatory.SignatoryId > 0)
                {
                    _signatoryRepository.Update(Cat_ReportSignatory);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ReportSignatory.SignatoryId.ToString(), "Update", Cat_ReportSignatory.Signatory1.ToString());

                }
                else
                {
                    _signatoryRepository.Add(Cat_ReportSignatory);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ReportSignatory.SignatoryId.ToString(), "Create", Cat_ReportSignatory.Signatory1.ToString());

                }
                return RedirectToAction("SignatoryList", "HRVariables");
            }
            return View(Cat_ReportSignatory);
        }

        public IActionResult EditSignatory(int? id)
        {
            ViewBag.ReportName = _signatoryRepository.ReportNames();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_ReportSignatory = _signatoryRepository.FindById(id);
            if (Cat_ReportSignatory == null)
            {
                return NotFound();
            }
            return View("CreateSignatory", Cat_ReportSignatory);
        }

        public IActionResult DeleteSignatory(int? id)
        {
            ViewBag.ReportName = _signatoryRepository.ReportNames();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_ReportSignatory = _signatoryRepository.FindById(id);

            if (Cat_ReportSignatory == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateSignatory", Cat_ReportSignatory);
        }

        [HttpPost, ActionName("DeleteSignatory")]
        public IActionResult DeleteSignatoryConfirmed(int id)
        {
            try
            {
                var Cat_ReportSignatory = _signatoryRepository.FindById(id);
                _signatoryRepository.Delete(Cat_ReportSignatory);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_ReportSignatory.SignatoryId.ToString(), "Delete", Cat_ReportSignatory.Signatory1);

                return Json(new { Success = 1, SignatoryId = Cat_ReportSignatory.SignatoryId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Ware House
        public IActionResult WareHouseList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_wareHouseRepository.GetAll().ToList());
        }

        public IActionResult CreateWareHouse()
        {
            ViewBag.ParentId = _wareHouseRepository.GetWarehouseList();
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateWareHouse(Warehouse Warehouse)
        {
            ViewBag.ParentId = _wareHouseRepository.GetWarehouseList();
            if (ModelState.IsValid)
            {
                if (Warehouse.WarehouseId > 0)
                {
                    _wareHouseRepository.Update(Warehouse);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Warehouse.WarehouseId.ToString(), "Update", Warehouse.WhName.ToString());

                }
                else
                {
                    _wareHouseRepository.Add(Warehouse);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Warehouse.WarehouseId.ToString(), "Create", Warehouse.WhName.ToString());

                }
                return RedirectToAction("WareHouseList", "HRVariables");
            }
            return View(Warehouse);
        }

        public IActionResult EditWareHouse(int? id)
        {
            ViewBag.ParentId = _wareHouseRepository.GetWarehouseList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Warehouse = _wareHouseRepository.FindById(id);
            if (Warehouse == null)
            {
                return NotFound();
            }
            return View("CreateWareHouse", Warehouse);
        }

        public IActionResult DeleteWareHouse(int? id)
        {
            ViewBag.ParentId = _wareHouseRepository.GetWarehouseList();
            if (id == null)
            {
                return NotFound();
            }

            var Warehouse = _wareHouseRepository.FindById(id);

            if (Warehouse == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateWareHouse", Warehouse);
        }

        [HttpPost, ActionName("DeleteWareHouse")]
        public IActionResult DeleteWareHouseConfirmed(int id)
        {
            ViewBag.ParentId = _wareHouseRepository.GetWarehouseList();
            try
            {
                var Warehouse = _wareHouseRepository.FindById(id);
                _wareHouseRepository.Delete(Warehouse);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Warehouse.WarehouseId.ToString(), "Delete", Warehouse.WhName);

                return Json(new { Success = 1, WareHouseId = Warehouse.WarehouseId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region HR Exp Setting
        public IActionResult HRExpList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_hRExpSettingRepository.GetAll().ToList());
        }

        public IActionResult CreateHRExp()
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateHRExp(Cat_HRExpSetting Cat_HRExpSetting)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (ModelState.IsValid)
            {
                if (Cat_HRExpSetting.Id > 0)
                {
                    _hRExpSettingRepository.Update(Cat_HRExpSetting);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_HRExpSetting.Id.ToString(), "Update", Cat_HRExpSetting.HRExp.ToString());

                }
                else
                {
                    _hRExpSettingRepository.Add(Cat_HRExpSetting);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_HRExpSetting.Id.ToString(), "Create", Cat_HRExpSetting.HRExp.ToString());

                }
                return RedirectToAction("HRExpList", "HRVariables");
            }
            return View(Cat_HRExpSetting);
        }

        public IActionResult EditHRExp(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_HRExpSetting = _hRExpSettingRepository.FindById(id);
            if (Cat_HRExpSetting == null)
            {
                return NotFound();
            }
            return View("CreateHRExp", Cat_HRExpSetting);
        }

        public IActionResult DeleteHRExp(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (id == null)
            {
                return NotFound();
            }

            var Cat_HRExpSetting = _hRExpSettingRepository.FindById(id);

            if (Cat_HRExpSetting == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateHRExp", Cat_HRExpSetting);
        }

        [HttpPost, ActionName("DeleteStyle")]
        public IActionResult DeleteHRExpConfirmed(int id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            try
            {
                var Cat_HRExpSetting = _hRExpSettingRepository.FindById(id);
                _hRExpSettingRepository.Delete(Cat_HRExpSetting);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_HRExpSetting.Id.ToString(), "Delete", Cat_HRExpSetting.HRExp.ToString());

                return Json(new { Success = 1, HRExpId = Cat_HRExpSetting.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region HR Setting
        public IActionResult HRSettingList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_hRSettingRepository.GetAllData());
        }

        public IActionResult CreateHRSetting()
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.Company = _hRSettingRepository.GetCompanyName();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            ViewBag.Company = new SelectList(_context.Companys.ToList(), "CompanyCode", "CompanyName");
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateHRSetting(Cat_HRSetting Cat_HRSetting)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.Company = _hRSettingRepository.GetCompanyName();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            Cat_HRSetting.UpdateByUserId = HttpContext.Session.GetString("userid");
            Cat_HRSetting.DateAdded = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));
            Cat_HRSetting.DateUpdated = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));

            if (ModelState.IsValid)
            {
                if (Cat_HRSetting.Id > 0)
                {
                    if (Cat_HRSetting.IsBSPersentage == true)
                    {
                        Cat_HRSetting.BS = Cat_HRSetting.BS / 100;
                    }
                    if (Cat_HRSetting.IsHRPersentage == true)
                    {
                        Cat_HRSetting.HR = Cat_HRSetting.HR / 100;
                    }
                    if (Cat_HRSetting.IsMAPersentage == true)
                    {
                        Cat_HRSetting.MA = Cat_HRSetting.MA / 100;
                    }
                    if (Cat_HRSetting.IsCAPersentage == true)
                    {
                        Cat_HRSetting.CA = Cat_HRSetting.CA / 100;
                    }
                    if (Cat_HRSetting.IsFAPersentage == true)
                    {
                        Cat_HRSetting.FA = Cat_HRSetting.FA / 100;
                    }
                    Cat_HRSetting.UpdateByUserId = HttpContext.Session.GetString("userid");
                    Cat_HRSetting.DateAdded = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));
                    Cat_HRSetting.DateUpdated = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));

                    _hRSettingRepository.Update(Cat_HRSetting);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_HRSetting.Id.ToString(), "Update", Cat_HRSetting.HR.ToString());

                }
                else
                {

                    if (Cat_HRSetting.IsBSPersentage == true)
                    {
                        Cat_HRSetting.BS = Cat_HRSetting.BS / 100;
                    }
                    else
                        Cat_HRSetting.BS = Cat_HRSetting.BS;

                    if (Cat_HRSetting.IsHRPersentage == true)
                    {
                        Cat_HRSetting.HR = Cat_HRSetting.HR / 100;
                    }
                    else
                        Cat_HRSetting.HR = Cat_HRSetting.HR;


                    if (Cat_HRSetting.IsMAPersentage == true)
                    {
                        Cat_HRSetting.MA = Cat_HRSetting.MA / 100;
                    }
                    else
                        Cat_HRSetting.MA = Cat_HRSetting.MA;

                    if (Cat_HRSetting.IsCAPersentage == true)
                    {
                        Cat_HRSetting.CA = Cat_HRSetting.CA / 100;
                    }
                    else
                        Cat_HRSetting.CA = Cat_HRSetting.CA;

                    if (Cat_HRSetting.IsFAPersentage == true)
                    {
                        Cat_HRSetting.FA = Cat_HRSetting.FA / 100;
                    }
                    else
                        Cat_HRSetting.FA = Cat_HRSetting.FA;

                    Cat_HRSetting.UpdateByUserId = HttpContext.Session.GetString("userid");
                    Cat_HRSetting.DateAdded = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));
                    Cat_HRSetting.DateUpdated = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));

                    _hRSettingRepository.Add(Cat_HRSetting);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_HRSetting.Id.ToString(), "Create", Cat_HRSetting.HR.ToString());

                }
                return RedirectToAction("HRSettingList", "HRVariables");
            }
            return View(Cat_HRSetting);
        }

        public IActionResult EditHRSetting(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.Company = _hRSettingRepository.GetCompanyName();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_HRSetting = _hRSettingRepository.FindById(id);
            if (Cat_HRSetting.IsBSPersentage == true)
            {
                Cat_HRSetting.BS = (float)Math.Round((Cat_HRSetting.BS * 100), 2);
            }
            if (Cat_HRSetting.IsHRPersentage == true)
            {
                Cat_HRSetting.HR = (float)Math.Round((Cat_HRSetting.HR * 100), 2);
            }
            if (Cat_HRSetting.IsMAPersentage == true)
            {
                Cat_HRSetting.MA = (float)Math.Round((Cat_HRSetting.MA * 100), 2);
            }
            if (Cat_HRSetting.IsCAPersentage == true)
            {
                Cat_HRSetting.CA = (float)Math.Round((Cat_HRSetting.CA * 100), 2);
            }
            if (Cat_HRSetting.IsFAPersentage == true)
            {
                Cat_HRSetting.FA = (float)Math.Round((Cat_HRSetting.FA * 100), 2);
            }
            if (Cat_HRSetting == null)
            {
                return NotFound();
            }
            return View("CreateHRSetting", Cat_HRSetting);
        }

        public IActionResult DeleteHRSetting(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.Company = _hRSettingRepository.GetCompanyName();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_HRSetting = _hRSettingRepository.FindById(id);
            if (Cat_HRSetting.IsBSPersentage == true)
            {
                Cat_HRSetting.BS = (float)Math.Round((Cat_HRSetting.BS * 100), 2);
            }
            if (Cat_HRSetting.IsHRPersentage == true)
            {
                Cat_HRSetting.HR = (float)Math.Round((Cat_HRSetting.HR * 100), 2);
            }
            if (Cat_HRSetting.IsMAPersentage == true)
            {
                Cat_HRSetting.MA = (float)Math.Round((Cat_HRSetting.MA * 100), 2);
            }
            if (Cat_HRSetting.IsCAPersentage == true)
            {
                Cat_HRSetting.CA = (float)Math.Round((Cat_HRSetting.CA * 100), 2);
            }
            if (Cat_HRSetting.IsFAPersentage == true)
            {
                Cat_HRSetting.FA = (float)Math.Round((Cat_HRSetting.FA * 100), 2);
            }
            if (Cat_HRSetting == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateHRSetting", Cat_HRSetting);
        }

        [HttpPost, ActionName("DeleteHRSetting")]
        public IActionResult DeleteHRSettingConfirmed(int id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.Company = _hRSettingRepository.GetCompanyName();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            try
            {
                var Cat_HRSetting = _hRSettingRepository.FindById(id);
                _hRSettingRepository.Delete(Cat_HRSetting);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_HRSetting.Id.ToString(), "Delete", Cat_HRSetting.HR.ToString());

                return Json(new { Success = 1, Id = Cat_HRSetting.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region HRReportType 
        public IActionResult HRReportTypeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_hRReportRepository.All().ToList());
        }

        public IActionResult CreateHRReportType()
        {
            ViewBag.Title = "Create";
            ViewBag.ReportType = _hRReportRepository.GetReportType();
            ViewBag.ComId = _hRReportRepository.GetComId();
            return View();
        }

        [HttpPost]
        public IActionResult CreateHRReportType(HR_ReportType HR_ReportType)
        {

            ViewBag.ReportType = _hRReportRepository.GetReportType();
            ViewBag.ComId = _hRReportRepository.GetComId();
            if (ModelState.IsValid)
            {
                if (HR_ReportType.ReportId > 0)
                {
                    _hRReportRepository.Update(HR_ReportType);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_ReportType.ReportId.ToString(), "Update", HR_ReportType.ReportName.ToString());

                }
                else
                {
                    _hRReportRepository.Add(HR_ReportType);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_ReportType.ReportId.ToString(), "Create", HR_ReportType.ReportName.ToString());

                }
                return RedirectToAction("HRReportTypeList", "HRVariables");
            }
            return View(HR_ReportType);
        }

        public IActionResult EditHRReportType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ReportType = _hRReportRepository.GetReportType();
            ViewBag.ComId = _hRReportRepository.GetComId();
            ViewBag.Title = "Edit";
            var HR_ReportType = _hRReportRepository.FindById(id);
            if (HR_ReportType == null)
            {
                return NotFound();
            }
            return View("CreateHRReportType", HR_ReportType);
        }

        public IActionResult DeleteHRReportType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ReportType = _hRReportRepository.GetReportType();
            ViewBag.ComId = _hRReportRepository.GetComId();
            var HR_ReportType = _hRReportRepository.FindById(id);

            if (HR_ReportType == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateHRReportType", HR_ReportType);
        }

        [HttpPost, ActionName("DeleteHRReportType")]
        public IActionResult DeleteHRReportTypeConfirmed(int id)
        {
            ViewBag.ReportType = _hRReportRepository.GetReportType();
            ViewBag.ComId = _hRReportRepository.GetComId();
            try
            {
                var HR_ReportType = _hRReportRepository.FindById(id);
                _hRReportRepository.Delete(HR_ReportType);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_ReportType.ReportId.ToString(), "Delete", HR_ReportType.ReportName);

                return Json(new { Success = 1, StyleId = HR_ReportType.ReportId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Custom Report
        public IActionResult HRCustomReportList()
        {
            string comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");



            //ViewBag.CompanyName = _context.HR_CustomReport
            //                    .Include(x => x.Companys)
            //                    .Where(x => x.CompanyCode == comid).ToList();

            return View(_hRReportRepository.GetCustomReport());
        }

        public IActionResult CreateHRCustomReport()
        {
            ViewBag.Title = "Create";
            ViewBag.ReportType = _hRCustomReportRepository.GetReportType();
            ViewBag.EmpType = _hRCustomReportRepository.GetEmpType();
            ViewBag.CustomType = _hRCustomReportRepository.GetReportTypeCustom();
            ViewBag.ComId = _hRCustomReportRepository.GetComId();
            return View();
        }

        [HttpPost]
        public IActionResult CreateHRCustomReport(HR_CustomReport HR_CustomReport)
        {

            ViewBag.ReportType = _hRCustomReportRepository.GetReportType();
            ViewBag.CustomType = _hRCustomReportRepository.GetReportTypeCustom();
            ViewBag.EmpType = _hRCustomReportRepository.GetEmpType();
            ViewBag.ComId = _hRCustomReportRepository.GetComId();
            if (ModelState.IsValid)
            {
                if (HR_CustomReport.CustomReportId > 0)
                {
                    _hRCustomReportRepository.Update(HR_CustomReport);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_CustomReport.CustomReportId.ToString(), "Update", HR_CustomReport.ReportName.ToString());

                }
                else
                {
                    _hRCustomReportRepository.Add(HR_CustomReport);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_CustomReport.CustomReportId.ToString(), "Create", HR_CustomReport.ReportName.ToString());

                }
                return RedirectToAction("HRCustomReportList", "HRVariables");
            }
            return View(HR_CustomReport);
        }

        public IActionResult EditHRCustomReport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ReportType = _hRCustomReportRepository.GetReportType();
            ViewBag.CustomType = _hRCustomReportRepository.GetReportTypeCustom();
            ViewBag.EmpType = _hRCustomReportRepository.GetEmpType();
            ViewBag.ComId = _hRCustomReportRepository.GetComId();
            ViewBag.Title = "Edit";
            var HR_CustomReport = _hRCustomReportRepository.FindById(id);
            if (HR_CustomReport == null)
            {
                return NotFound();
            }
            return View("CreateHRCustomReport", HR_CustomReport);
        }

        public IActionResult DeleteHRCustomReport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ReportType = _hRCustomReportRepository.GetReportType();
            ViewBag.CustomType = _hRCustomReportRepository.GetReportTypeCustom();
            ViewBag.EmpType = _hRCustomReportRepository.GetEmpType();
            ViewBag.ComId = _hRCustomReportRepository.GetComId();
            var HR_CustomReport = _hRCustomReportRepository.FindById(id);

            if (HR_CustomReport == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateHRCustomReport", HR_CustomReport);
        }

        [HttpPost, ActionName("DeleteHRCustomReport")]
        public IActionResult DeleteHRCustomReportConfirmed(int id)
        {
            ViewBag.ReportType = _hRCustomReportRepository.GetReportType();
            ViewBag.CustomType = _hRCustomReportRepository.GetReportTypeCustom();
            ViewBag.EmpType = _hRCustomReportRepository.GetEmpType();
            ViewBag.ComId = _hRCustomReportRepository.GetComId();
            try
            {
                var HR_CustomReport = _hRCustomReportRepository.FindById(id);
                _hRCustomReportRepository.Delete(HR_CustomReport);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), HR_CustomReport.CustomReportId.ToString(), "Delete", HR_CustomReport.ReportName);

                return Json(new { Success = 1, StyleId = HR_CustomReport.CustomReportId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Strength
        public IActionResult StrengthList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_strengthRepository.GetAll().ToList());
        }

        public IActionResult CreateStrength()
        {
            ViewBag.Title = "Create";
            ViewData["strengthType"] = _strengthRepository.GetStrengthList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateStrength(Cat_Strength Cat_Strength)
        {
            ViewData["strengthType"] = _strengthRepository.GetStrengthList();
            if (ModelState.IsValid)
            {
                if (Cat_Strength.StId > 0)
                {
                    _strengthRepository.Update(Cat_Strength);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Strength.StId.ToString(), "Update", Cat_Strength.StrengthType.ToString());

                }
                else
                {
                    _strengthRepository.Add(Cat_Strength);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Strength.StId.ToString(), "Create", Cat_Strength.StrengthType.ToString());

                }
                return RedirectToAction("StrengthList", "HRVariables");
            }
            return View(Cat_Strength);
        }
        public JsonResult GetStates(string id)
        {
            if (id == "1")
            {
                var state = _departmentRepository.GetAll()

                        .Select(l => new
                        {
                            a = l.DeptId,
                            b = l.DeptName,

                        }).OrderBy(l => l.b).ToList();

                return Json(state);

            }
            else if (id == "2")
            {
                var state = _sectionRepository.GetAll()

                    .Select(l => new
                    {
                        a = l.SectId,
                        b = l.SectName,

                    }).OrderBy(l => l.b).ToList();

                return Json(state);
            }
            else if (id == "3")
            {
                var state = _designationRepository.GetAll()

                    .Select(l => new
                    {
                        a = l.DesigId,
                        b = l.DesigName,

                    }).OrderBy(l => l.b).ToList();

                return Json(state);
            }

            return Json("No data found");
        }

        public IActionResult EditStrength(int? id)
        {
            ViewData["strengthType"] = _strengthRepository.GetStrengthList();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_Strength = _strengthRepository.FindById(id);
            if (Cat_Strength == null)
            {
                return NotFound();
            }
            return View("CreateStrength", Cat_Strength);
        }

        public IActionResult DeleteStrength(int? id)
        {
            ViewData["strengthType"] = _strengthRepository.GetStrengthList();
            if (id == null)
            {
                return NotFound();
            }

            var Cat_Strength = _strengthRepository.FindById(id);

            if (Cat_Strength == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("CreateStrength", Cat_Strength);
        }

        [HttpPost, ActionName("DeleteStrength")]
        public IActionResult DeleteStrengthConfirmed(int id)
        {
            ViewData["strengthType"] = _strengthRepository.GetStrengthList();
            try
            {
                var Cat_Strength = _strengthRepository.FindById(id);
                _strengthRepository.Delete(Cat_Strength);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Strength.StId.ToString(), "Delete", Cat_Strength.StrengthType);

                return Json(new { Success = 1, StId = Cat_Strength.StId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Sub Section
        public IActionResult SubSectList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            //var section
            return View(_subSectionRepository.GetSubSectionAll());
        }

        // GET: Section/Details/5
        public IActionResult SubSectDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat_SubSection = _subSectionRepository.Details(id);
            if (cat_SubSection == null)
            {
                return NotFound();
            }

            return View(cat_SubSection);
        }

        // GET: Section/Create
        public IActionResult CreateSubSect()
        {
            ViewBag.Title = "Create";
            ViewBag.DeptId = _departmentRepository.GetDepartmentList();
            ViewBag.SectId = _sectionRepository.GetSectionList();
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateSubSect(Cat_SubSection cat_SubSection)
        {
            if (ModelState.IsValid)
            {
                cat_SubSection.UserId = HttpContext.Session.GetString("userid");
                cat_SubSection.ComId = HttpContext.Session.GetString("comid");

                cat_SubSection.DateUpdated = DateTime.Today;
                cat_SubSection.DtInput = DateTime.Today;
                cat_SubSection.DateAdded = DateTime.Today;

                if (cat_SubSection.SubSectId > 0)
                {
                    cat_SubSection.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _subSectionRepository.Update(cat_SubSection);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_SubSection.SubSectId.ToString(), "Update", cat_SubSection.SubSectName.ToString());

                }
                else
                {
                    _subSectionRepository.Add(cat_SubSection);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_SubSection.SubSectId.ToString(), "Create", cat_SubSection.SubSectName.ToString());

                }
                return RedirectToAction(nameof(SubSectList));
            }
            return View(cat_SubSection);
        }

        // GET: Section/Edit/5
        public IActionResult EditSubSect(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var cat_SubSection = _subSectionRepository.FindById(id);
            ViewBag.DeptId = _departmentRepository.GetDepartmentList();
            ViewBag.SectId = _sectionRepository.GetSectionList();
            if (cat_SubSection == null)
            {
                return NotFound();
            }
            return View("CreateSubSect", cat_SubSection);
        }


        // GET: Section/Delete/5
        public IActionResult DeleteSubSect(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat_SubSection = _subSectionRepository.Details(id);
            if (cat_SubSection == null)
            {
                return NotFound();
            }


            ViewBag.Title = "Delete";
            ViewBag.DeptId = _departmentRepository.GetDepartmentList();
            ViewBag.SectId = _sectionRepository.GetSectionList();

            return View("CreateSubSect", cat_SubSection);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteSubSect")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteSubSectConfirmed(int id)
        {
            try
            {
                var cat_SubSection = _subSectionRepository.FindById(id);
                _subSectionRepository.Delete(cat_SubSection);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_SubSection.SubSectId.ToString(), "Delete", cat_SubSection.SubSectName);
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, SubSectId = cat_SubSection.SubSectId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Size
        public IActionResult SizeList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            return View(_sizeRepository.GetAll().Include(x => x.Cat_Color));
        }



        // GET: Size
        public IActionResult CreateSize()
        {
            ViewBag.Title = "Create";
            ViewBag.Color = _colorRepository.GetColor();
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateSize(Cat_Size cat_Size)
        {
            ViewBag.Color = _colorRepository.GetColor();
            if (ModelState.IsValid)
            {
                cat_Size.UserId = HttpContext.Session.GetString("userid");
                cat_Size.ComId = HttpContext.Session.GetString("comid");

                cat_Size.DateUpdated = DateTime.Today;
                cat_Size.DateAdded = DateTime.Today;

                if (cat_Size.SizeId > 0)
                {
                    cat_Size.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _sizeRepository.Update(cat_Size);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Size.SizeId.ToString(), "Update", cat_Size.SizeName.ToString());

                }
                else
                {
                    _sizeRepository.Add(cat_Size);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Size.SizeId.ToString(), "Create", cat_Size.SizeName.ToString());

                }
                return RedirectToAction(nameof(SizeList));
            }
            return View(cat_Size);
        }

        // GET: Size/Edit/5
        public IActionResult EditSize(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var cat_Size = _sizeRepository.FindById(id);
            ViewBag.Color = _colorRepository.GetColor();

            if (cat_Size == null)
            {
                return NotFound();
            }
            return View("CreateSize", cat_Size);
        }


        // GET: Section/Delete/5
        public IActionResult DeleteSize(int? id)
        {
            ViewBag.Color = _colorRepository.GetColor();
            if (id == null)
            {
                return NotFound();
            }

            var cat_Size = _sizeRepository.FindById(id);
            if (cat_Size == null)
            {
                return NotFound();
            }


            ViewBag.Title = "Delete";


            return View("CreateSize", cat_Size);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteSize")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteSizeConfirmed(int id)
        {
            ViewBag.Color = _colorRepository.GetColor();
            try
            {
                var cat_Size = _sizeRepository.FindById(id);
                _sizeRepository.Delete(cat_Size);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Size.SizeId.ToString(), "Delete", cat_Size.SizeName);
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, SubSectId = cat_Size.SizeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Color
        public IActionResult ColorList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            return View(_colorRepository.GetAll().Include(x => x.Cat_Style));
        }



        // GET: Size
        public IActionResult CreateColor()
        {
            ViewBag.Title = "Create";
            ViewBag.Color = _colorRepository.GetColor();
            ViewBag.Style = _styleRepository.GetStyleList();
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateColor(Cat_Color cat_Color)
        {
            ViewBag.Color = _colorRepository.GetColor();
            ViewBag.Style = _styleRepository.GetStyleList();
            if (ModelState.IsValid)
            {
                cat_Color.UserId = HttpContext.Session.GetString("userid");
                cat_Color.ComId = HttpContext.Session.GetString("comid");

                cat_Color.DateUpdated = DateTime.Today;
                cat_Color.DateAdded = DateTime.Today;

                if (cat_Color.ColorId > 0)
                {
                    cat_Color.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _colorRepository.Update(cat_Color);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Color.ColorId.ToString(), "Update", cat_Color.ColorName.ToString());

                }
                else
                {
                    _colorRepository.Add(cat_Color);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Color.ColorId.ToString(), "Create", cat_Color.ColorName.ToString());

                }
                return RedirectToAction(nameof(ColorList));
            }
            return View(cat_Color);
        }

        // GET: Size/Edit/5
        public IActionResult EditColor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var cat_Color = _colorRepository.FindById(id);
            ViewBag.Style = _styleRepository.GetStyleList();
            ViewBag.Color = _colorRepository.GetColor();

            if (cat_Color == null)
            {
                return NotFound();
            }
            return View("CreateColor", cat_Color);
        }


        // GET: Section/Delete/5
        public IActionResult DeleteColor(int? id)
        {
            ViewBag.Color = _colorRepository.GetColor();
            ViewBag.Style = _styleRepository.GetStyleList();
            if (id == null)
            {
                return NotFound();
            }

            var cat_Color = _colorRepository.FindById(id);
            if (cat_Color == null)
            {
                return NotFound();
            }


            ViewBag.Title = "Delete";


            return View("CreateColor", cat_Color);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteColor")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteColorConfirmed(int id)
        {
            ViewBag.Color = _colorRepository.GetColor();
            ViewBag.Style = _styleRepository.GetStyleList();
            try
            {
                var cat_Color = _colorRepository.FindById(id);
                _colorRepository.Delete(cat_Color);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_Color.ColorId.ToString(), "Delete", cat_Color.ColorName);
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, SubSectId = cat_Color.ColorId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region HR Overtime Setting
        public IActionResult OvertimeSettingList()
        {
            return View(_hROvertimeSettingRepository.GetOvertimeList());
        }
        public IActionResult CreateOvertimeSetting()
        {
            ViewBag.Title = "Create";
            ViewBag.CompanyName = _hROvertimeSettingRepository.GetCompany();
            return View();
        }

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateOvertimeSetting(HR_OverTimeSetting Overtime)
        {

            if (ModelState.IsValid)
            {
                Overtime.DateUpdated = DateTime.Today;
                Overtime.DateAdded = DateTime.Today;

                if (Overtime.Id > 0)
                {
                    //Overtime.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _hROvertimeSettingRepository.Update(Overtime);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Overtime.Id.ToString(), "Update", Overtime.CompanyId.ToString());

                }
                else
                {

                    _hROvertimeSettingRepository.Add(Overtime);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Overtime.Id.ToString(), "Create", Overtime.CompanyId.ToString());

                }
                return RedirectToAction(nameof(OvertimeSettingList));
            }
            return View(Overtime);
        }

        public IActionResult EditOvertimeSetting(int? id)
        {
            ViewBag.CompanyName = _hROvertimeSettingRepository.GetCompany();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Overtime = _hROvertimeSettingRepository.FindById(id);

            if (Overtime == null)
            {
                return NotFound();
            }
            return View("CreateOvertimeSetting", Overtime);
        }

        public IActionResult DeleteOvertimeSetting(int? id)
        {
            ViewBag.CompanyName = _hROvertimeSettingRepository.GetCompany();
            if (id == null)
            {
                return NotFound();
            }

            var Overtime = _hROvertimeSettingRepository.FindById(id);
            if (Overtime == null)
            {
                return NotFound();
            }


            ViewBag.Title = "Delete";


            return View("CreateOvertimeSetting", Overtime);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteOvertimeSetting")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteOvertimeSettingConfirmed(int id)
        {
            ViewBag.CompanyName = _hROvertimeSettingRepository.GetCompany();
            try
            {
                var Overtime = _hROvertimeSettingRepository.FindById(id);
                _hROvertimeSettingRepository.Delete(Overtime);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Overtime.Id.ToString(), "Delete", Overtime.CompanyId);
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, SubSectId = Overtime.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Income Tax Amount Setting
        public IActionResult TaxSettingList()
        {
            return View(_taxAmountSettingRepository.GetIncometaxList());
        }



        // GET: Size
        public IActionResult CreateTaxSetting()
        {
            ViewBag.CompanyName = _hROvertimeSettingRepository.GetCompany();
            ViewBag.Title = "Create";
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateTaxSetting(Payroll_InComeTaxAmountSetting tax)
        {

            if (ModelState.IsValid)
            {
                if (tax.Id > 0)
                {

                    _taxAmountSettingRepository.UpdateData(tax);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), tax.Id.ToString(), "Update", tax.IncomeTax.ToString());

                }
                else
                {
                    _taxAmountSettingRepository.SaveData(tax);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), tax.Id.ToString(), "Create", tax.IncomeTax.ToString());

                }
                return RedirectToAction(nameof(TaxSettingList));
            }
            return View(tax);
        }

        // GET: Size/Edit/5
        public IActionResult EditTaxSetting(int? id)
        {
            ViewBag.CompanyName = _hROvertimeSettingRepository.GetCompany();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var tax = _taxAmountSettingRepository.FindById(id);

            if (tax == null)
            {
                return NotFound();
            }
            return View("CreateTaxSetting", tax);
        }


        // GET: Section/Delete/5
        public IActionResult DeleteTaxSetting(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var tax = _taxAmountSettingRepository.FindById(id);
            if (tax == null)
            {
                return NotFound();
            }


            ViewBag.Title = "Delete";


            return View("CreateTaxSetting", tax);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteTaxSetting")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteTaxSettingConfirmed(int id)
        {

            try
            {
                var tax = _taxAmountSettingRepository.FindById(id);
                _taxAmountSettingRepository.Delete(tax);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), tax.Id.ToString(), "Delete", tax.IncomeTax.ToString());
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, Id = tax.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region HR Approval Setting
        public IActionResult HRApprovalSettingList()
        {
            var data = _hR_ApprovalSettingRepository.GetApproveList();
            return View(data);
        }



        // GET: Size
        public IActionResult CreateHRApproval()
        {
            var userid = HttpContext.Session.GetString("userid");
            ViewBag.CompanyName = _hR_ApprovalSettingRepository.GetCompanyList();

            ViewBag.useridPermission = _hR_ApprovalSettingRepository.GetUserList();

            ViewBag.ApprovalList = _hR_ApprovalSettingRepository.GetApprovalType();
            ViewBag.Title = "Create";
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateHRApproval(HR_ApprovalSetting approval)
        {

            if (ModelState.IsValid)
            {
                if (approval.ApprovalSettingId > 0)
                {

                    _hR_ApprovalSettingRepository.Update(approval);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), approval.ApprovalSettingId.ToString(), "Update", approval.UserId.ToString());

                }
                else
                {
                    var exitance = _context.HR_ApprovalSettings.Where(w => w.ComId == approval.ComId && w.UserId == approval.UserId && w.ApprovalType == approval.ApprovalType && w.IsDelete == false).Select(s => s.ApprovalSettingId).FirstOrDefault();
                    if (exitance == 0)
                    {
                        _hR_ApprovalSettingRepository.Add(approval);
                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), approval.ApprovalSettingId.ToString(), "Create", approval.UserId.ToString());
                    }
                    else
                    {
                        ViewBag.info = "Approval have already given";
                        var userid = HttpContext.Session.GetString("userid");
                        ViewBag.CompanyName = _hR_ApprovalSettingRepository.GetCompanyList();

                        ViewBag.useridPermission = _hR_ApprovalSettingRepository.GetUserList();

                        ViewBag.ApprovalList = _hR_ApprovalSettingRepository.GetApprovalType();
                        ViewBag.Title = "Create";
                        return View(approval);
                    }
                }
                return RedirectToAction(nameof(HRApprovalSettingList));
            }
            return View(approval);
        }

        // GET: Size/Edit/5
        public IActionResult EditHRApproval(int? id)
        {
            ViewBag.CompanyName = _hR_ApprovalSettingRepository.GetCompanyList();
            ViewBag.useridPermission = _hR_ApprovalSettingRepository.GetUserList();
            ViewBag.ApprovalList = _hR_ApprovalSettingRepository.GetApprovalType();

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var approval = _hR_ApprovalSettingRepository.FindById(id);

            if (approval == null)
            {
                return NotFound();
            }
            return View("CreateHRApproval", approval);
        }


        // GET: Section/Delete/5
        public IActionResult DeleteHRApproval(int? id)
        {
            ViewBag.CompanyName = _hR_ApprovalSettingRepository.GetCompanyList();
            ViewBag.useridPermission = _hR_ApprovalSettingRepository.GetUserList();
            ViewBag.ApprovalList = _hR_ApprovalSettingRepository.GetApprovalType();
            if (id == null)
            {
                return NotFound();
            }

            var approval = _hR_ApprovalSettingRepository.FindById(id);
            if (approval == null)
            {
                return NotFound();
            }


            ViewBag.Title = "Delete";


            return View("CreateHRApproval", approval);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteHRApproval")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteHRApprovalConfirmed(int id)
        {

            try
            {
                var approval = _hR_ApprovalSettingRepository.FindById(id);
                _hR_ApprovalSettingRepository.Delete(approval);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), approval.ApprovalSettingId.ToString(), "Delete", approval.UserId.ToString());
                return Json(new { Success = 1, Id = approval.ApprovalSettingId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        public IActionResult ApproveEntryMenu()
        {

            ViewBag.CompanyName = _hR_ApprovalSettingRepository.GetCompanyName();
            ViewBag.useridPermission = _hR_ApprovalSettingRepository.GetApprovedBy();
            ViewBag.ApprovalList = _hR_ApprovalSettingRepository.VarType();
            return View();
        }

        [HttpPost]
        public IActionResult ApproveEntryMenu(string comid, int approve)
        {
            var ComId = HttpContext.Session.GetString("comid");

            ViewBag.CompanyName = _hR_ApprovalSettingRepository.GetCompanyName();
            ViewBag.ApprovalList = _hR_ApprovalSettingRepository.VarType();
            ViewBag.useridPermission = _hR_ApprovalSettingRepository.GetApprovedBy();
            if (approve == 1173)
            {
                ViewBag.TableName = "Salary";
                List<SetApproveViewModel> data = _hR_ApprovalSettingRepository.GetApprovalList(comid, approve);
                ViewBag.ApproveList = data;
                return View(data);
            }
            if (approve == 1174)
            {
                ViewBag.TableName = "Employee";
                List<SetApproveViewModel> data = _hR_ApprovalSettingRepository.GetApprovalList(comid, approve);
                ViewBag.ApproveList = data;
                return View(data);
            }
            if (approve == 1175)
            {
                ViewBag.TableName = "Leave";
                List<SetApproveViewModel> data = _hR_ApprovalSettingRepository.GetApprovalList(comid, approve);
                ViewBag.ApproveList = data;
                return View(data);
            }
            if (approve == 1176)
            {
                ViewBag.TableName = "Increment";
                List<SetApproveViewModel> data = _hR_ApprovalSettingRepository.GetApprovalList(comid, approve);
                ViewBag.ApproveList = data;
                return View(data);
            }
            if (approve == 1177)
            {
                ViewBag.TableName = "Release";
                List<SetApproveViewModel> data = _hR_ApprovalSettingRepository.GetApprovalList(comid, approve);
                ViewBag.ApproveList = data;
                return View(data);
            }
            if (approve == 1178)
            {
                ViewBag.TableName = "FixedAtt";
                List<SetApproveViewModel> data = _hR_ApprovalSettingRepository.GetApprovalList(comid, approve);
                ViewBag.ApproveList = data;
                return View(data);
            }
            else
                return View();

        }
        [HttpPost]
        public IActionResult SaveApprovalList(List<ApprovalVM> Approve)
        {
            try
            {
                _hR_ApprovalSettingRepository.Approved(Approve);
                TempData["Message"] = "Data Approved Successfully";
                TempData["Status"] = "1";

                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return View("ApproveEntryMenu");

        }

        [HttpPost]
        public IActionResult SaveDisapproveList(List<ApprovalVM> Approve)
        {
            _hR_ApprovalSettingRepository.Disapproved(Approve);
            TempData["Message"] = "Data Disapproved Successfully";
            TempData["Status"] = "2";
            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
        }
        public class CompanyList
        {
            public Guid ComId { get; set; }
            public string CompanyShortName { get; set; }
        }
        #endregion

        #region Income Tax
        // GET: Section
        public IActionResult IncomeTaxList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            //var sectio
            return View(_incomeTaxRepository.GetAll());
        }


        // GET: Section/Create
        public IActionResult CreateIncomeTax()
        {

            ViewBag.Title = "Create";
            ViewBag.ProssType = new SelectList(_incomeTaxRepository.GetProssType(), "ProssType", "ProssType");

            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateIncomeTax(Cat_IncomeTaxChk cat_IncomeTaxChk)
        {
            if (ModelState.IsValid)
            {

                if (cat_IncomeTaxChk.Id > 0)
                {
                    _incomeTaxRepository.Update(cat_IncomeTaxChk);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_IncomeTaxChk.Id.ToString(), "Update", cat_IncomeTaxChk.Id.ToString());

                }
                else
                {
                    _incomeTaxRepository.Add(cat_IncomeTaxChk);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_IncomeTaxChk.Id.ToString(), "Create", cat_IncomeTaxChk.Id.ToString());

                }
                return RedirectToAction(nameof(IncomeTaxList));
            }
            return View(cat_IncomeTaxChk);
        }

        // GET: Section/Edit/5
        public IActionResult EditIncomeTax(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var cat_IncomeTaxChk = _incomeTaxRepository.FindById(id);

            ViewBag.ProssType = new SelectList(_incomeTaxRepository.GetProssType(), "ProssType", "ProssType", cat_IncomeTaxChk.ProssType);
            if (cat_IncomeTaxChk == null)
            {
                return NotFound();
            }
            return View("CreateIncomeTax", cat_IncomeTaxChk);
        }


        // GET: Section/Delete/5
        public IActionResult DeleteIncomeTax(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            var cat_IncomeTaxChk = _incomeTaxRepository.FindById(id);
            if (cat_IncomeTaxChk == null)
            {
                return NotFound();
            }

            ViewBag.ProssType = new SelectList(_incomeTaxRepository.GetProssType(), "ProssType", "ProssType", cat_IncomeTaxChk.ProssType);
            return View("CreateIncomeTax", cat_IncomeTaxChk);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteIncomeTax")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteIncomeTaxConfirmed(int id)
        {
            try
            {
                var cat_IncomeTaxChk = _incomeTaxRepository.FindById(id);
                _incomeTaxRepository.Delete(cat_IncomeTaxChk);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_IncomeTaxChk.Id.ToString(), "Delete", cat_IncomeTaxChk.Id.ToString());

                return Json(new { Success = 1, SectId = cat_IncomeTaxChk.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region Salary Month Setting

        public IActionResult SalaryMonthList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            var data = _salaryMonthRepository.GetAll();
            return View(data);
        }


        // GET: Section/Create
        public IActionResult CreateSalaryMonth()
        {

            ViewBag.Title = "Create";
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult CreateSalaryMonth(Cat_SalaryMonth cat_SalaryMonth)
        {
            if (ModelState.IsValid)
            {

                if (cat_SalaryMonth.Id > 0)
                {
                    _salaryMonthRepository.Update(cat_SalaryMonth);
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_SalaryMonth.Id.ToString(), "Update", cat_SalaryMonth.Id.ToString());

                }
                else
                {
                    _salaryMonthRepository.Add(cat_SalaryMonth);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_SalaryMonth.Id.ToString(), "Create", cat_SalaryMonth.Id.ToString());

                }
                return RedirectToAction(nameof(SalaryMonthList));
            }
            return View(cat_SalaryMonth);
        }

        // GET: Section/Edit/5
        public IActionResult EditSalaryMonth(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var cat_SalaryMonth = _salaryMonthRepository.FindById(id);
            if (cat_SalaryMonth == null)
            {
                return NotFound();
            }
            return View("CreateSalaryMonth", cat_SalaryMonth);
        }


        // GET: Section/Delete/5
        public IActionResult DeleteSalaryMonth(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            var cat_SalaryMonth = _salaryMonthRepository.FindById(id);
            if (cat_SalaryMonth == null)
            {
                return NotFound();
            }
            return View("CreateSalaryMonth", cat_SalaryMonth);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("DeleteSalaryMonth")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteSalaryMonthConfirmed(int id)
        {
            try
            {
                var cat_SalaryMonth = _salaryMonthRepository.FindById(id);
                _salaryMonthRepository.Delete(cat_SalaryMonth);
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cat_SalaryMonth.Id.ToString(), "Delete", cat_SalaryMonth.Id.ToString());

                return Json(new { Success = 1, SectId = cat_SalaryMonth.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion

        #region attendance Bonus Setting
        public IActionResult AttnBonusList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(_Cat_AttBonusSetting.GetAll().ToList());
        }

        public IActionResult CreateAttBonusSetting()
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateAttBonusSetting(Cat_AttBonusSetting Cat_AttBonusSetting)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (ModelState.IsValid)
            {
                if (Cat_AttBonusSetting.Id > 0)
                {
                    _Cat_AttBonusSetting.Update(Cat_AttBonusSetting);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_AttBonusSetting.Id.ToString(), "Update", Cat_AttBonusSetting.ToString());

                }
                else
                {
                    _Cat_AttBonusSetting.Add(Cat_AttBonusSetting);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_AttBonusSetting.Id.ToString(), "Create", Cat_AttBonusSetting.UpdateByUserId.ToString());

                }
                return RedirectToAction("AttnBonusList", "HRVariables");
            }
            return View(Cat_AttBonusSetting);
        }

        public IActionResult EditAttBonusSetting(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Cat_AttBonusSetting = _Cat_AttBonusSetting.FindById(id);
            if (Cat_AttBonusSetting == null)
            {
                return NotFound();
            }
            return View("CreateAttBonusSetting", Cat_AttBonusSetting);
        }

        public IActionResult DeleteAttBonusSetting(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (id == null)
            {
                return NotFound();
            }

            var AttBonusSetting = _Cat_AttBonusSetting.FindById(id);

            if (AttBonusSetting == null)
            {
                return NotFound();
            }
            _Cat_AttBonusSetting.Delete(AttBonusSetting);
            ViewBag.Title = "Delete";

            return RedirectToAction("AttnBonusList");
        }


        #endregion

        #region Cat_Stamp
        public IActionResult Cat_StampList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            IEnumerable<SelectListItem> paymode = _context.Cat_PayMode.Select(x => new SelectListItem
            {
                Value = x.PayModeId.ToString(),
                Text = x.PayModeName
            }); ;
            ViewBag.paymode = paymode;

            return View(_Cat_Stamp.GetStampList().ToList());
        }

        public IActionResult CreateCat_Stamp()
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            ViewBag.Title = "Create";
            IEnumerable<SelectListItem> paymode = _context.Cat_PayMode.Select(x => new SelectListItem
            {
                Value = x.PayModeId.ToString(),
                Text = x.PayModeName
            }); ;
            ViewBag.paymode = paymode;
            return View();
        }

        [HttpPost]
        public IActionResult CreateCat_Stamp(Cat_Stamp Cat_Stamp)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (ModelState.IsValid)
            {
                if (Cat_Stamp.Id > 0)
                {
                    _Cat_Stamp.Update(Cat_Stamp);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Stamp.Id.ToString(), "Update", Cat_Stamp.ComId.ToString());

                }
                else
                {
                    _Cat_Stamp.Add(Cat_Stamp);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_AttBonusSetting.Id.ToString(), "Create", Cat_AttBonusSetting.UpdateByUserId.ToString());

                }
                return RedirectToAction("Cat_StampList", "HRVariables");
            }
            return View(Cat_Stamp);
        }

        public IActionResult EditCat_Stamp(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            IEnumerable<SelectListItem> paymode = _context.Cat_PayMode.Select(x => new SelectListItem
            {
                Value = x.PayModeId.ToString(),
                Text = x.PayModeName
            }); ;
            ViewBag.paymode = paymode;

            var Cat_Stamp = _Cat_Stamp.FindById(id);
            if (Cat_Stamp == null)
            {
                return NotFound();
            }
            return View("CreateCat_Stamp", Cat_Stamp);
        }

        public IActionResult DeleteCat_Stamp(int? id)
        {
            ViewBag.EmpTypeId = _empTypeRepository.GetEmpTypeList();
            ViewBag.LId = _locationRepository.GetLocationList();
            ViewBag.BId = _buildingTypeRepository.GetBuildingType();

            if (id == null)
            {
                return NotFound();
            }

            var Cat_Stamp = _Cat_Stamp.FindById(id);

            if (Cat_Stamp == null)
            {
                return NotFound();
            }
            IEnumerable<SelectListItem> paymode = _context.Cat_PayMode.Select(x => new SelectListItem
            {
                Value = x.PayModeId.ToString(),
                Text = x.PayModeName
            }); ;
            ViewBag.paymode = paymode;
            ViewBag.Title = "Delete";

            return View("CreateCat_Stamp", Cat_Stamp);
        }

        //[ValidateAntiForgeryToken]
        public IActionResult DeleteCat_StampConfirmed(int id)
        {

            try
            {
                var Cat_Stamp = _Cat_Stamp.FindById(id);
                _Cat_Stamp.Delete(Cat_Stamp);

                //TempData["Message"] = "Data Delete Successfully";
                //TempData["Status"] = "3";
                //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), approval.ApprovalSettingId.ToString(), "Delete", approval.UserId.ToString());
                return RedirectToAction("Cat_StampList", "HRVariables");
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        #endregion

        #region VariableTransfer
        [HttpGet]
        public IActionResult VariableTransfer()
        {
            var comid = HttpContext.Session.GetString("comid");
            //var data = _context.TransferVariable.Where(x => x.ComId == comid).ToList();
            return View();
        }

        public IActionResult GetTransferDataHistory(string fromDate, string toDate)
        {
            var comid = HttpContext.Session.GetString("comid");


            DateTime fromDateValue = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime toDateValue = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var data = _context.TransferVariable
                       .Where(x => x.ComId == comid && x.EffectiveDate >= fromDateValue && x.EffectiveDate <= toDateValue)
                       .ToList();
            return Json(data);
        }

        public IActionResult VariableTransferJson()
        {
            var comid = HttpContext.Session.GetString("comid");
            var data = _context.TransferVariable.Where(x => x.ComId == comid).ToList();

            // Return JSON result
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> VariableTransfer(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/VariableTransfer/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/VariableTransfer/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetVariableData(file.FileName);
                    if (addition.Count() > 0)
                    {
                        foreach (var newItem in addition)
                        {

                            var existingItem = _context.TransferVariable
                                            .FirstOrDefault(e =>
                                                e.EmpCode == newItem.EmpCode &&
                                                e.ComId == newItem.ComId &&
                                                e.EffectiveDate == newItem.EffectiveDate);
                            if (existingItem != null)
                            {

                                UpdateExistingItem(existingItem, newItem);
                            }
                            else
                            {

                                _context.TransferVariable.Add(newItem);
                            }
                            // await _context.TransferVariable.AddRangeAsync(addition);

                        }

                        await _context.SaveChangesAsync();
                        DateTime today = DateTime.Now.Date;

                        var Query = $"ProcTransferVariableExcel '{comid}','{today}'";
                        SqlParameter[] sqlParameter = new SqlParameter[2];
                        sqlParameter[0] = new SqlParameter("@ComId", comid);
                        sqlParameter[1] = new SqlParameter("@EffectiveDate", today);
                        Helper.ExecProc("ProcTransferVariableExcel", sqlParameter);


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
            return RedirectToAction("VariableTransfer");
        }

        private void UpdateExistingItem(TransferVariable existingItem, TransferVariable newDataItem)
        {

            //existingItem.EffectiveDate = newDataItem.EffectiveDate;
            //existingItem.Name = newDataItem.Name;
            //existingItem.VandorName = newDataItem.VandorName;
            existingItem.PresentDepartment = newDataItem.PresentDepartment;
            existingItem.ProposedDepartment = newDataItem.ProposedDepartment;
            existingItem.PresentDesignation = newDataItem.PresentDesignation;
            existingItem.ProposedDesignation = newDataItem.ProposedDesignation;
            existingItem.PresentRole = newDataItem.PresentRole;
            existingItem.ProposedRole = newDataItem.ProposedRole;
            existingItem.PresentCostHead = newDataItem.PresentCostHead;
            existingItem.ProposedCostHead = newDataItem.ProposedCostHead;
            existingItem.PresentAltitudeCode = newDataItem.PresentAltitudeCode;
            existingItem.ProposedAltitudeCode = newDataItem.ProposedAltitudeCode;
            existingItem.PresentWorkerClassification = newDataItem.PresentWorkerClassification;
            existingItem.ProposedClassification = newDataItem.ProposedClassification;
            existingItem.UserId = newDataItem.UserId;
            existingItem.UpdateByUserId = newDataItem.UpdateByUserId;
            //existingItem.SINo = newDataItem.SINo;
            existingItem.IsDelete = newDataItem.IsDelete;
            existingItem.DateUpdated = DateTime.Now;


            _context.Entry(existingItem).State = EntityState.Modified;
        }


        private List<TransferVariable> GetVariableData(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/VariableTransfer/" + comid + "/" + userid + "/" + fName);

            List<TransferVariable> transferVariables = new List<TransferVariable>();

            try
            {
                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader != null && reader.Read())
                        {

                            if ((reader.GetValue(5) == null &&
                                reader.GetValue(7) == null &&
                                reader.GetValue(9) == null &&
                                reader.GetValue(11) == null &&
                                reader.GetValue(13) == null &&
                                reader.GetValue(15) == null) || startSl == 0)
                            {
                                startSl++;
                            }
                            else
                            {
                                TransferVariable transferVariable = new TransferVariable()
                                {
                                    EffectiveDate = ParseDateTime(reader.GetValue(0)) ?? DateTime.Now,
                                    EmpCode = reader.GetValue(1)?.ToString(),
                                    Name = reader.GetValue(2)?.ToString(),
                                    VandorName = reader.GetValue(3)?.ToString(),
                                    PresentDepartment = reader.GetValue(4)?.ToString(),
                                    ProposedDepartment = reader.GetValue(5)?.ToString(),
                                    PresentDesignation = reader.GetValue(6)?.ToString(),
                                    ProposedDesignation = reader.GetValue(7)?.ToString(),
                                    PresentRole = reader.GetValue(8)?.ToString(),
                                    ProposedRole = reader.GetValue(9)?.ToString(),
                                    PresentCostHead = reader.GetValue(10)?.ToString(),
                                    ProposedCostHead = reader.GetValue(11)?.ToString(),
                                    PresentAltitudeCode = reader.GetValue(12)?.ToString(),
                                    ProposedAltitudeCode = reader.GetValue(13)?.ToString(),
                                    PresentWorkerClassification = reader.GetValue(14)?.ToString(),
                                    ProposedClassification = reader.GetValue(15)?.ToString(),
                                    UserId = "DefaultUserId",
                                    ComId = HttpContext.Session.GetString("comid"),
                                    UpdateByUserId = HttpContext.Session.GetString("userid") ?? "DefaultUpdateByUserId",
                                    SINo = startSl.ToString(),
                                    IsDelete = false,
                                    DateAdded = DateTime.Now,
                                    DateUpdated = DateTime.Now
                                };


                                transferVariables.Add(transferVariable);
                                startSl++;
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

        private DateTime? ParseDateTime(object value)
        {
            if (value != null && DateTime.TryParse(value.ToString(), out DateTime result))
            {
                return result;
            }
            return null;
        }


        //private List<TransferVariable> GetVariableData(string fName)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    var filename = Path.GetFullPath("wwwroot/Content/VariableTransfer/" + comid + "/" + userid + "/" + fName);

        //    List<TransferVariable> transferVariables = new List<TransferVariable>();

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
        //                        transferVariables.Add(new TransferVariable()
        //                        {
        //                            EffectiveDate = DateTime.Parse(reader.GetValue(0).ToString()),
        //                            EmpCode = reader.GetValue(1).ToString(),
        //                            Name = reader.GetValue(2).ToString(),
        //                            VandorName = reader.GetValue(3).ToString(),
        //                            PresentDepartment = reader.GetValue(4).ToString(),
        //                            ProposedDepartment = reader.GetValue(5).ToString(),
        //                            PresentDesignation = reader.GetValue(6).ToString(),
        //                            ProposedDesignation = reader.GetValue(7).ToString(),
        //                            PresentRole = reader.GetValue(8).ToString(),
        //                            ProposedRole = reader.GetValue(9).ToString(),
        //                            PresentCostHead = reader.GetValue(10).ToString(),
        //                            ProposedCostHead = reader.GetValue(11).ToString(),
        //                            PresentAltitudeCode = reader.GetValue(12).ToString(),
        //                            ProposedAltitudeCode = reader.GetValue(13).ToString(),
        //                            PresentWorkerClassification = reader.GetValue(14).ToString(),
        //                            ProposedClassification = reader.GetValue(15).ToString(),
        //                            UserId = HttpContext.Session.GetString("userid"), 
        //                            ComId = HttpContext.Session.GetString("comid"), 
        //                            UpdateByUserId = HttpContext.Session.GetString("userid"), 
        //                            SINo = startSl.ToString(),
        //                            IsDelete = false, 
        //                            DateAdded = DateTime.Now, 
        //                            DateUpdated = DateTime.Now
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

        //    return transferVariables;
        //}


        public async Task<IActionResult> DownloadExcel()
        {
            var comId = HttpContext.Session.GetString("comid");

            //    var result = await _context.Set<EmployeeVariableDataExcel>()
            //.FromSqlRaw("EXEC variabletransferexceldata @ComId", new SqlParameter("@ComId", comId))
            //.ToListAsync();
            var quary = $"EXEC variabletransferexceldata '{comId}'";

            SqlParameter[] sqlParameter = new SqlParameter[1];

            sqlParameter[0] = new SqlParameter("@ComId", comId);


            var result = Helper.ExecProcMapTList<EmployeeVariableDataExcel>("variabletransferexceldata", sqlParameter);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");


                var headers = typeof(EmployeeVariableDataExcel).GetProperties();
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i].Name;
                }

                for (int i = 0; i < result.Count; i++)
                {
                    var rowData = headers.Select(h => h.GetValue(result[i])).ToArray();
                    for (int j = 0; j < rowData.Length; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = rowData[j];
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeData.xlsx");
                }
            }
            return RedirectToAction("VariableTransfer");
        }


        #endregion
    }


}
