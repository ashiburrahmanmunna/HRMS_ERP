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
    public class EmpWiseSalaryLedgerRepository : IEmpWiseSalaryLedgerRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public EmpWiseSalaryLedgerRepository(GTRDBContext context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public List<EmpWiseSalaryLedger> EmpWiseSalaryLedgerList(int? EmpId, int fun2, int pageIndex, int pageSize, string SearchColumns, string SearchKeywords, DateTime dtFrom, DateTime dtTo)
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

            //SqlParameter[] parameter1 = new SqlParameter[3];
            // parameter1[0] = new SqlParameter("@ComId", comid);


            List<EmpWiseSalaryLedger> EmpWiseSalaryLedger = null;
            if (EmpId != null)
            {
                SqlParameter[] parameter = new SqlParameter[10];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", EmpId);
                parameter[2] = new SqlParameter("@dtFrom", dtFrom);
                parameter[3] = new SqlParameter("@dtTo", dtTo);
                parameter[4] = new SqlParameter("@fun2", fun2);
                parameter[5] = new SqlParameter("@PageSize", pageSize);
                parameter[6] = new SqlParameter("@PageIndex", pageIndex);
                parameter[7] = new SqlParameter("@SearchKeywords", SearchKeywords);
                parameter[8] = new SqlParameter("@SearchColumns", SearchColumns);
                parameter[9] = new SqlParameter("@approval", temp);


                EmpWiseSalaryLedger = Helper.ExecProcMapTList<EmpWiseSalaryLedger>("Payroll_prcGetEmpSalaryLedgerTest1", parameter);

            }
            return EmpWiseSalaryLedger;
        }

        public List<PageNo> EmpWiseSalaryLedgerCount(int? EmpId, int fun2, int pageIndex, int pageSize, string SearchColumns, string SearchKeywords, DateTime dtFrom, DateTime dtTo)
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

            //SqlParameter[] parameter1 = new SqlParameter[3];
            // parameter1[0] = new SqlParameter("@ComId", comid);


            List<PageNo> EmpWiseSalaryLedger = null;
            if (EmpId != null)
            {
                SqlParameter[] parameter = new SqlParameter[10];
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@EmpId", EmpId);
                parameter[2] = new SqlParameter("@dtFrom", dtFrom);
                parameter[3] = new SqlParameter("@dtTo", dtTo);
                parameter[4] = new SqlParameter("@fun2", fun2);
                parameter[5] = new SqlParameter("@PageSize", pageSize);
                parameter[6] = new SqlParameter("@PageIndex", pageIndex);
                parameter[7] = new SqlParameter("@SearchKeywords", SearchKeywords);
                parameter[8] = new SqlParameter("@SearchColumns", SearchColumns);
                parameter[9] = new SqlParameter("@approval", temp);


                EmpWiseSalaryLedger = Helper.ExecProcMapTList<PageNo>("Payroll_prcGetEmpSalaryLedgerTest1", parameter);

            }
            return EmpWiseSalaryLedger;
        }
        public IEnumerable<SelectListItem> GetEmpList()
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");
            string userid = _httpcontext.HttpContext.Session.GetString("userid");
            var approvetype = _context.HR_ApprovalSettings.Where(w => w.ComId == comid && w.UserId == userid && w.IsDelete == false).Select(s => s.ApprovalType).ToList();


            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in _context.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ]  - [ " + emptype.EmpTypeName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");


        }

        public IEnumerable<SelectListItem> GetEmpWiseSalaryLedgerList()
        {
            throw new NotImplementedException();
        }
    }
}