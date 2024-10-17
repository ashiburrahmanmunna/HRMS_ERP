using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Accounts
{
    public interface IGeneralReportRepository
    {
        Acc_VoucherMain VoucherMain(int VoucherId);
        List<LedgerDetailsModel> BookingDeliveryChallan(string AccId, string FYId, string dtFrom, string dtTo, string CountryId, string IsLocalCurrency, string SupplierId, string CustomerId, string EmployeeId);
        IEnumerable<SelectListItem> CountryId();
        IEnumerable<SelectListItem> AccIdRecPay();
        IEnumerable<SelectListItem> AccIdLedger();
        IEnumerable<SelectListItem> AccIdGroup();
        IEnumerable<SelectListItem> AccIdNoteOneCT();
        IEnumerable<SelectListItem> SupplierList();
        IEnumerable<SelectListItem> CustomerId();
        IEnumerable<SelectListItem> VoucherTypeList();
        IEnumerable<SelectListItem> PrdUnitId();
        ShowVoucherViewModel Model();
        string GeneralSetSession(string criteria, string rptFormat, string rpttype, string dtFrom, string dtTo,
            int? Currency, int? isDetails, int? isLocalCurr, int? isMaterial, int? FYId, int? AccIdRecPay, int? AccIdLedger,
            int? AccIdGroup, int? PrdUnitId, int? AccVoucherTypeId,
            int? SupplierId, int? CustomerId, int? EmployeeId, string AccIdNoteOneCT, string MinAccCode, string MaxAccCode);
    }
}
