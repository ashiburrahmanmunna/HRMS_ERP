using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBomsEntryMainTemp
    {
        public DateTime? InputDate { get; set; }
        public int Aid { get; set; }
        public string DocumentNo { get; set; }
        public string PrdName { get; set; }
        public string PartNo { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Page { get; set; }
        public string Tabulation { get; set; }
        public string Edition { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string FileName { get; set; }
        public int? Buyerid { get; set; }
        public int? ColorId { get; set; }
        public byte? Status { get; set; }
        public int? Comid { get; set; }
        public int? Luserid { get; set; }
        public string PcName { get; set; }
        public Guid? Wid { get; set; }
        public int? Rowno { get; set; }
        public string SheetName { get; set; }
        public int? BomId { get; set; }
        public string Version { get; set; }
        public int? Bomrefid { get; set; }
    }
}
