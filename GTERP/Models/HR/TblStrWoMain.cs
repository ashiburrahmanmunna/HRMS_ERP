using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrWoMain
    {
        public TblStrWoMain()
        {
            TblStrWoSub = new HashSet<TblStrWoSub>();
            TblStrWoTerms = new HashSet<TblStrWoTerms>();
        }

        public byte ComId { get; set; }
        public long Woid { get; set; }
        public string Wono { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtEdt { get; set; }
        public string Remarks { get; set; }
        public long Prid { get; set; }
        public int? SupplierId { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public decimal? DisPer { get; set; }
        public decimal? DisAmt { get; set; }
        public decimal? Vamount { get; set; }
        public string VamountInWords { get; set; }
        public decimal? DollarRate { get; set; }
        public decimal? Usdamt { get; set; }
        public string PayTerms { get; set; }
        public int? SubSectIdto { get; set; }
        public byte IsComplete { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public short ShipInfoId { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public int? InvoiceId { get; set; }
        public DateTime? DtTime { get; set; }
        public string Origin { get; set; }
        public string Indenter { get; set; }

        public virtual TblStrPrMain Pr { get; set; }
        public virtual ICollection<TblStrWoSub> TblStrWoSub { get; set; }
        public virtual ICollection<TblStrWoTerms> TblStrWoTerms { get; set; }
    }
}
