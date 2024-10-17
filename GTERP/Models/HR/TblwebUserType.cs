using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblwebUserType
    {
        public byte UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public byte IsAdmin { get; set; }
    }
}
