using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrGrrreturnSub
    {
        public int Rtnid { get; set; }
        public long PrdDisId { get; set; }
        public double RtnQty { get; set; }
        public int Whid { get; set; }
        public int BinId { get; set; }
        public int RowNo { get; set; }
        public string Remarks { get; set; }
        public double OtherUnitRtnQty { get; set; }

        public virtual TblStrProductDistribution PrdDis { get; set; }
        public virtual TblStrGrrreturnMain Rtn { get; set; }
    }
}
