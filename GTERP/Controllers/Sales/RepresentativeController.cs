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
    // [OverridableAuthorize]
    public class RepresentativeController : Controller
    {
        private TransactionLogRepository tranlog;
        private GTRDBContext db;

        public RepresentativeController(GTRDBContext context, TransactionLogRepository tran)
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

            //return View(db.Representative.Where(c => c.RepresentativeId > 0).ToList());
            return View(db.Representative.Where(c => c.comid == HttpContext.Session.GetString("comid") && c.RepresentativeId > 0).ToList());

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
        public ActionResult Create(Representative Representative)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");


            //if (ModelState.IsValid)
            //{
            if (Representative.RepresentativeId > 0)
            {

                Representative.DateUpdated = DateTime.Now;
                Representative.comid = comid;

                if (Representative.userid == null)
                {
                    Representative.userid = userid;
                }
                Representative.useridUpdate = userid;



                db.Entry(Representative).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Data Update Successfully";
                TempData["Status"] = "2";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Representative.RepresentativeId.ToString(), "Update", Representative.RepresentativeName.ToString());





            }
            else
            {
                Representative.userid = userid;
                Representative.comid = comid;
                Representative.DateAdded = DateTime.Now;





                db.Representative.Add(Representative);
                db.SaveChanges();

                TempData["Message"] = "Data Save Successfully";
                TempData["Status"] = "1";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Representative.RepresentativeId.ToString(), "Create", Representative.RepresentativeName.ToString());






                //db.Categories.Add(Representative);
                return RedirectToAction("Index");
            }
            //}
            return RedirectToAction("Index");

            //return View(Representative);
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

            Representative Representative = db.Representative.Where(c => c.RepresentativeId == id && c.comid == comid).FirstOrDefault();
            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == comid).Where(c => c.CategoryId > 0), "CategoryId", "Name");

            if (Representative == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("Create", Representative);

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

            Representative Representative = db.Representative.Where(c => c.RepresentativeId == id && c.comid == comid).FirstOrDefault();
            //ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.comid == comid).Where(c => c.CategoryId > 0), "CategoryId", "Name");

            if (Representative == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("Create", Representative);
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
                Representative Representative = db.Representative.Where(c => c.comid == comid && c.RepresentativeId == id).FirstOrDefault();

                db.Representative.Remove(Representative);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Representative.RepresentativeId.ToString(), "Delete", Representative.RepresentativeName);


                return Json(new { Success = 1, RepresentativeId = Representative.RepresentativeId, ex = "" });

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
