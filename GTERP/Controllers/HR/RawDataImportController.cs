using GTERP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Access.Dao;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GTERP.Controllers.HR
{
    public class RawDataImportController : Controller
    {
        private readonly GTRDBContext _context;
        private readonly GSRawDataDBContext _RDB;
        private readonly ILogger<RawDataImportController> _logger;
        private readonly IWebHostEnvironment _env;
        public RawDataImportController(GTRDBContext context, ILogger<RawDataImportController> logger,
            IWebHostEnvironment env, GSRawDataDBContext rDB)
        {
            _context = context;
            _logger = logger;
            _env = env;
            _RDB = rDB;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadFiles()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {

            if (file is null)
            {
                TempData["Message"] = "Please, Select a file!";
                TempData["Status"] = "3";
                return View();
            }
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            string ext = Path.GetExtension(file.FileName);
            if (ext == ".txt")
            {
                try
                {


                    if (file != null)
                    {
                        string fileLocation = Path.GetFullPath("wwwroot/Content/Upload/" + comid + "/" + userid);
                        if (Directory.Exists(fileLocation))
                        {
                            Directory.Delete(fileLocation, true);
                        }
                        string uploadlocation = Path.GetFullPath("wwwroot/Content/Upload/" + comid + "/" + userid + "/");

                        if (!Directory.Exists(uploadlocation))
                        {
                            Directory.CreateDirectory(uploadlocation);
                        }

                        string filePath = uploadlocation + Path.GetFileName(file.FileName);

                        string extension = Path.GetExtension(file.FileName);
                        var fileStream = new FileStream(filePath, FileMode.Create);
                        file.CopyTo(fileStream);
                        fileStream.Close();

                        string path = filePath;
                        string[] readText = System.IO.File.ReadAllLines(path);

                        //var DeviceNo = readText[0][1];
                        List<string> newList = readText.ToList();
                        List<Hr_RawData_Upload> hr_RawDataList = new List<Hr_RawData_Upload>();
                        var hr_emp = await _context.HR_Emp_Info
                            .Where(x => x.ComId == comid && x.FingerId != null && !x.IsDelete)
                            .Select(x => new { x.EmpId, x.FingerId }).ToListAsync();

                        var RawDays = _context.Companys
                            .Where(x => x.CompanyCode == comid && !x.IsDelete)
                            .Select(x => x.DecimalField)
                            .SingleOrDefault();

                        if (RawDays==0)
                        {
                            RawDays = 10;
                        }


                        //var traceid = Guid.NewGuid().ToString();
                        foreach (var item in newList)
                        {

                            var data = item.Split(':');
                            var deviceNo = data[0];
                            var cardNo = data[1];
                            var punchDate = data[2];
                            var punchTime = data[3] + ":" + data[4] + ":" + data[5];
                            if (DateTime.Parse(punchDate) > DateTime.Now.AddDays(-RawDays))
                            {
                                var empId = hr_emp.Where(x => Convert.ToInt32(x.FingerId) == Convert.ToInt32(cardNo)).Select(x => x.EmpId).FirstOrDefault();

                                Hr_RawData_Upload hr_RawData = new Hr_RawData_Upload
                                {
                                    EmpId = empId == 0 ? null : empId,
                                    DeviceNo = deviceNo,
                                    CardNo = cardNo,
                                    DtPunchDate = DateTime.Parse(punchDate),
                                    DtPunchTime = DateTime.Parse(punchTime),
                                    ComId = comid,
                                    //TrackId = traceid
                                };
                                hr_RawDataList.Add(hr_RawData);
                            }
                        }
                        #region OLD
                        //List<string> newList = readText.ToList();
                        //List<Hr_RawData> hr_RawDataList = new List<Hr_RawData>();
                        //foreach (var item in newList)
                        //{

                        //    var data = item.Split(':');
                        //    var deviceNo = data[0];
                        //    var cardNo = data[1];
                        //    var punchDate = data[2];
                        //    var punchTime = data[3] + ":" + data[4] + ":" + data[5];
                        //    Hr_RawData hr_RawData = new Hr_RawData
                        //    {

                        //        DeviceNo = deviceNo,
                        //        CardNo = cardNo,
                        //        DtPunchDate = DateTime.Parse(punchDate),
                        //        DtPunchTime = DateTime.Parse(punchTime),
                        //        ComId = comid
                        //    };

                        //    hr_RawDataList.Add(hr_RawData);
                        //}
                        // await _context.Hr_RawData.AddRangeAsync(hr_RawDataList);
                        // await _context.SaveChangesAsync();
                        #endregion OLD
                        int pageSize = 20000;
                        int pageNumber = 1;
                        int pageCount = (hr_RawDataList.Count + pageSize - 1) / pageSize;
                        //int total = 0;
                        for (int i = 0; i < pageCount; i++)
                        {
                            //paging the big data
                            var hr_RawDataListPaged = hr_RawDataList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            //Get the Information
                            var cardNosToRemove = hr_RawDataListPaged.Select(x => x.CardNo).ToList();
                            var deviceNosToRemove = hr_RawDataListPaged.Select(x => x.DeviceNo).ToList();
                            var datesToRemove = hr_RawDataListPaged.Select(x => x.DtPunchDate).ToList();
                            var dateTimesToRemove = hr_RawDataListPaged.Select(x => x.DtPunchTime).ToList();

                            var dataToRemove = _RDB.Hr_RawData_Upload
                                .Where(x => x.ComId == comid &&
                                            datesToRemove.Contains(x.DtPunchDate) &&
                                            dateTimesToRemove.Contains(x.DtPunchTime) &&
                                            deviceNosToRemove.Contains(x.DeviceNo) &&
                                            cardNosToRemove.Contains(x.CardNo))
                                .ToList();

                            if (dataToRemove.Any())
                            {
                                _RDB.Hr_RawData_Upload.RemoveRange(dataToRemove);
                                await _RDB.SaveChangesAsync();
                            }
                            await _RDB.Hr_RawData_Upload.AddRangeAsync(hr_RawDataListPaged);
                            pageNumber++;
                            // Console.WriteLine(total += hr_RawDataListPaged.Count);
                            await _RDB.SaveChangesAsync();
                        }
                        TempData["Message"] = "Data Import Successfully";
                        TempData["Status"] = "1";

                        //var datas = _context.HR_Emp_Info.Where(x => x.ComId == comid && cardNosToRemove.Contains(x.FingerId)).ToList();
                        //_context.Hr_RawData.FromSqlRaw
                    }

                    else
                    {
                        TempData["Message"] = "Please, Select a file!";
                        TempData["Status"] = "3";
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.InnerException.Message);

                }
            }
            else
            if (ext == ".mdb")
            {
                string FilePathUrl = preprocess(file, comid, userid);

                var dbEngine = new DBEngine();
                var db = dbEngine.OpenDatabase(@FilePathUrl);
                var queryDef = db.CreateQueryDef("", "SELECT * FROM CHECKINOUT");
                var recordset = db.OpenRecordset(queryDef.SQL);
                List<Hr_RawData> list = new List<Hr_RawData>();
                while (!recordset.EOF)
                {
                    Hr_RawData data = new Hr_RawData();

                    string input = recordset.Fields["CHECKTIME"].Value.ToString();
                    string[] substrings = input.Split(new char[] { ' ' }, 2);
                    DateTime dateTimeValue;
                    if (DateTime.TryParse(input, out dateTimeValue))
                    {
                        // The string was successfully parsed to a DateTime value
                        DateTime dateValue = dateTimeValue.Date;    // Extract the date component
                        TimeSpan timeValue = dateTimeValue.TimeOfDay;    // Extract the time component
                        DateTime dateValue1 = DateTime.Now; // May 15, 2022

                        DateTime x = dateValue1 + timeValue;
                        data.DtPunchDate = dateValue;
                        data.DtPunchTime = x;
                    }
                    else
                    {
                        // The string was not in a valid DateTime format
                        Console.WriteLine("Invalid DateTime format.");
                    }

                    data.CardNo = recordset.Fields["USERID"].Value.ToString();
                    data.DeviceNo = "1151";
                    data.ComId = comid;
                    list.Add(data);
                    _RDB.Hr_RawData.Add(data);
                    // Extract other columns as needed
                    recordset.MoveNext();
                }

                recordset.Close();
                db.Close();

                _RDB.SaveChanges();
            }

            return View();
        }

        private string preprocess(IFormFile file, string comid, string userid)
        {
            string FileName = null;
            string path = "";
            if (file != null)
            {
                //string folder = "book/Gallery/";
                string serverFolder = Path.GetFullPath("wwwroot/Content/Upload/" + comid + "/" + userid + "/");
                FileName = file.FileName;
                string filePath = Path.Combine(serverFolder, FileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }
                path += filePath;
            }
            return path;
        }
    }
}
