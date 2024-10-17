using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ErrorHandlerController : Controller
    {
        [Authorize]
        // GET: ErrorHandler
        public ActionResult Index()
        {
            string url = HttpContext.Request.Url.AbsoluteUri;
            //Console.WriteLine("test");
            //return RedirectToAction("Home/Index");
            return View("Index");
        }
    }
}