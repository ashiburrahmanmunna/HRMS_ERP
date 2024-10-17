using AutoMapper;
using ExcelDataReader;
using GTERP.Controllers.Device.Helper;
using GTERP.EF;
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
using System.Net.Http;

namespace GTERP.Repository.Payroll
{
    public class SalaryAdditionRepository : BaseRepository<Payroll_SalaryAddition>, ISalaryAdditionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IMapper _mapping;
        private readonly IHttpContextAccessor _httpContext;
        private readonly gtrerp_allContext _ef;
        public SalaryAdditionRepository(GTRDBContext context,gtrerp_allContext ef, IHttpContextAccessor httpContext, IMapper mapping) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
            _mapping = mapping;
            _ef = ef;
        }

        public void AddSalaryAddition(Payroll_SalaryAddition salaryAddition)
        {
            salaryAddition.DateUpdated = DateTime.Now;
            salaryAddition.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            Add(salaryAddition);
        }

        public Payroll_SalaryAddition check(Payroll_SalaryAddition salaryAddition)
        {
            return GetAll()
                    .Where(s => s.EmpId == salaryAddition.EmpId
                    && s.DtInput.Date.Year == salaryAddition.DtInput.Date.Year
                    && s.DtInput.Date.Month == salaryAddition.DtInput.Date.Month
                    && s.OtherAddType == salaryAddition.OtherAddType && s.SalAddId != salaryAddition.SalAddId && s.IsDelete==false).FirstOrDefault();
        }

        public void DeleteSalaryAddition(int addId)
        {
            var salaryAddition = _context.Payroll_SalaryAddition.Find(addId);
            Delete(salaryAddition);
            _context.SaveChanges();
        }

        public FileContentResult DownloadSA(string file)
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

        public IEnumerable<HRProcessedDataSalVM> GetPrcDataSal(string prossType, string tableName)
        {
            var SalaryEditList= new List<HRProcessedDataSalVM>();
            string comid = _httpContext.HttpContext.Session.GetString("comid");            

            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            if (tableName == "Salary Process Update")
            {
                SalaryEditList = Helper.ExecProcMapTList<HRProcessedDataSalVM>("Payroll_prcGetSalaryEdit", parameter);
            }
            else
            {
                SalaryEditList = Helper.ExecProcMapTList<HRProcessedDataSalVM>("Payroll_prcGetSalaryEdit", parameter);
            }
            string query = $"Exec Payroll_prcGetSalaryEdit '{comid}', '{prossType}'";
            return SalaryEditList;
        }

        public List<Payroll_Temp_SalaryDataInputWithFile> GetSAList(string fName)
        {
            
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<Payroll_Temp_SalaryDataInputWithFile> salaryAdd = new List<Payroll_Temp_SalaryDataInputWithFile>();
            var filename = Path.GetFullPath("wwwroot/SampleFormat/" + fName);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

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

                            salaryAdd.Add(new Payroll_Temp_SalaryDataInputWithFile()
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
            return salaryAdd;


        }

        public List<SalaryAddition> LoadSAPartial(DateTime date)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var SalaryAdditions = GetAll()
                .Include(s => s.HR_Emp_Info)
                .Include(s => s.HR_Emp_Info.Cat_Section)
                .Where(s => s.DtInput.Date.Month == date.Month && s.DtInput.Date.Year == date.Year && s.ComId == comid && s.IsDelete == false)
                .Select(s => new SalaryAddition
                {
                    SalAddId = s.SalAddId,
                    EmpId = s.EmpId,
                    EmpCode = s.HR_Emp_Info.EmpCode,
                    EmpName = s.HR_Emp_Info.EmpName,
                    Section = s.HR_Emp_Info.Cat_Section.SectName,
                    Designation = s.HR_Emp_Info.Cat_Designation.DesigName,
                    DtJoin = s.DtJoin.ToString("dd-MMM-yyyy"),
                    DtInput = s.DtInput.ToString("dd-MMM-yyyy"),
                    Amount = s.Amount,
                    Remarks = s.Remarks,
                    OtherAddType = s.OtherAddType
                }).ToList();
            return SalaryAdditions;
        }

        public void ModifiedSalaryAddition(Payroll_SalaryAddition salaryAddition)
        {
            salaryAddition.DateAdded = DateTime.Now;
            Update(salaryAddition);
        }

        public IEnumerable<SelectListItem> OtherAddType()
        {
            return new SelectList(_context.Cat_Variable
               .Where(v => v.VarType == "SalaryAddition")
               .OrderBy(v => v.SLNo).ToList(), "VarName", "VarName");
        }

        public List<PayrollSalaryAddition> prcUploadSA()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter p1 = new SqlParameter("@ComId", comid);
            SqlParameter p2 = new SqlParameter("@Type", "Salary Addition");
            var data = Helper.ExecProcMapTList<PayrollSalaryAddition>("dbo.Payroll_prcSalaryDataInput", new SqlParameter[] { p1, p2 });
            return data;
        }
        public void SalaryUpdate(List<HrProcessedDataSalUpdate> HR_SalarySettlements, string ProssType)
        {
            foreach (var item in HR_SalarySettlements)
            {

                
                var exist = _ef.HrProcessedDataSalUpdates.FirstOrDefault(o => o.EmpId == item.EmpId && o.ProssType == ProssType && o.ComId == item.ComId);
                if (exist == null) {
                    var data = _ef.HrProcessedDataSals.FirstOrDefault(o => o.EmpId == item.EmpId && o.ProssType == ProssType && o.ComId == item.ComId);
                    //string datep = "01-Jan-1950 00:00:00";
                    //item.DtPayment = DateTime.Parse(datep);
                    //item.BankAcNo = "0";
                    item.IsManual = 1;
                    DateTime minValidDate = new DateTime(1753, 1, 1);
                   


                    if (item.DtPayment < minValidDate || item.DtPayment == null)
                    {
                        data.DtPayment = DateTime.Parse(DateTime.UtcNow.ToString());
                    }                    
                    if (item.BankAcNo == null)
                    {
                        data.BankAcNo = "0";

                    }

                    data.IsManual = 1;
                    data.OthrTtl = item.OthrTtl;
                    data.Present = item.Present;
                    data.Cl = item.Cl;
                    data.Sl = item.Sl;
                    data.El = item.El;
                    data.Ml = item.Ml;
                    data.Hday = item.Hday;
                    data.Wday = item.Wday;
                    data.Pday = item.Pday;
                    data.Absent = item.Absent;
                    data.CurrEl = item.CurrEl;

                    var config = new MapperConfiguration(cfg => {
                        cfg.CreateMap<HrProcessedDataSal, HrProcessedDataSalUpdate>();
                        cfg.CreateMap<HrProcessedDataSalUpdate, HrProcessedDataSal>();
                    });
                    var mapper = new Mapper(config);
                    var db = mapper.Map<HrProcessedDataSalUpdate>(data);
                    
                    _ef.HrProcessedDataSalUpdates.Add(db);
                   // _ef.HrProcessedDataSalUpdates.Add(item);
            }
                else
                {
                    exist.IsManual = 1;
                    exist.OthrTtl = item.OthrTtl;
                    exist.Present = item.Present;
                    exist.Cl = item.Cl;
                    exist.Sl = item.Sl;
                    exist.El = item.El;
                    exist.Ml = item.Ml;
                    exist.Hday = item.Hday;
                    exist.Wday = item.Wday;
                    exist.Pday = item.Pday;
                    exist.Absent = item.Absent;
                    exist.CurrEl = item.CurrEl;
                    _ef.Entry(exist).State = EntityState.Modified;
                }
            }
            _ef.SaveChanges();

            UpdateData(ProssType);

        }
        public List<HR_ProcessedDataSalVM> UpdateData(string prossType)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            var SalaryEditList = Helper.ExecProcMapTList<HR_ProcessedDataSalVM>("Payroll_prcGetSalaryEdit", parameter);

            return SalaryEditList;
        }


    }
}
