using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILoanReturnRepository : IBaseRepository<HR_Loan_Return>
    {
        IEnumerable<SelectListItem> CatVariableList();
        List<HR_Emp_Info> EmpData();
        HR_Loan_Return CheckData(HR_Loan_Return LoanReturn);

        List<LoanReturn> LoadLoanReturnPartial(DateTime date);
    }
}
