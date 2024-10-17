using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccBillRcvdData
    {
        public int RcvId { get; set; }
        public long BillId { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public byte RowNo { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public int TermsId { get; set; }

        public virtual TblAccBillRcvdInfo Rcv { get; set; }
    }
}
