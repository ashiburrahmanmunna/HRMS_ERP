using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAttFixMonthly
    {
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public short DesigId { get; set; }
        public short SectId { get; set; }
        public short DeptId { get; set; }
        public string EmpType { get; set; }
        public byte MngType { get; set; }
        public string Grade { get; set; }
        public DateTime SalMonth { get; set; }
        public string ProssType { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public decimal Hr { get; set; }
        public float WorkingDays { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float Dday { get; set; }
        public float TtlAbsent { get; set; }
        public float Late { get; set; }
        public float Cl { get; set; }
        public float El { get; set; }
        public float Sl { get; set; }
        public float Ml { get; set; }
        public float Lwp { get; set; }
        public decimal Lwppay { get; set; }
        public float Accl { get; set; }
        public float Wday { get; set; }
        public float Hday { get; set; }
        public decimal Dd { get; set; }
        public decimal Ab { get; set; }
        public decimal Adv { get; set; }
        public decimal Loan { get; set; }
        public decimal OthersDeduct { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal TotalDeduct { get; set; }
        public decimal OthersAddition { get; set; }
        public decimal Arrear { get; set; }
        public float OthrTtl { get; set; }
        public decimal Ot { get; set; }
        public decimal OtherAllow { get; set; }
        public decimal Trn { get; set; }
        public decimal MobileAllow { get; set; }
        public decimal Pf { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal NetSalaryPayable { get; set; }
        public byte Cf { get; set; }
        public string EditYn { get; set; }
        public byte IsReleased { get; set; }
    }
}
