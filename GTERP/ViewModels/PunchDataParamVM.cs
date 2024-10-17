using System;

namespace GTERP.ViewModels
{
    public class PunchDataParamVM
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int? EmpId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string? DeptName { get; set; }
        public string? EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? DesigName { get; set; }
        public string? SectName { get; set; }
        public string? DtPunchDate { get; set; }
        public string? DtPunchTime { get; set;}
    }
}
