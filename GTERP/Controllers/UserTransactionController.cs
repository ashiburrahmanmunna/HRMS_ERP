using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using GTCommercial.Models;
using PagedList.Mvc;
using PagedList;
using Newtonsoft.Json;
using GTCommercial.Helper;
using System.Linq.Dynamic;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class UserTransactionsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();


        [Authorize]

        public ActionResult TransactionLogSave()
        {
            //Server Side Parameter

            var request = ControllerContext.HttpContext.Request;



            //need for Mr. Noman Later More R&D need
            string UserAgent = request.UserAgent;


            ENT_TrackingData ret = new ENT_TrackingData()
            {
                IPAddress = request.UserHostAddress,
                Browser = request.Browser + " " + request.Browser.Version,
                DateStamp = DateTime.Now,
                PageViewed = request.Url.AbsolutePath,
                //NodeId = UmbracoHelper.GetCurrentNodeID(),
                IsMobileDevice = request.Browser.IsMobileDevice,
                Platform = request.Browser.Platform
            };

            return View();

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
