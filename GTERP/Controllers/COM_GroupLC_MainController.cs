using GTCommercial.Models;
using GTCommercial.Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class COM_GroupLC_MainController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        // GET: COM_GroupLC_Main
        public ActionResult Index()
        {
            return View(db.COM_GroupLC_Mains.ToList());
        }

        // GET: COM_GroupLC_Main/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            COM_GroupLC_Main cOM_GroupLC_Main = db.COM_GroupLC_Mains.Find(id);
            if (cOM_GroupLC_Main == null)
            {
                return NotFound();
            }
            return View(cOM_GroupLC_Main);
        }

        // GET: COM_GroupLC_Main/Create
        public ActionResult Create(int? BuyerID)
        {
            ViewBag.Title = "Create";
            ViewBag.BuyerID = BuyerID;
            if (BuyerID == null)
            {
                BuyerID = 0;
            }

            ViewBag.masterlc = (from masterlc in db.COM_MasterLCs.Where(m => m.comid.ToString() == AppData.intComId)
                                    //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                    //join s in db.COM_MasterLCExports on masterlc.MasterLCID equals s.MasterLCID
                                    //join ss in db.ExportOrders on s.ExportOrderID equals ss.ExportOrderID
                                    //join si in db.StyleInformation on ss.StyleID equals si.StyleId   
                                where masterlc.BuyerID == BuyerID &&
                                !db.COM_GroupLC_Subs.Any(f => f.MasterLCID == masterlc.MasterLCID && masterlc.comid.ToString() == AppData.intComId)
                                select masterlc).Distinct().ToList();

            //int masterlcid = db.COM_GroupLC_Subs.Where(m=>m.GroupLCId == )
            //int concernid = db.COM_MasterLCs.Where(m => m.MasterLCID == masterlcid).FirstOrDefault();


            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");





            return View();
        }

        // POST: COM_GroupLC_Main/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(COM_GroupLC_Main COM_GroupLC_main) //[Bind(/*Include =*/ "MasterLCID, LCRefNo,BuyerLCRef,CommercialCompanyId,LCOpenDate,LCExpirydate,BuyerID,OpeningBankId,LienBankId,TotalLCQty,UnitMasterId,LCValue,CurrencyId,Tenor,DestinationId,Addedby,DateAdded,Updatedby,DateUpdated,userid,comid,isDelete,PortOfLoadingId,LCTypeId,LCStatusId,LCNatureId,TradeTermId,ShipModeId,PaymentTermsId,DayListId")] 
        {
            try
            {
                if (AppData.intComId == "0" || AppData.intComId == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }

                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                //if (ModelState.IsValid)
                {

                    if (COM_GroupLC_main.GroupLCId > 0)
                    {
                        COM_GroupLC_main.DateUpdated = DateTime.Now;
                        //COM_GroupLC_main.DateAdded = DateTime.Now;
                        COM_GroupLC_main.useridUpdate = HttpContext.Session.GetString("userid");

                        if (COM_GroupLC_main.userid == null || COM_GroupLC_main.userid == "0")
                        {

                            COM_GroupLC_main.userid = HttpContext.Session.GetString("userid");

                        }


                        IQueryable<COM_GroupLC_Sub> grouplcsub = db.COM_GroupLC_Subs.Where(p => p.GroupLCId == COM_GroupLC_main.GroupLCId);

                        foreach (COM_GroupLC_Sub ss in grouplcsub)
                        {
                            db.COM_GroupLC_Subs.Remove(ss);
                        }

                        foreach (COM_GroupLC_Sub ss in COM_GroupLC_main.COM_GroupLC_Sub)
                        {
                            //ss.DateAdded = DateTime.Now;
                            //ss.DateUpdated = DateTime.Now;

                            //db.VoucherSubs.Add(ss);
                            db.COM_GroupLC_Subs.Add(ss);



                        }
                        db.Entry(COM_GroupLC_main).State = EntityState.Modified;

                    }
                    else
                    {
                        COM_GroupLC_main.DateAdded = DateTime.Now;
                        COM_GroupLC_main.DateUpdated = DateTime.Now;
                        COM_GroupLC_main.comid = int.Parse(Session["comid"].ToString());
                        COM_GroupLC_main.userid = HttpContext.Session.GetString("userid");

                        foreach (COM_GroupLC_Sub ss in COM_GroupLC_main.COM_GroupLC_Sub)
                        {
                            //ss.DateAdded = DateTime.Now;
                            //ss.DateUpdated = DateTime.Now;

                            //db.VoucherSubs.Add(ss);
                            //db.COM_GroupLC_mainExports.Add(ss);



                        }

                        db.COM_GroupLC_Mains.Add(COM_GroupLC_main);
                    }

                    db.SaveChanges();


                    //return RedirectToAction("Index");
                }
                return Json(new { Success = 1, MasterLCID = COM_GroupLC_main.GroupLCId, ex = "" });


                ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");
                ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
                //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            }
            catch (Exception ex)
            {

                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });

        }

        // GET: COM_GroupLC_Main/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (AppData.intComId == "0" || AppData.intComId == null)
            {
                return NotFound();

            }

            ViewBag.Title = "Edit";


            //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            COM_GroupLC_Main cOM_GroupLC_Main = db.COM_GroupLC_Mains.Where(M => M.GroupLCId == id).FirstOrDefault();
            if (cOM_GroupLC_Main == null)
            {
                return NotFound();
            }

            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", cOM_GroupLC_Main.BuyerId);
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", cOM_GroupLC_Main.CommercialCompanyId);


            ViewBag.masterlc = (from masterlc in db.COM_MasterLCs.Where(m => m.comid.ToString() == AppData.intComId)
                                    //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                    //join s in db.COM_MasterLCExports on masterlc.MasterLCID equals s.MasterLCID
                                    //join ss in db.ExportOrders on s.ExportOrderID equals ss.ExportOrderID
                                    //join si in db.StyleInformation on ss.StyleID equals si.StyleId   
                                where masterlc.BuyerID == cOM_GroupLC_Main.BuyerId &&
                                !db.COM_GroupLC_Subs.Any(f => f.MasterLCID == masterlc.MasterLCID && masterlc.comid.ToString() == AppData.intComId)
                                select masterlc).Distinct().ToList();


            return View("Create", cOM_GroupLC_Main);

        }



        // GET: COM_GroupLC_Main/Delete/5
        public ActionResult Delete(int? id)
        {
                  if (AppData.intComId == "0" || AppData.intComId == null)
            {
                return NotFound();

            }

            ViewBag.Title = "Delete";
            if (id == null)
            {
                return BadRequest();
            }


            ViewBag.BuyerID = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            //ViewBag.CurrencyId = new SelectList(db.Currency, "CurrencyId", "CurCode");
            COM_GroupLC_Main cOM_GroupLC_Main = db.COM_GroupLC_Mains.Where(M => M.GroupLCId == id).FirstOrDefault();
            if (cOM_GroupLC_Main == null)
            {
                return NotFound();
            }

            ViewBag.masterlc = (from masterlc in db.COM_MasterLCs.Where(m => m.comid.ToString() == AppData.intComId)
                                    //join s in db.COM_MasterLC_Detailss on masterlc.MasterLCID equals s.MasterLCID
                                    //join s in db.COM_MasterLCExports on masterlc.MasterLCID equals s.MasterLCID
                                    //join ss in db.ExportOrders on s.ExportOrderID equals ss.ExportOrderID
                                    //join si in db.StyleInformation on ss.StyleID equals si.StyleId   
                                where masterlc.BuyerID == cOM_GroupLC_Main.BuyerId &&
                                !db.COM_GroupLC_Subs.Any(f => f.MasterLCID == masterlc.MasterLCID && masterlc.comid.ToString() == AppData.intComId)
                                select masterlc).Distinct().ToList();

            return View("Create", cOM_GroupLC_Main);
        }

        // POST: COM_GroupLC_Main/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                COM_GroupLC_Main cOM_GroupLC_Main = db.COM_GroupLC_Mains.Find(id);
                db.COM_GroupLC_Mains.Remove(cOM_GroupLC_Main);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = cOM_GroupLC_Main.GroupLCId, ex = "" });

            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            // return RedirectToAction("Index");

            //COM_GroupLC_Main cOM_GroupLC_Main = db.COM_GroupLC_Mains.Find(id);
            //db.COM_GroupLC_Mains.Remove(cOM_GroupLC_Main);
            //db.SaveChanges();
            //return RedirectToAction("Index");
        }
        public ActionResult PrintMLCEX(int? id, string type)
        {
            try
            {
                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptMasterLCWiseExport.rdlc");
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptMasterLCWiseExport] '" + id + "','" + AppData.intComId + "'");

                string ReportPath = "~/Report/CommercialReport/rptMasterLCWiseExport.rdlc";
                string SQLQuery = "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptMasterLCWiseExport] '" + id + "','" + AppData.intComId + "'");
                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
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
