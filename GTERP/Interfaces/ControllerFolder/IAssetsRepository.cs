using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace GTERP.Interfaces.ControllerFolder
{
    public interface IAssetsRepository : IBaseRepository<Asset>
    {
        IEnumerable<SelectListItem> Items();
        IEnumerable<SelectListItem> AssetCategoryId();
        IEnumerable<SelectListItem> ComId();
        IEnumerable<SelectListItem> LocationId();
        IEnumerable<SelectListItem> Custodian();
        IEnumerable<SelectListItem> CategoryId();
        IEnumerable<SelectListItem> PurchaseType();
        IEnumerable<SelectListItem> Supplier();
        IEnumerable<SelectListItem> DepreciationMethod();
        IEnumerable<SelectListItem> Department();
        IEnumerable<SelectListItem> AssetName();
        IEnumerable<SelectListItem> FoDId();
        IEnumerable<SelectListItem> Employees();
    }
}
