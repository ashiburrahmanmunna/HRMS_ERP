using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TbltempEmpShift
    {
        public byte ComId { get; set; }
        public DateTime DtDate { get; set; }
        public long EmpId { get; set; }
        public short ShiftId { get; set; }
        public short ShiftIdR { get; set; }
        public string ShiftType { get; set; }
        public string ShiftCat { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
    }
}
