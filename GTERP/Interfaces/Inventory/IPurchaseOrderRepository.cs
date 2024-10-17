using DataTablesParser;
using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Inventory
{
    public interface IPurchaseOrderRepository:IBaseRepository<PurchaseOrderMain>
    {
        IEnumerable<SelectListItem> Userlist();
        IEnumerable<SelectListItem> DeptId();
        IEnumerable<SelectListItem> SectId();
        IEnumerable<SelectListItem> CurrencyId();
        IEnumerable<SelectListItem> PaymentTypeId();
        IEnumerable<SelectListItem> PrdUnitId();
        IEnumerable<SelectListItem> PurReqId();
        IEnumerable<SelectListItem> SupplierId();
        IEnumerable<SelectListItem> DistrictId();
        IQueryable<PurchaseOrderResult> parser();
        PurchaseOrderMain PurchaseOrderMain(int? id);
        List<PurchaseOrderDetailsModel> GetPurchaseRequisitionDataById(int? PurReqId);

        PurchaseOrderMain FindByIdPMain(int id);
        string PrintPurchaseOrder(int? id, string type);
        PurchaseOrderMain PurchaseOrderMains(int id);
        void CreateSupplier(Supplier supplier);
        void UpdateSupplier(Supplier supplier);
        void CreatePurchaseOrderSub(PurchaseOrderSub purchaseOrderSub);
        void UpdatePurchaseOrderSub(PurchaseOrderSub purchaseOrderSub);

        PurchaseOrderMain duplicateDocument(PurchaseOrderMain purchaseOrderMain);
        Acc_FiscalMonth FiscalMonth(PurchaseOrderMain purchaseOrderMain);
        Acc_FiscalYear FiscalYear(PurchaseOrderMain purchaseOrderMain);
        HR_ProcessLock LockCheck(PurchaseOrderMain purchaseOrderMain);
    }
}
