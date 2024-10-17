using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpTax
    {
        public DateTime DtDate { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public short SectId { get; set; }
        public short DesigId { get; set; }
        public string Grade { get; set; }
        public byte ComId { get; set; }
        public decimal? Gs { get; set; }
        public decimal? Bs { get; set; }
        public decimal? Hr { get; set; }
        public decimal? Ma { get; set; }
        public decimal? Conv { get; set; }
        public decimal? Oth { get; set; }
        public decimal? Pf { get; set; }
        public decimal? PyrBs { get; set; }
        public decimal? PyrHr { get; set; }
        public decimal? Pyrconv { get; set; }
        public decimal? LessHr { get; set; }
        public decimal? LessConv { get; set; }
        public decimal? LessPf { get; set; }
        public decimal? LessMa { get; set; }
        public decimal? NetPayF1 { get; set; }
        public decimal? Slv1 { get; set; }
        public decimal? Slv1P { get; set; }
        public decimal? Slv2 { get; set; }
        public decimal? Slv2P { get; set; }
        public decimal? Slv3 { get; set; }
        public decimal? Slv3P { get; set; }
        public decimal? Slv4 { get; set; }
        public decimal? Slv4P { get; set; }
        public decimal? Slv5 { get; set; }
        public decimal? Slv5P { get; set; }
        public decimal? TtlSlvP { get; set; }
        public decimal? InvPrcnt { get; set; }
        public decimal? RbtonInv { get; set; }
        public decimal? NetTxpayYr { get; set; }
        public decimal? InTxprMnth { get; set; }
        public int? PyrPf { get; set; }
    }
}
