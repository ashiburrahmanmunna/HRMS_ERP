using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILoanOtherRepository : IBaseRepository<HR_Loan_Other>
    {
        void SaveLoanOther(HR_Loan_Other hR_Loan_Other, bool newCalculation);
        HR_Loan_Data_Other unPaidOther1(HR_Loan_Other hR_Loan_Other);
        HR_Loan_Data_Other unPaidOther2(HR_Loan_Other hR_Loan_Other);
        HR_Loan_Other loanOtherMaster(int id);
        List<HR_Loan_Data_Other> LoanOtherCalc(int id);
        List<CalculateData> CalcualteLoanOtherPartial(decimal lAmount, decimal interest, int period, DateTime startDate, decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType);
        IEnumerable<SelectListItem> CompoundOtherList();
        IEnumerable<SelectListItem> GetEmpOtherList();
        IEnumerable<SelectListItem> PayBackOtherList();
        IEnumerable<SelectListItem> LoanTypeList();
        List<HR_Loan_Other> GetLoanOtherDataList();
        void DeleteOtherCycle(int id);
    }
}
