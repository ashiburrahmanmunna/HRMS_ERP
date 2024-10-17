using GTERP.EF;
using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;


namespace GTERP.Interfaces.Payroll
{
    public interface IPFWithdrawnRepository: IBaseRepository<HR_PF_Withdrawn>
    {
        IEnumerable<SelectListItem> GetEmpInfo();
        IEnumerable<HRProcessedDataSalVM> GetPrcDataSal(string prossType, string tableName);
        //void SalaryUpdate(List<HrProcessedDataSalUpdate> HR_SalarySettlements, string ProssType);
        //IEnumerable<SelectListItem> OtherAddType();
        List<PFWithdrawn> LoadPFWithdawnPartial(DateTime date);
        //List<PayrollSalaryAddition> prcUploadSA();
        //List<Payroll_Temp_SalaryDataInputWithFile> GetSAList(string fName);
        //FileContentResult DownloadSA(string file);


        HR_PF_Withdrawn check(HR_PF_Withdrawn pF_Withdrawn);
        void AddpF_WithdrawnAddition(HR_PF_Withdrawn pF_Withdrawn);
        void ModifiedpF_WithdrawnAddition(HR_PF_Withdrawn pF_Withdrawn);
        void DeletepF_WithdrawnAddition(int addId);
    }
}
