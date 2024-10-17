using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class MeetingRepository : BaseRepository<Cat_Meeting>, IMeetingRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;

        public MeetingRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetMeetingList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.MeetingId.ToString(),
                Text = x.MeetingType
            });
        }
    }
}
