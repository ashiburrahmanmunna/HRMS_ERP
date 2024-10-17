using GTERP.Models.Payroll;
using GTERP.Models.ViewModels;
using System;

namespace GTERP.Interfaces.Tax_Report
{
    public interface ITaxReportRepository
    {
        public String ClientInfo(TaxDto taxDto);
    }
}
