using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class TermsMainsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: TermsMains
        //[AllowAnonymous]
        public ActionResult Index()
        {
            ///return View(db.TermsMain.ToList());
            return View(db.TermsMain.ToList());

        }

        // GET: TermsMains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            TermsMain termsMain = db.TermsMain.Find(id);

            if (termsMain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Details";


            //Call Create View
            return View("Create", termsMain);
        }

        // GET: TermsMains/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }

        [HttpPost]
        public JsonResult Create(TermsMain termsmain)
        {
            try
            {

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {

                    // If sales main has SalesID then we can understand we have existing sales Information
                    // So we need to Perform Update Operation

                    // Perform Update
                    if (termsmain.TermsId > 0)
                    {

                        var CurrenttermsSUb = db.TermsSub.Where(p => p.TermsId == termsmain.TermsId);

                        foreach (TermsSub ss in CurrenttermsSUb)
                            db.TermsSub.Remove(ss);

                        foreach (TermsSub ss in termsmain.TermsSubs)
                            db.TermsSub.Add(ss);

                        db.Entry(termsmain).State = EntityState.Modified;
                    }
                    //Perform Save
                    else
                    {
                        db.TermsMain.Add(termsmain);
                    }

                    db.SaveChanges();

                    // If Sucess== 1 then Save/Update Successfull else there it has Exception
                    return Json(new { Success = 1, termsid = termsmain.TermsId, ex = "" });
                }
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }


        // GET: TermsMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            TermsMain termsMain = db.TermsMain.Find(id);

            if (termsMain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";


            //Call Create View
            return View("Create", termsMain);
        }



        // GET: TermsMains/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            TermsMain termsmain = db.TermsMain.Find(id);

            if (termsmain == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";


            //Call Create View
            return View("Create", termsmain);
        }

        // POST: TermsMains/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        // POST: /Sales/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                TermsMain termsmain = db.TermsMain.Find(id);
                db.TermsMain.Remove(termsmain);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = termsmain.TermsId, ex = "" });

            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            // return RedirectToAction("Index");
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
