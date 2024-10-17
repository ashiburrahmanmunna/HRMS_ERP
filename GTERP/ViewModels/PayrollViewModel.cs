using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    #region Emp Wise Salary Ledger
    public class EmpId
    {
        public string GetEmpId { get; set; }
    }
    public class DataCount
    {
        public string TotalRecord { get; set; }
    }
    public class EmpWiseSalaryLedger
    {
        public int EmpId { get; set; }
        [Display(Name = "Emp Code")]
        public string EmpCode { get; set; }
        [Display(Name = "Emp Name")]
        public string EmpName { get; set; }
        [Display(Name = "Department")]
        public string DeptName { get; set; }
        [Display(Name = "Section")]
        public string SectName { get; set; }
        [Display(Name = "Designation")]
        public string DesigName { get; set; }
        [Display(Name = "Emp Type")]
        public string EmpType { get; set; }
        [Display(Name = "Pross Type")]
        public string ProssType { get; set; }
        [Display(Name = "Gross Salary")]
        public float GS { get; set; }
        [Display(Name = "Basic Salary")]
        public float BS { get; set; }
        [Display(Name = "House Rent")]
        public float HR { get; set; }
        [Display(Name = "Medical ALlowance")]
        public float MA { get; set; }
        [Display(Name = "Food Allowance")]
        public float FA { get; set; }
        [Display(Name = "Transport Allowance")]
        public float Trn { get; set; }
        [Display(Name = "Other ALlowance")]
        public float OtherAllow { get; set; }
        [Display(Name = "AttBonus")]
        public float AttBonus { get; set; }
        [Display(Name = "Arrear")]
        public float Arrear { get; set; }
        [Display(Name = "Total OT Hr")]
        public float OTHrTtl { get; set; }
        [Display(Name = "Regular OT Hr")]
        public float ROT { get; set; }
        [Display(Name = "Extra OT Hr")]
        public float ExOTHr { get; set; }
        [Display(Name = "Dynamic Extra OT Hr")]
        public float DynamicExOTHr { get; set; }
        [Display(Name = "WHDay OT Hr")]
        public float WHDayOTHr { get; set; }
        [Display(Name = "WHDay OT Amt")]
        public float OT { get; set; }
        [Display(Name = "ROT Amt")]
        public float ROTAmount { get; set; }
        [Display(Name = "EOT Amt")]
        public float ExOTAmount { get; set; }
        [Display(Name = "Dynamic EOT Amt")]
        public float DynamicExOTAmount { get; set; }
        [Display(Name = "WHDay OT Amt")]
        public float WHDayOTAmt { get; set; }
        [Display(Name = "HDay Bonus")]
        public float HDayBonus { get; set; }
        [Display(Name = "Others Addition")]
        public float OthersAddition { get; set; }
        [Display(Name = "Total Payable")]
        public float TotalPayable { get; set; }
        [Display(Name = "Total Payable Buyer")]
        public float TotalPayableB { get; set; }

        //deduction part
        [Display(Name = "PF")]
        public float PF { get; set; }
        [Display(Name = "AB")]
        public float AB { get; set; }
        [Display(Name = "ADV")]
        public float ADV { get; set; }
        [Display(Name = "Stamp")]
        public float Stamp { get; set; }
        [Display(Name = "Loan")]
        public float Loan { get; set; }
        [Display(Name = "IncomeTax")]
        public float IncomeTax { get; set; }
        [Display(Name = "Others Deduction")]
        public float OthersDeduction { get; set; }
        [Display(Name = "Total Deduct")]
        public float TotalDeduct { get; set; }
        [Display(Name = "Total Deduct Buyer")]
        public float TotalDeductB { get; set; }
        [Display(Name = "Net Salary")]
        public float NetSalary { get; set; }
        [Display(Name = "Net Salary Buyer")]
        public float NetSalaryB { get; set; }
        [Display(Name = "Net Salary Payable")]
        public float NetSalaryPayable { get; set; }
        [Display(Name = "Net Salary Payable Buyer")]
        public float NetSalaryBuyer { get; set; }
    }

    public class PageNo
    {
        public int Results { get; set; }
    }
    #endregion

    #region PF Contribution
    public class PFContribution
    {
        public int PFContributionId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public string DtInput { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public double Amount { get; set; }
        public string PF { get; set; }
    }
    #endregion

    #region Salary Check
    public class SalaryCheck
    {
        public int EmpId { get; set; }
        [Display(Name = "Emp Code")]
        public string EmpCode { get; set; }
        [Display(Name = "Emp Name")]
        public string EmpName { get; set; }
        [Display(Name = "Designation")]
        public string DesigName { get; set; }

        [Display(Name = "Category")]
        public string SubCategoryName { get; set; }
        [Display(Name = "Emp Type")]
        public string EmpTypeName { get; set; }
        [Display(Name = "Department")]
        public string DeptName { get; set; }
        [Display(Name = "Section")]
        public string SectName { get; set; }
        [Display(Name = "SubSection")]
        public string SubSectName { get; set; }
        public string UnitName { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public string DtJoin { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string PayModeName { get; set; }
        public string PaySource { get; set; }
        public string Present { get; set; }
        public string Absent { get; set; }
        public string WDay { get; set; }
        public string HDay { get; set; }
        public string CL { get; set; }
        public string SL { get; set; }
        public string EL { get; set; }
        public string ML { get; set; }
        public string SPL { get; set; }
        public string LWP { get; set; }
        public float PDay { get; set; }
        [Display(Name = "Gross Salary")]
        public float GS { get; set; }
        [Display(Name = "Basic Salary")]
        public float BS { get; set; }
        [Display(Name = "House Rent")]
        public float HR { get; set; }
        [Display(Name = "Medical ALlowance")]
        public float MA { get; set; }
        [Display(Name = "Food Allowance")]
        public float FoodAllow { get; set; }
        [Display(Name = "Transport Allowance")]
        public float ConvAllow { get; set; }
        [Display(Name = "AttBonus")]
        public float AttBonus { get; set; }
        [Display(Name = "Other ALlowance")]
        public float OtherAllow { get; set; }
        public float CurrEL { get; set; }
        public float ELAmt { get; set; }
        [Display(Name = "Total OT Hr")]
        public float OTHrTtl { get; set; }
        [Display(Name = "OT Amt")]
        public float OT { get; set; }
        [Display(Name = "Total Payable")]
        public float TotalPayable { get; set; }
        [Display(Name = "AB")]
        public float AB { get; set; }
        [Display(Name = "ADV")]
        public float ADV { get; set; }
        [Display(Name = "PF")]
        public float PF { get; set; }
        [Display(Name = "IncomeTax")]
        public float IncomeTax { get; set; }
        [Display(Name = "Stamp")]
        public float Stamp { get; set; }
        [Display(Name = "Others Deduction")]
        public float OthersDeduct { get; set; }
        [Display(Name = "Total Deduct")]
        public float TotalDeduct { get; set; }
        [Display(Name = "Net Salary Payable")]
        public float NetSalaryPayable { get; set; }
        public float CashPF { get; set; }
        public float BankPF { get; set; }
        public float GrossCash { get; set; }
        public float GrossBank { get; set; }
        public float CashPay { get; set; }
        public float BankPay { get; set; }
        [Display(Name = "Special Allow.")]
        public float AttAllow { get; set; }
        [Display(Name = "Regular OT Hr")]
        public float OTHRBuyer { get; set; }
        [Display(Name = "ROT Amt")]
        public float OTAmtBuyer { get; set; }
        [Display(Name = "Extra OT Hr")]
        public float ExOTHr { get; set; }
        [Display(Name = "EOT Amt")]
        public float ExOTAmount { get; set; }
        [Display(Name = "DynamicExtra OT Hr")]
        public float DynamicExOTHr { get; set; }
        [Display(Name = "Dynamic EOT Amt")]
        public float DynamicExOTAmount { get; set; }
        [Display(Name = "WHDay OT Hr")]
        public float WHDayOTHr { get; set; }
        [Display(Name = "WHDay OT Amt")]
        public float WHDayOTAmt { get; set; }
        public float NetSalary { get; set; }
        [Display(Name = "Net Salary Buyer")]
        public float NetSalaryB { get; set; }
        [Display(Name = "Total Payable Buyer")]
        public float TotalPayableB { get; set; }
        [Display(Name = "Total Deduct Buyer")]
        public float TotalDeductB { get; set; }
        [Display(Name = "Net Salary")]
        public float NetSalaryPayableB { get; set; }
        [Display(Name = "Dynamic Net Salary")]
        public float DynamicNetSalary { get; set; }
        [Display(Name = "Dynamic Total Payable")]
        public float DynamicTotalPayable { get; set; }
        [Display(Name = "Dynamic Total Deduct")]
        public float DynamicTotalDeduct { get; set; }
        [Display(Name = "Dynamic Net Salary")]
        public float DynamicNetSalaryPayable { get; set; }

        [Display(Name = "Arrear")]
        public float Arrear { get; set; }
        [Display(Name = "Others Addition")]
        public float OthersAddition { get; set; }

        [Display(Name = "Pross Type")]
        public string ProssType { get; set; }

        //deduction part
        
        [Display(Name = "Loan")]
        public float Loan { get; set; }        
        public float LunchDeduct { get; set; }
        public float DD { get; set; }
        public float DynamicOTHr { get; set; }
        public float DynamicOTAmt { get; set; }
        public string GradeName { get; set; }
        public float WDayP { get; set; }
        public float HDayP { get; set; }
        public float HDayBonus { get; set; }
        public string NID { get; set; }
        

    }
    public class TotalCount
    {
        public int Results { get; set; }
        public string GS { get; set; }
        public string BS { get; set; }
        public string HR { get; set; }
        public string MA { get; set; }
        public string FoodAllow { get; set; }
        public string ConvAllow { get; set; }
        public string OtherAllow { get; set; }
        public string AttBonus { get; set; }
        public string Arrear { get; set; }
        public string OTHrTtl { get; set; }
        public string OTHRBuyer { get; set; }
        public string ExOTHr { get; set; }
        public string DynamicOTHr { get; set; }
        public string DynamicExOTHr { get; set; }
        public string WHDayOTHr { get; set; }
        public string OT { get; set; }
        public string OTAmtBuyer { get; set; }
        public string ExOTAmount { get; set; }
        public string DynamicOTAmt { get; set; }
        public string DynamicExOTAmount { get; set; }
        public string WHDayOTAmt { get; set; }
        public string HDayBonus { get; set; }
        public string OthersAddition { get; set; }
        public string TotalPayable { get; set; }
        public string TotalPayableB { get; set; }
        public string DynamicTotalPayable { get; set; }

        //deduction part
        public string PF { get; set; }
        public string AB { get; set; }
        public string ADV { get; set; }
        public string Stamp { get; set; }
        public string Loan { get; set; }
        public string IncomeTax { get; set; }
        public string OthersDeduct { get; set; }
        public string TotalDeduct { get; set; }
        public string TotalDeductB { get; set; }
        public string DynamicTotalDeduct { get; set; }
        public string NetSalary { get; set; }
        public string NetSalaryB { get; set; }
        public string DynamicNetSalary { get; set; }
        public string NetSalaryPayable { get; set; }
        public string NetSalaryPayableB { get; set; }
        public string DynamicNetSalaryPayable { get; set; }
        public string CurrEL { get; set; }
        public string ELAmt { get; set; }
        public string CashPF { get; set; }
        public string BankPF { get; set; }
        public string GrossBank { get; set; }
        public string GrossCash { get; set; }
        public string BankPay { get; set; }
        public string CashPay { get; set; }
        public string AttAllow { get; set; }
        public string DD { get; set; }
    }
    public class TotalSUm
    {
        public string GS { get; set; }
        public string BS { get; set; }
        public string HR { get; set; }
        public string MA { get; set; }
        public string FoodAllow { get; set; }
        public string ConvAllow { get; set; }
        public string OtherAllow { get; set; }
        public string AttBonus { get; set; }
        public string Arrear { get; set; }
        public string OTHrTtl { get; set; }
        public string OTHRBuyer { get; set; }
        public string ExOTHr { get; set; }
        public string DynamicOTHr { get; set; }
        public string DynamicExOTHr { get; set; }
        public string WHDayOTHr { get; set; }
        public string OT { get; set; }
        public string OTAmtBuyer { get; set; }
        public string ExOTAmount { get; set; }
        public string DynamicOTAmt { get; set; }
        public string DynamicExOTAmount { get; set; }
        public string WHDayOTAmt { get; set; }
        public string HDayBonus { get; set; }
        public string OthersAddition { get; set; }
        public string TotalPayable { get; set; }
        public string TotalPayableB { get; set; }
        public string DynamicTotalPayable { get; set; }

        //deduction part
        public string PF { get; set; }
        public string AB { get; set; }
        public string ADV { get; set; }
        public string Stamp { get; set; }
        public string Loan { get; set; }
        public string IncomeTax { get; set; }
        public string OthersDeduct { get; set; }
        public string TotalDeduct { get; set; }
        public string TotalDeductB { get; set; }
        public string DynamicTotalDeduct { get; set; }
        public string NetSalary { get; set; }
        public string NetSalaryB { get; set; }
        public string DynamicNetSalary { get; set; }
        public string NetSalaryPayable { get; set; }
        public string NetSalaryPayableB { get; set; }
        public string DynamicNetSalaryPayable { get; set; }
        public string CurrEL { get; set; }
        public string ELAmt { get; set; }
        public string CashPF { get; set; }
        public string BankPF { get; set; }
        public string GrossBank { get; set; }
        public string GrossCash { get; set; }
        public string BankPay { get; set; }
        public string CashPay { get; set; }
        public string AttAllow { get; set; }
        public string DD { get; set; }
    }

    #endregion

    #region Salary Addition
    public class PayrollSalaryAddition
    {
        public int EmpId { get; set; }
        public string ComId { get; set; }
        public string DtJoin { get; set; }
        public string DtInput { get; set; }
        public double Amount { get; set; }
        public string OtherAddType { get; set; }
        public string Remarks { get; set; }
    }
    public class SalaryAddition
    {
        public int WdId { get; set; }
        public int SalAddId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public string? DtInput { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public double Amount { get; set; }
        public string OtherAddType { get; set; }
        public string Remarks { get; set; }

    }
    #endregion

    #region Salary Deduction
    public class PayrollSalaryDeduction
    {
        public int EmpId { get; set; }
        public string ComId { get; set; }
        public string DtJoin { get; set; }
        public string DtInput { get; set; }
        public double Amount { get; set; }
        public string OtherAddType { get; set; }
        public string Remarks { get; set; }
    }
    public class SalaryDeduction
    {
        public int SalDedId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public string DtInput { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public double Amount { get; set; }
        public string OtherDedType { get; set; }
        public string Remarks { get; set; }
    }

    public class PFWithdrawn
    {
        public int WdId { get; set; }
      
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public string DtInput { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public string DateAdded { get; set; }
        public string DtWithdrawn { get; set; }
        public string Remarks { get; set; }

    }
    #endregion
}
