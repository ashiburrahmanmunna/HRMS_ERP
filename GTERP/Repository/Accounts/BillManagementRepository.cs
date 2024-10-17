using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Accounts
{
    public class BillManagementRepository:BaseRepository<Bill_Main>, IBillManagementRepository
    {
        private readonly GTRDBContext db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public BillManagementRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) :base(context)
        {
            db = context;
            _httpContext = httpContext;
            _urlHelper= urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public Task <List<Bill_Main>> GetBillManagement()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            return db.Bill_Main.Include(p => p.Supplier).Include(p => p.Acc_ChartOfAccount).ToListAsync();
        }

        public IQueryable<PurchaseReQuisitionResult> Query1()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = from e in db.PurchaseRequisitionMain.Where(x => x.ComId == comid)
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

        public decimal? LastPurchasePrice(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            decimal? lastpurchaseprice;
            lastpurchaseprice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id && x.GoodsReceiveMain.Status > 0).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate).FirstOrDefault();
            return lastpurchaseprice;
        }

        public object Product(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var ProductData = db.Products.Include(x => x.vProductUnit).Where(x => x.comid == comid).Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName,
                p.UnitId,
                LastPurchasePrice = LastPurchasePrice(id)
            }).Where(p => p.ProductId == id).FirstOrDefault();// ToList();
            return ProductData;
        }

        public int SingleBillData()
        {
            return db.Bill_Main.OrderByDescending(b => b.BillMainId).FirstOrDefault()?.BillMainId ?? 0;
        }

        public void UpdateBillManagement(Bill_Main bill_Main)
        {
            bill_Main.DateUpdated = DateTime.Now;
            bill_Main.ComId ??= _httpContext.HttpContext.Session.GetString("comid");
            bill_Main.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            bill_Main.UserId ??= _httpContext.HttpContext.Session.GetString("userid");


            if (bill_Main.Bill_Subs != null)
            {
                foreach (var item in bill_Main.Bill_Subs)
                {
                    if (item.IsDelete)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                    else
                    {
                        if (item.BillSubId > 0)
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

            db.Entry(bill_Main).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void AddBillManagement(Bill_Main bill_Main)
        {
            bill_Main.UserId = _httpContext.HttpContext.Session.GetString("userid");
            bill_Main.ComId = (_httpContext.HttpContext.Session.GetString("comid"));
            bill_Main.DateAdded = DateTime.Now;

            //if (bill_Main.Bill_Subs != null)
            //{
            //    foreach (var item in bill_Main.Bill_Subs)
            //    {
            //        item.DateAdded = DateTime.Now;
            //    }
            //}

            bill_Main?.Bill_Subs?.ForEach(b => b.DateAdded = DateTime.Now);

            db.Bill_Main.Add(bill_Main);
            db.SaveChanges();
        }

        public string ProductName(int id)
        {
            return db.Products.Where(y => y.ProductId == id).Select(a => a.ProductName).SingleOrDefault();
        }

        public void DeletePrbBill(int prsubid)
        {
            var sub = db.PurchaseRequisitionSub.Find(prsubid);
            db.PurchaseRequisitionSub.Remove(sub);
            db.SaveChanges();
        }

        public Task<Bill_Main> Bill(int? id)
        {
            return db.Bill_Main
                 .Include(p => p.Bill_Subs).ThenInclude(p => p.Product)
                 .Include(p => p.Acc_ChartOfAccount)
                 .Where(p => p.BillMainId == id)
                 .FirstOrDefaultAsync();
        }

        public IEnumerable<SelectListItem> SupplierIdIf()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Suppliers.Where(x => x.ComId == comid).Select(s => new { Text = s.SupplierName, Value = s.SupplierId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> ProductIdIf()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> AccIdIf()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Acc_ChartOfAccounts.Where(x => x.ComId == comid && x.AccCode.Substring(0, 6) == "1-1-11").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> SupplierIdElse(Bill_Main Bill_Main)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Suppliers.Where(x => x.ComId == comid).Select(s => new { Text = s.SupplierName, Value = s.SupplierId }).ToList(), "Value", "Text", Bill_Main.SupplierId);
        }

        public IEnumerable<SelectListItem> ProductIdElse()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> AccIdElse(Bill_Main Bill_Main)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(db.Acc_ChartOfAccounts.Where(x => x.ComId == comid && x.AccCode.Substring(0, 6) == "1-1-11").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text", Bill_Main.AccId);
        }

        public string PrintBillManagement(int id)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");


            //filename = "Bill_" + DateTime.Now.Date;
            //query = "Exec rptBillManagement '" + Id + "', '" + comid + "'";

            var reportname = "rptBillManagement";
            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            _httpContext.HttpContext.Session.SetString("reportquery", "Exec [rptBillManagement] '" + id + "','" + comid + "'");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = "Bill_" + DateTime.Now.Date;
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            //var a = Session["PrintFileName"].ToString();


            string DataSourceName = "DataSet1";
            _httpContext.HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }

        public string SetSessionAccountReportBill(string rptFormat, string CostAlloMainId, string FiscalYearId, string FiscalMonthId)
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
                reportname = "rptCostAllocation";
                filename = "Notes_" + DateTime.Now.Date;
                query = "Exec Acc_rptCostAllocation '" + CostAlloMainId + "', '" + FiscalYearId + "' ,'" + FiscalMonthId + "','" + comid + "','" + userid + "'";


                _httpContext.HttpContext.Session.SetString("reportquery", query);
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            }

            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";

            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
            return redirectUrl;
        }

        public string GrrDetailsReport(string rptFormat, string action, string FromDate, string ToDate)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            if (action == "PrintVatTax")
            {
                reportname = "rptVatTaxBill";
                filename = "rptVatTaxBill" + DateTime.Now.Date.ToString();
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [rptBillListReport] '" + comid + "','" + FromDate + "','" + ToDate + "','VatTax'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }

            string DataSourceName = "DataSet1";

            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
            return redirectUrl;
        }

        public string PrintSDReport(string rptFormat, string action, string FromDate, string ToDate)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            if (action == "PrintSD")
            {
                reportname = "rptSDBill";
                filename = "rptSDBill" + DateTime.Now.Date.ToString();
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [rptBillListReport] '" + comid + "','" + FromDate + "','" + ToDate + "','SD'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }
            string DataSourceName = "DataSet1";
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
            return redirectUrl;
        }

        public string PrintWelfareReport(string rptFormat, string action, string FromDate, string ToDate)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            if (action == "PrintWelfare")
            {
                reportname = "rptSDBill";
                filename = "rptSDBill" + DateTime.Now.Date.ToString();
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [rptBillListReport] '" + comid + "','" + FromDate + "','" + ToDate + "','Welfare'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";


            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            //var ConstrName = "ApplicationServices";
            //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            //redirectUrl = callBackUrl;

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
            return redirectUrl;
        }
    }
}
