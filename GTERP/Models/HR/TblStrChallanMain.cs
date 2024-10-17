using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrChallanMain
    {
        public byte ComId { get; set; }
        public long ChallanId { get; set; }
        public string ChallanNo { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtReport { get; set; }
        public string ChallanTo { get; set; }
        public string Address { get; set; }
        public string Cnfname { get; set; }
        public string DriverName { get; set; }
        public string TransPortNo { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public byte IsPosted { get; set; }
        public int? SubSectIdto { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public int LuserIdVerify { get; set; }
        public DateTime? DtVerify { get; set; }
        public string RemarksVerify { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public short Status { get; set; }
    }
}
