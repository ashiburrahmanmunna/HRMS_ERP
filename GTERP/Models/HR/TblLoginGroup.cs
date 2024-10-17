using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginGroup
    {
        public TblLoginGroup()
        {
            TblLoginGroupSub = new HashSet<TblLoginGroupSub>();
        }

        public byte LgroupId { get; set; }
        public string LgroupName { get; set; }
        public int AId { get; set; }
        public Guid Wid { get; set; }

        public virtual ICollection<TblLoginGroupSub> TblLoginGroupSub { get; set; }
    }
}
