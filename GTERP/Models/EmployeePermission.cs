using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class HR_Employee_VariablePermission
    {
        [Key]
        public int EmpPermissionId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "User")]
        [StringLength(128)]
        public string EmpSecretId { get; set; }

        [Display(Name = "Store Rate Check")]
        public bool IsStoreRateCheck { get; set; }

        [Display(Name = "Main Store Person")]
        public bool IsMainStorePerson { get; set; }

        [Display(Name = "Sub Store Person")]
        public bool IsSubStorePerson { get; set; }

    }
}
