using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Buffers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.Buffer
{
    public class BufferRepresentativeController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;

        public BufferRepresentativeController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
        }

        public IActionResult Index()
        {

            var comid = HttpContext.Session.GetString("comid");
            return View(db.BuferRepresentative.Where(x => x.comid == comid).ToList());
        }


        public IActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BufferRepresentative data)
        {



            if (ModelState.IsValid)
            {
                data.userid = HttpContext.Session.GetString("userid");
                data.comid = HttpContext.Session.GetString("comid");

                data.DateUpdated = DateTime.Today;
                data.DateAdded = DateTime.Today;


                if (data.BufferRepresentativeId > 0)
                {

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();


                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";

                }
                else
                {
                    db.Add(data);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";

                }
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            var data = await db.BuferRepresentative.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return View("Create", data);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await db.BuferRepresentative

                .FirstOrDefaultAsync(m => m.BufferRepresentativeId == id);
            if (data == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("Create", data);

        }


        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            try
            {
                var data = await db.BuferRepresentative.FindAsync(id);
                db.BuferRepresentative.Remove(data);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                // tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Designation.DesigId.ToString(), "Delete", Cat_Designation.DesigName);

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, BufferRepresentativeId = data.BufferRepresentativeId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        private bool BufferRepresentativeExists(int id)
        {
            return db.BuferRepresentative.Any(e => e.BufferRepresentativeId == id);
        }

    }
}
