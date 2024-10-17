using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILoanHouseBuilding : IBaseRepository<HR_Loan_HouseBuilding>
    {
        public List<HR_Loan_HouseBuilding> GetLoanHouseDataList();
        IEnumerable<SelectListItem> GetEmpHouseList();
        IEnumerable<SelectListItem> GetEmpList(int id);
        IEnumerable<SelectListItem> CompoundHouseList();
        IEnumerable<SelectListItem> PayBackHouseList();
        void SaveLoanHouseBuilding(HR_Loan_HouseBuilding hR_Loan_HouseBuilding, bool newCalculation);
        HR_Loan_Data_HouseBuilding unPaidHouse(HR_Loan_HouseBuilding hR_Loan_HouseBuilding);
        List<CalculateData> CalcualteLoanHousePartial(decimal lAmount, decimal interest, int period, DateTime startDate,
            decimal ttlLoanAmt, decimal ttlInterest, decimal monthlyLoanAmt, string loanType);

        List<HR_Loan_Data_HouseBuilding> LoanHouseCalc(int id);
        HR_Loan_HouseBuilding loanHouseMaster(int id);
        void DeleteHouseBuilding(int id);

    }
}
