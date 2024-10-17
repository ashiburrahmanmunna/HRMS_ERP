using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.AppData;
using GTCommercial.Models;
using GTCommercial.Models.Common;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class DashboardController : Controller
    {
        private Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private GTRDBContext db = new GTRDBContext();
        // GET: Deshboard

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult BuyerWiseShipmentStatus(int? ComId, int? BuyerId, int? Year)
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");

            return View(); //p.ComId == AppData.intComId && 

        }

        [HttpGet]
        public ActionResult GetData(int? ComId, int? BuyerId, int? Year)
        {


            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            if (BuyerId == null)
            {
                BuyerId = 0;
            }
            if (ComId == null)
            {
                ComId = 0;
            }
            if (Year == null)
            {
                Year = 0;
            }

            List<HMShipmentStatusModel> HMShipmentStatus = (db.Database.SqlQuery<HMShipmentStatusModel>("[rptDashboardShipmentStatus]  @ComId, @Id,@Year",
                new SqlParameter("ComId", ComId),
                new SqlParameter("Id", BuyerId),
                new SqlParameter("Year", Year))).ToList();
            //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.HMShipmentStatus = HMShipmentStatus;

            return Json(HMShipmentStatus, JsonRequestBehavior.AllowGet); //p.ComId == AppData.intComId && 
        }



        public class HMShipmentStatusModel
        {
            public string YearNo { get; set; }

            public string MonthName { get; set; }

            public int PCS { get; set; }

            public int DOZEN { get; set; }

            public decimal USD { get; set; }

            public decimal MILLIONS { get; set; }

        }





        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult GetOverDueShipment()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.BuyerId = new SelectList(db.BuyerInformation, "BuyerId", "BuyerName");
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult SetOverDueShipment(int? ComId, int? BuyerId , DateTime dtDate)
        {
            try
            {



                if (BuyerId == null)
                {
                    BuyerId = 0;
                }
                if (ComId == null)
                {
                    ComId = 0;
                }

                List<OverDueShipment> overDueShipments = (db.Database.SqlQuery<OverDueShipment>("[rptDashboardOverDueShipment]  @ComId, @Id , @dtDate",
                    new SqlParameter("ComId", ComId),
                    new SqlParameter("Id", BuyerId),
                    new SqlParameter("dtDate", dtDate))).ToList();
                return Json(overDueShipments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class OverDueShipment
        {
            public string CompanyName { get; set; }
            public string BuyerName { get; set; }
            public string LCRefNo { get; set; }
            public string StyleName { get; set; }
            public string ExportPONo { get; set; }
            public string HSCode { get; set; }
            public DateTime ShipmentDate { get; set; }
            public string DaysDue { get; set; }
            public string Destination { get; set; }
            public int OrderQty { get; set; }
            public string UnitMasterId { get; set; }
            public int QtyInPcs { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalValue { get; set; }

        }




        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult BBLCDetailsChart()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult SetBBLCDetails(int? ComId , string FromDate, string ToDate)
        {
            try
            {
                if (ComId == null)
                {
                    ComId = 0;
                }

                string sqlquery = "select *  from dbo.VWBBLCDetails where CompanyId = " + ComId + " and LCOpeningDate between convert(datetime,'" + FromDate + "') and convert(datetime,'" + ToDate +"')";
                List<BBLCDetails> bblcDetais = db.Database.SqlQuery<BBLCDetails>(sqlquery).ToList();


                sqlquery = "select SupplierName,sum(TotalValue) TotalValue from dbo.VWBBLCDetails where CompanyId = " + ComId + " and LCOpeningDate between convert(datetime,'" + FromDate + "') and convert(datetime,'" + ToDate + "') Group by SupplierName";
                List<BBLCChartSummary> bblcChartSummary = db.Database.SqlQuery<BBLCChartSummary>(sqlquery).ToList();


                var data = new { datad = bblcDetais, datas = bblcChartSummary };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public class BBLCChartSummary
        {

            public string SupplierName { get; set; }
            public decimal TotalValue { get; set; }

        }


        public class BBLCDetails
        {
            public string BBLCNo { get; set; }
            public string CompanyName { get; set; }
            public string SupplierName { get; set; }
            public string LcOpeningDate { get; set; }
            public string ExpiryDate { get; set; }
            public string UDNo { get; set; }
            public string UDDate { get; set; }
            public string PortOfLoadingName { get; set; }
            public string PINo { get; set; }
            public string FileNo { get; set; }
            public string ImportPONo { get; set; }
            public string HSCode { get; set; }
            public string ItemDescription { get; set; }
            public string ItemGroupName { get; set; }
            public decimal ImportRate { get; set; }
            public decimal TotalValue { get; set; }

        }




        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult GroupLCChart()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult GetGroupLC(int? ComId)
        {
            try
            {
                if (ComId == null)
                {
                    ComId = 0;
                }

                var query = "select * from VWGroupLC where CompanyID = " + ComId + " ";
                List<GroupLC> groupLC = db.Database.SqlQuery<GroupLC>(query).ToList();


                query = "select BuyerName,sum(TotalValue) as TotalMLCValue from VWGroupLC where CompanyID = " + ComId + "  group by BuyerName";
                List<GroupLCchart> groupLCChart = db.Database.SqlQuery<GroupLCchart>(query).ToList();

                var data = new { datag = groupLC, datac = groupLCChart };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class GroupLC
        {
            public string CompanyName { get; set; }
            public string BuyerName { get; set; }
            public string GroupLCRefName { get; set; }
            public decimal TotalGroupLCValue { get; set; }
            public string LCRefNo { get; set; }
            public string ExportPONo { get; set; }
            public string HSCode { get; set; }
            public string ItemName { get; set; }
            public string Fabrication { get; set; }
            public decimal TotalValue { get; set; }
            public int QtyInPcs { get; set; }
            public int OrderQty { get; set; }
        }
        public class GroupLCchart
        {
            public string BuyerName { get; set; }
            public decimal TotalMLCValue { get; set; }
        }





        #region rptDashboardMarginAlert Using Procedure

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult rptDashboardMarginAlert()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult rptDashboardMarginAlert(int? ComId)
        {

            if (ComId == null)
            {
                ComId = 0;
            }

            List<MarginAlert> marginAlert = (db.Database.SqlQuery<MarginAlert>("[rptDashboardMarginAlert]  @ComId",
                new SqlParameter("ComId", ComId))).ToList();
            var data = marginAlert;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public class MarginAlert
        {
            public string BBLCNo { get; set; }
            public string CompanyName { get; set; }
            public string SupplierName { get; set; }
            public string BuyerName { get; set; }
            public string LcOpeningDate { get; set; }
            public string ExpiryDate { get; set; }
            public int? TotalPI { get; set; }

            public string PortOfLoadingName { get; set; }
            public decimal TotalValue { get; set; }
            public decimal BBLCValue { get; set; }
            public decimal TotalMLCValue { get; set; }
            public decimal SalesContactAmount { get; set; }
            public decimal MarginUsed { get; set; }
        }
        #endregion rptDashboardMarginAlert Using Procedure

        #region rptDashboardSupplierBillMaturityOverdue Using Procedure
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult rptDashboardSupplierBillMaturityOverdue()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult rptDashboardSupplierBillMaturityOverdue(int? ComId, DateTime dtDate)
        {
            try
            {
                if (ComId == null)
                {
                    ComId = 0;
                }


                List<SupplierBillMaturityOverdue> supplierBillMaturityOverdue = (db.Database.SqlQuery<SupplierBillMaturityOverdue>("[rptDashboardSupplierBillMaturityOverdue]  @ComId, @dtDate",
                new SqlParameter("ComId", ComId), new SqlParameter("dtDate", dtDate))).ToList();
                var data = supplierBillMaturityOverdue;
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class SupplierBillMaturityOverdue
        {
            public string CompanyName { get; set; }
            public string SalesContact { get; set; }
            public string B2BNo { get; set; }
            public string Supplier { get; set; }
            public string BillRef { get; set; }
            public string InvoiceNo { get; set; }// as Total PI
            public string CommercialInvoiceDate { get; set; }
            public string BillDate { get; set; }
            public string MaturityDate { get; set; }

        }
        #endregion rptDashboardSupplierBillMaturityOverdue Using Procedure




        #region rptDashboardBillCreatePending using Procedure

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult rptDashboardBillCreatePending()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult rptDashboardBillCreatePending(int? ComId)
        {
            try
            {
                if (ComId == null)
                {
                    ComId = 0;
                }

                List<BillCreatePending> BillCreatePending = (db.Database.SqlQuery<BillCreatePending>("[rptDashboardBillCreatePending]  @ComId",
                    new SqlParameter("ComId", ComId))).ToList();
                var data = BillCreatePending;
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class BillCreatePending
        {
            public string CompanyName { get; set; }
            public string SupplierName { get; set; }
            public string BBLCNo { get; set; }
            public string CommercialInvoiceNo { get; set; }
            public string CommercialInvoiceDate { get; set; }
            public string ItemGroupName { get; set; }
            public string ItemDescription { get; set; }
            public string BLNo { get; set; }
            public string BLDate { get; set; }
            public string DocumentReceiptDate { get; set; }
            public string DocumentAssesmentDate { get; set; }
            public decimal TotalValue { get; set; }

        }
        #endregion



        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult rptForthComingShipment()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");

            return View();
        }

        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult PrintFCSR(int? id, string type)
        {
            try
            {


                AppData.dbGTCommercial = db.Database.GetDbConnection().Database;
                clsReport.rptList = null;

                Session["ReportPath"] = "~/Report/ExportReport/rptForthComingShipment.rdlc";
                HttpContext.Session.SetString("reportquery","Exec " + AppData.dbGTCommercial.ToString() + ".dbo.[rptForthComingShipment] " + AppData.intComId + " , '" + id + "'";
                HttpContext.Session.SetString("DataSourceName", "DataSet1");

                HttpContext.Session.SetObject("rptList", postData);

                clsReport.strReportPathMain = Session["ReportPath"].ToString();
                clsReport.strQueryMain = Session["reportquery"].ToString();
                clsReport.strDSNMain = Session["DataSourceName"].ToString();

                return RedirectToAction("Index", "ReportViewer");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [HttpPost, ActionName("SetSession")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSession(string reporttype, string action, int reportid)
        {
            try
            {
                Session["ReportType"] = reporttype;
                //AppData.AppDate.PrintDate = printdate.ToString();


                //return Json(new { Success = 1, TermsId = param, ex = "" });
                var redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "Dashboard", new { id = reportid }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                return Json(new { Url = redirectUrl });

            } 

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
            //return RedirectToAction("Index");

        }





        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin , Commercial-User ")]
        public ActionResult ExportInvoiceStatus(int? BuyerGroupId, string FromDate, string ToDate)
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
            ViewBag.BuyerGroupId = new SelectList(db.BuyerGroups, "BuyerGroupId", "BuyerGroupName");
            if (BuyerGroupId == null)
            {
                BuyerGroupId = 0;
            }


            List<ExportInvoiceStatusModel> ProductSerialresult = (db.Database.SqlQuery<ExportInvoiceStatusModel>("[rptExportInvoiceStatus]  @comid, @userid,@dtFrom,@dtTo,@BuyerGroupId", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("dtFrom", dtFrom), new SqlParameter("dtTo", dtTo), new SqlParameter("BuyerGroupId", BuyerGroupId))).ToList();
            //ViewBag.ProductSerial = new SelectList(ProductSerialresult, "ProductSerialId", "ProductSerialNo");
            ViewBag.ExportInvoiceStatus = ProductSerialresult;


            return View(); //p.ComId == AppData.intComId && 



        }

        public class ExportInvoiceStatusModel
        {
            public string RowNo { get; set; }

            public string BuyerLCRef { get; set; }
            public string InvoiceNo { get; set; }
            public decimal TotalInvoiceQty { get; set; }
            public decimal InvoiceValue { get; set; }
            public string CargoHandOverDate { get; set; }




            public string BLNO { get; set; }
           
            public string BankRef { get; set; }



            public decimal RealizationAmount { get; set; } ///TotalValue	ReceivingDate	PIStatus





            public decimal ShortPayment { get; set; }
            public string DestinationName { get; set; }


            public string ExpNo { get; set; }
        
            public string BuyerGroupName { get; set; }
            public string BuyerName { get; set; }


            public string FirstNotifyParty { get; set; }

            public decimal FBPAmount { get; set; }


            public string InvoiceDate { get; set; }
            public string VesselSailingDate { get; set; }

            public string PaymentMaturityDate { get; set; }


            public string PaymentReceiveDate { get; set; }

            public string InvoicePaymentStatus { get; set; }



            public string StatusColor { get; set; }




        }



    }
}