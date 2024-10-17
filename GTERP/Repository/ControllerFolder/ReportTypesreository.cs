using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.ControllerFolder
{
    public class ReportTypesRepository : SelfRepository<ReportType>, IReportTypesRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<ReportTypesRepository> _logger;


        public ReportTypesRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<ReportTypesRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public List<object> ApprovalList(string userId)
        {
            object approvalList = _context.ApprovalPanels
                .Where(x => x.CreatedUserId == userId)
                .Select(x => new { x.ReportType.Id, x.ReportType.TypeTitle })
                .Distinct().ToList();
            return (List<object>)approvalList;
        }

        public List<object> ApprovalList1(string userId, int reportTypeId)
        {
            object approvalList = _context.ApprovalPanels
                .Where(x => x.CreatedUserId == userId)
                .Select(x => new { x.ApprovalRole.Id, x.ApprovalRole.RoleTitle, x.ApprovedUserId })
                .Distinct().ToList();
            return (List<object>)approvalList;
        }

        public List<object> ApprovalList2(string userId, int reportTypeId)
        {
            object approvalUserList = _context.ApprovalPanels
                .Where(x => x.CreatedUserId == userId && x.ReportTypeId == reportTypeId)
                .Select(x => new { x.ApprovedUserId }).ToList();
            return (List<object>)approvalUserList;
        }

        public void ApprovalTransfer(string fromUserId, string toUserId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@newUserId", toUserId);
            sqlParameter[1] = new SqlParameter("@comId", _httpcontext.HttpContext.Session.GetString("comid"));
            sqlParameter[2] = new SqlParameter("@fromUserId", fromUserId);

            Helper.ExecProc("prcApprovalTransfer", sqlParameter);
        }
    }
}
