using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface IGratuityLedgerRepository : IBaseRepository<Gratuity_Ledger>
    {
        List<Gratuity_Ledger> GratuityLedgerList(string FromDate, string ToDate, string criteria);
        public IEnumerable<SelectListItem> BankAccountList();
        string GratuitySessionReport(string rptFormat, string FromDate, string ToDate, int? BankAccId);
        void CreateGratuityLedger(Gratuity_Ledger Gratuity_Ledger);
    }
}
