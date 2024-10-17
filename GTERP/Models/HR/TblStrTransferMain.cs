using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrTransferMain
    {
        public TblStrTransferMain()
        {
            TblStrTransferSub = new HashSet<TblStrTransferSub>();
        }

        public long TransferId { get; set; }
        public string TransferNo { get; set; }
        public DateTime DtTransfer { get; set; }
        public int ComId { get; set; }
        public int AId { get; set; }
        public string Remarks { get; set; }
        public byte IsPosted { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }

        public virtual ICollection<TblStrTransferSub> TblStrTransferSub { get; set; }
    }
}
