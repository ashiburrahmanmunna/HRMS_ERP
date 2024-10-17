using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IAttendanceProcessRepository
    {
        HR_AttendanceProcess GetAttProcess(string msg);
        IEnumerable<SelectListItem> GetEmpSelectList();
        void prcInsertEmp(HR_AttendanceProcess model, string optCriteria);
        void SaveAtt(HR_AttendanceProcess model);
        void RemoveProssType(HR_AttendanceProcess model);
    }
}
