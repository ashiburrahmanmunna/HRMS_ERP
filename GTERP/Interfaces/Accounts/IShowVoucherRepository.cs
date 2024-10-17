using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IShowVoucherRepository
    {
        int DefaultCountry();
        IEnumerable<SelectListItem> VoucherTypeId();
        IEnumerable<SelectListItem> PrdUnitId();
        List<Acc_FiscalMonth> FiscalMonth();
        List<Acc_FiscalYear> FiscalYear();
        string SetSession(string criteria, string rptFormat, string rpttype, string dtFrom, string dtTo,
            string VoucherFrom, string VoucherTo, int? Currency, int? isPosted, int? isOther, int? FYId, int? VoucherTypeId, int? AccId, int? PrdUnitId);
    }
}
