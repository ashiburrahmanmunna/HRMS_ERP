using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;

namespace GTERP.Repository.Accounts
{
    public class VoucherCheckSubNoRepository : BaseRepository<Acc_VoucherSubCheckno>, IVoucherCheckSubNoRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public VoucherCheckSubNoRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
    }
}
