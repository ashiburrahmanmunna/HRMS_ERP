using GTERP.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Production
    {
        public int ProductionId { get; set; }
        [Display(Name = "Plant")]
        public int PrdUnitId { get; set; }
        public virtual PrdUnit Unit { get; set; }

        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }

        [Display(Name = "Fiscal Month")]
        public int FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }


        [Display(Name = "From Date")]
        public DateTime ProductionDate { get; set; } = DateTime.Now;

        [Display(Name = "To Date")]
        [NotMapped]
        public DateTime? ProductionToDate { get; set; } = DateTime.Now;

        [Display(Name = "Seed Qty")]
        public float? DailySeedQty { get; set; }

        [Display(Name = "Monthly Seed Qty")]
        public float? MonthlySeedQty { get; set; }

        [Display(Name = "Yearly Seed Qty")]
        public float? YearlySeedQty { get; set; }


        [Display(Name = "Sale Seed Qty")]
        public float? DailySalesSeedQty { get; set; }

        [Display(Name = "Monthly Sale Seed Qty")]
        public float? MonthlySalesSeedQty { get; set; }

        [Display(Name = "Yearly Sale Seed Qty")]
        public float? YearlySalesSeedQty { get; set; }



        [Display(Name = "Closing Seed Stock")]
        public float? ClosingSeedStock { get; set; }




        [Display(Name = "Daily Bag Qty")]
        public float? DailyBagQty { get; set; }

        [Display(Name = "Monthly Bag Qty")]
        public float? MonthlyBagQty { get; set; }

        [Display(Name = "Yearly Bag Qty")]
        public float? YearlyBagQty { get; set; }


        [Display(Name = "Daily Sale Bag Qty")]
        public float? DailySalesBagQty { get; set; }

        [Display(Name = "Monthly Sale Bag Qty")]
        public float? MonthlySalesBagQty { get; set; }

        [Display(Name = "Yearly Sale Bag Qty")]
        public float? YearlySalesBagQty { get; set; }


        [Display(Name = "Closing Bag Stock")]
        public float? ClosingBagStock { get; set; }



        [Display(Name = "DO Balance")]
        public float? DOBalance { get; set; }

        [Display(Name = "Phosphoric Acid Per Ton")]
        public float PhosphoricPerTon { get; set; } = 1.03F;
        [Display(Name = "Ammonia Acid Per Ton")]
        public float AmmoniaPerTon { get; set; } = 0.322F;
        [Display(Name = "Sulfuric Acid Per Ton")]
        public float SulfuricPerTon { get; set; } = 0.00F;
        [Display(Name = "Sand Per Ton")]
        public float SandPerTon { get; set; } = 0.00F;


        [Display(Name = "Phosphoric Acid Received")]
        [StringLength(300)]
        public string PhosphoricReceive { get; set; }

        [Display(Name = "Ammonia Acid Received")]
        [StringLength(300)]
        public string AmmoniaReceive { get; set; }

        [Display(Name = "Sulfuric Acid Received")]
        [StringLength(300)]
        public string SulfuricReceive { get; set; }

        [Display(Name = "Sand Received")]
        [StringLength(300)]
        public string SandReceive { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        //[Required]
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }
    }


    public class DownTime
    {
        public int DownTimeId { get; set; }

        [Display(Name = "Plant")]
        public int PrdUnitId { get; set; }
        public virtual PrdUnit Unit { get; set; }

        [Display(Name = "Reason")]
        public int ReasonId { get; set; }
        [ForeignKey("ReasonId")]
        public virtual DownTimeReason DownTimeReason { get; set; }

        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }

        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }


        [Display(Name = "Date")]

        public DateTime StrikeDate { get; set; } = DateTime.Now;

        [Display(Name = "From Time")]
        [DataType(DataType.Time)]
        public TimeSpan FromTime { get; set; }

        [Display(Name = "To Time")]
        [DataType(DataType.Time)]
        public TimeSpan ToTime { get; set; }

        [Display(Name = "Total Time")]
        [StringLength(10)]
        public string TotalDownTime { get; set; }


        //[Display(Name ="Production Loss")]
        //public float? ProductionLoss { get; set; }

        //[Display(Name ="Money Loss")]
        //public float? MoneyLoss { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        //[Required]
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string comid { get; set; }
        [StringLength(128)]
        public string userid { get; set; }
    }

    public class ProductionTargetSetup
    {
        [Key]
        public int PrdTargetSetId { get; set; }

        [Display(Name = "Plant")]
        public int PrdUnitId { get; set; }
        public virtual PrdUnit Unit { get; set; }

        //[Display(Name = "Product")]
        //public int? ProductId { get; set; }
        //public virtual Product Product { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }

        [Display(Name = "Production Capacity Year")]
        public float PrdCapacityYear { get; set; }



        [Display(Name = "Production Capacity Month")]
        public float? PrdCapacityMonth { get; set; }

        [Display(Name = "Target (Fiscal Year)")]
        public float? FiscalYearGoal { get; set; }

        [Display(Name = "Target (Fiscal Month)")]
        public float? FiscalMonthGoal { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }
    }

    public class SalesTargetSetup
    {
        [Key]
        public int SaleTargetSetId { get; set; }

        [Display(Name = "Plant")]
        public int PrdUnitId { get; set; }
        public virtual PrdUnit Unit { get; set; }

        //[Display(Name = "Product")]
        //public int? ProductId { get; set; }
        //public virtual Product Product { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }

        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }


        [Display(Name = "Target (Fiscal Year)")]
        public float FiscalYearGoal { get; set; }

        [Display(Name = "Target (Fiscal Month)")]
        public float? FiscalMonthGoal { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }
    }


    public class MonthlySalesProduction
    {
        [Key]
        public int MonthlySalesProductionId { get; set; }

        [Display(Name = "Plant")]
        public int PrdUnitId { get; set; }
        public virtual PrdUnit Unit { get; set; }


        [Display(Name = "Fiscal Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }

        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }


        [Display(Name = "Production Qty.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductionQty { get; set; }

        [Display(Name = "Production Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductionRate { get; set; }

        [Display(Name = "Production Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductionValue { get; set; }


        [Display(Name = "Sales Qty.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesQty { get; set; }

        [Display(Name = "Sales Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesRate { get; set; }

        [Display(Name = "Total Sales Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesValue { get; set; }


        [StringLength(300)]
        public string Remarks { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }
    }



    public class UseUtilities
    {
        [Key]
        public int UtilitiesId { get; set; }

        [Display(Name = "Plant")]
        public int PrdUnitId { get; set; }
        public virtual PrdUnit Unit { get; set; }

        //[Display(Name = "Product")]
        //public int ProductId { get; set; }
        //public virtual Product Product { get; set; }

        [Display(Name = "Raw Material")]
        public int RawMaterialId { get; set; }
        public virtual Product RawMaterial { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }

        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }

        [Display(Name = "Date")]
        public DateTime DtInput { get; set; } = DateTime.Now;

        [Display(Name = "Remarks")]
        [StringLength(300)]
        public string Remarks { get; set; }

        [Display(Name = "Standard Design Value")]
        public float? StdDesignVal { get; set; }

        [Display(Name = "Fiscal Year Budget Value")]
        public float? FiscalYearBudgetVal { get; set; }

        [Display(Name = "Consumption")]
        public float? Consumption { get; set; }

        [Display(Name = "SL No")]
        public int? SLNo { get; set; }



        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }
    }

    public class DownTimeReason
    {
        [Key]
        public int DownTimeReasonId { get; set; }

        [Display(Name = "Reason")]
        [StringLength(300)]
        public string Reason { get; set; }

        [Display(Name = "Reason Bangla")]
        [StringLength(300)]
        public string ReasonB { get; set; }

        [Display(Name = "SL No")]
        [StringLength(10)]
        public string SLNo { get; set; }

        [Display(Name = "SL No Bangla")]
        [StringLength(10)]
        public string SLNoB { get; set; }


        [Display(Name = "Date")]
        public DateTime DtInput { get; set; } = DateTime.Now;

        [Display(Name = "Remarks")]
        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }
    }

    public class RawMaterialStock
    {
        [Key]
        public int StockId { get; set; }

        [Display(Name = "Warehouse")]
        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        [Display(Name = "Receive Warehouse")]
        [NotMapped]
        public int? ReceiveWarehouseId { get; set; }
        //[NotMapped]
        //public virtual Warehouse ReceiveWarehouse { get; set; }



        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }

        [Display(Name = "Receive Qty")]
        public float? ReceiveQty { get; set; } = 0;

        [Display(Name = "Stock Qty")]
        public float? StockQty { get; set; } = 0;

        [Display(Name = "Receive Stock Qty")]
        [NotMapped]
        public float? ReceiveStockQty { get; set; } = 0;


        [Display(Name = "Send Qty")]
        public float? SendQty { get; set; } = 0;

        [Display(Name = "Date")]
        public DateTime DtInput { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }
    }

    public class BOMMain
    {
        [Key]
        public int BOMMainId { get; set; }

        [Display(Name = "Unit")]
        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Display(Name = "Assemble Product")]
        [StringLength(50)]
        public string AssembleName { get; set; }



        [StringLength(300)]
        public string Remarks { get; set; }

        public virtual List<BOMSub> BOMSubs { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Company Id")]
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }


    }

    public class BOMSub
    {
        [Key]
        public int BOMSubId { get; set; }

        public int BOMMainId { get; set; }
        [ForeignKey("BOMMainId")]
        public virtual BOMMain BOMMain { get; set; }


        [Display(Name = "Warehouse")]
        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        [Display(Name = "Product")]
        public int InvProductId { get; set; }
        [ForeignKey("InvProductId")]
        public virtual Product Product { get; set; }

        [Display(Name = "Consumption")]
        public float Consumption { get; set; }

        [Display(Name = "SL No")]
        public int? SLNo { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(128)]
        public string AddedbyUserId { get; set; }

        public DateTime? DateAdded { get; set; }

        [StringLength(128)]
        public string UpdatedbyUserId { get; set; }

        public DateTime? DateUpdated { get; set; }


        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }


    }

    public class PRcostEntry : BaseModel
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Vendor Name")]
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Cat_Unit Cat_Unit { get; set; }
        public String? EmpName { get; set; }
        public int CategoryId { get; set; }
        public double? SafetyShoe { get; set; } = 0.0;
        public double? Uniform { get; set; } = 0.0;
        public double? ServiceComission { get; set; } = 0.0;
        public double? MedicalCost { get; set; } = 0.0;

    }

    public class Hr_BOFuploader : BaseModel
    {
        [Key]
        public int id { get; set; }
        //[Display(Name = "Name")]
        public String EmpCode { get; set; }
        public String? EmpName { get; set; }

        //[ForeignKey("EmpCode")]
        //public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        [Display(Name = "Department")]
        public int DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Cat_Department Cat_Department { get; set; }

        [Display(Name = "Designation")]
        public int DesigId { get; set; }
        [ForeignKey("DesigId")]
        public virtual Cat_Designation Cat_Designation { get; set; }
        [Display(Name = "FG Dispatch Primary")]
        public double? FgDispatch1st { get; set; }

        [Display(Name = "FG Dispatch Customer")]
        public double? FgDispatch2nd { get; set; }


        [Display(Name = "FG Dispatch Glycerin")]
        public double? Glycerin { get; set; }
        public double? Unloading { get; set; }
        public double? TotalEarnde { get; set; }
    }

    public class ProductionCostEntry : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date")]
        public DateTime? CreateDate { get; set; }
        public double? Boundle { get; set; }
        public double? BoxPacketReel { get; set; }
        public double? Bags { get; set; }
        public double? Drum { get; set; }

    }

    public class DailyProductionRules : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public double? FgDisPatch1st { get; set; }
        public double? FgDispatch2nd { get; set; }
        public double? Glycerin { get; set; }
        public double? Boo2 { get; set; }
        public double? unloading { get; set; }
        public double? OtherCost { get; set; }
    }
    public class PSuploader : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public String EmpCode { get; set; }
        public String? EmpName { get; set; }
        [Display(Name = "Department")]
        public int DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Cat_Department Cat_Department { get; set; }

        [Display(Name = "Designation")]
        public int DesigId { get; set; }
        [ForeignKey("DesigId")]
        public virtual Cat_Designation Cat_Designation { get; set; }
        public DateTime? DtJoin { get; set; }
        public double? TotalPresent { get; set; }
        public double? TotalAbsent { get; set; }
        public double? BoxPacketReels { get; set; }
        public double? Drums { get; set; }
        public double? Bags { get; set; }
        public double? Unloads { get; set; }
        public double? GsWages { get; set; }
    }



}
