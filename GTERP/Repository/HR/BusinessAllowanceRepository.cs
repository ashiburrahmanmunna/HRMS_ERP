using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class BusinessAllowanceRepository : BaseRepository<HR_Emp_BusinessAllow>, IBusinessAllowanceRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public BusinessAllowanceRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<BusinessAllowViewModel> BusinessAllowanceList(DateTime? todate)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@ComId", comid);
            //parameter[1] = new SqlParameter("@todate", todate);
            var BusinessAllow = Helper.ExecProcMapTList<BusinessAllowViewModel>("HR_prcGetBusinessAllow", parameter);
            return BusinessAllow;
        }

        public IEnumerable<SelectListItem> GetBusinessAllowanceList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.BAId.ToString(),
                Text = x.ttlBusinessDuty.ToString()
            });
        }
    }
}
