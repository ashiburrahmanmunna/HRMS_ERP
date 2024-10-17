using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HR
{
    public interface IEmployeeSalaryRepository : IBaseRepository<HR_Emp_Salary>
    {
        IQueryable<SalaryInfoVM> GetEmpSalary();
        IEnumerable<SelectListItem> LId1();
        IEnumerable<SelectListItem> LId2();
        IEnumerable<SelectListItem> LId3();
        IEnumerable<SelectListItem> PFLLId();
        IEnumerable<SelectListItem> PFLLId2();
        IEnumerable<SelectListItem> PFLLId3();
        IEnumerable<SelectListItem> GLId();
        IEnumerable<SelectListItem> HBLId();
        IEnumerable<SelectListItem> HBLId2();
        IEnumerable<SelectListItem> HBLId3();
        IEnumerable<SelectListItem> MCLId();
        IEnumerable<SelectListItem> PFLId();
        IEnumerable<SelectListItem> WelfareLId();

        Cat_GasChargeSetting GasChargeSetting(int EmpId, int LId1, int BId, Double BS);
        Cat_ElectricChargeSetting ElectricChargeSetting(int EmpId, int LId1, int BId, Double BS);
        Cat_HRSetting HR(int EmpId, int LId1, int BId, Double BS);
        Cat_HRExpSetting HRExp(int EmpId, int LId1, int BId, Double BS);
        Task<HR_Emp_Salary> EditEmpSalary(int? id);

        IEnumerable<SelectListItem> GetEmpList();
    }
}
