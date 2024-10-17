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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Repository.Inventory
{


    public class StoreRequisitionRepository : IStoreRequisitionRepository
    {
        POSRepository POS;
        PermissionLevel PL;
        private readonly IConfiguration _configuration;


        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;

        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();



        public StoreRequisitionRepository(
            GTRDBContext context,
            POSRepository _POS,
            PermissionLevel _pl,
            IConfiguration configuration,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _context = context;

            POS = _POS;
            PL = _pl;
            _configuration = configuration;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);

        }



        public dynamic GetUserList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");

            UserPermission permission = _httpContext.HttpContext.Session.GetObject<UserPermission>("userpermission");



            var appKey = _httpContext.HttpContext.Session.GetString("appkey");
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

            var Userlist = new SelectList(userList, "UserId", "UserName", userid);

            return Userlist;
        }
        public Task<StoreRequisitionMain> GetStoreRequisitionDetails(int? id)
        {
            if (id != null)
            {
                return null;
            }

            var storeRequisitionMain = _context.StoreRequisitionMain
                .Include(s => s.ApprovedBy)
                .Include(s => s.Department)
                .Include(s => s.PrdUnit)
                .Include(s => s.Purpose)
                .Include(s => s.RecommenedBy)
                .FirstOrDefaultAsync(m => m.StoreReqId == id);

            return storeRequisitionMain;
        }

        public StoreRequisitionMain GetUserwiseSRR()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");

            StoreRequisitionMain userwiseSRR = _context.StoreRequisitionMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.StoreReqId).FirstOrDefault();
            return userwiseSRR;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedical()
        {

            var query = from e in PL.GetSRRList()
                         .OrderByDescending(x => x.StoreReqId)

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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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
            var query = from e in PL.GetSRRList()
                            .Where(p => (p.ReqDate >= dtFrom && p.ReqDate <= dtTo))
                            .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                            .OrderByDescending(x => x.StoreReqId)

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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };

            return query;
        }
        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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
            var query = from e in PL.GetSRRList()
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByUser(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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
            var query = from e in PL.GetSRRList()
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByDate(string FromDate, string ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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
            var query = from e in PL.GetSRRList()
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };



            return query;
        }
        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProduction()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var query = from e in _context.StoreRequisitionMain.Include(s => s.PrdUnit)
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in _context.StoreRequisitionMain.Include(s => s.PrdUnit)
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in _context.StoreRequisitionMain.Include(s => s.PrdUnit)
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };

            return query;
        }


        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByUser(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in _context.StoreRequisitionMain.Include(s => s.PrdUnit)
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByDate(string FromDate, string ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in _context.StoreRequisitionMain.Include(s => s.PrdUnit)
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
                            Issue = _context.IssueMain.Where(i => i.StoreReqId == e.StoreReqId && i.Status == 1).FirstOrDefault(),
                        };


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResults()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
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


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetSRRList()
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


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetSRRList()
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


            return query;
        }


        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByUser(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetSRRList()
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


            return query;
        }

        public IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByDate(string FromDate, string ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetSRRList()
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




            return query;
        }

        public string PrintReport(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "rptSRForm";
            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            _httpContext.HttpContext.Session.SetString("reportquery", "Exec [rptSRRDetails] '" + comid + "', 'SNW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");

            string filename = _context.StoreRequisitionMain.Where(x => x.StoreReqId == id).Select(x => x.SRNo).Single();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


            string DataSourceName = "DataSet1";
            _httpContext.HttpContext.Session.SetObject("rptList", postData);

            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";


            string redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = type });
            return redirectUrl;
        }

        public StoreRequisitionMain StoreRequisitionMain(int? id)
        {
            return _context.StoreRequisitionMain
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
            .FirstOrDefault(m => m.StoreReqId == id && m.Status == 0);
        }


        public IEnumerable<SelectListItem> ApprovedByEmpId(StoreRequisitionMain storeRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.ApprovedByEmpId);
        }

        public IEnumerable<SelectListItem> DepartmentId(StoreRequisitionMain storeRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", storeRequisitionMain.DepartmentId);
        }

        public IEnumerable<SelectListItem> PrdUnitId(StoreRequisitionMain storeRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitId", storeRequisitionMain.PrdUnitId);
        }


        public IEnumerable<SelectListItem> PurposeId(StoreRequisitionMain storeRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Purpose.Where(x => x.ComId == comid), "PurposeId", "PurposeId", storeRequisitionMain.PurposeId);
        }
        public IEnumerable<SelectListItem> RecommenedByEmpId(StoreRequisitionMain storeRequisitionMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName", storeRequisitionMain.RecommenedByEmpId);
        }

        public StoreRequisitionMain FindById(int id)
        {
            var data = _context.StoreRequisitionMain.Find(id);
            return data;
        }

        public StoreRequisitionMain LastStorereq()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.StoreRequisitionMain.Where(x => x.ComId == comid).OrderByDescending(D => D.StoreReqId).FirstOrDefault();
        }

        public StoreRequisitionMain UserWiseSRR()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            return _context.StoreRequisitionMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(D => D.StoreReqId).FirstOrDefault();
        }
    }
}
