using GTERP.Interfaces.Tax;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Repository.Tax
{
    public class ClientTaxInfo: BaseRepository<Tax_ClientTaxInfo>, IClientTaxInfo
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ClientTaxInfo(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

       
    }
}
