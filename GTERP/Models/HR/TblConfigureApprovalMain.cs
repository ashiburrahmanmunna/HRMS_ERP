using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblConfigureApprovalMain
    {
        public TblConfigureApprovalMain()
        {
            TblConfigureApprovalSub = new HashSet<TblConfigureApprovalSub>();
        }

        public int Aid { get; set; }
        public short ConfigId { get; set; }
        public DateTime DtConfigure { get; set; }
        public string ConfigFor { get; set; }
        public string Pcname { get; set; }
        public short LuserId { get; set; }
        public Guid WId { get; set; }
        public int? Comid { get; set; }

        public virtual ICollection<TblConfigureApprovalSub> TblConfigureApprovalSub { get; set; }
    }
}
