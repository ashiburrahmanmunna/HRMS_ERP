using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblConfigureApprovalSub
    {
        public int Aid { get; set; }
        public short ConfigId { get; set; }
        public long LuserIdEntry { get; set; }
        public long LuserIdCheck { get; set; }
        public long LuserIdVerify { get; set; }
        public long LuserIdApprove { get; set; }
        public int RowNo { get; set; }
        public Guid WId { get; set; }
        public int? Scomid { get; set; }

        public virtual TblConfigureApprovalMain A { get; set; }
    }
}
