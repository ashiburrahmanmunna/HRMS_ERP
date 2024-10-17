using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.Inventory
{
    public interface IGoodsReceiveRepository
    {
        int ProductSave(Product product);

        IQueryable<GoodsReceiveResult> GoodsReceiveResults();
        IQueryable<GoodsReceiveResult> GoodsReceiveResultsByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<GoodsReceiveResult> GoodsReceiveResultsByCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<GoodsReceiveResult> GoodsReceiveResultsByUser(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<GoodsReceiveResult> GoodsReceiveResultsByDate(string FromDate, string ToDate);
        List<PurchaseOrderMain> GetData();
        List<GoodsReceiveDetailsModel> PurchaseOrderSubDataByPOMId(int id);
        Task<GoodsReceiveMain> GetGoodsReceiveMainById(int? id);
        IEnumerable<SelectListItem> AccountMain();
        GoodsReceiveMain LastGoodsReceiveMain();
        IEnumerable<SelectListItem> PrdUnit();
        IEnumerable<SelectListItem> CurrencyId();
        IEnumerable<SelectListItem> PaymentTypeId();
        IEnumerable<SelectListItem> WarehouseId();
        IEnumerable<SelectListItem> PurposeId();
        IEnumerable<SelectListItem> ProductId();
        IEnumerable<SelectListItem> CategoryId();
        IEnumerable<SelectListItem> ProductByCategoryId(int? id);
        Task<GoodsReceiveMain> GetDelete(int? id);
        void DeleteGoodReceive(int? id);
        IEnumerable<SelectListItem> PurOrderMainId(int? id);
        IEnumerable<SelectListItem> PurReqId(int? id);
        IEnumerable<SelectListItem> SupplierId(int? id);
        IEnumerable<SelectListItem> CurrencyId2(GoodsReceiveMain goodsReceiveMain);
        IEnumerable<SelectListItem> PaymentId2(GoodsReceiveMain goodsReceiveMain);
        IEnumerable<SelectListItem> PrdUnitId2(GoodsReceiveMain goodsReceiveMain);
        IEnumerable<SelectListItem> DepartmentList2(GoodsReceiveMain goodsReceiveMain);
        IEnumerable<SelectListItem> WareHouseId2();
        void Update(GoodsReceiveMain goodsReceiveMain);
        GoodsReceiveMain FindById(int? id);
        GoodsReceiveMain GetGoodsReceiveMain2(int? id);
        IEnumerable<SelectListItem> PurOrderMainId2(GoodsReceiveMain goodsReceiveMain);
        IEnumerable<SelectListItem> PurReqId2(GoodsReceiveMain goodsReceiveMain);
        IEnumerable<SelectListItem> SupplierId2(GoodsReceiveMain goodsReceiveMain);
        void CreateIfElsePart(GoodsReceiveMain goodsReceiveMain);
        HR_ProcessLock LockCheck(GoodsReceiveMain goodsReceiveMain);
        Acc_FiscalMonth FiscalMonth(GoodsReceiveMain goodsReceiveMain);
        Acc_FiscalYear FiscalYear(GoodsReceiveMain goodsReceiveMain);

        IEnumerable<SelectListItem> PurOrderMainId3();
        IEnumerable<SelectListItem> SupplierId3();
        IEnumerable<SelectListItem> PurReqId3();
        Task<GoodsReceiveMain> DuplicateData(GoodsReceiveMain goodsReceiveMain);


        string printGoodsReceive(int? id, string type);
        string GrrSummaryReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId);
        string GrrDetailsReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId);
        string PrintGrrVoucherLocal(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId);
        string PrintGrrVoucherForeign(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId);
        string PrintMissingSequence(string rptFormat, string action, string Type, string FromNo, string ToNo, int PrdUnitId);
    }
}
