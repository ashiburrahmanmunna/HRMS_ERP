using GTERP.Models.Base;
using GTERP.Models.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GTERP.Models
{

    public partial class Cat_Strength : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StId { get; set; }

        public string StrengthType { get; set; }

        public string RelId { get; set; }

        [Display(Name = "Man Power")]
        public float ManPower { get; set; }
        public virtual ICollection<Cat_Department> Cat_Department { get; set; }
        public virtual ICollection<Cat_Designation> Cat_Designation { get; set; }
        public virtual ICollection<Cat_Section> Cat_Section { get; set; }
    }

    public partial class Hr_RawData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int aId { get; set; }

        [Required, StringLength(80)]
        public string ComId { get; set; } = "";
        [StringLength(15)]
        public string DeviceNo { get; set; }
        [StringLength(80)]
        public string CardNo { get; set; }
        [StringLength(15)]
        public string FPId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        [Required, DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtPunchDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
        [Required, DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? DtPunchTime { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
        // [Required]
        public int? StNo { get; set; }

        [StringLength(10)]
        public string InOut { get; set; }

        [StringLength(15)]
        public string OvNMark { get; set; }
        public int? IsNew { get; set; }

        [StringLength(15)]
        public string PCName { get; set; }
        [StringLength(15)]
        public string DeviceType { get; set; }
        public int? LUserId { get; set; }
        [StringLength(20)]
        public string Latitude { get; set; }
        [StringLength(20)]
        public string Longitude { get; set; }

        [StringLength(100)]
        public string QRData { get; set; }
        [StringLength(20)]
        public string Imei { get; set; }

        [StringLength(200)]
        public string LocationName { get; set; }
        public byte[] PicFront { get; set; }
        public byte[] PicBack { get; set; }
        public bool IsDelete { get; set; }
    }
    [Index(nameof(ComId))]
    public partial class Hr_RawData_Upload
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int aId { get; set; }
        [Required]
        [StringLength(50)]
        public string ComId { get; set; } = "";
        [StringLength(15)]
        public string DeviceNo { get; set; }
        [StringLength(80)]
        public string CardNo { get; set; }

        [Required]
        public DateTime? DtPunchDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
        [Required]
        public DateTime? DtPunchTime { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
        //[StringLength(50)]
        //public string TrackId { get; set; } = string.Empty;
        public int? EmpId { get; set; }
        public HR_Emp_Info Emp { get; set; }

    }
    public partial class HR_RawData_App
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int aId { get; set; }

        [StringLength(15)]
        public string DeviceNo { get; set; }

        [StringLength(15)]
        public string CardNo { get; set; }

        [StringLength(20)]
        public string FPId { get; set; }

        public int? EmpId { get; set; }

        public DateTime? dtPunchDate { get; set; }
        public DateTime? dtPunchTime { get; set; }

        public int? StNo { get; set; }

        [StringLength(50)]
        public string InOut { get; set; }


        [StringLength(50)]
        public string OvNMark { get; set; }

        public byte? IsNew { get; set; }

        [StringLength(128)]
        public string wId { get; set; }

        [StringLength(15)]
        public string PCName { get; set; }
        public int? LUserId { get; set; }
        [StringLength(15)]
        public string DeviceType { get; set; }
        [StringLength(20)]
        public string Latitude { get; set; }
        [StringLength(20)]
        public string Longitude { get; set; }
        [StringLength(100)]
        public string QRData { get; set; }
        [StringLength(20)]
        public string Imei { get; set; }
        [StringLength(200)]
        public string LocationName { get; set; }

        public string PicBack { get; set; }
        public string PicFront { get; set; }

        public bool IsDelete { get; set; }
    }

    public partial class HR_Emp_Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpImageId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Employee Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] EmpImage { get; set; }

        [Display(Name = "Employee Image [Folder]")]

        public string EmpImagePath { get; set; }

        [Display(Name = "Employee Image Extension")]
        public string EmpImageExtension { get; set; }

        [Display(Name = "Employee Sign [DB]")]

        public byte[] EmpSign { get; set; }

        [Display(Name = "Employee Sign [Folder]")]

        public string EmpSignPath { get; set; }

        [Display(Name = "Employee Sign Extension")]
        public string EmpSignExtension { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }


    public partial class HR_Emp_Info : BaseModel
    {

        //[StringLength(80)]
        //public string ComId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }

        [Display(Name = "Employee Code")]
        [StringLength(30)]
        public string EmpCode { get; set; }

        [Display(Name = "Employee Name")]
        [Required(ErrorMessage = "Please input Employee Name")]
        [StringLength(100)]
        public string EmpName { get; set; }

        [Display(Name = "E. Name Bangla")]
        [StringLength(100)]
        public string EmpNameB { get; set; }

        [Display(Name = "Religion")]
        public int RelgionId { get; set; }
        [ForeignKey("RelgionId")]
        public virtual Cat_Religion Cat_Religion { get; set; }

        [Display(Name = "Relay")]
        public string? RelayId { get; set; }

        [Display(Name = "Blood Group")]
        public int BloodId { get; set; }
        [ForeignKey("BloodId")]
        public virtual Cat_BloodGroup Cat_BloodGroup { get; set; }

        [Display(Name = "Unit")]
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Cat_Unit Cat_Unit { get; set; }

        [Display(Name = "Department")]
        public int DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Cat_Department Cat_Department { get; set; }

        [Display(Name = "Shift")]
        public int ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public virtual Cat_Shift Cat_Shift { get; set; }

        [Display(Name = "Designation")]
        public int DesigId { get; set; }
        [ForeignKey("DesigId")]
        public virtual Cat_Designation Cat_Designation { get; set; }


        [Display(Name = "Section")]
        public int SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Cat_Section { get; set; }


        [Display(Name = "Sub Section")]
        public int? SubSectId { get; set; }
        [ForeignKey("SubSectId")]
        public virtual Cat_SubSection Cat_SubSection { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateTime? DtBirth { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Join Date")]
        public DateTime? DtJoin { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Increment Date")]
        public DateTime? DtIncrement { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Promotion Date")]
        public DateTime? DtPromotion { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Confirm Date")]
        public DateTime? DtConfirm { get; set; }

        // transfer into personal 
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "PF Date")]
        //public DateTime? DtPf { get; set; } 


        [Display(Name = "Employee Type")]
        public int? EmpTypeId { get; set; }
        [ForeignKey("EmpTypeId")]
        public virtual Cat_Emp_Type Cat_Emp_Type { get; set; }

        [Display(Name = "Employment Type")]
        public int? EmploymentTypeId { get; set; }
        [ForeignKey("EmploymentTypeId")]
        public virtual Cat_Employment_Type Cat_Employment_Type { get; set; }


        [Display(Name = "Gender")]
        public int? GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual Cat_Gender Cat_Gender { get; set; }

        [Display(Name = "Category Id")]
        public int? CategoryId { get; set; }

        [Display(Name = "SubCategory Id")]
        public int? SubCategoryId { get; set; }


        [StringLength(40)]
        [Display(Name = "National Id")]
        public string NID { get; set; }

        [StringLength(40)]
        [Display(Name = "Finger Id")]
        public string FingerId { get; set; }
        [Display(Name = "Is Casual")]
        public bool IsCasual { get; set; }

        [Display(Name = "Is Confirm")]
        public bool IsConfirm { get; set; }

        [Display(Name = "Skill")]
        public int? SkillId { get; set; }
        [ForeignKey("SkillId")]
        public Cat_Skill Cat_Skill { get; set; }


        [Display(Name = "Phone No 1")]
        [StringLength(20)]
        public string EmpPhone1 { get; set; }

        [Display(Name = "Phone No 2")]
        [StringLength(20)]
        public string EmpPhone2 { get; set; }

        public bool IsInactive { get; set; }
        //IsLunchAllow,IsLunchDed,IsTrnAllow,IsTrnDed
        public bool IsLunchAllow { get; set; } = false;
        public bool IsLunchDed { get; set; } = false;
        public bool IsTrnAllow { get; set; } = false;
        public bool IsTrnDed { get; set; } = false;
        public bool IsSalaryHold { get; set; } = false;

        [Display(Name = "Is Incentive Bonus")]
        public bool IsIncentiveBns { get; set; }

        [Display(Name = "Is Night Allow ")]
        public bool IsNightAllow { get; set; }
        [Display(Name = " Is Holiday Allow")]
        public bool IsHolidayAllow { get; set; }


        [StringLength(50)]
        public string EmpPerZip { get; set; }

        [Display(Name = "Email")]
        [StringLength(50)]
        public string EmpEmail { get; set; }


        [StringLength(120)]
        [DataType(DataType.MultilineText)]
        public string EmpRemarks { get; set; }

        [Display(Name = "Grade")]
        public int? GradeId { get; set; }
        [ForeignKey("GradeId")]
        public virtual Cat_Grade Cat_Grade { get; set; }

        [Display(Name = "Floor")]
        public int? FloorId { get; set; }
        [ForeignKey("FloorId")]
        public virtual Cat_Floor Cat_Floor { get; set; }

        [Display(Name = "Line")]
        public int? LineId { get; set; }
        [ForeignKey("LineId")]
        public virtual Cat_Line Cat_Line { get; set; }

        [Display(Name = "OT Activate")]
        public bool IsAllowOT { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "DAP Join")]
        public DateTime? DtLocalJoin { get; set; }


        [Display(Name = "Card No")]
        public int ManageType { get; set; } = 0;


        [StringLength(60)]
        public string PcName { get; set; }
        [ForeignKey("Cat_Variable")]
        public int? VendorType { get; set; }

        public int? JobNatureType { get; set; }

        public int? AltitudeData { get; set; }
        public double? Rate { get; set; }
        public string? Token { get; set; }

        [Display(Name = "Gross Salary")]
        public double? GrossSal { get; set; }
        public virtual Cat_Variable Cat_Variable { get; set; }
        public int? OldGradeId { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }



        public virtual HR_Emp_Address HR_Emp_Address { get; set; }
        public virtual ICollection<HR_Emp_Education> HR_Emp_Educations { get; set; }
        public virtual ICollection<HR_Emp_Projects> HR_Emp_Projects { get; set; }
        public virtual ICollection<HR_Emp_Devices> HR_Emp_Devices { get; set; }
        public virtual ICollection<HR_Emp_Experience> HR_Emp_Experiences { get; set; }

        public virtual HR_Emp_Family HR_Emp_Family { get; set; }
        public virtual HR_Emp_Image HR_Emp_Image { get; set; }
        public virtual HR_Emp_Nominee HR_Emp_Nominee { get; set; }
        public virtual HR_Emp_PersonalInfo HR_Emp_PersonalInfo { get; set; }
        public virtual HR_Leave_Avail HR_Leave_Avail { get; set; }
        public virtual HR_Leave_Balance HR_Leave_Balance { get; set; }
        public virtual HR_Emp_Increment HR_Emp_Increment { get; set; }
        public virtual HR_Emp_BankInfo HR_Emp_BankInfo { get; set; }



        [StringLength(128)]
        public string LinkUserId { get; set; }
        [Display(Name = "IsApprove")]
        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public int? FinalAprvId { get; set; }
        [NotMapped]
        public string FinalAprvName { get; set; }
        public int? FirstAprvId { get; set; }
        [NotMapped]
        public string FirstAprvName { get; set; }
        public bool IsHOD { get; set; }
        [Display(Name = "Status")]
        public bool IsStatus { get; set; }
        public bool IsHR { get; set; }
        public bool IsLateDeduct { get; set; }
        public string? MobileAttendence { get; set; }
    }


    public partial class HR_Emp_Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpAddId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Current City/Village")]
        [StringLength(200)]
        public string EmpCurrCityVill { get; set; }

        [Display(Name = "Permanent City/Village")]
        [StringLength(200)]
        public string EmpPerCityVill { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        [StringLength(200)]
        public string EmpRemarksCurr { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        [StringLength(200)]
        public string EmpRemarksPer { get; set; }

        [Display(Name = "Current Post Office")]
        public int? EmpCurrPOId { get; set; }
        [ForeignKey("EmpCurrPOId")]
        public virtual Cat_PostOffice Cat_PostOfficeCurr { get; set; }

        [Display(Name = "Permanent Post Office")]
        public int? EmpPerPOId { get; set; }
        [ForeignKey("EmpPerPOId")]
        public virtual Cat_PostOffice Cat_PostOfficePer { get; set; }

        [Display(Name = "Current Police Station")]
        public int? EmpCurrPSId { get; set; }
        [ForeignKey("EmpCurrPSId")]
        public virtual Cat_PoliceStation Cat_PoliceStationCurr { get; set; }


        [Display(Name = "Permanent Police Station")]
        public int? EmpPerPSId { get; set; }
        [ForeignKey("EmpPerPSId")]
        public virtual Cat_PoliceStation Cat_PoliceStationPer { get; set; }

        [Display(Name = "Current District")]
        public int? EmpCurrDistId { get; set; }
        [ForeignKey("EmpCurrDistId")]
        public virtual Cat_District Cat_DistrictCurr { get; set; }


        [Display(Name = "Permanent District")]
        public int? EmpPerDistId { get; set; }
        [ForeignKey("EmpPerDistId")]
        public virtual Cat_District Cat_DistrictPer { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(30)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }

    }
    public partial class HR_Emp_Projects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpProjectId { get; set; }
        public string ComId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dtStart { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dtEnd { get; set; }
        public string? ProjectRoll { get; set; }
        public string? Remarks { get; set; }
    }

    public partial class HR_Emp_Devices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpDeviceId { get; set; }
        public string ComId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Device Type")]
        public string? DeviceType { get; set; }

        [Display(Name = "Device Model")]
        public string? DeviceModel { get; set; }

        [Display(Name = "Device Brand")]
        public string? DeviceBrand { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dtIssue { get; set; }
        public string? Remarks { get; set; }
    }
    public partial class HR_Emp_Education
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpEduId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Examination")]
        [StringLength(30)]
        public string ExamName { get; set; }

        [Display(Name = "Result")]
        [StringLength(30)]
        public string ExamResult { get; set; }

        [Display(Name = "Major")]
        [StringLength(30)]
        public string MajorSub { get; set; }

        [Display(Name = "Institute")]
        [StringLength(30)]
        public string InstituteName { get; set; }

        [Display(Name = "Board")]
        [StringLength(30)]
        public string BoardName { get; set; }

        [Display(Name = "Passing Year")]
        [StringLength(30)]
        public string PassingYear { get; set; }

        [StringLength(128)]
        public string ComId { get; set; }

        [StringLength(30)]
        public string PcName { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Certificate { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dtStart { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dtEnd { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public IFormFile? Files { get; set; }

    }

    public partial class HR_Emp_Experience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpExpId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Company")]
        [StringLength(30)]
        public string PrevCompany { get; set; }

        [Display(Name = "Designation")]
        [StringLength(30)]
        public string PrevDesignation { get; set; }

        [Display(Name = "Salary")]
        [StringLength(30)]
        public string PrevSalary { get; set; }

        [Display(Name = "From")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtFromJob { get; set; }

        [Display(Name = "To")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtToJob { get; set; }

        [Display(Name = "Experience Year")]
        [StringLength(10)]
        public string ExpYear { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


        [DataType("NVARCHAR(128)")]
        public string ComId { get; set; }

        [StringLength(30)]
        public string PcName { get; set; }
        [DataType("NVARCHAR(128)")]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Emp_Family
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpFamilyId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Father Name")]
        [StringLength(60)]
        public string EmpFather { get; set; }

        [Display(Name = "F. Name Bangla")]
        [StringLength(60)]
        public string EmpFatherB { get; set; }

        [Display(Name = "Father NID")]
        [StringLength(25)]
        public string EmpFatherNID { get; set; }

        [Display(Name = "Father Mobile")]
        [StringLength(20)]
        public string EmpFatherMobile { get; set; }

        [Display(Name = "Mother Name")]
        [StringLength(60)]
        public string EmpMother { get; set; }

        [Display(Name = "M. Name Bangla")]
        [StringLength(60)]
        public string EmpMotherB { get; set; }

        [Display(Name = "Mother NID")]
        [StringLength(25)]
        public string EmpMotherNID { get; set; }

        [Display(Name = "Mother Mobile")]
        [StringLength(20)]
        public string EmpMotherMobile { get; set; }

        [Display(Name = "Spouse Name")]
        [StringLength(60)]
        public string EmpSpouse { get; set; }

        [Display(Name = "S. Name Bangla")]
        [StringLength(60)]
        public string EmpSpouseB { get; set; }

        [Display(Name = "Spouse NID")]
        [StringLength(25)]
        public string EmpSpouseNID { get; set; }

        [Display(Name = "Spouse Mobile")]
        [StringLength(20)]
        public string EmpSpouseMobile { get; set; }

        [Display(Name = "Child Name 1")]
        [StringLength(60)]
        public string EmpChildName1 { get; set; }

        [Display(Name = "Child DOB 1")]
        public DateTime? EmpChildDOB1 { get; set; }

        [Display(Name = "Child Edu 1")]
        [StringLength(20)]
        public string EmpChildEdu1 { get; set; }

        [Display(Name = "Child Birth Certificate 1")]
        [StringLength(25)]
        public string EmpChildBirthCer1 { get; set; }

        [Display(Name = "Child Name 2")]
        [StringLength(60)]
        public string EmpChildName2 { get; set; }

        [Display(Name = "Child DOB 2")]
        public DateTime? EmpChildDOB2 { get; set; }

        [Display(Name = "Child Edu 2")]
        [StringLength(20)]
        public string EmpChildEdu2 { get; set; }

        [Display(Name = "Child Birth Certificate 2")]
        [StringLength(25)]
        public string EmpChildBirthCer2 { get; set; }

        [Display(Name = "Child Name 3")]
        [StringLength(60)]
        public string EmpChildName3 { get; set; }

        [Display(Name = "Child DOB 3")]
        public DateTime? EmpChildDOB3 { get; set; }

        [Display(Name = "Child Edu 3")]
        [StringLength(20)]
        public string EmpChildEdu3 { get; set; }

        [Display(Name = "Child Birth Certificate 3")]
        [StringLength(25)]
        public string EmpChildBirthCer3 { get; set; }

        [Display(Name = "Child Name 4")]
        [StringLength(60)]
        public string EmpChildName4 { get; set; }

        [Display(Name = "Child DOB 4")]
        public DateTime? EmpChildDOB4 { get; set; }

        [Display(Name = "Child Edu 4")]
        [StringLength(20)]
        public string EmpChildEdu4 { get; set; }

        [Display(Name = "Child Birth Certificate 4")]
        [StringLength(25)]
        public string EmpChildBirthCer4 { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(30)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Emp_Nominee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpNomId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "EmpNominee Name 1")]
        [StringLength(60)]
        public string EmpNomineeName1 { get; set; }

        [Display(Name = "Nominee DOB 1")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EmpNomineeDOB1 { get; set; }

        [Display(Name = "Nominee Job Type 1")]
        [StringLength(50)]
        public string EmpNomineeJobType1 { get; set; }

        [Display(Name = "Nominee Mobile 1")]
        [StringLength(20)]
        public string EmpNomineeMobile1 { get; set; }

        [Display(Name = "Nominee NID 1")]
        [StringLength(25)]
        public string EmpNomineeNID1 { get; set; }

        [Display(Name = "Nominee Relation 1")]
        [StringLength(30)]
        public string EmpNomineeRelation1 { get; set; }

        [Display(Name = "Nominee Percentage 1")]
        [StringLength(30)]
        public string EmpNomineePercentage1 { get; set; }

        [Display(Name = "Nominee Address 1")]
        [StringLength(100)]
        public string EmpNomineeAddress1 { get; set; }

        [Display(Name = "EmpNominee Name 2")]
        [StringLength(60)]
        public string EmpNomineeName2 { get; set; }

        [Display(Name = "Nominee DOB 2")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EmpNomineeDOB2 { get; set; }

        [Display(Name = "Nominee Job Type 2")]
        [StringLength(50)]
        public string EmpNomineeJobType2 { get; set; }

        [Display(Name = "Nominee Mobile 2")]
        [StringLength(20)]
        public string EmpNomineeMobile2 { get; set; }

        [Display(Name = "Nominee NID 2")]
        [StringLength(25)]
        public string EmpNomineeNID2 { get; set; }

        [Display(Name = "Nominee Relation 2")]
        [StringLength(30)]
        public string EmpNomineeRelation2 { get; set; }

        [Display(Name = "Nominee Percentage 2")]
        [StringLength(30)]
        public string EmpNomineePercentage2 { get; set; }

        [Display(Name = "Nominee Address 2")]
        [StringLength(100)]
        public string EmpNomineeAddress2 { get; set; }


        [DataType("NVARCHAR(128)")]
        public string ComId { get; set; }

        [StringLength(50)]
        public string PcName { get; set; }
        [DataType("NVARCHAR(128)")]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Emp_PersonalInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpPersInfoId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Nick Name")]
        [StringLength(60)]
        public string NickName { get; set; }

        [Display(Name = "Passport No")]
        [StringLength(60)]
        public string PassportNo { get; set; }

        [Display(Name = "Birth Certificate")]
        [StringLength(60)]
        public string BirthCertificate { get; set; }

        [Display(Name = "TIN No")]
        [StringLength(60)]
        public string TINNo { get; set; }

        [Display(Name = "Marital Status")]
        public bool MaritalStatus { get; set; }

        [Display(Name = "Child No")]
        [StringLength(10)]
        public string ChildNo { get; set; }

        [Display(Name = "Nationality")]
        [StringLength(30)]
        public string Nationality { get; set; }

        [Display(Name = "Caste")]
        [StringLength(30)]
        public string Caste { get; set; }

        [Display(Name = "Identification Sign")]
        [StringLength(60)]
        public string IdentificationSign { get; set; }

        [Display(Name = "Height")]
        public string Height { get; set; }

        [Display(Name = "Weight")]
        public float? Weight { get; set; }

        [Display(Name = "Is Using House")]
        public bool IsUsingHouse { get; set; }


        [Display(Name = "Building No")]
        public int? BId { get; set; }
        [ForeignKey("BId")]
        public virtual Cat_BuildingType Cat_BuildingType { get; set; }

        [Display(Name = "File No")]
        [StringLength(30)]
        public string EmpFileNo { get; set; }

        [Display(Name = "Medical Book No")]
        [StringLength(30)]
        public string MedicalBookNo { get; set; }

        [Display(Name = "Emergency Cont Name")]
        [StringLength(60)]
        public string EmergencyContactName { get; set; }

        [Display(Name = "Emergency Cont. No")]
        [StringLength(30)]
        public string EmergencyContactNo { get; set; }

        [Display(Name = "Rel. Emer. Contact")]
        [StringLength(60)]
        public string RelationEmerContact { get; set; }

        [Display(Name = "Emp. Code BCIC")]
        [StringLength(30)]
        public string EmployeeCodeBCIC { get; set; }

        [Display(Name = "Grade")]
        [StringLength(30)]
        public string Grade { get; set; }

        [Display(Name = "Police Ver. Status")]
        public bool PoliceVerificationStatus { get; set; }

        [Display(Name = "PF Member")]
        [StringLength(30)]
        public string PFFileNo { get; set; }

        [Display(Name = "Is Allow PF")]
        public bool IsAllowPF { get; set; }

        [Display(Name = "PF Date")]
        public DateTime? DtPF { get; set; }

        [Display(Name = "Is PF Final")]
        public bool IsPFFinal { get; set; }

        [Display(Name = "PF Final Year")]
        public int? PFFinalYId { get; set; }
        [ForeignKey("PFFinalYId")]
        public virtual Acc_FiscalYear PFFinalYear { get; set; }

        [Display(Name = "Is PF Fund Transfer")]
        public bool IsPFFundTransfer { get; set; }

        [Display(Name = "PF Fund Transfer Year")]
        public int? PFFundTransferYId { get; set; }
        [ForeignKey("PFFundTransferYId")]
        public virtual Acc_FiscalYear PFFundTransferYear { get; set; }

        [Display(Name = "Is WF Final")]
        public bool IsWFFinal { get; set; }

        [Display(Name = "WF Final Year")]
        public int? WFFinalYId { get; set; }
        [ForeignKey("WFFinalYId")]
        public virtual Acc_FiscalYear WFFinalYear { get; set; }

        [Display(Name = "Is WF Fund Transfer")]
        public bool IsWFFundTransfer { get; set; }

        [Display(Name = "WF Fund Transfer Year")]
        public int? WFFundTransferYId { get; set; }
        [ForeignKey("WFFundTransferYId")]
        public virtual Acc_FiscalYear WFFundTransferYear { get; set; }

        [Display(Name = "is Gratuity Final")]
        public bool IsGratuityFinal { get; set; }

        [Display(Name = "WF Fund Transfer Year")]
        public int? GratuityFinalYId { get; set; }
        [ForeignKey("GratuityFinalYId")]
        public virtual Acc_FiscalYear GratuityFinalYear { get; set; }

        [Display(Name = "Is Gratuity Fund Transfer")]
        public bool IsGratuityFundTransfer { get; set; }

        [Display(Name = "Gratuity Fund Transfer Year")]
        public int? GratuityFundTransferYId { get; set; }
        [ForeignKey("GratuityFundTransferYId")]
        public virtual Acc_FiscalYear GratuityFundTransferYear { get; set; }


        //[Display(Name = "PF Member")]  
        //[StringLength(20)]
        //public string PFMember { get; set; }



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

        [Display(Name = "WeekDay")]
        public int? WeekDay { get; set; }

        [Display(Name = "2nd WeekDay")]
        public int? WeekDay2 { get; set; }

        //[Display(Name = "2nd WeekDay")]
        //public int? WeekDay2 { get; set; }


    }


    public partial class HR_Emp_PF_OPBal : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PFOPBalId { get; set; }


        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        public float PFOwn { get; set; }
        public float PFComp { get; set; }
        public float PFAdd { get; set; }

        [Display(Name = "Gratuity Fund Transfer Year")]
        public int? PFOPBalYID { get; set; }
        [ForeignKey("PFOPBalYID")]
        public virtual Acc_FiscalYear PFOPBalYear { get; set; }


        [Display(Name = "Debit Account Head")]
        public int? DebitAccId { get; set; }
        [Display(Name = "Debit Account Head")]

        [ForeignKey("DebitAccId")]
        public virtual PF_ChartOfAccount PFChartOfAccountDebit { get; set; }

        [Display(Name = "Credit Account Head")]
        public int? CreditAccId { get; set; }
        [Display(Name = "Credit Account Head")]

        [ForeignKey("CreditAccId")]
        public virtual PF_ChartOfAccount PFChartOfAccountCredit { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }


    }

    public partial class HR_Emp_BankInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankInfoId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Pay Mode")]
        public int? PayModeId { get; set; }
        [ForeignKey("PayModeId")]
        public virtual Cat_PayMode Cat_PayMode { get; set; }

        [Display(Name = "A/C Type")]
        public int? AccTypeId { get; set; }
        [ForeignKey("AccTypeId")]
        public virtual Cat_AccountType Cat_AccountType { get; set; }

        [Display(Name = "Bank Name")]
        public int? BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Cat_Bank Cat_Bank { get; set; }

        [Display(Name = "Branch Name")]
        public int? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Cat_BankBranch Cat_BankBranch { get; set; }

        [Display(Name = "Routing Number")]
        [StringLength(15)]
        public string RoutingNumber { get; set; }

        [Display(Name = "Account Number")]
        [StringLength(25)]
        public string AccountNumber { get; set; }

        [Display(Name = "Account Name")]
        [StringLength(60)]
        public string AccountName { get; set; }


        [Display(Name = "IsApproved")]
        public bool IsApproved { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public partial class TempLeaveEntryExcel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ComId { get; set; }
        public string EmpCode { get; set; }
        public int? EmpId { get; set; }
        public DateTime DtInput { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtTo { get; set; }
        public string LvType { get; set; }
        public int TotalDay { get; set; }
        public string? Remarks { get; set; }
    }

    public partial class HR_Leave_Avail
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
        public float? TotalHour { get; set; }

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
        public bool? IsDelete { get; set; }

    }

    public partial class HR_Leave_Balance : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LvBalId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        //public DateTime? DtOpBal { get; set; }
        [Display(Name = "Leave Balance Year")]
        public int DtOpeningBalance { get; set; } = 0;
        [Display(Name = "Casual Leave")]
        public float CL { get; set; } = 0;
        public float? ACL { get; set; } = 0;
        [Display(Name = "Sick Leave")]
        public float SL { get; set; } = 0;
        public float? ASL { get; set; } = 0;
        [Display(Name = "Earn Leave")]
        public float EL { get; set; } = 0;
        public float? AEL { get; set; } = 0;
        [Display(Name = "Maternity Leave")]
        public float? ML { get; set; } = 0;
        public float? AML { get; set; } = 0;
        [Display(Name = "Leave Without Pay")]
        public float? LWP { get; set; } = 0;

        public float? ALWP { get; set; } = 0;
        [Display(Name = "Accident Leave")]
        public float? ACCL { get; set; } = 0;
        public float? AACCL { get; set; } = 0;
        [Display(Name = "Special Leave")]
        public float? SPL { get; set; } = 0;
        public float? ASPL { get; set; } = 0;
        [Display(Name = "Training Leave")]
        public float? TL { get; set; } = 0;
        public float? ATL { get; set; } = 0;
        [Display(Name = "Business Leave")]
        public float? BL { get; set; } = 0;
        public float? ABL { get; set; } = 0;

        [Display(Name = "Is EL Paid")]
        public bool IsELPaid { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtInput { get; set; } = DateTime.Now;
        [NotMapped]
        public string Criteria { get; set; }
        [NotMapped]
        public int SectId { get; set; }
        [NotMapped]
        public int DeptId { get; set; }
        [NotMapped]
        public int LineId { get; set; }
        [NotMapped]
        public int FloorId { get; set; }
        [NotMapped]
        public float? PreviousLeave { get; set; }

        [NotMapped]
        public float CLBalance { get; set; }
        [NotMapped]
        public float SLBalance { get; set; }
        [NotMapped]
        public float ELBalance { get; set; }
        [NotMapped]
        public float MLBalance { get; set; }

    }

    public partial class HR_Emp_Increment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IncId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; } = "";

        [Display(Name = "Employee")]
        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Increment Date")]
        public DateTime? DtIncrement { get; set; }

        [Display(Name = "Promotion Date")]
        public DateTime? DtPromotion { get; set; }

        public double? Amount { get; set; } = 0;
        public float? Percentage { get; set; } = 0;
        public double? OldSalary { get; set; } = 0;
        public double? NewSalary { get; set; } = 0;
        public double? OldBS { get; set; } = 0;
        public double? NewBS { get; set; } = 0;
        public double? NewHR { get; set; } = 0;
        public double? OldHR { get; set; } = 0;

        public double? NewHRExp { get; set; } = 0;
        public double? OldHRExp { get; set; } = 0;
        public double? NewHRExpOther { get; set; } = 0;
        public double? OldHRExpOther { get; set; } = 0;

        public double? OldMA { get; set; } = 0;
        public double? NewMA { get; set; } = 0;
        public double? NewTA { get; set; } = 0;
        public double? OldTA { get; set; } = 0;
        public double? NewFA { get; set; } = 0;
        public double? OldFA { get; set; } = 0;
        public double? OldPA { get; set; } = 0;
        public double? NewPA { get; set; } = 0;
        public double? OldDA { get; set; } = 0;
        public double? NewDA { get; set; } = 0;
        public float? BSDiff { get; set; } = 0;
        public float? HRDiff { get; set; } = 0;
        public float? HRExpDiff { get; set; } = 0;
        public float? HRExpOtherDiff { get; set; } = 0;
        public float? MADiff { get; set; } = 0;
        public string? CompanyCode { get; set; }
        public string? New_EmpCode { get; set; }
        public int? FingerId { get; set; }
        public DateTime? TransferDate { get; set; }
        public int? OldLineId { get; set; }
        [ForeignKey("OldLineId")]
        public virtual Cat_Line Cat_LineOld { get; set; }
        public int? NewLineId { get; set; }
        [ForeignKey("NewLineId")]
        public virtual Cat_Line Cat_LineNew { get; set; }
        public int? OldFloorId { get; set; }
        [ForeignKey("OldFloorId")]
        public virtual Cat_Floor Cat_FloorOld { get; set; }
        public int? NewFloorId { get; set; }
        [ForeignKey("NewFloorId")]
        public virtual Cat_Floor Cat_FloorNew { get; set; }
        public int? NewGradeId { get; set; }
        [ForeignKey("NewGradeId")]
        public virtual Cat_Grade Cat_GradeNew { get; set; }
        public int? OldGradeId { get; set; }
        [ForeignKey("OldGradeId")]
        public virtual Cat_Grade Cat_GradeOld { get; set; }

        [Display(Name = "Old Unit")]
        public int? OldUnitId { get; set; }
        [ForeignKey("OldUnitId")]
        public virtual Cat_Unit Cat_UnitOld { get; set; }

        [Display(Name = "New Unit")]
        public int? NewUnitId { get; set; }
        [ForeignKey("NewUnitId")]
        public virtual Cat_Unit Cat_UnitNew { get; set; }

        [Display(Name = "Old Department")]
        public int? OldDeptId { get; set; }
        [ForeignKey("OldDeptId")]
        public virtual Cat_Department Cat_DepartmentOld { get; set; }

        [Display(Name = "New Department")]
        public int? NewDeptId { get; set; }
        [ForeignKey("NewDeptId")]
        public virtual Cat_Department Cat_DepartmentNew { get; set; }

        [Display(Name = "Old Section")]
        public int? OldSectId { get; set; }
        [ForeignKey("OldSectId")]
        public virtual Cat_Section Cat_SectionOld { get; set; }

        [Display(Name = "New Section")]
        public int? NewSectId { get; set; }
        [ForeignKey("NewSectId")]
        public virtual Cat_Section Cat_SectionNew { get; set; }

        [Display(Name = "Old Designation")]
        public int? OldDesigId { get; set; }
        [ForeignKey("OldDesigId")]
        public virtual Cat_Designation Cat_DesignationOld { get; set; }

        [Display(Name = "New Designation")]
        public int? NewDesigId { get; set; }
        [ForeignKey("NewDesigId")]
        public virtual Cat_Designation Cat_DesignationNew { get; set; }
        public int? OldShiftId { get; set; }
        [ForeignKey("OldShiftId")]
        public virtual Cat_Shift Cat_ShiftOld { get; set; }

        [Display(Name = "New Shift")]
        public int? NewShiftId { get; set; }
        [ForeignKey("NewShiftId")]
        public virtual Cat_Shift Cat_ShiftNew { get; set; }


        [Display(Name = "Increment Type")]
        public int? IncTypeId { get; set; }
        [ForeignKey("IncTypeId")]
        public virtual HR_IncType HR_IncType { get; set; }

        [Display(Name = "Old EmpType")]
        public int? OldEmpTypeId { get; set; }
        [ForeignKey("OldEmpTypeId")]
        public virtual Cat_Emp_Type Cat_Emp_TypeOld { get; set; }

        [Display(Name = "New EmpType")]
        public int? NewEmpTypeId { get; set; }
        [ForeignKey("NewEmpTypeId")]
        public virtual Cat_Emp_Type Cat_Emp_TypeNew { get; set; }


        [Display(Name = "Old Grade")]
        public int? OldGenderId { get; set; }
        [ForeignKey("OldGenderId")]
        public virtual Cat_Gender Cat_GenderOld { get; set; }

        [Display(Name = "New Grade")]
        public int? NewGenderId { get; set; }
        [ForeignKey("NewGenderId")]
        public virtual Cat_Gender Cat_GenderNew { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtInput { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));

        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }
    }

    public partial class HR_IncType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IncTypeId { get; set; }


        [StringLength(80)]
        public string ComId { get; set; }


        [StringLength(40)]
        public string IncType { get; set; }


        [StringLength(80)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime DtTran { get; set; } = DateTime.Now;

    }

    public partial class HR_Employee_Transfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransferId { get; set; }
        public string OldComId { get; set; }
        public string NewComId { get; set; }
        public int EmpId { get; set; }
        public string NewEmpCode { get; set; }
        public string EmpName { get; set; }
        public int? NewUnitId { get; set; }
        public int? NewDeptId { get; set; }
        public int? NewSectId { get; set; }
        public int? NewDesigId { get; set; }
        public int? NewShiftId { get; set; }
        public int? NewEmpTypeId { get; set; }
        public int? NewFloorId { get; set; }
        public int? NewLineId { get; set; }
        public int? FingerId { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;
    }
    public partial class HrAttFixed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime Othour { get; set; }
        public string Status { get; set; }
        public byte IsInactive { get; set; }
        public string Remarks { get; set; }
        public string Pcname { get; set; }
        public string LuserId { get; set; }
        public short? ShiftId { get; set; }
        public float? Ot { get; set; }
        public int? FirstAppId { get; set; }
        public int? FinalAppId { get; set; }
        public bool? AppFirst { get; set; }
        public bool AppFinal { get; set; }
        public bool Approved { get; set; }
        public byte IsUpdate { get; set; }
        public bool MailSend { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }

    public partial class HrEarnLeave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime DtInput { get; set; }
        public short? DeptId { get; set; }
        public short? SectId { get; set; }
        public short? SubSectId { get; set; }
        public short? DesigId { get; set; }
        public string EmpType { get; set; }
        public string ProssType { get; set; }
        public string PaySource { get; set; }
        public string PayMode { get; set; }
        public short Elyear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Gs { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Bs { get; set; }
        public float ElttlDays { get; set; }
        public float Elenjoy { get; set; }
        public float Elbal { get; set; }
        public float Elrate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Elamount { get; set; }
        public byte Stamp { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPay { get; set; }
        public byte? Cf { get; set; }
        public int? Amount { get; set; }
        public int Tk1000 { get; set; }
        public int Tk500 { get; set; }
        public int Tk100 { get; set; }
        public int Tk50 { get; set; }
        public int Tk20 { get; set; }
        public int Tk10 { get; set; }
        public int Tk5 { get; set; }
        public int Tk2 { get; set; }
        public int Tk1 { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }

    public partial class HrEmpIncr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public int? AIncId { get; set; }
        public int IncId { get; set; }
        public long EmpId { get; set; }
        public string IncType { get; set; }
        public DateTime? DtInc { get; set; }
        public double? Percentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldSal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NewSal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Bs { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Hr { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Ma { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Ta { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fa { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherAllow { get; set; }
        public int? OldDesigId { get; set; }
        public int? NewDesigId { get; set; }
        public byte? OldSectId { get; set; }
        public byte? NewSectId { get; set; }
        public int? OldSectIdSal { get; set; }
        public int? NewSectIdSal { get; set; }
        public string OldGrade { get; set; }
        public string NewGrade { get; set; }
        public string OldIsAllowOt { get; set; }
        public string NewIsAllowOt { get; set; }
        public byte IsInactive { get; set; }
        public Guid WId { get; set; }
        public string LuserId { get; set; }
        public string Pcname { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }

    public partial class HrEmpInfoBangla
    {
        public string ComId { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EmpId { get; set; }
        public string EmpNameB { get; set; }
        public string EmpFatherB { get; set; }
        public string EmpMotherB { get; set; }
        public string EmpSpouseB { get; set; }
        public string PreVillB { get; set; }
        public string PreThanaB { get; set; }
        public string PrePofficeB { get; set; }
        public string PreDistB { get; set; }
        public string PreAddB { get; set; }
        public string PvillB { get; set; }
        public string PthanaB { get; set; }
        public string PpofficeB { get; set; }
        public string PdistB { get; set; }
        public string PaddB { get; set; }
        public string Hedu { get; set; }
        public string ReligionB { get; set; }
        public string NationalityB { get; set; }
        public string EmergencyContactB { get; set; }
        public string BloodGroupB { get; set; }
        public string GradeB { get; set; }
        public string MaritalStsB { get; set; }
        public string EmergencyNameB { get; set; }
        public string EmergencyAddB { get; set; }
        public string EmergencyRelB { get; set; }
        public string EmpNomineeNameB { get; set; }
        public string EmpNomineeAddB { get; set; }
        public string EmpNomineeRelB { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }

    public partial class HR_Emp_Released : BaseModel
    {
        //[StringLength(80)]
        //public string ComId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtReleased { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(120)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Release Type")]
        [StringLength(30)]
        public string RelType { get; set; }

        public bool IsApproved { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Present Date")]
        public DateTime? DtPresentLast { get; set; }

        [Display(Name = "Submit Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtSubmit { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }

        // public string UserId { get; set; }
        // public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        // public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
        //public virtual Cat_Company Com { get; set; }
    }

    public partial class HR_Emp_Salary : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalaryId { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "House Location (Local)")]
        public int? LId1 { get; set; }
        [ForeignKey("LId1")]
        public virtual Cat_Location Cat_Location1 { get; set; }

        [Display(Name = "House Location (Other 1)")]
        public int? LId2 { get; set; }
        [ForeignKey("LId2")]
        public virtual Cat_Location Cat_Location2 { get; set; }

        [Display(Name = "House Location (Other 2)")]
        public int? LId3 { get; set; }
        [ForeignKey("LId3")]
        public virtual Cat_Location Cat_Location3 { get; set; }

        [Display(Name = "House Category")]
        public int? BId { get; set; }
        [ForeignKey("BId")]
        public virtual Cat_BuildingType Cat_BuildingTypeHC { get; set; }

        [Display(Name = "PF Location")]
        public int? PFLId { get; set; }
        [ForeignKey("PFLId")]
        public virtual Cat_Location Cat_LocationPF { get; set; }

        [Display(Name = "Welfare Location")]
        public int? WelfareLId { get; set; }
        [ForeignKey("WelfareLId")]
        public virtual Cat_Location Cat_LocationWelfare { get; set; }

        [Display(Name = "M.C. Location")]
        public int? MCLId { get; set; }
        [ForeignKey("MCLId")]
        public virtual Cat_Location Cat_LocationMC { get; set; }

        [Display(Name = "H.B. Loan Location (Local)")]
        public int? HBLId { get; set; }
        [ForeignKey("HBLId")]
        public virtual Cat_Location Cat_LocationHB { get; set; }

        [Display(Name = "H.B. Loan Location (Other1)")]
        public int? HBLId2 { get; set; }
        [ForeignKey("HBLId2")]
        public virtual Cat_Location Cat_LocationHB2 { get; set; }

        [Display(Name = "H.B. Loan Location (Other2)")]
        public int? HBLId3 { get; set; }
        [ForeignKey("HBLId3")]
        public virtual Cat_Location Cat_LocationHB3 { get; set; }



        [Display(Name = "PF Loan Location (Local)")]
        public int? PFLLId { get; set; }
        [ForeignKey("PFLLId")]
        public virtual Cat_Location Cat_PFLoanlocation { get; set; }

        [Display(Name = "PF Loan Location (Other 1)")]
        public int? PFLLId2 { get; set; }
        [ForeignKey("PFLLId2")]
        public virtual Cat_Location Cat_PFLoanlocation2 { get; set; }

        [Display(Name = "PF Loan Location (Other 2)")]
        public int? PFLLId3 { get; set; }
        [ForeignKey("PFLLId3")]
        public virtual Cat_Location Cat_PFLoanlocation3 { get; set; }

        [Display(Name = "Gratuity Location")]
        public int? GLId { get; set; }
        [ForeignKey("GLId")]
        public virtual Cat_Location Cat_GratuityLocation { get; set; }


        [Display(Name = "Basic Pay")]
        public float BasicSalary { get; set; }
        public bool IsBS { get; set; }

        [Display(Name = "House Rent")]
        public float? HouseRent { get; set; }
        public bool IsHr { get; set; }

        [Display(Name = "Madical Allowance")]
        public float? MadicalAllow { get; set; }
        public bool IsMa { get; set; }

        [Display(Name = "Food Allowance")]
        public float? FoodAllow { get; set; }
        public bool IsFa { get; set; }

        [Display(Name = "H.R. ExpensesOther")]
        public float? HRExpensesOther { get; set; }
        public bool IsHRExpensesOther { get; set; }

        [Display(Name = "Conveyance Allowance")]
        public float? ConveyanceAllow { get; set; }
        public bool IsConvAllow { get; set; }

        [Display(Name = "Dearness Allowance")]
        public float? DearnessAllow { get; set; }
        public bool IsDearAllow { get; set; }

        [Display(Name = "Gas Allowance")]
        public float? GasAllow { get; set; }
        public bool IsGasAllow { get; set; }
        [Display(Name = "Personal Pay")]
        public float? PersonalPay { get; set; }
        public bool IsPersonalPay { get; set; }
        [Display(Name = "Arrear Basic")]
        public float? ArrearBasic { get; set; }
        public bool IsArrearBasic { get; set; }

        [Display(Name = "Arrear Bonus")]
        public float? ArrearBonus { get; set; }
        public bool IsArrearBonus { get; set; }
        [Display(Name = "Washing Allowance")]
        public float? WashingAllow { get; set; }
        public bool IsWashingAllow { get; set; }

        [Display(Name = "Shift Allowance")]
        public float? SiftAllow { get; set; }
        public bool IsSiftAllow { get; set; }

        [Display(Name = "Charge Allowance")]
        public float? ChargeAllow { get; set; }
        public bool IsChargAllow { get; set; }

        [Display(Name = "Misc Add")]
        public float? MiscAdd { get; set; }
        public bool IsMiscAdd { get; set; }

        [Display(Name = "Contain Sub")]
        public float? ContainSub { get; set; }
        public bool IsContainSub { get; set; }

        [Display(Name = "Co. P.F. Count.")]
        public float? ComPfCount { get; set; }
        public bool IsComPfcount { get; set; }

        [Display(Name = "Education Allowance")]
        public float? EduAllow { get; set; }
        public bool IsEduAllow { get; set; }

        [Display(Name = "Tiffin Allowance")]
        public float? TiffinAllow { get; set; }
        public bool IsTiffinAllow { get; set; }

        [Display(Name = "Canteen Subsidy")]
        public float? CanteenAllow { get; set; }
        public bool IsCanteenAllow { get; set; }

        [Display(Name = "Attendance")]
        public float? AttAllow { get; set; }
        public bool IsAttAllow { get; set; }

        [Display(Name = "Festival Bonus")]
        public float? FestivalBonus { get; set; }
        public bool IsFestivalBonus { get; set; }

        [Display(Name = "Risk Allowance")]
        public float? RiskAllow { get; set; }
        public bool IsRiskAllow { get; set; }

        [Display(Name = "Night Allowance")]
        public float? NightAllow { get; set; }
        public bool IsNightAllow { get; set; }

        [Display(Name = "Mobile Allowance")]
        public float? MobileAllow { get; set; }
        public bool IsMobileAllow { get; set; }
        // Deduction

        [Display(Name = "P.F Contribution")]
        public float? Pf { get; set; }
        public bool IsPf { get; set; }

        [Display(Name = "Addl. P.F Contribution")]
        public float? PfAdd { get; set; }
        public bool IsPfAdd { get; set; }

        [Display(Name = "H.R Expenses")]
        public float? HrExp { get; set; }
        public bool IsHrexp { get; set; }

        [Display(Name = "Festival Bonus")]
        public float? FesBonusDed { get; set; }
        public bool IsFesBonus { get; set; }

        [Display(Name = "Transport Charge")]
        public float? Transportcharge { get; set; }
        public bool IsTrncharge { get; set; }

        [Display(Name = "Teliphone Charge")]
        public float? TeliphoneCharge { get; set; }
        public bool IsTelcharge { get; set; }

        //gaschargeother,electricchargeother,waterchargeother

        [Display(Name = "Gas Charge Other")]
        public float? GasChargeOther { get; set; }
        public bool IsGasChargeOther { get; set; }

        [Display(Name = "Electric Charge Other")]
        public float? ElectricChargeOther { get; set; }
        public bool IsElectricChargeOther { get; set; }

        [Display(Name = "Water Charge Other")]
        public float? WaterChargeOther { get; set; }
        public bool IsWaterChargeOther { get; set; }


        [Display(Name = "T.A/D.A Expense")]
        public float? TAExpense { get; set; }
        public bool IsTAExp { get; set; }

        [Display(Name = "Salary Advance")]
        public float? SalaryAdv { get; set; }
        public bool IsSalaryAdv { get; set; }


        [Display(Name = "Purchase Advance")]
        public float? PurchaseAdv { get; set; }
        public bool IsPurchaseAdv { get; set; }

        [Display(Name = "M.C. Loan Deduction")]
        public float? McloanDed { get; set; }
        public bool IsMcloan { get; set; }

        [Display(Name = "H.B. Loan Deduction")]
        public float? HbloanDed { get; set; }
        public bool IsHbloan { get; set; }

        [Display(Name = "P.F. Loan Deduction")]
        public float? PfloannDed { get; set; }
        public bool IsPfloann { get; set; }

        [Display(Name = "W.F. Loan Local")]
        public float? WfloanLocal { get; set; }
        public bool IsWfloanLocal { get; set; }

        [Display(Name = "W.F. Loan Other")]
        public float? WfloanOther { get; set; }
        public bool IsWfloanOther { get; set; }

        [Display(Name = "W.F. Loan Deduction")]
        public float? WpfloanDed { get; set; }
        public bool IsWpfloanDed { get; set; }

        [Display(Name = "Material Loan Deduction")]
        public float? MaterialLoanDed { get; set; }
        public bool IsMaterialLoan { get; set; }

        [Display(Name = "Misc Deduction")]
        public float? MiscDed { get; set; }
        public bool IsMiscDed { get; set; }

        [Display(Name = "Advance Against Expense")]
        public float? AdvAgainstExp { get; set; }
        public bool IsAdvAgainstExp { get; set; }

        [Display(Name = "Facility Advance")]
        public float? AdvFacility { get; set; }
        public bool IsAdvFacility { get; set; }

        [Display(Name = "Electric Charge")]
        public float? ElectricCharge { get; set; }
        public bool IsElectricCharge { get; set; }

        [Display(Name = "Gas Charge")]
        public float? Gascharge { get; set; }
        public bool IsGascharge { get; set; }

        [Display(Name = "Haz Scheme/ T. Jatra")]
        public float? HazScheme { get; set; }
        public bool IsHazScheme { get; set; }

        [Display(Name = "Donation")]
        public float? Donation { get; set; }
        public bool IsDonation { get; set; }

        [Display(Name = "Dish Antenna")]
        public float? Dishantenna { get; set; }
        public bool IsDishantenna { get; set; }

        [Display(Name = "Revenue stamp")]
        public float? RevenueStamp { get; set; }
        public bool IsRevenueStamp { get; set; }

        [Display(Name = "OWA Sub")]
        public float? OwaSub { get; set; }
        public bool IsOwaSub { get; set; }

        [Display(Name = "Deduction Incentive Bonus")]
        public float? DedIncBns { get; set; }
        public bool IsDedIncBns { get; set; }

        [Display(Name = "DAP Emp Office Club")]
        public float? DapEmpClub { get; set; }
        public bool IsDapEmpClub { get; set; }

        [Display(Name = "Moktab")]
        public float? Moktab { get; set; }
        public bool IsMoktab { get; set; }

        [Display(Name = "Che./A. Fou")]
        public float? ChemicalForum { get; set; }
        public bool IsChemicalForum { get; set; }

        [Display(Name = "Dip. Asso.")]
        public float? DiplomaassoDed { get; set; }
        public bool IsDiplomaassoDed { get; set; }

        [Display(Name = "Engg. Asso.")]
        public float? EnggassoDed { get; set; }
        public bool IsEnggassoDed { get; set; }

        [Display(Name = "WFSub")]
        public float? Wfsub { get; set; }
        public bool IsWfsub { get; set; }

        [Display(Name = "Edu. Allow. Deduction")]
        public float? EduAlloDed { get; set; }
        public bool IsEduAlloDed { get; set; }

        [Display(Name = "Pur Change")]
        public float? PurChange { get; set; }
        public bool IsPurChange { get; set; }

        [Display(Name = "Income Tax")]
        public float? IncomeTax { get; set; }
        public bool IsIncomeTax { get; set; }

        [Display(Name = "Arrear Income Tax Ded.")]
        public float? ArrearInTaxDed { get; set; }
        public bool IsArrearInTaxDed { get; set; }

        [Display(Name = "Off. Welfare Asso.")]
        public float? OffWlfareAsso { get; set; }
        public bool IsOffWlfareAsso { get; set; }

        [Display(Name = "Off. Welfare Asso.")]
        public float? OfficeclubDed { get; set; }
        public bool IsOfficeclubDed { get; set; }

        [Display(Name = "Incentive Bonus Ded.")]
        public float? IncBonusDed { get; set; }
        public bool IsIncBonusDed { get; set; }

        [Display(Name = "Water Change")]
        public float? Watercharge { get; set; }
        public bool IsWatercharge { get; set; }

        [Display(Name = "Chemical. Asso.")]
        public float? ChemicalAsso { get; set; }
        public bool IsChemicalAsso { get; set; }

        [Display(Name = "Adv. Income Tax Ded.")]
        public float? AdvInTaxDed { get; set; }
        public bool IsAdvInTaxDed { get; set; }

        [Display(Name = "Conv.Allow.Ded.")]
        public float? ConvAllowDed { get; set; }
        public bool IsConvAllowDed { get; set; }

        [Display(Name = "Ded.Inc.Bonus(O.P)")]
        public float? DedIncBonusOf { get; set; }
        public bool IsDedIncBonusOf { get; set; }

        [Display(Name = "Union Subcription")]
        public float? UnionSubDed { get; set; }
        public bool IsUnionSubDed { get; set; }

        [Display(Name = "Employee Club Ded.")]
        public float? EmpClubDed { get; set; }
        public bool IsEmpClubDed { get; set; }

        [Display(Name = "Medical Exp.")]
        public float? MedicalExp { get; set; }
        public bool IsMedicalExp { get; set; }

        [Display(Name = "Wages Advanc")]
        public float? WagesaAdv { get; set; }
        public bool IsWagesaAdv { get; set; }

        [Display(Name = "Medical Loan Ded.")]
        public float? MedicalLoanDed { get; set; }
        public bool IsMedicalLoanDed { get; set; }

        [Display(Name = "Adv.Against Wages Com.")]
        public float? AdvWagesDed { get; set; }
        public bool IsAdvWagesDed { get; set; }

        [Display(Name = "W.F.L(O.Proj.)")]
        public float? WFL { get; set; }
        public bool IsWFL { get; set; }

        [Display(Name = "Che. /A. Forum")]
        public float? CheForum { get; set; }
        public bool IsCheForum { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }

    }

    public partial class HrEmpShift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short ShiftId { get; set; }
        public string ComId { get; set; }
        public DateTime DtDate { get; set; }
        public long EmpId { get; set; }
        public string ShiftType { get; set; }
        public string ShiftCat { get; set; }
        public string Pcname { get; set; }
        public short LuserId { get; set; }

        //public virtual Cat_Company Com { get; set; }
        //public virtual HrEmpInfo Emp { get; set; }
    }

    public partial class HrLeaveAvail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long EmpId { get; set; }
        public string ComId { get; set; }
        public DateTime? DtInput { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime? DtTo { get; set; }
        public string InputType { get; set; }
        public double? TotalDay { get; set; }
        public string LvType { get; set; }
        public double LvApp { get; set; }
        public string Remark { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public string LuserId { get; set; }
        public int? WebLeaveId { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }

    public partial class HR_ProcessedData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int EmpId { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtPunchDate { get; set; }

        public int ShiftId { get; set; }
        public int DeptId { get; set; }
        public int SectId { get; set; }

        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Late { get; set; }

        [StringLength(5)]
        public string Status { get; set; }

        public float RegHour { get; set; }
        public float OTHour { get; set; }
        public float? OT { get; set; } = 0;
        public float OTHourDed { get; set; }
        public float ROT { get; set; }
        public float EOT { get; set; }
        public float StaffOT { get; set; }
        public float IsLunchDay { get; set; }
        public float IsNightShift { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShiftIn { get; set; }

        [StringLength(2)]
        public string AdJusted { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

    }

    public partial class HR_ProcessedDataB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int EmpId { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtPunchDate { get; set; }

        public int ShiftId { get; set; }
        public int DeptId { get; set; }
        public int SectId { get; set; }

        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Late { get; set; }

        [StringLength(5)]
        public string Status { get; set; }

        public float RegHour { get; set; }
        public float OTHour { get; set; }
        public float? OT { get; set; } = 0;
        public float OTHourDed { get; set; }
        public float ROT { get; set; }
        public float EOT { get; set; }
        public float StaffOT { get; set; }
        public float IsLunchDay { get; set; }
        public float IsNightShift { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShiftIn { get; set; }

        [StringLength(2)]
        public string AdJusted { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

    }


    public partial class HR_AttFixed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtPunchDate { get; set; }

        public int ShiftId { get; set; }
        // public string ShiftName { get; set; }

        //public virtual Cat_Shift Cat_Shift { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }

        public int? StatusId { get; set; }
        public string Status { get; set; }


        //public virtual Cat_AttStatus Cat_AttStatus { get; set; }

        public float OTHour { get; set; }

        public TimeSpan OTHourInTime { get; set; }

        public float OT { get; set; } = 0;

        [StringLength(50)]
        public string Remarks { get; set; }
        public bool IsInactive { get; set; }
        public TimeSpan TimeInPrev { get; set; }
        public TimeSpan TimeOutPrev { get; set; }
        public string StatusPrev { get; set; }
        public float OTHourPrev { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtTran { get; set; } = DateTime.Now;

        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }

    }





    public partial class HR_AttFixedB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtPunchDate { get; set; }

        public int ShiftId { get; set; }
        // public string ShiftName { get; set; }

        //public virtual Cat_Shift Cat_Shift { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }

        public int? StatusId { get; set; }
        public string Status { get; set; }


        //public virtual Cat_AttStatus Cat_AttStatus { get; set; }

        public float OTHour { get; set; }

        public TimeSpan OTHourInTime { get; set; }

        public float OT { get; set; } = 0;

        [StringLength(50)]
        public string Remarks { get; set; }
        public bool IsInactive { get; set; }
        public TimeSpan TimeInPrev { get; set; }
        public TimeSpan TimeOutPrev { get; set; }
        public string StatusPrev { get; set; }
        public float OTHourPrev { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtTran { get; set; } = DateTime.Now;

        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }

    }

    public partial class HR_AttFixedVM
    {

        public string ComId { get; set; }

        public int EmpId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtPunchDate { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public int? StatusId { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
    }

    public partial class HR_AttendanceProcess
    {
        [Display(Name = "Last Processing Date :")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtLast { get; set; }

        [Display(Name = "Enter Date To Process :")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtProcess { get; set; }

        [Display(Name = "To :")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtTo { get; set; }

        [Display(Name = "Day Type :")]
        public string dayType { get; set; }

        [Display(Name = "Ramadan Process")]
        public bool Ramadan { get; set; }

        [Display(Name = "Monthly Process")]
        public bool Monthly { get; set; }

        [Display(Name = "Employee Code :")]
        public int EmpId { get; set; }

        [Display(Name = "Name :")]
        public string EmpName { get; set; }

        [Display(Name = "Section :")]
        public int SectId { get; set; }

        [Display(Name = "Designation :")]
        public int DesigId { get; set; }


        public string optCriteria { get; set; }
    }

    public partial class HR_TempCount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TId { get; set; }

        [StringLength(50)]
        public string EmpId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public float Cnt { get; set; }
        public float Cnt1 { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Code1 { get; set; }

        public DateTime DateTime1 { get; set; }

        public DateTime DateTime2 { get; set; }
        public DateTime DateTime3 { get; set; }

        [StringLength(50)]
        public string Vchr { get; set; }

        [StringLength(50)]
        public string Vchr1 { get; set; }

        [StringLength(50)]
        public string Vchr2 { get; set; }

    }

    public partial class HR_ProssType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProssId { get; set; }


        [StringLength(80)]
        public string ComId { get; set; }

        public DateTime ProssDt { get; set; }


        [StringLength(10)]
        public string DaySts { get; set; }

        [StringLength(10)]
        public string DayStsB { get; set; }

        public string IsLock { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime DtTran { get; set; } = DateTime.Now;

    }


    public partial class HR_TempJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int TempCardId { get; set; }
        public string ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string BloodGroup { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Gs { get; set; }
        public short? DesigId { get; set; }
        public string DesigName { get; set; }
        public string Grade { get; set; }
        public short SectId { get; set; }
        public string SectName { get; set; }
        public short? DeptId { get; set; }
        public string DeptName { get; set; }
        public string Band { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public string DtJoin { get; set; }
        public string DtReleased { get; set; }
        public string CardNo { get; set; }
        public float? Present { get; set; }
        public float? Absent { get; set; }
        public float? LateDay { get; set; }
        public float? Leave { get; set; }
        public float? Hday { get; set; }
        public float? Wday { get; set; }
        public float? LateHrTtl { get; set; }
        public float? Othr { get; set; }
        public float? OthrDed { get; set; }
        public float? OthrsTtl { get; set; }
        public float? Rot { get; set; }
        public float? Eot { get; set; }
        //public int Slno { get; set; }
        public float? Night { get; set; }
        public float? Lunch { get; set; }
    }

    public partial class Payroll_SalaryAddition : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalAddId { get; set; }
        [DisplayName("Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [DisplayName("Input Date")]
        public DateTime DtInput { get; set; }

        [DisplayName("Join Date")]
        public DateTime DtJoin { get; set; }

        [StringLength(100)]
        [DisplayName("Others Addition Type")]
        public string OtherAddType { get; set; }

        [DisplayName("Addition Amount")]
        public double Amount { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }

    }

    public partial class Payroll_SalaryDeduction : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalDedId { get; set; }
        [DisplayName("Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [DisplayName("Deduction Date")]
        public DateTime DtInput { get; set; }

        [DisplayName("Join Date")]
        public DateTime DtJoin { get; set; }

        [StringLength(100)]
        [DisplayName("Others Deduction Type")]
        public string OtherDedType { get; set; }

        [DisplayName("Deduction Amount")]
        public double Amount { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
    }

    public class Payroll_Temp_SalaryDataInputWithFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(80), DefaultValue("0")]
        public string ComId { get; set; }
        [StringLength(50)]
        public string EmpCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime DtInput { get; set; }
        [Column(TypeName = "money")]
        public double Amount { get; set; }
        [StringLength(50)]
        public string OtherType { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        [DefaultValue("0")]
        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info Hr_Emp_Info { get; set; }

    }

    public partial class HR_Emp_ShiftInput : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftInputId { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [Display(Name = "Duty Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtDate { get; set; }

        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Shift")]
        public int ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public virtual Cat_Shift Cat_Shift { get; set; }

        [Display(Name = "Main Shift")]
        [NotMapped]
        public bool IsMain { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        public DateTime DtTran { get; set; } = DateTime.Now;
    }

    public partial class HR_ReportType : SelfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [Display(Name = "Report Name")]
        [StringLength(80)]
        public string ReportName { get; set; }

        [Display(Name = "Report Path")]
        [StringLength(150)]
        public string ReportPath { get; set; }

        [Display(Name = "Report Type")]
        [StringLength(40)]
        public string ReportType { get; set; }

        [Display(Name = "SL No")]
        public int? SLNo { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime DtTran { get; set; } = DateTime.Now;

    }

    public partial class HR_CustomReport : SelfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomReportId { get; set; }

        //[Display(Name = "Report Name")]
        //[StringLength(80)]
        //public string ReportName { get; set; }

        [Display(Name = "Report Name")]
        [StringLength(150)]
        public string ReportName { get; set; }

        [Display(Name = "Report Type")]
        [StringLength(40)]
        public string ReportType { get; set; }

        //[Display(Name = "SL No")]
        //public int? SLNo { get; set; }

        // FK
        [StringLength(80), ForeignKey("Companys")]
        public string ComId { get; set; }

        [ForeignKey("HR_ReportType")]
        public int ReportId { get; set; }
        [ForeignKey("Cat_Emp_Type")]
        public int EmpTypeId { get; set; }

        // navigation
        public virtual HR_ReportType HR_ReportType { get; set; }
        public virtual Company Company { get; set; }
        public virtual Cat_Emp_Type Cat_Emp_Type { get; set; }
    }


    public partial class HR_ProcessLock : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProcessId { get; set; }


        [Display(Name = "Lock Type")]
        [StringLength(50)]
        public string LockType { get; set; }

        [Display(Name = "Lock Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtDate { get; set; }

        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtToDate { get; set; }

        [Display(Name = "Fiscal Year")]
        public int? FiscalYearId { get; set; }
        [ForeignKey("FiscalYearId")]
        public virtual Acc_FiscalYear Acc_FiscalYears { get; set; }


        [Display(Name = "Fiscal Month")]
        public int? FiscalMonthId { get; set; }
        [ForeignKey("FiscalMonthId")]
        public virtual Acc_FiscalMonth Acc_FiscalMonths { get; set; }



        [Display(Name = "Is Lock")]
        public bool IsLock { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

    }

    public partial class HR_Loan_HouseBuilding : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanHouseId { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [Display(Name = "Emp.")]
        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }


        [Display(Name = "Loan Type")]
        [StringLength(50)]
        public string LoanType { get; set; }

        [Display(Name = "Loan Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanStart { get; set; }

        [Display(Name = "Loan End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanEnd { get; set; }

        [Display(Name = "Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanAmount { get; set; }

        [Display(Name = "Loan Term")]
        public float LoanTerm { get; set; }

        [Display(Name = "Interest Rate")]
        public float InterestRate { get; set; } = 0;

        [Display(Name = "Compound")]
        [StringLength(50)]
        public string Compound { get; set; }

        [Display(Name = "Pay Back")]
        [StringLength(50)]
        public string PayBack { get; set; }

        [Display(Name = "Monthly Deduct Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Monthly Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyInterest { get; set; }

        [Display(Name = "Total Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLoanAmount { get; set; }

        [Display(Name = "Total Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalInterest { get; set; } = 0;

        [Display(Name = "Is Inactive")]
        public bool Isinactive { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

        public virtual List<HR_Loan_Data_HouseBuilding> HR_Loan_Data_HouseBuildings { get; set; }
    }

    public partial class HR_Loan_Data_HouseBuilding
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanDataHouseId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int? LoanHouseId { get; set; }
        [ForeignKey("LoanHouseId")]
        public virtual HR_Loan_HouseBuilding HR_Loan_HouseBuilding { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Instalment")]
        public int? InstalmentNo { get; set; }


        [Display(Name = "Loan Month")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanMonth { get; set; }

        [Display(Name = "Beginning Loan Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BeginningLoanBalance { get; set; }

        [Display(Name = "Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InterestAmount { get; set; }

        [Display(Name = "Principal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Ending Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EndingBalance { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

    }





    public partial class HR_Loan_Welfare : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanWelId { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Loan Type")]
        [StringLength(50)]
        public string LoanType { get; set; }

        [Display(Name = "Loan Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanStart { get; set; }

        [Display(Name = "Loan End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanEnd { get; set; }

        [Display(Name = "Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanAmount { get; set; }

        [Display(Name = "Loan Term")]
        public float LoanTerm { get; set; }

        [Display(Name = "Interest Rate")]
        public float InterestRate { get; set; } = 0;

        [Display(Name = "Compound")]
        [StringLength(50)]
        public string Compound { get; set; }

        [Display(Name = "Pay Back")]
        [StringLength(50)]
        public string PayBack { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Monthly Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyInterest { get; set; }

        [Display(Name = "Total Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLoanAmount { get; set; }

        [Display(Name = "Total Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalInterest { get; set; } = 0;

        [Display(Name = "Is Inactive")]
        public bool Isinactive { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

        public virtual List<HR_Loan_Data_Welfare> HR_Loan_Data_Welfares { get; set; }
    }

    public partial class HR_Loan_Data_Welfare
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanDataWelId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int? LoanWelId { get; set; }
        [ForeignKey("LoanWelId")]
        public virtual HR_Loan_Welfare HR_Loan_Welfare { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Instalment")]
        public int? InstalmentNo { get; set; }


        [Display(Name = "Loan Month")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanMonth { get; set; }

        [Display(Name = "Beginning Loan Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BeginningLoanBalance { get; set; }

        [Display(Name = "Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InterestAmount { get; set; }

        [Display(Name = "Principal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Ending Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EndingBalance { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

    }



    public partial class HR_Loan_MotorCycle : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanMotorId { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Loan Type")]
        [StringLength(50)]
        public string LoanType { get; set; }

        [Display(Name = "Loan Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanStart { get; set; }

        [Display(Name = "Loan End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanEnd { get; set; }

        [Display(Name = "Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanAmount { get; set; }

        [Display(Name = "Loan Term")]
        public float LoanTerm { get; set; }

        [Display(Name = "Interest Rate")]
        public float InterestRate { get; set; } = 0;

        [Display(Name = "Compound")]
        [StringLength(50)]
        public string Compound { get; set; }

        [Display(Name = "Pay Back")]
        [StringLength(50)]
        public string PayBack { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Monthly Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyInterest { get; set; }

        [Display(Name = "Total Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLoanAmount { get; set; }

        [Display(Name = "Total Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalInterest { get; set; } = 0;

        [Display(Name = "Is Inactive")]
        public bool Isinactive { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

        public virtual List<HR_Loan_Data_MotorCycle> HR_Loan_Data_MotorCycles { get; set; }
    }

    public partial class HR_Loan_Data_MotorCycle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanDataMotorId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int? LoanMotorId { get; set; }
        [ForeignKey("LoanMotorId")]
        public virtual HR_Loan_MotorCycle HR_Loan_MotorCycle { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Instalment")]
        public int? InstalmentNo { get; set; }


        [Display(Name = "Loan Month")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanMonth { get; set; }

        [Display(Name = "Beginning Loan Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BeginningLoanBalance { get; set; }

        [Display(Name = "Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InterestAmount { get; set; }

        [Display(Name = "Principal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Ending Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EndingBalance { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

    }

    public partial class HR_Loan_PF : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanPFId { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Loan Type")]
        [StringLength(50)]
        public string LoanType { get; set; }

        [Display(Name = "Loan Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanStart { get; set; }

        [Display(Name = "Loan End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanEnd { get; set; }

        [Display(Name = "Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanAmount { get; set; }

        [Display(Name = "Loan Term")]
        public float LoanTerm { get; set; }

        [Display(Name = "Interest Rate")]
        public float InterestRate { get; set; } = 0;

        [Display(Name = "Compound")]
        [StringLength(50)]
        public string Compound { get; set; }

        [Display(Name = "Pay Back")]
        [StringLength(50)]
        public string PayBack { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Monthly Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyInterest { get; set; }

        [Display(Name = "Total Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLoanAmount { get; set; }

        [Display(Name = "Total Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalInterest { get; set; } = 0;

        [Display(Name = "Is Inactive")]
        public bool Isinactive { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

        public virtual List<HR_Loan_Data_PF> HR_Loan_Data_PFs { get; set; }
    }
    public partial class HR_Loan_Data_PF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanDataPFId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int? LoanPFId { get; set; }
        [ForeignKey("LoanPFId")]
        public virtual HR_Loan_PF HR_Loan_PF { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Instalment")]
        public int? InstalmentNo { get; set; }


        [Display(Name = "Loan Month")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanMonth { get; set; }

        [Display(Name = "Beginning Loan Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BeginningLoanBalance { get; set; }

        [Display(Name = "Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InterestAmount { get; set; }

        [Display(Name = "Principal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Ending Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EndingBalance { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

    }

    public partial class HR_Loan_Other : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanOtherId { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Loan Type")]
        [StringLength(50)]
        public string LoanType { get; set; }

        [Display(Name = "Loan Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanStart { get; set; }

        [Display(Name = "Loan End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanEnd { get; set; }

        [Display(Name = "Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanAmount { get; set; }

        [Display(Name = "Loan Term")]
        public float LoanTerm { get; set; }

        [Display(Name = "Interest Rate")]
        public float InterestRate { get; set; } = 0;

        [Display(Name = "Compound")]
        [StringLength(50)]
        public string Compound { get; set; }

        [Display(Name = "Pay Back")]
        [StringLength(50)]
        public string PayBack { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Monthly Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyInterest { get; set; }

        [Display(Name = "Total Loan Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalLoanAmount { get; set; }

        [Display(Name = "Total Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalInterest { get; set; } = 0;

        [Display(Name = "Is Inactive")]
        public bool Isinactive { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

        public virtual List<HR_Loan_Data_Other> HR_Loan_Data_Others { get; set; }
    }

    public partial class HR_Loan_Data_Other
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanDataOtherId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        public int? LoanOtherId { get; set; }
        [ForeignKey("LoanOtherId")]
        public virtual HR_Loan_Other HR_Loan_Other { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        [Display(Name = "Instalment")]
        public int? InstalmentNo { get; set; }


        [Display(Name = "Loan Month")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtLoanMonth { get; set; }

        [Display(Name = "Beginning Loan Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BeginningLoanBalance { get; set; }

        [Display(Name = "Interest")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InterestAmount { get; set; }

        [Display(Name = "Principal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }

        [Display(Name = "Payment Every Month")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyLoanAmount { get; set; }

        [Display(Name = "Ending Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EndingBalance { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DtTran { get; set; }

    }

    public partial class HR_OT_FC : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OtFcId { get; set; }

        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Pross Type")]
        [StringLength(20)]
        public string ProssType { get; set; }

        [Display(Name = "Over Time")]
        public float? ttlOT { get; set; }

        [Display(Name = "Food & Convince")]
        public float? ttlFC { get; set; }

        [Display(Name = "Night")]
        public float? ttlNight { get; set; }

        [Display(Name = "Shift Night")]
        public float? ttlShiftNight { get; set; }

        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtInput { get; set; }


    }

    public partial class HR_Emp_Recreation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime? DtPayment { get; set; }

        [Display(Name = "Reference Date")]
        public DateTime? DtReference { get; set; }



        [Display(Name = "Is Recreatoin")]
        public bool IsRecreation { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [StringLength(40)]
        public string ReferenceNo { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Emp_BusinessAllow : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BAId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Total Duty")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ttlBusinessDuty { get; set; }

        [Display(Name = "Rate")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [Display(Name = "From Date")]
        public DateTime? DtFrom { get; set; }

        [Display(Name = "To Date")]
        public DateTime? DtTo { get; set; }

        [StringLength(40)]
        public string Remarks { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }
    }

    public class HR_Emp_DocVM : BaseModel
    {
        public int Id { get; set; }
        public string RefCode { get; set; }
        public string Title { get; set; }
        public string VarType { get; set; }
        public string Remarks { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime DtInput { get; set; }
        public List<IFormFile>? FileToUpload { get; set; }
        public string PcName { get; set; }

        public List<string>? imageName { get; set; }

    }

    public partial class HR_Emp_Document : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Display(Name = "Employee")]
        //public int EmpId { get; set; }
        //[ForeignKey("EmpId")]
        //public HR_Emp_Info HR_Emp_Info { get; set; }

        [StringLength(20)]
        [Display(Name = "Reference Code")]
        [Required]
        public string RefCode { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(25)]
        [Display(Name = "Document Type")]
        public string VarType { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        [StringLength(300)]
        public string FilePath { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Input Date")]
        [Required]
        public DateTime DtInput { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select a file!")]
        public IFormFile FileToUpload { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Please select a Image!")]
        public IFormFile ImageTest { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }



        //[StringLength(80)]
        //public string ComId { get; set; }


        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Loan_IncreaseInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        //[ForeignKey("EmpId")]
        //public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [StringLength(20)]
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }

        [StringLength(50)]
        [Display(Name = "Other Loan Type")]
        public string OtherLoanType { get; set; }

        [Required]
        [Display(Name = "Increase Month")]
        public int TtlIncreaseMonth { get; set; }

        [Required]
        [Display(Name = "Effective Month")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtEffectiveMonth { get; set; }

        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [StringLength(15)]
        [Display(Name = "Criteria")]
        public string InputType { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Emp_TempIdCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string ComId { get; set; }
    }

    public partial class HR_SummerWinterAllowance : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }


        [Display(Name = "Month")]
        public int SWAllowanceId { get; set; }
        [ForeignKey("SWAllowanceId")]
        public virtual Cat_SummerWinterAllowanceSetting Cat_SWSetting { get; set; }

        [Display(Name = "Is Summer")]
        public bool IsSummer { get; set; }

        [Display(Name = "Is Winter")]
        public bool IsWinter { get; set; }

        [Display(Name = "Is Raincoat")]
        public bool IsRaincoat { get; set; }

        [Display(Name = "Summer Allow")]
        public float? SummerAllow { get; set; }

        [Display(Name = "Winter Allow")]
        public float? WinterAllow { get; set; }

        [Display(Name = "Rain Coat Allow")]
        public float? RainCoatAndGumbootAllow { get; set; }

        [Display(Name = "Vat Ded")]
        public float? VatDed { get; set; }

        [Display(Name = "Tax Ded")]
        public float? TaxDed { get; set; }

        [Display(Name = "Amount")]
        public float? Amount { get; set; }

        [Display(Name = "Stamp")]
        public float? Stamp { get; set; }

        [Display(Name = "Net Amount")]
        public float? NetAmount { get; set; }


        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtInput { get; set; }


        //[StringLength(80)]
        //public string ComId { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
    }
    public partial class HR_LvEncashment : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LvEncashmentId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "From Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtFrom { get; set; }

        [Display(Name = "To Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtTo { get; set; }

        [Display(Name = "From Next Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtFromNext { get; set; }

        [Display(Name = "To Next Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtToNext { get; set; }

        [Display(Name = "Total Days")]
        public int TotalDays { get; set; }

        [Display(Name = "EL Balance")]
        public int ELBalance { get; set; }

        [Display(Name = "Is EL Enjoy")]
        public bool IsELEnjoy { get; set; }

        [Display(Name = "Leave Encashment Year")]
        public int LvEncashmentIYear { get; set; }

        [Display(Name = "Reference No")]
        [StringLength(40)]
        public string ReferenceNo { get; set; }


        [Display(Name = "Payment Date")]
        public DateTime? DtInput { get; set; }


        public float? Amount { get; set; }
        [Display(Name = "Net Amount")]
        public float? NetAmount { get; set; }
        public int? Stamp { get; set; }

        [StringLength(20)]
        public string ProssType { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Emp_Suppliment : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplimentId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtInput { get; set; }

        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtFrom { get; set; }

        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtTo { get; set; }


        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Display(Name = "Basic")]
        public float Basic { get; set; }

        [Display(Name = "Is Basic")]
        public bool IsBS { get; set; }

        [Display(Name = "Is HR")]
        public bool IsHR { get; set; }

        [Display(Name = "Is Wash")]
        public bool IsWash { get; set; }

        [Display(Name = "Is MA")]
        public bool IsMA { get; set; }

        [Display(Name = "Is Co. PF")]
        public bool IsCPF { get; set; }

        [Display(Name = "Is Risk")]
        public bool IsRisk { get; set; }

        [Display(Name = "Is Edu.")]
        public bool IsEdu { get; set; }

        [Display(Name = "Is HR Exp Ded.")]
        public bool IsHRExpDed { get; set; }

        [Display(Name = "Persantage")]
        public int Persantage { get; set; }

        [Display(Name = "Is OPF")]
        public bool IsOPF { get; set; }

        [Display(Name = "Is AddPF")]
        public bool IsAddPF { get; set; }

        [Display(Name = "Is OA")]
        public bool IsOA { get; set; }

        [Display(Name = "Is WF Sub")]
        public bool IsWFSub { get; set; }

        [Display(Name = "Is RS")]
        public bool IsRS { get; set; }

        [Display(Name = "Is HB Loan")]
        public bool IsHBLoan { get; set; }

        [Display(Name = "Is MC Loan")]
        public bool IsMCLoan { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_OtherDedAssociation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OtherDedAssId { get; set; }

        [StringLength(30)]
        [Display(Name = "Pross Type")]
        public string ProssType { get; set; }

        [StringLength(100)]
        [Display(Name = "Other Ded Ass. Name")]
        public string OtherDedName { get; set; }

        [StringLength(100)]
        [Display(Name = "Other Ded Ass. Bangla")]
        public string OtherDedNameB { get; set; }

        [StringLength(50)]
        [Display(Name = "Account No")]
        public string AccountNo { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_PFContribution : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PFContributionId { get; set; }

        [DisplayName("Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [DisplayName("PF Date")]
        public DateTime DtInput { get; set; }

        [DisplayName("Join Date")]
        public DateTime DtJoin { get; set; }

        [StringLength(100)]
        [DisplayName("PF")]
        public string PF { get; set; }

        [DisplayName("Amount")]
        public double Amount { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
    }

    public partial class HR_Loan_Return : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanReturnId { get; set; }

        [DisplayName("Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [DisplayName("Return Date")]
        public DateTime DtInput { get; set; }

        [DisplayName("Join Date")]
        public DateTime DtJoin { get; set; }

        [StringLength(100)]
        [DisplayName("Loan Type")]
        public string LoanType { get; set; }

        [DisplayName("Return Amount")]
        public double Amount { get; set; }

        //[StringLength(80)]
        //public string ComId { get; set; }

        [StringLength(60)]
        public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
    }


    public partial class HR_Emp_ArrearBill : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArrBillId { get; set; }

        //[StringLength(128)]
        //public string ComId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }


        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please fill up input date")]
        public DateTime DtInput { get; set; }

        [Display(Name = "From Date")]
        [Required(ErrorMessage = "Please fill up from date")]
        public DateTime DtFrom { get; set; }

        [Display(Name = "To Date")]
        [Required(ErrorMessage = "Please fill up to date")]
        public DateTime DtTo { get; set; }

        [Display(Name = "Total Month")]
        public float TtlMonth { get; set; }

        [Display(Name = "Total Day")]
        public float TtlDay { get; set; }

        [Display(Name = "From Month")]
        public DateTime DtFromMonth { get; set; }

        [Display(Name = "To Month")]
        public DateTime DtToMonth { get; set; }

        [Display(Name = "New BS")]
        public float? NewBS { get; set; }

        [Display(Name = "Old BS")]
        public float? OldBS { get; set; }

        [Display(Name = "BS Diff.")]
        public float? BSDiff { get; set; }

        [Display(Name = "Total Arr. BS")]
        public float? TtlArrearBS { get; set; }

        [Display(Name = "New HR")]
        public float? NewHR { get; set; }

        [Display(Name = "Old HR")]
        public float? OldHR { get; set; }

        [Display(Name = "HR Diff.")]
        public float? HRDiff { get; set; }

        [Display(Name = "Total Arr. HR")]
        public float? TtlArrearHR { get; set; }

        [Display(Name = "New MA")]
        public float? NewMA { get; set; }

        [Display(Name = "Old MA")]
        public float? OldMA { get; set; }

        [Display(Name = "MA Diff.")]
        public float? MADiff { get; set; }

        [Display(Name = "Total Arr. MA")]
        public float? TtlArrearMA { get; set; }

        [Display(Name = "New Edu Allow")]
        public float? NewEduAllow { get; set; }

        [Display(Name = "Old Edu Allow")]
        public float? OldEduAllow { get; set; }

        [Display(Name = "Edu Allow Diff.")]
        public float? EduAllowDiff { get; set; }

        [Display(Name = "Total Arr. Edu Allow")]
        public float? TtlArrearEduAllow { get; set; }

        [Display(Name = "New Tiffin Allow")]
        public float? NewTiffinAllow { get; set; }

        [Display(Name = "Old Tiffin Allow")]
        public float? OldTiffinAllow { get; set; }

        [Display(Name = "Tiffin Allow Diff.")]
        public float? TiffinAllowDiff { get; set; }

        [Display(Name = "Total Arr. Tiffin Allow")]
        public float? TtlArrearTiffinAllow { get; set; }

        [Display(Name = "New Risk Allow")]
        public float? NewRiskAllow { get; set; }

        [Display(Name = "Old Tiffin Allow")]
        public float? OldRiskAllow { get; set; }

        [Display(Name = "Risk Allow Diff.")]
        public float? RiskAllowDiff { get; set; }

        [Display(Name = "Total Arr. RiskAllow")]
        public float? TtlArrearRiskAllow { get; set; }

        [Display(Name = "New PF")]
        public float? NewPF { get; set; }

        [Display(Name = "Old PF")]
        public float? OldPF { get; set; }

        [Display(Name = "PF Diff.")]
        public float? PFDiff { get; set; }

        [Display(Name = "Total Arr. PF")]
        public float? TtlPF { get; set; }

        [Display(Name = "Total Arr. Payable")]
        public float? TotalArrearPayable { get; set; }



        [Display(Name = "DA Allow Ded.")]
        public float? DAAllowDed { get; set; }

        [Display(Name = "Advance")]
        public float? Adv { get; set; }

        [Display(Name = "Stamp")]
        public float? Stamp { get; set; }

        [Display(Name = "Total Arr. Ded.")]
        public float? TotalArrearDeduction { get; set; }

        [Display(Name = "Net Arr. Payable")]
        public float? NetArrearPayable { get; set; }


        [Display(Name = "Income Tax")]
        public float? IncomeTax { get; set; }

        [Display(Name = "PF Add")]
        public float? PFAdd { get; set; }

        [Display(Name = "New Was. Allow")]
        public float? NewWashingAllow { get; set; }

        [Display(Name = "Old Was. Allow")]
        public float? OldWashingAllow { get; set; }

        [Display(Name = "Washing Allow Diff")]
        public float? WashingAllowDiff { get; set; }

        [Display(Name = "Ttl Arr. Was. Allow")]
        public float? ttlArrearWashingAllow { get; set; }

        [Display(Name = "Last Month Days")]
        public float? LastMonthDays { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        //[StringLength(80)]
        //public string UserId { get; set; }
        //public DateTime? DateAdded { get; set; }
        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }

    }

    public partial class HR_ProssType_WHDay : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WHId { get; set; }

        public DateTime? dtPunchDate { get; set; }

        [StringLength(20)]
        public string DaySts { get; set; }

        [StringLength(20)]
        public string DayStsB { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        //[StringLength(128)]
        //public string ComId { get; set; }
        //[StringLength(128)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }
        //[StringLength(128)]
        //public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }
    }


    public partial class HR_Emp_ML
    {
        [Key]
        public int MLId { get; set; }

        public int LvId { get; set; }
        [ForeignKey("LvId")]
        public virtual HR_Leave_Avail HR_Leave_Avail { get; set; }

        [StringLength(128)]
        [Required]
        public string ComId { get; set; }

        [Required]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Required, Display(Name = "DesigName")]
        public int DesigId { get; set; }
        [ForeignKey("DesigId")]

        [Required]
        public virtual Cat_Designation Cat_Designation { get; set; }
        [Required]
        public int SectId { get; set; }
        [ForeignKey("SectId")]
        public virtual Cat_Section Cat_Section { get; set; }


        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please fill up input date")]

        public DateTime DtInput { get; set; }


        [Display(Name = "From Date"),
           DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        [Required(ErrorMessage = "Please fill up from date")]
        public DateTime DtFrom { get; set; }


        [Required, Display(Name = "To Date"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        public DateTime? DtTo { get; set; }



        [Display(Name = "InputType")]
        [StringLength(10)]
        public string InputType { get; set; }


        [Required]
        [Display(Name = "Total Day")]
        public double TotalDay { get; set; }


        [Display(Name = "Leave Type")]
        public string lvType { get; set; }


        [Required]
        [Display(Name = "Leave App")]
        public double lvApp { get; set; }




        [Display(Name = "employee name")]
        public string EmpName { get; set; }


        [Display(Name = "Sectname")]
        public string SecNname { get; set; }


        [Display(Name = "Emp Code")]
        public string Empcode { get; set; }



        [Required]
        [Display(Name = "GS")]
        public decimal GS { get; set; }
        [Required]
        [Display(Name = "BS")]
        public decimal BS { get; set; }

        [Display(Name = "FirstSalMonth")]
        public string FirstSalMonth { get; set; }

        [Display(Name = "SecondSalMonth")]
        public string SecondSalMonth { get; set; }

        [Display(Name = "ThirdSalMonth")]
        public string ThirdSalMonth { get; set; }
        [Required]
        [Display(Name = "FirstAmt")]
        public decimal FirstAmt { get; set; }

        [Required]
        [Display(Name = "SecondAmt")]
        public decimal SecondAmt { get; set; }
        [Required]
        [Display(Name = "ThirdAmt")]
        public decimal ThirdAmt { get; set; }

        [Required]
        [Display(Name = "FirstDays")]
        public Single FirstDays { get; set; }

        [Required]
        [Display(Name = "SecondDays")]
        public Single SecondDays { get; set; }
        [Required]
        [Display(Name = "ThirdDays")]
        public Single ThirdDays { get; set; }

        [Required]
        [Display(Name = "TtlDays")]
        public Single TtlDays { get; set; }

        [Required]
        [Display(Name = "FirstPayment")]
        public decimal FirstPayment { get; set; }

        [Required]
        [Display(Name = "LastPayment")]
        public decimal LastPayment { get; set; }

        [Required]
        [Display(Name = "TtlAmount")]
        public decimal TtlAmount { get; set; }
        [Required]
        [Display(Name = "OtherBonus")]
        public decimal OtherBonus { get; set; }

        [Required]
        [Display(Name = "OtherDeduct")]
        public decimal OtherDeduct { get; set; }
        [Required]
        [Display(Name = "NetPayable")]
        public decimal NetPayable { get; set; }

        [Required]
        [Display(Name = "FirstPaid")]
        public bool FirstPaid { get; set; }
        [Required]
        [Display(Name = "LastPaid")]
        public bool LastPaid { get; set; }
        [Required]
        [Display(Name = "NoPay")]
        public bool NoPay { get; set; }

        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }



        public string PcName { get; set; }
        [StringLength(80)]
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        [StringLength(80)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }

    }

    public partial class HR_Emp_ML_View
    {
        public int MLId { get; set; }
        public int LvId { get; set; }

        public int EmpId { get; set; }

        public string EmpCode { get; set; }
        public string EmpName { get; set; }

        public string SectName { get; set; }
        public string DesigName { get; set; }

        [Required, Display(Name = "DtInput"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        public DateTime DtInput { get; set; }

        [Required, Display(Name = "DtFrom"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        public DateTime DtFrom { get; set; }

        [Required, Display(Name = "DtTo"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        public DateTime? DtTo { get; set; }

        public string InputType { get; set; }

        public double TotalDay { get; set; }

        public string lvType { get; set; }

        public double? lvApp { get; set; }

        public decimal GS { get; set; }

        public decimal BS { get; set; }

        public string FirstSalMonth { get; set; }

        public string SecondSalMonth { get; set; }

        public string ThirdSalMonth { get; set; }

        public decimal? FirstAmt { get; set; }

        public decimal? SecondAmt { get; set; }

        public decimal? ThirdAmt { get; set; }

        public Single? FirstDays { get; set; }

        public Single? SecondDays { get; set; }

        public Single? ThirdDays { get; set; }

        public Single? TtlDays { get; set; }

        public decimal? FirstPayment { get; set; }

        public decimal? LastPayment { get; set; }

        public decimal? TtlAmount { get; set; }

        [DisplayName("Addition")]
        public decimal? OtherBonus { get; set; }

        [DisplayName("Deduction")]
        public decimal? OtherDeduct { get; set; }

        public decimal? NetPayable { get; set; }

        public bool FirstPaid { get; set; }

        public bool LastPaid { get; set; }

        public bool NoPay { get; set; }

        public string Remarks { get; set; }

        public string MLRemarks { get; set; }

        public virtual HR_Emp_Info hrEmpinfo { get; set; }

    }
    public partial class HRGetMLViewModel
    {
        [Key]

        public int lvId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtFrom { get; set; }
        public string DtTo { get; set; }
        public double? TotalDay { get; set; }
        public string LvType { get; set; }
        public double? LvApp { get; set; }
        public decimal? FirstPayment { get; set; }
        public decimal? LastPayment { get; set; }
        public string Remarks { get; set; }

    }
    public partial class HR_Production
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrdnId { get; set; }
        public int EmpId { get; set; }
        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please fill up date")]
        public DateTime dtInput { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Please Input Quantity")]
        public float Quantity { get; set; }
        [Display(Name = "Amount")]
        public float Amount { get; set; }

        public int? StyleId { get; set; }
        [StringLength(80)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
    public partial class HR_Sal_Suspense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvId { get; set; }
        public int EmpId { get; set; }
        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please fill up date")]
        public DateTime dtInput { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Please Input Amount")]
        public float Amount { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        [StringLength(128)]
        public string ComId { get; set; }
        [StringLength(128)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }
        [StringLength(128)]
        public string UpdateByUserId { get; set; }
        public DateTime? DateUpdated { get; set; }
    }


    public partial class Hr_Emp_AbsentLetter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RefId { get; set; }

        [StringLength(80)]
        public string ComId { get; set; } = "";

        [Display(Name = "Employee")]
        [Required]
        public int? EmpId { get; set; }

        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "From"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        public DateTime? DtFrom { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "To"),
        DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        public DateTime? DtTo { get; set; }


        [Required, StringLength(100)]
        public string ShowCauseLetterRef { get; set; }

        [StringLength(100)]
        public string SelfDefenceRef { get; set; }
        [StringLength(100)]
        public string TerminationLetterRef { get; set; }
        [Column(TypeName = "date"), Display(Name = "Date"),
         DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd")]
        public DateTime? DtFirst { get; set; }
        public DateTime? DtSecond { get; set; }
        public DateTime? DtThird { get; set; }

    }


    //for pbc automation

    public class PBC_TaskAssign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskAssignId { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Quarter { get; set; }
        [Column(TypeName = "datetime"), Required]
        public DateTime AssignDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SubmiteDate { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string Type_T_Task { get; set; }
        public string Type_T_Description { get; set; }
        public string Type_M_Task { get; set; }
        public string Type_M_Description { get; set; }
        public string Type_K_Task { get; set; }
        public string Type_K_Description { get; set; }
        public string Type_BV_Task { get; set; }
        public string Type_BV_Description { get; set; }
        public string ComId { get; set; }
        public string UserId { get; set; }
        public string Documents { get; set; }
        //FK
        [ForeignKey("HR_Emp_Info")]
        public int EmpId { get; set; }
        //navigation
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }
    public class PBC_EmployeeTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeTaskId { get; set; }
        public string Files { get; set; }
        public string User_Comments { get; set; }
        public bool IsComplete { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime StatusDate { get; set; }
        public string HODRemarks { get; set; }
        public string MNGRemarks { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ComId { get; set; }
        public string UserId { get; set; }
        //FK
        [ForeignKey("HR_Emp_Info")]
        public int EmpId { get; set; }
        [ForeignKey("PBC_TaskAssign")]
        public int TaskAssignId { get; set; }
        [ForeignKey("PBC_Status")]
        public int PBCStatusId { get; set; }
        //navigation
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        public virtual PBC_TaskAssign PBC_TaskAssign { get; set; }
        public virtual PBC_Status PBC_Status { get; set; }

    }

    public class PBC_Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PBCStatusId { get; set; }
        public string Status { get; set; }
    }

    // For Fixed Attendance
    public class HR_TempFixAttExcel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string ComId { get; set; }
        public string ShiftName { get; set; }
        public string DtPunchDate { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string Status { get; set; }
        public string? Remarks { get; set; }
        public string? OtHour { get; set; }
        public float? OT { get; set; }

    }

    // For Overtime Setting

    public class HR_OverTimeSetting : SelfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool IsAllowMinute { get; set; }
        public bool IsManualOT { get; set; }
        public double OTRate { get; set; }
        public int GraceTimeFrom { get; set; }

        public int GraceTimeTo { get; set; }

        public int From1 { get; set; }

        public int To1 { get; set; }

        public float OT1 { get; set; }
        public int From2 { get; set; }

        public int To2 { get; set; }

        public float OT2 { get; set; }

        public int From3 { get; set; }

        public int To3 { get; set; }

        public float OT3 { get; set; }

        public int From4 { get; set; }

        public int To4 { get; set; }

        public float OT4 { get; set; }

        public int OTStart { get; set; }
        [ForeignKey("Companys")]
        public string CompanyId { get; set; }


    }

    // for Increment entry
    public class HR_TempIncrementExcel
    {
        public int Id { get; set; }
        public string ComId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime DtJoin { get; set; }
        public float OldSalary { get; set; }
        public float Amount { get; set; }
        public float NewSalary { get; set; }
        public float NewBS { get; set; }
        public float NewHR { get; set; }
        public float NewMA { get; set; }
        public float NewFA { get; set; }
        public float NewTA { get; set; }
        public string NewDeptName { get; set; }
        public string NewSectName { get; set; }
        public string NewDesigName { get; set; }
        public string NewGradeName { get; set; }
        public DateTime DtIncrement { get; set; }
        public string IncType { get; set; }
    }

    // For Leave Balance

    public class HR_TempLeaveBalanceExcel
    {
        public int Id { get; set; }
        public string ComId { get; set; }
        public string EmpCode { get; set; }
        public string ELYear { get; set; }
        public decimal EL { get; set; }
        public decimal CL { get; set; }
        public decimal SL { get; set; }
        public decimal PrevELBal { get; set; }
    }

    // For Check Button Handle

    public class Module_Menu_Action
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ComId { get; set; }
        public bool IsDelete { get; set; }
        [ForeignKey("ModuleMenu")]
        public int ModuleMenuId { get; set; }
        public virtual ModuleMenu ModuleMenu { get; set; }
    }

    // For Approval Setting 

    public class HR_ApprovalSetting : SelfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovalSettingId { get; set; }
        public string ComId { get; set; }
        public string UserId { get; set; }
        public int ApprovalType { get; set; }
        public bool IsApprove { get; set; }
        public bool IsFirstLeaveApprove { get; set; }
    }

    // For Employee Excel File Import
    public class TempEmpData
    {
        public int Id { get; set; }
        public string ComId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpNameB { get; set; } = "N/A";
        public string CardNo { get; set; } = "N/A";
        public string Department { get; set; } = "N/A";
        public string DepartmentB { get; set; } = "N/A";
        public string Section { get; set; } = "N/A";
        public string SectionB { get; set; } = "N/A";
        public string Designation { get; set; } = "N/A";
        public string DesignationB { get; set; } = "N/A";
        public string Grade { get; set; } = "N/A";
        public string Unit { get; set; } = "N/A";
        public string EmpType { get; set; } = "N/A";
        public string TinNo { get; set; } = "N/A";
        public string NID { get; set; } = "N/A";
        public string Mobile { get; set; } = "N/A";
        public string PayMode { get; set; } = "N/A";
        public string BankAccountNo { get; set; } = "N/A";
        public string Shift { get; set; } = "N/A";
        public float GS { get; set; } = 0;
        public float BS { get; set; } = 0;
        public float HR { get; set; } = 0;
        public float MA { get; set; } = 0;
        public float TA { get; set; } = 0;
        public float FA { get; set; } = 0;
        public string Sex { get; set; } = "N/A";
        public string Religion { get; set; } = "N/A";
        public DateTime JoinDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Line { get; set; } = "N/A";
        public string Floor { get; set; } = "N/A";
        public string MaritalStatus { get; set; } = "N/A";
        public string SpouseName { get; set; } = "N/A";
        public string SpouseContactNo { get; set; } = "N/A";
        public string ChildNo { get; set; } = "N/A";
        public string FatherName { get; set; } = "N/A";
        public string MotherName { get; set; } = "N/A";
        public string EmergencyContactNo { get; set; } = "N/A";
        public string EducationQualification { get; set; } = "N/A";
        public string BloodGroup { get; set; } = "N/A";
        public string NomineeName { get; set; } = "N/A";
        public string NomineeAddress { get; set; } = "N/A";
        public string NomineeMobileNo { get; set; } = "N/A";
        public string RelationWithNominee { get; set; } = "N/A";
        public string PresentDistrict { get; set; } = "N/A";
        public string PresentPS { get; set; } = "N/A";
        public string PresentPO { get; set; } = "N/A";
        public string PresentVill { get; set; } = "N/A";
        public string PresentVillB { get; set; } = "N/A";
        public string PerVill { get; set; } = "N/A";
        public string PerVillB { get; set; } = "N/A";
        public string PerPO { get; set; } = "N/A";
        public string PerPS { get; set; } = "N/A";
        public string PerDistrict { get; set; } = "N/A";
        public string FatherNameB { get; set; } = "N/A";
        public string MotherNameB { get; set; } = "N/A";
        public string SpouseNameB { get; set; } = "N/A";
    }

    ///[HR_RptAttendSumEntry]
    public class Daily_req_entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Comid { get; set; }
        public int deptid { get; set; }
        public string DeptName { get; set; }
        public int unitid { get; set; }
        public string Job_Loc { get; set; }
        public int desigid { get; set; }
        public string Job_Nat { get; set; }

        public int SectId { get; set; }
        public string Cost_head { get; set; }
        public int Sup_A { get; set; }
        public int Sup_G { get; set; }
        public int Sup_B { get; set; }
        public int Sup_C { get; set; }
        public int Exc_A { get; set; }
        public int Exc_G { get; set; }
        public int Exc_B { get; set; }
        public int Exc_C { get; set; }
        public int Wor_A { get; set; }
        public int Wor_G { get; set; }
        public int Wor_B { get; set; }
        public int Wor_C { get; set; }
        public DateTime dateTime { get; set; }

    }
    public class Daily_req_entryVM
    {
        public int ID { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public int deptid { get; set; }
        public string DeptName { get; set; }
        public int unitid { get; set; }
        public string Job_Loc { get; set; }
        public int desigid { get; set; }
        public string Job_Nat { get; set; }
        public int SectId { get; set; }
        public string Cost_head { get; set; }
        public int Sup_A { get; set; }
        public int Sup_G { get; set; }
        public int Sup_B { get; set; }
        public int Sup_C { get; set; }
        public int Exc_A { get; set; }
        public int Exc_G { get; set; }
        public int Exc_B { get; set; }
        public int Exc_C { get; set; }
        public int Wor_A { get; set; }
        public int Wor_G { get; set; }
        public int Wor_B { get; set; }
        public int Wor_C { get; set; }


    }

    public class HR_TempReleasedExcel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ComId { get; set; }
        public string EmpCode { get; set; }
        public DateTime dtReleased { get; set; }
        public DateTime dtPresentLast { get; set; }
        public DateTime dtSubmit { get; set; }
        public string Remarks { get; set; }
        public string RelType { get; set; }



    }

    public class HR_PF_Cheque : BaseModel
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }


        [Display(Name = "Cheque Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ChequeDate { get; set; }


        [Display(Name = "Cheque No")]

        public string ChequeNo { get; set; }

        [Display(Name = " Return Cheque Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnChequeDate { get; set; }

        [Display(Name = "Return Cheque No")]
        [StringLength(50)]
        public string ReturnChequeNo { get; set; }

        [Display(Name = "Return Cheque AMT")]
        public float ReturnChequeAmt { get; set; }

        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsInactive { get; internal set; }
    }
    public class HR_PFCheque : BaseModel
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PFId { get; set; }


        [Display(Name = "Employee ID")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }


        [Display(Name = "Cheque Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ChequeDate { get; set; }


        [Display(Name = "Cheque No")]

        public string ChequeNo { get; set; }

        [Display(Name = " Return Cheque Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnChequeDate { get; set; }

        [Display(Name = "Return Cheque No")]
        [StringLength(50)]
        public string ReturnChequeNo { get; set; }

        [Display(Name = "Return Cheque AMT")]
        public float ReturnChequeAmt { get; set; }

        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsInactive { get; internal set; }
    }

    public class HR_Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NtfId { get; set; }

        [Display(Name = "Notification Title")]
        public string Title { get; set; }
        [Display(Name = "Notification Body")]
        public string Body { get; set; }
        [Display(Name = "Company Id")]
        public string ComId { get; set; }
        public DateTime execTime { get; set; }
    }

    public class HR_Notify
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotifyId { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        public int NtfId { get; set; }
        [ForeignKey("NtfId")]
        public virtual HR_Notification HR_Notification { get; set; }
        public string ComId { get; set; }
        public string? EmpName { get; set; }
        public string? EmpMobile { get; set; }
        public string? Email { get; set; }
        public string? SMSText { get; set; }
        public string? EmailText { get; set; }
        public string? WhatsAppText { get; set; }
        public string? MobileAppText { get; set; }
        public int? IsEmail { get; set; }
        public int? IsSMS { get; set; }
        public int? IsWhatsApp { get; set; }
        public int? IsMobileApp { get; set; }
        public DateTime? DtEntry { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailCc { get; set; }
        public string? EmailBCc { get; set; }

    }

    public class EmployeeListFilterData
    {
        public string? EmpName { get; set; }
        public string? EmpCode { get; set; }
        public string? EmpNameB { get; set; }
        public string? EmpFather { get; set; }
        public string? EmpFatherB { get; set; }
        public string? EmpMother { get; set; }
        public string? EmpMotherB { get; set; }
        public string? EmpSpouse { get; set; }
        public string? EmpSpouseB { get; set; }

        public string? HouseType { get; set; }
        public string? ReligionName { get; set; }
        public string? BloodName { get; set; }
        public string? UnitName { get; set; }
        public string? ShitfName { get; set; }
        public string? DeptName { get; set; }
        public string? ShiftName { get; set; }
        public string? CurrVillName { get; set; }
        public string? CurrDistName { get; set; }
        public string? CurrPStationName { get; set; }
        public string? CurrPOName { get; set; }
        public string? EmpCurrCityVill { get; set; }
        public string? EmpPerCityVill { get; set; }

        public string? PerVillName { get; set; }
        public string? PerDistName { get; set; }
        public string? PerPStationName { get; set; }
        public string? PerPOName { get; set; }
        //public List<HR_Emp_Experience> Experiences { get; set; }
        //public List<HR_Emp_Education> Educations { get; set; }

        public string? Experiences { get; set; }
        public string? Educations { get; set; }
        public string? DesigName { get; set; }
        public string? SectName { get; set; }
        public string? SubSectName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtBirth { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtJoin { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtIncrement { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtConfirm { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped]
        public DateTime? DtReleased { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtPf { get; set; }
        public string? EmpTypeName { get; set; }
        public string? GenderName { get; set; }
        public string? NID { get; set; }
        public string? FingerId { get; set; }
        public string? EmpPhone1 { get; set; }
        public string? EmpPhone2 { get; set; }
        public bool? IsInactive { get; set; }
        public string? EmpPerZip { get; set; }
        public string? EmpEmail { get; set; }
        public string? EmpRemarks { get; set; }
        public string? GradeName { get; set; }
        public string? FloorName { get; set; }
        public string? LineName { get; set; }
        public bool? IsAllowOT { get; set; }
        public int? ManageType { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtLocalJoin { get; set; }
        public string? EmpNomineeName1 { get; set; }
        public string? EmpNomineeMobile1 { get; set; }
        public string? EmpNomineeNID1 { get; set; }
        public string? EmpNomineeRelation1 { get; set; }
        public string? EmpNomineeAddress1 { get; set; }
        public string? EmpNomineeName2 { get; set; }
        public string? EmpNomineeMobile2 { get; set; }
        public string? EmpNomineeNID2 { get; set; }
        public string? EmpNomineeRelation2 { get; set; }
        public string? EmpNomineeAddress2 { get; set; }
        public string? Skill { get; set; }
        public int? pageIndex { get; set; }
        public int? pageSize { get; set; }

    }

    public class Cat_JobCardDynamic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JcdId { get; set; }
        public string ComId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? dtFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? dtTo { get; set; }
        [Display(Name = "OT Hour Limit")]
        public float? OTHrLimit { get; set; }
        [Display(Name = "Type")]
        public string? Type { get; set; }

        [Display(Name = "Is Hide WH Day OT")]
        public bool IsHideWHDayOT { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
        public string? userId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string? UpdateByUserId { get; set; }
        public DateTime? Dateupdated { get; set; }

    }

    public class Cat_MailSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MSId { get; set; }
        public string ComId { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string SenderAddress { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsApplicant { get; set; }
        public bool IsHOD { get; set; }
        public bool IsFirstApprvd { get; set; }
        public bool IsRejected { get; set; }
        //public int Cancellation { get; set; }
        public string? CompanyTitle { get; set; }
        public bool IsHR { get; set; }

    }

    public class HR_Leave_Adjust
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime dtJoin { get; set; }
        public DateTime DtFrom { get; set; }
        public string Remark { get; set; }
        public string ComId { get; set; }
    }

    public partial class HR_PF_Withdrawn : BaseModel
    {
        //[StringLength(80)]
        //public string ComId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WdId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        [Display(Name = "Withdrawn Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtWithdrawn { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(120)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        //[Display(Name = "Withdrawn Type")]
        //[StringLength(30)]
        //public string WdType { get; set; }

        public bool IsApproved { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Last Present Date")]
        //public DateTime? DtPresentLast { get; set; }

        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtInput { get; set; }

        [StringLength(80)]
        public string PcName { get; set; }
        public bool IsApprove { get; set; }
        public string ApprovedBy { get; set; }

        // public string UserId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }
        //[StringLength(80)]
        // public string UpdateByUserId { get; set; }
        //public DateTime? DateUpdated { get; set; }

        //public virtual Cat_Company Com { get; set; }
        //public DateTime DtJoin { get; set; }

    }

    public class EmployeeReportModel
    {
        public string ComName { get; set; }
        public int EmpID { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string DesigName { get; set; }
        public string EmpType { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public string dtPunchDate { get; set; }
        public string Status { get; set; }
        public string dtJoin { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string RegHour { get; set; }
        public string OTHour { get; set; }
        public string? OTAmount { get; set; }

        //public double? ComAdd1 { get; set; }
        //public string? Caption { get; set; }
        //public double? MorOT { get; set; }  

    }

    public class EmployeeReportParameters
    {
        public string ComId { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
        public int EmpId { get; set; }
        public string ShiftId { get; set; }
        public string DesigId { get; set; }
        public string DeptId { get; set; }
        public string SectId { get; set; }
        public string SubSectId { get; set; }
        public string EmpTypeId { get; set; }
        public string LineId { get; set; }
        public string UnitId { get; set; }
        public string FloorId { get; set; }
        public string Type { get; set; }
        public string optCriteria { get; set; }
        public string EmpStatus { get; set; }
    }
    public partial class HR_PR_Cost_Entry : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PRId { get; set; }

        [Display(Name = "Cat_Unit")]
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Cat_Unit Cat_Unit { get; set; }

        [Display(Name = "Cat_Variable")]
        public int VariableId { get; set; }
        [ForeignKey("VariableId")]
        public virtual Cat_Variable Cat_Variable { get; set; }


        [Display(Name = "Safety Shoe(Per Month)")]

        public int SafetyShoe { get; set; }


        [Display(Name = "Uniform(Per Month)")]

        public int Uniform { get; set; }
        [Display(Name = "Uniform(Per Month)")]

        public int MedicalCost { get; set; }
        [Display(Name = "Uniform(Per Month)")]

        public double ServiceCommission { get; set; }




    }


    public class HR_ZktFinger
    {
        [Key]

        public Guid id { get; set; }
        public string? comId { get; set; }
        public string? empCode { get; set; }
        public string? empName { get; set; }
        public string? cardNo { get; set; }
        public string? empImage { get; set; }
        public string? fingerindex1 { get; set; }
        public string? fingerindex2 { get; set; }
        public string? fingerindex3 { get; set; }
        public string? fingerindex4 { get; set; }
        public string? fingerindex5 { get; set; }
        public string? fingerindex6 { get; set; }
        public string? fingerindex7 { get; set; }
        public string? fingerindex8 { get; set; }
        public string? fingerindex9 { get; set; }
        public string? fingerindex10 { get; set; }
        public string? DeviceName { get; set; }

        public long? totalIndex { get; set; }
        public bool isDelete { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        [NotMapped]
        public int? weekday { get; set; }

    }
    public class Blocklist
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? comId { get; set; }
        public string? empCode { get; set; }
        public string? empName { get; set; }
        public bool isBlock { get; set; }
        public DateTime? blockdate { get; set; }
        public DateTime? unblockdate { get; set; }
        public string? remark { get; set; }


    }
    public class HR_Leave_AdjustUbL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public DateTime ReplaceDate { get; set; }
        public DateTime DutyDate { get; set; }
        public string? Remark { get; set; }
        public string ComId { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string? UpdateByUserId { get; set; }

        public bool? IsDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }
    }
    public class HR_EmployeeTransferVM
    {
        public string EmpId { get; set; }
        public int EmpCode { get; set; }
        public string New_EmpCode { get; set; }
        public String EmpName { get; set; }
        public string Old_DeptName { get; set; }
        public string New_DeptName { get; set; }
        public string Old_DesigName { get; set; }
        public string New_DesigName { get; set; }
        public string Old_SectName { get; set; }
        public string New_SectName { get; set; }
        public string Old_Unit { get; set; }
        public string New_Unit { get; set; }
        public string Old_EmpType { get; set; }
        public string New_EmpType { get; set; }
        public int FingerId { get; set; }
        public DateTime PFDate { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;

        public string CompanyCode { get; set; }

        //navigation property
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info Emp_Info { get; set; }
        [ForeignKey("")]
        public int? NewGradeId { get; set; }
        [ForeignKey("NewGradeId")]
        public virtual Cat_Grade Cat_GradeNew { get; set; }
        public int? OldGradeId { get; set; }
        [ForeignKey("OldGradeId")]
        public virtual Cat_Grade Cat_GradeOld { get; set; }

        [Display(Name = "Old Unit")]
        public int? OldUnitId { get; set; }
        [ForeignKey("OldUnitId")]
        public virtual Cat_Unit Cat_UnitOld { get; set; }

        [Display(Name = "New Unit")]
        public int? NewUnitId { get; set; }
        [ForeignKey("NewUnitId")]
        public virtual Cat_Unit Cat_UnitNew { get; set; }

        [Display(Name = "Old Department")]
        public int? OldDeptId { get; set; }
        [ForeignKey("OldDeptId")]
        public virtual Cat_Department Cat_DepartmentOld { get; set; }

        [Display(Name = "New Department")]
        public int? NewDeptId { get; set; }
        [ForeignKey("NewDeptId")]
        public virtual Cat_Department Cat_DepartmentNew { get; set; }

        [Display(Name = "Old Section")]
        public int? OldSectId { get; set; }
        [ForeignKey("OldSectId")]
        public virtual Cat_Section Cat_SectionOld { get; set; }

        [Display(Name = "New Section")]
        public int? NewSectId { get; set; }
        [ForeignKey("NewSectId")]
        public virtual Cat_Section Cat_SectionNew { get; set; }

        [Display(Name = "Old Designation")]
        public int? OldDesigId { get; set; }
        [ForeignKey("OldDesigId")]
        public virtual Cat_Designation Cat_DesignationOld { get; set; }

        [Display(Name = "New Designation")]
        public int? NewDesigId { get; set; }
        [ForeignKey("NewDesigId")]
        public virtual Cat_Designation Cat_DesignationNew { get; set; }
        [Display(Name = "Old EmpType")]
        public int? OldEmpTypeId { get; set; }
        [ForeignKey("OldEmpTypeId")]
        public virtual Cat_Emp_Type Cat_Emp_TypeOld { get; set; }

        [Display(Name = "New EmpType")]
        public int? NewEmpTypeId { get; set; }
        [ForeignKey("NewEmpTypeId")]
        public virtual Cat_Emp_Type Cat_Emp_TypeNew { get; set; }


    }
}