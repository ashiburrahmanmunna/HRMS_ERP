using GTCommercial.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ExportRealizationBankInfoController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: ExportRealizationBankInfo
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Index(string UserList, string FromDate, string ToDate)
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
            List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria", new SqlParameter("Criteria", "ExportRealizationBankInfo"))).ToList();
            ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName");


            if (UserList == null)
            {
                UserList = HttpContext.Session.GetString("userid");
            }


            // return View(db.ExportOrders.Where(p => (p.ShipDate >= dtFrom && p.ShipDate <= dtTo) && (p.StyleID.ToString() == supplierid.ToString()) && p.isDelete == false).ToList()); //p.ComId == AppData.intComId && 


            //var x = db.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString()) && (p.PIDate >= dtFrom && p.PIDate <= dtTo)).ToList();
            var x = db.ExportRealizationBankInfo.Where(a => a.BankRefDate >= dtFrom && a.BankRefDate <= dtTo).ToList();

            //var x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.SupplierId.ToString() == supplierid.ToString()) && (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


                if (UserList == "")
                {
                    x = db.ExportRealizationBankInfo.Where(p => (p.BankRefDate >= dtFrom && p.BankRefDate <= dtTo)).ToList();
                    //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


                }
                else
                {
                    x = db.ExportRealizationBankInfo.Where(p => (p.BankRefDate >= dtFrom && p.BankRefDate <= dtTo) && (p.userid.ToString() == UserList.ToString())).ToList();

                }

                //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


           



            return View(x); //p.ComId == AppData.intComId && 
        }

        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Create()
        {
            try
            {
               

                ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName");
                ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
                ViewBag.CurrencyId = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");



                ViewBag.Title = "Create";

                    return View();
    

               
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(/*Include =*/ "ExportOrderID,StyleID,BuyerContactPONo,POLineNo,PoDate,DestinationID,OrderQty,UnitMasterId,Rate,CM,ShipMode,ExFactoryDate,ShipDate,AddedBy,DateAdded,UpdatedBy,DateUpdated,ExportOrderStatus,ExportOrderCategory,Remark")] ExportOrder exportOrder)
        public ActionResult Create(List<ExportRealizationBankInfo> ExportRealizationBankInfos)
        {
            try
            {
                      if (AppData.intComId == "0" || AppData.intComId == null)
                {
                    return NotFound();
                }

                      if (AppData.intComId == "0" || AppData.intComId == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }

                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });


                {
                    if (ModelState.IsValid)
                    {
                        //var text = "";
                        List<COM_ProformaInvoice_Sub> com_proforma_itemgrouplist = new List<COM_ProformaInvoice_Sub>();

                        foreach (var item in ExportRealizationBankInfos)
                        {
                            //WeekdaySectionWise data = db.WeekdaySectionWise.FirstOrDefault(s => s.ComId == item.ComId && s.SectionId == item.SectionId && s.FromDate.Date >= item.FromDate.Date && s.ToDate.Date <= item.ToDate.Date);
                            if (item.BankRefId > 0)
                            {
                                if (item.isDelete == false)
                                {
                                    db.Entry(item).State = EntityState.Modified;
                                    //item.DateAdded = DateTime.Now;
                                    item.DateUpdated = DateTime.Now;
                                    item.comid = int.Parse(AppData.intComId);

                                }
                                else
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                item.DateAdded = DateTime.Now;
                                //item.DateUpdated = DateTime.Now;
                                item.comid = int.Parse(AppData.intComId);
                                item.userid = HttpContext.Session.GetString("userid");
                                //text = "";
                                if (item.isDelete == false)
                                {

                                    db.ExportRealizationBankInfo.Add(item);
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
                        return Json(new { Success = 1, BankRefId = 0, ex = "Data Save Successfully" });
                    }

                    //return View(exportOrder);
                    return Json(new { Success = 0, BankRefId = 0, ex = "Unable To Save.." });

                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Success = 0,
                    ex = ex.InnerException.InnerException.Message.ToString()
                });

            }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Edit(int id)
        {
                  if (AppData.intComId == "0" || AppData.intComId == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Edit";
            ExportRealizationBankInfo exportRealizationBankInfo = db.ExportRealizationBankInfo.Where(m => m.BankRefId.ToString() == id.ToString()).FirstOrDefault();
            if (exportRealizationBankInfo == null)
            {
                return NotFound();
            }


            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName", exportRealizationBankInfo.BuyerGroupId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", exportRealizationBankInfo.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", exportRealizationBankInfo.CurrencyId);


            return View("Edit", exportRealizationBankInfo);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExportRealizationBankInfo exportRealizationBankInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    exportRealizationBankInfo.DateUpdated = DateTime.Now;
                    exportRealizationBankInfo.useridUpdate = HttpContext.Session.GetString("userid");

                    db.Entry(exportRealizationBankInfo).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //return Json(new { Success = 1, PIId = 0, ex = "" });
                }

                ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName", exportRealizationBankInfo.BuyerGroupId);
                ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", exportRealizationBankInfo.CommercialCompanyId);
                ViewBag.CurrencyId = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", exportRealizationBankInfo.CurrencyId);

                return View();
            }
            catch (Exception ex)
            {
                return View();
                //throw ex;
                //return Json(new { Success = 1, PIId = 0, ex = ex.Message });
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
                  if (AppData.intComId == "0" || AppData.intComId == null)
            {
                return BadRequest();
            }
            ViewBag.Title = "Delete";
            ExportRealizationBankInfo exportRealizationBankInfo = db.ExportRealizationBankInfo.Where(m => m.BankRefId.ToString() == id.ToString()).FirstOrDefault();
            if (exportRealizationBankInfo == null)
            {
                return NotFound();
            }
            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName", exportRealizationBankInfo.BuyerGroupId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", exportRealizationBankInfo.CommercialCompanyId);
            ViewBag.CurrencyId = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", exportRealizationBankInfo.CurrencyId);


            return View("Edit", exportRealizationBankInfo);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                ExportRealizationBankInfo exportRealizationBankInfo = db.ExportRealizationBankInfo.Find(id);
                db.ExportRealizationBankInfo.Remove(exportRealizationBankInfo);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = exportRealizationBankInfo.BankRefId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public JsonResult PI_Duplicate_Check(string id)
        {
            try
            {
                int comid = int.Parse(AppData.intComId);
                List<ExportRealizationBankInfo> exportRealizationBankInfos = db.ExportRealizationBankInfo.ToList(); //x.PINo == id &&  //.Where(x =>  x.comid == comid)

                List<SelectListItem> pilist = new List<SelectListItem>();

                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                if (exportRealizationBankInfos != null)
                {
                    foreach (ExportRealizationBankInfo x in exportRealizationBankInfos)
                    {
                        pilist.Add(new SelectListItem { Text = x.BankRefNo, Value = x.BankRefId.ToString() });
                    }
                }

                return Json(new SelectList(pilist, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {

                return Json(new { success = 0, values = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);

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