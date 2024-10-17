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
    public class BookingController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext _context;

        public BookingController(GTRDBContext context, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;

        }


        public static List<SelectListItem> AllotmentTypeList = new List<SelectListItem>()
        {
        new SelectListItem() {Text="Regular", Value="Regular"},
        new SelectListItem() { Text="Extra", Value="Extra"}
        };

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            string comid = HttpContext.Session.GetString("comid");

            return View(await _context.Booking.Take(1).Where(x => x.ComId == comid).Include(y => y.YearName).Include(m => m.MonthName).Include(d => d.Cat_District).Include(p => p.Cat_PoliceStation).Include(c => c.Customer).ToListAsync());
        }

        // GET: Booking/Details/5
        public IActionResult DealerByDistricAndPolicStation(string AllotmentType, int? Yearid, int? Monthid, int? Districtid, int? PolicStationid, int? fromcode, int? tocode, float? allotmentQty)
        {
            try
            {
                var result = "";
                var comid = HttpContext.Session.GetString("comid");
                //var userid = HttpContext.Session.GetString("userid");
                if (comid == null)
                {
                    result = "Please Login first";
                }
                var quary = $"EXEC BookingDeliveryChallan '{comid}',{Yearid},'{Monthid}','{Districtid}','{PolicStationid}' ,'{allotmentQty}','{AllotmentType}' ";



                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@ComId", comid);
                parameters[1] = new SqlParameter("@YearId", Yearid);
                parameters[2] = new SqlParameter("@MonthId", Monthid);
                parameters[3] = new SqlParameter("@DistId", Districtid);
                parameters[4] = new SqlParameter("@PStationId", PolicStationid);
                parameters[5] = new SqlParameter("@AllotmentQty", allotmentQty);
                parameters[6] = new SqlParameter("@AllotmentType", AllotmentType);


                List<BookingDeliveryChallan> bookingDeliveryChallan = Helper.ExecProcMapTList<BookingDeliveryChallan>("BookingDeliveryChallan", parameters);

                return Json(new { bookingDeliveryChallan, ex = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AllotmentInfo(string AllotmentType, int id, int yearid, int monthid, int PolicStationid)
        {
            try
            {
                DistrictWiseBooking distbooking = _context.DistrictWiseBooking.Where(y => y.DistId == id && y.FiscalYearId == yearid && y.FiscalMonthId == monthid && y.AllotmentType == AllotmentType).FirstOrDefault();
                var PrevAllotmentQtyDist = _context.Booking.Where(y => y.DistId == id && y.FiscalYearId == yearid && y.FiscalMonthId == monthid && y.AllotmentType == AllotmentType).Sum(x => x.AllotmentQty);
                double PrevAllotmentQtyPS = 0.00;
                if (PolicStationid > 0)
                {
                    PrevAllotmentQtyPS = _context.Booking.Where(y => y.PStationId == PolicStationid && y.FiscalYearId == yearid && y.FiscalMonthId == monthid && y.AllotmentType == AllotmentType).Sum(x => x.AllotmentQty);

                }
                else
                {
                    PrevAllotmentQtyPS = _context.Booking.Where(y => y.DistId == id && y.FiscalYearId == yearid && y.FiscalMonthId == monthid && y.AllotmentType == AllotmentType).Sum(x => x.AllotmentQty);
                }
                var PrevAllotmentQty = PrevAllotmentQtyDist - PrevAllotmentQtyPS;

                distbooking.PrevAllotmentQty = PrevAllotmentQty;


                return Json(distbooking);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
        }


        public class BookingDeliveryChallan
        {
            public int? BookingId { get; set; }
            public int? FiscalYearId { get; set; }
            public int? FiscalMonthId { get; set; }
            public int? DistId { get; set; }
            public int? PStationId { get; set; }
            public int? CustomerId { get; set; }
            public int? BookingNo { get; set; }
            public string AllotmentType { get; set; }

            public string Year { get; set; }
            public string Month { get; set; }
            public string DistName { get; set; }
            public string PStationName { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public float? AllotmentQty { get; set; }
            public int? SLNo { get; set; }

        }
        [HttpPost]
        public IActionResult GetMonthByYear(int? FYId)
        {
            var Month = _context.Acc_FiscalMonths.Where(m => m.FYId == FYId).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            if (Month != null)
            {
                foreach (var item in Month)
                {
                    items.Add(new SelectListItem { Value = item.FiscalMonthId.ToString(), Text = item.MonthName });
                }
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult GetPoliceStation(int distid)
        {
            List<Cat_PoliceStation> PoliceStation = _context.Cat_PoliceStation.Include(d => d.Cat_District).Where(p => p.DistId == distid).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            if (PoliceStation != null)
            {
                foreach (Cat_PoliceStation item in PoliceStation)
                {
                    items.Add(new SelectListItem { Value = item.PStationId.ToString(), Text = item.PStationName });
                }
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        public ActionResult DistWiseBookingQtyCheck(string AllotmentType, int? DistId, string Year, string Month)
        {
            var comid = HttpContext.Session.GetString("comid");
            var BookingQty = _context.DistrictWiseBooking.Where(b => b.ComId == comid && b.DistId == DistId && b.FiscalYearId.ToString() == Year && b.FiscalMonthId.ToString() == Month && b.AllotmentType == AllotmentType).ToList();
            List<DistrictWiseBooking> items = new List<DistrictWiseBooking>();
            foreach (DistrictWiseBooking item in BookingQty)
            {
                items.Add(item);
            }
            var Qty = items.Sum(q => q.Qty);

            if (Qty == 0)
            {
                TempData["Message"] = "Entry District Qty First";
                TempData["Status"] = "2";
                return Json(new { Success = 2, ex = TempData["Message"] });
            }
            else
            {

            }
            return Json(Qty);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            var comid = HttpContext.Session.GetString("comid");

            //ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            //var booking = new SelectList(_context.DistrictWiseBooking.Where(x => x.ComId == comid), "DistWiseBookingId", "Qty");
            //ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
            //ViewBag.DistId = new SelectList(_context.Cat_District.OrderBy(d => d.SL), "DistId", "DistName");
            //ViewBag.PStationId = new SelectList(_context.Cat_PoliceStation, "PStationId", "PStationName");
            //ViewBag.CustomerId = new SelectList(_context.Customers.Where(x => x.comid == comid), "CustomerId", "CustomerName");


            var lastbookingdata = _context.Booking.Take(1).Where(x => x.ComId == comid).OrderByDescending(x => x.BookingId).FirstOrDefault();
            if (lastbookingdata != null)
            {
                var samplebookingdata = new Booking();
                //samplebookingdata.FiscalYearId = lastbookingdata.FiscalYearId;
                //samplebookingdata.FiscalMonthId = lastbookingdata.FiscalMonthId;
                ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", lastbookingdata.FiscalYearId);
                ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.FYId == lastbookingdata.FiscalYearId), "FiscalMonthId", "MonthName", lastbookingdata.FiscalMonthId);
                ViewBag.DistId = new SelectList(_context.Cat_District.OrderBy(d => d.SL), "DistId", "DistName", lastbookingdata.DistId);
                ViewBag.PStationId = new SelectList(_context.Cat_PoliceStation.Where(x => x.DistId == lastbookingdata.DistId), "PStationId", "PStationName", lastbookingdata.PStationId);
                ViewData["AllotmentTypeList"] = new SelectList(AllotmentTypeList.OrderByDescending(x => x.Text), "Value", "Text", lastbookingdata.AllotmentType);
                return View();
            }

            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
            ViewBag.DistId = new SelectList(_context.Cat_District.OrderBy(d => d.SL), "DistId", "DistName");
            ViewBag.PStationId = new SelectList(_context.Cat_PoliceStation, "PStationId", "PStationName");
            ViewData["AllotmentTypeList"] = new SelectList(AllotmentTypeList.OrderByDescending(x => x.Text), "Value", "Text");

            return View();
        }


        // POST: Booking/Create
        [HttpPost]
        public IActionResult Create(List<Booking> booking)
        {
            try
            {
                var message = "";
                var result = "";
                //if (ModelState.IsValid)
                //{

                //foreach (var bookigdata in oldbookng)
                //{
                foreach (var item in booking)
                {
                    //var oldbookng = _context.Booking.FirstOrDefault();
                    //if (oldbookng.CustomerId == item.CustomerId && oldbookng.FiscalYearId == item.FiscalYearId && oldbookng.FiscalMonthId == item.FiscalMonthId)
                    //{
                    //    //_context.Update(item);
                    //    message = "Data Alredy Exist";
                    //}

                    if (item.BookingId > 0)
                    {
                        _context.Update(item);
                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), item.BookingId.ToString(), "Update", item.DistId + " " + item.PStationId + " " + item.FiscalYearId + " " + item.FiscalMonthId);


                    }
                    else
                    {
                        _context.Add(item);
                        TempData["Message"] = "Data Save Successfully.";
                        TempData["Status"] = "1";
                        //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), item.BookingId.ToString(), "Save", item.DistId + " " + item.PStationId + " " + item.FiscalYearId + " " + item.FiscalMonthId);


                    }
                    _context.SaveChanges();
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";

                }
                //}
                //}
                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "0", "Save", "0");

                return Json(new { Success = 1, ex = TempData["Message"] });

            }
            catch (Exception ex)
            {

                //throw ex;
                return Json(new { Success = false, ex = ex });

            }

        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(y => y.YearName)
                .Include(m => m.MonthName)
                .Include(d => d.Cat_District)
                .Include(p => p.Cat_PoliceStation)
                .Include(c => c.Customer)
                .Where(b => b.BookingId == id).FirstOrDefaultAsync();
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(y => y.FiscalYearId == booking.FiscalYearId), "FiscalYearId", "FYName", booking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(m => m.FiscalMonthId == booking.FiscalMonthId), "FiscalMonthId", "MonthName", booking.FiscalMonthId);
            ViewBag.DistId = new SelectList(_context.Cat_District.Where(d => d.DistId == booking.DistId), "DistId", "DistName", booking.DistId);
            ViewBag.PStationId = new SelectList(_context.Cat_PoliceStation.Where(s => s.PStationId == booking.PStationId), "PStationId", "PStationName", booking.PStationId);
            ViewBag.CustomerId = new SelectList(_context.Customers.Where(c => c.CustomerId == booking.CustomerId), "CustomerId", "CustomerName", booking.CustomerId);
            ViewBag.AllotmentType = new SelectList(AllotmentTypeList.OrderByDescending(x => x.Text), "Value", "Text", booking.AllotmentType);
            //ViewBag.CustomerName = new SelectList(_context.Customers, "CustomerId", "CustomerName", booking.CustomerId);
            return View("Edit", booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {


                    booking.DateUpdated = DateTime.Now;
                    booking.UpdateByUserId = HttpContext.Session.GetString("userid");

                    _context.Booking.Attach(booking);
                    _context.Entry(booking).Property(x => x.AllotmentQty).IsModified = true;
                    _context.Entry(booking).Property(x => x.DateUpdated).IsModified = true;
                    _context.Entry(booking).Property(x => x.UpdateByUserId).IsModified = true;

                    //_context.SaveChanges();




                    //_context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(y => y.YearName)
                .Include(m => m.MonthName)
                .Include(d => d.Cat_District)
                .Include(p => p.Cat_PoliceStation)
                .Include(c => c.Customer)
                .Where(b => b.BookingId == id).FirstOrDefaultAsync();
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(y => y.FiscalYearId == booking.FiscalYearId), "FiscalYearId", "FYName", booking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(m => m.FiscalMonthId == booking.FiscalMonthId), "FiscalMonthId", "MonthName", booking.FiscalMonthId);
            ViewBag.DistId = new SelectList(_context.Cat_District.Where(d => d.DistId == booking.DistId), "DistId", "DistName", booking.DistId);
            ViewBag.PStationId = new SelectList(_context.Cat_PoliceStation.Where(s => s.PStationId == booking.PStationId), "PStationId", "PStationName", booking.PStationId);
            ViewBag.CustomerId = new SelectList(_context.Customers.Where(c => c.CustomerId == booking.CustomerId), "CustomerId", "CustomerName", booking.CustomerId);
            ViewData["AllotmentTypeList"] = new SelectList(AllotmentTypeList.OrderByDescending(x => x.Text), "Value", "Text", booking.AllotmentType);
            //ViewBag.CustomerName = new SelectList(_context.Customers, "CustomerId", "CustomerName", booking.CustomerId);
            return View("Delete", booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();


            TempData["Message"] = "Data Delete Successfully";
            TempData["Status"] = "3";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), booking.BookingId.ToString(), "Delete", booking.BookingId.ToString());



            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }




        public class BookingList
        {
            public int BookingId { get; set; }
            public string AllotmentType { get; set; }

            public string Year { get; set; }
            public string Month { get; set; }

            public string District { get; set; }
            public string PoliceStation { get; set; }

            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public int? SLNo { get; set; }

            public float AllotmentQty { get; set; }
            public string RemainingQty { get; set; }



        }

        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));
                //var abc = db.Products.Include(y => y.vPrimaryCategory); Include(x=>x.DistrictWiseBooking).Include(x=>x.YearName).Include(x=>x.MonthName).Include(x=>x.Cat_PoliceStation).Include(x=>x.Cat_District).
                var query = from e in _context.Booking.Where(x => x.BookingId > 0 && x.ComId == comid).OrderByDescending(x => x.BookingId)
                            select new BookingList
                            {
                                BookingId = e.BookingId,
                                AllotmentType = e.AllotmentType,

                                Year = e.YearName.FYName,
                                Month = e.MonthName.MonthName,

                                District = e.Cat_District.DistName,
                                PoliceStation = e.Cat_PoliceStation.PStationName,

                                CustomerCode = e.Customer.CustomerCode,
                                CustomerName = e.Customer.CustomerName,
                                SLNo = e.Customer.SLNo,


                                AllotmentQty = e.AllotmentQty,
                                RemainingQty = Math.Round(e.AllotmentQty - ((e.vDeliveryOrder.Select(x => x.Qty) != null ? (e.vDeliveryOrder.Sum(x => x.Qty)) : 0)), 2).ToString("0.00")
                                //e.vDeliveryOrder.Sum(x=>x.Qty)).ToString()
                                //e.AllotmentQty - float.Parse
                            };



                var parser = new Parser<BookingList>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
