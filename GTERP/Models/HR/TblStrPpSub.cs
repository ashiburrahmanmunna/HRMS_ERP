using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPpSub
    {
        public long Ppid { get; set; }
        public long Bmid { get; set; }
        public string Ppno { get; set; }
        public decimal Qty { get; set; }
        public Guid WId { get; set; }
        public short RowNo { get; set; }
        public DateTime? PrdStartDate { get; set; }
        public DateTime? PrdFinisDate { get; set; }
        public byte Iscomplete { get; set; }

        public virtual TblStrPpMain Pp { get; set; }
    }
}
