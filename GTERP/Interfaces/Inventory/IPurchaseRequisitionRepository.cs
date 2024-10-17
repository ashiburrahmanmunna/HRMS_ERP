using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Interfaces.Inventory
{
    public interface IPurchaseRequisitionRepository
    {
        List<AspnetUserList> IndexList();
        IQueryable<PurchaseReQuisitionResult> Get();
        IQueryable<PurchaseReQuisitionResult> QueryTest(string UserList);
        IQueryable<PurchaseReQuisitionResult> QueryTestElse();
        PurchaseRequisitionMain Details(int? id);

        void UpdateProduct(Product product);
        void AddProduct(Product product);
        Product getProduct(int id);
        Unit getUnit(int id);
        void SavePurchaseElse(PurchaseRequisitionMain purchaseRequisitionMain);
        void EditRequest(PurchaseRequisitionMain purchaseRequisitionMain);
        IEnumerable<SelectListItem> ProductList();
        IEnumerable<SelectListItem> ProductList2(int? id);
        void DeletePrSub(int prsubid);
        PurchaseRequisitionMain Edit(int? id);
        void UpdatePurchase(PurchaseRequisitionMain model);

        IEnumerable<SelectListItem> ApprovedByEmpId(PurchaseRequisitionMain purchaseRequisitionMain);
        IEnumerable<SelectListItem> PrdUnitId(PurchaseRequisitionMain purchaseRequisitionMain);
        IEnumerable<SelectListItem> PurposeId(PurchaseRequisitionMain purchaseRequisitionMain);
        IEnumerable<SelectListItem> RecommendedEmpId(PurchaseRequisitionMain purchaseRequisitionMain);
        IEnumerable<SelectListItem> ProductId();
        IEnumerable<SelectListItem> SectId(PurchaseRequisitionMain purchaseRequisitionMain);
        void DeletePurchase(int? id);
        string Print(int? id, string type);

    }

}
