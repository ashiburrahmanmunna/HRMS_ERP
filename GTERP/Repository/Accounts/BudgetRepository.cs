using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public BudgetRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            )
        {
            _context = context;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public void Delete(int id)
        {
            var BudgetMains = _context.BudgetMains.Find(id);
            _context.BudgetMains.Remove(BudgetMains);
            _context.SaveChanges();
        }

        public void Edit(int id, BudgetDetails BudgetDetails)
        {

            BudgetDetails.DateUpdated = DateTime.Now;
            BudgetDetails.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");

            _context.BudgetDetails.Attach(BudgetDetails);
            _context.Entry(BudgetDetails).Property(x => x.BudgetDebit).IsModified = true;
            _context.Entry(BudgetDetails).Property(x => x.BudgetCredit).IsModified = true;
            _context.Entry(BudgetDetails).Property(x => x.DateUpdated).IsModified = true;
            _context.Entry(BudgetDetails).Property(x => x.UpdateByUserId).IsModified = true;

            _context.SaveChanges();
        }
        private bool BudgetMainsExists(int id)
        {
            return _context.BudgetMains.Any(e => e.BudgetMainId == id);
        }

        public IEnumerable<SelectListItem> FiscalMonthId(int? id)
        {
            var BudgetMains = _context.BudgetMains
              .Include(y => y.YearName)
              .Include(m => m.MonthName)
              .Where(b => b.BudgetMainId == id).FirstOrDefault();

            return new SelectList(_context.Acc_FiscalMonths
                .Where(m => m.FiscalMonthId == BudgetMains.FiscalMonthId), "FiscalMonthId", "MonthName", BudgetMains.FiscalMonthId);
        }

        public IEnumerable<SelectListItem> FiscalYearId(int? id)
        {
            var BudgetMains = _context.BudgetMains
               .Include(y => y.YearName)
               .Include(m => m.MonthName)
               .Where(b => b.BudgetMainId == id).FirstOrDefault();

            return new SelectList(_context.Acc_FiscalYears
               .Where(y => y.FiscalYearId == BudgetMains.FiscalYearId), "FiscalYearId", "FYName", BudgetMains.FiscalYearId);
        }

        public IQueryable<BudgetMainsList> Get()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            //var abc = db.Products.Include(y => y.vPrimaryCategory); Include(x=>x.DistrictWiseBudgetMains).Include(x=>x.YearName).Include(x=>x.MonthName).Include(x=>x.Cat_PoliceStation).Include(x=>x.Cat_District).
            var query = from e in _context.BudgetMains.Where(x => x.BudgetMainId > 0 && x.ComId == comid).OrderByDescending(x => x.BudgetMainId)
                        select new BudgetMainsList
                        {
                            BudgetMainId = e.BudgetMainId,
                            Year = e.YearName.FYName,
                            Month = e.MonthName.MonthName,
                            TotalBudgetDebit = e.TotalBudgetDebit.ToString(),
                            TotalBudgetCredit = e.TotalBudgetCredit.ToString(),
                        };
            return query;
        }

        public IEnumerable<SelectListItem> ParentId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts.Where(x => x.ComId == comid && x.AccType == "G"), "AccId", "AccName");
        }

        public void Update(BudgetDetails item)
        {

            _context.Update(item);

            _context.Entry(item).State = EntityState.Modified;
        }

        public string Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");


            //var abcvouchermain = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id && x.ComId == comid).FirstOrDefault();

            var reportname = "rptCOA_Budget";

            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Acc_rptCOA_Budget] '" + comid + "'," + id + " ");


            string filename = _context.BudgetMains.Include(x => x.YearName).Where(x => x.BudgetMainId == id).Select(x => x.YearName.FYName).Single();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";
            _httpContext.HttpContext.Session.SetObject("rptList", postData);


            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            string redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = type });//, new { id = 1 }
            return redirectUrl;
        }

        public List<Acc_BudgetData> BudgetDataLoadByParameter(int? Yearid, int? Monthid, int? ParentId)
        {
            var result = "";
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            //var userid = HttpContext.Session.GetString("userid");
            if (comid == null)
            {
                result = "Please Login first";
            }
            var quary = $"EXEC Acc_BudgetData '{comid}',{Yearid},'{Monthid}','{ParentId}' ";

            //EXEC[Acc_BudgetData] '31312c54-659b-4e63-b4ba-2bc3d7b05792',2,'14',10141


            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@YearId", Yearid);
            parameters[2] = new SqlParameter("@MonthId", Monthid);
            parameters[3] = new SqlParameter("@ParentId", ParentId);

            List<Acc_BudgetData> Acc_BudgetData = Helper.ExecProcMapTList<Acc_BudgetData>("Acc_BudgetData", parameters);
            return Acc_BudgetData;
        }

        public List<BudgetMain> GetAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.BudgetMains.Take(1).Where(x => x.ComId == comid).Include(y => y.YearName).Include(m => m.MonthName).ToList();
        }
    }
}
