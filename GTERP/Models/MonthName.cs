using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class MonthName
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MonthNameId { get; set; }


        [Display(Name = "Month Name")]
        public string Name { get; set; }

        [Display(Name = "Short Month Name")]
        public string ShortName { get; set; }


        [Display(Name = "Month Bangla Name")]
        public string BanglaName { get; set; }


        [Display(Name = "Short Month Name Bangla")]
        public string ShortBanglaName { get; set; }



        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }


        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        public string ComId { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<City> vProducts { get; set; }
    }

    public class YearName
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int YearNameId { get; set; }


        [Display(Name = "Year Eng")]
        public int YearEng { get; set; }

        [Display(Name = "Year Bangla")]
        public string YearBng { get; set; }


        public string ComId { get; set; }


    }

}