using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public partial class HrTemp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public string ComName { get; set; }
        public string ComAddress { get; set; }
        public string ComAddress2 { get; set; }
        public string ComImagePath { get; set; }
        public string ComPhone { get; set; }
        public string ComPhone2 { get; set; }
        public string ComWeb { get; set; }
        public string LuserId { get; set; }
        public string EmpName { get; set; }
        public string EmpDesig { get; set; }
        public string EmpDept { get; set; }
        public string EmpMobile { get; set; }
        public string EmpEmail { get; set; }
        public string EmpCode { get; set; }
        public int? MasterId { get; set; }
        public string VarCaption1 { get; set; }
        public string VarCaption2 { get; set; }
        public string VarMaster1 { get; set; }
        public string VarMaster2 { get; set; }
        public string VarMaster3 { get; set; }
        public string VarMaster4 { get; set; }
        public string VarMaster5 { get; set; }
        public string VarStr1 { get; set; }
        public string VarStr2 { get; set; }
        public string VarStr3 { get; set; }
        public DateTime? VarDate1 { get; set; }
        public DateTime? VarDate2 { get; set; }
        public DateTime? VarDate3 { get; set; }
        public int? VarInt1 { get; set; }
        public int? VarInt2 { get; set; }
        public int? VarInt3 { get; set; }
        public int? VarInt4 { get; set; }
        public int? VarInt5 { get; set; }

        public virtual Cat_Company Com { get; set; }
    }

    public partial class HrTempAttend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public int SectId { get; set; }
        public string SectName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string CardNo { get; set; }
        public int DesigId { get; set; }
        public string DesigName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime? DtPunchDate { get; set; }
        public string Status { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Late { get; set; }
        public DateTime? RegHour { get; set; }
        public DateTime? Othour { get; set; }
        public float? Othr { get; set; }
        public string Remarks { get; set; }
        public int Sl { get; set; }
        public int? SslNo { get; set; }
        public int? DslNo { get; set; }
        //public int? Slno { get; set; }
        public string Pstatus { get; set; }
        public DateTime? PtimeIn { get; set; }
        public DateTime? PtimeOut { get; set; }
        public int? AbTn { get; set; }
        public DateTime? DtFromDate { get; set; }
        public string EmpType { get; set; }
        public short DeptId { get; set; }
        public string DeptName { get; set; }
        public string Band { get; set; }
        public DateTime? DtJoin { get; set; }
        public string Operation { get; set; }
        public short? CscomId { get; set; }
        public int SubSectId { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual Cat_Department ComNavigation { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        public virtual Cat_Section Sect { get; set; }
        public virtual Cat_Shift Shift { get; set; }
        public virtual Cat_SubSection SubSect { get; set; }
    }

    public partial class HrTempAttendMonth
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DtJoin { get; set; }
        public int? DesigId { get; set; }
        public string DesigName { get; set; }
        public string Grade { get; set; }
        public int SectId { get; set; }
        public string SectName { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public string Band { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public byte DayMonth { get; set; }
        public float DayTtl { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }
        public float LateDay { get; set; }
        public float Leave { get; set; }
        public float Hday { get; set; }
        public float Wday { get; set; }
        public float Cl { get; set; }
        public float El { get; set; }
        public float Sl { get; set; }
        public float Ml { get; set; }
        public float AccL { get; set; }
        public float Lwp { get; set; }
        public float LateHrs { get; set; }
        public float EarlyLvHrs { get; set; }
        public float ShortLvHrs { get; set; }
        public string OthrsTtl { get; set; }
        public byte Lunch { get; set; }
        public byte Night { get; set; }
        public float Othr { get; set; }
        public string LateHrTtl { get; set; }
        public float OthrDed { get; set; }
        public float? Rot { get; set; }
        public float? Eot { get; set; }
        public int? Sslno { get; set; }
        public int? Dslno { get; set; }
        public int? ShiftId { get; set; }
        public decimal? Gs { get; set; }
        public decimal? Bs { get; set; }
        public float? Otrate { get; set; }
        public decimal? Ot { get; set; }
        public string SubSectName { get; set; }
        public string DaysCnt { get; set; }
        public int SubSectId { get; set; }
        public float? NewJoinAbs { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual Cat_Department Dept { get; set; }
        public virtual Cat_Designation Desig { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
        public virtual Cat_Section Sect { get; set; }
        public virtual Cat_SubSection SubSect { get; set; }
    }

    public partial class HrTempCount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long? EmpId { get; set; }
        public double? Cnt { get; set; }
        public double? Cnt1 { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? DateTime1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public string Vchr { get; set; }
        public string Vchr1 { get; set; }
        public string Vchr2 { get; set; }
    }

    public partial class HrTempCount1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long? EmpId { get; set; }
        public double? Cnt { get; set; }
        public double? Cnt1 { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? DateTime1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public string Vchr { get; set; }
        public string Vchr1 { get; set; }
        public DateTime? DateTime3 { get; set; }
        public string Vchr2 { get; set; }
        public int? Cnt2 { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public int? ShiftId { get; set; }
        public string ShiftType { get; set; }
    }

    public partial class HrTempCount2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long? EmpId { get; set; }
        public double? Cnt { get; set; }
        public double? Cnt1 { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? DateTime1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public string Vchr { get; set; }
        public string Vchr1 { get; set; }
        public int? ComId { get; set; }
        public int? Cnt3 { get; set; }
        public int? Cnt4 { get; set; }
        public float? Cnt5 { get; set; }
        public string DocType { get; set; }
        public double? CntFloat { get; set; }
        public double? BinId { get; set; }
        public double? WhId { get; set; }
    }

    public partial class HrTempCount3
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long? EmpId { get; set; }
        public double? Cnt { get; set; }
        public double? Cnt1 { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? DateTime1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public string Vchr { get; set; }
        public string Vchr1 { get; set; }
        public int? ComId { get; set; }
        public int? Cnt3 { get; set; }
        public int? Cnt4 { get; set; }
        public float? Cnt5 { get; set; }
        //public long AId { get; set; }
    }

    public partial class HrTempCountGtr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string IpAddress { get; set; }
        public double? Cnt { get; set; }
        public int? Cnt1 { get; set; }
        public DateTime? DtDate { get; set; }
        public DateTime? DtTime { get; set; }
        public string ComId { get; set; }
    }

    public partial class HrTempDailySum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public DateTime DtDate { get; set; }
        public int SectId { get; set; }
        public string SectName { get; set; }
        public int? DesigId { get; set; }
        public string DesigName { get; set; }
        public float Male { get; set; }
        public float Female { get; set; }
        public float TtlEmp { get; set; }
        public float Present { get; set; }
        public float PresentPer { get; set; }
        public float Late { get; set; }
        public float LatePer { get; set; }
        public float TtlPresent { get; set; }
        public float TtlPresentPer { get; set; }
        public float Absent { get; set; }
        public float AbsentPer { get; set; }
        public float Leave { get; set; }
        public float LeavePer { get; set; }
        public float Total { get; set; }
        public int? Sslno { get; set; }
        public int? Dslno { get; set; }
        public float? OffDay { get; set; }
        public float? NewJoin { get; set; }
    }

    public partial class HrTempElBalance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public string ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime? DtStart { get; set; }
        public DateTime? DtEnd { get; set; }
        public DateTime? DtOpbal { get; set; }
        public int TtlPresent { get; set; }
        public int PrevEl { get; set; }
        public int Ael { get; set; }
        public int El { get; set; }
        public int CashedEl { get; set; }
        public int CurrBel { get; set; }
        public byte BalType { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }

    public partial class HrTempEmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public long EmpId { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }

    public partial class HrTempJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string BloodGroup { get; set; }
        public decimal Gs { get; set; }
        public short DesigId { get; set; }
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
        public float Present { get; set; }
        public float Absent { get; set; }
        public float LateDay { get; set; }
        public float Leave { get; set; }
        public float Hday { get; set; }
        public float Wday { get; set; }
        public float LateHrTtl { get; set; }
        public float Othr { get; set; }
        public float OthrDed { get; set; }
        public float OthrsTtl { get; set; }
        public float? Rot { get; set; }
        public float? Eot { get; set; }
        //public int Slno { get; set; }
        public float Night { get; set; }
        public float Lunch { get; set; }
    }

    public partial class HrTempProssCount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime DtPunchDate { get; set; }
        public string EmpCode { get; set; }
        public short? ShiftId { get; set; }
        public short? SectId { get; set; }
        public short? DesigId { get; set; }
        public string Band { get; set; }
        public short? DeptId { get; set; }
        public string Grade { get; set; }
        public decimal? Gs { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Late { get; set; }
        public DateTime? LunchIn { get; set; }
        public DateTime? LunchOut { get; set; }
        public DateTime? LunchLate { get; set; }
        public string Status { get; set; }
        public DateTime? Othour { get; set; }
        public DateTime? RegHour { get; set; }
        public DateTime? TotalHour { get; set; }
        public float OthourMin { get; set; }
        public float? Ot { get; set; }
        public float RegMin { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public float? RegHr { get; set; }
        public float? LunchTime { get; set; }
        public DateTime? TiffinTime1 { get; set; }
        public float? TiffinD1 { get; set; }
        public DateTime? TiffinTime2 { get; set; }
        public float? TiffinD2 { get; set; }
        public DateTime? TiffinTime3 { get; set; }
        public float? TiffinD3 { get; set; }
        public float? TotalMin { get; set; }
        public float? WeeklyMin { get; set; }
        public float? RegOt { get; set; }
        public float? WeeklyOt { get; set; }
        public float? Cnt { get; set; }
        public float? Cnt1 { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public string Vchr { get; set; }
        public string Vchr1 { get; set; }
        public string Vchr2 { get; set; }
        public string Remarks { get; set; }
        public byte IsNightShift { get; set; }
    }

    public partial class HrTempProssCount2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime DtPunchDate { get; set; }
        public string EmpCode { get; set; }
        public short? ShiftId { get; set; }
        public short? SectId { get; set; }
        public short? DesigId { get; set; }
        public string Band { get; set; }
        public short? DeptId { get; set; }
        public string Grade { get; set; }
        public decimal? Gs { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Late { get; set; }
        public DateTime? LunchIn { get; set; }
        public DateTime? LunchOut { get; set; }
        public DateTime? LunchLate { get; set; }
        public string Status { get; set; }
        public DateTime? Othour { get; set; }
        public DateTime? RegHour { get; set; }
        public DateTime? TotalHour { get; set; }
        public float OthourMin { get; set; }
        public float? Ot { get; set; }
        public float RegMin { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public float? RegHr { get; set; }
        public float? LunchTime { get; set; }
        public DateTime? TiffinTime1 { get; set; }
        public float? TiffinD1 { get; set; }
        public DateTime? TiffinTime2 { get; set; }
        public float? TiffinD2 { get; set; }
        public DateTime? TiffinTime3 { get; set; }
        public float? TiffinD3 { get; set; }
        public float? TotalMin { get; set; }
        public float? WeeklyMin { get; set; }
        public float? RegOt { get; set; }
        public float? WeeklyOt { get; set; }
        public float? Cnt { get; set; }
        public float? Cnt1 { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public string Vchr { get; set; }
        public string Vchr1 { get; set; }
        public string Vchr2 { get; set; }
        public string Remarks { get; set; }
    }

    public partial class HrTempProssCount3
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime DtPunchDate { get; set; }
        public string EmpCode { get; set; }
        public short? ShiftId { get; set; }
        public short? SectId { get; set; }
        public short? DesigId { get; set; }
        public string Band { get; set; }
        public short? DeptId { get; set; }
        public string Grade { get; set; }
        public decimal? Gs { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Late { get; set; }
        public DateTime? LunchIn { get; set; }
        public DateTime? LunchOut { get; set; }
        public DateTime? LunchLate { get; set; }
        public string Status { get; set; }
        public DateTime? Othour { get; set; }
        public DateTime? RegHour { get; set; }
        public DateTime? TotalHour { get; set; }
        public float OthourMin { get; set; }
        public float? Ot { get; set; }
        public float RegMin { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public float? RegHr { get; set; }
        public float? LunchTime { get; set; }
        public DateTime? TiffinTime1 { get; set; }
        public float? TiffinD1 { get; set; }
        public DateTime? TiffinTime2 { get; set; }
        public float? TiffinD2 { get; set; }
        public DateTime? TiffinTime3 { get; set; }
        public float? TiffinD3 { get; set; }
        public float? TotalMin { get; set; }
        public float? WeeklyMin { get; set; }
        public float? RegOt { get; set; }
        public float? WeeklyOt { get; set; }
        public float? Cnt { get; set; }
        public float? Cnt1 { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public string Vchr { get; set; }
        public string Vchr1 { get; set; }
        public string Vchr2 { get; set; }
        public string Remarks { get; set; }
    }

    public partial class HrTempProssData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public byte ComId { get; set; }
        public long EmpId { get; set; }
        public DateTime DtPunchDate { get; set; }
        public string EmpCode { get; set; }
        public short? ShiftId { get; set; }
        public short? SectId { get; set; }
        public short? DesigId { get; set; }
        public string Band { get; set; }
        public short? DeptId { get; set; }
        public string Grade { get; set; }
        public decimal? Gs { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? Late { get; set; }
        public DateTime? LunchIn { get; set; }
        public DateTime? LunchOut { get; set; }
        public DateTime? LunchLate { get; set; }
        public string Status { get; set; }
        public DateTime Othour { get; set; }
        public float OthourMin { get; set; }
        public float? Ot { get; set; }
        public float RegMin { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? RegHour { get; set; }
        public float? RegHr { get; set; }
        public float? LunchTime { get; set; }
        public DateTime? TiffinTime1 { get; set; }
        public float? TiffinD1 { get; set; }
        public DateTime? TiffinTime2 { get; set; }
        public float? TiffinD2 { get; set; }
        public DateTime? TiffinTime3 { get; set; }
        public float? TiffinD3 { get; set; }
        public float? TotalMin { get; set; }
        public string Remarks { get; set; }
        public byte IsNightShift { get; set; }
        public float WeeklyMin { get; set; }
    }

    public partial class HrTempRawData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string Empid { get; set; }
        public DateTime? Punchdate { get; set; }
        public TimeSpan? Punchtime { get; set; }
        public int? RowNo { get; set; }
    }

    public partial class HrTempSalCal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string EmpId { get; set; }
        public DateTime? Date { get; set; }
        public double? Number { get; set; }
        public double? Number1 { get; set; }
    }

    public partial class HrWeekday
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ComId { get; set; }
        public long WeekId { get; set; }
        public long EmpId { get; set; }
        public DateTime DtPunchDate { get; set; }
        public string Remarks { get; set; }
        public string Pcname { get; set; }
        public string LuserId { get; set; }

        public virtual Cat_Company Com { get; set; }
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }
    }


}
