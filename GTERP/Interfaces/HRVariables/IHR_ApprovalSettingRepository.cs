using GTERP.Interfaces.Self;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HRVariables
{
    public interface IHR_ApprovalSettingRepository:ISelfRepository<HR_ApprovalSetting>
    {
        SelectList GetUserList();
        IEnumerable<SelectListItem> GetApprovalType();
        List<GetApprovalListVM> GetApproveList();
        SelectList GetCompanyList();
        List<SetApproveViewModel> GetApprovalList(string comid, int approve);
        Company GetCompanyName();
        List<Cat_Variable> VarType();

        void Approved(List<ApprovalVM> approve);
        void Disapproved(List<ApprovalVM> disapprove);

        SelectList GetApprovedBy();
    }
}
