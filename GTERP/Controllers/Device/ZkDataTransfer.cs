using GTERP.Controllers.Device.Model;
using GTERP.Models;
using HVFaceManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StandaloneSDKDemo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace GTERP.Controllers.Device
{
    public class ZkDataTransfer : Controller
    {
        private readonly IConfiguration config;
        private readonly  GTRDBContext db;
        private IWebHostEnvironment HostEnvironment;
     
        private string DeviceName;
        private readonly IHttpContextAccessor _httpContext;

        public ZkDataTransfer(IConfiguration config, GTRDBContext _db, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContext)
        {
            this.config = config;
            db = _db;
            _httpContext = httpContext;
            HostEnvironment = hostEnvironment;
           
            DeviceName = config.GetValue<string>("DeviceName");
            
        }
             
        SDKHelper SDK = new SDKHelper();

        private void getDeviceInfo()
        { 
            string sFirmver = "";
            string sMac = "";
            string sPlatform = "";
            string sSN = "";
            string sProductTime = "";
            string sDeviceName = "";
            int iFPAlg = 0;
            int iFaceAlg = 0;
            string sProducter = "";

            SDK.sta_GetDeviceInfo(out sFirmver, out sMac, out sPlatform, out sSN, out sProductTime, out sDeviceName, out iFPAlg, out iFaceAlg, out sProducter);

        }

        private void getCapacityInfo()
        {
            int adminCnt = 0;
            int userCount = 0;
            int fpCnt = 0;
            int recordCnt = 0;
            int pwdCnt = 0;
            int oplogCnt = 0;
            int faceCnt = 0;
            SDK.sta_GetCapacityInfo(out adminCnt, out userCount, out fpCnt, out recordCnt, out pwdCnt, out oplogCnt, out faceCnt);

        }
        
        public IActionResult WeekdaySet()
        {
            return View();
        }

        [Route("DataTranfer")]
        public IActionResult Index()
        {
            return View();
        }


        #region GetUesrInfo

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = Int32.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public IActionResult GetAllUserInfo(List<HR_MachineNoZKT> Device)
        {
            List<viewModel> vm = new List<viewModel>();
            foreach (var d in Device)
            {


                int ret = SDK.sta_ConnectTCP(d.IpAddress, d.PortNo, d.ZKtPassword);

                if (ret == 1)
                {
                    var data = SDK.GetAllUserFPInfo();
                    vm.AddRange(data);
                }
                else
                {
                    return Json("DisConnected");
                }
            }
            return Json(vm);
        }
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = Int32.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public IActionResult SaveAllUserInfo(List<HR_MachineNoZKT> Device)
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            List<HR_Emp_DeviceInfoHIK> EmpInfoList = new List<HR_Emp_DeviceInfoHIK>();
            List<string> Ermsg = new List<string>();
            List<string> SucIp = new List<string>();
            foreach (var dev in Device)
            {

                int ret = SDK.sta_ConnectTCP(dev.IpAddress, dev.PortNo, dev.ZKtPassword);

                if (ret == 1)
                {
                    var data = SDK.GetAllUserFPInfo();
                    foreach (var dd in data)
                    {
                        HR_Emp_DeviceInfoHIK ff = new HR_Emp_DeviceInfoHIK();
                        ff.cardNo = dd.EmpCode;
                        ff.comId = ComId;
                        ff.empName = dd.EmpName;
                        ff.IpAddress = dev.IpAddress;
                        ff.DeviceName = DeviceName;
                        ff.userGroup = dd.userGroup;

                        if (dd.finger_Data != null)
                        { ff.fingerData = Convert.FromBase64String(dd.finger_Data); }
                        if (dd.emp_image != null)
                        {
                            ff.empImage = Convert.FromBase64String(dd.emp_image);
                        };

                        var dbdata =(from d in db.HR_Emp_DeviceInfoHIK where d.cardNo == dd.EmpCode && d.comId == ComId && d.DeviceName == DeviceName select d ).FirstOrDefault();

                        if (dbdata == null)
                        {
                            EmpInfoList.Add(ff);

                        }
                    }
                    if (EmpInfoList.Count > 0)
                    {
                        db.HR_Emp_DeviceInfoHIK.AddRange(EmpInfoList);
                        db.SaveChanges();
                    }
                    else { return Json("data already exist in database"); }

                }
                else
                {
                    var er = "Device Ip " + dev.IpAddress + "is failed to connect";
                    Ermsg.Add(er);
                }



            }
            if (Ermsg.Count > 0)
            {
                return Json(Ermsg + "and Rest data are saved");
            }
            else { return Json("Data are Saved Successfully"); }


        }

        [HttpGet]
        public IActionResult Empinfo()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            List<viewModel> viewModels = new List<viewModel>();
            var dbconfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
            try
            {
                var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                string sql = $"exec prcGetDevicesDataTranHIK'{ComId}'";
                using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 0;
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            viewModel vm = new viewModel();
                            vm.EmpCode = dataReader["EmpCode"].ToString();
                            vm.CardNo = dataReader["CardNo"].ToString();
                            vm.EmpName = dataReader["EmpName"].ToString();
                            vm.DeptName = dataReader["DeptName"].ToString();
                            vm.SectName = dataReader["SectName"].ToString();
                            vm.desigName = dataReader["DesigName"].ToString();
                            vm.floor = dataReader["Floor"].ToString();
                            vm.line = dataReader["Line"].ToString();
                            vm.IpAddress = dataReader["IPAddress"].ToString();
                            vm.emp_image = dataReader["empImage"].ToString();
                            vm.finger_Data = dataReader["fingerData"].ToString();
                            viewModels.Add(vm);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(viewModels);
        }

        [HttpGet]
        public IActionResult EmpWeekday()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            List<viewModel> viewModels = new List<viewModel>();
            var dbconfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
            try
            {
                var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                string sql = $"exec prcGetWeekday'{ComId}', 0";
                using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 0;
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            viewModel vm = new viewModel();
                            vm.EmpCode = dataReader["EmpCode"].ToString();
                            vm.CardNo = dataReader["CardNo"].ToString();
                            vm.EmpName = dataReader["EmpName"].ToString();
                            vm.DeptName = dataReader["DeptName"].ToString();
                            vm.SectName = dataReader["SectName"].ToString();
                            vm.desigName = dataReader["DesigName"].ToString();
                            vm.floor = dataReader["Floor"].ToString();
                            vm.line = dataReader["Line"].ToString();
                            vm.IpAddress = dataReader["IPAddress"].ToString();
                            vm.emp_image = dataReader["empImage"].ToString();
                            vm.finger_Data = dataReader["fingerData"].ToString();
                            vm.userGroup = (int)dataReader["userGroup"];
                            viewModels.Add(vm);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(viewModels);
        }

        #endregion

        #region SetUserInfo
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = Int32.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public IActionResult SetFingerPrint(List<empData> SelectedData, List<HR_MachineNoZKT> Device)
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");

            List<viewModel> data = new List<viewModel>();
            int result = 0;
            // old code. 

            foreach (var a in SelectedData)
            {
                viewModel vm = new viewModel();
                vm.CardNo = a.cardNo;
                vm.EmpName = a.empName;
                vm.userGroup = a.userGroup;

                if (a.cardNo != null)
                {
                    var dbdata = (from d in db.HR_Emp_DeviceInfoHIK where d.cardNo == a.cardNo && d.comId==ComId select d).FirstOrDefault();

                    if (dbdata != null)
                    {
                        vm.emp_image = dbdata.empImage == null ? "" : Convert.ToBase64String(dbdata.empImage);
                        vm.finger_Data = dbdata.fingerData == null ? "" : Convert.ToBase64String(dbdata.fingerData);
                    }
                }

                data.Add(vm);
            }
            foreach (var dev in Device)
            {

                int ret = SDK.sta_ConnectTCP(dev.IpAddress, dev.PortNo, dev.ZKtPassword);

                if (ret == 1)
                {
                    result = SDK.sta_SetAllUserFPInfo(data);

                }
                else
                {
                    return Json("Connection failed");
                }

            }
            if (result == 1)
            {
                return Json("Data Sucessfully set to device");
            }

            else
            {
                return Json("Something is wrong");
            }
        }

        public IActionResult DltUserInfo(List<HR_MachineNoZKT> Device)
        {

            foreach (var dev in Device)
            {

                int ret = SDK.sta_ConnectTCP(dev.IpAddress, dev.PortNo, dev.ZKtPassword);

                if (ret == 1)
                {
                    if (SDK.axCZKEM1.ClearData(1, 5))
                    {
                        return Json("All data deleted");
                    };

                }
                else
                {
                    return Json("Connection failed");
                }

            }

            return Ok();
        }
        #endregion


        #region GetDeviceInfo
        [HttpGet]
        public IActionResult deviceInfo()
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");

            var a = (from b in db.HR_MachineNoZkt where b.ComId == ComId select b).ToList();
            return Json(a);
        }


        #endregion




    }
}

