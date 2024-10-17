using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblSalesReference
    {
        public short SrefId { get; set; }
        public string SrefCode { get; set; }
        public string SrefName { get; set; }
        public string ContPerson { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public short AId { get; set; }
        public string Pcname { get; set; }
        public int LuserId { get; set; }
        public Guid WId { get; set; }
        public byte? Comid { get; set; }
        public DateTime? DtInput { get; set; }
        public string Remarks { get; set; }
    }
}
