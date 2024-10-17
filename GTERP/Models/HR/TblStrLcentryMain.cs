using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrLcentryMain
    {
        public TblStrLcentryMain()
        {
            TblStrInvoiceMain = new HashSet<TblStrInvoiceMain>();
            TblStrLcentrySub = new HashSet<TblStrLcentrySub>();
        }

        public int Lcid { get; set; }
        public byte ComId { get; set; }
        public string Lcno { get; set; }
        public DateTime DtDate { get; set; }
        public DateTime DtShip { get; set; }
        public DateTime DtExpire { get; set; }
        public int? ImportCountryId { get; set; }
        public int? PortId { get; set; }
        public int? ExporterComId { get; set; }
        public int? ImporterComid { get; set; }
        public int LcbankId { get; set; }
        public int LcincuranceId { get; set; }
        public string InsurancePoliceNo { get; set; }
        public string CoverNotes { get; set; }
        public decimal? Lcqty { get; set; }
        public int? UnitId { get; set; }
        public string PlusMinus { get; set; }
        public decimal? Lcamount { get; set; }
        public int? CurrencyId { get; set; }
        public byte? IsCloseLc { get; set; }
        public byte? IsCancelLc { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public int? LuserId { get; set; }
        public byte Iscomplete { get; set; }
        public short Status { get; set; }
        public int LuserIdApprove { get; set; }
        public DateTime? DtApprove { get; set; }
        public string RemarksApprove { get; set; }

        public virtual ICollection<TblStrInvoiceMain> TblStrInvoiceMain { get; set; }
        public virtual ICollection<TblStrLcentrySub> TblStrLcentrySub { get; set; }
    }
}
