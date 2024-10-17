using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    [OverridableAuthorize]
    public class ProductMainGroupController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;
        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        public ProductMainGroupController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;

        }

        // GET: catagory
        public async Task<IActionResult> Index()
        {
            var comid = HttpContext.Session.GetString("comid");
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            var data = await db.ProductMainGroups.Where(x => x.comid == comid).ToListAsync();
            return View(data);
        }

        // GET: catagory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catagory = await db.ProductMainGroups
                .FirstOrDefaultAsync(m => m.ProductMainGroupId == id);
            if (catagory == null)
            {
                return NotFound();
            }

            return View(catagory);
        }

        // GET: catagory/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            return View();
        }

        // POST: catagory/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductMainGroup catagory, string imageDatatest)
        {
            var uploadlocation = "/Content/img/ProductGroupMain/";
            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });


            if (ModelState.IsValid)
            {
                catagory.userid = HttpContext.Session.GetString("userid");
                catagory.comid = HttpContext.Session.GetString("comid");

                if (catagory.ProductMainGroupId > 0)
                {


                    catagory.DateUpdated = DateTime.Now;
                    catagory.useridUpdate = HttpContext.Session.GetString("userid");
                    db.Entry(catagory).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "2";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), catagory.ProductMainGroupId.ToString(), "Update", catagory.Name.ToString());


                }
                else
                {
                    db.Add(catagory);
                    await db.SaveChangesAsync();

                    TempData["Message"] = "Data Save Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), catagory.ProductMainGroupId.ToString(), "Create", catagory.Name.ToString());

                    db.Entry(catagory).GetDatabaseValues();
                    int id = catagory.ProductMainGroupId; // Yes it's here


                }
                return RedirectToAction(nameof(Index));
            }
            return View(catagory);
        }

        // GET: catagory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            //Cat_Country  cat_Country  = db.Cat_Country.Where(m => m.BuyerId.ToString() == id.ToString()).FirstOrDefault(); //Find(id);// 
            var catagory = await db.ProductMainGroups.FindAsync(id);
            ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups, "ProductMainGroupId", "Name", catagory.ProductMainGroupId);
            if (catagory == null)
            {
                return NotFound();
            }

            return View("Create", catagory);
        }

        // POST: catagory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductMainGroupId,ComId,Name,Remarks,SLNO,IsInactive,UserId, Pcname")] ProductMainGroup catagory)
        {
            if (id != catagory.ProductMainGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(catagory);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatagoryExists(catagory.ProductMainGroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(catagory);
        }

        // GET: catagory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catagory = await db.ProductMainGroups
                .FirstOrDefaultAsync(m => m.ProductMainGroupId == id);

            if (catagory == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            ViewBag.ProductMainGroupId = new SelectList(db.ProductMainGroups, "ProductMainGroupId", "Name", catagory.ProductMainGroupId);
            return View("Create", catagory);
        }

        // POST: catagory/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            try
            {
                var catagory = await db.ProductMainGroups.FindAsync(id);
                db.ProductMainGroups.Remove(catagory);
                db.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), catagory.ProductMainGroupId.ToString(), "Delete", catagory.Name);

                //TempData["Message"] = "Data Delete Successfully";
                return Json(new { Success = 1, ProductMainGroupId = catagory.ProductMainGroupId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

        }

        private bool CatagoryExists(int id)
        {
            return db.ProductMainGroups.Any(e => e.ProductMainGroupId == id);
        }
    }
}
