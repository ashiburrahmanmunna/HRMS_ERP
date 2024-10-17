using DataTablesParser;
using GTERP.Interfaces.Inventory;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Repository.Inventory
{
    public class PurchaseOrderRepository: BaseRepository<PurchaseOrderMain>, IPurchaseOrderRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<PurchaseOrderRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }

        public PurchaseOrderRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<PurchaseOrderRepository> logger,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor,
             IUrlHelperFactory urlHelperFactory
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public IEnumerable<SelectListItem> Userlist()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");
            var appKey = _httpcontext.HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            var l = new List<AspnetUserList>();
            foreach (var c in list)
            {
                var le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }
            return new SelectList(l.Where(u => !_context.UserPermission.Any(p => p.AppUserId == u.UserId)), "UserId", "UserName");
        }

        public IQueryable<PurchaseOrderResult> parser()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var query = from e in _context.PurchaseOrderMain.Where(x => x.ComId == comid)
                                .OrderByDescending(x => x.PurOrderMainId)
                        select new PurchaseOrderResult
                        {
                            PurOrderMainId = e.PurOrderMainId,
                            PONo = e.PONo,
                            PODate = e.PODate.ToString("dd-MMM-yy"),
                            Department = e.Department.DeptName,
                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                            ConvertionRate = e.ConvertionRate,
                            TotalPOValue = e.TotalPOValue,
                            Deduction = e.Deduction,
                            NetPOValue = e.NetPOValue,
                            SectName = e.Section != null ? e.Section.SectName : "",
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            LastDateOfDelivery = e.LastDateOfDelivery.ToString("dd-MMM-yy"),
                            ExpectedRecivedDate = ((DateTime)e.ExpectedRecivedDate).ToString("dd-MMM-yy")
                        };
            return  query;
            //return null;
        }

        public PurchaseOrderMain PurchaseOrderMain(int? id)
        {
             return _context.PurchaseOrderMain
                .Include(p => p.Section)
                .Include(p => p.Currency)
                .Include(p => p.PaymentType)
                .Include(p => p.PrdUnit)
                .Include(p => p.PurchaseRequisitionMain)
                .Include(p => p.Supplier)
                .FirstOrDefault(m => m.PurOrderMainId == id);
        }

        public List<PurchaseOrderDetailsModel> GetPurchaseRequisitionDataById(int? PurReqId)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");

            var quary = $"EXEC PurchaseOrderDetailsInformation '{comid}','{userid}',{PurReqId}";

            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@userid", userid);
            parameters[2] = new SqlParameter("@PurReqId", PurReqId);
            List<PurchaseOrderDetailsModel> PurchaseOrderDetailsInformation = Helper.ExecProcMapTList<PurchaseOrderDetailsModel>("PurchaseOrderDetailsInformation", parameters);

            return PurchaseOrderDetailsInformation;
        }

        public IEnumerable<SelectListItem> DeptId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Department.Where(x => x.ComId == comid && !x.IsDelete), "DeptId", "DeptName");
        }

        public IEnumerable<SelectListItem> SectId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Section.Where(x => x.ComId == comid && !x.IsDelete), "SectId", "SectName");
        }

        public IEnumerable<SelectListItem> CurrencyId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Currency.OrderByDescending(x => x.isDefault && !x.IsDelete), "CurrencyId", "CurCode");
        }

        public IEnumerable<SelectListItem> PaymentTypeId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName");
            
        }

        public IEnumerable<SelectListItem> PrdUnitId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PrdUnits.Where(x => x.ComId == comid && !x.IsDelete), "PrdUnitId", "PrdUnitName");
        }

        public IEnumerable<SelectListItem> PurReqId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1 && !x.IsDelete), "PurReqId", "PRNo");
        }

        public IEnumerable<SelectListItem> SupplierId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Suppliers.Where(x => x.ComId == comid && !x.IsDelete), "SupplierId", "SupplierName");
        }

        public IEnumerable<SelectListItem> DistrictId()
        {
            return new SelectList(_context.Cat_District.Where(x=> !x.IsDelete), "DistId", "DistName");
        }

        public PurchaseOrderMain FindByIdPMain(int id)
        {
            return _context.PurchaseOrderMain.Include(a => a.PurchaseOrderSub).Where(a => a.PurReqId == id).FirstOrDefault();
        }

        public string PrintPurchaseOrder(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpcontext.HttpContext.Session.GetString("comid");

            //var abcvouchermain = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id && x.ComId == comid).FirstOrDefault();

            var reportname = "rptPO";

            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            _httpcontext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            _httpcontext.HttpContext.Session.SetString("reportquery", "Exec [rptPODetails] '" + comid + "', 'PONW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = _context.PurchaseOrderMain.Where(x => x.PurOrderMainId == id).Select(x => x.PONo).Single();
            _httpcontext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";
            _httpcontext.HttpContext.Session.SetObject("rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpcontext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpcontext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            string redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = type });//, new { id = 1 }
            return redirectUrl;
        }

        public PurchaseOrderMain PurchaseOrderMains(int id)
        {
            return _context.PurchaseOrderMain
                .Include(p => p.PurchaseOrderSub)
                .ThenInclude(p => p.vProduct)
                .ThenInclude(p => p.vProductUnit)
                .FirstOrDefault(m => m.PurOrderMainId == id && m.Status == 0);
        }

        public void CreateSupplier(Supplier supplier)
        {
            _context.Add(supplier);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            _context.Entry(supplier).State = EntityState.Modified;
        }

        public void CreatePurchaseOrderSub(PurchaseOrderSub purchaseOrderSub)
        {
            _context.PurchaseOrderSub.Add(purchaseOrderSub);
        }

        public void UpdatePurchaseOrderSub(PurchaseOrderSub purchaseOrderSub)
        {
             _context.Entry(purchaseOrderSub).State = EntityState.Modified;
        }

        public PurchaseOrderMain duplicateDocument(PurchaseOrderMain purchaseOrderMain)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return _context.PurchaseOrderMain.Where(i => i.PONo == purchaseOrderMain.PONo && i.PurReqId != purchaseOrderMain.PurReqId && i.ComId == comid).FirstOrDefault();
        }

        public Acc_FiscalMonth FiscalMonth(PurchaseOrderMain purchaseOrderMain)
        {
            DateTime date = purchaseOrderMain.PODate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
        }

        public Acc_FiscalYear FiscalYear(PurchaseOrderMain purchaseOrderMain)
        {
            return _context.Acc_FiscalYears.Where(x => x.FYId == FiscalMonth(purchaseOrderMain).FYId).FirstOrDefault();
        }

        public HR_ProcessLock LockCheck(PurchaseOrderMain purchaseOrderMain)
        {
            return _context.HR_ProcessLock
                .Where(p => p.LockType.Contains("Store Lock") && p.DtDate.Date <= purchaseOrderMain.PODate.Date && p.DtToDate.Value.Date >= purchaseOrderMain.PODate.Date
                    && p.IsLock == true).FirstOrDefault();
        }

       
    }
}
