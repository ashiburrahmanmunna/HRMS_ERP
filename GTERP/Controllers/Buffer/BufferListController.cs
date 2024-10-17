using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Buffers;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.Buffer
{
    public class BufferListController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public BufferListController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        // GET: Designation
        public async Task<IActionResult> Index()
        {
            var ComId = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            var data = db.Buffers.Include(d => d.BufferRepresentativeList).Include(b => b.DistictBuffer).Where(x => x.ComId == ComId);
            return View(await data.ToListAsync());
        }



        // GET: BufferList/Create
        public IActionResult Create(int? Id)
        {
            var comId = HttpContext.Session.GetString("comid");

            // ViewData["RepresentativeId"] = new SelectList(db.BufferRepresentative, "RepresentativeId", "RepresentativeName");
            BufferVm model = new BufferVm();
            List<int> districtIds = new List<int>();
            List<int> bufferRepIds = new List<int>();
            if (Id.HasValue)
            {
                ViewBag.Title = "Edit";
                //Get Buffer   
                var buffer = db.Buffers.Include(r => r.BufferRepresentativeList).Include(d => d.DistictBuffer).FirstOrDefault(x => x.BufferListId == Id.Value);

                buffer.DistictBuffer.ToList().ForEach(result => districtIds.Add(result.DistId));
                buffer.BufferRepresentativeList.ToList().ForEach(result => bufferRepIds.Add(result.BufferRepresentativeId));
                model = new BufferVm();
                model.drpDistricts = db.Cat_District.Select(x => new SelectListItem { Text = x.DistName, Value = x.DistId.ToString() }).ToList();
                model.drpBufferRep = db.BuferRepresentative.Select(x => new SelectListItem { Text = x.RepresentativeName, Value = x.BufferRepresentativeId.ToString() }).ToList();
                model.BufferListId = buffer.BufferListId;
                model.BufferName = buffer.BufferName;
                model.ComId = comId;

                model.BufferCode = buffer.BufferCode;
                model.BufferNameBangla = buffer.BufferNameBangla;
                model.bufferRepId = bufferRepIds.ToArray();
                model.districtId = districtIds.ToArray();
                return View("Create", model);

            }
            else
            {
                ViewBag.Title = "Create";
                model = new BufferVm();
                model.drpDistricts = db.Cat_District.Select(x => new SelectListItem { Text = x.DistName, Value = x.DistId.ToString() }).ToList();
                model.drpBufferRep = db.BuferRepresentative.Select(x => new SelectListItem { Text = x.RepresentativeName, Value = x.BufferRepresentativeId.ToString() }).ToList();
                return View(model);
            }






        }








        // POST: BufferList/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BufferVm model)
        {
            var comId = HttpContext.Session.GetString("comid");
            BufferList buffers = new BufferList();
            List<RepresentativeBuffer> RepresentativeBuffer = new List<RepresentativeBuffer>();
            List<DistictBuffer> DistictBuffer = new List<DistictBuffer>();

            if (model.BufferListId > 0)
            {
                //first find teacher subjects list and then remove all from db   
                var buffer = db.Buffers.Include(r => r.BufferRepresentativeList).Include(d => d.DistictBuffer).FirstOrDefault(x => x.BufferListId == model.BufferListId);
                buffer.DistictBuffer.ToList().ForEach(result => DistictBuffer.Add(result));
                buffer.BufferRepresentativeList.ToList().ForEach(result => RepresentativeBuffer.Add(result));



                db.BufferDisticts.RemoveRange(DistictBuffer);
                db.RepresentativeBuffers.RemoveRange(RepresentativeBuffer);
                db.SaveChanges();

                //Now update teacher details  
                buffers.BufferListId = model.BufferListId;
                buffers.ComId = comId;
                buffers.BufferCode = model.BufferCode;
                buffers.BufferName = model.BufferName;
                buffers.BufferNameBangla = model.BufferNameBangla;

                if (model.bufferRepId.Length > 0)
                {
                    RepresentativeBuffer = new List<RepresentativeBuffer>();

                    foreach (var bufferRepId in model.bufferRepId)
                    {
                        RepresentativeBuffer.Add(new RepresentativeBuffer { BufferRepresentativeId = bufferRepId, BufferListId = model.BufferListId });
                    }
                    buffers.BufferRepresentativeList = RepresentativeBuffer;
                }
                if (model.districtId.Length > 0)
                {
                    DistictBuffer = new List<DistictBuffer>();

                    foreach (var districtId in model.districtId)
                    {
                        DistictBuffer.Add(new DistictBuffer { DistId = districtId, BufferListId = model.BufferListId });
                    }
                    buffers.DistictBuffer = DistictBuffer;
                }
                var existing = db.Buffers.Local.SingleOrDefault(o => o.BufferListId == model.BufferListId);
                if (existing != null)
                    db.Entry(existing).State = EntityState.Detached;

                db.Update(buffers);
                await db.SaveChangesAsync();





            }
            else
            {
                buffers.BufferListId = model.BufferListId;
                buffers.ComId = comId;
                buffers.BufferCode = model.BufferCode;
                buffers.BufferName = model.BufferName;
                buffers.BufferNameBangla = model.BufferNameBangla;
                if (model.bufferRepId.Length > 0)
                {
                    RepresentativeBuffer = new List<RepresentativeBuffer>();

                    foreach (var bufferRepId in model.bufferRepId)
                    {
                        RepresentativeBuffer.Add(new RepresentativeBuffer { BufferRepresentativeId = bufferRepId, BufferListId = model.BufferListId });
                    }
                    buffers.BufferRepresentativeList = RepresentativeBuffer;
                }
                if (model.districtId.Length > 0)
                {
                    DistictBuffer = new List<DistictBuffer>();

                    foreach (var districtId in model.districtId)
                    {
                        DistictBuffer.Add(new DistictBuffer { DistId = districtId, BufferListId = model.BufferListId });
                    }
                    buffers.DistictBuffer = DistictBuffer;
                }

                db.Buffers.Add(buffers);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }



        public async Task<IActionResult> Delete(int? id)
        {

            ViewBag.Title = "Delete";

            if (id == null)
            {
                return NotFound();
            }

            var comId = HttpContext.Session.GetString("comid");

            // ViewData["RepresentativeId"] = new SelectList(db.BufferRepresentative, "RepresentativeId", "RepresentativeName");
            BufferVm model = new BufferVm();
            List<int> districtIds = new List<int>();
            List<int> bufferRepIds = new List<int>();

            var buffer = db.Buffers.Include(r => r.BufferRepresentativeList).Include(d => d.DistictBuffer).FirstOrDefault(x => x.BufferListId == id.Value);

            buffer.DistictBuffer.ToList().ForEach(result => districtIds.Add(result.DistId));
            buffer.BufferRepresentativeList.ToList().ForEach(result => bufferRepIds.Add(result.BufferRepresentativeId));
            model = new BufferVm();
            model.drpDistricts = db.Cat_District.Select(x => new SelectListItem { Text = x.DistName, Value = x.DistId.ToString() }).ToList();
            model.drpBufferRep = db.BuferRepresentative.Select(x => new SelectListItem { Text = x.RepresentativeName, Value = x.BufferRepresentativeId.ToString() }).ToList();
            model.BufferListId = buffer.BufferListId;
            model.BufferName = buffer.BufferName;
            model.ComId = comId;

            model.BufferCode = buffer.BufferCode;
            model.BufferNameBangla = buffer.BufferNameBangla;
            model.bufferRepId = bufferRepIds.ToArray();
            model.districtId = districtIds.ToArray();
            return View("Create", model);
        }



        // GET: BufferList/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            try
            {
                var BufferList = await db.Buffers.FindAsync(id);
                db.Buffers.Remove(BufferList);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), BufferList.BufferListId.ToString(), "Delete", BufferList.BufferName);

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, BufferId = BufferList.BufferListId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        private bool BufferExists(int id)
        {
            return db.Buffers.Any(e => e.BufferListId == id);
        }

        [HttpPost, ActionName("BufferListReport")]
        public JsonResult BufferListReport(string rptFormat, string action, string DistrictId, string FromDate, string ToDate, string MonthId, string YearId, int? FromDOId, int? ToDOId, string Bank, int? GatePassId, int? DONo, int? BookingMonthId, string ReceiverPerson, string Type, string FromNo, string ToNo, int? RepresentativeId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                if (action == "PrintBufferWiseSDeallerList")
                {
                    reportname = "rptDistrictWiseDailySales";
                    filename = "rptDistrictWiseDailySales_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptGetDistrictWiseDailySaleReport '" + comid + "','" + DistrictId + "','" + FromDate + "','" + ToDate + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptGetDistrictWiseDailySaleReport '" + comid + "','" + DistrictId + "','" + FromDate + "','" + ToDate + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintNameOfBuffer")
                {
                    reportname = "rptDeliveryOrderDateWise";
                    filename = "rptDeliveryOrderDateWiseSales_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptDeliveryOrderDateWiseReport '" + comid + "','" + FromDate + "','" + ToDate + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptDeliveryOrderDateWiseReport '" + comid + "','" + FromDate + "','" + ToDate + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintDeliveryOrderNumberWiseReport")
                {
                    reportname = "rptDeliveryOrderNumberWiseReport";
                    filename = "rptDeliveryOrderNumberWiseReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptDeliveryOrderNumberWiseReport '" + comid + "','" + FromDOId + "','" + ToDOId + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptDeliveryOrderNumberWiseReport '" + comid + "','" + FromDOId + "','" + ToDOId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintBankDipositrDateWiseReport")
                {
                    reportname = "rptBankDipositDateWiseReport";
                    filename = "rptBankDipositDateWiseReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptBankDipositDateWiseReport '" + comid + "','" + FromDate + "','" + ToDate + "','" + Bank + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptBankDipositDateWiseReport '" + comid + "','" + FromDate + "','" + ToDate + "','" + Bank + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintDeliveryOrderNumberWiseReport")
                {
                    reportname = "rptDeliveryOrderNumberWiseReport";
                    filename = "rptDeliveryOrderNumberWiseReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptDeliveryOrderNumberWiseReport '" + comid + "','" + YearId + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptDeliveryOrderNumberWiseReport '" + comid + "','" + YearId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintDailyDateWiseSalesSummaryReport")
                {
                    reportname = "rptDailyDateWiseSalesSummaryReport";
                    filename = "rptDailyDateWiseSalesSummaryReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptDailyDateWiseSalesSummaryReport '" + comid + "','" + FromDate + "','" + ToDate + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptDailyDateWiseSalesSummaryReport '" + comid + "','" + FromDate + "','" + ToDate + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintAnnualSalesReport")
                {
                    reportname = "rptAnnualSalesReport";
                    filename = "rptAnnualSalesReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptAnnualSalesReport '" + comid + "','" + YearId + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptAnnualSalesReport '" + comid + "','" + YearId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintDeliveryOrderWiseChallanReport")
                {
                    reportname = "rptDeliveryOrderWiseChallanReport";
                    filename = "rptDeliveryOrderWiseChallanReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptDeliveryOrderWiseChallanReport '" + comid + "','" + FromDOId + "','" + ToDOId + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptDeliveryOrderWiseChallanReport '" + comid + "','" + FromDOId + "','" + ToDOId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptDeliveryDateWiseBankSummaryReport")
                {
                    reportname = "rptDeliveryDateWiseBankSummary";
                    filename = "rptDeliveryDateWiseBankSummaryReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec Sales_rptDeliveryDateWiseBankSummaryReport '" + comid + "','" + FromDate + "','" + ToDate + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptDeliveryDateWiseBankSummaryReport '" + comid + "','" + FromDate + "','" + ToDate + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }

                else if (action == "PrintrptDeliveryChallanDateWiseBankSummaryReport")
                {
                    reportname = "rptIDeliveryChallanSummary";
                    filename = "rptIDeliveryChallanSummary_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptDeliveryChallanSummary] '" + comid + "','" + FromDate + "','" + ToDate + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDeliveryChallanSummary] '" + comid + "','" + FromDate + "','" + ToDate + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }


                else if (action == "PrintrptBankDipositMonthWiseSummaryReport")
                {
                    reportname = "rptBankDipositMonthWiseSummaryReport";
                    filename = "rptBankDipositMonthWiseSummaryReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptBankDipositMonthWiseSummaryReport] '" + comid + "','" + FromDate + "','" + ToDate + "','" + MonthId + "' ";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptBankDipositMonthWiseSummaryReport] '" + comid + "','" + FromDate + "','" + ToDate + "','" + MonthId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptAllotmentWiseSummaryReport")
                {
                    reportname = "rptAllotmentWiseSummaryReport";
                    filename = "rptAllotmentWiseSummaryReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptAllotmentWiseSummaryReport] '" + comid + "','" + YearId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptAllotmentWiseSummaryReport] '" + comid + "','" + YearId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptAllotmentMonthWiseDistrictReport")
                {
                    reportname = "rptAllotmentMonthWiseDistrictReport";
                    filename = "rptAllotmentMonthWiseDistrictReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptAllotmentMonthWiseDistrictReport] '" + comid + "','" + YearId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptAllotmentMonthWiseDistrictReport] '" + comid + "','" + YearId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptChallanReport")
                {
                    reportname = "rptChallanReport";
                    filename = "rptChallanReport_List_" + DateTime.Now.Date.ToString();
                    // var query = "Exec [Sales_rptChallanReport] '" + comid + "','" + FromDOId + "','" + ToDOId + "'";
                    var query = "Exec [Sales_rptChallanReport] '" + comid + "','" + FromDate + "','" + ToDate + "'";
                    // HttpContext.Session.SetString("reportquery", "Exec [Sales_rptChallanReport] '" + comid + "','" + FromDOId + "','" + ToDOId + "'");
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptChallanReport] '" + comid + "','" + FromDate + "','" + ToDate + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptBankWiseSummaryReport")
                {
                    reportname = "rptBankWiseYearlySummaryReport";
                    filename = "rptBankWiseYearlySummaryReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptBankWiseSummaryReport] '" + comid + "','" + YearId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptBankWiseSummaryReport] '" + comid + "','" + YearId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptMonthWiseSalesReport")
                {
                    reportname = "rptMonthWiseSalesReport";
                    filename = "rptMonthWiseSalesReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptAllotmentWiseSummaryReport] '" + comid + "','" + YearId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptAllotmentWiseSummaryReport] '" + comid + "','" + YearId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptDellerWiseBalanceReport")
                {
                    reportname = "rptDellerWiseBalanceReport";
                    filename = "rptDellerWiseBalanceReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptDellerWiseBalanceReport] '" + comid + "','" + YearId + "','" + MonthId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDellerWiseBalanceReport] '" + comid + "','" + YearId + "','" + MonthId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptDeliveryChallanReport")
                {
                    reportname = "rptDeliveryChallanReport";
                    filename = "rptrptDeliveryChallan_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptDeliveryChallanReport] '" + comid + "','" + FromDate + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDeliveryChallanReport] '" + comid + "','" + FromDate + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptTruckGatePassReport")
                {
                    reportname = "rptTruckGatePassReport";
                    filename = "rptTruckGatePassReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptTruckGatePassReport] '" + comid + "','" + GatePassId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptTruckGatePassReport] '" + comid + "','" + GatePassId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptDOChallanOrderReport")
                {
                    reportname = "rptDOChallanOrderReport";
                    filename = "rptDOChallanOrderReport_" + DateTime.Now.Date.ToString();
                    var query = "Exec [rptDeliveryOrder] '" + comid + "','" + DONo + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [rptDeliveryOrder] '" + comid + "','" + DONo + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptBoraddhoVittikMulloJomaanduttolonReport")
                {
                    reportname = "rptBoraddhoVittikMulloJomaanduttolonReport";
                    filename = "rptBoraddhoVittikMulloJomaanduttolonReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptDebitOrCreditPriceBasedOnQtyReport] '" + comid + "','" + BookingMonthId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDebitOrCreditPriceBasedOnQtyReport] '" + comid + "','" + MonthId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptDealerVittikMulloJomaanduttolonReport")
                {
                    reportname = "rptDealerVittikMulloJomaanduttolonReport";
                    filename = "rptDealerVittikMulloJomaanduttolonReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptDealerWiseDebitOrCreditPriceBasedOnQtyReport] '" + comid + "','" + MonthId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDealerWiseDebitOrCreditPriceBasedOnQtyReport] '" + comid + "','" + MonthId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptYearlyDistributionReport")
                {
                    reportname = "rptYearlyDistributionReport";
                    filename = "rptYearlyDistributionReport_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptYearlyDistributionReport] '" + comid + "','" + YearId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptYearlyDistributionReport] '" + comid + "','" + YearId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptTaliSheet")
                {
                    reportname = "rptTruckJugeSharSorboraherTaliSheet";
                    filename = "rptTruckJugeSharSorboraherTaliSheet_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptTaliSheet] '" + comid + "','" + FromDate + "','" + ToDate + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptTaliSheet] '" + comid + "','" + FromDate + "','" + ToDate + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptDeliveryChallanReceiverPersonWise")
                {
                    reportname = "rptDeliveryChallanByReceiver";
                    filename = "rptDeliveryChallanByReceiver_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptDeliveryChallanByReceiverReport] '" + comid + "','" + FromDate + "','" + ToDate + "','" + ReceiverPerson + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDeliveryChallanByReceiverReport] '" + comid + "','" + FromDate + "','" + ToDate + "','" + ReceiverPerson + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintrptDeliveryChallanRepresentativeWise")
                {
                    reportname = "rptDeliveryChallanByRepresentative";
                    filename = "rptDeliveryChallanByRepresentative_List_" + DateTime.Now.Date.ToString();
                    var query = "Exec [Sales_rptDeliveryChallanByRepresentativeReport] '" + comid + "','" + FromDate + "','" + ToDate + "','" + RepresentativeId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDeliveryChallanByRepresentativeReport] '" + comid + "','" + FromDate + "','" + ToDate + "','" + RepresentativeId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (action == "PrintMissingSequence")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rpt_MissingSequence";
                    filename = "rpt_MissingSequence" + DateTime.Now.Date.ToString();
                    var query = "Exec [rpt_MissingSequence] '" + comid + "' , ";
                    HttpContext.Session.SetString("reportquery", "Exec [rpt_MissingSequence] '" + comid + "',  '" + Type + "' , '" + FromNo + "','" + ToNo + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }

                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;



                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }

                return Json(new { Url = redirectUrl });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");
        }

        public class BufferVm
        {
            public int BufferListId { get; set; }

            public string ComId { get; set; }


            public string BufferCode { get; set; }


            public string BufferName { get; set; }

            public string BufferNameBangla { get; set; }
            public List<SelectListItem> drpDistricts { get; set; }
            public List<SelectListItem> drpBufferRep { get; set; }
            public int[] districtId { get; set; }

            public int[] bufferRepId { get; set; }
        }
    }
}
