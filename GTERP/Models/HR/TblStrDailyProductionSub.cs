using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrDailyProductionSub
    {
        public int DailyProductionId { get; set; }
        public int MachineId { get; set; }
        public int? Doid { get; set; }
        public string Brand { get; set; }
        public int? Prddisid { get; set; }
        public string Impression { get; set; }
        public double? Qty { get; set; }
        public string BoardSize { get; set; }
        public string BoardWidth { get; set; }
        public string BoardHeight { get; set; }
        public string BoardLength { get; set; }
        public string Remarks { get; set; }
        public string ReelSize { get; set; }
        public string Size { get; set; }
        public byte? IsWax { get; set; }
        public string FluiteSize { get; set; }
        public int? BusinessUnitId { get; set; }
        public string DospecManual { get; set; }
        public byte? DorowNo { get; set; }
        public byte? IsFinished { get; set; }
        public DateTime? Starttime { get; set; }
        public DateTime? Endtime { get; set; }
        public int? Slno { get; set; }

        public virtual TblStrDailyProductionMain DailyProduction { get; set; }
    }
}
