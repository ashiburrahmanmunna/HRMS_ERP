using DataTablesParser;
using DocumentFormat.OpenXml.Bibliography;
using GTERP.Interfaces.HRrecruitment;
using GTERP.Migrations.GTRDB;
using GTERP.Models;
using GTERP.Models.Recruitment;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GTERP.Repository.HrRecruitment
{
    public class PostRepository : IPostRepository
    {

        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public PostRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public List<HR_Recruitment_Post> GetAll()
        
        {

            // var comid = _httpContext.HttpContext.Session.GetString("comid");


            //var comid = "1";
            //return new SelectList(_context.HR_Recruitment_Posts);


          //List<PostVM> postlist = _context.HR_Recruitment_Posts.Include(s => s.Department).ToList();

             

            return _context.HR_Recruitment_Posts.Include(s => s.Department).ToList();
        }


        

        public void DeleteConfirmed(int id) {

            var post = _context.HR_Recruitment_Posts.Find(id);
            _context.HR_Recruitment_Posts.Remove(post);
             _context.SaveChanges();


        }

       public List<HR_Recruitment_Templete> GetTemplete(int id) {

            //var data = _context.HR_Recruitment_Templetes.Where(w => w.TempId == id).Select(s => s).FirstOrDefault();


            List<HR_Recruitment_Templete> result = _context.HR_Recruitment_Templetes.Where(w => w.TempId == id).ToList();          return result;
        }

        public List<HR_Recruitment_Post> GetPostbyId(int id)
        {

            //List<HR_Recruitment_Post> data = _context.HR_Recruitment_Templetes.Where(w => w.TempId == id).Select(s=>s).ToList();


            List<HR_Recruitment_Post> result = _context.HR_Recruitment_Posts.Include(x=>x.Department).Where(w => w.PostId == id).ToList(); 
            
            return result;
        }

        public void CreatePost(HR_Recruitment_Post p) {

            

            var title = _context.HR_Recruitment_Templetes.Where(x => x.TempId == Convert.ToInt32(p.PostTitle)).FirstOrDefault();

            


            if (p.PostId > 0)
            {

                _context.Update(p);
                _context.SaveChanges();
            }
            else
            {
                _context.Add(p);
                _context.SaveChanges();
            }
          



        }
        public bool PostExists(int id) { 
        
        
        
        return _context.HR_Recruitment_Posts.Any(e => e.PostId == id);
        }
        public void UpdatePost(HR_Recruitment_Post post) {

           
            
            
            
            
            _context.Update(post);
            _context.SaveChanges();



        }

        public List<HR_Recruitment_Department> GetDepartment() {


            return _context.HR_Recruitment_Department.ToList();


        }
        public List<HR_Recruitment_Department> DepartmentDetails(int id) {

            List<HR_Recruitment_Department> result = _context.HR_Recruitment_Department.Where(m => m.DeptId == id).ToList();
            return result;
        }
        public bool DepartmentExists(int id)
        {
            return _context.HR_Recruitment_Department.Any(e => e.DeptId == id);
        }
        public void DepartmentCreate(DeptVm model) {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("comid");
            HR_Recruitment_Department dd = new HR_Recruitment_Department();
            dd.ComId = comid;
            dd.UserId = userid;
            dd.Dept_Name=model.Dept_Name;

            _context.Add(dd);
            _context.SaveChanges();

        }
        public void DepartmentEdit( HR_Recruitment_Department model) {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("comid");
            model.ComId = comid;
            model.UserId = userid;
            _context.Update(model);
            _context.SaveChanges();



        }
       public void DepartmentDelete(int id) {


            var Dep = _context.HR_Recruitment_Department.Find(id);
            _context.HR_Recruitment_Department.Remove(Dep);
            _context.SaveChanges();

        }

    }
}
