using System;

namespace GTERP.ViewModels
{
    public class RawDataMapVm
    {
            public int Aid { get; set; }
            public string ComId { get; set; }
            public int? EmpId { get; set; }
            public string? EmpCode { get; set; }
            public string? EmpName { get; set; }
            public DateTime? DtJoin { get; set; }
            public string? DesigName { get; set; }
            public string? SectName { get; set; }
            public string? DeptName { get; set; }
            public DateTime? DtPunchDate { get; set; }
            public DateTime? DtPunchTime { get; set; }
    }
}
