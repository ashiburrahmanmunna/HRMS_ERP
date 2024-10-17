using GTERP.Models;
using GTERP.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.Accounts
{
    public interface IAccPostVoucherRepository
    {
        List<Acc_VoucherMain> ModelList1(string FromDate, string ToDate);
        List<Acc_VoucherMain> ModelList2(string FromDate, string ToDate);
        List<Acc_VoucherMain> ModelList3(string FromDate, string ToDate);
        IQueryable<VoucherView> Query();
        void SetProcess(string[] voucherid, string criteria);
        string Print(int? id, string type);
    }
}
