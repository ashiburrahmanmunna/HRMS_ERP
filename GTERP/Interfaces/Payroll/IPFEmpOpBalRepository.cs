using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface IPFEmpOpBalRepository : IBaseRepository<HR_Emp_PF_OPBal>
    {
        List<HR_Emp_PF_OPBal> GetIndexInfo();
        HR_Emp_PF_OPBal GetEmpPFOpBal(int? id);
        IEnumerable<SelectListItem> DelEmpInfoList(int? id);
        IEnumerable<SelectListItem> DebitAccId(int? id);
        IEnumerable<SelectListItem> CreditAccId(int? id);
        IEnumerable<SelectListItem> PFOPBalYID(int? id);


        IEnumerable<SelectListItem> CreatelEmpInfoList();
        IEnumerable<SelectListItem> CreateDebitAccId();
        IEnumerable<SelectListItem> CreateCreditAccId();
        IEnumerable<SelectListItem> CreatePFOPBalYID();
    }
}
