using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace GTERP.Models.LeaveVM
{
    public class HR_Leave_AvailVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LvId { get; set; }


        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Leave Type")]
        public int LTypeId { get; set; }
        [ForeignKey("LTypeId")]
        public virtual Cat_Leave_Type Cat_Leave_Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")] //, ApplyFormatInEditMode = true
        [Column(TypeName = "date")]

        public DateTime DtLvInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Column(TypeName = "date")]
        public DateTime DtFrom { get; set; }
        [Display(Name = "Leave Option")]
        public string? LeaveOption { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Column(TypeName = "date")]
        public DateTime? dtWork { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Column(TypeName = "date")]
        public DateTime DtTo { get; set; }
        public string InputType { get; set; }
        public float? TotalDay { get; set; }
        public float? TotalHour{ get; set; }

        [Display(Name = "Leave Type")]
        [StringLength(25)]
        [DataType("NVARCHAR(25)")]
        public string LvType { get; set; }

        public float? LvApp { get; set; }

        public int Status { get; set; } = 0;

        [Display(Name = "Leave Reason")]
        //[Required(ErrorMessage = "Please Provide Leave Reason.")]
        [StringLength(80)]
        public string Remark { get; set; }

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
        public DateTime? DtInput { get; set; }
        [NotMapped]
        public float? PreviousDay { get; set; }
        [NotMapped]
        public string PreviousType { get; set; }

        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string? FileName { get; set; }
        [NotMapped]
        public List<IFormFile> FileToUpload { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Column(TypeName = "date")]
        public DateTime? dtJoin { get; set; }
        [NotMapped]
        public string? LTypeName { get; set; }
    }
}
