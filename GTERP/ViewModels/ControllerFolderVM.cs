using GTERP.Models;
using System;

namespace GTERP.ViewModels
{
    public class ControllerFolderVM
    {
        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        public class CompanyList
        {
            public int isChecked { get; set; }
            public Guid ComId { get; set; }
            public string UserId { get; set; }
            public int CompanyPermissionId { get; set; }
            public string CompanyName { get; set; }
            public int isDefault { get; set; }
        }

        public class CompanyList1
        {
            public int isChecked { get; set; }

            public Guid ComId { get; set; }
            public string UserId { get; set; }

            public int ReportPermissionsId { get; set; }

            public string ReportName { get; set; }
            public int isDefault { get; set; }

        }

        public class UserLogResult : UserLogingInfo
        {
            public string logindatestring { get; set; }
            public string logintimestring { get; set; }
        }

        public class RoleAndUsers
        {
            public int Id { get; set; }
            public string ComId { get; set; }
            public string RoleId { get; set; }
            public string UserId { get; set; }
        }
        public class users
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public string Email { get; set; }
        }
        public class AssignApproval
        {
            public int Id { get; set; }
            public int RptTypeId { get; set; }
            public int RoleId { get; set; }
            public string CreatedUserId { get; set; }
            public string ApprovalUserId { get; set; }
        }

    }
}
