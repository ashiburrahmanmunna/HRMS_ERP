using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class InvoiceSub
    {
        [Required]
        [Key, Column(Order = 0)]
        public int InvoiceId { get; set; }

        //[Key, Column(Order = 1)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Package / Item")]
        public int SoftwarePackageId { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "Duration Month")]
        public int Qty { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }


        [Required]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }


        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime ActiveFromDate { get; set; }


        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime ActiveToDate { get; set; }

        public bool ActiveYesNo { get; set; }




        public virtual InvoiceMain InvoiceMain { get; set; }

        public virtual SoftwarePackage vsoftwarepackage { get; set; }

    }

    public class InvoiceTermsSub
    {

        [Key, Column(Order = 0)]
        public int InvoiceId { get; set; }

        //[Key, Column(Order = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms")]
        public string Terms { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public int RowNo { get; set; }

        public virtual InvoiceMain InvoiceMain { get; set; }

    }

    public class JobNo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobId { get; set; }


        [Display(Name = "Prefix")]
        public string Prefix { get; set; }


        [Display(Name = "Job No")]
        public string DocumentNo { get; set; }


        [Required]
        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InputDate { get; set; }

        public string ComId { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<City> vProducts { get; set; }
    }
}