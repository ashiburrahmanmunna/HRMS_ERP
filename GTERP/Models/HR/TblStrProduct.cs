using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProduct
    {
        public TblStrProduct()
        {
            TblStrProductDistribution = new HashSet<TblStrProductDistribution>();
            TblStrProductDistributionBin = new HashSet<TblStrProductDistributionBin>();
            TblStrProductSupplier = new HashSet<TblStrProductSupplier>();
            TblStrReplaceSub2 = new HashSet<TblStrReplaceSub2>();
        }

        public byte ComId { get; set; }
        public long PrdId { get; set; }
        public int TypeId { get; set; }
        public short ProdCatId { get; set; }
        public int ProdScatId { get; set; }
        public string PrdCode { get; set; }
        public string PrdCodeOld { get; set; }
        public int? PrdDeptId { get; set; }
        public string PrdName { get; set; }
        public string PrdDesc { get; set; }
        public short UnitId { get; set; }
        public short UnitIdrpt { get; set; }
        public byte IsNonInventory { get; set; }
        public int LuserId { get; set; }
        public string Pcname { get; set; }
        public long? AId { get; set; }
        public Guid WId { get; set; }
        public double? CurrencyValue { get; set; }
        public int? CurrencyId { get; set; }
        public string Prdimage { get; set; }
        public string PartNo { get; set; }
        public byte IsFinishedGoods { get; set; }

        public virtual TblCatProdCategory ProdCat { get; set; }
        public virtual TblCatProdSubCategory ProdScat { get; set; }
        public virtual ICollection<TblStrProductDistribution> TblStrProductDistribution { get; set; }
        public virtual ICollection<TblStrProductDistributionBin> TblStrProductDistributionBin { get; set; }
        public virtual ICollection<TblStrProductSupplier> TblStrProductSupplier { get; set; }
        public virtual ICollection<TblStrReplaceSub2> TblStrReplaceSub2 { get; set; }
    }
}
