using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class CatAttStatusRepository : BaseRepository<Cat_AttStatus>, ICatAttStatusRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public CatAttStatusRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        //public IEnumerable<SelectListItem> GetAttShiftNameList()
        //{
        //    return _context.Cat_Shift.Select(x => new SelectListItem
        //    {
        //        Value = x.ShiftId.ToString(),
        //        Text = x.ShiftName
        //    });
        //}
        public IEnumerable<SelectListItem> GetAttStatusList()
        {
            return _context.Cat_AttStatus.Select(x => new SelectListItem
            {
                Value = x.StatusId.ToString(),
                Text = x.AttStatus
            });
        }
    }
}
