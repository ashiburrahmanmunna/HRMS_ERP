using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class OTAndNightShiftRepository : BaseRepository<HR_OT_FC>, IOTAndNightShiftRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public OTAndNightShiftRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public void ExistingData(List<HR_OT_FC> hR_OT_FCs)
        {
            foreach (var item in hR_OT_FCs)
            {
                var exist = _context.HR_OT_FC.Where(o => o.EmpId == item.EmpId && o.ProssType == item.ProssType && o.ComId == item.ComId).FirstOrDefault();
                if (exist == null)
                    _context.Add(item);
                else
                {
                    exist.ttlOT = item.ttlOT;
                    exist.ttlShiftNight = item.ttlShiftNight;
                    _context.Entry(exist).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();

        }

        public IEnumerable<SelectListItem> ProssType()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@ComId", comid);
            var prossType = Helper.ExecProcMapTList<Pross>("GetProssType", parameter);
            return new SelectList(prossType, "ProssType", "ProssType");
        }

        public List<OTFC> SearchOTAndNight(string prossType)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            var OTFCList = Helper.ExecProcMapTList<OTFC>("Payroll_prcGetOTandOTNight", parameter);
            return OTFCList;
        }
    }
}
