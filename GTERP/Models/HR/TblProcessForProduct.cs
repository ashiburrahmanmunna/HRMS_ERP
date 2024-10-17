using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblProcessForProduct
    {
        public TblProcessForProduct()
        {
            TblProcessId = new HashSet<TblProcessId>();
        }

        public int ProcessForProductId { get; set; }
        public string ProcessForProductName { get; set; }
        public Guid? Wid { get; set; }

        public virtual ICollection<TblProcessId> TblProcessId { get; set; }
    }
}
