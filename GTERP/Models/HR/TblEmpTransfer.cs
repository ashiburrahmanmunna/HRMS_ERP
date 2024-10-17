using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpTransfer
    {
        public long AId { get; set; }
        public byte? ComId { get; set; }
        public long? EmpId { get; set; }
        public DateTime? DtTranDate { get; set; }
        public short? ShiftId { get; set; }
        public byte? ComIdTran { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
