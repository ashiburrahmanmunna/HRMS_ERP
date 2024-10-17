using GTERP.Interfaces.Tax;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;

namespace GTERP.Repository.Tax
{
    public class ClientContractPayment :BaseRepository<Tax_ClientContactPayment>, IClientContractPayment
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ClientContractPayment(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

    }
}
