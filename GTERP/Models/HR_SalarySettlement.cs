using GTERP.Models.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public partial class HR_SalarySettlement : BaseModel
    {
        [Key]
        public int StlId { get; set; }
        public int? aEmpId { get; set; }


        //[StringLength(100)]
        //public string ComId { get; set; }

        public int EmpId { get; set; }
        [DataType("VARCHAR(15)")]
        public string Month { get; set; }
        [DataType("smallint")]
        public int Year { get; set; }
        [Required]
        [DataType("VARCHAR(40)")]
        public string EmpCode { get; set; }
        //[Required]
        [DataType("VARCHAR(40)")]
        public string EmpType { get; set; }

        public DateTime? dtInput { get; set; }

        public DateTime? dtPayment { get; set; }
        [Required]
        [DataType("VARCHAR(40)")]
        public string ProssType { get; set; }

        [DisplayName("Join Date")]
        public DateTime? dtJoin { get; set; }
        [Display(Name = "Designation")]
        public int? DesigId { get; set; }
        [ForeignKey("DesigId")]
        public virtual Cat_Designation Cat_Designation { get; set; }
        [Display(Name = "Section")]
        public int? SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Cat_Section { get; set; }
        public int? DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Cat_Department Cat_Department { get; set; }
        [DataType("VARCHAR(40)")]
        // [Required]
        public string Band { get; set; }
        //[Required]
        [DataType("VARCHAR(40)")]
        public string Grade { get; set; }
        [DataType("VARCHAR(40)")]
        //[Required]
        public string PaySource { get; set; }
        [DataType("VARCHAR(40)")]
        //[Required]
        public string PayMode { get; set; }

        public float SerYear { get; set; }
        public float SerMonth { get; set; }
        public float SerDay { get; set; }
        public float WorkingDays { get; set; }
        public float Pday { get; set; }
        public float Dday { get; set; }
        public float Wday { get; set; }
        public float Hday { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float HalfDay { get; set; }
        public float TtlAbsent { get; set; }
        public float Late { get; set; }
        public float LateMin { get; set; }
        public float CL { get; set; }
        public float EL { get; set; }
        public float SL { get; set; }
        public float ML { get; set; }
        public float AccL { get; set; }
        public float LWP { get; set; }
        public float SPL { get; set; }
        public float CLH { get; set; }
        public float SLH { get; set; }
        public float ShortLeave { get; set; }
        public double ShortLeaveDay { get; set; }
        public float LWPPay { get; set; }

        public double GSUSD { get; set; }
        public double GS { get; set; }
        public double BS { get; set; }
        public double HR { get; set; }
        public double MA { get; set; }
        public double OTRate { get; set; }
        public float OTHrTtl { get; set; }
        public double OT { get; set; }
        public double AttBonus { get; set; }
        public double PBonus { get; set; }
        public double NoticePayAdd { get; set; }
        public double CompensationAdd { get; set; }
        public double Arrear { get; set; }
        public double IncenBns { get; set; }
        public double OthersAddition { get; set; }
        public double OtherAllow { get; set; }
        public double TrnAllow { get; set; }
        public double PFOwn { get; set; }
        public double PFComp { get; set; }
        public double PFProfit { get; set; }
        public double AB { get; set; }
        public double ADV { get; set; }
        public double PF { get; set; }
        public double Loan { get; set; }
        public double LoanBalance { get; set; }
        public double IncomeTax { get; set; }
        public double OthersDeduct { get; set; }
        public float Trnday { get; set; }
        public double TrnDed { get; set; }
        public double Stamp { get; set; }
        public double NoticePayDed { get; set; }
        public double SuspentionDed { get; set; }
        public double NetSalary { get; set; }
        public double TotalAddition { get; set; }
        public double TotalDeduct { get; set; }
        public double TotalPayable { get; set; }
        public double NetSalaryPayable { get; set; }
        public int CF { get; set; }
        public float WdayWork { get; set; }
        public double WDayBonus { get; set; }
        public double HDayWork { get; set; }
        public double HDayBonus { get; set; }
        [DataType("VARCHAR(40)")]
        public string OTDes { get; set; }
        public float OTMinTtl { get; set; }
        public int IsReleased { get; set; }
        public int BankId { get; set; }
        [DataType("VARCHAR(40)")]
        public string BankAcNo { get; set; }
        public double ExchRate { get; set; }
        public double DD { get; set; }
        public float PrevEL { get; set; }
        public double PrevELAmt { get; set; }
        public float CurrEL { get; set; }
        public double CurrELAmt { get; set; }
        public double MobileAllow { get; set; }
        public double MobileDeduction { get; set; }
        public int Lunch { get; set; }
        public double LunchAmt { get; set; }
        public int Night { get; set; }
        public double NightAmt { get; set; }
        public double MLPay { get; set; }
        public float LunchRamadan { get; set; }
        public float LeaveAdjust { get; set; }
        public int Amount { get; set; }
        public int TK1000 { get; set; }
        public int TK500 { get; set; }
        public int TK100 { get; set; }
        public int TK50 { get; set; }
        public int TK20 { get; set; }
        public int TK10 { get; set; }
        public int TK5 { get; set; }
        public int TK2 { get; set; }
        public int TK1 { get; set; }
        public double NetAmount { get; set; }
        public float TtlWorkDays { get; set; }
        public DateTime dtPresentLast { get; set; }
        public int IsPaid { get; set; }
        public float TotalDaysEL { get; set; }
        public float TotalPresentEL { get; set; }
        public float TotalWDayEL { get; set; }
        public float TotalHDayEL { get; set; }
        public float TotalAbsentEL { get; set; }
        public float TotalLeaveEL { get; set; }
        public double PFTotal { get; set; }
        public double NetPayableAmt { get; set; }
        public double Trn { get; set; }
        public float ROTHr { get; set; }
        public float ExOTHr { get; set; }
        public float ExOTHr1 { get; set; }
        public float ExOTHr2 { get; set; }
        public float ExOTMin { get; set; }
        public float NightDuty { get; set; }
        public double ExOTAmount { get; set; }
        public double ROTAmount { get; set; }
        public double ExOTAmount1 { get; set; }
        public double ExOTAmount2 { get; set; }
        public float MonthDays { get; set; }
        public float BCL { get; set; }
        public float BEL { get; set; }
        public float BSL { get; set; }
        public double NetSalaryB { get; set; }
        public double totalpayableB { get; set; }
        public double totaldeductB { get; set; }
        public double StampB { get; set; }
        public double NetSalaryBuyer { get; set; }
        [DataType("VARCHAR(50)")]
        public string Remarks { get; set; }
        public double SubsistenceAdd { get; set; }
        public double DeathFacilityAdd { get; set; }
        public double ServiceBenifitAdd { get; set; }
        [DataType("VARCHAR(10)")]
        public string TotalOT { get; set; }
        [DataType("VARCHAR(10)")]
        public string TotalOTB { get; set; }
        public float SuspentionDay { get; set; }
        public float NoticePayDedDay { get; set; }
        public float NoticePayDay { get; set; }
        public float ServiceBenifitDay { get; set; }
        public float CompensationDay { get; set; }
        public float SubsistenceDay { get; set; }
        public float DeathFacilityDay { get; set; }
        [DataType("VARCHAR(30)")]
        //[Required]
        public string SalaryType { get; set; }

        public DateTime? dtElEffective { get; set; }

        public double FoodAllow { get; set; }




    }
}
