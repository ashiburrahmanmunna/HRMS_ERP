using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class HolidaySetupRepository : BaseRepository<HR_ProssType_WHDay>, IHolidaySetupRepository
    {


        private readonly IHttpContextAccessor _httpContext;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        public HolidaySetupRepository(IHttpContextAccessor httpContext,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor) : base(context)
        {
            _httpContext = httpContext;
            _context = context;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public void UpdateHoliDaySetUp(HR_ProssType_WHDay WHProssType)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            WHProssType.ComId = comid;
            WHProssType.UserId = _httpContext.HttpContext.Session.GetString("userid");
            WHProssType.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");

            WHProssType.DateAdded = DateTime.Now;
            _context.Entry(WHProssType).State = EntityState.Modified;

        }

        public void CreateHoliDaySetUp(HR_ProssType_WHDay WHProssType)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            WHProssType.ComId = comid;
            WHProssType.DateUpdated = DateTime.Now;
            WHProssType.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            WHProssType.UserId = _httpContext.HttpContext.Session.GetString("userid");
            _context.Add(WHProssType);
        }

        List<HR_ProssType_WHDay> IHolidaySetupRepository.ProssTypeWHDayPartial(DateTime date)
        {

            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var WHProssTypes = _context.HR_ProssType_WHDay
                .Where(s => s.dtPunchDate.Value.Date == date && s.ComId == comid).ToList();

            return WHProssTypes;
        }
    }
}
