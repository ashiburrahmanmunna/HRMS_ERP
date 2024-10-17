using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class HRExpSettingRepository : BaseRepository<Cat_HRExpSetting>, IHRExpSettingRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public HRExpSettingRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetHRExpList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.HRExp.ToString()
            });
        }
    }
}
