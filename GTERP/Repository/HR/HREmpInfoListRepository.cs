using GTERP.Interfaces.HR;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class HREmpInfoListRepository : IHREmpInfoRepository
    {
        private readonly GTRDBContext db;
        private readonly IHttpContextAccessor _httpContext;
        public HREmpInfoListRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            db = context;
            _httpContext = httpContext;
        }
        public IEnumerable<SelectListItem> AccTypeList()
        {
            return new SelectList(db.Cat_AccountType, "AccTypeId", "AccTypeName");
        }

        public IEnumerable<SelectListItem> EmpAccTypeList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_AccountType, "AccTypeId", "AccTypeName", hrEmpInfo.HR_Emp_BankInfo.AccTypeId);
        }

        public IEnumerable<SelectListItem> EmpBankList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_Bank, "BankId", "BankName", hrEmpInfo.HR_Emp_BankInfo.BankId);
        }

        public IEnumerable<SelectListItem> EmpBranchList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_BankBranch, "BranchId", "BranchName", hrEmpInfo.HR_Emp_BankInfo.BranchId);
        }

        public IEnumerable<SelectListItem> EmpBuildingTypeList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_BuildingType, "BId", "BuildingName", hrEmpInfo.HR_Emp_PersonalInfo.BId);
        }

        public IEnumerable<SelectListItem> EmpCurrPOList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_PostOffice
                    .Where(p => p.PStationId == hrEmpInfo.HR_Emp_Address.EmpCurrPSId), "POId", "POName", hrEmpInfo.HR_Emp_Address.EmpCurrPOId);
        }

        public IEnumerable<SelectListItem> EmpCurrPSList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_PoliceStation
                     .Where(p => p.DistId == hrEmpInfo.HR_Emp_Address.EmpCurrDistId), "PStationId", "PStationName", hrEmpInfo.HR_Emp_Address.EmpCurrPSId);
        }

        public List<HR_Emp_Education> EmpEducationDelete(string HR_Emp_Educations)
        {
            var emp_Educations = JsonConvert.DeserializeObject<List<HR_Emp_Education>>(HR_Emp_Educations);
            if (emp_Educations.Count < 1)
            {
                var odlData = db.HR_Emp_Education.Where(e => e.EmpId == _httpContext.HttpContext.Session.GetInt32("empid")).ToList();
                db.RemoveRange(odlData);
            }
            else
            {
                var empId = emp_Educations.FirstOrDefault()?.EmpId;
                if (empId.HasValue)
                {
                    var odlData = db.HR_Emp_Education.Where(e => e.EmpId == empId.Value).ToList();
                    db.RemoveRange(odlData);
                    db.AddRange(emp_Educations);
                }
            }

            db.SaveChanges();
            return emp_Educations;
          
        }

        public List<HR_Emp_Projects> EmpProjectDelete(string HR_Emp_Projects)
        {
            var emp_Educations = JsonConvert.DeserializeObject<List<HR_Emp_Projects>>(HR_Emp_Projects);
            if (emp_Educations.Count < 1)
            {
                var odlData = db.HR_Emp_Projects.Where(e => e.EmpId == _httpContext.HttpContext.Session.GetInt32("empid")).ToList();
                db.RemoveRange(odlData);
            }
            else
            {
                var empId = emp_Educations.FirstOrDefault()?.EmpId;
                if (empId.HasValue)
                {
                    var odlData = db.HR_Emp_Projects.Where(e => e.EmpId == empId.Value).ToList();
                    db.RemoveRange(odlData);
                    db.AddRange(emp_Educations);
                }
            }

            db.SaveChanges();
            return emp_Educations;

        }

        public List<HR_Emp_Devices> EmpDeviceDelete(string HR_Emp_Devices)
        {
            var emp_Educations = JsonConvert.DeserializeObject<List<HR_Emp_Devices>>(HR_Emp_Devices);
            if (emp_Educations.Count < 1)
            {
                var odlData = db.HR_Emp_Devices.Where(e => e.EmpId == _httpContext.HttpContext.Session.GetInt32("empid")).ToList();
                db.RemoveRange(odlData);
            }
            else
            {
                var empId = emp_Educations.FirstOrDefault()?.EmpId;
                if (empId.HasValue)
                {
                    var odlData = db.HR_Emp_Devices.Where(e => e.EmpId == empId.Value).ToList();
                    db.RemoveRange(odlData);
                    db.AddRange(emp_Educations);
                }
            }

            db.SaveChanges();
            return emp_Educations;

        }
        

        public List<HR_Emp_Experience> EmpExperienceDelete(string HR_Emp_Experiences)
        {
            var JObject = new JObject();
            var data = JObject.Parse(HR_Emp_Experiences);
            string objct = data["HR_Emp_Experiences"].ToString();
            var emp_Experiences = JsonConvert.DeserializeObject<List<HR_Emp_Experience>>(objct);

            if (emp_Experiences.Count < 1)
            {
                var odlData = db.HR_Emp_Experience.Where(e => e.EmpId == _httpContext.HttpContext.Session.GetInt32("empid")).ToList();
                db.RemoveRange(odlData);
            }
            else
            {
                var odlData = db.HR_Emp_Experience.Where(e => e.EmpId == emp_Experiences.FirstOrDefault().EmpId).ToList();
                db.RemoveRange(odlData);
                db.AddRange(emp_Experiences);
            }

            db.SaveChanges();
            return emp_Experiences;
        }

        public IEnumerable<SelectListItem> EmpPayModeList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_PayMode, "PayModeId", "PayModeName", hrEmpInfo.HR_Emp_BankInfo.PayModeId);
        }

        public IEnumerable<SelectListItem> EmpPerPOList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_PostOffice
                    .Where(p => p.PStationId == hrEmpInfo.HR_Emp_Address.EmpPerPSId), "POId", "POName", hrEmpInfo.HR_Emp_Address.EmpPerPOId);
        }

        public IEnumerable<SelectListItem> EmpPerPSList(HR_Emp_Info hrEmpInfo)
        {
            return new SelectList(db.Cat_PoliceStation
                    .Where(p => p.DistId == hrEmpInfo.HR_Emp_Address.EmpPerDistId), "PStationId", "PStationName", hrEmpInfo.HR_Emp_Address.EmpPerPSId);
        }



        public IEnumerable<SelectListItem> GratuityFinalYList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var year = db.Acc_FiscalYears.Where(f => f.ComId == comid)
                .Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            return new SelectList(year, "FiscalYearId", "FYName");
        }

        public IEnumerable<SelectListItem> GratuityFundTransferYList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var year = db.Acc_FiscalYears.Where(f => f.ComId == comid)
                .Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            return new SelectList(year, "FiscalYearId", "FYName");
        }

        public IEnumerable<SelectListItem> PayModeList()
        {
            return new SelectList(db.Cat_PayMode, "PayModeId", "PayModeName");
        }

        public IEnumerable<SelectListItem> PFFinalYList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var year = db.Acc_FiscalYears.Where(f => f.ComId == comid)
                .Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            return new SelectList(year, "FiscalYearId", "FYName");
        }

        public IEnumerable<SelectListItem> PFFundTransferYList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var year = db.Acc_FiscalYears.Where(f => f.ComId == comid)
                .Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            return new SelectList(year, "FiscalYearId", "FYName");
        }

        public IEnumerable<SelectListItem> SubSectList()
        {

            return new SelectList(db.Cat_SubSection, "SubSectId", "SubSectName");
        }

        public IEnumerable<SelectListItem> CategoryList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            return new SelectList(db.Categories.Where(c => c.ComId == comid).ToList(), "CategoryId", "Name");

        }

        public IEnumerable<SelectListItem> SubCategoryList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            return new SelectList(db.SubCategory.Where(c => c.comid == comid).ToList(), "SubCategoryId", "SubCategoryName");

        }

        public IEnumerable<SelectListItem> WFFinalYList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var year = db.Acc_FiscalYears.Where(f => f.ComId == comid)
                .Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            return new SelectList(year, "FiscalYearId", "FYName");
        }

        public IEnumerable<SelectListItem> WFFundTransferYList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var year = db.Acc_FiscalYears.Where(f => f.ComId == comid)
                .Select(y => new { FiscalYearId = y.FiscalYearId, FYName = y.FYName });
            return new SelectList(year, "FiscalYearId", "FYName");
        }
    }
}
