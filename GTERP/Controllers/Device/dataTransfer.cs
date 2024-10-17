//using FaceManagement;
//using GTERP.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Drawing.Drawing2D;
//using System.Runtime.InteropServices;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using HVFaceManagement.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using GTERP.Controllers.Device.Model;

//namespace GTERP.Controllers.Device
//{
//    public class dataTransfer : Controller
//    {

//        private readonly GTRDBContext db;
//        private IWebHostEnvironment HostEnvironment;
//        private IHttpContextAccessor _httpContext;
//        private ILogger<dataTransfer> _logger;
//        public dataTransfer(GTRDBContext _db, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContext, ILogger<dataTransfer> logger)
//        {
//            db = _db;
//            HostEnvironment = hostEnvironment;
//            _httpContext = httpContext;
//            _logger = logger;

//        }


//        string pic;
//        byte[] image;
//        byte[] byFingerData;
//        int iRet;

//        public static DeviceInfo struDeviceInfo;
//        public List<DeviceInfo> deviceInfos = new List<DeviceInfo>();

//        List<empData> empData = new List<empData>();

//        public IActionResult Index()
//        {
//            return View();
//        }

//        private string ActionISAPI(string szUrl, string szRequest, string szMethod, DeviceInfo struDeviceInfo)
//        {

//            string szResponse = string.Empty;
//            if (struDeviceInfo == null)
//            {

//                return szResponse;
//            }
//            if (!struDeviceInfo.bIsLogin)
//            {
//                return szResponse = "Login Failed";
//            }

//            if (!szUrl.Substring(0, 4).Equals("http"))
//            {
//                szUrl = "http://" + struDeviceInfo.strDeviceIP + ":" + struDeviceInfo.strHttpPort + szUrl;
//            }
//            HttpClient clHttpClient = new HttpClient();
//            byte[] byResponse = { 0 };
//            iRet = 0;
//            string szContentType = string.Empty;

//            switch (szMethod)
//            {
//                case "GET":
//                    iRet = clHttpClient.HttpRequest(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, szMethod, ref byResponse, ref szContentType);
//                    break;
//                case "PUT":
//                    iRet = clHttpClient.HttpPut(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, szMethod, szRequest, ref szResponse);
//                    break;
//                case "POST":
//                    iRet = clHttpClient.HttpPut(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, szMethod, szRequest, ref szResponse);
//                    break;
//                default:
//                    break;
//            }

//            if (iRet == (int)HttpClient.HttpStatus.Http200)
//            {
//                if ((!szMethod.Equals("GET")) || (szContentType.IndexOf("application/xml") != -1))
//                {
//                    if (szResponse != string.Empty)
//                    {
//                        return szResponse;
//                    }

//                    if (szMethod.Equals("GET"))
//                    {
//                        szResponse = Encoding.Default.GetString(byResponse);
//                        return szResponse;
//                    }
//                }
//                else
//                {
//                    if (byResponse.Length != 0)
//                    {
//                        szResponse = Encoding.Default.GetString(byResponse);
//                        return szResponse;
//                    }
//                }
//            }
//            else if (iRet == (int)HttpClient.HttpStatus.HttpOther)
//            {
//                string szCode = string.Empty;
//                string szError = string.Empty;
//                clHttpClient.ParserResponseStatus(szResponse, ref szCode, ref szError);
//                return ("Request failed! Error code:" + szCode + " Describe:" + szError + "\r\n");
//            }
//            else if (iRet == (int)HttpClient.HttpStatus.HttpTimeOut)
//            {
//                return (szMethod + " " + szUrl + "error!Time out");
//            }

//            return (szResponse);
//        }



//        private string ActionISAPI2(string szUrl, string szRequest, string szMethod, DeviceInfo struDeviceInfo)

//        {

//            string szResponse = string.Empty;
//            if (struDeviceInfo == null)
//            {
//                return szResponse;
//            }
//            if (!struDeviceInfo.bIsLogin)
//            {
//                return szResponse;
//            }
//            szUrl = szUrl.Substring(szUrl.IndexOf("/LOCALS"));
//            if (!szUrl.Substring(0, 4).Equals("http"))
//            {
//                szUrl = "http://" + struDeviceInfo.strDeviceIP + ":" + struDeviceInfo.strHttpPort + szUrl;
//            }
//            HttpClient clHttpClient = new HttpClient();
//            byte[] byResponse = { 0 };
//            int iRet = 0;
//            string szContentType = string.Empty;

//            iRet = clHttpClient.HttpRequest2(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, szMethod, ref byResponse, ref szContentType);

//            if (iRet == (int)HttpClient.HttpStatus.Http200)
//            {
//                if ((!szMethod.Equals("GET")) || (szContentType.IndexOf("application/xml") != -1))
//                {
//                    if (szResponse != string.Empty)
//                    {
//                        return szResponse;
//                    }

//                    if (szMethod.Equals("GET"))
//                    {
//                        szResponse = Encoding.Default.GetString(byResponse);
//                        return szResponse;
//                    }
//                }
//                else
//                {
//                    if (byResponse.Length != 0)
//                    {
//                        image = byResponse;
//                        szResponse = Encoding.UTF8.GetString(byResponse);

//                        string szPath = string.Format("{0}\\employee.jpg", Environment.CurrentDirectory);
//                        pic = szResponse;
//                        try
//                        {
//                            using (FileStream fs = new FileStream(szPath, FileMode.OpenOrCreate))
//                            {

//                                BinaryWriter objBinaryWrite = new BinaryWriter(fs);
//                                fs.Write(byResponse, 0, byResponse.Length);
//                                fs.Close();

//                            }

//                        }
//                        catch (Exception ex)
//                        {

//                        }
//                        return szResponse;
//                    }

//                }
//            }
//            else if (iRet == (int)HttpClient.HttpStatus.HttpOther)
//            {
//                string szCode = string.Empty;
//                string szError = string.Empty;
//                clHttpClient.ParserResponseStatus(szResponse, ref szCode, ref szError);
//                //MessageBox.Show("Request failed! Error code:" + szCode + " Describe:" + szError + "\r\n");
//            }
//            else if (iRet == (int)HttpClient.HttpStatus.HttpTimeOut)
//            {
//                //MessageBox.Show(szMethod + " " + szUrl + "error!Time out");
//            }
//            return szResponse;
//        }

//        [HttpPost]


//        [RequestFormLimits(ValueCountLimit = 10000)]
//        [RequestSizeLimit(int.MaxValue)]
//        public IActionResult SetFingerPrint(List<Model.viewModel> data, List<HR_MachineNoHIK> Device)

//        {
//            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();

//            foreach (var dd in Device)
//            {
//                struDeviceInfo = new DeviceInfo();
//                struDeviceInfo.strUsername = dd.Hikuser;
//                struDeviceInfo.strPassword = dd.Hikpassword;
//                struDeviceInfo.strDeviceIP = dd.IpAddress;
//                struDeviceInfo.strHttpPort = dd.PortNo;

//                if (Security.Login(struDeviceInfo))
//                {
//                    // user check success
//                    struDeviceInfo.bIsLogin = true;
//                    deviceInfos.Add(struDeviceInfo);

//                }
//                else
//                {
//                    ViewBag.errmsg = $"{dd.IpAddress} :Login Fail";
//                    continue;
//                }
//            }
//            foreach (viewModel a in data)
//            {
//                string szUrl = "/ISAPI/AccessControl/UserInfo/Search?format=json";
//                string szResponse = string.Empty;
//                string szRequest = "{\"UserInfoSearchCond\":{\"searchID\":\"1\",\"searchResultPosition\":0,\"maxResults\":30,\"EmployeeNoList\":[{\"employeeNo\":\"" + a.CardNo + "\"}]}}";
//                string szMethod = "POST";

//                foreach (var struDeviceInfo in deviceInfos)
//                {
//                    //查询是否存在工号
//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);


//                    var condition = !string.IsNullOrEmpty(szResponse);

//                    if (condition)
//                    {
//                        UserInfoSearchRoot us = new UserInfoSearchRoot();
//                        us = JsonConvert.DeserializeObject<UserInfoSearchRoot>(szResponse);

//                        if (1 == us.UserInfoSearch.numOfMatches)
//                        {
//                            continue;
//                        }
//                    }
//                    szUrl = "/ISAPI/AccessControl/UserInfo/SetUp?format=json";
//                    szRequest = "{\"UserInfo\":{\"employeeNo\":\"" + a.CardNo +
//                       "\",\"name\":\"" + a.EmpName +
//                       "\",\"userType\":\"normal\",\"Valid\":{\"enable\":true,\"beginTime\":\"2019-08-01T17:30:08\",\"endTime\":\"2035-08-01T17:30:08\"},\"doorRight\": \"1\",\"RightPlan\":[{\"doorNo\":1,\"planTemplateNo\":\"" + "1" + "\"}]}}";
//                    szMethod = "PUT";

//                    //下发人员信息,若人员存在则修改人员信息 ------//Send the personnel information, if the personnel exists, modify the personnel information

//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);
//                    if (szResponse != string.Empty && iRet == 0)
//                    {
//                        ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
//                        if ("1" == rs.statusCode)
//                        {
//                            ViewBag.setinfo = ("Set UserInfo Succ!");
//                        }
//                        else
//                        {
//                            ViewBag.setinfo2 = ("Set UserInfo fail!");
//                        }
//                    }
//                    /// delete pic if exist
//                    szUrl = "/ISAPI/Intelligent/FDLib/FDSearch?format=json";

//                    szResponse = string.Empty;
//                    szRequest = "{\"searchResultPosition\":0,\"maxResults\":30,\"faceLibType\":\"blackFD\",\"FDID\":\"1\",\"FPID\":\"" + a.CardNo + "\"}";

//                    szMethod = "POST";

//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);
//                    if (szResponse != string.Empty && iRet == 0)
//                    {
//                        Root rt = JsonConvert.DeserializeObject<Root>(szResponse);
//                        if (rt.statusCode == 1)
//                        {
//                            if (rt.totalMatches != 0)
//                            {
//                                szUrl = "/ISAPI/Intelligent/FDLib/FDSearch/Delete?format=json&FDID=1&faceLibType=blackFD";
//                                szResponse = string.Empty;
//                                szRequest = "{\"FPID\":[{\"value\":\"" + a.CardNo + "\"}]}";
//                                szMethod = "PUT";

//                                szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);
//                                if (szResponse != string.Empty)
//                                {
//                                    ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
//                                    if (!rs.statusCode.Equals("1"))
//                                    {
//                                        return Ok();
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    /// set pic
//                    szUrl = "/ISAPI/Intelligent/FDLib/FaceDataRecord?format=json";

//                    if (!szUrl.Substring(0, 4).Equals("http"))
//                    {
//                        szUrl = "http://" + struDeviceInfo.strDeviceIP + ":" + struDeviceInfo.strHttpPort + szUrl;
//                    }
//                    HttpClient clHttpClient = new HttpClient();
//                    szResponse = string.Empty;
//                    szRequest = "{\"faceLibType\":\"blackFD\",\"FDID\":\"1\",\"FPID\":\"" + a.CardNo + "\"}";
//                    //string filePath = string.Format(@".\Image\{0}.jpg", a.CardNo);
//                    byte[] imgfile = a.EmpImage;
//                    if (imgfile != null)
//                    {

//                        szResponse = clHttpClient.HttpPostData(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, imgfile, szRequest);
//                        ResponseStatus res = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
//                        if (res != null && res.statusCode.Equals("1"))
//                        {

//                            ViewBag.successmsg = "pic has been set";
//                        }

//                    }

//                    szUrl = "/ISAPI/AccessControl/FingerPrint/SetUp?format=json";
//                    szResponse = string.Empty;
//                    string fingerData;

//                    if (a.FingerData != null)
//                    {
//                        try
//                        {
//                            fingerData = Convert.ToBase64String(a.FingerData);
//                        }
//                        catch
//                        {
//                            fingerData = null;
//                        }




//                        szRequest = "{\"FingerPrintCfg\":{\"employeeNo\":\"" + a.CardNo +
//                            "\",\"enableCardReader\":[1],\"fingerPrintID\":1,\"fingerType\":\"normalFP\",\"fingerData\":\"" + fingerData + "\"}}";
//                        //System.IO.File.WriteAllText(@"C:\Users\chendaliang\Desktop\WriteText.txt", szRequest);
//                        szMethod = "POST";

//                        szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);

//                        if (szResponse != string.Empty && iRet == 0)
//                        {
//                            FPRoot fr = JsonConvert.DeserializeObject<FPRoot>(szResponse);
//                            foreach (StatusListItem item in fr.FingerPrintStatus.StatusList)
//                            {
//                                if (item.id.ToString() == "1")
//                                {
//                                    if (item.cardReaderRecvStatus != 1)
//                                    {
//                                        ViewBag.error = ("Set fingerData failed! errorMsg:" + item.errorMsg);
//                                    }
//                                    else
//                                    {
//                                        ViewBag.success = ("Set fingerData succeed!");
//                                    }
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return Json("uploaded");
//        }
//        [HttpPost]
//        public IActionResult GetFingerFace(List<HR_MachineNoHIK> Device)
//        {

//            List<empData> edlist = new List<empData>();
//            List<HR_Emp_DeviceInfoHIK> deviceData = new List<HR_Emp_DeviceInfoHIK>();
//            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
//            UserInfoSearchRoot us = new UserInfoSearchRoot();
//            var ComId = _httpContext.HttpContext.Session.GetString("comid");


//            foreach (var dd in Device)
//            {
//                struDeviceInfo = new DeviceInfo();
//                struDeviceInfo.strUsername = dd.Hikuser;
//                struDeviceInfo.strPassword = dd.Hikpassword;
//                struDeviceInfo.strDeviceIP = dd.IpAddress;
//                struDeviceInfo.strHttpPort = dd.PortNo;

//                if (Security.Login(struDeviceInfo))
//                {
//                    // user check success
//                    struDeviceInfo.bIsLogin = true;
//                    deviceInfos.Add(struDeviceInfo);

//                }
//                else
//                {
//                    ViewBag.errmsg = $"{dd.IpAddress} :Login Fail";
//                    continue;
//                }
//            }


//            foreach (var struDeviceInfo in deviceInfos)
//            {
//                //查询是否存在工号
//                for (int i = 0; i < 1000; i += 30)
//                {
//                    string szUrl = "/ISAPI/AccessControl/UserInfo/Search?format=json";
//                    string szResponse = string.Empty;
//                    string szRequest = "{\"UserInfoSearchCond\":{\"searchID\":\"1\",\"searchResultPosition\":" + i + ",\"maxResults\":30}}";
//                    //string szRequest = "{\"UserInfoSearchCond\":{\"searchID\":\"1\",\"searchResultPosition\":0,\"maxResults\":30,\"EmployeeNoList\":[{\"employeeNo\":\"" + i + "\"}]}}";
//                    string szMethod = "POST";


//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);

//                    var condition = !string.IsNullOrEmpty(szResponse);

//                    if (condition)
//                    {
//                        us = JsonConvert.DeserializeObject<UserInfoSearchRoot>(szResponse);
//                        var user = us.UserInfoSearch.UserInfo;

//                        if (0 == us.UserInfoSearch.numOfMatches)
//                        {

//                            continue;
//                        }


//                        foreach (var uu in user)
//                        {
//                            empData ui = new empData();
//                            if (uu == null) { continue; }
//                            ui.CardNo = uu.employeeNo;
//                            ui.EmpName = uu.name;

//                            var dbCompare = (from dc in db.HR_Emp_DeviceInfoHIK where dc.CardNo == uu.employeeNo && dc.comId == ComId select dc).FirstOrDefault();
//                            if (dbCompare == null)
//                            {
//                                edlist.Add(ui);
//                            }
//                        }

//                        //查询人脸库是否存在
//                        //Query whether the face database exists
//                    }
//                    else
//                    {
//                        return Content("Please Connect Device First!");
//                    };
//                    edlist.Count();

//                }

//                foreach (var dd in edlist)
//                {

//                    string fingerData;
//                    string fingerPath = "";
//                    string picPath = "";

//                    string szUrl = "/ISAPI/AccessControl/FingerPrintUpload?format=json";
//                    string szResponse = string.Empty;
//                    int searchID = 1;



//                    string szRequest = "{\"FingerPrintCond\":{\"searchID\":\"" + searchID + "\",\"employeeNo\":\"" + dd.CardNo +
//                           "\",\"cardReaderNo\":1,\"fingerPrintID\":1}}";
//                    string szMethod = "POST";
//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);
//                    if (szResponse != string.Empty && iRet == 0)
//                    {
//                        FPInfo fi = JsonConvert.DeserializeObject<FPInfo>(szResponse);
//                        if (szResponse != string.Empty)
//                        {
//                            if (fi.FingerPrintInfo.status.Equals("OK"))
//                            {
//                                foreach (FingerPrintListItem item in fi.FingerPrintInfo.FingerPrintList)
//                                {
//                                    fingerData = item.fingerData;

//                                    byFingerData = Convert.FromBase64String(fingerData);
//                                    fingerPath = string.Format(@".\finger\{0}.dat", dd.CardNo);

//                                }



//                            }
//                            else if (fi.FingerPrintInfo.status.Equals("NoFP"))
//                            {
//                                byFingerData = null;


//                            }
//                        }

//                    }
//                    szUrl = "/ISAPI/Intelligent/FDLib/FDSearch?format=json";
//                    szResponse = string.Empty;
//                    szRequest = "{\"searchResultPosition\":0,\"maxResults\":5,\"faceLibType\":\"blackFD\",\"FDID\":\" 1\",\"FPID\":\"" + dd.CardNo + "\"}";
//                    szMethod = "POST";

//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);
//                    if (szResponse != string.Empty && iRet == 0)
//                    {
//                        Root rt = JsonConvert.DeserializeObject<Root>(szResponse);
//                        if (rt.statusCode == 1)
//                        {

//                            if (rt.totalMatches == 1)
//                            {
//                                string picData = string.Empty;
//                                foreach (MatchListItem item in rt.MatchList)
//                                {
//                                    picData = item.modelData;
//                                    string url = item.faceURL;
//                                    string data = ActionISAPI2(url, szRequest, "GET", struDeviceInfo);

//                                    picPath = string.Format(@".\Image\{0}.jpg", dd.CardNo);

//                                }
//                            }
//                            else { image = null; }
//                        }
//                    }
//                    HR_Emp_DeviceInfoHIK ff = new HR_Emp_DeviceInfoHIK();
//                    ff.CardNo = dd.CardNo;
//                    ff.comId = _httpContext.HttpContext.Session.GetString("comid"); ;
//                    ff.EmpName = dd.EmpName;
//                    ff.FingerData = byFingerData;
//                    ff.EmpImage = image;
//                    ff.IpAddress = struDeviceInfo.strDeviceIP;
//                    var dbdata = (from d in db.HR_Emp_DeviceInfoHIK where d.CardNo == dd.CardNo && d.comId == ff.comId select d).FirstOrDefault();

//                    if (dbdata == null)
//                    {
//                        db.HR_Emp_DeviceInfoHIK.Add(ff);
//                        db.SaveChanges();
//                    }
//                    deviceData.Add(ff);
//                }
//            }

//            return Json("Data Imported Successfully!");
//        }
//        [HttpGet]
//        public IActionResult Empinfo()
//        {
//            string comid = _httpContext.HttpContext.Session.GetString("comid");
//            List<viewModel> viewModels = new List<viewModel>();
//            var dbconfig = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile("appsettings.json").Build();
//            try
//            {
//                var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
//                string sql = $"exec prcGetDevicesDataTranHIK'{comid}'";
//                using (SqlConnection connection = new SqlConnection(dbconnectionStr))
//                {
//                    SqlCommand command = new SqlCommand(sql, connection);
//                    connection.Open();
//                    using (SqlDataReader dataReader = command.ExecuteReader())
//                    {
//                        while (dataReader.Read())
//                        {
//                            viewModel vm = new viewModel();
//                            vm.EmpCode = dataReader["EmpCode"].ToString();
//                            vm.CardNo = dataReader["CardNo"].ToString();
//                            vm.EmpName = dataReader["EmpName"].ToString();
//                            //vm.EmpImage = dataReader["empImage"] == DBNull.Value ? new byte[0] : (byte[])dataReader["EmpImage"];
//                            vm.emp_image = dataReader["empImage"].ToString();

//                            //vm.emp_image = dataReader["empImage"] == DBNull.Value ? "No" : "yes";
//                            //vm.FingerData = dataReader["fingerData"] == DBNull.Value ? new byte[0] : (byte[])dataReader["FingerData"];
//                            vm.finger_data = dataReader["fingerData"].ToString();

//                            //vm.finger_Data = dataReader["fingerData"] == DBNull.Value ? "No" : "yes";
//                            vm.DeptName = dataReader["DeptName"].ToString();
//                            vm.DesigName = dataReader["DesigName"].ToString();
//                            vm.SectName = dataReader["SectName"].ToString();
//                            vm.IpAddress = dataReader["IPAddress"].ToString();
//                            //ViewBag=
//                            viewModels.Add(vm);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(viewModels);
//        }
//        [HttpGet]
//        public IActionResult deviceInfo()
//        {

//            try
//            {
//                string comid = _httpContext.HttpContext.Session.GetString("comid");
//                var IpAddress = db.HR_MachineNoHIK.Where(a => a.ComId == comid).ToList();
//                return Json(IpAddress);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogInformation(ex.InnerException.ToString());
//            }

//            return Json(null);

//            var a = db.HR_MachineNoHIK.ToList();
//            return Json(a);

//        }

//        [HttpPost]
//        public IActionResult ipSet(string adress, string name, string password)
//        {
//            HR_MachineNoHIK mn = new HR_MachineNoHIK();
//            mn.Id = new Guid();
//            mn.IpAddress = adress;
//            mn.ComId = _httpContext.HttpContext.Session.GetString("comid");
//            mn.Hikuser = name;
//            mn.Hikpassword = password;

//            var device = (from d in db.HR_MachineNoHIK where d.IpAddress == adress select d).FirstOrDefault();
//            if (device == null)
//            {
//                db.HR_MachineNoHIK.Add(mn);
//                db.SaveChanges();
//            }
//            return RedirectToAction("Index");
//        }

//        //[HttpPost]
//        //public IActionResult btnDel_Click(List<viewModel> data)
//        //{
//        //    foreach (var i in data) {
//        //        var a = (from dlt in db.HR_Emp_DeviceInfoHIK where dlt.cardNo == i.CardNo select dlt).FirstOrDefault();

//        //        db.HR_Emp_DeviceInfoHIK.Remove(a);
//        //        db.SaveChanges();

//        //    }
//        //    return Json("Selected data deleted");
//        //}
//        [HttpPost]

//        [RequestFormLimits(ValueCountLimit = 10000)]
//        [RequestSizeLimit(int.MaxValue)]
//        public IActionResult btnDel(List<viewModel> data, List<HR_MachineNoHIK> Device)
//        {
//            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
//            foreach (var dd in Device)
//            {
//                struDeviceInfo = new DeviceInfo();
//                struDeviceInfo.strUsername = dd.Hikuser;
//                struDeviceInfo.strPassword = dd.Hikpassword;
//                struDeviceInfo.strDeviceIP = dd.IpAddress;
//                struDeviceInfo.strHttpPort = dd.PortNo;

//                if (Security.Login(struDeviceInfo))
//                {
//                    // user check success
//                    struDeviceInfo.bIsLogin = true;
//                    deviceInfos.Add(struDeviceInfo);

//                }
//                else
//                {
//                    ViewBag.errmsg = $"{dd.IpAddress} :Login Fail";
//                    continue;
//                }
//            }
//            foreach (var i in data)
//            {
//                UserInfoSearchRoot us = new UserInfoSearchRoot();



//                string szUrl = "/ISAPI/AccessControl/UserInfo/Search?format=json";
//                string szResponse = string.Empty;
//                string szRequest = "{\"UserInfoSearchCond\":{\"searchID\":\"1\",\"searchResultPosition\":0,\"maxResults\":30,\"EmployeeNoList\":[{\"employeeNo\":\"" + i.CardNo + "\"}]}}";
//                string szMethod = "POST";

//                var DI = (from dlt in db.HR_MachineNoHIK where dlt.IpAddress == i.IpAddress select dlt).FirstOrDefault();
//                struDeviceInfo = new DeviceInfo();
//                struDeviceInfo.strUsername = DI.Hikuser;
//                struDeviceInfo.strPassword = DI.Hikpassword;
//                struDeviceInfo.strDeviceIP = DI.IpAddress;
//                struDeviceInfo.strHttpPort = "80";


//                foreach (var struDeviceInfo in deviceInfos)
//                {
//                    //查询是否存在工号
//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);


//                    var condition = !string.IsNullOrEmpty(szResponse);

//                    if (condition)
//                    {
//                        us = JsonConvert.DeserializeObject<UserInfoSearchRoot>(szResponse);

//                        if (0 == us.UserInfoSearch.totalMatches)
//                        {

//                            continue;
//                        }
//                    }


//                    szUrl = "/ISAPI/AccessControl/UserInfo/Delete?format=json";
//                    szResponse = string.Empty;
//                    szRequest = "{\"UserInfoDelCond\":{\"EmployeeNoList\":[{\"employeeNo\":\"" + i.CardNo + "\"}]}}";
//                    szMethod = "PUT";

//                    szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);
//                    if (szResponse != string.Empty && iRet == 0)
//                    {

//                        ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
//                        if (rs.statusString.Equals("OK"))
//                        {
//                            continue;
//                        }
//                        else
//                        {
//                            return Content("data hasn't been deleted");
//                        }
//                    }
//                }
//            }

//            return Content("data has been deleted");
//        }
//        protected override void Dispose(bool disposing)
//        {
//            base.Dispose(disposing);

//            GC.Collect();
//        }
//    }

//}

