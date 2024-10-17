using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrTransactionMain
    {
        public TblStrTransactionMain()
        {
            TblStrTransactionSub = new HashSet<TblStrTransactionSub>();
        }

        public byte ComId { get; set; }
        public long TranId { get; set; }
        public string BasedOn { get; set; }
        public int BasedId { get; set; }
        public short LuserId { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }
        public int? OrdId { get; set; }

        public virtual ICollection<TblStrTransactionSub> TblStrTransactionSub { get; set; }
    }
}
