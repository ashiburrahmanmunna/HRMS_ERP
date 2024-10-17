using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using GTERP.Interfaces.API;
using GTERP.Models;
using GTERP.ViewModels;
using ZXing;
using ZXing.QrCode;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.ReportingServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using QuickMailer;
using GTERP.Models.Email;
using Microsoft.Extensions.Options;



namespace GTERP.Controllers.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class EasyHRController : ControllerBase
    {
        private readonly IOptions<SMTPConfigModel> _smtpConfig;
        private readonly IEasyHRRepository _easyHRRepository;
        private readonly IConfiguration _configuraiton;
        private readonly GTRDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EasyHRController(IEasyHRRepository easyHRRepository, IConfiguration configuration, GTRDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _easyHRRepository = easyHRRepository;
            _configuraiton = configuration;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        #region GetAppInfos

        [HttpGet]
        [Route("GetAppInfos/{email}/{softwareId}/{versionId}")]
        public object GetAppMenus(string email, int softwareId, int versionId)
        {
            try
            {
                if (softwareId is not 0 && versionId is not 0)
                {
                    var appInfo = _easyHRRepository.GetAppInfos(email, softwareId, versionId);
                    return Ok(appInfo);
                }
                return Ok(Array.Empty<string>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region SetAttendance

        [HttpPost]
        [Route("SetAttendance")]
        public async Task<bool> SetAttendance(HR_RawData_App attendanceData)
        {
            try
            {
                bool response = await _easyHRRepository.SetAttendance(attendanceData);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetJobCard

        [HttpGet]
        [Route("GetJobCard/{comId}/{employeeId}/{sectionId}/{fromDate}/{toDate}")]
        public object GetJobCard(string comId, int employeeId, int sectionId, string fromDate, string toDate)
        {
            try
            {
                AppData.AppData.dbdaperpconstring = _configuraiton.GetConnectionString("DefaultConnection");
                var jobCardList = _easyHRRepository.GetJobCard(comId, employeeId, sectionId, fromDate, toDate);
                return Ok(jobCardList);
            }
            catch (Exception ex)
            {
                throw (ex.InnerException);
            }
        }

        #endregion

        #region GetProcessType

        [HttpGet]
        [Route("GetProcessType/{comId}")]
        public object GetProcessType(string comId)
        {
            try
            {
                AppData.AppData.dbdaperpconstring = _configuraiton.GetConnectionString("DefaultConnection");
                var processTypeList = _easyHRRepository.GetProcessType(comId);
                return Ok(processTypeList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetPaySlip

        [HttpGet]
        [Route("GetPaySlip/{employeeId}/{processType}")]
        public object GetPaySlip(int employeeId, string processType)
        {
            try
            {
                AppData.AppData.dbdaperpconstring = _configuraiton.GetConnectionString("DefaultConnection");
                var processTypeList = _easyHRRepository.GetPaySlip(employeeId, processType);
                return Ok(processTypeList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetLeaveInfo

        [HttpGet]
        [Route("GetLeaveInfo/{employeeId}")]
        public object GetLeaveInfo(int employeeId)
        {
            try
            {
                var leaveInfoResponse = _easyHRRepository.GetLeaveInfo(employeeId);
                return Ok(leaveInfoResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ApplyLeave

        [HttpPost]
        [Route("ApplyLeave")]
        public async Task<bool> ApplyLeave(HR_Leave_Avail leaveApplication)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    bool response = await _easyHRRepository.ApplyLeave(leaveApplication);
                    return response;

                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #region GetPendingLeaveApplications Old

        [HttpGet]
        [Route("GetPendingLeaveApplications/{comId}")]
        public List<LeaveApplications> GetPendingLeaveApplications(string comId)
        {
            try
            {
                var pendingLeaveApplication = _easyHRRepository.GetPendingLeaveApplications(comId);
                return pendingLeaveApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #endregion

        #region GetPendingLeaveApplications

        [HttpGet]
        [Route("GetPendingLeaveApplications/{comId}/{UserId}")]
        public List<LeaveApplications> GetPendingLeaveApplications(string comId, int userId)
        {
            try
            {
                var pendingLeaveApplication = _easyHRRepository.GetPendingLeaveApplications(comId, userId);
                return pendingLeaveApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region GetFinalLeaveApplications

        //Old Code GetFinalLeaveApplications

        [HttpGet]
        [Route("GetFinalLeaveApplications/{comId}")]
        public List<LeaveApplications> GetFinalLeaveApplications(string comId)
        {
            try
            {
                var finalLeaveApplication = _easyHRRepository.GetFinalLeaveApplications(comId);
                return finalLeaveApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFinalLeaveApplications/{comId}/{userId}")]
        public List<LeaveApplications> GetFinalLeaveApplications(string comId, int userId)
        {
            try
            {
                var finalLeaveApplication = _easyHRRepository.GetFinalLeaveApplications(comId, userId);
                return finalLeaveApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetEmployeeLeaveApplications

        [HttpGet]
        [Route("GetEmployeeLeaveApplications/{employeeId}")]
        public List<LeaveApplications> GetEmployeeLeaveApplications(int employeeId)
        {
            try
            {
                var leaveApplication = _easyHRRepository.GetEmployeeLeaveApplications(employeeId);
                return leaveApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LeaveApproval

        [HttpPost]
        [Route("LeaveApproval/{leaveId}/{approvalType}")]
        public async Task<bool> LeaveApproval(int leaveId, string approvalType)
        {
            try
            {
                bool response = await _easyHRRepository.LeaveApproval(leaveId, approvalType);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region FinalLeaveApproval

        [HttpPost]
        [Route("FinalLeaveApproval/{leaveId}/{approvalType}")]
        public async Task<bool> FinalLeaveApproval(int leaveId, string approvalType)
        {
            try
            {
                bool response = await _easyHRRepository.FinalLeaveApproval(leaveId, approvalType);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetAttendenceHistories

        [HttpGet]
        [Route("GetAttendenceHistory/{employeeId}/{date}")]
        public object GetAttendenceHistories(int employeeId, string date)
        {
            try
            {
                var attendenceHistory = _easyHRRepository.GetAttendenceHistories(employeeId, date);
                return attendenceHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region QrCode generator

        [HttpPost]
        [Route("CompanyQr/{comid}/{lon}/{lat}")]
        public IActionResult CompanyQr(string comid, string lon, string lat)
        {



            var Company = _context.Companys.Where(r => r.CompanyCode == comid).Select(x => new { x.ComLogo, x.CompanyName }).FirstOrDefault();

            var CompanyName = Company.CompanyName;

            string base64logo = Convert.ToBase64String(Company.ComLogo);
            string QRImg = "";

            var QRCodeText = comid + "," + lat + "," + lon;

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 200,
                    Height = 200
                }
            };
            var result = writer.Write(QRCodeText);
            var bitmap = new Bitmap(result);
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            var base64String = Convert.ToBase64String(stream.ToArray());


            string path = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode/" + comid + ".png");
            bitmap.Save(filePath, ImageFormat.Png);
            //byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            //string fileName = Path.GetFileName(filePath);
            //string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
            //ViewBag.QrCodeUri = imageUrl;

            QRImg = base64String;



            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }




            return Ok(new { CompanyName = CompanyName, Companylogo = base64logo, QRImg = QRImg });

        }
        #endregion

        #region SendNotification
        [HttpPost("SendPushNotification")]
        public async Task<IActionResult> SendPushNotification([FromForm] string tkn)
        {
            var path = _webHostEnvironment.ContentRootPath;
            path = path + "\\auth.json";
            FirebaseApp app = null;
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "myApp");
            }
            catch (Exception ex)
            {
                app = FirebaseApp.GetInstance("myApp");
            }

            var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);

            var token = tkn;
            Message message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = "My push notification title",
                    Body = "Content for this push notification"
                },
                Data = new Dictionary<string, string>()
                 {
                     { "AdditionalData1", "This is Additional data" },
                     { "AdditionalData2", "data 2" },
                     //{ "image", "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80"},
                 },
                Token = token
            };
            var result = await fcm.SendAsync(message);

            return Ok(result);
        }
        #endregion

        #region Store Notification
        [HttpPost("StoreToken")]
        public IActionResult StoreToken([FromForm] int empId, [FromForm] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("EmpId and Token parameters are required");
            }
            string conn = _configuraiton.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE HR_Emp_Info SET Token = @Token WHERE EmpId = @EmpId", connection);

                command.Parameters.AddWithValue("@EmpId", empId);
                command.Parameters.AddWithValue("@Token", token);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok("Token updated successfully");
                }
                else
                {
                    return NotFound("No record found for the specified EmpId");
                }
            }
        }

        #endregion

        #region Get NotificationList
        [HttpPost("GetNotificationList")]
        public async Task<IActionResult> GetNotificationList([FromForm] string comid, [FromForm] int empid)
        {

            var data = _context.HR_Notification.Where(x => x.ComId == comid).Select(p => new { p.NtfId, p.Title, p.Body, p.execTime }).ToList();
            return Ok(data);
        }

        #endregion
        [HttpGet]
        [Route("GetAttendanceType/{EmpId}")]
        public IActionResult GetAttendanceType(int EmpId)
        {
            try
            {
                var response = _easyHRRepository.GetAttendanceTypeByEmpId(EmpId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
