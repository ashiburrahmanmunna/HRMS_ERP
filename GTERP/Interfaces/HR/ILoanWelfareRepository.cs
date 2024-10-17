using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILoanWelfareRepository : IBaseRepository<HR_Loan_Welfare>
    {
        void SaveLoanWelfare(HR_Loan_Welfare HR_Loan_Welfare, bool newCalculation);
        HR_Loan_Data_Welfare unPaidWelfare1(HR_Loan_Welfare HR_Loan_Welfare);
        HR_Loan_Data_Welfare unPaidWelfare2(HR_Loan_Welfare HR_Loan_Welfare);
        HR_Loan_Welfare loanWelfareMaster(int id);
        List<HR_Loan_Data_Welfare> LoanWelfareCalc(int id);
        List<CalculateData> CalcualteLoanWelfarePartial(decimal lAmount, decimal interest, int period, DateTime startDate, decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType);
        IEnumerable<SelectListItem> CompoundWelfareList();
        IEnumerable<SelectListItem> GetEmpWelfareList();
        IEnumerable<SelectListItem> PayBackWelfareList();
        List<HR_Loan_Welfare> GetLoanWelfareDataList();
        void DeleteWelfare(int id);
    }
}
