using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebBdayWish
    {
        public long WishId { get; set; }
        public DateTime DtSubmit { get; set; }
        public DateTime DtBirth { get; set; }
        public long BirthOfId { get; set; }
        public long WishById { get; set; }
        public string Wish { get; set; }
        public short Year { get; set; }
        public byte IsReplied { get; set; }
        public Guid WId { get; set; }
    }
}
