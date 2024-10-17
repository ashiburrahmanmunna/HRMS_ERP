using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Repository.HRVariables
{
    public class SummerWinterAllowanceRepository : BaseRepository<Cat_SummerWinterAllowanceSetting>, ISummerWinterAllowanceRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public SummerWinterAllowanceRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetSummerWinterList()
        {
            throw new NotImplementedException();
        }
    }
}
