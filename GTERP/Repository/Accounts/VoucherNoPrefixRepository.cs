using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class VoucherNoPrefixRepository : BaseRepository<Acc_VoucherNoPrefix>, IVoucherNoPrefixRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public VoucherNoPrefixRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public Acc_VoucherNoPrefix FindByIdVoucherNo(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = _context.Acc_VoucherNoPrefixes
                .Where(c => c.ComId == comid).Where(c => c.VoucherNoPrefixId == id).FirstOrDefault();
            return data;
        }

        public List<Acc_VoucherNoPrefix> GetVoucherNo()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = _context.Acc_VoucherNoPrefixes
                .Include(x => x.vVoucherTypes).Where(c => c.ComId == comid && !c.IsDelete).ToList();
            return data;
        }

        public IEnumerable<SelectListItem> VoucherTypeList()
        {
            return new SelectList(_context.Acc_VoucherTypes
                .Where(c => c.VoucherTypeId > 0), "VoucherTypeId", "VoucherTypeName");
        }
    }
}
