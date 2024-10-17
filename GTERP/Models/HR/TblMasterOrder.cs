using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblMasterOrder
    {
        public int Id { get; set; }
        public DateTime? DtShpDate { get; set; }
        public string OrdNo { get; set; }
        public double? TtlOrdQty { get; set; }
        public string RubberPpno { get; set; }
        public DateTime? DtSoleComp { get; set; }
        public string UpperPp { get; set; }
        public DateTime? DtOrdComp { get; set; }
        public string Rfidsts { get; set; }
        public string Remarks { get; set; }
        public byte ComId { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
    }
}
