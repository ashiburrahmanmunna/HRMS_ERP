using GTERP.BLL;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc;

namespace GTERP.Controllers.HouseKeeping
{
    //[OverridableAuthorize]

    public class SurveyController : Controller
    {
        private TransactionLogRepository tranlog;
        private readonly GTRDBContext db;


        public SurveyController(GTRDBContext context, TransactionLogRepository tran)
        {
            db = context;
            tranlog = tran;

        }

        // GET: BankBranch
        public IActionResult Index()
        {

            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");


            return View();
        }

        [HttpPost]
        public IActionResult Create(string SurveyResult)
        {

            return Json(SurveyResult);
        }


    }
}