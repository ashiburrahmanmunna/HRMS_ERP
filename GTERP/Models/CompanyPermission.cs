using GTERP.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{

    public class CompanyPermissionVM
    {
        [Key, Column(Order = 0)]

        public int isChecked { get; set; }
        [Column(Order = 1)]
        public int CompanyPermissionId { get; set; }
        [Column(Order = 2)]
        public string UserId { get; set; }
        [Column(Order = 3)]
        public string ComId { get; set; }
        [Column(Order = 4)]
        public string CompanyName { get; set; }
        [Column(Order = 5)]
        public int isDefault { get; set; }
    }

    public class ReportPermissionsVM
    {
        public int isChecked { get; set; }
        public int ReportPermissionsId { get; set; }
        public string UserId { get; set; }
        public string ComId { get; set; }
        public string ReportName { get; set; }
        public int ReportId { get; set; }
        public string ReportType { get; set; }
    }

    public class CompanyPermission : BaseModel
    {

        public int CompanyPermissionId { get; set; }

        //[Required]
        //[StringLength(128)]
        //public string ComId { get; set; }

        //public virtual Company vCompany { get; set; }
        public int isDefault { get; set; }
        public int isChecked { get; set; }
        //[Required]
        //[StringLength(128)]
        //public string UserId { get; set; }

    }

    public class ReportPermissions : BaseModel
    {

        public int ReportPermissionsId { get; set; }


        //[StringLength(128)]
        //public string ComId { get; set; }
        [ForeignKey("hr_reporttype")]
        public int ReportId { get; set; }

        public virtual HR_ReportType hr_reporttype { get; set; }
        public int isChecked { get; set; }
        //[Required]
        //[StringLength(128)]
        //public string UserId { get; set; }
    }


    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }



}