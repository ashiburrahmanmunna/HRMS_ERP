using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccFiscalYear
    {
        public TblAccFiscalYear()
        {
            TblAccFiscalHalfYear = new HashSet<TblAccFiscalHalfYear>();
            TblAccFiscalMonth = new HashSet<TblAccFiscalMonth>();
            TblAccFiscalQtr = new HashSet<TblAccFiscalQtr>();
        }

        public byte ComId { get; set; }
        public short Fyid { get; set; }
        public string Fyname { get; set; }
        public DateTime? OpDate { get; set; }
        public DateTime? ClDate { get; set; }
        public byte IsRunnig { get; set; }
        public byte IsWorking { get; set; }
        public byte RowNo { get; set; }

        public virtual ICollection<TblAccFiscalHalfYear> TblAccFiscalHalfYear { get; set; }
        public virtual ICollection<TblAccFiscalMonth> TblAccFiscalMonth { get; set; }
        public virtual ICollection<TblAccFiscalQtr> TblAccFiscalQtr { get; set; }
    }
}
