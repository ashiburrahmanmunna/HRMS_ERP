using GTCommercial.Models;
using GTCommercial.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class CustomerSerialController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //public static int PackageId = 0;



        public static string token_id;
        public static string refresh_token;
        public static string paymentID;
        public static string agreementID;

        public static string ResponseText = "";
        public static string username = "sandboxTestUser";
        public static string password = "hWD@8vtzw0";
        public static string app_secret = "1vggbqd4hqk9g96o9rrrp2jftvek578v7d2bnerim12a87dbrrka";
        public static string app_key = "5tunt4masn6pv2hnvte1sb5n3j";
        public static string currency = "BDT";
        public static float amount = 99;
        public static string intent = "sale";
        public static string merchantInvoiceNumber = "126iXJi3omiO";
        public static string trxID = "";
        public static DateTime createTime = DateTime.Now.Date;
        public static DateTime updateTime = DateTime.Now.Date;
        public static string userid = ""; 
        public static string userphone = ""; 


        //Function to get random number
        private static readonly Random getrandom = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }




        [Authorize]
        // GET: CustomerSerials
        public ActionResult Index()
        {
            DateTime dttoday = Convert.ToDateTime(DateTime.Now.Date);
            //return View(db.CustomerSerials.Where(c => c.CustomerSerialId > 0).ToList());
            //return View(db.CustomerSerials.Where(c => c.EntryDate >= dttoday).ToList());

            //var listCLientResult = (db.CustomerSerials.Where(c => c.EntryDate >= dttoday && c.userid == HttpContext.Session.GetString("userid")));
            //return View(listCLientResult);

            var userid = HttpContext.Session.GetString("userid");

            return View(db.CustomerSerials.Where(c => c.EntryDate >= dttoday && c.userid == userid).ToList());

            //Where(x=>x.id == x && x.name == name)
            //   c => c.userid == HttpContext.Session.GetString("userid")).ToList());

            //Where(c => c.userid == System.Web.HttpContext.Current.HttpContext.Session.GetString("userid")).

        }

        public class ENT_TrackingData
        {

            public string IPAddress { get; set; }
            public string Browser { get; set; }
            public DateTime DateStamp { get; set; }
            public string PageViewed { get; set; }
            public string NodeId { get; set; }
            public Boolean IsMobileDevice { get; set; }
            public string Platform { get; set; }


        }

        [Authorize]
        // GET: CustomerSerials
        public ActionResult MonthlyList(int yearid, int monthid, int stateid)
        {
            try
            {
                if (Session["activepackage"].ToString() == "0")
                {

                    return BadRequest();



       
                }
                if (yearid == null)
                {
                    return NotFound();

                }




                //HttpBrowserCapabilities bc = ControllerContext.HttpContext.Request.Browser;


                /////////////////  for saving user transaction //////////////////////
                var request = ControllerContext.HttpContext.Request;

                ////need for Mr. Noman Later More R&D need
                //string UserAgent = request.UserAgent;


                //ENT_TrackingData ret = new ENT_TrackingData()
                //{
                //    IPAddress = request.UserHostAddress,
                //    Browser = request.Browser + " " + request.Browser.Version,
                //    DateStamp = DateTime.Now,
                //    PageViewed = request.Url.AbsolutePath,
                //    //NodeId = UmbracoHelper.GetCurrentNodeID(),
                //    IsMobileDevice = request.Browser.IsMobileDevice ,
                //    Platform = request.Browser.Platform
                //};

                var devicetype = "";
                if (request.Browser.IsMobileDevice)
                {
                    devicetype = "Mobile";
                    //mobile
                }
                else
                {
                    devicetype = "Computer";

                    //laptop or desktop
                }


                //UserTransactionLog usertran = new UserTransactionLog();
                //usertran.IPAddress = GTRGetIPAddress();
                //usertran.MacAddress = GTRGetMacAddress();
                //usertran.ComId =0;
                //usertran.userid = HttpContext.Session.GetString("userid");
                //usertran.DeviceType = devicetype;
                //usertran.FlagValue = "1";
                //usertran.WebLink = HttpContext.Request.Url.AbsoluteUri;

                //usertran.FromDateTime = DateTime.Now;
                //usertran.ToDateTime = DateTime.Now;

                //usertran.PcName = GetWorkstation();

                //usertran.TransactionStatement = "select monthlylist";


                //db.UserTransactionLogs.Add(usertran);
                //db.SaveChanges();

                /////////////////  for saving user transaction //////////////////////

                if (stateid != 0)
                {
                    var xx =  db.YearNames.Where(x => x.YearNameId.ToString() == yearid.ToString()).FirstOrDefault();
                    var xxx = xx.YearEng;

                    var yy = db.MonthNames.Where(x => x.MonthNameId.ToString() == monthid.ToString()).FirstOrDefault();
                    var yyy = yy.Name;



                    int intyear = DateTime.ParseExact(xxx, "yyyy", CultureInfo.CurrentCulture).Year;
                    int intmonth = DateTime.ParseExact(yyy, "MMMM", CultureInfo.CurrentCulture).Month;



                    DateTime reportmonth = new DateTime(intyear, intmonth, 1).AddMonths(stateid);

                    ///var reportmonth = DateTime.Today.AddMonths(-1);
                    ///


                    var year = reportmonth.ToString("yyyy");
                    var yearname = db.YearNames.Where(x => x.YearEng.ToString() == year.ToString()).FirstOrDefault();
                    if (yearname == null)
                    {
                        yearname = db.YearNames.Where(x => x.YearEng.ToString() == intyear.ToString()).FirstOrDefault();
                    }
                    yearid = yearname.YearNameId;

                    var month = reportmonth.ToString("MMMM");
                    var monthname = db.MonthNames.Where(x => x.Name.ToString() == month.ToString()).FirstOrDefault();
                    monthid = monthname.MonthNameId;

                }



                //db.UserTransactionLogs

                if (yearid == 0)
                {
                    var year = DateTime.Now.Year;
                    var yearname = db.YearNames.Where(x => x.YearEng.ToString() == year.ToString()).FirstOrDefault();
                    yearid = yearname.YearNameId;
                }


                if (monthid == 0)
                {
                    var month = DateTime.Now.ToString("MMMM");
                    var monthname = db.MonthNames.Where(x => x.Name.ToString() == month.ToString()).FirstOrDefault();
                    monthid = monthname.MonthNameId;
                }

                ///var result = db.MonthNames.ToList();

                ViewBag.Month = db.MonthNames;// new SelectList(db.MonthNames, "MonthNameId", "BanglaName").ToList();
                ViewBag.Year = db.YearNames;// new SelectList(db.YearNames, "YearNameId", "YearBng").ToList();



            List<ViewModel.ProductRegistrationMonthlyList> productregistrationlist = (db.Database.SqlQuery<ViewModel.ProductRegistrationMonthlyList>("[prcGetMonthlyProductregistraionList]  @yearid , @monthid, @userid , @userphoneno", new SqlParameter("yearid", yearid),new SqlParameter("monthid", monthid), new SqlParameter("userid", Session["userid"]) , new SqlParameter("userphoneno", Session["userphoneno"]))).ToList();

                ViewModel.ProductRegistrationMonthly productregistraion = new ViewModel.ProductRegistrationMonthly {
                    ProductRegistrationMonthlyId = 1,
                    YearNameId = yearid,
                    MonthNameId = monthid,
                    ProductRegistrationMonthlysub = productregistrationlist

                    //new ViewModel.ProductRegistrationMonthlyList {
                    //    ProductRegistrationMonthlyId = 1,
                    //    ProductName = "abc",
                    //    TotalQty = 0}

                };

                //foreach (var material in productregistrationlist)
                //{
                //    var productregsub = new ViewModel.ProductRegistrationMonthlyList();
                //    productregsub.ProductRegistrationMonthlyId = 1;
                //    productregsub.ProductName = material.ProductName;
                //    productregsub.TotalQty = material.TotalQty;

                //    productregistraion.ProductRegistrationMonthlysub.Add(productregsub);
                //}

                //foreach (var item in ProductSerialresult)
                //{
                //    productregistraion.ProductRegistrationMonthlysub.Add(new ViewModel.ProductRegistrationMonthlyList
                //    {
                //        ProductRegistrationMonthlyId = 1,
                //        ProductName = item.ProductName,
                //        TotalQty = item.TotalQty
                //    });
                //}


                //productregistraion.ProductRegistrationMonthlysub = new List<ViewModel.ProductRegistrationMonthlyList>{

                //    ProductSerialresult;

                //        //new ViewModel.ProductRegistrationMonthlyList {ProductName = "Fahad", TotalQty = 50} 
                //        //, new InvoiceSub {Id = 2, Name = "Gavin", ParentId = 12}
                //    };



                return View(productregistraion);
                //return View(db.CustomerSerials.Where(c => c.EntryDate >= dttoday && c.userid == userid).ToList());


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GTRGetIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return "";
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host
                .AddressList
                .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork
                ).ToString();
        }
        public string GTRGetMacAddress()
        {
            foreach (NetworkInterface NIC in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (NIC.NetworkInterfaceType == NetworkInterfaceType.Ethernet && NIC.OperationalStatus == OperationalStatus.Up)
                {
                    return NIC.GetPhysicalAddress().ToString();
                }

                if (NIC.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && NIC.OperationalStatus == OperationalStatus.Up)
                {
                    return NIC.GetPhysicalAddress().ToString();
                }
            }
            return "";
        }
        private string GetWorkstation()
        {
            string ip = Request.UserHostName;
            IPAddress myIP = IPAddress.Parse(ip);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First().ToUpper();
        }

        [Authorize]
        // GET: CustomerSerials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CustomerSerial CustomerSerials = db.CustomerSerials.Find(id);
            if (CustomerSerials == null)
            {
                return NotFound();
            }
            return View(CustomerSerials);
        }
        [Authorize]
        // GET: CustomerSerials/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create";


            //this.ViewBag.PackageSearch = db.SoftwarePackages.Where(c => c.SoftwarePackageId > 0).ToList();
            //PostRequest();

            userid =  HttpContext.Session.GetString("userid");
            userphone = Session["userphoneno"].ToString();

            return View();
        }
        [Authorize]
        // GET: CustomerSerials/Create
        public ActionResult Package(string packageid)
        {
            ViewBag.Title = "Package";


            //this.ViewBag.PackageSearch = db.SoftwarePackages.Where(c => c.SoftwarePackageId > 0).ToList();
            //PostRequest();

            userid = HttpContext.Session.GetString("userid");
            userphone = Session["userphoneno"].ToString();

            return View(db.SoftwarePackages.Where(c => c.SoftwarePackageId.ToString() == packageid.ToString()).FirstOrDefault());
        }
        [Authorize]
        // GET: CustomerSerials/Create
        public ActionResult Success(string customerpaymentno)
        {

            CustomerPayment CustomerSerialssingle = db.CustomerPayments.Where(c => c.CustomerPaymentNo.ToString() == merchantInvoiceNumber.ToString()).FirstOrDefault();


            //if (customerpaymentno == null)
            //{
            //    return BadRequest();
            //}
            CustomerPayment customerpayment = db.CustomerPayments.Find(CustomerSerialssingle.CustomerPaymentId);
            if (customerpayment == null)
            {
                return NotFound();
            }


            ViewBag.Title = "Payment Success";

            return View(customerpayment);
        }
        [Authorize]
        // GET: CustomerSerials
        public ActionResult Payment()
        {
            

            return View();

        }



        public async Task<JsonResult> createpayment(string invno , decimal invamount)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/token/grant"))
                    {
                        request.Headers.TryAddWithoutValidation("password", password);
                        request.Headers.TryAddWithoutValidation("username", username);

                        request.Content = new StringContent("{\"app_secret\":\"" + app_secret + "\",\"app_key\":\"" + app_key + "\"}", Encoding.UTF8, "application/json");


                        //HttpResponseMessage response =await httpClient.SendAsync(request);
                        //ResponseText = response.AsyncState.ToString();

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            ResponseText = await response.Content.ReadAsStringAsync();


                        }

                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
                        token_id = routes_list.id_token;
                        refresh_token = routes_list.refresh_token;

                        //return Json(new { Success = 1, ResponseText, ex = "" });
                        //string idToken = response.Content.ReadAsStreamAsync["id_token"].Trim();
                    }

                    //return Json(token_id, JsonRequestBehavior.AllowGet);

                    //return Json(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(ResponseText));


                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/create"))
                    {
                        merchantInvoiceNumber = invno;// "INV#" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        amount = float.Parse(invamount.ToString());// GetRandomNumber(10,50);
                        currency = "BDT";

                        request.Headers.TryAddWithoutValidation("Authorization", token_id);
                        request.Headers.TryAddWithoutValidation("x-app-key", app_key);

                        request.Content = new StringContent("{\"currency\":\"" + currency + "\",\"amount\":\"" + amount + "\",\"intent\":\"" + intent + "\",\"merchantInvoiceNumber\":\"" + merchantInvoiceNumber + "\"}", Encoding.UTF8, "application/json");

                        //var response = await httpClient.SendAsync(request);


                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            ResponseText = await response.Content.ReadAsStringAsync();


                        }

                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
                        //token_id = routes_list.id_token;
                        //refresh_token = routes_list.refresh_token;
                        paymentID = routes_list.paymentID;

                        //var response = await httpClient.SendAsync(request);
                    }


                    return Json(ResponseText, JsonRequestBehavior.AllowGet);

                }





            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

            //return Json(new { Success = 0, ex = new Exception("Unable to Execute").Message.ToString() });
        }

        public async Task<JsonResult> executepayment()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/execute/"+paymentID+""))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", token_id);
                        request.Headers.TryAddWithoutValidation("x-app-key", app_key);

                        //HttpResponseMessage response =await httpClient.SendAsync(request);
                        //ResponseText = response.AsyncState.ToString();

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            ResponseText = await response.Content.ReadAsStringAsync();


                        }

                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
                        //token_id = routes_list.id_token;
                        //refresh_token = routes_list.refresh_token;
                        trxID = routes_list.trxID;
                        createTime = DateTime.Now;
                        updateTime = DateTime.Now;
                        amount = routes_list.amount;
                        currency = routes_list.currency;


                        int duration = 30;
                        DateTime startDate = DateTime.Parse(DateTime.Now.Date.ToString("dd-MMM-yy"));
                        DateTime expiryDate = startDate.AddDays(duration);


                        CustomerPayment cstpayment = new CustomerPayment();
                        cstpayment.CustomerPaymentNo = merchantInvoiceNumber;
                        cstpayment.PaymentDate = createTime;
                        cstpayment.Amount = amount;
                        cstpayment.Currency = currency;
                        cstpayment.TrxID = trxID;
                        cstpayment.PaymentId = trxID;
                        cstpayment.Status = "Success";
                        cstpayment.transactionStatus = "Success";
                        cstpayment.merchantInvoiceNumber = merchantInvoiceNumber;
                        cstpayment.ActiveFromDate = startDate;
                        cstpayment.ActiveToDate = expiryDate;
                        cstpayment.ActiveYesNo = true;



                        //cstpayment.userid = HttpContext.Session.GetString("userid");
                        //cstpayment.UserPhoneNo = Session["userphoneno"].ToString();


                        cstpayment.userid = HttpContext.Session.GetString("userid");
                        cstpayment.UserPhoneNo = Session["userphoneno"].ToString();








                        //cstpayment.PaymentDate =
                        //if (trxID != null)
                        {
                            db.CustomerPayments.Add(cstpayment);
                            db.SaveChanges();
                            Session["activepackage"] = 1;

                        }

                    }

                    return Json(ResponseText, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }



        public async Task<JsonResult> transactiondetails()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/search/"+ trxID +""))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", token_id);
                        request.Headers.TryAddWithoutValidation("x-app-key", app_key);

                        //HttpResponseMessage response =await httpClient.SendAsync(request);
                        //ResponseText = response.AsyncState.ToString();

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            ResponseText = await response.Content.ReadAsStringAsync();


                        }

                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
                        //token_id = routes_list.id_token;
                        //refresh_token = routes_list.refresh_token;
                        trxID = routes_list.trxID;
                        createTime = DateTime.Now.Date;
                        updateTime = DateTime.Now.Date;
                        amount = routes_list.amount;
                        currency = routes_list.currency;





                        //CustomerPayment cstpayment = new CustomerPayment();
                        //cstpayment.CustomerPaymentNo = merchantInvoiceNumber;
                        //cstpayment.PaymentDate = createTime;
                        //cstpayment.Amount = amount;
                        //cstpayment.Currency = currency;
                        //cstpayment.TrxID = trxID;
                        //cstpayment.PaymentId = trxID;
                        //cstpayment.Status = "Success";
                        //cstpayment.transactionStatus = "Success";
                        //cstpayment.merchantInvoiceNumber = merchantInvoiceNumber;


                        ////cstpayment.userid = HttpContext.Session.GetString("userid");
                        ////cstpayment.UserPhoneNo = Session["userphoneno"].ToString();


                        //cstpayment.userid = HttpContext.Session.GetString("userid");
                        //cstpayment.UserPhoneNo = Session["userphoneno"].ToString();








                        ////cstpayment.PaymentDate =
                        //if (trxID != null)
                        //{
                        //    db.CustomerPayments.Add(cstpayment);
                        //    db.SaveChanges();

                        //}

                    }

                    return Json(ResponseText, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }




        //public JsonResult createpayment()
        //{
        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://direct.sandbox.bka.sh/v1.2.0-beta/checkout/payment/capture/paymentid"))
        //            {
        //                request.Headers.TryAddWithoutValidation("Authorization", token_id);
        //                request.Headers.TryAddWithoutValidation("x-app-key", app_key);

        //                // var response = await httpClient.SendAsync(request);


        //                var response = httpClient.SendAsync(request);

        //                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
        //                MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
        //                //token_id = routes_list.id_token;
        //                //refresh_token = routes_list.refresh_token;
        //                paymentID = routes_list.paymentID;



        //                return Json(new { Success = 1, token = token_id, ex = "" });
        //                //string idToken = response.Content.ReadAsStreamAsync["id_token"].Trim();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Success = 0, ex = ex.Message.ToString() });

        //    }

        //    return Json(new { Success = 0, ex = new Exception("Unable to Execute Create Payment").Message.ToString() });
        //}


        //public JsonResult executepayment()
        //{
        //    try
        //    {
                
        //    using (var httpClient = new HttpClient())
        //        {
        //            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/execute/" + paymentID + ""))
        //            {
        //                request.Headers.TryAddWithoutValidation("Authorization", token_id);
        //                request.Headers.TryAddWithoutValidation("x-app-key", app_key);

        //                //var response = await httpClient.SendAsync(request);


        //                var  response = httpClient.SendAsync(request);
                        

        //                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
        //                MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);


        //                return Json(new { Success = 1, token = token_id, ex = "" });
        //                //string idToken = response.Content.ReadAsStreamAsync["id_token"].Trim();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Success = 0, ex = ex.Message.ToString() });

        //    }

        //    return Json(new { Success = 0, ex = new Exception("Unable to Execute Create Payment").Message.ToString() });
        //}




        struct MyObj
        {
            public string token_type { get; set; }
            public string id_token { get; set; }
            public string refresh_token { get; set; }
            public string paymentID { get; set; }
            public string agreementID { get; set; }
            public string trxID { get; set; }
            //public DateTime createTime { get; set; }
            //public DateTime updateTime { get; set; }
            public float amount { get; set; }
            public string currency { get; set; }

        }

        async static void PostRequest()
        {
            /////grant token
            //using (var httpClient = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/token/grant"))
            //    {
            //        request.Headers.TryAddWithoutValidation("password", password);
            //        request.Headers.TryAddWithoutValidation("username", username);

            //        request.Content = new StringContent("{\"app_secret\":\""+ app_secret + "\",\"app_key\":\""+ app_key +"\"}", Encoding.UTF8, "application/json");

            //        HttpResponseMessage response = await httpClient.SendAsync(request);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            ResponseText = await response.Content.ReadAsStringAsync();


            //        }

            //        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
            //        token_id = routes_list.id_token;
            //        refresh_token = routes_list.refresh_token;

            //        //string idToken = response.Content.ReadAsStreamAsync["id_token"].Trim();
            //    }






            }



            ////refresh token
            //using (var httpClient = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/token/refresh"))
            //    {
            //        request.Headers.TryAddWithoutValidation("password", password);
            //        request.Headers.TryAddWithoutValidation("username", username);


            //        request.Content = new StringContent("{\"refresh_token\":\" "+ refresh_token + "\",\"app_secret\":\""+ app_secret +"\",\"app_key\":\""+ app_key +"\"}", Encoding.UTF8, "application/json");

            //        //var response = await httpClient.SendAsync(request);

            //        HttpResponseMessage response = await httpClient.SendAsync(request);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            ResponseText = await response.Content.ReadAsStringAsync();


            //        }

            //        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
            //        token_id = routes_list.id_token;
            //        refresh_token = routes_list.refresh_token;
            //    }
            //}



            //////x-app-key not found for aggrement id
            //using (var httpClient = new HttpClient())
            //{
            //        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/create"))
            //        {
            //            request.Headers.TryAddWithoutValidation("Authorization", token_id);
            //            request.Headers.TryAddWithoutValidation("x-app-key", app_key);

            //            request.Content = new StringContent("{\"currency\":\""+ currency +"\",\"amount\":\""+ amount +"\",\"intent\":\""+intent+"\",\"merchantInvoiceNumber\":\""+ merchantInvoiceNumber +"\"}", Encoding.UTF8, "application/json");

            //            //var response = await httpClient.SendAsync(request);
                    
                
            //        HttpResponseMessage response = await httpClient.SendAsync(request);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            ResponseText = await response.Content.ReadAsStringAsync();


            //        }

            //        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
            //        //token_id = routes_list.id_token;
            //        //refresh_token = routes_list.refresh_token;
            //        paymentID = routes_list.paymentID;

            //        //var response = await httpClient.SendAsync(request);
            //    }
            //}




            //using (var httpClient = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://direct.sandbox.bka.sh/v1.2.0-beta/checkout/payment/capture/paymentid"))
            //    {
            //        request.Headers.TryAddWithoutValidation("Authorization", token_id);
            //        request.Headers.TryAddWithoutValidation("x-app-key", app_key);

            //       // var response = await httpClient.SendAsync(request);


            //        HttpResponseMessage response = await httpClient.SendAsync(request);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            ResponseText = await response.Content.ReadAsStringAsync();


            //        }

            //        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
            //        //token_id = routes_list.id_token;
            //        //refresh_token = routes_list.refresh_token;
            //        paymentID = routes_list.paymentID;

            //    }
            //}


            //using (var httpClient = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/execute/"+paymentID+""))
            //    {
            //        request.Headers.TryAddWithoutValidation("Authorization", token_id);
            //        request.Headers.TryAddWithoutValidation("x-app-key", app_key);

            //        //var response = await httpClient.SendAsync(request);


            //        HttpResponseMessage response = await httpClient.SendAsync(request);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            ResponseText = await response.Content.ReadAsStringAsync();


            //        }

            //        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //        MyObj routes_list = json_serializer.Deserialize<MyObj>(ResponseText);
            //        //token_id = routes_list.id_token;
            //        //refresh_token = routes_list.refresh_token;
            //        //agreementID = routes_list.agreementID;


            //    }
            //}


        //}

        //async static void PostRequestjson()
        //{

        //    ScriptManager.RegisterClientScriptBlock(Button1, Button1.GetType(), "Hello", "alert('Hello World');", true

        //    var data = JSON.stringify({"app_secret": "1vggbqd4hqk9g96o9rrrp2jftvek578v7d2bnerim12a87dbrrka","app_key": "5tunt4masn6pv2hnvte1sb5n3j"});

        //    var xhr = new XMLHttpRequest();
        //    xhr.withCredentials = true;

        //    xhr.addEventListener("readystatechange", function()
        //    {
        //        if (this.readyState === this.DONE)
        //        {
        //            console.log(this.responseText);
        //        }
        //    });

        //    xhr.open("POST", "https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/token/grant");
        //    xhr.setRequestHeader("username", "sandboxTestUser");
        //    xhr.setRequestHeader("password", "hWD@8vtzw0");

        //    xhr.send(data);
        //    );
        //}
        //private static void Main(string[] args)
        //{
        //    RunPostAsync();
        //}

        //static HttpClient client = new HttpClient();

        //private static void RunPostAsync()
        //{

        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));

        //    Inputs inputs = new Inputs();

        //    inputs.username = "sandboxTestUser";
        //    inputs.password = "hWD@8vtzw0";
        //    inputs.app_key = "5tunt4masn6pv2hnvte1sb5n3j";
        //    inputs.app_secret = "1vggbqd4hqk9g96o9rrrp2jftvek578v7d2bnerim12a87dbrrka";

        //    var res = client.PostAsync("https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/token/grant", new StringContent(JsonConvert.SerializeObject(inputs)));

        //    try
        //    {
        //        res.Result.EnsureSuccessStatusCode();

        //        Console.WriteLine("Response " + res.Result.Content.ReadAsStringAsync().Result + Environment.NewLine);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error " + res + " Error " +
        //        ex.ToString());
        //    }

        //    Console.WriteLine("Response: {0}", res);
        //}

        //public class Inputs
        //{
        //    public string username;
        //    public string password;
        //    public string app_key;
        //    public string app_secret;
        //}

        // POST: CustomerSerials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "CustomerSerialId,CustomerSerialNo , CustomerSerialName,EntryDate ,CustomerSerialAddress , CustomerSerialRemarks , CustomerSerialMobile ,UserId  , userphoneno")] CustomerSerial CustomerSerials)
        {


            CustomerSerials.userid = System.Web.HttpContext.Current.HttpContext.Session.GetString("userid");
            CustomerSerials.UserPhoneNo = System.Web.HttpContext.Current.Session["userphoneno"].ToString();



            var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

            if (CustomerSerials.CustomerSerialId == 0)
            {
                ViewBag.Title = "Create";
            }
            else
            {
                ViewBag.Title = "Edit";

            }

            //var z = clsMain.CheckIsPhoneNumber(CustomerSerials.CustomerSerialMobile);
            //if (z == "NAN")
            //{
            //    this.ModelState.AddModelError(CustomerSerials.CustomerSerialMobile, "Phone Number / Mobile No is not Valid");

            //    ViewBag.Error = "Phone Number / Mobile No is not Valid";
            //}

            if (ModelState.IsValid)
            {
                if (CustomerSerials.CustomerSerialId > 0)
                {
                    CustomerSerials.EntryDate = DateTime.Now;
                    db.Entry(CustomerSerials).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    CustomerSerials.EntryDate = DateTime.Now;
                    CustomerSerials.userid = System.Web.HttpContext.Current.HttpContext.Session.GetString("userid");
                    CustomerSerials.UserPhoneNo = System.Web.HttpContext.Current.Session["userphoneno"].ToString();

                    db.CustomerSerials.Add(CustomerSerials);
                    db.SaveChanges();

                    //db.CustomerSerials.Add(CustomerSerials);
                    return RedirectToAction("Index");
                }
            }
            //return RedirectToAction("Index");

            return View(CustomerSerials);
        }

        [Authorize]
        // GET: CustomerSerials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CustomerSerial CustomerSerials = db.CustomerSerials.Find(id);
            if (CustomerSerials == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            return View("Create", CustomerSerials);

        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: CustomerSerials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CustomerSerial CustomerSerials = db.CustomerSerials.Find(id);
            if (CustomerSerials == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            return View("Create", CustomerSerials);
        }
        //        [Authorize]
        // POST: CustomerSerials/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
  //      [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                CustomerSerial CustomerSerials = db.CustomerSerials.Find(id);
                db.CustomerSerials.Remove(CustomerSerials);
                db.SaveChanges();

             
                return Json(new { Success = 1, CustomerSerialsId = CustomerSerials.CustomerSerialId, ex = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

            return Json(new { Success = 0, ex = new Exception("Unable to Delete").Message.ToString() });
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
