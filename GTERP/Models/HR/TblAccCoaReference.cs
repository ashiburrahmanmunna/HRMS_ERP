using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccCoaReference
    {
        public int Slno { get; set; }
        public byte ComId { get; set; }
        public int AccId { get; set; }
        public short RefId { get; set; }
        public double OpDebit { get; set; }
        public double OpCredit { get; set; }
        public string Flag { get; set; }
        public Guid WId { get; set; }
        public short CountryId { get; set; }
        public short CountryIdLocal { get; set; }
        public float Rate { get; set; }
        public double OpDebitLocal { get; set; }
        public double OpCreditLocal { get; set; }
        public DateTime OpDate { get; set; }
        public short OpFyid { get; set; }
    }
}
