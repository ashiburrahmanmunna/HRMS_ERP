using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrGrrMain
    {
        public TblStrGrrMain()
        {
            TblStrGrrSub = new HashSet<TblStrGrrSub>();
            TblStrGrrTran = new HashSet<TblStrGrrTran>();
        }

        public byte ComId { get; set; }
        public long GrrId { get; set; }
        public string GrrNo { get; set; }
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
        public byte? IsBill { get; set; }
        public DateTime? DtBill { get; set; }
        public DateTime? DtTime { get; set; }

        public virtual ICollection<TblStrGrrSub> TblStrGrrSub { get; set; }
        public virtual ICollection<TblStrGrrTran> TblStrGrrTran { get; set; }
    }
}
