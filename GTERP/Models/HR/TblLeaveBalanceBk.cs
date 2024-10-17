using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLeaveBalanceBk
    {
        public long EmpId { get; set; }
        public DateTime DtOpBal { get; set; }
        public float Cl { get; set; }
        public float Acl { get; set; }
        public float Sl { get; set; }
        public float Asl { get; set; }
        public float El { get; set; }
        public float Ael { get; set; }
        public float Ml { get; set; }
        public float Aml { get; set; }
        public float Asp { get; set; }
        public float Aal { get; set; }
        public float Alwp { get; set; }
        public float Bel { get; set; }
        public float Arl { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public byte? ComId { get; set; }
        public float? Aaccl { get; set; }
    }
}
