using GTERP.Interfaces.Accounts;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class AccountDashboardRepository : IAccountDashboardRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public AccountDashboardRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<Dashboard1> BankPayment(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> BankPayment = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
            new SqlParameter[]
                {
                new SqlParameter("@Criteria", "Bank Payment"),
                new SqlParameter("@FromDate", FromDate),
                new SqlParameter("@ToDate", ToDate),
                new SqlParameter("@ComId", comid),
                }
            );
            return BankPayment;
        }

        public IEnumerable<SelectListItem> BuyerGroupId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.BuyerGroups.Where(x => x.ComId.ToString() == comid).Take(0), "BuyerGroupId", "BuyerGroupName");
        }

        public List<Dashboard1> CashPayment(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "Cash Payment");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard1> CashPayment = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts", parameters);
            return CashPayment;
        }

        public List<Dashboard1> CashReceipt(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> CashReceipt = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
                new SqlParameter[] {
                    new SqlParameter("@Criteria", "Cash Receipt"),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ComId", comid)
                    }
                );
            return CashReceipt;

        }

        public List<Dashboard1> BankReceipt(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "Bank Receipt");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard1> BankReceipt = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts", parameters);
            return BankReceipt;
        }

        public IEnumerable<SelectListItem> SisterConcernCompanyId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.SisterConcernCompany
                .Where(x => x.ComId.ToString() == comid.ToString()), "SisterConcernCompanyId", "CompanyName");
        }

        public List<Dashboard1> Contra(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "Contra");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);


            List<Dashboard1> Contra = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts", parameters);
            return Contra;
        }

        public List<Dashboard1> Journal(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "Journal");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard1> Journal = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts", parameters);
            return Journal;
        }

        public List<Dashboard2> MonthWiseCashReceipt(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TotalCashReceipt");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard2> MonthWiseCashReceipt = Helper.ExecProcMapTList<Dashboard2>("prcGetDashboard_Accounts", parameters);
            return MonthWiseCashReceipt;
        }

        public List<Dashboard2> MonthWiseCashPayment(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TotalCashPayment");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard2> MonthWiseCashPayment = Helper.ExecProcMapTList<Dashboard2>("prcGetDashboard_Accounts", parameters);

            return MonthWiseCashPayment;
        }

        public List<Dashboard2> MonthWiseBankReceipt(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "TotalBankReceipt");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard2> MonthWiseBankReceipt = Helper.ExecProcMapTList<Dashboard2>("prcGetDashboard_Accounts", parameters);
            return MonthWiseBankReceipt;
        }

        public List<Dashboard2> MonthWiseBankPayment(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "TotalBankPayment");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard2> MonthWiseBankPayment = Helper.ExecProcMapTList<Dashboard2>("prcGetDashboard_Accounts", parameters);
            return MonthWiseBankPayment;
        }

        public List<Dashboard2> MonthWiseContra(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "TotalContra");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard2> MonthWiseContra = Helper.ExecProcMapTList<Dashboard2>("prcGetDashboard_Accounts", parameters);
            return MonthWiseContra;
        }

        public List<Dashboard2> MonthWiseJournal(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "TotalJournal");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard2> MonthWiseJournal = Helper.ExecProcMapTList<Dashboard2>("prcGetDashboard_Accounts", parameters);
            return MonthWiseJournal;
        }

        public List<Dashboard3> MonthUserWiseAllVoucher(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "MonthUserWiseAllVoucher");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard3> MonthUserWiseAllVoucher = Helper.ExecProcMapTList<Dashboard3>("prcGetDashboard_Accounts", parameters);
            return MonthUserWiseAllVoucher;
        }

        public List<Dashboard3> MonthUserWiseCashPayment(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "MonthUserWiseCashPayment");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard3> MonthUserWiseCashPayment = Helper.ExecProcMapTList<Dashboard3>("prcGetDashboard_Accounts", parameters);
            return MonthUserWiseCashPayment;
        }

        public List<Dashboard3> MonthUserWiseCashReceipt(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "MonthUserWiseCashReceipt");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard3> MonthUserWiseCashReceipt = Helper.ExecProcMapTList<Dashboard3>("prcGetDashboard_Accounts", parameters);

            return MonthUserWiseCashReceipt;
        }

        public List<Dashboard3> MonthUserWiseBankPayment(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "MonthUserWiseBankPayment");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard3> MonthUserWiseBankPayment = Helper.ExecProcMapTList<Dashboard3>("prcGetDashboard_Accounts", parameters);
            return MonthUserWiseBankPayment;
        }

        public List<Dashboard3> MonthUserWiseBankReceipt(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "MonthUserWiseBankReceipt");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard3> MonthUserWiseBankReceipt = Helper.ExecProcMapTList<Dashboard3>("prcGetDashboard_Accounts", parameters);
            return MonthUserWiseBankReceipt;
        }

        public List<Dashboard3> MonthUserWiseContra(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "MonthUserWiseContra");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard3> MonthUserWiseContra = Helper.ExecProcMapTList<Dashboard3>("prcGetDashboard_Accounts", parameters);
            return MonthUserWiseContra;
        }

        public List<Dashboard3> MonthUserWiseJournal(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "MonthUserWiseJournal");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);
            List<Dashboard3> MonthUserWiseJournal = Helper.ExecProcMapTList<Dashboard3>("prcGetDashboard_Accounts", parameters);
            return MonthUserWiseJournal;
        }

        public List<TopTransaction> TopTransaction(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "TopTransactionCashPayment");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);
            List<TopTransaction> TopTransactionCP = Helper.ExecProcMapTList<TopTransaction>("prcGetDashboard_Accounts", parameters);
            return TopTransactionCP;
        }

        public List<TopTransaction> TopTransactionCR(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TopTransactionCashReceipt");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);
            List<TopTransaction> TopTransactionCR = Helper.ExecProcMapTList<TopTransaction>("prcGetDashboard_Accounts", parameters);
            return TopTransactionCR;
        }

        public List<TopTransaction> TopTransactionBP(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TopTransactionBankPayment");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);
            List<TopTransaction> TopTransactionBP = Helper.ExecProcMapTList<TopTransaction>("prcGetDashboard_Accounts", parameters);
            return TopTransactionBP;
        }

        public List<TopTransaction> TopTransactionBR(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TopTransactionBankReceipt");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<TopTransaction> TopTransactionBR = Helper.ExecProcMapTList<TopTransaction>("prcGetDashboard_Accounts", parameters);
            return TopTransactionBR;
        }

        public List<TopTransaction> TopTransactionContra(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TopTransactionContra");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<TopTransaction> TopTransactionContra = Helper.ExecProcMapTList<TopTransaction>("prcGetDashboard_Accounts", parameters);
            return TopTransactionContra;
        }

        public List<TopTransaction> TopTransactionJournal(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TopTransactionJournal");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<TopTransaction> TopTransactionJournal = Helper.ExecProcMapTList<TopTransaction>("prcGetDashboard_Accounts", parameters);
            return TopTransactionJournal;
        }

        public List<UserLogingInfoes> TopLogin(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "TopLogin");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<UserLogingInfoes> TopLogin = Helper.ExecProcMapTList<UserLogingInfoes>("prcGetDashboard_Accounts", parameters);
            return TopLogin;
        }

        public List<UserCountDocumentLastTransaction> VoucherUserCount(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "VoucherUserCount");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<UserCountDocumentLastTransaction> VoucherUserCount = Helper.ExecProcMapTList<UserCountDocumentLastTransaction>("prcGetDashboard_Accounts", parameters);
            return VoucherUserCount;
        }

        public List<TopTransaction> AllUserTransaction(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "TopUserTransaction");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<TopTransaction> AllUserTransaction = Helper.ExecProcMapTList<TopTransaction>("prcGetDashboard_Accounts", parameters);
            return AllUserTransaction;
        }

        public List<UserLogingInfoes> UserLogingInfoes(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "UserLogingInfoes");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);
            List<UserLogingInfoes> UserLogingInfoes = Helper.ExecProcMapTList<UserLogingInfoes>("prcGetDashboard_Accounts", parameters);
            return UserLogingInfoes;
        }

        public List<LastTransactions> LastTransactions(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "LastTransactions");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<LastTransactions> LastTransactions = Helper.ExecProcMapTList<LastTransactions>("prcGetDashboard_Accounts", parameters);
            return LastTransactions;
        }

        public List<Dashboard4> DayWiseVoucher(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "DayWiseVoucher");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<Dashboard4> DayWiseVoucher = Helper.ExecProcMapTList<Dashboard4>("prcGetDashboard_Accounts", parameters);
            return DayWiseVoucher;

        }

        public List<Dashboard1> PostVoucher(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> PostVoucher = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
                new SqlParameter[] {
                    new SqlParameter("@Criteria", "Post Voucher"),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ComId", comid)
                    }
                );
            return PostVoucher;
        }

        public List<Dashboard1> UnPostVoucher(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> UnPostVoucher = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
               new SqlParameter[] {
                    new SqlParameter("@Criteria", "UnPost Voucher"),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ComId", comid)
                   }
               );
            return UnPostVoucher;
        }

        public List<Dashboard1> TotalDebit(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> TotalDebit = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
                new SqlParameter[] {
                    new SqlParameter("@Criteria", "Total Debit"),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ComId", comid)
                    }
                );
            return TotalDebit;

        }

        public List<Dashboard1> TotalCredit(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> TotalCredit = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
                new SqlParameter[] {
                    new SqlParameter("@Criteria", "Total Credit"),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ComId", comid)
                    }
                );
            return TotalCredit;
        }

        public List<Dashboard1> TotalCashBalance(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> TotalCashBalance = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
                new SqlParameter[] {
                    new SqlParameter("@Criteria", "Total Cash Balance"),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ComId", comid)
                    }
                );
            return TotalCashBalance;
        }

        public List<Dashboard1> TotalBankBalance(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Dashboard1> TotalBankBalance = Helper.ExecProcMapTList<Dashboard1>("prcGetDashboard_Accounts",
                new SqlParameter[] {
                    new SqlParameter("@Criteria", "Total Bank Balance"),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ComId", comid)
                    }
                );
            return TotalBankBalance;

        }

        public List<LedgerBalance> CashBalanceList(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "CashBalance");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<LedgerBalance> CashBalanceList = Helper.ExecProcMapTList<LedgerBalance>("prcGetDashboard_Accounts", parameters);
            return CashBalanceList;
        }

        public List<LedgerBalance> BankBalanceList(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "BankBalance");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<LedgerBalance> BankBalanceList = Helper.ExecProcMapTList<LedgerBalance>("prcGetDashboard_Accounts", parameters);
            return BankBalanceList;
        }

        public List<LedgerBalance> AccountsPayableBalanceList(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "AccountsPayableBalance");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<LedgerBalance> AccountsPayableBalanceList = Helper.ExecProcMapTList<LedgerBalance>("prcGetDashboard_Accounts", parameters);
            return AccountsPayableBalanceList;
        }

        public List<LedgerBalance> AccountsReceivableBalanceList(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "AccountsReceivableBalance");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<LedgerBalance> AccountsReceivableBalanceList = Helper.ExecProcMapTList<LedgerBalance>("prcGetDashboard_Accounts", parameters);
            return AccountsReceivableBalanceList;
        }

        public List<LedgerBalance> FixedDepostiBalanceList(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = new SqlParameter("@Criteria", "FixedDepostiBalance");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);
            List<LedgerBalance> FixedDepostiBalanceList = Helper.ExecProcMapTList<LedgerBalance>("prcGetDashboard_Accounts", parameters);
            return FixedDepostiBalanceList;
        }

        public List<LedgerBalance> FixedAssetBalanceList(DateTime? FromDate, DateTime? ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@Criteria", "FixedAssetBalance");
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@ComId", comid);

            List<LedgerBalance> FixedAssetBalanceList = Helper.ExecProcMapTList<LedgerBalance>("prcGetDashboard_Accounts", parameters);
            return FixedAssetBalanceList;
        }
    }
}
