using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface IPFContributionRepository : IBaseRepository<HR_PFContribution>
    {
        IEnumerable<SelectListItem> GetPFContributionList();

        IEnumerable<SelectListItem> GetEmpList();
        IEnumerable<SelectListItem> PFContributionList();
        IEnumerable<SelectListItem> CatVariableList();
        List<HR_Emp_Info> EmpData();
        List<PFContribution> PFContributionPartial(DateTime date);
        HR_PFContribution PFContributionCheck(HR_PFContribution PFContribution);

    }
}
