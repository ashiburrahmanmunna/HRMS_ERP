using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface ISalarySettlementRepository : IBaseRepository<HR_SalarySettlement>
    {
        IEnumerable<SelectListItem> GetSalarySettlementList();
        List<Pross> GetProssType();
        List<OTFCSalarySettlement> UpdateData(string prossType);
        List<OTFCSalarySettlement> Search(string prossType);
        void CreateSalarySettlement(List<HR_SalarySettlement> HR_SalarySettlements, string ProssType);
    }
}
