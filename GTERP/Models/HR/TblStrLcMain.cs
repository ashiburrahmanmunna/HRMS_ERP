using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrLcMain
    {
        public int Lcid { get; set; }
        public byte ComId { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public string Pino { get; set; }
        public double DocValue { get; set; }
        public DateTime DtDate { get; set; }
        public DateTime DtShip { get; set; }
        public string ShipMode { get; set; }
        public string ModeOfDoc { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
    }
}
