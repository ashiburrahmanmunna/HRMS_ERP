using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface IPFLedgerRepository : IBaseRepository<PF_Ledger>
    {
        List<PF_Ledger> PFLedgerList(string FromDate, string ToDate, string criteria);
        public IEnumerable<SelectListItem> BankAccountList();
        string PFSessionReport(string rptFormat, string FromDate, string ToDate, int? BankAccId);
        void CreatePFLedger(PF_Ledger PF_Ledger);
    }
}
