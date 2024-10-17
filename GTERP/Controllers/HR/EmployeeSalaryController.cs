using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HR
{
    [OverridableAuthorize]
    public class EmployeeSalaryController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext _context;

        public EmployeeSalaryController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            _context = context;
        }

        // GET: HR_Emp_Salary
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {

                var empsalaryall = await _context.HR_Emp_Salary
                .Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Department)
                .Include(x => x.HR_Emp_Info.Cat_Designation)
                .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                .Include(x => x.HR_Emp_Info.Cat_Floor)
                .Include(x => x.HR_Emp_Info.Cat_Line)
                .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                .ThenInclude(x => x.Cat_PayMode) 
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true).ToListAsync();

                return View(empsalaryall);


            }

            else if (approvetype.Contains(1186) && approvetype.Contains(1187))
            {

                var empsalaryall = _context.HR_Emp_Salary
                            .Include(x => x.HR_Emp_Info)
                            .Include(x => x.HR_Emp_Info.Cat_Department)
                            .Include(x => x.HR_Emp_Info.Cat_Designation)
                            .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                            .Include(x => x.HR_Emp_Info.Cat_Floor)
                            .Include(x => x.HR_Emp_Info.Cat_Line)
                            .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                            .ThenInclude(x => x.Cat_PayMode)
                            //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.Cat_PayMode.PayModeName)
                            //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                            .Where(x => x.ComId == comid &&
                             !x.IsDelete &&
                             !x.HR_Emp_Info.IsInactive &&
                             (x.HR_Emp_Info.EmpTypeId == 1 || x.HR_Emp_Info.EmpTypeId == 2));

                return View(await empsalaryall.ToListAsync());


            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1257))
            {

                var empsalaryall = _context.HR_Emp_Salary
                            .Include(x => x.HR_Emp_Info)
                            .Include(x => x.HR_Emp_Info.Cat_Department)
                            .Include(x => x.HR_Emp_Info.Cat_Designation)
                            .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                            .Include(x => x.HR_Emp_Info.Cat_Floor)
                            .Include(x => x.HR_Emp_Info.Cat_Line)
                            .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                            .ThenInclude(x => x.Cat_PayMode)
                            //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.Cat_PayMode.PayModeName)
                            //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                            .Where(x => x.ComId == comid &&
                             !x.IsDelete &&
                             !x.HR_Emp_Info.IsInactive &&
                             (x.HR_Emp_Info.EmpTypeId == 1 || x.HR_Emp_Info.EmpTypeId == 3));

                return View(await empsalaryall.ToListAsync());


            }
            else if (approvetype.Contains(1187) && approvetype.Contains(1257))
            {

                var empsalaryall = _context.HR_Emp_Salary
                            .Include(x => x.HR_Emp_Info)
                            .Include(x => x.HR_Emp_Info.Cat_Department)
                            .Include(x => x.HR_Emp_Info.Cat_Designation)
                            .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                            .Include(x => x.HR_Emp_Info.Cat_Floor)
                            .Include(x => x.HR_Emp_Info.Cat_Line)
                            .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                            .ThenInclude(x => x.Cat_PayMode)
                            //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.Cat_PayMode.PayModeName)
                            //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                            .Where(x => x.ComId == comid &&
                             !x.IsDelete &&
                             !x.HR_Emp_Info.IsInactive &&
                             (x.HR_Emp_Info.EmpTypeId == 2 || x.HR_Emp_Info.EmpTypeId == 3));

                return View(await empsalaryall.ToListAsync());


            }

            else if (approvetype.Contains(1186))
            {
                var empsalaryByAT = await _context.HR_Emp_Salary
                            .Include(x => x.HR_Emp_Info)
                            .Include(x => x.HR_Emp_Info.Cat_Department)
                            .Include(x => x.HR_Emp_Info.Cat_Designation)
                            .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                            .Include(x => x.HR_Emp_Info.Cat_Floor)
                            .Include(x => x.HR_Emp_Info.Cat_Line)
                            .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                            .ThenInclude(x => x.Cat_PayMode)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.Cat_PayMode.PayModeName)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId == 1).ToListAsync();
                return View(empsalaryByAT);
            }
            else if (approvetype.Contains(1187))
            {
                var empsalaryByAT = await _context.HR_Emp_Salary
                             .Include(x => x.HR_Emp_Info)
                            .Include(x => x.HR_Emp_Info.Cat_Department)
                            .Include(x => x.HR_Emp_Info.Cat_Designation)
                            .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                            .Include(x => x.HR_Emp_Info.Cat_Floor)
                            .Include(x => x.HR_Emp_Info.Cat_Line)
                            .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                            .ThenInclude(x => x.Cat_PayMode)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.Cat_PayMode.PayModeName)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId == 2).ToListAsync();
                return View(empsalaryByAT);
            }
            if (approvetype.Contains(1257))
            {
                var empsalaryByAT = await _context.HR_Emp_Salary
                            .Include(x => x.HR_Emp_Info)
                            .Include(x => x.HR_Emp_Info.Cat_Department)
                            .Include(x => x.HR_Emp_Info.Cat_Designation)
                            .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                            .Include(x => x.HR_Emp_Info.Cat_Floor)
                            .Include(x => x.HR_Emp_Info.Cat_Line)
                            .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                            .ThenInclude(x => x.Cat_PayMode)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.Cat_PayMode.PayModeName)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true && x.HR_Emp_Info.EmpTypeId == 3).ToListAsync();
                return View(empsalaryByAT);
            }

            else
            {
                var empsalaryByAT = await _context.HR_Emp_Salary
                            .Include(x => x.HR_Emp_Info)
                            .Include(x => x.HR_Emp_Info.Cat_Department)
                            .Include(x => x.HR_Emp_Info.Cat_Designation)
                            .Include(x => x.HR_Emp_Info.Cat_Emp_Type)
                            .Include(x => x.HR_Emp_Info.Cat_Floor)
                            .Include(x => x.HR_Emp_Info.Cat_Line)
                            .Include(x => x.HR_Emp_Info.HR_Emp_BankInfo)
                             .ThenInclude(x => x.Cat_PayMode)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.Cat_PayMode.PayModeName)
                //.Include(x => x.HR_Emp_Info.HR_Emp_BankInfo.AccountNumber)
                .Where(x => x.ComId == comid && !x.IsDelete && !x.HR_Emp_Info.IsInactive == true).ToListAsync();
                return View(empsalaryByAT);
            }
        }

        #region commented code

        //public class SalaryInfo
        //{
        //    public int SalaryId { get; set; }
        //    public string EmpCode { get; set; }
        //    public string EmpName { get; set; }
        //    public string DesigName { get; set; }
        //    public string EmpTypeName { get; set; }
        //    public float? GrossSalary { get; set; }
        //    public float? BasicSalary { get; set; }
        //    public float? HouseRent { get; set; }
        //    public float? MA { get; set; }
        //    public float? Trn { get; set; }
        //    public float? FA { get; set; }
        //    public float? OtherAllow { get; set; }
        //    public float? CasualSalary { get; set; }
        //}

        //[HttpPost]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        var comid = (HttpContext.Session.GetString("comid"));

        //        Microsoft.Extensions.Primitives.StringValues y = "";

        //        var x = Request.Form.TryGetValue("search[value]", out y);

        //        //if (y.ToString().Length > 0)
        //        //{

        //        var query = from e in _context.HR_Emp_Salary
        //                //.Include(h => h.Cat_BuildingTypeHC).Include(h => h.Cat_Location1)
        //                //.Include(h => h.Cat_LocationHB).Include(h => h.HR_Emp_Info.Cat_Designation)
        //                //.Include(h => h.HR_Emp_Info).Include(h => h.HR_Emp_Info.Cat_Emp_Type)
        //                .Where(x => x.ComId == comid && x.HR_Emp_Info.IsInactive == false && x.HR_Emp_Info.IsCasual == false && !x.IsDelete).OrderByDescending(x => x.SalaryId)
        //                    select new SalaryInfo
        //                    {
        //                        SalaryId = e.SalaryId,
        //                        EmpCode = e.HR_Emp_Info.EmpCode,
        //                        EmpName = e.HR_Emp_Info.EmpName,
        //                        DesigName = e.HR_Emp_Info.Cat_Designation.DesigName,
        //                        EmpTypeName = e.HR_Emp_Info.Cat_Emp_Type.EmpTypeName,
        //                        GrossSalary = (float)e.PersonalPay,
        //                        BasicSalary = e.BasicSalary,
        //                        HouseRent = e.HouseRent,
        //                        MA = (float)e.MadicalAllow,
        //                        Trn = e.ConveyanceAllow,
        //                        FA = (float)e.CanteenAllow,
        //                        OtherAllow = e.MiscAdd,
        //                        CasualSalary = (float)e.DearnessAllow,

        //                    };




        //        var parser = new Parser<SalaryInfo>(Request.Form, query);

        //        var data = Json(parser.Parse());

        //        return data;

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = "0", error = ex.Message });
        //        //throw ex;
        //    }

        //}

        #endregion


        public void empDropdownFill()
        {
            var comid = HttpContext.Session.GetString("comid");

            var DollerData = _context.Companys.Where(x => x.CompanyCode == comid && x.IsDoller == true).FirstOrDefault();



            if (DollerData == null)
            {
                ViewBag.IsDoller = 0;
            }
            else
            {
                ViewBag.IsDoller = 1;
            }
            //string comid = HttpContext.Session.GetString("comid");

            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();
                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");


            }
            else if (approvetype.Contains(1186) && approvetype.Contains(1187))
            {
                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false && (emp.EmpTypeId == 1 || emp.EmpTypeId == 2)
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();
                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");


            }
            else if (approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false && (emp.EmpTypeId == 2 || emp.EmpTypeId == 3)
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();
                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");


            }

            else if (approvetype.Contains(1186) && approvetype.Contains(1257))
            {
                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false && (emp.EmpTypeId == 1 || emp.EmpTypeId == 3)
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();
                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");


            }
            else if (approvetype.Contains(1187))
            {

                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false && emp.EmpTypeId == 2
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();
                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            }
            else if (approvetype.Contains(1186))
            {

                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false && emp.EmpTypeId == 1
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();
                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            }
            else if (approvetype.Contains(1257))
            {

                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false && emp.EmpTypeId == 3
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();
                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            }
            else
            {
                var empInfo = (from emp in _context.HR_Emp_Info
                               join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                               join s in _context.Cat_Section on emp.SectId equals s.SectId
                               join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                               join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                               join Line in _context.Cat_Line on emp.LineId equals Line.LineId
                               where emp.ComId == comid && emp.IsDelete == false
                               orderby emp.EmpCode
                               select new
                               {
                                   Value = emp.EmpId,
                                   Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                               }).ToList();

                ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            }



        }



        // GET: HR_Emp_Salary/Create
        public IActionResult Create()
        {
            string comid = HttpContext.Session.GetString("comid");

            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

            ViewBag.Title = "Create";
            ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName");
            ViewData["LId1"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");
            ViewData["LId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
            ViewData["LId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
            ViewData["PFLLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");
            ViewData["PFLLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
            ViewData["PFLLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
            ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
            ViewData["HBLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");
            ViewData["HBLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
            ViewData["HBLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
            ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
            ViewData["PFLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");

            ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");

            var DollerData = _context.Companys.Where(x => x.CompanyCode == comid && x.IsDoller == true).FirstOrDefault();



            if (DollerData == null)
            {
                ViewBag.IsDoller = 0;
            }
            else
            {
                ViewBag.IsDoller = 1;
            }


            var data = _context.HR_OverTimeSetting.Where(x => x.CompanyId == comid).FirstOrDefault();
            if (data != null)
            {
                ViewBag.IsManualOT = data.IsManualOT;
                ViewBag.OTRate = data.OTRate;
            }
            else
            {
                ViewBag.OTRate = 0;
                ViewBag.IsManualOT = 0;
            }

            empDropdownFill();

            return View();
        }

        // POST: HR_Emp_Salary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Bind("SalaryId,ComId,EmpId,LId1,LId2,LId3,BId,PFLId,WelfareLId,MCLId,HBLId,BasicSalary,IsBS,HouseRent,IsHr,MadicalAllow,IsMa,ConveyanceAllow,IsConvAllow,DearnessAllow,IsDearAllow,GasAllow,IsGasAllow,PersonalPay,IsPersonalPay,ArrearBasic,IsArrearBasic,ArrearBonus,IsArrearBonus,WashingAllow,IsWashingAllow,SiftAllow,ChargeAllow,IsChargAllow,MiscAdd,IsMiscAdd,ContainSub,IsContainSub,ComPfCount,IsComPfcount,EduAllow,IsEduAllow,TiffinAllow,IsTiffinAllow,CanteenAllow,IsCanteenAllow,AttAllow,IsAttAllow,FestivalBonus,IsFestivalBonus,RiskAllow,IsRiskAllow,NightAllow,IsNightAllow,MobileAllow,IsMobileAllow,Pf,IsPf,PfAdd,IsPfAdd,HrExp,IsHrexp,FesBonusDed,IsFesBonus,Transportcharge,IsTrncharge,TeliphoneCharge,IsTelcharge,TAExpense,IsTAExp,SalaryAdv,IsSalaryAdv,PurchaseAdv,McloanDed,IsMcloan,HbloanDed,IsHbloan,PfloannDed,IsPfloann,WfloanLocal,IsWfloanLocal,WfloanOther,IsWfloanOther,WpfloanDed,IsWpfloanDed,MaterialLoanDed,IsMaterialLoan,MiscDed,IsMiscDed,AdvAgainstExp,IsAdvAgainstExp,AdvFacility,IsAdvFacility,ElectricCharge,IsElectricCharge,Gascharge,IsGascharge,HazScheme,IsHazScheme,Donaton,IsDonaton,Dishantenna,IsDishantenna,RevenueStamp,IsRevenueStamp,OwaSub,IsOwaSub,DedIncBns,IsDedIncBns,DapEmpClub,IsDapEmpClub,Moktab,IsMoktab,ChemicalForum,IsChemicalForum,DiplomaassoDed,IsDiplomaassoDed,EnggassoDed,IsEnggassoDed,Wfsub,IsWfsub,EduAlloDed,IsEduAlloDed,PurChange,IsPurChange,IncomeTax,IsIncomeTax,ArrearInTaxDed,IsArrearInTaxDed,OffWlfareAsso,IsOffWlfareAsso,OfficeclubDed,IsOfficeclubDed,IncBonusDed,IsIncBonusDed,Watercharge,IsWatercharge,ChemicalAsso,IsChemicalAsso,AdvInTaxDed,IsAdvInTaxDed,ConvAllowDed,IsConvAllowDed,DedIncBonusOf,IsDedIncBonusOf,UnionSubDed,IsUnionSubDed,EmpClubDed,IsEmpClubDed,MedicalExp,IsMedicalExp,WagesaAdv,IsWagesaAdv,MedicalLoanDed,IsMedicalLoanDed,AdvWagesDed,IsAdvWagesDed,WFL,IsWFL,CheForum,IsCheForum")]
        public async Task<IActionResult> Create(HR_Emp_Salary hR_Emp_Salary)
        {
            string comid = HttpContext.Session.GetString("comid");
            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1173 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            if (ModelState.IsValid)
            {
                hR_Emp_Salary.UserId = HttpContext.Session.GetString("userid");
                hR_Emp_Salary.ComId = HttpContext.Session.GetString("comid");
                if (hR_Emp_Salary.SalaryId > 0)
                {
                    hR_Emp_Salary.UpdateByUserId = HttpContext.Session.GetString("userid");
                    hR_Emp_Salary.DateUpdated = DateTime.Now;
                    _context.Entry(hR_Emp_Salary).State = EntityState.Modified;

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hR_Emp_Salary.EmpId.ToString(), "Update", hR_Emp_Salary.EmpId.ToString());
                }
                else
                {
                    if (approveData == null)
                    {
                        hR_Emp_Salary.IsApprove = true;
                    }

                    else if (approveData.IsApprove == true)
                    {
                        hR_Emp_Salary.IsApprove = false;
                    }
                    else
                    {
                        hR_Emp_Salary.IsApprove = true;
                    }
                    hR_Emp_Salary.DateAdded = DateTime.Now;
                    _context.Add(hR_Emp_Salary);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hR_Emp_Salary.EmpId.ToString(), "Create", hR_Emp_Salary.EmpId.ToString());

                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName", hR_Emp_Salary.BId);
            ViewData["LId1"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.LId1);
            ViewData["LId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId2);
            ViewData["LId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId3);
            ViewData["PFLLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.GLId);
            ViewData["HBLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.MCLId);
            ViewData["PFLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.PFLId);
            ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.WelfareLId);
            if (hR_Emp_Salary == null || hR_Emp_Salary.Donation == null)
            {
                var data = _context.HR_OverTimeSetting.Where(x => x.CompanyId == comid).FirstOrDefault();
                if (data != null)
                {
                    ViewBag.IsManualOT = data.IsManualOT;
                    ViewBag.OTRate = data.OTRate;
                }
                else
                {
                    ViewBag.OTRate = 0;
                    ViewBag.IsManualOT = 0;
                }
            }
            else
            {

                ViewBag.IsManualOT = 1;
                ViewBag.OTRate = hR_Emp_Salary.Donation;
            }

            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpName", hR_Emp_Salary.EmpId);

            empDropdownFill();

            //ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", hR_Emp_Salary.EmpId);

            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info
            //  .Where(s => s.ComId == comid)
            //  .Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ]", Value = s.EmpId })
            //  .ToList(), "Value", "Text", hR_Emp_Salary.EmpId);

            return View(hR_Emp_Salary);
        }

        // GET: HR_Emp_Salary/Edit/5


        [HttpPost]
        public IActionResult AutoSalaryCalculation(int EmpId, int LId1, int BId, Double BS)
        {
            int? empTypeId = _context.HR_Emp_Info.Find(EmpId).EmpTypeId;
            Cat_HRSetting hr = null;
            Cat_HRExpSetting hrExp = null;

            if (empTypeId != null)
            {
                hr = _context.Cat_HRSetting
                    .Where(h => h.EmpTypeId == empTypeId && h.LId == LId1 && h.BS <= BS && h.BId == BId)
                    .OrderByDescending(h => h.BS).FirstOrDefault();                    //.ToList();
                hrExp = _context.Cat_HRExpSetting
                   .Where(h => h.EmpTypeId == empTypeId && h.LId == LId1 && h.BId == BId && h.BS <= BS)
                   .OrderByDescending(h => h.BS).FirstOrDefault();

            }

            var gasCharge = _context.Cat_GasChargeSetting
             .Where(h => h.LId == LId1 && h.BId == BId).FirstOrDefault();

            var electricCharge = _context.Cat_ElectricChargeSetting
             .Where(h => h.LId == LId1 && h.BId == BId).FirstOrDefault();

            return Json(new { HR = hr, HRExp = hrExp, GasCharge = gasCharge, ElectricCharge = electricCharge });

        }

        [HttpGet]
        public IActionResult SalaryCalculation(int EmpId, Double GS)
        {
            int? empTypeId = _context.HR_Emp_Info.Find(EmpId).EmpTypeId;
            var comid = HttpContext.Session.GetString("comid");
            var hr_allowance = _context.Cat_HRSetting.Include(x => x.Cat_Location)
                .Include(x => x.Cat_BuildingType)
                .Include(x => x.Cat_Emp_Type)
                .Where(x => x.CompanyCode == comid && x.Cat_Emp_Type.EmpTypeId == empTypeId).FirstOrDefault();
            double bs = 0, hr = 0, fa = 0, ma = 0, trn = 0;



            var DollerData = _context.Companys.Where(x => x.CompanyCode == comid && x.IsDoller == true).FirstOrDefault();



            if (DollerData == null)
            {
                ViewBag.IsDoller = 0;
            }
            else
            {
                ViewBag.IsDoller = 1;
            }

            if (empTypeId != null)
            {//// for worker


                if (empTypeId == 1)
                {
                    if (empTypeId == 1 && hr_allowance == null)
                    {
                        bs = (GS - 2450) / 1.5;
                        hr = bs * .50;
                        ma = 750;
                        trn = 450;
                        fa = 1250;
                    }
                    else if (hr_allowance.EmpTypeId == 1 && hr_allowance.CompanyCode == comid && hr_allowance.IsCADifference == true && hr_allowance.IsFADifference == true)
                    {
                        ma = hr_allowance.MA;
                        fa = hr_allowance.FA;
                        trn = hr_allowance.CA;
                        bs = (GS - (ma)) / 1.5;
                        hr = (bs * hr_allowance.HR);

                    }
                    else if (hr_allowance.EmpTypeId == 1 && hr_allowance.CompanyCode == comid
                        && hr_allowance.IsCADifference == false && hr_allowance.IsFADifference == false)
                    {
                        ma = hr_allowance.MA;
                        fa = hr_allowance.FA;
                        trn = hr_allowance.CA;
                        bs = (GS - (ma + fa + trn)) / 1.5;
                        hr = (bs * hr_allowance.HR);
                    }

                }
                //Staff
                if (empTypeId != 1)
                {
                    if (empTypeId != 1 && hr_allowance == null)
                    {
                        bs = GS * .60;
                        hr = GS * .30;
                        ma = GS * .07;
                        trn = GS * .03;
                        fa = 0;
                    }
                    else if (hr_allowance.EmpTypeId != 1 && hr_allowance.CompanyCode == comid
                        && hr_allowance.IsCADifference == false && hr_allowance.IsFADifference == false && hr_allowance.BS == 0)
                    {
                        ma = hr_allowance.MA;
                        fa = hr_allowance.FA;
                        trn = hr_allowance.CA;
                        bs = (GS - (ma + fa + trn)) / 1.5;
                        hr = (bs * hr_allowance.HR);
                    }

                    else if (hr_allowance.EmpTypeId != 1 && hr_allowance.CompanyCode == comid
                        && hr_allowance.IsCADifference == false && hr_allowance.IsFADifference == true)
                    {
                        bs = GS * hr_allowance.BS;
                        hr = GS * hr_allowance.HR;
                        ma = GS * hr_allowance.MA;
                        trn = GS * hr_allowance.CA;
                        fa = hr_allowance.FA;
                    }

                    else if (hr_allowance.EmpTypeId != 1 && hr_allowance.CompanyCode == comid
                        && hr_allowance.IsCADifference == true && hr_allowance.IsFADifference == false)
                    {
                        bs = GS * hr_allowance.BS;
                        hr = GS * hr_allowance.HR;
                        ma = GS * hr_allowance.MA;
                        trn = hr_allowance.CA;
                        fa = GS * hr_allowance.FA;
                    }

                    else if (hr_allowance.EmpTypeId != 1 && hr_allowance.CompanyCode == comid && hr_allowance.BS > 0)
                    {
                        bs = GS * hr_allowance.BS;
                        hr = GS * hr_allowance.HR;
                        ma = GS * hr_allowance.MA;
                        trn = GS * hr_allowance.CA;
                        fa = GS * hr_allowance.FA;
                    }
                }



                //else
                //{
                //    bs = (GS - 2450) / 1.5;
                //    hr = bs * .50;
                //    ma = 1250;
                //    trn = 450;
                //    fa = 750;
                //}
            }


            return Json(new { bs = bs, hr = hr, ma = ma, trn = trn, fa = fa });

        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string comid = HttpContext.Session.GetString("comid");
            //var hR_Emp_Salary = await _context.HR_Emp_Salary.FindAsync(id);
            var hR_Emp_Salary = await _context.HR_Emp_Salary
                .Include(h => h.Cat_BuildingTypeHC)
                .Include(h => h.Cat_Location1)
                .Include(h => h.Cat_Location2)
                .Include(h => h.Cat_Location3)
                .Include(h => h.Cat_PFLoanlocation)
                .Include(h => h.Cat_GratuityLocation)
                .Include(h => h.Cat_LocationHB)
                .Include(h => h.Cat_LocationMC)
                .Include(h => h.Cat_LocationPF)
                .Include(h => h.Cat_LocationWelfare)
                .Include(h => h.HR_Emp_Info)
                .FirstOrDefaultAsync(m => m.SalaryId == id);

            var DollerData = _context.Companys.Where(x => x.CompanyCode == comid && x.IsDoller == true).FirstOrDefault();



            if (DollerData == null)
            {
                ViewBag.IsDoller = 0;
            }
            else
            {
                ViewBag.IsDoller = 1;
            }



            if (hR_Emp_Salary == null || hR_Emp_Salary.Donation == null || hR_Emp_Salary.Donation == 0)

            {
                var data = _context.HR_OverTimeSetting.Where(x => x.CompanyId == comid).FirstOrDefault();
                if (data != null)
                {
                    ViewBag.IsManualOT = data.IsManualOT;
                    ViewBag.OTRate = data.OTRate;
                }
                else
                {
                    ViewBag.OTRate = 0;
                    ViewBag.IsManualOT = 0;
                }
            }
            else
            {

                ViewBag.IsManualOT = 1;
                ViewBag.OTRate = hR_Emp_Salary.Donation;
            }
            if (hR_Emp_Salary == null)
            {
                ViewBag.Title = "Create";
                ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName");
                ViewData["LId1"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["LId2"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["LId3"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["PFLLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["HBLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["PFLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpName", id);

                empDropdownFill();

                //ViewData["EmpId"] = new SelectList(empInfos, "Value", "Text", hR_Emp_Salary.EmpId);

                //  ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info
                //.Where(s => s.ComId == comid)
                //.Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ]", Value = s.EmpId })
                //.ToList(), "Value", "Text", hR_Emp_Salary.EmpId);

                return View("Create", hR_Emp_Salary);
            }

            ViewBag.Title = "Edit";
            ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName", hR_Emp_Salary.BId);
            ViewData["LId1"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.LId1);
            ViewData["LId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId2);
            ViewData["LId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId3);
            ViewData["PFLLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.GLId);
            ViewData["HBLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.MCLId);
            ViewData["PFLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.PFLId);
            ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.WelfareLId);
            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpName", hR_Emp_Salary.EmpId);
            empDropdownFill();

            // ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", hR_Emp_Salary.EmpId);

            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info
            //.Where(s => s.ComId == comid)
            //.Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ]", Value = s.EmpId })
            //.ToList(), "Value", "Text", hR_Emp_Salary.EmpId);

            return View("Create", hR_Emp_Salary);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmp(int? id)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }


            var DollerData = _context.Companys.Where(x => x.CompanyCode == comid && x.IsDoller == true).FirstOrDefault();



            if (DollerData == null)
            {
                ViewBag.IsDoller = 0;
            }
            else
            {
                ViewBag.IsDoller = 1;
            }
            //string comid = HttpContext.Session.GetString("comid");
            //var hR_Emp_Salary = await _context.HR_Emp_Salary.FindAsync(id);
            var hR_Emp_Salary = await _context.HR_Emp_Salary
                .Include(h => h.Cat_BuildingTypeHC)
                .Include(h => h.Cat_Location1)
                .Include(h => h.Cat_Location2)
                .Include(h => h.Cat_Location3)
                .Include(h => h.Cat_PFLoanlocation)
                .Include(h => h.Cat_GratuityLocation)
                .Include(h => h.Cat_LocationHB)
                .Include(h => h.Cat_LocationMC)
                .Include(h => h.Cat_LocationPF)
                .Include(h => h.Cat_LocationWelfare)
                .Include(h => h.HR_Emp_Info)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (hR_Emp_Salary == null || hR_Emp_Salary.Donation == null || hR_Emp_Salary.Donation == 0)
            {
                var data = _context.HR_OverTimeSetting.Where(x => x.CompanyId == comid).FirstOrDefault();
                if (data != null)
                {
                    ViewBag.IsManualOT = data.IsManualOT;
                    ViewBag.OTRate = data.OTRate;
                }
                else
                {
                    ViewBag.OTRate = 0;
                    ViewBag.IsManualOT = 0;
                }
            }
            else
            {

                ViewBag.IsManualOT = 1;
                ViewBag.OTRate = hR_Emp_Salary.Donation;
            }
            if (hR_Emp_Salary == null)
            {
                ViewBag.Title = "Create";
                ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName");
                ViewData["LId1"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["LId2"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["LId3"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["PFLLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["HBLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["PFLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName");
                //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpName", id);


                //empDropdownFill();

                //ViewData["EmpId"] = new SelectList(empInfos, "Value", "Text", id);

                ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info
            .Where(s => s.ComId == comid)
            .Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ]", Value = s.EmpId })
            .ToList(), "Value", "Text", id);

                return View("Create", hR_Emp_Salary);
            }

            ViewBag.Title = "Edit";
            ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName", hR_Emp_Salary.BId);
            ViewData["LId1"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.LId1);
            ViewData["LId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId2);
            ViewData["LId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId3);
            ViewData["PFLLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.GLId);
            ViewData["HBLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.MCLId);
            ViewData["PFLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.PFLId);
            ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.WelfareLId);
            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpName", hR_Emp_Salary.EmpId);

            empDropdownFill();

            //ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", hR_Emp_Salary.EmpId);
            hR_Emp_Salary.HouseRent = (float?)Math.Round((decimal)hR_Emp_Salary.HouseRent);

            hR_Emp_Salary.ArrearBonus = hR_Emp_Salary.ArrearBonus ?? 0;
            hR_Emp_Salary.ChargeAllow = hR_Emp_Salary.ChargeAllow ?? 0;
            hR_Emp_Salary.FestivalBonus = hR_Emp_Salary.PersonalPay + hR_Emp_Salary.ArrearBonus + hR_Emp_Salary.ChargeAllow;

            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info
            //.Where(s => s.ComId == comid)
            //.Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ]", Value = s.EmpId })
            //.ToList(), "Value", "Text", hR_Emp_Salary.EmpId);
            return View("Create", hR_Emp_Salary);
        }
        // POST: HR_Emp_Salary/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryId,ComId,EmpId,LId1,LId2,LId3,BId,PFLId,WelfareLId,MCLId,HBLId,BasicSalary,IsBS,HouseRent,IsHr,MadicalAllow,IsMa,ConveyanceAllow,IsConvAllow,DearnessAllow,IsDearAllow,GasAllow,IsGasAllow,PersonalPay,IsPersonalPay,ArrearBasic,IsArrearBasic,ArrearBonus,IsArrearBonus,WashingAllow,IsWashingAllow,SiftAllow,ChargeAllow,IsChargAllow,MiscAdd,IsMiscAdd,ContainSub,IsContainSub,ComPfCount,IsComPfcount,EduAllow,IsEduAllow,TiffinAllow,IsTiffinAllow,CanteenAllow,IsCanteenAllow,AttAllow,IsAttAllow,FestivalBonus,IsFestivalBonus,RiskAllow,IsRiskAllow,NightAllow,IsNightAllow,MobileAllow,IsMobileAllow,Pf,IsPf,PfAdd,IsPfAdd,HrExp,IsHrexp,FesBonusDed,IsFesBonus,Transportcharge,IsTrncharge,TeliphoneCharge,IsTelcharge,TAExpense,IsTAExp,SalaryAdv,IsSalaryAdv,PurchaseAdv,McloanDed,IsMcloan,HbloanDed,IsHbloan,PfloannDed,IsPfloann,WfloanLocal,IsWfloanLocal,WfloanOther,IsWfloanOther,WpfloanDed,IsWpfloanDed,MaterialLoanDed,IsMaterialLoan,MiscDed,IsMiscDed,AdvAgainstExp,IsAdvAgainstExp,AdvFacility,IsAdvFacility,ElectricCharge,IsElectricCharge,Gascharge,IsGascharge,HazScheme,IsHazScheme,Donaton,IsDonaton,Dishantenna,IsDishantenna,RevenueStamp,IsRevenueStamp,OwaSub,IsOwaSub,DedIncBns,IsDedIncBns,DapEmpClub,IsDapEmpClub,Moktab,IsMoktab,ChemicalForum,IsChemicalForum,DiplomaassoDed,IsDiplomaassoDed,EnggassoDed,IsEnggassoDed,Wfsub,IsWfsub,EduAlloDed,IsEduAlloDed,PurChange,IsPurChange,IncomeTax,IsIncomeTax,ArrearInTaxDed,IsArrearInTaxDed,OffWlfareAsso,IsOffWlfareAsso,OfficeclubDed,IsOfficeclubDed,IncBonusDed,IsIncBonusDed,Watercharge,IsWatercharge,ChemicalAsso,IsChemicalAsso,AdvInTaxDed,IsAdvInTaxDed,ConvAllowDed,IsConvAllowDed,DedIncBonusOf,IsDedIncBonusOf,UnionSubDed,IsUnionSubDed,EmpClubDed,IsEmpClubDed,MedicalExp,IsMedicalExp,WagesaAdv,IsWagesaAdv,MedicalLoanDed,IsMedicalLoanDed,AdvWagesDed,IsAdvWagesDed,WFL,IsWFL,CheForum,IsCheForum")] HR_Emp_Salary hR_Emp_Salary)
        {
            string comid = HttpContext.Session.GetString("comid");
            if (id != hR_Emp_Salary.SalaryId)
            {
                return NotFound();
            }
            if (hR_Emp_Salary == null || hR_Emp_Salary.Donation == null || hR_Emp_Salary.Donation == 0)
            {
                var data = _context.HR_OverTimeSetting.Where(x => x.CompanyId == comid).FirstOrDefault();
                if (data != null)
                {
                    ViewBag.IsManualOT = data.IsManualOT;
                    ViewBag.OTRate = data.OTRate;
                }
                else
                {
                    ViewBag.OTRate = 0;
                    ViewBag.IsManualOT = 0;
                }
            }
            else
            {

                ViewBag.IsManualOT = 1;
                ViewBag.OTRate = hR_Emp_Salary.Donation;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hR_Emp_Salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HR_Emp_SalaryExists(hR_Emp_Salary.SalaryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName", hR_Emp_Salary.BId);
            ViewData["LId1"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.LId1);
            ViewData["LId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId2);
            ViewData["LId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId3);
            ViewData["PFLLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.GLId);
            ViewData["HBLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.MCLId);
            ViewData["PFLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.PFLId);
            ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.WelfareLId);
            return View(hR_Emp_Salary);
        }

        // GET: HR_Emp_Salary/Delete/5
        //[Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            string comid = HttpContext.Session.GetString("comid");

            var hR_Emp_Salary = await _context.HR_Emp_Salary
                .Include(h => h.Cat_BuildingTypeHC)
                .Include(h => h.Cat_Location1)
                .Include(h => h.Cat_Location2)
                .Include(h => h.Cat_Location3)
                .Include(h => h.Cat_PFLoanlocation)
                .Include(h => h.Cat_GratuityLocation)
                .Include(h => h.Cat_LocationHB)
                .Include(h => h.Cat_LocationMC)
                .Include(h => h.Cat_LocationPF)
                .Include(h => h.Cat_LocationWelfare)
                .Include(h => h.HR_Emp_Info)
                .FirstOrDefaultAsync(m => m.SalaryId == id);


            if (hR_Emp_Salary == null || hR_Emp_Salary.Donation == null || hR_Emp_Salary.Donation == 0)

            {
                var data = _context.HR_OverTimeSetting.Where(x => x.CompanyId == comid).FirstOrDefault();
                if (data != null)
                {
                    ViewBag.IsManualOT = data.IsManualOT;
                    ViewBag.OTRate = data.OTRate;
                }
                else
                {
                    ViewBag.OTRate = 0;
                    ViewBag.IsManualOT = 0;
                }
            }
            else
            {

                ViewBag.IsManualOT = 1;
                ViewBag.OTRate = hR_Emp_Salary.Donation;
            }
            if (hR_Emp_Salary == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewData["BId"] = new SelectList(_context.Cat_BuildingType, "BId", "BuildingName", hR_Emp_Salary.BId);
            ViewData["LId1"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.LId1);
            ViewData["LId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId2);
            ViewData["LId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.LId3);
            ViewData["PFLLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["PFLLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.PFLLId);
            ViewData["GLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.GLId);
            ViewData["HBLId"] = new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId2"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["HBLId3"] = new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName", hR_Emp_Salary.HBLId);
            ViewData["MCLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.MCLId);
            ViewData["PFLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.PFLId);
            ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.WelfareLId);
            ViewData["WelfareLId"] = new SelectList(_context.Cat_Location, "LId", "LocationName", hR_Emp_Salary.WelfareLId);
            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpName", hR_Emp_Salary.EmpId);
            empDropdownFill();

            //ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", hR_Emp_Salary.EmpId);

            // ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info
            //.Where(s => s.ComId == comid)
            //.Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ]", Value = s.EmpId })
            //.ToList(), "Value", "Text", hR_Emp_Salary.EmpId);
            return View("Create", hR_Emp_Salary);
        }

        // POST: HR_Emp_Salary/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var hR_Emp_Salary = await _context.HR_Emp_Salary.FindAsync(id);
                hR_Emp_Salary.IsDelete = true;
                _context.Entry(hR_Emp_Salary).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), hR_Emp_Salary.EmpId.ToString(), "Delete", hR_Emp_Salary.EmpId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, EmpId = hR_Emp_Salary.SalaryId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        private bool HR_Emp_SalaryExists(int id)
        {
            return _context.HR_Emp_Salary.Any(e => e.SalaryId == id);
        }
    }
}
