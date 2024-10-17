using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class FixAttRepository : IFixAttRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        DateTime Date = DateTime.Now.Date;
        public FixAttRepository(IHttpContextAccessor httpContext,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContext = httpContext;
            _context = context;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        public List<AttFixGrid> FixAttendanceList(string DtFrom, string DtTo, string criteria, string value)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var quary = $"EXEC HR_PrcGetFixAtt '1','{comid}','{criteria}','{value}','{DtFrom}','{DtTo}'";

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@Id", "1");
            sqlParameter[1] = new SqlParameter("@ComId", comid);
            sqlParameter[2] = new SqlParameter("@OptCriteria", criteria);
            sqlParameter[3] = new SqlParameter("@Value", value);
            sqlParameter[4] = new SqlParameter("@dtfrom", DtFrom);
            sqlParameter[5] = new SqlParameter("@dtTo", DtTo);

            var listOfAttFixed = Helper.ExecProcMapTList<AttFixGrid>("HR_PrcGetFixAtt", sqlParameter);
            return listOfAttFixed;
        }


        public List<AttFixGrid> FixAttendanceListB(string DtFrom, string DtTo, string criteria, string value)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var quary = $"EXEC HR_PrcGetFixAttB '1','{comid}','{criteria}','{value}','{DtFrom}','{DtTo}'";

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@Id", "1");
            sqlParameter[1] = new SqlParameter("@ComId", comid);
            sqlParameter[2] = new SqlParameter("@OptCriteria", criteria);
            sqlParameter[3] = new SqlParameter("@Value", value);
            sqlParameter[4] = new SqlParameter("@dtfrom", DtFrom);
            sqlParameter[5] = new SqlParameter("@dtTo", DtTo);

            var listOfAttFixed = Helper.ExecProcMapTList<AttFixGrid>("HR_PrcGetFixAttB", sqlParameter);
            return listOfAttFixed;
        }

        public List<AttFixGridUBL> FixAttendanceListUBL(string DtFrom, string DtTo, string criteria, string value)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var quary = $"EXEC HR_PrcGetFixAtt_ubl '1','{comid}','{criteria}','{value}','{DtFrom}','{DtTo}'";

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@Id", "1");
            sqlParameter[1] = new SqlParameter("@ComId", comid);
            sqlParameter[2] = new SqlParameter("@OptCriteria", criteria);
            sqlParameter[3] = new SqlParameter("@Value", value);
            sqlParameter[4] = new SqlParameter("@dtfrom", DtFrom);
            sqlParameter[5] = new SqlParameter("@dtTo", DtTo);

            var listOfAttFixed = Helper.ExecProcMapTList<AttFixGridUBL>("HR_PrcGetFixAtt_ubl", sqlParameter);
            return listOfAttFixed;
        }



        public void UpdateSelectedData(string GridDataList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var pcName = _httpContext.HttpContext.Session.GetString("pcname");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            if (GridDataList != null)
            {
                var JObject = new JObject();
                var d = JObject.Parse(GridDataList);
                string objct = d["GridDataList"].ToString();
                var model = JsonConvert.DeserializeObject<List<AttFixGrid>>(objct);

                using (var tr = _context.Database.BeginTransaction())
                {
                    foreach (var aGridData in model)
                    {

                        try
                        {
                            #region update HR_AttFixed working

                            var f = _context.HR_AttFixed.Where(x => x.EmpId == aGridData.EmpId).Where(y => y.DtPunchDate == Convert.ToDateTime(aGridData.dtPunchDate)).ToList();
                            //var stId = db.Cat_AttStatus.Where(x => x.AttStatus == aGridData.Status).Select(x => x.StatusId).FirstOrDefault();
                            var EmpTypeId = _context.HR_Emp_Info.Where(x => x.EmpId == aGridData.EmpId).Select(x => x.EmpTypeId).FirstOrDefault();
                            if (f.Count() > 0)
                            {
                                _context.HR_AttFixed.RemoveRange(f);
                                _context.SaveChanges();
                            }



                            var fixAtt = new HR_AttFixed();
                            var processData = new HR_ProcessedData();

                            fixAtt.ComId = comid;
                            fixAtt.EmpId = aGridData.EmpId;
                            fixAtt.DtPunchDate = Convert.ToDateTime(aGridData.dtPunchDate);
                            fixAtt.TimeIn = TimeSpan.Parse(aGridData.TimeIn);
                            fixAtt.TimeOut = TimeSpan.Parse(aGridData.TimeOut);
                            fixAtt.StatusId = Convert.ToInt16(aGridData.StatusId);// stId;
                            fixAtt.Status = aGridData.Status;// stId;

                            fixAtt.ShiftId = Convert.ToInt16(aGridData.ShiftId);
                            //fixAtt.ShiftName = aGridData.ShiftName;
                            if (aGridData.Remarks == "") { fixAtt.Remarks = "Manual Entry"; } else { fixAtt.Remarks = aGridData.Remarks; }

                            //fixAtt.Remarks = aGridData.Remarks;
                            fixAtt.PcName = pcName;
                            fixAtt.TimeInPrev = TimeSpan.Parse(aGridData.TimeIn);
                            fixAtt.TimeOutPrev = TimeSpan.Parse(aGridData.TimeOut);
                            fixAtt.DtTran = DateTime.Now;
                            fixAtt.IsInactive = true;
                            fixAtt.OT = float.Parse(aGridData.OtHour);
                            fixAtt.OTHour = float.Parse(aGridData.OtHour);
                            fixAtt.OTHourInTime = TimeSpan.Parse(aGridData.OTHourInTime);

                            fixAtt.OTHourPrev = 0;
                            fixAtt.StatusPrev = aGridData.Status;

                            // for isapprove field
                            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1178 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
                            if (approveData == null)
                            {
                                fixAtt.IsApprove = true;
                            }
                            else if (approveData.IsApprove == true)
                            {
                                fixAtt.IsApprove = false;
                            }
                            else
                            {
                                fixAtt.IsApprove = true;
                            }
                            _context.HR_AttFixed.Add(fixAtt);
                            _context.SaveChanges();
                            #endregion

                            #region Method 1 to update ProcessData
                            var prosData = _context.HR_ProcessedData.Where(x => x.EmpId == aGridData.EmpId && x.DtPunchDate.Date == aGridData.dtPunchDate.Date && x.ComId == comid).FirstOrDefault();
                            if (prosData != null)
                            {
                                prosData.ShiftId = aGridData.ShiftId;
                                prosData.TimeIn = TimeSpan.Parse(aGridData.TimeIn);
                                prosData.TimeOut = TimeSpan.Parse(aGridData.TimeOut);
                                if (EmpTypeId == 1) { prosData.OTHour = float.Parse(aGridData.OtHour); } else { prosData.OTHour = 0; }
                                if (EmpTypeId == 1) { prosData.OT = float.Parse(aGridData.OtHour); } else { prosData.OT = 0; }
                                if ((EmpTypeId == 1) && (float.Parse(aGridData.OtHour) <= 2) && (aGridData.Status == "P" || aGridData.Status == "L"))
                                    { prosData.ROT = float.Parse(aGridData.OtHour); }
                                else if ((EmpTypeId == 1) && (float.Parse(aGridData.OtHour) > 2) && (aGridData.Status == "P" || aGridData.Status == "L"))
                                    { prosData.ROT = 2; }
                                else if ((EmpTypeId == 1) && float.Parse(aGridData.OtHour) > 0 && (aGridData.Status == "W" || aGridData.Status == "H"))
                                    { prosData.ROT = 0; }
                                if ((EmpTypeId == 1) && (float.Parse(aGridData.OtHour) <= 2) && (aGridData.Status == "P" || aGridData.Status == "L"))
                                    { prosData.EOT = 0; }
                                else if ((EmpTypeId == 1) && float.Parse(aGridData.OtHour) > 2 && (aGridData.Status =="P" || aGridData.Status == "L")) 
                                    { prosData.EOT = float.Parse(aGridData.OtHour) - 2; }
                                else if ((EmpTypeId == 1) && float.Parse(aGridData.OtHour) > 0 && (aGridData.Status == "W" || aGridData.Status == "H"))
                                    { prosData.EOT = float.Parse(aGridData.OtHour); }
                                prosData.Status = aGridData.Status;
                                if (aGridData.Remarks == "") { prosData.Remarks = "Manual Entry"; } else { prosData.Remarks = aGridData.Remarks; }
                                //prosData.Remarks = aGridData.Remarks;

                                _context.Entry(prosData).State = EntityState.Modified;
                                _context.SaveChanges();
                            }
                            #endregion
                        }
                        catch (SqlException ex)
                        {

                            Console.WriteLine(ex.Message);
                            tr.Rollback();

                        }
                    }
                    tr.Commit();
                }
            }
        }


        public void UpdateSelectedDataB(string GridDataList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var pcName = _httpContext.HttpContext.Session.GetString("pcname");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            if (GridDataList != null)
            {
                var JObject = new JObject();
                var d = JObject.Parse(GridDataList);
                string objct = d["GridDataList"].ToString();
                var model = JsonConvert.DeserializeObject<List<AttFixGrid>>(objct);

                using (var tr = _context.Database.BeginTransaction())
                {
                    foreach (var aGridData in model)
                    {

                        try
                        {
                            #region update HR_AttFixed working

                            var f = _context.HR_AttFixedB.Where(x => x.EmpId == aGridData.EmpId).Where(y => y.DtPunchDate == Convert.ToDateTime(aGridData.dtPunchDate)).ToList();
                            //var stId = db.Cat_AttStatus.Where(x => x.AttStatus == aGridData.Status).Select(x => x.StatusId).FirstOrDefault();
                            var EmpTypeId = _context.HR_Emp_Info.Where(x => x.EmpId == aGridData.EmpId).Select(x => x.EmpTypeId).FirstOrDefault();
                            if (f.Count() > 0)
                            {
                                _context.HR_AttFixedB.RemoveRange(f);
                                _context.SaveChanges();
                            }



                            var fixAtt = new HR_AttFixedB();
                            var processData = new HR_ProcessedDataB();

                            fixAtt.ComId = comid;
                            fixAtt.EmpId = aGridData.EmpId;
                            fixAtt.DtPunchDate = Convert.ToDateTime(aGridData.dtPunchDate);
                            fixAtt.TimeIn = TimeSpan.Parse(aGridData.TimeIn);
                            fixAtt.TimeOut = TimeSpan.Parse(aGridData.TimeOut);
                            fixAtt.StatusId = Convert.ToInt16(aGridData.StatusId);// stId;
                            fixAtt.Status = aGridData.Status;// stId;

                            fixAtt.ShiftId = Convert.ToInt16(aGridData.ShiftId);
                            //fixAtt.ShiftName = aGridData.ShiftName;
                            if (aGridData.Remarks == "") { fixAtt.Remarks = "Manual Entry"; } else { fixAtt.Remarks = aGridData.Remarks; }

                            //fixAtt.Remarks = aGridData.Remarks;
                            fixAtt.PcName = pcName;
                            fixAtt.TimeInPrev = TimeSpan.Parse(aGridData.TimeIn);
                            fixAtt.TimeOutPrev = TimeSpan.Parse(aGridData.TimeOut);
                            fixAtt.DtTran = DateTime.Now;
                            fixAtt.IsInactive = true;
                            fixAtt.OT = float.Parse(aGridData.OtHour);
                            fixAtt.OTHour = float.Parse(aGridData.OtHour);
                            fixAtt.OTHourInTime = TimeSpan.Parse(aGridData.OTHourInTime);

                            fixAtt.OTHourPrev = 0;
                            fixAtt.StatusPrev = aGridData.Status;

                            // for isapprove field
                            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1178 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
                            if (approveData == null)
                            {
                                fixAtt.IsApprove = true;
                            }
                            else if (approveData.IsApprove == true)
                            {
                                fixAtt.IsApprove = false;
                            }
                            else
                            {
                                fixAtt.IsApprove = true;
                            }
                            _context.HR_AttFixedB.Add(fixAtt);
                            _context.SaveChanges();
                            #endregion

                            #region Method 1 to update ProcessDataB
                            //var prosData = _context.HR_ProcessedDataB.Where(x => x.EmpId == aGridData.EmpId && x.DtPunchDate.Date == aGridData.dtPunchDate.Date && x.ComId == comid).FirstOrDefault();
                            //if (prosData != null)
                            //{
                            //    prosData.ShiftId = aGridData.ShiftId;
                            //    prosData.TimeIn = TimeSpan.Parse(aGridData.TimeIn);
                            //    prosData.TimeOut = TimeSpan.Parse(aGridData.TimeOut);
                            //    if (EmpTypeId == 1) { prosData.OTHour = float.Parse(aGridData.OtHour); } else { prosData.OTHour = 0; }
                            //    if (EmpTypeId == 1) { prosData.OT = float.Parse(aGridData.OtHour); } else { prosData.OT = 0; }
                            //    if ((EmpTypeId == 1) && (float.Parse(aGridData.OtHour) <= 2) && (aGridData.Status == "P" || aGridData.Status == "L"))
                            //    { prosData.ROT = float.Parse(aGridData.OtHour); }
                            //    else if ((EmpTypeId == 1) && (float.Parse(aGridData.OtHour) > 2) && (aGridData.Status == "P" || aGridData.Status == "L"))
                            //    { prosData.ROT = 2; }
                            //    else if ((EmpTypeId == 1) && float.Parse(aGridData.OtHour) > 0 && (aGridData.Status == "W" || aGridData.Status == "H"))
                            //    { prosData.ROT = 0; }
                            //    if ((EmpTypeId == 1) && (float.Parse(aGridData.OtHour) <= 2) && (aGridData.Status == "P" || aGridData.Status == "L"))
                            //    { prosData.EOT = 0; }
                            //    else if ((EmpTypeId == 1) && float.Parse(aGridData.OtHour) > 2 && (aGridData.Status == "P" || aGridData.Status == "L"))
                            //    { prosData.EOT = float.Parse(aGridData.OtHour) - 2; }
                            //    else if ((EmpTypeId == 1) && float.Parse(aGridData.OtHour) > 0 && (aGridData.Status == "W" || aGridData.Status == "H"))
                            //    { prosData.EOT = float.Parse(aGridData.OtHour); }
                            //    prosData.Status = aGridData.Status;
                            //    if (aGridData.Remarks == "") { prosData.Remarks = "Manual Entry"; } else { prosData.Remarks = aGridData.Remarks; }
                            //    //prosData.Remarks = aGridData.Remarks;

                            //    _context.Entry(prosData).State = EntityState.Modified;
                            //    _context.SaveChanges();
                            //}
                            #endregion
                        }
                        catch (SqlException ex)
                        {

                            Console.WriteLine(ex.Message);
                            tr.Rollback();

                        }
                    }
                    tr.Commit();
                }
            }
        }
        public void SaveToFixedAtt(HrAttFixed hrAttFixed)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var pcName = _httpContext.HttpContext.Session.GetString("pcname");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            
        }
    }
}
