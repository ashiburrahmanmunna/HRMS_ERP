using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblCustomerSecurity
    {
        public int Aid { get; set; }
        public int? CustId { get; set; }
        public decimal? Amount { get; set; }
        public string Notes { get; set; }
        public int? RowNo { get; set; }
        public Guid? Wid { get; set; }
        public byte? Comid { get; set; }
        public int? Userid { get; set; }
    }
}
