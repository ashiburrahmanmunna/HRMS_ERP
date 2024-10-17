using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblWebUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string SecQuestion { get; set; }
        public string SecAnswer { get; set; }
        public byte IsInactive { get; set; }
        public byte UserTypeId { get; set; }
        public byte UserCatId { get; set; }
        public int RefId { get; set; }
        public int RelId { get; set; }
        public string DisplayName { get; set; }
        public byte? ComId { get; set; }
        public long? ApprovedId { get; set; }
        public string Email { get; set; }
        public DateTime? DtBirth { get; set; }
        public string Mobile { get; set; }
        public string BloodGroup { get; set; }
    }
}
