using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
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
    public class DeliveryOrdersController : Controller
    {
        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }
        //public CommercialRepository Repository { get; set; }


        public DeliveryOrdersController(GTRDBContext context, clsProcedure _clsProc, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;
            clsProc = _clsProc;
            //Repository = rep;
        }

        // GET: DeliveryOrders
        public async Task<IActionResult> Index()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            var gTRDBContext = _context.DeliveryOrder.Where(x => x.ComId == comid).Take(1).Include(d => d.Acc_ChartOfAccount).Include(d => d.Booking).ThenInclude(c => c.Customer);


            int currentfiscalyearid = _context.Acc_FiscalYears.Where(x => x.isRunning == true).FirstOrDefault().FYId;
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", currentfiscalyearid);


            ViewBag.Year = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FiscalYearId), "FiscalYearId", "FYName");
            ViewBag.DONo = new SelectList(_context.DeliveryOrder.Where(s => s.ComId == comid).OrderByDescending(s => s.DOId), "DOId", "DONo");

            return View(await gTRDBContext.ToListAsync());
        }


        public IActionResult BookingDataById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            //var bookingdetails = await _context.Booking.Where(b => b.BookingId == id).Select( b=> new 
            //{
            //   Year = b.YearName.FYName,
            //   Month = b.MonthName.MonthName,
            //   BookingNo = b.BookingNo,
            //   District = b.Cat_District.DistName,
            //   PoliceStation = b.Cat_PoliceStation.PStationName,
            //   Dealar = b.Customer.CustomerName,
            //   Qty = b.AllotmentQty
            //}).FirstOrDefaultAsync();

            var query = $"Exec PrcDeliveryOrder '{comid}',{id},{0}";

            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@BookingId", id);
            parameters[2] = new SqlParameter("@DOId", "0");

            List<PrcDeliveryOrder> bookingdetails = Helper.ExecProcMapTList<PrcDeliveryOrder>("[PrcDeliveryOrder]", parameters);

            if (bookingdetails == null)
            {
                return Json(new { Success = 2, ex = "Data Not Found" });
            }
            else
            {
                var lastbookingdata = bookingdetails.OrderByDescending(b => b.CustomerName).FirstOrDefault();
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
            List<Booking> booking = _context.Booking
                .Include(x => x.vDeliveryOrder)
                .Include(x => x.YearName)
                .Include(x => x.MonthName)
                .Include(x => x.Customer)
                .Where(x => x.CustomerId == id
                //&& Math.Round(x.AllotmentQty - ((x.vDeliveryOrder != null : (x.vDeliveryOrder.Sum(x => x.Qty)) ? 0)), 2) > 0
                ).OrderByDescending(x => x.FiscalMonthId)
                .ThenByDescending(x => x.AllotmentType).ToList();

            List<SelectListItem> listbooking = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });



            if (booking != null)
            {
                foreach (Booking x in booking)
                {
                    if (x.AllotmentQty - ((x.vDeliveryOrder != null ? (x.vDeliveryOrder.Sum(x => x.Qty)) : 0)) > 0)
                    {
                        listbooking.Add(new SelectListItem { Text = x.AllotmentType + " - " + "[" + x.MonthName.MonthName + "] " + x.Customer.CustomerName + " [ Rem. : " + Math.Round(x.AllotmentQty - ((x.vDeliveryOrder != null ? (x.vDeliveryOrder.Sum(x => x.Qty)) : 0)), 2).ToString("0.00") + " ]", Value = x.BookingId.ToString() });
                    }
                }

                if (listbooking == null || listbooking.Count == 0)
                {
                    foreach (Booking x in booking.OrderByDescending(x => x.BookingId).Take(1))
                    {

                        listbooking.Add(new SelectListItem { Text = x.AllotmentType + " - " + "[" + x.MonthName.MonthName + "] " + x.Customer.CustomerName + " [ Rem. : " + Math.Round(x.AllotmentQty - ((x.vDeliveryOrder != null ? (x.vDeliveryOrder.Sum(x => x.Qty)) : 0)), 2).ToString("0.00") + " ]", Value = x.BookingId.ToString() });

                    }
                }

            }

            return Json(new SelectList(listbooking, "Value", "Text"));//.OrderByDescending(x=>x.Value)
        }


        public class PrcDeliveryOrder
        {
            public int BookingId { get; set; }
            public string BookingNo { get; set; }
            public string Year { get; set; }
            public string Month { get; set; }
            public string DistName { get; set; }
            public string PStationName { get; set; }
            public string CustomerName { get; set; }
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
            public string AllotmentType { get; set; }

            public string Month { get; set; }
            public string Year { get; set; }


            public string BankName { get; set; }
            public string Dealer { get; set; }
            public string DealerCode { get; set; }

            public string PayInSlipNo { get; set; }
            public string PaySlipDate { get; set; }
            public float Qty { get; set; }
            public string RemainingQty { get; set; }


            public string DeliveryChallanNo { get; set; }


            public string UnitPrice { get; set; }
            public string TotalPrice { get; set; }
            public string Remarks { get; set; }
            public string Representative { get; set; }



            public List<DeliveryChallan> DeliveryChallanList { get; set; }

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

                //UserMenuPermission abc = _context.MenuPermissionDetails.Include(). _context.MenuPermissionMasters.Where(x => x.comid == comid && x.userid == userid).FirstOrDefault(). .Where(x=>x.ModuleMenuController == controller) MenuPermissionMasters.MenuPermission_Details.FirstOrDefault().menu


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




                    var query = from e in _context.DeliveryOrder.Where(x => x.ComId == comid) //&& x.Booking.FiscalYearId == FiscalYearId
                                .OrderByDescending(x => x.DOId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                let ChallanNo = e.vDeliveryChallan != null ? e.vDeliveryChallan.Select(x => new DeliveryChallan { ChallanNo = x.ChallanNo }).ToList() : null

                                select new DeliveryOrderabc
                                {
                                    DOId = e.DOId,
                                    DONo = e.DONo.ToString(),
                                    DODate = e.DODate.ToString("dd-MMM-yy"),

                                    AllotmentType = e.Booking.AllotmentType,

                                    Month = e.Booking.MonthName.MonthName,
                                    Year = e.Booking.YearName.FYName,


                                    BankName = e.Acc_ChartOfAccount.AccName,
                                    Dealer = e.Booking.Customer.CustomerName,
                                    DealerCode = e.Booking.Customer.CustomerCode,

                                    PayInSlipNo = e.PayInSlipNo.ToString(),
                                    PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                    Qty = e.Qty,
                                    UnitPrice = e.UnitPrice.ToString(),
                                    TotalPrice = e.TotalPrice.ToString(),
                                    DeliveryChallanList = ChallanNo,
                                    Representative = e.vRepresentative != null ? (e.vRepresentative.RepresentativeName) : "",

                                    RemainingQty = Math.Round(e.Qty - ((e.vDeliveryChallan.Select(x => x.DeliveryQty) != null ? (e.vDeliveryChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),
                                    Remarks = e.Remarks

                                };


                    var parser = new Parser<DeliveryOrderabc>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (FromDate != null && ToDate != null)
                    {
                        var querytest = from e in _context.DeliveryOrder
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DODate >= dtFrom && p.DODate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        //.Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                        //.Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.DOId)
                                        let ChallanNo = e.vDeliveryChallan != null ? e.vDeliveryChallan.Select(x => new DeliveryChallan { ChallanNo = x.ChallanNo }).ToList() : null
                                        select new DeliveryOrderabc
                                        {
                                            DOId = e.DOId,
                                            DONo = e.DONo.ToString(),
                                            DODate = e.DODate.ToString("dd-MMM-yy"),

                                            AllotmentType = e.Booking.AllotmentType,

                                            Month = e.Booking.MonthName.MonthName,
                                            Year = e.Booking.YearName.FYName,


                                            BankName = e.Acc_ChartOfAccount.AccName,
                                            Dealer = e.Booking.Customer.CustomerName,
                                            DealerCode = e.Booking.Customer.CustomerCode,

                                            PayInSlipNo = e.PayInSlipNo.ToString(),
                                            PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                            Qty = e.Qty,
                                            UnitPrice = e.UnitPrice.ToString(),
                                            TotalPrice = e.TotalPrice.ToString(),
                                            DeliveryChallanList = ChallanNo,
                                            Representative = e.vRepresentative != null ? (e.vRepresentative.RepresentativeName) : "",
                                            RemainingQty = Math.Round(e.Qty - ((e.vDeliveryChallan.Select(x => x.DeliveryQty) != null ? (e.vDeliveryChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),

                                            Remarks = e.Remarks

                                        };

                        var parser = new Parser<DeliveryOrderabc>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (FromDONo != null && ToDONo != null)
                    {

                        var querytest = from e in _context.DeliveryOrder
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DONo >= FromDONo && p.DONo <= ToDONo))
                        ////.Where(p => p.userid == UserList)
                        //.Where(p => p.UserId.ToLower().Contains(FromDONo.ToString()))


                        .OrderByDescending(x => x.DOId)
                                        let ChallanNo = e.vDeliveryChallan != null ? e.vDeliveryChallan.Select(x => new DeliveryChallan { ChallanNo = x.ChallanNo }).ToList() : null
                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new DeliveryOrderabc
                                        {
                                            DOId = e.DOId,
                                            DONo = e.DONo.ToString(),
                                            DODate = e.DODate.ToString("dd-MMM-yy"),


                                            AllotmentType = e.Booking.AllotmentType,

                                            Month = e.Booking.MonthName.MonthName,
                                            Year = e.Booking.YearName.FYName,


                                            BankName = e.Acc_ChartOfAccount.AccName,
                                            Dealer = e.Booking.Customer.CustomerName,
                                            DealerCode = e.Booking.Customer.CustomerCode,

                                            PayInSlipNo = e.PayInSlipNo.ToString(),
                                            PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                            Qty = e.Qty,
                                            UnitPrice = e.UnitPrice.ToString(),
                                            TotalPrice = e.TotalPrice.ToString(),
                                            DeliveryChallanList = ChallanNo,
                                            Representative = e.vRepresentative != null ? (e.vRepresentative.RepresentativeName) : "",
                                            RemainingQty = Math.Round(e.Qty - ((e.vDeliveryChallan.Select(x => x.DeliveryQty) != null ? (e.vDeliveryChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),

                                            Remarks = e.Remarks


                                        };


                        var parser = new Parser<DeliveryOrderabc>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in _context.DeliveryOrder
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DODate >= dtFrom && p.DODate <= dtTo))

                        .OrderByDescending(x => x.DOId)
                                        let ChallanNo = e.vDeliveryChallan != null ? e.vDeliveryChallan.Select(x => new DeliveryChallan { ChallanNo = x.ChallanNo }).ToList() : null
                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new DeliveryOrderabc
                                        {
                                            DOId = e.DOId,
                                            DONo = e.DONo.ToString(),
                                            DODate = e.DODate.ToString("dd-MMM-yy"),

                                            AllotmentType = e.Booking.AllotmentType,

                                            Month = e.Booking.MonthName.MonthName,
                                            Year = e.Booking.YearName.FYName,


                                            BankName = e.Acc_ChartOfAccount.AccName,
                                            Dealer = e.Booking.Customer.CustomerName,
                                            DealerCode = e.Booking.Customer.CustomerCode,

                                            PayInSlipNo = e.PayInSlipNo.ToString(),
                                            PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
                                            Qty = e.Qty,
                                            UnitPrice = e.UnitPrice.ToString(),
                                            TotalPrice = e.TotalPrice.ToString(),
                                            DeliveryChallanList = ChallanNo,
                                            Representative = e.vRepresentative != null ? (e.vRepresentative.RepresentativeName) : "",
                                            RemainingQty = Math.Round(e.Qty - ((e.vDeliveryChallan.Select(x => x.DeliveryQty) != null ? (e.vDeliveryChallan.Sum(x => x.DeliveryQty)) : 0)), 2).ToString("0.00"),

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
                //throw ex;
            }

        }


        //public IActionResult Get()
        //{
        //    try
        //    {
        //        var comid = (HttpContext.Session.GetString("comid"));

        //        var query = from e in _context.DeliveryOrder.Include(x=>x.Booking).Where(x => x.DOId > 0)
        //                    select new DeliveryOrderabc
        //                    {
        //                        DOId = e.DOId,
        //                        DONo = e.DONo.ToString(),
        //                        DODate = e.DODate.ToString("dd-MMM-yy"),

        //                        Month = e.Booking.MonthName.MonthName,
        //                        Year = e.Booking.YearName.FYName,


        //                        BankName = e.Acc_ChartOfAccount.AccName,
        //                        Dealer = e.Booking.Customer.CustomerName,
        //                        DealerCode = e.Booking.Customer.CustomerCode,

        //                        PayInSlipNo = e.PayInSlipNo.ToString(),
        //                        PaySlipDate = e.PayInSlipDate.ToString("dd-MMM-yy"),
        //                        Qty = e.Qty,
        //                        UnitPrice = e.UnitPrice.ToString(),
        //                        TotalPrice = e.TotalPrice.ToString(),
        //                        Remarks = e.Remarks

        //                    };

        //        var parser = new Parser<DeliveryOrderabc>(Request.Form, query);

        //        return Json(parser.Parse());

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}


        public JsonResult Customer_Check(string id)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                Customer customer = _context.Customers.Where(x => x.CustomerCode == id && x.comid == comid).FirstOrDefault(); //x.PINo == id &&  //.Where(x =>  x.comid == comid)

                return Json(new { success = true, values = customer });

            }
            catch (Exception ex)
            {

                return Json(new { success = false, values = ex.Message.ToString() });

            }

        }


        // GET: DeliveryOrders/Create
        public IActionResult Create()
        {

            //var QtyinwordsBng = clsProc.GTRInwordsBangla("77000.2".ToString(), "", "");


            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            //var lastDoNoDataUserWise = _context.DeliveryOrder.Where(x=>x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.DOId).FirstOrDefault();
            var lastDoNoData = _context.DeliveryOrder.Where(x => x.ComId == comid).OrderByDescending(D => D.DOId).FirstOrDefault();

            var UserWiselastDoNoData = _context.DeliveryOrder.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.DOId).FirstOrDefault();


            //var abc = _context.DeliveryOrder.FirstOrDefault();
            //var maxDODate = DateTime.Now.Date;
            //if (abc != null)
            //{
            //    maxDODate = _context.DeliveryOrder.Max(x => x.DODate);
            //}
            ViewBag.Title = "Create";
            //ViewBag.CustomerId = new SelectList(_context.Customers.Take(100), "CustomerId", "CustomerName");
            ViewData["AccId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(a => a.IsBankItem == true && a.AccCode.Contains("1-1-11") && a.ComId == comid && a.AccType == "L"), "AccId", "AccName");
            //this.ViewBag.CustomerId = new SelectList(_context.Customers.Where(p => p.comid == comid).Select(s => new { Text = s.CustomerName + " - [ " + s.CustomerCode + " ]", Value = s.CustomerId }).ToList(), "Value", "Text");
            ViewData["BookingId"] = new SelectList(_context.Booking.Take(0).Where(x => x.ComId == comid).Select(b => new { BookingId = b.BookingId, YearMonthName = b.YearName.FYName + " - " + "[" + b.MonthName.MonthName + "] " + b.Customer.CustomerName }), "BookingId", "YearMonthName");



            if (lastDoNoData == null)
            {
                lastDoNoData = new DeliveryOrder();
                lastDoNoData.DONo = 100001;
                lastDoNoData.DODate = DateTime.Now.Date;
                lastDoNoData.PayInSlipDate = DateTime.Now.Date;
                lastDoNoData.UnitPrice = 0;
                ViewData["RepresentativeId"] = new SelectList(_context.Representative.Where(a => a.comid == comid), "RepresentativeId", "RepresentativeName");

                return View(lastDoNoData);


            }
            else if (UserWiselastDoNoData == null)
            {
                UserWiselastDoNoData = new DeliveryOrder();
                UserWiselastDoNoData.DONo = _context.DeliveryOrder.Where(x => x.ComId == comid).Max(x => x.DONo) + 1;
                UserWiselastDoNoData.DODate = DateTime.Now.Date;
                UserWiselastDoNoData.PayInSlipDate = DateTime.Now.Date;
                UserWiselastDoNoData.UnitPrice = 0;
                //UserWiselastDoNoData.Remarks = QtyinwordsBng;
                ViewData["RepresentativeId"] = new SelectList(_context.Representative.Where(a => a.comid == comid), "RepresentativeId", "RepresentativeName");

                return View(UserWiselastDoNoData);


            }
            else if (UserWiselastDoNoData != null)
            {
                UserWiselastDoNoData.DOId = 0;
                //UserWiselastDoNoData.DONo = _context.DeliveryOrder.Where(x => x.ComId == comid && x.UserId == userid).Max(x => x.DONo) + 1;
                UserWiselastDoNoData.DONo = _context.DeliveryOrder.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(x => x.DOId).FirstOrDefault().DONo + 1;



                var donoexist = _context.DeliveryOrder.Where(x => x.ComId == comid && x.DONo == UserWiselastDoNoData.DONo);
                ViewData["RepresentativeId"] = new SelectList(_context.Representative.Where(a => a.comid == comid), "RepresentativeId", "RepresentativeName", UserWiselastDoNoData.RepresentativeId);


                //if (donoexist == null)
                //{
                //}
                //else
                //{
                //    UserWiselastDoNoData.DONo = _context.DeliveryOrder.Where(x => x.ComId == comid).Max(x => x.DONo) + 1;
                //}
                return View(UserWiselastDoNoData);


                //lastDoNoData.DODate = DateTime.Now.Date;
                //lastDoNoData.PayInSlipDate = DateTime.Now.Date;
                //lastDoNoData.UnitPrice = 0;
            }

            return View(lastDoNoData);


        }

        // POST: DeliveryOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(DeliveryOrder deliveryOrder)

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

                    var query = $"Exec PrcDeliveryOrder '{comid}',{deliveryOrder.BookingId},{deliveryOrder.DOId}";
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@ComId", comid);
                    parameters[1] = new SqlParameter("@BookingId", deliveryOrder.BookingId);
                    parameters[2] = new SqlParameter("@DOId", deliveryOrder.DOId);

                    List<PrcDeliveryOrder> bookingdetails = Helper.ExecProcMapTList<PrcDeliveryOrder>("[PrcDeliveryOrder]", parameters);
                    List<PrcDeliveryOrder> deliverydata = new List<PrcDeliveryOrder>();
                    foreach (var item in bookingdetails)
                    {
                        deliverydata.Add(item);
                    }


                    var qty = float.Parse(deliverydata.Sum(d => d.Qty).ToString("0.00"));
                    //var a = (float)aqty;

                    var totalgivenqty = (qty + deliveryOrder.Qty);
                    // totalgivenqty = Math.Round(totalgivenqty, 2);
                    //var allqty = bookingdetails.FirstOrDefault().RemainingQty + deliveryOrder.Qty;

                    var allotmentQty = _context.Booking.Find(deliveryOrder.BookingId).AllotmentQty;


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
                        if (deliveryOrder.DOId > 0)
                        {


                            if (deliveryOrder.DONo != deliveryOrder.OldDONo)
                            {

                                var countcustomer = _context.DeliveryOrder.Where(k => k.DONo == deliveryOrder.DONo && k.ComId == comid).Count();

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
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryOrder.DOId.ToString(), "Update", deliveryOrder.DONo.ToString() + " Booking Id :" + deliveryOrder.BookingId);



                            return Json(new { Success = result, ex = ViewData["Message"].ToString() });
                            //return Json(new { Success = 1, MasterLCId = cOM_MasterLC.MasterLCID, ex = TempData["Message"].ToString() });

                        }
                        else
                        {

                            //var lastDONO = _context.DeliveryOrder.Where(x => x.ComId == comid).OrderByDescending(x => x.DOId).FirstOrDefault();
                            //if (lastDONO == null)
                            //{
                            //    deliveryOrder.DONo = 100001;
                            //}
                            //else
                            //{
                            //    var currentDONOForSave =   _context.DeliveryOrder.Where(x => x.ComId == comid).Max(x => x.DONo) + 1;
                            //    deliveryOrder.DONo = currentDONOForSave;
                            //}
                            var countcustomer = _context.DeliveryOrder.Where(k => k.DONo == deliveryOrder.DONo && k.ComId == comid).Count();

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
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryOrder.DOId.ToString(), "Create", deliveryOrder.DONo.ToString() + " Booking Id :" + deliveryOrder.BookingId);




                            ViewData["AccId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(a => a.IsBankItem == true && a.AccCode.Contains("1-1-11") && a.AccType == "L"), "AccId", "AccName", deliveryOrder.AccId);
                            ViewData["BookingId"] = new SelectList(_context.Booking.Select(b => new { BookingId = b.BookingId, CustomerName = b.Customer.CustomerName + " - " + "[" + b.Customer.CustomerCode + "]" }), "BookingId", "CustomerName", deliveryOrder.BookingId);
                            ViewData["RepresentativeId"] = new SelectList(_context.Representative.Where(a => a.comid == comid), "RepresentativeId", "RepresentativeName", deliveryOrder.RepresentativeId);
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
                return Json(new { Success = 2, deliveryOrder.DOId, ex = ex });
            }



            ViewData["AccId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(a => a.IsBankItem == true && a.AccCode.Contains("1-1-11") && a.AccType == "L"), "AccId", "AccName", deliveryOrder.AccId);
            ViewData["BookingId"] = new SelectList(_context.Booking.Select(b => new { BookingId = b.BookingId, CustomerName = b.Customer.CustomerName + " - " + "[" + b.Customer.CustomerCode + "]" }), "BookingId", "CustomerName", deliveryOrder.BookingId);
            ViewData["RepresentativeId"] = new SelectList(_context.Representative.Where(a => a.comid == comid), "RepresentativeId", "RepresentativeName", deliveryOrder.RepresentativeId);
            return Json(new { Success = result, deliveryOrder.DOId, ex = ViewData["Message"].ToString() });
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


            var deliveryOrder = await _context.DeliveryOrder
                .Include(ban => ban.Acc_ChartOfAccount)
                .Include(b => b.Booking).ThenInclude(y => y.YearName)
                .Include(b => b.Booking).ThenInclude(m => m.MonthName)

                .Include(b => b.Booking).ThenInclude(m => m.Cat_District)
                .Include(b => b.Booking).ThenInclude(m => m.Cat_PoliceStation)
                .Include(b => b.Booking).ThenInclude(m => m.Customer)


                .Where(x => x.DOId == id).FirstOrDefaultAsync();

            var totaldoqty = _context.DeliveryOrder.Where(x => x.BookingId == deliveryOrder.BookingId && x.DOId != id).Sum(x => x.Qty);

            var remainingqtyabc = deliveryOrder.Booking.AllotmentQty - totaldoqty;

            deliveryOrder.RemainingQty = float.Parse(remainingqtyabc.ToString("0.00"));

            deliveryOrder.OldDONo = deliveryOrder.DONo;
            if (deliveryOrder == null)
            {
                return NotFound();
            }

            //ViewData["AccId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(a => a.IsBankItem == true), "AccId", "AccName", deliveryOrder.AccId);
            //ViewData["BookingId"] = new SelectList(_context.Booking.Select(b => new { BookingId = b.BookingId, CustomerName = b.Customer.CustomerName + " - " + "[" + b.Customer.CustomerCode + "]" }), "BookingId", "CustomerName" , deliveryOrder.BookingId);
            // ViewData["Title"] = "Edit";
            ViewData["AccId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(a => a.IsBankItem == true && a.AccCode.Contains("1-1-11") && a.ComId == comid && a.AccType == "L"), "AccId", "AccName", deliveryOrder.AccId);
            //ViewData["CustomerId"] = new SelectList(_context.Customers.Where(p => p.comid == comid).Select(s => new { Text = s.CustomerName + " - [ " + s.CustomerCode + " ]", Value = s.CustomerId }).ToList(), "Value", "Text" , deliveryOrder.Booking.CustomerId);
            ViewData["BookingId"] = new SelectList(_context.Booking.Where(x => x.ComId == comid && x.BookingId == deliveryOrder.BookingId).Select(b => new { BookingId = b.BookingId, YearMonthName = b.AllotmentType + " - " + "[" + b.MonthName.MonthName + "] " + b.Customer.CustomerName }), "BookingId", "YearMonthName", deliveryOrder.BookingId);
            ViewData["RepresentativeId"] = new SelectList(_context.Representative.Where(a => a.comid == comid), "RepresentativeId", "RepresentativeName", deliveryOrder.RepresentativeId);

            return View("Create", deliveryOrder);
        }

        // POST: DeliveryOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, DeliveryOrder deliveryOrder)
        //{
        //    if (id != deliveryOrder.DOId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(deliveryOrder);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DeliveryOrderExists(deliveryOrder.DOId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AccId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(a => a.IsBankItem == true && a.AccCode.Contains("1-1-11")), "AccId", "AccName" , );
        //    ViewData["BookingId"] = new SelectList(_context.Booking.Select(b => new { BookingId = b.BookingId, CustomerName = b.Customer.CustomerName + " - " + "[" + b.Customer.CustomerCode + "]" }), "BookingId", "CustomerName", deliveryOrder.Booking.CustomerId);
        //    return View(deliveryOrder);
        //}

        // GET: DeliveryOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");
            var deliveryOrder = await _context.DeliveryOrder
                .Include(ban => ban.Acc_ChartOfAccount)
                .Include(b => b.Booking)
                .Include(y => y.Booking.YearName)
                .Include(m => m.Booking.MonthName)
                .Include(d => d.Booking.Cat_District)
                .Include(p => p.Booking.Cat_PoliceStation)
                .Include(c => c.Booking.Customer)
                .Where(del => del.DOId == id).FirstOrDefaultAsync();



            if (deliveryOrder == null)
            {
                return NotFound();
            }


            ViewData["AccId"] = new SelectList(_context.Acc_ChartOfAccounts.Where(a => a.IsBankItem == true && a.AccCode.Contains("1-1-11") && a.ComId == comid && a.AccType == "L"), "AccId", "AccName", deliveryOrder.AccId);
            this.ViewBag.CustomerId = new SelectList(_context.Customers.Where(p => p.comid == comid).Select(s => new { Text = s.CustomerName + " - [ " + s.CustomerCode + " ]", Value = s.CustomerId }).ToList(), "Value", "Text", deliveryOrder.Booking.CustomerId);
            ViewData["BookingId"] = new SelectList(_context.Booking.Where(x => x.ComId == comid && x.BookingId == deliveryOrder.BookingId).Select(b => new { BookingId = b.BookingId, YearMonthName = b.AllotmentType + " - " + "[" + b.MonthName.MonthName + "] " + b.Customer.CustomerName }), "BookingId", "YearMonthName", deliveryOrder.BookingId);
            ViewData["RepresentativeId"] = new SelectList(_context.Representative.Where(a => a.comid == comid), "RepresentativeId", "RepresentativeName", deliveryOrder.RepresentativeId);
            return View("Create", deliveryOrder);
        }

        // POST: DeliveryOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                var deliveryOrder = await _context.DeliveryOrder.FindAsync(id);
                _context.DeliveryOrder.Remove(deliveryOrder);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";

                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryOrder.DONo.ToString(), "Delete", " DO Id :" + deliveryOrder.DOId.ToString());

                return Json(new { Success = 1, ContactID = deliveryOrder.DOId, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        private bool DeliveryOrderExists(int id)
        {
            return _context.DeliveryOrder.Any(e => e.DOId == id);
        }
    }
}
