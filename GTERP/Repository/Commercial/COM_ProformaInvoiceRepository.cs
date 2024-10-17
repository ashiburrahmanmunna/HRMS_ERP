using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static GTERP.ViewModels.CommercialVM;

namespace GTERP.Repository.Commercial
{
    public class COM_ProformaInvoiceRepository : BaseRepository<COM_ProformaInvoice>, ICOM_ProformaInvoiceRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<COM_ProformaInvoiceRepository> _logger;

        public COM_ProformaInvoiceRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<COM_ProformaInvoiceRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> AspNetUserList()
        {
            List<AspnetUserList> AspNetUserList = new List<AspnetUserList>();
            return new SelectList(AspNetUserList, "UserId", "UserName");
        }

        public IEnumerable<SelectListItem> BankAccountId()
        {
            return new SelectList(_context.BankAccountNos, "BankAccountId", "BankAccountNumber");
        }


        public void Create(int? supplierid, int? Flag)
        {
            var userid = _httpcontext.HttpContext.Session.GetString("userid");

            List<Temp_COM_ProformaInvoice> InvoiceList = _context.Temp_COM_ProformaInvoices.Where(m => m.userid == userid.ToString()).ToList();
            List<COM_ProformaInvoice> InvoiceListForView = new List<COM_ProformaInvoice>();

            int supplierinformationid = 0;
            int SisterConcernCompanyid = 0;
            int currencyid = 0;
            string unitmasterid = "Pcs";


            foreach (Temp_COM_ProformaInvoice item in InvoiceList)
            {

                SupplierInformation supplier = _context.SupplierInformations.Where(m => m.SupplierName.ToUpper().Contains(item.Supplier)).FirstOrDefault();
                if (supplier != null)
                {
                    supplierinformationid = supplier.ContactID;
                }

                SisterConcernCompany company = _context.SisterConcernCompany.Where(m => m.CompanyName.ToUpper().Contains(item.Company)).FirstOrDefault();
                if (company != null)
                {
                    SisterConcernCompanyid = company.SisterConcernCompanyId;
                }

                Currency currency = _context.Currency.Where(m => m.CurCode.ToUpper().Contains(item.Currency)).FirstOrDefault();
                if (currency != null)
                {
                    currencyid = currency.CurrencyId;
                }

                UnitMaster unitmaster = _context.UnitMasters.Where(m => m.UnitMasterId.ToUpper().Contains(item.Unit)).FirstOrDefault();
                if (unitmaster != null)
                {
                    unitmasterid = unitmaster.UnitMasterId;
                }

                COM_ProformaInvoice COM_CNFBillImportDetail = new COM_ProformaInvoice
                {
                    PINo = item.PINo,
                    PIDate = (DateTime.Parse(item.PIDate.ToString()).Date),
                    CommercialCompanyId = SisterConcernCompanyid,
                    CurrencyId = currencyid,
                    SupplierId = supplierinformationid,
                    Company = company,
                    Currency = currency,
                    SupplierInformation = supplier,
                    UnitMaster = unitmaster,
                    ImportPONo = item.ImportPONo,
                    FileNo = item.FileNo,
                    LCAF = item.LCAF,
                    ItemGroupName = item.ItemGroupName,
                    ItemDescription = item.ItemDescription,
                    UnitMasterId = item.Unit, //"Pcs"
                    ImportQty = decimal.Parse(item.ImportQty),
                    ImportRate = decimal.Parse(item.ImportRate),
                    TotalValue = decimal.Parse(item.TotalValue),
                    IsDelete = false
                };
                InvoiceListForView.Add(COM_CNFBillImportDetail);
            }

        }

        public IEnumerable<SelectListItem> CurrencyId()
        {
            return new SelectList(_context.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");
        }

        public IEnumerable<SelectListItem> DayListId()
        {
            return new SelectList(_context.DayLists, "DayListId", "DayListName");
        }

        public IEnumerable<SelectListItem> EmployeeId()
        {
            return new SelectList(_context.Employee, "EmployeeId", "EmployeeName");
        }

        public IEnumerable<SelectListItem> GroupLCId()
        {
            return new SelectList(_context.COM_GroupLC_Mains, "GroupLCId", "GroupLCRefName");
        }

        public IEnumerable<SelectListItem> ItemDescArray()
        {
            return new MultiSelectList(_context.ItemDescs, "ItemDescId", "ItemDescName");
        }

        public IEnumerable<SelectListItem> ItemDescId()
        {
            return new SelectList(_context.ItemDescs, "ItemDescId", "ItemDescName");
        }

        public IEnumerable<SelectListItem> ItemGroupId()
        {
            return new SelectList(_context.ItemGroups, "ItemGroupId", "ItemGroupName");
        }

        public IEnumerable<SelectListItem> LienBankAccountId()
        {
            return new SelectList(_context.BankAccountNoLienBanks, "LienBankAccountId", "BankAccountNumber");
        }

        public IEnumerable<SelectListItem> OpeningBankId()
        {
            return new SelectList(_context.OpeningBanks, "OpeningBankId", "OpeningBankName");
        }

        public IEnumerable<SelectListItem> PaymentTermsId()
        {
            return new SelectList(_context.PaymentTerms, "PaymentTermsId", "PaymentTermsName");
        }

        public void PIDailyReceiving(int? supplierid, string FromDate, string ToDate)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            if (FromDate == null || FromDate == "")
            {
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

            }
            if (ToDate == null || ToDate == "")
            {
            }
            else
            {
                dtTo = Convert.ToDateTime(ToDate);

            }

            if (supplierid == null)
            {
                supplierid = 0;
            }
        }

        public List<PIDailyReceivingModel> ProductSerialresult()
        {
            List<PIDailyReceivingModel> ProductSerialresult = new List<PIDailyReceivingModel>();
            return ProductSerialresult;
        }

        public List<COM_ProformaInvoice> ProformaInvoiceList(int? supplierid, string UserList, string FromDate, string ToDate)
        {
            DateTime dtFrom = Convert.ToDateTime(DateTime.Now.Date);
            DateTime dtTo = Convert.ToDateTime(DateTime.Now.Date);

            if (FromDate == null || FromDate == "")
            {
            }
            else
            {
                dtFrom = Convert.ToDateTime(FromDate);

            }
            if (ToDate == null || ToDate == "")
            {
            }
            else
            {
                dtTo = Convert.ToDateTime(ToDate);

            }

            if (UserList == null)
            {
                UserList = _httpcontext.HttpContext.Session.GetString("userid");
            }

            if (supplierid == null)
            {
                supplierid = 0;
            }

            var x = _context.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString()) && (p.PIDate >= dtFrom && p.PIDate <= dtTo)).ToList();

            if (supplierid == 0)
            {
                if (UserList == "")
                {
                    x = _context.COM_ProformaInvoices.Where(p => (p.PIDate >= dtFrom && p.PIDate <= dtTo)).ToList();

                }
                else
                {
                    x = _context.COM_ProformaInvoices.Where(p => (p.PIDate >= dtFrom && p.PIDate <= dtTo) && (p.UserId == UserList.ToString())).ToList();

                }

            }
            else
            {
                if (UserList == "")
                {
                    x = _context.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString()) && (p.PIDate >= dtFrom && p.PIDate <= dtTo)).ToList();
                }
                else
                {
                    x = _context.COM_ProformaInvoices.Where(p => (p.SupplierId.ToString() == supplierid.ToString()) && (p.PIDate >= dtFrom && p.PIDate <= dtTo) && (p.UserId.ToString() == UserList.ToString())).ToList();

                }

            }

            return x;
        }

        public IEnumerable<SelectListItem> SisterConcernCompanyId()
        {
            return new SelectList(_context.SisterConcernCompany, "SisterConcernCompanyId", "CompanyName");
        }

        public IEnumerable<SelectListItem> SupplierId()
        {
            return new SelectList(_context.SupplierInformations, "ContactId", "SupplierName");
        }

        public IEnumerable<SelectListItem> UnitMasterId()
        {
            return new SelectList(_context.UnitMasters, "UnitMasterId", "UnitMasterId");
        }

        public void CreatePost(List<COM_ProformaInvoice> COM_ProformaInvoices)
        {
            var text = "";
            foreach (var item in COM_ProformaInvoices)
            {
                if (item.PIId > 0)
                {
                    if (item.IsDelete == false)
                    {
                        _context.Entry(item).State = EntityState.Modified;
                        item.DateUpdated = DateTime.Now;
                        item.ComId = _httpcontext.HttpContext.Session.GetString("comid");

                        var asdf = _context.COM_ProformaInvoice_Subs.Where(x => x.PIId == item.PIId);
                        _context.COM_ProformaInvoice_Subs.RemoveRange(asdf);

                        item.COM_ProformaInvoice_Subs = new List<COM_ProformaInvoice_Sub>();
                        for (int i = 0; i < item.ItemDescArray.Length; i++)
                        {
                            text += item.ItemDescArray[i] + ",";

                            COM_ProformaInvoice_Sub COM_ProformaInvoice_Subs = new COM_ProformaInvoice_Sub { ItemDescId = int.Parse(item.ItemDescArray[i]) }; //InvoiceId = 1,

                            item.COM_ProformaInvoice_Subs.Add(COM_ProformaInvoice_Subs);
                        }
                    }
                    else
                    {
                        _context.Entry(item).State = EntityState.Deleted;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    item.DateAdded = DateTime.Now;
                    item.ComId = _httpcontext.HttpContext.Session.GetString("comid");
                    item.UserId = _httpcontext.HttpContext.Session.GetString("userid");
                    text = "";
                    if (item.IsDelete == false)
                    {
                        item.COM_ProformaInvoice_Subs = new List<COM_ProformaInvoice_Sub>();
                        for (int i = 0; i < item.ItemDescArray.Length; i++)
                        {
                            text += item.ItemDescArray[i] + ",";

                            COM_ProformaInvoice_Sub COM_ProformaInvoice_Subs = new COM_ProformaInvoice_Sub { ItemDescId = int.Parse(item.ItemDescArray[i]) }; //InvoiceId = 1,

                            item.COM_ProformaInvoice_Subs.Add(COM_ProformaInvoice_Subs);
                        }
                        item.ItemDescList = text.TrimEnd(',');

                        _context.COM_ProformaInvoices.Add(item);

                        _context.SaveChanges();
                    }
                    else
                    {
                    }
                }
            }
        }

    }
}
