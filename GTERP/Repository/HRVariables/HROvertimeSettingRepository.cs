using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.Repository.Self;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HRVariables
{
    public class HROvertimeSettingRepository:SelfRepository<HR_OverTimeSetting>,IHROvertimeSettingRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public HROvertimeSettingRepository(GTRDBContext context, IHttpContextAccessor httpContext):base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IEnumerable<SelectListItem> GetCompany()
        {
            return _context.Companys.Select(x => new SelectListItem
            {
                Value=x.CompanyCode,
                Text=x.CompanyName
            });
        }

        public List<HR_OverTimeSettingVM> GetOvertimeList()
        {
            var query = $"Exec HR_PrcGetOvertimeList";
            return Helper.ExecProcMapTList<HR_OverTimeSettingVM>("HR_PrcGetOVertimeList");
        }
    }
}
