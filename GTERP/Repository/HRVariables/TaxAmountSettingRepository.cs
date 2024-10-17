using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HRVariables
{
    public class TaxAmountSettingRepository:BaseRepository<Payroll_InComeTaxAmountSetting>,ITaxAmountSettingRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public TaxAmountSettingRepository(GTRDBContext context, IHttpContextAccessor httpContext):base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<Payroll_IncomeTaxAmountVM> GetIncometaxList()
        {
            var query = $"Exec Payroll_PrcIncomeTaxAmount";
            return Helper.ExecProcMapTList<Payroll_IncomeTaxAmountVM>("Payroll_PrcIncomeTaxAmount");
        }

        public void SaveData(Payroll_InComeTaxAmountSetting payroll_InComeTaxAmountSetting)
        {

            payroll_InComeTaxAmountSetting.UserId = _httpContext.HttpContext.Session.GetString("userid");
            payroll_InComeTaxAmountSetting.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            payroll_InComeTaxAmountSetting.ComId = payroll_InComeTaxAmountSetting.ComId;
            payroll_InComeTaxAmountSetting.DateUpdated = DateTime.Today;
            payroll_InComeTaxAmountSetting.DateAdded = DateTime.Today;

            _context.Add(payroll_InComeTaxAmountSetting);
            _context.SaveChanges();
        }

        public void UpdateData(Payroll_InComeTaxAmountSetting payroll_InComeTaxAmountSetting)
        {
            payroll_InComeTaxAmountSetting.UserId = _httpContext.HttpContext.Session.GetString("userid");
            payroll_InComeTaxAmountSetting.ComId = payroll_InComeTaxAmountSetting.ComId;
            payroll_InComeTaxAmountSetting.DateUpdated = DateTime.Today;
            payroll_InComeTaxAmountSetting.DateAdded = DateTime.Today;
            payroll_InComeTaxAmountSetting.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");

            _context.Update(payroll_InComeTaxAmountSetting);
            _context.SaveChanges();
        }
    }
}
