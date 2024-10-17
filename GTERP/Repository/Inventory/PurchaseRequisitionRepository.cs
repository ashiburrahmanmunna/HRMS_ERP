using GTERP.BLL;
using GTERP.Interfaces.Inventory;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Repository.Inventory
{

    public class PurchaseRequisitionRepository : IPurchaseRequisitionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private readonly IConfiguration _configuration;
        PermissionLevel PL;
        public clsProcedure clsProc { get; }

        public PurchaseRequisitionRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IConfiguration configuration,
             PermissionLevel _pl
            )
        {
            _context = context;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _configuration = configuration;
            PL = _pl;
        }

        public void AddProduct(Models.Product product)
        {
            product.userid = _httpContext.HttpContext.Session.GetString("userid");
            product.comid = _httpContext.HttpContext.Session.GetString("comid");
            product.DateAdded = DateTime.Now;
            product.ProductImage = null;

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public IEnumerable<SelectListItem> ApprovedByEmpId(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid), "EmployeeId", "EmployeeName", purchaseRequisitionMain.ApprovedByEmpId);
        }

        public void DeletePrSub(int prsubid)
        {
            var sub = _context.PurchaseRequisitionSub.Find(prsubid);
            _context.PurchaseRequisitionSub.Remove(sub);
            _context.SaveChanges();
        }

        public void DeletePurchase(int? id)
        {
            var storeRequisitionMain = _context.PurchaseRequisitionMain.Find(id);
            _context.PurchaseRequisitionMain.Remove(storeRequisitionMain);
            _context.SaveChangesAsync();
        }

        public PurchaseRequisitionMain Details(int? id)
        {
            var purchaseRequisitionMain = _context.PurchaseRequisitionMain
                .Include(p => p.ApprovedBy)
                .Include(p => p.Department)
                .Include(p => p.PrdUnit)
                .Include(p => p.Purpose)
                .Include(p => p.RecommenedBy)
                .FirstOrDefault(m => m.PurReqId == id);
            return purchaseRequisitionMain;
        }

        public PurchaseRequisitionMain Edit(int? id)
        {

            var data = _context.PurchaseRequisitionMain.Include(p => p.PurchaseRequisitionSub).
               ThenInclude(p => p.vProduct)
               .ThenInclude(p => p.vProductUnit)
               .Where(p => p.PurReqId == id && p.Status == 0)
               .FirstOrDefault();
            return data;
        }

        public void EditRequest(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            DateTime date = purchaseRequisitionMain.ReqDate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("comid");
            var activefiscalmonth = _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
            var activefiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();

            purchaseRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
            purchaseRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;

            var UpdateDate = DateTime.Now;
            purchaseRequisitionMain.ComId = comid;
            purchaseRequisitionMain.UpdateByUserId = userid;
            purchaseRequisitionMain.DateUpdated = UpdateDate;
            IQueryable<PurchaseRequisitionSub> PurchaseRequisitionSub = _context.PurchaseRequisitionSub.Where(p => p.PurReqId == purchaseRequisitionMain.PurReqId);

            var sl = 0;
            foreach (PurchaseRequisitionSub item in purchaseRequisitionMain.PurchaseRequisitionSub)
            {
                sl++;
                if (item.PurReqSubId > 0)
                {
                    if (item.IsDelete != true)
                    {
                        _context.Entry(item).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.Entry(item).State = EntityState.Deleted;
                        _context.SaveChanges();
                    }

                }
                else
                {
                    try
                    {
                        var PcName = "";
                        var sub = new PurchaseRequisitionSub();
                        sub.ComId = comid;
                        sub.DateAdded = item.DateAdded;
                        sub.DateUpdated = item.DateUpdated;
                        sub.Note = item.Note;
                        sub.PcName = PcName;
                        sub.ProductId = item.ProductId;
                        sub.PurReqId = item.PurReqId;
                        sub.PurReqQty = item.PurReqQty;
                        sub.PurReqSubId = item.PurReqSubId;
                        sub.RemainingReqQty = item.RemainingReqQty;
                        sub.SLNo = sl;
                        sub.UpdateByUserId = item.UpdateByUserId;

                        sub.UserId = userid;

                        _context.PurchaseRequisitionSub.Add(sub);

                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }

                }

            }

            _context.Entry(purchaseRequisitionMain).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public IQueryable<PurchaseReQuisitionResult> Get()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var query = from e in _context.PurchaseRequisitionMain.Where(x => x.ComId == comid)
                                 .OrderByDescending(x => x.PurReqId)
                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                        select new PurchaseReQuisitionResult
                        {
                            PurReqId = e.PurReqId,
                            PRNo = e.PRNo,
                            RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                            ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                            ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                            RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                            BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                            PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                            DeptName = e.Department != null ? e.Department.DeptName : "",
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            Remarks = e.Remarks,
                            ReqRef = e.ReqRef,
                        };
            return query;
        }



        public Models.Product getProduct(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Products.Where(y => y.ProductId == id && y.comid == comid).SingleOrDefault();
        }

        public Unit getUnit(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var product = _context.Products.Where(y => y.ProductId == id && y.comid == comid).SingleOrDefault();
            return _context.Unit.Where(y => y.UnitId == product.UnitId).SingleOrDefault();
        }

        public List<AspnetUserList> IndexList()
        {
            var model = new GetUserModel();
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
            return l;
        }

        public IEnumerable<SelectListItem> PrdUnitId(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitShortName", purchaseRequisitionMain.PrdUnitId);
        }

        public string Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "rptPR";

            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            _httpContext.HttpContext.Session.SetString("reportquery", "Exec [rptPRDetails] '" + comid + "', 'PRNW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

            string filename = _context.PurchaseRequisitionMain.Where(x => x.PurReqId == id).Select(x => x.PRNo).Single();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";
            _httpContext.HttpContext.Session.SetObject("rptList", postData);

            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });//, new { id = 1 }
            return callBackUrl;


        }

        public IEnumerable<SelectListItem> ProductId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Products.Where(x => x.comid == comid), "ProductId", "ProductName");
        }

        public IEnumerable<SelectListItem> ProductList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> ProductList2(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> PurposeId(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            return new SelectList(_context.Purpose, "PurposeId", "PurposeName", purchaseRequisitionMain.PurposeId);
        }

        public IQueryable<PurchaseReQuisitionResult> QueryTest(string UserList)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var querytest = from e in _context.PurchaseRequisitionMain
                          .Where(x => x.ComId == comid)
                          .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                          //.Where(p => p.userid == UserList)
                          .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                          //.Where(p => p.CustomerId == int.Parse(CustomerList))

                          .OrderByDescending(x => x.PurReqId)
                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                            select new PurchaseReQuisitionResult
                            {
                                PurReqId = e.PurReqId,
                                PRNo = e.PRNo,
                                RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                                ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                                ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                                RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                                BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                                PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                                DeptName = e.Department != null ? e.Department.DeptName : "",
                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                Remarks = e.Remarks,
                                ReqRef = e.ReqRef,
                            };
            return querytest;
        }

        public IQueryable<PurchaseReQuisitionResult> QueryTestElse()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var querytest = from e in _context.PurchaseRequisitionMain
                         .Where(x => x.ComId == comid)
                         .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                         //.Where(p => p.userid == UserList)
                         // .Where(p => p.CustomerId == int.Parse(CustomerList))

                         .OrderByDescending(x => x.PurReqId)
                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                            select new PurchaseReQuisitionResult
                            {
                                PurReqId = e.PurReqId,
                                PRNo = e.PRNo,
                                RecommenedBy = e.RecommenedBy != null ? e.RecommenedBy.EmpName : "",
                                ApprovedBy = e.ApprovedBy != null ? e.ApprovedBy.EmpName : "",
                                ReqDate = e.ReqDate.ToString("dd-MMM-yy"),
                                RequiredDate = e.RequiredDate.ToString("dd-MMM-yy"),
                                BoardMeetingDate = e.BoardMeetingDate.ToString("dd-MMM-yy"),
                                PurposeName = e.Purpose != null ? e.Purpose.PurposeName : "",
                                DeptName = e.Department != null ? e.Department.DeptName : "",
                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                Remarks = e.Remarks,
                                ReqRef = e.ReqRef,
                            };

            return querytest;
        }

        public IEnumerable<SelectListItem> RecommendedEmpId(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid), "EmployeeId", "EmployeeName", purchaseRequisitionMain.RecommenedByEmpId);
        }

        public void SavePurchaseElse(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var AddDate = DateTime.Now;
            var DateUpdated = DateTime.Now;
            DateTime date = purchaseRequisitionMain.ReqDate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var activefiscalmonth = _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
            var activefiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
            var PcName = "";
            purchaseRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
            purchaseRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;

            purchaseRequisitionMain.ComId = comid;
            purchaseRequisitionMain.UserId = userid;
            purchaseRequisitionMain.DateAdded = AddDate;
            var main = new PurchaseRequisitionMain();
            main = purchaseRequisitionMain;
            var sl = 0;
            foreach (var item in purchaseRequisitionMain.PurchaseRequisitionSub)
            {
                sl++;
                var sub = new PurchaseRequisitionSub();
                sub.ComId = comid;
                sub.DateAdded = AddDate;
                sub.DateUpdated = item.DateUpdated;
                sub.Note = item.Note;
                sub.PcName = PcName;
                sub.ProductId = item.ProductId;
                sub.PurReqId = purchaseRequisitionMain.PurReqId;
                sub.PurReqQty = item.PurReqQty;
                sub.LastPurchasePrice = item.LastPurchasePrice;
                sub.RemainingReqQty = item.RemainingReqQty;
                sub.SLNo = sl;
                sub.UpdateByUserId = item.UpdateByUserId;
                sub.UserId = userid;
            }

            _context.PurchaseRequisitionMain.Add(main);
            _context.SaveChanges();

        }

        public IEnumerable<SelectListItem> SectId(PurchaseRequisitionMain purchaseRequisitionMain)
        {
            return new SelectList(_context.Cat_Section.Select(x => new { x.SectId, x.SectName }), "SectId", "SectName", purchaseRequisitionMain.SectId);
        }

        public void UpdateProduct(Models.Product product)
        {
            product.DateUpdated = DateTime.Now;
            product.comid = _httpContext.HttpContext.Session.GetString("comid");

            if (product.userid == null)
            {
                product.userid = _httpContext.HttpContext.Session.GetString("userid");
            }
            product.useridUpdate = _httpContext.HttpContext.Session.GetString("userid");

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void UpdatePurchase(PurchaseRequisitionMain model)
        {
            _context.Update(model);
            _context.SaveChanges();
        }
    }
}
