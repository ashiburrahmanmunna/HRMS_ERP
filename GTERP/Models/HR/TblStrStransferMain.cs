using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrStransferMain
    {
        public long StransferId { get; set; }
        public string StransferNo { get; set; }
        public DateTime DtStransfer { get; set; }
        public string Remarks { get; set; }
        public int BuyerId { get; set; }
        public int ComId { get; set; }
        public int AId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public Guid WId { get; set; }
    }
}
