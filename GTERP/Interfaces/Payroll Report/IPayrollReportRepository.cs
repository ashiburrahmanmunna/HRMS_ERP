using GTERP.Models.Payroll;
using GTERP.ViewModels;


using System;

namespace GTERP.Interfaces.Payroll_Report
{
    public interface IPayrollReportRepository
    {
        //public String AdvSalary(AdvSalaryReport aAdvSalaryReport);
        //public String BoardPaper(BoardPaper BoardPapermodel);
        public String EarnLeaveSheet(EarnLeaveSheet earnLeaveSheet);
        public String FestBonus(FestivalBonus FestivalBonusmodel);
        //public String Loan(LoanReport LoanReportmodel);

        public String SalarySheet(SalarySheet SalarySheetmodel);
        public String PFSheet(PFSheet PFSheetmodel);
        public String CasualSalarySheet(SalarySheet CasualSalarySheetmodel);
        public string SalarySheetB(SalarySheet SalarySheetBmodel);
        public string DynamicSalarySheet(SalarySheet SalarySheetBmodel);
        public string ExtraOTSheet(SalarySheet ExtraOTSheet);

        public string MGTSalarySheet(SalarySheet SalarySheetmodel);
        public string MLGetReport(int? lvId, string rptFormat);

    }
}
