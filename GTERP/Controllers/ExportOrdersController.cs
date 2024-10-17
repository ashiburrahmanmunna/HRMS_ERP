using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ExportOrdersController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: ExportOrders
        public ActionResult Index(int? styleid , string FromDate, string ToDate)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            if (FromDate == null || FromDate == "")
            {
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

            }
            if (ToDate == null || ToDate == "")
            {
            }
            else
            {
                dtTo = Convert.ToDateTime(ToDate);

            }
            ViewBag.StyleId = new SelectList(db.StyleInformation , "StyleId", "StyleName");
            if (styleid == null)
            {
                styleid = 0;
            }
           // return View(db.ExportOrders.Where(p => (p.ShipDate >= dtFrom && p.ShipDate <= dtTo) && (p.StyleID.ToString() == styleid.ToString()) && p.isDelete == false).ToList()); //p.ComId == AppData.intComId && 

            return View(db.ExportOrders.Where(p => (p.StyleID.ToString() == styleid.ToString()) && p.isDelete == false).ToList()); //p.ComId == AppData.intComId && 

        }
        public JsonResult getExportOrders(int id)
        {
            ViewBag.StyleId = id;
            //List<ExportOrder> ExportOrders = db.ExportOrders.Where(x => x.StyleID == id).ToList();
            //List<TempStyleInformation> StyleInformation = (db.Database.SqlQuery<TempStyleInformation>("[prcGetExportOrder]  @comid,@Type", new SqlParameter("comid", Session["comid"].ToString()), new SqlParameter("Type", "StyleInformation"))).ToList();

            //List<SelectListItem> termssubslists = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            //if (terms != null)
            //{
            //    foreach (TermsSub x in terms)
            //    {
            //        termssubslists.Add(new SelectListItem { Text = x.TermsDescription.ToString(), Value = x.Terms.ToString() });
            //    }
            //}
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            List<ExportOrder> asdf = db.ExportOrders.Where(p => (p.StyleID.ToString() == id.ToString()) && p.isDelete == false).ToList();


            //return View(asdf);

            return Json(new { data = asdf }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {



            ViewBag.Title = "Edit";
            ExportOrder ExportOrders = db.ExportOrders.Where(m => m.ExportOrderID.ToString() == id.ToString()).FirstOrDefault();
            if (ExportOrders == null)
            {
                return NotFound();
            }
            ViewBag.StyleId = new SelectList(db.StyleInformation, "StyleId", "StyleName" , ExportOrders.StyleID);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", ExportOrders.UnitMasterId);
            ViewBag.DestinationID = new SelectList(db.Destinations.Where(c => c.DestinationID > 0), "DestinationID", "DestinationNameSearch", ExportOrders.DestinationID);
            ViewBag.ExportOrderStatusId = new SelectList(db.ExportOrderStatus.Where(c => c.ExportOrderStatusId > 0), "ExportOrderStatusId", "ExportOrderStatusName", ExportOrders.ExportOrderStatusId);
            ViewBag.ExportOrderCategoryId = new SelectList(db.ExportOrderCategorys.Where(c => c.ExportOrderCategoryId > 0), "ExportOrderCategoryId", "ExportOrderCategoryName", ExportOrders.ExportOrderCategoryId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes.Where(c => c.ShipModeId > 0), "ShipModeId", "ShipModeName", ExportOrders.ShipModeId);

            //return View(db.ExportOrders.Where(p => (p.ShipDate >= dtFrom && p.ShipDate <= dtTo) && (p.StyleID.ToString() == styleid.ToString()) && p.isDelete == false).ToList()); //p.ComId == AppData.intComId && 
            return View("Edit", ExportOrders);
        }

        // GET: ExportOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ExportOrder exportOrder = db.ExportOrders.Find(id);
            if (exportOrder == null)
            {
                return NotFound();
            }
            return View(exportOrder);
        }

        // GET: ExportOrders/Create
        public ActionResult Create(int? styleid)
        {
            ViewBag.StyleId = styleid;
            if (styleid == null)
            {
                styleid = 0;
            }
            ViewBag.styleinfo = new SelectList(db.StyleInformation, "StyleId", "StyleName");
            ViewBag.unitmaster = new SelectList(db.UnitMasters.Where(u => u.UnitGroupId == "Piece"), "UnitMasterId", "UnitMasterId");
            ViewBag.destination = new SelectList(db.Destinations.Where(c => c.DestinationID > 0), "DestinationID", "DestinationNameSearch");

            ViewBag.ExportOrderStatusId = new SelectList(db.ExportOrderStatus.Where(c => c.ExportOrderStatusId > 0), "ExportOrderStatusId", "ExportOrderStatusName");
            ViewBag.ExportOrderCategoryId = new SelectList(db.ExportOrderCategorys.Where(c => c.ExportOrderCategoryId > 0), "ExportOrderCategoryId", "ExportOrderCategoryName");
            ViewBag.ShipModeId = new SelectList(db.ShipModes.Where(c => c.ShipModeId > 0), "ShipModeId", "ShipModeName");


            //IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));

            //List<TempStyleInformation> StyleInformation = (db.Database.SqlQuery<TempStyleInformation>("[prcGetExportOrder]  @comid,@Type", new SqlParameter("comid", Session["comid"].ToString()), new SqlParameter("Type", "StyleInformation"))).ToList();
            //this.ViewBag.StyleInformation = StyleInformation;

            //return View();
            List<ExportOrder> asdf = db.ExportOrders.Where(p => (p.StyleID.ToString() == styleid.ToString()) && p.isDelete == false).ToList();

            return View(asdf);
        }

        public class TempStyleInformation
        {
            //public IEnumerable<ChartOfAccount> coa { get; set; }
            /// <summary>
            /// StyleId	,StyleName	,StyleCode	,CompanyName	,BuyerName	,OrderQty	
            /// ,UnitMasterId	,FOB	,CurCode	,SalesCost	,StyleStatus	,FirstShipDate	,
            /// LastShipDate	,CategoryName	,BrandName	,ProductGroupName

            /// </summary>
            public int StyleId { get; set; }
            public string StyleCode { get; set; }
            public string StyleName { get; set; }


            //public string CompanyName { get; set; }
            //public string BuyerName { get; set; }

            //public string OrderQty { get; set; }

            //public string UnitShortName { get; set; }

            //public string FOB { get; set; }
            //public string CurCode { get; set; }
            //public string SalesCost { get; set; }
            //public string StyleStatus { get; set; }
            //public DateTime FirstShipDate { get; set; }
            //public DateTime LastShipDate { get; set; }

            //public string CategoryName { get; set; }

            //public string BrandName { get; set; }

            //public string ProductGroupName { get; set; }

            

        }

        // POST: ExportOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(/*Include =*/ "ExportOrderID,StyleID,BuyerContactPONo,POLineNo,PoDate,DestinationID,OrderQty,UnitMasterId,Rate,CM,ShipMode,ExFactoryDate,ShipDate,AddedBy,DateAdded,UpdatedBy,DateUpdated,ExportOrderStatus,ExportOrderCategory,Remark")] ExportOrder exportOrder)
        public ActionResult CreateTest(List<ExportOrder> ExportOrderSubs)
        {
            try
            {
                      if (AppData.intComId == "0" || AppData.intComId == null)
                {
                    return NotFound();

                }



                {
                    if (ModelState.IsValid)
                    {
                        foreach (var item in ExportOrderSubs)
                        {
                            //WeekdaySectionWise data = db.WeekdaySectionWise.FirstOrDefault(s => s.ComId == item.ComId && s.SectionId == item.SectionId && s.FromDate.Date >= item.FromDate.Date && s.ToDate.Date <= item.ToDate.Date);
                            if (item.ExportOrderID > 0)
                            {
                                if (item.isDelete == false)
                                {
                                    db.Entry(item).State = EntityState.Modified;
                                    item.DateAdded = DateTime.Now;
                                    item.DateUpdated = DateTime.Now;
                                    item.comid =int.Parse(AppData.intComId);


                                }
                                else
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                    db.SaveChanges();
                                }

                         
                                //message = "Weekend update succeded";
                            }
                            else
                            {
                                item.DateAdded = DateTime.Now;
                                item.DateUpdated = DateTime.Now;
                                item.comid = int.Parse(AppData.intComId);
                                if (item.isDelete == false)
                                {
                                   

                                    db.ExportOrders.Add(item);
                                    //db.WeekdaySectionWise.Add(item);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //db.ExportOrders.Add(item);
                                    ////db.WeekdaySectionWise.Add(item);
                                    //db.SaveChanges();

                                }
                                //message = "Weekend save succeded";
                            }
                        }

                        //db.ExportOrders.Add(exportOrder);
                        //db.SaveChanges();

                    }

                    //return View(exportOrder);
                    return Json(new { Success = 1, ProductId = 0, ex = "" });

                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Success = 0,
                    ex = ex.Message.ToString()
                });

            }

           // return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
        }

        // GET: ExportOrders/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    ExportOrder exportOrder = db.ExportOrders.Find(id);
        //    if (exportOrder == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.styleinfo = new SelectList(db.StyleInformation, "StyleId", "StyleName");
        //    ViewBag.unitmaster = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId");
        //    ViewBag.destination = new SelectList(db.Destinations.Where(c => c.DestinationID > 0), "DestinationID", "DestinationNameSearch");
        //    //IQueryable<Product> Productresult = db.Products.Where(c => c.ProductId > 0 && c.comid.ToString() == (AppData.intComId));

        //    List<TempStyleInformation> StyleInformation = (db.Database.SqlQuery<TempStyleInformation>("[prcGetExportOrder]  @comid,@Type", new SqlParameter("comid", Session["comid"].ToString()), new SqlParameter("Type", "StyleInformation"))).ToList();
        //    this.ViewBag.StyleInformation = StyleInformation;
        //    return View("Create", exportOrder);
        //    //return View(exportOrder);
        //}

        // POST: ExportOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExportOrder exportOrder)
        {
            try
            {

                //var errors = ModelState.Where(x => x.Value.Errors.Any())
                //.Select(x => new { x.Key, x.Value.Errors });

                ViewBag.Title = "Edit";
                if (ModelState.IsValid)
                {
                    db.Entry(exportOrder).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //return Json(new { Success = 1, PIId = 0, ex = "" });
                }
                ViewBag.styleinfo = new SelectList(db.StyleInformation, "StyleId", "StyleName");
                ViewBag.unitmaster = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId");
                ViewBag.destination = new SelectList(db.Destinations.Where(c => c.DestinationID > 0), "DestinationID", "DestinationNameSearch");
                ViewBag.ExportOrderStatusId = new SelectList(db.ExportOrderStatus.Where(c => c.ExportOrderStatusId > 0), "ExportOrderStatusId", "ExportOrderStatusName");
                ViewBag.ExportOrderCategoryId = new SelectList(db.ExportOrderCategorys.Where(c => c.ExportOrderCategoryId > 0), "ExportOrderCategoryId", "ExportOrderCategoryName");
                ViewBag.ShipModeId = new SelectList(db.ShipModes.Where(c => c.ShipModeId > 0), "ShipModeId", "ShipModeName");

                return View();
                //return Json(new { Success = 1, PIId = 0, ex = "Unable to Update the Date" });
            }
            catch (Exception ex)
            {
                return View();
                //throw ex;
                //return Json(new { Success = 1, PIId = 0, ex = ex.Message });
            }
        }

        // GET: ExportOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }


            ViewBag.Title = "Delete";
            ExportOrder ExportOrders = db.ExportOrders.Where(m => m.ExportOrderID.ToString() == id.ToString()).FirstOrDefault();
            if (ExportOrders == null)
            {
                return NotFound();
            }
            ViewBag.StyleId = new SelectList(db.StyleInformation, "StyleId", "StyleName", ExportOrders.StyleID);
            ViewBag.UnitMasterId = new SelectList(db.UnitMasters, "UnitMasterId", "UnitMasterId", ExportOrders.UnitMasterId);
            ViewBag.DestinationID = new SelectList(db.Destinations.Where(c => c.DestinationID > 0), "DestinationID", "DestinationNameSearch", ExportOrders.DestinationID);
            ViewBag.ExportOrderStatusId = new SelectList(db.ExportOrderStatus.Where(c => c.ExportOrderStatusId > 0), "ExportOrderStatusId", "ExportOrderStatusName", ExportOrders.ExportOrderStatusId);
            ViewBag.ExportOrderCategoryId = new SelectList(db.ExportOrderCategorys.Where(c => c.ExportOrderCategoryId > 0), "ExportOrderCategoryId", "ExportOrderCategoryName", ExportOrders.ExportOrderCategoryId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes.Where(c => c.ShipModeId > 0), "ShipModeId", "ShipModeName", ExportOrders.ShipModeId);

            //return View(db.ExportOrders.Where(p => (p.ShipDate >= dtFrom && p.ShipDate <= dtTo) && (p.StyleID.ToString() == styleid.ToString()) && p.isDelete == false).ToList()); //p.ComId == AppData.intComId && 
            return View("Edit", ExportOrders);
        }

        // POST: ExportOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                ExportOrder exportOrder = db.ExportOrders.Find(id);
                db.ExportOrders.Remove(exportOrder);
                db.SaveChanges();
                return Json(new { Success = 1, ExportOrderID = exportOrder.ExportOrderID, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
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
