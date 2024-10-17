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
    [OverridableAuthorize]
    public class RepresentativeBookingController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext _context;

        public RepresentativeBookingController(GTRDBContext context, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;

        }




        // GET: RepresentativeBooking
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            string comid = HttpContext.Session.GetString("comid");
            var data = await _context.RepresentativeBooking.Take(1).Where(x => x.ComId == comid).Include(y => y.YearName).Include(m => m.MonthName).Include(b => b.BufferList).Include(x => x.BufferRepresentative).ToListAsync();
            return View(data);
        }


        public IActionResult RepresentativeByBuffer(int? Yearid, int? Monthid, int? BufferListId, int? fromcode, int? tocode, float? allotmentQty)
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
                var quary = $"EXEC RepBookingDeliveryChallan '{comid}',{Yearid},'{Monthid}','{BufferListId}' ,'{allotmentQty}'";



                SqlParameter[] parameters = new SqlParameter[5];

                parameters[0] = new SqlParameter("@ComId", comid);
                parameters[1] = new SqlParameter("@YearId", Yearid);
                parameters[2] = new SqlParameter("@MonthId", Monthid);
                parameters[3] = new SqlParameter("@BufferId", BufferListId);
                parameters[4] = new SqlParameter("@AllotmentQty", allotmentQty);


                List<BufferBookingDelChallan> bookingDeliveryChallan = Helper.ExecProcMapTList<BufferBookingDelChallan>("RepBookingDeliveryChallan", parameters);

                return Json(new { bookingDeliveryChallan, ex = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public JsonResult AllotmentInfo(int id, int yearid, int monthid)
        {
            try
            {
                BuffertWiseBooking bufferbooking = _context.BuffertWiseBookings.Where(y => y.BufferID == id && y.FiscalYearId == yearid && y.FiscalMonthId == monthid).FirstOrDefault();
                var PrevAllotmentQty = _context.RepresentativeBooking.Where(y => y.BufferListId == id && y.FiscalYearId == yearid && y.FiscalMonthId == monthid).Sum(x => x.AllotmentQty);



                bufferbooking.PrevAllotmentQty = PrevAllotmentQty;


                return Json(bufferbooking);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
        }



        public class BufferBookingDelChallan
        {
            public int? RepresentativeBookingId { get; set; }
            public int? FiscalYearId { get; set; }
            public int? FiscalMonthId { get; set; }
            public int? BufferListId { get; set; }
            public int? BufferRepresentativeId { get; set; }
            public int? BookingNo { get; set; }

            public string Year { get; set; }
            public string Month { get; set; }
            public string BufferName { get; set; }
            public string RepresentativeCode { get; set; }
            public string RepresentativeName { get; set; }
            public float? AllotmentQty { get; set; }

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



        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            var comid = HttpContext.Session.GetString("comid");



            var lastbookingdata = _context.RepresentativeBooking.Take(1).Where(x => x.ComId == comid).OrderByDescending(x => x.RepresentativeBookingId).FirstOrDefault();
            if (lastbookingdata != null)
            {
                var samplebookingdata = new RepresentativeBooking();
                //samplebookingdata.FiscalYearId = lastbookingdata.FiscalYearId;
                //samplebookingdata.FiscalMonthId = lastbookingdata.FiscalMonthId;
                ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", lastbookingdata.FiscalYearId);
                ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.FYId == lastbookingdata.FiscalYearId), "FiscalMonthId", "MonthName", lastbookingdata.FiscalMonthId);
                ViewBag.BufferId = new SelectList(_context.Buffers.Where(x => x.ComId == comid), "BufferListId", "BufferName", lastbookingdata.BufferListId);

                return View();
            }

            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.ComId == comid), "FiscalMonthId", "MonthName");
            ViewBag.BufferId = new SelectList(_context.Buffers.Where(x => x.ComId == comid), "BufferListId", "BufferName");

            return View();
        }


        // POST: Booking/Create
        [HttpPost]
        public IActionResult Create(List<RepresentativeBooking> booking)
        {
            try
            {
                var message = "";
                var result = "";

                foreach (var item in booking)
                {

                    if (item.RepresentativeBookingId > 0)
                    {
                        _context.Update(item);
                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";



                    }
                    else
                    {
                        _context.Add(item);

                        TempData["Message"] = "Data Save Successfully.";
                        TempData["Status"] = "1";


                    }

                    _context.SaveChanges();



                }



                return Json(new { Success = 1, ex = TempData["Message"] });

            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex });

            }

        }






        public async Task<IActionResult> Edit(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.RepresentativeBooking
                .Include(y => y.YearName)
                .Include(m => m.MonthName)
                .Include(d => d.BufferList)
                .Include(p => p.BufferRepresentative)
                .Where(b => b.RepresentativeBookingId == id).FirstOrDefaultAsync();
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(y => y.FiscalYearId == booking.FiscalYearId), "FiscalYearId", "FYName", booking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(m => m.FiscalMonthId == booking.FiscalMonthId), "FiscalMonthId", "MonthName", booking.FiscalMonthId);
            ViewBag.BufferId = new SelectList(_context.Buffers.Where(d => d.BufferListId == booking.BufferListId), "BufferListId", "BufferName", booking.BufferListId);
            ViewBag.RepresentativeId = new SelectList(_context.BuferRepresentative.Where(c => c.BufferRepresentativeId == booking.BufferRepresentativeId), "BufferRepresentativeId", "RepresentativeName", booking.BufferRepresentativeId);
            return View("Edit", booking);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, RepresentativeBooking booking)
        {
            if (id != booking.RepresentativeBookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {


                    booking.DateUpdated = DateTime.Now;
                    booking.UpdateByUserId = HttpContext.Session.GetString("userid");

                    _context.RepresentativeBooking.Attach(booking);
                    _context.Entry(booking).Property(x => x.AllotmentQty).IsModified = true;
                    _context.Entry(booking).Property(x => x.DateUpdated).IsModified = true;
                    _context.Entry(booking).Property(x => x.UpdateByUserId).IsModified = true;

                    _context.SaveChanges();




                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.RepresentativeBooking
                .Include(y => y.YearName)
                .Include(m => m.MonthName)
                .Include(d => d.BufferList)
                .Include(p => p.BufferRepresentative)
                .Where(b => b.RepresentativeBookingId == id).FirstOrDefaultAsync();
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(y => y.FiscalYearId == booking.FiscalYearId), "FiscalYearId", "FYName", booking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(m => m.FiscalMonthId == booking.FiscalMonthId), "FiscalMonthId", "MonthName", booking.FiscalMonthId);
            ViewBag.BufferId = new SelectList(_context.Buffers.Where(d => d.BufferListId == booking.BufferListId), "BufferListId", "BufferName", booking.BufferListId);
            ViewBag.RepresentativeId = new SelectList(_context.BuferRepresentative.Where(c => c.BufferRepresentativeId == booking.BufferRepresentativeId), "BufferRepresentativeId", "RepresentativeName", booking.BufferRepresentativeId);
            return View("Delete", booking);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.RepresentativeBooking.FindAsync(id);
            _context.RepresentativeBooking.Remove(booking);
            await _context.SaveChangesAsync();


            TempData["Message"] = "Data Delete Successfully";
            TempData["Status"] = "3";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), booking.RepresentativeBookingId.ToString(), "Delete", booking.RepresentativeBookingId.ToString());



            return RedirectToAction(nameof(Index));
        }


        private bool BookingExists(int id)
        {
            return _context.RepresentativeBooking.Any(e => e.RepresentativeBookingId == id);
        }



        public class RepresentativeBookingList
        {
            public int RepresentativeBookingId { get; set; }

            public string Year { get; set; }
            public string Month { get; set; }

            public string Buffer { get; set; }

            public string RepresentativeCode { get; set; }
            public string RepresentativeName { get; set; }

            public float AllotmentQty { get; set; }
            public string RemainingQty { get; set; }



        }



        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));
                //var abc = db.Products.Include(y => y.vPrimaryCategory); Include(x=>x.DistrictWiseBooking).Include(x=>x.YearName).Include(x=>x.MonthName).Include(x=>x.Cat_PoliceStation).Include(x=>x.Cat_District).
                var query = from e in _context.RepresentativeBooking.Where(x => x.RepresentativeBookingId > 0 && x.ComId == comid).OrderByDescending(x => x.RepresentativeBookingId)
                            select new RepresentativeBookingList
                            {
                                RepresentativeBookingId = e.RepresentativeBookingId,

                                Year = e.YearName.FYName,
                                Month = e.MonthName.MonthName,

                                Buffer = e.BufferList.BufferName,
                                RepresentativeCode = e.BufferRepresentative.RepresentativeCode,
                                RepresentativeName = e.BufferRepresentative.RepresentativeName,


                                AllotmentQty = e.AllotmentQty,
                                RemainingQty = Math.Round(e.AllotmentQty - ((e.BufferDeliveryOrder.Select(x => x.Qty) != null ? (e.BufferDeliveryOrder.Sum(x => x.Qty)) : 0)), 2).ToString("0.00")
                                //e.vDeliveryOrder.Sum(x=>x.Qty)).ToString()
                                //e.AllotmentQty - float.Parse
                            };



                var parser = new Parser<RepresentativeBookingList>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult BufferWiseBookingQtyCheck(int? BufferId, string Year, string Month)
        {
            var comid = HttpContext.Session.GetString("comid");
            var BookingQty = _context.BuffertWiseBookings.Where(b => b.ComId == comid && b.BufferID == BufferId && b.FiscalYearId.ToString() == Year && b.FiscalMonthId.ToString() == Month).ToList();
            List<BuffertWiseBooking> items = new List<BuffertWiseBooking>();
            foreach (BuffertWiseBooking item in BookingQty)
            {
                items.Add(item);
            }
            var Qty = items.Sum(q => q.Qty);

            if (Qty == 0)
            {
                TempData["Message"] = "Entry Buffer Qty First";
                TempData["Status"] = "2";
                return Json(new { Success = 2, ex = TempData["Message"] });
            }
            else
            {

            }
            return Json(Qty);
        }






    };



};
