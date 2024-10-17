using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceManagement;
using HVFaceManagement.Models;
using Newtonsoft.Json;
using System.Data.Odbc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Net;
using Microsoft.EntityFrameworkCore.Query.Internal;
using GTERP.Models;

namespace HVFaceManagement.Controllers
{
    public class eventManagement : Controller
    {
        private readonly IConfiguration config;
        private string ComId;
        private string dowloadOption;
        private string IsSMS;
        private string IsSQL;
        private string IsAutoDownloader;
        private string Duration;


        //private string warning;



        GTRDBContext db;
        private readonly GSDeviceDataDBContext rdata;
        public eventManagement(GTRDBContext _db, GSDeviceDataDBContext _rdata, IConfiguration config)
        {

            db = _db;
            rdata = _rdata;
            this.config = config;
            ComId = config.GetValue<string>("ComId");
            dowloadOption = config.GetValue<string>("dowloadOption");
            IsSMS = config.GetValue<string>("IsSMS");
            IsSQL = config.GetValue<string>("IsSQL");
            IsAutoDownloader = config.GetValue<string>("IsAutoDownloader");
            Duration = config.GetValue<string>("Duration");


        }

        [HttpGet]
        public IActionResult deviceInfo()
        {
            string comid = HttpContext.Session.GetString("comid");
            var a = (from b in rdata.HR_MachineNoHIK where b.ComId == comid select b).ToList();
            return Json(a);
        }
        public IActionResult Event()
        {
            ViewBag.option = dowloadOption;

            ViewBag.result = HttpContext.Session.GetString("IPresult");
            ViewBag.warning = HttpContext.Session.GetString("IPresultExist");
            ViewBag.IsAutoDownloader = IsAutoDownloader;
            //int dur = int.Parse(Duration);
            //ViewBag.Duration = dur;
            return View();
        }

        int iRet;
        public string ActionISAPI(string szUrl, string szRequest, string szMethod, DeviceInfo struDeviceInfo)
        {

            string szResponse = string.Empty;
            if (struDeviceInfo == null)
            {

                return szResponse;
            }
            if (!szUrl.Substring(0, 4).Equals("http"))
            {
                szUrl = "http://" + struDeviceInfo.strDeviceIP + ":" + struDeviceInfo.strHttpPort + szUrl;
            }
            HttpClient clHttpClient = new HttpClient();
            byte[] byResponse = { 0 };
            iRet = 0;
            string szContentType = string.Empty;

            switch (szMethod)
            {
                case "GET":
                    iRet = clHttpClient.HttpRequest(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, szMethod, ref byResponse, ref szContentType);
                    break;
                case "PUT":
                    iRet = clHttpClient.HttpPut(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, szMethod, szRequest, ref szResponse);
                    break;
                case "POST":
                    iRet = clHttpClient.HttpPut(struDeviceInfo.strUsername, struDeviceInfo.strPassword, szUrl, szMethod, szRequest, ref szResponse);
                    break;
                default:
                    break;
            }

            if (iRet == (int)HttpClient.HttpStatus.Http200)
            {
                if ((!szMethod.Equals("GET")) || (szContentType.IndexOf("application/xml") != -1))
                {
                    if (szResponse != string.Empty)
                    {
                        return szResponse;
                    }

                    if (szMethod.Equals("GET"))
                    {
                        szResponse = Encoding.Default.GetString(byResponse);
                        return szResponse;
                    }
                }
                else
                {
                    if (byResponse.Length != 0)
                    {
                        szResponse = Encoding.Default.GetString(byResponse);
                        return szResponse;
                    }
                }
            }
            else if (iRet == (int)HttpClient.HttpStatus.HttpOther)
            {
                string szCode = string.Empty;
                string szError = string.Empty;
                clHttpClient.ParserResponseStatus(szResponse, ref szCode, ref szError);
                return ("Request failed! Error code:" + szCode + " Describe:" + szError + "\r\n");
            }
            else if (iRet == (int)HttpClient.HttpStatus.HttpTimeOut)
            {
                return (szMethod + " " + szUrl + "error!Time out");
            }

            return (szResponse);
        }

        [HttpPost]
        public IActionResult ipSet(string adress, string name, string port, string password, string location, string inout)
        {

            string comid = HttpContext.Session.GetString("comid");
            HR_MachineNoHIK mn = new HR_MachineNoHIK();
            mn.Id = new Guid();
            mn.IpAddress = adress;
            mn.Hikuser = name;
            mn.Location = location;
            mn.Hikpassword = password;
            mn.InOut = inout;
            mn.PortNo = port;
            mn.ComId = comid;

            var device = (from d in rdata.HR_MachineNoHIK where d.IpAddress == adress && d.ComId == ComId select d).FirstOrDefault();
            if (device == null)
            {
                rdata.HR_MachineNoHIK.Add(mn);
                rdata.SaveChanges();
                HttpContext.Session.SetString("IPresult", "Saved to data base");

            }
            else
            {
                HttpContext.Session.SetString("IPresultExist", "already in data base");

            }

            return RedirectToAction("Event");
        }


        public IActionResult update(List<HR_MachineNoHIK> Device)
        {
            string comid = HttpContext.Session.GetString("comid");
            foreach (var dd in Device)
            {
                // var device = (from d in db.HR_MachineNoHIK where d.ComId == comid select d).FirstOrDefault();
                var existingDevice = rdata.HR_MachineNoHIK.FirstOrDefault(x => x.Id == dd.Id);
                if (existingDevice != null)
                {
                    existingDevice.IpAddress = dd.IpAddress;
                    existingDevice.Hikuser = dd.Hikuser;
                    existingDevice.Location = dd.Location;
                    existingDevice.Hikpassword = dd.Hikpassword;
                    existingDevice.InOut = dd.InOut;
                    existingDevice.PortNo = dd.PortNo;
                    rdata.HR_MachineNoHIK.Update(existingDevice);
                    rdata.SaveChanges();
                }
                //db.HR_MachineNoHIK.Remove(device);
               // db.SaveChanges();
                //HR_MachineNoHIK mn = new HR_MachineNoHIK();
                //mn.Id = new Guid();
                //mn.IpAddress = dd.IpAddress;
                //mn.Hikuser = dd.Hikuser;
                //mn.Location = dd.Location;
                //mn.Hikpassword = dd.Hikpassword;
                //mn.InOut = dd.InOut;
                //mn.PortNo = "80";
                //mn.ComId = comid;



               


            }

            return Ok();
        }
        public IActionResult delete(List<HR_MachineNoHIK> Device)
        {
            foreach (var dd in Device)
            {
                var existingDevice = rdata.HR_MachineNoHIK.FirstOrDefault(x => x.Id == dd.Id);

               // var device = (from d in rdata.HR_MachineNoHIK where d.IpAddress == dd.IpAddress && d.ComId == ComId select d).FirstOrDefault();
                rdata.HR_MachineNoHIK.Remove(existingDevice);
                rdata.SaveChanges();
            }
            return Ok("Device deleted");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,ComId,Location,IsActive,port,LuserId,Pcname,IpAddress,Hikuser,Hikpassword,Status,Inout")] HR_MachineNoHIK hR_MachineNoHIK)
        //{
        //    if (id != hR_MachineNoHIK.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            db.Update(hR_MachineNoHIK);
        //            await db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!HR_MachineNoHIKExists(hR_MachineNoHIK.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(hR_MachineNoHIK);
        //}
        //private bool HR_MachineNoHIKExists(Guid id)
        //{
        //    return db.HR_MachineNoHIK.Any(e => e.Id == id);
        //}

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = Int32.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> getEvent(string fdate, string tdate, List<HR_MachineNoHIK> Device)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
            string Fdate = fdate + "+06:00";
            string Tdate = tdate + "+06:00";
            string IP = "";

            uint dwMajor = 5;
            uint dwMinor = 0;
            List<EventViewModel> evml = new List<EventViewModel>();
            foreach (var dd in Device)
            {
                DeviceInfo DeviceInfo = new DeviceInfo();
                DeviceInfo.strUsername = dd.Hikuser;
                DeviceInfo.strPassword = dd.Hikpassword;
                DeviceInfo.strDeviceIP = dd.IpAddress;
                DeviceInfo.strHttpPort = dd.PortNo;
                IP += dd.IpAddress.ToString();

                if (Security.Login(DeviceInfo))
                {
                    // user check success
                    DeviceInfo.bIsLogin = true;
                    deviceInfos.Add(DeviceInfo);

                }
                else
                {

                    ViewBag.warning = $"{DeviceInfo.strUsername} --Couldn't connect";
                    continue;
                }
            }
            if (deviceInfos.Count != 0)
            {
                foreach (var struDeviceInfo in deviceInfos)
                {
                    String pos = "0";
                    string szUrl = "/ISAPI/AccessControl/AcsEvent?format=json";
                    while (true)
                    {

                        string szResponse = string.Empty;
                        string szRequest = "{\"AcsEventCond\":{\"searchID\":\"1\",\"searchResultPosition\":" + pos +
                            ",\"maxResults\":1000,\"major\":" + dwMajor +
                        ",\"minor\":" + dwMinor +
                        ",\"startTime\":\"" + Fdate +
                        "\",\"endTime\":\"" + Tdate +
                        "\"}}";
                        string szMethod = "POST";

                        szResponse = ActionISAPI(szUrl, szRequest, szMethod, struDeviceInfo);
                        if (szResponse != string.Empty && iRet == 0)
                        {
                            //进行JSON内容提取
                            AcsEventObject rb = JsonConvert.DeserializeObject<AcsEventObject>(szResponse);

                            if (rb.AcsEvent == null)
                            {
                                //MessageBox.Show("AcsEvent is NULL!");
                                break;
                            }

                            String responseTemp = rb.AcsEvent.responseStatusStrg;
                            if (responseTemp.Equals("NO MATCH"))
                            {
                                break;
                            }

                            foreach (InfoListItem info in rb.AcsEvent.InfoList)
                            {
                                EventViewModel evm = new EventViewModel();
                                evm.cardNo = info.employeeNoString;
                                if (evm.cardNo == "" || evm.cardNo is null)
                                { continue; };
                                //evm.deviceNo = struDeviceInfo.strDeviceIP.Substring(8);

                                if (!string.IsNullOrEmpty(struDeviceInfo.strDeviceIP))
                                {
                                    string[] IPAdd = struDeviceInfo.strDeviceIP.Split('.');
                                    evm.deviceNo = IPAdd[3];
                                }

                                evm.date = info.time.Substring(0, 10);
                                evm.time = info.time.Substring(11, 5);
                                evml.Add(evm);

                                string conn = config.GetConnectionString("DefaultConnection");
                                if (IsSQL == "1" && IsSMS == "0")
                                {
                                    using (SqlConnection connection = new SqlConnection(conn))
                                    {
                                        connection.Open();
                                        string sql = "INSERT INTO Hr_RawData(CardNo,dtPunchDate,dtPunchTime,StNO,inout,OvNMark,DeviceNo,ComID) VALUES (@value1, @value2, @value3,@value4, @value5, @value6,@value7, @value8)";
                                        using (SqlCommand command = new SqlCommand(sql, connection))
                                        {
                                            command.Parameters.AddWithValue("@value1", info.employeeNoString);
                                            command.Parameters.AddWithValue("@value2", evm.date);
                                            command.Parameters.AddWithValue("@value3", evm.time);
                                            command.Parameters.AddWithValue("@value4", 0);
                                            command.Parameters.AddWithValue("@value5", 0);
                                            command.Parameters.AddWithValue("@value6", 0);
                                            command.Parameters.AddWithValue("@value7", IP);
                                            command.Parameters.AddWithValue("@value8", 2);
                                            command.ExecuteNonQuery();
                                        }
                                        connection.Close();
                                    }
                                }
                                if (IsSMS == "1" && IsSQL == "0")
                                {
                                    using (SqlConnection connection = new SqlConnection(conn))
                                    {
                                        connection.Open();
                                        string sql = "INSERT INTO tblRawData(CardNo,dtPunchDate,dtPunchTime,StNO,inout,OvNMark,DeviceNo,ComID) VALUES (@value1, @value2, @value3,@value4, @value5, @value6,@value7, @value8)";
                                        using (SqlCommand command = new SqlCommand(sql, connection))
                                        {
                                            command.Parameters.AddWithValue("@value1", info.employeeNoString);
                                            command.Parameters.AddWithValue("@value2", evm.date);
                                            command.Parameters.AddWithValue("@value3", evm.time);
                                            command.Parameters.AddWithValue("@value4", 0);
                                            command.Parameters.AddWithValue("@value5", 0);
                                            command.Parameters.AddWithValue("@value6", 0);
                                            command.Parameters.AddWithValue("@value7", evm.deviceNo);
                                            command.Parameters.AddWithValue("@value8", 2);
                                            command.ExecuteNonQuery();
                                        }
                                        connection.Close();
                                    }
                                    using (SqlConnection conns = new SqlConnection(conn))
                                    {
                                        conns.Open();
                                        SqlCommand cmd = new SqlCommand("Exec prcGetSmsGrid @param1, @param2, @param3, @param4, @param5, @param6", conns);
                                        cmd.CommandType = CommandType.Text;

                                        cmd.Parameters.AddWithValue("@param1", 2);
                                        cmd.Parameters.AddWithValue("@param2", 0);
                                        cmd.Parameters.AddWithValue("@param3", "AutoSMSMachine");
                                        cmd.Parameters.AddWithValue("@param4", 0);
                                        cmd.Parameters.AddWithValue("@param5", evm.date);
                                        cmd.Parameters.AddWithValue("@param6", evm.date);

                                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                        DataSet dataSet = new DataSet();

                                        await Task.Run(() => adapter.Fill(dataSet));

                                        DataTable table1 = dataSet.Tables[0];
                                        DataTable table2 = dataSet.Tables[1];
                                        conns.Close();

                                        string smsAddress = "";
                                        string smsUser = "";
                                        string smsSender = "";
                                        string smsPassword = "";
                                        string restpart = "&csmsid=123456789&sid=cambrian";
                                        foreach (DataRow row in table2.Rows)
                                        {
                                            // access the data in each column of the current row
                                            smsAddress = (string)row["smsAddress"];
                                            smsUser = (string)row["smsUser"];
                                            smsSender = (string)row["smsSender"];
                                            smsPassword = (string)row["smsPassword"];
                                            // etc.
                                        }
                                        string BaseUrl = "";
                                        string MobileNo = "";
                                        int flag;
                                        foreach (DataRow row in table1.Rows)
                                        {
                                            // access the data in each column of the current row
                                            MobileNo += (string)row["MobileNo"];
                                            flag = Convert.ToInt32(row["sms"]);
                                            string AutoMessage = "&sms=" + (string)row["AutoMessage"];
                                            BaseUrl = smsAddress + smsUser + smsPassword + MobileNo + AutoMessage + restpart;
                                            string result;
                                            string commandText = "UPDATE tblRawData SET sms=@Value1 WHERE CardNo=@ID";
                                            using (SqlCommand command = new SqlCommand(commandText, conns))
                                            {
                                                conns.Open();
                                                // set the parameter values
                                                command.Parameters.AddWithValue("@Value1", 1);
                                                command.Parameters.AddWithValue("@ID", (int.Parse((string)row["CardNo"])));

                                                // execute the command
                                                command.ExecuteNonQuery();
                                                conns.Close();
                                            }
                                            if (flag == 0)
                                            {
                                                using (var client = new WebClient())
                                                {
                                                    using (var data = client.OpenRead(BaseUrl))
                                                    {
                                                        using (var reader = new StreamReader(data))
                                                        {
                                                            result = reader.ReadToEnd();
                                                        }
                                                    }
                                                }
                                            }


                                        }


                                    }
                                }



                            }


                            int posTmp = 0;
                            int.TryParse(pos, out posTmp);
                            int numOfMatches = rb.AcsEvent.numOfMatches;
                            posTmp += numOfMatches;
                            pos = posTmp.ToString();

                            if (rb.AcsEvent.responseStatusStrg.Equals("OK"))
                            {
                                break;
                            }
                        }

                    }

                }
            }
            else
            {
                //warning = "No device is connected";
                return Json("noDevice");
            }
            return Json(evml);
        }
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = Int32.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public IActionResult saveToDB(List<EventViewModel> Sdata)
        {
            var eTIME = "";
            try
            {
                foreach (EventViewModel evm in Sdata)
                {
                    Hr_RawData hrRawData = new Hr_RawData();
                    hrRawData.CardNo = evm.cardNo;
                    hrRawData.ComId = ComId;
                    hrRawData.DtPunchDate = Convert.ToDateTime(evm.date);
                    eTIME = "1900-01-01 " + evm.time;
                    hrRawData.DtPunchTime = Convert.ToDateTime(eTIME);
                    hrRawData.EmpId = null;
                    var dbdata = (from d in db.Hr_RawData
                                  where d.CardNo == evm.cardNo && d.ComId == ComId && d.DtPunchDate == hrRawData.DtPunchDate && d.DtPunchTime == hrRawData.DtPunchTime
                                  select d).FirstOrDefault();
                    if (dbdata == null)
                    {
                        db.Add(hrRawData);
                        db.SaveChanges();
                    }

                }
                return Json("Data Saved Successfully!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public IActionResult ExportToText(List<EventViewModel> fileData)
        {
            string filepath = string.Format("C:\\Users\\ACER\\Download\\new.txt");
            string JsString = "";
            foreach (EventViewModel ev in fileData)
            {
                string jsonString = JsonConvert.SerializeObject(ev);
                JsString += jsonString;
            };
            var fileName = "test.txt";
            var mimeType = "text/plain";
            var fileBytes = Encoding.ASCII.GetBytes(JsString);
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                BinaryWriter objBinaryWrite = new BinaryWriter(fs);
                fs.Write(fileBytes, 0, fileBytes.Length);
                fs.Close();
            }
            return new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = fileName
            };
        }


        [HttpPost]
        public IActionResult saveTxt(List<EventViewModel> fileData)
        {
            try
            {
                string FileLocation = @".\Download\";
                string FileName = DateTime.Now.ToString("dd-MMM-yyyy") + "_" +
                                   DateTime.Now.ToString("hh" + "'Hr'" + "-mm" + "'Min'");
                if (!System.IO.File.Exists(FileLocation + FileName + ".txt"))
                {
                    System.IO.File.Create(FileLocation + FileName + ".txt").Close();
                }
                StreamWriter sw = System.IO.File.CreateText(FileLocation + FileName + ".txt");
                foreach (EventViewModel row in fileData)
                {
                    string DeviceNo = row.deviceNo;
                    string CardNo = row.cardNo;
                    string pDate = row.date;
                    string pTime = row.time;
                    string pInfoByGTR_txt = DeviceNo + ":" + CardNo + ":" + pDate + ":" + pTime + ":00";
                    sw.WriteLine("{0}", pInfoByGTR_txt);
                } //for each
                sw.Close();
                return Json("Data Download successfully!.");

            }  //try
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        [HttpPost]

        [RequestFormLimits(ValueCountLimit = Int32.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public IActionResult saveToMA(List<EventViewModel> fileData)
        {
            string MAdata = @".\Download\Database1.accdb";

            //MAdata += '" + DeviceNo + "', '" + CardNo + "', '" + pDate + "', '" + pTime + "';


            string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + MAdata + "";

            OleDbConnection con = new OleDbConnection(constring);

            con.Open();
            OleDbCommand cmd = new OleDbCommand("delete from myData", con);
            cmd.ExecuteNonQuery();
            foreach (EventViewModel row in fileData)
            {

                //Select DeviceNo, InOut, CardNo, dtPunchDate, dtPunchTime from tblRawdata
                int index = 1;
                string DeviceNo = row.deviceNo;
                string CardNo = row.cardNo;
                string pDate = row.date;
                string pTime = row.time;

                OleDbCommand cmd1 = new OleDbCommand("insert into myData values ('" + DeviceNo + "','" + CardNo + "','" + pDate + "','" + pTime + "')", con);

                cmd1.CommandType = CommandType.Text;
                cmd1.ExecuteNonQuery();


            }
            return Json("Data Export successfully complete.");

        }

        [HttpPost]
        public IActionResult clearEvent(List<HR_MachineNoHIK> Device, string time)
        {

            string szUrl = "/ISAPI/AccessControl/AcsEvent/StorageCfg?format=json";
            string szResponse = string.Empty;
            string szRequest = "{\"EventStorageCfg\":{\"mode\":\"time\",\"checkTime\":\"" + time + "\"}}";


            string szMethod = "PUT";
            foreach (var dd in Device)
            {

                DeviceInfo DeviceInfo = new DeviceInfo();
                DeviceInfo.strUsername = dd.Hikuser;
                DeviceInfo.strPassword = dd.Hikpassword;
                DeviceInfo.strDeviceIP = dd.IpAddress;
                DeviceInfo.strHttpPort = dd.PortNo;

                if (Security.Login(DeviceInfo))
                {
                    // user check success
                    DeviceInfo.bIsLogin = true;


                }
                else
                {

                    return Json($"{DeviceInfo.strUsername} --Couldn't connect");

                }

                szResponse = ActionISAPI(szUrl, szRequest, szMethod, DeviceInfo);
            }
            return Json(szResponse);
        }




    }
}
