using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class EmpTypeRepository : SelfRepository<Cat_Emp_Type>, IEmpTypeRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public EmpTypeRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetEmpTypeList()
        {
            return _context.Cat_Emp_Type.Where(x=>!x.IsDelete).Select(x => new SelectListItem
            {
                Value = x.EmpTypeId.ToString(),
                Text = x.EmpTypeName
            });
        }
    }
}
