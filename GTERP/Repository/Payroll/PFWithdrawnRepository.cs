using AutoMapper;
using GTERP.EF;
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
    public class PFWithdrawnRepository: BaseRepository<HR_PF_Withdrawn>, IPFWithdrawnRepository
    {
        private readonly GTRDBContext _context;
        private readonly IMapper _mapping;
        private readonly IHttpContextAccessor _httpContext;
        private readonly gtrerp_allContext _ef;
        public PFWithdrawnRepository(GTRDBContext context, gtrerp_allContext ef, IHttpContextAccessor httpContext, IMapper mapping) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
            _mapping = mapping;
            _ef = ef;
        }

        public void AddpF_WithdrawnAddition(HR_PF_Withdrawn withdrawn)
        {
            withdrawn.DateAdded = DateTime.Now;
            withdrawn.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            Add(withdrawn);
        }

        public HR_PF_Withdrawn check(HR_PF_Withdrawn withdrawn)
        {
            //return GetAll()
            //        .Where(s => s.EmpId == withdrawn.EmpId && s.DateAdded == withdrawn.DateAdded
            //        && s.WdId != withdrawn.WdId && s.IsDelete == false).FirstOrDefault();
            return GetAll()
                   .Where(s => s.EmpId == withdrawn.EmpId && s.DtWithdrawn.Date.Month == withdrawn.DtWithdrawn.Date.Month
               && s.WdId != withdrawn.WdId && s.IsDelete == false).FirstOrDefault();
        }

        public void DeletepF_WithdrawnAddition(int addId)
        {
            var withdrawn = _context.HR_PF_Withdrawn.Find(addId);
            Delete(withdrawn);
            _context.SaveChanges();
        }

        public void ModifiedpF_WithdrawnAddition(HR_PF_Withdrawn pF_Withdrawn)
        {
            pF_Withdrawn.DateAdded = DateTime.Now;
            Update(pF_Withdrawn);
        }
        public IEnumerable<SelectListItem> GetEmpInfo()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == comid && emp.IsDelete == false
                           select new
                           {
                               Value = emp.EmpId,
                               Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");
        }

        public IEnumerable<HRProcessedDataSalVM> GetPrcDataSal(string prossType, string tableName)
        {
            throw new System.NotImplementedException();
        }


        public List<PFWithdrawn> LoadPFWithdawnPartial(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var pFWithdrawns = GetAll()
                .Include(s => s.HR_Emp_Info)
                .Include(s => s.HR_Emp_Info.Cat_Section)
                .Where(s => s.ComId == comid &&
                            !s.IsDelete &&
                            s.DtWithdrawn.Month== date.Month && s.DtWithdrawn.Year==date.Year && s.ComId == comid && s.IsDelete == false)
                .Select(s => new PFWithdrawn
                {
                    WdId = s.WdId,
                    EmpId = s.EmpId,
                    EmpCode = s.HR_Emp_Info.EmpCode,
                    EmpName = s.HR_Emp_Info.EmpName,
                    Section = s.HR_Emp_Info.Cat_Section.SectName,
                    Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                    DtWithdrawn = s.DtWithdrawn.ToString("dd-MMM-yyyy"),
                    // DtInput = s.DtInput.ToString("dd-MMM-yyyy"),
                    DateAdded=s.DateAdded.ToString("dd-MMM-yyyy"),
                    Remarks = s.Remarks,
                })
                .ToList();

            return pFWithdrawns;
        }
    }
}
