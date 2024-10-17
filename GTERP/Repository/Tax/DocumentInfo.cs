using GTERP.Interfaces.Tax;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Repository.Tax
{
    public class DocumentInfo: BaseRepository<Tax_DocumentInfo>, IDocumentInfo
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public DocumentInfo(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
    }
}
