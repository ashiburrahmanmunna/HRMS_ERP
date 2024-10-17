using GTERP.BLL;
using GTERP.Interfaces.PF;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;


namespace GTERP.Repository.PF
{
    public class PFProcessRepository : BaseRepository<PF_FiscalYear>, IPFProcessRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<PFProcessRepository> _logger;
        private readonly POSRepository POS;

        public PFProcessRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            ILogger<PFProcessRepository> logger,
            POSRepository pOSRepository
            ) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
            POS = pOSRepository;
        }

        public List<PF_FiscalMonth> PF_FiscalMonth(int? id)
        {
            var data = _context.PF_FiscalMonth
               .Where(x => x.FYId == id && x.isLocked == false
               && !_context.HR_ProcessLock.Any(p => p.FiscalMonthId == x.FiscalMonthId && p.IsLock == true && p.LockType.Contains("Account Lock"))).ToList();

            return data;
        }

        public IEnumerable<SelectListItem> CountryId()
        {
            return new SelectList(POS.GetCountry(), "CountryId", "CurrencyShortName", DefaultCountry());
        }

        public int DefaultCountry()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            int data = (_context.Companys.Where(a => a.CompanyCode == comid.ToString()).Select(a => a.CountryId).FirstOrDefault());
            return data;
        }

        public List<PF_FiscalMonth> FiscalMonth()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var fiscalyear = _context.PF_FiscalYear.Where(x => x.ComId == comid && x.isLocked == false).ToList();
            if (fiscalyear.Count == 0)
            {
                comid = "576B68B3-DA3F-4FE5-9656-BC92E4DCDF72";
                fiscalyear = _context.PF_FiscalYear.Where(x => x.ComId == comid && x.isLocked == false).ToList();
            }
            int fiscalyid = fiscalyear.Max(p => p.FYId);
            var data = _context.PF_FiscalMonth.Where(x => x.FYId == fiscalyid && x.isLocked == false).ToList();
            return data;
        }

        public List<PF_FiscalYear> FiscalYear()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = _context.PF_FiscalYear.Where(x => x.ComId == comid && x.isLocked == false).ToList();
            if (data.Count == 0)
            {
                comid = "576B68B3-DA3F-4FE5-9656-BC92E4DCDF72";
                data = _context.PF_FiscalYear.Where(x => x.ComId == comid && x.isLocked == false).ToList();
            }
            return data;
        }

        public IEnumerable<SelectListItem> FiscalYearID()
        {
            var fiscalYear = _context.PF_FiscalYear.Where(f => f.isRunning == true && f.isWorking == true).FirstOrDefault();
            return new SelectList(_context.PF_FiscalYear.OrderByDescending(y => y.FYName), "FiscalYearId", "FYName", fiscalYear.FiscalYearId);
        }

        public List<PF_FiscalYear> GetFiscalYear()
        {

            var data = GetAll()
                .OrderByDescending(x => x.FiscalYearId).ToList();
            return data;
        }



        public void prcDataSave(PF_PFProcessViewModel model, string criteria)
        {
            var sqlQuery = "";
            try
            {
                if (criteria.ToUpper().ToString() == "TrialB".ToUpper())
                {
                    if (model.ProcessMonths.Count > 0)
                    {
                        for (var i = 0; i < model.ProcessMonths.Count; i++)
                        {
                            if (model.ProcessMonths[i].isCheck == true)
                            {
                                sqlQuery = "Exec prcProcessTrailBalance " + _httpContext.HttpContext.Session.GetString("userid") + ", " + _httpContext.HttpContext.Session.GetString("comid") + ", " +
                                    model.ProcessMonths[i].MonthId + ", " + model.CountryId + ", '' ";
                                //arQuery.Add(sqlQuery);
                            }
                        }
                    }
                }
                else if (criteria.ToUpper().ToString() == "cogs".ToUpper())
                {
                    if (model.ProcessMonths.Count > 0)
                    {
                        for (var i = 0; i < model.ProcessMonths.Count; i++)
                        {
                            if (model.ProcessMonths[i].isCheck == true)
                            {
                                sqlQuery = "Exec GTRAccounts.dbo.prcProcessCostOfService " + _httpContext.HttpContext.Session.GetString("userid") + ", " +
                                _httpContext.HttpContext.Session.GetString("comid") + ", " + model.ProcessMonths[i].MonthId + ", " + model.CountryId + ", '' ";
                                //arQuery.Add(sqlQuery);
                            }
                        }
                    }
                }
                else if (criteria.ToUpper().ToString() == "income".ToUpper())
                {
                    if (model.ProcessMonths.Count > 0)
                    {
                        for (var i = 0; i < model.ProcessMonths.Count; i++)
                        {
                            if (model.ProcessMonths[i].isCheck == true)
                            {
                                sqlQuery = "Exec GTRAccounts.dbo.prcProcessIncome " + _httpContext.HttpContext.Session.GetString("userid") + ", " + _httpContext.HttpContext.Session.GetString("comid") + ", " + model.ProcessMonths[i].MonthId + ", " + model.CountryId + ", '' ";
                                //arQuery.Add(sqlQuery);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                throw ex;
            }
        }

        public void SetProcess(string[] monthid, string criteria, int? Currency, string FYId, string MinAccCode, string MaxAccCode)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");

            if (criteria.ToUpper().ToString() == "TrialB".ToUpper())
            {
                if (monthid.Count() > 0)
                {
                    for (var i = 0; i < monthid.Count(); i++)
                    {
                        var monthidsingle = monthid[i];

                        var query = $"Exec prcProcessTrailBalancePF '{userid}','{comid}',{monthidsingle},{Currency},'TrialBProcess' ";

                        SqlParameter[] sqlParameter = new SqlParameter[5];

                        sqlParameter[0] = new SqlParameter("@UserId", userid);
                        sqlParameter[1] = new SqlParameter("@ComId", comid);
                        sqlParameter[2] = new SqlParameter("@MonthId", monthidsingle);
                        sqlParameter[3] = new SqlParameter("@Currency", Currency);
                        sqlParameter[4] = new SqlParameter("@PCName", "TrialBProcess");

                        Helper.ExecProc("prcProcessTrailBalancePF", sqlParameter);

                    }
                }
            }
            else if (criteria.ToUpper().ToString() == "AllLedger".ToUpper())
            {
                if (monthid.Count() > 0)
                {
                    if (MinAccCode == null)
                    {
                        MinAccCode = "1-0-00-000-00000";
                    }
                    if (MaxAccCode == null)
                    {
                        MaxAccCode = "5-0-00-000-00000";
                    }

                    var query = $"Exec Acc_Process_LedgerMultiALL '{comid}','{userid}',{monthid.FirstOrDefault()},{Currency},'{MinAccCode}','{MaxAccCode}' ";

                    SqlParameter[] sqlParameter = new SqlParameter[6];

                    sqlParameter[0] = new SqlParameter("@ComId", comid);
                    sqlParameter[1] = new SqlParameter("@UserId", userid);
                    //sqlParameter[2] = new SqlParameter("@FYId", FYId);
                    sqlParameter[2] = new SqlParameter("@MonthId", monthid.FirstOrDefault());
                    sqlParameter[3] = new SqlParameter("@Currency", Currency);
                    sqlParameter[4] = new SqlParameter("@MinAccCode", MinAccCode);
                    sqlParameter[5] = new SqlParameter("@MaxAccCode", MaxAccCode);

                    //sqlParameter[6] = new SqlParameter("@PCName", "AllLedger");

                    Helper.ExecProc("Acc_Process_LedgerMultiALL", sqlParameter);



                }
            }
            else if (criteria.ToUpper().ToString() == "cogs".ToUpper())
            {
                if (monthid.Count() > 0)
                {
                    for (var i = 0; i < monthid.Count(); i++)
                    {
                        var monthidsingle = monthid[i];
                        var query = $"Exec prcProcessCostOfService '{userid}','{comid}',{monthidsingle},{Currency},'COGSProcess' ";

                        SqlParameter[] sqlParameter = new SqlParameter[5];
                        sqlParameter[0] = new SqlParameter("@UserId", userid);
                        sqlParameter[1] = new SqlParameter("@ComId", comid);
                        sqlParameter[2] = new SqlParameter("@MonthId", monthidsingle);
                        sqlParameter[3] = new SqlParameter("@Currency", Currency);
                        sqlParameter[4] = new SqlParameter("@PCName", "COGSProcess");

                        Helper.ExecProc("prcProcessCostOfService", sqlParameter);
                    }
                }
            }
            else if (criteria.ToUpper().ToString() == "income".ToUpper())
            {
                if (monthid.Count() > 0)
                {
                    for (var i = 0; i < monthid.Count(); i++)
                    {
                        var monthidsingle = monthid[i];
                        var query = $"Exec prcProcessIncomePF '{userid}','{comid}',{monthidsingle},{Currency},'IncomeProcess' ";

                        SqlParameter[] sqlParameter = new SqlParameter[5];
                        sqlParameter[0] = new SqlParameter("@UserId", userid);
                        sqlParameter[1] = new SqlParameter("@ComId", comid);
                        sqlParameter[2] = new SqlParameter("@MonthId", monthidsingle);
                        sqlParameter[3] = new SqlParameter("@Currency", Currency);
                        sqlParameter[4] = new SqlParameter("@PCName", "IncomeProcess");
                        Helper.ExecProc("prcProcessIncomePF", sqlParameter);

                    }
                }
            }
            else if (criteria.ToUpper().ToString() == "bs".ToUpper())
            {
                if (monthid.Count() > 0)
                {

                    for (var i = 0; i < monthid.Count(); i++)
                    {
                        var monthidsingle = monthid[i];
                        var query = $"Exec [prcProcessBalanceSheetPF] '{userid}','{comid}',{monthidsingle},{Currency},'BalanceSheetProcess' ";

                        SqlParameter[] sqlParameter = new SqlParameter[5];
                        sqlParameter[0] = new SqlParameter("@UserId", userid);
                        sqlParameter[1] = new SqlParameter("@ComId", comid);
                        sqlParameter[2] = new SqlParameter("@MonthId", monthidsingle);
                        sqlParameter[3] = new SqlParameter("@Currency", Currency);
                        sqlParameter[4] = new SqlParameter("@PCName", "BalanceSheetProcess");
                        Helper.ExecProc("prcProcessBalanceSheetPF", sqlParameter);

                    }
                }
            }
            else if (criteria.ToUpper().ToString() == "cb".ToUpper())
            {
                if (monthid.Count() > 0)
                {
                    for (var i = 0; i < monthid.Count(); i++)
                    {
                        var monthidsingle = monthid[i];

                        //Exec prcProcessIncome '4864add7-0ab2-4c4f-9eb8-6b63a425e665','31312c54-659b-4e63-b4ba-2bc3d7b05792',13,18,'Fahad'
                        var query = $"Exec [prcProcessCostBreakup] '{userid}','{comid}',{monthidsingle},{Currency},'CBProcess' ";

                        SqlParameter[] sqlParameter = new SqlParameter[5];
                        sqlParameter[0] = new SqlParameter("@UserId", userid);
                        sqlParameter[1] = new SqlParameter("@ComId", comid);
                        sqlParameter[2] = new SqlParameter("@MonthId", monthidsingle);
                        sqlParameter[3] = new SqlParameter("@Currency", Currency);
                        sqlParameter[4] = new SqlParameter("@PCName", "CBProcess");

                        Helper.ExecProc("prcProcessCostBreakup", sqlParameter);

                    }

                }
            }
            else if (criteria.ToUpper().ToString() == "all".ToUpper())
            {
                if (monthid.Count() > 0)
                {

                    for (var i = 0; i < monthid.Count(); i++)
                    {
                        var monthidsingle = monthid[i];

                        //Exec prcProcessIncome '4864add7-0ab2-4c4f-9eb8-6b63a425e665','31312c54-659b-4e63-b4ba-2bc3d7b05792',13,18,'Fahad'
                        var query = $"Exec prcProcessTrailBalance '{userid}','{comid}',{monthidsingle},{Currency},'TrialBProcess' ";
                        query = $"Exec prcProcessCostOfService '{userid}','{comid}',{monthidsingle},{Currency},'COGSProcess' ";
                        query = $"Exec prcProcessIncome '{userid}','{comid}',{monthidsingle},{Currency},'IncomeProcess' ";
                        query = $"Exec [prcProcessBalanceSheet] '{userid}','{comid}',{monthidsingle},{Currency},'BalanceSheetProcess' ";
                        query = $"Exec [prcProcessCostBreakup] '{userid}','{comid}',{monthidsingle},{Currency},'CBProcess' ";

                        SqlParameter[] sqlParameter = new SqlParameter[5];
                        sqlParameter[0] = new SqlParameter("@UserId", userid);
                        sqlParameter[1] = new SqlParameter("@ComId", comid);
                        sqlParameter[2] = new SqlParameter("@MonthId", monthidsingle);
                        sqlParameter[3] = new SqlParameter("@Currency", Currency);
                        sqlParameter[4] = new SqlParameter("@PCName", "ALLProcess");
                        Helper.ExecProc("prcProcessTrailBalance", sqlParameter);
                        Helper.ExecProc("prcProcessCostOfService", sqlParameter);
                        Helper.ExecProc("prcProcessIncome", sqlParameter);
                        Helper.ExecProc("prcProcessBalanceSheet", sqlParameter);
                        Helper.ExecProc("prcProcessCostBreakup", sqlParameter);

                    }

                }
            }
            else if (criteria.ToUpper().ToString() == "notes".ToUpper())
            {
                var monthidsingle = 0;// monthid[i];

                //Exec prcProcessIncome '4864add7-0ab2-4c4f-9eb8-6b63a425e665','31312c54-659b-4e63-b4ba-2bc3d7b05792',13,18,'Fahad'
                var query = $"Exec [prcProcessNotesBCIC] '{userid}','{comid}',{monthidsingle},{Currency},'NoteProcess' ,{FYId}  ";

                SqlParameter[] sqlParameter = new SqlParameter[6];

                sqlParameter[0] = new SqlParameter("@UserId", userid);
                sqlParameter[1] = new SqlParameter("@ComId", comid);
                sqlParameter[2] = new SqlParameter("@MonthId", monthidsingle);
                sqlParameter[3] = new SqlParameter("@Currency", Currency);
                sqlParameter[4] = new SqlParameter("@PCName", "NoteProcess");
                sqlParameter[5] = new SqlParameter("@FiscalYearId", FYId);

                Helper.ExecProc("prcProcessNotesBCIC", sqlParameter);

            }
        }

      
    }
}
