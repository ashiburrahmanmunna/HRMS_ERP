using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTexinfo
    {
        public int? AEmpId { get; set; }
        public DateTime? DtInput { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public int? DesigId { get; set; }
        public string VEmpDesig { get; set; }
        public string VEmpSec { get; set; }
        public int? SectId { get; set; }
        public string VEmpSex { get; set; }
        public byte? ComId { get; set; }
        public string CompName { get; set; }
        public double? Gs { get; set; }
        public double? Bs { get; set; }
        public double? Hr { get; set; }
        public double? Ma { get; set; }
        public double? Conv { get; set; }
        public double? Oth { get; set; }
        public double? Pf { get; set; }
        public double? PyrBs { get; set; }
        public double? PyrHr { get; set; }
        public double? Pyrconv { get; set; }
        public double? LessHr { get; set; }
        public double? LessConv { get; set; }
        public double? LessPf { get; set; }
        public double? LessMa { get; set; }
        public double? NetPayF1 { get; set; }
        public double? Slv1 { get; set; }
        public double? Slv1P { get; set; }
        public double? Slv2 { get; set; }
        public double? Slv2P { get; set; }
        public double? Slv3 { get; set; }
        public double? Slv3P { get; set; }
        public double? Slv4 { get; set; }
        public double? Slv4P { get; set; }
        public double? Slv5 { get; set; }
        public double? Slv5P { get; set; }
        public double? TtlSlvP { get; set; }
        public double? InvPrcnt { get; set; }
        public double? RbtonInv { get; set; }
        public double? NetTxpayYr { get; set; }
        public double? InTxprMnth { get; set; }
        public int? Pyrpf { get; set; }
    }
}
