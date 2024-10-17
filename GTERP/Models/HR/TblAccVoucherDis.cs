using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccVoucherDis
    {
        public int Aid { get; set; }
        public int VoucherId { get; set; }
        public int Accid { get; set; }
        public int SubSectId { get; set; }
        public byte Rowno { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public double? Amount { get; set; }
        public Guid? Wid { get; set; }
        public int? SrowNo { get; set; }

        public virtual TblAccVoucherMain Voucher { get; set; }
    }
}
