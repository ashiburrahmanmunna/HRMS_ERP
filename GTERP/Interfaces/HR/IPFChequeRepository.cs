using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HR
{
    public interface IPFChequeRepository : IBaseRepository<HR_PF_Cheque>
    {
        IEnumerable<SelectListItem> HR_PF_Cheque();
        IEnumerable<SelectListItem> EmpList();
        IEnumerable<SelectListItem> EmpListEdit(int id);
        IEnumerable<SelectListItem> EmpListWithLessInfo();
        IEnumerable<SelectListItem> CatVariableList();

        IQueryable<HR_PF_Cheque> GetReleasedAll();
        
        void ApproveSet(HR_PF_Cheque hR_PF_Cheque);
    }
}
