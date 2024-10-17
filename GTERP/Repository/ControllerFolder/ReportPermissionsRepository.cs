using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.ControllerFolder
{
    public class ReportPermissionsRepository : BaseRepository<ReportPermissions>, IReportPermissionsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<ReportPermissionsRepository> _logger;


        public ReportPermissionsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<ReportPermissionsRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        //public void AddRangeForReport(List<ReportPermissions> ReportPermissions)
        //{
        //    string comid = _httpcontext.HttpContext.Session.GetString("comid");
        //    string userid = _httpcontext.HttpContext.Session.GetString("userid");
        //    ReportPermissions.ForEach(x =>
        //    {
        //        x.ComId = comid;
        //        x.UserId = userid.ToString();
        //    });

        //    //ReportPermissions.ForEach(p => p.ComId = comid ,p.UserId = userid.ToString());

        //    _context.ReportPermissions.AddRange(ReportPermissions);
        //    _context.SaveChanges();
        //}

        public void DeleteCountry(int id)
        {
            Country country = _context.Countries.Find(id);
            _context.Countries.Remove(country);
            _context.SaveChanges();
        }

        public List<ReportPermissionsVM> GetReportPermissionsList(string userid)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@comid", comid);
            parameters[1] = new SqlParameter("@userid", userid);

            List<ReportPermissionsVM> ListOfReportPermission = Helper.ExecProcMapTList<ReportPermissionsVM>("HR_ReportPermission", parameters);
            return ListOfReportPermission;
        }

        //public void RemoveRangeForReport()
        //{
        //    string comid = _httpcontext.HttpContext.Session.GetString("comid");
        //    string userid = _httpcontext.HttpContext.Session.GetString("userid");

        //    var exist = _context.ReportPermissions.Where(c => c.ComId == comid && c.UserId == userid).ToList();
        //    if (exist.Count > 0)
        //    {
        //        _context.RemoveRange(exist);
        //        _context.SaveChanges();
        //    }
        //}

        public void UpdateCountry(Country country)
        {
            country.DateUpdated = DateTime.Now;
            _context.Entry(country).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(country);
            _context.SaveChanges();
        }
    }
}
