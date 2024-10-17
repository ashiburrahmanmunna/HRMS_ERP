using GTERP.Models.Self;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Module : SelfModel
    {

        public int ModuleId { get; set; }

        [Required]
        [Display(Name = "Module Code")]
        public string ModuleCode { get; set; }

        [Required]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        [Required]
        [Display(Name = "Module Caption")]
        public string ModuleCaption { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Module Description")]
        public string ModuleDescription { get; set; }

        [Display(Name = "Module Link")]
        public string ModuleLink { get; set; }


        [Display(Name = "Module Class a")]
        public string ModuleClassa { get; set; }
        [Display(Name = "Module Class i")]

        public string ModuleClassi { get; set; }

        [Display(Name = "Module Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ModuleImage { get; set; }

        [Display(Name = "Module Image [Folder]")]

        public string ModuleImagePath { get; set; }

        [Display(Name = "Module Image Extension")]
        public string ModuleImageExtension { get; set; }

        [Display(Name = "Is InActive")]
        public int isInactive { get; set; }


        [Display(Name = "SL No.")]
        public Nullable<int> SLNo { get; set; }



        [Display(Name = "Module Group")]
        public virtual ICollection<ModuleGroup> vModuleGroup { get; set; }
        public virtual ICollection<ModuleCourses> ModuleCourses { get; set; }
    }

    public class ModuleGroup : SelfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleGroupId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Module Group Name")]
        public string ModuleGroupName { get; set; }

        [Required]
        [Display(Name = "Module Group Caption")]
        public string ModuleGroupCaption { get; set; }

        [Required]
        [Display(Name = "Module")]
        public int ModuleId { get; set; }
        public virtual Module vModule { get; set; }


        [Display(Name = "Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ModuleGroupImage { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]

        [Display(Name = "Image [Folder]")]

        public string ImagePath { get; set; }

        [Display(Name = "Image Extension")]
        public string ImageExtension { get; set; }

        [Display(Name = "SL No.")]
        public Nullable<int> SLNo { get; set; }


        //[Display(Name = "Module Menu")]
        public virtual ICollection<ModuleMenu> vModuleMenu { get; set; }

    }

    public class ModuleMenu : SelfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleMenuId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Module Menu Name")]
        public string ModuleMenuName { get; set; }

        [Required]
        [Display(Name = "Module Menu Caption")]
        public string ModuleMenuCaption { get; set; }

        [Required]
        [Display(Name = "Module Group")]
        public int ModuleGroupId { get; set; }
        public virtual ModuleGroup vModuleGroup { get; set; }

        [Required]
        [Display(Name = "Image Criteria")]
        public int ImageCriteriaId { get; set; }
        public virtual ImageCriteria vImageCriteria { get; set; }

        [Required]
        [Display(Name = "Module")]
        public int ModuleId { get; set; }
        public virtual Module vModule { get; set; }


        [Display(Name = "Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]
        public byte[] ModuleMenuImage { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]
        [Display(Name = "Image [Folder]")]
        public string ModuleImagePath { get; set; }


        [Display(Name = "Image Extension")]
        public string ModuleImageExtension { get; set; }

        public string ModuleMenuController { get; set; }

        public string ModuleMenuLink { get; set; }

        [StringLength(100)]
        [Display(Name = "Class")]
        public string ModuleMenuClass { get; set; }

        public int isInactive { get; set; }
        public int isParent { get; set; }

        [Display(Name = "SL No.")]
        public Nullable<int> SLNo { get; set; }

        public bool Active { get; set; }


        [ForeignKey("ParentModuleMenu")]
        [Display(Name = "Parent Menu")]
        public int? ParentId { get; set; }
        public bool isCertificate { get; set; }

        public virtual ModuleMenu ParentModuleMenu { get; set; }

        public virtual ICollection<ModuleMenu> ModuleMenuChildren { get; set; }


        [Display(Name = "Module Menu Class i")]

        public string ModuleMenuClassi { get; set; }
        public virtual ICollection<MenuPermission_Details> MenuPermissionDetails { get; set; }
        public virtual ICollection<ModuleCourses> ModuleCourses { get; set; }


    }

    public class ImageCriteria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageCriteriaId { get; set; }

        public string ImageCriteriaCaption { get; set; }

    }

    // for menu permission module

    public class MenuPermission_Master
    {
        //public MenuPermission_Master()
        //{
        //    this.MenuPermission_Details = new HashSet<MenuPermission_Details>();

        //}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuPermissionId { get; set; }

        [StringLength(128)]
        [Display(Name = "User")]
        public string useridPermission { get; set; }
        public int? DefaultModuleId { get; set; }

        [StringLength(128)]
        public string comid { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        [NotMapped]
        public bool Active { get; set; }


        public DateTime? DateAdded { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        public Nullable<System.DateTime> DateUpdated { get; set; }


        public virtual ICollection<MenuPermission_Details> MenuPermission_Details { get; set; }

    }
    public class MenuPermission_Details
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuPermissionDetailsId { get; set; }


        [ForeignKey("MenuPermissionMasters")]
        public int MenuPermissionId { get; set; }
        public virtual MenuPermission_Master MenuPermissionMasters { get; set; }

        public int ModuleMenuId { get; set; }
        public virtual ModuleMenu ModuleMenus { get; set; }

        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
        public bool IsReport { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public int SLNo { get; set; }
        public bool isDefault { get; set; }



    }

    // for menu permission report

    public class ReportPermission_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportPermission_MasterId { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string UserId { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string UpdateByUser { get; set; }
        public string useridPermission { get; set; }

        [NotMapped]
        public bool Active { get; set; }


        public DateTime? DateAdded { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        public Nullable<System.DateTime> DateUpdated { get; set; }

        //navigation
        public virtual ICollection<ReportPermission_Details> ReportPermission_Details { get; set; }
    }
    public class ReportPermission_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportPermission_DetailsId { get; set; }

        [ForeignKey("ReportPermission_Master")]
        public int ReportPermission_MasterId { get; set; }
        public virtual ReportPermission_Master ReportPermission_Master { get; set; }
        public int ReportId { get; set; }
        public virtual HR_ReportType ReportType { get; set; }

        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int SLNo { get; set; }
    }


    //for app version permission module menu

    public class VersionMenuPermission_Master
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuPermissionId { get; set; }

        public int VersionId { get; set; }
        public int? DefaultModuleId { get; set; }

        public int SoftwareId { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        [NotMapped]
        public bool Active { get; set; }


        public DateTime? DateAdded { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        public Nullable<System.DateTime> DateUpdated { get; set; }


        public virtual ICollection<VersionMenuPermission_Details> VersionMenuPermission_Details { get; set; }



        // public virtual ICollection<UserReport_Details> UserReport_Details { get; set; }
    }
    public class VersionMenuPermission_Details
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuPermissionDetailsId { get; set; }


        [ForeignKey("VersionMenuPermissionMasters")]
        public int MenuPermissionId { get; set; }
        public virtual VersionMenuPermission_Master VersionMenuPermissionMasters { get; set; }

        public int ModuleMenuId { get; set; }
        public virtual ModuleMenu ModuleMenus { get; set; }

        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
        public bool IsReport { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public int SLNo { get; set; }
        public bool isDefault { get; set; }
    }

    // for app version permission report permission

    public class Version_Report_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VersionReportMasterId { get; set; }

        public int VersionId { get; set; }

        public int SoftwareId { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string UserId { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string UpdateByUser { get; set; }

        [NotMapped]
        public bool Active { get; set; }


        public DateTime? DateAdded { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        public Nullable<System.DateTime> DateUpdated { get; set; }

        //navigation
        public virtual ICollection<Version_Report_Details> Version_Report_Details { get; set; }
    }
    public class Version_Report_Details
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VersionReportDetailsId { get; set; }

        [ForeignKey("VersionReportPermissionMasters")]
        public int VersionReportMasterId { get; set; }
        public virtual Version_Report_Master VersionReportPermissionMasters { get; set; }

        
        public int ReportId { get; set; }
      
        public virtual HR_ReportType ReportType { get; set; }

        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
       
        //public DateTime? DateAdded { get; set; }
        //public DateTime? DateUpdated { get; set; }
        //public int SLNo { get; set; }
    
        // public bool IsReport { get; set; }

        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int SLNo { get; set; }
      

    }

    // for sub menu permission

    public class SubMenuPermissionMasters
    {
        //public MenuPermission_Master()
        //{
        //    this.MenuPermission_Details = new HashSet<MenuPermission_Details>();

        //}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubMenuPermissionId { get; set; }

        [StringLength(128)]
        [Display(Name = "User")]
        public string UserIdPermission { get; set; }
        public int? DefaultModuleId { get; set; }
        public int VersionId { get; set; }
        public int SoftwareId { get; set; }
        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string UserId { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string UserIdUpdate { get; set; }

        [NotMapped]
        public bool Active { get; set; }


        public DateTime? DateAdded { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual ICollection<SubMenuPermissionDetails> SubMenuPermissionDetails { get; set; }

    }
    public class SubMenuPermissionDetails
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubMenuPermissionDetailsId { get; set; }
        [StringLength(128)]
        public string ComId { get; set; }

        [ForeignKey("SubMenuPermissionMasters")]
        public int SubMenuPermissionId { get; set; }
        public virtual SubMenuPermissionMasters SubMenuPermissionMasters { get; set; }

        public int ModuleMenuId { get; set; }
        public virtual ModuleMenu ModuleMenus { get; set; }

        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
        public bool IsReport { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public int SLNo { get; set; }
        public bool isDefault { get; set; }
    }

    // for sub menu permission report
    public class SubMenuPermissionReportMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubMenuPermissionReportMasterId { get; set; }

        public int VersionId { get; set; }

        public int SoftwareId { get; set; }

        [StringLength(128)]
        [Display(Name = "Company")]

        public string ComId { get; set; }

        [StringLength(128)]
        [Display(Name = "Entry User")]

        public string UserId { get; set; }

        [StringLength(128)]
        [Display(Name = "User Permission")]

        public string UserIdPermission { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string UpdateByUser { get; set; }

        [NotMapped]
        public bool Active { get; set; }


        public DateTime? DateAdded { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        public Nullable<System.DateTime> DateUpdated { get; set; }

        //navigation
        public virtual ICollection<SubMenuPermissionReportDetails> SubMenuPermissionReportDetails { get; set; }
    }
    public class SubMenuPermissionReportDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubMenuPermissionReportDetailsId { get; set; }

        [ForeignKey("SubMenuPermissionReportMaster")]
        public int SubMenuPermissionReportMasterId { get; set; }
        public virtual SubMenuPermissionReportMaster SubMenuPermissionReportMasters { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }
        public int ReportId { get; set; }

        public virtual HR_ReportType ReportType { get; set; }

        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }

        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int SLNo { get; set; }

    }
}