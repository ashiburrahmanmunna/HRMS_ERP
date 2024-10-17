using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccTokenSub
    {
        public int VoucherId { get; set; }
        public int AccId { get; set; }
        public double Tkdebit { get; set; }
        public double Tkcredit { get; set; }
        public double TkdebitLocal { get; set; }
        public double TkcreditLocal { get; set; }
        public byte RowNo { get; set; }

        public virtual TblAccTokenMain Voucher { get; set; }
    }
}
