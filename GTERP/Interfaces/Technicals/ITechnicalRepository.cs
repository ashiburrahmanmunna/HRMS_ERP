using GTERP.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GTERP.Interfaces.Technicals
{
    public interface ITechnicalRepository:IBaseRepository<Technical>
    {
        IEnumerable<SelectListItem> MeetingId();
        IEnumerable<SelectListItem> MeetingType();
        IEnumerable<SelectListItem> MeetingId1(int? id);
        IEnumerable<SelectListItem> MeetingType1(int? id);
        void GetReport(string reportType, DateTime fromDate, DateTime toDate, string rptFormat);
    }
}
