using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class DesignationRepository : BaseRepository<Cat_Designation>, IDesignationRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public DesignationRepository(GTRDBContext context,IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IQueryable<Cat_Designation> GetAllDesignations()
        {
            var comId = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_Designation.Include(c => c.Grade).Where(x => x.ComId == comId && !x.IsDelete);
        }
        public List<Cat_Designation> GetDesignationsByCompany(string comid) 
        { 
            var desiglist = _context.Cat_Designation.Where(a => a.ComId == comid).ToList();
            return desiglist;
        }
        public IEnumerable<SelectListItem> GetDesignationList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.DesigId.ToString(),
                Text = x.DesigName
            });
        }
        //public IEnumerable<SelectListItem> GetDesignationListByName()
        //{
        //    return GetAll().Select(x => new SelectListItem
        //    {
        //        Value = x.DesigId.ToString(),
        //        Text = x.DesigName
        //    });
        //}

        public void UpdateDesignation(Cat_Designation cat_Designation)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid"); 
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            cat_Designation.ComId = comid;
            cat_Designation.UserId = userid;
            cat_Designation.DateUpdated = DateTime.Now;
           // _context.Cat_Designation.Update(cat_Designation);
            var EmpInfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && x.DesigId==cat_Designation.DesigId && !x.IsDelete).ToList();
            foreach(var emp in EmpInfo)
            {
                emp.GradeId = cat_Designation.GradeId;
                _context.HR_Emp_Info.Update(emp);
            }
            _context.SaveChanges();
        }
    }
}
