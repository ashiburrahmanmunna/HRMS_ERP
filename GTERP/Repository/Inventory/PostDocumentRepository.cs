using GTERP.BLL;
using GTERP.Interfaces.Inventory;
using GTERP.Models;
using GTERP.Models.Common;
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

namespace GTERP.Repository.Inventory
{
    public class PostDocumentRepository:IPostDocumentRepository
    {
        #region Common Property
        private readonly GTRDBContext _context;
        PermissionLevel PL;
        private readonly IHttpContextAccessor _httpContext;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private readonly IUrlHelper _urlHelper;
        #endregion

        #region Constructor
        public PostDocumentRepository(
            GTRDBContext context, 
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            PermissionLevel pL
            )
        {
            _context = context;
            _httpContext = httpContext;
            PL = pL;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        #endregion
        public static List<SelectListItem> DocTypeList = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Store Requisition", Value="SRR"},
            new SelectListItem() { Text="Issue", Value="ISSUE"},
            new SelectListItem() { Text="Purchase Requisition", Value="PR"},
            new SelectListItem() { Text="Purchase Order", Value="PO"},
            new SelectListItem() { Text="Goods Receive", Value="GRR"}
        };

        /// <summary>
        /// only for create production module
        /// </summary>
        public static List<SelectListItem> DocTypeListProduction = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Store Requistion", Value="GRR"},
            new SelectListItem() { Text="Transfer", Value="SRR"},
            new SelectListItem() { Text="Issue", Value="ISSUE"},
            //new SelectListItem() { Text="Purchase Requisition", Value="PR"},
           // new SelectListItem() { Text="Purchase Order", Value="PO"},
            
        };

        public List<DocumentList> GetDocument(string FromDate, string ToDate, string criteria, string DocType, int DeptId, int PrdUnitId)
        {
            List<DocumentList> doclist = new List<DocumentList>();
            DocumentList doc;
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));

            var srrlist = _context.StoreRequisitionMain.Take(0).ToList();
            var issuelist = _context.IssueMain.Take(0).ToList();
            var prlist = _context.PurchaseRequisitionMain.Take(0).ToList();
            var polist = _context.PurchaseOrderMain.Take(0).ToList();
            var grrlist = _context.GoodsReceiveMain.Take(0).ToList();

            var lvList = new List<HR_Leave_Avail>();


            if (DocType == "SRR")
            {
                if (criteria == "All")
                {
                    srrlist = PL.GetSRR().Where(p => p.ReqDate >= dtFrom && p.ReqDate <= dtTo).ToList();
                }
                else if (criteria == "Post")
                {
                    srrlist = PL.GetSRR().Where(p => p.ReqDate >= dtFrom && p.ReqDate <= dtTo && p.Status > 0).OrderByDescending(o => o.StoreReqId).ToList();
                }
                else if (criteria == "UnPost")
                {
                    srrlist = PL.GetSRR().Where(p => p.ReqDate >= dtFrom && p.ReqDate <= dtTo && p.Status == 0).ToList();
                }



                foreach (var item in srrlist)
                {
                    doc = new DocumentList();
                    doc.DocumentId = item.StoreReqId;
                    doc.DocumentNo = item.SRNo;
                    doc.DocumentDate = item.ReqDate.ToString("dd-MMM-yy");
                    doc.DocumentType = "SRR";
                    doc.NetAmount = 0;
                    doc.Remarks = item.Remarks;
                    doc.DocumentStatus = item.Status.ToString() != "0" ? "Posted" : "Not Posted";

                    doclist.Add(doc);
                }
            }
            if (DocType == "ISSUE")
            {
                if (criteria == "All")
                {
                    issuelist = PL.GetIssue().Where(p => p.IssueDate >= dtFrom && p.IssueDate <= dtTo && p.PrdUnitId == PrdUnitId).ToList();
                }
                else if (criteria == "Post")
                {
                    issuelist = PL.GetIssue().Where(p => p.IssueDate >= dtFrom && p.IssueDate <= dtTo && p.PrdUnitId == PrdUnitId && p.Status > 0).OrderByDescending(o => o.IssueMainId).ToList();
                }
                else if (criteria == "UnPost")
                {
                    issuelist = PL.GetIssue().Where(p => p.IssueDate >= dtFrom && p.IssueDate <= dtTo && p.PrdUnitId == PrdUnitId && p.Status == 0).ToList();
                }



                foreach (var item in issuelist)
                {
                    doc = new DocumentList();
                    doc.DocumentId = item.IssueMainId;
                    doc.DocumentNo = "IN No : " + item.IssueNo + " SRR No : " + item.ManualSRRNo;
                    doc.DocumentDate = item.IssueDate.ToString("dd-MMM-yy");
                    doc.DocumentType = "ISSUE";
                    doc.NetAmount = (decimal)item.NetIssueValue;
                    doc.Remarks = item.Remarks;
                    doc.DocumentStatus = item.Status.ToString() != "0" ? "Posted" : "Not Posted";

                    doclist.Add(doc);
                }
            }
            if (DocType == "PR")
            {
                if (criteria == "All")
                {
                    prlist = PL.GetPR().Where(p => p.ReqDate >= dtFrom && p.ReqDate <= dtTo).ToList();
                }
                else if (criteria == "Post")
                {
                    prlist = PL.GetPR().Where(p => p.ReqDate >= dtFrom && p.ReqDate <= dtTo && p.Status > 0).OrderByDescending(o => o.PurReqId).ToList();
                }
                else if (criteria == "UnPost")
                {
                    prlist = PL.GetPR().Where(p => p.ReqDate >= dtFrom && p.ReqDate <= dtTo && p.Status == 0).ToList();
                }

                foreach (var item in prlist)
                {
                    doc = new DocumentList();
                    doc.DocumentId = item.PurReqId;
                    doc.DocumentNo = item.PRNo;
                    doc.DocumentDate = item.ReqDate.ToString("dd-MMM-yy");
                    doc.DocumentType = "PR";
                    doc.NetAmount = 0;
                    doc.Remarks = item.Remarks;
                    doc.DocumentStatus = item.Status.ToString() != "0" ? "Posted" : "Not Posted";

                    doclist.Add(doc);
                }
            }
            if (DocType == "PO")
            {
                if (criteria == "All")
                {
                    polist = PL.GetPO().Where(p => p.PODate >= dtFrom && p.PODate <= dtTo).ToList();
                }
                else if (criteria == "Post")
                {
                    polist = PL.GetPO().Where(p => p.PODate >= dtFrom && p.PODate <= dtTo && p.Status > 0).OrderByDescending(o => o.PurOrderMainId).ToList();
                }
                else if (criteria == "UnPost")
                {
                    polist = _context.PurchaseOrderMain.Where(p => p.PODate >= dtFrom && p.PODate <= dtTo && p.Status == 0).ToList();
                }

                foreach (var item in polist)
                {
                    doc = new DocumentList();
                    doc.DocumentId = item.PurOrderMainId;
                    doc.DocumentNo = item.PONo;
                    doc.DocumentDate = item.PODate.ToString("dd-MMM-yy");
                    doc.DocumentType = "PO";
                    doc.NetAmount = (decimal)item.TotalPOValue;
                    doc.Remarks = item.Remarks;
                    doc.DocumentStatus = item.Status.ToString() != "0" ? "Posted" : "Not Posted";

                    doclist.Add(doc);
                }
            }
            if (DocType == "GRR")
            {
                if (criteria == "All")
                {
                    grrlist = PL.GetGRR().Where(p => p.GRRDate >= dtFrom && p.GRRDate <= dtTo).ToList();
                }
                else if (criteria == "Post")
                {
                    grrlist = PL.GetGRR().Where(p => p.GRRDate >= dtFrom && p.GRRDate <= dtTo && p.Status > 0).OrderByDescending(o => o.GRRMainId).ToList();
                }
                else if (criteria == "UnPost")
                {
                    grrlist = PL.GetGRR().Where(p => p.GRRDate >= dtFrom && p.GRRDate <= dtTo && p.Status == 0).ToList();
                }


                foreach (var item in grrlist)
                {
                    doc = new DocumentList();
                    doc.DocumentId = item.GRRMainId;
                    doc.DocumentNo = item.GRRNo;
                    doc.DocumentDate = item.GRRDate.ToString("dd-MMM-yy");
                    doc.DocumentType = "GRR";
                    doc.NetAmount = (decimal)item.NetGRRValue;
                    doc.Remarks = item.Remarks;
                    doc.DocumentStatus = item.Status.ToString() != "0" ? "Posted" : "Not Posted";

                    doclist.Add(doc);
                }
                
            }
            return doclist;

        }

        public IEnumerable<SelectListItem> DocTypeListProductionList1()
        {
            return new SelectList(DocTypeListProduction, "Value", "Text");
        }

        public IEnumerable<SelectListItem> DocTypeList1()
        {
            return new SelectList(DocTypeList, "Value", "Text");
        }

        public void Leave()
        {
            List<DocumentList> doclist = new List<DocumentList>();
            DocumentList doc;
            var lvList = new List<HR_Leave_Avail>();
            foreach (var item in lvList)
            {
                doc = new DocumentList();
                doc.DocumentId = item.LvId;
                doc.DocumentNo = item.HR_Emp_Info != null ? item.HR_Emp_Info.EmpCode + "-" + item.HR_Emp_Info.EmpName : "";
                doc.DocumentDate = item.DtFrom.ToString("dd-MMM-yy") + " - " + item.DtTo.ToString("dd-MMM-yy");
                doc.DocumentType = item.LvType;
                doc.Remarks = item.TotalDay.ToString();

                if (item.Status == 0) doc.DocumentStatus = "Pending";
                else if (item.Status == 1) doc.DocumentStatus = "Approved";
                else if (item.Status == 2) doc.DocumentStatus = "DisApproved";

                //doc.DeptId = item.HR_Emp_Info.Cat_Department.DeptName;

                doclist.Add(doc);
            }
        }

        public List<HR_Leave_Avail> Leave1(int DeptId)
        {
            var transactioncomid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            return _context.HR_Leave_Avail.Include(l => l.HR_Emp_Info).ThenInclude(l => l.Cat_Department).Where(p => p.ComId == transactioncomid && p.DtInput.Value.Date >= dtFrom.Date &&
                         p.DtInput.Value.Date <= dtTo.Date && p.HR_Emp_Info.DeptId == DeptId).ToList();
        }

        public List<HR_Leave_Avail> Leave2(int DeptId)
        {
            var transactioncomid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            return _context.HR_Leave_Avail.Include(l => l.HR_Emp_Info).ThenInclude(l => l.Cat_Department).Where(p => p.ComId == transactioncomid && p.DtInput.Value.Date >= dtFrom.Date &&
                    p.DtInput.Value.Date <= dtTo.Date && p.HR_Emp_Info.DeptId == DeptId && p.Status == 0).ToList();
        }

        public List<HR_Leave_Avail> Leave3(int DeptId)
        {
            var transactioncomid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            return _context.HR_Leave_Avail.Include(l => l.HR_Emp_Info).Include(l => l.HR_Emp_Info.Cat_Department).Where(p => p.ComId == transactioncomid && (p.DtInput.Value.Date >= dtFrom.Date &&
                    p.DtInput.Value.Date <= dtTo.Date) && p.HR_Emp_Info.DeptId == DeptId && p.Status == 1).ToList();
        }

        public List<HR_Leave_Avail> Leave4(int DeptId)
        {
            var transactioncomid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            return _context.HR_Leave_Avail.Include(l => l.HR_Emp_Info).Include(l => l.HR_Emp_Info.Cat_Department).Where(p => p.ComId == transactioncomid && (p.DtInput.Value.Date >= dtFrom.Date &&
                    p.DtInput.Value.Date <= dtTo.Date) && p.HR_Emp_Info.DeptId == DeptId && p.Status == 2).ToList();
        }

        public string Print(int? id, string type, string docname)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            string DataSourceName = "DataSet1";
            string filename = "";
            var reportname = "";
            

            if (docname.ToUpper() == "Issue".ToUpper())
            {
                reportname = "rptSRForm";
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndIssueDetails] '" + comid + "', 'ISSUENW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

                filename = _context.IssueMain.Where(x => x.IssueMainId == id).Select(x => x.IssueNo).Single();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();



                _httpContext.HttpContext.Session.SetObject("rptList", postData);
            }
            else if (docname.ToUpper() == "Store Requisition".ToUpper())
            {
                reportname = "rptSRForm";
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndIssueDetails] '" + comid + "', 'ISSUENW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

                filename = _context.IssueMain.Where(x => x.IssueMainId == id).Select(x => x.IssueNo).Single();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();



                _httpContext.HttpContext.Session.SetObject("rptList", postData);
            }
            else if (docname.ToUpper() == "Purchase Requisition".ToUpper())
            {
                reportname = "rptSRForm";
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndIssueDetails] '" + comid + "', 'ISSUENW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

                filename = _context.IssueMain.Where(x => x.IssueMainId == id).Select(x => x.IssueNo).Single();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();



                _httpContext.HttpContext.Session.SetObject("rptList", postData);
            }
            else if (docname.ToUpper() == "Purchase Order".ToUpper())
            {
                reportname = "rptSRForm";
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndIssueDetails] '" + comid + "', 'ISSUENW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

                filename = _context.IssueMain.Where(x => x.IssueMainId == id).Select(x => x.IssueNo).Single();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();



                _httpContext.HttpContext.Session.SetObject("rptList", postData);
            }
            else if (docname.ToUpper() == "Goods Receive".ToUpper())
            {
                reportname = "rptMRRForm";
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndGRRDetails] '" + comid + "', 'ISSUENW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

                filename = _context.IssueMain.Where(x => x.IssueMainId == id).Select(x => x.IssueNo).Single();
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();



                _httpContext.HttpContext.Session.SetObject("rptList", postData);
            }

            //else if (docname == "")
            //{

            //    reportname = "rptShowVoucher";


            //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
            //    var str = _context.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            //    var Currency = "1";
            //    HttpContext.Session.SetString("reportquery", "Exec Acc_rptVoucher 0, 'VID','All','dapa26-414a-44e4-a287-18e846b51d99', '01-Jan-1900', '01-Jan-1900', '" + str + "','" + str + "', " + id + ", " + Currency + ", 0");


            //    //Session["reportquery"] = "Exec " + AppData.AppData._contextGTCommercial.ToString() + "._contexto.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            //    filename = _context.StoreRequisitionMain.Where(x => x.StoreReqId == id).Select(x => x.SRNo + "_" + x.ReqRef).Single();
            //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            //    //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData._contextGTCommercial.ToString() + "._contexto.rptInvoice_Terms '" + id + "','" +HttpContext.Session.GetString("comid"); + "',''"));

            //    HttpContext.Session.SetObject("rptList", postData);
            //}




            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            //string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);
            //return Redirect(callBackUrl);



            string redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });//, new { id = 1 }
            return redirectUrl;

        }
    }
}
