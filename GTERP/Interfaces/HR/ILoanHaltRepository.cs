
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILoanHaltRepository
    {
        public IEnumerable<SelectListItem> LoanTypeCat_Variable();

        public IEnumerable<SelectListItem> OtherTypeCat_Variable();

        public List<EmpViewModel> GetEmployee();

        void LoanCreate(LoanHalt loanHalt);

    }
}
