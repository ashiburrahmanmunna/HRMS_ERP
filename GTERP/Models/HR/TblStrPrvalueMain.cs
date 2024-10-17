using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrPrvalueMain
    {
        public TblStrPrvalueMain()
        {
            TblStrPrvalueSub = new HashSet<TblStrPrvalueSub>();
        }

        public int Prid { get; set; }
        public string Prno { get; set; }
        public int SubSectId { get; set; }
        public DateTime DtPr { get; set; }
        public double? VAmount { get; set; }
        public string VAmountInWords { get; set; }
        public string Remarks { get; set; }
        public byte ComId { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public int? SubSectIdto { get; set; }
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

        public virtual ICollection<TblStrPrvalueSub> TblStrPrvalueSub { get; set; }
    }
}
