using GTERP.Interfaces.HRVariables;
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
using System.Threading.Tasks;

namespace GTERP.Repository.HRVariables
{
    public class IncomeTaxRepository:BaseRepository<Cat_IncomeTaxChk>,IIncomeTaxRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public IncomeTaxRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<Pross> GetProssType()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter1 = new SqlParameter[1];
            parameter1[0] = new SqlParameter("@ComId", comid);
            var prossType = Helper.ExecProcMapTList<Pross>("GetProssType", parameter1);
            return prossType;
        }
    }
}
