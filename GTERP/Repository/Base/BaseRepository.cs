using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly GTRDBContext _context;
        private DbSet<T> _entities;
        private readonly IHttpContextAccessor _httpcontext;
        public BaseRepository(GTRDBContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
            _httpcontext = new HttpContextAccessor();
        }

        public IQueryable<T> GetAll()
        {
            var comId = _httpcontext.HttpContext.Session.GetString("comid");
            return _entities.Where(x => !x.IsDelete && x.ComId == comId);
        }

        public T Find(string ComId)
        {
            return _entities.Where(x => !x.IsDelete && x.ComId == ComId).SingleOrDefault();
        }

        public T FindById(int? Id)
        {
            return _entities.Find(Id);
        }

        public void Add(T model)
        {
            model.ComId = _httpcontext.HttpContext.Session.GetString("comid");
            model.UserId = _httpcontext.HttpContext.Session.GetString("userid");
           
            model.DateAdded = DateTime.Now;
            
            _entities.Add(model);
            _context.SaveChanges();
        }

        public void Delete(T model)
        {
            model.ComId = _httpcontext.HttpContext.Session.GetString("comid");
            model.UserId = _httpcontext.HttpContext.Session.GetString("userid");
            model.IsDelete = true;
            _entities.Update(model);
            _context.SaveChanges();
        }
        public void Update(T model)
        {
            model.ComId = _httpcontext.HttpContext.Session.GetString("comid");
            model.UserId = _httpcontext.HttpContext.Session.GetString("userid");
            model.UpdateByUserId = _httpcontext.HttpContext.Session.GetString("userid");
            model.DateUpdated = DateTime.Now;
            _context.Entry(model).Property("DateAdded").IsModified = false;
            _entities.Update(model);
            _context.SaveChanges();
        }

        public void RemoveRange(List<T> entity)
        {
            var Comid = entity.Select(x => x.ComId).ToString();
            var Userid = entity.Select(x => x.UserId).ToString();
            var DateUpdated = entity.Select(x => x.DateAdded).ToString();
            Comid = _httpcontext.HttpContext.Session.GetString("comid");
            Userid = _httpcontext.HttpContext.Session.GetString("userid");
            DateUpdated = DateTime.Now.ToString("dd-MMM-yyyy");
            _entities.RemoveRange(entity);
            _context.SaveChanges();
        }

        public void AddRange(List<T> entity)
        {
            var Comid = entity.Select(x => x.ComId).ToString();
            var Userid = entity.Select(x => x.UserId).ToString();
            var DateUpdated = entity.Select(x => x.DateAdded).ToString();
            Comid = _httpcontext.HttpContext.Session.GetString("comid");
            Userid = _httpcontext.HttpContext.Session.GetString("userid");
            DateUpdated = DateTime.Now.ToString("dd-MMM-yyyy");
            _entities.AddRange(entity);
            _context.SaveChanges();
        }

        public void AddRange(ICollection<T> entity)
        {
            throw new NotImplementedException();
        }


        public void Remove(T entity)
        {
            entity.IsDelete = true;
            _entities.Remove(entity);
            _context.SaveChanges();
        }

    }
}
