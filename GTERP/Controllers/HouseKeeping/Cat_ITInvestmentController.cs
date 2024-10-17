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
    //[OverridableAuthorize]
    public class Cat_ITInvestmentController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public Cat_ITInvestmentController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        // GET: Section
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            //var sectio
            return View(await db.Cat_ITInvestmentItem
                .Include(s => s.HR_Emp_Info)
                  .ToListAsync());
        }

        // GET: Section/Create
        public IActionResult Create()
        {
            string ComId = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Create";

            var empInfo = (from emp in db.HR_Emp_Info
                           join ep in db.HR_Emp_PersonalInfo on emp.EmpId equals ep.EmpId
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == ComId & ep.IsAllowPF == true
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");

            ViewBag.FYID = new SelectList(db.Acc_FiscalYears.Where(x => x.ComId == ComId), "FiscalYearId", "FYName");
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cat_ITInvestmentItem ITInvestment)
        {
            if (ModelState.IsValid)
            {
                ITInvestment.userid = HttpContext.Session.GetString("userid");
                ITInvestment.comid = HttpContext.Session.GetString("comid");

                ITInvestment.DateUpdated = DateTime.Today;

                ITInvestment.DateAdded = DateTime.Today;

                if (ITInvestment.ITInvestId > 0)
                {
                    ITInvestment.useridUpdate = HttpContext.Session.GetString("userid");
                    db.Entry(ITInvestment).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ITInvestment.FYID.ToString(), "Update", ITInvestment.FYID.ToString());

                }
                else
                {
                    db.Add(ITInvestment);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ITInvestment.FYID.ToString(), "Create", ITInvestment.FYID.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(ITInvestment);
        }

        // GET: Section/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string ComId = HttpContext.Session.GetString("comid");

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var ITInvestment = await db.Cat_ITInvestmentItem.FindAsync(id);

            var empInfo = (from emp in db.HR_Emp_Info
                           join ep in db.HR_Emp_PersonalInfo on emp.EmpId equals ep.EmpId
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == ComId & ep.IsAllowPF == true
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", ITInvestment.EmpId);
            ViewBag.FYID = new SelectList(db.Acc_FiscalYears.Where(x => x.ComId == ComId), "FiscalYearId", "FYName", ITInvestment.FYID);

            if (ITInvestment == null)
            {
                return NotFound();
            }
            return View("Create", ITInvestment);
        }


        // GET: Section/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string ComId = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }

            var ITInvestment = await db.Cat_ITInvestmentItem
                  .Include(c => c.HR_Emp_Info)
               .FirstOrDefaultAsync(m => m.ITInvestId == id);

            if (ITInvestment == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            var empInfo = (from emp in db.HR_Emp_Info
                           join ep in db.HR_Emp_PersonalInfo on emp.EmpId equals ep.EmpId
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == ComId & ep.IsAllowPF == true
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text", ITInvestment.EmpId);
            ViewBag.FYID = new SelectList(db.Acc_FiscalYears.Where(x => x.ComId == ComId), "FiscalYearId", "FYName", ITInvestment.FYID);

            return View("Create", ITInvestment);

        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var ITInvestment = await db.Cat_ITInvestmentItem.FindAsync(id);
                db.Cat_ITInvestmentItem.Remove(ITInvestment);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), ITInvestment.FYID.ToString(), "Delete", ITInvestment.FYID.ToString());
                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = "1", ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }

    }
}
