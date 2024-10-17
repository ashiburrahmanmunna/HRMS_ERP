using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace GTERP.ViewModels
{
    public class ActiveUser
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime DtBirth { get; set; }
        public DateTime DtJoin { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string GradeName { get; set; }
        public string ShiftName { get; set; }
        public string DesigName { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }

    }
    public class DailyAttendanceActive
    {
        public string EmpCode { get; set; }
        public DateTime DtPunchDate { get; set; }
        public string EmpName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string DesigName { get; set; }
        public string GradeName { get; set; }
        public string ShiftName { get; set; }
        public string Status { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public TimeSpan TimeIn {get; set;}
        public TimeSpan TimeOut { get; set; }
        public TimeSpan Late { get; set; }
        public double OTHour { get; set; }

    }

    public class BusinessAllowViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpType { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string DesigName { get; set; }
        public decimal ttlBusinessDuty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string DtFrom { get; set; }
        public string DtTo { get; set; }
        public string Remarks { get; set; }
    }

    #region Dashboard View Models
    public class DailyAttendanceDepartmentWiseData
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpTypeName { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public string GradeName { get; set; }
        public string Status { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtPunchDate { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public TimeSpan Late { get; set; }
        public double OTHour { get; set; }
    }
    public class DailyAttendance
    {
        public float TotalEmployee { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float Late { get; set; }
        public float Leave { get; set; }
        public float WHDay { get; set; }
        public DateTime LastProcessDate { get; set; }
        public string PresentPercent { get; set; }
        public string AbsentPercent { get; set; }
        public string LeavePercent { get; set; }
        public string LatePercent { get; set; }
        public string WHDayPercent { get; set; }

    }

    public class DailyAttendanceSum
    {
        [Display(Name = "Company Id")]
        //[Index("IX_ComAccnameUniqe", 1, IsUnique = true)]
        [StringLength(128)]
        public string ComId { get; set; }
        public string SectName { get; set; }
        public float Male { get; set; }
        public float Female { get; set; }
        public float TotalEmployee { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float Late { get; set; }
        public float Leave { get; set; }
        public float PresentPercent { get; set; }
        public float AbsentPercent { get; set; }
        public float LeavePercent { get; set; }
        public float LatePercent { get; set; }
        public float OffDay { get; set; }
        public float OffDayPercent { get; set; }
    }

    public class SalarySummeryDetails
    {
        public string Section { get; set; }
        public string EmpTypeName { get; set; }
        public int TotalEmployee { get; set; }
        public float GS { get; set; }
        public float TotalAmount { get; set; }
        public float TotalOTAmount { get; set; }
        public float OTHour { get; set; }
        public float AttBonus { get; set; }
        public float ConveyanceAllowance { get; set; }
        public float FoodAllowance { get; set; }
        public float OtherAllow { get; set; }
        public float NetAmount { get; set; }
        public float Absent { get; set; }
        public float PF { get; set; }
        public float Adv { get; set; }
        public float OtherDeduct { get; set; }
        public float Stamp { get; set; }
        public float LunchDeduct { get; set; }
        public float TotalDeduct { get; set; }
        public float NetPayable { get; set; }
        public string ProssType { get; set; }
    }
    public class SalaryDetails
    {
        public string AvgSalary { get; set; }
        public string PerDaySalary { get; set; }
        public string NetPayableSalary { get; set; }
        public string TotalOtCost { get; set; }
        public string TotalAdvance { get; set; }
        public string LastProssType { get; set; }
        public string ProssType { get; set; }
        public DateTime LastProcessDate { get; set; }


    }
    public class EmployeeDetails
    {
        public string ActiveEmployee { get; set; }
        public string ReleasedEmployee { get; set; }
        public string OtYesEmployee { get; set; }
        public string MaleEmployee { get; set; }
        public string FemaleEmployee { get; set; }
        public string TotalEmployee { get; set; }
        public DateTime LastProcessDate { get; set; }
        public DateTime FirstDayOfMonth { get; set; }


    }
    public class MonthlyAttendance
    {
        public string MonthlyTotalOtHour { get; set; }
        public string MonthlyExtraOtHour { get; set; }
        public string MonthlyRegOThour { get; set; }
        public float MonthlyTotalOtHourAmt { get; set; }
        public float MonthlyExtraOtHourAmt { get; set; }
        public float MonthlyRegOThourAmt { get; set; }
        public string TotalOtHour { get; set; }
        public string ExtraOtHour { get; set; }
        public string RegOThour { get; set; }
        public float TotalOtHourAmt { get; set; }
        public float ExtraOtHourAmt { get; set; }
        public float RegOThourAmt { get; set; }
        public string MonthlyCl { get; set; }
        public string MonthlySl { get; set; }
        public string Prosstype { get; set; }
        public DateTime LastProcessDate { get; set; }
        public DateTime FirstDayOfMonth { get; set; }
    }
    public class Dashboard
    {
        public DailyAttendance DailyAttendance { get; set; }
        public MonthlyAttendance MonthlyAttendance { get; set; }
        public EmployeeDetails EmployeeDetails { get; set; }
        public SalaryDetails SalaryDetails { get; set; }
        public List<TotalEmpType> TotalEmpType { get; set; }
        public List<DailyAttendanceSum> DailyAttendanceSum { get; set; }
        public List<SalarySummeryDetails> SalarySummeryDetails { get; set; }
    }
    public class TotalEmpType
    {
        public string EmpTypeName { get; set; }
        public float? ttlEmployee { get; set; }
        public float? TtlEmpTypeWise { get; set; }
    }

    public class MonthlyOTRenderChartVM
    {
        //public int month { get; set; }
        public string DtPunchDate { get; set; }
        public float OtAmount { get; set; }

    }
    public class DailyOTRenderChartVM
    {
        public string date { get; set; }
        //public string dayName { get; set; }
        public float otAmount { get; set; }
    }

    public class DailyPresentRenderChartVM
    {
        public string date { get; set; }
        //public string dayName { get; set; }
        public float TtlPresent { get; set; }
    }
    public class DailyPresentRatioChartVM
    {
        public string date { get; set; }
        //public string dayName { get; set; }
        public int ttlpresent { get; set; }
        public int ttlAbsent { get; set; }
        public int ttlemp { get; set; }
    }

    public class MonthlyJoinReleaseVM
    {
        public string DtJoin { get; set; }
        public int JoinCount { get; set; }
        public int ReleaseCount { get; set; }
    }
    public class ManPowerHistoryVM
    {
        public string linename { get; set; }
        public int LineEmpCount { get; set; }

    }


    public class LineVM
    {
        public string linename { get; set; }
        public int EmpId { get; set; }

    }
    public class DeptWiseVM
    {
        public string DeptName { get; set; }
        public int EmpId { get; set; }

    }
    //Creating a model for line wise man Power
    //for line 
    public class DeptWiseManPowerVM
    {
        public string DeptName { get; set; }
        public int DeptEmpCount { get; set; }
    }
    
    public class MonthlyJoinVM
    {
        public string DtJoin { get; set; }
        public int EmpId { get; set; }

    }
    public class MonthlyReleaseVM
    {
        public string DtReleased { get; set; }
        public int EmpId { get; set; }

    }
    #endregion


    #region Employee List ViewModel
    public class EmpList
    {
        public int EmpId { get; set; }

        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string EmpNameB { get; set; }
        public string EmpFather { get; set; }
        public string EmpFatherB { get; set; }
        public string EmpMother { get; set; }
        public string EmpMotherB { get; set; }
        public string EmpSpouse { get; set; }
        public string EmpSpouseB { get; set; }

        public string HouseType { get; set; }
        public string ReligionName { get; set; }
        public string BloodName { get; set; }
        public string UnitName { get; set; }
        public string ShitfName { get; set; }
        public string DeptName { get; set; }
        public string ShiftName { get; set; }
        public string CurrVillName { get; set; }
        public string CurrDistName { get; set; }
        public string CurrPStationName { get; set; }
        public string CurrPOName { get; set; }
        public string EmpCurrCityVill { get; set; }
        public string EmpPerCityVill { get; set; }
        public string BankAccNo { get; set; }

        public string PerVillName { get; set; }
        public string PerDistName { get; set; }
        public string PerPStationName { get; set; }
        public string PerPOName { get; set; }
        //public List<HR_Emp_Experience> Experiences { get; set; }
        //public List<HR_Emp_Education> Educations { get; set; }

        public string Experiences { get; set; }
        public string Educations { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public string SubSectName { get; set; }
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
        public string EmpTypeName { get; set; }
        public string GenderName { get; set; }
        public string NID { get; set; }
        public string FingerId { get; set; }
        public string EmpPhone1 { get; set; }
        public string EmpPhone2 { get; set; }
        public bool IsInactive { get; set; }
        public string EmpPerZip { get; set; }
        public string EmpEmail { get; set; }
        public string EmpRemarks { get; set; }
        public string GradeName { get; set; }
        public string FloorName { get; set; }
        public string CategoryName { get; set; }
        public string LineName { get; set; }
        public bool IsAllowOT { get; set; }
        public bool? IsAllowPF { get; set; }
        public bool? IsLunchAllow { get; set; }
        public bool? IsTrnAllow { get; set; }
        public bool? IsLunchDed { get; set; }
        public bool? IsTrnDed { get; set; }
        public int ManageType { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtLocalJoin { get; set; }
        public string EmpNomineeName1 { get; set; }
        public string EmpNomineeMobile1 { get; set; }
        public string EmpNomineeNID1 { get; set; }
        public string EmpNomineeRelation1 { get; set; }
        public string EmpNomineeAddress1 { get; set; }
        public string EmpNomineeName2 { get; set; }
        public string EmpNomineeMobile2 { get; set; }
        public string EmpNomineeNID2 { get; set; }
        public string EmpNomineeRelation2 { get; set; }
        public string EmpNomineeAddress2 { get; set; }
        public string Skill { get; set; }
        public string BirthCertificate { get; set; }
        public bool PF { get; set; }
        public string BusStopName { get; set; }
        public int? Salary { get; set; }

    }

    public class EmpListCount
    {
        public int TotalRecord { get; set; }
    }

    public class EmpProfile
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public string SubSectName { get; set; }
        public DateTime? DtJoin { get; set; }
        public string EmpPhone1 { get; set; }
        public string EmpEmail { get; set; }
        public DateTime? DtConfirm { get; set; }
        public string EmpFather { get; set; }
        public string EmpMother { get; set; }
        public DateTime? DtBirth { get; set; }
        public string EmpPhone2 { get; set; }
        public byte[] EmpImage { get; set; }

    }

    public class AttendanceDetails
    {
        public int EmpId { get; set; }
        public int P { get; set; }
        public int L { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int A { get; set; }
        public int CL { get; set; }
        public int SL { get; set; }
        public int EL { get; set; }
    }

    public class LeaveDetails
    {
        public string EmpId { get; set; }
        public string CL { get; set; }
        public string ACL { get; set; }
        public string EL { get; set; }
        public string AEL { get; set; }
        public string ML { get; set; }
        public string AML { get; set; }
        public string SL { get; set; }
        public string ASL { get; set; }
    }

    public class PaymentDetails
    {
        public string PersonalPay { get; set; }
        public string BasicSalary { get; set; }
        public string HouseRent { get; set; }
        public string MadicalAllow { get; set; }
        public string CanteenAllow { get; set; }
        public string MiscAdd { get; set; }
        public string ConveyanceAllow { get; set; }
    }

    public class NomineeDetails
    {
        public string EmpNomineeName1 { get; set; }
        public string EmpNomineeMobile1 { get; set; }
        public string EmpNomineeName2 { get; set; }
        public string EmpNomineeMobile2 { get; set; }

    }

    public class ShowCauseDetails
    {
        public string dtNotice { get; set; }
        public string dtEvent { get; set; }
        public string Decision { get; set; }
    }
    public class LoanDetails
    {
        public string LoanType { get; set; }
        public string DtLoanStart { get; set; }
        public string LoanAmount { get; set; }
        public string MonthlyLoanAmount { get; set; }
        public string DueAmount { get; set; }
        public string DtLoanEnd { get; set; }
    }

    public class SalaryStructure
    {
        public string BS { get; set; }
        public string HR { get; set; }
        public string MA { get; set; }
        public string CA { get; set; }
        public string FA { get; set; }
    }

    public class TaxDetails
    {
        public string IncomeTax { get; set; }
        public int yearlypay { get; set; }
    }
    public class EmpProfileVM
    {
        public List<EmpProfile> EmpProfile { get; set; }
        public List<AttendanceDetails> AttendanceDetails { get; set; }
        public List<LeaveDetails> LeaveDetails { get;set; }
        public List<PaymentDetails> PaymentDetails { get; set; }
        public List<NomineeDetails> NomineeDetails { get; set; }
        public List<ShowCauseDetails> ShowCauseDetails { get; set; }
        public List<LoanDetails> LoanDetails { get; set; }
        public List<SalaryStructure> SalaryStructure { get; set; }
        public List<TaxDetails> TaxDetails { get; set; }
    }
    #endregion

    public class EmpForShit
    {
        public string ComId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string Shift { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public string EmpType { get; set; }
    }
    public class SalaryInfo
    {
        public int SalaryId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DesigName { get; set; }
        public float BasicSalary { get; set; }
        public float? HouseRent { get; set; }
        public float? HRExp { get; set; }
        public string EmpTypeName { get; set; }
        public string LocationName { get; set; }
        public string BuildingName { get; set; }
    }

    public class SummerWinterAllowViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpType { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string DesigName { get; set; }
        public int BS { get; set; }
        public DateTime DtInput { get; set; }
        public bool IsSummer { get; set; }
        public bool IsWinter { get; set; }
        public bool IsRainCoat { get; set; }
    }

    public class Pross
    {
        public string ProssType { get; set; }
        public DateTime dtInput { get; set; }
    }


    public class LeaveBalance
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
         public string EmpType { get; set; }
        public string FloorName { get; set; }
        public string Line { get;set; } 
        public string dtOpeningDate { get; set; }
        public float CL { get; set; }
        public float SL { get; set; }
        public float EL { get; set; }
        public float ML { get; set; }
        public int LvBalId { get; set; }
    }

    public class IncrementViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }

        public string EmpName { get; set; }
        public DateTime DtJoin { get; set; }
        public string DesigName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string EmpType { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public double Gross { get; set; }
        public double Basic { get; set; }
        public double HR { get; set; }
        public double MA { get; set; }
        public double TA { get; set; }
        public double FA { get; set; }
        public double Amount { get; set; }
        public double NewGS { get; set; }
        public double NewBS { get; set; }
        public double NewHR { get; set; }
        public double NewMA { get; set; }
        public double NewTA { get; set; }
        public double NewFA { get; set; }


    }

    public class AttFixGrid
    {
        public bool IsChecked { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string SectName { get; set; }
        public string DeptName { get; set; }
        public string DesigName { get; set; }
        public int ShiftId { get; set; }
       // public string ShiftName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtPunchDate { get; set; } = DateTime.Now.Date;

        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeIn { get; set; } = DateTime.Now.ToShortTimeString();
        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeOut { get; set; } = DateTime.Now.ToShortTimeString();

        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string OTHourInTime { get; set; } = DateTime.Now.ToShortTimeString();

        public string Status { get; set; }
        public int StatusId { get; set; }

        public string Remarks { get; set; }
        //[DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string OtHour { get; set; }
        public float OT { get; set; }
        public string Line { get; set; }
        public bool IsInactive { get; set; }
        public int SectId { get; set; }
        public string Criteria { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DtFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DtTo { get; set; }
    }

    //public class RecreationViewModel
    //{
    //    public int EmpId { get; set; }
    //    public string EmpCode { get; set; }

    //    public string EmpName { get; set; }
    //    public string EmpType { get; set; }
    //    public string DeptName { get; set; }
    //    public string SectName { get; set; }
    //    public string DesigName { get; set; }
    //    public int BS { get; set; }

    //    public float TtlOT { get; set; }
    //    public float TtlNight { get; set; }
    //    public float TtlShiftNight { get; set; }
    //    public float TtlFC { get; set; }
    //    public string Remarks { get; set; }

    //}

    public class RecreationViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpType { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string DesigName { get; set; }
        public int BS { get; set; }
        public string DtPayment { get; set; }
        public string DtReference { get; set; }
        public bool IsReCreation { get; set; }
        public string ReferenceNo { get; set; }
    }

    public class LoanHalt
    {
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }

        [Display(Name = "Other Loan Type")]
        public string OtherLoanType { get; set; }

        public string Criteria { get; set; }

        [Display(Name = "Increase Month")]
        public int IncreaseMonth { get; set; }
        [Display(Name = "Effective Month")]
        public DateTime EffectiveMonth { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public List<int> Employess { get; set; }
    }

    public class EmpViewModel
    {
        public int EmpId { get; set; }
        public string ComId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string DesigName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
    }


    #region IdCard
    public class IdcardGreadData
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string SectName { get; set; }
        public string DeptName { get; set; }
        public string DesigName { get; set; }
        public DateTime DtJoin { get; set; }
        public string ComId { get; set; }
    }


    #endregion


    #region HolidaySetup
    public class WHProssType
    {
        public int SalAddId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public string DtInput { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public double Amount { get; set; }
        public string OtherAddType { get; set; }
        public string LoanType { get; set; }
    }
    #endregion


    public class LoanReturn
    {
        public int LoanReturnId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public string DtInput { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public double Amount { get; set; }
        public string OtherAddType { get; set; }
        public string LoanType { get; set; }
    }
    public class EmployeeInfo
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DesigName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string EmpType { get; set; }
        public DateTime? DtJoin { get; set; }
        public string Email { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public string FingerId { get; set; }

    }
    public class CalculateData
    {
        public int PERIOD { get; set; }
        public DateTime PAYDATE { get; set; }
        public decimal PAYMENT { get; set; }
        public decimal CURRENT_BALANCE { get; set; }
        public decimal INTEREST { get; set; }
        public decimal PRINCIPAL { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal EndingBalance { get; set; }
    }


    public class OTFC
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string ProssType { get; set; }
        public string EmpName { get; set; }
        public string EmpType { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string DesigName { get; set; }
        public int BS { get; set; }
        public float TtlOT { get; set; }
        public float TtlNight { get; set; }
        public float TtlShiftNight { get; set; }
        public float TtlFC { get; set; }
        public string Remarks { get; set; }

    }
    public class IdCard
    {
        [Display(Name = "From Date: ")]
        public string FromDate { get; set; }

        [Display(Name = "To Date: ")]
        public string ToDate { get; set; }

        [Display(Name = "View As: ")]
        public string ViewReportAs { get; set; }
        public string ViewReportCat { get; set; }
        public string ViewReportType { get; set; }
        public List<int> Employess { get; set; }
        //public string EmployeeIdString { get; set; }

        //public string EmpCode { get; set; }
        //public string EmpName { get; set; }
    }

    public class LeaveEntryView
    {
        public int LvId { get; set; }

        public int EmpId { get; set; }
        public int LTypeId { get; set; }

        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string LTypeNameShort { get; set; }
        public string? LTypeName { get; set; }
        public string Remark { get; set; }


        public string DtFrom { get; set; }
        public string DtTo { get; set; }
        public string dtWork { get; set; }
        public string DtLvInput { get; set; }
        public string DtInput { get; set; }


        public float? TotalDay { get; set; }
        public float? LvApp { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public string? LeaveOption { get; set; }

    }
    public class SalaryInfoVM
    {
        public int SalaryId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DesigName { get; set; }
        public string EmpTypeName { get; set; }
        public float GrossSalary { get; set; }
        public float BasicSalary { get; set; }
        public float? HouseRent { get; set; }
        public float MA { get; set; }
        public float? Trn { get; set; }
        public float FA { get; set; }
        public float? OtherAllow { get; set; }
        public float CasualSalary { get; set; }
        public float GradeBonus { get; set; }
        public float MobileAllow { get; set; }
        public float IncomeTax { get; set; }
    }

    public class ProdGrid
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public int SectId { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public int varID { get; set; }
        public string DeptName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtPunchDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtTo { get; set; }

        public float Quantity { get; set; }
        public string Criteria { get; set; }

        public int? StyleId { get; set; }
        public string StyleName { get; set; }
        public string Remarks { get; set; }
    }
    public class SupplimentViewModel
    {
        public int SupplimentId { get; set; }

        public int EmpId { get; set; }
        public int SectId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string Designation { get; set; }
        public DateTime DtInput { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtTo { get; set; }
        public int Duration { get; set; }
        public float Basic { get; set; }
        public bool IsBS { get; set; } = true;
        public bool IsHR { get; set; } = true;
        public bool IsWash { get; set; } = true;
        public bool IsMA { get; set; } = true;
        public bool IsCPF { get; set; } = true;
        public bool IsRisk { get; set; } = true;
        public bool IsEdu { get; set; } = true;
        public bool IsHRExpDed { get; set; } = true;
        public int Persantage { get; set; }
        public bool IsOPF { get; set; } = true;
        public bool IsAddPF { get; set; } = true;
        public bool IsOA { get; set; } = true;
        public bool IsWFSub { get; set; } = true;
        public bool IsRS { get; set; } = true;
        public bool IsHBLoan { get; set; } = true;
        public bool IsMCLoan { get; set; } = true;

    }
    public class Basic
    {
        public int BS { get; set; }
    }

    public class CatHRSettingVM
    {
        public int Id { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Employee Type")]
        public string EmpTypeName { get; set; }
        [Display(Name = "Basic")]
        public float BS { get; set; }
        [Display(Name = "House Rent")]
        public float HR { get; set; }
        [Display(Name = "Medical Allowance")]
        public float MA { get; set; }
        [Display(Name = "Conveyance Allowance")]
        public float CA { get; set; }
        public float FA { get; set; }

        public bool IsBSPersentage { get; set; }
        public bool IsCAPersentage { get; set; }
        public bool IsFAPersentage { get; set; }
        public bool IsHRPersentage { get; set; }
        public bool IsMAPersentage { get; set; }
        public bool IsCADifference { get; set; }
        public bool IsFADifference { get; set; }
        public bool IsLateDedcution { get; set; }
        public bool DeductionType { get; set; }
    }

    public class CustomReportVM
    {
        public int CustomReportId { get; set; }
        public string CompanyName { get; set; }
        public string RDLCName { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string EmpType { get; set; }
    }

    public class OTFCSalarySettlement
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string ProssType { get; set; }
        public string EmpName { get; set; }
        public int GS { get; set; }
        public int AttBonus { get; set; }
        public int Arrear { get; set; }
        public int OtherAllow { get; set; }
        public int TrnAllow { get; set; }
        public int OTAmt { get; set; }
        public int OthersAddition { get; set; }
        public int CompensationDay { get; set; }
        public int CompensationAdd { get; set; }
        public int SubsistenceDay { get; set; }
        public int SubsistenceAdd { get; set; }

        public int DeathFacilityDay { get; set; }
        public int DeathFacilityAdd { get; set; }
        public int NoticePayDay { get; set; }
        public int NoticePayAdd { get; set; }
        public int ServiceBenifitDay { get; set; }
        public int ServiceBenifitAdd { get; set; }
        public int ELDay { get; set; }
        public int ELAmt { get; set; }
        public int TotalPayable { get; set; }
        public int AbsentAmt { get; set; }
        public int Adv { get; set; }
        public int PF { get; set; }
        public int BS { get; set; }
        public int IncomeTax { get; set; }
        public int OthersDeduct { get; set; }
        public int Stamp { get; set; }
        public int NoticePayDedDay { get; set; }
        public int NoticePayDed { get; set; }
        public int SuspentionDay { get; set; }
        public int SuspentionDed { get; set; }
        public int TotalDeduct { get; set; }
        public int NetSalaryPay { get; set; }
        public int PFOwn { get; set; }
        public int PFComp { get; set; }
        public int PFProfit { get; set; }
        public int PFTotal { get; set; }
        public int NetPayableAmt { get; set; }
        public string Remarks { get; set; }

        public bool IsPaid { get; set; } = true;

        public string EmployeeCode { get; set; }


    }

    // for HR Overtime Setting
    public class HR_OverTimeSettingVM
    {

        public int Id { get; set; }

        public bool IsAllowMinute { get; set; }

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

        public string CompanyId { get; set; }
        public string CompanyName { get; set; }

    }

    // for Income Tax Amount
    public class Payroll_IncomeTaxAmountVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public float GSLimitFrom { get; set; }
        public float GSLimitTo { get; set; }
        public float IncomeTax { get; set; }
        public bool IsInComeTax { get; set; }
    }

    public class GetApprovalListVM
    {
        public int? ApprovalSettingId { get; set; }
        public string? CompanyName { get; set; }
        public string? UserName { get; set; }
        public string? ApproveType { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsFirstLeaveApprove { get; set; }
    }

    public class SetApproveViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DesigName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public float GS { get; set; }
        public float BS { get; set; }
        public float HR { get; set; }
        public float MA { get; set; }
        public float CA { get; set; }
        public float FA { get; set; }
        public bool IsApprove { get; set; }
        public DateTime JoinDate { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string Line { get; set; }
        public string Grade { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime LastPresentDate { get; set; }
        public string RelType { get; set; }
        public string InputType { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtTo { get; set; }
        public float TotalDay { get; set; }
        public string Status { get; set; }
        public int ApprovalType { get; set; }
        public string Remark { get; set; }
        public string Shift { get; set; }
        public DateTime PunchDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeIn { get; set; } = DateTime.Now.ToShortTimeString();
        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeOut { get; set; } = DateTime.Now.ToShortTimeString();

        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string OTHour { get; set; } = DateTime.Now.ToShortTimeString();
        public float Amount { get; set; }
        public float NewSalary { get; set; }
        public float NewBS { get; set; }
        public float NewHR { get; set; }
        public float NewMA { get; set; }
        public float NewTA { get; set; }
        public float NewFA { get; set; }
        public string IncType { get; set; }
        public DateTime InputDate { get; set; }
        public string OldDesig { get; set; }
        public string OldSect { get; set; }
        public string OldEmpType { get; set; }
        public string EmpTypeName { get; set; }
    }

    public class ApprovalVM
    {
        public int EmpId { get; set; }
        public int ApproveType { get; set; }
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public DateTime InputDate { get; set; }
        public string LeaveType { get; set; }
    }

    // for get employee information 
    public class EmployeeInfoVM
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public int UnitId { get; set; }
        public string DeptName { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public string EmpTypeName { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public string FingerId { get; set; }
        public DateTime JoinDate { get; set; }
        public string Email { get; set; }
        public bool IsApprove { get; set; }
        public string EmpPhone1 { get; set; }

    }

    public class VendorInfoVM
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string UnitName { get; set; }
        public string DeptName { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public string VendorType { get; set; }
        public string JobNatureType { get; set; }
        public string AltitudeData { get; set; }
        public string EmpTypeName { get; set; }
        public string DisName { get; set; }
        public string Vaccinated { get; set; }


        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string EmpFather { get; set; }
        public string EmpMother { get; set; }
        public string EmpSpouse { get; set; }
        public bool MaritalStatus { get; set; }
        public DateTime DtBirth { get; set; }
        public int Age { get; set; }
        public string ServiceLentgh { get; set; }

        public string GenderName { get; set; }
        public string NID { get; set; }
        public string EmpPhone1 { get; set; }
        public string EmpPhone2 { get; set; }
        public string ReligionName { get; set; }
        public string BloodName { get; set; }
        public string FingerId { get; set; }
        public string TINNo { get; set; }
        public double rate { get; set; }
        public string Offday { get; set; }

        public decimal? GrossSalary { get; set; }
        public string? Skill { get; set; }
        public string? Relay { get; set; }
        public string? SecondWeekDay { get; set; }
        public string? Category { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? RoutingNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? AccountType { get; set; }
        public bool IsInactive { get; set; }

        public DateTime DtJoin { get; set; }
        public string Email { get; set; }
        public bool IsApprove { get; set; }

    }


    public class DailyCostSummary
    {
        public string DeptName { get; set; }
        public int ttlEmp { get; set; }
        public float Gs { get; set; }
        public float BasicSalary { get; set; }
        public float OTHour { get; set; }
        public float OTAmt { get; set; }
        public float NetAmt { get; set; }

    }

    public class DailyCostDetails
    {
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public string Remarks { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }

        public string EmpName { get; set; }
        public int CardNo { get; set; }
        public string Status { get; set; }
        public string DesigName { get; set; }
        public string ShiftName { get; set; }
        public string Grade { get; set; }


        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeIn { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeOut { get; set; }


        public float? Gs { get; set; }
        public float? BS { get; set; }
        public float PerDaySalary { get; set; }
        public float OTHr { get; set; }
        public float OTRate { get; set; }
        public float OTAmt { get; set; }
        public float NetAmt { get; set; }


    }

    public class NotificationPeram
    {
        public string Token { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

}
