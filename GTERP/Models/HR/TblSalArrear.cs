using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalArrear
    {
        public int ComId { get; set; }
        public int ArId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtInput { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }

        public virtual TblCatCompany Com { get; set; }
    }
}
