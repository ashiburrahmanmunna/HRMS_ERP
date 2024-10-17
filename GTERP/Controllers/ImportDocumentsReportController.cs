using GTCommercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ImportDocumentsReportController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        // GET: ImportDocumentsReport
        public ActionResult Index()
        {
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.BBLCId = new SelectList(db.COM_BBLC_Master, "BBLCId", "BBLCNo");
            ViewBag.SupplierId = new SelectList(db.SupplierInformations, "ContactId", "SupplierName");
            ViewBag.PIId = new SelectList(db.COM_ProformaInvoices, "PIId", "PINo");
            ViewBag.MasterLCId = new SelectList(db.COM_MasterLCs, "MasterLCId", "LCRefNo");
            return View();
        }

    }
}
