using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IAccountChartRepository:IBaseRepository<Acc_ChartOfAccount>
    {

        List<Acc_ChartOfAccount> ChartOfAccountList();
        List<Acc_ChartOfAccount> All();
        Acc_ChartOfAccount LastAcccoa();
        List<COAtemp> COAParent1();
        IEnumerable<SelectListItem> OpFYId();
        int DefaultCurrency();
        IEnumerable<SelectListItem> ParentId();

        IEnumerable<SelectListItem> ParentId1(Acc_ChartOfAccount model);
        IEnumerable<SelectListItem> OpFYId1(Acc_ChartOfAccount model);
        IEnumerable<SelectListItem> CountryId1(Acc_ChartOfAccount model);
        IEnumerable<SelectListItem> ParentId2(int? Id);
        IEnumerable<SelectListItem> CountryId2(int? Id);
        IEnumerable<SelectListItem> OpFYId2(int? Id);
        IEnumerable<SelectListItem> AccumulatedDepId2(int? Id);
        IEnumerable<SelectListItem> DepExpenseId2(int? Id);
        List<SelectListItem> AccumulatedDepId();
        IEnumerable<SelectListItem> DepExpenseId();
        IEnumerable<SelectListItem> ParentIdElse();
        IEnumerable<SelectListItem> AccumulatedDepIdElse();
        IEnumerable<SelectListItem> DepExpenseIdElse();
        IEnumerable<SelectListItem> CountryId();
        void UpdateAccountChart(Acc_ChartOfAccount model);
        void AddAccountChart(Acc_ChartOfAccount model);
    }
}
