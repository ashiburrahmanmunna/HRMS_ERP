using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{

    public partial class COM_MachinaryLC_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MachinaryLCId { get; set; }
        [Required]
        [Display(Name = "MachinaryLC NO")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(100)]
        //[Index(IsUnique = true)]
        public string MachinaryLCNo { get; set; }

        [Display(Name = "UD NO")]
        public string UDNo { get; set; }

        [Display(Name = "Concern")]
        public Nullable<int> CommercialCompanyId { get; set; }

        [Display(Name = "ShipMode")]
        public Nullable<int> ShipModeId { get; set; }

        [Display(Name = "Total Value")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal TotalValue { get; set; }
        [Display(Name = "Tenor")]
        [Column(TypeName = "decimal(18,2)")]

        public Nullable<decimal> Tenor { get; set; }
        [Display(Name = "Payment Term")]
        public string PaymentTerm { get; set; }

        [Display(Name = "Payment Term")]
        public Nullable<int> PaymentTermsId { get; set; }
        public virtual PaymentTerms PaymentTermss { get; set; }


        [Display(Name = "Day List Term")]
        public Nullable<int> DayListId { get; set; }
        public virtual DayList daylists { get; set; }


        public string Insurance { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "Port Of Loading")]
        public int PortOfLoadingId { get; set; }
        [Display(Name = "Port Of Discharge")]
        public Nullable<int> PortOfDischargeId { get; set; }


        [Display(Name = "Concern Bank")]
        public Nullable<int> OpeningBankId { get; set; }

        [Display(Name = "Supplier Bank")]
        public Nullable<int> LienBankId { get; set; }

        [Display(Name = "Incoterm")]
        public Nullable<int> TradeTermId { get; set; }
        public virtual TradeTerm TradeTerms { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "LC Opening Date")]
        public DateTime LcOpeningDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "UD Date")]
        public Nullable<DateTime> UDDate { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "First Shipment Date")]
        public Nullable<DateTime> FirstShipmentDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Shipment Date")]
        public Nullable<DateTime> LastShipmentDate { get; set; }


        [Display(Name = "Final Destination")]
        public int DestinationID { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "ConvertRate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConvertRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "MachinaryLC Value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MachinaryLCValue { get; set; }


        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Increase")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal IncreaseValue { get; set; }

        [Display(Name = "Decrease")]

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DecreaseValue { get; set; }
        [Display(Name = "Final Value")]

        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetValue { get; set; }




        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Margin [%]")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GroupLCAverage { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }


        [Display(Name = "MachinaryLC Print Doc Ref")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [StringLength(50)]

        public string MachinaryLCPrintDocRef { get; set; }

        [Display(Name = "MachinaryLC Print Doc Date")]
        public Nullable<System.DateTime> MachinaryLCPrintDocDate { get; set; }

        public virtual SisterConcernCompany Company { get; set; }

        [Display(Name = "Supplier")]
        public Nullable<int> SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierInformation SupplierInformation { get; set; }
        public virtual ShipMode ShipMode { get; set; }


        //public virtual COM_MasterLC COM_MasterLC { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual PortOfLoading PortOfLoading { get; set; }
        public virtual PortOfDischarge PortOfDischarge { get; set; }

        public virtual Destination Destination { get; set; }

        public virtual OpeningBank OpeningBank { get; set; }
        public virtual LienBank LienBank { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COM_MachinaryLC_Details> COM_MachinaryLC_Details { get; set; }
        //public virtual ICollection<COM_CommercialInvoice> COM_CommercialInvoice { get; set; }

        public string ApprovalPerson { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> DateApproval { get; set; }
        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        [Display(Name = "Remarks")]

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Item Group")]
        public Nullable<int> ItemGroupId { get; set; }

        public virtual ItemGroup ItemGroups { get; set; }

    }

    public partial class COM_MachinaryLC_Details
    {
        [Key, Column(Order = 0)]
        public int MachinaryLCId { get; set; }
        //[Key, Column(Order = 1)]

        public int PIId { get; set; }

        public string Addedby { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Updatedby { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        [Display(Name = "Master MachinaryLC")]


        public virtual COM_MachinaryLC_Master COM_MachinaryLC_Master { get; set; }

        [Display(Name = "Proforma Invoice")]

        public virtual COM_ProformaInvoice COM_ProformaInvoice { get; set; }
        //public virtual ICollection<COM_GroupLC_Main> COM_GroupLC_Main { get; set; }
    }


}