namespace GTERP.Models
{
    public class CompanyUser
    {
        public string ComId { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public bool isDefault { get; set; }
        public int isChecked { get; set; }
        public int CompanyPermissionId { get; set; }
    }
}