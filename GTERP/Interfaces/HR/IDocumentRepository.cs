using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace GTERP.Interfaces.HR
{
    public interface IDocumentRepository : IBaseRepository<HR_Emp_Document>
    {
        IEnumerable<SelectListItem> GetDocumentList();
        IEnumerable<HR_Emp_Document> GetDocumentAll();
        IEnumerable<SelectListItem> CatVariableList();
        string GetFileName(string code, IFormFile file);
        void AddDocument(HR_Emp_Document model);
        void UpdateDocumnet(HR_Emp_Document model);
    }
}
