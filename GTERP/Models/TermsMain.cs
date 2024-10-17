using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class TermsMain
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int TermsId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms Name")]
        public string TermsName { get; set; }


        [StringLength(20)]
        [DataType(DataType.Text)]
        [Display(Name = "Remarks")]
        public string TermsRemarks { get; set; }


        [StringLength(128)]
        public string comid { get; set; }
        [StringLength(128)]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }



        public virtual ICollection<TermsSub> TermsSubs { get; set; }


    }


    public class TermsSub
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TermsSubId { get; set; }


        //[Key, Column(Order = 0)]
        public int TermsId { get; set; }

        ////[Key, Column(Order = 1)]
        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms")]
        public string Terms { get; set; }
        //public int ProductId { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Terms Description")]
        public string TermsDescription { get; set; }
        //public int ProductId { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Serial No")]
        public int TermsSerialNo { get; set; }

        [ForeignKey("TermsId")]

        public virtual TermsMain TermsMain { get; set; }


        public virtual ICollection<TermsSerialSub> TermsSerialSubs { get; set; }

    }


    public class TermsSerialSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TermsSerialSubId { get; set; }

        //[Key, Column(Order = 0)]
        public int TermsId { get; set; }

        ////[Key, Column(Order = 1)]
        public int TermsSubId { get; set; }

        ////[Key, Column(Order = 2)]
        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Terms Serial No")]
        public int TermsSerialNo { get; set; }
        //public int ProductId { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Serial No")]
        public int SerialNo { get; set; }

        [ForeignKey("TermsSubId")]

        public virtual TermsSub vTermsSub { get; set; }

    }
}