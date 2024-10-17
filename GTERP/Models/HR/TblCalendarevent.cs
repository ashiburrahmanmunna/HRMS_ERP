using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblCalendarevent
    {
        public int Aid { get; set; }
        public string Attendees { get; set; }
        public string Created { get; set; }
        public string CreatedRaw { get; set; }
        public string CreatorEmail { get; set; }
        public string Etag { get; set; }
        public Guid? Wid { get; set; }
        public int? Comid { get; set; }
        public string DtStart { get; set; }
        public string DtEnd { get; set; }
        public string Htmllink { get; set; }
        public string Id { get; set; }
        public string Uid { get; set; }
        public string Recurrance { get; set; }
        public string Reminders { get; set; }
        public string Status { get; set; }
        public string Summury { get; set; }
        public string Updated { get; set; }
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CaledarId { get; set; }
        public string PcName { get; set; }
        public string Reftype { get; set; }
        public int? Luserid { get; set; }
        public int RefId { get; set; }
    }
}
