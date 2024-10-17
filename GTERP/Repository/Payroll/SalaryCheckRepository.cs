using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using GTERP.Interfaces.Payroll;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Payroll
{
    public class SalaryCheckRepository : ISalaryCheckRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public SalaryCheckRepository(GTRDBContext context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public List<Pross> GetProssType()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter1 = new SqlParameter[1];
            parameter1[0] = new SqlParameter("@ComId", comid);
            var pType = Helper.ExecProcMapTList<Pross>("GetProssType", parameter1);
            return pType;
        }

        public IEnumerable<SelectListItem> GetSalaryCheckList()
        {
            throw new NotImplementedException();
        }

        public List<SalaryCheck> GetAllSalaryCheck(string prossType, int fun2)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            string userid = _httpcontext.HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

            var temp = 0;
            if (approvetype.Contains(1186) && approvetype.Count == 1)
            {
                temp = 1186;
            }
            else
            if (approvetype.Contains(1187) && approvetype.Count == 1)
            {
                temp = 1187;
            }
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Count == 2)
            {
                temp = 11861187;
            }
            if (approvetype.Contains(1257) && approvetype.Count == 1)
            {
                temp = 1257;
            }

            SqlParameter[] parameter = new SqlParameter[8];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            parameter[2] = new SqlParameter("@fun2", fun2);
            parameter[3] = new SqlParameter("@PageSize", 10);
            parameter[4] = new SqlParameter("@PageIndex", 1);
            parameter[5] = new SqlParameter("@SearchKeywords", "");
            parameter[6] = new SqlParameter("@SearchColumns", "");
            parameter[7] = new SqlParameter("@approval", temp);
            var salCheck = Helper.ExecProcMapTList<SalaryCheck>("Payroll_prcGetSalaryCheck", parameter);
            return salCheck;
        }
        public List<SalaryCheck> SalaryCheckList(string prossType, int fun2, int pageIndex, int pageSize, string searchColumns, string searchKeywords)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            string userid = _httpcontext.HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

            var temp = 0;
            if(approvetype.Contains(1186) && approvetype.Count == 1)
            {
                temp = 1186;
            }
            else
            if (approvetype.Contains(1187) && approvetype.Count == 1)
            {
                temp = 1187;
            }
            if(approvetype.Contains(1186)&& approvetype.Contains(1187) && approvetype.Count == 2)
            {
                temp = 11861187;
            }
            if (approvetype.Contains(1257) && approvetype.Count == 1)
            {
                temp = 1257;
            }

            SqlParameter[] parameter = new SqlParameter[8];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            parameter[2] = new SqlParameter("@fun2", fun2);
            parameter[3] = new SqlParameter("@PageSize", pageSize);
            parameter[4] = new SqlParameter("@PageIndex", pageIndex);
            parameter[5] = new SqlParameter("@SearchKeywords", searchKeywords);
            parameter[6] = new SqlParameter("@SearchColumns", searchColumns);
            parameter[7] = new SqlParameter("@approval", temp);
            var salCheck = Helper.ExecProcMapTList<SalaryCheck>("Payroll_prcGetSalaryCheck", parameter);

            return salCheck;
        }

        public List<TotalCount> SalaryCheckCount(string prossType, int fun2, int pageIndex, int pageSize, string searchColumns, string searchKeywords)
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            string userid = _httpcontext.HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();

            //var temp = 0;
            //if (approvetype.Contains(1186))
            //{
            //    temp = 1186;
            //}
            //if (approvetype.Contains(1187))
            //{
            //    temp = 1187;
            //}
            var temp = 0;
            if (approvetype.Contains(1186) && approvetype.Count == 1)
            {
                temp = 1186;
            }
            else
            if (approvetype.Contains(1187) && approvetype.Count == 1)
            {
                temp = 1187;
            }
            if (approvetype.Contains(1186) && approvetype.Contains(1187) && approvetype.Count == 2)
            {
                temp = 11861187;
            }
            if (approvetype.Contains(1257) && approvetype.Count == 1)
            {
                temp = 1257;
            }

            SqlParameter[] parameter = new SqlParameter[8];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            parameter[2] = new SqlParameter("@fun2", fun2);
            parameter[3] = new SqlParameter("@PageSize", pageSize);
            parameter[4] = new SqlParameter("@PageIndex", pageIndex);
            parameter[5] = new SqlParameter("@SearchKeywords", searchKeywords);
            parameter[6] = new SqlParameter("@SearchColumns", searchColumns);
            parameter[7] = new SqlParameter("@approval", temp);

            var salCheck = Helper.ExecProcMapTList<TotalCount>("Payroll_prcGetSalaryCheck", parameter);

            return salCheck;
        }
    }
}
