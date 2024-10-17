using GTERP.Interfaces.Tax;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Tax
{
    public class TaxRepository : BaseRepository<Tax_ClientInfo>, ITaxRepository
    {
        
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public TaxRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context) 
        {
            _context = context;
            _httpContext = httpContext;
        }
        public IEnumerable<SelectListItem> ComList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Tax_ClientCompany
              .Where(x => x.IsDelete == false && x.ComId == comid)
            .Select(s => new { Text = s.ClientComName, Value = s.ClientComId })
            .ToList(), "Value", "Text");
        }


        public IEnumerable<SelectListItem> ClientList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Tax_ClientInfo
              .Where(x=>x.IsDelete==false && x.ComId== comid)
            .Select(s => new { Text = "[" + s.ClientCode + "] - " + s.ClientName, Value = s.ClientId })
            .ToList(), "Value", "Text");

        }


    }
}
