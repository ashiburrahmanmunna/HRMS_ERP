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
    public class BufferWishBookingController : Controller
    {

        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }



        public BufferWishBookingController(GTRDBContext context, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;
            //Repository = rep;
        }

        // GET: DistrictWiseBookings
        public async Task<IActionResult> Index(string FiscalYearId, string FiscalMonthId)
        {

            var comid = (HttpContext.Session.GetString("comid"));

            List<BuffertWiseBooking> listBuffertWiseBooking = new List<BuffertWiseBooking>();

            if (FiscalYearId == null)
            {
                var countmax = _context.BuffertWiseBookings.Where(x => x.ComId == comid).Count();

                if (countmax > 0)
                {
                    var maxyear = _context.BuffertWiseBookings.Where(x => x.ComId == comid).Max(x => x.FiscalYearId);
                    var maxmonth = _context.BuffertWiseBookings.Where(x => x.ComId == comid && x.FiscalYearId == maxyear).Max(x => x.FiscalMonthId);

                    listBuffertWiseBooking = _context.BuffertWiseBookings.Take(1).Include(d => d.BufferList).Include(d => d.YearName).Include(d => d.MonthName).Where(x => x.FiscalYearId == maxyear && x.FiscalMonthId == maxmonth && x.ComId == comid).ToList();

                    FiscalYearId = maxyear.ToString();
                    FiscalMonthId = maxmonth.ToString();
                }
                else
                {
                    ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
                    ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.ComId == comid), "FiscalMonthId", "MonthName");
                    ViewData["BufferId"] = new SelectList(_context.Buffers.Where(x => x.BufferListId > 0 && x.ComId == comid), "BufferListId", "BufferName");

                    return View(listBuffertWiseBooking);
                }

            }
            else
            {
                listBuffertWiseBooking = _context.BuffertWiseBookings.Take(1).Include(d => d.BufferList).Include(d => d.YearName).Include(d => d.MonthName).Where(x => x.FiscalYearId.ToString() == FiscalYearId && x.FiscalMonthId.ToString() == FiscalMonthId && x.ComId == comid).ToList();

            }
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.ComId == comid), "FiscalMonthId", "MonthName", FiscalMonthId);
            ViewData["BufferId"] = new SelectList(_context.Buffers.Where(x => x.BufferListId > 0 && x.ComId == comid), "BufferListId", "BufferName");

            return View(listBuffertWiseBooking);
        }



        //[HttpPost, ActionName("SetSessionDistrictWiseBookingReport")]

        //public JsonResult SetSessionDistrictWiseBookingReport(string rptFormat, string action, string DistId, string FiscalYearId, string FiscalMonthId, string AllotmentType)
        //{
        //    try
        //    {
        //        string comid = HttpContext.Session.GetString("comid");

        //        var reportname = "";
        //        var filename = "";
        //        string redirectUrl = "";
        //        //return Json(new { Success = 1, TermsId = param, ex = "" });
        //        if (action == "PrintDistWiseAllotment")
        //        {
        //            //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
        //            reportname = "rptDistWiseAllotment";
        //            filename = "DistWiseAllotment_List_" + DateTime.Now.Date.ToString();
        //            HttpContext.Session.SetString("reportquery", "Exec Sales_rptDistWiseAllotment '" + comid + "','" + DistId + "' ,'" + FiscalYearId + "','" + FiscalMonthId + "' ,'" + AllotmentType + "'  ");
        //            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");

        //        }



        //        HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


        //        string DataSourceName = "DataSet1";

        //        //HttpContext.Session.SetObject("Acc_rptList", postData);

        //        //Common.Classes.clsMain.intHasSubReport = 0;
        //        clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
        //        clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
        //        clsReport.strDSNMain = DataSourceName;

        //        //var ConstrName = "ApplicationServices";
        //        //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
        //        //redirectUrl = callBackUrl;


        //        redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
        //        return Json(new { Url = redirectUrl });

        //    }

        //    catch (Exception ex)
        //    {
        //        // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }

        //    return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
        //    //return RedirectToAction("Index");

        //}





        public class BufferWiseBookingabc
        {
            public int BufferWiseBookingId { get; set; }
            public string Year { get; set; }
            public string Month { get; set; }
            public string Buffer { get; set; }
            public double AllotmentQty { get; set; }
            public float RemainingQty { get; set; }



        }

        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

                //var abc = db.Products.Include(y => y.vPrimaryCategory);
                var query = from e in _context.BuffertWiseBookings.Where(x => x.BufferBookingId > 0 && x.ComId == comid).OrderByDescending(x => x.BufferBookingId)
                            select new BufferWiseBookingabc
                            {
                                BufferWiseBookingId = e.BufferBookingId,
                                Year = e.YearName.FYName,

                                Month = e.MonthName.MonthName,
                                Buffer = e.BufferList.BufferName,
                                AllotmentQty = Math.Round(e.Qty, 2)

                            };



                var parser = new Parser<BufferWiseBookingabc>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public class BufferWiseAllotment
        {
            public int BufferWiseBookingId { get; set; }
            public int FiscalYearId { get; set; }

            public int FiscalMonthId { get; set; }


            public string YearName { get; set; }
            public string MonthName { get; set; }
            public int BufferListId { get; set; }

            public string BufferName { get; set; }

            public string Qty { get; set; }
        }



        [HttpPost]
        public ActionResult GetAllBuffer(int? Year, int? Month)
        {


            var comid = HttpContext.Session.GetString("comid");

            var query = $"EXEC PrcGetBufferWiseBookingAllotment '{comid}', '{Year}' , '{Month}' , ";
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@Year", Year);
            parameters[2] = new SqlParameter("@Month", Month);



            List<BufferWiseAllotment> DeliveryOrderData = Helper.ExecProcMapTList<BufferWiseAllotment>("PrcGetBufferWiseBookingAllotment", parameters);

            return Json(DeliveryOrderData);
        }



        // GET: BufferWiseBooking/Create
        public IActionResult Create()
        {
            var comid = (HttpContext.Session.GetString("comid"));

            int fyid = _context.Acc_FiscalYears.Where(x => x.ComId == comid && x.isRunning == true).Select(x => x.FiscalYearId).FirstOrDefault();


            ViewBag.Title = "Create";
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.ComId == comid), "FiscalMonthId", "MonthName");
            ViewData["BufferId"] = new SelectList(_context.Buffers.Where(x => x.BufferListId > 0 && x.ComId == comid), "BufferListId", "BufferName");


            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(List<BuffertWiseBooking> bufferWiseBooking)
        {
            var comid = (HttpContext.Session.GetString("comid"));
            if (ModelState.IsValid)
            {
                foreach (var item in bufferWiseBooking)
                {
                    if (item.BufferBookingId > 0)
                    {
                        _context.Update(item);

                    }
                    else
                    {
                        _context.Add(item);

                    }
                    _context.SaveChanges();
                }


            }
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(x => x.ComId == comid), "FiscalMonthId", "MonthName");
            ViewData["BufferId"] = new SelectList(_context.Buffers.Where(x => x.BufferListId > 0 && x.ComId == comid), "BufferListId", "BufferName");
            return Json(new { Success = "1", ex = "" });
        }

        // GET: DistrictWiseBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var comid = (HttpContext.Session.GetString("comid"));
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return NotFound();
            }

            var bufferWiseBooking = await _context.BuffertWiseBookings.FindAsync(id);
            if (bufferWiseBooking == null)
            {
                return NotFound();
            }
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", bufferWiseBooking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName", bufferWiseBooking.FiscalMonthId);
            ViewData["BufferId"] = new SelectList(_context.Buffers.Where(x => x.BufferListId > 0 && x.ComId == comid), "BufferListId", "BufferName", bufferWiseBooking.BufferID);


            return View(bufferWiseBooking);
        }

        // POST: DistrictWiseBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BuffertWiseBooking bufferWiseBooking)
        {
            var comid = (HttpContext.Session.GetString("comid"));

            if (ModelState.IsValid)
            {
                try
                {

                    var userid = (HttpContext.Session.GetString("userid"));


                    bufferWiseBooking.UpdateByUserId = userid;
                    bufferWiseBooking.DateUpdated = DateTime.Now;

                    _context.Update(bufferWiseBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BufferWiseBookingExists(bufferWiseBooking.BufferBookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            };

            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
            ViewData["BufferId"] = new SelectList(_context.Buffers.Where(x => x.BufferListId > 0 && x.ComId == comid), "BufferListId", "BufferName");
            return View(bufferWiseBooking);
        }

        // GET: BufferWiseBooking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var comid = (HttpContext.Session.GetString("comid"));
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return NotFound();
            }

            var bufferWiseBooking = await _context.BuffertWiseBookings
                .Include(d => d.BufferList)
                .FirstOrDefaultAsync(m => m.BufferBookingId == id);
            if (bufferWiseBooking == null)
            {
                return NotFound();
            }
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", bufferWiseBooking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName", bufferWiseBooking.FiscalMonthId);
            ViewData["BufferId"] = new SelectList(_context.Buffers.Where(x => x.BufferListId > 0 && x.ComId == comid), "BufferListId", "BufferName", bufferWiseBooking.BufferID);

            return View(bufferWiseBooking);
        }

        // POST: BufferWiseBooking/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var BufferWiseBooking = await _context.BuffertWiseBookings.FindAsync(id);
            _context.BuffertWiseBookings.Remove(BufferWiseBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BufferWiseBookingExists(int id)
        {
            return _context.BuffertWiseBookings.Any(e => e.BufferBookingId == id);
        }


    }
}
