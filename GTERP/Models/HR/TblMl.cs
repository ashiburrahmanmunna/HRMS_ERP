using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblMl
    {
        public long LvId { get; set; }
        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public short SectId { get; set; }
        public short DesigId { get; set; }
        public DateTime? DtInput { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime? DtTo { get; set; }
        public string InputType { get; set; }
        public double TotalDay { get; set; }
        public string LvType { get; set; }
        public double LvApp { get; set; }
        public decimal Gs { get; set; }
        public decimal Bs { get; set; }
        public string FirstSalMonth { get; set; }
        public string SecondSalMonth { get; set; }
        public string ThirdSalMonth { get; set; }
        public decimal FirstAmt { get; set; }
        public decimal SecondAmt { get; set; }
        public decimal ThirdAmt { get; set; }
        public float FirstDays { get; set; }
        public float SecondDays { get; set; }
        public float ThirdDays { get; set; }
        public float TtlDays { get; set; }
        public decimal FirstPayment { get; set; }
        public decimal LastPayment { get; set; }
        public decimal TtlAmount { get; set; }
        public decimal OtherBonus { get; set; }
        public decimal OtherDeduct { get; set; }
        public byte StampF { get; set; }
        public byte StampS { get; set; }
        public decimal NetPayable { get; set; }
        public byte FirstPaid { get; set; }
        public byte LastPaid { get; set; }
        public byte NoPay { get; set; }
        public string Remarks { get; set; }
        public int AId { get; set; }
        public int? LuserId { get; set; }
        public string Pcname { get; set; }
    }
}
