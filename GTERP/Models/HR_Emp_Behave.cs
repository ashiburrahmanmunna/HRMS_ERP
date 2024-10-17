
using GTERP.Models.Base;
using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public partial class HR_Emp_Behave : BaseModel
    {
        //[StringLength(80)]
        //public string ComId { get; set; }

        [Key]
        public int BehaveId { get; set; }

        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info Employee { get; set; }

        //[Required]
        //[Display(Name = "Employee Code")]
        //public int EmpCode { get; set; }


        [Display(Name = "Notice Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dtNotice { get; set; }

        [Display(Name = "Event Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dtEvent { get; set; }

        [Required]
        public string NoticeType { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Violence Description")]
        public string ViolenceDesc { get; set; }
        public int ActionResult { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Decision")]
        public string Decision { get; set; }
        [StringLength(80)]
        public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }


    }
}
