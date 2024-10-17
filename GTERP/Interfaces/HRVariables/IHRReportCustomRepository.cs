using GTERP.Interfaces.Self;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface IHRCustomReportRepository : ISelfRepository<HR_CustomReport>
    {
        IEnumerable<SelectListItem> GetReportType();
        IEnumerable<SelectListItem> GetEmpType();
        IEnumerable<SelectListItem> GetComId();
        IEnumerable<SelectListItem> GetReportTypeCustom();
        List<HR_CustomReport> GetAllReport();
        
    }
}
