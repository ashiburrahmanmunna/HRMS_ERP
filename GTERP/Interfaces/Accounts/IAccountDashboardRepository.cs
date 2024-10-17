using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IAccountDashboardRepository
    {
        IEnumerable<SelectListItem> SisterConcernCompanyId();
        IEnumerable<SelectListItem> BuyerGroupId();
        List<Dashboard1> CashPayment(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> CashReceipt(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> BankPayment(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> BankReceipt(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> Contra(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> Journal(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard2> MonthWiseCashReceipt(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard2> MonthWiseCashPayment(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard2> MonthWiseBankReceipt(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard2> MonthWiseBankPayment(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard2> MonthWiseContra(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard2> MonthWiseJournal(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard3> MonthUserWiseAllVoucher(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard3> MonthUserWiseCashPayment(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard3> MonthUserWiseCashReceipt(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard3> MonthUserWiseBankPayment(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard3> MonthUserWiseBankReceipt(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard3> MonthUserWiseContra(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard3> MonthUserWiseJournal(DateTime? FromDate, DateTime? ToDate);
        List<TopTransaction> TopTransaction(DateTime? FromDate, DateTime? ToDate);
        List<TopTransaction> TopTransactionCR(DateTime? FromDate, DateTime? ToDate);
        List<TopTransaction> TopTransactionBP(DateTime? FromDate, DateTime? ToDate);
        List<TopTransaction> TopTransactionBR(DateTime? FromDate, DateTime? ToDate);
        List<TopTransaction> TopTransactionContra(DateTime? FromDate, DateTime? ToDate);
        List<TopTransaction> TopTransactionJournal(DateTime? FromDate, DateTime? ToDate);
        List<UserLogingInfoes> TopLogin(DateTime? FromDate, DateTime? ToDate);
        List<UserCountDocumentLastTransaction> VoucherUserCount(DateTime? FromDate, DateTime? ToDate);
        List<TopTransaction> AllUserTransaction(DateTime? FromDate, DateTime? ToDate);
        List<UserLogingInfoes> UserLogingInfoes(DateTime? FromDate, DateTime? ToDate);
        List<LastTransactions> LastTransactions(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard4> DayWiseVoucher(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> PostVoucher(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> UnPostVoucher(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> TotalDebit(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> TotalCredit(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> TotalCashBalance(DateTime? FromDate, DateTime? ToDate);
        List<Dashboard1> TotalBankBalance(DateTime? FromDate, DateTime? ToDate);
        List<LedgerBalance> CashBalanceList(DateTime? FromDate, DateTime? ToDate);
        List<LedgerBalance> BankBalanceList(DateTime? FromDate, DateTime? ToDate);
        List<LedgerBalance> AccountsPayableBalanceList(DateTime? FromDate, DateTime? ToDate);
        List<LedgerBalance> AccountsReceivableBalanceList(DateTime? FromDate, DateTime? ToDate);
        List<LedgerBalance> FixedDepostiBalanceList(DateTime? FromDate, DateTime? ToDate);
        List<LedgerBalance> FixedAssetBalanceList(DateTime? FromDate, DateTime? ToDate);

    }
}
