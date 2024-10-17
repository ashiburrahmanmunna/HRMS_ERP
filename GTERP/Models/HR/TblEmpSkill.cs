using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpSkill
    {
        public byte ComId { get; set; }
        public long SkillId { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public DateTime DtSkill { get; set; }
        public string SkillType { get; set; }
        public string SkillInfo { get; set; }
        public float SkillPoint { get; set; }
        public string SkillResult { get; set; }
        public string Decision { get; set; }
        public long AId { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
