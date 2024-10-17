using GTERP.Interfaces.Self;
using GTERP.Models;
using GTERP.Models.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Self
{
    public class SelfRepository<T> : ISelfRepository<T> where T : SelfModel
    {
        private readonly GTRDBContext _context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        private readonly IHttpContextAccessor httpcontext;

        public SelfRepository(GTRDBContext context)
        {
            _context = context;
            entities = _context.Set<T>();
            httpcontext = new HttpContextAccessor();
        }

        public IQueryable<T> All()
        {
            return entities.Where(x => !x.IsDelete);
        }

        public T Find(string ComId)
        {
            throw new NotImplementedException();
        }

        public int Insert(T model)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            entity.DateAdded = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            _context.Entry(entity).Property("DateAdded").IsModified = false;
            entities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            entity.IsDelete = true;
            entities.Update(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(List<T> entity)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            entity.DateAdded = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            entities.Add(entity);
            _context.SaveChanges();
        }

        public T FindById(int? Id)
        {
            return entities.Find(Id);
        }
    }
}
