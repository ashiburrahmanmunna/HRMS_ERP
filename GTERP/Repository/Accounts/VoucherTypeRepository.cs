using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class VoucherTypeRepository : SelfRepository<Acc_VoucherType>, IVoucherTypeRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public VoucherTypeRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<Acc_VoucherType> GetVoucherType()
        {
            return _context.Acc_VoucherTypes.Where(x => !x.IsDelete).ToList();
        }

        public void SaveVoucherType(Acc_VoucherType VoucherType)
        {
            _context.Acc_VoucherTypes.Add(VoucherType);
            _context.SaveChanges();
            _context.Entry(VoucherType).GetDatabaseValues();

        }
    }
}
