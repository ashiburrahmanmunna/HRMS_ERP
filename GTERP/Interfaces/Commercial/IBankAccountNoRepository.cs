using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Commercial
{
    public interface IBankAccountNoRepository : IBaseRepository<BankAccountNo>
    {
        IEnumerable<SelectListItem> SisterConcernCompanyId();
        IEnumerable<SelectListItem> OpeningBankId();
        List<BankAccountNo> GetBanAccNo();
        BankAccountNo FindByIdBankAccNo(int? id);
    }
}
