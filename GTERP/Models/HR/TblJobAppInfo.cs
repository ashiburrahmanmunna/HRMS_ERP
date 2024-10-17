using System;
using System.Collections.Generic;

namespace GTERP_DAP_Model.CustomModels
{
    public partial class TblJobAppInfo
    {
        public byte ComId { get; set; }
        public long AAppId { get; set; }
        public long AppId { get; set; }
        public string AppCode { get; set; }
        public string AppName { get; set; }
        public string AppNameB { get; set; }
        public string AppFather { get; set; }
        public string AppFatherB { get; set; }
        public string AppMother { get; set; }
        public string AppMotherB { get; set; }
        public string AppSpouse { get; set; }
        public string AppSpouseB { get; set; }
        public string AppCurrAdd { get; set; }
        public string AppCurrCity { get; set; }
        public string AppCurrPo { get; set; }
        public string AppCurrPs { get; set; }
        public short AppCurrDistId { get; set; }
        public string AppPerAdd { get; set; }
        public string AppPerVill { get; set; }
        public string AppPerPo { get; set; }
        public string AppPerPs { get; set; }
        public string AppPerCity { get; set; }
        public short? AppPerDistId { get; set; }
        public string AppPerZip { get; set; }
        public string AppPhone { get; set; }
        public string AppMobile { get; set; }
        public string AppEmail { get; set; }
        public string AppPicLocation { get; set; }
        public string AppRemarks { get; set; }
        public string Sex { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalSts { get; set; }
        public DateTime? DtBirth { get; set; }
        public DateTime? DtApp { get; set; }
        public string Nationality { get; set; }
        public string PassportNo { get; set; }
        public string VoterNo { get; set; }
        public string AppType { get; set; }
        public string CardNo { get; set; }
        public long? RefId { get; set; }
        public int? AId { get; set; }
        public Guid WId { get; set; }
        public string Pcname { get; set; }
        public short? LuserId { get; set; }
        public string Mobileself { get; set; }
        public string MobileHome { get; set; }
        public byte? IsPassed { get; set; }
        public byte? IsWaiting { get; set; }
        public byte? IsFaild { get; set; }
        public byte? IsAppointed { get; set; }
        public DateTime? DtJoin { get; set; }
        public string Remarks { get; set; }
    }
}
