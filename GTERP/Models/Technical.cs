using GTERP.Models.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{

    public partial class Cat_Meeting : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeetingId { get; set; }

        [StringLength(200)]
        [DisplayName("Name")]
        public string Meeting { get; set; }

        [StringLength(200)]
        [DisplayName("Name Bangla")]
        public string MeetingBangla { get; set; }

        [StringLength(100)]
        [DisplayName("Technical Type")]
        public string MeetingType { get; set; }


        [DisplayName("SLNo")]
        public int? SLNo { get; set; }

        //[StringLength(128)]
        //public string ComId { get; set; }

        //[StringLength(128)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }

        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }
    }

    public partial class Technical:BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TechnicalId { get; set; }

        [StringLength(100)]
        [DisplayName("Meeting Type")]
        public string MeetingType { get; set; }

        [DisplayName("Meeting")]
        public int MeetingId { get; set; }
        [ForeignKey("MeetingId")]
        public virtual Cat_Meeting Cat_Meeting { get; set; }

        [StringLength(200)]
        [DisplayName("Meeting Name")]
        public string MeetingName { get; set; }

        [StringLength(50)]
        [DisplayName("Meeting No")]
        public string MeetingNo { get; set; }


        [DisplayName("Meeting Date")]
        public DateTime? MeetingDate { get; set; } = DateTime.Now;


        [DisplayName("Meeting Total Qty")]
        public float? MeetingTotalQty { get; set; }

        [StringLength(200)]
        [DisplayName("Training Name")]
        public string TrainingName { get; set; }

        [StringLength(200)]
        [DisplayName("Training Sub")]
        public string TrainingSub { get; set; }

        [DisplayName("Training Present Qty")]
        public float? TrainingPresentQty { get; set; }

        [DisplayName("Training Day")]
        public float? TrainingDay { get; set; }

        [DisplayName("Training Hour")]
        public float? TrainingHour { get; set; }

        [StringLength(200)]
        [DisplayName("Visit Name")]
        public string VisitName { get; set; }

        [StringLength(200)]
        [DisplayName("Visit Org")]
        public string VisitOrg { get; set; }

        [DisplayName("Visit Trainer Qty")]
        public float? VisitTrainerQty { get; set; }

        [DisplayName("Visit Workhour")]
        public float? VisitWorkhour { get; set; }

        [DisplayName("Visit Trainee Qty")]
        public float? VisitTraineeQty { get; set; }

        [DisplayName("Visit Other")]
        public float? VisitOther { get; set; }

        [DisplayName("Visit Total Qty")]
        public float? VisitTotalQty { get; set; }

        [StringLength(200)]
        [DisplayName("Import Phosphoric")]
        public string ImportPhosphoric { get; set; }

        [StringLength(100)]
        [DisplayName("Import Chalan No")]
        public string ImportChalanNo { get; set; }

        [DisplayName("Import Qty")]
        public float? ImportQty { get; set; }

        [DisplayName("Import Date")]
        public DateTime? ImportDate { get; set; } = DateTime.Now;


        [DisplayName("Import Density")]
        [StringLength(50)]
        public string ImportDensity { get; set; }

        [DisplayName("Import Phosphorus Pentoxide")]
        [StringLength(50)]
        public string ImportPhosphorusPentoxide { get; set; }



        [DisplayName("Waste Sample Collect Date")]
        public DateTime? WasteSampleCollectDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        [DisplayName("Waste Test Num")]
        public string WasteTestNum { get; set; }

        [StringLength(50)]
        [DisplayName("Waste Report Send Num")]
        public string WasteReportSendNum { get; set; }


        [DisplayName("WasteTestDate")]
        public DateTime? WasteTestDate { get; set; } = DateTime.Now;

        [DisplayName("Waste Report Send Date")]
        public DateTime? WasteReportSendDate { get; set; } = DateTime.Now;



        [DisplayName("License Application Date")]
        public DateTime? LicenseApplicationDate { get; set; } = DateTime.Now;

        [DisplayName("License Certificate Rcv. Date")]
        public DateTime? LicenseCertificateRcvDate { get; set; }

        [DisplayName("License Expire Date")]
        public DateTime? LicenseExpireDate { get; set; } = DateTime.Now;


        [StringLength(200)]
        [DisplayName("License Renewing Auth")]
        public string LicenseRenewingAuth { get; set; }

        [StringLength(200)]
        [DisplayName("Fire & Safety Name")]
        public string FSName { get; set; }

        [StringLength(50)]
        [DisplayName("Fire & Safety Number")]
        public string FSNum { get; set; }


        [DisplayName("Fire & Safety Date")]
        public DateTime? FSDate { get; set; } = DateTime.Now;


        [DisplayName("Fire & Safety Location")]
        public string FSLocaiton { get; set; }


        [StringLength(200)]
        [DisplayName("Extinguisher Type")]
        public string EGType { get; set; }

        [StringLength(50)]
        [DisplayName("Extinguisher Number")]
        public string EGNum { get; set; }


        [DisplayName("Extinguisher Refill Date")]
        public DateTime? EGRefillDate { get; set; } = DateTime.Now;

        [DisplayName("Extinguisher Expire Date")]
        public DateTime? EGExpireDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        [DisplayName("Extinguisher Hydro Test")]
        public string EGHydroTest { get; set; }


        [DisplayName("Date")]
        public DateTime dtInput { get; set; } = DateTime.Now;



        //[StringLength(128)]
        //public string ComId { get; set; }

        //[StringLength(128)]
        //public string UserId { get; set; }

        //public DateTime? DateAdded { get; set; }

        //[StringLength(80)]
        //public string UpdateByUserId { get; set; }

        //public DateTime? DateUpdated { get; set; }
    }

    //public partial class Cat_AuditYear
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int AuditYearId { get; set; }
    //    [StringLength(50)]
    //    public string AuditYear { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_AuditType
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int AuditTypeId { get; set; }
    //    [StringLength(50)]
    //    public string AuditType { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_AuditObjType
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int AuditObjTypeId { get; set; }
    //    [StringLength(50)]
    //    public string AuditObjType { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}


    //public partial class AuditInfo
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int AuditInfoId { get; set; }

    //    public int? AuditYearId { get; set; }
    //    [ForeignKey("AuditYearId")]
    //    public virtual Cat_AuditYear AuditYear { get; set; }

    //    public int? AuditTypeId { get; set; }
    //    [ForeignKey("AuditTypeId")]
    //    public virtual Cat_AuditType AuditType { get; set; }

    //    public int? AuditObjTypeId { get; set; }
    //    [ForeignKey("AuditObjTypeId")]
    //    public virtual Cat_AuditObjType AuditObjType { get; set; }
    //    //public int? PendObjTypeId { get; set; }
    //    //public virtual Cat_PendObjType PendObjType { get; set; }
    //    public int AuditNum { get; set; }
    //    public int GenAudit { get; set; }
    //    public int AdvAudit { get; set; }
    //    public int DraftAudit { get; set; }
    //    public int CollecttAudit { get; set; }
    //    public decimal Amount { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}


    //public partial class Cat_Meeting
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int MeetingId { get; set; }

    //    [StringLength(50)]
    //    [DisplayName("Meeting Name")]
    //    [Required]
    //    public string MeetingName { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    ////public partial class Cat_MeetingNum
    ////{
    ////    [Key]
    ////    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    ////    public int MeetingNumId { get; set; }
    ////    [StringLength(50)]
    ////    public string MeetingNum { get; set; }

    ////    [StringLength(128)]
    ////    public string ComId { get; set; }

    ////    [StringLength(128)]
    ////    public string UserId { get; set; }

    ////    public DateTime? DateAdded { get; set; }

    ////    [StringLength(80)]
    ////    public string UpdateByUserId { get; set; }

    ////    public DateTime? DateUpdated { get; set; }
    ////}

    //public partial class Cat_Training
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int TrainingId { get; set; }

    //    [StringLength(50)]
    //    [DisplayName("Training Name")]       
    //    [Required]
    //    public string TrainingName { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_TrainingSub
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int TrainingSubId { get; set; }

    //    [StringLength(50)]        
    //    [DisplayName("Training Sub. Name")]
    //    [Required]
    //    public string TrainingSubName { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_FireSafetyItem
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int FireSafetyItemId { get; set; }

    //    [StringLength(50)]        
    //    [DisplayName("Name")]
    //    [Required]
    //    public string ItemName { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_FireSafetyType
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int FireSafetyTypeId { get; set; }

    //    [StringLength(50)]
    //    [DisplayName("Fire Safety Type")]
    //    [Required]
    //    public string SafetyType { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_FireSafetyLocation
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int FireSafetyLocationId { get; set; }

    //    [StringLength(50)]
    //    [DisplayName("Location Name")]
    //    [Required]
    //    public string SafetyLocationName { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    ////public partial class Cat_ImportAcidItem
    ////{
    ////    [Key]
    ////    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    ////    public int ImportAcidItemId { get; set; }

    ////    [StringLength(50)]
    ////    [Required]
    ////    [DisplayName("")]
    ////    public string ImportAcidItem { get; set; }

    ////    [StringLength(128)]
    ////    public string ComId { get; set; }

    ////    [StringLength(128)]
    ////    public string UserId { get; set; }

    ////    public DateTime? DateAdded { get; set; }

    ////    [StringLength(80)]
    ////    public string UpdateByUserId { get; set; }

    ////    public DateTime? DateUpdated { get; set; }
    ////}

    //public partial class Cat_Inspection
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int InspectionId { get; set; }
    //    [StringLength(50)]
    //    [Required]   
    //    [DisplayName("Name")]
    //    public string InspectionName { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_InspectionInst
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int InspectionInstId { get; set; }
    //    [StringLength(50)]
    //    [Required]
    //    [DisplayName("Institute Name")]
    //    public string InspectionInstitute { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_License
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int LicenseId { get; set; }

    //    [StringLength(50)]
    //    [Required]
    //    [DisplayName("License No")]
    //    public string LicenseNo { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_LicenseAuth
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int LicenseAuthId { get; set; }
    //    [StringLength(50)]
    //    [Required]
    //    [DisplayName("License Auth.")]
    //    public string LicenseAuth { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Cat_Waste
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int WasteId { get; set; }

    //    [StringLength(50)]
    //    [DisplayName("Name")]
    //    [Required]
    //    public string Name { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Tech_Meeting
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Tech_MeetingId { get; set; }
    //    public int? MeetingId { get; set; }
    //    [ForeignKey("MeetingId")]
    //    public virtual Cat_Meeting Meeting { get; set; }

    //    [StringLength(20)]
    //    public string MeetingNum { get; set; }
    //    //public int? MeetingNumId { get; set; }
    //    //[ForeignKey("MeetingNumId")]
    //    //public virtual Cat_MeetingNum MeetingNum { get; set; }

    //    public int? DeptId { get; set; }
    //    [ForeignKey("DeptId")]
    //    public virtual Cat_Department Dept { get; set; }
    //    public float TtlMeeting { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtMeeting { get; set; } = DateTime.Now;

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Tech_Training
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Tech_TrainingId { get; set; }
    //    public int? TrainingId { get; set; }
    //    [ForeignKey("TrainingId")]
    //    public virtual Cat_Training Training { get; set; }
    //    public int? TrainingSubId { get; set; }
    //    [ForeignKey("TrainingSubId")]
    //    public virtual Cat_TrainingSub TrainingSub { get; set; }
    //    public int? DeptId { get; set; }
    //    [ForeignKey("DeptId")]
    //    public virtual Cat_Department Dept { get; set; }
    //    [StringLength(50)]
    //    public string ParticipantName { get; set; }
    //    public int DayNum { get; set; }
    //    [DataType(DataType.Time)]
    //    [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
    //    public DateTime WorkingHr { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtTraining { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Tech_Inspection
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Tech_InspectionId { get; set; }
    //    public int? InspectionId { get; set; }
    //    [ForeignKey("InspectionId")]
    //    public virtual Cat_Inspection Inspection { get; set; }
    //    public int? InspectionInstId { get; set; }
    //    [ForeignKey("InspectionInstId")]
    //    public virtual Cat_InspectionInst InspectionInst { get; set; }
    //    public int? DeptId { get; set; }
    //    [ForeignKey("DeptId")]
    //    public virtual Cat_Department Dept { get; set; }
    //    public int TeacherNum { get; set; }
    //    public int TraineeNum { get; set; }
    //    public int OtherNum { get; set; }
    //    public int TtlNum { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Tech_ImportAcid
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Tech_ImportAcidId { get; set; }

    //    public int? ImportAcidItemId { get; set; }
    //    [ForeignKey("ImportAcidItemId")]
    //    public virtual Product ImportAcidItem { get; set; }

    //    [StringLength(50)]
    //    public string ChallanNo { get; set; }
    //    public float Quantity { get; set; }
    //    public float Density { get; set; }
    //    public float Percent { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtDate { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Tech_WasteMgt
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Tech_WasteMgtId { get; set; }
    //    public int? WasteId { get; set; }
    //    public virtual Cat_Waste Waste { get; set; }

    //    public decimal TestNum { get; set; }
    //    public decimal SendRptNum { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtSampleCollect { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtTest { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtSendRpt { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }

    //}

    //public partial class Tech_License
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Tech_LicenseId { get; set; }
    //    public int? LicenseId { get; set; }
    //    public virtual Cat_License License { get; set; }
    //    public int? DeptId { get; set; }
    //    public virtual Cat_Department Dept { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtApply { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtLicenseRecptRenew { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtExpired { get; set; }
    //    public int? LicenseAuthId { get; set; }
    //    public virtual Cat_LicenseAuth LicenseAuth { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}

    //public partial class Tech_FireSafety
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Tech_FireSafetyId { get; set; }
    //    public int? FireSafetyItemId { get; set; }
    //    public virtual Cat_FireSafetyItem FireSafetyItem { get; set; }

    //    public int? FireSafetyTypeId { get; set; }
    //    public virtual Cat_FireSafetyType FireSafetyType { get; set; }
    //    public int? DeptId { get; set; }
    //    [ForeignKey("DeptId")]
    //    public virtual Cat_Department Dept { get; set; }

    //    public float Quantity { get; set; }
    //    [StringLength(50)]
    //    public string Location { get; set; }
    //    [StringLength(50)]
    //    public string HydroTest { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtDate { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtRefill { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime DtExpired { get; set; }

    //    [StringLength(128)]
    //    public string ComId { get; set; }

    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    public DateTime? DateAdded { get; set; }

    //    [StringLength(80)]
    //    public string UpdateByUserId { get; set; }

    //    public DateTime? DateUpdated { get; set; }
    //}
}
