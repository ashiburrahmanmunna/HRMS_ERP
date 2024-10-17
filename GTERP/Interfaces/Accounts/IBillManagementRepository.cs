using GTERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTERP.ViewModels;
using GTERP.Interfaces.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GTERP.Interfaces.Accounts
{
    public interface IBillManagementRepository:IBaseRepository<Bill_Main>
    {
        Task<List<Bill_Main>> GetBillManagement();
        IQueryable<PurchaseReQuisitionResult> Query1();
        IQueryable<PurchaseReQuisitionResult> Query2(string UserList);
        IQueryable<PurchaseReQuisitionResult> Query3();
        decimal? LastPurchasePrice(int id);
        object Product(int id);
        int SingleBillData();
        void UpdateBillManagement(Bill_Main bill_Main);
        void AddBillManagement(Bill_Main bill_Main);
        string ProductName(int id);
        void DeletePrbBill(int prsubid);
        Task<Bill_Main> Bill(int? id);
        IEnumerable<SelectListItem> SupplierIdIf();
        IEnumerable<SelectListItem> ProductIdIf();
        IEnumerable<SelectListItem> AccIdIf();
        IEnumerable<SelectListItem> SupplierIdElse(Bill_Main Bill_Main);
        IEnumerable<SelectListItem> ProductIdElse();
        IEnumerable<SelectListItem> AccIdElse(Bill_Main Bill_Main);
        string PrintBillManagement(int id);
        string SetSessionAccountReportBill(string rptFormat, string CostAlloMainId, string FiscalYearId, string FiscalMonthId);
        string PrintSDReport(string rptFormat, string action, string FromDate, string ToDate);
        string GrrDetailsReport(string rptFormat, string action, string FromDate, string ToDate);
        string PrintWelfareReport(string rptFormat, string action, string FromDate, string ToDate);
    }
}
