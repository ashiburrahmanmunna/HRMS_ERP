using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrDailyProductionMain
    {
        public TblStrDailyProductionMain()
        {
            TblStrDailyProductionSub = new HashSet<TblStrDailyProductionSub>();
        }

        public int DailyProductionId { get; set; }
        public byte ComId { get; set; }
        public string DocNo { get; set; }
        public DateTime DtDate { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public string RevNo { get; set; }

        public virtual ICollection<TblStrDailyProductionSub> TblStrDailyProductionSub { get; set; }
    }
}
