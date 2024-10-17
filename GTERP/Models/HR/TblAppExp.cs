using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAppExp
    {
        public long AppId { get; set; }
        public string PrevCom { get; set; }
        public string PrevDesig { get; set; }
        public decimal PrevSalary { get; set; }
        public string Remarks { get; set; }
        public DateTime? DtFrom { get; set; }
        public DateTime? DtTo { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
