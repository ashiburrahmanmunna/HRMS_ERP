using GTERP.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.OleDb;
using Microsoft.Extensions.Logging;
using GTERP.Models;
using ExcelDataReader;

namespace GTERP.Controllers
{
    public class InsertEmpInfoByExcel : Controller
    {
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;
        private readonly ILogger<InsertEmpInfoByExcel> _logger;
        private readonly GTRDBContext db;
        public InsertEmpInfoByExcel(
            IWebHostEnvironment _environment,
            IConfiguration _configuration,
            ILogger<InsertEmpInfoByExcel> logger,
            GTRDBContext _db
            )
        {
            Environment = _environment;
            Configuration = _configuration;
            _logger = logger;
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/CompanyData/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/CompanyData/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var data = this.GetCompanyData(file.FileName);
                    if (data.Count() > 0)
                    {
                        await db.TempEmpData.AddRangeAsync(data);
                        await db.SaveChangesAsync();

                        // for import database data from temporary table

                        SqlParameter[] sqlParameter1 = new SqlParameter[2];
                        sqlParameter1[0] = new SqlParameter("@ComId", comid);
                        sqlParameter1[1] = new SqlParameter("@InputType", "Insert");

                        string query = $"Exec HR_prcProcessEmpInfoExcel '{comid}','Insert'";
                        Helper.ExecProc("HR_prcProcessEmpInfoExcel", sqlParameter1);

                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Upload
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Update(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/CompanyData/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/CompanyData/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var data = this.GetCompanyDataForUpdate(file.FileName);
                    if (data.Count() > 0)
                    {
                        await db.TempEmpData.AddRangeAsync(data);
                        await db.SaveChangesAsync();

                        // for import database data from temporary table

                        SqlParameter[] sqlParameter1 = new SqlParameter[2];
                        sqlParameter1[0] = new SqlParameter("@ComId", comid);
                        sqlParameter1[1] = new SqlParameter("@InputType", "Update");

                        string query = $"Exec HR_prcProcessEmpInfoExcel '{comid}','Update'";
                        Helper.ExecProc("HR_prcProcessEmpInfoExcel", sqlParameter1);

                        TempData["Message"] = "Data Upload Successfully";
                        TempData["Status"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "Something is wrong!";
                        TempData["Status"] = "3";
                    }
                }
                else
                {
                    TempData["Message"] = "Please, Select a excel file!";
                }


            }
            catch (Exception e)
            {
                throw e;
                ViewData["Message"] = "Error Occured";
            }
            //Process();
            return RedirectToAction("Index");
        }



        private List<TempEmpData> GetCompanyDataForUpdate (string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/CompanyData/" + comid + "/" + userid + "/" + fName);

            List<TempEmpData> empdata = new List<TempEmpData>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                               
                                empdata.Add(new TempEmpData()
                                {

                                    EmpCode = reader.GetValue(1) == null ? "N/A" : reader.GetValue(1).ToString(),
                                    EmpName = reader.GetValue(2) == null ? "N/A" : reader.GetValue(2).ToString(),
                                    Department = reader.GetValue(3) == null ? "N/A" : reader.GetValue(3).ToString(),
                                    Section = reader.GetValue(4) == null ? "N/A" : reader.GetValue(4).ToString(),
                                    Designation = reader.GetValue(5) == null ? "N/A" : reader.GetValue(5).ToString(),
                                    PayMode = reader.GetValue(6) == null ? "N/A" : reader.GetValue(6).ToString(),
                                    BankAccountNo = reader.GetValue(7) == null ? "N/A" : reader.GetValue(7).ToString(),
                                    GS = reader.GetValue(8) == null ? 0 : float.Parse(reader.GetValue(8).ToString()),
                                    BS = reader.GetValue(9) == null ? 0 : float.Parse(reader.GetValue(9).ToString()),
                                    HR = reader.GetValue(10) == null ? 0 : float.Parse(reader.GetValue(10).ToString()),
                                    MA = reader.GetValue(11) == null ? 0 : float.Parse(reader.GetValue(11).ToString()),
                                    TA = reader.GetValue(12) == null ? 0 : float.Parse(reader.GetValue(12).ToString()),
                                    FA = reader.GetValue(13) == null ? 0 : float.Parse(reader.GetValue(13).ToString()),
                                    Line = reader.GetValue(14) == null ? "N/A" : reader.GetValue(14).ToString(),
                                    Floor = reader.GetValue(15) == null ? "N/A" : reader.GetValue(15).ToString(),
                                    SpouseName = reader.GetValue(16) == null ? "N/A" : reader.GetValue(16).ToString(),
                                    SpouseContactNo = reader.GetValue(17) == null ? "N/A" : reader.GetValue(17).ToString(),
                                    ChildNo = reader.GetValue(18) == null ? "N" : reader.GetValue(18).ToString(),
                                    FatherName = reader.GetValue(19) == null ? "N/A" : reader.GetValue(19).ToString(),
                                    MotherName = reader.GetValue(20) == null ? "N/A" : reader.GetValue(20).ToString(),
                                    EmergencyContactNo = reader.GetValue(21) == null ? "N/A" : reader.GetValue(21).ToString(),
                                    EducationQualification = reader.GetValue(22) == null ? "N/A" : reader.GetValue(22).ToString(),
                                    BloodGroup = reader.GetValue(23) == null ? "N/A" : reader.GetValue(23).ToString(),
                                    NomineeName = reader.GetValue(24) == null ? "N/A" : reader.GetValue(24).ToString(),
                                    NomineeAddress = reader.GetValue(25) == null ? "N/A" : reader.GetValue(25).ToString(),
                                    NomineeMobileNo = reader.GetValue(26) == null ? "N/A" : reader.GetValue(26).ToString(),
                                    RelationWithNominee = reader.GetValue(27) == null ? "N/A" : reader.GetValue(27).ToString(),
                                    PresentDistrict = reader.GetValue(28) == null ? "N/A" : reader.GetValue(28).ToString(),
                                    PresentPS = reader.GetValue(29) == null ? "N/A" : reader.GetValue(29).ToString(),
                                    PresentPO = reader.GetValue(30) == null ? "N/A" : reader.GetValue(30).ToString(),
                                    PresentVill = reader.GetValue(31) == null ? "N/A" : reader.GetValue(31).ToString(),
                                    PresentVillB = reader.GetValue(32) == null ? "N/A" : reader.GetValue(32).ToString(),
                                    PerDistrict = reader.GetValue(33) == null ? "N/A" : reader.GetValue(33).ToString(),
                                    PerPS = reader.GetValue(34) == null ? "N/A" : reader.GetValue(34).ToString(),
                                    PerPO = reader.GetValue(35) == null ? "N/A" : reader.GetValue(35).ToString(),
                                    PerVill = reader.GetValue(36) == null ? "N/A" : reader.GetValue(36).ToString(),
                                    PerVillB = reader.GetValue(37) == null ? "N/A" : reader.GetValue(37).ToString(),
                                    EmpNameB = reader.GetValue(38) == null ? "N/A" : reader.GetValue(38).ToString(),
                                    FatherNameB = reader.GetValue(39) == null ? "N/A" : reader.GetValue(39).ToString(),
                                    MotherNameB = reader.GetValue(40) == null ? "N/A" : reader.GetValue(40).ToString(),
                                    SpouseNameB = reader.GetValue(41) == null ? "N/A" : reader.GetValue(41).ToString(),
                                    MaritalStatus = reader.GetValue(42) == null ? "N/A" : reader.GetValue(42).ToString(),
                                    Grade = reader.GetValue(43) == null ? "N/A" : reader.GetValue(43).ToString(),
                                    Unit = reader.GetValue(44) == null ? "N/A" : reader.GetValue(44).ToString(),
                                    ComId = comid,

                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                _logger.LogError(e.InnerException.Message);
            }

            return empdata;
        }

        private List<TempEmpData> GetCompanyData(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/CompanyData/" + comid + "/" + userid + "/" + fName);

            List<TempEmpData> empdata = new List<TempEmpData>();

            try
            {

                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                empdata.Add(new TempEmpData()
                                {
                                    EmpCode = reader.GetValue(1)  == null ? "N/A" : reader.GetValue(1).ToString(),
                                    EmpName = reader.GetValue(2)  == null ? "N/A" : reader.GetValue(2).ToString(),
                                    EmpNameB = reader.GetValue(3)  == null ? "N/A" : reader.GetValue(3).ToString(),
                                    CardNo = reader.GetValue(4)  == null ? "N/A" : reader.GetValue(4).ToString(),
                                    Department = reader.GetValue(5) == null ? "N/A" : reader.GetValue(5).ToString(), 
                                    DepartmentB = reader.GetValue(6) == null ? "N/A" : reader.GetValue(6).ToString(),
                                    Section = reader.GetValue(7) == null ? "N/A" : reader.GetValue(7).ToString(),
                                    SectionB = reader.GetValue(8) == null ? "N/A" : reader.GetValue(8).ToString(),
                                    Designation = reader.GetValue(9) == null ? "N/A" : reader.GetValue(9).ToString(), 
                                    DesignationB = reader.GetValue(10) == null ? "N/A" : reader.GetValue(10).ToString(), 
                                    EmpType = reader.GetValue(11) == null ? "N/A" : reader.GetValue(11).ToString(),
                                    TinNo = reader.GetValue(12) == null ? "N/A" : reader.GetValue(12).ToString(),
                                    NID = reader.GetValue(13) == null ? "N/A" : reader.GetValue(13).ToString(), 
                                    Mobile = reader.GetValue(14) == null ? "N/A" : reader.GetValue(14).ToString(),
                                    PayMode = reader.GetValue(15)  == null ? "N/A" : reader.GetValue(15).ToString(),
                                    BankAccountNo = reader.GetValue(16)  == null ? "N/A" : reader.GetValue(16).ToString(),
                                    Shift = reader.GetValue(17) == null ? "N/A" : reader.GetValue(17).ToString(),
                                    GS = reader.GetValue(18) == null ? 0 : float.Parse(reader.GetValue(18).ToString()),
                                    BS = reader.GetValue(19) == null ? 0 : float.Parse(reader.GetValue(19).ToString()),
                                    HR = reader.GetValue(20) == null ? 0 : float.Parse(reader.GetValue(20).ToString()),
                                    MA = reader.GetValue(21) == null ? 0 : float.Parse(reader.GetValue(21).ToString()),
                                    TA = reader.GetValue(22) == null ? 0 : float.Parse(reader.GetValue(22).ToString()),
                                    FA = reader.GetValue(23) == null ? 0 : float.Parse(reader.GetValue(23).ToString()),
                                    Sex = reader.GetValue(24) == null ? "N/A" : reader.GetValue(24).ToString(), 
                                    Religion = reader.GetValue(25) == null ? "N/A" : reader.GetValue(25).ToString(), 
                                    JoinDate = DateTime.Parse(reader.GetValue(26).ToString()),
                                    BirthDate = DateTime.Parse(reader.GetValue(27).ToString()),
                                    Line = reader.GetValue(28)  == null ? "N/A" : reader.GetValue(28).ToString(),
                                    Floor = reader.GetValue(29)  == null ? "N/A" : reader.GetValue(29).ToString(),
                                    ComId = comid,


                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                _logger.LogError(e.InnerException.Message);
            }

            return empdata;
        }

        public ActionResult Download(string file)
        {

            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return response;
        }
    }
}

