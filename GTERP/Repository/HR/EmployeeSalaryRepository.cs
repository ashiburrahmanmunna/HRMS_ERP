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
    public class EmployeeSalaryRepository : BaseRepository<HR_Emp_Salary>, IEmployeeSalaryRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public EmployeeSalaryRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public Task<HR_Emp_Salary> EditEmpSalary(int? id)
        {
            var hR_Emp_Salary = _context.HR_Emp_Salary
                .Include(h => h.Cat_BuildingTypeHC)
                .Include(h => h.Cat_Location1)
                .Include(h => h.Cat_Location2)
                .Include(h => h.Cat_Location3)
                .Include(h => h.Cat_PFLoanlocation)
                .Include(h => h.Cat_GratuityLocation)
                .Include(h => h.Cat_LocationHB)
                .Include(h => h.Cat_LocationMC)
                .Include(h => h.Cat_LocationPF)
                .Include(h => h.Cat_LocationWelfare)
                .Include(h => h.HR_Emp_Info)
                .FirstOrDefaultAsync(m => m.SalaryId == id);
            return hR_Emp_Salary;
        }

        public Cat_ElectricChargeSetting ElectricChargeSetting(int EmpId, int LId1, int BId, double BS)
        {
            return _context.Cat_ElectricChargeSetting
             .Where(h => h.LId == LId1 && h.BId == BId).FirstOrDefault();
        }

        public Cat_GasChargeSetting GasChargeSetting(int EmpId, int LId1, int BId, Double BS)
        {
            return _context.Cat_GasChargeSetting
             .Where(h => h.LId == LId1 && h.BId == BId).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> GetEmpList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid && emp.IsDelete==false
                           orderby emp.EmpId
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            return new SelectList(empInfo, "Value", "Text");

        }

        public IQueryable<SalaryInfoVM> GetEmpSalary()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var query = from e in _context.HR_Emp_Salary
                        //.Include(h => h.Cat_BuildingTypeHC).Include(h => h.Cat_Location1)
                        //.Include(h => h.Cat_LocationHB).Include(h => h.HR_Emp_Info.Cat_Designation)
                        //.Include(h => h.HR_Emp_Info).Include(h => h.HR_Emp_Info.Cat_Emp_Type)
                        .Where(x => x.ComId == comid && x.HR_Emp_Info.IsInactive == false && x.HR_Emp_Info.IsCasual == false).OrderByDescending(x => x.SalaryId)
                        select new SalaryInfoVM
                        {
                            SalaryId = e.SalaryId,
                            EmpCode = e.HR_Emp_Info.EmpCode,
                            EmpName = e.HR_Emp_Info.EmpName,
                            DesigName = e.HR_Emp_Info.Cat_Designation != null ? e.HR_Emp_Info.Cat_Designation.DesigName : "",
                            EmpTypeName = e.HR_Emp_Info.Cat_Emp_Type != null ? e.HR_Emp_Info.Cat_Emp_Type.EmpTypeName : "",
                            GrossSalary = (float)e.PersonalPay,
                            BasicSalary = e.BasicSalary,
                            HouseRent = e.HouseRent,
                            MA = (float)e.MadicalAllow,
                            Trn = e.ConveyanceAllow,
                            FA = (float)e.CanteenAllow,
                            OtherAllow = e.MiscAdd,
                            CasualSalary = (float)e.DearnessAllow,
                            GradeBonus = (float)e.ArrearBasic,
                            MobileAllow = (float)e.MobileAllow,
                            IncomeTax = (float)e.IncomeTax,

                        };
            return query;

        }

        public IEnumerable<SelectListItem> GLId()
        {
            return new SelectList(_context.Cat_Location, "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> HBLId()
        {
            return new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> HBLId2()
        {
            return new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> HBLId3()
        {
            return new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
        }

        public Cat_HRSetting HR(int EmpId, int LId1, int BId, double BS)
        {
            int? empTypeId = _context.HR_Emp_Info.Find(EmpId).EmpTypeId;
            return _context.Cat_HRSetting
                     .Where(h => h.EmpTypeId == empTypeId && h.LId == LId1 && h.BS <= BS && h.BId == BId)
                     .OrderByDescending(h => h.BS).FirstOrDefault();
        }

        public Cat_HRExpSetting HRExp(int EmpId, int LId1, int BId, double BS)
        {
            int? empTypeId = _context.HR_Emp_Info.Find(EmpId).EmpTypeId;

            return _context.Cat_HRExpSetting
                   .Where(h => h.EmpTypeId == empTypeId && h.LId == LId1 && h.BId == BId && h.BS <= BS)
                   .OrderByDescending(h => h.BS).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> LId1()
        {
            return new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> LId2()
        {
            return new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> LId3()
        {
            return new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> MCLId()
        {
            return new SelectList(_context.Cat_Location, "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> PFLId()
        {
            return new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> PFLLId()
        {
            return new SelectList(_context.Cat_Location.Take(2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> PFLLId2()
        {
            return new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> PFLLId3()
        {
            return new SelectList(_context.Cat_Location.Where(l => l.LId != 2), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> WelfareLId()
        {
            return new SelectList(_context.Cat_Location, "LId", "LocationName");
        }

    }
}
