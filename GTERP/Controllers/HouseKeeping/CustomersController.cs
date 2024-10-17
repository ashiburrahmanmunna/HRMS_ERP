using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Controllers.HouseKeeping
{
    [OverridableAuthorize]
    public class CustomersController : Controller
    {
        private TransactionLogRepository tranlog;
        private GTRDBContext db = new GTRDBContext();

        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }

        public CustomersController(GTRDBContext context, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = context;
            //Repository = rep;

        }

        // GET: Customers
        public ActionResult Index()
        {
            ViewBag.DistId = new SelectList(db.Cat_District.Where(c => c.DistId > 0), "DistId", "DistName");
            ViewBag.PStationId = new SelectList(db.Cat_PoliceStation.Where(c => c.PStationId > 0), "PStationId", "PStationName");
            //var customers = db.Customers.Where(c => c.comid == HttpContext.Session.GetString("comid")).ToList(); //.Include(c => c.vCustomerCountry)
            //return View(customers);
            return View();

        }



        public partial class CustomerList
        {
            // FirstSortNumber SortNumber  SLNo Description DealersCode DistId  PStationId RowNumber   PageNumber
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComAddress2 { get; set; }

            public string Caption { get; set; }
            public string FirstSortNumber { get; set; }
            public string SortNumber { get; set; }
            public string SLNo { get; set; }
            public string Description { get; set; }
            public string DealersCode { get; set; }
            public string RowNumber { get; set; }
            public string PageNumber { get; set; }



        }

        public ActionResult ReportCustomer()
        {

            try
            {

                var result = "";
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");

                if (comid == null)
                {
                    result = "Please Login first";
                }

                //@ComId nvarchar(200),   @DistId varchar(15)= '' , @PStationId varchar(15)= ''
                //string criteria = "Month";
                string distid = "";
                string pstationid = "";

                var quary = $"Exec [Sales_rptCustomerList_IndexPage] '" + comid + "','" + distid + "','" + pstationid + "'";

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ComId", comid);
                parameters[1] = new SqlParameter("@DistId", distid);
                parameters[2] = new SqlParameter("@PStationId", pstationid);

                List<CustomerList> CustomerListIndexing = Helper.ExecProcMapTList<CustomerList>("Sales_rptCustomerList_IndexPage", parameters);



                return View(CustomerListIndexing);

                //return Json(new { bookingDeliveryChallan, ex = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }



            //ViewBag.DistId = new SelectList(db.Cat_District.Where(c => c.DistId > 0), "DistId", "DistName");
            //ViewBag.PStationId = new SelectList(db.Cat_PoliceStation.Where(c => c.PStationId > 0), "PStationId", "PStationName");
            //var customers = db.Customers.Where(c => c.comid == HttpContext.Session.GetString("comid")).ToList(); //.Include(c => c.vCustomerCountry)
            //var list = new List<CustomerResult>();
            //var sl = 0;
            //foreach (var c in customers)
            //{
            //    var ci = new CustomerResult
            //    {
            //        SLNo = sl++,
            //        DistName = c.vCat_District.DistName,
            //        PStationName = c.vCat_PoliceStation.PStationName
            //    };
            //    list.Add(ci);                
            //}

            //return View(list);
            ////return View();

        }





        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));
                //var abc = db.Customers.Include(y => y.vPrimaryCategory);
                var query = from e in db.Customers.Where(x => x.CustomerId > 0 && x.comid == comid).OrderByDescending(x => x.CustomerId)
                                //let FullName = e.CustomerName + " " + e.CustomerCode
                            select new CustomerResult
                            {
                                CustomerId = e.CustomerId,
                                CustomerCode = e.CustomerCode,
                                SLNo = e.SLNo,


                                CustomerName = e.CustomerName,
                                EmailId = e.EmailId,


                                Country = e.vCustomerCountry.CountryName,
                                PrimaryAddress = e.PrimaryAddress,
                                SecoundaryAddress = e.SecoundaryAddress,

                                PostalCode = e.PostalCode,
                                City = e.City,
                                PhoneNo = e.PhoneNo,

                                OpBalance = e.OpBalance,
                                IsInActive = e.IsInActive,
                                IsDealer = e.IsDealer,



                                DistName = e.vCat_District.DistName,
                                DistNameB = e.vCat_District.DistNameB,

                                PStationName = e.vCat_PoliceStation.PStationName,
                                PStationNameB = e.vCat_PoliceStation.PStationNameB



                            };



                var parser = new Parser<CustomerResult>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost, ActionName("SetSessionCustomerReport")]
        ////[Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public JsonResult SetSessionCustomerReport(string rptFormat, string action, string DistId, string PStationId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintCustomerList")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptCustomerList";
                    filename = "Dealer_List_" + DateTime.Now.Date.ToString();
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptCustomerList '" + comid + "','" + DistId + "' ,'" + PStationId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");

                }
                else if (action == "PrintCustomerListIndex")
                {
                    reportname = "rptCustomerListGroup_OnlyPageIndex";
                    filename = "Dealer_List_" + DateTime.Now.Date.ToString();
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptCustomerList_IndexPage '" + comid + "','" + DistId + "' ,'" + PStationId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");

                }
                else if (action == "PrintCustomerListGroup")
                {
                    reportname = "rptCustomerListGroup_PageIndex";
                    filename = "Dealer_List_" + DateTime.Now.Date.ToString();
                    HttpContext.Session.SetString("reportquery", "Exec Sales_rptCustomerList_IndexPage '" + comid + "','" + DistId + "' ,'" + PStationId + "' ");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");

                    //reportname = "rptCustomerListGroup";
                    //filename = "Dealer_List_GroupBy" + DateTime.Now.Date.ToString();
                    //HttpContext.Session.SetString("reportquery", "Exec Sales_rptCustomerList '" + comid + "','" + DistId + "' ,'" + PStationId + "' ");
                    //HttpContext.Session.SetString("ReportPath", "~/ReportViewer/SalesReport/" + reportname + ".rdlc");

                    //
                    //var vals = reportid.Split(',')[0];
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }

                }


                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                var ConstrName = "ApplicationServices";
                string callBackUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });
                redirectUrl = callBackUrl;



                return Json(new { Url = redirectUrl });

            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }




        public JsonResult getPoliceStation(int id)
        {
            List<Cat_PoliceStation> PStation = db.Cat_PoliceStation.Where(x => x.DistId == id).ToList();

            List<SelectListItem> lipstation = new List<SelectListItem>();

            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (PStation != null)
            {
                foreach (Cat_PoliceStation x in PStation)
                {
                    lipstation.Add(new SelectListItem { Text = x.PStationName, Value = x.PStationId.ToString() });
                }
            }
            return Json(new SelectList(lipstation, "Value", "Text"));
        }


        // GET: Customers/Create
        public ActionResult Create()
        {

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.DistId = new SelectList(db.Cat_District.Where(c => c.DistId > 0), "DistId", "DistName");
            ViewBag.PStationId = new SelectList(db.Cat_PoliceStation.Where(c => c.PStationId > 0), "PStationId", "PStationName");


            ViewBag.Title = "Create";
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
        //[Bind(include= "CustomerId,CustomerCode,CustomerName,EmailId,CountryId,PrimaryAddress,SecoundaryAddress,PostalCode,City,PhoneNo,IsInActive,OpBalance,comid,userid,useridUpdate,DateAdded,DateUpdated")]
        Customer customer)
        {

            try
            {

                var comid = (HttpContext.Session.GetString("comid"));

                var errors = ModelState.Where(x => x.Value.Errors.Any())
               .Select(x => new { x.Key, x.Value.Errors });
                customer.comid = (HttpContext.Session.GetString("comid"));

                if (customer.CustomerId > 0)
                {
                    ViewBag.Title = "Edit";


                    if (customer.CustomerCode != customer.OldCustomerCode)
                    {
                        var countdeliveryorder = db.Customers.Where(k => k.CustomerCode == customer.CustomerCode && k.comid == comid).Count();

                        if (countdeliveryorder >= 1)
                        {
                            ModelState.AddModelError("DuplicateError", "Customer Code already exist.");
                        }
                    }


                    customer.DateUpdated = DateTime.Now;
                    customer.comid = comid;

                    if (customer.userid == null)
                    {
                        customer.userid = HttpContext.Session.GetString("userid");
                    }
                    customer.useridUpdate = HttpContext.Session.GetString("userid");

                    //customer.comid = int.ParseHttpContext.Session.GetString("comid");
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");


                }
                else
                {
                    ViewBag.Title = "Create";

                    if (db.Customers.Any(k => k.CustomerCode == customer.CustomerCode && k.comid == comid))
                        ModelState.AddModelError("DuplicateError", "Customer Code already exists");

                    customer.userid = HttpContext.Session.GetString("userid");
                    customer.comid = (HttpContext.Session.GetString("comid"));

                    customer.DateAdded = DateTime.Now;
                    //customer.comid = int.ParseHttpContext.Session.GetString("comid");
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");


                }
            }
            catch (Exception ex)
            {
                ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", customer.CountryId);
                ViewBag.DistId = new SelectList(db.Cat_District.Where(c => c.DistId > 0), "DistId", "DistName", customer.DistId);
                ViewBag.PStationId = new SelectList(db.Cat_PoliceStation.Where(c => c.PStationId > 0), "PStationId", "PStationName", customer.PStationId);
                return View(customer);

                throw ex;
            }


        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer customer = db.Customers.Where(c => c.comid == HttpContext.Session.GetString("comid") && c.CustomerId == id).FirstOrDefault();
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", customer.CountryId);
            ViewBag.DistId = new SelectList(db.Cat_District.Where(c => c.DistId > 0), "DistId", "DistName", customer.DistId);
            ViewBag.PStationId = new SelectList(db.Cat_PoliceStation.Where(c => c.PStationId > 0), "PStationId", "PStationName", customer.PStationId);
            customer.OldCustomerCode = customer.CustomerCode;


            return View("Create", customer);
        }



        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer customer = db.Customers.Where(c => c.comid == HttpContext.Session.GetString("comid") && c.CustomerId == id).FirstOrDefault();
            if (customer == null)
            {
                return NotFound();
            }
            customer.OldCustomerCode = customer.CustomerCode;

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", customer.CountryId);
            ViewBag.DistId = new SelectList(db.Cat_District.Where(c => c.DistId > 0), "DistId", "DistName", customer.DistId);
            ViewBag.PStationId = new SelectList(db.Cat_PoliceStation.Where(c => c.PStationId > 0), "PStationId", "PStationName", customer.PStationId);
            ViewBag.Title = "Delete";
            return View("Create", customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {

                Customer customer = db.Customers.Find(id);
                db.Customers.Remove(customer);
                db.SaveChanges();
                return Json(new { Success = 1, CustomerId = customer.CustomerId, ex = "" });

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



    public class CustomerResult
    {

        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public int? SLNo { get; set; }

        public string CustomerName { get; set; }

        public string EmailId { get; set; }
        public string Country { get; set; }


        public string PrimaryAddress { get; set; }
        public string SecoundaryAddress { get; set; }

        public string PostalCode { get; set; }
        public string City { get; set; }

        public string PhoneNo { get; set; }
        public decimal OpBalance { get; set; }


        public bool IsInActive { get; set; }
        public bool IsDealer { get; set; }


        public string DistName { get; set; }
        public string DistNameB { get; set; }

        public string PStationName { get; set; }
        public string PStationNameB { get; set; }


    }

}
