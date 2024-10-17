using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace GTERP.Repository.HR
{
    public class DocumentRepository : BaseRepository<HR_Emp_Document>, IDocumentRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IHostingEnvironment _env;
        public DocumentRepository(GTRDBContext context, IHostingEnvironment env) : base(context)
        {
            _context = context;
            _env = env;
            _httpcontext = new HttpContextAccessor();
        }

        public void AddDocument(HR_Emp_Document model)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");
            string userid = _httpcontext.HttpContext.Session.GetString("userid");

            var path = _env.WebRootPath + "\\EmpDocument\\";
            path = Path.Combine(path, GetFileName(model.RefCode, model.FileToUpload));

            model.FileName = model.FileToUpload.FileName;
            model.FilePath = model.RefCode + "_" + model.FileName; ;
            using (var stream = new FileStream(path, FileMode.Create))
            {
                model.FileToUpload.CopyToAsync(stream);
            }

            model.ComId = comid;
            model.UserId = userid;
            model.DateAdded = DateTime.Now;

            _context.Add(model);
        }

        public IEnumerable<SelectListItem> CatVariableList()
        {
            return new SelectList(_context.Cat_Variable
                .Where(v => v.VarType == "DocumentType"), "VarName", "VarName");
        }

        public IEnumerable<HR_Emp_Document> GetDocumentAll()
        {
            return (_context.HR_Emp_Document.ToList());
        }

        public IEnumerable<SelectListItem> GetDocumentList()
        {
            throw new NotImplementedException();
        }
        public string GetFileName(string code, IFormFile file)
        {
            var name = ContentDispositionHeaderValue.Parse(
                            file.ContentDisposition).FileName.ToString().Trim('"');
            return code + "_" + name;
        }

        public void UpdateDocumnet(HR_Emp_Document model)
        {
            string userid = _httpcontext.HttpContext.Session.GetString("userid");
            var path = _env.WebRootPath + "\\EmpDocument\\";
            var exist = _context.HR_Emp_Document.Find(model.Id);

            if (Directory.Exists(path + exist.FilePath))
            {
                System.IO.File.Delete(exist.FilePath);
            }


            path = Path.Combine(path, GetFileName(model.RefCode, model.FileToUpload));
            exist.RefCode = model.RefCode;
            exist.Title = model.Title;
            exist.Remarks = model.Remarks;
            exist.FileName = model.FileToUpload.FileName;
            exist.FilePath = model.RefCode + "_" + model.FileName;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                model.FileToUpload.CopyToAsync(stream);
            }

            exist.UpdateByUserId = userid;
            exist.DateUpdated = DateTime.Now;

            _context.Entry(exist).State = EntityState.Modified;
        }
    }

}
