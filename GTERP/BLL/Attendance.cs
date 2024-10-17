using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.BLL
{
    public class AttendanceRepository
    {
        private readonly GTRDBContext _context;
        private IHttpContextAccessor _httpContextAccessor;


        public AttendanceRepository(GTRDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //public Task<List<HR_AttFixed>> GetAttFixedAsync()
        //{
        //    var FixedAttendenc = _context.HR_AttFixed.Include(h => h.Cat_AttStatus).Include(h => h.Cat_Shift).Include(h => h.HR_Emp_Info); ;
        //    return FixedAttendenc.ToListAsync();
        //}


        public List<HR_AttFixed> PrcGetEmployeeInfo(string comid)
        {
            var AllEmployee = _context.HR_AttFixed.Where(e => e.ComId == comid).ToList();
            return AllEmployee;
        }

        public List<MenuPermission_Details> PrcGetUserMenuPermission(string comid)
        {
            var MenuPermission_Details = _context.MenuPermissionDetails.Include(x => x.MenuPermissionMasters).Where(e => e.MenuPermissionMasters.comid == comid).ToList();
            return MenuPermission_Details;
        }
    }
}
