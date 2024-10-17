using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.BLL
{
    public class POSRepository
    {
        //OK NOW test
        private readonly GTRDBContext db;
        private IHttpContextAccessor _httpContextAccessor;


        public POSRepository(GTRDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            _httpContextAccessor = httpContextAccessor;

        }


        //private string comid = HttpContext.Current.Session["comid"].ToString();
        //private string userid = HttpContext.Current.Session["userid"].ToString();

        public List<Product> GetProducts(string comid)
        {
            return db.Products.Where(c => c.ProductId > 0 && c.comid == comid).ToList();
        }


        public List<SalesType> GetSalesType(string comid)
        {
            return db.SalesType.ToList();
        }

        public List<Customer> GetCustomer(string comid)
        {
            return db.Customers.Where(c => c.CustomerId > 0 && c.comid == comid).ToList();
        }

        public List<Category> GetCategory(string comid)
        {
            return db.Categories.Where(c => c.CategoryId > 0 && c.ComId == comid).ToList();
        }


        public Task<List<Country>> GetCountryAsync()
        {

            var country = db.Countries;
            return country.ToListAsync();
        }
        public List<Country> GetCountry()
        {
            return db.Countries.ToList();
        }



        public List<ProductSerialtemp> GetSerialNoProcedure(string comid, string userid)
        {

            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@comid", comid);
            sqlParameter[1] = new SqlParameter("@userid", userid);
            return Helper.ExecProcMapTList<ProductSerialtemp>("POS_prcgetSerialNo", sqlParameter);
            //return db.Database.SqlQuery<ProductSerialtemp>("[prcgetSerialNo]  @comid, @userid", new SqlParameter("comid", comid), new SqlParameter("userid", HttpContext.Current.Session["userid"])).ToList();

        }


        public List<PaymentType> GetPaymentTypes(string comid)
        {
            return db.PaymentTypes.ToList();

        }

        public List<Acc_ChartOfAccount> GetChartOfAccountCash(string comid)
        {
            return db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccName.Contains("CASH") && c.AccType == "L" && ((c.AccName.Contains("103070") || (c.AccName.Contains("1-1-10-"))))).ToList();

        }


        public List<Acc_ChartOfAccount> GetChartOfAccountCashAndBank(string comid)
        {
            return db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccName.Contains("CASH") && c.AccType == "L").ToList();

        }

        public List<Acc_ChartOfAccount> GetChartOfAccountLedger(string comid)
        {
            return db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccType == "L").ToList();

        }
        public List<Acc_ChartOfAccount> GetChartOfAccountGroup(string comid)
        {
            return db.Acc_ChartOfAccounts.Where(c => c.ComId == comid && c.AccType == "G").ToList();

        }


        public IEnumerable<SalesType> GetSalesType()
        {
            return db.SalesType.ToList();
        }
        public IEnumerable<Supplier> GetSupplier(string comid)
        {
            return db.Suppliers.Where(c => c.SupplierId > 0 && c.ComId == comid);
        }

        public IEnumerable<Warehouse> GetWarehouse(string comid)
        {
            return db.Warehouses.Where(x => x.ComId == comid && x.WhType == "L");
        }
        public List<SalesMain> GetSalesList(string comid)
        {
            return db.SalesMains.Where(c => c.SalesId > 0 && c.comid == comid).ToList();
        }
        public List<PurchaseMain> GetPurchaseList(string comid)
        {
            return db.PurchaseMains.Where(c => c.PurchaseId > 0 && c.comid == comid).ToList();
        }
        public IEnumerable<ProductSerial> GetProductSerial(string comid)
        {
            return db.ProductSerial.Where(c => c.ProductSerialId > 0 && c.ComId == comid);

        }
        public IEnumerable<TermsMain> GetTerms(string comid)
        {
            return db.TermsMain.Where(c => c.comid == comid);

        }




        public IEnumerable<PaymentType> GetPaymentTypes()
        {
            return db.PaymentTypes;

        }


        public void SetProductInSession()
        {
            string comid = _httpContextAccessor.HttpContext.Session.GetString("comid");
            var products = _httpContextAccessor.HttpContext.Session.GetObject<List<Product>>("productlist");


            if (products == null)
            {
                var productList = db.Products.Where(x => x.AccIdInventory != x.AccIdConsumption).Where(x => x.comid == comid && x.AccIdConsumption > 0)
                    .Select(p => new SessionProduct
                    {
                        ProductId = p.ProductId,
                        ProductCode = p.ProductCode,
                        ProductName = p.ProductName + " - [ " + p.ProductCode + " ]",
                        CategoryId = p.CategoryId,
                        ComId = p.comid
                    })
                    .ToList();
                _httpContextAccessor.HttpContext.Session.SetObject("productlist", productList);
            }
            else
            {
                var productCount = db.Products.Where(x => x.AccIdInventory != x.AccIdConsumption).Where(x => x.comid == comid && x.AccIdConsumption > 0).Count();
                if (products.Count() != productCount)
                {
                    var productList = db.Products.Where(x => x.AccIdInventory != x.AccIdConsumption).Where(x => x.comid == comid && x.AccIdConsumption > 0)
                      .Select(p => new SessionProduct
                      {
                          ProductId = p.ProductId,
                          ProductCode = p.ProductCode,
                          ProductName = p.ProductName + " - [ " + p.ProductCode + " ]",
                          CategoryId = p.CategoryId,
                          ComId = p.comid
                      })
                      .ToList();
                    _httpContextAccessor.HttpContext.Session.SetObject("productlist", productList);
                }
            }
        }


















    }

    public class ProductSerialtemp
    {
        public int ProductId { get; set; }

        public int ProductSerialId { get; set; }
        public string ProductSerialNo { get; set; }
    }
}
