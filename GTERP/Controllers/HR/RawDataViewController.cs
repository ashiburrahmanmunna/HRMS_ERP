using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.HR;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Management;
using ZKTeco;
using ZXing;

namespace GTERP.Controllers.HR
{
    public class RawDataViewController : Controller
    {

        GTRDBContext db;
        private readonly GSRawDataDBContext _RDB;
        private readonly RawDataViewRepository _repository;
        public RawDataViewController(GTRDBContext context, RawDataViewRepository repository, GSRawDataDBContext rDB)
        {
            db = context;
            _repository = repository;
            _RDB = rDB;
        }


        public Image _currentBitmap;
        string _FileName = "";
        string _path = "";
        string fileName = null;
        string Extension = null;

        public IActionResult Index(DateTime? From, DateTime? To, string Emp, string act = "")
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            string companyrole = HttpContext.Session.GetString("companyRole");

            ViewBag.Role = companyrole;

            return View();


        }
        public IActionResult Modern()
        {

            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");
            string companyrole = HttpContext.Session.GetString("companyRole");

            ViewBag.Role = companyrole;

            return View();


        }

        [AllowAnonymous]
        public JsonResult GetPunchData(int pageIndex, int pageSize, string From, string To, int Emp, string searchquery = "")
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                

                int Offset = (pageIndex - 1) * pageSize;
                int fetch = Offset + pageSize;
                string dateString = "1950-01-01 00:00:00.0000000";
                DateTime startDate = DateTime.Parse(dateString);
                DateTime endDate = DateTime.Parse(dateString);
                DateTime dateValue1;
                DateTime dateValue2;
                DateTime today = DateTime.Today;

                string formattedDate = today.ToString("d-MMM-yyyy");
                if(From == null)
                {
                    From = formattedDate;
                    To = formattedDate; //test1
                }

                if (DateTime.TryParseExact(From, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue1))
                {
                    startDate = DateTime.ParseExact(dateValue1.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }
                if (DateTime.TryParseExact(To, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue2))
                {
                    endDate = DateTime.ParseExact(dateValue2.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }

                JObject jsonObject = JObject.Parse(searchquery);

                string SearchColumns = "";
                string SearchKeywords = "";

                foreach (JProperty property in jsonObject.Properties())
                {
                    string columnName = property.Name;
                    string value = property.Value.ToString();
                    if (columnName == "Mobile")
                    {
                        columnName = "EmpPhone1";
                    }
                    if (columnName == "PunchLocation")
                    {
                        columnName = "LocationName";
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        SearchColumns += columnName + ",";
                        SearchKeywords += value + ",";
                    }
                }

                string toremove = pageIndex.ToString() + "," + pageSize.ToString() + ",";
                string toremove1 = "pageIndex,pageSize,";
                int lastIndex = SearchKeywords.LastIndexOf(toremove);
                int lastIndex1 = SearchColumns.LastIndexOf(toremove1);
                if (lastIndex1 == -1)
                {
                    lastIndex1 = 0;
                }
                if (lastIndex == -1)
                {
                    lastIndex = 0;
                }
                SearchKeywords = SearchKeywords.Substring(0, lastIndex) + SearchKeywords.Substring(lastIndex + toremove.Length);
                SearchColumns = SearchColumns.Substring(0, lastIndex1) + SearchColumns.Substring(lastIndex1 + toremove1.Length);

                if (Emp == null)
                {
                    Emp = 0;
                }
                SqlParameter p1 = new SqlParameter("@dtFrom", startDate);
                SqlParameter p2 = new SqlParameter("@dtTo", endDate);
                SqlParameter p3 = new SqlParameter("@ComId", comid);
                SqlParameter p4 = new SqlParameter("@EmpId", Emp);
                SqlParameter p5 = new SqlParameter("@PageSize", pageSize);
                SqlParameter p6 = new SqlParameter("@PageIndex", pageIndex);
                SqlParameter p7 = new SqlParameter("@SearchKeywords", SearchKeywords);
                SqlParameter p8 = new SqlParameter("@SearchColumns", SearchColumns);
                SqlParameter p9 = new SqlParameter("@Criteria", '0');
                //SqlParameter p10 = new SqlParameter("@Criteria", '1');
                string query = $"Exec Hr_prcGetRawData '{startDate}', '{endDate}', " +
                        $"'{comid}','{Emp}','{pageSize}','{pageIndex}','{SearchKeywords}','{SearchColumns}','0'";

                var employeelist = Helper.ExecProcMapTList<RawDataVM>("dbo.Hr_prcGetRawData", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 });

                SqlParameter pr1 = new SqlParameter("@dtFrom", startDate);
                SqlParameter pr2 = new SqlParameter("@dtTo", endDate);
                SqlParameter pr3 = new SqlParameter("@ComId", comid);
                SqlParameter pr4 = new SqlParameter("@EmpId", Emp);
                SqlParameter pr5 = new SqlParameter("@PageSize", pageSize);
                SqlParameter pr6 = new SqlParameter("@PageIndex", pageIndex);
                SqlParameter pr7 = new SqlParameter("@SearchKeywords", SearchKeywords);
                SqlParameter pr8 = new SqlParameter("@SearchColumns", SearchColumns);
                SqlParameter pr9 = new SqlParameter("@Criteria", '1');

                string query1 = $"Exec Hr_prcGetRawData '{startDate}', '{endDate}', " +
                    $"'{comid}','{Emp}','{pageSize}','{pageIndex}','{SearchKeywords}','{SearchColumns}','1'";

                var totalRow = Helper.ExecProcMapTList<TotalCount>("dbo.Hr_prcGetRawData", new SqlParameter[] { pr1, pr2, pr3, pr4, pr5, pr6, pr7, pr8, pr9 });
                decimal TotalRecordCount = totalRow[0].Results;
                var PageCountabc = decimal.Parse((TotalRecordCount / pageSize).ToString());
                var PageCount = Math.Ceiling(PageCountabc);

                var pageinfo = new PagingInfo();
                pageinfo.PageCount = int.Parse(PageCount.ToString());
                pageinfo.PageNo = pageIndex;
                pageinfo.PageSize = int.Parse(pageSize.ToString());
                pageinfo.TotalRecordCount = int.Parse(TotalRecordCount.ToString());

                //return  abcd;
                return Json(new { Success = 1, error = false, EmployeeList = employeelist, PageInfo = pageinfo });


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class TotalCount
        {
            public int Results { get; set; }
        }
        public class PagingInfo
        {
            public int PageCount { get; set; }
            public int PageNo { get; set; }
            public int PageSize { get; set; }
            public int TotalRecordCount { get; set; }
        }

        public JsonResult GetEmployeesAll(string From, string To, int Emp, string searchquery = "")
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                string dateString = "1950-01-01 00:00:00.0000000";
                DateTime startDate = DateTime.Parse(dateString);
                DateTime endDate = DateTime.Parse(dateString);
                DateTime dateValue1;
                DateTime dateValue2;
                if (DateTime.TryParseExact(From, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue1))
                {
                    startDate = DateTime.ParseExact(dateValue1.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }
                if (DateTime.TryParseExact(To, new[] { "d-MMM-yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue2))
                {
                    endDate = DateTime.ParseExact(dateValue2.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }
                string SearchColumns = "";
                string SearchKeywords = "";
                if (searchquery != "")
                {
                    JObject jsonObject = JObject.Parse(searchquery);

                    foreach (JProperty property in jsonObject.Properties())
                    {
                        string columnName = property.Name;
                        string value = property.Value.ToString();

                        if (!string.IsNullOrEmpty(value))
                        {
                            SearchColumns += columnName + ",";
                            SearchKeywords += value + ",";
                        }
                    }

                    string toremove = "1,10,";
                    string toremove1 = "pageIndex,pageSize,";
                    int lastIndex = SearchKeywords.LastIndexOf(toremove);
                    int lastIndex1 = SearchColumns.LastIndexOf(toremove1);
                    if (lastIndex1 == -1)
                    {
                        lastIndex1 = 0;
                    }
                    if (lastIndex == -1)
                    {
                        lastIndex = 0;
                    }
                    SearchKeywords = SearchKeywords.Substring(0, lastIndex) + SearchKeywords.Substring(lastIndex + toremove.Length);
                    SearchColumns = SearchColumns.Substring(0, lastIndex1) + SearchColumns.Substring(lastIndex1 + toremove1.Length);
                }
                if (Emp == null)
                {
                    Emp = 0;
                }
                int idx = 10;
                SqlParameter p1 = new SqlParameter("@dtFrom", startDate);
                SqlParameter p2 = new SqlParameter("@dtTo", endDate);
                SqlParameter p3 = new SqlParameter("@ComId", comid);
                SqlParameter p4 = new SqlParameter("@EmpId", Emp);
                SqlParameter p5 = new SqlParameter("@PageSize", '0');
                SqlParameter p6 = new SqlParameter("@PageIndex", idx);
                SqlParameter p7 = new SqlParameter("@SearchKeywords", SearchKeywords);
                SqlParameter p8 = new SqlParameter("@SearchColumns", SearchColumns);
                SqlParameter p9 = new SqlParameter("@Criteria", '2');

                string query2 = $"Exec Hr_prcGetRawData '{startDate}', '{endDate}', " +
                    $"'{comid}','{Emp}','0','{idx}','{SearchKeywords}','{SearchColumns}','2'";

                var employeelist = Helper.ExecProcMapTList<RawDataVM>("dbo.Hr_prcGetRawData", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 });


                //return  abcd;
                return Json(new { Success = 1, error = false, EmployeeList = employeelist });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetPunchDataAll(PunchDataParamVM punchData)
        {
            string comid = HttpContext.Session.GetString("comid");
            if(punchData.EmpId == null)
            {
                punchData.EmpId = 0;
            }
            
            string SearchColumns = "";
            string SearchKeywords = "";
          
            if (punchData.DeptName != null)
            {
                string columnName = "DeptName";
                string value = punchData.DeptName;
                SearchColumns += columnName + ",";
                SearchKeywords += value + ",";
            }
            if(punchData.DesigName != null)
            {
                string columnName = "DesigName";
                string value = punchData.DesigName;
                SearchColumns += columnName + ",";
                SearchKeywords += value + ",";
            }
            if (punchData.SectName != null)
            {
                string columnName = "SectName";
                string value = punchData.SectName;
                SearchColumns += columnName + ",";
                SearchKeywords += value + ",";
            }
            if (punchData.EmpCode != null)
            {
                string columnName = "EmpCode";
                string value = punchData.EmpCode;
                SearchColumns += columnName + ",";
                SearchKeywords += value + ",";
            }
            if (punchData.EmpName != null)
            {
                string columnName = "EmpName";
                string value = punchData.EmpName;
                SearchColumns += columnName + ",";
                SearchKeywords += value + ",";
            }

            SqlParameter p1 = new SqlParameter("@dtFrom", punchData.From);
            SqlParameter p2 = new SqlParameter("@dtTo", punchData.To);
            SqlParameter p3 = new SqlParameter("@ComId", comid);
            SqlParameter p4 = new SqlParameter("@EmpId", punchData.EmpId);
            SqlParameter p5 = new SqlParameter("@PageSize", punchData.Size);
            SqlParameter p6 = new SqlParameter("@PageIndex", punchData.Page);
            SqlParameter p7 = new SqlParameter("@SearchKeywords", SearchKeywords);
            SqlParameter p8 = new SqlParameter("@SearchColumns", SearchColumns);
            SqlParameter p9 = new SqlParameter("@Criteria", '0');

            string query3 = $"Exec Hr_prcGetRawData '{punchData.From}', '{punchData.To}', " +
                $"'{comid}','{punchData.EmpId}','{punchData.Size}','{punchData.Page}','{SearchKeywords}','{SearchColumns}','0'";

            var employeelist = Helper.ExecProcMapTList<RawDataVM>("dbo.Hr_prcGetRawData", new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 });


            SqlParameter pr1 = new SqlParameter("@dtFrom", punchData.From);
            SqlParameter pr2 = new SqlParameter("@dtTo", punchData.To);
            SqlParameter pr3 = new SqlParameter("@ComId", comid);
            SqlParameter pr4 = new SqlParameter("@EmpId", punchData.EmpId);
            SqlParameter pr5 = new SqlParameter("@PageSize", punchData.Size);
            SqlParameter pr6 = new SqlParameter("@PageIndex", punchData.Page);
            SqlParameter pr7 = new SqlParameter("@SearchKeywords", SearchKeywords);
            SqlParameter pr8 = new SqlParameter("@SearchColumns", SearchColumns);
            SqlParameter pr9 = new SqlParameter("@Criteria", '1');

            string query4 = $"Exec Hr_prcGetRawData '{punchData.From}', '{punchData.To}', " +
                $"'{comid}','{punchData.EmpId}','{punchData.Size}','{punchData.Page}','{SearchKeywords}','{SearchColumns}','1'";

            var totalRow = Helper.ExecProcMapTList<TotalCount>("dbo.Hr_prcGetRawData", new SqlParameter[] { pr1, pr2, pr3, pr4, pr5, pr6, pr7, pr8, pr9 });
            decimal TotalRecordCount = totalRow[0].Results;
            var PageCountabc = decimal.Parse((TotalRecordCount / punchData.Size).ToString());
            var PageCount = Math.Ceiling(PageCountabc);

            var pageinfo = new PagingInfo();
            pageinfo.PageCount = int.Parse(PageCount.ToString());
            pageinfo.PageNo = punchData.Page;
            pageinfo.PageSize = int.Parse(punchData.Size.ToString());
            pageinfo.TotalRecordCount = int.Parse(TotalRecordCount.ToString());

            //return  abcd;
            return Json(new { Success = 1, error = false, EmployeeList = employeelist, PageInfo = pageinfo });

        }

        public IActionResult GetRawData(int? emText, string From, string To, string? DeptName, string? DesigName, 
            string? SectName, string? EmpName, string? EmpCode, string? DtPunchDate, string? DtPunchTime, int page =1,int size=5)

        {
            try
            {
                if (emText == null)
                {
                    emText = 0;
                }
                var punchData = new PunchDataParamVM();
                punchData.DtPunchDate = DtPunchDate;
                punchData.DtPunchTime = DtPunchTime;
                punchData.Page = page;
                punchData.Size = size;
                punchData.EmpId = emText;
                punchData.From = From;
                punchData.To = To;
                punchData.DeptName = DeptName;
                punchData.SectName = SectName;
                punchData.EmpCode = EmpCode;
                punchData.DesigName = DesigName;
                punchData.EmpName = EmpName;

                var query = _repository.GetRawData(punchData);
                decimal TotalRecordCount = query.ToList().Count();

                var PageCountabc = decimal.Parse((TotalRecordCount / punchData.Size).ToString());
                var PageCount = Math.Ceiling(PageCountabc);

                var pageinfo = new PagingInfo();
                pageinfo.PageCount = int.Parse(PageCount.ToString());
                pageinfo.PageNo = punchData.Page;
                pageinfo.PageSize = int.Parse(punchData.Size.ToString());
                pageinfo.TotalRecordCount = int.Parse(TotalRecordCount.ToString());
                int offset = (punchData.Page - 1) * punchData.Size;
                var FinalQuery = query.OrderBy(i => i.EmpId).Skip(offset).Take(punchData.Size);
                var EmployeeList = FinalQuery.ToList();

                var filterObject = new FilterData();
                if (EmpCode != null) filterObject.EmpCode = EmpCode;
                if (EmpName != null) filterObject.EmpName = EmpName;
                if (DesigName != null) filterObject.DesigName = DesigName;
                if (DeptName != null) filterObject.DeptName = DeptName;
                if (SectName != null) filterObject.SectName = SectName;
                if (DtPunchDate != null) filterObject.DtPunchDate = DtPunchDate;
                if (DtPunchTime != null) filterObject.DtPunchTime = DtPunchTime;
                return Json(new { Success = 1, error = false, EmployeeList = EmployeeList, PageInfo = pageinfo, Filters = filterObject });

            }
            catch (Exception ex)
            {

                throw;
            }
           
            
        }
        public IActionResult GetFilteredEmployee(string term)
        {
            string comid = HttpContext.Session.GetString("comid");


            // Perform case-insensitive filtering based on employee names or other relevant properties
            term = term.ToLower();

            var empInfo = (from emp in db.HR_Emp_Info
                           join d in db.Cat_Department on emp.DeptId equals d.DeptId
                           join s in db.Cat_Section on emp.SectId equals s.SectId
                           join des in db.Cat_Designation on emp.DesigId equals des.DesigId
                           join emptype in db.Cat_Emp_Type on emp.EmpTypeId equals emptype.EmpTypeId

                           where emp.ComId == comid && (emp.EmpName.Contains(term) || emp.EmpCode.Contains(term))
                           select new
                           {
                               value = emp.EmpId,
                               label = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + emptype.EmpTypeName + " ] - [ " + d.DeptName + " ]"
                           }).Take(10).ToList();



            return Json(empInfo);
        }
        public IActionResult DeletePunchData(int  id)
        {
            var data = _RDB.Hr_RawData.Where(x => x.aId == id).FirstOrDefault();
            if (data == null)
            {
                var data1 = db.HR_RawData_Apps.Where(x => x.aId == id).FirstOrDefault();
                data1.IsDelete = true;
                db.Update(data1);
                db.SaveChanges();
            }
            else
            {
                data.IsDelete = true;
                _RDB.Update(data);
                _RDB.SaveChanges();
            }            

            return RedirectToAction("Index");
        }
      
        public IActionResult Delete(int id)
        {
            var data = _RDB.Hr_RawData.Where(x => x.aId == id).FirstOrDefault();
            var data1 = db.HR_RawData_Apps.Where(x => x.aId == id).FirstOrDefault();
            if (data1 != null)
            {
                data1.IsDelete = true;
                db.Update(data1);
                db.SaveChanges();
            }
            if (data != null)
            {
                data.IsDelete = true;
                _RDB.Update(data);
                _RDB.SaveChanges();
            }
            

            return RedirectToAction("Modern");
        }

        public IActionResult Edit(int id)
        {
            //var punchData = new RawDataEditVM { aId = id };

            //var data = (from _rawdata in _RDB.Hr_RawData
            //            where _rawdata.aId == id
            //            join empInfo in db.HR_Emp_Info
            //            on _rawdata.EmpId equals empInfo.EmpId
            //            select new
            //            {
            //                _rawdata,
            //                empInfo.EmpCode
            //            }).FirstOrDefault();


            //if (data != null)
            //{
            //    punchData.EmpCode = data.EmpCode;
            //    punchData.DtPunchDate = (DateTime)data._rawdata.DtPunchDate;
            //    punchData.DtPunchTime = (DateTime)data._rawdata.DtPunchTime;
            //}

            //return View(punchData);

            var punchData = new RawDataEditVM { aId = id };

            // Fetch data from the first context (_RDB)
            var rawDataQuery = _RDB.Hr_RawData.FirstOrDefault(r => r.aId == id);

            if (rawDataQuery != null)
            {
                // Fetch data from the second context (db)
                var empInfoQuery = db.HR_Emp_Info.FirstOrDefault(i => i.EmpId == rawDataQuery.EmpId).EmpCode;

                if (empInfoQuery != null)
                {
                    punchData.EmpCode = empInfoQuery;
                    punchData.DtPunchDate =(DateTime) rawDataQuery.DtPunchDate;
                    punchData.DtPunchTime = (DateTime)rawDataQuery.DtPunchTime;
                }
            }

            return View(punchData);

        }
        [HttpPost]
        public IActionResult Edit(RawDataEditVM obj)
        {
            var RawData = obj;
            var data = _RDB.Hr_RawData.Where(x => x.aId == obj.aId).FirstOrDefault();
            var data1 = db.HR_RawData_Apps.Where(x => x.aId == obj.aId).FirstOrDefault();
            if (data != null)
            {
                data.DtPunchTime = obj.DtPunchTime;
                _RDB.Update(data);
                _RDB.SaveChanges();
            }
            if(data1 != null)
            {
                data1.dtPunchTime = obj.DtPunchTime;
                db.Update(data1);
                db.SaveChanges();
            }
            
            
            return RedirectToAction("Modern");
        }


        [HttpPost]
        public IActionResult DeletePunchDataList([FromBody] List<RawDataVM> itemList)
        {
            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    var data = _RDB.Hr_RawData.Where(x => x.aId == item.aId).FirstOrDefault();
                    var data1 = db.HR_RawData_Apps.Where(x => x.aId == item.aId).FirstOrDefault();
                    
                    if (data != null)
                    {
                        data.IsDelete = true;
                        _RDB.Update(data);
                        _RDB.SaveChanges();
                    }
                    if (data1 != null)
                    {
                        data1.IsDelete = true;
                        db.Update(data1);
                        db.SaveChanges();
                    }

                }
                return Json(true);
            }
            return Json(false); 
        }
    }
    public class FilterData
    {
        public string? EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? DeptName { get; set; }
        public string? DesigName { get; set; }
        public string? SectName { get; set; }
        public string? DtPunchDate { get; set; }
        public string? DtPunchTime { get; set;}
    }

        public class RawDataEditViewModel
    {
        public int aId { get; set; }
        public string EmpCode { get; set; }
        public string PunchDate { get; set; }
        public string PunchTime { get; set; }
    }
    public class RawDataVM
    {

        public int aId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime DtJoin { get; set; }
        public string DesigName { get; set; }
        public string? DeviceNo { get; set; }
        public string SectName { get; set; }
        public string DeptName { get; set; }

        public string Mobile { get; set; }
        public DateTime DtPunchDate { get; set; }
        public DateTime DtPunchTime { get; set; }
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string PunchLocation { get; set; }
        public string InOut { get; set; }
        public byte[]? PicFront { get; set; }
        public byte[]? PicBack { get; set; }
    }
}


