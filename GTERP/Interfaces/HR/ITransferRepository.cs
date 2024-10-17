using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.HR
{
    public interface ITransferRepository
    {
        IEnumerable<SelectListItem> GetEmpList1();
        IEnumerable<SelectListItem> GetEmpList2();
        IEnumerable<SelectListItem> IncTypeList();
        //IEnumerable<SelectListItem> IncTypeList1();
        List<HR_Emp_Info> EmpInfo(int empid);
       //  List<HR_Emp_Salary> EmpSalary(int empid);
       // void GetSalaryEmp(int empId, float BS);
        void SaveTransferInfo(HR_Emp_Increment hR_Emp_Increment);
        void GetIncrementId(int incId);
        IQueryable<HR_Emp_Increment> GetTransferInfo(int IncId);
        void DeleteTransfer(int id);
    }
}
