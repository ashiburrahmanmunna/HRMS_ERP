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
    public class DistrictWiseBookingsController : Controller
    {
        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }



        public DistrictWiseBookingsController(GTRDBContext context, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;
            //Repository = rep;
        }

        // GET: DistrictWiseBookings
        public async Task<IActionResult> Index(string AllotmentType, string FiscalYearId, string FiscalMonthId)
        {

            var comid = (HttpContext.Session.GetString("comid"));

            List<DistrictWiseBooking> listDistwiseallotment = new List<DistrictWiseBooking>();

            if (FiscalYearId == null)
            {
                var countmax = _context.DistrictWiseBooking.Where(x => x.ComId == comid).Count();

                if (countmax > 0)
                {
                    var maxyear = _context.DistrictWiseBooking.Where(x => x.ComId == comid).Max(x => x.FiscalYearId);
                    var maxmonth = _context.DistrictWiseBooking.Where(x => x.ComId == comid && x.FiscalYearId == maxyear).Max(x => x.FiscalMonthId);

                    listDistwiseallotment = _context.DistrictWiseBooking.Take(1).Include(d => d.Cat_District).Include(d => d.YearName).Include(d => d.MonthName).Where(x => x.FiscalYearId == maxyear && x.FiscalMonthId == maxmonth && x.ComId == comid).ToList();

                    FiscalYearId = maxyear.ToString();
                    FiscalMonthId = maxmonth.ToString();
                }
                else
                {
                    ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
                    ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
                    ViewData["DistId"] = new SelectList(_context.Cat_District.Where(x => x.DistId > 0), "DistId", "DistName");
                    ViewData["AllotmentType"] = new SelectList(AllotmentTypeList, "Value", "Text");

                    return View(listDistwiseallotment);
                }

            }
            else
            {
                listDistwiseallotment = _context.DistrictWiseBooking.Take(1).Include(d => d.Cat_District).Include(d => d.YearName).Include(d => d.MonthName).Where(x => x.FiscalYearId.ToString() == FiscalYearId && x.FiscalMonthId.ToString() == FiscalMonthId && x.ComId == comid).ToList();

            }
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName", FiscalMonthId);
            ViewData["DistId"] = new SelectList(_context.Cat_District, "DistId", "DistName");
            ViewData["AllotmentType"] = new SelectList(AllotmentTypeList, "Value", "Text");

            return View(listDistwiseallotment);
        }



        [HttpPost, ActionName("SetSessionDistrictWiseBookingReport")]

        public JsonResult SetSessionDistrictWiseBookingReport(string rptFormat, string action, string DistId, string FiscalYearId, string FiscalMonthId, string AllotmentType)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintDistWiseAllotment")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptDistWiseAllotment";
                    filename = "DistWiseAllotment_List_" + DateTime.Now.Date.ToString();
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptDistWiseAllotment '" + comid + "','" + DistId + "' ,'" + FiscalYearId + "','" + FiscalMonthId + "' ,'" + AllotmentType + "'  ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");

                }



                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


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





        public class DistrictWiseBookingabc
        {
            public int DistWiseBookingId { get; set; }
            public string AllotmentType { get; set; }

            public string Year { get; set; }
            public string Month { get; set; }
            public string District { get; set; }
            public double AllotmentQty { get; set; }
            public float RemainingQty { get; set; }



        }

        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

                //var abc = db.Products.Include(y => y.vPrimaryCategory);
                var query = from e in _context.DistrictWiseBooking.Where(x => x.DistWiseBookingId > 0 && x.ComId == comid).OrderByDescending(x => x.DistWiseBookingId)
                            select new DistrictWiseBookingabc
                            {
                                DistWiseBookingId = e.DistWiseBookingId,
                                Year = e.YearName.FYName,
                                AllotmentType = e.AllotmentType,
                                Month = e.MonthName.MonthName,
                                District = e.Cat_District.DistName,
                                AllotmentQty = Math.Round(e.Qty, 2)
                            };



                var parser = new Parser<DistrictWiseBookingabc>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public class DistrictWiseAllotment
        {
            public int DistWiseBookingId { get; set; }
            public int FiscalYearId { get; set; }

            public int FiscalMonthId { get; set; }
            public string AllotmentType { get; set; }


            public string YearName { get; set; }
            public string MonthName { get; set; }
            public int DistId { get; set; }

            public string DistName { get; set; }

            public string Qty { get; set; }
        }



        [HttpPost]
        public ActionResult GetAllDistrict(string AllotmentType, int? Year, int? Month)
        {
            //string comid = HttpContext.Session.GetString("comid");

            //var DistBookingAllotmentList = (from distwiseallotment in _context.Cat_District.Where(m => m.DistId >  0)
            //              where !_context.DistrictWiseBooking.Where(s => s.YearName.YearBng == Year.ToString() && s.MonthName.Name == Month && s.ComId == comid).Any(f => f.DistId == distwiseallotment.DistId)
            //              select distwiseallotment).ToList();

            //foreach (var item in DistBookingAllotmentList)

            //{
            //    //DistrictWiseBooking distwisebooking = new DistrictWiseBooking();
            //    //distwisebooking.YearName.FYName = Year.ToString();
            //    //distwisebooking.Qty = 0;
            //    //distwisebooking.MonthName.Name = Month;

            //    //distwisebooking.Cat_District = new Cat_District { DistId = item.DistId, DistName = item.DistName,  = item. };

            //}


            var comid = HttpContext.Session.GetString("comid");

            var query = $"EXEC PrcGetDistWiseBookingAllotment '{comid}', '{Year}' , '{Month}' , '{AllotmentType}'";
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@Year", Year);
            parameters[2] = new SqlParameter("@Month", Month);
            parameters[3] = new SqlParameter("@AllotmentType", AllotmentType);


            List<DistrictWiseAllotment> DeliveryOrderData = Helper.ExecProcMapTList<DistrictWiseAllotment>("PrcGetDistWiseBookingAllotment", parameters);

            //return Json(DeliveryOrderData);



            //var DistrictList = _context.Cat_District.Select(d=> new 
            //{
            //    DistId = d.DistId,
            //    DistName = d.DistName,
            //    YearName = Year,
            //    MonthName = Month
            //}).ToList();
            return Json(DeliveryOrderData);
        }

        public static List<SelectListItem> AllotmentTypeList = new List<SelectListItem>()
        {
        new SelectListItem() {Text="Regular", Value="Regular"},
        new SelectListItem() { Text="Extra", Value="Extra"}
        };

        // GET: DistrictWiseBookings/Create
        public IActionResult Create()
        {
            var comid = (HttpContext.Session.GetString("comid"));

            int fyid = _context.Acc_FiscalYears.Where(x => x.ComId == comid && x.isRunning == true).Select(x => x.FiscalYearId).FirstOrDefault();


            ViewBag.Title = "Create";
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");
            ViewData["DistId"] = new SelectList(_context.Cat_District.Where(x => x.DistId > 0), "DistId", "DistName");
            ViewData["AllotmentType"] = new SelectList(AllotmentTypeList, "Value", "Text");
            return View();
        }

        // POST: DistrictWiseBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(List<DistrictWiseBooking> districtWiseBooking)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in districtWiseBooking)
                {
                    if (item.DistWiseBookingId > 0)
                    {
                        _context.Update(item);

                    }
                    else
                    {
                        _context.Add(item);

                    }
                    _context.SaveChanges();
                }

                // return RedirectToAction(nameof(Index));
            }
            //ViewBag.FiscalYearId = new SelectList(_context.YearNames, "FiscalYearId", "FYName", districtWiseBooking.FiscalYearId);
            //ViewBag.FiscalMonthId = new SelectList(_context.MonthNames, "FiscalMonthId", "MonthName", districtWiseBooking.FiscalMonthId);
            //ViewData["DistId"] = new SelectList(_context.Cat_District, "DistId", "DistName", districtWiseBooking.DistId);
            return Json(new { Success = "1", ex = "" });
        }

        // GET: DistrictWiseBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return NotFound();
            }

            var districtWiseBooking = await _context.DistrictWiseBooking.FindAsync(id);
            if (districtWiseBooking == null)
            {
                return NotFound();
            }
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(y => y.FiscalYearId == districtWiseBooking.FiscalYearId), "FiscalYearId", "FYName", districtWiseBooking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(m => m.FiscalMonthId == districtWiseBooking.FiscalMonthId), "FiscalMonthId", "MonthName", districtWiseBooking.FiscalMonthId);
            ViewData["DistId"] = new SelectList(_context.Cat_District.Where(d => d.DistId == districtWiseBooking.DistId), "DistId", "DistName", districtWiseBooking.DistId);
            ViewData["AllotmentType"] = new SelectList(AllotmentTypeList, "Value", "Text", districtWiseBooking.AllotmentType);


            return View(districtWiseBooking);
        }

        // POST: DistrictWiseBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DistrictWiseBooking districtWiseBooking)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var userid = (HttpContext.Session.GetString("userid"));


                    districtWiseBooking.UpdateByUserId = userid;
                    districtWiseBooking.DateUpdated = DateTime.Now;

                    _context.Update(districtWiseBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictWiseBookingExists(districtWiseBooking.DistWiseBookingId))
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
            ViewBag.FiscalYearId = new SelectList(_context.Acc_FiscalYears.Where(y => y.FiscalYearId == districtWiseBooking.FiscalYearId), "FiscalYearId", "FYName", districtWiseBooking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.Acc_FiscalMonths.Where(m => m.FiscalMonthId == districtWiseBooking.FiscalMonthId), "FiscalMonthId", "MonthName", districtWiseBooking.FiscalMonthId);
            ViewData["DistId"] = new SelectList(_context.Cat_District.Where(d => d.DistId == districtWiseBooking.DistId), "DistId", "DistName", districtWiseBooking.DistId);
            ViewData["AllotmentType"] = new SelectList(AllotmentTypeList, "Value", "Text", districtWiseBooking.AllotmentType);

            return View(districtWiseBooking);
        }

        // GET: DistrictWiseBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return NotFound();
            }

            var districtWiseBooking = await _context.DistrictWiseBooking
                .Include(d => d.Cat_District)
                .FirstOrDefaultAsync(m => m.DistWiseBookingId == id);
            if (districtWiseBooking == null)
            {
                return NotFound();
            }
            ViewBag.FiscalYearId = new SelectList(_context.YearNames, "FiscalYearId", "FYName", districtWiseBooking.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(_context.MonthNames, "FiscalMonthId", "MonthName", districtWiseBooking.FiscalMonthId);
            ViewData["DistId"] = new SelectList(_context.Cat_District, "DistId", "DistName", districtWiseBooking.DistId);
            ViewData["AllotmentType"] = new SelectList(AllotmentTypeList, "Value", "Text", districtWiseBooking.AllotmentType);
            return View(districtWiseBooking);
        }

        // POST: DistrictWiseBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var districtWiseBooking = await _context.DistrictWiseBooking.FindAsync(id);
            _context.DistrictWiseBooking.Remove(districtWiseBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictWiseBookingExists(int id)
        {
            return _context.DistrictWiseBooking.Any(e => e.DistWiseBookingId == id);
        }
    }
}
