using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Accounts
{
    public class VoucherTranGroupRepository:BaseRepository<VoucherTranGroup>, IVoucherTranGroupRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<VoucherTranGroupRepository> _logger;

        public VoucherTranGroupRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<VoucherTranGroupRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }
    }
}
