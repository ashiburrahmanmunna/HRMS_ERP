using GTERP.Models;
using GTERP.ViewModels;
using System.Linq;
using System.Threading.Tasks;

using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Inventory
{
    public interface IStoreRequisitionRepository
    {

        dynamic GetUserList();
        Task<StoreRequisitionMain> GetStoreRequisitionDetails(int? id);
        StoreRequisitionMain GetUserwiseSRR();

        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedical();
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByUser(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForMedicalByDate(string FromDate, string ToDate);

        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProduction();
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByUser(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsForProductionByDate(string FromDate, string ToDate);

        IQueryable<StoreReQuisitionResult> storeReQuisitionResults();
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByCustomer(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByUser(string UserList, string FromDate, string ToDate, string CustomerList);
        IQueryable<StoreReQuisitionResult> storeReQuisitionResultsByDate(string FromDate, string ToDate);

        string PrintReport(int? id, string type);


        StoreRequisitionMain StoreRequisitionMain(int? id);
        IEnumerable<SelectListItem> ApprovedByEmpId(StoreRequisitionMain storeRequisitionMain);
        IEnumerable<SelectListItem> DepartmentId(StoreRequisitionMain storeRequisitionMain);
        IEnumerable<SelectListItem> PrdUnitId(StoreRequisitionMain storeRequisitionMain);
        IEnumerable<SelectListItem> PurposeId(StoreRequisitionMain storeRequisitionMain);
        IEnumerable<SelectListItem> RecommenedByEmpId(StoreRequisitionMain storeRequisitionMain);
        StoreRequisitionMain FindById(int id);
        StoreRequisitionMain LastStorereq();
        StoreRequisitionMain UserWiseSRR();
    }
}
