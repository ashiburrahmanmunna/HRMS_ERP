using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBomEntrySub
    {
        public int? Aid { get; set; }
        public int? Bomid { get; set; }
        public string Bomno { get; set; }
        public int? LevelId { get; set; }
        public string LevelName { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Revision { get; set; }
        public double? Quantity { get; set; }
        public string UnitofMeasure { get; set; }
        public string ProcurementType { get; set; }
        public string ReferanceDesignators { get; set; }
        public string Notes { get; set; }
        public int? Parent { get; set; }
        public int? Child { get; set; }
        public int? PrdidTwo { get; set; }
        public int? PrdidThree { get; set; }
        public int? PrdidFour { get; set; }
        public int? RowNo { get; set; }
        public int? Prddisid { get; set; }
        public int? Prdid { get; set; }

        public virtual TblBomEntryMain Bom { get; set; }
    }
}
