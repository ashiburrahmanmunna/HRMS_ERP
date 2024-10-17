using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class LeaveEncashmentRepository : BaseRepository<HR_LvEncashment>, ILeaveEncashmentRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public LeaveEncashmentRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<HR_LvEncashment> GetLvEncashments()
        {
            return _context.HR_LvEncashment.Include(e => e.HR_Emp_Info).ToList();
        }
        public IEnumerable<SelectListItem> GetEmpList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == comid
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            return new SelectList(empInfo, "Value", "Text");
        }

        public HR_Leave_Balance CreateLvEncashment(HR_LvEncashment lvEncashment)
        {
            return _context.HR_Leave_Balance
               .Where(l => l.EmpId == lvEncashment.EmpId && l.DtOpeningBalance == lvEncashment.LvEncashmentIYear)
               .FirstOrDefault();
        }

        public List<Basic> prcBasic(HR_LvEncashment lvEncashment)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[3];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@EmpId", lvEncashment.EmpId);
            parameter[2] = new SqlParameter("@dtInput", lvEncashment.DtInput);
            var basic = Helper.ExecProcMapTList<Basic>("HR_prcGetBS", parameter);
            return basic;
        }

        public HR_Leave_Balance GetLeaveBalance(int empId, int year)
        {
            return _context.HR_Leave_Balance
                .Where(l => l.EmpId == empId && l.DtOpeningBalance == year).FirstOrDefault();
        }
    }
}
