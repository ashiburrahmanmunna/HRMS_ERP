#region Namespace

using ExcelDataReader;
using GTERP.Interfaces;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTERP.BLL;

#endregion

namespace GTERP.Controllers.HR
{
    [OverridableAuthorize]

    public class FixedAttController : Controller
    {
        #region Common Property
        private readonly GTRDBContext db;
        private readonly TransactionLogRepository _tranlog;
        private readonly ISectionRepository _sectionRepository;
        private readonly IEmpInfoRepository _empInfoRepository;
       // private readonly ICatAttStatusRepository _catAttShiftNameRepository;
        private readonly ICatAttStatusRepository _catAttStatusRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IFixAttRepository _fixAttRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IShiftRepository _catAttShiftNameRepository;
        private readonly IEmpReleaseRepository _empReleaseRepository;
        private readonly IHttpContextAccessor _httpContext;

        #endregion

        #region Constructor
        public FixedAttController(
            GTRDBContext context,
            ISectionRepository sectionRepository,
            IEmpInfoRepository empInfoRepository,
            ICatAttStatusRepository catAttStatusRepository,
            IDepartmentRepository departmentRepository,
            ILineRepository lineRepository,
            IFixAttRepository fixAttRepository,
            IDesignationRepository designationRepository,
            IShiftRepository shiftRepository,
            IShiftRepository catAttShiftNameRepository,
            IEmpReleaseRepository empReleaseRepository,
          IHttpContextAccessor httpContext,
          TransactionLogRepository tranlog

            )
        {
            db = context;
            _httpContext=httpContext;
            _sectionRepository = sectionRepository;
            _empInfoRepository = empInfoRepository;
            _catAttStatusRepository = catAttStatusRepository;
            _departmentRepository = departmentRepository;
            _lineRepository = lineRepository;
            _fixAttRepository = fixAttRepository;
            _designationRepository = designationRepository;
            _shiftRepository = shiftRepository;
            _catAttShiftNameRepository = shiftRepository;
            _empReleaseRepository = empReleaseRepository;
            _tranlog = tranlog;

        }

        #endregion

        #region View Model
        public class AttFixGrid
        {
            public bool IsChecked { get; set; }
            public int EmpId { get; set; }
            public string EmpCode { get; set; }
            public string EmpName { get; set; }
            public string SectName { get; set; }
            public string DeptName { get; set; }
            public string DesigName { get; set; }
            public int ShiftId { get; set; }
            public string ShiftName { get; set; }

            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
            public DateTime dtPunchDate { get; set; } = DateTime.Now.Date;

            [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
            public string TimeIn { get; set; } = DateTime.Now.ToShortTimeString();
            [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
            public string TimeOut { get; set; } = DateTime.Now.ToShortTimeString();

            [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
            public string OTHourInTime { get; set; } = DateTime.Now.ToShortTimeString();

            public string Status { get; set; }
            public int StatusId { get; set; }
            public string? Roles { get; set; }
            public string? VandorName { get; set; }
            public string Remarks { get; set; }
            //[DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
            public string OtHour { get; set; }
            public float OT { get; set; }
            public string Line { get; set; }
            public bool IsInactive { get; set; }
            public int SectId { get; set; }
            public string Criteria { get; set; }

            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

            public DateTime DtFrom { get; set; }
            [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

            public DateTime DtTo { get; set; }

        }

        #endregion

        public ActionResult Index()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.EmpId = _empReleaseRepository.EmpList();
            ViewBag.Designation = _designationRepository.GetDesignationList();
            ViewBag.Departments = _departmentRepository.GetDepartmentList();
            ViewBag.Sections = _sectionRepository.GetSectionList();
            ViewBag.Lines = _lineRepository.GetLineList();
            ViewBag.ShiftName = _shiftRepository.GetAttShiftNameList();
            ViewBag.StatusId = _catAttStatusRepository.GetAttStatusList();
            ViewBag.ShiftId = _shiftRepository.GetShiftList();
            return View("MyView");
        }


        public ActionResult FixedAttendB()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.EmpId = _empReleaseRepository.EmpList();
            ViewBag.Designation = _designationRepository.GetDesignationList();
            ViewBag.Departments = _departmentRepository.GetDepartmentList();
            ViewBag.Sections = _sectionRepository.GetSectionList();
            ViewBag.Lines = _lineRepository.GetLineList();
            ViewBag.ShiftName = _shiftRepository.GetAttShiftNameList();
            ViewBag.StatusId = _catAttStatusRepository.GetAttStatusList();
            ViewBag.ShiftId = _shiftRepository.GetShiftList();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Index(AttFixGrid fixAttendance)
        public IActionResult FixedAttendB(string DtFrom, string DtTo, string criteria, string value)
        {
            try
            {
                var listOfAttFixed = _fixAttRepository.FixAttendanceListB(DtFrom, DtTo, criteria, value);
                TempData["Message"] = "Data Load Successfully";
                return Json(new { Success = 1, Result = listOfAttFixed, message = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });

            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Index(AttFixGrid fixAttendance)
        public IActionResult Index(string DtFrom, string DtTo, string criteria, string value)
        {
            try
            {
                var listOfAttFixed = _fixAttRepository.FixAttendanceList(DtFrom, DtTo, criteria, value);
                TempData["Message"] = "Data Load Successfully";
                return Json(new { Success = 1, Result = listOfAttFixed, message = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });

            }
        }

        public ActionResult IndexForUBL()
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.EmpId = _empReleaseRepository.EmpList();
            ViewBag.Designation = _designationRepository.GetDesignationList();
            ViewBag.Departments = _departmentRepository.GetDepartmentList();
            ViewBag.Sections = _sectionRepository.GetSectionList();
            ViewBag.Lines = _lineRepository.GetLineList();
            ViewBag.ShiftName = _shiftRepository.GetAttShiftNameList();
            ViewBag.StatusId = _catAttStatusRepository.GetAttStatusList();
            ViewBag.ShiftId = _shiftRepository.GetShiftList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Index(AttFixGrid fixAttendance)
        public IActionResult IndexForUBL(string DtFrom, string DtTo, string criteria, string value)
        {
            try
            {
                var listOfAttFixed = _fixAttRepository.FixAttendanceListUBL(DtFrom, DtTo, criteria, value);
                TempData["Message"] = "Data Load Successfully";
                return Json(new { Success = 1, Result = listOfAttFixed, message = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });

            }
        }




        #region Commented Code
        //public List<AttFixGrid> PopulateGrid(AttFixGrid fixAttendance)
        //{
        //    string optCritaria = fixAttendance.Criteria;
        //    string valueLine = "0";
        //    string valueSec = "0";
        //    string valueStatus = "=ALL=";
        //    string valueShift = "0";
        //    string value = "";
        //    switch (optCritaria)
        //    {
        //        case "EmpID":
        //            value = fixAttendance.EmpId.ToString();
        //            break;
        //        case "Sec":
        //            value = fixAttendance.SectId.ToString();
        //            break;
        //        case "ShiftTime":
        //            value = fixAttendance.ShiftId.ToString();
        //            break;
        //        case "Status":
        //            value = fixAttendance.Status;
        //            break;
        //        case "Line":
        //            value = fixAttendance.Line;
        //            break;
        //    }
        //    if (fixAttendance.Line != null)
        //    {
        //        valueLine = fixAttendance.Line;
        //    }
        //    if (fixAttendance.Status != null)
        //    {
        //        valueStatus = fixAttendance.Status;
        //    }
        //    if (fixAttendance.SectId != 0)
        //    {
        //        valueSec = fixAttendance.SectId.ToString();
        //    }
        //    if (fixAttendance.ShiftId != 0)
        //    {
        //        valueShift = fixAttendance.ShiftId.ToString();
        //    }

        //    DateTime dtFrom = fixAttendance.DtFrom;
        //    DateTime dtTo = fixAttendance.DtTo;

        //    List<AttFixGrid> list = new List<AttFixGrid>();

        //    try
        //    {
        //        var comid = HttpContext.Session.GetString("comid");

        //        SqlParameter[] sqlParameter = new SqlParameter[10];
        //        sqlParameter[0] = new SqlParameter("@Id", "1");
        //        sqlParameter[1] = new SqlParameter("@ComId", comid);
        //        sqlParameter[2] = new SqlParameter("@OptCriteria", optCritaria);
        //        sqlParameter[3] = new SqlParameter("@Value", value);
        //        sqlParameter[4] = new SqlParameter("@dtfrom", dtFrom);
        //        sqlParameter[5] = new SqlParameter("@dtTo", dtTo);
        //        sqlParameter[6] = new SqlParameter("@Line", valueLine);
        //        sqlParameter[7] = new SqlParameter("@SectId", valueSec);
        //        sqlParameter[8] = new SqlParameter("@Status", valueStatus);
        //        sqlParameter[9] = new SqlParameter("@ShiftId", valueShift);
        //        List<AttFixGrid> listOfAttFixed = Helper.ExecProcMapTList<AttFixGrid>("HR_PrcGetFixAtt", sqlParameter);

        //        return listOfAttFixed;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        //clsCon = null;
        //    }
        //}

        //private List<AttFixGrid> GetAllData()
        //{


        //    try
        //    {

        //        var comid = HttpContext.Session.GetString("comid");

        //        SqlParameter[] sqlParameter = new SqlParameter[9];
        //        sqlParameter[0] = new SqlParameter("@Id", "1");
        //        sqlParameter[1] = new SqlParameter("@ComId", comid);
        //        sqlParameter[2] = new SqlParameter("@OptCriteria", "All");
        //        sqlParameter[3] = new SqlParameter("@dtfrom", "01-jan-20");
        //        sqlParameter[4] = new SqlParameter("@dtTo", "01-june-20");
        //        sqlParameter[6] = new SqlParameter("@SectId", "");
        //        sqlParameter[7] = new SqlParameter("@Status", "");
        //        sqlParameter[8] = new SqlParameter("@ShiftId", "");
        //        List<AttFixGrid> listOfAttFixed = Helper.ExecProcMapTList<AttFixGrid>("HR_PrcGetFixAtt", sqlParameter);


        //        return listOfAttFixed;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSelectedData(string GridDataList)
        {
            if (GridDataList != null)
            {
                _fixAttRepository.UpdateSelectedData(GridDataList);
                return Json(new { Success = 1, message = "Data Updated Successfully" });
            }
            return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSelectedDataB(string GridDataList)
        {
            if (GridDataList != null)
            {
                _fixAttRepository.UpdateSelectedDataB(GridDataList);
                return Json(new { Success = 1, message = "Data Updated Successfully" });
            }
            return null;
        }
        public JsonResult ShiftNamelistFromMVC()
        {
            var JObject = new JObject();
            var GridDataList = _shiftRepository.GetAttShiftNameList();
            var output = JsonConvert.SerializeObject(GridDataList);

            return Json(output);
        }

        public JsonResult StatusListFromMVC()
        {
            var JObject = new JObject();
            var GridDataList = _catAttStatusRepository.GetAttStatusList();
            var output = JsonConvert.SerializeObject(GridDataList);

            return Json(output);
        }
        public async Task<IActionResult> FixedAttUploadFileUBL(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetFixedAttUBL(file.FileName);
                    if (addition.Count() > 0)
                    {
                        await db.HR_TempFixAttExcel.AddRangeAsync(addition);
                        await db.SaveChangesAsync();

                        var Query = $"Exec HR_PrcGetFixAttnExl_ubl '{comid}'";
                        SqlParameter[] sqlParameter = new SqlParameter[1];
                        sqlParameter[0] = new SqlParameter("@ComId", comid);
                        Helper.ExecProc("HR_PrcGetFixAttnExl_ubl", sqlParameter);

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
            return RedirectToAction("IndexForUBL");
        }


        //private List<HR_TempFixAttExcel> GetFixedAttUBL(string fName)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    var filename = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid + "/" + fName);

        //    List<HR_TempFixAttExcel> fixedAtts = new List<HR_TempFixAttExcel>();

        //    try
        //    {
        //        using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
        //        using (var reader = ExcelReaderFactory.CreateReader(stream))
        //        {
        //            var startSl = 0;
        //            while (reader != null && reader.Read())
        //            {
        //                startSl++;
        //                if (startSl == 1)
        //                {
        //                    continue;
        //                }

        //                var empIdString = reader.GetValue(0)?.ToString();
        //                var empCode = reader.GetValue(1)?.ToString();
        //                var shiftName = reader.GetValue(4)?.ToString();
        //                var comId = HttpContext.Session.GetString("comid");
        //                var dtPunchDate = reader.GetValue(5)?.ToString();
        //                var timeInString = reader.GetValue(6)?.ToString();
        //                var timeOutString = reader.GetValue(7)?.ToString();
        //                var otHour = reader.GetValue(8)?.ToString();
        //                var status = reader.GetValue(9)?.ToString();
        //                var remarks = reader.GetValue(10)?.ToString();

        //                if (int.TryParse(empIdString, out int empId) &&
        //                    DateTime.TryParse(timeInString, out DateTime timeIn) &&
        //                    DateTime.TryParse(timeOutString, out DateTime timeOut) &&
        //                    int.TryParse(otHour, out int otHours))
        //                {
        //                    fixedAtts.Add(new HR_TempFixAttExcel()
        //                    {
        //                        EmpId = empId,
        //                        EmpCode = empCode,
        //                        ShiftName = shiftName,
        //                        ComId = comId,
        //                        DtPunchDate = dtPunchDate,
        //                        TimeIn = timeIn.ToString("HH:mm:ss"),
        //                        TimeOut = timeOut.ToString("HH:mm:ss"),
        //                        //OT = otHours,
        //                        OT = float.TryParse(reader.GetValue(8)?.ToString(), out float otValue) ? otValue : 0.0f,
        //                        Status = status,
        //                        Remarks = remarks
        //                    });
        //                }
        //                else
        //                {
        //                    // Handle parsing errors or log the problematic data
        //                    Console.WriteLine($"Error parsing data at row {startSl}");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        // Handle or log the exception
        //        Console.WriteLine($"An error occurred: {e.Message}");
        //    }

        //    return fixedAtts;
        //}





        //new add
        private List<HR_TempFixAttExcel> GetFixedAttUBL(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid + "/" + fName);

            List<HR_TempFixAttExcel> fixedAtts = new List<HR_TempFixAttExcel>();

            try
            {
                using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var startSl = 0;
                        while (reader != null && reader.Read())
                        {
                            startSl++;
                            if (startSl == 1)
                            {
                                continue;
                            }
                            else
                            {
                                // Check if all values in the row are null
                                bool allValuesNull = true;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (reader.GetValue(i) != null)
                                    {
                                        allValuesNull = false;
                                        break;
                                    }
                                }

                                if (!allValuesNull)
                                {
                                    fixedAtts.Add(new HR_TempFixAttExcel()
                                    {
                                        EmpId = int.TryParse(reader.GetValue(0)?.ToString(), out int empId) ? empId : 0,
                                        EmpCode = reader.GetValue(1)?.ToString(),
                                        ShiftName = reader.GetValue(4)?.ToString(),
                                        ComId = HttpContext.Session.GetString("comid"),
                                        DtPunchDate = reader.GetValue(5)?.ToString(),
                                        TimeIn = DateTime.Parse(reader.GetValue(6)?.ToString()).ToString("HH:mm:ss"),
                                        TimeOut = DateTime.Parse(reader.GetValue(7)?.ToString()).ToString("HH:mm:ss"),
                                        OtHour = reader.GetValue(8)?.ToString(),
                                        OT = float.TryParse(reader.GetValue(8)?.ToString(), out float otValue) ? otValue : 0.0f,
                                        Status = reader.GetValue(9)?.ToString(),
                                        Remarks = reader.GetValue(10)?.ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return fixedAtts;
        }











        //private List<HR_TempFixAttExcel> GetFixedAttUBL(string fName)
        //{
        //    var comid = HttpContext.Session.GetString("comid");
        //    var userid = HttpContext.Session.GetString("userid");
        //    var filename = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid + "/" + fName);

        //    List<HR_TempFixAttExcel> fixedAtts = new List<HR_TempFixAttExcel>();

        //    try
        //    {

        //        using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
        //        {
        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //            {
        //                var startSl = 0;
        //                while (reader != null && reader.Read())
        //                {
        //                    startSl++;
        //                    if (startSl == 1)
        //                    {
        //                        continue;
        //                    }
        //                    else
        //                    {

        //                        fixedAtts.Add(new HR_TempFixAttExcel()
        //                        {
        //                            EmpId = int.Parse(reader.GetValue(0)?.ToString()),
        //                            EmpCode = reader.GetValue(1)?.ToString(),
        //                            ShiftName = reader.GetValue(4)?.ToString(),
        //                            ComId = HttpContext.Session.GetString("comid"),
        //                            DtPunchDate = reader.GetValue(5)?.ToString(),
        //                            TimeIn = DateTime.Parse(reader.GetValue(6)?.ToString()).ToString("HH:mm:ss"),
        //                            TimeOut = DateTime.Parse(reader.GetValue(7)?.ToString()).ToString("HH:mm:ss"),
        //                            OtHour = reader.GetValue(8)?.ToString(),
        //                            OT = float.TryParse(reader.GetValue(8)?.ToString(), out float otValue) ? otValue : 0.0f,

        //                            // OT = int.Parse(reader.GetValue(8)?.ToString()),
        //                            Status = reader.GetValue(9)?.ToString(),
        //                            Remarks = reader.GetValue(10)?.ToString()
        //                        });




        //                        //fixedAtts.Add(new HR_TempFixAttExcel()
        //                        //{
        //                        //    EmpId = int.Parse(reader.GetValue(0)?.ToString()),
        //                        //    EmpCode = reader.GetValue(1)?.ToString(),
        //                        //    ShiftName = reader.GetValue(4)?.ToString(),
        //                        //    ComId = HttpContext.Session.GetString("comid"),
        //                        //    DtPunchDate = reader.GetValue(5)?.ToString(),
        //                        //    TimeIn = reader.GetValue(6)?.ToString(),
        //                        //    TimeOut = reader.GetValue(7)?.ToString(),
        //                        //    OtHour = reader.GetValue(8)?.ToString(),
        //                        //    OT = int.Parse(reader.GetValue(8)?.ToString()),
        //                        //    Status = reader.GetValue(9)?.ToString(),
        //                        //    Remarks = reader.GetValue(10)?.ToString()
        //                        //});
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //        //_logger.LogError(e.Message);
        //    }

        //    return fixedAtts;
        //}

        public async Task<IActionResult> FixedAttUploadFile(IFormFile file)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            try
            {

                if (file != null)
                {
                    string fileLocation = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid);
                    if (Directory.Exists(fileLocation))
                    {
                        Directory.Delete(fileLocation, true);
                    }
                    string uploadlocation = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid + "/");

                    if (!Directory.Exists(uploadlocation))
                    {
                        Directory.CreateDirectory(uploadlocation);
                    }

                    string filePath = uploadlocation + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Close();

                    var addition = this.GetFixedAtt(file.FileName);
                    if (addition.Count() > 0)
                    {
                        await db.HR_TempFixAttExcel.AddRangeAsync(addition);
                        await db.SaveChangesAsync();

                        var Query = $"Exec HR_PrcGetFixAttnExl '{comid}'";
                        SqlParameter[] sqlParameter = new SqlParameter[1];
                        sqlParameter[0] = new SqlParameter("@ComId", comid);
                        Helper.ExecProc("HR_PrcGetFixAttnExl", sqlParameter);

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


        private List<HR_TempFixAttExcel> GetFixedAtt(string fName)
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/FixedAtt/" + comid + "/" + userid + "/" + fName);

            List<HR_TempFixAttExcel> fixedAtts = new List<HR_TempFixAttExcel>();

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
                                fixedAtts.Add(new HR_TempFixAttExcel()
                                {
                                    EmpId = int.Parse(reader.GetValue(0).ToString()),
                                    EmpCode = reader.GetValue(1).ToString(),
                                    ShiftName = reader.GetValue(4).ToString(),
                                    ComId = HttpContext.Session.GetString("comid"),
                                    DtPunchDate = reader.GetValue(5).ToString(),
                                    TimeIn = reader.GetValue(6).ToString(),
                                    TimeOut = reader.GetValue(7).ToString(),
                                    Status = reader.GetValue(9).ToString()
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
                //_logger.LogError(e.Message);
            }

            return fixedAtts;
        }




        #region VendorWiseDailyEntry
        public IActionResult VendorWiseEntry()
        {
            ViewBag.EmpId = _empReleaseRepository.EmpListWithLessInfo();
            ViewBag.StatusId = _catAttStatusRepository.GetAttStatusList();
            ViewBag.Title = "Create";
            return View();
        }
        [HttpPost]
        public IActionResult VendorWiseEntryCreate(HR_AttFixedVM hrAttFixed)
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            try
            {
                var data = db.HR_AttFixed.Where(w => w.EmpId == hrAttFixed.EmpId && w.DtPunchDate == hrAttFixed.DtPunchDate && w.ComId == ComId).Select(s => s).FirstOrDefault();

                if (data != null)
                {
                    TempData["Message"] = "data updated";
                     _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), userid, "Updated", DateTime.Now.ToString());
                           
                    data.TimeIn = hrAttFixed.TimeIn;
                    data.TimeOut = hrAttFixed.TimeOut;
                    data.StatusId = hrAttFixed.StatusId;
                    data.Remarks = hrAttFixed.Remarks;
                    db.HR_AttFixed.Update(data);
                    db.SaveChanges();
                    return Json("Data Updated");
                }
                else
                {
                    TempData["Message"] = "data Saved";
                    _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), userid, "Saved",DateTime.Now.ToString());


                    var fixedAtts = new HR_AttFixed();
                    fixedAtts.ComId = ComId;
                    fixedAtts.EmpId = hrAttFixed.EmpId;
                    fixedAtts.DtPunchDate = hrAttFixed.DtPunchDate;
                    fixedAtts.TimeIn = hrAttFixed.TimeIn;
                    fixedAtts.TimeOut = hrAttFixed.TimeOut;
                    fixedAtts.StatusId = hrAttFixed.StatusId;
                    fixedAtts.Remarks = hrAttFixed.Remarks;

                    db.HR_AttFixed.Add(fixedAtts);
                    db.SaveChanges();
                    return Json("Data Saved");
                }
            }
            catch (Exception ex) {

                return Json(ex);
            }
        
        }
        [HttpPost]
        public IActionResult VendorWiseEntryCheck(int id, DateTime dateTime)
        {


            try
            {
                var ComId = _httpContext.HttpContext.Session.GetString("comid");
                var data = db.HR_AttFixed.Where(w => w.EmpId == id && w.DtPunchDate == dateTime && w.ComId==ComId).Select(s => s).FirstOrDefault();
                if (data!=null)
                {
                    return Json(data);
                }
                else {
                    return Json("No record Found");
                }
          
            }
            catch (Exception ex)
            {

                return Json(ex);
            }

        }

        [HttpPost]
        public IActionResult VendorWiseEntryDlt(int id, DateTime dateTime)
        {
            var ComId = _httpContext.HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            try
            {

                TempData["Message"] = "data deleted";
                _tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), userid, "Updated", DateTime.Now.ToString());

                var data = db.HR_AttFixed.Where(w => w.EmpId == id && w.DtPunchDate == dateTime && w.ComId == ComId).Select(s => s).FirstOrDefault();
                if (data != null)
                {
                    db.HR_AttFixed.Remove(data);
                    db.SaveChanges();
                    return Json("Data Successfully Deleted");
                }
                else
                {
                    return Json("No record Found");
                }

            }
            catch (Exception ex)
            {

                return Json(ex);
            }

        }
        #endregion
    }
}
