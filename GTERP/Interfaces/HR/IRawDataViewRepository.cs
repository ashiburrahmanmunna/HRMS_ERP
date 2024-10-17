using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IRawDataViewRepository
    {
        public String EmpPunchDataPrint(int? id, string type = "pdf");
        IEnumerable<SelectListItem> GetEmpList();

    }
}
