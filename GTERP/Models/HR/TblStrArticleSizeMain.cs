using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrArticleSizeMain
    {
        public TblStrArticleSizeMain()
        {
            TblStrArticleSizeSub = new HashSet<TblStrArticleSizeSub>();
        }

        public byte ComId { get; set; }
        public long Asid { get; set; }
        public long ArticleId { get; set; }
        public DateTime DtConfig { get; set; }
        public short LuserId { get; set; }
        public string Pcname { get; set; }
        public Guid WId { get; set; }
        public long AId { get; set; }

        public virtual ICollection<TblStrArticleSizeSub> TblStrArticleSizeSub { get; set; }
    }
}
