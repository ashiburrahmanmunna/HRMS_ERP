using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblStrSubStrIssueSub
    {
        public long SubStrIssueId { get; set; }
        public long PrdId { get; set; }
        public long PrddisId { get; set; }
        public double QtyIssued { get; set; }
        public double? Amount { get; set; }
        public double? UnitPrice { get; set; }
        public string Remarks { get; set; }
        public byte RowNo { get; set; }
        public Guid WId { get; set; }
        public byte IsComplete { get; set; }
        public string RemarksSubStrIssue { get; set; }
        public string BasedOn { get; set; }
        public int BasedId { get; set; }
        public byte? IsDelete { get; set; }

        public virtual TblStrSubStrIssueMain SubStrIssue { get; set; }
    }
}
