using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSubStore
    {
        public TblStrSubStore()
        {
            TblStrSubStoreStock = new HashSet<TblStrSubStoreStock>();
        }

        public short SubStoreId { get; set; }
        public string SubStoreName { get; set; }
        public int? AssaignLuserId { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public short AId { get; set; }
        public DateTime? DtStockConfig { get; set; }
        public byte? Comid { get; set; }

        public virtual ICollection<TblStrSubStoreStock> TblStrSubStoreStock { get; set; }
    }
}
