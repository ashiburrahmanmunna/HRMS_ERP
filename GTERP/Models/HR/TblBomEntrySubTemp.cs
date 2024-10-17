using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBomEntrySubTemp
    {
        public int? ComId { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string PartNo { get; set; }
        public long PrdId { get; set; }
        public string PartName { get; set; }
        public long PrdDisId { get; set; }
        public string Revision { get; set; }
        public double? Quantity { get; set; }
        public string UnitofMeasure { get; set; }
        public string ProcurementType { get; set; }
        public string ReferanceDesignators { get; set; }
        public string Notes { get; set; }
        public int? UserId { get; set; }
    }
}
