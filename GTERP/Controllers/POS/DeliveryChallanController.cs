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

namespace GTERP.Controllers
{
    public class DeliveryChallanController : Controller
    {//
        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }
        [OverridableAuthorize]

        public DeliveryChallanController(GTRDBContext context, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;
            //Repository = rep;
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


        // GET: DeliveryChallan
        public async Task<IActionResult> Index()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            var gTRDBContext = _context.DeliveryChallan.Take(1).Where(x => x.ComId == comid).Take(1).Include(d => d.DeliveryOrder);


            ViewBag.Year = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FiscalYearId), "FiscalYearId", "FYName");
            ViewBag.ChallanNo = new SelectList(_context.DeliveryChallan.Where(x => x.ComId == comid), "DeliveryChallanId", "ChallanNo");
            ViewBag.RepresentativeId = new SelectList(_context.Representative.Where(x => x.comid == comid).OrderByDescending(y => y.RepresentativeName), "RepresentativeId", "RepresentativeName");



            return View(await gTRDBContext.ToListAsync());
        }

        // GET: DeliveryChallan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryChallan = await _context.DeliveryChallan
                .Include(d => d.DeliveryOrder)
                .FirstOrDefaultAsync(m => m.DeliveryChallanId == id);
            if (deliveryChallan == null)
            {
                return NotFound();
            }

            return View(deliveryChallan);
        }

        public JsonResult GetDeliveryOrderData(string DONo, int? ChallanId)
        {
            //var DeliveryOrderData = await _context.DeliveryOrder
            //    .Include(b => b.Booking)
            //    .Include(b => b.Acc_ChartOfAccount)
            //    .Include(d => d.Booking.Cat_District)
            //    .Include(d => d.Booking.Cat_PoliceStation)
            //    .Include(d => d.Booking.MonthName)
            //    .Include(d => d.Booking.YearName)
            //    .Include(d => d.Booking.Customer)
            //    .FirstOrDefaultAsync(d => d.DOId == DOId);

            var comid = HttpContext.Session.GetString("comid");

            if (ChallanId == null)
            {
                ChallanId = 0;
            }

            var query = $"EXEC PrcGetDeliveryOrderData '{comid}', '{DONo}' ,'{ChallanId}'";
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@DONo", DONo);
            parameters[2] = new SqlParameter("@ChallanId", ChallanId);

            List<DeliveryOrderVm> DeliveryOrderData = Helper.ExecProcMapTList<DeliveryOrderVm>("PrcGetDeliveryOrderData", parameters);

            return Json(DeliveryOrderData);
        }


        public JsonResult GetDeliveryChallanList(string id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.MasterLCID = id;
            List<DeliveryChallan> deliveryChallanList = _context.DeliveryChallan.Include(x => x.DeliveryOrder).Where(p => (p.DeliveryOrder.DONo.ToString() == id.ToString())).ToList();
            //var deliveryChallanList = _context.DeliveryChallan.Include(x=>x.DeliveryOrder).Where(m => m.DeliveryOrder.DONo.ToString() == id).ToList();
            List<DeliveryChallanVm> data = new List<DeliveryChallanVm>();

            foreach (var item in deliveryChallanList)
            {
                DeliveryChallanVm asdf = new DeliveryChallanVm();
                //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
                asdf.DeliveryChallanId = item.DeliveryChallanId;
                asdf.ChallanNo = item.ChallanNo;
                asdf.DeliveryDate = DateTime.Parse(item.DeliveryDate.ToString()).ToString("dd-MMM-yy");
                asdf.DeliveryQty = item.DeliveryQty;

                data.Add(asdf);
            }

            return Json(data);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public class DeliveryChallanVm
        {
            public int DeliveryChallanId { get; set; }
            public int ChallanNo { get; set; }
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
            public string DeliveryDate { get; set; }
            public float DeliveryQty { get; set; }
        }


        public class DeliveryOrderVm
        {
            public int DOId { get; set; }
            public int DONo { get; set; }

            public string MaxChallanNo { get; set; }



            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
            public string DODate { get; set; }
            public string BankName { get; set; }
            public string DealerName { get; set; }
            public string DealerCode { get; set; }
            public int PayInSlipNo { get; set; }
            public string RepresentativeName { get; set; }



            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
            public string PayInSlipDate { get; set; }
            public float OrderQty { get; set; }
            public float RemainingQty { get; set; }
            public float TotalDOQty { get; set; }

            public float UnitPrice { get; set; }
            public float TotalPrice { get; set; }
            public string MonthName { get; set; }
            public string YearName { get; set; }
            public string StationName { get; set; }
            public string DistName { get; set; }
        }

        // GET: DeliveryChallan/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["remainqty"] = 0;



            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");



            var lastChallanNoDataUserWise = _context.DeliveryChallan.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.DeliveryChallanId).FirstOrDefault();
            var lastChallanNoData = _context.DeliveryChallan.Where(x => x.ComId == comid).OrderByDescending(D => D.DeliveryChallanId).FirstOrDefault();
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.ComId == comid), "PrdUnitId", "PrdUnitName");


            if (lastChallanNoData == null)
            {
                lastChallanNoData = new DeliveryChallan();
                lastChallanNoData.ChallanNo = 100001;
                lastChallanNoData.DeliveryDate = DateTime.Now.Date;

                ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.ComId == comid), "PrdUnitId", "PrdUnitName");


            }
            else
            {
                lastChallanNoData.DeliveryChallanId = 0;
                lastChallanNoData.ChallanNo = _context.DeliveryChallan.Where(x => x.ComId == comid).Max(x => x.ChallanNo);


                if (lastChallanNoDataUserWise != null)
                {
                    lastChallanNoData.DeliveryDate = lastChallanNoDataUserWise.DeliveryDate;
                    lastChallanNoData.PrdUnitId = lastChallanNoDataUserWise.PrdUnitId;
                    ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.ComId == comid), "PrdUnitId", "PrdUnitName", lastChallanNoData.PrdUnitId);

                }

                //lastDoNoData.DODate = DateTime.Now.Date;
                //lastDoNoData.PayInSlipDate = DateTime.Now.Date;
                //lastDoNoData.UnitPrice = 0;
            }


            //DeliveryChallan abc = new DeliveryChallan();

            //abc.DeliveryDate = DateTime.Now.Date;



            //ViewData["DOId"] = new SelectList(_context.DeliveryOrder, "DOId", "DONo");
            //ViewData["Bank"] = new SelectList(_context.Acc_ChartOfAccounts.Where(b => b.IsBankItem == true), "AccId", "AccName");
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
        // POST: DeliveryChallan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(DeliveryChallan deliveryChallan)
        //public JsonResult Create(DeliveryChallan deliveryChallan)
        {
            //if (ModelState.IsValid)
            //{
            var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
            int success = 0, error = 0;
            var result = 0;

            try
            {
                var comid = HttpContext.Session.GetString("comid");


                if (ModelState.IsValid)
                {




                    //if (ModelState.IsValid)
                    //{
                    var userid = HttpContext.Session.GetString("userid");

                    //if (comid == null)
                    //{
                    //    return NotFound();
                    //}

                    //94438
                    //var query = $"Exec PrcDeliveryChallan '{comid}',{deliveryChallan.DOId},{deliveryChallan.DeliveryChallanId}";
                    //SqlParameter[] parameters = new SqlParameter[3];
                    //parameters[0] = new SqlParameter("@ComId", comid);
                    //parameters[1] = new SqlParameter("@DOId", deliveryChallan.DOId);
                    //parameters[2] = new SqlParameter("@DeliveryChallanId", deliveryChallan.DeliveryChallanId);

                    //PrcDeliveryChallan challandetails = Helper.ExecProcMapTList<PrcDeliveryChallan>("[PrcdeliveryChallan]", parameters).FirstOrDefault();



                    //var DOQty = float.Parse(challandetails.RemainingQty.ToString("0.00"));

                    var DOQtyFromChallan = float.Parse(_context.DeliveryChallan.Where(x => x.DOId == deliveryChallan.DOId).Sum(x => x.DeliveryQty).ToString("0.00"));


                    var TotalChallanQty = Math.Round((DOQtyFromChallan + deliveryChallan.DeliveryQty), 2);

                    var AllotmentDOQty = Math.Round(_context.DeliveryOrder.Find(deliveryChallan.DOId).Qty, 2);





                    if (TotalChallanQty > AllotmentDOQty)
                    {
                        result = 3;
                        ViewData["Message"] = "You Crose the given limit.";
                        return Json(new { Success = result, ex = ViewData["Message"].ToString() });
                    }
                    else
                    {


                        if (deliveryChallan.DeliveryChallanId > 0)
                        {
                            ViewBag.Title = "Create";
                            deliveryChallan.UpdateByUserId = userid;
                            deliveryChallan.DateUpdated = DateTime.Now;


                            if (deliveryChallan.ComId == null || deliveryChallan.ComId == "")
                            {
                                deliveryChallan.ComId = comid;
                                deliveryChallan.UserId = userid;

                            }


                            _context.DeliveryChallan.Attach(deliveryChallan);
                            _context.Entry(deliveryChallan).Property(x => x.DeliveryQty).IsModified = true;
                            _context.Entry(deliveryChallan).Property(x => x.DeliveryDate).IsModified = true;
                            _context.Entry(deliveryChallan).Property(x => x.PrdUnitId).IsModified = true;

                            _context.SaveChanges();

                            //_context.Entry(deliveryChallan).State = EntityState.Modified;
                            //_context.SaveChanges();


                            // _context.Entry(deliveryChallan).State = EntityState.Modified;
                            //_context.DeliveryChallan.Update(deliveryChallan);


                            //_context.Update(deliveryChallan);
                            //_context.SaveChanges();


                            //return RedirectToAction(nameof(Index));

                            result = 2;
                            ViewData["Message"] = "Data Update Successfully.";
                            TempData["Message"] = "Data Update Successfully.";


                            TempData["Status"] = "2";
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryChallan.DeliveryChallanId.ToString(), "Update", deliveryChallan.ChallanNo.ToString() + " Delivery Order Id :" + deliveryChallan.DOId);


                            return Json(new { Success = result, ex = ViewData["Message"].ToString() });
                        }
                        else
                        {

                            var lastChallanNo = _context.DeliveryChallan.Where(x => x.ComId == comid).OrderByDescending(x => x.DeliveryChallanId).FirstOrDefault();
                            if (lastChallanNo == null)
                            {
                                deliveryChallan.ChallanNo = 100001;
                            }
                            else
                            {
                                var currentChallanNoForSave = _context.DeliveryChallan.Where(x => x.ComId == comid).Max(x => x.ChallanNo) + 1;
                                deliveryChallan.ChallanNo = currentChallanNoForSave;
                            }


                            DeliveryChallan de = new DeliveryChallan();
                            de.ComId = comid;
                            de.ChallanNo = deliveryChallan.ChallanNo;
                            de.DateAdded = DateTime.Now;
                            de.DeliveryDate = deliveryChallan.DeliveryDate;
                            de.DeliveryQty = deliveryChallan.DeliveryQty;
                            de.UserId = deliveryChallan.UserId;
                            de.PayInSlipDate = deliveryChallan.PayInSlipDate;
                            de.DOId = deliveryChallan.DOId;
                            de.PrdUnitId = deliveryChallan.PrdUnitId;


                            _context.DeliveryChallan.Add(de);
                            _context.SaveChanges();


                            _context.Entry(de).GetDatabaseValues();
                            int id = de.DeliveryChallanId; // Yes it's here

                            result = 1;
                            ViewData["Message"] = "Data Save Successfully.";
                            TempData["Message"] = "Data Save Successfully.";
                            TempData["Status"] = "1";
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Create", de.ChallanNo.ToString() + " Delivery Order Id :" + de.DOId);

                            ViewBag.Title = "Create";

                        }
                        //}
                        //}
                    }

                }
                else
                {
                    ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.ComId == comid), "PrdUnitId", "PrdUnitName");
                    return Json(new { Success = 0, ex = errors.FirstOrDefault().Errors[0].ErrorMessage.ToString() });

                }
            }
            //catch (Exception ex)
            //{

            //   // Console.WriteLine(ex.Message);

            //    return View("Create", deliveryChallan);

            //}
            catch (Exception ex)
            {

                //throw ex;
                return Json(new { Success = 2, deliveryChallan.DeliveryChallanId, ex = ex });
            }
            ///ViewData["DOId"] = new SelectList(_context.DeliveryOrder, "DOId", "DONo", deliveryChallan.DOId);
            //return View("Index");
            //return View("Create");
            return Json(new { Success = result, deliveryChallan.DeliveryChallanId, ex = ViewData["Message"].ToString() });


        }

        // GET: DeliveryChallan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return NotFound();
            }

            var comid = (HttpContext.Session.GetString("comid"));

            var deliveryChallan = await _context.DeliveryChallan.Include(x => x.DeliveryOrder).ThenInclude(x => x.Booking).Where(x => x.DeliveryChallanId == id).FirstOrDefaultAsync();
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.ComId == comid), "PrdUnitId", "PrdUnitName", deliveryChallan.PrdUnitId);

            //.FindAsync(id);
            if (deliveryChallan == null)
            {
                return NotFound();
            }
            var orderqty = _context.DeliveryOrder.Where(d => d.DOId == deliveryChallan.DOId).FirstOrDefault().Qty;
            var remainingqty = orderqty - deliveryChallan.DeliveryQty;
            var positivevalue = Math.Abs(remainingqty);
            ViewData["remainqty"] = positivevalue;
            //ViewData["DOId"] = new SelectList(_context.DeliveryOrder, "DOId", "DONo", deliveryChallan.DOId);

            return View("Create", deliveryChallan);
        }

        // POST: DeliveryChallan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeliveryChallanId,ChallanNo,DeliveryDate,DeliveryQty,DOId")] DeliveryChallan deliveryChallan)
        {
            if (id != deliveryChallan.DeliveryChallanId)
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
                    if (!DeliveryChallanExists(deliveryChallan.DeliveryChallanId))
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
            // ViewData["DOId"] = new SelectList(_context.DeliveryOrder, "DOId", "DONo", deliveryChallan.DOId);
            return View(deliveryChallan);
        }

        // GET: DeliveryChallan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return NotFound();
            }
            var comid = (HttpContext.Session.GetString("comid"));



            var deliveryChallan = await _context.DeliveryChallan
                .Include(d => d.DeliveryOrder)
                .FirstOrDefaultAsync(m => m.DeliveryChallanId == id);


            if (deliveryChallan == null)
            {
                return NotFound();
            }
            ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits.Where(a => a.isPrdUnit == true && a.ComId == comid), "PrdUnitId", "PrdUnitName", deliveryChallan.PrdUnitId);

            return View("Create", deliveryChallan);
        }

        // POST: DeliveryChallan/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try

            {


                var deliveryChallan = _context.DeliveryChallan.Find(id);
                _context.DeliveryChallan.Remove(deliveryChallan);
                _context.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), deliveryChallan.ChallanNo.ToString(), "Delete", deliveryChallan.DeliveryChallanId.ToString());

                return Json(new { Success = 1, ContactID = deliveryChallan.DeliveryChallanId, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        private bool DeliveryChallanExists(int id)
        {
            return _context.DeliveryChallan.Any(e => e.DeliveryChallanId == id);
        }





        public class DeliveryChallanabc
        {
            public int DeliveryChallanId { get; set; }
            public string DeliveryDate { get; set; }
            public string ChallanNo { get; set; }
            public string GatePassNo { get; set; }

            public string Dealer { get; set; }
            public string DealerCode { get; set; }
            public string FiscalYear { get; set; }
            public string FiscalMonth { get; set; }

            public string District { get; set; }
            public string PoliceStation { get; set; }


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


                    var query = from e in _context.DeliveryChallan.Where(x => x.ComId == comid)
                                .OrderByDescending(x => x.DeliveryChallanId)
                                    //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                let GatePassNo = e.GatePassChallans != null ? e.GatePassChallans.Select(x => new GatePassResult { GatePassNo = x.GatePass.GatePassNo.ToString() }).ToList() : null
                                select new DeliveryChallanabc
                                {
                                    DeliveryChallanId = e.DeliveryChallanId,
                                    DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                    ChallanNo = e.ChallanNo.ToString(),
                                    GatePassList = GatePassNo,

                                    District = e.DeliveryOrder.Booking.Cat_District.DistName,
                                    PoliceStation = e.DeliveryOrder.Booking.Cat_PoliceStation.PStationName,


                                    FiscalYear = e.DeliveryOrder.Booking.YearName.FYName,
                                    FiscalMonth = e.DeliveryOrder.Booking.MonthName.MonthName,

                                    DealerCode = e.DeliveryOrder.Booking.Customer.CustomerCode,
                                    Dealer = e.DeliveryOrder.Booking.Customer.CustomerName,
                                    DeliveryChallanQty = e.DeliveryQty.ToString(),
                                    DeliveryOrderQty = e.DeliveryOrder.Qty.ToString(),
                                    DeliveryOrderNo = e.DeliveryOrder.DONo.ToString(),
                                    RemainingQty = Math.Round(e.DeliveryQty - ((e.GatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.GatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),


                                };


                    var parser = new Parser<DeliveryChallanabc>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {


                    if (FromDate != null && ToDate != null)
                    {
                        var querytest = from e in _context.DeliveryChallan
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DeliveryDate >= dtFrom && p.DeliveryDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        //.Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                        //.Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.DeliveryChallanId)
                                        let GatePassNo = e.GatePassChallans != null ? e.GatePassChallans.Select(x => new GatePassResult { GatePassNo = x.GatePass.GatePassNo.ToString() }).ToList() : null

                                        select new DeliveryChallanabc
                                        {
                                            DeliveryChallanId = e.DeliveryChallanId,
                                            DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                            ChallanNo = e.ChallanNo.ToString(),
                                            GatePassList = GatePassNo,

                                            District = e.DeliveryOrder.Booking.Cat_District.DistName,
                                            PoliceStation = e.DeliveryOrder.Booking.Cat_PoliceStation.PStationName,


                                            FiscalYear = e.DeliveryOrder.Booking.YearName.FYName,
                                            FiscalMonth = e.DeliveryOrder.Booking.MonthName.MonthName,

                                            DealerCode = e.DeliveryOrder.Booking.Customer.CustomerCode,
                                            Dealer = e.DeliveryOrder.Booking.Customer.CustomerName,
                                            DeliveryChallanQty = e.DeliveryQty.ToString(),
                                            DeliveryOrderQty = e.DeliveryOrder.Qty.ToString(),
                                            DeliveryOrderNo = e.DeliveryOrder.DONo.ToString(),
                                            RemainingQty = Math.Round(e.DeliveryQty - ((e.GatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.GatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),


                                        };

                        var parser = new Parser<DeliveryChallanabc>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (FromChallanNo != null && ToChallanNo != null)
                    {

                        var querytest = from e in _context.DeliveryChallan
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.ChallanNo >= FromChallanNo && p.ChallanNo <= ToChallanNo))
                        ////.Where(p => p.userid == UserList)
                        //.Where(p => p.UserId.ToLower().Contains(FromChallanNo.ToString()))


                        .OrderByDescending(x => x.DeliveryChallanId)
                                        let GatePassNo = e.GatePassChallans != null ? e.GatePassChallans.Select(x => new GatePassResult { GatePassNo = x.GatePass.GatePassNo.ToString() }).ToList() : null

                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new DeliveryChallanabc
                                        {
                                            DeliveryChallanId = e.DeliveryChallanId,
                                            DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                            ChallanNo = e.ChallanNo.ToString(),
                                            GatePassList = GatePassNo,

                                            District = e.DeliveryOrder.Booking.Cat_District.DistName,
                                            PoliceStation = e.DeliveryOrder.Booking.Cat_PoliceStation.PStationName,


                                            FiscalYear = e.DeliveryOrder.Booking.YearName.FYName,
                                            FiscalMonth = e.DeliveryOrder.Booking.MonthName.MonthName,

                                            DealerCode = e.DeliveryOrder.Booking.Customer.CustomerCode,
                                            Dealer = e.DeliveryOrder.Booking.Customer.CustomerName,
                                            DeliveryChallanQty = e.DeliveryQty.ToString(),
                                            DeliveryOrderQty = e.DeliveryOrder.Qty.ToString(),
                                            DeliveryOrderNo = e.DeliveryOrder.DONo.ToString()


                                        };


                        var parser = new Parser<DeliveryChallanabc>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in _context.DeliveryChallan
                        .Where(x => x.ComId == comid)
                        .Where(p => (p.DeliveryDate >= dtFrom && p.DeliveryDate <= dtTo))

                        .OrderByDescending(x => x.DeliveryChallanId)
                                        let GatePassNo = e.GatePassChallans != null ? e.GatePassChallans.Select(x => new GatePassResult { GatePassNo = x.GatePass.GatePassNo.ToString() }).ToList() : null

                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                        select new DeliveryChallanabc
                                        {
                                            DeliveryChallanId = e.DeliveryChallanId,
                                            DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                            ChallanNo = e.ChallanNo.ToString(),
                                            GatePassList = GatePassNo,

                                            District = e.DeliveryOrder.Booking.Cat_District.DistName,
                                            PoliceStation = e.DeliveryOrder.Booking.Cat_PoliceStation.PStationName,


                                            FiscalYear = e.DeliveryOrder.Booking.YearName.FYName,
                                            FiscalMonth = e.DeliveryOrder.Booking.MonthName.MonthName,

                                            DealerCode = e.DeliveryOrder.Booking.Customer.CustomerCode,
                                            Dealer = e.DeliveryOrder.Booking.Customer.CustomerName,
                                            DeliveryChallanQty = e.DeliveryQty.ToString(),
                                            DeliveryOrderQty = e.DeliveryOrder.Qty.ToString(),
                                            DeliveryOrderNo = e.DeliveryOrder.DONo.ToString(),
                                            RemainingQty = Math.Round(e.DeliveryQty - ((e.GatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.GatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),


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

                var query = from e in _context.DeliveryChallan.Include(x => x.DeliveryOrder).ThenInclude(x => x.Booking).ThenInclude(x => x.Customer).Where(x => x.DeliveryChallanId > 0 && x.ComId == comid).OrderByDescending(x => x.DeliveryChallanId)
                            let GatePassNo = e.GatePassChallans != null ? e.GatePassChallans.Select(x => new GatePassResult { GatePassNo = x.GatePass.GatePassNo.ToString() }).ToList() : null
                            select new DeliveryChallanabc
                            {
                                DeliveryChallanId = e.DeliveryChallanId,
                                DeliveryDate = e.DeliveryDate.ToString("dd-MMM-yy"),
                                ChallanNo = e.ChallanNo.ToString(),
                                GatePassList = GatePassNo,

                                District = e.DeliveryOrder.Booking.Cat_District.DistName,
                                PoliceStation = e.DeliveryOrder.Booking.Cat_PoliceStation.PStationName,


                                FiscalYear = e.DeliveryOrder.Booking.YearName.FYName,
                                FiscalMonth = e.DeliveryOrder.Booking.MonthName.MonthName,

                                DealerCode = e.DeliveryOrder.Booking.Customer.CustomerCode,
                                Dealer = e.DeliveryOrder.Booking.Customer.CustomerName,
                                DeliveryChallanQty = e.DeliveryQty.ToString(),
                                DeliveryOrderQty = e.DeliveryOrder.Qty.ToString(),
                                DeliveryOrderNo = e.DeliveryOrder.DONo.ToString(),
                                RemainingQty = Math.Round(e.DeliveryQty - ((e.GatePassChallans.Select(x => x.TruckLoadQty) != null ? (e.GatePassChallans.Sum(x => x.TruckLoadQty)) : 0)), 2).ToString("0.00"),

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