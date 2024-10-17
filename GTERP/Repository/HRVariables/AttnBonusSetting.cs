using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;

namespace GTERP.Repository.HRVariables
{
    public class AttnBonusSetting : BaseRepository<Cat_AttBonusSetting>, ICat_AttBonusSetting
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public AttnBonusSetting(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
    
    }
}
