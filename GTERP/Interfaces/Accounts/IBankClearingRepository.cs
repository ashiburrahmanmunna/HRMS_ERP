using GTERP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Accounts
{
    public interface IBankClearingRepository
    {
        List<BankClearing> GetBankClearing(string FromDate, string ToDate, string criteria);
        string Print(int? id, string type);
        void SetProcess(List<BankClearing> BankClearinglist, string criteria);
    }
}
