using GTERP.Interfaces.Self;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IVoucherTypeRepository : ISelfRepository<Acc_VoucherType>
    {
        List<Acc_VoucherType> GetVoucherType();
        void SaveVoucherType(Acc_VoucherType VoucherType);
    }
}
