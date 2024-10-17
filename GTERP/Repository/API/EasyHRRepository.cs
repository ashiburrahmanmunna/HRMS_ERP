using GTERP.Interfaces.API;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using QuickMailer;
using GTERP.Controllers.Device.Model;

namespace GTERP.Repository.API
{
    public class EasyHRRepository : IEasyHRRepository
    {
        private readonly GTRDBContext _context;
        public EasyHRRepository(GTRDBContext context)
        {
            _context = context;
        }

        #region GetAppInfos

        public AppInfoRespone GetAppInfos(string email, int softwareId, int versionId)
        {
            AppInfoRespone appInfoRespone = new AppInfoRespone();

            var employeeInfo = _context.HR_Emp_Info.Include(a => a.Cat_Designation).Where(a => a.EmpEmail == email && a.IsDelete == false).FirstOrDefault();

            if (employeeInfo is not null)
            {
                if (softwareId is not 0 && versionId is not 0)
                {
                    var menus = (from vmm in _context.VersionMenuPermission_Masters
                                 join vmd in _context.VersionMenuPermission_Details
                                 on vmm.MenuPermissionId equals vmd.MenuPermissionId
                                 join mm in _context.ModuleMenus
                                 on vmd.ModuleMenuId equals mm.ModuleMenuId
                                 where vmm.SoftwareId == softwareId && vmm.VersionId == versionId
                                 select
                                 new
                                 {
                                     MenuId = mm.ModuleMenuId,
                                     MenuName = mm.ModuleMenuName
                                 }).ToList();

                    if (menus.Count > 0)
                    {
                        List<MenuList> menuLists = new List<MenuList>();
                        foreach (var item in menus)
                        {
                            var menu = new MenuList();
                            menu.MenuId = item.MenuId;
                            menu.MenuName = item.MenuName;
                            menuLists.Add(menu);
                        }
                        appInfoRespone.MenuList = menuLists ?? new List<MenuList>();
                    }

                    appInfoRespone.EmployeeId = employeeInfo.EmpId;
                    appInfoRespone.SectionId = employeeInfo.SectId;
                    appInfoRespone.FullName = employeeInfo.EmpName ?? "";
                    appInfoRespone.Designation = employeeInfo.Cat_Designation?.DesigName ?? "";
                    appInfoRespone.FingerId = employeeInfo.FingerId ?? "";

                    var employeeImage = _context.HR_Emp_Image.Where(a => a.EmpId == employeeInfo.EmpId).Select(a => a.EmpImage).SingleOrDefault();

                    if (employeeImage is not null)
                    {
                        appInfoRespone.ProfilePicture = Convert.ToBase64String(employeeImage) ?? "";
                    }

                    var companyImage = _context.Companys.Where(a => a.CompanyCode == employeeInfo.ComId).Select(a => a.ComLogo).SingleOrDefault();
                    if (companyImage is not null)
                    {
                        appInfoRespone.CompanyImage = Convert.ToBase64String(companyImage) ?? "";
                    }

                }
                return appInfoRespone;
            }
            return appInfoRespone;
        }

        #endregion

        #region SetAttendance

        public async Task<bool> SetAttendance(HR_RawData_App attendanceData)
        {
            try
            {
                if (attendanceData is not null)
                {
                    if (attendanceData.EmpId == null || attendanceData.EmpId < 1)
                    {
                        return false;
                    }
                    attendanceData.dtPunchDate = DateTime.Now;
                    attendanceData.dtPunchTime = DateTime.Now;
                    await _context.HR_RawData_Apps.AddAsync(attendanceData);
                    int response = await _context.SaveChangesAsync();
                    if (response > 0) return true;
                    else return false;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region GetJobCard

        public JobCardResponse GetJobCard(string comId, int employeeId, int sectionId, string fromDate, string toDate)
        {
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[5];
                sqlParameter[0] = new SqlParameter("@ComId", comId);
                sqlParameter[1] = new SqlParameter("@dtFrom", fromDate);
                sqlParameter[2] = new SqlParameter("@dtTo", toDate);
                sqlParameter[3] = new SqlParameter("@SectId", sectionId);
                sqlParameter[4] = new SqlParameter("@EmpId", employeeId);
                List<JobCardList> jobCardList = Helper.ExecProcMapTList<JobCardList>("rptJobCard", sqlParameter).ToList();

                List<AttendencList> attendenceList = new List<AttendencList>();
                JobCardResponse jobCardResponse = new JobCardResponse();

                if (jobCardList is not null)
                {
                    foreach (var item in jobCardList)
                    {
                        AttendencList attendence = new AttendencList
                        {
                            PunchDate = item.dtPunchDate,
                            Late = item.Late,
                            Status = item.Status,
                            TimeIn = item.TimeIn,
                            TimeOut = item.TimeOut,
                            OTHour = item.OTHour
                        };
                        attendenceList.Add(attendence);
                    }

                    var jobCardObj = jobCardList.Select(x => new
                    {
                        Present = x.Present,
                        Absent = x.Absent,
                        HoliDay = x.HDay,
                        LateDay = x.LateDay,
                        OTHrsTtl = x.OTHrsTtl,
                        Weekend = x.WDay,
                        Leave = x.Leave
                    }).FirstOrDefault();


                    if (jobCardObj is not null)
                    {
                        jobCardResponse.Absent = jobCardObj.Absent;
                        jobCardResponse.Present = jobCardObj.Present;
                        jobCardResponse.HoliDay = jobCardObj.HoliDay;
                        jobCardResponse.LateDay = jobCardObj.LateDay;
                        jobCardResponse.OTHourTotal = jobCardObj.OTHrsTtl;
                        jobCardResponse.Weekend = jobCardObj.Weekend;
                        jobCardResponse.Leave = jobCardObj.Leave;
                    }
                }
                jobCardResponse.AttendanceList = attendenceList ?? new List<AttendencList>();

                return jobCardResponse;
            }
            catch (Exception ex)
            {
                throw (ex.InnerException);
            }
        }

        #endregion

        #region GetProcessType

        public List<ProcessType> GetProcessType(string comId)
        {
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[1];
                sqlParameter[0] = new SqlParameter("@ComId", comId);
                List<ProcessType> processTypeList = Helper.ExecProcMapTList<ProcessType>("GetProssType", sqlParameter).ToList();
                return processTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetPaySlip

        public PaySlip GetPaySlip(int employeeId, string processType)
        {
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@EmployeeId", employeeId);
                sqlParameter[1] = new SqlParameter("@ProcessType", processType);
                List<GetPaySlip> paySlipList = Helper.ExecProcMapTList<GetPaySlip>("GetPaySlip", sqlParameter).ToList();

                PaySlip paySlip = new PaySlip();

                if (paySlipList is not null)
                {
                    foreach (var item in paySlipList)
                    {
                        paySlip.GrossSalay = item.GS;
                        paySlip.BasicSalary = item.BS;
                        paySlip.HouseRent = item.HR;
                        paySlip.MedicalAllowance = item.MA;
                        paySlip.AttendanceBonus = item.AttBonus;
                        paySlip.Conveyance = item.ConvAllow;
                        paySlip.FoodAllowance = item.FoodAllow;
                        paySlip.ProvidentFund = item.PF;
                        paySlip.Stamp = item.Stamp;
                        paySlip.AbsentDeduction = item.ADV;
                        paySlip.OthersDeduct = item.OthersDeduct;
                        paySlip.TotalDeduct = item.TotalDeduct;
                        paySlip.NetSalary = item.NetSalary;
                        paySlip.NetSalaryPayable = item.NetSalaryPayable;
                        paySlip.TotalPayable = item.TotalPayable;
                    }
                }

                return paySlip;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetLeaveInfo

        public LeaveInfoResponse GetLeaveInfo(int employeeId)
        {
            try
            {
                LeaveInfoResponse leaveInfoResponse = new LeaveInfoResponse();
                var employeeLeaveBalance = _context.HR_Leave_Balance.Where(a => a.EmpId == employeeId).FirstOrDefault();
                if (employeeLeaveBalance is not null)
                {
                    leaveInfoResponse.TotalCL = employeeLeaveBalance.CL;
                    leaveInfoResponse.EnjoyedCL = (float)employeeLeaveBalance.ACL;
                    leaveInfoResponse.TotalSL = employeeLeaveBalance.SL;
                    leaveInfoResponse.EnjoyedSL = (float)employeeLeaveBalance.ASL;
                    leaveInfoResponse.TotalEL = employeeLeaveBalance.EL;
                    leaveInfoResponse.EnjoyedEL = (float)employeeLeaveBalance.AEL;
                }
                List<LeaveType> leaveTypeList = new List<LeaveType>();
                var leaveTypeData = _context.Cat_Leave_Type.Select(x => new
                {
                    LeaveTypeId = x.LTypeId,
                    LeaveTypeName = x.LTypeNameShort
                }).ToList();

                foreach (var item in leaveTypeData)
                {
                    LeaveType leaveType = new LeaveType
                    {
                        LeaveTypeId = item.LeaveTypeId,
                        LeaveTypeName = item.LeaveTypeName
                    };
                    leaveTypeList.Add(leaveType);
                }

                leaveInfoResponse.LeaveTypesList = leaveTypeList ?? new List<LeaveType>();

                return leaveInfoResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ApplyLeave

        public async Task<bool> ApplyLeave(HR_Leave_Avail leaveApplication)
        {
            try
            {
                if (leaveApplication is not null)
                {
                    float availableLeaveCount = 0;
                    var employeeLeaveBalance = _context.HR_Leave_Balance.Where(a => a.EmpId == leaveApplication.EmpId && a.DtOpeningBalance == DateTime.Now.Year).FirstOrDefault();

                    if (employeeLeaveBalance is not null)
                    {
                        if (leaveApplication.LvType == "CL" || leaveApplication.LvType == "CLH")
                        {
                            availableLeaveCount = (float)(employeeLeaveBalance.CL - employeeLeaveBalance.ACL);
                        }
                        else if (leaveApplication.LvType == "EL" || leaveApplication.LvType == "ELH")
                        {
                            availableLeaveCount = (float)(employeeLeaveBalance.EL - employeeLeaveBalance.AEL);
                        }
                        else if (leaveApplication.LvType == "SL" || leaveApplication.LvType == "SLH")
                        {
                            availableLeaveCount = (float)(employeeLeaveBalance.SL - employeeLeaveBalance.ASL);
                        }
                        else if (leaveApplication.LvType == "ML")
                        {
                            availableLeaveCount = (float)(employeeLeaveBalance.ML - employeeLeaveBalance.AML);
                        }
                        else
                        {
                            availableLeaveCount = 365;
                        }

                        if (leaveApplication.TotalDay <= availableLeaveCount)
                        {
                            leaveApplication.DateAdded = DateTime.Now.Date;
                            leaveApplication.DtLvInput = DateTime.Now.Date;
                            leaveApplication.DtTo = leaveApplication.DtFrom.AddDays(Convert.ToInt32(leaveApplication.TotalDay - 1));
                            leaveApplication.DtInput = DateTime.Now.Date;
                            var leaveid = await _context.HR_Leave_Avail.AddAsync(leaveApplication);
                            int response = await _context.SaveChangesAsync();

                            var temp = _context.HR_Emp_Info.Where(x => x.EmpId == leaveApplication.EmpId).FirstOrDefault();
                            SendEmailForLeave(temp.EmpEmail, true, false, false, false, leaveApplication, leaveApplication.ComId);
                            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == leaveApplication.ComId && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
                            if (approveData.IsFirstLeaveApprove == true)
                            {
                                var firstData = _context.HR_Emp_Info.Where(x => x.EmpId == temp.FirstAprvId).FirstOrDefault();

                                if (firstData != null)
                                {
                                    var emailtoHod = firstData.EmpEmail;
                                    SendEmailForLeave(emailtoHod, false, true, false, false, leaveApplication, leaveApplication.ComId);
                                }

                            }
                            else
                            {
                                var firstData = _context.HR_Emp_Info.Where(x => x.EmpId == temp.FinalAprvId).FirstOrDefault();

                                if (firstData != null)
                                {
                                    var emailtoHod = firstData.EmpEmail;
                                    SendEmailForLeave(emailtoHod, false, true, false, false, leaveApplication, leaveApplication.ComId);
                                }

                            }

                            //SendEmailForLeave(string emailTo, bool IsApplicant, bool HOD, bool IsFirstApprvd, bool IsRejected, HR_Leave_Avail model, string comid)

                            if (response > 0) return true;
                            else return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region GetPendingLeaveApplication OLD

        public List<LeaveApplications> GetPendingLeaveApplications(string comId)
        {
            try
            {
                var pendingLeaveApplication = _context.HR_Leave_Avail.Where(a => a.ComId == comId && a.Status == 0 && a.IsApprove == false).ToList();
                List<LeaveApplications> leaveApplications = new List<LeaveApplications>();
                if (pendingLeaveApplication is not null)
                {
                    foreach (var item in pendingLeaveApplication)
                    {
                        LeaveApplications leaveApplication = new();

                        var employee = _context.HR_Emp_Info?.Where(a => a.EmpId == item.EmpId)?.SingleOrDefault();

                        leaveApplication.LeaveId = item.LvId;
                        leaveApplication.EmployeeName = employee.EmpName ?? "";
                        leaveApplication.EmployeeId = employee.EmpCode ?? "";
                        leaveApplication.DateFrom = item.DtFrom.ToShortDateString() ?? "";
                        leaveApplication.DateTo = item.DtTo.ToShortDateString() ?? "";
                        leaveApplication.LeaveInputDate = item.DtLvInput.ToShortDateString() ?? "";
                        leaveApplication.LeaveType = _context.Cat_Leave_Type?.Where(a => a.LTypeId == item.LTypeId)?.Select(a => a.LTypeNameShort)?.SingleOrDefault() ?? "";
                        leaveApplication.Remarks = item.Remark ?? "";
                        leaveApplication.TotalDay = item.TotalDay?.ToString() ?? "";

                        leaveApplications.Add(leaveApplication);
                    }
                }
                return leaveApplications;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetPendingLeaveApplications

        public List<LeaveApplications> GetPendingLeaveApplications(string comId, int UserId)
        {
            var pendingLeaveApplication = _context.HR_Leave_Avail.Where(a => a.ComId == comId && a.HR_Emp_Info.FirstAprvId == UserId && a.Status == 0 && a.IsApprove == false).OrderByDescending(x => x.DtFrom)
             .Select(x => new LeaveApplications
             {
                 LeaveId = x.LvId,
                 EmployeeName = x.HR_Emp_Info.EmpName,
                 EmployeeId = x.HR_Emp_Info.EmpCode,
                 DateFrom = x.DtFrom.ToFormatslasMMddyyyy(),
                 LeaveInputDate = x.DtLvInput.ToFormatslasMMddyyyy(),
                 DateTo = x.DtTo.ToFormatslasMMddyyyy(),
                 LeaveType = x.Cat_Leave_Type.LTypeNameShort,
                 Remarks = x.Remark,
                 TotalDay = x.TotalDay.ToString(),
             })
             .ToList();

            return pendingLeaveApplication;

        }

        public void SendEmailForLeave(string emailTo, bool IsApplicant, bool HOD, bool IsFirstApprvd, bool IsRejected, HR_Leave_Avail model, string comid)
        {
            var link = "<br>Please follow the link to proceed.<a href=\"https://gtrbd.net/ERP/PostDocument/Index?DocType=Leave&criteria=Pending\"> Click here to approve or disapprove</a>";
            //var comid = HttpContext.Session.GetString("comid");
            string subject = "", body = "Dear ", senderAddress = "", host = "", userName = "", password = "", title = "";
            int port = 0;
            try
            {
                var leave = _context.Cat_Leave_Type.Where(x => x.LTypeId == model.LTypeId).Select(y => y.LTypeName).FirstOrDefault();
                var leaveName = model.LvType + "[ " + leave + " ]";

                string[] substrings1 = model.DtLvInput.ToString().Split(' ');
                var dtinput = substrings1[0];

                string[] substrings2 = model.DtTo.ToString().Split(' ');
                var dtTo = substrings2[0];

                string[] substrings3 = model.DtFrom.ToString().Split(' ');
                var dtFrom = substrings3[0];

                string[] substrings4 = model.dtWork.ToString().Split(' ');
                var dtWork = substrings4[0];
                if (IsApplicant == true && IsFirstApprvd == false && IsRejected == false)
                {
                    var empName = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var mailData = _context.Cat_MailSettings.Where(x => x.ComId == comid && x.IsApplicant == true && x.IsFirstApprvd == false && x.IsRejected == false).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>       :" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtTo + " To " + dtFrom + "<br><b>Reason</b>      :" + model.Remark;
                    title = mailData.CompanyTitle;
                    //SendEmailForHR(empName, dtinput, leaveName, model.TotalDay, dtTo, dtFrom, model.Remark);
                }

                if (IsApplicant == true && IsFirstApprvd == true && IsRejected == false)
                {
                    var empName = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var mailData = _context.Cat_MailSettings.Where(x => x.ComId == comid && x.IsApplicant == true && x.IsFirstApprvd == true && x.IsRejected == false).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>       :" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtTo + " To " + dtFrom + "<br><b>Reason</b>      :" + model.Remark;
                    title = mailData.CompanyTitle;
                    //SendEmailForHR(empName, dtinput, leaveName, model.TotalDay, dtTo, dtFrom, model.Remark);
                }

                if (HOD == true && IsRejected == false)
                {
                    var empName = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var empCode = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpCode).FirstOrDefault();

                    var mailData = _context.Cat_MailSettings.Where(x => x.ComId == comid && x.IsHOD == true && x.IsRejected == false).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    if (model.LTypeId == 9)
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Working Date</b>      :" + dtWork + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    else
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    body += link;
                    title = mailData.CompanyTitle;
                }
                if (HOD == true && IsRejected == true)
                {
                    var empName = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var empCode = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpCode).FirstOrDefault();

                    var mailData = _context.Cat_MailSettings.Where(x => x.ComId == comid && x.IsHOD == true && x.IsRejected == true).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    if (model.LTypeId == 9)
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Working Date</b>      :" + dtWork + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    else
                    {
                        body = body + "Sir/Madam," + mailData.MailBody + dtinput + "<br><b>Applicant's Name<b>       :" + empCode + "_" + empName + "<br><b>Leave Type</b>       :" +
                            leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    title = mailData.CompanyTitle;
                }
                if (IsApplicant == false && IsFirstApprvd == false && IsRejected == true)
                {
                    var empName = _context.HR_Emp_Info.Where(x => x.EmpId == model.EmpId).Select(y => y.EmpName).FirstOrDefault();
                    var mailData = _context.Cat_MailSettings.Where(x => x.ComId == comid && x.IsApplicant == false && x.IsFirstApprvd == false && x.IsRejected == true).FirstOrDefault();
                    subject = mailData.MailSubject;
                    senderAddress = mailData.SenderAddress;
                    host = mailData.Host;
                    port = mailData.Port;
                    userName = mailData.Username;
                    password = mailData.Password;
                    if (model.LTypeId == 9)
                    {
                        body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>       :" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Working Date</b>      :" + dtWork + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    else
                    {
                        body = body + empName + mailData.MailBody + dtinput + "<br><b>Leave Type</b>       :" + leaveName + "<br><b>Leave Day(s)</b>      :" + model.TotalDay.ToString()
                           + "<br><b>Leave Date</b>        :" + dtFrom + " To " + dtTo + "<br><b>Reason</b>      :" + model.Remark;
                    }
                    title = mailData.CompanyTitle;
                }
                //emailTo = "sr.sourav@gmail.com";
                var message = new MailMessage();
                message.From = new MailAddress(senderAddress, title);

                message.To.Add(new MailAddress(emailTo));


                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true; //true;

                using (var client = new SmtpClient())
                {
                    client.Host = host; //"smtp.gmail.com";
                    client.Port = port;//587;
                    client.EnableSsl = true;// true;
                                            //client.Credentials = new NetworkCredential(config.GetSection("CredentialMail").Value, config.GetSection("CredentialPassword").Value);
                    client.Credentials = new NetworkCredential(userName, password);
                    client.Send(message);
                }


                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region LeaveApproval

        public async Task<bool> LeaveApproval(int leaveId, string approvalType)
        {
            try
            {
                var leaveApplication = await _context.HR_Leave_Avail.FindAsync(leaveId);
                if (leaveApplication is not null)
                {
                    if (approvalType.ToLower() == "Approve".ToLower())
                    {
                        var employeeLeaveBalance = _context.HR_Leave_Balance.Where(a => a.EmpId == leaveApplication.EmpId && a.DtOpeningBalance == DateTime.Now.Year).FirstOrDefault();

                        if (employeeLeaveBalance is not null)
                        {

                            leaveApplication.Status = 1;
                            _context.HR_Leave_Avail.Update(leaveApplication);
                            //_context.HR_Leave_Balance.Update(employeeLeaveBalance);
                            int response = await _context.SaveChangesAsync();
                            if (response > 0)
                            {
                                var temp = _context.HR_Emp_Info.Where(x => x.EmpId == leaveApplication.EmpId).FirstOrDefault();

                                SendEmailForLeave(temp.EmpEmail, true, false, true, false, leaveApplication, leaveApplication.ComId);
                                return true;
                            }
                            else
                            {
                                return false;
                            }


                        }
                    }
                    else
                    {
                        leaveApplication.Status = 2;
                        _context.HR_Leave_Avail.Update(leaveApplication);
                        int response = await _context.SaveChangesAsync();
                        if (response > 0)
                        {

                            var temp = _context.HR_Emp_Info.Where(x => x.EmpId == leaveApplication.EmpId).FirstOrDefault();

                            SendEmailForLeave(temp.EmpEmail, true, false, false, false, leaveApplication, leaveApplication.ComId);
                            return true;
                        }
                        else
                        {
                            return false;
                        };
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetFinalLeaveApplications
        //Old Code GetFinalLeaveApplications
        public List<LeaveApplications> GetFinalLeaveApplications(string comId)
        {
            try
            {
                var FinalLeaveApplication = _context.HR_Leave_Avail.Where(a => a.ComId == comId && a.Status == 1 && a.IsApprove == false).ToList();//
                List<LeaveApplications> leaveApplications = new List<LeaveApplications>();
                if (FinalLeaveApplication is not null)
                {
                    foreach (var item in FinalLeaveApplication)
                    {
                        LeaveApplications leaveApplication = new();

                        var employee = _context.HR_Emp_Info?.Where(a => a.EmpId == item.EmpId)?.SingleOrDefault();

                        leaveApplication.LeaveId = item.LvId;
                        leaveApplication.EmployeeName = employee.EmpName ?? "";
                        leaveApplication.EmployeeId = employee.EmpCode ?? "";
                        leaveApplication.DateFrom = item.DtFrom.ToShortDateString() ?? "";
                        leaveApplication.DateTo = item.DtTo.ToShortDateString() ?? "";
                        leaveApplication.LeaveInputDate = item.DtLvInput.ToShortDateString() ?? "";
                        leaveApplication.LeaveType = _context.Cat_Leave_Type?.Where(a => a.LTypeId == item.LTypeId)?.Select(a => a.LTypeNameShort)?.SingleOrDefault() ?? "";
                        leaveApplication.Remarks = item.Remark ?? "";
                        leaveApplication.TotalDay = item.TotalDay?.ToString() ?? "";

                        leaveApplications.Add(leaveApplication);
                    }
                }
                return leaveApplications;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LeaveApplications> GetFinalLeaveApplications(string comId, int userId)
        {


            try
            {

                var pendingLeaveApplication = _context.HR_Leave_Avail.Where(a => a.ComId == comId && a.HR_Emp_Info.FinalAprvId == userId && a.Status == 1 && a.IsApprove == false).OrderByDescending(x => x.DtFrom)
                    .Select(x => new LeaveApplications
                    {
                        LeaveId = x.LvId,
                        EmployeeName = x.HR_Emp_Info.EmpName,
                        EmployeeId = x.HR_Emp_Info.EmpCode,
                        DateFrom = x.DtFrom.ToFormatslasMMddyyyy(),
                        LeaveInputDate = x.DtLvInput.ToFormatslasMMddyyyy(),
                        DateTo = x.DtTo.ToFormatslasMMddyyyy(),
                        LeaveType = x.Cat_Leave_Type.LTypeNameShort,
                        Remarks = x.Remark,
                        TotalDay = x.TotalDay.ToString(),
                    })
                    .ToList();
                return pendingLeaveApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region FinalLeaveApproval
        public async Task<bool> FinalLeaveApproval(int leaveId, string approvalType)
        {
            try
            {
                var leaveApplication = await _context.HR_Leave_Avail.FindAsync(leaveId);
                if (leaveApplication is not null)
                {
                    if (approvalType.ToLower() == "Approve".ToLower())
                    {
                        var employeeLeaveBalance = _context.HR_Leave_Balance.Where(a => a.EmpId == leaveApplication.EmpId && a.DtOpeningBalance == DateTime.Now.Year).FirstOrDefault();

                        if (employeeLeaveBalance is not null)
                        {
                            if (leaveApplication.LvType == "CL" || leaveApplication.LvType == "CLH")
                            {
                                employeeLeaveBalance.ACL = employeeLeaveBalance.ACL + leaveApplication.TotalDay;
                            }
                            else if (leaveApplication.LvType == "SL" || leaveApplication.LvType == "SLH")
                            {
                                employeeLeaveBalance.ASL = employeeLeaveBalance.ASL + leaveApplication.TotalDay;
                            }
                            else if (leaveApplication.LvType == "EL" || leaveApplication.LvType == "ELH")
                            {
                                employeeLeaveBalance.AEL = employeeLeaveBalance.AEL + leaveApplication.TotalDay;
                            }
                            else if (leaveApplication.LvType == "ML")
                            {
                                employeeLeaveBalance.AML = employeeLeaveBalance.AML + leaveApplication.TotalDay;
                            }
                            leaveApplication.Status = 1;
                            leaveApplication.IsApprove = true;
                            _context.HR_Leave_Avail.Update(leaveApplication);
                            _context.HR_Leave_Balance.Update(employeeLeaveBalance);
                            int response = await _context.SaveChangesAsync();
                            if (response > 0)
                            {

                                var applicantemail = _context.HR_Emp_Info.Where(x => x.EmpId == leaveApplication.EmpId).Select(x => x.EmpEmail).FirstOrDefault();
                                SendEmailForLeave(applicantemail, true, false, true, false, leaveApplication, leaveApplication.ComId);
                                return true;


                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        leaveApplication.IsApprove = false;
                        leaveApplication.Status = 2;
                        _context.HR_Leave_Avail.Update(leaveApplication);
                        int response = await _context.SaveChangesAsync();
                        if (response > 0)
                        {
                            var firstaprvId = _context.HR_Emp_Info.Where(x => x.EmpId == leaveApplication.EmpId).Select(x => x.FirstAprvId).FirstOrDefault();
                            if (firstaprvId is not null)
                            {
                                var firstapvemail = _context.HR_Emp_Info.Where(x => x.EmpId == firstaprvId).Select(x => x.EmpEmail).FirstOrDefault();
                                SendEmailForLeave(firstapvemail, true, true, false, true, leaveApplication, leaveApplication.ComId);

                            }
                            var Applicant = _context.HR_Emp_Info.Where(x => x.EmpId == leaveApplication.EmpId).Select(x => x.EmpEmail).FirstOrDefault();
                            SendEmailForLeave(Applicant, false, false, false, true, leaveApplication, leaveApplication.ComId);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion



        #region GetAttendenceHistories

        public List<AttendenceHistory> GetAttendenceHistories(int employeeId, string date)
        {
            try
            {
                var punchList = _context?.HR_RawData_Apps?.Where(a => a.EmpId == employeeId && a.dtPunchDate.Value.Date.ToString() == date)?.OrderByDescending(a => a.dtPunchTime)?.ToList();
                List<AttendenceHistory> attendenceHistories = new List<AttendenceHistory>();
                if (punchList is not null)
                {
                    foreach (var item in punchList)
                    {
                        AttendenceHistory attendenceHistory = new AttendenceHistory
                        {
                            PunchDate = item.dtPunchDate.Value.ToShortDateString(),
                            PunchTime = item.dtPunchTime.Value.ToShortTimeString(),
                            LocationName = item.LocationName
                        };
                        attendenceHistories.Add(attendenceHistory);
                    }
                }
                return attendenceHistories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LeaveApplications> GetEmployeeLeaveApplications(int employeeId)
        {
            try
            {
                var pendingLeaveApplication = _context.HR_Leave_Avail.Where(a => a.EmpId == employeeId).OrderByDescending(x => x.DtFrom).ToList();
                List<LeaveApplications> leaveApplications = new List<LeaveApplications>();
                if (pendingLeaveApplication is not null)
                {
                    foreach (var item in pendingLeaveApplication)
                    {
                        LeaveApplications leaveApplication = new();

                        var employee = _context.HR_Emp_Info?.Where(a => a.EmpId == item.EmpId)?.SingleOrDefault();

                        leaveApplication.LeaveId = item.LvId;
                        leaveApplication.EmployeeName = employee.EmpName ?? "";
                        leaveApplication.EmployeeId = employee.EmpCode ?? "";
                        leaveApplication.DateFrom = item.DtFrom.ToFormatslasMMddyyyy() ?? "";
                        leaveApplication.DateTo = item.DtTo.ToFormatslasMMddyyyy() ?? "";
                        leaveApplication.LeaveInputDate = item.DtLvInput.ToFormatslasMMddyyyy() ?? "";
                        leaveApplication.LeaveType = _context.Cat_Leave_Type?.Where(a => a.LTypeId == item.LTypeId)?.Select(a => a.LTypeNameShort)?.SingleOrDefault() ?? "";
                        leaveApplication.Remarks = item.Remark ?? "";
                        leaveApplication.Status = item.Status;
                        leaveApplication.IsApprove = item.IsApprove;
                        leaveApplication.TotalDay = item.TotalDay?.ToString() ?? "";

                        leaveApplications.Add(leaveApplication);
                    }
                }
                return leaveApplications;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string GetAttendanceTypeByEmpId(int empId)
        {
            return _context.HR_Emp_Info.Where(x => x.EmpId == empId).Select(x => x.MobileAttendence).FirstOrDefault();
        }
        #endregion
    }
}
