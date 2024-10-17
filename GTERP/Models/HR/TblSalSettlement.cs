using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalSettlement
    {
        public int? AEmpId { get; set; }
        public byte ComId { get; set; }
        public int RelId { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public DateTime? DtInput { get; set; }
        public string ProssType { get; set; }
        public string EmpType { get; set; }
        public short? DeptId { get; set; }
        public short SectId { get; set; }
        public short? SubSectId { get; set; }
        public short? DesigId { get; set; }
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
        public float Leave { get; set; }
        public short TtlPresent { get; set; }
        public float Late { get; set; }
        public float LateMin { get; set; }
        public float Sl { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public decimal Hr { get; set; }
        public decimal Ma { get; set; }
        public decimal OtherAllow { get; set; }
        public decimal Otrate { get; set; }
        public decimal Ot { get; set; }
        public float OthrTtl { get; set; }
        public decimal Trn { get; set; }
        public float LunchDay { get; set; }
        public decimal LunchAmt { get; set; }
        public decimal Arrear { get; set; }
        public decimal AttBonus { get; set; }
        public decimal MobileAllow { get; set; }
        public decimal Benifit { get; set; }
        public decimal OthersAddition { get; set; }
        public float TtlEl { get; set; }
        public decimal Elamount { get; set; }
        public decimal Gratuity { get; set; }
        public decimal Ab { get; set; }
        public decimal Dd { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal NoticeDeduct { get; set; }
        public decimal Adv { get; set; }
        public decimal Loan { get; set; }
        public decimal LoanBalance { get; set; }
        public decimal OthersDeduct { get; set; }
        public decimal MobileDeduction { get; set; }
        public decimal Stamp { get; set; }
        public decimal TotalDeduct { get; set; }
        public decimal TotalAddition { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal NetSalary { get; set; }
        public decimal NetSalaryPayable { get; set; }
        public decimal CashAmt { get; set; }
        public decimal BankAmt { get; set; }
        public byte IsReleased { get; set; }
        public int Amount { get; set; }
        public int Tk1000 { get; set; }
        public int Tk500 { get; set; }
        public int Tk100 { get; set; }
        public int Tk50 { get; set; }
        public int Tk20 { get; set; }
        public int Tk10 { get; set; }
        public int Tk5 { get; set; }
        public int Tk2 { get; set; }
        public int Tk1 { get; set; }
        public long AId { get; set; }
        public string Pcname { get; set; }
        public short LuserId { get; set; }
    }
}
