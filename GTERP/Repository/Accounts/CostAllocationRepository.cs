using GTERP.Interfaces.Accounts;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTERP.ViewModels;
using GTERP.Repository.Base;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GTERP.Repository.Accounts
{
    public class CostAllocationRepository : BaseRepository<CostAllocation_Main>, ICostAllocationRepository
    {
        private readonly GTRDBContext db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        public CostAllocationRepository(
            GTRDBContext context, 
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) :base(context)
        {
            db = context;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public IEnumerable<SelectListItem> CostAlloMainId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.CostAllocation_Main.Where(x => x.ComId == comid).Select(s => new { Text = s.Name, Value = s.CostAlloMainId }).ToList(), "Value", "Text");
        }

        public void UpdateCostAllocation(CostAllocation_Main CostAllocation_Main)
        {
            CostAllocation_Main.DateUpdated = DateTime.Now;
            CostAllocation_Main.ComId = _httpContext.HttpContext.Session.GetString("comid");
            CostAllocation_Main.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            CostAllocation_Main.DateUpdated = DateTime.Now;

            if (CostAllocation_Main.UserId == null)
            {
                CostAllocation_Main.UserId = _httpContext.HttpContext.Session.GetString("userid");
            }
            CostAllocation_Main.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");


            if (CostAllocation_Main.CostAllocation_Detailses != null)
            {
                foreach (var item in CostAllocation_Main.CostAllocation_Detailses)
                {
                    if (item.IsDelete)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                    else
                    {

                        if (item.CostAlloSubId > 0)
                        {
                            item.DateUpdated = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            item.DateAdded = DateTime.Now;
                            db.Entry(item).State = EntityState.Added;
                        }
                    }

                }
            }
            if (CostAllocation_Main.CostAllocation_Distributes != null)
            {
                foreach (var item in CostAllocation_Main.CostAllocation_Distributes)
                {
                    if (item.IsDelete)
                    {
                        db.Entry(item).State = EntityState.Deleted;

                    }
                    else
                    {
                        if (item.CostAlloDistributeId > 0)
                        {
                            item.DateUpdated = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            item.DateAdded = DateTime.Now;
                            db.Entry(item).State = EntityState.Added;
                        }

                    }

                }
            }
            db.Entry(CostAllocation_Main).State = EntityState.Modified;
            db.SaveChanges();

        }

        public Acc_FiscalMonth FiscalMonth()
        {
            return db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> FiscalMonthId()
        {
            return new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", FiscalMonth().FiscalMonthId);
        }

        public IEnumerable<SelectListItem> FiscalMonthIdElse()
        {
            return new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");
        }

        public Acc_FiscalYear FiscalYear()
        {
            return db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> FiscalYearId()
        {
            return new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", FiscalYear().FiscalYearId);
        }

        public IEnumerable<SelectListItem> FiscalYearIdElse()
        {
            return new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
        }

        public Task<List<CostAllocation_Main>> GetCostAllocation()
        {
            return db.CostAllocation_Main.Include(p => p.Acc_FiscalYear).Include(p => p.Acc_FiscalMonth).ToListAsync();
        }

        public decimal LastPurchasePrice(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain)
                .Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id && x.GoodsReceiveMain.Status > 0)
                .OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate)
                .Take(1).Select(x => x.Rate)
                .FirstOrDefault();
        }

        public object Product(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = db.Products.Include(x => x.vProductUnit).Where(x => x.comid == comid).Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName,
                p.UnitId,
                LastPurchasePrice = LastPurchasePrice(id)
            }).Where(p => p.ProductId == id).FirstOrDefault();
            return data;
        }

        public IQueryable<PurchaseReQuisitionResult> Query1()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var query = from e in db.PurchaseRequisitionMain.Where(x => x.ComId == comid)
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

        public IQueryable<PurchaseReQuisitionResult> Query2(string UserList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            var querytest = from e in db.PurchaseRequisitionMain
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

        public IQueryable<PurchaseReQuisitionResult> Query3()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            var data = from e in db.PurchaseRequisitionMain
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
            return data;

        }

        public void CreateCostAllocation(CostAllocation_Main CostAllocation_Main)
        {
            CostAllocation_Main.UserId = _httpContext.HttpContext.Session.GetString("userid");
            CostAllocation_Main.ComId = (_httpContext.HttpContext.Session.GetString("comid"));
            CostAllocation_Main.DateAdded = DateTime.Now;

            if (CostAllocation_Main.CostAllocation_Detailses != null)
            {
                foreach (var item in CostAllocation_Main.CostAllocation_Detailses)
                {
                    item.DateAdded = DateTime.Now;
                }
            }
            if (CostAllocation_Main.CostAllocation_Distributes != null)
            {
                foreach (var item in CostAllocation_Main.CostAllocation_Distributes)
                {
                    item.DateAdded = DateTime.Now;
                }
            }

            db.CostAllocation_Main.Add(CostAllocation_Main);
            db.SaveChanges();
        }

        public Product Prod(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return db.Products.Where(y => y.ProductId == id && y.comid == comid).SingleOrDefault();
        }

        public Unit Unit(int id)
        {
            return db.Unit.Where(y => y.UnitId == Prod(id).UnitId).SingleOrDefault();
        }

        public IEnumerable<SelectListItem> ProductList1()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> ProductList2(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> ProductList3()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public void DeletePrSub(int prsubid)
        {
            var sub = db.PurchaseRequisitionSub.Find(prsubid);
            db.PurchaseRequisitionSub.Remove(sub);
            db.SaveChanges();
        }

        public string SetSessionAccountReport(string rptFormat, string CostAlloMainId, string FiscalYearId, string FiscalMonthId)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            string query = "";
            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (true)
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rptCostAllocation";
                filename = "Notes_" + DateTime.Now.Date;
                query = "Exec Acc_rptCostAllocation '" + CostAlloMainId + "', '" + FiscalYearId + "' ,'" + FiscalMonthId + "','" + comid + "','" + userid + "'";


                _httpContext.HttpContext.Session.SetString("reportquery", query);
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            }



            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";

            //HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
            return redirectUrl;
        }

        public IEnumerable<SelectListItem> FiscalYearIdCost(CostAllocation_Main CostAllocation_Main)
        {
            return new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", CostAllocation_Main.FiscalYearId);
        }

        public IEnumerable<SelectListItem> FiscalMonthIdCost(CostAllocation_Main CostAllocation_Main)
        {
            return new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", CostAllocation_Main.FiscalMonthId);
        }

        public IEnumerable<SelectListItem> AccId()
        {
           var comid = _httpContext.HttpContext.Session.GetString("comid");
           return new SelectList(db.Acc_ChartOfAccounts.Where(x => x.ComId == comid).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public Task<CostAllocation_Main> CostAllocation(int? id)
        {

            var CostAllocation_Main = db.CostAllocation_Main
                .Include(p => p.CostAllocation_Detailses)
                .ThenInclude(p => p.Acc_ChartOfAccount)
                .Include(p => p.CostAllocation_Distributes)
                .ThenInclude(p => p.Acc_ChartOfAccount)
                .Include(p => p.Acc_FiscalYear).Include(p => p.Acc_FiscalMonth)
                .Where(p => p.CostAlloMainId == id)
                .FirstOrDefaultAsync();
            return CostAllocation_Main;
        }
    }
}
