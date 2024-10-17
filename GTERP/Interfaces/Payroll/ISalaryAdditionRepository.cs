using GTERP.EF;
using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface ISalaryAdditionRepository : IBaseRepository<Payroll_SalaryAddition>
    {

        IEnumerable<SelectListItem> GetEmpInfo();
        IEnumerable<HRProcessedDataSalVM> GetPrcDataSal(string prossType, string tableName);
        void SalaryUpdate(List<HrProcessedDataSalUpdate> HR_SalarySettlements, string ProssType);
        IEnumerable<SelectListItem> OtherAddType();
        List<SalaryAddition> LoadSAPartial(DateTime date);
        List<PayrollSalaryAddition> prcUploadSA();
        List<Payroll_Temp_SalaryDataInputWithFile> GetSAList(string fName);
        FileContentResult DownloadSA(string file);
        Payroll_SalaryAddition check(Payroll_SalaryAddition salaryAddition);
        void AddSalaryAddition(Payroll_SalaryAddition salaryAddition);
        void ModifiedSalaryAddition(Payroll_SalaryAddition salaryAddition);
        void DeleteSalaryAddition(int addId);
    }
}
