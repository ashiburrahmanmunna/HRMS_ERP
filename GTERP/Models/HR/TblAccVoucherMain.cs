using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAccVoucherMain
    {
        public TblAccVoucherMain()
        {
            TblAccVoucherChequeNo = new HashSet<TblAccVoucherChequeNo>();
            TblAccVoucherDis = new HashSet<TblAccVoucherDis>();
            TblAccVoucherSub = new HashSet<TblAccVoucherSub>();
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
        public string Source { get; set; }
        public long SourceId { get; set; }
        public float ConvRate { get; set; }
        public double VamountLocal { get; set; }
        public byte IsContra { get; set; }
        public int? LuserId { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }
        public int LuserIdCheck { get; set; }
        public DateTime? DtCheck { get; set; }
        public string RemarksCheck { get; set; }
        public string Referance { get; set; }
        public string ReferanceTwo { get; set; }
        public string ReferanceThree { get; set; }
        public long? Lcid { get; set; }

        public virtual ICollection<TblAccVoucherChequeNo> TblAccVoucherChequeNo { get; set; }
        public virtual ICollection<TblAccVoucherDis> TblAccVoucherDis { get; set; }
        public virtual ICollection<TblAccVoucherSub> TblAccVoucherSub { get; set; }
    }
}
