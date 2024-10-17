using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using GTERP.Interfaces.Base;


namespace GTERP.Interfaces.PF
{
    public interface IPFProcessRepository
    {
        List<PF_FiscalYear> FiscalYear();
        IEnumerable<SelectListItem> FiscalYearID();
        List<PF_FiscalMonth> PF_FiscalMonth(int? id);
        void SetProcess(string[] monthid, string criteria, int? Currency, string FYId, string MinAccCode, string MaxAccCode);
        int DefaultCountry();
        List<PF_FiscalMonth> FiscalMonth();
        void prcDataSave(PF_PFProcessViewModel model, string criteria);
        IEnumerable<SelectListItem> CountryId();
        List<PF_FiscalYear> GetFiscalYear();
    }
}
