using ExcelDataReader;
using GTERP.Interfaces.Payroll;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GTERP.Repository.Payroll
{
    public class SalaryDeductionRepository : BaseRepository<Payroll_SalaryDeduction>, ISalaryDeductionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public SalaryDeductionRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public void AddSalaryDeduction(Payroll_SalaryDeduction salaryDeduction)
        {
            salaryDeduction.DateUpdated = DateTime.Now;
            salaryDeduction.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            Add(salaryDeduction);
        }

        public Payroll_SalaryDeduction check(Payroll_SalaryDeduction salaryDeduction)
        {
            return _context.Payroll_SalaryDeduction
                    .Where(s => s.EmpId == salaryDeduction.EmpId
                    && s.DtInput.Date.Year == salaryDeduction.DtInput.Date.Year
                    && s.DtInput.Date.Month == salaryDeduction.DtInput.Date.Month
                    && s.OtherDedType == salaryDeduction.OtherDedType && s.SalDedId != salaryDeduction.SalDedId && s.IsDelete == false).FirstOrDefault();
        }

        public void DeleteSalaryDeduction(int DedId)
        {
            var salaryDeduction = _context.Payroll_SalaryDeduction.Find(DedId);
            Delete(salaryDeduction);
            _context.SaveChanges();
        }

        public FileContentResult DownloadSD(string file)
        {
            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\SampleFormat"}" + "\\" + file;


            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return response;
        }

        public IEnumerable<SelectListItem> GetEmpInfo()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == comid && emp.IsDelete == false
                           select new
                           {
                               Value = emp.EmpId,
                               Text = "-" + emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();
            return new SelectList(empInfo, "Value", "Text");
        }

        public List<Payroll_Temp_SalaryDataInputWithFile> GetSDList(string fName)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Payroll_Temp_SalaryDataInputWithFile> salarydeduct = new List<Payroll_Temp_SalaryDataInputWithFile>();
            var filename = Path.GetFullPath("wwwroot/SampleFormat/" + fName);
            //var filename = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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
                                salarydeduct.Add(new Payroll_Temp_SalaryDataInputWithFile()
                                {
                                    ComId = comid,
                                    EmpCode = reader.GetValue(1).ToString(),
                                    DtInput = DateTime.Parse(reader.GetValue(2).ToString()),
                                    Amount = reader.GetDouble(3),
                                    OtherType = reader.GetValue(4).ToString(),
                                    Remarks = reader.GetValue(5).ToString(),
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                throw e;
            }

            return salarydeduct;


        }

        public List<SalaryDeduction> LoadSDPartial(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var SalaryDeduction = _context.Payroll_SalaryDeduction
                .Include(s => s.HR_Emp_Info)
                .Include(s => s.HR_Emp_Info.Cat_Section)
                .Where(s => s.DtInput.Date.Month == date.Month && s.DtInput.Date.Year == date.Year && s.ComId == comid)
                .Select(s => new SalaryDeduction
                {
                    SalDedId = s.SalDedId,
                    EmpId = s.EmpId,
                    EmpCode = s.HR_Emp_Info.EmpCode,
                    EmpName = s.HR_Emp_Info.EmpName,
                    Section = s.HR_Emp_Info.Cat_Section.SectName,
                    Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                    DtJoin = s.DtJoin.ToString("dd-MMM-yyyy"),
                    DtInput = s.DtInput.ToString("dd-MMM-yyyy"),
                    Amount = s.Amount,
                    Remarks = s.Remarks,
                    OtherDedType = s.OtherDedType
                }).ToList();
            return SalaryDeduction;
        }

        public void ModifiedSalaryDeduction(Payroll_SalaryDeduction salaryDeduction)
        {
            salaryDeduction.DateAdded = DateTime.Now;
            Update(salaryDeduction);
        }

        public IEnumerable<SelectListItem> OtherDedType()
        {
            return new SelectList(_context.Cat_Variable
               .Where(v => v.VarType == "SalaryDeduction")
               .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
        }

        public List<PayrollSalaryDeduction> prcUploadSD()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter p1 = new SqlParameter("@ComId", comid);
            SqlParameter p2 = new SqlParameter("@Type", "Salary Deduction");
            var data = Helper.ExecProcMapTList<PayrollSalaryDeduction>("dbo.Payroll_prcSalaryDataInput", new SqlParameter[] { p1, p2 });
            return data;
        }


    }
}
