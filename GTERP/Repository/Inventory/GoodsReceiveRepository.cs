using GTERP.BLL;
using GTERP.Interfaces.Inventory;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.Inventory
{
    public class GoodsReceiveRepository : IGoodsReceiveRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUrlHelper _urlHelper;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        private readonly IConfiguration _configuration;
        public clsProcedure clsProc { get; }
        PermissionLevel PL;

        public GoodsReceiveRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IConfiguration configuration,
            PermissionLevel _pl
            )
        {
            _context = context;
            _httpContext = httpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _configuration = configuration;
            PL = _pl;
        }


        public int ProductSave(Product product)
        {

            if (product.ProductId > 0)
            {
                product.DateUpdated = DateTime.Now;
                product.comid = (_httpContext.HttpContext.Session.GetString("comid"));

                if (product.userid == null)
                {
                    product.userid = _httpContext.HttpContext.Session.GetString("userid");
                }
                product.useridUpdate = _httpContext.HttpContext.Session.GetString("userid");

                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();

                return 2;




            }
            else
            {
                product.userid = _httpContext.HttpContext.Session.GetString("userid");
                product.comid = (_httpContext.HttpContext.Session.GetString("comid"));
                product.DateAdded = DateTime.Now;
                product.ProductImage = null;

                _context.Products.Add(product);

                _context.SaveChanges();

                return 1;

            }
        }


        public IQueryable<GoodsReceiveResult> GoodsReceiveResults()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var query = from e in PL.GetGRR()
                                 .OrderByDescending(x => x.GRRMainId)
                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                        select new GoodsReceiveResult
                        {
                            GRRMainId = e.GRRMainId,
                            GRRNo = e.GRRNo,
                            GRRDate = e.GRRDate.ToString("dd-MMM-yy"),
                            GRRRef = e.GRRRef,
                            Department = e.vDepartment.DeptName,
                            PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",
                            PRNo = e.PurchaseRequisitionMain != null ? e.PurchaseRequisitionMain.PRNo : "",
                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                            ConvertionRate = e.ConvertionRate,
                            TotalGRRValue = e.TotalGRRValue,
                            //Deduction = e.Deduction,
                            NetGRRValue = e.NetGRRValue,
                            //SectName = e.Cat_Section != null ? e.Cat_Section.SectName : "",
                            PONo = e.PurchaseOrderMain.PONo,
                            ExpectedReciveDate = e.ExpectedReciveDate,
                            GateInHouseDate = e.GateInHouseDate,
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            //TermsAndCondition = e.TermsAndCondition,
                            Remarks = e.Remarks
                        };


            return query;
        }

        public IQueryable<GoodsReceiveResult> GoodsReceiveResultsByUserAndCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetGRR()
                        .Where(p => (p.GRRDate >= dtFrom && p.GRRDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))
                        //.Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.GRRMainId)
                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                        select new GoodsReceiveResult
                        {
                            GRRMainId = e.GRRMainId,
                            GRRNo = e.GRRNo,
                            GRRDate = e.GRRDate.ToString("dd-MMM-yy"),
                            GRRRef = e.GRRRef,
                            Department = e.vDepartment.DeptName,
                            PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",
                            PRNo = e.PurchaseRequisitionMain != null ? e.PurchaseRequisitionMain.PRNo : "",
                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                            ConvertionRate = e.ConvertionRate,
                            TotalGRRValue = e.TotalGRRValue,
                            //Deduction = e.Deduction,
                            NetGRRValue = e.NetGRRValue,
                            //SectName = e.Cat_Section != null ? e.Cat_Section.SectName : "",
                            PONo = e.PurchaseOrderMain.PONo,
                            ExpectedReciveDate = e.ExpectedReciveDate,
                            GateInHouseDate = e.GateInHouseDate,
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            //TermsAndCondition = e.TermsAndCondition,
                            Remarks = e.Remarks
                        };



            return query;
        }

        public IQueryable<GoodsReceiveResult> GoodsReceiveResultsByCustomer(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetGRR()
                        .Where(p => (p.GRRDate >= dtFrom && p.GRRDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        // .Where(p => p.CustomerId == int.Parse(CustomerList))

                        .OrderByDescending(x => x.GRRMainId)
                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                        select new GoodsReceiveResult
                        {
                            GRRMainId = e.GRRMainId,
                            GRRNo = e.GRRNo,
                            GRRDate = e.GRRDate.ToString("dd-MMM-yy"),
                            GRRRef = e.GRRRef,
                            Department = e.vDepartment.DeptName,
                            PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",
                            PRNo = e.PurchaseRequisitionMain != null ? e.PurchaseRequisitionMain.PRNo : "",
                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                            ConvertionRate = e.ConvertionRate,
                            TotalGRRValue = e.TotalGRRValue,
                            //Deduction = e.Deduction,
                            NetGRRValue = e.NetGRRValue,
                            //SectName = e.Cat_Section != null ? e.Cat_Section.SectName : "",
                            PONo = e.PurchaseOrderMain.PONo,
                            ExpectedReciveDate = e.ExpectedReciveDate,
                            //TermsAndCondition = e.TermsAndCondition,
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            //TermsAndCondition = e.TermsAndCondition,
                            Remarks = e.Remarks
                        };

            return query;
        }


        public IQueryable<GoodsReceiveResult> GoodsReceiveResultsByUser(string UserList, string FromDate, string ToDate, string CustomerList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetGRR()
                        .Where(p => (p.GRRDate >= dtFrom && p.GRRDate <= dtTo))
                        //.Where(p => p.userid == UserList)
                        .Where(p => p.UserId.ToLower().Contains(UserList.ToLower()))


                        .OrderByDescending(x => x.GRRMainId)
                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                        select new GoodsReceiveResult
                        {
                            GRRMainId = e.GRRMainId,
                            GRRNo = e.GRRNo,
                            GRRDate = e.GRRDate.ToString("dd-MMM-yy"),
                            GRRRef = e.GRRRef,
                            Department = e.vDepartment.DeptName,
                            PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",
                            PRNo = e.PurchaseRequisitionMain != null ? e.PurchaseRequisitionMain.PRNo : "",
                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                            ConvertionRate = e.ConvertionRate,
                            TotalGRRValue = e.TotalGRRValue,
                            //Deduction = e.Deduction,
                            NetGRRValue = e.NetGRRValue,
                            //SectName = e.Cat_Section != null ? e.Cat_Section.SectName : "",
                            PONo = e.PurchaseOrderMain.PONo,
                            ExpectedReciveDate = e.ExpectedReciveDate,
                            //TermsAndCondition = e.TermsAndCondition,
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            //TermsAndCondition = e.TermsAndCondition,
                            Remarks = e.Remarks
                        };


            return query;
        }

        public IQueryable<GoodsReceiveResult> GoodsReceiveResultsByDate(string FromDate, string ToDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

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


            var query = from e in PL.GetGRR()
                        .Where(p => (p.GRRDate >= dtFrom && p.GRRDate <= dtTo))

                        .OrderByDescending(x => x.GRRMainId)
                            //let ImagePath = e.ImagePath + e.ProductId + e.FileExtension
                        select new GoodsReceiveResult
                        {
                            GRRMainId = e.GRRMainId,
                            GRRNo = e.GRRNo,
                            GRRDate = e.GRRDate.ToString("dd-MMM-yy"),
                            GRRRef = e.GRRRef,
                            Department = e.vDepartment.DeptName,
                            PrdUnitName = e.PrdUnit != null ? e.PrdUnit.PrdUnitName : "",
                            PRNo = e.PurchaseRequisitionMain != null ? e.PurchaseRequisitionMain.PRNo : "",
                            SupplierName = e.Supplier != null ? e.Supplier.SupplierName : "",
                            TypeName = e.PaymentType != null ? e.PaymentType.TypeName : "",
                            CurCode = e.Currency != null ? e.Currency.CurCode : "",
                            ConvertionRate = e.ConvertionRate,
                            TotalGRRValue = e.TotalGRRValue,
                            //Deduction = e.Deduction,
                            NetGRRValue = e.NetGRRValue,
                            //SectName = e.Cat_Section != null ? e.Cat_Section.SectName : "",
                            PONo = e.PurchaseOrderMain.PONo,
                            ExpectedReciveDate = e.ExpectedReciveDate,
                            //TermsAndCondition = e.TermsAndCondition,
                            Status = e.Status.ToString() != "0" ? "Posted" : "Not Posted",
                            //TermsAndCondition = e.TermsAndCondition,
                            Remarks = e.Remarks
                        };




            return query;
        }

        public List<PurchaseOrderMain> GetData()
        {
            var data = _context.PurchaseOrderMain.ToList();
            return data;
        }

        public List<GoodsReceiveDetailsModel> PurchaseOrderSubDataByPOMId(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");

            var quary = $"EXEC GoodsReceiveDetailsInformation '{comid}','{userid}',{id}";

            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@userid", userid);
            parameters[2] = new SqlParameter("@PurOrderMainId", id);
            List<GoodsReceiveDetailsModel> GoodsReceiveDetailsInformation = Helper.ExecProcMapTList<GoodsReceiveDetailsModel>("GoodsReceiveDetailsInformation", parameters);
            return GoodsReceiveDetailsInformation;
        }


        Task<GoodsReceiveMain> GetGoodsReceiveMainById(int? id)
        {

            var goodsReceiveMain = _context.GoodsReceiveMain
                .Include(g => g.vDepartment)
                .Include(g => g.Currency)
                .Include(g => g.PaymentType)
                .Include(g => g.PrdUnit)
                .Include(g => g.PurchaseOrderMain)
                .Include(g => g.PurchaseRequisitionMain)
                .Include(g => g.Supplier)
                .FirstOrDefaultAsync(m => m.GRRMainId == id);

            return goodsReceiveMain;
        }


        public IEnumerable<SelectListItem> AccountMain()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.AccCode.Contains("2-1-30") || p.AccCode.Contains("1-1-64") && p.AccType == "L" || p.AccCode.Contains("1-1-28") && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
        }


        GoodsReceiveMain LastGoodsReceiveMain()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var data = _context.GoodsReceiveMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(x => x.GRRMainId).FirstOrDefault();
            return data;
        }


        public IEnumerable<SelectListItem> PrdUnit()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");
        }



        GoodsReceiveMain IGoodsReceiveRepository.LastGoodsReceiveMain()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            return _context.GoodsReceiveMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(x => x.GRRMainId).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> CurrencyId()
        {
            return new SelectList(_context.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");
        }

        public IEnumerable<SelectListItem> PaymentTypeId()
        {
            return new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName");
        }

        public IEnumerable<SelectListItem> WarehouseId()
        {
            return new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
        }

        public IEnumerable<SelectListItem> PurposeId()
        {
            return new SelectList(_context.Purpose.Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName");
        }

        public IEnumerable<SelectListItem> ProductId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Products.Take(0).Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");
        }

        Task<GoodsReceiveMain> IGoodsReceiveRepository.GetGoodsReceiveMainById(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> CategoryId()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Categories.Where(c => c.ComId == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
        }

        public IEnumerable<SelectListItem> ProductByCategoryId(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
        }

        public void DeleteGoodReceive(int? id)
        {
            var goodsReceiveMain =  _context.GoodsReceiveMain.Find(id);
            var costcalculated = _context.CostCalculated.Where(x => x.GRRMainId == id).ToList();

            if (costcalculated != null)
            {
                foreach (var item in costcalculated)
                {
                    _context.CostCalculated.Remove(item);
                }
                _context.SaveChanges();
            }


            var grrprovisiondetails = _context.GoodsReceiveProvision.Where(x => x.GRRMainId == id).ToList();

            if (grrprovisiondetails != null)
            {
                foreach (var item in grrprovisiondetails)
                {
                    _context.GoodsReceiveProvision.Remove(item);
                }
                _context.SaveChanges();
            }


            if (goodsReceiveMain != null)
            {
                _context.GoodsReceiveMain.Remove(goodsReceiveMain);
                _context.SaveChanges();
            }
        }

        public Task<GoodsReceiveMain> GetDelete(int? id)
        {
            return  _context.GoodsReceiveMain
                            .Include(g => g.PurchaseOrderMain)
                            .ThenInclude(g => g.PurchaseOrderSub)
                            .Include(g => g.PurchaseRequisitionMain)
                            .ThenInclude(g => g.PurchaseRequisitionSub)

                     .Include(g => g.GoodsReceiveSub)
                            .ThenInclude(g => g.PurchaseOrderSub)
                            .ThenInclude(g => g.PurchaseRequisitionSub)
                            .ThenInclude(g => g.vProduct)
                            .ThenInclude(g => g.vProductUnit)

                     .Include(g => g.GoodsReceiveSub)
                            .ThenInclude(g => g.PurchaseOrderSub)
                            .ThenInclude(g => g.PurchaseRequisitionSub)
                            .ThenInclude(s => s.vProduct)
                            .ThenInclude(s => s.vPrimaryCategory)
                            .ThenInclude(s => s.vProducts)
                            .ThenInclude(s => s.vSubCategory)


                          .Include(g => g.GoodsReceiveSub)
                            .ThenInclude(g => g.vWarehouse)
                            .Include(g => g.GoodsReceiveProvision)
                             .ThenInclude(g => g.vChartOfAccounts)
                            .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefaultAsync();

        }

        public IEnumerable<SelectListItem> PurOrderMainId(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var goodsReceiveMain =  _context.GoodsReceiveMain
                           .Include(g => g.PurchaseOrderMain)
                           .ThenInclude(g => g.PurchaseOrderSub)
                           .Include(g => g.PurchaseRequisitionMain)
                           .ThenInclude(g => g.PurchaseRequisitionSub)

                    .Include(g => g.GoodsReceiveSub)
                           .ThenInclude(g => g.PurchaseOrderSub)
                           .ThenInclude(g => g.PurchaseRequisitionSub)
                           .ThenInclude(g => g.vProduct)
                           .ThenInclude(g => g.vProductUnit)

                    .Include(g => g.GoodsReceiveSub)
                           .ThenInclude(g => g.PurchaseOrderSub)
                           .ThenInclude(g => g.PurchaseRequisitionSub)
                           .ThenInclude(s => s.vProduct)
                           .ThenInclude(s => s.vPrimaryCategory)
                           .ThenInclude(s => s.vProducts)
                           .ThenInclude(s => s.vSubCategory)


                         .Include(g => g.GoodsReceiveSub)
                           .ThenInclude(g => g.vWarehouse)
                           .Include(g => g.GoodsReceiveProvision)
                            .ThenInclude(g => g.vChartOfAccounts)
                           .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefault();

            return new SelectList(_context.PurchaseOrderMain.Where(x => x.ComId == comid && x.Status == 1), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
        }

        public IEnumerable<SelectListItem> PurReqId(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var goodsReceiveMain = _context.GoodsReceiveMain
                          .Include(g => g.PurchaseOrderMain)
                          .ThenInclude(g => g.PurchaseOrderSub)
                          .Include(g => g.PurchaseRequisitionMain)
                          .ThenInclude(g => g.PurchaseRequisitionSub)

                   .Include(g => g.GoodsReceiveSub)
                          .ThenInclude(g => g.PurchaseOrderSub)
                          .ThenInclude(g => g.PurchaseRequisitionSub)
                          .ThenInclude(g => g.vProduct)
                          .ThenInclude(g => g.vProductUnit)

                   .Include(g => g.GoodsReceiveSub)
                          .ThenInclude(g => g.PurchaseOrderSub)
                          .ThenInclude(g => g.PurchaseRequisitionSub)
                          .ThenInclude(s => s.vProduct)
                          .ThenInclude(s => s.vPrimaryCategory)
                          .ThenInclude(s => s.vProducts)
                          .ThenInclude(s => s.vSubCategory)


                        .Include(g => g.GoodsReceiveSub)
                          .ThenInclude(g => g.vWarehouse)
                          .Include(g => g.GoodsReceiveProvision)
                           .ThenInclude(g => g.vChartOfAccounts)
                          .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefault();

            return new SelectList(_context.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
        }

        public IEnumerable<SelectListItem> SupplierId(int? id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var goodsReceiveMain = _context.GoodsReceiveMain
                          .Include(g => g.PurchaseOrderMain)
                          .ThenInclude(g => g.PurchaseOrderSub)
                          .Include(g => g.PurchaseRequisitionMain)
                          .ThenInclude(g => g.PurchaseRequisitionSub)

                   .Include(g => g.GoodsReceiveSub)
                          .ThenInclude(g => g.PurchaseOrderSub)
                          .ThenInclude(g => g.PurchaseRequisitionSub)
                          .ThenInclude(g => g.vProduct)
                          .ThenInclude(g => g.vProductUnit)

                   .Include(g => g.GoodsReceiveSub)
                          .ThenInclude(g => g.PurchaseOrderSub)
                          .ThenInclude(g => g.PurchaseRequisitionSub)
                          .ThenInclude(s => s.vProduct)
                          .ThenInclude(s => s.vPrimaryCategory)
                          .ThenInclude(s => s.vProducts)
                          .ThenInclude(s => s.vSubCategory)


                        .Include(g => g.GoodsReceiveSub)
                          .ThenInclude(g => g.vWarehouse)
                          .Include(g => g.GoodsReceiveProvision)
                           .ThenInclude(g => g.vChartOfAccounts)
                          .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefault();
            return new SelectList(_context.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
        }

        public IEnumerable<SelectListItem> CurrencyId2(GoodsReceiveMain goodsReceiveMain)
        {
            return new SelectList(_context.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
        }

        public IEnumerable<SelectListItem> PaymentId2(GoodsReceiveMain goodsReceiveMain)
        {
           return new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
        }

        public IEnumerable<SelectListItem> PrdUnitId2(GoodsReceiveMain goodsReceiveMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            
            return new SelectList(_context.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", goodsReceiveMain.PrdUnitId);
        }

        public IEnumerable<SelectListItem> WareHouseId2()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Warehouses.Where(x => x.ComId == comid && x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
        }

        public void Update(GoodsReceiveMain goodsReceiveMain)
        {
            _context.Update(goodsReceiveMain);
            _context.SaveChanges();
        }

        public IEnumerable<SelectListItem> DepartmentList2(GoodsReceiveMain goodsReceiveMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName", goodsReceiveMain.DeptId);
        }

        public GoodsReceiveMain FindById(int? id)
        {
            var data = _context.GoodsReceiveMain.Find(id);
            return data;
        }

        public GoodsReceiveMain GetGoodsReceiveMain2(int? id)
        {
            return _context.GoodsReceiveMain
                  .Include(g => g.GoodsReceiveSub)
                  .ThenInclude(g => g.vProduct)
                  .ThenInclude(g => g.vProductUnit)
                  .Include(g => g.GoodsReceiveProvision)
                  .ThenInclude(g => g.vChartOfAccounts)

                  .Where(g => g.GRRMainId == id).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> PurOrderMainId2(GoodsReceiveMain goodsReceiveMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
             return new SelectList(_context.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
        }

        public IEnumerable<SelectListItem> PurReqId2(GoodsReceiveMain goodsReceiveMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
        }

        public IEnumerable<SelectListItem> SupplierId2(GoodsReceiveMain goodsReceiveMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
        }

        public void CreateIfElsePart(GoodsReceiveMain goodsReceiveMain)
        {
            var result = "";

            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var pcname = _httpContext.HttpContext.Session.GetString("pcname");
            var nowdate = DateTime.Now;

            var duplicateDocument = _context.GoodsReceiveMain.Where(i => i.GRRNo == goodsReceiveMain.GRRNo && i.GRRMainId != goodsReceiveMain.GRRMainId && i.ComId == comid).FirstOrDefault();
           
            DateTime date = goodsReceiveMain.GRRDate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var activefiscalmonth = _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
           
            var activefiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
            

            var lockCheck = _context.HR_ProcessLock
             .Where(p => p.LockType.Contains("Store Lock") && p.DtDate.Date <= goodsReceiveMain.GRRDate.Date && p.DtToDate.Value.Date >= goodsReceiveMain.GRRDate.Date
                 && p.IsLock == true).FirstOrDefault();

            if (goodsReceiveMain.GRRMainId > 0)
            {
                goodsReceiveMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                goodsReceiveMain.FiscalYearId = activefiscalyear.FiscalYearId;

                goodsReceiveMain.ComId = comid;
                goodsReceiveMain.UserId = userid;
                goodsReceiveMain.PcName = pcname;
                goodsReceiveMain.DateUpdated = nowdate;
                var CurrentGoodsReceiveSub = _context.GoodsReceiveSub.Include(x => x.GoodsReceiveSubWarehouse).Where(p => p.GRRMainId == goodsReceiveMain.GRRMainId);


                foreach (GoodsReceiveSub ss in CurrentGoodsReceiveSub)
                {
                    _context.GoodsReceiveSub.Remove(ss);
                }
                _context.SaveChanges();


                /////for provision amount 
                var CurrentGoodsReceiveProvision = _context.GoodsReceiveProvision.Where(p => p.GRRMainId == goodsReceiveMain.GRRMainId);

                foreach (GoodsReceiveProvision prv in CurrentGoodsReceiveProvision)
                {
                    _context.GoodsReceiveProvision.Remove(prv);
                }
                _context.SaveChanges();



                foreach (GoodsReceiveSub item in goodsReceiveMain.GoodsReceiveSub)
                {
                    if (item.GRRSubId > 0)
                    {
                        foreach (GoodsReceiveSub ss in goodsReceiveMain.GoodsReceiveSub)
                        {
                            if (ss.GoodsReceiveSubWarehouse != null)
                            {
                                foreach (GoodsReceiveSubWarehouse sss in ss.GoodsReceiveSubWarehouse)
                                {
                                    sss.GRRSubWarehouseId = 0;
                                }
                            }
                            item.GRRSubId = 0;
                            _context.GoodsReceiveSub.Add(item);

                        }

                    }
                    else
                    {
                        _context.GoodsReceiveSub.Add(item);
                    }
                }
                //db.SaveChanges();




                foreach (GoodsReceiveProvision itemprovision in goodsReceiveMain.GoodsReceiveProvision)
                {
                    if (itemprovision.GRRProvisionId > 0)
                    {
                        itemprovision.GRRProvisionId = 0;
                        _context.GoodsReceiveProvision.Add(itemprovision);
                    }
                    else
                    {
                        _context.GoodsReceiveProvision.Add(itemprovision);
                    }
                }
                //db.SaveChanges();


                goodsReceiveMain.UpdateByUserId = userid;
                goodsReceiveMain.DateUpdated = nowdate;
                _context.Entry(goodsReceiveMain).State = EntityState.Modified;
                result = "2";
            }
            else
            {
                goodsReceiveMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                goodsReceiveMain.FiscalYearId = activefiscalyear.FiscalYearId;

                goodsReceiveMain.ComId = comid;
                goodsReceiveMain.UserId = userid;
                goodsReceiveMain.PcName = pcname;
                goodsReceiveMain.DateAdded = nowdate;


                _context.Add(goodsReceiveMain);
                result = "1";
            }
            _context.SaveChanges();

        }

        public HR_ProcessLock LockCheck(GoodsReceiveMain goodsReceiveMain)
        {
            return _context.HR_ProcessLock
                .Where(p => p.LockType.Contains("Store Lock") && p.DtDate.Date <= goodsReceiveMain.GRRDate.Date && p.DtToDate.Value.Date >= goodsReceiveMain.GRRDate.Date
                    && p.IsLock == true).FirstOrDefault();

        }

        public Acc_FiscalMonth FiscalMonth(GoodsReceiveMain goodsReceiveMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            DateTime date = goodsReceiveMain.GRRDate;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var activefiscalmonth = _context.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
            return activefiscalmonth;
        }

        public Acc_FiscalYear FiscalYear(GoodsReceiveMain goodsReceiveMain)
        {
            
            var activefiscalyear = _context.Acc_FiscalYears.Where(x => x.FYId == FiscalMonth(goodsReceiveMain).FYId).FirstOrDefault();
            return activefiscalyear;
        }

        public IEnumerable<SelectListItem> PurOrderMainId3()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PurchaseOrderMain.Where(x => x.ComId == comid && x.Status == 1), "PurOrderMainId", "PONo");
        }

        public IEnumerable<SelectListItem> SupplierId3()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName");
        }

        public IEnumerable<SelectListItem> PurReqId3()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo");
        }

        public Task<GoodsReceiveMain> DuplicateData(GoodsReceiveMain goodsReceiveMain)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.GoodsReceiveMain.Where(i => i.GRRNo == goodsReceiveMain.GRRNo && i.GRRMainId != goodsReceiveMain.GRRMainId && i.ComId == comid).FirstOrDefaultAsync();
        }

        public string printGoodsReceive(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = _httpContext.HttpContext.Session.GetString("comid");



            var reportname = "rptMRRForm";

            _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndGRRDetails] '" + comid + "', 'GRRNW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");


            string filename = _context.GoodsReceiveMain.Where(x => x.GRRMainId == id).Select(x => x.GRRNo).Single();
            _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            string DataSourceName = "DataSet1";
            _httpContext.HttpContext.Session.SetObject("rptList", postData);



            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";




            string redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });

            return redirectUrl;
        }

        public string GrrSummaryReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            if (action == "PrintGrrSummary")
            {
                reportname = "rptGRRSummary";
                filename = "GRRSummary" + DateTime.Now.Date.ToString();
                var query = "Exec [Inv_GRRSummary] '" + comid + "'";
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRSummary] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";


            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;


            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return redirectUrl;
        }

        public string GrrDetailsReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
          
            if (action == "PrintGrrDetails")
            {
        
                reportname = "rptGRRDetails";
                filename = "GrrDetails" + DateTime.Now.Date.ToString();
                var query = "Exec [Inv_GRRDetails] '" + comid + "'";
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRDetails] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return redirectUrl;
        }

        public string PrintGrrVoucherLocal(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (action == "PrintGrrVoucherLocal")
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rptGRRVoucher_Local";
                filename = "GRRVoucher_Local" + DateTime.Now.Date.ToString();
                var query = "Exec [Inv_GRRVoucher_Local] '" + comid + "'";
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRVoucher_Local] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";

           
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            var ConstrName = "ApplicationServices";
            

            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return redirectUrl;
        }

        public string PrintGrrVoucherForeign(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (action == "PrintGrrVoucherForeign")
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rptGRRVoucher_Foreign";
                filename = "GRRVoucher_Foreign" + DateTime.Now.Date.ToString();
                var query = "Exec [Inv_GRRVoucher_Foreign] '" + comid + "'";

                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRVoucher_Foreign] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";

            //HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            var ConstrName = "ApplicationServices";
            //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            //redirectUrl = callBackUrl;


            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });

            return redirectUrl;
        }

        public string PrintMissingSequence(string rptFormat, string action, string Type, string FromNo, string ToNo, int PrdUnitId)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");

            var reportname = "";
            var filename = "";
            string redirectUrl = "";
            //return Json(new { Success = 1, TermsId = param, ex = "" });
            if (action == "PrintMissingSequence")
            {
                //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                reportname = "rpt_MissingSequence";
                filename = "rpt_MissingSequence" + DateTime.Now.Date.ToString();
                var query = "Exec [rpt_MissingSequence] '" + comid + "' , ";
                _httpContext.HttpContext.Session.SetString("reportquery", "Exec [rpt_MissingSequence] '" + comid + "',  '" + Type + "' , '" + FromNo + "','" + ToNo + "','" + PrdUnitId + "'");
                _httpContext.HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                _httpContext.HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
            }


            string DataSourceName = "DataSet1";

            //HttpContext.Session.SetObject("Acc_rptList", postData);

            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = _httpContext.HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
            clsReport.strQueryMain = _httpContext.HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;

            //var ConstrName = "ApplicationServices";
            //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
            //redirectUrl = callBackUrl;


            redirectUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = rptFormat });
            return redirectUrl;
        }
    }
}
