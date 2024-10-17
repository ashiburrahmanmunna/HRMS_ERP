using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesSubRefSub
    {
        public long Id { get; set; }
        public long SresfsubId { get; set; }
        public string RefName { get; set; }
        public string ContractPerson { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public int AId { get; set; }
    }
}
