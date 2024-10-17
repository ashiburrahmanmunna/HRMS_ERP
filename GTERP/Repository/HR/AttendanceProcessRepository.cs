using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class AttendanceProcessRepository : IAttendanceProcessRepository
    {
        DataSet dsList;
        DataSet dsDetails;
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public AttendanceProcessRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public HR_AttendanceProcess GetAttProcess(string msg)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            HR_AttendanceProcess model = new HR_AttendanceProcess();

            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            model.dtLast = _context.HR_ProssType.Where(x => x.ComId == comid).OrderByDescending(p => p.ProssDt).FirstOrDefault().ProssDt;
            model.dtProcess = now;
            model.dtTo = endDate;
            return model;
        }

        public IEnumerable<SelectListItem> GetEmpSelectList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid && x.IsDelete==false), "EmpId", "EmpCode");
        }



        public void prcInsertEmp(HR_AttendanceProcess model, string optCriteria)
        {
            ArrayList arQuery = new ArrayList();
            //clsProcedure clsProc = new clsProcedure();
            //clsConnectionNew clsCon = new clsConnectionNew("GTRHRIS_WEBDEMO", true);
            string SQLQuery = "", SectId = "", DesigId = "", EmpId = "", ShiftId = "", SubSectId = "", Band = "";
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");

            var processDate = Helper.GTRDate(model.dtProcess.ToString());
            SqlParameter[] parameter = new SqlParameter[4];
            //Collecting Parameter Value
            if (optCriteria.ToString().ToUpper() == "All".ToUpper())
            {

                //SQLQuery = "Delete tblTempCount;Insert Into tblTempCount (EmpID, DateTime1) Select EmpId,'" + Helper.GTRDate(model.dtProcess.ToString())
                //           + "' from tblEmp_Info Where ComId = " + comid + " ";
                //arQuery.Add(SQLQuery);
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@dtPross", processDate);
                parameter[2] = new SqlParameter("@optCriteria", "All");
                parameter[3] = new SqlParameter("@id", "0");// no needed for all
            }
            else if (optCriteria.ToUpper() == "Sec".ToUpper())
            {
                SectId = model.SectId.ToString();
                //SQLQuery = "Delete tblTempCount;Insert Into tblTempCount (EmpID, DateTime1) Select EmpID,'" + Helper.GTRDate(model.dtProcess.ToString())
                //           + "' from tblEmp_Info Where ComId = " + comid + " and SectId = '" + SectId + "' ";
                //arQuery.Add(SQLQuery);
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@dtPross", processDate);
                parameter[2] = new SqlParameter("@optCriteria", "Sec");
                parameter[3] = new SqlParameter("@id", SectId);
            }

            else if (optCriteria.ToUpper() == "Desig".ToUpper())
            {
                DesigId = model.DesigId.ToString();
                //SQLQuery = "Delete tblTempCount;Insert Into tblTempCount (EmpID, DateTime1) Select EmpId,'" + Helper.GTRDate(model.dtProcess.ToString())
                //           + "' from tblEmp_Info Where ComID = " + comid + " and DesigId = '" + DesigId + "'";
                //arQuery.Add(SQLQuery);               
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@dtPross", processDate);
                parameter[2] = new SqlParameter("@optCriteria", "EmpId");
                parameter[3] = new SqlParameter("@id", DesigId);
            }

            else if (optCriteria.ToUpper() == "EmpId".ToUpper())
            {
                EmpId = model.EmpId.ToString();
                //SQLQuery = "Delete tblTempCount;Insert Into tblTempCount (EmpID, DateTime1) Select EmpId,'" + Helper.GTRDate(model.dtProcess.ToString())
                //           + "' from tblEmp_Info Where ComID = " + comid + " and EmpId = '" + EmpId + "'";
                //arQuery.Add(SQLQuery);
                parameter[0] = new SqlParameter("@ComId", comid);
                parameter[1] = new SqlParameter("@dtPross", processDate);
                parameter[2] = new SqlParameter("@optCriteria", "EmpId");
                parameter[3] = new SqlParameter("@id", EmpId);
            }

            Helper.ExecProc("HR_prcSetAttData", parameter);
        }

        public void SaveAtt(HR_AttendanceProcess model)
        {
            string optSts = model.dayType;
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            HR_ProssType nPross = new HR_ProssType();
            nPross.ComId = comid;
            nPross.ProssDt = Convert.ToDateTime(Helper.GTRDate(model.dtProcess.ToString()));
            nPross.DaySts = optSts;
            nPross.DayStsB = optSts;
            nPross.IsLock = "0";
            _context.Add(nPross);
            _context.SaveChanges();
        }

        public void RemoveProssType(HR_AttendanceProcess model)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var prossType = _context.HR_ProssType.Where(p => p.ComId == comid && p.ProssDt == Convert.ToDateTime(Helper.GTRDate(model.dtProcess.ToString()))).FirstOrDefault();
            if (prossType != null)
            {
                _context.Remove(prossType);
            }
        }
    }
}
