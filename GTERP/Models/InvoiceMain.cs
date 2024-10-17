using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class InvoiceMain
    {


        [Key]
        public int InvoiceId { get; set; }


        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "Billing Name")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }


        //[Required]
        [Display(Name = "Invoice Referance")]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]

        public string InvoiceReferance { get; set; } = "=N/A=";

        [Display(Name = "Total Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlSumAmt { get; set; }

        [Display(Name = "Net Amount")]

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }
        [Display(Name = "Paid Amount")]


        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmt { get; set; }
        [Display(Name = "Due Amount")]


        [Column(TypeName = "decimal(18,2)")]
        public decimal DueAmt { get; set; }

        [Display(Name = "Currency")]


        public int CountryId { get; set; }


        //[Required]
        [StringLength(128)]
        public string ComId { get; set; }


        [Required]
        [StringLength(128)]
        public string userid { get; set; }


        public Boolean isPost { get; set; }
        [Display(Name = "Posted")]

        public virtual ICollection<InvoiceSub> InvoiceSubs { get; set; }

        public virtual ICollection<InvoiceTermsSub> InvoiceTermsSubs { get; set; }



    }
}