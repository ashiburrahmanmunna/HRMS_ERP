using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class EmployeeReleasedRepository : BaseRepository<HR_Emp_Released>, IEmpReleaseRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public EmployeeReleasedRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public void ApproveSet(HR_Emp_Released released)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1177 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            if (approveData == null)
            {
                released.IsApprove = true;
            }
            else if (approveData.IsApprove == true)
            {
                released.IsApprove = false;
            }
            else
            {
                released.IsApprove = true;
            }

        }

        public IEnumerable<SelectListItem> CatVariableList()
        {
            return new SelectList(_context.Cat_Variable
               .Where(x => x.VarType == "ReleasedType")
               .OrderBy(x => x.SLNo).ToList(), "VarName", "VarName");

        }

        public IEnumerable<SelectListItem> EmpList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId
                           
                           where emp.ComId == comid && emp.IsDelete==false && emp.IsInactive==false
                           orderby emp.EmpId
                           select new
                           {
                               Value = emp.EmpId,
                               Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");

        }

        public IEnumerable<SelectListItem> EmpListEdit(int id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid && emp.IsDelete == false && emp.EmpId == id
                           orderby emp.EmpId
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");

        }

        public IEnumerable<SelectListItem> EmpListWithLessInfo()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info

                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid && emp.IsDelete == false && emp.IsInactive == false
                           orderby emp.EmpId
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + "]"
                           });
            return new SelectList(empInfo, "Value", "Text");

        }

        public IQueryable<HR_Emp_Released> GetReleasedAll()
        {
            return GetAll().Include(x => x.HR_Emp_Info)
                .Include(x => x.HR_Emp_Info.Cat_Designation);
        }

        public IEnumerable<SelectListItem> HR_Emp_Info()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.RelId.ToString(),
                Text = x.RelType
            });
        }


    }
}
