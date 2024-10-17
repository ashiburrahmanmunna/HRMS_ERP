using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblOrderMain
    {
        public TblOrderMain()
        {
            TblOrderBreak = new HashSet<TblOrderBreak>();
        }

        public byte ComId { get; set; }
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? DtOrder { get; set; }
        public short CountryId { get; set; }
        public short? SeasonId { get; set; }
        public short BuyerId { get; set; }
        public string ItemName { get; set; }
        public int? Mlcid { get; set; }
        public string Mlcno { get; set; }
        public double Mlcamount { get; set; }
        public byte IsInactive { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }

        public virtual TblCatBuyer Buyer { get; set; }
        public virtual ICollection<TblOrderBreak> TblOrderBreak { get; set; }
    }
}
