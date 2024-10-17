using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IAccountProcessRepository
    {
        List<Acc_FiscalYear> FiscalYear();
        List<Acc_FiscalMonth> Acc_FiscalMonth(int? id);
        void SetProcess(string[] monthid, string criteria, int? Currency, string FYId, string MinAccCode, string MaxAccCode);
        int DefaultCountry();
        List<Acc_FiscalMonth> FiscalMonth();
        void prcDataSave(Acc_AccProcessViewModel model, string criteria);
        IEnumerable<SelectListItem> CountryId();

    }
}
