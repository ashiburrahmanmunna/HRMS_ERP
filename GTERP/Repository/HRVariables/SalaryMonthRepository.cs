using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HRVariables
{
    public class SalaryMonthRepository:BaseRepository<Cat_SalaryMonth>,ISalaryMonthRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public SalaryMonthRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            this._context = context;
            this._httpContext = httpContext;
        }
    }
}
