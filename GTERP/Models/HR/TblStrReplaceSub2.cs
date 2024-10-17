using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrReplaceSub2
    {
        public long RepId { get; set; }
        public long PrdId { get; set; }
        public string Spec { get; set; }
        public short ColorId { get; set; }
        public double Qty { get; set; }
        public string ActionCost { get; set; }
        public string ActionTrans { get; set; }
        public int UnitId { get; set; }
        public string ActionFrt { get; set; }
        public int Poid { get; set; }
        public DateTime? ActiondtShip { get; set; }
        public Guid WId { get; set; }
        public byte RowNo { get; set; }

        public virtual TblCatColor Color { get; set; }
        public virtual TblStrProduct Prd { get; set; }
        public virtual TblStrReplaceMain Rep { get; set; }
    }
}
