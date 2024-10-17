using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSubStrRcvMain
    {
        public TblStrSubStrRcvMain()
        {
            TblStrSubStrRcvSub = new HashSet<TblStrSubStrRcvSub>();
        }

        public byte ComId { get; set; }
        public long SubStrRcvId { get; set; }
        public string SubStrRcvNo { get; set; }
        public DateTime? DtDate { get; set; }
        public string Remarks { get; set; }
        public int? Srrid { get; set; }
        public int? IssuedId { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public byte IsPosted { get; set; }
        public int? SubSectIdto { get; set; }
        public short Status { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public int LuserIdVerify { get; set; }
        public DateTime? DtVerify { get; set; }
        public string RemarksVerify { get; set; }
        public int? LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public int? LuserIdcheckFor { get; set; }
        public int? SubStoreId { get; set; }

        public virtual ICollection<TblStrSubStrRcvSub> TblStrSubStrRcvSub { get; set; }
    }
}
