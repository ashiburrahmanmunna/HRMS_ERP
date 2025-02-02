﻿using System;
using System.Collections.Generic;

#nullable disable

namespace GTERP.EF
{
    public partial class HrProcessedDataSal
    {
        public int? AEmpId { get; set; }
        public string ComId { get; set; }
        public long EmpId { get; set; }
        public string Month { get; set; }
        public short Year { get; set; }
        public string EmpCode { get; set; }
        public DateTime? DtInput { get; set; }
        public string ProssType { get; set; }
        public short? DesigId { get; set; }
        public short SectId { get; set; }
        public short? SubSectId { get; set; }
        public short? DeptId { get; set; }
        public string Band { get; set; }
        public string Grade { get; set; }
        public string GradeSal { get; set; }
        public string PaySlipNo { get; set; }
        public string PaySource { get; set; }
        public string PayMode { get; set; }
        public string EmpSts { get; set; }
        public float WorkingDays { get; set; }
        public float Pday { get; set; }
        public float Dday { get; set; }
        public float Wday { get; set; }
        public float Hday { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float Late { get; set; }
        public float LateMin { get; set; }
        public float Cl { get; set; }
        public float El { get; set; }
        public float Sl { get; set; }
        public float Ml { get; set; }
        public float AccL { get; set; }
        public short? Lwp { get; set; }
        public short? Lwppay { get; set; }
        public decimal Gs { get; set; }
        public decimal? Gsmin { get; set; }
        public decimal? Gsdiff { get; set; }
        public decimal? Gsfinal { get; set; }
        public decimal Bs { get; set; }
        public decimal Hr { get; set; }
        public decimal Ma { get; set; }
        public decimal Trn { get; set; }
        public decimal Ab { get; set; }
        public decimal Adv { get; set; }
        public decimal Gp { get; set; }
        public decimal Pf { get; set; }
        public decimal GradeAmt { get; set; }
        public decimal Arrear { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal AttBonus { get; set; }
        public decimal HdayBonus { get; set; }
        public decimal Fbonus { get; set; }
        public decimal Pbonus { get; set; }
        public decimal OthersAddition { get; set; }
        public decimal OthersDeduct { get; set; }
        public decimal OtherAllow { get; set; }
        public decimal? TotalDeduct { get; set; }
        public decimal TotalAddition { get; set; }
        public decimal? TotalPayable { get; set; }
        public decimal NetSalary { get; set; }
        public decimal? NetSalaryPayable { get; set; }
        public byte? Cf { get; set; }
        public decimal Stamp { get; set; }
        public string Otdes { get; set; }
        public decimal Otrate { get; set; }
        public decimal Ot { get; set; }
        public float OtminTtl { get; set; }
        public float OthrTtl { get; set; }
        public float ExOtmin { get; set; }
        public float ExOthr { get; set; }
        public decimal ExOtamount { get; set; }
        public byte IsAllowPf { get; set; }
        public byte? IsReleased { get; set; }
        public short BankId { get; set; }
        public string BankAcNo { get; set; }
        public decimal ExchRate { get; set; }
        public decimal Pp { get; set; }
        public decimal Dd { get; set; }
        public decimal Bcl { get; set; }
        public decimal Bel { get; set; }
        public decimal Bsl { get; set; }
        public double? Trnd { get; set; }
        public decimal? Epf { get; set; }
        public short? MobileDeduction { get; set; }
        public short? MobileAllow { get; set; }
        public int? Cash { get; set; }
        public int? Bank1 { get; set; }
        public short? Lunch { get; set; }
        public short? LunchAmt { get; set; }
        public short? Night { get; set; }
        public short? NightAmt { get; set; }
        public int? Amount { get; set; }
        public int Tk1000 { get; set; }
        public int Tk500 { get; set; }
        public int Tk100 { get; set; }
        public int Tk50 { get; set; }
        public int Tk20 { get; set; }
        public int Tk10 { get; set; }
        public int Tk5 { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public decimal? Loan { get; set; }
        public byte? IsAllowGradeBns { get; set; }
        public decimal? IncenBns { get; set; }
        public decimal? Mlpay { get; set; }
        public short? Clh { get; set; }
        public short? Slh { get; set; }
        public decimal? Inc { get; set; }
        public decimal? Conf { get; set; }
        public decimal? Rvi { get; set; }
        public DateTime? DtJoin { get; set; }
        public float? Othrbuyer { get; set; }
        public decimal? OtamtBuyer { get; set; }
        public decimal? NetSalaryBuyer { get; set; }
        public short? TtlAbsent { get; set; }
        public decimal LoanBalance { get; set; }
        public float Elh { get; set; }
        public DateTime DtPayment { get; set; }
        public decimal Pfown { get; set; }
        public decimal Pfcom { get; set; }
        public decimal Pfprofit { get; set; }
        public string DtSalary { get; set; }
        public float CurrEl { get; set; }
        public float PrevEl { get; set; }
        public decimal Elamt { get; set; }
        public int Tk2 { get; set; }
        public int Tk1 { get; set; }
        public float WhdayOthr { get; set; }
        public decimal WhdayOtamt { get; set; }
        public float ServiceYear { get; set; }
        public float ServiceMonth { get; set; }
        public decimal ServiceBenefit { get; set; }
        public float ShiftTtl { get; set; }
        public decimal ShiftAmt { get; set; }
        public byte MngType { get; set; }
        public string PaymentType { get; set; }
        public float TotalOffDay { get; set; }
        public decimal OffDayAllowAmt { get; set; }
        public decimal ShiftAllowanceAmt { get; set; }
        public decimal ShiftIncenAmt { get; set; }
        public float WhdayAbsent { get; set; }
        public float TotalWorkDay { get; set; }
        public decimal OtallowAmt { get; set; }
        public decimal NoticePayDed { get; set; }
        public decimal TotalSalary { get; set; }
        public string Remarks { get; set; }
        public string UnitName { get; set; }
        public DateTime? DtPresentLast { get; set; }
        public decimal Gsusd { get; set; }
        public byte IsNewJoin { get; set; }
        public string Line { get; set; }
        public int BandSl { get; set; }
        public int LineSl { get; set; }
        public decimal FoodAllow { get; set; }
        public decimal ConvAllow { get; set; }
        public float BasicRate { get; set; }
        public float Spl { get; set; }
        public string TotalOt { get; set; }
        public string TotalOtb { get; set; }
        public float LateHr { get; set; }
        public decimal NetSalaryB { get; set; }
        public decimal TotaldeductB { get; set; }
        public decimal TotalpayableB { get; set; }
        public decimal NetSalaryPayableB { get; set; }
        public decimal BankPay { get; set; }
        public decimal CashPay { get; set; }
        public short PreLeave { get; set; }
        public short ProLeave { get; set; }
        public float WhdayP { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseRent { get; set; }
        public decimal MadicalAllow { get; set; }
        public decimal ConveyanceAllow { get; set; }
        public decimal DearnessAllow { get; set; }
        public decimal GasAllow { get; set; }
        public decimal PersonalPay { get; set; }
        public decimal ArrearBasic { get; set; }
        public decimal ArrearBonus { get; set; }
        public decimal WashingAllow { get; set; }
        public decimal SiftAllow { get; set; }
        public decimal ChargeAllow { get; set; }
        public decimal ContainSub { get; set; }
        public decimal ComPfCount { get; set; }
        public decimal EduAllow { get; set; }
        public decimal TiffinAllow { get; set; }
        public decimal CanteenAllow { get; set; }
        public decimal AttAllow { get; set; }
        public decimal FestivalBonus { get; set; }
        public decimal RiskAllow { get; set; }
        public decimal NightAllow { get; set; }
        public decimal PfAdd { get; set; }
        public decimal HrExp { get; set; }
        public decimal FesBonusDed { get; set; }
        public decimal Transportcharge { get; set; }
        public decimal TeliphoneCharge { get; set; }
        public decimal Taexpense { get; set; }
        public decimal SalaryAdv { get; set; }
        public decimal PurchaseAdv { get; set; }
        public decimal McloanDed { get; set; }
        public decimal HbloanDed { get; set; }
        public decimal PfloannDed { get; set; }
        public decimal WfloanLocal { get; set; }
        public decimal WfloanOther { get; set; }
        public decimal WpfloanDed { get; set; }
        public decimal MaterialLoanDed { get; set; }
        public decimal MiscDed { get; set; }
        public decimal AdvAgainstExp { get; set; }
        public decimal AdvFacility { get; set; }
        public decimal ElectricCharge { get; set; }
        public decimal Gascharge { get; set; }
        public decimal HazScheme { get; set; }
        public decimal Dishantenna { get; set; }
        public decimal RevenueStamp { get; set; }
        public decimal OwaSub { get; set; }
        public decimal DedIncBns { get; set; }
        public decimal DapEmpClub { get; set; }
        public decimal Moktab { get; set; }
        public decimal ChemicalForum { get; set; }
        public decimal DiplomaassoDed { get; set; }
        public decimal EnggassoDed { get; set; }
        public decimal Wfsub { get; set; }
        public decimal EduAlloDed { get; set; }
        public decimal PurChange { get; set; }
        public decimal ArrearInTaxDed { get; set; }
        public decimal OffWlfareAsso { get; set; }
        public decimal OfficeclubDed { get; set; }
        public decimal IncBonusDed { get; set; }
        public decimal Watercharge { get; set; }
        public decimal ChemicalAsso { get; set; }
        public decimal AdvInTaxDed { get; set; }
        public decimal ConvAllowDed { get; set; }
        public decimal DedIncBonusOf { get; set; }
        public decimal UnionSubDed { get; set; }
        public decimal EmpClubDed { get; set; }
        public decimal MedicalExp { get; set; }
        public decimal WagesaAdv { get; set; }
        public decimal MedicalLoanDed { get; set; }
        public decimal AdvWagesDed { get; set; }
        public decimal Wfl { get; set; }
        public decimal CheForum { get; set; }
        public decimal Donation { get; set; }
        public decimal HbloanBalance { get; set; }
        public decimal McloanBalance { get; set; }
        public decimal WflloanBalance { get; set; }
        public decimal PfloanBalance { get; set; }
        public decimal NetFoodAllow { get; set; }
        public float Fcttl { get; set; }
        public decimal NetOtamt { get; set; }
        public decimal HrexpOther { get; set; }
        public decimal OtherloannDed { get; set; }
        public decimal OtherLoanBalance { get; set; }
        public decimal DedIncBnsBal { get; set; }
        public decimal ElectricChargeOther { get; set; }
        public decimal GasChargeOther { get; set; }
        public decimal WaterChargeOther { get; set; }
        public decimal HbloanDedOther { get; set; }
        public decimal HbloanDedOtherBal { get; set; }
        public decimal AdvExploanBal { get; set; }
        public decimal DedIncBonusOfBal { get; set; }
        public decimal PfloanDedOther { get; set; }
        public decimal PfloanDedOtherBal { get; set; }
        public decimal WagesaAdvBal { get; set; }
        public decimal SalAdvBal { get; set; }
        public decimal WfloanOtherBal { get; set; }
        public bool IsOk { get; set; }
        public decimal UniFormDed { get; set; }
        public decimal MiscAddAllow { get; set; }
        public int EmpTypeId { get; set; }
        public decimal ArrearHrexp { get; set; }
        public decimal ArrearHrexpBal { get; set; }
        public decimal ArrearOt { get; set; }
        public decimal McloanDedOther { get; set; }
        public decimal McloanDedOtherBal { get; set; }
        public decimal PfloanRefund { get; set; }
        public decimal Bsded { get; set; }
        public decimal Hrded { get; set; }
        public decimal Maded { get; set; }
        public decimal RiskDed { get; set; }
        public decimal WashingAllowDed { get; set; }
        public decimal ShiftAllowDed { get; set; }
        public decimal ArrearGasExp { get; set; }
        public decimal ArrearElectricityExp { get; set; }
        public decimal ArrearGasExpBal { get; set; }
        public decimal ArrearElectricityExpBal { get; set; }
        public decimal Otstamp { get; set; }
        public decimal FesBonusDedBal { get; set; }
        public decimal HbloanDedOther1 { get; set; }
        public decimal HbloanDedOther2 { get; set; }
        public decimal HbloanDedOtherBal1 { get; set; }
        public decimal HbloanDedOtherBal2 { get; set; }
        public decimal PfloanDedOther1 { get; set; }
        public decimal PfloanDedOther2 { get; set; }
        public decimal PfloanDedOtherBal2 { get; set; }
        public decimal PfloanDedOtherBal1 { get; set; }
        public decimal TiffinAllowDed { get; set; }
        public decimal IncomeTaxRefund { get; set; }
        public decimal UniFormAdd { get; set; }
        public decimal DishantennaRefund { get; set; }
        public string OtherAddType { get; set; }
        public string OtherDedType { get; set; }
        public decimal ComPfRefund { get; set; }
        public int FiscalYearId { get; set; }
        public int FiscalMonthId { get; set; }
        public int Lid1 { get; set; }
        public int Lid2 { get; set; }
        public int Lid3 { get; set; }
        public int Hblid { get; set; }
        public int Hblid2 { get; set; }
        public int Hblid3 { get; set; }
        public int Mclid { get; set; }
        public int Pflid { get; set; }
        public int Pfllid { get; set; }
        public int Pfllid2 { get; set; }
        public int Pfllid3 { get; set; }
        public int WelfareLid { get; set; }
        public int Glid { get; set; }
        public decimal HbloanRefund { get; set; }
        public decimal AdvAgainstExpRefund { get; set; }
        public byte IsAllowOt { get; set; }
        public decimal ArrearHrexpOther { get; set; }
        public decimal ArrearHrexpOtherBal { get; set; }
        public decimal ArrearGasExpOther { get; set; }
        public decimal ArrearGasExpOtherBal { get; set; }
        public decimal ArrearElectricityExpOther { get; set; }
        public decimal ArrearElectricityExpOtherBal { get; set; }
        public decimal GasExpRefund { get; set; }
        public decimal ElectricityExpRefund { get; set; }
        public decimal HrexpRefund { get; set; }
        public decimal AdjustedItded { get; set; }
        public decimal VigilanceDutyAllow { get; set; }
        public decimal WaterExpRefund { get; set; }
        public decimal ShiftAllowDedBal { get; set; }
        public decimal AbLamt { get; set; }
        public short AbL { get; set; }
        public float WdayP { get; set; }
        public float HdayP { get; set; }
        public decimal GrossBank { get; set; }
        public decimal GrossCash { get; set; }
        public decimal TotalDeductCash { get; set; }
        public decimal CashPf { get; set; }
        public decimal TotalDeductBank { get; set; }
        public decimal BankPf { get; set; }
        public float ManualDay { get; set; }
        public byte? IsManual { get; set; }
    }
}
