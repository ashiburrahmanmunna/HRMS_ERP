using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrLpMain
    {
        public TblStrLpMain()
        {
            TblStrLpSub = new HashSet<TblStrLpSub>();
            TblStrLpTran = new HashSet<TblStrLpTran>();
        }

        public byte ComId { get; set; }
        public long Lpid { get; set; }
        public string Lpno { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtReport { get; set; }
        public int SupplierId { get; set; }
        public string ChallanNo { get; set; }
        public string Igpno { get; set; }
        public string Receiver { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public byte IsPosted { get; set; }
        public int? SubSectIdto { get; set; }
        public int? BasedId { get; set; }
        public byte? IsBased { get; set; }
        public byte Status { get; set; }
        public DateTime? DtApprove { get; set; }
        public byte? SubSectId { get; set; }

        public virtual ICollection<TblStrLpSub> TblStrLpSub { get; set; }
        public virtual ICollection<TblStrLpTran> TblStrLpTran { get; set; }
    }
}
