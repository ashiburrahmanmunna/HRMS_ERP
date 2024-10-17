using GTERP.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class User
    {

        public int UserId { get; set; }

        [Required]
        [Display(Name = "User Code")]
        public string UserCode { get; set; }


        [Required]
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }


        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


    }

    public partial class UserPermission : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }


        [Display(Name = "User")]
        [StringLength(128)]
        public string AppUserId { get; set; }

        [StringLength(200)]
        public string UserName { get; set; }


        [Display(Name = "Employee")]
        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public HR_Emp_Info HR_Emp_Info { get; set; }


        [Display(Name = "Is HR & Payroll")]
        public bool IsHRAndPayroll { get; set; }

        [Display(Name = "Is All")]
        public bool IsAll { get; set; }

        [Display(Name = "Is Store & Accounts")]
        public bool IsStoreAndAccounts { get; set; }

        [Display(Name = "Is Store")]
        public bool IsStore { get; set; }

        [Display(Name = "Is Accounts")]
        public bool IsGeneralAccouonts { get; set; }

        [Display(Name = "Is Cashbank Mangement")]
        public bool IsCashbankMangement { get; set; }

        [Display(Name = "Is Bill Management")]
        public bool IsBillManagement { get; set; }

        [Display(Name = "Is Medical")]
        public bool IsMedical { get; set; }

        [Display(Name = "Is Production")]
        public bool IsProduction { get; set; }

        [Display(Name = "Is InActive")]
        public bool IsInActive { get; set; }


        //[StringLength(128)]
        //public string ComId { get; set; }

        //[StringLength(128)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

    }
}