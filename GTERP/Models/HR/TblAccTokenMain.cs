using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccTokenMain
    {
        public TblAccTokenMain()
        {
            TblAccTokenSub = new HashSet<TblAccTokenSub>();
        }

        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public string VoucherDesc { get; set; }
        public byte ComId { get; set; }
        public byte IsAutoEntry { get; set; }
        public byte IsPosted { get; set; }
        public short CountryId { get; set; }
        public short CountryIdLocal { get; set; }
        public double Vamount { get; set; }
        public string VamountInWords { get; set; }
        public float Rate { get; set; }
        public double VamountLocal { get; set; }

        public virtual ICollection<TblAccTokenSub> TblAccTokenSub { get; set; }
    }
}
