using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblTempOt
    {
        public byte ComId { get; set; }
        public string ComName { get; set; }
        public string ComAdd1 { get; set; }
        public string ComAdd2 { get; set; }
        public string Caption { get; set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public decimal Gs { get; set; }
        public short DesigId { get; set; }
        public string DesigName { get; set; }
        public string Grade { get; set; }
        public short SectId { get; set; }
        public string SectName { get; set; }
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
        public int OthrMin { get; set; }
        public string Othr { get; set; }
    }
}
