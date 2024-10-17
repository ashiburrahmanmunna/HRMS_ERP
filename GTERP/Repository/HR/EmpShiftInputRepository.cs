using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HR
{
    public class EmpShiftInputRepository : BaseRepository<HR_Emp_ShiftInput>, IEmpShiftInputRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public EmpShiftInputRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<int> EmpShiftPost(List<HR_Emp_ShiftInput> empShifts)
        {
            DateTime fromDate = empShifts.Select(e => e.FromDate).FirstOrDefault();
            DateTime toDate = empShifts.Select(e => e.ToDate).FirstOrDefault();
            int days = (toDate - fromDate).Days + 1;
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            foreach (var item in empShifts)
            {

                for (int i = 0; i < days; i++)
                {
                    item.DtDate = fromDate.AddDays(i);
                    HR_Emp_ShiftInput old = _context.HR_Emp_ShiftInput.Where(e => e.DtDate.Date == item.DtDate.Date && e.EmpId == item.EmpId && e.ComId == item.ComId).FirstOrDefault();
                    if (old != null)
                    {
                        _context.Remove(old);
                    }
                    _context.Add(item);
                }
                if (item.IsMain)
                {
                    var emp = _context.HR_Emp_Info.Where(e => e.EmpId == item.EmpId && e.ComId == comid).FirstOrDefault();
                    emp.ShiftId = item.ShiftId;
                    _context.Entry(emp).State = EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();
            return 1;
        }

        public List<EmpForShit> GetEmpShiftAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<EmpForShit> employees = _context.HR_Emp_Info
                .Include(e => e.Cat_Shift)
                .Include(e => e.Cat_Department)
                .Include(e => e.Cat_Designation)
                .Include(e => e.Cat_Section)
                .Include(e => e.Cat_Line)
                .Include(e => e.Cat_Floor)
                .Include(e => e.Cat_Emp_Type)
                .Where(x=>x.ComId==comid && !x.IsDelete && !x.IsInactive)
                .Select(e => new EmpForShit()
                {
                    ComId = e.ComId,
                    EmpId = e.EmpId,
                    EmpName = e.EmpName,
                    EmpCode = e.EmpCode,
                    Shift = e.Cat_Shift.ShiftName,
                    Department = e.Cat_Department.DeptName,
                    Section = e.Cat_Section.SectName,
                    Designation = e.Cat_Designation.DesigName,
                    Floor = e.Cat_Floor.FloorName,
                    Line = e.Cat_Line.LineName,
                    EmpType = e.Cat_Emp_Type.EmpTypeName,

                }).Where(e => e.ComId == comid).ToList();
            return employees;
        }

        public IEnumerable<SelectListItem> GetEmpShiftInputList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.ShiftInputId.ToString(),
                Text = x.Cat_Shift.ShiftName
            });
        }

        public List<Cat_Shift> GetShiftAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_Shift.Where(x => x.ComId == comid && x.IsDelete==false).ToList();
        }
    }
}
