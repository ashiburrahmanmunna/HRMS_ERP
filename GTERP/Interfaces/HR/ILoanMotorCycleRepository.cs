using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILoanMotorCycleRepository : IBaseRepository<HR_Loan_MotorCycle>
    {
        void SaveLoanMotorCycle(HR_Loan_MotorCycle HR_Loan_MotorCycle, bool newCalculation);
        HR_Loan_Data_MotorCycle unPaidMotor(HR_Loan_MotorCycle HR_Loan_MotorCycle);
        HR_Loan_MotorCycle loanMotorMaster(int id);
        List<HR_Loan_Data_HouseBuilding> LoanMotorCalc(int id);
        List<CalculateData> CalcualteLoanMotorPartial(decimal lAmount, decimal interest, int period, DateTime startDate, decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType);
        IEnumerable<SelectListItem> CompoundMotorList();
        IEnumerable<SelectListItem> GetEmpMotorList();
        IEnumerable<SelectListItem> PayBackMotorList();
        List<HR_Loan_MotorCycle> GetLoanMotorDataList();
        void DeleteMotorCycle(int id);
    }
}
