using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginUserMenuMain
    {
        public TblLoginUserMenuMain()
        {
            TblLoginUserMenuSub = new HashSet<TblLoginUserMenuSub>();
        }

        public int UserMenuId { get; set; }
        public int UserId { get; set; }
        public DateTime DtDate { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }

        public virtual ICollection<TblLoginUserMenuSub> TblLoginUserMenuSub { get; set; }
    }
}
