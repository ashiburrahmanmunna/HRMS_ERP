using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Buffers;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.Buffer
{
    public class BufferDCController : Controller
    {
        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        [OverridableAuthorize]

        public BufferDCController(GTRDBContext context, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;

        }

        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");

            var reportname = "rptDeliveryChallanReport";
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
            HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDeliveryChallanReport_Ind] '" + comid + "','" + id + "'");

            string filename = "Challan_No_" + _context.DeliveryChallan.Where(x => x.DeliveryChallanId == id).Select(x => x.ChallanNo).Single().ToString();


            //var query = "Exec [Sales_rptDeliveryChallanReport] '" + comid + "','" + FromDate + "'";
            //HttpContext.Session.SetString("reportquery", "Exec [Sales_rptDeliveryChallanReport] '" + comid + "','" + FromDate + "'");


            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";
            HttpContext.Session.SetObject("rptList", postData);

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
        }



        public async Task<IActionResult> Index()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            var gTRDBContext = _context.BufferDelChallan.Take(1).Where(x => x.ComId == comid).Take(1).Include(d => d.BufferDelOrder);


            ViewBag.Year = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FiscalYearId), "FiscalYearId", "FYName");
            ViewBag.ChallanNo = new SelectList(_context.BufferDelChallan.Where(x => x.ComId == comid), "BufferDelChallanId", "ChallanNo");
            ViewBag.RepresentativeId = new SelectList(_context.BuferRepresentative.Where(x => x.comid == comid).OrderByDescending(y => y.RepresentativeName), "BuferRepresentativeId", "RepresentativeName");



            return View(await gTRDBContext.ToListAsync());
        }

        // GET: DeliveryChallan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryChallan = await _context.BufferDelChallan
                .Include(d => d.BufferDelOrder)
                .FirstOrDefaultAsync(m => m.BufferDelChallanId == id);
            if (deliveryChallan == null)
            {
                return NotFound();
            }

            return View(deliveryChallan);
        }

        public JsonResult GetDeliveryOrderData(string DONo, int? ChallanId)
        {


            var comid = HttpContext.Session.GetString("comid");

            if (ChallanId == null)
            {
                ChallanId = 0;
            }

            var query = $"EXEC PrcGetBufferDOData '{comid}', '{DONo}' ,'{ChallanId}'";
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@DONo", DONo);
            parameters[2] = new SqlParameter("@ChallanId", ChallanId);

            List<DeliveryOrderVm> DeliveryOrderData = Helper.ExecProcMapTList<DeliveryOrderVm>("PrcGetBufferDOData", parameters);

            return Json(DeliveryOrderData);
        }


        public JsonResult GetDeliveryChallanList(string id)
        {



            List<BufferDelChallan> deliveryChallanList = _context.BufferDelChallan.Include(x => x.BufferDelOrder).Where(p => (p.BufferDelOrder.DONo.ToString() == id.ToString())).ToList();
            List<DeliveryChallanVm> data = new List<DeliveryChallanVm>();

            foreach (var item in deliveryChallanList)
            {
                DeliveryChallanVm asdf = new DeliveryChallanVm();
                asdf.DeliveryChallanId = item.BufferDelChallanId;
                asdf.ChallanNo = item.ChallanNo;
                asdf.DeliveryDate = DateTime.Parse(item.DeliveryDate.ToString()).ToString("dd-MMM-yy");
                asdf.DeliveryQty = item.DeliveryQty;

                data.Add(asdf);
            }

            return Json(data);

        }

        public class DeliveryChallanVm
        {
            public int DeliveryChallanId { get; set; }
            public int ChallanNo { get; set; }
            public string DeliveryDate { get; set; }
            public float DeliveryQty { get; set; }
        }


        public class DeliveryOrderVm
        {
            public int DOId { get; set; }
            public int DONo { get; set; }
            public string MaxChallanNo { get; set; }
            public string DODate { get; set; }
            public string RepresentativeName { get; set; }
            public string RepresentativeCode { get; set; }
            public int PayInSlipNo { get; set; }
            public string BufferName { get; set; }
            public string PayInSlipDate { get; set; }
            public float OrderQty { get; set; }
            public float RemainingQty { get; set; }
            public float TotalDOQty { get; set; }
            public float UnitPrice { get; set; }
            public float TotalPrice { get; set; }
            public string MonthName { get; set; }
            public string YearName { get; set; }

        }

        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["remainqty"] = 0;



            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");



            var lastChallanNoDataUserWise = _context.BufferDelChallan.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.BufferDelChallanId).FirstOrDefault();
            var lastChallanNoData = _context.BufferDelChallan.Where(x => x.ComId == comid).OrderByDescending(D => D.BufferDelChallanId).FirstOrDefault();


            if (lastChallanNoData == null)
            {
                lastChallanNoData = new BufferDelChallan();
                lastChallanNoData.ChallanNo = 100001;
                lastChallanNoData.DeliveryDate = DateTime.Now.Date;



            }
            else
            {
                lastChallanNoData.BufferDelChallanId = 0;
                lastChallanNoData.ChallanNo = _context.BufferDelChallan.Where(x => x.ComId == comid).Max(x => x.ChallanNo);


                if (lastChallanNoDataUserWise != null)
                {
                    lastChallanNoData.DeliveryDate = lastChallanNoDataUserWise.DeliveryDate;


                }


            }

            return View(lastChallanNoData);
        }
        public class PrcDeliveryChallan
        {
            public int DOId { get; set; }
            public string DONo { get; set; }
            public float DOQty { get; set; }
            public float RemainingQty { get; set; }
            public float CurrentRemainingQty { get; set; }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult Create(BufferDelChallan deliveryChallan)

        {


            var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
            int success = 0, error = 0;
            var result = 0;

            try
            {
                var comid = HttpContext.Session.GetString("comid");


                if (ModelState.IsValid)
                {





                    var userid = HttpContext.Session.GetString("userid");



                    var DOQtyFromChallan = float.Parse(_context.BufferDelChallan.Where(x => x.BufferDelOrderId == deliveryChallan.BufferDelOrderId).Sum(x => x.DeliveryQty).ToString("0.00"));


                    var TotalChallanQty = Math.Round((DOQtyFromChallan + deliveryChallan.DeliveryQty), 2);

                    var AllotmentDOQty = Math.Round(_context.BufferDelOrder.Find(deliveryChallan.BufferDelOrderId).Qty, 2);





                    if (TotalChallanQty > AllotmentDOQty)
                    {
                        result = 3;
                        ViewData["Message"] = "You Cross the given limit.";
                        return Json(new { Success = result, ex = ViewData["Message"].ToString() });
                    }
                    else
                    {


                        if (deliveryChallan.BufferDelChallanId > 0)
                        {
                            ViewBag.Title = "Create";
                            deliveryChallan.UpdateByUserId = userid;
                            deliveryChallan.DateUpdated = DateTime.Now;


                            if (deliveryChallan.ComId == null || deliveryChallan.ComId == "")
                            {
                                deliveryChallan.ComId = comid;
                                deliveryChallan.UserId = userid;

                            }


                            _context.BufferDelChallan.Attach(deliveryChallan);
                            _context.Entry(deliveryChallan).Property(x => x.DeliveryQty).IsModified = true;
                            _context.Entry(deliveryChallan).Property(x => x.DeliveryDate).IsModified = true;
                            // _context.Entry(deliveryChallan).Property(x => x.PrdUnitId).IsModified = true;

                            _context.SaveChanges();



                            result = 2;
                            ViewData["Message"] = "Data Update Successfully.";
                            TempData["Message"] = "Data Update Successfully.";


                            TempData["Status"] = "2";
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryChallan.BufferDelChallanId.ToString(), "Update", deliveryChallan.ChallanNo.ToString() + " Delivery Order Id :" + deliveryChallan.BufferDelChallanId);


                            return Json(new { Success = result, ex = ViewData["Message"].ToString() });
                        }
                        else
                        {

                            var lastChallanNo = _context.BufferDelChallan.Where(x => x.ComId == comid).OrderByDescending(x => x.BufferDelChallanId).FirstOrDefault();
                            if (lastChallanNo == null)
                            {
                                deliveryChallan.ChallanNo = 100001;
                            }
                            else
                            {
                                var currentChallanNoForSave = _context.BufferDelChallan.Where(x => x.ComId == comid).Max(x => x.ChallanNo) + 1;
                                deliveryChallan.ChallanNo = currentChallanNoForSave;
                            }


                            BufferDelChallan de = new BufferDelChallan();
                            de.ComId = comid;
                            de.ChallanNo = deliveryChallan.ChallanNo;
                            de.DateAdded = DateTime.Now;
                            de.DeliveryDate = deliveryChallan.DeliveryDate;
                            de.DeliveryQty = deliveryChallan.DeliveryQty;
                            de.UserId = deliveryChallan.UserId;
                            de.PayInSlipDate = deliveryChallan.PayInSlipDate;
                            de.BufferDelOrderId = deliveryChallan.BufferDelOrderId;
                            //de.PrdUnitId = deliveryChallan.PrdUnitId;


                            _context.BufferDelChallan.Add(de);
                            _context.SaveChanges();


                            _context.Entry(de).GetDatabaseValues();
                            int id = de.BufferDelChallanId; // Yes it's here

                            result = 1;
                            ViewData["Message"] = "Data Save Successfully.";
                            TempData["Message"] = "Data Save Successfully.";
                            TempData["Status"] = "1";
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Create", de.ChallanNo.ToString() + " Delivery Order Id :" + de.BufferDelOrderId);

                            ViewBag.Title = "Create";

                        }

                    }

                }
                else
                {
                    ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.ComId == comid), "PrdUnitId", "PrdUnitName");
                    return Json(new { Success = 0, ex = errors.FirstOrDefault().Errors[0].ErrorMessage.ToString() });

                }
            }

            catch (Exception ex)
            {


                return Json(new { Success = 2, deliveryChallan.BufferDelOrderId, ex = ex });
            }

            return Json(new { Success = result, deliveryChallan.BufferDelOrderId, ex = ViewData["Message"].ToString() });


        }


        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return NotFound();
            }

            var comid = (HttpContext.Session.GetString("comid"));

            var deliveryChallan = await _context.BufferDelChallan.Include(x => x.BufferDelOrder).ThenInclude(x => x.RepresentativeBooking).Where(x => x.BufferDelChallanId == id).FirstOrDefaultAsync();
            //ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.comid == comid), "PrdUnitId", "PrdUnitName", deliveryChallan.PrdUnitId);

            //.FindAsync(id);
            if (deliveryChallan == null)
            {
                return NotFound();
            }
            var orderqty = _context.BufferDelOrder.Where(d => d.BufferDelOrderId == deliveryChallan.BufferDelOrderId).FirstOrDefault().Qty;
            var remainingqty = orderqty - deliveryChallan.DeliveryQty;
            var positivevalue = Math.Abs(remainingqty);
            ViewData["remainqty"] = positivevalue;


            return View("Create", deliveryChallan);
        }


        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("BufferDelChallanId,ChallanNo,DeliveryDate,DeliveryQty,DOId")] BufferDelChallan deliveryChallan)
        {
            if (id != deliveryChallan.BufferDelChallanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryChallan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BufferDelChallanExists(deliveryChallan.BufferDelChallanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(deliveryChallan);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return NotFound();
            }
            var comid = (HttpContext.Session.GetString("comid"));



            var deliveryChallan = await _context.BufferDelChallan
                .Include(d => d.BufferDelOrder)
                .FirstOrDefaultAsync(m => m.BufferDelChallanId == id);


            if (deliveryChallan == null)
            {
                return NotFound();
            }
            // ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.comid == comid), "PrdUnitId", "PrdUnitName", deliveryChallan.PrdUnitId);

            return View("Create", deliveryChallan);
        }


        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try

            {


                var deliveryChallan = _context.BufferDelChallan.Find(id);
                _context.BufferDelChallan.Remove(deliveryChallan);
                _context.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryChallan.ChallanNo.ToString(), "Delete", deliveryChallan.BufferDelChallanId.ToString());

                return Json(new { Success = 1, ContactID = deliveryChallan.BufferDelChallanId, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        private bool BufferDelChallanExists(int id)
        {
            return _context.BufferDelChallan.Any(e => e.BufferDelChallanId == id);
        }





        public class DeliveryChallanabc
        {
            public int DeliveryChallanId { get; set; }
            public string DeliveryDate { get; set; }
            public string ChallanNo { get; set; }
            public string GatePassNo { get; set; }

            public string Buffer { get; set; }
            public string RepresentativeCode { get; set; }
            public string FiscalYear { get; set; }
            public string FiscalMonth { get; set; }

            public string Representative { get; set; }


            public string DeliveryChallanQty { get; set; }
            public string DeliveryOrderNo { get; set; }
            public string DeliveryOrderQty { get; set; }
            public string RemainingQty { get; set; }


            public List<GatePassResult> GatePassList { get; set; }



        }
        public class GatePassResult
        {
            public string GatePassNo { get; set; }

        }
        public IActionResult Get(string FromDate, string ToDate, int? FromChallanNo, int? ToChallanNo, string Criteria)
        {


            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

                DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
                DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

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

                Microsoft.Extensions.Primitives.StringValues y = "";

                var x = Request.Form.TryGetValue("search[value]", out y);

                if (y.ToString().Length > 0)
                {


                    var query = from e in _context.BufferDelChallan.Where(x => x.ComId == comid)
                                .OrderByDescending(x => x.BufferDelChallanId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                let GatePassNo = e.BufferGatePassChallans != null ? e.BufferGatePassChallans.Select(x => new GatePassResult { GatePassNo = x.BufferGatePass.GatePassNo.ToString() }).ToList() : null
                                select new DeliveryChallanabc
                                {
                                    DeliveryChallanId = e.BufferDelChallanId,
                                    DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                    ChallanNo = e.ChallanNo.ToString(),
                                    GatePassList = GatePassNo,

                                    Buffer = e.BufferDelOrder.RepresentativeBooking.BufferList.BufferName,



                                    FiscalYear = e.BufferDelOrder.RepresentativeBooking.YearName.FYName,
                                    FiscalMonth = e.BufferDelOrder.RepresentativeBooking.MonthName.MonthName,

                                    RepresentativeCode = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeCode,
                                    Representative = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeName,
                                    DeliveryChallanQty = e.DeliveryQty.ToString(),
                                    DeliveryOrderQty = e.BufferDelOrder.Qty.ToString(),
                                    DeliveryOrderNo = e.BufferDelOrder.DONo.ToString(),
                                    RemainingQty = Math.Round(e.DeliveryQty - ((e.BufferGatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.BufferGatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),


                                };


                    var parser = new Parser<DeliveryChallanabc>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (FromDate != null && ToDate != null)
                    {
                        var querytest = from e in _context.BufferDelChallan
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DeliveryDate >= dtFrom && p.DeliveryDate <= dtTo))


                        .OrderByDescending(x => x.BufferDelChallanId)
                                        let GatePassNo = e.BufferGatePassChallans != null ? e.BufferGatePassChallans.Select(x => new GatePassResult { GatePassNo = x.BufferGatePass.GatePassNo.ToString() }).ToList() : null

                                        select new DeliveryChallanabc
                                        {

                                            DeliveryChallanId = e.BufferDelChallanId,
                                            DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                            ChallanNo = e.ChallanNo.ToString(),
                                            GatePassList = GatePassNo,

                                            Buffer = e.BufferDelOrder.RepresentativeBooking.BufferList.BufferName,



                                            FiscalYear = e.BufferDelOrder.RepresentativeBooking.YearName.FYName,
                                            FiscalMonth = e.BufferDelOrder.RepresentativeBooking.MonthName.MonthName,

                                            RepresentativeCode = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeCode,
                                            Representative = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeName,
                                            DeliveryChallanQty = e.DeliveryQty.ToString(),
                                            DeliveryOrderQty = e.BufferDelOrder.Qty.ToString(),
                                            DeliveryOrderNo = e.BufferDelOrder.DONo.ToString(),
                                            RemainingQty = Math.Round(e.DeliveryQty - ((e.BufferGatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.BufferGatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),


                                        };

                        var parser = new Parser<DeliveryChallanabc>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (FromChallanNo != null && ToChallanNo != null)
                    {

                        var querytest = from e in _context.BufferDelChallan
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.ChallanNo >= FromChallanNo && p.ChallanNo <= ToChallanNo))



                        .OrderByDescending(x => x.BufferDelChallanId)
                                        let GatePassNo = e.BufferGatePassChallans != null ? e.BufferGatePassChallans.Select(x => new GatePassResult { GatePassNo = x.BufferGatePass.GatePassNo.ToString() }).ToList() : null

                                        select new DeliveryChallanabc
                                        {
                                            DeliveryChallanId = e.BufferDelChallanId,
                                            DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                            ChallanNo = e.ChallanNo.ToString(),
                                            GatePassList = GatePassNo,

                                            Buffer = e.BufferDelOrder.RepresentativeBooking.BufferList.BufferName,



                                            FiscalYear = e.BufferDelOrder.RepresentativeBooking.YearName.FYName,
                                            FiscalMonth = e.BufferDelOrder.RepresentativeBooking.MonthName.MonthName,

                                            RepresentativeCode = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeCode,
                                            Representative = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeName,
                                            DeliveryChallanQty = e.DeliveryQty.ToString(),
                                            DeliveryOrderQty = e.BufferDelOrder.Qty.ToString(),
                                            DeliveryOrderNo = e.BufferDelOrder.DONo.ToString(),
                                            RemainingQty = Math.Round(e.DeliveryQty - ((e.BufferGatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.BufferGatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),


                                        };


                        var parser = new Parser<DeliveryChallanabc>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in _context.BufferDelChallan
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DeliveryDate >= dtFrom && p.DeliveryDate <= dtTo))

                        .OrderByDescending(x => x.BufferDelChallanId)
                                        let GatePassNo = e.BufferGatePassChallans != null ? e.BufferGatePassChallans.Select(x => new GatePassResult { GatePassNo = x.BufferGatePass.GatePassNo.ToString() }).ToList() : null

                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new DeliveryChallanabc
                                        {
                                            DeliveryChallanId = e.BufferDelChallanId,
                                            DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                            ChallanNo = e.ChallanNo.ToString(),
                                            GatePassList = GatePassNo,

                                            Buffer = e.BufferDelOrder.RepresentativeBooking.BufferList.BufferName,



                                            FiscalYear = e.BufferDelOrder.RepresentativeBooking.YearName.FYName,
                                            FiscalMonth = e.BufferDelOrder.RepresentativeBooking.MonthName.MonthName,

                                            RepresentativeCode = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeCode,
                                            Representative = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeName,
                                            DeliveryChallanQty = e.DeliveryQty.ToString(),
                                            DeliveryOrderQty = e.BufferDelOrder.Qty.ToString(),
                                            DeliveryOrderNo = e.BufferDelOrder.DONo.ToString(),
                                            RemainingQty = Math.Round(e.DeliveryQty - ((e.BufferGatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.BufferGatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),



                                        };


                        var parser = new Parser<DeliveryChallanabc>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", error = ex.Message });
                //throw ex;
            }


            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

                var query = from e in _context.BufferDelChallan.Include(x => x.BufferDelOrder).ThenInclude(x => x.RepresentativeBooking).ThenInclude(x => x.BufferRepresentative).Where(x => x.BufferDelChallanId > 0 && x.ComId == comid).OrderByDescending(x => x.BufferDelChallanId)
                            let GatePassNo = e.BufferGatePassChallans != null ? e.BufferGatePassChallans.Select(x => new GatePassResult { GatePassNo = x.BufferGatePass.GatePassNo.ToString() }).ToList() : null
                            select new DeliveryChallanabc
                            {
                                DeliveryChallanId = e.BufferDelChallanId,
                                DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                ChallanNo = e.ChallanNo.ToString(),
                                GatePassList = GatePassNo,

                                Buffer = e.BufferDelOrder.RepresentativeBooking.BufferList.BufferName,



                                FiscalYear = e.BufferDelOrder.RepresentativeBooking.YearName.FYName,
                                FiscalMonth = e.BufferDelOrder.RepresentativeBooking.MonthName.MonthName,

                                RepresentativeCode = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeCode,
                                Representative = e.BufferDelOrder.RepresentativeBooking.BufferRepresentative.RepresentativeName,
                                DeliveryChallanQty = e.DeliveryQty.ToString(),
                                DeliveryOrderQty = e.BufferDelOrder.Qty.ToString(),
                                DeliveryOrderNo = e.BufferDelOrder.DONo.ToString(),
                                RemainingQty = Math.Round(e.DeliveryQty - ((e.BufferGatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.BufferGatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),


                            };

                var parser = new Parser<DeliveryChallanabc>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
    }
}


























