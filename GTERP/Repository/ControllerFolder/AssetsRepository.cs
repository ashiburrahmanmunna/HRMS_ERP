using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.ControllerFolder
{
    public class AssetsRepository : BaseRepository<Asset>, IAssetsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<AssetsRepository> _logger;


        public AssetsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<AssetsRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public IEnumerable<SelectListItem> AssetCategoryId()
        {
            return new SelectList(_context.Set<AssetCategory>(), "AssetCategoryId", "CatName");
        }

        public IEnumerable<SelectListItem> AssetName()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Asset.Where(x => x.ComId == comid), "AssetId", "AssetName");
        }

        public IEnumerable<SelectListItem> CategoryId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Categories.Where(c => c.ComId == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
        }

        public IEnumerable<SelectListItem> ComId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Companys.Where(x => x.CompanyCode == comid), "ComId", "CompanyName");
        }

        public IEnumerable<SelectListItem> Custodian()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid), "EmpId", "EmpName");
        }

        public IEnumerable<SelectListItem> Department()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName");
        }

        public IEnumerable<SelectListItem> DepreciationMethod()
        {
            return new SelectList(_context.DepreciationMethods, "DMId", "DMName");
        }

        public IEnumerable<SelectListItem> Employees()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(c => c.ComId == comid).Select(x => new { x.EmpId, Text = x.EmpCode + " -" + x.EmpName }), "EmpId", "Text");
        }

        public IEnumerable<SelectListItem> FoDId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.depreciationFrequencies.Where(x => x.ComId == comid).Select(x => new { x.FoDId, Text = x.Title + " -(" + x.CompoundingPeriod + ")" }), "FoDId", "Text");
        }

        public IEnumerable<SelectListItem> Items()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Products.Where(x => x.comid == comid), "ProductId", "ProductName");
        }

        public IEnumerable<SelectListItem> LocationId()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Location.Where(x => x.ComId == comid), "LId", "LocationName");
        }

        public IEnumerable<SelectListItem> PurchaseType()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PurchaseTypes.Where(x => x.ComId == comid), "PurchaseTypeId", "TypeName");
        }

        public IEnumerable<SelectListItem> Supplier()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName");
        }
    }
}
