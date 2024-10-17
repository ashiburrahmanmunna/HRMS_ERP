using GTERP.BLL;
using GTERP.Interfaces.Technicals;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    //[OverridableAuthorize]
    public class TechnicalController : Controller
    {
        private readonly GTRDBContext _context;
        private readonly ITechnicalRepository _technicalRepository;
        private readonly TransactionLogRepository _tranlog;
        public TechnicalController(GTRDBContext context, ITechnicalRepository technicalRepository, TransactionLogRepository tranlog)
        {
            _context = context;
            _technicalRepository = technicalRepository;
            _tranlog = tranlog;
        }

        // GET: Technical
        public IActionResult TechnicalList()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            //var sectio
            var data = _technicalRepository.GetAll()
                .Include(m => m.Cat_Meeting)
                .Where(x => !x.IsDelete).ToList();
            return View(data);
        }

        // GET: Technical/Create
        public IActionResult CreateTechnical()
        {
            ViewBag.Title = "Create";
            ViewBag.MeetingId = _technicalRepository.MeetingId();
            ViewBag.MeetingType = _technicalRepository.MeetingType();
            return View(new Technical());
        }

        [HttpPost]
        public IActionResult CreateTechnical(Technical Technical)
        {
            if (ModelState.IsValid)
            {
                Technical.UserId = HttpContext.Session.GetString("userid");
                Technical.ComId = HttpContext.Session.GetString("comid");
                if (Technical.TechnicalId > 0)
                {
                    Technical.DateUpdated = DateTime.Today;
                    Technical.UpdateByUserId = HttpContext.Session.GetString("userid");
                    _technicalRepository.Update(Technical);

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    _tranlog.TransactionLog(RouteData.Values["controller"]
                            .ToString(), RouteData.Values["action"]
                            .ToString(), TempData["Message"]
                            .ToString(), Technical.TechnicalId
                            .ToString(), "Update", Technical.MeetingId.ToString());
                }
                else
                {
                    Technical.DateAdded = DateTime.Today;
                    _technicalRepository.Add(Technical);

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    _tranlog.TransactionLog(RouteData.Values["controller"]
                            .ToString(), RouteData.Values["action"]
                            .ToString(), TempData["Message"]
                            .ToString(), Technical.TechnicalId
                            .ToString(), "Create", Technical.MeetingId.ToString());

                }
                return RedirectToAction(nameof(TechnicalList));
            }
            return View(Technical);
        }

        // GET: Technical/Edit/5
        public IActionResult EditTechnical(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var Technical = _technicalRepository.FindById(id);
            ViewBag.MeetingId = _technicalRepository.MeetingId1(id);
            ViewBag.MeetingType = _technicalRepository.MeetingType1(id);
            if (Technical == null)
            {
                return NotFound();
            }
            return View("CreateTechnical", Technical);
        }

        // GET: Technical/Delete/5
        public IActionResult DeleteTechnical(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Technical = _technicalRepository.FindById(id);
            if (Technical == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            ViewBag.MeetingId = _technicalRepository.MeetingId1(id);
            ViewBag.MeetingType = _technicalRepository.MeetingType1(id);
            return View("CreateTechnical", Technical);
        }

        // POST: Technical/Delete/5
        [HttpPost, ActionName("DeleteTechnical")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteTechnicalConfirmed(int id)
        {
            try
            {
                var Technical = _technicalRepository.FindById(id);
                _technicalRepository.Delete(Technical);

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Technical.TechnicalId.ToString(), "Delete", Technical.MeetingId.ToString());
                return Json(new { Success = 1, TechnicalId = Technical.TechnicalId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        [HttpGet]
        public IActionResult GetMeeting(string meeting)
        {
            var data = _context.Cat_Meeting.Where(m => m.MeetingType == meeting)
                .Select(m => new
                {
                    MeetingId = m.MeetingId,
                    Meeting = m.Meeting
                }).ToList();
            return Json(data);
        }

        private bool TechnicalExists(int id)
        {
            var data = _context.Technical.Any(e => e.TechnicalId == id);
            return data;
        }

        public static List<SelectListItem> ReportTypes = new List<SelectListItem>()
        {
            new SelectListItem() {Text="Meeting", Value="Meeting"},
            new SelectListItem() { Text="Quality Test of Phosphoric Acid", Value="Import"},
            new SelectListItem() { Text="Waste Management", Value="Waste"},
            new SelectListItem() { Text="License Receive/ Renewal", Value="License"},
            new SelectListItem() { Text="Fire & Safety", Value="Fire"},
            new SelectListItem() { Text="Extinguisher", Value="Extinguisher"},
            new SelectListItem() { Text="Training", Value="Training"},
            new SelectListItem() { Text="Visit", Value="Visit"}
        };

        public IActionResult TechnicalReport()
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewData["ReportType"] = new SelectList(ReportTypes, "Value", "Text");
            return View();
        }

        [HttpGet]
        public IActionResult GetReport(string reportType, DateTime fromDate, DateTime toDate, string rptFormat)
        {
            try
            {
                //string comid = HttpContext.Session.GetString("comid");
                //var reportname = "";
                //var filename = "";
                //string redirectUrl = "";

                //if (reportType == "Meeting")
                //{
                //    reportname = "Meeting";
                //    filename = "rptMeeting_List_" + DateTime.Now.Date.ToString();
                //    var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}
                //else if (reportType == "Import")
                //{
                //    reportname = "Import";
                //    filename = "rptImport_List_" + DateTime.Now.Date.ToString();
                //    var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}
                //else if (reportType == "Waste")
                //{
                //    reportname = "Waste";
                //    filename = "rptWaste_List_" + DateTime.Now.Date.ToString();
                //    var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}
                //else if (reportType == "License")
                //{
                //    reportname = "License";
                //    filename = "rptLicense_List_" + DateTime.Now.Date.ToString();
                //    var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}
                //else if (reportType == "Fire")
                //{
                //    reportname = "Fire";
                //    filename = "rptFire_List_" + DateTime.Now.Date.ToString();
                //    var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}
                //else if (reportType == "Extinguisher")
                //{
                //    reportname = "Extinguisher";
                //    filename = "rptExtinguisher_List_" + DateTime.Now.Date.ToString();
                //    var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}
                //else if (reportType == "Training")
                //{
                //    reportname = "Training";
                //    filename = "rptTraining_List_" + DateTime.Now.Date.ToString();
                //    var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}
                //else if (reportType == "Visit")
                //{
                //    reportname = "Visit";
                //    filename = "rptVisit_List_" + DateTime.Now.Date.ToString(); var query = "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'";
                //    HttpContext.Session.SetString("reportquery", "Exec Tech_rptTechnical '" + comid + "','" + fromDate + "','" + toDate + "','" + reportname + "'");
                //    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Technical/" + "rpt" + reportname + ".rdlc");
                //    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                //}

                //string DataSourceName = "DataSet1";
                //GTERP.Models.Common.clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                //GTERP.Models.Common.clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                //GTERP.Models.Common.clsReport.strDSNMain = DataSourceName;

                _technicalRepository.GetReport(reportType, fromDate, toDate, rptFormat);
                var redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
                return Json(new { Url = redirectUrl });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}