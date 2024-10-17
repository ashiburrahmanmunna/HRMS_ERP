using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.Payroll
{
    public interface IEmpWiseSalaryLedgerRepository
    {
        IEnumerable<SelectListItem> GetEmpWiseSalaryLedgerList();

        List<EmpWiseSalaryLedger> EmpWiseSalaryLedgerList(int? EmpId, int fun2, int pageIndex, int pageSize, string SearchColumns, string SearchKeywords, DateTime dtFrom, DateTime dtTo);

        List<PageNo> EmpWiseSalaryLedgerCount(int? EmpId, int fun2, int pageIndex, int pageSize, string SearchColumns, string SearchKeywords, DateTime dtFrom, DateTime dtTo);

        IEnumerable<SelectListItem> GetEmpList();

    }
}
