using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Accounts
{
    public interface IAccVoucherRepository:IBaseRepository<Acc_VoucherMain>
    {
        IEnumerable<SelectListItem> UserList();
        IEnumerable<SelectListItem> FiscalYearId();
        IEnumerable<SelectListItem> FiscalYearId1();
        IEnumerable<SelectListItem> FiscalMonthId();
        IEnumerable<SelectListItem> FiscalMonthId1();
        IEnumerable<SelectListItem> IntegrationSettingMainId();
        IEnumerable<SelectListItem> Acc_VoucherType(string Type);
        IEnumerable<SelectListItem> PrdUnitId();
        IEnumerable<SelectListItem> EmpId();
        IEnumerable<SelectListItem> CustomerId();
        IEnumerable<SelectListItem> SupplierId();
        IEnumerable<SelectListItem> VoucherTranGroupArray();
        IEnumerable<SelectListItem> VoucherTranGroupId();
        IEnumerable<SelectListItem> VoucherTranGroupIdRow();
        IEnumerable<SelectListItem> Country();
        IEnumerable<SelectListItem> CountryIdVoucher();
        IEnumerable<SelectListItem> AccountMain();
        IEnumerable<SelectListItem> Account();
        IEnumerable<SelectListItem> Account1();

        string PrintCheck(int? id, string type);
        string Print1(int? id, string type);
        Acc_VoucherMain GetVoucherMain(int VoucherId = 0);
        Acc_VoucherMain vouchersamplemodel(int FiscalMonthId = 0, int? IntegrationSettingMainId = 0);
        List<VoucherTranGroup> VoucherTranGroupList();
    }
}
