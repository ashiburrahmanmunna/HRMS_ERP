using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrReplaceMain
    {
        public TblStrReplaceMain()
        {
            TblStrReplaceSub1 = new HashSet<TblStrReplaceSub1>();
            TblStrReplaceSub2 = new HashSet<TblStrReplaceSub2>();
        }

        public byte ComId { get; set; }
        public long RepId { get; set; }
        public DateTime? DtDate { get; set; }
        public string RepNo { get; set; }
        public int SupplierId { get; set; }
        public byte IsDefect { get; set; }
        public byte IsReplace { get; set; }
        public byte IsShortage { get; set; }
        public byte IsOthers { get; set; }
        public string Rcvd { get; set; }
        public DateTime? DtEtd { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }

        public virtual ICollection<TblStrReplaceSub1> TblStrReplaceSub1 { get; set; }
        public virtual ICollection<TblStrReplaceSub2> TblStrReplaceSub2 { get; set; }
    }
}
