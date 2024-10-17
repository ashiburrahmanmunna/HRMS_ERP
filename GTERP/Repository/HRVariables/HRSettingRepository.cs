using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class HRSettingRepository : BaseRepository<Cat_HRSetting>, IHRSettingRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;

        public HRSettingRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public List<CatHRSettingVM> GetAllData()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            string userId = _httpContext.HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userId && w.IsApprove==true&& w.IsDelete==false).Select(s =>s.ApprovalType).ToList();
            if (approvetype.Contains(1186))
            {
                //string comid = _httpContext.HttpContext.Session.GetString("comid");

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EmpType", 1186);

                string query = $"Exec HR_prcGetHRSetting";

                var data = Helper.ExecProcMapTList<CatHRSettingVM>("HR_prcGetHRSetting", parameters);
                return data;
            }
            else {
                string query = $"Exec HR_prcGetHRSetting";

                var data = Helper.ExecProcMapTList<CatHRSettingVM>("HR_prcGetHRSetting");
                return data;
            }
        }

        public IEnumerable<SelectListItem> GetCompanyName()
        {
            return new SelectList(_context.Companys, "CompanyCode", "CompanyName");
        }

        public IEnumerable<SelectListItem> GetHRSettingList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.HR.ToString()
            });
        }
    }
}
