using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProductDistributionOhid
    {
        public long PrdId { get; set; }
        public long PrdDisId { get; set; }
        public string Spec { get; set; }
        public short ColorId { get; set; }
        public int SupplierId { get; set; }
        public decimal RateMin { get; set; }
        public decimal RateSales { get; set; }
        public decimal RateCgs { get; set; }
        public double Rol { get; set; }
        public double Roq { get; set; }
        public short Moq { get; set; }
        public double Qoh { get; set; }
        public double Qop { get; set; }
        public string Hscode { get; set; }
        public string VendorCode { get; set; }
        public double QtyLast { get; set; }
        public double RateLast { get; set; }
        public DateTime? DtPurLast { get; set; }
        public string Brand { get; set; }
        public string Remarks { get; set; }
        public int CountryId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
        public int RowNo { get; set; }
        public long? AId { get; set; }
        public Guid WId { get; set; }
        public string TranCode { get; set; }
        public short ArticleId { get; set; }
    }
}
