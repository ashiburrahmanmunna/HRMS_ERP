using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Inventory
{
    public interface IPostDocumentRepository
    {
        List<DocumentList> GetDocument(string FromDate, string ToDate, string criteria, string DocType, int DeptId, int PrdUnitId);
        IEnumerable<SelectListItem> DocTypeListProductionList1();
        IEnumerable<SelectListItem> DocTypeList1();
        void Leave();
        List<HR_Leave_Avail> Leave1(int DeptId);
        List<HR_Leave_Avail> Leave2(int DeptId);
        List<HR_Leave_Avail> Leave3(int DeptId);
        List<HR_Leave_Avail> Leave4(int DeptId);
        string Print(int? id, string type, string docname);
        

    }
}
