using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrProductDistribution
    {
        public TblStrProductDistribution()
        {
            TblStrArticleItemSizeSub = new HashSet<TblStrArticleItemSizeSub>();
            TblStrArticleItemSub = new HashSet<TblStrArticleItemSub>();
            TblStrBomItem = new HashSet<TblStrBomItem>();
            TblStrChemicalBatchSub = new HashSet<TblStrChemicalBatchSub>();
            TblStrFpTran = new HashSet<TblStrFpTran>();
            TblStrGrrAmtSub = new HashSet<TblStrGrrAmtSub>();
            TblStrGrrSub = new HashSet<TblStrGrrSub>();
            TblStrGrrTran = new HashSet<TblStrGrrTran>();
            TblStrGrrreturnSub = new HashSet<TblStrGrrreturnSub>();
            TblStrInventoryBatchSub = new HashSet<TblStrInventoryBatchSub>();
            TblStrIssueSub = new HashSet<TblStrIssueSub>();
            TblStrIssueTran = new HashSet<TblStrIssueTran>();
            TblStrIssueTranOtherUnit = new HashSet<TblStrIssueTranOtherUnit>();
            TblStrMasterPoCreateSub = new HashSet<TblStrMasterPoCreateSub>();
            TblStrMasterPoCreateSubInvoice = new HashSet<TblStrMasterPoCreateSubInvoice>();
            TblStrMasterPoItem = new HashSet<TblStrMasterPoItem>();
            TblStrPoSub = new HashSet<TblStrPoSub>();
            TblStrPrSub = new HashSet<TblStrPrSub>();
            TblStrProductDistributionBin = new HashSet<TblStrProductDistributionBin>();
            TblStrProductSupplier = new HashSet<TblStrProductSupplier>();
            TblStrPrvalueSub = new HashSet<TblStrPrvalueSub>();
            TblStrPuItem = new HashSet<TblStrPuItem>();
            TblStrReturnSub = new HashSet<TblStrReturnSub>();
            TblStrSrrSub = new HashSet<TblStrSrrSub>();
            TblStrSubStoreStock = new HashSet<TblStrSubStoreStock>();
            TblStrSupplierPoSub = new HashSet<TblStrSupplierPoSub>();
            TblStrTransactionSub = new HashSet<TblStrTransactionSub>();
            TblStrTransferSub = new HashSet<TblStrTransferSub>();
            TblStrWoSub = new HashSet<TblStrWoSub>();
        }

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
        public double? RateLastUsd { get; set; }
        public int? Currency { get; set; }
        public DateTime? DtPurLast { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public int CountryId { get; set; }
        public string Pcname { get; set; }
        public int? LuserId { get; set; }
        public int RowNo { get; set; }
        public long? AId { get; set; }
        public Guid WId { get; set; }
        public string TranCode { get; set; }
        public short ArticleId { get; set; }
        public byte? Dcomid { get; set; }
        public string PartNoSpec { get; set; }
        public double OtherUnitRateLast { get; set; }
        public string Gsm { get; set; }
        public string Thickness { get; set; }
        public string Inch { get; set; }
        public string SpecSize { get; set; }
        public double Rolthree { get; set; }
        public double Roltwo { get; set; }
        public double Rolsu { get; set; }
        public double RoltwoSu { get; set; }
        public double RolthreeSu { get; set; }
        public string Density { get; set; }
        public double MorderQty { get; set; }
        public double MrequireQty { get; set; }
        public string Mpono { get; set; }
        public short? UnitIdD { get; set; }
        public short? UnitIdrptD { get; set; }

        public virtual TblCatColor Color { get; set; }
        public virtual TblStrProduct Prd { get; set; }
        public virtual TblCatSupplier Supplier { get; set; }
        public virtual ICollection<TblStrArticleItemSizeSub> TblStrArticleItemSizeSub { get; set; }
        public virtual ICollection<TblStrArticleItemSub> TblStrArticleItemSub { get; set; }
        public virtual ICollection<TblStrBomItem> TblStrBomItem { get; set; }
        public virtual ICollection<TblStrChemicalBatchSub> TblStrChemicalBatchSub { get; set; }
        public virtual ICollection<TblStrFpTran> TblStrFpTran { get; set; }
        public virtual ICollection<TblStrGrrAmtSub> TblStrGrrAmtSub { get; set; }
        public virtual ICollection<TblStrGrrSub> TblStrGrrSub { get; set; }
        public virtual ICollection<TblStrGrrTran> TblStrGrrTran { get; set; }
        public virtual ICollection<TblStrGrrreturnSub> TblStrGrrreturnSub { get; set; }
        public virtual ICollection<TblStrInventoryBatchSub> TblStrInventoryBatchSub { get; set; }
        public virtual ICollection<TblStrIssueSub> TblStrIssueSub { get; set; }
        public virtual ICollection<TblStrIssueTran> TblStrIssueTran { get; set; }
        public virtual ICollection<TblStrIssueTranOtherUnit> TblStrIssueTranOtherUnit { get; set; }
        public virtual ICollection<TblStrMasterPoCreateSub> TblStrMasterPoCreateSub { get; set; }
        public virtual ICollection<TblStrMasterPoCreateSubInvoice> TblStrMasterPoCreateSubInvoice { get; set; }
        public virtual ICollection<TblStrMasterPoItem> TblStrMasterPoItem { get; set; }
        public virtual ICollection<TblStrPoSub> TblStrPoSub { get; set; }
        public virtual ICollection<TblStrPrSub> TblStrPrSub { get; set; }
        public virtual ICollection<TblStrProductDistributionBin> TblStrProductDistributionBin { get; set; }
        public virtual ICollection<TblStrProductSupplier> TblStrProductSupplier { get; set; }
        public virtual ICollection<TblStrPrvalueSub> TblStrPrvalueSub { get; set; }
        public virtual ICollection<TblStrPuItem> TblStrPuItem { get; set; }
        public virtual ICollection<TblStrReturnSub> TblStrReturnSub { get; set; }
        public virtual ICollection<TblStrSrrSub> TblStrSrrSub { get; set; }
        public virtual ICollection<TblStrSubStoreStock> TblStrSubStoreStock { get; set; }
        public virtual ICollection<TblStrSupplierPoSub> TblStrSupplierPoSub { get; set; }
        public virtual ICollection<TblStrTransactionSub> TblStrTransactionSub { get; set; }
        public virtual ICollection<TblStrTransferSub> TblStrTransferSub { get; set; }
        public virtual ICollection<TblStrWoSub> TblStrWoSub { get; set; }
    }
}
