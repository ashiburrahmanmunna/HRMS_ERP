using GTERP.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{


    public class Budget_ProductionTarget:BaseModel
    {
        [Key]
        public int ProductionTargetId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int? PrdUnitId { get; set; }
        [ForeignKey("PrdUnitId")]
        public virtual PrdUnit PrdUnit { get; set; }



        [Display(Name = "Installed Capacity")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InstalledCapacity { get; set; }


        public DateTime FromDateProposed { get; set; }
        public DateTime ToDateProposed { get; set; }
        [Display(Name = "Proposed Budget")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProposedQty { get; set; }


        public DateTime FromDateRevised { get; set; }
        public DateTime ToDateRevised { get; set; }
        [Display(Name = "Revised Budget")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RevisedQty { get; set; }


        public DateTime FromDateEstimate { get; set; }
        public DateTime ToDateEstimate { get; set; }
        [Display(Name = "Estimate Budget")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimageQty { get; set; }


        public DateTime FromDateActual { get; set; }
        public DateTime ToDateActual { get; set; }
        [Display(Name = "Actual Production")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualQty { get; set; }

        public DateTime FromDateApproved { get; set; }
        public DateTime ToDateApproved { get; set; }
        [Display(Name = "Approved Budget")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ApprovedQty { get; set; }

        public DateTime FromDatePreviousActual { get; set; }
        public DateTime ToDatePreviousActual { get; set; }
        [Display(Name = "Previous Year Actual Production")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PreviousActualQty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "SalesRate")]
        public decimal SalesRate { get; set; }


        public bool isPost { get; set; }

        //[Display(Name = "Company Id")]
        //[StringLength(128)]
        //public string comid { get; set; }
        //[StringLength(128)]
        //public string userid { get; set; }

        [StringLength(128)]
        public string useridCheck { get; set; }

        [StringLength(128)]
        public string useridApprove { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

    }



}
