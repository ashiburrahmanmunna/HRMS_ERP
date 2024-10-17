using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblEmpBehave
    {
        public byte ComId { get; set; }
        public long BehaveId { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public DateTime DtNotice { get; set; }
        public DateTime DtEvent { get; set; }
        public string NoticeType { get; set; }
        public string ViolenceDesc { get; set; }
        public float ActionResult { get; set; }
        public string Decision { get; set; }
        public long AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
    }
}
