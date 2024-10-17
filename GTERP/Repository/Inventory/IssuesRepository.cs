using GTERP.BLL;
using GTERP.Interfaces.Inventory;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;


namespace GTERP.Repository.Inventory
{
    public class IssuesRepository : IIssuesRepository
    {
        #region common property
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpConext;
        PermissionLevel PL;
        POSRepository POS;
        private readonly IConfiguration _configuration;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        #endregion

        #region Constructor
        public IssuesRepository(
               GTRDBContext context,
               IHttpContextAccessor httpContext,
               PermissionLevel pl,
               IConfiguration configuration,
               IUrlHelperFactory urlHelperFactory,
               IActionContextAccessor actionContextAccessor,
               POSRepository pOS
               )
        {
            _context = context;
            _httpConext = httpContext;
            PL = pl;
            POS = pOS;
            _configuration = configuration;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        #endregion


        public IEnumerable<SelectListItem> UserList()
        {
            var model = new GetUserModel();
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var userid = _httpConext.HttpContext.Session.GetString("userid");
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            List<string> moduleUser = PL.GetModuleUser().ToList();

            var l = new List<AspnetUserList>();
            foreach (var c in list)
            {
                if (moduleUser.Contains(c.UserID))
                {
                    var le = new AspnetUserList();
                    le.Email = c.UserName;
                    le.UserId = c.UserID;
                    le.UserName = c.UserName;
                    l.Add(le);
                }
            }

            return new SelectList(l, "UserId", "UserName", userid);
        }

        public IssueMain lastissueMain()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var userid = _httpConext.HttpContext.Session.GetString("userid");
            return _context.IssueMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(x => x.IssueMainId).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> PrdUnitIdIf()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var userid = _httpConext.HttpContext.Session.GetString("userid");
            var lastissueMain = _context.IssueMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(x => x.IssueMainId).FirstOrDefault();
            return new SelectList(PL.GetPrdUnit()
                .Select(x => new { x.PrdUnitId, x.PrdUnitShortName }),
                "PrdUnitId", "PrdUnitShortName", lastissueMain.PrdUnitId);
        }

        public IEnumerable<SelectListItem> PrdUnitIdElse()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            return new SelectList(PL.GetPrdUnit()

                .Where(x => x.ComId == comid)
                .Select(x => new { x.PrdUnitId, x.PrdUnitShortName }),

                "PrdUnitId", "PrdUnitShortName") ;
        }

        public IQueryable<IssueResult> Query1()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var query = from e in _context.IssueMain
                                     .Include(x => x.IssueSub)
                                     .ThenInclude(x => x.vProduct)
                                     .Include(x => x.PrdUnit)
                                     .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("MED"))
                                     .OrderByDescending(x => x.IssueMainId)

                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                        let ItemDesc = e.IssueSub != null ? e.IssueSub//Take(1).
                        .Select(x => new ItemResult
                        {
                            ItemName = x.vProduct.ProductName,
                            ItemCode = x.vProduct.ProductCode,
                            IssueQty = x.IssueQty.ToString(),
                            IssueRate = x.Rate.ToString(),
                            IssueValue = x.TotalValue.ToString()
                        })
                        .Take(1).ToList() : null

                        select new IssueResult
                        {
                            IssueMainId = e.IssueMainId,
                            IssueNo = e.IssueNo,
                            IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                            IssueRef = e.IssueRef,
                            Department = e.Department.DeptName,
                            PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                            SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                            SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                            //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                            ConvertionRate = e.ConvertionRate,
                            TotalIssueValue = e.TotalIssueValue,
                            Deduction = e.Deduction,
                            NetIssueValue = e.NetIssueValue,
                            SectName = e.Section != null ? e.Section.SectName : "",

                            ItemResultList = ItemDesc,
                            //InNo = e.INNo,
                            //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                            //ExpectedReciveDate = e.ExpectedReciveDate,
                            //TermsAndCondition = e.TermsAndCondition,
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            Store = e.IsSubStore ? "Sub Store" : "Main Store",
                            Remarks = e.Remarks
                        };
            return query;
        }

        public IQueryable<IssueResult> QueryTest(string UserList)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var querytest = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("MED"))
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                            //.Where(p => p.CustomerId == int.Parse(PrdUnitId))

                            .OrderByDescending(x => x.IssueMainId)

                            let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                            select new IssueResult
                            {
                                IssueMainId = e.IssueMainId,
                                IssueNo = e.IssueNo,
                                IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                                IssueRef = e.IssueRef,
                                Department = e.Department.DeptName,
                                PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                                SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                                SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                                //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                                CurCode = e.Currency != null ? e.Currency.CurCode : "",
                                ConvertionRate = e.ConvertionRate,
                                TotalIssueValue = e.TotalIssueValue,
                                Deduction = e.Deduction,
                                NetIssueValue = e.NetIssueValue,
                                SectName = e.Section != null ? e.Section.SectName : "",

                                ItemResultList = ItemDesc,
                                //InNo = e.INNo,
                                //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                                //ExpectedReciveDate = e.ExpectedReciveDate,
                                //TermsAndCondition = e.TermsAndCondition,
                                Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                                Store = e.IsSubStore ? "Sub Store" : "Main Store",
                                Remarks = e.Remarks
                            };
            return querytest;
        }

        public IQueryable<IssueResult> QueryTest2()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var data = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("MED"))
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            // .Where(p => p.CustomerId == int.Parse(PrdUnitId))

                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null

                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",


                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,

                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest3(string UserList)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var data = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("MED"))
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,
                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest4()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var data = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                     .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("MED"))
                             .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))

                             .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",


                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,

                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> Query2()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            var data = from e in _context.IssueMain
                                    .Include(x => x.IssueSub)
                                    .ThenInclude(x => x.vProduct)
                                    .Include(x => x.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") && x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                                    .OrderByDescending(x => x.IssueMainId)

                           //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       let ItemDesc = e.IssueSub != null ? e.IssueSub//Take(1).
                       .Select(x => new ItemResult
                       {
                           ItemName = x.vProduct.ProductName,
                           ItemCode = x.vProduct.ProductCode,
                           IssueQty = x.IssueQty.ToString(),
                           IssueRate = x.Rate.ToString(),
                           IssueValue = x.TotalValue.ToString()
                       })
                       .Take(1).ToList() : null

                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,
                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest5(string UserList)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var data = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                   .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                           .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                           //.Where(p => p.userid == UserList)
                           .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                           //.Where(p => p.CustomerId == int.Parse(PrdUnitId))

                           .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,
                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest6()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var data = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            // .Where(p => p.CustomerId == int.Parse(PrdUnitId))

                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null

                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",


                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,

                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest7(string UserList)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var data = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,
                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };

            return data;
        }

        public IQueryable<IssueResult> QueryTest8()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var data = from e in _context.IssueMain.Include(x => x.PrdUnit)
                                    .Where(x => x.ComId == comid && x.PrdUnit.PrdUnitShortName.Contains("UN1") || x.PrdUnit.PrdUnitShortName.Contains("UN2"))
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))

                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",


                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,

                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> Query3()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var data = from e in _context.IssueMain
                                    .Include(x => x.IssueSub)
                                    .ThenInclude(x => x.vProduct)
                                    .Where(x => x.ComId == comid)
                                    .OrderByDescending(x => x.IssueMainId)
                           //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       let ItemDesc = e.IssueSub != null ? e.IssueSub//Take(1).
                       .Select(x => new ItemResult
                       {
                           ItemName = x.vProduct.ProductName,
                           ItemCode = x.vProduct.ProductCode,
                           IssueQty = x.IssueQty.ToString(),
                           IssueRate = x.Rate.ToString(),
                           IssueValue = x.TotalValue.ToString()
                       })
                       .Take(1).ToList() : null

                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,
                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest9(string UserList)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var data = from e in _context.IssueMain
                            .Where(x => x.ComId == comid)
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                            //.Where(p => p.CustomerId == int.Parse(PrdUnitId))

                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,
                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest10()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var data = from e in _context.IssueMain
                            .Where(x => x.ComId == comid)
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            // .Where(p => p.CustomerId == int.Parse(PrdUnitId))

                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null

                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",


                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,

                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest11(string UserList)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var data = from e in _context.IssueMain
                            .Where(x => x.ComId == comid)
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))
                            //.Where(p => p.userid == UserList)
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,
                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IQueryable<IssueResult> QueryTest12()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);
            var comid = _httpConext.HttpContext.Session.GetString("comid");

            var data = from e in _context.IssueMain
                            .Where(x => x.ComId == comid)
                            .Where(p => (p.IssueDate >= dtFrom && p.IssueDate <= dtTo))

                            .OrderByDescending(x => x.IssueMainId)

                       let ItemDesc = e.IssueSub != null ? e.IssueSub.Select(x => new ItemResult { ItemName = x.vProduct.ProductName, ItemCode = x.vProduct.ProductCode, IssueQty = x.IssueQty.ToString(), IssueRate = x.Rate.ToString(), IssueValue = x.TotalValue.ToString() }).ToList() : null


                       //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                       select new IssueResult
                       {
                           IssueMainId = e.IssueMainId,
                           IssueNo = e.IssueNo,
                           IssueDate = e.IssueDate.ToString("dd-MMM-yy"),

                           IssueRef = e.IssueRef,
                           Department = e.Department.DeptName,
                           PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",

                           SRNo = e.StoreRequisitionMain != null ? e.StoreRequisitionMain.SRNo : e.ManualSRRNo,
                           SRDate = e.ManualSRRDate.HasValue ? e.ManualSRRDate.Value.ToString("dd-MMM-yy") : "[N/A]",


                           //TypeName = e.PaymentType!=null?e.PaymentType.TypeName:"",
                           CurCode = e.Currency != null ? e.Currency.CurCode : "",
                           ConvertionRate = e.ConvertionRate,
                           TotalIssueValue = e.TotalIssueValue,
                           Deduction = e.Deduction,
                           NetIssueValue = e.NetIssueValue,
                           SectName = e.Section != null ? e.Section.SectName : "",

                           ItemResultList = ItemDesc,

                           //InNo = e.INNo,
                           //InDate = e.INDate.HasValue ? e.INDate.Value.ToString("dd-MMM-yy") : "[N/A]",

                           //ExpectedReciveDate = e.ExpectedReciveDate,
                           //TermsAndCondition = e.TermsAndCondition,
                           Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                           Store = e.IsSubStore ? "Sub Store" : "Main Store",
                           Remarks = e.Remarks
                       };
            return data;
        }

        public IssueMain Details(int? id)
        {
            var data = _context.IssueMain
                 .Include(i => i.Section)
                 .Include(i => i.Currency)
                 //.Include(i => i.PaymentType)
                 .Include(i => i.PrdUnit)
                 .Include(i => i.StoreRequisitionMain)
                 //.Include(i => i.Supplier)
                 .FirstOrDefault(m => m.IssueMainId == id);
            return data;
        }

        public IQueryable<Currency> GetCurrency(int id)
        {
            var data = _context.Currency.Where(c => c.CurrencyId == id);
            return data;
        }

        public Object GetDepartmentByStore(int id)
        {
            Object data = _context.StoreRequisitionMain.Where(p => p.StoreReqId == id).Select(p => new
            {
                DeptName = p.Department.DeptName,
                SectId = p.SectId,
                DeptId = p.DepartmentId,
                PrdUnitId = p.PrdUnitId,
                SRNo = p.SRNo,
                INNo = p.INNo,
                INDate = p.INDate,
                ReqRef = p.ReqRef,
                Remarks = p.Remarks,
                ReqDate = p.ReqDate


            }).FirstOrDefault();
            return data;
        }

        public List<IssueDetailsModel> GetStoreRequisitionDataById(int? StoreReqId)
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var userid = _httpConext.HttpContext.Session.GetString("userid");

            var quary = $"EXEC IssueDetailsInformation '{comid}','{userid}',{StoreReqId}";

            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@userid", userid);
            parameters[2] = new SqlParameter("@StoreReqId", StoreReqId);
            //parameters[3] = new SqlParameter("@IsSubStore", false);
            List<IssueDetailsModel> IssueDetailsInformation = Helper.ExecProcMapTList<IssueDetailsModel>("IssueDetailsInformation", parameters);
            return IssueDetailsInformation;
        }

        public List<IssueDetailsModel> GetSubStoreRequisitionDataById(int? StoreReqId)
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var userid = _httpConext.HttpContext.Session.GetString("userid");

            var quary = $"EXEC IssueDetailsInformation '{comid}','{userid}',{StoreReqId}";

            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@userid", userid);
            parameters[2] = new SqlParameter("@StoreReqId", StoreReqId);
            parameters[3] = new SqlParameter("@IsSubStore", true);
            List<IssueDetailsModel> IssueDetailsInformation = Helper.ExecProcMapTList<IssueDetailsModel>("IssueDetailsInformation", parameters);
            return IssueDetailsInformation;
        }

        public IEnumerable<SelectListItem> CurrencyId()
        {
            return new SelectList(_context.Currency.OrderByDescending(x => x.isDefault).OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");
        }

        public IEnumerable<SelectListItem> PaymentTypeId()
        {
            return new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName");
        }

        public IQueryable<Object> StoreRequisition()
        {
            var storeRequisitions = PL.GetSRR().Where(x => x.Complete == 0 && x.Status == 1 && x.IsSubStore == true)
                    .Select(s => new { Value = s.StoreReqId, Text = s.SRNo + " [S.S.]" });
            return storeRequisitions;
        }

        public IEnumerable<SelectListItem> StoreRequisitionList()
        {
            return new SelectList(StoreRequisition(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> WareHouseId()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            return new SelectList(PL.GetWarehouse().Where(x => x.ComId == comid && x.WarehouseId != 0 && x.WhType == "L" && x.ParentId != null && x.IsSubWarehouse == true), "WarehouseId", "WhShortName");
        }

        public IEnumerable<SelectListItem> BOMMainId()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.BOMMain.Where(x => x.ComId == comid), "BOMMainId", "AssembleName");
        }

        public IEnumerable<SelectListItem> PatientId()
        {

            return new SelectList(_context.Cat_Variable.Where(c => c.VarType == "ReleationType").OrderBy(c => c.SLNo), "VarName", "VarName");
        }

        public IEnumerable<SelectListItem> DoctocId()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var doctorInfo = (from emp in _context.HR_Emp_Info
                              join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                              join s in _context.Cat_Section on emp.SectId equals s.SectId
                              join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                              where emp.ComId == comid && des.DesigName.ToUpper().Contains("MEDICAL OFFICER")
                              orderby emp.EmpCode
                              select new
                              {
                                  Value = emp.EmpId,
                                  Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                              }).ToList();

            return new SelectList(doctorInfo, "Value", "Text");
        }

        public IEnumerable<SelectListItem> WareHouseId2()
        {
            return new SelectList(PL.GetWarehouse().Where(x => x.WarehouseId != 0 && x.WhType == "L" && x.ParentId != null && x.WarehouseId == 5), "WarehouseId", "WhShortName");
        }

        public IssueMain DuplicateDocument(IssueMain issueMain)
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            return _context.IssueMain.Where(i => i.IssueNo == issueMain.IssueNo && i.IssueMainId != issueMain.IssueMainId && i.ComId == comid).FirstOrDefault();
        }

        public Acc_FiscalMonth AccFiscalMonth(IssueMain issueMain)
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            DateTime date = issueMain.IssueDate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
        }

        public Acc_FiscalYear AccFiscalYear(IssueMain issueMain)
        {

            DateTime date = issueMain.IssueDate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return _context.Acc_FiscalYears.Where(x => x.FYId == AccFiscalMonth(issueMain).FYId).FirstOrDefault();
        }

        public HR_ProcessLock LockCheck(IssueMain issueMain)
        {
            return _context.HR_ProcessLock
                .Where(p => p.LockType.Contains("Store Lock") && p.DtDate.Date <= issueMain.IssueDate.Date && p.DtToDate.Value.Date >= issueMain.IssueDate.Date
                    && p.IsLock == true).FirstOrDefault();
        }

        public void CreateIssueMain(IssueMain issueMain)
        {
            var result = "";

            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var userid = _httpConext.HttpContext.Session.GetString("userid");
            var pcname = _httpConext.HttpContext.Session.GetString("pcname");
            var nowdate = DateTime.Now.ToString("dd-MMM-yyyy");

            if (issueMain.IssueMainId > 0)
            {

                issueMain.FiscalMonthId = AccFiscalMonth(issueMain).FiscalMonthId;
                issueMain.FiscalYearId = AccFiscalYear(issueMain).FiscalYearId;

                var currentissuesub = _context.IssueSub.Include(x => x.IssueSubWarehouse).Where(p => p.IssueMainId == issueMain.IssueMainId);


                foreach (IssueSub ss in currentissuesub)
                {
                    _context.IssueSub.Remove(ss);
                }
                _context.SaveChanges();


                foreach (IssueSub item in issueMain.IssueSub)
                {
                    if (item.IssueSubId > 0)
                    {
                        foreach (IssueSub ss in issueMain.IssueSub)
                        {
                            if (ss.IssueSubWarehouse != null)
                            {
                                foreach (IssueSubWarehouse sss in ss.IssueSubWarehouse)
                                {
                                    sss.IssueSubWarehouseId = 0;
                                }
                            }
                            item.IssueSubId = 0;
                            _context.IssueSub.Add(item);

                        }

                    }
                    else
                    {
                        _context.IssueSub.Add(item);
                    }
                }
                issueMain.UpdateByUserId = userid;
                issueMain.DateUpdated = DateTime.Parse(nowdate);
                _context.Entry(issueMain).State = EntityState.Modified;
                result = "2";

                _context.SaveChanges();

                if (!issueMain.IsDirectIssue)
                {

                    SqlParameter[] sqlParameter = new SqlParameter[3];
                    sqlParameter[0] = new SqlParameter("@ComId", comid);
                    sqlParameter[1] = new SqlParameter("@Id", issueMain.IssueMainId);
                    sqlParameter[2] = new SqlParameter("@Type", "Update");

                    Helper.ExecProc("prcProcessUpdateSRRAfterIssue", sqlParameter);
                }
            }
            else
            {
                issueMain.FiscalMonthId = AccFiscalMonth(issueMain).FiscalMonthId;
                issueMain.FiscalYearId = AccFiscalYear(issueMain).FiscalYearId;

                issueMain.ComId = comid;
                issueMain.UserId = userid;
                issueMain.PcName = pcname;
                issueMain.DateAdded = DateTime.Parse(nowdate);


                _context.Add(issueMain);
                result = "1";
                _context.SaveChanges();

                if (!issueMain.IsDirectIssue)
                {

                    SqlParameter[] sqlParameter = new SqlParameter[3];
                    sqlParameter[0] = new SqlParameter("@ComId", comid);
                    sqlParameter[1] = new SqlParameter("@Id", issueMain.IssueMainId);
                    sqlParameter[2] = new SqlParameter("@Type", "Insert");

                    Helper.ExecProc("prcProcessUpdateSRRAfterIssue", sqlParameter);
                }
            }
        }

        public IEnumerable<SelectListItem> PrdRqId(IssueMain issueMain)
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", issueMain.StoreReqId);
        }

        public string PrintMissingSequence(string rptFormat, string action, string Type, string FromNo, string ToNo, string PrdUnitId)
        {

            string comid = _httpConext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            //string redirectUrl = "";
            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (action == "PrintMissingSequence")
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rpt_MissingSequence";
                filename = "rpt_MissingSequence" + DateTime.Now.Date.ToString();
                var query = "Exec [rpt_MissingSequence] '" + comid + "' , ";
                _httpConext.HttpContext.Session.SetString("reportquery", "Exec [rpt_MissingSequence] '" + comid + "',  '" + Type + "' , '" + FromNo + "','" + ToNo + "' , '" + PrdUnitId + "' ");
                _httpConext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpConext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";

            //HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpConext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpConext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            //var ConstrName = "ApplicationServices";
            //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            //redirectUrl = callBackUrl;

            string redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }

            return redirectUrl;

        }

        public string PrintIssueVoucher(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId)
        {
            string comid = _httpConext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (action == "PrintIssueVoucher")
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rptIssueVoucher";
                filename = "IssueVoucher" + DateTime.Now.Date.ToString();
                var query = "Exec [Inv_IssueVoucher] '" + comid + "'";
                _httpConext.HttpContext.Session.SetString("reportquery", "Exec [Inv_IssueVoucher] '" + comid + "','" + FromDate + "','" + ToDate + "',0,'" + PrdUnitId + "' ");
                _httpConext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpConext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";

            //HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpConext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpConext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            //var ConstrName = "ApplicationServices";
            //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            //redirectUrl = callBackUrl;

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
            return redirectUrl;
        }

        public string IssueDetailsReport(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId)
        {
            string comid = _httpConext.HttpContext.Session.GetString("comid");
            string redirectUrl = "";
            var reportname = "";
            var filename = "";

            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (action == "PrintIssueDetails")
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rptIssueDetails";
                filename = "IssueDetails" + DateTime.Now.Date.ToString();
                var query = "Exec [Inv_IssueDetails] '" + comid + "'";
                _httpConext.HttpContext.Session.SetString("reportquery", "Exec [Inv_IssueDetails] '" + comid + "','" + FromDate + "','" + ToDate + "', '" + PrdUnitId + "' ");
                _httpConext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpConext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";

            //HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpConext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpConext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;
            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            //var ConstrName = "ApplicationServices";
            //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            //redirectUrl = callBackUrl;
            return redirectUrl;
        }

        public string PrintIssueSummary(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId)
        {
            string comid = _httpConext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (action == "PrintIssueSummary")
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rptIssueSummary";
                filename = "rptIssueSummary" + DateTime.Now.Date.ToString();
                var query = "Exec [Inv_rptIssueSummary] '" + comid + "'";
                _httpConext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIssueSummary] '" + comid + "','" + FromDate + "','" + ToDate + "' , '" + PrdUnitId + "'  ");
                _httpConext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpConext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";

            //HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpConext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpConext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            //var ConstrName = "ApplicationServices";
            //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            //redirectUrl = callBackUrl;


            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return redirectUrl;
        }

        public bool IssueMainExists(int id)
        {
            return _context.IssueMain.Any(e => e.IssueMainId == id);
        }

        public IEnumerable<object> GetProducts(int? id)
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            IEnumerable<object> product;
            if (id != null)
            {
                if (id == 0 || id == -1)
                {
                    //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                    product = new SelectList(_context.Products.Where(x => x.AccIdInventory != x.AccIdConsumption).Where(x => x.comid == comid && x.AccIdConsumption > 0).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

                }
                else
                {
                    product = new SelectList(_context.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
                    //product = db.Products.Where(x => x.CategoryId == id).Select(x => new { x.ProductId, x.ProductName }).ToList();
                }
            }
            else
            {
                //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                product = new SelectList(_context.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

            }
            return product;
        }

        public IssueMain FindData(int id)
        {

            //var issueMain = await db.IssueMain.FindAsync(id);
            var issueMain = _context.IssueMain
               .Include(p => p.IssueSub)
               .ThenInclude(p => p.vProduct)
               .ThenInclude(p => p.vProductUnit)
               .Include(g => g.IssueSub)
               .ThenInclude(g => g.vWarehouse)
               .Include(p => p.IssueSub)
               .ThenInclude(p => p.IssueSubWarehouse)
               .FirstOrDefault(m => m.IssueMainId == id && m.Status == 0);

            return issueMain;
        }

        public void DeleteData(int id)
        {
            var issueMain = FindData(id);
            var CurrentIssueSub = _context.IssueSub.Include(x => x.IssueSubWarehouse).Where(p => p.IssueMainId == issueMain.IssueMainId);
            _context.IssueSub.RemoveRange(CurrentIssueSub);
            _context.SaveChanges();

            _context.IssueMain.Remove(issueMain);
            _context.SaveChanges();
        }

        public IssueMain FindData2(int? id)
        {
            var issueMain = _context.IssueMain
               .Include(p => p.IssueSub)
               .ThenInclude(p => p.vProduct)
               .ThenInclude(p => p.vProductUnit)
               .Include(g => g.IssueSub)
               .ThenInclude(g => g.vWarehouse)
               .Include(p => p.IssueSub)
               .ThenInclude(p => p.IssueSubWarehouse)
               .ThenInclude(p => p.vWarehouse)
               .FirstOrDefault(m => m.IssueMainId == id && m.Status == 0);
            return issueMain;
        }

        public IEnumerable<SelectListItem> StoreReqId(int? id)
        {
            var issueMain = FindData2(id);
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.StoreRequisitionMain.Where(x => x.ComId == comid && x.Complete == 0 && x.Status > 0), "StoreReqId", "SRNo", issueMain.StoreReqId);
        }

        public List<SelectListItem> CategoryId()
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            List<Category> categorydb = POS.GetCategory(comid).ToList();

            List<SelectListItem> categoryid = new List<SelectListItem>();
            //categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
            categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });



            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (categorydb != null)
            {
                foreach (Category x in categorydb)
                {
                    categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                }
            }
            return categoryid;
        }

        public void Update(IssueMain issueMain)
        {
            _context.Update(issueMain);
            _context.SaveChanges();
        }

        public List<IssueDetailsModel> IssueDetailsInformation(int? id)
        {
            var comid = _httpConext.HttpContext.Session.GetString("comid");
            var userid = _httpConext.HttpContext.Session.GetString("userid");
            var pcname = _httpConext.HttpContext.Session.GetString("pcname");
            var issueMain = FindData2(id);
            var quary = $"EXEC IssueDetailsInformation '{comid}','{userid}',{issueMain.StoreReqId},{issueMain.IssueMainId}";
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@userid", userid);
            parameters[2] = new SqlParameter("@StoreReqId", issueMain.StoreReqId.ToString());
            parameters[3] = new SqlParameter("@IssueId", issueMain.IssueMainId);
            parameters[4] = new SqlParameter("@IsSubStore", issueMain.IsSubStore);
            List<IssueDetailsModel> IssueDetailsInformation = Helper.ExecProcMapTList<IssueDetailsModel>("IssueDetailsInformation", parameters);
            return IssueDetailsInformation;
        }

        public string Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpConext.HttpContext.Session.GetString("comid");


            //var abcvouchermain = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id && x.ComId == comid).FirstOrDefault();

            //var reportname = "rptIssueIndividual";
            var reportname = "rptSRForm";
            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            _httpConext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            _httpConext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndIssueDetails] '" + comid + "', 'ISSUENW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = _context.IssueMain.Where(x => x.IssueMainId == id).Select(x => x.IssueNo).Single();
            _httpConext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            //var a = Session["PrintFileName"].ToString();

            string DataSourceName = "DataSet1";
            _httpConext.HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpConext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpConext.HttpContext.Session.GetString("reportquery");
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
            //string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);


            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }
    }
}
