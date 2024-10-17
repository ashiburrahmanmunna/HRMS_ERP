using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSrrMain
    {
        public TblStrSrrMain()
        {
            TblStrIssueMain = new HashSet<TblStrIssueMain>();
            TblStrSrrSub = new HashSet<TblStrSrrSub>();
        }

        public byte ComId { get; set; }
        public long SrrId { get; set; }
        public string SrrNo { get; set; }
        public int? SubSectId { get; set; }
        public long? OrdId { get; set; }
        public DateTime DtDate { get; set; }
        public byte IsComplete { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public int? SubSectIdto { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public byte? IsSubStore { get; set; }
        public int? SubStoreId { get; set; }
        public int? MachineId { get; set; }
        public int? BomTypeIdMain { get; set; }
        public int? PartialIndentQty { get; set; }
        public string ConfigType { get; set; }
        public int Bmid { get; set; }
        public int? ProcessId { get; set; }
        public DateTime? Dttime { get; set; }

        public virtual ICollection<TblStrIssueMain> TblStrIssueMain { get; set; }
        public virtual ICollection<TblStrSrrSub> TblStrSrrSub { get; set; }
    }
}
