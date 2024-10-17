using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblLoginUser
    {
        public int LuserId { get; set; }
        public string LuserName { get; set; }
        public string LuserPass { get; set; }
        public short LsubGroupId { get; set; }
        public long EmpId { get; set; }
        public byte IsMaster { get; set; }
        public byte IsClientMaster { get; set; }
        public byte IsInactive { get; set; }
        public int AId { get; set; }
        public Guid Wid { get; set; }
        public string ImgName { get; set; }
        public byte? IsAccounts { get; set; }
        public string DisplayName { get; set; }
        public int? CreatedBy { get; set; }
        public string Pcname { get; set; }
        public byte? IsDelete { get; set; }
        public byte? IsApplicationPath { get; set; }
        public byte? IsSalesDept { get; set; }
        public byte? IsMaintainTwoUnit { get; set; }
        public byte? IsMcddept { get; set; }
        public byte IsLockBackDateVoucherEntry { get; set; }
        public byte? LoginStatus { get; set; }
        public byte? IsDashboard { get; set; }
        public byte? IsAllowDuplicateEntry { get; set; }
        public byte? IsFefomethod { get; set; }

        public virtual TblEmpInfo Emp { get; set; }
        public virtual TblLoginGroupSub LsubGroup { get; set; }
    }
}
