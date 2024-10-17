using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HR
{
    public interface IEmployeeArrearBill : IBaseRepository<HR_Emp_ArrearBill>
    {
        IEnumerable<SelectListItem> GetArrearBillList();

        IQueryable<HR_Emp_ArrearBill> GetAllEmpArrearBill();
        int? FindByEmpId(int? Id);
    }
}
