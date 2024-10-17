using ExcelDataReader;
using GTERP.BLL;
using GTERP.EF;
using GTERP.Interfaces.Payroll;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ZKTeco;

namespace GTERP.Controllers.Payroll
{
    [OverridableAuthorize]
    public class PayrollController : Controller
    {
        

        private readonly TransactionLogRepository tranlog;
        private readonly ILogger<PayrollController> _logger;
        private readonly GTRDBContext _context;
        private readonly IEmpWiseSalaryLedgerRepository _empWiseSalaryLedgerRepository;
        private readonly IPFContributionRepository _pFContributionRepository;
        private readonly ISalaryCheckRepository _salaryCheckRepository;
        private readonly ISalarySettlementRepository _salarySettlementRepository;
        private readonly IWFLedgerRepository _wFLedgerRepository;
        private readonly IPFLedgerRepository _pFLedgerRepository;
        private readonly IGratuityLedgerRepository _gratuityLedgerRepository;
        private readonly IWebHostEnvironment environment;
        private readonly ISalaryAdditionRepository _salaryAdditionRepository;
        private readonly ISalaryDeductionRepository _salaryDeductionRepository;
        private readonly IExcelUploadRepository _excelUploadRepository;
        private readonly IMLExcelUploadRepository _mLExcelUploadRepository;
        private readonly IPELExcelFileUploadRepository _pELExcelFileUploadRepository;
        private readonly IPFEmpOpBalRepository _pFEmpOpBalRepository;
        private readonly IPFWithdrawnRepository _pFWithdrawnRepository;
        public PayrollController(
           TransactionLogRepository tran,
           GTRDBContext context,
           IEmpWiseSalaryLedgerRepository empWiseSalaryLedgerRepository,
           IPFContributionRepository pFContributionRepository,
           ISalaryCheckRepository salaryCheckRepository,
           ISalarySettlementRepository salarySettlementRepository,
           IWFLedgerRepository wFLedgerRepository,
           IPFLedgerRepository pFLedgerRepository,
           IGratuityLedgerRepository gratuityLedgerRepository,
           IWebHostEnvironment env,
           ISalaryAdditionRepository salaryAdditionRepository,
           ILogger<PayrollController> loggers,
           ISalaryDeductionRepository salaryDeductionRepository,
           IExcelUploadRepository excelUploadRepository,
           IMLExcelUploadRepository mLExcelUploadRepository,
           IPELExcelFileUploadRepository pELExcelFileUploadRepository,
           IPFEmpOpBalRepository pFEmpOpBalRepository,
           IPFWithdrawnRepository pFWithdrawnRepository
            )
        {
            tranlog = tran;
            _context = context;
            _logger = loggers;
            environment = env;
            _empWiseSalaryLedgerRepository = empWiseSalaryLedgerRepository;
            _pFContributionRepository = pFContributionRepository;
            _salaryCheckRepository = salaryCheckRepository;
            _salarySettlementRepository = salarySettlementRepository;
            _wFLedgerRepository = wFLedgerRepository;
            _pFLedgerRepository = pFLedgerRepository;
            _gratuityLedgerRepository = gratuityLedgerRepository;
            _salaryAdditionRepository = salaryAdditionRepository;
            _salaryDeductionRepository = salaryDeductionRepository;
            _excelUploadRepository = excelUploadRepository;
            _mLExcelUploadRepository = mLExcelUploadRepository;
            _pELExcelFileUploadRepository = pELExcelFileUploadRepository;
            _pFEmpOpBalRepository = pFEmpOpBalRepository;
            _pFWithdrawnRepository = pFWithdrawnRepository;

        }

        #region EmpWiseSalaryLedger
        public IActionResult EmpWiseSalaryLedgerList()
        {
            List<string> myList = new List<string> { "This Month", "This Week", "Prev Month", "Prev Quarter", "Prev 6 Month", "This Year", "Prev Year", "Custom" };
            IEnumerable<SelectListItem> mySelectList = myList.Select(s => new SelectListItem { Value = s, Text = s });
            List<SelectListItem> mySelectListAsList = mySelectList.ToList();
            ViewBag.period = mySelectListAsList;
            return View();
        }
        [AllowAnonymous]
        public JsonResult GETEmpWiseSalaryLedgerList(int? EmpId, DateTime dtFrom, DateTime dtTo, string searchquery, int pageIndex, int pageSize)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            var pageinfo = new PagingInfo();

            JObject jsonObject = JObject.Parse(searchquery);

            string SearchColumns = "";
            string SearchKeywords = "";

            foreach (JProperty property in jsonObject.Properties())
            {
                string columnName = property.Name;
                string value = property.Value.ToString();

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

            // Geting List Of Emplyee

            var EmpWiseSalaryLedger = _empWiseSalaryLedgerRepository.EmpWiseSalaryLedgerList(EmpId, 0, pageIndex, pageSize, SearchColumns, SearchKeywords, dtFrom, dtTo);
            var PageCount = _empWiseSalaryLedgerRepository.EmpWiseSalaryLedgerCount(EmpId, 1, pageIndex, pageSize, SearchColumns, SearchKeywords, dtFrom, dtTo);
            decimal TotalRecordCount = 0;
            if (PageCount != null)
            {
                TotalRecordCount = PageCount[0].Results;
            }
            if (EmpWiseSalaryLedger is null)
            {
                EmpWiseSalaryLedger = new List<EmpWiseSalaryLedger>();
            }
            var PageCountabc = decimal.Parse((TotalRecordCount / pageSize).ToString());
            var PageCounts = Math.Ceiling(PageCountabc);

            pageinfo.PageCount = int.Parse(PageCounts.ToString());
            pageinfo.PageNo = pageIndex;
            pageinfo.PageSize = int.Parse(pageSize.ToString());
            pageinfo.TotalRecordCount = int.Parse(TotalRecordCount.ToString());

            // Return JSON result instead of view
            return Json(new { Success = 1, error = false, EmployeeList = EmpWiseSalaryLedger, PageInfo = pageinfo });
        }

        public JsonResult SearchEmployees(string term)
        {
            string comid = HttpContext.Session.GetString("comid");
            var employees = _context.HR_Emp_Info
                .Where(e => e.EmpName.Contains(term) || e.EmpCode.Contains(term) && e.ComId == comid)
                .Select(e => new { label = e.EmpCode + " " + e.EmpName, value = e.EmpId })
                .Take(10)
                .ToList();

            return new JsonResult(employees);
        }

        public JsonResult GetEmployeesSalaryLedgerAll(int? EmpId, DateTime dtFrom, DateTime dtTo, string searchquery)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                JObject jsonObject = JObject.Parse(searchquery);

                string SearchColumns = "";
                string SearchKeywords = "";

                foreach (JProperty property in jsonObject.Properties())
                {
                    string columnName = property.Name;
                    string value = property.Value.ToString();

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

                string userid = HttpContext.Session.GetString("userid");
                var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

                var temp = 0;
                if (approvetype.Contains(1186) && approvetype.Count == 1)
                {
                    temp = 1186;
                }
                else
                if (approvetype.Contains(1187) && approvetype.Count == 1)
                {
                    temp = 1187;
                }
                if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Count == 2)
                {
                    temp = 11861187;
                }
                if (approvetype.Contains(1257) && approvetype.Count == 1)
                {
                    temp = 1257;
                }

                SqlParameter p1 = new SqlParameter("@ComId", comid);
                SqlParameter p2 = new SqlParameter("@EmpId", EmpId);
                SqlParameter p3 = new SqlParameter("@dtfrom", dtFrom);
                SqlParameter p4 = new SqlParameter("@dtto", dtTo);
                SqlParameter p5 = new SqlParameter("@fun2", 2);
                SqlParameter p6 = new SqlParameter("@PageSize", 10);
                SqlParameter p7 = new SqlParameter("@PageIndex", 1);
                SqlParameter p8 = new SqlParameter("@SearchKeywords", SearchKeywords);
                SqlParameter p9 = new SqlParameter("@SearchColumns", SearchColumns);
                SqlParameter p10 = new SqlParameter("@approval", temp);
                var employeelist = Helper.ExecProcMapTList<EmpWiseSalaryLedger>("dbo.Payroll_prcGetEmpSalaryLedgerTest1", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });


                //return  abcd;
                return Json(new { Success = 1, error = false, EmployeeList = employeelist });


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PFContribution
        public IActionResult PFContributionList()
        {
            ViewBag.EmpId = _pFContributionRepository.GetEmpList();

            var empData = _pFContributionRepository.EmpData()
               .Select(s => new
               {
                   EmpId = s.EmpId,
                   EmpCode = s.HR_Emp_PersonalInfo.PFFileNo,
                   EmpName = s.EmpName,
                   DtJoin = s.DtJoin
               })
               .ToList();
            ViewBag.EmpInfo = empData;

            ViewBag.PF = _pFContributionRepository.CatVariableList();

            return View();
        }

        [HttpPost]
        public JsonResult CreatePFContribution(HR_PFContribution PFContribution)
        {
            ViewBag.Comid = HttpContext.Session.GetString("comid");
            if (ModelState.IsValid)
            {
                var check = _pFContributionRepository.PFContributionCheck(PFContribution);
                if (check != null)
                {
                    TempData["Message"] = "Data Already Exist";
                    return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                }

                //string comid = _context.HttpContext.Session.GetString("comid");
                //PFContribution.ComId = comid;
                //PFContribution.UserId = _context.HttpContext.Session.GetString("userid");
                if (PFContribution.PFContributionId > 0)
                {
                    PFContribution.ComId = HttpContext.Session.GetString("comid");
                    PFContribution.UserId = HttpContext.Session.GetString("userid");
                    _pFContributionRepository.Update(PFContribution);
                    TempData["Message"] = "Data Update Successfully";
                }
                else
                {
                    _pFContributionRepository.Add(PFContribution);
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
        public JsonResult DeletePFContributionAjax(int DedId)
        {
            var PFContribution = _pFContributionRepository.FindById(DedId);
            if (PFContribution != null)
            {
                _pFContributionRepository.Delete(PFContribution);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }

            TempData["Message"] = "Data Not Found";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }


        //load salary addition partial view
        public ActionResult LoadPFContributionPartial(DateTime date)
        {
            var PFContributions = _pFContributionRepository.PFContributionPartial(date);
            return Json(PFContributions);
        }
        #endregion






        #region SalaryCheckfill

        public int reportempFill()
        {

            int fun = 0;
            var comid = HttpContext.Session.GetString("comid");
            string userId = HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsDelete == false).Select(s => s.ApprovalType).ToList();
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Contains(1257))
            {
                fun =123;
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



        #region SalaryCheck 
        public IActionResult SalaryCheckList()
        {
            var pType = _salaryCheckRepository.GetProssType();
            ViewBag.ProssType = new SelectList(pType, "ProssType", "ProssType");
            ViewBag.GS = 0;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SalaryCheckList(int pageIndex, int pageSize, string prossType, string searchquery = "")
        {
            
            int fun2= reportempFill();
            var pType = _salaryCheckRepository.GetProssType();
            ViewBag.ProssType = new SelectList(pType, "ProssType", "ProssType");
            List<SalaryCheck> salCheck = null;
            var pageinfo = new PagingInfo();
            var total = new TotalSUm();

            if (prossType != null)
            {
                JObject jsonObject = JObject.Parse(searchquery);

                string SearchColumns = "";
                string SearchKeywords = "";

                foreach (JProperty property in jsonObject.Properties())
                {
                    string columnName = property.Name;
                    string value = property.Value.ToString();

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


                salCheck = _salaryCheckRepository.SalaryCheckList(prossType, 0, pageIndex, pageSize, SearchColumns, SearchKeywords);
                ViewBag.Pross = prossType;
                var totalrow = _salaryCheckRepository.SalaryCheckCount(prossType, 1, pageIndex, pageSize, SearchColumns, SearchKeywords);
                decimal TotalRecordCount = totalrow[0].Results;
                if(TotalRecordCount == 0)
                {
                    totalrow[0].TotalPayable = "0";
                    totalrow[0].TotalPayableB = "0";
                    totalrow[0].NetSalary = "0";
                    totalrow[0].NetSalaryB = "0";
                    totalrow[0].Stamp = "0";
                    totalrow[0].TotalDeduct = "0";
                    totalrow[0].TotalDeductB = "0";
                    totalrow[0].NetSalaryPayable = "0";
                    totalrow[0].NetSalaryPayableB = "0";
                    totalrow[0].CurrEL = "0";
                    totalrow[0].CashPF = "0";
                    totalrow[0].BankPF = "0";
                    totalrow[0].GrossCash = "0";
                    totalrow[0].GrossBank = "0";
                    totalrow[0].CashPay = "0";
                    totalrow[0].BankPay = "0";
                    totalrow[0].AttAllow = "0";
                    totalrow[0].DD = "0";
                }

                total.GS = "Total GS: " + totalrow[0].GS;
                total.BS = "Total BS: " + totalrow[0].BS;
                total.HR = "Total HR: " + totalrow[0].HR;
                total.MA = "Total MA: " + totalrow[0].MA;
                total.FoodAllow = "Total FA: " + totalrow[0].FoodAllow;
                total.ConvAllow = "Total Trn: " + totalrow[0].ConvAllow;
                total.AttBonus = "Total AttBonus: " + totalrow[0].AttBonus;
                total.Arrear = "Total Arrear: " + totalrow[0].Arrear;
                total.OtherAllow = "Total OtherAllow: " + totalrow[0].OtherAllow;
                total.OTHrTtl = "Total OTHour: " + totalrow[0].OTHrTtl;
                total.OT = "Total OT Amount: " + totalrow[0].OT;
                total.OTHRBuyer = "Total Reg OT Hour: " + totalrow[0].OTHRBuyer;
                total.OTAmtBuyer = "Total Reg. OT Amount: " + totalrow[0].OTAmtBuyer;
                total.ExOTHr = "Total Ex. OT Hour: " + totalrow[0].ExOTHr;
                total.ExOTAmount = "Total Ex. OT Amount: " + totalrow[0].ExOTAmount;
                total.DynamicOTHr = "Total Dynamic OT Hour: " + totalrow[0].DynamicOTHr;
                total.DynamicOTAmt = "Total Dynamic OT Amount: " + totalrow[0].DynamicOTAmt;
                total.DynamicExOTHr = "Total Dynamic Ex. OT Hour: " + totalrow[0].DynamicExOTHr;
                total.DynamicExOTAmount = "Total Dynamic Ex. OT Amount: " + totalrow[0].DynamicExOTAmount;
                total.WHDayOTHr = "Total WHDay OT Hour: " + totalrow[0].WHDayOTHr;
                total.WHDayOTAmt = "Total WHDay OT Amount: " + totalrow[0].WHDayOTAmt;
                total.HDayBonus = "Total HDayBonus: " + totalrow[0].HDayBonus;
                total.TotalPayable = "Total Payable: " + convert(totalrow[0].TotalPayable);
                total.TotalPayableB = "Total Payable**: " + convert(totalrow[0].TotalPayableB);
                total.DynamicTotalPayable = "Dynamic Total Payable : " + convert(totalrow[0].DynamicTotalPayable);
                total.AB = "Total Absent amount: " + totalrow[0].AB;
                total.ADV = "Total Adv: " + totalrow[0].ADV;  
                total.Loan = "Total Loan: " + totalrow[0].Loan;
                total.IncomeTax = "Total Tax: " + totalrow[0].IncomeTax;
                total.NetSalary = "Net Salary: " + convert(totalrow[0].NetSalary);
                total.NetSalaryB = "Net Salary**: " + convert(totalrow[0].NetSalaryB);
                total.DynamicNetSalary = "Dynamic Net Salary: " + convert(totalrow[0].DynamicNetSalary);
                total.PF = "Total PF: " + totalrow[0].PF;
                total.Stamp = "Total Stamp: " + convert(totalrow[0].Stamp);
                total.OthersDeduct = "Others Deduction: " + totalrow[0].OthersDeduct;
                total.TotalDeduct = "Total Deduct: " + convert(totalrow[0].TotalDeduct);
                total.TotalDeductB = "Total Dedcut**: " + convert(totalrow[0].TotalDeductB);
                total.DynamicTotalDeduct = "Dynamic Total Deduct: " + convert(totalrow[0].DynamicTotalDeduct);
                total.NetSalaryPayable = "Total Net Salary: " + convert(totalrow[0].NetSalaryPayable);
                total.NetSalaryPayableB = "Total Net Salary**: " + convert(totalrow[0].NetSalaryPayableB);
                total.DynamicNetSalaryPayable = "Total Dynamic Net Salary: " + convert(totalrow[0].DynamicNetSalaryPayable);

                total.CurrEL = "Total ELDay: " + convert(totalrow[0].CurrEL);
                total.ELAmt = "Total EL Amt: " + totalrow[0].ELAmt;
                total.CashPF = "Total Cash PF: " + convert(totalrow[0].CashPF);
                total.BankPF = "Total Bank PF: " + convert(totalrow[0].BankPF);
                total.GrossCash = "Total Gross Cash: " + convert(totalrow[0].GrossCash);
                total.GrossBank = "Total Gross Bank: " + convert(totalrow[0].GrossBank);
                total.CashPay = "Total Cash Pay: " + convert(totalrow[0].CashPay);
                total.BankPay = "Total Bank Pay: " + convert(totalrow[0].BankPay);
                total.AttAllow = "Total Special Allow.: " + convert(totalrow[0].AttAllow);
                total.DD = "Total Day Dedcut: " + convert(totalrow[0].DD);


                var PageCountabc = decimal.Parse((TotalRecordCount / pageSize).ToString());
                var PageCount = Math.Ceiling(PageCountabc);

                pageinfo.PageCount = int.Parse(PageCount.ToString());
                pageinfo.PageNo = pageIndex;
                pageinfo.PageSize = int.Parse(pageSize.ToString());
                pageinfo.TotalRecordCount = int.Parse(TotalRecordCount.ToString());
            }
            return Json(new { Success = 1, error = false, EmployeeList = salCheck, PageInfo = pageinfo, Total = total });


        }
       
        public IActionResult SalaryCheck(string prossType)
        {
            int fun2 = reportempFill();
            List<SalaryCheck> salCheck = null;
            salCheck = _salaryCheckRepository.GetAllSalaryCheck(prossType, 5);
            int TotalRecordCount;
            if (salCheck != null)
            {
                TotalRecordCount = salCheck.Count;
            }
            else
            {
                TotalRecordCount = 0;
            }
            return Json(new { Success = 1, error = false, EmployeeList = salCheck, Total = TotalRecordCount });

        }
        public IActionResult SalaryCheckModern()
        {
            var pType = _salaryCheckRepository.GetProssType();
            ViewBag.ProssType = new SelectList(pType, "ProssType", "ProssType");
            ViewBag.GS = 0;
            return View();
        }

        public string convert(string str)
        {
            int dotIndex1 = str.IndexOf('.');
            string str1 = dotIndex1 != -1 ? str.Substring(0, dotIndex1) : str;

            return str1;
        }

        public class PagingInfo
        {
            public int PageCount { get; set; }
            public int PageNo { get; set; }
            public int PageSize { get; set; }
            public int TotalRecordCount { get; set; }
        }

        public JsonResult GetEmployeesAll(string prossType, string searchquery = "")
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                JObject jsonObject = JObject.Parse(searchquery);

                string SearchColumns = "";
                string SearchKeywords = "";

                foreach (JProperty property in jsonObject.Properties())
                {
                    string columnName = property.Name;
                    string value = property.Value.ToString();

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

                string userid = HttpContext.Session.GetString("userid");
                var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

                //var temp = 0;
                //if (approvetype.Contains(1186))
                //{
                //    temp = 1186;
                //}
                //if (approvetype.Contains(1187))
                //{
                //    temp = 1187;
                //}
                var temp = 0;
                if (approvetype.Contains(1186) && approvetype.Count == 1)
                {
                    temp = 1186;
                }
                else
                if (approvetype.Contains(1187) && approvetype.Count == 1)
                {
                    temp = 1187;
                }
                if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Count == 2)
                {
                    temp = 11861187;
                }
                if (approvetype.Contains(1257) && approvetype.Count == 1)
                {
                    temp = 1257;
                }

                SqlParameter p1 = new SqlParameter("@ComId", comid);
                SqlParameter p2 = new SqlParameter("@ProssType", prossType);
                SqlParameter p3 = new SqlParameter("@fun2", 2);
                SqlParameter p4 = new SqlParameter("@PageSize", 10);
                SqlParameter p5 = new SqlParameter("@PageIndex", 1);
                SqlParameter p6 = new SqlParameter("@SearchKeywords", SearchKeywords);
                SqlParameter p7 = new SqlParameter("@SearchColumns", SearchColumns);
                SqlParameter p8 = new SqlParameter("@approval", temp);
                var employeelist = Helper.ExecProcMapTList<SalaryCheck>("dbo.Payroll_prcGetSalaryCheck", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7, p8 });


                //return  abcd;
                return Json(new { Success = 1, error = false, EmployeeList = employeelist });


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SalarySettlement

        public IActionResult SalarySettlementList()
        {
            var prossType = _salarySettlementRepository.GetProssType();
            ViewBag.ProssType = new SelectList(prossType, "ProssType", "ProssType");
            ViewBag.OTFCList = new List<OTFC>();
            return View();
        }
        [HttpPost]
        public IActionResult CreateSalarySettlement(List<HR_SalarySettlement> HR_SalarySettlements, string ProssType)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                               .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    _salarySettlementRepository.CreateSalarySettlement(HR_SalarySettlements, ProssType);

                    TempData["Message"] = "Salary Settlement Save/Update Successfully";
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
            var OTFCList = _salarySettlementRepository.Search(prossType);

            return Json(new { result = OTFCList, Success = "1" });
        }

        public IActionResult UpdateData(string prossType)
        {
            var OTFCList1 = _salarySettlementRepository.UpdateData(prossType);

            return Json(new { result = OTFCList1, Success = "1" });
        }
        #endregion

        #region WFLedger
        public ViewResult WFLedgerList(string FromDate, string ToDate, string criteria)
        {
            ViewBag.BankAccountId = _wFLedgerRepository.BankAccountList();

            var abcd = _wFLedgerRepository.WFLedgerList(FromDate, ToDate, criteria);
            return View(abcd);

        }
        // GET: Categories/Create
        public ActionResult CreateWFLedger()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            WF_Ledger abc = new WF_Ledger();

            var x = _wFLedgerRepository.GetAll().OrderByDescending(x => x.WFLedgerId).FirstOrDefault();
            if (x != null)
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");

                ViewBag.BankAccountId = _wFLedgerRepository.BankAccountList();

                abc.TranDate = x.TranDate.AddYears(1);
                abc.ReceivedTK = x.ReceivedTK;
                abc.PaymentTK = x.PaymentTK;
                abc.Balance = x.Balance;
                abc.Description = x.Description;
                abc.Remarks = x.Remarks;
                abc.ChequeNo = x.ChequeNo;
                abc.VoucherNo = x.VoucherNo;
                abc.BankAccountId = x.BankAccountId;
            }
            else
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");
                ViewBag.BankAccountId = _wFLedgerRepository.BankAccountList();
                abc.TranDate = DateTime.Now.Date;
            }

            ViewBag.Title = "Create";
            return View(abc);
        }


        public static List<SelectListItem> CriteriaList = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Add", Value="Add"},
            new SelectListItem() { Text="Less", Value="Less"},
        };
        public static List<SelectListItem> AmountTypeList = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Received", Value="Received"},
            new SelectListItem() { Text="Payment", Value="Payment"},
        };

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWFLedger(WF_Ledger WF_Ledger)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });
            if (WF_Ledger.WFLedgerId > 0)
            {
                _wFLedgerRepository.Update(WF_Ledger);

                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), WF_Ledger.WFLedgerId.ToString(), "Update", WF_Ledger.Balance.ToString());

            }
            else
            {
                _wFLedgerRepository.Add(WF_Ledger);

                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), WF_Ledger.WFLedgerId.ToString(), "Create", WF_Ledger.Balance.ToString());

            }
            // _wFLedgerRepository.CreateWFLedger(WF_Ledger);
            return RedirectToAction("WFLedgerList");
        }
        // GET: Categories/Edit/5
        public ActionResult EditWFLedger(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            WF_Ledger WF_Ledger = _wFLedgerRepository.FindById(id);

            if (WF_Ledger == null)
            {
                return NotFound();
            }
            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", WF_Ledger.Criteria);
            ViewBag.BankAccountId = _wFLedgerRepository.BankAccountList();

            ViewBag.Title = "Edit";

            return View("CreateWFLedger", WF_Ledger);

        }

        public JsonResult SetSessionAccountReport(string rptFormat, string FromDate, string ToDate, int? BankAccId)
        {
            try
            {
                var redirectUrl = _wFLedgerRepository.SessionReport(rptFormat, FromDate, ToDate, BankAccId);
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        }

        // GET: Categories/Delete/5
        public ActionResult DeleteWFLedger(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            WF_Ledger WF_Ledger = _wFLedgerRepository.FindById(id);

            if (WF_Ledger == null)
            {
                return NotFound();
            }

            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", WF_Ledger.Criteria);
            ViewBag.BankAccountId = _wFLedgerRepository.BankAccountList();
            ViewBag.Title = "Delete";

            return View("CreateWFLedger", WF_Ledger);
        }
        // POST: Categories/Delete/5
        [HttpPost, ActionName("DeleteWFLedger")]
        //      [ValidateAntiForgeryToken]
        public JsonResult DeleteWFLedgerConfirmed(int? id)
        {
            try
            {
                //_wFLedgerRepository.DeleteWFLedgerConfirmed(id);
                var data = _wFLedgerRepository.FindById(id);
                _wFLedgerRepository.Delete(data);
                return Json(new { Success = 1, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region PFLedger
        public ViewResult PFLedgerList(string FromDate, string ToDate, string criteria)
        {
            ViewBag.BankAccountId = _pFLedgerRepository.BankAccountList();

            var abcd = _pFLedgerRepository.PFLedgerList(FromDate, ToDate, criteria);
            return View(abcd);

        }
        // GET: Categories/Create
        public ActionResult CreatePFLedger()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            PF_Ledger abc = new PF_Ledger();

            var x = _pFLedgerRepository.GetAll().OrderByDescending(x => x.PFLedgerId).FirstOrDefault();
            if (x != null)
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");

                ViewBag.BankAccountId = _pFLedgerRepository.BankAccountList();

                abc.TranDate = x.TranDate.AddYears(1);
                abc.ReceivedTK = x.ReceivedTK;
                abc.PaymentTK = x.PaymentTK;
                abc.Balance = x.Balance;
                abc.Description = x.Description;
                abc.Remarks = x.Remarks;
                abc.ChequeNo = x.ChequeNo;
                abc.VoucherNo = x.VoucherNo;
                abc.BankAccountId = x.BankAccountId;
            }
            else
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");
                ViewBag.BankAccountId = _pFLedgerRepository.BankAccountList();
                abc.TranDate = DateTime.Now.Date;
            }

            ViewBag.Title = "Create";
            return View(abc);
        }


        //public static List<SelectListItem> CriteriaList = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Text="Add", Value="Add"},
        //    new SelectListItem() { Text="Less", Value="Less"},
        //};
        //public static List<SelectListItem> AmountTypeList = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Text="Received", Value="Received"},
        //    new SelectListItem() { Text="Payment", Value="Payment"},
        //};

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePFLedger(PF_Ledger PF_Ledger)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });
            if (PF_Ledger.PFLedgerId > 0)
            {
                _pFLedgerRepository.Update(PF_Ledger);

                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PF_Ledger.PFLedgerId.ToString(), "Update", PF_Ledger.Balance.ToString());

            }
            else
            {
                _pFLedgerRepository.Add(PF_Ledger);

                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PF_Ledger.PFLedgerId.ToString(), "Create", PF_Ledger.Balance.ToString());

            }
            // _wFLedgerRepository.CreateWFLedger(WF_Ledger);
            return RedirectToAction("PFLedgerList");
        }
        // GET: Categories/Edit/5
        public ActionResult EditPFLedger(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            PF_Ledger PF_Ledger = _pFLedgerRepository.FindById(id);

            if (PF_Ledger == null)
            {
                return NotFound();
            }
            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", PF_Ledger.Criteria);
            ViewBag.BankAccountId = _pFLedgerRepository.BankAccountList();

            ViewBag.Title = "Edit";

            return View("CreatePFLedger", PF_Ledger);

        }

        public JsonResult PFSetSessionAccountReport(string rptFormat, string FromDate, string ToDate, int? BankAccId)
        {
            try
            {
                var redirectUrl = _pFLedgerRepository.PFSessionReport(rptFormat, FromDate, ToDate, BankAccId);
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        }

        // GET: Categories/Delete/5
        public ActionResult DeletePFLedger(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            PF_Ledger PF_Ledger = _pFLedgerRepository.FindById(id);

            if (PF_Ledger == null)
            {
                return NotFound();
            }

            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", PF_Ledger.Criteria);
            ViewBag.BankAccountId = _pFLedgerRepository.BankAccountList();
            ViewBag.Title = "Delete";

            return View("CreatePFLedger", PF_Ledger);
        }
        // POST: Categories/Delete/5
        [HttpPost, ActionName("DeletePFLedger")]
        //      [ValidateAntiForgeryToken]
        public JsonResult DeletePFLedgerConfirmed(int? id)
        {
            try
            {
                //_wFLedgerRepository.DeleteWFLedgerConfirmed(id);
                var data = _pFLedgerRepository.FindById(id);
                _pFLedgerRepository.Delete(data);
                return Json(new { Success = 1, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
        #endregion

        #region GratuityLedger
        public ViewResult GratuityLedgerList(string FromDate, string ToDate, string criteria)
        {
            ViewBag.BankAccountId = _gratuityLedgerRepository.BankAccountList();

            var abcd = _gratuityLedgerRepository.GratuityLedgerList(FromDate, ToDate, criteria);
            return View(abcd);

        }
        // GET: Categories/Create
        public ActionResult CreateGratuityLedger()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            Gratuity_Ledger abc = new Gratuity_Ledger();

            var x = _gratuityLedgerRepository.GetAll().OrderByDescending(x => x.GratuityLedgerId).FirstOrDefault();
            if (x != null)
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");

                ViewBag.BankAccountId = _gratuityLedgerRepository.BankAccountList();

                abc.TranDate = x.TranDate.AddYears(1);
                abc.ReceivedTK = x.ReceivedTK;
                abc.PaymentTK = x.PaymentTK;
                abc.Balance = x.Balance;
                abc.Description = x.Description;
                abc.Remarks = x.Remarks;
                abc.ChequeNo = x.ChequeNo;
                abc.VoucherNo = x.VoucherNo;
                abc.BankAccountId = x.BankAccountId;
            }
            else
            {
                ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text");
                ViewBag.BankAccountId = _gratuityLedgerRepository.BankAccountList();
                abc.TranDate = DateTime.Now.Date;
            }

            ViewBag.Title = "Create";
            return View(abc);
        }


        //public static List<SelectListItem> CriteriaList = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Text="Add", Value="Add"},
        //    new SelectListItem() { Text="Less", Value="Less"},
        //};
        //public static List<SelectListItem> AmountTypeList = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Text="Received", Value="Received"},
        //    new SelectListItem() { Text="Payment", Value="Payment"},
        //};

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGratuityLedger(Gratuity_Ledger Gratuity_Ledger)
        {

            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });
            if (Gratuity_Ledger.GratuityLedgerId > 0)
            {
                _gratuityLedgerRepository.Update(Gratuity_Ledger);

                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Gratuity_Ledger.GratuityLedgerId.ToString(), "Update", Gratuity_Ledger.Balance.ToString());

            }
            else
            {
                _gratuityLedgerRepository.Add(Gratuity_Ledger);

                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Gratuity_Ledger.GratuityLedgerId.ToString(), "Create", Gratuity_Ledger.Balance.ToString());

            }
            // _wFLedgerRepository.CreateWFLedger(WF_Ledger);
            return RedirectToAction("GratuityLedgerList");
        }
        // GET: Categories/Edit/5
        public ActionResult EditGratuityLedger(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Gratuity_Ledger Gratuity_Ledger = _gratuityLedgerRepository.FindById(id);

            if (Gratuity_Ledger == null)
            {
                return NotFound();
            }
            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", Gratuity_Ledger.Criteria);
            ViewBag.BankAccountId = _gratuityLedgerRepository.BankAccountList();

            ViewBag.Title = "Edit";

            return View("CreateGratuityLedger", Gratuity_Ledger);

        }

        public JsonResult GratuitySetSessionAccountReport(string rptFormat, string FromDate, string ToDate, int? BankAccId)
        {
            try
            {
                var redirectUrl = _gratuityLedgerRepository.GratuitySessionReport(rptFormat, FromDate, ToDate, BankAccId);
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        }

        // GET: Categories/Delete/5
        public ActionResult DeleteGratuityLedger(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Gratuity_Ledger Gratuity_Ledger = _gratuityLedgerRepository.FindById(id);

            if (Gratuity_Ledger == null)
            {
                return NotFound();
            }

            ViewData["Criteria"] = new SelectList(CriteriaList, "Value", "Text", Gratuity_Ledger.Criteria);
            ViewBag.BankAccountId = _gratuityLedgerRepository.BankAccountList();
            ViewBag.Title = "Delete";

            return View("CreateGratuityLedger", Gratuity_Ledger);
        }
        // POST: Categories/Delete/5
        [HttpPost, ActionName("DeleteGratuityLedger")]
        //      [ValidateAntiForgeryToken]
        public JsonResult DeleteGratuityLedgerConfirmed(int? id)
        {
            try
            {
                //_wFLedgerRepository.DeleteWFLedgerConfirmed(id);
                var data = _gratuityLedgerRepository.FindById(id);
                _gratuityLedgerRepository.Delete(data);
                return Json(new { Success = 1, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }
        #endregion

        #region Salary Addition
        public IActionResult SAList()
        {
            var comid = HttpContext.Session.GetString("comid");

            //ViewBag.SalaryDeductionTypeList = db.Cat_Location
            //    .Select(x => new { x.LId, x.LocationName }).ToList();
            

            var empData = _context.HR_Emp_Info
                .Where(s => s.ComId == comid && s.IsDelete==false)
                .Select(s => new
                {
                    EmpId = s.EmpId,
                    EmpCode = s.EmpCode,
                    EmpName = s.EmpName,
                    DtJoin = s.DtJoin
                })
                .ToList();
            ViewBag.EmpInfo = empData;

            ViewData["EmpId"] = _salaryAdditionRepository.GetEmpInfo();

            ViewBag.OtherAddType = _salaryAdditionRepository.OtherAddType();

            return View();
            //return PartialView("~/Views/Folder/_PartialName.cshtml", new SalaryDeduction());

        }

        //load salary addition partial view
        public ActionResult LoadSalaryAdditionPartial(DateTime date)
        {
            var SalaryAdditions = _salaryAdditionRepository.LoadSAPartial(date);
            return Json(SalaryAdditions);
        }
        public IActionResult UploadSA(IFormFile file, [FromServices] IWebHostEnvironment environment, Payroll_SalaryAddition salaryAddition)
        {
            try
            {

                if (file != null)
                {
                    string uploadlocation = Path.GetFullPath("wwwroot/SampleFormat/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    //string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();


                    //string filename = $"{environment.WebRootPath}\\files\\{file.FileName}";
                    //using (FileStream stream = System.IO.File.Create(filename))
                    //{
                    //    file.CopyTo(stream);
                    //    stream.Flush();
                    //}
                    var addition = this.GetSAList(file.FileName);
                    if (addition.Count() > 0)
                    {
                        foreach (var p in addition)
                        {
                            _context.Payroll_Temp_SalaryDataInputWithFile.Add(p);
                            _context.SaveChanges();
                        }
                        var data = _salaryAdditionRepository.prcUploadSA();

                        TempData["notice"] = "Your data successfully uploaded.";
                        ViewBag.SuccessMessage = TempData["SuccesMeassge"];
                    }
                    else
                    {
                        TempData["Message"] = "Data Save Failed!";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }
            }
            catch (Exception e)
            {
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("SAList");
        }

        private List<Payroll_Temp_SalaryDataInputWithFile> GetSAList(string fName)
        {          
            var salaryAdd = _salaryAdditionRepository.GetSAList(fName);
            return salaryAdd;
        }

        public ActionResult DownloadSA(string file)
        {
         
            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }

            var response = _salaryAdditionRepository.DownloadSA(file);
            return response;
        }

        [HttpPost]
        public JsonResult CreateSA(Payroll_SalaryAddition salaryAddition)
        {
            if (ModelState.IsValid)
            {
                var check = _salaryAdditionRepository.check(salaryAddition);
                if (check != null)
                {
                    TempData["Message"] = "Data Already Exist";
                    return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                }

                string comid = HttpContext.Session.GetString("comid");
                salaryAddition.ComId = comid;
                salaryAddition.UserId = HttpContext.Session.GetString("userid");
                if (salaryAddition.SalAddId > 0)
                {
                    _salaryAdditionRepository.ModifiedSalaryAddition(salaryAddition);
                    TempData["Message"] = "Data Update Successfully";
                }
                else
                {
                    _salaryAdditionRepository.AddSalaryAddition(salaryAddition);
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
        public JsonResult DeleteSalaryAdditionAjax(int addId)
        {
            var salaryAddition = _context.Payroll_SalaryAddition.Find(addId);
            if (salaryAddition != null)
            {
                _salaryAdditionRepository.DeleteSalaryAddition(addId);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }

            TempData["Message"] = "No Data Found";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }
        #endregion

        #region Salary Deduction
        public IActionResult SDList()
        {
            var comid = HttpContext.Session.GetString("comid");

            //ViewBag.SalaryDeductionTypeList = db.Cat_Location
            //    .Select(x => new { x.LId, x.LocationName }).ToList();

            var empData = _context.HR_Emp_Info
                .Where(s => s.ComId == comid && s.IsDelete == false)
                .Select(s => new
                {
                    EmpId = s.EmpId,
                    EmpCode = s.EmpCode,
                    EmpName = s.EmpName,
                    DtJoin = s.DtJoin
                }).ToList();
            ViewBag.EmpInfo = empData;

            ViewData["EmpId"] = _salaryDeductionRepository.GetEmpInfo();

            ViewBag.OtherDedType = _salaryDeductionRepository.OtherDedType();

            return View();
            //return PartialView("~/Views/Folder/_PartialName.cshtml", new SalaryDeduction());

        }

        [HttpPost]
        public JsonResult CreateSD(Payroll_SalaryDeduction SalaryDeduction)
        {
            if (ModelState.IsValid)
            {
                var check = _salaryDeductionRepository.check(SalaryDeduction);
                if (check != null)
                {
                    TempData["Message"] = "Data Already Exist";
                    return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                }

                string comid = HttpContext.Session.GetString("comid");
                SalaryDeduction.ComId = comid;
                SalaryDeduction.UserId = HttpContext.Session.GetString("userid");
                if (SalaryDeduction.SalDedId > 0)
                {
                    _salaryDeductionRepository.ModifiedSalaryDeduction(SalaryDeduction);
                    TempData["Message"] = "Data Update Successfully";
                }
                else
                {
                    _salaryDeductionRepository.AddSalaryDeduction(SalaryDeduction);
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
        public JsonResult DeleteSalaryDeductionAjax(int SalDedId)
        {
            var salaryDeduction = _context.Payroll_SalaryDeduction.Find(SalDedId);
            if (salaryDeduction != null)
            {
                _salaryDeductionRepository.DeleteSalaryDeduction(SalDedId);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }

            TempData["Message"] = "Data Not Found";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }


        //load salary addition partial view
        public ActionResult LoadSalaryDeductionPartial(DateTime date)
        {
            string comid = HttpContext.Session.GetString("comid");
            var SalaryDeductions = _context.Payroll_SalaryDeduction
                .Include(s => s.HR_Emp_Info)
                .Include(s => s.HR_Emp_Info.Cat_Section)
                .Where(s => s.DtInput.Date.Month == date.Month && s.DtInput.Date.Year == date.Year && s.ComId == comid && s.IsDelete==false)
                .Select(s => new SalaryDeduction
                {
                    SalDedId = s.SalDedId,
                    EmpId = s.EmpId,
                    EmpCode = s.HR_Emp_Info.EmpCode,
                    EmpName = s.HR_Emp_Info.EmpName,
                    Section = s.HR_Emp_Info.Cat_Section.SectName,
                    Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                    DtJoin = s.HR_Emp_Info.DtJoin.HasValue ? s.HR_Emp_Info.DtJoin.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    DtInput = s.DtInput.ToString("dd-MMM-yyyy"),
                    Amount = s.Amount,
                    Remarks = s.Remarks,
                    OtherDedType = s.OtherDedType
                }).ToList();

            return Json(SalaryDeductions);
        }
        public IActionResult UploadSD(IFormFile file, [FromServices] IWebHostEnvironment environment, SalaryDeduction SalaryDeduction)
        {
            try
            {

                if (file != null)   
                {

                    string uploadlocation = Path.GetFullPath("wwwroot/SampleFormat/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var deduction = this.GetSDList(file.FileName);
                    if (deduction.Count() > 0)
                    {
                        foreach (var p in deduction)
                        {
                            _context.Payroll_Temp_SalaryDataInputWithFile.Add(p);
                            _context.SaveChanges();
                        }

                        var data = _salaryDeductionRepository.prcUploadSD();

                        TempData["notice"] = "Your data successfully uploaded.";

                        ViewBag.SuccessMessage = TempData["SuccesMeassge"];
                    }
                    else
                    {
                        TempData["Message"] = "Data Save Failed!";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                ViewData["Message"] = "Error Occured";
            }
            return RedirectToAction("SDList");


        }
        private List<Payroll_Temp_SalaryDataInputWithFile> GetSDList(string fName)
        {
            var salarydeduct = _salaryDeductionRepository.GetSDList(fName);
            
            return salarydeduct;
        }
        public ActionResult DownloadSD(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }

            var response = _salaryDeductionRepository.DownloadSD(file);
            return response;
        }
        #endregion

        #region Excel Upload
        public IActionResult ExcelUploadList()
        {
            var comid = HttpContext.Session.GetString("comid");
            return View();
        }


        [HttpPost]
        [Obsolete]
        public ActionResult ExcelUploadFiles(IList<IFormFile> fileData)
        {
            try
            {

                #region excelupload

                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }

                _excelUploadRepository.ExcelUploadFiles(fileData);

                #endregion


                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;



                return Json(new { Success = 1 });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }

        [Obsolete]
        public ActionResult DownloadExcelUpload(string file)
        {

            string filepath = environment.WebRootPath + "\\SampleFormat\\" + file;
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


        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        [HttpPost]
        public ActionResult UploadFilesPOList()
        {
            try
            {

                #region excelupload
                //var userid = HttpContext.Session.GetString("userid");
                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }
                #endregion


                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;

                List<Temp_COM_MasterLC_Detail> data = _excelUploadRepository.UploadFilePO();
                // return Json(data, JsonRequestBehavior.AllowGet);
                return Json(data);

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }

        public DataTable CustomTable(DataTable excelTable, string currentuserid)
        {
            var table = _excelUploadRepository.CustomTable(excelTable, currentuserid);
            return table;
        } /// <summary>

        #endregion

        #region ML Excel File Upload

        public IActionResult MLExcelUploadList()
        {
            var comid = HttpContext.Session.GetString("comid");
            return View();
        }
        [HttpPost]
        [Obsolete]
        public ActionResult MLUploadFiles(IList<IFormFile> fileData)
        {
            try
            {

                #region excelupload
                //var userid = HttpContext.Session.GetString("userid");
                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }

                _mLExcelUploadRepository.MLExcelUploadFiles(fileData);

                #endregion


                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;



                return Json(new { Success = 1 });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }

        [Obsolete]
        public ActionResult MLDownload(string file)
        {

            string filepath = environment.WebRootPath + "\\SampleFormat\\MLUpload\\" + file;
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

        [HttpPost]
        public ActionResult MLUploadFilesPOList()
        {
            try
            {

                #region excelupload
                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }
                #endregion

                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;

                List<Temp_COM_MasterLC_Detail> data = _mLExcelUploadRepository.MLUploadFilePO();

                return Json(data);

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }

        public DataTable MLCustomTable(DataTable excelTable, string currentuserid)
        {
            var table = _mLExcelUploadRepository.MLCustomTable(excelTable, currentuserid);
            return table;
        } /// <summary>
        #endregion

        #region PEL Excel File Upload
        public IActionResult PELExcelFileUploadList()
        {
            var comid = HttpContext.Session.GetString("comid");
            return View();
        }

        [HttpPost]
        [Obsolete]
        public ActionResult PELUploadFiles(IList<IFormFile> fileData)
        {
            try
            {

                #region excelupload

                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }

                _pELExcelFileUploadRepository.PELExcelUploadFiles(fileData);
                #endregion


                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;



                return Json(new { Success = 1 });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }

        [Obsolete]
        public ActionResult PELDownload(string file)
        {

            string filepath = environment.WebRootPath + "\\SampleFormat\\PELUpload\\" + file;
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

        [HttpPost]
        public ActionResult PELUploadFilesPOList()
        {
            try
            {

                #region excelupload
                //var userid = HttpContext.Session.GetString("userid");
                var userid = HttpContext.Session.GetString("userid");

                if (userid.ToString() == "" || userid == null)
                {

                    return BadRequest();
                }
                #endregion


                ViewBag.Title = "Create";
                ViewBag.BuyerID = 1;

                List<Temp_COM_MasterLC_Detail> data = _pELExcelFileUploadRepository.PELUploadFilePO();
                return Json(data);

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }
        public DataTable PELCustomTable(DataTable excelTable, string currentuserid)
        {
            var table = _pELExcelFileUploadRepository.PELCustomTable(excelTable, currentuserid);
            return table;
        } /// <summary>

        #endregion

        #region PFEmpOpBal
        public IActionResult PFEmpOpBalList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            //var sectio
            return View(_pFEmpOpBalRepository.GetIndexInfo());
        }

        // GET: Section/Create
        public IActionResult CreatePFEmpOpBal()
        {
            string ComId = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Create";


            ViewData["EmpId"] = _pFEmpOpBalRepository.CreatelEmpInfoList();

            ViewBag.DebitAccId = _pFEmpOpBalRepository.CreateDebitAccId();
            ViewBag.CreditAccId = _pFEmpOpBalRepository.CreateCreditAccId();


            ViewBag.PFOPBalYID = _pFEmpOpBalRepository.CreatePFOPBalYID();
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePFEmpOpBal(HR_Emp_PF_OPBal PFOPBal)
        {
            if (ModelState.IsValid)
            {
                PFOPBal.UserId = HttpContext.Session.GetString("userid");
                PFOPBal.ComId = HttpContext.Session.GetString("comid");

                PFOPBal.DateUpdated = DateTime.Today;

                PFOPBal.DateAdded = DateTime.Today;

                if (PFOPBal.PFOPBalId > 0)
                {
                    PFOPBal.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _pFEmpOpBalRepository.Update(PFOPBal);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PFOPBal.PFOPBalYID.ToString(), "Update", PFOPBal.PFOPBalYID.ToString());

                }
                else
                {
                    _pFEmpOpBalRepository.Add(PFOPBal);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PFOPBal.PFOPBalYID.ToString(), "Create", PFOPBal.PFOPBalYID.ToString());

                }
                return RedirectToAction(nameof(PFEmpOpBalList));
            }
            return View(PFOPBal);
        }

        // GET: Section/Edit/5
        public IActionResult EditPFEmpOpBal(int? id)
        {
            string ComId = HttpContext.Session.GetString("comid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var PFOPBal = _pFEmpOpBalRepository.FindById(id);


            ViewData["EmpId"] = _pFEmpOpBalRepository.DelEmpInfoList(id);
            ViewData["DebitAccId"] = _pFEmpOpBalRepository.DebitAccId(id);
            ViewData["CreditAccId"] = _pFEmpOpBalRepository.CreditAccId(id);

            ViewBag.PFOPBalYID = _pFEmpOpBalRepository.PFOPBalYID(id);

            if (PFOPBal == null)
            {
                return NotFound();
            }
            return View("CreatePFEmpOpBal", PFOPBal);
        }


        // GET: Section/Delete/5
        public IActionResult DeletePFEmpOpBal(int? id)
        {
            string ComId = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }

            var PFOPBal = _pFEmpOpBalRepository.GetEmpPFOpBal(id);

            if (PFOPBal == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            ViewData["EmpId"] = _pFEmpOpBalRepository.DelEmpInfoList(id);

            ViewBag.PFOPBalYID = _pFEmpOpBalRepository.PFOPBalYID(id);
            ViewData["DebitAccId"] = _pFEmpOpBalRepository.DebitAccId(id);
            ViewData["CreditAccId"] = _pFEmpOpBalRepository.CreditAccId(id);

            return View("CreatePFEmpOpBal", PFOPBal);

        }
        [HttpPost, ActionName("DeletePFEmpOpBal")]

        public async Task<IActionResult> DeletePFEmpOpBalConfirmed(int id)
        {
            try
            {
                var PFOPBal = _pFEmpOpBalRepository.FindById(id);
                _pFEmpOpBalRepository.Delete(PFOPBal);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PFOPBal.PFOPBalYID.ToString(), "Delete", PFOPBal.PFOPBalYID.ToString());

                return Json(new { Success = 1, PFOPBalYID = PFOPBal.HR_Emp_Info.EmpId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
        #endregion


        #region SalaryEdit
        [HttpPost]
        public IActionResult SearchProssData(string prossType, string tableName)
        {
            var prossDataList = _salaryAdditionRepository.GetPrcDataSal(prossType, tableName);

            return Json(new { result = prossDataList, Success = "1" });
        }
        public IActionResult SalaryEdit()
        {
            var prossType = _salarySettlementRepository.GetProssType();
            ViewBag.ProssType = new SelectList(prossType, "ProssType", "ProssType");
            ViewBag.OTFCList = new List<HrProcessedDataSal>();
            
            
            
            return View();
        }
        [HttpPost]
        public IActionResult SalaryEdit(List<HrProcessedDataSalUpdate> hrProcessedDataSals, string ProssType)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                               .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    _salaryAdditionRepository.SalaryUpdate(hrProcessedDataSals, ProssType);

                    TempData["Message"] = "Salary Data has been Updated Successfully";
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


        public IActionResult PFWithdrawnList()
        {
            var comid = HttpContext.Session.GetString("comid");

            //ViewBag.SalaryDeductionTypeList = db.Cat_Location
            //    .Select(x => new { x.LId, x.LocationName }).ToList();


            var empData = _context.HR_Emp_Info
                .Where(s => s.ComId == comid && s.IsDelete == false)
                .Select(s => new
                {
                    EmpId = s.EmpId,
                    EmpCode = s.EmpCode,
                    EmpName = s.EmpName,
                    DtJoin = s.DtJoin
                })
                .ToList();
            ViewBag.EmpInfo = empData;

            ViewBag.EmpId = _pFContributionRepository.GetEmpList();
            //ViewData["EmpId"] = _pFWithdrawnRepository.GetEmpInfo();

            //ViewData["EmpId"] = _salaryAdditionRepository.GetEmpInfo();

            //ViewBag.OtherAddType = _salaryAdditionRepository.OtherAddType();

            return View();
            //return PartialView("~/Views/Folder/_PartialName.cshtml", new SalaryDeduction());

        }
        public ActionResult LoadPFWithdawnPartial(DateTime date)
        {
            var pFWithdrawns = _pFWithdrawnRepository.LoadPFWithdawnPartial(date);
            return Json(pFWithdrawns);
        }
        //public IActionResult UploadPFWithdrawn(IFormFile file, [FromServices] IWebHostEnvironment environment, HR_PF_Withdrawn pFWithdrawn)
        //{
        //    try
        //    {

        //        if (file != null)
        //        {
        //            string uploadlocation = Path.GetFullPath("wwwroot/SampleFormat/");

        //            if (!Directory.Exists(uploadlocation))
        //            {
        //                Directory.CreateDirectory(uploadlocation);
        //            }

        //            string filePath = uploadlocation + Path.GetFileName(file.FileName);

        //            //string extension = Path.GetExtension(file.FileName);
        //            var fileStream = new FileStream(filePath, FileMode.Create);
        //            file.CopyTo(fileStream);
        //            fileStream.Close();


        //            //string filename = $"{environment.WebRootPath}\\files\\{file.FileName}";
        //            //using (FileStream stream = System.IO.File.Create(filename))
        //            //{
        //            //    file.CopyTo(stream);
        //            //    stream.Flush();
        //            //}
        //            var addition = this.GetPFWithdrawnList(file.FileName);
        //            if (addition.Count() > 0)
        //            {
        //                foreach (var p in addition)
        //                {
        //                    _context.Payroll_Temp_SalaryDataInputWithFile.Add(p);
        //                    _context.SaveChanges();
        //                }
        //                var data = _salaryAdditionRepository.prcUploadSA();

        //                TempData["notice"] = "Your data successfully uploaded.";
        //                ViewBag.SuccessMessage = TempData["SuccesMeassge"];
        //            }
        //            else
        //            {
        //                TempData["Message"] = "Data Save Failed!";
        //            }
        //        }
        //        else
        //        {
        //            TempData["Message"] = "Please, Select a excel file!";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ViewData["Message"] = "Error Occured";
        //    }
        //    //Process();
        //    return RedirectToAction("PFWithdrawnList");
        //}

        //private List<Payroll_Temp_SalaryDataInputWithFile> GetSAList(string fName)
        //{
        //    var pFWithdrawnAdd = _pFWithdrawnListRepository.GetSAList(fName);
        //    return pFWithdrawnAdd;
        //}

        //public ActionResult DownloadPFWithdrawnList(string file)
        //{

        //    string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
        //    if (!System.IO.File.Exists(filepath))
        //    {
        //        return NotFound();
        //    }

        //    var response = _salaryAdditionRepository.DownloadSA(file);
        //    return response;
        //}

        [HttpPost]
        public JsonResult CreatePFWithdrawn(HR_PF_Withdrawn PFWithdrawn)
        {

            var check = _pFWithdrawnRepository.check(PFWithdrawn);
            if (check != null)
            {
                TempData["Message"] = "Data Already Exist";
                return Json(new { Success = 2, ex = TempData["Message"].ToString() });
            }

            string comid = HttpContext.Session.GetString("comid");
                PFWithdrawn.ComId = comid;
                PFWithdrawn.UserId = HttpContext.Session.GetString("userid");
                if (PFWithdrawn.WdId > 0)
                {
                    _pFWithdrawnRepository.ModifiedpF_WithdrawnAddition(PFWithdrawn);
                    TempData["Message"] = "Data Update Successfully";
                }
                else
                {
                    _pFWithdrawnRepository.AddpF_WithdrawnAddition(PFWithdrawn);
                    TempData["Message"] = "Data Save Successfully";
                }
                _context.SaveChanges();
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            
          
                TempData["Message"] = "Models state is not valid";
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            

        }

        [HttpPost]
        public JsonResult DeletePFWithdrawnAdditionAjax(int addId)
        {
            var PFWithdrawn = _context.HR_PF_Withdrawn.Find(addId);
            if (PFWithdrawn != null)
            {
                _pFWithdrawnRepository.DeletepF_WithdrawnAddition(addId);
                TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ex = TempData["Message"].ToString() });
            }

            TempData["Message"] = "No Data Found";
            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
        }

    }
}
