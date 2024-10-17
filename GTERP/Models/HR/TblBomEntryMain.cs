using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblBomEntryMain
    {
        public TblBomEntryMain()
        {
            TblBomEntrySub = new HashSet<TblBomEntrySub>();
        }

        public int Aid { get; set; }
        public int BomId { get; set; }
        public string AssemblyNo { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyRevision { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public Guid? Wid { get; set; }
        public int? Comid { get; set; }
        public DateTime? InputDate { get; set; }
        public string PrdName { get; set; }
        public string PcName { get; set; }
        public int? Luserid { get; set; }
        public byte? Status { get; set; }
        public string FileName { get; set; }
        public int? Buyerid { get; set; }
        public int? ColorId { get; set; }

        public virtual ICollection<TblBomEntrySub> TblBomEntrySub { get; set; }
    }
}
