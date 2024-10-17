using GTERP.Models.Self;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class ReportType : SelfModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeTitle { get; set; }
        public string Description { get; set; }
        public virtual List<ApprovalPanel> ApprovalPanels { get; set; }

    }
    public class ApprovalRole
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RoleTitle { get; set; }
        public string RoleDescription { get; set; }
        public virtual List<ApprovalPanel> ApprovalPanels { get; set; }
    }
    public class ApprovalPanel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ApprovalRoleId { get; set; }
        public int ReportTypeId { get; set; }
        public string CreatedUserId { get; set; }
        public string ApprovedUserId { get; set; }
        public string ComId { get; set; }
        public virtual ApprovalRole ApprovalRole { get; set; }
        public virtual ReportType ReportType { get; set; }
    }
}
