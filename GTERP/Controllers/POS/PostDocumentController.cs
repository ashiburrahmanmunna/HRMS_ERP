using DocumentFormat.OpenXml.InkML;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using QuickMailer;
using System.Reflection.Emit;
//using AlanJuden.MvcReportViewer;

namespace GTERP.Controllers.Account
{
    [OverridableAuthorize]
    public class PostDocumentController : Controller
    {
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db;
        //public CommercialRepository Repository { get; set; } ///for report service
        PermissionLevel PL;
        public PostDocumentController(GTRDBContext _db, PermissionLevel _pl)
        {
            db = _db;
            //Repository = repository; ///for report service
            PL = _pl;
        }

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

        // GET: Acc_VoucherMain
        public ViewResult Index(string FromDate, string ToDate, string criteria, string DocType, int DeptId, int PrdUnitId)
        {
            var transactioncomid = HttpContext.Session.GetString("comid");

            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            if (criteria == null)
            {
                criteria = "UnPost";
            }

            UserPermission permission = HttpContext.Session.GetObject<UserPermission>("userpermission");
            if (permission.IsProduction)
            {
                ViewData["DocType"] = new SelectList(DocTypeListProduction, "Value", "Text");
            }
            else
            {
                ViewData["DocType"] = new SelectList(DocTypeList, "Value", "Text");
            }

            ViewData["DeptId"] = new SelectList(db.Cat_Department, "DeptId", "DeptName");
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", PrdUnitId);


            if (FromDate == null || FromDate == "")
            {
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

            }
            if (ToDate == null || ToDate == "")
            {
            }
            else
            {
                dtTo = Convert.ToDateTime(ToDate);

            }

            List<DocumentList> doclist = new List<DocumentList>();
            DocumentList doc;

            ViewBag.Isleave = false;
            ViewBag.Title = criteria;
            var srrlist = db.StoreRequisitionMain.Take(0).ToList();
            var issuelist = db.IssueMain.Take(0).ToList();
            var prlist = db.PurchaseRequisitionMain.Take(0).ToList();
            var polist = db.PurchaseOrderMain.Take(0).ToList();
            var grrlist = db.GoodsReceiveMain.Take(0).ToList();

            var lvList = new List<HR_Leave_Avail>();

            string username = HttpContext.Session.GetString("username");
            int empId = db.HR_Emp_Info.Where(x=>x.EmpEmail ==username).Select(y=>y.EmpId).FirstOrDefault();  
            var firstaprv = db.HR_Emp_Info.Where(x => x.FirstAprvId == empId ).FirstOrDefault();
            var finalprv = db.HR_Emp_Info.Where(x => x.FinalAprvId == empId ).FirstOrDefault();
            if (DocType == "Leave")
            {
                ViewBag.Isleave = true;
                if (criteria == null)
                {
                    criteria = "Pending";
                }

                ViewBag.Title = criteria;  //Hossain Naim
                var query1 = db.HR_Leave_Avail
                        .Include(l => l.HR_Emp_Info)
                        .Include(l => l.Cat_Leave_Type)
                        .Where(p => p.ComId == transactioncomid &&
                        p.DtInput.Value.Date >= dtFrom.Date &&
                        p.DtInput.Value.Date <= dtTo.Date &&
                        (p.HR_Emp_Info.FirstAprvId == empId) && p.Status == 0);

                // Execute the first query and add the results to the list
                lvList.AddRange(query1);

                var query2 = db.HR_Leave_Avail
                        .Include(l => l.HR_Emp_Info)
                        .Include(l => l.Cat_Leave_Type)
                        .Where(p => p.ComId == transactioncomid &&
                        p.DtInput.Value.Date >= dtFrom.Date &&
                        p.DtInput.Value.Date <= dtTo.Date &&
                        (p.HR_Emp_Info.FinalAprvId == empId) && p.Status == 1 && p.IsApprove == false);

                lvList.AddRange(query2);

                DateTime currentDate = DateTime.Now;

                // Get the current year
                int currentYear = currentDate.Year;


                char[] spearator = { '!' };
                foreach (var item in lvList) //Hossain Naim
                {
                    doc = new DocumentList();

                    doc.DocumentId = item.LvId;

                    doc.DocumentNo = GetEmpCode(item.EmpId);
                    //doc.LTypeName = item.LTypeName;
                    //var weekNameFrom = item.DtFrom.ToString("ddd");
                    //var weekNameTo = item.DtTo.ToString("ddd");
                    //doc.DocumentDate = weekNameFrom + " - " + weekNameTo + "=" + item.TotalDay.ToString();
                    //doc.Remarks = item.TotalDay.ToString();
                    doc.DocumentDate = item.DtFrom.ToString("dd-MMM-yy") + "(" + item.DtFrom.ToString("ddd") + ")" + " - " + item.DtTo.ToString("dd-MMM-yy") + "(" + item.DtFrom.ToString("ddd") + ")" + "=" + item.TotalDay.ToString();

                    if (item.Cat_Leave_Type != null)
                    {
                        doc.DocumentType = item.Cat_Leave_Type.FullType;
                    }
                    else
                    {
                        doc.DocumentType = "";
                    }


                    doc.Remark = item.Remark;

                    if (item.FileName != null)
                    {
                        if (item.FileName.Length > 0)
                        {
                            String[] strlist = item.FileName.Split(spearator);
                            strlist = strlist.SkipLast(1).ToArray();
                            doc.Attachment = strlist[0];
                        }

                    }
                    else
                    {
                        doc.Attachment = "";
                    }

                    //doc.Attachment = item.FileName;

                    var balanceinAvail = (from emp in db.HR_Leave_Avail
                                          where emp.IsApprove == true && emp.ComId == transactioncomid && emp.LTypeId == item.LTypeId && emp.EmpId == item.EmpId
                                          select emp).Sum(x => x.TotalDay);

                    if (item.LTypeId == 1)
                    {
                        var data = db.HR_Leave_Balance.Where(x => x.ComId == transactioncomid && x.DtOpeningBalance == currentYear && x.EmpId == item.EmpId).Select(y => y.CL).FirstOrDefault();

                        doc.LeaveBalance = data - balanceinAvail;
                    }

                    if (item.LTypeId == 2)
                    {
                        var data = db.HR_Leave_Balance.Where(x => x.ComId == transactioncomid && x.DtOpeningBalance == currentYear && x.EmpId == item.EmpId).Select(y => y.SL).FirstOrDefault();

                        doc.LeaveBalance = data - balanceinAvail;
                    }

                    if (item.LTypeId == 4)
                    {
                        var data = db.HR_Leave_Balance.Where(x => x.ComId == transactioncomid && x.DtOpeningBalance == currentYear && x.EmpId == item.EmpId).Select(y => y.ML).FirstOrDefault();

                        doc.LeaveBalance = data - balanceinAvail;
                    }

                    if (item.LTypeId == 5)
                    {
                        var data = db.HR_Leave_Balance.Where(x => x.ComId == transactioncomid && x.DtOpeningBalance == currentYear && x.EmpId == item.EmpId).Select(y => y.LWP).FirstOrDefault();

                        doc.LeaveBalance = data - balanceinAvail;
                    }

                    if (item.LTypeId > 5)
                    {
                        doc.LeaveBalance = 0;
                    }

                    //doc.LeaveBalance = 0;



                    if (item.Status == 0) doc.DocumentStatus = "Pending";
                    else if (item.Status == 1)
                    {
                        if (item.IsApprove == false)
                        {
                            doc.DocumentStatus = "First Approved";
                        }
                        else
                        {
                            doc.DocumentStatus = "Approved";
                        }

                    }
                    else if (item.Status >= 2) doc.DocumentStatus = "DisApproved";

                    //doc.DeptId = item.HR_Emp_Info.Cat_Department.DeptName;

                    doclist.Add(doc);
                }
            }

            return View(doclist);
        }

        public string GetEmpCode(int EmpId)
        {
            var data = db.HR_Emp_Info.Where(x => x.EmpId == EmpId).FirstOrDefault();
            return data.EmpCode + "-" + data.EmpName;
        }


        public ActionResult Print(int? id, string type, string docname)
        {
            try
            {


                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                string SqlCmd = "";
                string ReportPath = "";
                var ConstrName = "ApplicationServices";
                var ReportType = "PDF";
                string DataSourceName = "DataSet1";
                string filename = "";
                var reportname = "";

                int empid = db.HR_Leave_Avail.Where(x => x.LvId == id).Select(x => x.EmpId).Single();
                DateTime dtleave = db.HR_Leave_Avail.Where(x => x.LvId == id).Select(x => x.DtFrom).Single();
                int EmpTypeId = (int)db.HR_Emp_Info.Where(x => x.EmpId == empid).Select(x => x.EmpTypeId).Single();

                if (EmpTypeId == 2)
                {
                    reportname = "rptLeaveForm";
                }
                else if (EmpTypeId == 3)
                {
                    reportname = "rptLeaveForm";
                }
                else
                {
                    reportname = "rptLeaveFormWorker";
                }
                //reportname = "rptLeaveFormWorker";
                HttpContext.Session.SetString("ReportPath", "~/ReportViewer/HR/" + reportname + ".rdlc");
                HttpContext.Session.SetString("reportquery", "Exec HR_rptLeaveForm '" + comid + "',  '" + dtleave + "',  '" + dtleave + "','" + empid + "'," +
                          " '0', '0',  '0','0','0','0','0','0','0', 'Leave Form', 'Leave Form','=ALL='");


                filename = db.HR_Emp_Info.Where(x => x.EmpId == empid).Select(x => x.EmpName).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();



                HttpContext.Session.SetObject("rptList", postData);



                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;




                SqlCmd = clsReport.strQueryMain;
                ReportPath = clsReport.strReportPathMain;
                ReportType = "PDF";

                //string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);
                //return Redirect(callBackUrl);



                string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = ReportType });//, new { id = 1 }
                return Redirect(redirectUrl);

                ///return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Obsolete]
        [ValidateAntiForgeryToken]
        public JsonResult SetProcess(string[] docid, string criteria, string[] doctype)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                //using (var tr = db.Database.BeginTransaction())
                //{
                string data = "";
                //t
                //{
                if (criteria.ToUpper().ToString() == "Post".ToUpper())
                {
                    if (docid.Count() > 0)
                    {
                        for (var i = 0; i < docid.Count(); i++)
                        {
                            string docidsingle = docid[i];
                            string doctypesingle = doctype[i];
                            Console.WriteLine(doctypesingle);

                             if (doctypesingle == "Leave")
                            {
                                var lvAvail = db.HR_Leave_Avail.Where(x => x.LvId == int.Parse(docidsingle)).FirstOrDefault();
                                var temp = db.HR_Emp_Info.Where (x=>x.EmpId == lvAvail.EmpId).FirstOrDefault();
                                if (lvAvail.Status == 0)
                                {
                                    lvAvail.Status = 1;
                                    if(temp.FirstAprvId != null && temp.FinalAprvId != null)
                                    {
                                        var temp1 = db.HR_Emp_Info.Where(x => x.EmpId == temp.FinalAprvId).FirstOrDefault();

                                        SendEmailForLeave(temp1.EmpEmail, false, true, false,false, lvAvail);
                                    }
                                }
                                else
                                {
                                    lvAvail.IsApprove = true;//
                                    SendEmailForLeave(temp.EmpEmail, true, false, true,false, lvAvail);
                                }
                                
                                db.Entry(lvAvail).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }
                    }
                }
                else if (criteria.ToUpper().ToString() == "UnPost".ToUpper())
                {
                    if (docid.Count() > 0)
                    {
                        for (var i = 0; i < docid.Count(); i++)
                        {
                            string docidsingle = docid[i];
                            string doctypesingle = doctype[i];


                           if (doctypesingle == "Leave")
                            {
                                string username = HttpContext.Session.GetString("username");
                                int empId = db.HR_Emp_Info.Where(x => x.EmpEmail == username).Select(y => y.EmpId).FirstOrDefault();
                                var lvAvail = db.HR_Leave_Avail.Where(x => x.LvId == int.Parse(docidsingle)).FirstOrDefault();

                                var firstaprv = db.HR_Emp_Info.Where(x => x.FirstAprvId == empId && x.EmpId == lvAvail.EmpId).FirstOrDefault();
                                var finalprv = db.HR_Emp_Info.Where(x => x.FinalAprvId == empId && x.EmpId == lvAvail.EmpId).FirstOrDefault();
                                if(firstaprv != null) {
                                    lvAvail.Status = 2;
                                    SendEmailForLeave(lvAvail.HR_Emp_Info.EmpEmail, false, false, false, true, lvAvail);

                                    var firstApproverMail = db.HR_Emp_Info.Where(x => x.EmpId == lvAvail.HR_Emp_Info.FirstAprvId).Select(y => y.EmpEmail).FirstOrDefault();
                                    SendEmailForLeave(firstApproverMail, true, true, false, true, lvAvail);
                                }
                                else
                                {
                                    lvAvail.Status = 3;
                                    SendEmailForLeave(lvAvail.HR_Emp_Info.EmpEmail, false, false, false, true, lvAvail);

                                    var firstApproverMail = db.HR_Emp_Info.Where(x => x.EmpId == lvAvail.HR_Emp_Info.FirstAprvId).Select(y => y.EmpEmail).FirstOrDefault();
                                    SendEmailForLeave(firstApproverMail, true, true, false, true, lvAvail);
                                }
                                
                                db.Entry(lvAvail).State = EntityState.Modified;
                            }
                            db.SaveChanges();


                        }
                    }

                }

                return Json(new { Success = "1", ex = "Leave Approved/Disapproved Successfully" });

                //}
            }
            catch (Exception ex)
            {
                return Json(new { Success = "3", ex = ex.Message });
                throw ex;

            }


        }

        public void SendEmailForLeave(string emailTo, bool IsApplicant, bool HOD, bool IsFirstApprvd,bool IsRejected, HR_Leave_Avail model)
        {
            var link = "<br>Please follow the link to proceed.<a href=\"https://gtrbd.net/ERP/PostDocument/Index?DocType=Leave&criteria=Pending\"> Click here to approve or disapprove</a>";
            var comid = HttpContext.Session.GetString("comid");
            string subject = "", body = "Dear ", senderAddress = "", host = "", userName = "", password = "", title = "";
            int port = 0;
            try
            {
                var leave = db.Cat_Leave_Type.Where(x => x.LTypeId == model.LTypeId).Select(y => y.LTypeName).FirstOrDefault();
                var leaveName = model.LvType + "[ " + leave + " ]";

                string[] substrings1 = model.DtLvInput.ToString().Split(' ');
                var dtinput = substrings1[0];

                string[] substrings2 = model.DtTo.ToString().Split(' ');
                var dtTo = substrings2[0];

                string[] substrings3 = model.DtFrom.ToString().Split(' ');
                var dtFrom = substrings3[0];

                string[] substrings4 = model.dtWork.ToString().Split(' ');
                var dtWork = substrings4[0];

                if (IsApplicant == true && IsFirstApprvd == true && IsRejected == false)
                {
                    var empName = db.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var mailData = db.Cat_MailSettings.Where(x => x.IsApplicant == true && x.IsFirstApprvd == true && x.IsRejected==false ).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>       :" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Reason</b>      :" + model.Remark;
                    title = mailData.CompanyTitle;
                    SendEmailForHR(empName, dtinput, leaveName, model.TotalDay, dtTo, dtFrom, model.Remark);
                }

                if (HOD == true && IsRejected == false)
                {
                    var empName = db.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var empCode = db.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpCode).FirstOrDefault();

                    var mailData = db.Cat_MailSettings.Where(x => x.IsHOD == true && x.IsRejected == false).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    if (model.LTypeId == 9)
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Working Date</b>      :" + dtWork + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    else
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    body += link;
                    title = mailData.CompanyTitle;
                }
                if (HOD == true && IsRejected == true)
                {
                    var empName = db.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var empCode = db.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpCode).FirstOrDefault();

                    var mailData = db.Cat_MailSettings.Where(x => x.IsHOD == true && x.IsRejected == true).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    if (model.LTypeId == 9)
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Working Date</b>      :" + dtWork + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    else
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    title = mailData.CompanyTitle;
                }
                if (IsApplicant == false && IsFirstApprvd == false && IsRejected == true)
                {
                    var empName = db.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var mailData = db.Cat_MailSettings.Where(x => x.IsApplicant == false && x.IsFirstApprvd == false && x.IsRejected == true).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    if (model.LTypeId == 9)
                    {
                        body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>       :" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Working Date</b>      :" + dtWork + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    else
                    {
                        body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>       :" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    title = mailData.CompanyTitle;
                }
                //emailTo = "sd05@gtrbd.com";
                var message = new MailMessage();
                message.From = new MailAddress(senderAddress, title);

                message.To.Add(new MailAddress(emailTo));


                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true; //true;

                using (var client = new SmtpClient())
                {
                    client.Host = host; //"smtp.gmail.com";
                    client.Port = port;//587;
                    client.EnableSsl = true;// true;
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

        public void SendEmailForHR(string Empname, string dtInput, string leaveName, float? totalDay, string dtTo,  string dtFrom, string Remark)
        {
            var comid = HttpContext.Session.GetString("comid");
            string subject = "", body = "Dear ", senderAddress = "", host = "", userName = "", password = "", title = "";
            int port = 0;
            try
            {
                var hrexists = db.HR_Emp_Info.Where(x => x.ComId == comid && x.IsHR == true).ToList();
                if(hrexists != null || hrexists.Count>0)
                {
                    foreach(var hr in hrexists)
                    {
                        var mailData = db.Cat_MailSettings.Where(x => x.ComId == comid && x.IsHR == true).FirstOrDefault();
                        subject = mailData.MailSubject;
                        senderAddress = mailData.SenderAddress;
                        host = mailData.Host;
                        port = mailData.Port;
                        userName = mailData.Username;
                        password = mailData.Password;
                     
                        body = body + "HR," + mailData.MailBody + dtInput + "<br><b>Applicant's Name<b>       :" + hr.EmpCode + "_" + Empname + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + totalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtTo + " To " + dtFrom + "<br><b>Reason</b>      :" + Remark;
                        title = mailData.CompanyTitle;
                        var message = new MailMessage();
                        message.From = new MailAddress(senderAddress, title);

                        //hr.EmpEmail = "Future03@gtrbd.com";
                        message.To.Add(new MailAddress(hr.EmpEmail));


                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true; //true;

                        using (var client = new SmtpClient())
                        {
                            client.Host = host; //"smtp.gmail.com";
                            client.Port = port;//587;
                            client.EnableSsl = true;// true;
                                                    //client.Credentials = new NetworkCredential(config.GetSection("CredentialMail").Value, config.GetSection("CredentialPassword").Value);
                            client.Credentials = new NetworkCredential(userName, password);
                            client.Send(message);
                        }
                    }
                    
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult create()
        {
            return View();
        }
        public string prcSaveData(Acc_VoucherMain model)
        {
            ArrayList arQuery = new ArrayList();

            try
            {
                var sqlQuery = "";
                // Count total Debit & Credit
                //foreach (var item in model.Collection)
                //{
                //    if (item.IsCheck == true)
                //    {
                //        sqlQuery = " Update tblAcc_Voucher_Main Set IsPosted = 1 ,LuserIdCheck = " + Session["Luserid"].ToString() + "   Where ComId = " + HttpContext.Session.GetString("comid").ToString() + " And docid = " + (item.docid) + "";
                //        arQuery.Add(sqlQuery);
                //    }
                //}
                //clsCon.GTRSaveDataWithSQLCommand(arQuery);
                return "Data Posted Successfuly";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            finally
            {
                //clsCon = null;
            }
        }

    }
}