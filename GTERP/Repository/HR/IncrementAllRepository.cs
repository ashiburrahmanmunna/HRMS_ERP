using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class IncrementAllRepository : IIncrementAllRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly GTRDBContext _context;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        DateTime Date = DateTime.Now.Date;
        public IncrementAllRepository(IHttpContextAccessor httpContext,
            GTRDBContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContext = httpContext;
            _context = context;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public void CreateSalary(List<HR_Emp_Increment> Increments)
        {
            try
            {

                foreach (var item in Increments)
                {
                    var exist = _context.HR_Emp_Increment.Where(o => o.EmpId == item.EmpId && o.ComId == item.ComId).FirstOrDefault();
                    var existSalary = _context.HR_Emp_Salary.Where(s => s.EmpId == item.EmpId && s.ComId == item.ComId && s.IsDelete == false).FirstOrDefault();
                    if (exist == null)
                        _context.Add(item);
                    else
                    {

                        exist.EmpId = item.EmpId;
                        exist.Percentage = item.Percentage; //(float?)item.Amount / 100;
                        exist.Amount = item.Amount;
                        exist.NewBS = item.NewBS;
                        exist.NewSalary = item.NewSalary;
                        exist.NewHR = item.NewHR;
                        exist.NewMA = item.NewMA;
                        exist.NewFA = item.NewFA;
                        exist.NewTA = item.NewTA;

                        _context.Entry(exist).State = EntityState.Modified;

                        
                    }
                    existSalary.EmpId = item.EmpId;
                    existSalary.PersonalPay = (float)(item.NewSalary);
                    existSalary.BasicSalary = (float)(item.NewBS);
                    existSalary.HouseRent = (float)(item.NewHR);
                    existSalary.MadicalAllow = (float)(item.NewMA);
                    existSalary.CanteenAllow = (float)(item.NewFA);
                    existSalary.Transportcharge = (float)(item.NewTA);

                    _context.Entry(existSalary).State = EntityState.Modified;
                }
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Cat_HRSetting ForStaff()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var hrsetting = _context.Cat_HRSetting
                            .Include(x => x.Cat_Emp_Type)
                            .Where(x => x.CompanyCode == comid && x.EmpTypeId == 2 && !x.IsDelete)
                            .FirstOrDefault();
            if (hrsetting == null)
            {
                return new Cat_HRSetting();
            }
            return hrsetting;
        }

        public Cat_HRSetting ForWorker()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var hrsetting = _context.Cat_HRSetting
                            .Include(x => x.Cat_Emp_Type)
                            .Where(x => x.CompanyCode == comid && x.EmpTypeId==1 && !x.IsDelete)
                            .FirstOrDefault();
            if (hrsetting == null)
            {
                return new Cat_HRSetting();
            }
            return hrsetting;
        }       

        public List<IncrementViewModel> GetIncrementList(int aproval,DateTime? from, string act = "")
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var hrsetting = _context.Cat_HRSetting
                            .Include(x => x.Cat_Emp_Type)
                            .Where(x => x.CompanyCode == comid && !x.IsDelete)
                            .ToList();

            var dateFrom = from.HasValue ? from.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (act == "prev") dateFrom = dateFrom.AddMonths(-1);
            if (act == "next") dateFrom = dateFrom.AddMonths(1);
            var dateTo = dateFrom.AddMonths(1).AddSeconds(-1).Date;
            SqlParameter p1 = new SqlParameter("@dateFrom", dateFrom);
            SqlParameter p2 = new SqlParameter("@dateTo", dateTo);
            SqlParameter p3 = new SqlParameter("@ComId", comid);
            SqlParameter p4 = new SqlParameter("@aproval", aproval);
            var query = $"Exec Hr_PrcGetIncrAll '{dateFrom}', '{dateTo}', '{comid}', '{aproval}'";
            var data = Helper.ExecProcMapTList<IncrementViewModel>("dbo.Hr_PrcGetIncrAll", new SqlParameter[] { p1, p2, p3 ,p4 });
            return data;
        }

        public List<IncrementViewModel> IncrementReport(DateTime? from, string act = "")
        {
            var dateFrom = from.HasValue ? from.Value.Date : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (act == "prev") dateFrom = dateFrom.AddMonths(-1);
            if (act == "next") dateFrom = dateFrom.AddMonths(1);
            var dateTo = dateFrom.AddMonths(1).AddSeconds(-1).Date;
            SqlParameter p1 = new SqlParameter("@dateFrom", dateFrom);
            SqlParameter p2 = new SqlParameter("@dateTo", dateTo);
            var data = Helper.ExecProcMapTList<IncrementViewModel>("dbo.Hr_PrcGetIncrAll", new SqlParameter[] { p1, p2 });
            return data;
        }

        public void UpdateSalary(List<HR_Emp_Salary> Salaries)
        {

            foreach (var item in Salaries)
            {
                var exist = _context.HR_Emp_Salary.Where(o => o.EmpId == item.EmpId && o.ComId == item.ComId).FirstOrDefault();
                if (exist == null)
                    _context.Add(item);
                else
                {

                    exist.EmpId = item.EmpId;
                    exist.BasicSalary = item.BasicSalary; //(float?)item.Amount / 100;
                    exist.PersonalPay = item.PersonalPay;

                    _context.Entry(exist).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();

        }
    }
}
