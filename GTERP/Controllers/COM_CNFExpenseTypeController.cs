using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class COM_CNFExpenseTypeController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;
        public COM_CNFExpenseTypeController()
        {
            //UserLog = new //UserLog();
        }

        // GET: COM_CNFExpenseType
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            return View(db.COM_CNFExpenseTypes.Where(b => b.comid.ToString() == AppData.intComId).ToList());
        }



        // GET: COM_CNFExpenseType/Create
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }

        // POST: COM_CNFExpenseType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "ExpenseHeadID,CNFExpenseNo,CNFExpenseName,AmountPercentage,DefaultAmount,ImportOrExport,IsActive,AddedBy,DateAdded,UpdatedBy,userid,comid,DateUpdated")] COM_CNFExpenseType cOM_CNFExpanseType)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                if (cOM_CNFExpanseType.ExpenseHeadID > 0)
                {
                    cOM_CNFExpanseType.DateAdded = DateTime.Now;
                    cOM_CNFExpanseType.DateUpdated = DateTime.Now;

                    cOM_CNFExpanseType.comid = int.Parse(AppData.intComId);
                    cOM_CNFExpanseType.userid = HttpContext.Session.GetString("userid");
                    //COM_CNFExpenseType.comid = int.Parse(AppData.intComId.ToString());
                    //COM_CNFExpenseType.userid = HttpContext.Session.GetString("userid");

                    db.Entry(cOM_CNFExpanseType).State = EntityState.Modified;
                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cOM_CNFExpanseType.ExpenseHeadID.ToString(), "Update");
                }
                else
                {
                    cOM_CNFExpanseType.DateAdded = DateTime.Now;
                    cOM_CNFExpanseType.DateUpdated = DateTime.Now;

                    cOM_CNFExpanseType.comid = int.Parse(AppData.intComId);
                    cOM_CNFExpanseType.userid = HttpContext.Session.GetString("userid");
                    //COM_CNFExpenseType.comid = int.Parse(AppData.intComId.ToString());
                    //COM_CNFExpenseType.userid = HttpContext.Session.GetString("userid");
                    db.COM_CNFExpenseTypes.Add(cOM_CNFExpanseType);
                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), cOM_CNFExpanseType.ExpenseHeadID.ToString(), "Create");
                }
                db.SaveChanges();

                return RedirectToAction("Index");
                //}

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Save / Update";
                TempData["Status"] = "0";
                throw ex;
                return View(cOM_CNFExpanseType);

            }
        }

        // GET: COM_CNFExpenseType/Edit/5
        //[OverridableAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";

            COM_CNFExpenseType COM_CNFExpenseType = db.COM_CNFExpenseTypes.Find(id);
            if (COM_CNFExpenseType == null)
            {
                return NotFound();
            }
            return View("Create", COM_CNFExpenseType);
        }

        // POST: COM_CNFExpenseType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "ExpenseHeadID,CNFExpenseNo,CNFExpenseName,DefaultAmount,ImportOrExport,IsActive,AddedBy,DateAdded,UpdatedBy,userid,comid,DateUpdated")] COM_CNFExpenseType cOM_CNFExpanseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOM_CNFExpanseType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cOM_CNFExpanseType);
        }

        // GET: COM_CNFExpenseType/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";

            COM_CNFExpenseType COM_CNFExpenseType = db.COM_CNFExpenseTypes.Find(id);
            if (COM_CNFExpenseType == null)
            {
                return NotFound();
            }
            return View("Create", COM_CNFExpenseType);
        }

        // POST: COM_CNFExpenseType/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[OverridableAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                COM_CNFExpenseType COM_CNFExpenseType = db.COM_CNFExpenseTypes.Find(id);
                db.COM_CNFExpenseTypes.Remove(COM_CNFExpenseType);
                db.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), id.ToString(), "Delete");
                return Json(new { Success = 1, id = COM_CNFExpenseType.ExpenseHeadID, ex = TempData["Message"].ToString() });

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
