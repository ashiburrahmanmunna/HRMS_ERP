using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Controllers.POS
{
    [OverridableAuthorize]
    public class StoreRequisitionController : Controller
    {
        private readonly GTRDBContext db;
        private TransactionLogRepository tranlog;
        private readonly IConfiguration _configuration;

        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }
        POSRepository POS;
        PermissionLevel PL;


        public StoreRequisitionController(GTRDBContext context, POSRepository _POS, TransactionLogRepository tran, PermissionLevel _pl, IConfiguration configuration)
        {
            db = context;
            tranlog = tran;
            //Repository = rep;
            POS = _POS;
            PL = _pl;
            _configuration = configuration;
        }

        // GET: StoreRequisition
        public async Task<IActionResult> Index()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");

            UserPermission permission = HttpContext.Session.GetObject<UserPermission>("userpermission");


            //var gTRDBContext = db.StoreRequisitionMain.Where(x => x.ComId == comid).Include(s => s.ApprovedBy).Include(s => s.Department).Include(s => s.PrdUnit).Include(s => s.Purpose).Include(s => s.RecommenedBy);

            ///////////get user list from the server //////

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();

            //List<string> moduleUser = PL.GetModuleUser().ToList();

            var userList = new List<AspnetUserList>();
            foreach (var item in list)
            {
                //if (moduleUser.Contains(c.UserID))
                //{
                var newUser = new AspnetUserList();
                newUser.Email = item.UserName;
                newUser.UserId = item.UserID;
                newUser.UserName = item.UserName;
                userList.Add(newUser);
                //}
            }


            ViewBag.Userlist = new SelectList(userList, "UserId", "UserName", userid);

            //return View(await gTRDBContext.ToListAsync());
            return View();

        }

        // GET: StoreRequisition/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeRequisitionMain = await db.StoreRequisitionMain
                .Include(s => s.ApprovedBy)
                .Include(s => s.Department)
                .Include(s => s.PrdUnit)
                .Include(s => s.Purpose)
                .Include(s => s.RecommenedBy)
                .FirstOrDefaultAsync(m => m.StoreReqId == id);
            if (storeRequisitionMain == null)
            {
                return NotFound();
            }

            return View(storeRequisitionMain);
        }

        // GET: StoreRequisition/Create
        public IActionResult Create()
        {

            string comid = (HttpContext.Session.GetString("comid"));
            string userid = (HttpContext.Session.GetString("userid"));

            var laststorereq = db.StoreRequisitionMain.Where(x => x.ComId == comid).OrderByDescending(D => D.StoreReqId).FirstOrDefault();
            var userwiseSRR = db.StoreRequisitionMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.StoreReqId).FirstOrDefault();


            if (userwiseSRR != null)
            {
                userwiseSRR.StoreReqId = 0;
                userwiseSRR.Status = 0;
                InitViewBag("Create");
                return View(userwiseSRR);
            }
            else
            {
                StoreRequisitionMain storereq = new StoreRequisitionMain();
                storereq.ReqDate = DateTime.Now.Date;
                storereq.INDate = DateTime.Now.Date;
                storereq.Remarks = "";
                storereq.BoardMeetingDate = DateTime.Now.Date;


                InitViewBag("Create");
                return View(storereq);

            }


        }

        public IActionResult SubStoreCreate()
        {
            StoreRequisitionMain storereq = new StoreRequisitionMain();
            storereq.ReqDate = DateTime.Now.Date;
            storereq.BoardMeetingDate = DateTime.Now.Date;
            storereq.IsSubStore = true;

            InitViewBag("Create");
            return View(storereq);
        }


        public partial class StoreReQuisitionResult
        {
            public int StoreReqId { get; set; }

            public string SRNo { get; set; }
            [Display(Name = "Reference")]
            public string Reference { get; set; }


            [Display(Name = "Required Date")]
            public DateTime? RequiredDate { get; set; }

            [Display(Name = "Requisition Date")]
            public string RequisitionDate { get; set; }

            [Display(Name = "Board Meeting Date")]
            public DateTime BoardMeetingDate { get; set; }


            public string Purpose { get; set; }


            public string Department { get; set; }


            public string ApprovedBy { get; set; }


            public string RecommenedBy { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }

            public string Remarks { get; set; }
            public virtual IssueMain Issue { get; set; }





        }
        public IActionResult Get(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

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

                Microsoft.Extensions.Primitives.StringValues y = "";

                var x = Request.Form.TryGetValue("search[value]", out y);

                UserPermission permission = HttpContext.Session.GetObject<UserPermission>("userpermission");


                if (permission.IsMedical)
                {
                    if (y.ToString().Length > 0)
                    {


                        //var query = from e in db.StoreRequisitionMain.Include(s => s.PrdUnit)
                        //            .Where(x => x.ComId == comid)
                        var query = from e in PL.GetSRRList()
                                     .OrderByDescending(x => x.StoreReqId)
                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                    select new StoreReQuisitionResult
                                    {
                                        StoreReqId = e.StoreReqId,
                                        SRNo = e.SRNo,
                                        RecommenedBy = e.RecommenedBy.EmpName,
                                        ApprovedBy = e.ApprovedBy.EmpName,
                                        RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                        RequiredDate = e.RequiredDate,
                                        BoardMeetingDate = e.BoardMeetingDate,
                                        Purpose = e.Purpose.PurposeName,
                                        Department = e.Department.DeptName,
                                        Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                        Remarks = e.Remarks,
                                        Reference = e.ReqRef,
                                        Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                    };


                        var parser = new Parser<StoreReQuisitionResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {


                        if (CustomerList != null && UserList != null)
                        {
                            var querytest = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                            //.Where(p => p.CustomerId == int.Parse(CustomerList))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList != null && UserList == null)
                        {
                            var querytest = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            // .Where(p => p.CustomerId == int.Parse(CustomerList))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList == null && UserList != null)
                        {

                            var querytest = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }
                        else
                        {

                            var querytest = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }

                    }
                }
                else if (permission.IsProduction)
                {
                    if (y.ToString().Length > 0)
                    {


                        var query = from e in db.StoreRequisitionMain.Include(s => s.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                                    .OrderByDescending(x => x.StoreReqId)
                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                    select new StoreReQuisitionResult
                                    {
                                        StoreReqId = e.StoreReqId,
                                        SRNo = e.SRNo,
                                        RecommenedBy = e.RecommenedBy.EmpName,
                                        ApprovedBy = e.ApprovedBy.EmpName,
                                        RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                        RequiredDate = e.RequiredDate,
                                        BoardMeetingDate = e.BoardMeetingDate,
                                        Purpose = e.Purpose.PurposeName,
                                        Department = e.Department.DeptName,
                                        Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                        Remarks = e.Remarks,
                                        Reference = e.ReqRef,
                                        Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                    };


                        var parser = new Parser<StoreReQuisitionResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {


                        if (CustomerList != null && UserList != null)
                        {
                            var querytest = from e in db.StoreRequisitionMain.Include(s => s.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                            //.Where(p => p.CustomerId == int.Parse(CustomerList))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList != null && UserList == null)
                        {
                            var querytest = from e in db.StoreRequisitionMain.Include(s => s.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            // .Where(p => p.CustomerId == int.Parse(CustomerList))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList == null && UserList != null)
                        {

                            var querytest = from e in db.StoreRequisitionMain.Include(s => s.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }
                        else
                        {

                            var querytest = from e in db.StoreRequisitionMain.Include(s => s.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                                Issue = db.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }

                    }
                }

                else
                {
                    if (y.ToString().Length > 0)
                    {


                        var query = from e in PL.GetSRRList()
                                    .OrderByDescending(x => x.StoreReqId)
                                        //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                    select new StoreReQuisitionResult
                                    {
                                        StoreReqId = e.StoreReqId,
                                        SRNo = e.SRNo,
                                        RecommenedBy = e.RecommenedBy.EmpName,
                                        ApprovedBy = e.ApprovedBy.EmpName,
                                        RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                        RequiredDate = e.RequiredDate,
                                        BoardMeetingDate = e.BoardMeetingDate,
                                        Purpose = e.Purpose.PurposeName,
                                        Department = e.Department.DeptName,
                                        Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                        Remarks = e.Remarks,
                                        Reference = e.ReqRef,
                                    };


                        var parser = new Parser<StoreReQuisitionResult>(Request.Form, query);

                        return Json(parser.Parse());

                    }
                    else
                    {


                        if (CustomerList != null && UserList != null)
                        {
                            var querytest = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                            //.Where(p => p.CustomerId == int.Parse(CustomerList))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList != null && UserList == null)
                        {
                            var querytest = from e in PL.GetSRRList()
                             .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            // .Where(p => p.CustomerId == int.Parse(CustomerList))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());
                        }
                        else if (CustomerList == null && UserList != null)
                        {

                            var querytest = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }
                        else
                        {

                            var querytest = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))

                            .OrderByDescending(x => x.StoreReqId)
                                                //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                                            select new StoreReQuisitionResult
                                            {
                                                StoreReqId = e.StoreReqId,
                                                SRNo = e.SRNo,
                                                RecommenedBy = e.RecommenedBy.EmpName,
                                                ApprovedBy = e.ApprovedBy.EmpName,
                                                RequisitionDate = e.ReqDate.ToString("dd-MMM-yy"),
                                                RequiredDate = e.RequiredDate,
                                                BoardMeetingDate = e.BoardMeetingDate,
                                                Purpose = e.Purpose.PurposeName,
                                                Department = e.Department.DeptName,
                                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                                Remarks = e.Remarks,
                                                Reference = e.ReqRef,
                                            };


                            var parser = new Parser<StoreReQuisitionResult>(Request.Form, querytest);
                            return Json(parser.Parse());


                        }

                    }
                }



            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", error = ex.Message });
                //throw ex;
            }

        }


        // POST: StoreRequisition/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoreRequisitionMain storeRequisitionMain)
        {
            var ex = "";
            try
            {
                //if (ModelState.IsValid)
                //{





                #region Mandatory Parameter

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var AddDate = DateTime.Now;
                var UpdateDate = DateTime.Now;
                var PcName = HttpContext.Session.GetString("pcname");
                #endregion

                var duplicateDocument = db.StoreRequisitionMain.Where(i => i.SRNo == storeRequisitionMain.SRNo && i.StoreReqId != storeRequisitionMain.StoreReqId && i.ComId == comid).FirstOrDefault();
                if (duplicateDocument != null)
                {
                    return Json(new { Success = 0, ex = storeRequisitionMain.SRNo + " already exist. Document No can not be Duplicate." });
                }


                DateTime date = storeRequisitionMain.ReqDate;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var activefiscalmonth = db.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
                if (activefiscalmonth == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
                }
                var activefiscalyear = db.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
                if (activefiscalyear == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
                }


                //if (ModelState.IsValid)
                //{
                #region Edit request 
                if (storeRequisitionMain.StoreReqId > 0)
                {

                    storeRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                    storeRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;

                    storeRequisitionMain.ComId = comid;
                    storeRequisitionMain.UpdateByUserId = userid;
                    storeRequisitionMain.DateUpdated = UpdateDate;
                    IQueryable<StoreRequisitionSub> StoreRequisitionSubs = db.StoreRequisitionSub.Where(p => p.StoreReqId == storeRequisitionMain.StoreReqId);


                    var sl = 0;
                    foreach (StoreRequisitionSub item in storeRequisitionMain.StoreRequisitionSub)
                    {
                        item.ComId = comid;
                        item.UserId = userid;

                        sl++;
                        if (item.StoreReqSubId > 0)
                        {
                            item.DateUpdated = DateTime.Now;
                            item.UpdateByUserId = userid;
                            if (item.IsDelete != true)
                            {

                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.Entry(item).State = EntityState.Deleted;
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            try
                            {

                                if (item.IsDelete != true)
                                {
                                    var sub = new StoreRequisitionSub();
                                    sub.ComId = comid;
                                    sub.DateAdded = DateTime.Now;
                                    // sub.DateUpdated = item.DateUpdated;
                                    sub.Note = item.Note;
                                    sub.PcName = PcName;
                                    sub.ProductId = item.ProductId;
                                    sub.StoreReqId = item.StoreReqId;
                                    sub.StoreReqQty = item.StoreReqQty;
                                    sub.StoreReqSubId = item.StoreReqSubId;
                                    sub.RemainingReqQty = item.RemainingReqQty;
                                    sub.SLNo = sl;
                                    //sub.UpdateByUserId = item.UpdateByUserId;

                                    sub.UserId = userid;

                                    db.StoreRequisitionSub.Add(sub);
                                }
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                            }

                        }

                    }

                    db.Entry(storeRequisitionMain).State = EntityState.Modified;
                    db.SaveChanges();


                    TempData["Message"] = "Data Update Successfully";
                    TempData["Status"] = "1";
                    tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), storeRequisitionMain.StoreReqId.ToString(), "Update", storeRequisitionMain.StoreReqId.ToString() + " ReqNo" + storeRequisitionMain.SRNo);
                    return Json(new { Success = 2, StoreReqId = storeRequisitionMain.StoreReqId, ex = "" });
                }
                #endregion

                #region Create Request

                else
                {
                    using (var tr = db.Database.BeginTransaction())
                    {





                        storeRequisitionMain.ComId = comid;
                        storeRequisitionMain.UserId = userid;
                        storeRequisitionMain.DateAdded = AddDate;
                        storeRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;
                        storeRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;

                        var main = new StoreRequisitionMain();
                        main = storeRequisitionMain;
                        var sl = 0;
                        foreach (var item in storeRequisitionMain.StoreRequisitionSub)
                        {
                            sl++;
                            var sub = new StoreRequisitionSub();
                            sub.ComId = comid;
                            sub.DateAdded = AddDate;
                            sub.DateUpdated = item.DateUpdated;
                            sub.Note = item.Note;
                            sub.PcName = PcName;
                            sub.ProductId = item.ProductId;
                            sub.StoreReqId = storeRequisitionMain.StoreReqId;
                            sub.StoreReqQty = item.StoreReqQty;
                            sub.RemainingReqQty = item.RemainingReqQty;
                            sub.SLNo = sl;
                            sub.UpdateByUserId = item.UpdateByUserId;
                            sub.UserId = userid;
                        }

                        db.StoreRequisitionMain.Add(main);
                        db.SaveChanges();
                        //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), commercialCompany.CommercialCompanyId.ToString(), "Update");

                        TempData["Message"] = "Data Save Successfully";
                        TempData["Status"] = "1";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), storeRequisitionMain.StoreReqId.ToString(), "Save", storeRequisitionMain.StoreReqId.ToString() + " ReqNo" + storeRequisitionMain.SRNo);

                        tr.Commit();
                    }
                    return Json(new { Success = 1, StoreReqId = storeRequisitionMain.StoreReqId, ex = "" });
                }
                #endregion


                return Json(new { Success = 1, StoreReqId = storeRequisitionMain.StoreReqId, ex = "" });
                //}
                //}



            }
            catch (Exception e)
            {
                ex = e.Message;
                //return Json(new { Success = 0, error = 1, ex = e.Message });
            }

            return Json(new { Success = 0, error = 1, ex = ex });

        }




        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


            //var abcvouchermain = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id && x.ComId == comid).FirstOrDefault();

            //var reportname = "rptSRR";
            var reportname = "rptSRForm";
            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            HttpContext.Session.SetString("reportquery", "Exec [rptSRRDetails] '" + comid + "', 'SNW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = db.StoreRequisitionMain.Where(x => x.StoreReqId == id).Select(x => x.SRNo).Single();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            //var a = Session["PrintFileName"].ToString();


            string DataSourceName = "DataSet1";
            HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            /////////////////////// sub report test to our report server


            //var subReport = new SubReport();
            //var subReportObject = new List<SubReport>();

            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ChequeNo'";
            //subReport.strRptPathSub = "rptShowVoucher_ChequeNo";
            //subReportObject.Add(subReport);


            //subReport = new SubReport();
            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ReceiptPerson'";
            //subReport.strRptPathSub = "rptShowVoucher_ReceiptPerson";
            //subReportObject.Add(subReport);


            //var jsonData = JsonConvert.SerializeObject(subReportObject);

            ////string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType, jsonData);
            ////string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);
            //return Redirect(callBackUrl);



            string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = type });//, new { id = 1 }
            return Redirect(redirectUrl);

            ///return RedirectToAction("Index", "ReportViewer");


        }







        // GET: StoreRequisition/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";

            //StoreRequisitionMain StoreRequisitionMain = await db.StoreRequisitionMain.Include(p => p.StoreRequisitionSub).ThenInclude(p => p.vProduct).ThenInclude(p => p.vProductUnit).Where(p => p.StoreReqId == id).FirstOrDefaultAsync();


            var storeRequisitionMain = await db.StoreRequisitionMain
            .Include(s => s.ApprovedBy)
            .Include(s => s.Department)
            .Include(s => s.PrdUnit)
            .Include(s => s.Purpose)
            .Include(s => s.RecommenedBy)
            .Include(s => s.StoreRequisitionSub)
            .ThenInclude(s => s.vProduct)
            .ThenInclude(s => s.vPrimaryCategory)
            .ThenInclude(s => s.vProducts)
            .ThenInclude(s => s.vSubCategory)
            .FirstOrDefaultAsync(m => m.StoreReqId == id && m.Status == 0);


            //ViewData["ApprovedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.ApprovedByEmpId);
            //ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", storeRequisitionMain.DepartmentId);
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.comid == comid), "PrdUnitId", "PrdUnitId", storeRequisitionMain.PrdUnitId);
            //ViewData["PurposeId"] = new SelectList(db.Purpose.Where(x => x.ComId == comid), "PurposeId", "PurposeId", storeRequisitionMain.PurposeId);
            //ViewData["RecommenedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.RecommenedByEmpId);



            // PurchaseRequisitionMain storeRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
            if (storeRequisitionMain == null)
            {
                return NotFound();
            }
            InitViewBag("Edit", id, storeRequisitionMain);

            if (storeRequisitionMain.IsSubStore)
            {
                return View("Create", storeRequisitionMain);
            }
            else
            {
                return View("Create", storeRequisitionMain);
            }
            //return Json(new {data= storeRequisitionMain });

        }


        // POST: StoreRequisition/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreRequisitionMain storeRequisitionMain)
        {
            if (id != storeRequisitionMain.StoreReqId)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(storeRequisitionMain);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreRequisitionMainExists(storeRequisitionMain.StoreReqId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApprovedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.ApprovedByEmpId);
            ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", storeRequisitionMain.DepartmentId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitId", storeRequisitionMain.PrdUnitId);
            ViewData["PurposeId"] = new SelectList(db.Purpose.Where(x => x.ComId == comid), "PurposeId", "PurposeId", storeRequisitionMain.PurposeId);
            ViewData["RecommenedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.RecommenedByEmpId);

            return View(storeRequisitionMain);
        }

        // GET: StoreRequisition/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = "Delete";


            var storeRequisitionMain = await db.StoreRequisitionMain
            .Include(s => s.ApprovedBy)
            .Include(s => s.Department)
            .Include(s => s.PrdUnit)
            .Include(s => s.Purpose)
            .Include(s => s.RecommenedBy)
            .Include(s => s.StoreRequisitionSub)
            .ThenInclude(s => s.vProduct)
            .ThenInclude(s => s.vPrimaryCategory)
            .ThenInclude(s => s.vProducts)
            .ThenInclude(s => s.vSubCategory)
            .FirstOrDefaultAsync(m => m.StoreReqId == id && m.Status == 0);

            InitViewBag("Delete", id, storeRequisitionMain);

            if (storeRequisitionMain == null)
            {
                return NotFound();
            }

            //ViewData["ApprovedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.ApprovedByEmpId);
            //ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", storeRequisitionMain.DepartmentId);
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.comid == comid), "PrdUnitId", "PrdUnitId", storeRequisitionMain.PrdUnitId);
            //ViewData["PurposeId"] = new SelectList(db.Purpose.Where(x => x.ComId == comid), "PurposeId", "PurposeId", storeRequisitionMain.PurposeId);
            //ViewData["RecommenedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.RecommenedByEmpId);



            //return View(storeRequisitionMain);
            if (storeRequisitionMain.IsSubStore)
            {
                return View("SubStoreCreate", storeRequisitionMain);
            }
            else
            {
                return View("Create", storeRequisitionMain);
            }
        }

        // POST: StoreRequisition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var storeRequisitionMain = await db.StoreRequisitionMain.FindAsync(id);
                db.StoreRequisitionMain.Remove(storeRequisitionMain);
                await db.SaveChangesAsync();


                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), storeRequisitionMain.StoreReqId.ToString(), "Delete", storeRequisitionMain.StoreReqId.ToString());

                return Json(new { Success = 1, VoucherID = storeRequisitionMain.StoreReqId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new
                {
                    Success = 0,
                    ex = ex.Message.ToString()
                });
            }
            //return RedirectToAction(nameof(Index));
        }
        public JsonResult GerProducts(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            IEnumerable<object> product;
            if (id != null)
            {
                if (id == 0 || id == -1)
                {
                    //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                    product = new SelectList(db.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

                }
                else
                {
                    product = new SelectList(db.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
                    //product = db.Products.Where(x => x.CategoryId == id).Select(x => new { x.ProductId, x.ProductName }).ToList();
                }
            }
            else
            {
                //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                product = new SelectList(db.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

            }
            return Json(new { item = product });
        }
        public ActionResult GetProductInfo(int id)
        {
            var comid = HttpContext.Session.GetString("comid");
            var ProductData = db.Products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName
            }).Where(p => p.ProductId == id).ToList();
            return Json(ProductData);
        }

        [HttpPost]
        public ActionResult DeletePrSub(int prsubid)
        {
            try
            {
                var sub = db.PurchaseRequisitionSub.Find(prsubid);
                db.PurchaseRequisitionSub.Remove(sub);
                db.SaveChanges();
                return Json(new { error = 0, success = 1, message = "This record deleted successfully" });
            }
            catch (Exception ex)
            {
                var m = $" Message:{ex.Message}\nInner Exception:{ex.InnerException.Message}";
                return Json(new { error = 1, success = 0, message = m });
            }

        }

        private void InitViewBag(string title, int? id = null, StoreRequisitionMain storeRequisitionMain = null)
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = title;
            if (title == "Create")
            {
                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
                //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x => x.comid == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");


                #region CategoryId viewbag selectlist
                //List<Category> categorydb = POS.GetCategory(comid).ToList();
                List<Category> categorydb = PL.GetCategory().Where(c => c.CategoryId > 0).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });

                var permission = HttpContext.Session.GetObject<UserPermission>("userpermission");
                if (!permission.IsProduction && !permission.IsMedical)
                {
                    categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });
                }


                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                if (categorydb != null)
                {
                    foreach (Category x in categorydb)
                    {
                        categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                    }
                }
                ViewData["CategoryId"] = (categoryid);
                #endregion



                ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName");
                ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid).Select(x => new { x.SectId, x.SectName }), "SectId", "SectName");
                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid).Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");

                //ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");

                ViewData["PurposeId"] = new SelectList(db.Purpose.Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName");
                //ViewData["RecommenedByEmpId"] = new SelectList(db.Employee.Select(x => new { x.}), "EmployeeId", "EmployeeName");
                //ViewData["ProductId"] = new SelectList(db.Products.Select(x => new { x.ProductId,x.ProductName}), "ProductId", "ProductName");
                //ViewData["ProductBrand"] = new SelectList(db.Products.Where(x => x.ProductBrand != null).Select(x => new { x.}), "ProductBrand", "ProductBrand");
                ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName");
                ViewData["SubWarehouseId"] = new SelectList(PL.GetWarehouse().Select(x => new { x.WarehouseId, x.WhName }), "WarehouseId", "WhName");
            }
            if (title == "Edit" || title == "Delete")
            {
                //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x => x.comid == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");

                #region CategoryId viewbag selectlist
                List<Category> categorydb = POS.GetCategory(comid).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });

                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                if (categorydb != null)
                {
                    foreach (Category x in categorydb)
                    {
                        categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                    }
                }
                ViewData["CategoryId"] = (categoryid);
                #endregion


                ViewBag.reqsub = db.StoreRequisitionSub.Where(r => r.StoreReqId == id).ToList();
                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text", storeRequisitionMain.ApprovedByEmpId);
                ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", storeRequisitionMain.DepartmentId);
                ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid).Select(x => new { x.SectId, x.SectName }), "SectId", "SectName", storeRequisitionMain.SectId);
                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitShortName", storeRequisitionMain.PrdUnitId);

                //ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", storeRequisitionMain.PrdUnitId);

                ViewData["PurposeId"] = new SelectList(db.Purpose, "PurposeId", "PurposeName", storeRequisitionMain.PurposeId);
                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text", storeRequisitionMain.RecommenedByEmpId);
                //ViewData["ProductId"] = new SelectList(db.Products.Take(0).Where(x => x.comid == comid), "ProductId", "ProductName");
                //ViewData["ProductId"] = new SelectList(db.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

                ViewData["SubWarehouseId"] = new SelectList(PL.GetWarehouse().Select(x => new { x.WarehouseId, x.WhName }), "WarehouseId", "WhName", storeRequisitionMain.SubWarehouseId);

                ViewData["UnitId"] = new SelectList(db.Unit, "UnitId", "UnitName");
            }
        }

        private bool StoreRequisitionMainExists(int id)
        {
            return db.StoreRequisitionMain.Any(e => e.StoreReqId == id);
        }
    }
}
