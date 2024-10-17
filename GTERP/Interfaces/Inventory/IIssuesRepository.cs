using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GTERP.Interfaces.Inventory
{
    public interface IIssuesRepository
    {
        IEnumerable<SelectListItem> UserList();
        IssueMain lastissueMain();
        IEnumerable<SelectListItem> PrdUnitIdIf();
        IEnumerable<SelectListItem> PrdUnitIdElse();
        IEnumerable<SelectListItem> PrdRqId(IssueMain issueMain);
        IEnumerable<SelectListItem> CurrencyId();
        IEnumerable<SelectListItem> PaymentTypeId();
        IEnumerable<SelectListItem> WareHouseId();
        IEnumerable<SelectListItem> WareHouseId2();
        IEnumerable<SelectListItem> BOMMainId();
        IEnumerable<SelectListItem> PatientId();
        IEnumerable<SelectListItem> DoctocId();
        IQueryable<Object> StoreRequisition();
        IEnumerable<SelectListItem> StoreRequisitionList();
        IQueryable<IssueResult> Query1();
        IQueryable<IssueResult> Query2();
        IQueryable<IssueResult> Query3();
        IQueryable<IssueResult> QueryTest(string UserList);
        IQueryable<IssueResult> QueryTest2();
        IQueryable<IssueResult> QueryTest3(string UserList);
        IQueryable<IssueResult> QueryTest4();
        IQueryable<IssueResult> QueryTest5(string UserList);
        IQueryable<IssueResult> QueryTest6();
        IQueryable<IssueResult> QueryTest7(string UserList);
        IQueryable<IssueResult> QueryTest8();
        IQueryable<IssueResult> QueryTest9(string UserList);
        IQueryable<IssueResult> QueryTest10();
        IQueryable<IssueResult> QueryTest11(string UserList);
        IQueryable<IssueResult> QueryTest12();
        IssueMain Details(int? id);
        IQueryable<Currency> GetCurrency(int id);
        Object GetDepartmentByStore(int id);
        List<IssueDetailsModel> GetStoreRequisitionDataById(int? StoreReqId);
        List<IssueDetailsModel> GetSubStoreRequisitionDataById(int? StoreReqId);
        IssueMain DuplicateDocument(IssueMain issueMain);
        Acc_FiscalMonth AccFiscalMonth(IssueMain issueMain);
        Acc_FiscalYear AccFiscalYear(IssueMain issueMain);
        HR_ProcessLock LockCheck(IssueMain issueMain);
        void CreateIssueMain(IssueMain issueMain);

        string PrintMissingSequence(string rptFormat, string action, string Type, string FromNo, string ToNo, string PrdUnitId);
        string PrintIssueVoucher(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId);
        string IssueDetailsReport(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId);
        string PrintIssueSummary(string rptFormat, string action, string FromDate, string ToDate, string PrdUnitId);
        bool IssueMainExists(int id);
        IEnumerable<Object> GetProducts(int? id);
        IssueMain FindData(int id);
        IssueMain FindData2(int? id);
        void DeleteData(int id);
        IEnumerable<SelectListItem> StoreReqId(int? id);
        List<SelectListItem> CategoryId();
        void Update(IssueMain issueMain);
        List<IssueDetailsModel> IssueDetailsInformation(int? id);
        string Print(int? id, string type);

    }
}
