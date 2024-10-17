using GTERP.Interfaces;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository
{
    public class DepartmentRepository : BaseRepository<Cat_Department>, IDepartmentRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public DepartmentRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public List<Cat_Department> GetDepartmentByCompany(string comid)
        {
            var deptList = _context.Cat_Department.Where(a => a.ComId == comid).ToList();
            return deptList;
        }
        public IEnumerable<SelectListItem> GetDepartmentList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Text = x.DeptName,
                Value = x.DeptId.ToString(),
            });
        }
    }
}
