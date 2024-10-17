using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface ISalaryDeductionRepository : IBaseRepository<Payroll_SalaryDeduction>
    {

        IEnumerable<SelectListItem> GetEmpInfo();
        IEnumerable<SelectListItem> OtherDedType();
        List<SalaryDeduction> LoadSDPartial(DateTime date);
        List<PayrollSalaryDeduction> prcUploadSD();
        List<Payroll_Temp_SalaryDataInputWithFile> GetSDList(string fName);
        FileContentResult DownloadSD(string file);
        Payroll_SalaryDeduction check(Payroll_SalaryDeduction salaryDeduction);
        void AddSalaryDeduction(Payroll_SalaryDeduction salaryDeduction);
        void ModifiedSalaryDeduction(Payroll_SalaryDeduction salaryDeduction);
        void DeleteSalaryDeduction(int dedId);
    }
}
