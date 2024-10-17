using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GTERP.Models.Base;

namespace GTERP.Models
{
    public class GatePassMs : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GatePassID { get; set; }
        [Display(Name = "GatePass No")]
        public short? GatePassNo { get; set; }
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info Employee { get; set; }
        //public string EmpCode { get; set; }
        //public string EmpName { get; set; }
        [ForeignKey("DeptId")]
        public int DeptId { get; set; }
        public virtual Cat_Department Department { get; set; }
        [ForeignKey("SectId")]
        public int SectId { get; set; }
        public virtual Cat_Section Section { get; set; }

        [Display(Name = "Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        [Display(Name = "Request Time")]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]        
        public DateTime? RequestTime { get; set; }
        [Required(ErrorMessage ="Please Select Request Type.")]
        public string RequestType { get; set; }

        [Display(Name = "In Time")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = @"{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? TimeIn { get; set; }

        [Display(Name = "Out Time")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = @"{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? TimeOut { get; set; }

        [Display(Name = "Total Duration")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = @"{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Duration { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Reasone Description")]
        public string Reasone { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string RequestBy { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsApproved { get; set; }
        public bool IsConfirmed { get; set; }
        [StringLength(80)]
        public string ComId { get; set; }
        public string PcName { get; set; }
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
