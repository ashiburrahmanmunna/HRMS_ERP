using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILoanPFRepository : IBaseRepository<HR_Loan_PF>
    {
        void SaveLoanPF(HR_Loan_PF HR_Loan_PF, bool newCalculation);
        HR_Loan_Data_PF unPaidPF1(HR_Loan_PF HR_Loan_PF);
        HR_Loan_Data_PF unPaidPF2(HR_Loan_PF HR_Loan_PF);
        HR_Loan_PF loanPFMaster(int id);
        List<HR_Loan_Data_PF> LoanPFCalc(int id);
        List<CalculateData> CalcualteLoanPFPartial(decimal lAmount, decimal interest, int period, DateTime startDate, decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType);
        IEnumerable<SelectListItem> CompoundPFList();
        IEnumerable<SelectListItem> GetEmpPFList();
        IEnumerable<SelectListItem> PayBackPFList();
        List<HR_Loan_PF> GetLoanPFDataList();
        void DeletePFCycle(int id);
    }
}
