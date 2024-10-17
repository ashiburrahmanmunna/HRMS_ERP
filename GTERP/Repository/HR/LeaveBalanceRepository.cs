using DocumentFormat.OpenXml.Bibliography;
using ExcelDataReader;
using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HR
{
    public class LeaveBalanceRepository : BaseRepository<HR_Leave_Balance>, ILeaveBalanceRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<LeaveBalanceRepository> _logger;
        public LeaveBalanceRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            ILogger<LeaveBalanceRepository> logger

            ) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> GetLeaveBalanceList()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<SelectListItem> GetOpeningYear()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var data = new SelectList(_context.HR_Leave_Balance
            .Where(a => a.ComId == comid && a.DtOpeningBalance>2010)
            .Select(a => a.DtOpeningBalance)
            .Union(_context.HR_Leave_Balance.Select(a => DateTime.Now.Year))
            .Distinct()
            .OrderByDescending(x => x)).OrderByDescending(x => x.Text).ToList();
            return data;
        }
        public IEnumerable<SelectListItem> EmpList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            return new SelectList(_context.HR_Emp_Info
            .Where(v => v.ComId == comid && !v.IsDelete).
            Select(e => new { Text = e.EmpName + "- [" + e.EmpCode + "]", Value = e.EmpId })
            .OrderBy(e => e.Value).ToList(), "Value", "Text");

        }

        public async Task<List<LeaveBalance>> GetLeaveBalance(string Criteria, int EmpId, int SectId,int DeptId,int LineId,int FloorId, string DtOpBal)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[9];

            parameter[0] = new SqlParameter("@Id", 1);
            parameter[1] = new SqlParameter("@ComId", comid);
            parameter[2] = new SqlParameter("@Criteria", Criteria);
            parameter[3] = new SqlParameter("@EmpId", EmpId);
            parameter[4] = new SqlParameter("@dtOPBal", DtOpBal);
            parameter[5] = new SqlParameter("@sectID", SectId);
            parameter[6] = new SqlParameter("@DeptId", DeptId);
            parameter[7] = new SqlParameter("@LineId", LineId);
            parameter[8] = new SqlParameter("@FloorId", FloorId);
            List<LeaveBalance> ProductSerialresult = Helper.ExecProcMapTList<LeaveBalance>("Hr_prcgetleavebalance", parameter);
            string query = $"Exec Hr_prcgetleavebalance '{1}','{comid}','{Criteria}','{EmpId}','{DtOpBal}','{SectId}','{DeptId}','{LineId}','{FloorId}'";
            return ProductSerialresult;
        }

        public async Task<int> SaveLeaveBalance(List<HR_Leave_Balance> LeaveBalance)
        {
            try
            {
                foreach (HR_Leave_Balance item in LeaveBalance)
                {
                    var userid = _httpContext.HttpContext.Session.GetString("userid");
                    item.ComId = _httpContext.HttpContext.Session.GetString("comid");
                    if (item.LvBalId > 0)
                    {
                        HR_Leave_Balance mdata = _context.HR_Leave_Balance.Where(m => m.LvBalId == item.LvBalId).FirstOrDefault();
                        mdata.UpdateByUserId = userid;
                        mdata.UserId = userid;
                        mdata.DateUpdated = DateTime.Now;
                        mdata.EL = item.EL;
                        mdata.CL = item.CL;
                        mdata.SL = item.SL;
                        mdata.ML = item.ML;
                        _context.Entry(mdata).Property(x => x.UpdateByUserId).IsModified = true;
                        _context.Entry(mdata).Property(x => x.UserId).IsModified = true;
                        _context.Entry(mdata).Property(x => x.DateUpdated).IsModified = true;
                        _context.Entry(mdata).Property(x => x.SL).IsModified = true;
                        _context.Entry(mdata).Property(x => x.EL).IsModified = true;
                        _context.Entry(mdata).Property(x => x.CL).IsModified = true;
                        _context.Entry(mdata).Property(x => x.ML).IsModified = true;
                        //_context.SaveChanges();
                    }
                    else
                    {
                        item.UserId = userid;
                        item.UpdateByUserId = userid;
                        item.DateAdded = DateTime.Now;
                        _context.HR_Leave_Balance.Add(item);
                       // _context.SaveChanges();
                    }
                }
                await _context.SaveChangesAsync();
                return 1;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public List<HR_TempLeaveBalanceExcel> GetLeaveBalanceExcel(string fName)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var filename = Path.GetFullPath("wwwroot/Content/LeaveBL/" + comid + "/" + userid + "/" + fName);

            List<HR_TempLeaveBalanceExcel> LeaveBL = new List<HR_TempLeaveBalanceExcel>();
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
                                LeaveBL.Add(new HR_TempLeaveBalanceExcel()
                                {
                                    EmpCode = reader.GetValue(0).ToString(),
                                    ELYear = reader.GetValue(1).ToString(),
                                    ComId = _httpContext.HttpContext.Session.GetString("comid"),
                                    EL = decimal.Parse(reader.GetValue(2).ToString()),
                                    CL = decimal.Parse(reader.GetValue(3).ToString()),
                                    SL = decimal.Parse(reader.GetValue(4).ToString()),
                                    PrevELBal = decimal.Parse(reader.GetValue(5).ToString())
                                });
                            }
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
            }
            return LeaveBL;
        }

        public void FileUploadDirectory(IFormFile file)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");

            string fileLocation = Path.GetFullPath("wwwroot/Content/LeaveBL/" + comid + "/" + userid);
            if (Directory.Exists(fileLocation))
            {
                Directory.Delete(fileLocation, true);
            }
            string uploadlocation = Path.GetFullPath("wwwroot/Content/LeaveBL/" + comid + "/" + userid + "/");

            if (!Directory.Exists(uploadlocation))
            {
                Directory.CreateDirectory(uploadlocation);
            }

            string filePath = uploadlocation + Path.GetFileName(file.FileName);

            string extension = Path.GetExtension(file.FileName);
            var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Close();
        }

        public FileContentResult DownloadSampleFile(string file)
        {
            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;
            
            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return response;
        }

        public void SaveUploadedData()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var Query = $"Exec HR_PrcLeaveBalanceFromExcel '{comid}'";
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ComId", comid);
            Helper.ExecProc("HR_PrcLeaveBalanceFromExcel", sqlParameter);
        }
    }
}
