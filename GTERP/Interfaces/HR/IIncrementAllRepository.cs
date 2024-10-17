using GTERP.Models;
using GTERP.ViewModels;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IIncrementAllRepository
    {
        List<IncrementViewModel> IncrementReport(DateTime? from, string act = "");
        void UpdateSalary(List<HR_Emp_Salary> Salaries);
        void CreateSalary(List<HR_Emp_Increment> Increments);
        List<IncrementViewModel> GetIncrementList(int aproval, DateTime? from, string act = "");

        Cat_HRSetting ForWorker();
        Cat_HRSetting ForStaff();
    }
}
