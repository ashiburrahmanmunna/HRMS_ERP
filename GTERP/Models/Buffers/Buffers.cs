using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models.Buffers
{

    public partial class BufferRepresentative
    {

        public BufferRepresentative()
        {
            // BufferList = new HashSet<BufferList>();
        }

        public int BufferRepresentativeId { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string RepresentativeCode { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Representative Name")]
        public string RepresentativeName { get; set; }


        [Display(Name = "Representative Address")]

        public string RepresentativeAddress { get; set; }

        [Display(Name = "Mobile No")]

        public string RepresentativeMobile { get; set; }

        [StringLength(128)]
        public string comid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }



        public ICollection<RepresentativeBuffer> RepresentativeBuffer { get; set; }

    }

    public partial class BufferList
    {
        public int BufferListId { get; set; }
        [StringLength(80)]
        public string ComId { get; set; }

        [Required]
        [Display(Name = "Buffer Code")]
        public string BufferCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Buffer Name")]
        public string BufferName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Buffer Name Bangla")]
        public string BufferNameBangla { get; set; }




        //public ICollection<Cat_District> DistrictList { get; set; }
        public ICollection<RepresentativeBuffer> BufferRepresentativeList { get; set; }
        public ICollection<DistictBuffer> DistictBuffer { get; set; }




    }


    public class RepresentativeBuffer
    {
        public int Id { get; set; }
        public int BufferListId { get; set; }
        public BufferList BufferList { get; set; }
        public int BufferRepresentativeId { get; set; }
        public BufferRepresentative BufferRepresentative { get; set; }
    }


    public class DistictBuffer
    {
        public int Id { get; set; }
        public int BufferListId { get; set; }
        public BufferList BufferList { get; set; }
        public int DistId { get; set; }
        public Cat_District Cat_District { get; set; }
    }




    public partial class BuffertWiseBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BufferBookingId { get; set; }
        [Display(Name = "Fiscal Year")]

        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }
        [Display(Name = "Fiscal Month")]

        public int FiscalMonthId { get; set; }

        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }
        [Display(Name = "Buffer")]

        public int BufferID { get; set; }
        [ForeignKey("BufferID")]
        public virtual BufferList BufferList { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtInput { get; set; }
        public float Qty { get; set; }
        [NotMapped]
        [Column(TypeName = "decimal(18,2)")]

        public double PrevAllotmentQty { get; set; }

        public string AllotmentType { get; set; }


        [StringLength(80)]
        public string ComId { get; set; }

        public string PcName { get; set; }
        [DataType("NVARCHAR(128)")]
        public string UserId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

        // public virtual ICollection<Booking> Booking { get; set; }

        //public virtual ICollection<Cat_PoliceStation> Cat_PoliceStations { get; set; }
        //public virtual ICollection<Cat_Employee> Cat_Employee { get; set; }
    }


    public partial class RepresentativeBooking
    {
        public int RepresentativeBookingId { get; set; }
        /// public int BookingNo { get; set; }
        public int FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear YearName { get; set; }
        public int FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth MonthName { get; set; }
        public int BufferRepresentativeId { get; set; }
        [ForeignKey("BufferRepresentativeId")]
        public virtual BufferRepresentative BufferRepresentative { get; set; }
        public int BufferListId { get; set; }
        [ForeignKey("BufferListId")]
        public virtual BufferList BufferList { get; set; }
        public float AllotmentQty { get; set; }



        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }


        public int? BufferBookingId { get; set; }
        [ForeignKey("BufferBookingId")]
        public virtual BuffertWiseBooking BuffertWiseBooking { get; set; }
        public virtual ICollection<BufferDelOrder> BufferDeliveryOrder { get; set; }



    }


    public partial class BufferDelOrder
    {
        public BufferDelOrder()
        {

        }
        [Key]
        public int BufferDelOrderId { get; set; }
        public int DONo { get; set; } = 1;

        [NotMapped]
        public int OldDONo { get; set; }


        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime DODate { get; set; }


        public int RepresentativeBookingId { get; set; }
        [ForeignKey("RepresentativeBookingId")]
        public virtual RepresentativeBooking RepresentativeBooking { get; set; }

        public int? BufferRepresentativeId { get; set; }
        [ForeignKey("BufferRepresentativeId")]
        public virtual BufferRepresentative BufferRepresentative { get; set; }


        public int PayInSlipNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime PayInSlipDate { get; set; }
        public float Qty { get; set; }
        public float RemainingQty { get; set; }
        [NotMapped]
        public float CurrentRemainingQty { get; set; }

        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public string Remarks { get; set; }

        public string QtyInWordsEng { get; set; }
        public string QtyInWordsBng { get; set; }

        public string TotalPriceInWordsEng { get; set; }
        public string TotalPriceInWordsBng { get; set; }


        [StringLength(80)]
        public string ComId { get; set; }
        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }


        public virtual ICollection<BufferDelChallan> BufferDelChallan { get; set; }
    }



    public class BufferDelChallan
    {

        public BufferDelChallan()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BufferDelChallanId { get; set; }
        public int ChallanNo { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }
        public float DeliveryQty { get; set; }
        public int BufferDelOrderId { get; set; }
        [ForeignKey("BufferDelOrderId")]
        public virtual BufferDelOrder BufferDelOrder { get; set; }

        public virtual ICollection<BufferGatePassSub> BufferGatePassChallans { get; set; }


        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public string DODate { get; set; }
        [NotMapped]
        public string PayInSlipDate { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        [StringLength(128)]
        public string UpdateByUserId { get; set; }
    }

    public class BufferGatePass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BufferGatePassId { get; set; }
        public int GatePassNo { get; set; }
        public string GatePassFrom { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime GatePassDate { get; set; }

        [Required]
        public string TruckNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverMobile { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverMobile { get; set; }


        public int FiscalYearId { get; set; }
        public int FiscalMonthId { get; set; }
        public int? BufferId { get; set; }

        public float TotalQty { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        [StringLength(128)]
        public string UpdateByUserId { get; set; }

        public int Status { get; set; } = 0;

        public virtual ICollection<BufferGatePassSub> BufferChallans { get; set; }
    }

    public class BufferGatePassSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BufferGatePassSubId { get; set; }


        public int BufferGatePassId { get; set; }
        [ForeignKey("BufferGatePassId")]
        public virtual BufferGatePass BufferGatePass { get; set; }

        public int BufferDelChallanId { get; set; }
        [ForeignKey("BufferDelChallanId")]

        public virtual BufferDelChallan BufferDelChallan { get; set; }


        public float TruckLoadQty { get; set; }
        public float BalanceQty { get; set; }




        [NotMapped]
        public string DistName { get; set; }
        [NotMapped]
        public string FyName { get; set; }
        [NotMapped]
        public string RepresentativeName { get; set; }


    }





}
