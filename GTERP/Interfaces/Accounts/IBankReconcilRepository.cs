using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IBankReconcilRepository
    {
        List<Acc_VoucherSubCheckno> GetAccVoucherSub();
        List<Acc_VoucherSubCheckno> GetAccVoucherSubElse();
    }
}
