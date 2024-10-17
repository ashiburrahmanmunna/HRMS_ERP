using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TempLedger
    {
        public byte ComId { get; set; }
        public long VoucherId { get; set; }
        public int AccId { get; set; }
        public decimal Tkdebit { get; set; }
        public decimal Tkcredit { get; set; }
        public int RowDr { get; set; }
        public int RowCr { get; set; }
        public int IsDebit { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public int? SubSectId { get; set; }
        public string Referance { get; set; }
        public decimal? TkdebitFc { get; set; }
        public decimal? TkcreditFc { get; set; }
        public string ReferanceTwo { get; set; }
        public string ReferanceThree { get; set; }
    }
}
