using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class InsureGradeRepository : BaseRepository<Cat_InsurGrade>, IInsureGradeRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public InsureGradeRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetInsureGradeList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.InGId.ToString(),
                Text = x.InSurGrade
            });
        }
    }
}
