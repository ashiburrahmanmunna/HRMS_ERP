using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginGroupSub
    {
        public TblLoginGroupSub()
        {
            TblLoginUser = new HashSet<TblLoginUser>();
        }

        public short LsubGroupId { get; set; }
        public string LsubGroupName { get; set; }
        public byte LgroupId { get; set; }
        public int AId { get; set; }
        public Guid Wid { get; set; }

        public virtual TblLoginGroup Lgroup { get; set; }
        public virtual ICollection<TblLoginUser> TblLoginUser { get; set; }
    }
}
