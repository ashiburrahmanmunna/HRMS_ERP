using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HR
{
    public interface IIncrementRepository
    {
        IEnumerable<SelectListItem> GetEmpList1(string comid, string userId);
        IEnumerable<SelectListItem> GetEmpList2();
        IEnumerable<SelectListItem> IncTypeList();
        List<HR_Emp_Info> EmpInfo(int empid);
        List<HR_Emp_Salary> EmpSalary(int empid);
        void GetSalaryEmp(int empId, float BS);
        void SaveIncrementInfo(HR_Emp_Increment hR_Emp_Increment);
        void GetIncrementId(int incId);
        IQueryable<HR_Emp_Increment> GetIncrementInfo(int IncId);
        void DeleteIncrement(int id);
    }
}
