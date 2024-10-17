using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrWorkingScheduleMain
    {
        public TblStrWorkingScheduleMain()
        {
            TblStrWorkingScheduleSub = new HashSet<TblStrWorkingScheduleSub>();
        }

        public int WorkingScheduleId { get; set; }
        public byte ComId { get; set; }
        public string DocNo { get; set; }
        public DateTime DtDate { get; set; }
        public string Remarks { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }

        public virtual ICollection<TblStrWorkingScheduleSub> TblStrWorkingScheduleSub { get; set; }
    }
}
