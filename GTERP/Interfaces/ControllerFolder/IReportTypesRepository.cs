using GTERP.Interfaces.Self;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface IReportTypesRepository : ISelfRepository<ReportType>
    {
        List<object> ApprovalList(string userId);
        List<object> ApprovalList1(string userId, int reportTypeId);
        List<object> ApprovalList2(string userId, int reportTypeId);
        void ApprovalTransfer(string fromUserId, string toUserId);
    }
}
