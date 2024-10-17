using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrGrrAmtMain
    {
        public TblStrGrrAmtMain()
        {
            TblStrGrrAmtSub = new HashSet<TblStrGrrAmtSub>();
        }

        public byte ComId { get; set; }
        public long GrrAmtId { get; set; }
        public string GrrAmtNo { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtReport { get; set; }
        public int SupplierId { get; set; }
        public string Receiver { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public byte IsPosted { get; set; }
        public int? SubSectIdto { get; set; }
        public string BasedOn { get; set; }
        public short Status { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public int LuserIdVerify { get; set; }
        public DateTime? DtVerify { get; set; }
        public string RemarksVerify { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public int? LuserIdcheckFor { get; set; }
        public long? BasedId { get; set; }
        public double? DisPer { get; set; }
        public double? Discount { get; set; }
        public double? NetTtlAmount { get; set; }
        public string InWords { get; set; }

        public virtual ICollection<TblStrGrrAmtSub> TblStrGrrAmtSub { get; set; }
    }
}
