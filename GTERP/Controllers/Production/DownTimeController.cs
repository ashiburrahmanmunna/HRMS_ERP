using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    [OverridableAuthorize]
    public class DownTimeController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;
        private PermissionLevel PL;

        public DownTimeController(GTRDBContext context, TransactionLogRepository tran, PermissionLevel _PL)
        {
            tranlog = tran;
            db = context;
            PL = _PL;
        }

        // GET: downTime
        public async Task<IActionResult> Index(int? unitId, int? yearId, int? monthId)
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            // tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
            var DownTime = db.DownTime
                .Include(l => l.DownTimeReason)
                .Include(y => y.YearName)
                .Include(m => m.MonthName)
                .Include(u => u.Unit);

            if (unitId == null || yearId == null || monthId == null)
            {
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");

                var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
                if (fiscalYear != null)
                {
                    var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

                    ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
                    ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(m => m.FYId == fiscalYear.FiscalYearId).OrderBy(y => y.FiscalMonthId).Where(f => f.FYId == fiscalYear.FYId), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);

                    return View(await DownTime.Where(u => u.FiscalYearId == fiscalYear.FiscalYearId && u.FiscalMonthId == fiscalMonth.FiscalMonthId).ToListAsync());

                }

                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName");
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName");

                TempData["Message"] = "Data Load Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

                return View(new List<DownTime>());
            }
            else
            {
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", unitId);
                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderBy(y => y.FYId), "FiscalYearId", "FYName", yearId);
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.Where(m => m.FYId == yearId).OrderBy(y => y.FiscalMonthId), "FiscalMonthId", "MonthName", monthId);

                TempData["Message"] = "Data Load Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");
                return View(await DownTime.Where(u => u.FiscalYearId == yearId && u.FiscalMonthId == monthId && u.PrdUnitId == unitId).ToListAsync());
            }

        }



        // GET: downTime/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["ReasonId"] = new SelectList(db.DownTimeReason, "DownTimeReasonId", "Reason");
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");


            var fiscalYear = db.Acc_FiscalYears.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            if (fiscalYear != null)
            {
                var fiscalMonth = db.Acc_FiscalMonths.Where(f => f.OpeningdtFrom.Date <= DateTime.Now.Date && f.ClosingdtTo >= DateTime.Now.Date).FirstOrDefault();

                ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
                ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName).Where(f => f.FYId == fiscalYear.FYId), "FiscalMonthId", "MonthName", fiscalMonth.FiscalMonthId);
                return View(new DownTime());
            }

            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName");
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName");

            return View(new DownTime());
        }

        // POST: downTime/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DownTime downTime)
        {
            if (ModelState.IsValid)
            {
                downTime.userid = HttpContext.Session.GetString("userid");
                downTime.comid = HttpContext.Session.GetString("comid");
                if (downTime.DownTimeId > 0)
                {
                    downTime.UpdatedbyUserId = HttpContext.Session.GetString("userid");
                    downTime.DateUpdated = DateTime.Now;
                    db.Entry(downTime).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), downTime.DownTimeId.ToString(), "Update", downTime.DownTimeId.ToString());

                }
                else
                {
                    downTime.AddedbyUserId = HttpContext.Session.GetString("userid");
                    downTime.DateAdded = DateTime.Now;
                    db.Add(downTime);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), downTime.DownTimeId.ToString(), "Create", downTime.DownTimeId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(downTime);
        }

        // GET: downTime/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var downTime = await db.DownTime.FindAsync(id);
            ViewData["ReasonId"] = new SelectList(db.DownTimeReason, "DownTimeReasonId", "Reason", downTime.ReasonId);
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", downTime.PrdUnitId);
            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", downTime.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", downTime.FiscalMonthId);

            if (downTime == null)
            {
                return NotFound();
            }
            return View("Create", downTime);
        }

        // GET: downTime/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var downTime = await db.DownTime
                .FirstOrDefaultAsync(m => m.DownTimeId == id);

            if (downTime == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", downTime.PrdUnitId);
            ViewData["ReasonId"] = new SelectList(db.DownTimeReason, "DownTimeReasonId", "Reason", downTime.ReasonId);
            ViewBag.FiscalYearId = new SelectList(db.Acc_FiscalYears.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", downTime.FiscalYearId);
            ViewBag.FiscalMonthId = new SelectList(db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName), "FiscalMonthId", "MonthName", downTime.FiscalMonthId);
            return View("Create", downTime);
        }

        // POST: downTime/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var downTime = await db.DownTime.FindAsync(id);
                db.DownTime.Remove(downTime);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), downTime.DownTimeId.ToString(), "Delete", downTime.DownTimeId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, DownTimeId = downTime.DownTimeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult GetFiscalMonth(int id)
        {
            string comid = HttpContext.Session.GetString("comid");
            var data = db.Acc_FiscalMonths.OrderByDescending(y => y.MonthName).Where(m => m.FYId == id && m.ComId == comid).ToList();
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetDownTimeReasonRemarks(int id)
        {
            string comid = HttpContext.Session.GetString("comid");
            var data = db.DownTimeReason.Where(m => m.DownTimeReasonId == id).Select(d => d.Remarks).FirstOrDefault();
            return Json(data);
        }


        //public IActionResult IsExist(string DeptName,int DownTimeId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.downTime.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.downTime.Any(d => d.DeptName == DeptName));
        //}


        private bool downTimeExists(int id)
        {
            return db.DownTime.Any(e => e.DownTimeId == id);
        }
    }
}