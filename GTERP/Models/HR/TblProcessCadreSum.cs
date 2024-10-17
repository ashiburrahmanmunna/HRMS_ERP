using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProcessCadreSum
    {
        public byte ComId { get; set; }
        public DateTime DtDate { get; set; }
        public string ProssType { get; set; }
        public short ComIdUnit { get; set; }
        public string UnitName { get; set; }
        public float Ideal { get; set; }
        public float Actual { get; set; }
        public float Var { get; set; }
        public string Remarks { get; set; }
        public string Spremarks { get; set; }
        public float Expat { get; set; }
        public float Total { get; set; }
        public bool? Approved { get; set; }
        public short? AppNumber { get; set; }
        public int? ApId1 { get; set; }
        public int? ApId2 { get; set; }
        public float Ml { get; set; }
    }
}
