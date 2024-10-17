using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IVoucherNoPrefixRepository : IBaseRepository<Acc_VoucherNoPrefix>
    {
        List<Acc_VoucherNoPrefix> GetVoucherNo();
        IEnumerable<SelectListItem> VoucherTypeList();
        Acc_VoucherNoPrefix FindByIdVoucherNo(int? id);
    }
}
