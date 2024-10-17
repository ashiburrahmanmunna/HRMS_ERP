using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.BLL
{
    public class PermissionLevel
    {
        private readonly GTRDBContext db;
        private IHttpContextAccessor _httpContextAccessor;
        private UserPermission permission;


        public PermissionLevel(GTRDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            _httpContextAccessor = httpContextAccessor;
            permission = _httpContextAccessor.HttpContext.Session.GetObject<UserPermission>("userpermission");

        }

        public IQueryable<PrdUnit> GetPrdUnit()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    return db.PrdUnits.Where(p => p.ComId == comid && p.PrdUnitShortName.Contains("UN1") || p.PrdUnitShortName.Contains("UN2"));
                }
                else if (permission.IsMedical)
                {
                    return db.PrdUnits.Where(p => p.ComId == comid && p.PrdUnitShortName.Contains("MED"));
                }
                else
                {
                    return db.PrdUnits.Where(p => p.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }

        public IQueryable<string> GetModuleUser()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsAll)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsAll == true).Select(u => u.AppUserId);
                }
                else if (permission.IsProduction)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsProduction == true).Select(u => u.AppUserId);
                }
                else if (permission.IsMedical)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsMedical == true).Select(u => u.AppUserId);
                }
                else if (permission.IsStoreAndAccounts)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsStoreAndAccounts == true).Select(u => u.AppUserId);
                }
                else if (permission.IsStore)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsStore == true).Select(u => u.AppUserId);
                }
                else if (permission.IsStore)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsStore == true).Select(u => u.AppUserId);
                }
                else if (permission.IsBillManagement)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsBillManagement == true).Select(u => u.AppUserId);
                }
                else if (permission.IsCashbankMangement)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsCashbankMangement == true).Select(u => u.AppUserId);
                }
                else if (permission.IsGeneralAccouonts)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsGeneralAccouonts == true).Select(u => u.AppUserId);
                }
                else if (permission.IsHRAndPayroll)
                {
                    return db.UserPermission.Where(u => u.ComId == comid && u.IsHRAndPayroll == true).Select(u => u.AppUserId);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public IQueryable<Warehouse> GetWarehouse()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsAll)
                {
                    return db.Warehouses.Where(w => w.ComId == comid);
                }
                else if (permission.IsStore || permission.IsStoreAndAccounts)
                {
                    return db.Warehouses.Where(w => w.ComId == comid && w.IsSubWarehouse == false);
                }
                else if (permission.IsProduction)
                {
                    return db.Warehouses.Where(w => w.ComId == comid && w.IsProductionWarehouse);
                }
                else if (permission.IsMedical)
                {
                    return db.Warehouses.Where(w => w.ComId == comid && w.IsMedicalWarehouse);
                }
                else
                {
                    return db.Warehouses.Take(0).Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }

        public IQueryable<StoreRequisitionMain> GetSRR()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    return db.StoreRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("UN1") || w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else if (permission.IsMedical)
                {
                    return db.StoreRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("MED"));
                }
                else if (permission.IsStore || permission.IsStoreAndAccounts)
                {
                    return db.StoreRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid
                        && !w.PrdUnit.PrdUnitShortName.Contains("MED")
                        && !w.PrdUnit.PrdUnitShortName.Contains("UN1") && !w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else
                {
                    return db.StoreRequisitionMain.Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }

        public IQueryable<StoreRequisitionMain> GetSRRList() // for SR index list data
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    return db.StoreRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("UN1") || w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else if (permission.IsMedical)
                {
                    var moduleUser = GetModuleUser().ToList();
                    return db.StoreRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && (w.PrdUnit.PrdUnitShortName.Contains("MED") || moduleUser.Contains(w.UserId)));
                }
                else if (permission.IsStore || permission.IsStoreAndAccounts)
                {
                    var moduleUser = GetModuleUser().ToList();
                    return db.StoreRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid
                        && !w.PrdUnit.PrdUnitShortName.Contains("MED")
                        && !w.PrdUnit.PrdUnitShortName.Contains("UN1") && !w.PrdUnit.PrdUnitShortName.Contains("UN2")
                        //&& moduleUser.Contains(w.UserId)
                        );
                }
                else
                {
                    return db.StoreRequisitionMain.Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }

        public IQueryable<IssueMain> GetIssue()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    return db.IssueMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("UN1") || w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else if (permission.IsMedical)
                {
                    return db.IssueMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("MED"));
                }
                else if (permission.IsStore || permission.IsStoreAndAccounts)
                {
                    return db.IssueMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid
                        && !w.PrdUnit.PrdUnitShortName.Contains("MED")
                        && !w.PrdUnit.PrdUnitShortName.Contains("UN1") && !w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else
                {
                    return db.IssueMain.Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }

        public IQueryable<GoodsReceiveMain> GetGRR()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    return db.GoodsReceiveMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("UN1") || w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else if (permission.IsMedical)
                {
                    return db.GoodsReceiveMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("MED"));
                }
                else if (permission.IsStore || permission.IsStoreAndAccounts)
                {
                    return db.GoodsReceiveMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid
                        && !w.PrdUnit.PrdUnitShortName.Contains("MED")
                        && !w.PrdUnit.PrdUnitShortName.Contains("UN1") && !w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else
                {
                    return db.GoodsReceiveMain.Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }

        public IQueryable<PurchaseRequisitionMain> GetPR()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    return db.PurchaseRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("UN1") || w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else if (permission.IsMedical)
                {
                    return db.PurchaseRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("MED"));
                }
                else if (permission.IsStore || permission.IsStoreAndAccounts)
                {
                    return db.PurchaseRequisitionMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid
                        && !w.PrdUnit.PrdUnitShortName.Contains("MED")
                        && !w.PrdUnit.PrdUnitShortName.Contains("UN1") && !w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else
                {
                    return db.PurchaseRequisitionMain.Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }

        public IQueryable<PurchaseOrderMain> GetPO()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    return db.PurchaseOrderMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("UN1") || w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else if (permission.IsMedical)
                {
                    return db.PurchaseOrderMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid && w.PrdUnit.PrdUnitShortName.Contains("MED"));
                }
                else if (permission.IsStore || permission.IsStoreAndAccounts)
                {
                    return db.PurchaseOrderMain.Include(s => s.PrdUnit)
                        .Where(w => w.ComId == comid
                        && !w.PrdUnit.PrdUnitShortName.Contains("MED")
                        && !w.PrdUnit.PrdUnitShortName.Contains("UN1") && !w.PrdUnit.PrdUnitShortName.Contains("UN2"));
                }
                else
                {
                    return db.PurchaseOrderMain.Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }


        public IQueryable<Category> GetCategory()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            if (permission != null)
            {
                if (permission.IsProduction)
                {
                    var cat = new List<int>() { 18, 19 };
                    return db.Categories.Where(w => w.ComId == comid && cat.Contains(w.CategoryId));
                }
                else if (permission.IsMedical)
                {
                    var cat = new List<int>() { 35 };
                    return db.Categories.Where(w => w.ComId == comid && cat.Contains(w.CategoryId));
                }
                else
                {
                    return db.Categories.Where(w => w.ComId == comid);
                }
            }
            else
            {
                return null;
            }

        }







    }
}
