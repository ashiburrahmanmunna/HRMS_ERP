using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class PostOfficeRepository : BaseRepository<Cat_PostOffice>, IPostOfficeRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public PostOfficeRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<Cat_PostOffice> GetPOList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_PostOffice
                .Include(x => x.Cat_District)
                .Include(x => x.Cat_PoliceStation)
                .Where(x => !x.IsDelete);
        }

        public IEnumerable<SelectListItem> GetPostOfficeList()
        {
            return GetPOList().Select(x => new SelectListItem
            {
                Value = x.POId.ToString(),
                Text = x.POName
            });
        }
    }
}
