using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static GTERP.ViewModels.CommercialVM;

namespace GTERP.Interfaces.Commercial
{
    public interface ICOM_ProformaInvoiceRepository : IBaseRepository<COM_ProformaInvoice>
    {
        List<COM_ProformaInvoice> ProformaInvoiceList(int? supplierid, string UserList, string FromDate, string ToDate);
        void PIDailyReceiving(int? supplierid, string FromDate, string ToDate);
        void Create(int? supplierid, int? Flag);
        void CreatePost(List<COM_ProformaInvoice> COM_ProformaInvoices);
        IEnumerable<SelectListItem> AspNetUserList();
        IEnumerable<SelectListItem> SupplierId();
        List<PIDailyReceivingModel> ProductSerialresult();
        IEnumerable<SelectListItem> SisterConcernCompanyId();
        IEnumerable<SelectListItem> CurrencyId();
        IEnumerable<SelectListItem> UnitMasterId();
        IEnumerable<SelectListItem> EmployeeId();
        IEnumerable<SelectListItem> ItemGroupId();
        IEnumerable<SelectListItem> ItemDescId();
        IEnumerable<SelectListItem> ItemDescArray();
        IEnumerable<SelectListItem> GroupLCId();
        IEnumerable<SelectListItem> PaymentTermsId();
        IEnumerable<SelectListItem> DayListId();
        IEnumerable<SelectListItem> OpeningBankId();
        IEnumerable<SelectListItem> BankAccountId();
        IEnumerable<SelectListItem> LienBankAccountId();

    }
}
