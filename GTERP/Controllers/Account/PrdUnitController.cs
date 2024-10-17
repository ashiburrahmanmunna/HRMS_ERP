using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;

namespace GTERP.Controllers.Account
{
    [OverridableAuthorize]
    public class PrdUnitController : Controller
    {
        private TransactionLogRepository tranlog;
        private GTRDBContext db;

        public PrdUnitController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }


        //[Authorize]
        // GET: Categories
        public ActionResult Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            //return View(db.PrdUnits.Where(c => c.PrdUnitId > 0).ToList());
            return View(db.PrdUnits.Where(c => c.ComId == HttpContext.Session.GetString("comid") && c.PrdUnitId > 0).ToList());

        }

        //[Authorize]
        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";
            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == (HttpContext.Session.GetString("comid"))).Where(c => c.CategoryId > 0), "CategoryId", "Name");

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PrdUnit PrdUnit)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");


            //if (ModelState.IsValid)
            //{
            if (PrdUnit.PrdUnitId > 0)
            {

                PrdUnit.DateUpdated = DateTime.Now;
                PrdUnit.ComId = comid;

                if (PrdUnit.UserId == null)
                {
                    PrdUnit.UserId = userid;
                }
                PrdUnit.UpdateByUserId = userid;



                db.Entry(PrdUnit).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PrdUnit.PrdUnitId.ToString(), "Update", PrdUnit.PrdUnitName.ToString());





            }
            else
            {
                PrdUnit.UserId = userid;
                PrdUnit.ComId = comid;
                PrdUnit.DateAdded = DateTime.Now;





                db.PrdUnits.Add(PrdUnit);
                db.SaveChanges();

                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                //tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PrdUnit.PrdUnitId.ToString(), "Create", PrdUnit.PrdUnitName.ToString());






                //db.Categories.Add(PrdUnit);
                return RedirectToAction("Index");
            }
            //}
            return RedirectToAction("Index");

            //return View(PrdUnit);
        }


        //[Authorize]
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var comid = HttpContext.Session.GetString("comid");

            PrdUnit PrdUnit = db.PrdUnits.Where(c => c.PrdUnitId == id && c.ComId == comid).FirstOrDefault();
            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == comid).Where(c => c.CategoryId > 0), "CategoryId", "Name");

            if (PrdUnit == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("Create", PrdUnit);

        }


        //[Authorize]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var comid = (HttpContext.Session.GetString("comid")).ToString();

            PrdUnit PrdUnit = db.PrdUnits.Where(c => c.PrdUnitId == id && c.ComId == comid).FirstOrDefault();
            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == comid).Where(c => c.CategoryId > 0), "CategoryId", "Name");

            if (PrdUnit == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("Create", PrdUnit);
        }
        //        //[Authorize]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        //      [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid")).ToString();
                PrdUnit PrdUnit = db.PrdUnits.Where(c => c.ComId == comid && c.PrdUnitId == id).FirstOrDefault();

                db.PrdUnits.Remove(PrdUnit);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), PrdUnit.PrdUnitId.ToString(), "Delete", PrdUnit.PrdUnitName);


                return Json(new { Success = 1, PrdUnitId = PrdUnit.PrdUnitId, ex = "" });

            }
            catch (Exception ex)
            {

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
