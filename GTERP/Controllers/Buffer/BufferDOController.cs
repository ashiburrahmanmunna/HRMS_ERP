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

namespace GTERP.Controllers.POS
{
    [OverridableAuthorize]
    public class BufferDOController : Controller
    {
        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }
        //public CommercialRepository Repository { get; set; }


        public BufferDOController(GTRDBContext context, clsProcedure _clsProc, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;
            clsProc = _clsProc;
            //Repository = rep;
        }


        public async Task<IActionResult> Index()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            var data = _context.BufferDelOrder.Where(x => x.ComId == comid).Take(1).Include(d => d.RepresentativeBooking).ThenInclude(c => c.BufferRepresentative);


            int currentfiscalyearid = _context.Acc_FiscalYears.Where(x => x.isRunning == true).FirstOrDefault().FYId;
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", currentfiscalyearid);


            ViewBag.Year = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FiscalYearId), "FiscalYearId", "FYName");
            ViewBag.DONo = new SelectList(_context.BufferDelOrder.Where(s => s.ComId == comid).OrderByDescending(s => s.BufferDelOrderId), "BufferDelOrderId", "DONo");

            return View(await data.ToListAsync());
        }

        public IActionResult BookingDataById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");



            var query = $"Exec PrcBufferDO '{comid}',{id},{0}";

            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@BookingId", id);
            parameters[2] = new SqlParameter("@DOId", "0");

            List<PrcDeliveryOrder> bookingdetails = Helper.ExecProcMapTList<PrcDeliveryOrder>("[PrcBufferDO]", parameters);

            if (bookingdetails == null)
            {
                return Json(new { Success = 2, ex = "Data Not Found" });
            }
            else
            {
                var lastbookingdata = bookingdetails.OrderByDescending(b => b.RepresentativeName).FirstOrDefault();
                if (lastbookingdata == null)
                {
                    return NotFound();
                }
                return Json(lastbookingdata);
            }

        }


        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");

            var reportname = "rptDOChallanOrderReport";
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
            HttpContext.Session.SetString("reportquery", "Exec [rptDeliveryOrder] '" + comid + "','" + id + "'");

            string filename = _context.DeliveryOrder.Where(x => x.DONo == id).Select(x => x.DONo).Single().ToString();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            string DataSourceName = "DataSet1";
            HttpContext.Session.SetObject("rptList", postData);

            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;


            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            ////string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);
            //return Redirect(callBackUrl);




            string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = type });//, new { id = 1 }
            return Redirect(redirectUrl);


        }

        public JsonResult getBookingList(int id)
        {
            List<RepresentativeBooking> booking = _context.RepresentativeBooking
                .Include(x => x.BufferDeliveryOrder)
                .Include(x => x.YearName)
                .Include(x => x.MonthName)
                .Include(x => x.BufferRepresentative)
                .Where(x => x.BufferRepresentativeId == id
                ).OrderByDescending(x => x.FiscalMonthId)
                .ToList();

            List<SelectListItem> listbooking = new List<SelectListItem>();




            if (booking != null)
            {
                foreach (RepresentativeBooking x in booking)
                {
                    if (x.AllotmentQty - ((x.BufferDeliveryOrder != null ? (x.BufferDeliveryOrder.Sum(x => x.Qty)) : 0)) > 0)
                    {
                        listbooking.Add(new SelectListItem { Text = "[" + x.MonthName.MonthName + "] " + x.BufferRepresentative.RepresentativeName + " [ Rem. : " + Math.Round(x.AllotmentQty - ((x.BufferDeliveryOrder != null ? (x.BufferDeliveryOrder.Sum(x => x.Qty)) : 0)), 2).ToString("0.00") + " ]", Value = x.RepresentativeBookingId.ToString() });
                    }
                }

                if (listbooking == null || listbooking.Count == 0)
                {
                    foreach (RepresentativeBooking x in booking.OrderByDescending(x => x.RepresentativeBookingId).Take(1))
                    {
                        listbooking.Add(new SelectListItem { Text = "[" + x.MonthName.MonthName + "] " + x.BufferRepresentative.RepresentativeName + " [ Rem. : " + Math.Round(x.AllotmentQty - ((x.BufferDeliveryOrder != null ? (x.BufferDeliveryOrder.Sum(x => x.Qty)) : 0)), 2).ToString("0.00") + " ]", Value = x.RepresentativeBookingId.ToString() });


                    }
                }

            }

            return Json(new SelectList(listbooking, "Value", "Text"));
        }


        public JsonResult Customer_Check(string id)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var customer = _context.BuferRepresentative.Where(x => x.RepresentativeCode == id && x.comid == comid).FirstOrDefault(); //x.PINo == id &&  //.Where(x =>  x.comid == comid)

                return Json(new { success = true, values = customer });

            }
            catch (Exception ex)
            {

                return Json(new { success = false, values = ex.Message.ToString() });

            }

        }


        public class PrcDeliveryOrder
        {
            public int BookingId { get; set; }
            public string BookingNo { get; set; }
            public string Year { get; set; }
            public string Month { get; set; }
            public string BufferName { get; set; }
            public string RepresentativeName { get; set; }
            public float AllotmentQty { get; set; }
            public float Qty { get; set; }
            public float RemainingQty { get; set; }
            public float CurrentRemainingQty { get; set; }
            public float UnitPrice { get; set; }
            public float TotalPrice { get; set; }
        }

        public class DeliveryOrderabc
        {
            public int DOId { get; set; }
            public string DONo { get; set; }

            public string DODate { get; set; }

            public string Month { get; set; }
            public string Year { get; set; }


            public string BankName { get; set; }
            public string Buffer { get; set; }
            public string BufferCode { get; set; }

            public string PayInSlipNo { get; set; }
            public string PaySlipDate { get; set; }
            public float Qty { get; set; }
            public string RemainingQty { get; set; }


            public string DeliveryChallanNo { get; set; }


            public string UnitPrice { get; set; }
            public string TotalPrice { get; set; }
            public string Remarks { get; set; }
            public string Representative { get; set; }



            public List<BufferDelChallan> DeliveryChallanList { get; set; }

        }

        public class DeliveryChallan
        {
            public int ChallanNo { get; set; }

        }


        public IActionResult Get(string FromDate, string ToDate, int? FromDONo, int? ToDONo, string Criteria, int FiscalYearId)
        {
            try
            {


                var comid = (HttpContext.Session.GetString("comid"));
                var userid = (HttpContext.Session.GetString("userid"));
                string controller = ControllerContext.ActionDescriptor.ControllerName;



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




                    var query = from e in _context.BufferDelOrder.Where(x => x.ComId == comid)
                                .OrderByDescending(x => x.BufferDelOrderId)

                                let ChallanNo = e.BufferDelChallan != null ? e.BufferDelChallan.Select(x => new BufferDelChallan { ChallanNo = x.ChallanNo }).ToList() : null

                                select new DeliveryOrderabc
                                {
                                    DOId = e.BufferDelOrderId,
                                    DONo = e.DONo.ToString(),
                                    DODate = e.DODate.ToString("dd-MMM-yy"),


                                    Month = e.RepresentativeBooking.MonthName.MonthName,
                                    Year = e.RepresentativeBooking.YearName.FYName,


                                    Buffer = e.RepresentativeBooking.BufferList.BufferName,
                                    BufferCode = e.RepresentativeBooking.BufferList.BufferCode,

                                    PayInSlipNo = e.PayInSlipNo.ToString(),
                                    PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                    Qty = e.Qty,
                                    UnitPrice = e.UnitPrice.ToString(),
                                    TotalPrice = e.TotalPrice.ToString(),
                                    DeliveryChallanList = ChallanNo,
                                    Representative = e.RepresentativeBooking != null ? (e.RepresentativeBooking.BufferRepresentative.RepresentativeName) : "",

                                    RemainingQty = Math.Round(e.Qty - ((e.BufferDelChallan.Select(x => x.DeliveryQty) != null ? (e.BufferDelChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),
                                    Remarks = e.Remarks

                                };


                    var parser = new Parser<DeliveryOrderabc>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (FromDate != null && ToDate != null)
                    {
                        var querytest = from e in _context.BufferDelOrder
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DODate >= dtFrom && p.DODate <= dtTo))


                        .OrderByDescending(x => x.BufferDelOrderId)
                                        let ChallanNo = e.BufferDelChallan != null ? e.BufferDelChallan.Select(x => new BufferDelChallan { ChallanNo = x.ChallanNo }).ToList() : null
                                        select new DeliveryOrderabc
                                        {
                                            DOId = e.BufferDelOrderId,
                                            DONo = e.DONo.ToString(),
                                            DODate = e.DODate.ToString("dd-MMM-yy"),


                                            Month = e.RepresentativeBooking.MonthName.MonthName,
                                            Year = e.RepresentativeBooking.YearName.FYName,


                                            Buffer = e.RepresentativeBooking.BufferRepresentative.RepresentativeName,
                                            BufferCode = e.RepresentativeBooking.BufferRepresentative.RepresentativeCode,

                                            PayInSlipNo = e.PayInSlipNo.ToString(),
                                            PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                            Qty = e.Qty,
                                            UnitPrice = e.UnitPrice.ToString(),
                                            TotalPrice = e.TotalPrice.ToString(),
                                            DeliveryChallanList = ChallanNo,
                                            Representative = e.RepresentativeBooking != null ? (e.RepresentativeBooking.BufferRepresentative.RepresentativeName) : "",

                                            RemainingQty = Math.Round(e.Qty - ((e.BufferDelChallan.Select(x => x.DeliveryQty) != null ? (e.BufferDelChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),
                                            Remarks = e.Remarks

                                        };

                        var parser = new Parser<DeliveryOrderabc>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (FromDONo != null && ToDONo != null)
                    {

                        var querytest = from e in _context.BufferDelOrder
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DONo >= FromDONo && p.DONo <= ToDONo))



                        .OrderByDescending(x => x.BufferDelOrderId)
                                        let ChallanNo = e.BufferDelChallan != null ? e.BufferDelChallan.Select(x => new BufferDelChallan { ChallanNo = x.ChallanNo }).ToList() : null

                                        select new DeliveryOrderabc
                                        {
                                            DOId = e.BufferDelOrderId,
                                            DONo = e.DONo.ToString(),
                                            DODate = e.DODate.ToString("dd-MMM-yy"),


                                            Month = e.RepresentativeBooking.MonthName.MonthName,
                                            Year = e.RepresentativeBooking.YearName.FYName,



                                            Buffer = e.RepresentativeBooking.BufferList.BufferName,
                                            BufferCode = e.RepresentativeBooking.BufferList.BufferCode,

                                            PayInSlipNo = e.PayInSlipNo.ToString(),
                                            PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                            Qty = e.Qty,
                                            UnitPrice = e.UnitPrice.ToString(),
                                            TotalPrice = e.TotalPrice.ToString(),
                                            DeliveryChallanList = ChallanNo,
                                            Representative = e.RepresentativeBooking != null ? (e.RepresentativeBooking.BufferRepresentative.RepresentativeName) : "",

                                            RemainingQty = Math.Round(e.RepresentativeBooking.AllotmentQty - ((e.BufferDelChallan.Select(x => x.DeliveryQty) != null ? (e.BufferDelChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),
                                            Remarks = e.Remarks


                                        };


                        var parser = new Parser<DeliveryOrderabc>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in _context.BufferDelOrder
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DODate >= dtFrom && p.DODate <= dtTo))

                        .OrderByDescending(x => x.BufferDelOrderId)
                                        let ChallanNo = e.BufferDelChallan != null ? e.BufferDelChallan.Select(x => new BufferDelChallan { ChallanNo = x.ChallanNo }).ToList() : null

                                        select new DeliveryOrderabc
                                        {
                                            DOId = e.BufferDelOrderId,
                                            DONo = e.DONo.ToString(),
                                            DODate = e.DODate.ToString("dd-MMM-yy"),


                                            Month = e.RepresentativeBooking.MonthName.MonthName,
                                            Year = e.RepresentativeBooking.YearName.FYName,


                                            Buffer = e.RepresentativeBooking.BufferList.BufferName,
                                            BufferCode = e.RepresentativeBooking.BufferList.BufferCode,

                                            PayInSlipNo = e.PayInSlipNo.ToString(),
                                            PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                            Qty = e.Qty,
                                            UnitPrice = e.UnitPrice.ToString(),
                                            TotalPrice = e.TotalPrice.ToString(),
                                            DeliveryChallanList = ChallanNo,
                                            Representative = e.RepresentativeBooking != null ? (e.RepresentativeBooking.BufferRepresentative.RepresentativeName) : "",

                                            RemainingQty = Math.Round(e.Qty - ((e.BufferDelChallan.Select(x => x.DeliveryQty) != null ? (e.BufferDelChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),
                                            Remarks = e.Remarks

                                        };


                        var parser = new Parser<DeliveryOrderabc>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", error = ex.Message });

            }

        }





        public JsonResult Representative_Check(string id)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                BufferRepresentative representative = _context.BuferRepresentative.Where(x => x.RepresentativeCode == id && x.comid == comid).FirstOrDefault(); //x.PINo == id &&  //.Where(x =>  x.comid == comid)

                return Json(new { success = true, values = representative });

            }
            catch (Exception ex)
            {

                return Json(new { success = false, values = ex.Message.ToString() });

            }

        }


        // GET: DeliveryOrders/Create
        public IActionResult Create()
        {



            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            var lastDoNoData = _context.BufferDelOrder.Where(x => x.ComId == comid).OrderByDescending(D => D.BufferDelOrderId).FirstOrDefault();

            var UserWiselastDoNoData = _context.BufferDelOrder.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.BufferDelOrderId).FirstOrDefault();



            ViewBag.Title = "Create";
            ViewBag.AccId = new SelectList(_context.Acc_ChartOfAccounts, "AccId", "AccName");
            ViewBag.BookingId = new SelectList(_context.RepresentativeBooking.Select(b => new { BookingId = b.RepresentativeBookingId, YearMonthName = b.YearName.FYName + " - " + "[" + b.MonthName.MonthName + "] " + b.BufferRepresentative.RepresentativeName }), "BookingId", "YearMonthName");



            if (lastDoNoData == null)
            {
                lastDoNoData = new BufferDelOrder();
                lastDoNoData.DONo = 100001;
                lastDoNoData.DODate = DateTime.Now.Date;
                lastDoNoData.PayInSlipDate = DateTime.Now.Date;
                lastDoNoData.UnitPrice = 0;
                ViewData["RepresentativeId"] = new SelectList(_context.BuferRepresentative.Where(a => a.comid == comid), "BufferRepresentativeId", "RepresentativeName");

                return View(lastDoNoData);


            }
            else if (UserWiselastDoNoData == null)
            {
                UserWiselastDoNoData = new BufferDelOrder();
                UserWiselastDoNoData.DONo = _context.BufferDelOrder.Where(x => x.ComId == comid).Max(x => x.DONo) + 1;
                UserWiselastDoNoData.DODate = DateTime.Now.Date;
                UserWiselastDoNoData.PayInSlipDate = DateTime.Now.Date;
                UserWiselastDoNoData.UnitPrice = 0;
                //UserWiselastDoNoData.Remarks = QtyinwordsBng;
                ViewData["RepresentativeId"] = new SelectList(_context.BuferRepresentative.Where(a => a.comid == comid), "BufferRepresentativeId", "RepresentativeName");

                return View(UserWiselastDoNoData);


            }
            else if (UserWiselastDoNoData != null)
            {
                UserWiselastDoNoData.BufferDelOrderId = 0;
                UserWiselastDoNoData.DONo = _context.BufferDelOrder.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(x => x.BufferDelOrderId).FirstOrDefault().DONo + 1;



                var donoexist = _context.BufferDelOrder.Where(x => x.ComId == comid && x.DONo == UserWiselastDoNoData.DONo);
                ViewData["RepresentativeId"] = new SelectList(_context.BuferRepresentative.Where(a => a.comid == comid), "BufferRepresentativeId", "RepresentativeName");



                return View(UserWiselastDoNoData);



            }

            return View(lastDoNoData);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(BufferDelOrder deliveryOrder)

        {
            var result = 0;

            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");



            try
            {
                if (ModelState.IsValid)
                {

                    var query = $"Exec PrcBufferDO '{comid}',{deliveryOrder.RepresentativeBookingId},{deliveryOrder.BufferDelOrderId}";
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@ComId", comid);
                    parameters[1] = new SqlParameter("@BookingId", deliveryOrder.RepresentativeBookingId);
                    parameters[2] = new SqlParameter("@DOId", deliveryOrder.BufferDelOrderId);

                    List<PrcDeliveryOrder> bookingdetails = Helper.ExecProcMapTList<PrcDeliveryOrder>("[PrcBufferDO]", parameters);
                    List<PrcDeliveryOrder> deliverydata = new List<PrcDeliveryOrder>();
                    foreach (var item in bookingdetails)
                    {
                        deliverydata.Add(item);
                    }


                    var qty = float.Parse(deliverydata.Sum(d => d.Qty).ToString("0.00"));


                    var totalgivenqty = (qty + deliveryOrder.Qty);

                    var allotmentQty = _context.RepresentativeBooking.Find(deliveryOrder.RepresentativeBookingId).AllotmentQty;


                    var QtyinwordsEng = clsProc.GTRInwordsFormatBD(deliveryOrder.Qty.ToString(), "", "");
                    deliveryOrder.QtyInWordsEng = QtyinwordsEng;

                    var QtyinwordsBng = clsProc.GTRInwordsBangla(deliveryOrder.Qty.ToString(), "", "");
                    deliveryOrder.QtyInWordsBng = QtyinwordsBng;



                    var TotalPriceinwordsEng = clsProc.GTRInwordsFormatBD(deliveryOrder.TotalPrice.ToString(), "", "");
                    deliveryOrder.TotalPriceInWordsEng = TotalPriceinwordsEng;

                    var TotalPriceinwordsBng = clsProc.GTRInwordsBangla(deliveryOrder.TotalPrice.ToString(), "", "");
                    deliveryOrder.TotalPriceInWordsBng = TotalPriceinwordsBng;


                    if (totalgivenqty > allotmentQty)
                    {
                        result = 3;
                        ViewData["Message"] = "You Crose the given limit.";
                        return Json(new { Success = result, ex = ViewData["Message"].ToString() });
                    }
                    else
                    {
                        if (deliveryOrder.BufferDelOrderId > 0)
                        {


                            if (deliveryOrder.DONo != deliveryOrder.OldDONo)
                            {

                                var countcustomer = _context.BufferDelOrder.Where(k => k.DONo == deliveryOrder.DONo && k.ComId == comid).Count();

                                if (countcustomer >= 1)
                                {
                                    ModelState.AddModelError("DuplicateError", "Delivery Order No already exist.");
                                    return Json(new { Success = 0, ex = "Delivery Order No already exist." });
                                }
                            }


                            deliveryOrder.DateUpdated = DateTime.Now;
                            deliveryOrder.ComId = comid;

                            if (deliveryOrder.UserId == null)
                            {
                                deliveryOrder.UserId = HttpContext.Session.GetString("userid");
                            }




                            if (deliveryOrder.ComId == null)
                            {
                                deliveryOrder.ComId = comid;
                                deliveryOrder.DateAdded = DateTime.Now;

                            }
                            deliveryOrder.UpdateByUserId = HttpContext.Session.GetString("userid");
                            deliveryOrder.DateUpdated = DateTime.Now;
                            _context.Update(deliveryOrder);
                            _context.SaveChanges();
                            result = 2;

                            ViewData["Message"] = "Data Update Successfully.";
                            TempData["Message"] = "Data Update Successfully.";
                            TempData["Status"] = "2";
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryOrder.BufferDelOrderId.ToString(), "Update", deliveryOrder.DONo.ToString() + " Booking Id :" + deliveryOrder.RepresentativeBookingId);



                            return Json(new { Success = result, ex = ViewData["Message"].ToString() });

                        }
                        else
                        {


                            var countcustomer = _context.BufferDelOrder.Where(k => k.DONo == deliveryOrder.DONo && k.ComId == comid).Count();

                            if (countcustomer >= 1)
                            {
                                ModelState.AddModelError("DuplicateError", "Delivery Order No already exist.");
                                return Json(new { Success = 0, ex = "Delivery Order No already exist." });
                            }
                            deliveryOrder.DateAdded = DateTime.Now;
                            deliveryOrder.UserId = userid;


                            _context.Add(deliveryOrder);
                            _context.SaveChanges();
                            result = 1;
                            ViewData["Message"] = "Data Save Successfully.";
                            TempData["Message"] = "Data Save Successfully.";
                            TempData["Status"] = "1";
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryOrder.BufferDelOrderId.ToString(), "Create", deliveryOrder.DONo.ToString() + " Booking Id :" + deliveryOrder.RepresentativeBookingId);




                            ViewData["BookingId"] = new SelectList(_context.RepresentativeBooking.Select(b => new { BookingId = b.RepresentativeBookingId, RepresentativeName = b.BufferRepresentative.RepresentativeName + " - " + "[" + b.BufferRepresentative.RepresentativeCode + "]" }), "BookingId", "RepresentativeName", deliveryOrder.RepresentativeBookingId);
                            ViewData["RepresentativeId"] = new SelectList(_context.BuferRepresentative.Where(a => a.comid == comid), "BufferRepresentativeId", "RepresentativeName", deliveryOrder.BufferRepresentativeId);
                            return Json(new { Success = result, ex = ViewData["Message"].ToString() });
                        }
                    }

                }
                else
                {
                    return Json(new { Success = 0, ex = errors.FirstOrDefault().Errors[0].ErrorMessage.ToString() });

                }

            }
            catch (Exception ex)
            {

                //throw ex;
                return Json(new { Success = 2, deliveryOrder.BufferDelOrderId, ex = ex });
            }



            ViewData["BookingId"] = new SelectList(_context.RepresentativeBooking.Select(b => new { BookingId = b.RepresentativeBookingId, RepresentativeName = b.BufferRepresentative.RepresentativeName + " - " + "[" + b.BufferRepresentative.RepresentativeCode + "]" }), "BookingId", "RepresentativeName", deliveryOrder.RepresentativeBookingId);
            ViewData["RepresentativeId"] = new SelectList(_context.BuferRepresentative.Where(a => a.comid == comid), "BufferRepresentativeId", "RepresentativeName", deliveryOrder.BufferRepresentativeId);
            return Json(new { Success = result, deliveryOrder.BufferDelOrderId, ex = ViewData["Message"].ToString() });
        }

        // GET: DeliveryOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");


            var deliveryOrder = await _context.BufferDelOrder

                .Include(b => b.RepresentativeBooking)
                .Include(y => y.RepresentativeBooking.YearName)
                .Include(m => m.RepresentativeBooking.MonthName)
                .Include(d => d.RepresentativeBooking.BufferList)
                .Include(c => c.RepresentativeBooking.BufferRepresentative)
                .Where(del => del.BufferDelOrderId == id).FirstOrDefaultAsync();



            var totaldoqty = _context.BufferDelOrder.Where(x => x.RepresentativeBooking == deliveryOrder.RepresentativeBooking && x.BufferDelOrderId != id).Sum(x => x.Qty);

            var remainingqtyabc = deliveryOrder.RepresentativeBooking.AllotmentQty - totaldoqty;

            deliveryOrder.RemainingQty = float.Parse(remainingqtyabc.ToString("0.00"));

            deliveryOrder.OldDONo = deliveryOrder.DONo;
            if (deliveryOrder == null)
            {
                return NotFound();
            }


            this.ViewBag.BufferRepresentativeId = new SelectList(_context.BuferRepresentative.Where(p => p.comid == comid).Select(s => new { Text = s.RepresentativeName + " - [ " + s.RepresentativeCode + " ]", Value = s.BufferRepresentativeId }).ToList(), "Value", "Text", deliveryOrder.RepresentativeBooking.BufferRepresentativeId);
            ViewData["RepresentativeBookingId"] = new SelectList(_context.RepresentativeBooking.Where(x => x.ComId == comid && x.RepresentativeBookingId == deliveryOrder.RepresentativeBookingId).Select(b => new { RepresentativeBookingId = b.RepresentativeBookingId, YearMonthName = "[" + b.MonthName.MonthName + "] " + b.BufferRepresentative.RepresentativeName }), "RepresentativeBookingId", "YearMonthName", deliveryOrder.RepresentativeBookingId);

            return View("Create", deliveryOrder);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var deliveryOrder = await _context.BufferDelOrder

                .Include(b => b.RepresentativeBooking)
                .Include(y => y.RepresentativeBooking.YearName)
                .Include(m => m.RepresentativeBooking.MonthName)
                .Include(d => d.RepresentativeBooking.BufferList)
                .Include(c => c.RepresentativeBooking.BufferRepresentative)
                .Where(del => del.BufferDelOrderId == id).FirstOrDefaultAsync();



            if (deliveryOrder == null)
            {
                return NotFound();
            }


            this.ViewBag.BufferRepresentativeId = new SelectList(_context.BuferRepresentative.Where(p => p.comid == comid).Select(s => new { Text = s.RepresentativeName + " - [ " + s.RepresentativeCode + " ]", Value = s.BufferRepresentativeId }).ToList(), "Value", "Text", deliveryOrder.RepresentativeBooking.BufferRepresentativeId);
            ViewData["RepresentativeBookingId"] = new SelectList(_context.RepresentativeBooking.Where(x => x.ComId == comid && x.RepresentativeBookingId == deliveryOrder.RepresentativeBookingId).Select(b => new { RepresentativeBookingId = b.RepresentativeBookingId, YearMonthName = "[" + b.MonthName.MonthName + "] " + b.BufferRepresentative.RepresentativeName }), "RepresentativeBookingId", "YearMonthName", deliveryOrder.RepresentativeBookingId);
            return View("Create", deliveryOrder);
        }

        // POST: DeliveryOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                var deliveryOrder = await _context.BufferDelOrder.FindAsync(id);
                _context.BufferDelOrder.Remove(deliveryOrder);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";

                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryOrder.DONo.ToString(), "Delete", " DO Id :" + deliveryOrder.BufferDelOrderId.ToString());

                return Json(new { Success = 1, ContactID = deliveryOrder.BufferDelOrderId, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        private bool BufferDelOrderExists(int id)
        {
            return _context.BufferDelOrder.Any(e => e.BufferDelOrderId == id);
        }
    }
}
