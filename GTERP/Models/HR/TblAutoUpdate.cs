using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblAutoUpdate
    {
        public int AutoDownloadId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSourceName { get; set; }
        public string FileSourceNameWeb { get; set; }
        public string VershionNo { get; set; }
        public byte? IsDownLoad { get; set; }
        public int FileType { get; set; }
        public string FileFormate { get; set; }
        public int LuserId { get; set; }
        public string Remarks { get; set; }
    }
}
