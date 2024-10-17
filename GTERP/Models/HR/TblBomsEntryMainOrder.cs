using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBomsEntryMainOrder
    {
        public DateTime? InputDate { get; set; }
        public int Aid { get; set; }
        public int? OrdId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string FileName { get; set; }
        public byte? Status { get; set; }
        public int? Comid { get; set; }
        public int? Luserid { get; set; }
        public string PcName { get; set; }
        public Guid? Wid { get; set; }
        public int? BomRefId { get; set; }
        public double? OrdQty { get; set; }
        public int? Buyerid { get; set; }
        public string BomordNo { get; set; }
        public int? RowNo { get; set; }
    }
}
