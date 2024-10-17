using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class SupplimentRepository : BaseRepository<HR_Emp_Suppliment>, ISupplimentRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public SupplimentRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public void CreateSuppliment(List<HR_Emp_Suppliment> suppliments)
        {
            foreach (var item in suppliments)
            {
                var exist = _context.HR_Emp_Suppliment
                    .Where(o => o.EmpId == item.EmpId && o.DtInput.Month == item.DtInput.Month)
                    .FirstOrDefault();
                if (exist != null)
                {
                    _context.HR_Emp_Suppliment.Remove(exist);

                }
                //db.SaveChanges();
                _context.Add(item);

            }

            _context.SaveChanges();
        }

        public List<SupplimentViewModel> HREmpSuppliment1(DateTime? dtInput, int? sectId)
        {
            var data = _context.HR_Emp_Salary
                .Include(e => e.HR_Emp_Info)
                .Include(e => e.HR_Emp_Info.Cat_Designation)
               .Select(s => new SupplimentViewModel
               {
                   EmpId = s.HR_Emp_Info.EmpId,
                   EmpName = s.HR_Emp_Info.EmpName,
                   EmpCode = s.HR_Emp_Info.EmpCode,
                   Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                   Basic = s.BasicSalary,
                   DtFrom = DateTime.Now.Date,
                   DtTo = DateTime.Now.Date

               }).ToList();
            return data;
        }

        public List<SupplimentViewModel> HREmpSuppliment2(DateTime? dtInput, int? sectId)
        {
            var data = _context.HR_Emp_Salary
                .Include(e => e.HR_Emp_Info)
                .Include(e => e.HR_Emp_Info.Cat_Designation)
              .Select(s => new SupplimentViewModel
              {
                  EmpId = s.HR_Emp_Info.EmpId,
                  EmpName = s.HR_Emp_Info.EmpName,
                  EmpCode = s.HR_Emp_Info.EmpCode,
                  SectId = s.HR_Emp_Info.SectId,
                  Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                  Basic = s.BasicSalary,
                  DtFrom = DateTime.Now.Date,
                  DtTo = DateTime.Now.Date

              }).Where(s => s.SectId == sectId).ToList();
            return data;
        }

        public List<SupplimentViewModel> HREmpSuppliment3(DateTime? dtInput, int? sectId)
        {
            var data = _context.HR_Emp_Suppliment
                .Include(e => e.HR_Emp_Info)
                .ThenInclude(e => e.Cat_Designation)
                   .Select(s => new SupplimentViewModel
                   {
                       EmpId = s.HR_Emp_Info.EmpId,
                       EmpName = s.HR_Emp_Info.EmpName,
                       EmpCode = s.HR_Emp_Info.EmpCode,
                       Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                       Basic = s.Basic,
                       Duration = s.Duration,
                       DtInput = s.DtInput,
                       DtFrom = s.DtFrom,
                       DtTo = s.DtTo,
                       IsBS = s.IsBS,
                       IsHR = s.IsHR,
                       IsWash = s.IsWash,
                       IsMA = s.IsMA,
                       IsCPF = s.IsCPF,
                       IsRisk = s.IsRisk,
                       IsEdu = s.IsEdu,
                       IsHRExpDed = s.IsHRExpDed,
                       Persantage = s.Persantage,
                       IsOPF = s.IsOPF,
                       IsAddPF = s.IsAddPF,
                       IsOA = s.IsOA,
                       IsWFSub = s.IsWFSub


                   }).Where(s => s.DtInput.Month == dtInput.Value.Month).ToList();
            return data;
        }

        public List<SupplimentViewModel> HREmpSuppliment4(DateTime? dtInput, int? sectId)
        {
            var data = _context.HR_Emp_Suppliment.Include(e => e.HR_Emp_Info).Include(e => e.HR_Emp_Info.Cat_Designation)
                 .Select(s => new SupplimentViewModel
                 {
                     EmpId = s.HR_Emp_Info.EmpId,
                     EmpName = s.HR_Emp_Info.EmpName,
                     EmpCode = s.HR_Emp_Info.EmpCode,
                     Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                     SectId = s.HR_Emp_Info.SectId,
                     Basic = s.Basic,
                     Duration = s.Duration,
                     DtInput = s.DtInput,
                     DtFrom = s.DtFrom,
                     DtTo = s.DtTo,
                     IsBS = s.IsBS,
                     IsHR = s.IsHR,
                     IsWash = s.IsWash,
                     IsMA = s.IsMA,
                     IsCPF = s.IsCPF,
                     IsRisk = s.IsRisk,
                     IsEdu = s.IsEdu,
                     IsHRExpDed = s.IsHRExpDed,
                     Persantage = s.Persantage,
                     IsOPF = s.IsOPF,
                     IsAddPF = s.IsAddPF,
                     IsOA = s.IsOA,
                     IsWFSub = s.IsWFSub

                 }).Where(s => s.DtInput.Month == dtInput.Value.Month && s.SectId == sectId).ToList();
            return data;
        }
    }
}
