using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface IWFLedgerRepository : IBaseRepository<WF_Ledger>
    {
        List<WF_Ledger> WFLedgerList(string FromDate, string ToDate, string criteria);
        public IEnumerable<SelectListItem> BankAccountList();
        //void DeleteWFLedgerConfirmed(int? id);
        string SessionReport(string rptFormat, string FromDate, string ToDate, int? BankAccId);
        void CreateWFLedger(WF_Ledger WF_Ledger);
    }
}
