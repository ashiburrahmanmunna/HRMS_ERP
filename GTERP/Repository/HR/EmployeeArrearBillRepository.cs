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
    public class EmployeeArrearBillRepository : BaseRepository<HR_Emp_ArrearBill>, IEmployeeArrearBill
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public EmployeeArrearBillRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public int? FindByEmpId(int? Id)
        {
            var id = _context.HR_Emp_Info.Find(Id).EmpTypeId;
            return id;
        }

        public IQueryable<HR_Emp_ArrearBill> GetAllEmpArrearBill()
        {
            return _context.HR_Emp_ArrearBill
                 .Where(x => !x.IsDelete)
                 .Include(h => h.HR_Emp_Info);

        }

        public IEnumerable<SelectListItem> GetArrearBillList()
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
    }
}
