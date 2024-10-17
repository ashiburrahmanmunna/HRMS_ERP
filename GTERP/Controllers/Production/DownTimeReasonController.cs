using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    [OverridableAuthorize]
    public class DownTimeReasonController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public DownTimeReasonController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        // GET: downTimeReason
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(await db.DownTimeReason.ToListAsync());
        }

        // GET: downTimeReason/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var downTimeReason = await db.DownTimeReason
                .FirstOrDefaultAsync(m => m.DownTimeReasonId == id);
            if (downTimeReason == null)
            {
                return NotFound();
            }

            return View(downTimeReason);
        }

        // GET: downTimeReason/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits, "PrdUnitId", "PrdUnitName");
            return View(new DownTimeReason());
        }

        // POST: downTimeReason/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DownTimeReason downTimeReason)
        {
            if (ModelState.IsValid)
            {
                downTimeReason.UserId = HttpContext.Session.GetString("userid");
                downTimeReason.ComId = HttpContext.Session.GetString("comid");
                if (downTimeReason.DownTimeReasonId > 0)
                {
                    downTimeReason.UpdatedbyUserId = HttpContext.Session.GetString("userid");
                    downTimeReason.DateUpdated = DateTime.Now;
                    db.Entry(downTimeReason).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), downTimeReason.DownTimeReasonId.ToString(), "Update", downTimeReason.DownTimeReasonId.ToString());

                }
                else
                {
                    downTimeReason.AddedbyUserId = HttpContext.Session.GetString("userid");
                    downTimeReason.DateUpdated = DateTime.Now;
                    db.Add(downTimeReason);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), downTimeReason.DownTimeReasonId.ToString(), "Create", downTimeReason.DownTimeReasonId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(downTimeReason);
        }

        // GET: downTimeReason/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var downTimeReason = await db.DownTimeReason.FindAsync(id);

            if (downTimeReason == null)
            {
                return NotFound();
            }
            return View("Create", downTimeReason);
        }

        // GET: downTimeReason/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var downTimeReason = await db.DownTimeReason
                .FirstOrDefaultAsync(m => m.DownTimeReasonId == id);

            if (downTimeReason == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            return View("Create", downTimeReason);
        }

        // POST: downTimeReason/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var downTimeReason = await db.DownTimeReason.FindAsync(id);
                db.DownTimeReason.Remove(downTimeReason);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), downTimeReason.DownTimeReasonId.ToString(), "Delete", downTimeReason.DownTimeReasonId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, DownTimeReasonId = downTimeReason.DownTimeReasonId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int DownTimeReasonId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.downTimeReason.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.downTimeReason.Any(d => d.DeptName == DeptName));
        //}


        private bool downTimeReasonExists(int id)
        {
            return db.DownTimeReason.Any(e => e.DownTimeReasonId == id);
        }
    }
}