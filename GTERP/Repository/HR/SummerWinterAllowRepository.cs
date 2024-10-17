using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace GTERP.Repository.HR
{
    public class SummerWinterAllowRepository : BaseRepository<HR_SummerWinterAllowance>, ISummerWinterAllowRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public SummerWinterAllowRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> SWAllowList()
        {
            return new SelectList(_context.Cat_SummerWinterAllowanceSetting, "SWAllowanceId", "ProssType");
        }

        public List<SummerWinterAllowViewModel> GetSummerWinterAllowAll(int? SWAId)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");


            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@SWAId", SWAId);
            var allowances = Helper.ExecProcMapTList<SummerWinterAllowViewModel>("HR_prcGetSummerWinterAllow", parameter);
            return allowances;


        }

        public IEnumerable<SelectListItem> GetSummerWinterAllowList()
        {
            throw new NotImplementedException();
        }

        public HR_SummerWinterAllowance AllowCalculation(HR_SummerWinterAllowance item, Cat_SummerWinterAllowanceSetting setting)
        {
            if (item.IsSummer)
            {
                item.SummerAllow = setting.SummerAllow;
            }
            if (item.IsWinter)
            {
                item.WinterAllow = setting.WinterAllow;
            }
            if (item.IsRaincoat)
            {
                item.RainCoatAndGumbootAllow = setting.RainCoatAndGumbootAllow;
            }

            item.Amount = item.SummerAllow + item.WinterAllow + item.RainCoatAndGumbootAllow;
            item.VatDed = (setting.VatDiduction / 100) * item.Amount;
            item.TaxDed = (setting.TaxDiduction / 100) * item.Amount;

            var ttlPercent = setting.VatDiduction + setting.TaxDiduction;

            item.NetAmount = item.Amount - (item.VatDed + item.TaxDed + item.Stamp);

            return item;
        }
    }
}
