using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPrMain
    {
        public TblStrPrMain()
        {
            TblStrPrSub = new HashSet<TblStrPrSub>();
            TblStrWoMain = new HashSet<TblStrWoMain>();
        }

        public long Prid { get; set; }
        public string Prno { get; set; }
        public int SubSectId { get; set; }
        public DateTime DtPr { get; set; }
        public string Remarks { get; set; }
        public byte ComId { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public int? SubSectIdto { get; set; }
        public byte IsComplete { get; set; }
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
        public string PrType { get; set; }
        public string DocNo { get; set; }
        public string Potype { get; set; }
        public string BuyerName { get; set; }
        public string ItemNo { get; set; }
        public string Purpose { get; set; }
        public string SampleDetails { get; set; }
        public string Prrto { get; set; }
        public string MadeBy { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? DtShipDate { get; set; }
        public double Quantity { get; set; }
        public DateTime? DtPrtime { get; set; }

        public virtual ICollection<TblStrPrSub> TblStrPrSub { get; set; }
        public virtual ICollection<TblStrWoMain> TblStrWoMain { get; set; }
    }
}
