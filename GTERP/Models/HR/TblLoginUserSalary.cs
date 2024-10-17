using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginUserSalary
    {
        public long LuserId { get; set; }
        public long EmpId { get; set; }
        public byte? IsActiveSalary { get; set; }
        public byte? IsActiveSalaryOver { get; set; }
        public byte? IsActiveSalaryLess { get; set; }
        public int AId { get; set; }
        public string EmpType { get; set; }
        public byte ComId { get; set; }
        public long? Amount { get; set; }
        public int? TblLoginUserSalary1 { get; set; }
        public int? IsSalaryLevel { get; set; }
    }
}
