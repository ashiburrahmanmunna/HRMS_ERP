using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebLeave
    {
        public int LeaveId { get; set; }
        public int EmpId { get; set; }
        public byte? LtypeId { get; set; }
        public string Loption { get; set; }
        public float Ldays { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int DeskLookId { get; set; }
        public string Reason { get; set; }
        public DateTime? EntryDate { get; set; }
        public byte Status { get; set; }
        public int RptId { get; set; }
        public DateTime? RptAppDate { get; set; }
        public int AppId { get; set; }
        public DateTime? AppAppDate { get; set; }
        public string DisapproveReason { get; set; }
        public byte? IsUpdate { get; set; }
        public string Ltype { get; set; }
    }
}
