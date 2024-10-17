using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class TotalNightRepository : ITotalNightRepository
    {

        private readonly IHttpContextAccessor _httpContext;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        public TotalNightRepository(IHttpContextAccessor httpContext,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContext = httpContext;
            _context = context;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public void CreateTotalNight(List<HR_OT_FC> hR_OT_FCs)
        {

            foreach (var item in hR_OT_FCs)
            {
                var exist = _context.HR_OT_FC.Where(o => o.EmpId == item.EmpId && o.ProssType == item.ProssType && o.ComId == item.ComId).FirstOrDefault();
                if (exist == null)
                    _context.Add(item);
                else
                {

                    exist.ttlNight = item.ttlNight;

                    _context.Entry(exist).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();

        }

        public List<OTFC> SearchTotalNight(string prossType)
        {

            string comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            return Helper.ExecProcMapTList<OTFC>("[Payroll_prcGetSalaryNight]", parameter);

        }

        public List<Pross> TotalNightList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@ComId", comid);
            return Helper.ExecProcMapTList<Pross>("GetProssType", parameter);

        }
    }
}
