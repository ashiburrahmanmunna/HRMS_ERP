#region Assembly refferance
using DocumentFormat.OpenXml.InkML;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.Payroll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace GTERP.Controllers.Payroll
{
    #region Controller
    [OverridableAuthorize]
    public class SalaryProcessesController : Controller
    {
        #region Feild and Poperties
        private readonly GTRDBContext db;

        public clsConnectionNew clsCon { get; set; }
        public clsProcedure clsProc { get; }

        public SalaryProcessesController(GTRDBContext context, clsConnectionNew _clsCon, clsProcedure _clsProc)
        {
            db = context;
            clsCon = _clsCon;
            clsProc = _clsProc;
        }
        #endregion

        // GET: SalaryProcesses

        #region Index
        public async Task<IActionResult> Create()
        {

            return View("Create");

        }

        #endregion

        #region Create get
        //[HttpGet]
        //[Route("Create")]
        public async Task<IActionResult> Index()
        {



            //if (HttpContext.Session.GetString("DisplayName") == null)
            //{
            //    return RedirectToRoute("GTR");
            //}
            SalaryProcess model = new SalaryProcess();
            //ViewBag.SalaryPross=  db.SalaryProcess.ToList();
            string comid = HttpContext.Session.GetString("comid");
            var data = db.ButtonPermissions.FirstOrDefault(x => x.ComId == comid);

            // for default 26/25 date 

            var salaryMonth = db.Cat_SalaryMonths.Where(x => x.ComId == comid && !x.IsDelete).FirstOrDefault();
            if (salaryMonth != null)
            {
                var month = DateTime.Now.AddMonths(-1);
                var day = salaryMonth.DtFrom;
                var year = DateTime.Now.Year;
                var fromdate = new DateTime(year, month.Month, day);
                ViewBag.Start = fromdate;

                var monthto = DateTime.Now.Month;
                var dayto = salaryMonth.DtTo;
                var todate = new DateTime(year, monthto, dayto);
                ViewBag.End = todate;

                ViewBag.dayfrom = day;
                ViewBag.dayto = dayto;
            }


            ViewBag.salarymonth = salaryMonth;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            ViewBag.ButtonPermission = data;

            ViewBag.FestType = new SelectList(db.Cat_Variable
                .Where(x => x.VarType == "FestivalType").OrderBy(x => x.SLNo).ToList(), "VarName", "VarName");
            ViewBag.SalType = new SelectList(db.Cat_Variable
                .Where(x => x.VarType == "SalaryType").OrderByDescending(x => x.SLNo).ToList(), "VarName", "VarName");

            ViewBag.EmpType = new SelectList(db.Cat_Emp_Type, "EmpTypeId", "EmpTypeName");
            ViewBag.Religion = new SelectList(db.Cat_Religion.Where(x => x.ComId == comid), "RelgionId", "ReligionName");
          
            //Employee Type
            //List<Cat_Emp_Type> EmpTypes = new List<Cat_Emp_Type>();
            //EmpTypes = db.Cat_Emp_Type.Where(x => !x.IsDelete).ToList();
            //ViewBag.EmpType = EmpTypes;

            return View(model);
        }

        #endregion

        #region Create Post

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Index(SalaryProcess salary)
        {

            //var JObject = new JObject();
            //var d = JObject.Parse(salProssModel);
            //string objct = d["salProssModel"].ToString();

            //var model = JsonConvert.DeserializeObject<SalaryProcess>(objct);
            var model = salary;
            var command = model.Command;
            try
            {
                if (ModelState.IsValid)
                {
                    if (command.ToUpper().ToString() == "ProssSalAct".ToUpper())
                    {
                        var values = prcProcessSalaryAct(model);
                        if (values.ToString().ToUpper().Contains("Added".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "ProssSalRel".ToUpper())
                    {
                        var values = prcProcessSalaryRel(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }

                    else if (command.ToUpper().ToString() == "ProssSalSettlement".ToUpper())
                    {
                        var values = prcProcessSalarySettlement(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }

                    else if (command.ToUpper().ToString() == "prossFest".ToUpper())
                    {
                        var values = prcProcessFestBonus(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "prossSalAdv".ToUpper())
                    {
                        var values = prcProcessFestAdv(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {

                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "dltprossFest".ToUpper())
                    {
                        var values = prcProcessFestBonusDelete(model);
                        if (values.ToString().ToUpper().Contains("Delete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "dltprossSalAdv".ToUpper())
                    {
                        var values = prcDeleteFestAdv(model);
                        if (values.ToString().ToUpper().Contains("Delete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "dltprossSalAdv".ToUpper())
                    {
                        var values = prcDeleteFestAdv(model);
                        if (values.ToString().ToUpper().Contains("Delete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "prossEarnLeave".ToUpper())
                    {
                        var values = prcProcessEarnLeave(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "PFIndividual".ToUpper())
                    {
                        var values = prcProcessPFIndividual(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    //18-Apr-2024
                    else if (command.ToUpper().ToString() == "HolidayAllowance".ToUpper())
                    {
                        var values = PrcProcessHolidayAllowance(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "CasualSalary".ToUpper())
                    {
                        var values = prcProcessCasualSalary(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else if (command.ToUpper().ToString() == "CasualSalaryDlt".ToUpper())
                    {
                        var values = prcProcessCasualSalaryDlt(model);
                        if (values.ToString().ToUpper().Contains("Complete".ToUpper()))
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 1, ex = TempData["Message"].ToString() });
                        }
                        else
                        {
                            TempData["Message"] = values;
                            return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                        }
                    }
                    else
                    {
                        TempData["Message"] = "Criteria Not Mach";
                        return Json(new { Success = 2, ex = TempData["Message"].ToString() });
                    }
                }
                else
                {
                    TempData["Message"] = "Model State Not Valid";
                    return Json(new { Success = 3, ex = TempData["Message"].ToString() });
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Json(new { Success = 3, ex = TempData["Message"].ToString() });
            }

        }
        #endregion

        #region Commented code

        //[HttpPost]
        //public ActionResult EarnLeavePrc(SalaryProcess earnLeavePrc)
        //{
        //    SalaryProcess salaryProcess=new SalaryProcess();
        //    int rowEffect=salaryProcess.PrcEarnLeave(earnLeavePrc);
        //    if (rowEffect>0)
        //    {
        //        ViewBag.loadMsg = "save";
        //    }
        //    else
        //    {
        //        ViewBag.msgErr = "error";
        //    }
        //    return Json(rowEffect, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region PrcLoadCombo

        public void prcLoadCombo()
        {
            var comid = HttpContext.Session.GetString("comid");
            DataSet dsList = new DataSet();
            try
            {
                string sqlQuery = "Exec prcGetSalPross '" + comid + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);

                // Load Combo
                List<clsCommon.clsCombo1> Sal = clsGenerateList.prcColumnOne(dsList.Tables[0]);
                ViewBag.SalType = Sal;
                List<clsCommon.clsCombo1> Fest = clsGenerateList.prcColumnOne(dsList.Tables[1]);
                ViewBag.FestType = Fest;
                ViewBag.firstDate = dsList.Tables[3].Rows[0]["firstDate"].ToString();
                ViewBag.lastDate = dsList.Tables[4].Rows[0]["lastDate"].ToString();
                ViewBag.SalDesc = dsList.Tables[5].Rows[0]["SalDesc"].ToString();
                List<clsCommon.clsCombo1> Rel = clsGenerateList.prcColumnOne(dsList.Tables[6]);
                List<clsCommon.clsCombo1> Type = clsGenerateList.prcColumnOne(dsList.Tables[7]);
                ViewBag.EmpType = Type;
                ViewBag.Religions = Rel;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
            }
        }
        #endregion

        #region PrcProcessSalaryAct
        public String prcProcessSalaryAct(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            ArrayList arQuery = new ArrayList();

            var queueid = 0;
            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "Active Salary Lock"
                && (x.DtDate >= Convert.ToDateTime(model.dtFirst) && x.DtDate <= Convert.ToDateTime(model.dtLast) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {

                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@FirstDate", Helper.GTRDate(model.dtFirst.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtLast.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", model.salDesc);
                    var query = $"Exec HR_prcProcessSalary '{comid}', '{Helper.GTRDate(model.dtFirst.ToString())}'," +
                        $" '{Helper.GTRDate(model.dtLast.ToString())}','{Helper.GTRDate(model.dtPayment.ToString())}','{model.salDesc}'";
                    var queue = new QueryProcessQueue
                    {
                        ComId = comid,
                        Query = query,
                        ExcuteById = userid,
                        Type = "sal",
                        RequestFrom = Convert.ToDateTime(model.dtFirst),
                        RequestTo = Convert.ToDateTime(model.dtLast)
                    };

                    db.QueryProcessQueues.AddAsync(queue).GetAwaiter().GetResult();
                    db.SaveChangesAsync().GetAwaiter().GetResult();
                    Helper.ExecCommand($"Exec ExecuteProcess '{queue.Id}'").GetAwaiter().GetResult();
                    // Helper.ExecCommand("Exec ExecuteProcessQueueSetBased");
                    //return "Process Complete";
                    return "Process Complete";
                }
            }
            catch (Exception ex)
            {
                var errorqueue = db.QueryProcessQueues.SingleOrDefaultAsync(x => x.Id == queueid).GetAwaiter().GetResult();
                errorqueue.ErrorLog = ex.Message;
                errorqueue.IsExecuted = 4;
                 db.SaveChangesAsync().GetAwaiter().GetResult();
                TempData["Message"] = "Process Fail";
                return "Process Fail";
            }



        }


        #endregion

        #region PrcProcessSalarySettlement
        public String prcProcessSalarySettlement(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "Settlement Salary Lock"
                && (x.DtDate >= Convert.ToDateTime(model.dtFirst) && x.DtDate <= Convert.ToDateTime(model.dtLast) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {

                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@FirstDate", Helper.GTRDate(model.dtFirst.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtLast.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", model.salDesc);
                    Helper.ExecProc("HR_ProcessSalarySettlement", parameter);
                    string query = $"Exec HR_ProcessSalarySettlement '{comid}', '{Helper.GTRDate(model.dtFirst.ToString())}','{Helper.GTRDate(model.dtLast.ToString())}', '{Helper.GTRDate(model.dtPayment.ToString())}','{model.salDesc}'";
                    return "Process Complete";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }



        }
        #endregion

        #region PrcProcessSalaryRel
        public String prcProcessSalaryRel(SalaryProcess model)
        {
            ArrayList arQuery = new ArrayList();
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }
                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("ComId") + ", 'Released Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                //ChkLock = Helper.GTRCountingDataLarge(sqlQuery1);


                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "Released Salary Lock"
                && (x.DtDate >= Convert.ToDateTime(model.dtFirst) && x.DtDate <= Convert.ToDateTime(model.dtLast) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@FirstDate", Helper.GTRDate(model.dtFirst.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtLast.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", model.salDesc);
                    Helper.ExecProc("HR_PrcProcessSalaryReleased", parameter);

                    return "Process Complete";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        #endregion

        #region prcProcessFestBonus

        public String prcProcessFestBonus(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock
                    .Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == model.FestType
                    && (x.DtDate >= Convert.ToDateTime(model.dtFest)
                    && x.DtDate <= Convert.ToDateTime(model.dtFest)
                    && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[9];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@JoinDate", Helper.GTRDate(model.dtFestEffect));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtFest));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment));
                    parameter[4] = new SqlParameter("@ProssType", (model.salDesc));
                    parameter[5] = new SqlParameter("@EmpType", (model.EmpType));
                    parameter[6] = new SqlParameter("@SalaryType", (model.SalType));
                    parameter[7] = new SqlParameter("@Festivaltype", (model.FestType));
                    parameter[8] = new SqlParameter("@Percen", (model.bonusPer));
                    string query = $"Exec HR_prcProcessFestivalBonus '{comid}', '{model.dtFestEffect}', " +
                        $"'{model.dtFest}','{model.dtPayment}','{model.salDesc}','{model.EmpType}','{model.SalType}','{model.FestType}','{model.bonusPer}'";

                    Helper.ExecProc("HR_prcProcessFestivalBonus", parameter);
                    return "Process Complete";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        #endregion

        #region prcProcessFestBonusDelete

        public String prcProcessFestBonusDelete(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == model.FestType && (x.DtDate >= Convert.ToDateTime(model.dtFest) && x.DtDate <= Convert.ToDateTime(model.dtFest) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[8];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@JoinDate", Helper.GTRDate(model.dtFestEffect.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtFest.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", (model.salDesc.ToString()));
                    parameter[5] = new SqlParameter("@SalaryType", (model.SalType.ToString()));
                    parameter[6] = new SqlParameter("@Festivaltype", (model.FestType.ToString()));
                    parameter[7] = new SqlParameter("@Percen", (model.bonusPer.ToString()));

                    string query = $"Exec HR_prcProcessFestivalBonusDelete '{comid}', '{model.dtFestEffect}', " +
                        $"'{model.dtFest}','{model.dtPayment}','{model.salDesc}','{model.SalType}','{model.FestType}','{model.bonusPer}'";
                    Helper.ExecProc("HR_prcProcessFestivalBonusDelete", parameter);
                    return "Process Delete Complete";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        #endregion

        #region prcProcessEarnLeave
        public string prcProcessEarnLeave(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "Earn Leave Lock" && (x.DtDate >= Convert.ToDateTime(model.dtELPrcFirst) && x.DtDate <= Convert.ToDateTime(model.dtELPrcLast) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@FirstDate", Helper.GTRDate(model.dtELPrcFirst.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtELPrcLast.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", model.salDesc);
                    Helper.ExecProc("HR_prcProcessEarnLeave", parameter);
                    return "Process Complete";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        #endregion

        #region prcPFIndividual

        public string prcProcessPFIndividual(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "PFProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "PF Lock" && (x.DtDate >= Convert.ToDateTime(model.PFIndDtFrom) && x.DtDate <= Convert.ToDateTime(model.PFIndDtTo) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[4];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@dtFirst", Helper.GTRDate(model.PFIndDtFrom.ToString()));
                    parameter[2] = new SqlParameter("@dtLast", Helper.GTRDate(model.PFIndDtTo.ToString()));
                    parameter[3] = new SqlParameter("@Rate", model.PFIndPercentage);
                    var query = $"Exec HR_prcProcessPF '{comid}', '{Helper.GTRDate(model.PFIndDtFrom.ToString())}'," +
                        $" '{Helper.GTRDate(model.PFIndDtTo.ToString())}','{model.PFIndPercentage}'";
                    Helper.ExecProc("HR_prcProcessPF", parameter);

                    return "Process Complete";

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        #endregion
        //18-apr-2024
        #region HolidayAllowance

        public string PrcProcessHolidayAllowance(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            //string userid = HttpContext.Session.GetString("userid");
            ArrayList arQuery = new ArrayList();

            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "HolidayAllowance";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "Holiday Allowance Lock"
                && (x.DtDate >= Convert.ToDateTime(model.dtFirst) && x.DtDate <= Convert.ToDateTime(model.dtLast) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@FirstDate", Helper.GTRDate(model.dtFirst.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtLast.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", model.salDesc);
                    var query = $"Exec HR_PrcProcessHolidayAllowance  '{comid}', '{Helper.GTRDate(model.dtFirst.ToString())}'," +
                        $" '{Helper.GTRDate(model.dtLast.ToString())}','{Helper.GTRDate(model.dtPayment.ToString())}','{model.salDesc}'";
                    Helper.ExecProc("HR_PrcProcessHolidayAllowance", parameter);

                    return "Process Complete";

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        #endregion

        #region prcProcessCasualSalary

        public string prcProcessCasualSalary(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "Casual Salary Lock" && (x.DtDate >= Convert.ToDateTime(model.CasualDtFrom) && x.DtDate <= Convert.ToDateTime(model.CasualDtTo) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    //SqlParameter[] parameter = new SqlParameter[4];
                    //parameter[0] = new SqlParameter("@ComId", comid);
                    //parameter[1] = new SqlParameter("@dtFrom", Helper.GTRDate(model.PFIndDtFrom));
                    //parameter[2] = new SqlParameter("@dtTo", Helper.GTRDate(model.PFIndDtTo.ToString()));

                    //parameter[3] = new SqlParameter("@Percentage", (model.PFIndPercentage));
                    //Helper.ExecProc("HR_ProcessPFIndividual", parameter);
                    //return "Process Complete";

                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@FirstDate", Helper.GTRDate(model.CasualDtFrom.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.CasualDtTo.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", model.salDesc);
                    var query = $"Exec HR_PrcProcessSalaryCasual '{comid}', '{Helper.GTRDate(model.CasualDtFrom.ToString())}'," +
                        $" '{Helper.GTRDate(model.CasualDtTo.ToString())}','{Helper.GTRDate(model.dtPayment.ToString())}','{model.salDesc}'";
                    Helper.ExecProc("HR_PrcProcessSalaryCasual", parameter);

                    return "Process Complete";

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        #endregion

        #region prcProcessCasualSalary

        public string prcProcessCasualSalaryDlt(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == "Casual Salary Lock" && (x.DtDate >= Convert.ToDateTime(model.PFIndDtFrom) && x.DtDate <= Convert.ToDateTime(model.PFIndDtTo) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    //SqlParameter[] parameter = new SqlParameter[4];
                    //parameter[0] = new SqlParameter("@ComId", comid);
                    //parameter[1] = new SqlParameter("@dtFrom", Helper.GTRDate(model.PFIndDtFrom));
                    //parameter[2] = new SqlParameter("@dtTo", Helper.GTRDate(model.PFIndDtTo.ToString()));

                    //parameter[3] = new SqlParameter("@Percentage", (model.PFIndPercentage));
                    //Helper.ExecProc("HR_ProcessPFIndividual", parameter);
                    //return "Process Complete";

                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@FirstDate", Helper.GTRDate(model.CasualDtFrom.ToString()));
                    parameter[2] = new SqlParameter("@LastDate", Helper.GTRDate(model.CasualDtTo.ToString()));
                    parameter[3] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment.ToString()));
                    parameter[4] = new SqlParameter("@ProssType", model.salDesc);
                    var query = $"Exec HR_PrcProcessSalaryCasual '{comid}', '{Helper.GTRDate(model.CasualDtFrom.ToString())}'," +
                        $" '{Helper.GTRDate(model.CasualDtTo.ToString())}','{Helper.GTRDate(model.dtPayment.ToString())}','{model.salDesc}'";
                    Helper.ExecProc("HR_PrcProcessSalaryCasualDelete", parameter);

                    return "Process Complete";

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        #endregion

        #region prcProcessFestAdv
        public String prcProcessFestAdv(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock
                    .Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == model.FestType
                    && (x.DtDate >= Convert.ToDateTime(model.dtFest)
                    && x.DtDate <= Convert.ToDateTime(model.dtFest)
                    && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[7];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtSalAdv));
                    parameter[2] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment));
                    parameter[3] = new SqlParameter("@Description", Helper.GTRDate(model.salDesc));
                    parameter[4] = new SqlParameter("@Festivaltype", (model.AdvFestType));
                    parameter[5] = new SqlParameter("@SalaryType", (model.AdvType));
                    parameter[6] = new SqlParameter("@Percen", (model.bonusAdvPer));

                    string query = $"Exec HR_prcProcessFestivalAdv '{comid}', '{Helper.GTRDate(model.dtSalAdv)}', '{Helper.GTRDate(model.dtPayment)}','{Helper.GTRDate(model.salDesc)}','{model.AdvType}','{model.AdvFestType}','{model.bonusAdvPer}'";

                    Helper.ExecProc("HR_prcProcessFestivalAdv", parameter);
                    return "Process Complete";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region prcDeleteFestAdv
        public String prcDeleteFestAdv(SalaryProcess model)
        {
            var comid = HttpContext.Session.GetString("comid");
            ArrayList arQuery = new ArrayList();


            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }

                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                List<HR_ProcessLock> hrprocesslock = db.HR_ProcessLock.Where(x => x.ComId == comid && x.IsDelete == false && x.LockType == model.FestType && (x.DtDate >= Convert.ToDateTime(model.dtFest) && x.DtDate <= Convert.ToDateTime(model.dtFest) && x.IsLock == true)).ToList();
                //sqlQuery1 = "Select dbo.fncProcessLock (" + HttpContext.Session.GetString("comid") + ", 'Active Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = hrprocesslock.Count();// clsCon.GTRCountingDataLarge(sqlQuery1);

                if (ChkLock >= 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[7];
                    parameter[0] = new SqlParameter("@ComId", comid);
                    parameter[1] = new SqlParameter("@LastDate", Helper.GTRDate(model.dtSalAdv));
                    parameter[2] = new SqlParameter("@PaymentDate", Helper.GTRDate(model.dtPayment));
                    parameter[3] = new SqlParameter("@Description", Helper.GTRDate(model.salDesc));
                    parameter[4] = new SqlParameter("@Festivaltype", (model.AdvFestType));
                    parameter[5] = new SqlParameter("@SalaryType", (model.AdvType));
                    parameter[6] = new SqlParameter("@Percen", (model.bonusAdvPer));

                    string query = $"Exec HR_prcProcessFestivalAdvDelete '{comid}', '{Helper.GTRDate(model.dtSalAdv)}', '{Helper.GTRDate(model.dtPayment)}','{Helper.GTRDate(model.salDesc)}','{model.AdvType}','{model.AdvFestType}','{model.bonusAdvPer}'";

                    Helper.ExecProc("HR_prcProcessFestivalAdvDelete", parameter);
                    return "Process Delete Complete";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        #endregion

        #region prcDeleteFestBonus
        public String prcDeleteFestBonus(SalaryProcess model)
        {
            ArrayList arQuery = new ArrayList();
            try
            {
                var path = HttpContext.Request.GetEncodedUrl().ToString();
                if (path == null || path.Length == 0)
                {
                    path = "SalaryProcess";
                }
                string sqlQuery1 = "", SelDescription = "";
                Int64 ChkLock = 0;

                sqlQuery1 = "Select dbo.HR_fncProcessLock ('" + HttpContext.Session.GetString("comid") + "','Released Salary Lock','" + clsProc.GTRDate(model.dtFirst) + "')";
                ChkLock = clsCon.GTRCountingDataLarge(sqlQuery1);
                if (ChkLock == 1)
                {
                    return "Process Lock. Please communicate to Administrator";
                }
                else
                {
                    string sqlQuery = " Delete from HR_FestBonusAll where ProssType='" + model.salDesc + "' and ComId='" + HttpContext.Session.GetString("comid") + "'";


                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into UserTransactionLogs (LUserId, formName, tranStatement, tranType,PCName)"
                                + " Values (" + HttpContext.Session.GetString("UserId") + ", '"
                                + path + "','" + sqlQuery.Replace("'", "|") + "','FestBonusPross',"
                                + " '"
                                + HttpContext.Session.GetString("PCName") + "')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    return "Process Delete Successfully";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }


            #endregion
        }

    }
    #endregion
}