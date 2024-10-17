using GTERP.Interfaces.Payroll;
using GTERP.Models;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Payroll
{
    public class PFContributionRepository : BaseRepository<HR_PFContribution>, IPFContributionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public PFContributionRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetEmpList()
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetPFContributionList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> PFContributionList()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<SelectListItem> CatVariableList()
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");

            return new SelectList(_context.Cat_Variable
               .Where(v => v.ComId == comid && v.VarType == "PFContributionType")
               .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");

        }

        public List<HR_Emp_Info> EmpData()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");

            var empData = _context.HR_Emp_Info
                .Where(s => s.ComId == comid)
                .Include(x => x.HR_Emp_PersonalInfo)
                .ToList();
            return empData;
        }

        public List<PFContribution> PFContributionPartial(DateTime date)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");
            var PFContributions = _context.HR_PFContribution
                .Include(s => s.HR_Emp_Info)
                .Include(s => s.HR_Emp_Info.Cat_Section)
                .Where(s => s.DtInput.Date.Month == date.Month
                && s.DtInput.Date.Year == date.Year && s.ComId == comid && !s.IsDelete)
                .Select(s => new PFContribution
                {
                    PFContributionId = s.PFContributionId,
                    EmpId = s.EmpId,
                    EmpCode = s.HR_Emp_Info.EmpCode,
                    EmpName = s.HR_Emp_Info.EmpName,
                    Section = s.HR_Emp_Info.Cat_Section.SectName,
                    Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                    DtJoin = s.DtJoin.ToString("dd-MMM-yyyy"),
                    DtInput = s.DtInput.ToString("dd-MMM-yyyy"),
                    Amount = s.Amount,
                    PF = s.PF
                }).ToList();
            return PFContributions;
        }

        public HR_PFContribution PFContributionCheck(HR_PFContribution PFContribution)
        {
            return _context.HR_PFContribution
                   .Where(s => s.EmpId == PFContribution.EmpId && s.DtInput.Date.Month == PFContribution.DtInput.Date.Month
                   && s.PF == PFContribution.PF && s.PFContributionId != PFContribution.PFContributionId).FirstOrDefault();
        }
    }
}
