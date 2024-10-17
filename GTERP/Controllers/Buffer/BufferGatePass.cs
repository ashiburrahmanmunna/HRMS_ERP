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
    public class BufferGatePassController : Controller
    {
        private readonly GTRDBContext _context;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }
        //public CommercialRepository Repository { get; set; }

        public BufferGatePassController(GTRDBContext context, clsProcedure _clsProc, TransactionLogRepository tran)
        {
            _context = context;
            tranlog = tran;
            clsProc = _clsProc;
            //Repository = rep;
        }

        // GET: GatePasses
        public async Task<IActionResult> Index()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            var gTRDBContext = _context.BufferGatePass.Where(x => x.ComId == comid).Take(1);

            ViewBag.Year = new SelectList(_context.Acc_FiscalYears.OrderByDescending(y => y.FiscalYearId), "FiscalYearId", "FYName");
            ViewBag.DONo = new SelectList(_context.BufferDelOrder, "DOId", "DONo");

            ViewBag.GatePassId = new SelectList(_context.BufferGatePass, "BufferGatePassId", "GatePassNo");
            ViewData["BufferId"] = new SelectList(_context.Buffers, "BufferListId", "BufferName");
            ViewBag.Month = new SelectList(_context.Acc_FiscalMonths, "FiscalMonthId", "MonthName");


            var recivernamedb = _context.BufferGatePass.Where(x => x.ComId == comid)
               .Select(m => new { m.ReceiverName })
               .Distinct()
               .ToList();


            List<SelectListItem> receiverpersonlist = new List<SelectListItem>();
            if (recivernamedb != null)
            {
                foreach (var x in recivernamedb)
                {
                    receiverpersonlist.Add(new SelectListItem { Text = x.ReceiverName, Value = "0" });

                }
            }
            ViewBag.ReceiverPerson = new SelectList(receiverpersonlist, "Value", "Text");


            return View(await gTRDBContext.ToListAsync());
        }
        public class GatePassabc
        {
            public int GatePassId { get; set; }
            public int GatePassNo { get; set; }
            public string ReceiverName { get; set; }
            public string ReceiverAddress { get; set; }
            //public string GatePassFrom { get; set; }
            public string GatePassDate { get; set; }
            public float TotalQty { get; set; }
            public string Status { get; set; }


            public List<BufferDelChallan> DeliveryChallanList { get; set; }

        }
        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

                var query = from e in _context.BufferGatePass.Where(x => x.BufferGatePassId > 0 && x.ComId == comid)
                            let ChallanNo = e.BufferChallans != null ? e.BufferChallans.Select(x => new BufferDelChallan { ChallanNo = x.BufferDelChallan.ChallanNo }).ToList() : null

                            select new GatePassabc
                            {
                                GatePassId = e.BufferGatePassId,
                                GatePassNo = e.GatePassNo,
                                ReceiverName = e.BufferChallans.FirstOrDefault().BufferDelChallan.BufferDelOrder.BufferRepresentative.RepresentativeName,
                                ReceiverAddress = e.ReceiverAddress,
                                GatePassDate = e.GatePassDate.ToString("dd-MMM-yy"),
                                DeliveryChallanList = ChallanNo,
                                TotalQty = e.TotalQty,
                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",

                            };

                var parser = new Parser<GatePassabc>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gatePass = await _context.BufferGatePass
                .FirstOrDefaultAsync(m => m.BufferGatePassId == id);
            if (gatePass == null)
            {
                return NotFound();
            }

            return View(gatePass);
        }


        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            var lastGatePassDataUserWise = _context.BufferGatePass.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.BufferGatePassId).FirstOrDefault();
            var lastGatePassData = _context.BufferGatePass.Where(x => x.ComId == comid).OrderByDescending(D => D.BufferGatePassId).FirstOrDefault();

            if (lastGatePassData == null)
            {
                //GatePass abc = new GatePass();
                //abc.GatePassDate = DateTime.Now.Date;

                lastGatePassData = new BufferGatePass();
                lastGatePassData.GatePassNo = 100001;
                lastGatePassData.GatePassDate = DateTime.Now.Date;

                ViewData["FiscalYearId"] = new SelectList(_context.Acc_FiscalYears.Where(a => a.ComId == comid), "FiscalYearId", "FYName");
                ViewData["FiscalMonthId"] = new SelectList(_context.Acc_FiscalMonths.Take(0).Where(p => p.ComId == comid), "FiscalMonthId", "MonthName");
                ViewData["BufferId"] = new SelectList(_context.Buffers.Take(0).Where(p => p.ComId == comid), "BufferListId", "BufferName");
            }
            else
            {
                lastGatePassData.BufferGatePassId = 0;
                lastGatePassData.GatePassNo = _context.BufferGatePass.Where(x => x.ComId == comid).Max(x => x.GatePassNo);
                if (lastGatePassDataUserWise != null)
                {
                    lastGatePassData.GatePassDate = lastGatePassDataUserWise.GatePassDate;

                }
                else
                {

                }

                ViewData["FiscalYearId"] = new SelectList(_context.Acc_FiscalYears.Where(a => a.ComId == comid), "FiscalYearId", "FYName", lastGatePassData.FiscalYearId);
                ViewData["FiscalMonthId"] = new SelectList(_context.Acc_FiscalMonths.Where(p => p.ComId == comid && p.FYId == lastGatePassData.FiscalYearId), "FiscalMonthId", "MonthName", lastGatePassData.FiscalMonthId);
                ViewData["BufferId"] = new SelectList(_context.Buffers.Take(0).Where(p => p.ComId == comid), "BufferListId", "BufferName");


            }

            var Receiver = _context.BufferGatePass.Select(r => r.ReceiverName).Distinct().ToList();

            List<SelectListItem> ReceiverName = new List<SelectListItem>();

            if (Receiver != null)
            {
                foreach (var receiver in Receiver)
                {
                    ReceiverName.Add(new SelectListItem { Text = receiver, Value = "0" });

                }
            }

            ViewBag.Receiver = ReceiverName;



            return View(lastGatePassData);
        }

        [ValidateAntiForgeryToken]

        public ActionResult LoadFiscalMonth(int fyid)
        {
            var comid = HttpContext.Session.GetString("comid");
            var fm = _context.Acc_FiscalMonths.Where(x => x.ComId == comid).Where(x => x.FYId == fyid).ToList();
            return Json(new { fmonth = fm });
        }
        [ValidateAntiForgeryToken]

        public ActionResult ComboInitialize()
        {
            var comid = HttpContext.Session.GetString("comid");
            //var fyear = _context.Acc_FiscalYears.Where(x => x.comid == comid).OrderByDescending(x => x.FYId).ToList();
            var buffer = _context.Buffers.Where(x => x.ComId == comid).ToList();

            //return Json(new {FYear= fyear, District=district});
            return Json(new { Buffer = buffer });

        }

        [ValidateAntiForgeryToken]
        public ActionResult GetChallanDetails(int yearid, int monthid, string bufferid)
        {
            try
            {
                if (bufferid == null)
                {
                    bufferid = "";
                }


                //int yearid,int monthid

                var comid = HttpContext.Session.GetString("comid");
                var query = $"EXEC BufferDelChallanDetailsInformation '{comid}', '{yearid}' ,'{monthid}','{bufferid}'";


                SqlParameter[] sqlParameter = new SqlParameter[4];
                sqlParameter[0] = new SqlParameter("@ComId", comid);
                sqlParameter[1] = new SqlParameter("@YearID", yearid);
                sqlParameter[2] = new SqlParameter("@MonthID", monthid);
                sqlParameter[3] = new SqlParameter("@BufferId", bufferid);



                var res = Helper.ExecProcMapTList<BufferDelChallanDetailInfo>("BufferDelChallanDetailsInformation", sqlParameter);

                return Json(new { data = res, success = 1 });

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = 2 });

                //throw ex;
            }
        }


        internal class BufferDelChallanDetailInfo
        {

            public int Id { get; set; }
            public int DeliveryChallanId { get; set; }
            public int GatePassSubId { get; set; }
            public int FiscalMonthId { get; set; }
            public string RepresentativeCode { get; set; }
            public int FYId { get; set; }
            public int BufferId { get; set; }
            public int TruckLoadQty { get; set; }
            public string FyName { get; set; }
            public string MonthName { get; set; }
            public int DONo { get; set; }

            public int ChallanNo { get; set; }
            public string RepresentativeName { get; set; }
            public string BufferName { get; set; }
            public decimal BalanceQty { get; set; }

            public decimal DeliveryQty { get; set; }




        }


        // POST: GatePasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BufferGatePass gatePass)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });
            // if (ModelState.IsValid)
            // {

            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");

                if (gatePass.BufferGatePassId > 0)
                {

                    var CurrentGatePassSubs = _context.BufferGatePassSub.Where(p => p.BufferGatePassId == gatePass.BufferGatePassId).ToList();


                    gatePass.DateUpdated = DateTime.Now;
                    gatePass.UpdateByUserId = userid;

                    //gatePass.UpdateByUserId = DateTime.Now.Date;


                    _context.BufferGatePassSub.RemoveRange(CurrentGatePassSubs);
                    _context.SaveChanges();


                    //if (gatePass.Challans != null)
                    //{
                    //    foreach (GatePassSub ss in gatePass.Challans)
                    //    {
                    //        //ss.DateAdded = DateTime.Now;
                    //        //ss.DateUpdated = DateTime.Now;
                    //        ss.GatePassId = gatePass.GatePassId;
                    //        _context.GatePassSub.Add(ss);
                    //        _context.SaveChanges();
                    //    }
                    //}
                    //_context.Entry(gatePass).State = EntityState.Modified;
                    //_context.SaveChanges();



                    foreach (BufferGatePassSub ss in gatePass.BufferChallans)
                    {
                        //if (ss.VoucherSubId > 0)
                        //{
                        //db.Entry(ss).State = EntityState.Modified;
                        //ss.GatePassId = 0;
                        ss.BufferGatePassId = gatePass.BufferGatePassId;
                        ss.BufferGatePassSubId = 0;

                        _context.BufferGatePassSub.Add(ss);
                        await _context.SaveChangesAsync();

                    }



                    _context.Entry(gatePass).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    //_context.Add(gatePass);
                    //await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));

                    ViewData["Message"] = "Data Update Successfully.";
                    TempData["Message"] = "Data Update Successfully.";

                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), gatePass.BufferGatePassId.ToString(), "Update", gatePass.GatePassNo.ToString());


                    return Json(new { Success = 1, BufferGatePassId = gatePass.BufferGatePassId, ex = "Data Update Successfully" });
                }
                else
                {



                    foreach (var item in gatePass.BufferChallans)
                    {
                        var challandata = _context.BufferDelChallan.Where(x => x.BufferDelChallanId == item.BufferDelChallanId).FirstOrDefault();
                        var PrevLoadQty = _context.BufferGatePassSub.Where(x => x.BufferDelChallan.BufferDelChallanId == item.BufferDelChallanId).Sum(x => x.TruckLoadQty);
                        var currLoadQty = item.TruckLoadQty;

                        if (PrevLoadQty == null)
                        {
                            PrevLoadQty = 0;
                        }
                        if (currLoadQty == null)
                        {
                            currLoadQty = 0;
                        }
                        var totalqty = Math.Round((PrevLoadQty + currLoadQty), 3); ;


                        if (Math.Round(challandata.DeliveryQty, 3) < Math.Round((PrevLoadQty + currLoadQty), 3))
                        {
                            return Json(new { Success = 0, ex = "Delivery Challan : " + challandata.ChallanNo.ToString() + " Have not Sufficient Balance to meet this Transaction." });
                        }


                    }



                    var lastGatePassNo = _context.BufferGatePass.Where(x => x.ComId == comid).OrderByDescending(x => x.BufferGatePassId).FirstOrDefault();
                    if (lastGatePassNo == null)
                    {
                        gatePass.GatePassNo = 100001;
                    }
                    else
                    {
                        var currentGatePassNoForSave = _context.BufferGatePass.Where(x => x.ComId == comid).Max(x => x.GatePassNo) + 1;
                        gatePass.GatePassNo = currentGatePassNoForSave;
                    }

                    gatePass.DateAdded = DateTime.Now;
                    gatePass.UserId = userid;


                    _context.Add(gatePass);
                    await _context.SaveChangesAsync();


                    ViewData["Message"] = "Data Save Successfully.";
                    TempData["Message"] = "Data Save Successfully.";

                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), gatePass.BufferGatePassId.ToString(), "Create", gatePass.GatePassNo.ToString());

                    var result = new
                    {
                        Success = 1,
                        GatePassId = gatePass.BufferGatePassId,
                        GatePassNo = gatePass.GatePassNo,
                        TruckNumber = gatePass.TruckNumber,
                        ex = "Data Save Successfully"
                    };
                    return Json(result);

                    //return RedirectToAction(nameof(Index));

                }
                //}


            }
            catch (Exception ex)
            {

                throw ex;
                //return View(gatePass);
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        // GET: GatePasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            var fyear = _context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(x => x.FYId).ToList();
            var buffer = _context.Buffers.Where(x => x.ComId == comid).ToList();




            var gatePass = await _context.BufferGatePass
                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.BufferList)
                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.MonthName)

                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.YearName)

                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.BufferRepresentative)


                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.BufferRepresentative)

                .Where(x => x.BufferGatePassId == id)
                .FirstOrDefaultAsync();




            var Receiver = _context.BufferGatePass.Select(r => r.ReceiverName).Distinct().ToList();

            List<SelectListItem> ReceiverName = new List<SelectListItem>();

            if (Receiver != null)
            {
                foreach (var receiver in Receiver)
                {
                    ReceiverName.Add(new SelectListItem { Text = receiver, Value = "0" });

                }
            }

            ViewBag.Receiver = ReceiverName;



            ViewData["FiscalYearId"] = new SelectList(_context.Acc_FiscalYears.Where(a => a.ComId == comid), "FiscalYearId", "FYName", gatePass.FiscalYearId);
            ViewData["FiscalMonthId"] = new SelectList(_context.Acc_FiscalMonths.Where(p => p.ComId == comid), "FiscalMonthId", "MonthName", gatePass.FiscalMonthId);
            ViewData["BufferId"] = new SelectList(_context.Buffers.Take(0).Where(p => p.ComId == comid), "BufferListId", "BufferName");






            if (gatePass == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            return View("Create", gatePass);
        }



        // GET: GatePasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            var fyear = _context.Acc_FiscalYears.Where(x => x.ComId == comid).OrderByDescending(x => x.FYId).ToList();
            var buffer = _context.Buffers.Where(x => x.ComId == comid).ToList();




            var gatePass = await _context.BufferGatePass
                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.BufferList)
                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.MonthName)

                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.YearName)

                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.RepresentativeBooking)
                .ThenInclude(x => x.BufferRepresentative)


                .Include(x => x.BufferChallans)
                .ThenInclude(x => x.BufferDelChallan)
                .ThenInclude(x => x.BufferDelOrder)
                .ThenInclude(x => x.BufferRepresentative)

                .Where(x => x.BufferGatePassId == id)
                .FirstOrDefaultAsync();




            var Receiver = _context.BufferGatePass.Select(r => r.ReceiverName).Distinct().ToList();

            List<SelectListItem> ReceiverName = new List<SelectListItem>();

            if (Receiver != null)
            {
                foreach (var receiver in Receiver)
                {
                    ReceiverName.Add(new SelectListItem { Text = receiver, Value = "0" });

                }
            }

            ViewBag.Receiver = ReceiverName;



            ViewData["FiscalYearId"] = new SelectList(_context.Acc_FiscalYears.Where(a => a.ComId == comid), "FiscalYearId", "FYName", gatePass.FiscalYearId);
            ViewData["FiscalMonthId"] = new SelectList(_context.Acc_FiscalMonths.Where(p => p.ComId == comid), "FiscalMonthId", "MonthName", gatePass.FiscalMonthId);
            ViewData["BufferId"] = new SelectList(_context.Buffers.Take(0).Where(p => p.ComId == comid), "BufferListId", "BufferName");





            if (gatePass == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            return View("Create", gatePass);
        }

        // POST: GatePasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                var GatePasses = await _context.BufferGatePass.FindAsync(id);
                _context.BufferGatePass.Remove(GatePasses);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";

                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), GatePasses.BufferGatePassId.ToString(), "Update", GatePasses.GatePassNo.ToString());

                return Json(new { Success = 1, ContactID = GatePasses.BufferGatePassId, ex = TempData["Message"].ToString() });

            }

            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");

            var reportname = "rptTruckGatePassReport";
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");
            HttpContext.Session.SetString("reportquery", "Exec [Sales_rptTruckGatePassReport] '" + comid + "','" + id + "'");

            string filename = _context.GatePass.Where(x => x.GatePassId == id).Single().GatePassNo.ToString();
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

        private bool GatePassExists(int id)
        {
            return _context.BufferGatePass.Any(e => e.BufferGatePassId == id);
        }
    }
}

