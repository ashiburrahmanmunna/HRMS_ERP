using GTERP.Interfaces.Accounts;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class BankReconcilRepository : IBankReconcilRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public BankReconcilRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public List<Acc_VoucherSubCheckno> GetAccVoucherSub()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_VoucherSubChecnos.Include(x => x.vAcc_ChartOfAccount).Where(x => x.VoucherId == null && x.ComId == comid).Where(x => x.dtChk == null).ToList();
        }

        public List<Acc_VoucherSubCheckno> GetAccVoucherSubElse()
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yy"));
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Acc_VoucherSubChecnos.Include(x => x.vAcc_ChartOfAccount).Where(x => x.VoucherId == null && x.ComId == comid)
                .Where(x => x.dtChk >= dtFrom && x.dtChk <= dtTo).ToList();
        }
    }
}
