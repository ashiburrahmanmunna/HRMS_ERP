using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GTERP.Controllers
{
    [OverridableAuthorize]
    public class ExportInvoiceMastersController : Controller
    {
        private GTRDBContext db;

        public ExportInvoiceMastersController(GTRDBContext context)
        {
            db = context;
        }

        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();

        //[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        // GET: ExportInvoiceMasters
        public ActionResult Index(string UserList, string FromDate, string ToDate)
        {
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>());

            //var UserId = Session["userid"];

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


            //List<AspnetUserList> AspNetUserList = (db.Database.SqlQuery<AspnetUserList>("[prcgetAspnetUserList]  @Criteria", new SqlParameter("Criteria", "ExportInvoiceUser"))).ToList();
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@p0", "ExportInvoiceUser");
            //List<AspnetUserList> AspNetUserList = Helper.ExecProcMapTList<AspnetUserList>("sp_Demo", sqlParameter);
            List<AspnetUserList> AspNetUserList = new List<AspnetUserList>();

            ViewBag.Userlist = new SelectList(AspNetUserList, "UserId", "UserName");


            if (UserList == null)
            {
                UserList = HttpContext.Session.GetString("userid");
            }


            //var x = db.ExportInvoiceMasters.Where(p => (p.userid.ToString() == "") && (p.InvoiceDate >= dtFrom && p.InvoiceDate <= dtTo)).ToList();
            var x = db.ExportInvoiceMasters.Where(p => p.UserId.ToString() == "" && p.InvoiceDate >= dtFrom && p.InvoiceDate <= dtTo).ToList();

            if (UserList == "")
            {
                x = db.ExportInvoiceMasters.Where(p => (p.InvoiceDate >= dtFrom && p.InvoiceDate <= dtTo)).ToList();
                //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


            }
            else
            {
                x = db.ExportInvoiceMasters.Where(p => p.UserId == UserList && p.InvoiceDate >= dtFrom && p.InvoiceDate <= dtTo).ToList();
                //x = db.COM_BBLC_Details.Include(r => r.COM_ProformaInvoice).Where(p => (p.COM_ProformaInvoice.SupplierId.ToString() == supplierid.ToString()) && (p.COM_ProformaInvoice.PIDate >= dtFrom && p.COM_ProformaInvoice.PIDate <= dtTo)).ToList();


            }


            return View(x);
            //if (User.IsInRole("Admin"))
            //{
            //var exportInvoiceMasters = db.ExportInvoiceMasters;
            //    IQueryable<ExportInvoiceMaster> ExportInvoiceMasters = db.ExportInvoiceMasters;
            //    return View(ExportInvoiceMasters.ToList());
            //}
            //else
            //{


            //    IQueryable<ExportInvoiceMaster> ExportInvoiceMasters = db.ExportInvoiceMasters.Where(x => x.userid == UserId.ToString());
            //    return View(ExportInvoiceMasters.ToList());


            //}

        }

        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        // GET: ExportInvoiceMasters/Details/5

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]

        // GET: ExportInvoiceMasters/Create
        public ActionResult Create(int? MasterLCId)
        {


            ViewBag.Title = "Create";
            ViewBag.MasterLCIDNo = MasterLCId;
            if (MasterLCId == null)
            {
                MasterLCId = 0;
            }








            ViewBag.ExportDetails = db.ExportInvoiceMasters.Where(m => m.MasterLCId == 0).ToList();
            ViewBag.ExportPackingList = db.ExportInvoicePackingLists.Where(m => m.ExportInvoiceDetailsId == 0).ToList(); /////fahad


            if (MasterLCId > 0)
            {
                COM_MasterLC testMasterLC = new COM_MasterLC();
                testMasterLC = db.COM_MasterLCs.Where(m => m.MasterLCID == MasterLCId).FirstOrDefault();

                ExportInvoiceMaster ExportInvoiceMasterss = new ExportInvoiceMaster();

                ExportInvoiceMasterss.MasterLCId = testMasterLC.MasterLCID;



                ExportInvoiceMasterss.InvoiceNo = ""; //testMasterLC.LCRefNo;
                ExportInvoiceMasterss.InvoiceDate = DateTime.Now.Date;

                ExportInvoiceMasterss.DeliveryTerm = "";
                //ExportInvoiceMasterss.SisterConcernCompanyId = 1;
                //ExportInvoiceMasterss.SupplierId = 1;
                //ExportInvoiceMasterss.DestinationId = testMasterLC.DestinationId;
                //ExportInvoiceMasterss.ShipModeId = testMasterLC.ShipModeId;
                //ExportInvoiceMasterss.BuyerId = testMasterLC.BuyerID;
                //ExportInvoiceMasterss.PortOfLoadingId = testMasterLC.PortOfLoadingId;


                ExportInvoiceMasterss.ExfactoryDate = DateTime.Now.Date;
                ExportInvoiceMasterss.OnboardDate = DateTime.Now.Date;


                ExportInvoiceMasterss.ExpNo = "";
                //ExportInvoiceMasterss.ExpDate = testMasterLC.LCExpirydate;
                ExportInvoiceMasterss.BLNo = "";
                ExportInvoiceMasterss.BLDate = DateTime.Now.Date;
                ExportInvoiceMasterss.TotalShipped = 0;
                ExportInvoiceMasterss.GoodsDescription = "";
                ExportInvoiceMasterss.BalanceShip = 0;
                ExportInvoiceMasterss.CartonMeasurement = "";
                ExportInvoiceMasterss.TotalInvoiceQty = testMasterLC.TotalLCQty;
                ExportInvoiceMasterss.Rate = 0;
                ExportInvoiceMasterss.TotalValue = 0;

                ExportInvoiceMasterss.Discount = testMasterLC.BuyerInformations.DiscountPercentage;
                ExportInvoiceMasterss.CMPPercentage = testMasterLC.BuyerInformations.CMPPercentage;

                var abc = db.NotifyPartys.Where(x => x.BuyerId == testMasterLC.BuyerID).FirstOrDefault();

                List<NotifyParty> NotifyParty = db.NotifyPartys.Where(x => x.BuyerInformations.BuyerGroupId == testMasterLC.BuyerGroupID).ToList();
                //ExportInvoiceMasterss.PortOfDischargeId = abc.PortOfDischargeId;



                ///for gettting the last inputed data related buyer
                var PrevExportInv = db.ExportInvoiceMasters.Where(x => x.BuyerId == testMasterLC.BuyerID).OrderByDescending(y => y.InvoiceId).FirstOrDefault();

                if (PrevExportInv != null)
                {
                    ExportInvoiceMasterss.InvoiceNo = PrevExportInv.InvoiceNo;
                    ExportInvoiceMasterss.VesselName = PrevExportInv.VesselName;
                    ExportInvoiceMasterss.VoyageNo = PrevExportInv.VoyageNo;
                    ExportInvoiceMasterss.ExpNo = PrevExportInv.ExpNo;
                    ExportInvoiceMasterss.BLNo = PrevExportInv.BLNo;
                    ExportInvoiceMasterss.GoodsDescription = PrevExportInv.GoodsDescription;
                    ExportInvoiceMasterss.CartonMeasurement = PrevExportInv.CartonMeasurement;

                    ExportInvoiceMasterss.ShipmentAuthorization = PrevExportInv.ShipmentAuthorization;
                    ExportInvoiceMasterss.Remarks = PrevExportInv.Remarks;
                    ExportInvoiceMasterss.Session = PrevExportInv.Session;


                }



                //ExportInvoice.ExportInvoiceDetails = new List<ExportInvoiceDetails>();
                //foreach (var item in testMasterLC.COM_MasterLC_Details)
                //{
                //    ExportInvoiceDetails ExportInvoiceDetailss = new ExportInvoiceDetails();
                //    ExportInvoiceDetailss.StyleNo = item.StyleName;
                //    ExportInvoiceDetailss.ExportPoNo = item.ExportPONo;
                //    ExportInvoiceDetailss.ShipmentDate = item.ShipmentDate;
                //    ExportInvoiceDetailss.MasterLCDetailsID = item.MasterLCDetailsID;
                //    ExportInvoiceDetailss.Destination = item.Destination;
                //    ExportInvoiceDetailss.LCQty = item.OrderQty;
                //    ExportInvoiceDetailss.UnitMasterId = item.UnitMasterId;

                //    ExportInvoiceDetailss.COM_MasterLC_Detail = item.COM_MasterLC.COM_MasterLC_Details.Where(m=>m.MasterLCDetailsID == item.MasterLCDetailsID).FirstOrDefault();
                //    //ExportInvoiceDetailss.COM_MasterLC_Detail.TotalValue = item.TotalValue;


                //    ExportInvoiceDetailss.InvoiceQty = 0;
                //    ExportInvoiceDetailss.InvoiceRate = item.UnitPrice;
                //    ExportInvoiceDetailss.InvoiceValue = item.TotalValue;

                //    ExportInvoiceDetailss.GrossWeightLine = 0;
                //    ExportInvoiceDetailss.NetWeightLine = 0;
                //    ExportInvoiceDetailss.CBMLine = 0;
                //    ExportInvoiceDetailss.CartonQty = 0;


                //    ExportInvoiceMasterss.ExportInvoiceDetails.Add(ExportInvoiceDetailss);
                //}




                ViewBag.BuyerId = new SelectList(db.BuyerInformation.Where(x => x.BuyerGroupId == testMasterLC.BuyerGroupID && x.CountryId == testMasterLC.Destinations.CountryId), "BuyerId", "BuyerSearchName");
                ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName");

                ViewBag.FirstNotifyPartyId = new SelectList(db.NotifyPartys, "NotifyPartyId", "NotifyPartyNameSearch");
                ViewBag.SecoundNotifyPartyId = new SelectList(db.NotifyPartys, "NotifyPartyId", "NotifyPartyNameSearch");
                ViewBag.ThirdNotifyPartyId = new SelectList(db.NotifyPartys, "NotifyPartyId", "NotifyPartyNameSearch");

                if (PrevExportInv != null)
                {
                    ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", PrevExportInv.TradeTermId);
                }
                else
                {
                    ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName");

                }

                ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo");
                //ViewBag.DestinationId = new SelectList(db.Destinations.Where(x => x.CountryId == testMasterLC.BuyerInformations.CountryId), "DestinationID", "DestinationNameSearch");
                ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", testMasterLC.DestinationId);

                ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName");
                ViewBag.ManufactureId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName");

                ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
                if (PrevExportInv != null)
                {
                    ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", PrevExportInv.PortOfLoadingId);
                }
                else
                {
                    ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName");

                }
                if (abc != null)
                {
                    ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", abc.PortOfDischargeId);
                }
                else
                {
                    ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName");

                }


                if (PrevExportInv != null)
                {
                    ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", PrevExportInv.ShipModeId);
                }
                else
                {
                    ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName");

                }



                if (PrevExportInv != null)
                {

                    ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", PrevExportInv.OpeningBankId);
                }
                else
                {
                    ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName");

                }


                if (PrevExportInv != null)
                {

                    ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber", PrevExportInv.BankAccountId);
                }
                else
                {
                    ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber");

                }
                ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");


                //ViewBag.ExportInvoiceDetailsInformation = db.COM_MasterLC_Detailss.Where(m => m.COM_MasterLC.MasterLCID == MasterLCId).ToList();


                //List<ExportOrderDetailsModel> ExportInvoiceDetailsInformations = (db.Database.SqlQuery<ExportOrderDetailsModel>("[ExportInvoiceDetailsInformation]  @comid, @userid,@MasterLCId", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("MasterLCId", MasterLCId))).ToList();

                //new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("MasterLCId", MasterLCId))).ToList()

                SqlParameter[] SqlParameter = new SqlParameter[3];
                SqlParameter[0] = new SqlParameter("@p0", HttpContext.Session.GetString("comid"));
                SqlParameter[1] = new SqlParameter("@p1", HttpContext.Session.GetString("userid"));
                SqlParameter[2] = new SqlParameter("@p2", MasterLCId);

                List<ExportOrderDetailsModel> ExportInvoiceDetailsInformations = Helper.ExecProcMapTList<ExportOrderDetailsModel>("ExportInvoiceDetailsInformation", SqlParameter);

                //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
                ViewBag.ExportInvoiceDetailsInformation = ExportInvoiceDetailsInformations;

                return View(ExportInvoiceMasterss);
            }


            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName");
            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName");

            ViewBag.FirstNotifyPartyId = new SelectList(db.NotifyPartys, "NotifyPartyId", "NotifyPartyNameSearch");
            ViewBag.SecoundNotifyPartyId = new SelectList(db.NotifyPartys, "NotifyPartyId", "NotifyPartyNameSearch");
            ViewBag.ThirdNotifyPartyId = new SelectList(db.NotifyPartys, "NotifyPartyId", "NotifyPartyNameSearch");

            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName");

            ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCID", "LCRefNo");
            ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch");
            ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName");
            ViewBag.ManufactureId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName");

            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName");
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName");





            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName");
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");

            ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber");
            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName");


            return View();
        }

        public JsonResult GetExportInvoiceSummaryByMasterLCId(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //ViewBag.MasterLCID = id;
            //List<cOM_BBLC_Master> asdf = db.cOM_BBLC_Masters.Where(p => (p.MasterLCID.ToString() == id.ToString())).ToList();
            List<ExportInvoiceMaster> ExportInvoiceMaster = db.ExportInvoiceMasters.Where(m => m.MasterLCId == id).ToList();
            List<MasterLCInfoClass> data = new List<MasterLCInfoClass>();

            foreach (var item in ExportInvoiceMaster)
            {
                MasterLCInfoClass asdf = new MasterLCInfoClass();
                //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
                asdf.InvoiceId = item.InvoiceId;
                asdf.InvoiceNo = item.InvoiceNo;
                asdf.InvoiceDate = DateTime.Parse(item.InvoiceDate.ToString()).ToString("dd-MMM-yy");
                asdf.TotalInvoiceQty = item.TotalInvoiceQty;
                asdf.TotalValue = item.TotalValue;

                data.Add(asdf);
            }

            return Json(data);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public class MasterLCInfoClass
        {
            public int InvoiceId { get; set; }

            /// </summary>
            public string InvoiceNo { get; set; }
            public string InvoiceDate { get; set; }

            public decimal? TotalInvoiceQty { get; set; }
            public decimal? TotalValue { get; set; }

        }

        // POST: ExportInvoiceMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Bind(/*Include =*/ "InvoiceId,InvoiceNo,InvoiceDate,DeliveryTerm,TotalShipped,BalanceShip,MasterLCID,BuyerId,SisterConcernCompanyId,PortOfLoadingId,DestinationId,ShipModeId,SupplierId,ExfactoryDate,OnboardDate,ExpNo,ExpDate,BLNo,BLDate,GoodsDescription,CartonMeasurement,TotalInvoiceQty,Rate,TotalValue,Addedby,comid,userid,Dateadded,Updatedby,Dateupdated")] 
        public ActionResult Create(ExportInvoiceMaster exportInvoiceMaster)
        {

            try
            {
                if (AppData.AppData.intComId == "0" || AppData.AppData.intComId == null)
                {
                    return Json(new { Success = 0, ex = new Exception("Please Login Again for This Transaction").Message.ToString() });

                }
                ViewBag.Title = "Create";
                //Mastersmain.VoucherInputDate = DateTime.Now.Date;

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    if (exportInvoiceMaster.InvoiceId > 0)
                    {
                        if (exportInvoiceMaster.DateAdded == null)
                        {
                            exportInvoiceMaster.DateAdded = DateTime.Now;
                        }

                        exportInvoiceMaster.DateUpdated = DateTime.Now;
                        exportInvoiceMaster.ComId = (HttpContext.Session.GetString("comid"));


                        if (exportInvoiceMaster.UserId == null)
                        {
                            exportInvoiceMaster.UserId = HttpContext.Session.GetString("userid");

                        }




                        foreach (var item in exportInvoiceMaster.ExportInvoiceDetails)
                        {


                            if (item.ExportInvoiceDetailsId > 0)
                            {



                                foreach (var itemPackingList in item.ExportInvoicePackingLists)
                                {

                                    if (itemPackingList.ExportInvoicePackingListId > 0)
                                    {

                                        if (itemPackingList.isDelete == true)
                                        {

                                            db.Entry(itemPackingList).State = EntityState.Deleted;
                                            //db.ExportInvoicePackingLists.Remove(itemPackingList);

                                        }
                                        else
                                        {

                                            db.Entry(itemPackingList).State = EntityState.Modified;

                                        }



                                    }
                                    else
                                    {
                                        db.ExportInvoicePackingLists.Add(itemPackingList);
                                    }


                                }
                                db.SaveChanges();
                                db.Entry(item).State = EntityState.Modified;



                            }
                            else
                            {

                                db.ExportInvoiceDetailss.Add(item);

                                db.SaveChanges();

                            }
                            //db.SaveChanges();

                        }


                        List<ExportInvoiceDetails> dbexportinvoicelist = db.ExportInvoiceDetailss.Where(x => x.InvoiceId == exportInvoiceMaster.InvoiceId).ToList();
                        //List<ExportInvoiceDetails> newlyaddedlist = new List<ExportInvoiceDetails>();
                        foreach (var item1 in exportInvoiceMaster.ExportInvoiceDetails)
                        {
                            foreach (var item2 in dbexportinvoicelist)
                            {
                                if (item1 == item2)
                                {
                                    item2.isDelete = true;

                                }
                            }
                        }
                        db.ExportInvoiceDetailss.RemoveRange(dbexportinvoicelist.Where(x => x.isDelete != true));
                        db.Entry(exportInvoiceMaster).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        exportInvoiceMaster.DateAdded = DateTime.Now;
                        exportInvoiceMaster.DateUpdated = DateTime.Now;
                        exportInvoiceMaster.ComId = (HttpContext.Session.GetString("comid"));
                        exportInvoiceMaster.UserId = HttpContext.Session.GetString("userid");

                        db.ExportInvoiceMasters.Add(exportInvoiceMaster);
                        db.SaveChanges();
                    }
                    return Json(new { Success = 1, InvoiceId = exportInvoiceMaster.InvoiceId, ex = "Dave Save Successfully" });

                    // return RedirectToAction("Index");
                }
                else
                {
                    return Json(new { Success = 0, InvoiceId = exportInvoiceMaster.InvoiceId, ex = "Unable to Save / Update" });

                }

                //return View(exportInvoiceMaster);

            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Success = 0,
                    ex = ex.InnerException.InnerException.Message
                    //ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }


        }

        // GET: ExportInvoiceMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ExportInvoiceMaster exportInvoiceMaster = db.ExportInvoiceMasters.Find(id);
            if (exportInvoiceMaster == null)
            {
                return NotFound();
            }


            //var MasterLCDetails = (from COM_MasterLC_Details in db.COM_MasterLC_Detailss.Where(m => m.MasterLCID == exportInvoiceMaster.MasterLCId)
            //              where !db.ExportInvoiceDetailss.Where(s => s.InvoiceId.ToString() == id.ToString()).Any(f => f.MasterLCDetailsID == COM_MasterLC_Details.MasterLCDetailsID)
            //              select COM_MasterLC_Details).ToList();


            //var MasterLCDetails = (from COM_MasterLC_Details in db.COM_MasterLC_Detailss.Where(m => m.MasterLCID == exportInvoiceMaster.MasterLCId)
            //                       where !db.ExportInvoiceDetailss.Where(s => s.InvoiceId.ToString() == id.ToString()).Any(f => f.MasterLCDetailsID == COM_MasterLC_Details.MasterLCDetailsID)
            //                       select COM_MasterLC_Details).ToList();


            //foreach (var item in MasterLCDetails)
            //{
            //    ExportInvoiceDetails ExportInvoiceDetail = new ExportInvoiceDetails();
            //    ExportInvoiceDetail.MasterLCDetailsID = item.MasterLCDetailsID;


            //    ExportInvoiceDetail.StyleNo = item.StyleName;
            //    ExportInvoiceDetail.ExportPoNo = item.ExportPONo;
            //    ExportInvoiceDetail.ShipmentDate = item.ShipmentDate;
            //    ExportInvoiceDetail.Destination = item.Destination;
            //    ExportInvoiceDetail.LCQty = item.OrderQty;
            //    ExportInvoiceDetail.UnitMasterId = item.UnitMasterId;
            //    ExportInvoiceDetail.InvoiceRate = item.UnitPrice;
            //    ExportInvoiceDetail.InvoiceValue = item.TotalValue;
            //    ExportInvoiceDetail.InvoiceQty = 0;


            //    ExportInvoiceDetail.GrossWeightLine = 0;
            //    ExportInvoiceDetail.NetWeightLine = 0;
            //    ExportInvoiceDetail.CBMLine = 0;
            //    ExportInvoiceDetail.CartonQty = 0;


            //    ExportInvoiceDetail.COM_MasterLC_Detail = item.COM_MasterLC.COM_MasterLC_Details.Where(m => m.MasterLCDetailsID == item.MasterLCDetailsID).FirstOrDefault();



            //    //ExportInvoiceDetail. = 0;


            //    //COM_CNFBillImportDetail.COM_CNFExpenseTypes = new COM_CNFExpenseType { ExpenseHeadID = item.ExpenseHeadID, CNFExpenseNo = item.CNFExpenseNo, CNFExpenseName = item.CNFExpenseName };



            //    exportInvoiceMaster.ExportInvoiceDetails.Add(ExportInvoiceDetail);


            //}


            //ViewBag.ExportInvoiceDetailsInformation = db.COM_MasterLC_Detailss.Where(m => m.COM_MasterLC.MasterLCID == id).ToList();

            //List<ExportOrderDetailsModel> ExportInvoiceDetailsInformations = (db.Database.SqlQuery<ExportOrderDetailsModel>("[ExportInvoiceDetailsInformation]  @comid, @userid,@MasterLCId", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("MasterLCId", exportInvoiceMaster.MasterLCId))).ToList();
            //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");

            SqlParameter[] SqlParameter = new SqlParameter[3];
            SqlParameter[0] = new SqlParameter("@p0", HttpContext.Session.GetString("comid"));
            SqlParameter[1] = new SqlParameter("@p1", HttpContext.Session.GetString("userid"));
            SqlParameter[2] = new SqlParameter("@p2", exportInvoiceMaster.MasterLCId);

            List<ExportOrderDetailsModel> ExportInvoiceDetailsInformations = Helper.ExecProcMapTList<ExportOrderDetailsModel>("ExportInvoiceDetailsInformation", SqlParameter);



            ViewBag.ExportInvoiceDetailsInformation = ExportInvoiceDetailsInformations;


            ViewBag.Title = "Edit";
            ViewBag.ExportDetails = db.ExportInvoiceDetailss.Take(100).Where(m => m.ExportInvoiceMasters.MasterLCId == 0).ToList();
            //ViewBag.ExportDetails = db.ExportInvoiceMasters.Where(m => m.MasterLCId == 0).ToList();


            ViewBag.BuyerId = new SelectList(db.BuyerInformation.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "BuyerId", "BuyerSearchName", exportInvoiceMaster.BuyerId);
            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups.Where(x => x.BuyerGroupId == exportInvoiceMaster.BuyerGroupId), "BuyerGroupId", "BuyerGroupName", exportInvoiceMaster.BuyerGroupId);


            //ViewBag.FirstNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.NotifyPartyId == exportInvoiceMaster.FirstNotifyPartyId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.FirstNotifyPartyId);
            //ViewBag.SecoundNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.NotifyPartyId == exportInvoiceMaster.SecoundNotifyPartyId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.SecoundNotifyPartyId);
            //ViewBag.ThirdNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.NotifyPartyId == exportInvoiceMaster.ThirdNotifyPartyId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.ThirdNotifyPartyId);


            var a = db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId).ToList();


            ViewBag.FirstNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.FirstNotifyPartyId);

            ViewBag.SecoundNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.SecoundNotifyPartyId);
            ViewBag.ThirdNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.ThirdNotifyPartyId);




            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", exportInvoiceMaster.TradeTermId);

            ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCId", "LCRefNo", exportInvoiceMaster.MasterLCId);
            ViewBag.DestinationId = new SelectList(db.Destinations.Where(x => x.CountryId == exportInvoiceMaster.BuyerInformation.CountryId), "DestinationID", "DestinationNameSearch", exportInvoiceMaster.DestinationId);
            ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", exportInvoiceMaster.CommercialCompanyId);
            ViewBag.ManufactureId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", exportInvoiceMaster.ManufactureId);

            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", exportInvoiceMaster.SupplierId);
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", exportInvoiceMaster.PortOfLoadingId);
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", exportInvoiceMaster.PortOfDischargeId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", exportInvoiceMaster.ShipModeId);

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");

            ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber", exportInvoiceMaster.BankAccountId);
            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", exportInvoiceMaster.OpeningBankId);
            return View("Create", exportInvoiceMaster);
        }


        public class ExportOrderDetailsModel
        {

            public int MasterLCDetailsID { get; set; }

            public int ExportInvoiceDetailsId { get; set; }

            public string StyleName { get; set; }

            public string ExportPONo { get; set; }

            public string ShipmentDate { get; set; }

            public string Destination { get; set; }
            public float OrderQty { get; set; }
            public string UnitMasterId { get; set; }
            public decimal UnitPrice { get; set; }

            public decimal TotalValue { get; set; }


            public string PODate { get; set; }
            public string ColorCode { get; set; }
            public string DestinationPO { get; set; }

            //public int InvoiceQty { get; set; }
            //public decimal InvoiceRate { get; set; }


            //public decimal InvoiceValue { get; set; }
            //public decimal GrossWeightLine { get; set; }
            //public decimal NetWeightLine { get; set; }
            //public decimal CBMLine { get; set; }
            //public int CartonQty { get; set; }



        }

        // POST: ExportInvoiceMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExportInvoiceMaster exportInvoiceMaster)
        {
            //[Bind(/*Include =*/ "InvoiceId,InvoiceNo,InvoiceDate,DeliveryTerm,TotalShipped,BalanceShip,MasterLCID,BuyerId,SisterConcernCompanyId,PortOfLoadingId,DestinationId,ShipModeId,SupplierId,ExfactoryDate,OnboardDate,ExpNo,ExpDate,BLNo,BLDate,GoodsDescription,CartonMeasurement,TotalInvoiceQty,Rate,TotalValue,Addedby,comid,userid,Dateadded,Updatedby,Dateupdated")]
            if (ModelState.IsValid)
            {
                db.Entry(exportInvoiceMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", exportInvoiceMaster.BuyerId);
            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName", exportInvoiceMaster.BuyerGroupId);

            ViewBag.FirstNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.FirstNotifyPartyId);
            ViewBag.SecoundNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.SecoundNotifyPartyId);
            ViewBag.ThirdNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.ThirdNotifyPartyId);


            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", exportInvoiceMaster.TradeTermId);

            ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCId", "LCRefNo", exportInvoiceMaster.MasterLCId);
            ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", exportInvoiceMaster.DestinationId);
            ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", exportInvoiceMaster.CommercialCompanyId);
            ViewBag.ManufactureId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", exportInvoiceMaster.ManufactureId);


            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "SupplierId", "SupplierName", exportInvoiceMaster.SupplierId);
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", exportInvoiceMaster.PortOfLoadingId);
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", exportInvoiceMaster.PortOfDischargeId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", exportInvoiceMaster.ShipModeId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View(exportInvoiceMaster);
        }

        // GET: ExportInvoiceMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ExportInvoiceMaster exportInvoiceMaster = db.ExportInvoiceMasters.Find(id);
            if (exportInvoiceMaster == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";
            //ViewBag.ExportDetails = db.ExportInvoiceDetailss.Where(m => m.ExportInvoiceMasters.MasterLCId == 0).ToList();

            ViewBag.ExportDetails = db.ExportInvoiceMasters.Where(m => m.MasterLCId == 0).ToList();


            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerSearchName", exportInvoiceMaster.BuyerId);
            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName", exportInvoiceMaster.BuyerGroupId);

            ViewBag.FirstNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.FirstNotifyPartyId);
            ViewBag.SecoundNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.SecoundNotifyPartyId);
            ViewBag.ThirdNotifyPartyId = new SelectList(db.NotifyPartys.Where(x => x.BuyerId == exportInvoiceMaster.BuyerId), "NotifyPartyId", "NotifyPartyNameSearch", exportInvoiceMaster.ThirdNotifyPartyId);



            ViewBag.TradeTermId = new SelectList(db.TradeTerms, "TradeTermId", "TradeTermName", exportInvoiceMaster.TradeTermId);

            ViewBag.MasterLCID = new SelectList(db.COM_MasterLCs, "MasterLCId", "LCRefNo", exportInvoiceMaster.MasterLCId);
            ViewBag.DestinationId = new SelectList(db.Destinations, "DestinationID", "DestinationNameSearch", exportInvoiceMaster.DestinationId);
            ViewBag.SisterConcernCompanyId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", exportInvoiceMaster.CommercialCompanyId);
            ViewBag.ManufactureId = new SelectList(db.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName", exportInvoiceMaster.ManufactureId);

            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName", exportInvoiceMaster.SupplierId);
            ViewBag.PortOfLoadingId = new SelectList(db.PortOfLoadings, "PortOfLoadingId", "PortOfLoadingName", exportInvoiceMaster.PortOfLoadingId);
            ViewBag.PortOfDischargeId = new SelectList(db.PortOfDischarges, "PortOfDischargeId", "PortOfDischargeName", exportInvoiceMaster.PortOfDischargeId);
            ViewBag.ShipModeId = new SelectList(db.ShipModes, "ShipModeId", "ShipModeName", exportInvoiceMaster.ShipModeId);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");




            ViewBag.BankAccountId = new SelectList(db.BankAccountNos, "BankAccountId", "BankAccountNumber", exportInvoiceMaster.BankAccountId);
            ViewBag.OpeningBankId = new SelectList(db.OpeningBanks, "OpeningBankId", "OpeningBankName", exportInvoiceMaster.OpeningBankId);


            return View("Create", exportInvoiceMaster);
        }

        // POST: ExportInvoiceMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ExportInvoiceMaster exportInvoiceMaster = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).FirstOrDefault();
                db.ExportInvoiceMasters.Remove(exportInvoiceMaster);
                db.SaveChanges();
                return Json(new { Success = 1, TermsId = exportInvoiceMaster.InvoiceId, ex = "" });

            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }


        }

        public JsonResult MasterLCInfoByMasterLCId(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;



            COM_MasterLC COM_MasterLCs = db.COM_MasterLCs.Where(m => m.MasterLCID == id).FirstOrDefault();

            //}

            MasterLCInfo data = new MasterLCInfo();

            data.BuyerId = COM_MasterLCs.BuyerID;
            data.BuyerGroupId = COM_MasterLCs.BuyerGroupID;
            data.PortOfLoadingId = COM_MasterLCs.PortOfLoadingId;
            data.PortOfDischargeId = COM_MasterLCs.PortOfDischargeId;
            data.SisterConcernCompanyId = COM_MasterLCs.CommercialCompanyId;
            data.ShipModeId = COM_MasterLCs.ShipModeId;
            data.DestinationId = COM_MasterLCs.DestinationId;
            data.MasterLCId = COM_MasterLCs.MasterLCID;
            data.TotalLCQty = COM_MasterLCs.TotalLCQty;



            //var x = db.COM_GroupLC_Subs.Where(y => y.GroupLCId == COM_MasterLCs.GroupLCId).ToList();

            //string jointext = "";
            //foreach (var item in x)
            //{
            //    jointext = jointext.ToString() + item.COM_MasterLC.BuyerLCRef + ",";
            //    data.BuyerId = item.COM_MasterLC.BuyerID;
            //}
            //jointext = jointext.Substring(0, jointext.Length - 1);

            //data.MasterLCRef = jointext;


            return Json(data);
            //return Json(new { Success = 1, data = asdf }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuyerWiseDiscountbyBuyerId(int? id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;



            BuyerInformation data = db.BuyerInformation.Where(m => m.BuyerId == id).FirstOrDefault();


            return Json(data);

            //return Json(new { Success = 1, data }, JsonRequestBehavior.AllowGet);
        }


        public class MasterLCInfo
        {

            /// </summary>
            public int BuyerId { get; set; }
            public int? BuyerGroupId { get; set; }

            public int? PortOfLoadingId { get; set; }
            public int? PortOfDischargeId { get; set; }

            public int SisterConcernCompanyId { get; set; }
            public int ShipModeId { get; set; }
            public int DestinationId { get; set; }

            public int? MasterLCId { get; set; }
            public Decimal? TotalLCQty { get; set; }
            public int TradeTermId { get; set; }


            //public DateTime ExfactoryDate { get; set; }
            //public DateTime OnBoardDate { get; set; }
            //public string ExpNo { get; set; }
            //public DateTime ExpDate { get; set; }
            //public string BLNo { get; set; }

            //public DateTime BLDate { get; set; }
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIE(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);



                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEETAM(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_ETAM.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIESAINSBURY(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_SAINSBURYS.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIELPP(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_LPP.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIENEXT(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_NEXT.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEHM(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_HM.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public ActionResult PrintCIEHM_WAREHOUSE(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_HM_Warehouse.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEPVH(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_PVH.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //string a = filename.Replace(@"\", "");

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETEDDY(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;


                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_TEDDY.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEPRIMARK(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_PRIMARK.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEDRESSMAN(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_DressmanAB.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEMANDS(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_MANDS.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIECANDA(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_CANDA.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETRINITY(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_CANDA_Trinity.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETRINITY_APPARELS(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Trinity_APPARELS.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETRINITY_AVENGER(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Trinity_AVENGER.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETRINITY_BELAMY(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Trinity_BELAMY.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETRINITY_DYEING(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Trinity_FOUR_H_DYEING.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETRINITY_ISLAMPACK(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Trinity_ISLAM_PACK.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETRINITY_BRANDELLA(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Trinity_Brandella.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETESCO(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_TESCO.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETESCOUK(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_TescoUK.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETARGET(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_TargetAUS.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIELINDEX(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_EXPORT_AB_LINDEX.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEPENNEY(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_JCPENNEY.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIERNA(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_RNA.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEVERTBAUDET(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_VERTBAUDET.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEEMMAROSE(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_EmmaRose.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIETEXSPORT(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_TEXSPORT.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEASOS(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_EXPORT_ASOS.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEMOTHERCARE(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_MOTHERCARE.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEOKAIDI(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_OKAIDI.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEMIES(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_MIES.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIECARTER(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Carters.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEASDA(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_ASDA.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEBRANDELLA(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Brandella.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEWOLF(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_WOLF.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintCIEWALMART(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;

                //Session["rptList"] = null;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/CommercialReport/rptCommercialInvoice_Export_Walmart.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'");
                string filename = db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.BuyerInformation.BuyerName + "_" + x.ShipMode.ShipModeName + "_" + x.Destination.DestinationName + "_" + x.InvoiceNo).Single();
                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                //var a = Session["PrintFileName"].ToString();


                string DataSourceName = "DataSet1";
                //string FormCaption = "Report :: Sales Acknowledgement ...";


                //postData.Add(1, new subReport("rptInvoice_Terms", "", "DataSet1", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.rptInvoice_Terms '" + id + "','" + Session["comid"].ToString() + "',''"));


                HttpContext.Session.SetObject("rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;


                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpPost, ActionName("SetSessionInd")]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSessionInd(string reporttype, string action, string reportid)
        {
            try
            {

                if (action == "PrintCIE")
                {

                    //ExportInvoiceMaster exportInvoiceMaster = new ExportInvoiceMaster();

                    //if (db.ExportInvoiceMasters.wher)
                    //{

                    //}


                }

                HttpContext.Session.SetString("ReportType", reporttype);
                return Json(new { Success = 1 });

            }

            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }


        }

        public JsonResult getNotifyPartyList(int id, int? finaldestinationid)
        {

            Destination FinalDestination = db.Destinations.Where(x => x.DestinationID == finaldestinationid).FirstOrDefault();
            List<NotifyParty> NotifyParty = db.NotifyPartys.Where(x => x.BuyerId == id).ToList();



            int? countryid = FinalDestination.CountryId;
            if (countryid == null)
            {

            }
            else
            {
                NotifyParty = db.NotifyPartys.Where(x => x.BuyerId == id && x.CountryId == countryid).ToList();

            }

            List<SelectListItem> linotify = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (NotifyParty != null)
            {
                foreach (NotifyParty x in NotifyParty)
                {
                    linotify.Add(new SelectListItem { Text = x.NotifyPartyNameSearch, Value = x.NotifyPartyId.ToString() });
                }
            }
            return Json(new SelectList(linotify, "Value", "Text"));
        }


        public JsonResult getPortOfDischargeList(int id)
        {

            //PortOfDischarge PortOfDischarges = db.PortOfDischarges.Where(x => x.PortOfDischargeId == id).FirstOrDefault();
            List<PortOfDischarge> PortOfDischarges = db.PortOfDischarges.Where(x => x.PortOfDischargeId == id).ToList();

            List<SelectListItem> liportofdischarge = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (PortOfDischarges.Count > 0)
            {
                foreach (PortOfDischarge x in PortOfDischarges)
                {
                    liportofdischarge.Add(new SelectListItem { Text = x.PortOfDischargeName, Value = x.PortOfDischargeId.ToString() });
                }
            }
            else
            {
                List<PortOfDischarge> allPortOfDischarges = db.PortOfDischarges.ToList();

                foreach (PortOfDischarge x in allPortOfDischarges)
                {
                    liportofdischarge.Add(new SelectListItem { Text = x.PortOfDischargeName, Value = x.PortOfDischargeId.ToString() });
                }
            }

            return Json(new SelectList(liportofdischarge, "Value", "Text"));
        }

        public JsonResult getBuyerList(int buyergroupid, int? finaldestinationid)
        {
            Destination FinalDestination = db.Destinations.Where(x => x.DestinationID == finaldestinationid).FirstOrDefault();
            List<BuyerInformation> BuyerInformations = db.BuyerInformation.Where(x => x.BuyerGroupId == buyergroupid).ToList();

            int? countryid = FinalDestination.CountryId;
            if (countryid == null)
            {

            }
            else
            {
                BuyerInformations = db.BuyerInformation.Where(x => x.BuyerGroupId == buyergroupid && x.CountryId == countryid).ToList();

            }

            List<SelectListItem> listbuyer = new List<SelectListItem>();


            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (BuyerInformations != null)
            {
                foreach (BuyerInformation x in BuyerInformations)
                {
                    listbuyer.Add(new SelectListItem { Text = x.BuyerSearchName, Value = x.BuyerId.ToString() });
                }
            }
            return Json(new SelectList(listbuyer, "Value", "Text"));
        }


        public ActionResult ExportShippingReport()
        {

            ViewBag.MasterLCId = new SelectList(db.COM_MasterLCs, "MasterLCId", "LCRefNo");
            ViewBag.InvoiceId = new SelectList(db.ExportInvoiceMasters.Where(m => m.InvoiceId == 0), "InvoiceId", "InvoiceNo");

            return View();

        }


        public ActionResult PrintBL(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptBLDraft_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "BL_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public ActionResult PrintSAFTA(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptSAFTAforIndia_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "SAFTA_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult PrintGSP(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptGSP_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "GSP_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public ActionResult PrintB255(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptB255E_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "B255_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult PrintEPB(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptEPB_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "EPB_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult PrintKPT(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptKEPTKoreaPossible_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "KPT_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //Himu
        public ActionResult PrintBillOfExchange(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptBillOfExchange_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "BillOfExchange_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }






        public ActionResult PrintCOP(int id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptCollectionOfProceeds_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCollectionOfProceeds_Export] '" + HttpContext.Session.GetString("MultiSelectData") + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "COP_" + db.COM_MasterLCs.Where(x => x.MasterLCID == id).Select(x => x.BuyerLCRef).Single());

                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult PrintCertificateOfOrigin(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptCertificateOfOrigin_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "CertificateOfOrigin_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());

                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult PrintCountryOfOrigin(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptCountryOfOrigin_Export.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "CountryOfOrigin_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());

                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult PrintCanadaCustom(int? id, string type)
        {
            try
            {
                AppData.AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                HttpContext.Session.SetString("ReportPath", "~/Report/ExportReport/rptCanadaCustomInvoice.rdlc");
                HttpContext.Session.SetString("reportquery", "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptExportDocumentsAutomation] '" + id + "','" + AppData.AppData.intComId + "','" + AppData.AppData.RefNo + "','" + AppData.AppData.PrintDate + "'");
                HttpContext.Session.SetString("PrintFileName", "PrintCanadaCustom_" + db.ExportInvoiceMasters.Where(x => x.InvoiceId == id).Select(x => x.InvoiceNo).Single());


                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = HttpContext.Session.GetString("DataSourceName");

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //[HttpPost, ActionName("SetSession")]
        ////[ValidateAntiForgeryToken]
        //public JsonResult SetSession(string reporttype, string action, int masterlcid, string reportid, DateTime printdate)
        //{
        //    try
        //    {

        //        HttpContext.Session.SetString("ReportType",reporttype);
        //        HttpContext.Session.SetString("MultiSelectData",reportid);

        //        AppData.AppData.PrintDate = printdate.ToString();

        //        var redirectUrl = "";
        //        //return Json(new { Success = 1, TermsId = param, ex = "" });
        //        if (action == "PrintCOP")
        //        {
        //            redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "ExportInvoiceMasters", new { id = masterlcid }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
        //        }
        //        else
        //        {
        //            var vals = reportid.Split(',')[0];
        //            redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "ExportInvoiceMasters", new { id = vals }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }

        //        }
        //        return Json(new { Url = redirectUrl });

        //    }

        //    catch (Exception ex)
        //    {
        //        // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
        //        return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
        //    }

        //    return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
        //    //return RedirectToAction("Index");

        //}

        public JsonResult getExportInvoiceByMasterLC(int id)
        {
            List<ExportInvoiceMaster> ExportInvoiceMasters = db.ExportInvoiceMasters.Where(x => x.MasterLCId == id).ToList();

            List<SelectListItem> liexportinvoice = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (ExportInvoiceMasters != null)
            {
                foreach (ExportInvoiceMaster x in ExportInvoiceMasters)
                {
                    liexportinvoice.Add(new SelectListItem { Text = x.InvoiceNo, Value = x.InvoiceId.ToString() });
                }
            }
            return Json(new SelectList(liexportinvoice, "Value", "Text"));
        }


        public JsonResult ExportInvoiceInfoByInvoiceId(string[] id)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;

            //string[] partsFromString = id.Split(new string[] { "," }, StringSplitOptions.None);

            //string stringCutted = id.Split(',').Last();


            int a = int.Parse(id.Last());

            ExportInvoiceMaster ExportInvoiceMasters = db.ExportInvoiceMasters.Where(m => m.InvoiceId == a).FirstOrDefault();

            //}

            ExportInvoiceInfo data = new ExportInvoiceInfo();

            data.BuyerName = ExportInvoiceMasters.BuyerInformation.BuyerName;
            data.ExporterName = ExportInvoiceMasters.CommercialCompany.CompanyName;
            data.TotalInvoiceQty = ExportInvoiceMasters.TotalInvoiceQty;
            data.TotalValue = ExportInvoiceMasters.TotalValue;



            return Json(data);
        }

        public class ExportInvoiceInfo
        {
            /// </summary>
            public string BuyerName { get; set; }
            public string ExporterName { get; set; }
            public decimal? TotalInvoiceQty { get; set; }
            public decimal? TotalValue { get; set; }
        }


        [HttpPost]
        public JsonResult SaveNotifyParty(NotifyParty notifyParty)
        {
            if (notifyParty.BuyerId != 0)
            {
                db.NotifyPartys.Add(notifyParty);
                db.SaveChanges();
            }
            return Json("");
        }

        public JsonResult SavePortOfLoading(PortOfLoading portOfLoading)
        {
            if (portOfLoading.PortOfLoadingId == 0)
            {
                db.PortOfLoadings.Add(portOfLoading);
                db.SaveChanges();

                db.Entry(portOfLoading).GetDatabaseValues();


            }
            return Json(portOfLoading);
        }
        [HttpPost]
        public JsonResult SavePortOfDischarge(PortOfDischarge portOfDischarge)
        {
            if (portOfDischarge.PortOfDischargeId == 0)
            {
                db.PortOfDischarges.Add(portOfDischarge);
                db.SaveChanges();
                db.Entry(portOfDischarge).GetDatabaseValues();
            }
            return Json(portOfDischarge);
        }

        [HttpPost]
        public JsonResult SaveDestination(Destination destination)
        {
            if (destination.DestinationID == 0)
            {
                db.Destinations.Add(destination);
                db.SaveChanges();

                db.Entry(destination).GetDatabaseValues();
            }
            return Json(destination);
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
