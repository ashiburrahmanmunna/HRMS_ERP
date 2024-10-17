using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class EmployeeBehaviourRepository : BaseRepository<HR_Emp_Behave>, IEmployeeBehaviourRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public EmployeeBehaviourRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public IEnumerable<HR_Emp_Behave> GetBehaveAll()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return _context.HR_Emp_Behave.Include(h => h.Employee).Where(e => e.ComId == comid && e.IsDelete == false).ToList();
        }

        public IEnumerable<SelectListItem> GetEmpList()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info
            .Select(s => new { Text = s.EmpName + " - [ " + s.EmpCode + " ]", Value = s.EmpId })
            .ToList(), "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetEmployeeBehaviourList()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Behave
            .Select(s => new { Text = s.Employee + " - [ " + s.Employee + " ]", Value = s.Employee })
            .ToList(), "Value", "Text");
            //throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> CatVariableList()
        {
            return new SelectList(_context.Cat_Variable
              .Where(v => v.VarType == "NoticeType")
              .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");

        }

        public void DeleteBehave(int id)
        {
            var HR_Emp_Behave = _context.HR_Emp_Behave.Find(id);
            var empinfo = _context.HR_Emp_Info.Where(e => e.EmpId == HR_Emp_Behave.EmpId).FirstOrDefault();
            if (empinfo != null)
            {
                empinfo.IsInactive = false;
                _context.Entry(empinfo).State = EntityState.Modified;

            }
            _context.SaveChanges();
        }
    }
}
