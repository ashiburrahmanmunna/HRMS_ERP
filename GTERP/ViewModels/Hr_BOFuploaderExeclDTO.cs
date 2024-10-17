using GTERP.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace GTERP.ViewModels
{
    public class Hr_BOFuploaderExeclDTO
    {  
        public String EmpCode { get; set; }
        public String? EmpName { get; set; }
        public String? Department { get; set; }   
        public String? Designation { get; set; } 
        public double? FgDispatch1st { get; set; }
        public double? FgDispatch2nd { get; set; }
        public double? Glycerin { get; set; }
        public double? Unloading { get; set; }
        public double? TotalEarnde { get; set; }

        [Display(Name = "Company Id")]
        public string ComId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string UpdateByUserId { get; set; }

        public bool IsDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }
    }
}
