using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTERP.ViewModels;
using GTERP.Interfaces.Base;

namespace GTERP.Interfaces.Accounts
{
    public interface ICostAllocationRepository:IBaseRepository<CostAllocation_Main>
    {
        Acc_FiscalYear FiscalYear();
        Acc_FiscalMonth FiscalMonth();
        IEnumerable<SelectListItem> FiscalYearId();
        IEnumerable<SelectListItem> FiscalYearIdCost(CostAllocation_Main CostAllocation_Main);
        IEnumerable<SelectListItem> FiscalMonthId();
        IEnumerable<SelectListItem> FiscalMonthIdCost(CostAllocation_Main CostAllocation_Main);
        IEnumerable<SelectListItem> FiscalYearIdElse();
        IEnumerable<SelectListItem> FiscalMonthIdElse();
        IEnumerable<SelectListItem> CostAlloMainId();
        Task<List<CostAllocation_Main>> GetCostAllocation();
        IQueryable<PurchaseReQuisitionResult> Query1();
        IQueryable<PurchaseReQuisitionResult> Query2(string UserList);
        IQueryable<PurchaseReQuisitionResult> Query3();
        decimal LastPurchasePrice(int id);
        object Product(int id);
        void UpdateCostAllocation(CostAllocation_Main CostAllocation_Main);
        void CreateCostAllocation(CostAllocation_Main CostAllocation_Main);
        Product Prod(int id);
        Unit Unit(int id);
        IEnumerable<SelectListItem> ProductList1();
        IEnumerable<SelectListItem> ProductList2(int? id);
        IEnumerable<SelectListItem> ProductList3();
        void DeletePrSub(int prsubid);
        string SetSessionAccountReport(string rptFormat, string CostAlloMainId, string FiscalYearId, string FiscalMonthId);
        IEnumerable<SelectListItem> AccId();
        Task<CostAllocation_Main> CostAllocation(int? id);
    }
}
