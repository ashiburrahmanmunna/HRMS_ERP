using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    //[OverridableAuthorize]
    public class OtherDeductionAssociationController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public OtherDeductionAssociationController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        // GET: otherDedAssociation
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            return View(await db.HR_OtherDedAssociation.ToListAsync());
        }

        public class Pross
        {
            public string ProssType { get; set; }
            public DateTime dtInput { get; set; }
        }

        // GET: otherDedAssociation/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] parameter1 = new SqlParameter[1];
            parameter1[0] = new SqlParameter("@ComId", comid);
            var pType = Helper.ExecProcMapTList<Pross>("GetProssType", parameter1);
            ViewBag.ProssType = new SelectList(pType, "ProssType", "ProssType");
            //ViewBag.OtherDedAssId = new SelectList(db.HR_OtherDedAssociation, "OtherDedAssId", "DeptName");
            return View();
        }

        // POST: otherDedAssociation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HR_OtherDedAssociation otherDedAssociation)
        {
            if (ModelState.IsValid)
            {
                otherDedAssociation.UserId = HttpContext.Session.GetString("userid");
                otherDedAssociation.ComId = HttpContext.Session.GetString("comid");
                if (otherDedAssociation.OtherDedAssId > 0)
                {
                    db.Entry(otherDedAssociation).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), otherDedAssociation.OtherDedAssId.ToString(), "Update", otherDedAssociation.OtherDedAssId.ToString());

                }
                else
                {
                    db.Add(otherDedAssociation);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), otherDedAssociation.OtherDedAssId.ToString(), "Create", otherDedAssociation.OtherDedAssId.ToString());

                }
                return RedirectToAction(nameof(Index));
            }
            return View(otherDedAssociation);
        }

        // GET: otherDedAssociation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var otherDedAssociation = await db.HR_OtherDedAssociation.FindAsync(id);
            //ViewBag.OtherDedAssId = new SelectList(db.HR_OtherDedAssociation, "OtherDedAssId", "DeptName", otherDedAssociation.ParentId);
            if (otherDedAssociation == null)
            {
                return NotFound();
            }
            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] parameter1 = new SqlParameter[1];
            parameter1[0] = new SqlParameter("@ComId", comid);
            var pType = Helper.ExecProcMapTList<Pross>("GetProssType", parameter1);
            ViewBag.ProssType = new SelectList(pType, "ProssType", "ProssType", otherDedAssociation.ProssType);
            return View("Create", otherDedAssociation);
        }

        // GET: otherDedAssociation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otherDedAssociation = await db.HR_OtherDedAssociation
                .FirstOrDefaultAsync(m => m.OtherDedAssId == id);

            if (otherDedAssociation == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] parameter1 = new SqlParameter[1];
            parameter1[0] = new SqlParameter("@ComId", comid);
            var pType = Helper.ExecProcMapTList<Pross>("GetProssType", parameter1);
            ViewBag.ProssType = new SelectList(pType, "ProssType", "ProssType", otherDedAssociation.ProssType);
            //ViewBag.OtherDedAssId = new SelectList(db.HR_OtherDedAssociation, "OtherDedAssId", "DeptName", otherDedAssociation.ParentId);
            return View("Create", otherDedAssociation);
        }

        // POST: otherDedAssociation/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var otherDedAssociation = await db.HR_OtherDedAssociation.FindAsync(id);
                db.HR_OtherDedAssociation.Remove(otherDedAssociation);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), otherDedAssociation.OtherDedAssId.ToString(), "Delete", otherDedAssociation.OtherDedAssId.ToString());

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, OtherDedAssId = otherDedAssociation.OtherDedAssId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }


        //public IActionResult IsExist(string DeptName,int OtherDedAssId)
        //{
        //    if (ViewBag.Title == "Create")
        //        return Json(!db.HR_OtherDedAssociation.Any(d => d.DeptName == DeptName));
        //    else
        //        return Json(!db.HR_OtherDedAssociation.Any(d => d.DeptName == DeptName));
        //}


        private bool otherDedAssociationExists(int id)
        {
            return db.HR_OtherDedAssociation.Any(e => e.OtherDedAssId == id);
        }
    }
}