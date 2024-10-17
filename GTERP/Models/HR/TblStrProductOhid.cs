using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProductOhid
    {
        public byte ComId { get; set; }
        public long PrdId { get; set; }
        public int TypeId { get; set; }
        public short ProdCatId { get; set; }
        public int ProdScatId { get; set; }
        public string PrdCode { get; set; }
        public string PrdCodeOld { get; set; }
        public int? PrdDeptId { get; set; }
        public string PrdName { get; set; }
        public string PrdDesc { get; set; }
        public short UnitId { get; set; }
        public short UnitIdrpt { get; set; }
        public byte IsNonInventory { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public long? AId { get; set; }
        public Guid WId { get; set; }
    }
}
