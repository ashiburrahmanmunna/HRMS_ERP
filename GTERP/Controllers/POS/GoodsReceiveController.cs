using DataTablesParser;
using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Controllers.POS
{
    [OverridableAuthorize]
    public class GoodsReceiveController : Controller
    {
        private readonly GTRDBContext db;
        private TransactionLogRepository tranlog;
        private readonly IConfiguration _configuration;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }
        POSRepository POS;
        PermissionLevel PL;


        public GoodsReceiveController(IConfiguration configuration, GTRDBContext context, POSRepository _POS, TransactionLogRepository tran, PermissionLevel _PL)
        {
            db = context;
            tranlog = tran;
            //Repository = rep;
            POS = _POS;
            PL = _PL;
            _configuration = configuration;
        }




        [HttpPost]
        public IActionResult CreateProduct(Models.Product product)
        {
            try
            {
                //    var errors = ModelState.Where(x => x.Value.Errors.Any())
                //.Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    if (product.ProductId > 0)
                    {
                        product.DateUpdated = DateTime.Now;
                        product.comid = (HttpContext.Session.GetString("comid"));

                        if (product.userid == null)
                        {
                            product.userid = HttpContext.Session.GetString("userid");
                        }
                        product.useridUpdate = HttpContext.Session.GetString("userid");

                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();

                        TempData["Status"] = "2";
                        TempData["Message"] = "Data Update Successfully";

                        return Json(new { Success = 2, data = product, ex = TempData["Message"].ToString() });
                    }
                    else
                    {
                        product.userid = HttpContext.Session.GetString("userid");
                        product.comid = (HttpContext.Session.GetString("comid"));
                        product.DateAdded = DateTime.Now;
                        product.ProductImage = null;

                        db.Products.Add(product);
                        db.SaveChanges();
                        TempData["Status"] = "1";
                        TempData["Message"] = "Data Save Successfully";

                        return Json(new { Success = 1, data = product, ex = TempData["Message"].ToString() }); ;
                    }
                }
                else
                {
                    return Json(new { Success = 3, ex = "Model State Not Valid" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = 3, ex = e.InnerException });

            }
        }

        // GET: GoodsReceive
        public async Task<IActionResult> Index()
        {
            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");
            //var gTRDBContext = db.StoreRequisitionMain.Where(x => x.ComId == comid).Include(s => s.ApprovedBy).Include(s => s.Department).Include(s => s.PrdUnit).Include(s => s.Purpose).Include(s => s.RecommenedBy);

            ///////////get user list from the server //////

            var appKey = HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            //List<string> moduleUser = PL.GetModuleUser().ToList();

            var userList = new List<AspnetUserList>();
            foreach (var item in list)
            {
                //if (moduleUser.Contains(c.UserID))
                //{
                var newUser = new AspnetUserList();
                newUser.Email = item.UserName;
                newUser.UserId = item.UserID;
                newUser.UserName = item.UserName;
                userList.Add(newUser);
                //}
            }

            ViewBag.Userlist = new SelectList(userList, "UserId", "UserName", userid);
            ViewBag.PrdUnitId = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");

            //return View(await gTRDBContext.ToListAsync());
            return View();
        }

        public partial class GoodsReceiveResult
        {
            public int GRRMainId { get; set; }

            public string GRRNo { get; set; }

            public string GRRDate { get; set; }

            public string GRRRef { get; set; }

            public string Department { get; set; }

            public string PrdUnitName { get; set; }
            public string PRNo { get; set; }

            public string SupplierName { get; set; }

            public string TypeName { get; set; }
            public string CurCode { get; set; }
            public float ConvertionRate { get; set; }
            public float TotalGRRValue { get; set; }
            public float? Deduction { get; set; }
            public float? NetGRRValue { get; set; }
            public string SectName { get; set; }
            public string PONo { get; set; }
            public DateTime? GateInHouseDate { get; set; }
            public DateTime? ExpectedReciveDate { get; set; }
            public string TermsAndCondition { get; set; }
            public string Remarks { get; set; }
            public string Status { get; set; }


        }
        public IActionResult Get(string UserList, string FromDate, string ToDate, string CustomerList, int isAll)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));

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

                Microsoft.Extensions.Primitives.StringValues y = "";

                var x = Request.Form.TryGetValue("search[value]", out y);

                if (y.ToString().Length > 0)
                {
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


                    var parser = new Parser<GoodsReceiveResult>(Request.Form, query);

                    return Json(parser.Parse());

                }
                else
                {
                    if (CustomerList != null && UserList != null)
                    {
                        var querytest = from e in PL.GetGRR()
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


                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList != null && UserList == null)
                    {
                        var querytest = from e in PL.GetGRR()
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


                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
                        return Json(parser.Parse());
                    }
                    else if (CustomerList == null && UserList != null)
                    {

                        var querytest = from e in PL.GetGRR()
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


                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }
                    else
                    {

                        var querytest = from e in PL.GetGRR()
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


                        var parser = new Parser<GoodsReceiveResult>(Request.Form, querytest);
                        return Json(parser.Parse());


                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", error = ex.Message });
                //throw ex;
            }

        }


        public JsonResult PurchaseOrderDetailsById(int? POId)
        {

            var OrderDetails = db.PurchaseOrderMain.Where(o => o.PurOrderMainId == POId).Select(o => new
            {

                o.PurOrderMainId,
                o.PurchaseOrderSub.FirstOrDefault().PurOrderSubId,
                o.PurchaseOrderSub.FirstOrDefault().PurReqSubId,
                o.PurchaseOrderSub.FirstOrDefault().vProduct.ProductName,
                o.PurchaseOrderSub.FirstOrDefault().vProduct.vProductUnit.UnitName,
                o.ConvertionRate,
                o.CurrencyId,
                o.Currency.CurCode,
                o.Deduction,
                o.Department,
                //ExpectedReciveDateString = o.ExpectedReciveDate.ToString(),
                //GateInHouseDateString = o.GateInHouseDate.ToString(),
                //o.ExpectedReciveDate,
                //o.GateInHouseDate,
                o.LastDateOfDelivery,
                o.ExpectedRecivedDate,
                o.NetPOValue,
                o.PaymentTypeId,
                o.PaymentType.TypeName,
                o.PODate,
                o.PONo,
                o.PORef,
                o.PurReqId,
                o.PurchaseRequisitionMain.PRNo,
                o.PrdUnitId,
                o.PrdUnit.PrdUnitName,
                o.Remarks,
                o.SectId,
                o.Section.SectName,
                o.SupplierId,
                o.Supplier.SupplierName,
                o.TermsAndCondition
            }).FirstOrDefault();

            return Json(OrderDetails);
        }

        public JsonResult PurchaseOrderSubDataByPOMId(int PurOrderMainId)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");

                var quary = $"EXEC GoodsReceiveDetailsInformation '{comid}','{userid}',{PurOrderMainId}";

                SqlParameter[] parameters = new SqlParameter[3];

                parameters[0] = new SqlParameter("@ComId", comid);
                parameters[1] = new SqlParameter("@userid", userid);
                parameters[2] = new SqlParameter("@PurOrderMainId", PurOrderMainId);
                List<GoodsReceiveDetailsModel> GoodsReceiveDetailsInformation = Helper.ExecProcMapTList<GoodsReceiveDetailsModel>("GoodsReceiveDetailsInformation", parameters);

                return Json(GoodsReceiveDetailsInformation);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        ///[{"SLNo":"1"
        ///,"ProductId":9018,"UnitId":13,
        ///"ProductName":"Toyota Hiace Ambulance","UnitName":"Nos",
        ///"RequisitionQty":3,"PurchaseQty":3.00,"RemainingReqQty":0.00,
        ///"Rate":3000000.00,"TotalValue":9000000.00,"Received":0.00,
        ///"Quality":0.00,"Damage":0.00,"PurReqSubId":1055,"Remarks":null,
        ///"PurReqId":1030,"PONo":"po-tes-001","GRRSubId":null,
        ///"PurOrderMainId":1007,"PurOrderSubId":1014},
        ///{"SLNo":"2","ProductId":10172,"UnitId":13,"ProductName":"Panasonic Steno Telephone Set","UnitName":"Nos","RequisitionQty":10,"PurchaseQty":8.00,"RemainingReqQty":0.00,"Rate":3500.00,"TotalValue":28000.00,"Received":0.00,"Quality":0.00,"Damage":0.00,"PurReqSubId":1056,"Remarks":null,"PurReqId":1030,"PONo":"po-tes-001","GRRSubId":null,"PurOrderMainId":1007,"PurOrderSubId":1015}]
        public class GoodsReceiveDetailsModel
        {
            public int? PurOrderMainId { get; set; }
            public int? PurOrderSubId { get; set; }
            public int? GRRSubId { get; set; }
            public int? ProductId { get; set; }
            public int? UnitId { get; set; }
            public int? SLNo { get; set; }
            public string PONo { get; set; }
            public string ProductName { get; set; }
            public string UnitName { get; set; }
            //public int? PurReqQty { get; set; }
            public int? RequisitionQty { get; set; }
            public float? RemainingReqQty { get; set; }
            public float? PurchaseQty { get; set; }
            public float? Rate { get; set; }
            public float? TotalValue { get; set; }
            public float? Quality { get; set; }
            public float? Received { get; set; }
            public float? Damage { get; set; }
            public string PORemarks { get; set; }
            public string Remarks { get; set; }
            public int? PurReqId { get; set; }
            public int? PurReqSubId { get; set; }
            public int? WarehouseId { get; set; }

        }


        // GET: GoodsReceive/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GoodsReceiveMain goodsReceiveMain = await db.GoodsReceiveMain
                .Include(g => g.vDepartment)
                .Include(g => g.Currency)
                .Include(g => g.PaymentType)
                .Include(g => g.PrdUnit)
                .Include(g => g.PurchaseOrderMain)
                .Include(g => g.PurchaseRequisitionMain)
                .Include(g => g.Supplier)
                .FirstOrDefaultAsync(m => m.GRRMainId == id);
            if (goodsReceiveMain == null)
            {
                return NotFound();
            }

            return View(goodsReceiveMain);
        }


        public IActionResult DirectGrr()
        {

            var comid = (HttpContext.Session.GetString("comid"));
            var userid = (HttpContext.Session.GetString("userid"));

            this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.AccCode.Contains("2-1-30") || p.AccCode.Contains("1-1-64") && p.AccType == "L" || p.AccCode.Contains("1-1-28") && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");


            GoodsReceiveMain goodsreceive = new GoodsReceiveMain();



            var lastgoodsreceiveMain = db.GoodsReceiveMain.Where(x => x.ComId == comid && x.UserId == userid).OrderByDescending(x => x.GRRMainId).FirstOrDefault();

            if (lastgoodsreceiveMain != null)
            {
                goodsreceive = lastgoodsreceiveMain;

                //goodsreceive.GRRDate = lastgoodsreceiveMain.GRRDate;

                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", lastgoodsreceiveMain.PrdUnitId);
            }
            else
            {

                goodsreceive.GRRDate = DateTime.Now.Date;
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");
            }


            //goodsreceive.GRRDate = DateTime.Now.Date;
            goodsreceive.GRRMainId = 0;  // for create data added himu
            goodsreceive.ChallanDate = DateTime.Now.Date;
            goodsreceive.CertificateDate = DateTime.Now.Date;
            goodsreceive.LPDate = DateTime.Now.Date;


            //this.ViewBag.WarehouseList = db.Warehouses.Where(x => x.comid == comid && x.WhType == "L");
            this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

            ViewBag.Title = "Create";
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName");
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName");
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.comid == comid), "PrdUnitId", "PrdUnitName");
            // ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo");
            //ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo");
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName");
            ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
            ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");

            ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid).Select(x => new { x.EmpId, x.EmpCode, x.EmpName }), "EmpId", "EmpName");
            //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x => x.comid == comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");

            #region CategoryId viewbag selectlist
            List<Category> categorydb = PL.GetCategory().Where(c => c.CategoryId > 0).ToList();

            List<SelectListItem> categoryid = new List<SelectListItem>();
            categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });

            var permission = HttpContext.Session.GetObject<UserPermission>("userpermission");
            if (!permission.IsProduction && !permission.IsMedical)
            {
                categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });
            }


            //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
            if (categorydb != null)
            {
                foreach (Category x in categorydb)
                {
                    categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                }
            }
            ViewData["CategoryId"] = (categoryid);
            #endregion


            ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName");
            //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.comid == comid).Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");
            ViewData["PurposeId"] = new SelectList(db.Purpose.Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName");

            ViewData["ProductId"] = new SelectList(db.Products.Take(0).Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");

            ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName");

            return View(goodsreceive);
        }

        public JsonResult GetProductsGRR(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            IEnumerable<object> product;
            if (id != null)
            {
                if (id == 0 || id == -1)
                {
                    //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                    product = new SelectList(db.Products.Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

                }
                else
                {
                    product = new SelectList(db.Products.Where(x => x.comid == comid && x.CategoryId == id).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");
                    //product = db.Products.Where(x => x.CategoryId == id).Select(x => new { x.ProductId, x.ProductName }).ToList();
                }
            }
            else
            {
                //product = db.Products.Where(x => x.comid == comid).Select(x => new { x.ProductId, x.ProductName }).ToList();
                product = new SelectList(db.Products.Take(0).Where(x => x.comid == comid).Select(s => new { Text = s.ProductName + " - [ " + s.ProductCode + " ]", Value = s.ProductId }).ToList(), "Value", "Text");

            }
            return Json(new { item = product });
        }

        public ActionResult GetProductInfo(int id, int whId)
        {
            var comid = HttpContext.Session.GetString("comid");

            //decimal? lastpurchaseprice;
            //lastpurchaseprice=  db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id && x.GoodsReceiveMain.Status > 0).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate).FirstOrDefault();
            //if (lastpurchaseprice == null)
            //{
            //    lastpurchaseprice = 0;
            //}

            var ProductData = db.Products.Include(x => x.vProductUnit).Include(x => x.CostCalculated).Where(x => x.comid == comid).Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.ProductBrand,
                p.ProductModel,
                UnitName = p.vProductUnit.UnitName,
                p.UnitId,
                //AvgRate = p.CostCalculated.Where(x=>x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CalculatedPrice).FirstOrDefault().ToString(), //lastpurchaseprice,
                //StockQty = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CurrQty).FirstOrDefault().ToString(),
                AvgRate = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Take(1).Select(x => x.CalculatedPrice).FirstOrDefault().ToString(), //lastpurchaseprice,
                StockQty = p.CostCalculated.Where(x => x.WarehouseId == whId && x.isDelete == false).OrderByDescending(x => x.CostCalculatedId).Sum(x => x.CurrQty).ToString()
            }).Where(p => p.ProductId == id).FirstOrDefault();// ToList();

            ///ProductData.CostPrice = db.PurchaseOrderMain.Include(x => x.PurchaseOrderSub).Where(x => x.ComId == comid).Select(x=>x.p).OrderByDescending(x => x.PODate);
            //ProductData.CostPrice = db.GoodsReceiveSub.Include(x => x.GoodsReceiveMain).Where(x => x.GoodsReceiveMain.ComId == comid && x.ProductId == id).OrderByDescending(x => x.PurchaseOrderSub.PurchaseOrderMain.PODate).Take(1).Select(x => x.Rate);


            return Json(ProductData);  
        }

        // GET: GoodsReceive/Create
        public IActionResult Create()
        {
            var comid = (HttpContext.Session.GetString("comid"));
            this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WarehouseId != 0 && x.WhType == "L");


            GoodsReceiveMain goodsreceive = new GoodsReceiveMain();
            goodsreceive.GRRDate = DateTime.Now.Date;
            goodsreceive.ChallanDate = DateTime.Now.Date;
            goodsreceive.CertificateDate = DateTime.Now.Date;
            goodsreceive.LPDate = DateTime.Now.Date;

            ViewBag.Title = "Create";
            ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName");
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode");
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName");
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName");
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo");
            ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid && x.Status == 1), "PurOrderMainId", "PONo");
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName");
            ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WarehouseId != 0 && x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");

            return View(goodsreceive);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GoodsReceiveMain goodsReceiveMain)
        {
            try
            {
                //var errors = ModelState.Where(x => x.Value.Errors.Any())
                //               .Select(x => new { x.Key, x.Value.Errors });

                var result = "";

                var comid = HttpContext.Session.GetString("comid");
                var userid = HttpContext.Session.GetString("userid");
                var pcname = HttpContext.Session.GetString("pcname");
                var nowdate = DateTime.Now;

                var duplicateDocument = db.GoodsReceiveMain.Where(i => i.GRRNo == goodsReceiveMain.GRRNo && i.GRRMainId != goodsReceiveMain.GRRMainId && i.ComId == comid).FirstOrDefault();
                if (duplicateDocument != null)
                {
                    return Json(new { Success = 0, ex = goodsReceiveMain.GRRNo + " already exist. Document No can not be Duplicate." });
                }


                DateTime date = goodsReceiveMain.GRRDate;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var activefiscalmonth = db.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
                if (activefiscalmonth == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
                }
                var activefiscalyear = db.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
                if (activefiscalyear == null)
                {
                    return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
                }

                var lockCheck = db.HR_ProcessLock
                 .Where(p => p.LockType.Contains("Store Lock") && p.DtDate.Date <= goodsReceiveMain.GRRDate.Date && p.DtToDate.Value.Date >= goodsReceiveMain.GRRDate.Date
                     && p.IsLock == true).FirstOrDefault();

                if (lockCheck != null)
                {
                    return Json(new { Success = 0, ex = "Store Lock this date!!!" });
                }

                /////for rectifying the entry value + and value -
                foreach (var item in goodsReceiveMain.GoodsReceiveSub)
                {
                    if (item.Quality == 0 || item.Quality == 0)
                    {
                        item.TotalValue = item.Rate;
                    }

                }


                using (var tr = db.Database.BeginTransaction())
                {
                    if (goodsReceiveMain != null)
                    {
                        try
                        {
                            if (goodsReceiveMain.GRRMainId > 0)
                            {
                                goodsReceiveMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
                                goodsReceiveMain.FiscalYearId = activefiscalyear.FiscalYearId;

                                goodsReceiveMain.ComId = comid;
                                goodsReceiveMain.UserId = userid;
                                goodsReceiveMain.PcName = pcname;
                                goodsReceiveMain.DateUpdated = nowdate;
                                var CurrentGoodsReceiveSub = db.GoodsReceiveSub.Include(x => x.GoodsReceiveSubWarehouse).Where(p => p.GRRMainId == goodsReceiveMain.GRRMainId);


                                foreach (GoodsReceiveSub ss in CurrentGoodsReceiveSub)
                                {
                                    db.GoodsReceiveSub.Remove(ss);
                                }
                                db.SaveChanges();


                                /////for provision amount 
                                var CurrentGoodsReceiveProvision = db.GoodsReceiveProvision.Where(p => p.GRRMainId == goodsReceiveMain.GRRMainId);

                                foreach (GoodsReceiveProvision prv in CurrentGoodsReceiveProvision)
                                {
                                    db.GoodsReceiveProvision.Remove(prv);
                                }
                                db.SaveChanges();



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
                                            db.GoodsReceiveSub.Add(item);

                                        }

                                    }
                                    else
                                    {
                                        db.GoodsReceiveSub.Add(item);
                                    }
                                }
                                //db.SaveChanges();




                                foreach (GoodsReceiveProvision itemprovision in goodsReceiveMain.GoodsReceiveProvision)
                                {
                                    if (itemprovision.GRRProvisionId > 0)
                                    {
                                        itemprovision.GRRProvisionId = 0;
                                        db.GoodsReceiveProvision.Add(itemprovision);
                                    }
                                    else
                                    {
                                        db.GoodsReceiveProvision.Add(itemprovision);
                                    }
                                }
                                //db.SaveChanges();


                                goodsReceiveMain.UpdateByUserId = userid;
                                goodsReceiveMain.DateUpdated = nowdate;
                                db.Entry(goodsReceiveMain).State = EntityState.Modified;
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


                                db.Add(goodsReceiveMain);
                                result = "1";
                            }
                            db.SaveChanges();


                        }
                        catch (SqlException ex)
                        {

                            Console.WriteLine(ex.Message);
                            tr.Rollback();
                            return Json(new { Success = 0, ex = ex.Message.ToString() });

                        }
                    }
                    tr.Commit();
                }

                ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
                ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", goodsReceiveMain.PrdUnitId);
                ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
                ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
                ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
                ViewData["WarehouseId"] = new SelectList(db.Warehouses.Where(x => x.ComId == comid && x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
                return Json(new { Success = result, ex = "" });
            }
            catch (Exception ex)
            {

                //throw ex;
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        public IActionResult View(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "View";

            var goodsreceive = db.GoodsReceiveMain.Find(id);



            if (goodsreceive == null)
            {
                return NotFound();
            }




            if (goodsreceive.IsDirectGRR == true)
            {
                var goodsReceiveMain = db.GoodsReceiveMain
                 .Include(g => g.GoodsReceiveSub)
                 .ThenInclude(g => g.vProduct)
                 .ThenInclude(g => g.vProductUnit)
                 .Include(g => g.GoodsReceiveProvision)
                 .ThenInclude(g => g.vChartOfAccounts)

                 .Where(g => g.GRRMainId == id).FirstOrDefault();



                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid).Select(x => new { x.EmpId, x.EmpCode, x.EmpName }), "EmpId", "EmpName");

                #region CategoryId viewbag selectlist
                List<Category> categorydb = POS.GetCategory(comid).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
                categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });



                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                if (categorydb != null)
                {
                    foreach (Category x in categorydb)
                    {
                        categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                    }
                }
                ViewData["CategoryId"] = (categoryid);
                #endregion


                //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x=>x.comid==comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
                ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName", goodsReceiveMain.DeptId);
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", goodsReceiveMain.PrdUnitId);
                //ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
                ViewData["PurposeId"] = new SelectList(db.Purpose.Where(x => x.ComId == comid).Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName");
                ViewData["ProductId"] = new SelectList(db.Products.Where(x => x.comid == comid).Take(0).Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");
                ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName");
                ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
                ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
                ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
                ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
                this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.AccCode.Contains("2-1-30") || p.AccCode.Contains("1-1-64") && p.AccType == "L" || p.AccCode.Contains("1-1-28") && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                return View("DirectGrr", goodsReceiveMain);
            }
            else
            {

                var goodsReceiveMain = db.GoodsReceiveMain
               .Include(g => g.PurchaseOrderMain)
               .ThenInclude(g => g.PurchaseOrderSub)
               .Include(g => g.PurchaseRequisitionMain)
               .ThenInclude(g => g.PurchaseRequisitionSub)

                .Include(g => g.GoodsReceiveSub)
               .ThenInclude(g => g.PurchaseOrderSub)
               //.ThenInclude(g => g.PurchaseRequisitionSub)
               .ThenInclude(g => g.vProduct)
               .ThenInclude(g => g.vProductUnit)




               .Include(g => g.GoodsReceiveSub)
               .ThenInclude(g => g.vWarehouse)


                .Include(p => p.GoodsReceiveSub)
                .ThenInclude(p => p.GoodsReceiveSubWarehouse)
                .ThenInclude(p => p.vWarehouse)

               .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefault();


                //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.DeptId);
                ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName", goodsReceiveMain.DeptId);
                ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
                ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", goodsReceiveMain.PrdUnitId);
                ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid && x.Status == 1), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
                //ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
                ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
                ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");


                return View("Create", goodsReceiveMain);
            }
        }







        // GET: GoodsReceive/Edit/5
        public IActionResult Edit(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            var goodsreceive = db.GoodsReceiveMain.Find(id);





            if (goodsreceive == null)
            {
                return NotFound();
            }




            if (goodsreceive.IsDirectGRR == true)
            {
                var goodsReceiveMain = db.GoodsReceiveMain
                 .Include(g => g.GoodsReceiveSub)
                 .ThenInclude(g => g.vProduct)
                 .ThenInclude(g => g.vProductUnit)
                 .Include(g => g.GoodsReceiveProvision)
                 .ThenInclude(g => g.vChartOfAccounts)

                 //.Include(g => g.GoodsReceiveSub)
                 //.ThenInclude(s => s.vProduct)
                 //.ThenInclude(s => s.vPrimaryCategory)
                 //.ThenInclude(s => s.vProducts)
                 //.ThenInclude(s => s.vSubCategory)


                 //.Include(g => g.GoodsReceiveSub)
                 //.ThenInclude(g => g.vWarehouse)

                 //.Include(p => p.GoodsReceiveSub)
                 //.ThenInclude(p => p.GoodsReceiveSubWarehouse)
                 //.ThenInclude(p => p.vWarehouse)


                 .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefault();



                // goodsReceiveMain = await db.GoodsReceiveMain               
                //.Include(g => g.GoodsReceiveSub)
                //.ThenInclude(g => g.vProduct)
                //.ThenInclude(g => g.vProductUnit)
                //.Where(g => g.GRRMainId == id).FirstOrDefaultAsync();

                //goodsreceive.GoodsReceiveSub.OrderBy(x => x.SLNo);


                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid).Select(x => new { x.EmpId, x.EmpCode, x.EmpName }), "EmpId", "EmpName");

                #region CategoryId viewbag selectlist
                List<Category> categorydb = POS.GetCategory(comid).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
                categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });



                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                if (categorydb != null)
                {
                    foreach (Category x in categorydb)
                    {
                        categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                    }
                }
                ViewData["CategoryId"] = (categoryid);
                #endregion


                //ViewData["CategoryId"] = new SelectList(db.Categories.Where(x=>x.comid==comid).Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
                ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName", goodsReceiveMain.DeptId);
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", goodsReceiveMain.PrdUnitId);
                //ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
                ViewData["PurposeId"] = new SelectList(db.Purpose.Where(x => x.ComId == comid).Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName");
                ViewData["ProductId"] = new SelectList(db.Products.Where(x => x.comid == comid).Take(0).Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");
                ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName");
                ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
                ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
                ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
                ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
                this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.AccCode.Contains("2-1-30") || p.AccCode.Contains("1-1-64") && p.AccType == "L" || p.AccCode.Contains("1-1-28") && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                return View("DirectGrr", goodsReceiveMain);
            }
            else
            {

                var goodsReceiveMain = db.GoodsReceiveMain
               .Include(g => g.PurchaseOrderMain)
               .ThenInclude(g => g.PurchaseOrderSub)
               .Include(g => g.PurchaseRequisitionMain)
               .ThenInclude(g => g.PurchaseRequisitionSub)

                .Include(g => g.GoodsReceiveSub)
               .ThenInclude(g => g.PurchaseOrderSub)
               //.ThenInclude(g => g.PurchaseRequisitionSub)
               .ThenInclude(g => g.vProduct)
               .ThenInclude(g => g.vProductUnit)

                //.Include(g => g.GoodsReceiveSub)
                //.ThenInclude(g => g.PurchaseOrderSub)
                //.ThenInclude(g => g.PurchaseRequisitionSub)
                //.ThenInclude(s => s.vProduct)
                //.ThenInclude(s => s.vPrimaryCategory)
                //.ThenInclude(s => s.vProducts)
                //.ThenInclude(s => s.vSubCategory)


               .Include(g => g.GoodsReceiveSub)
               .ThenInclude(g => g.vWarehouse)


                .Include(p => p.GoodsReceiveSub)
                .ThenInclude(p => p.GoodsReceiveSubWarehouse)
                .ThenInclude(p => p.vWarehouse)

               .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefault();



                //goodsReceiveMain.GoodsReceiveSub.OrderBy(p => p.SLNo);


                // goodsreceive.GoodsReceiveSub.OrderBy(x => x.SLNo);

                //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.DeptId);
                ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName", goodsReceiveMain.DeptId);
                ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
                ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
                ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", goodsReceiveMain.PrdUnitId);
                ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid && x.Status == 1), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
                //ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
                ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
                ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");


                return View("Create", goodsReceiveMain);
            }
        }

        // POST: GoodsReceive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GoodsReceiveMain goodsReceiveMain)
        {
            if (id != goodsReceiveMain.GRRMainId)
            {
                return NotFound();
            }
            var comid = HttpContext.Session.GetString("comid");

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(goodsReceiveMain);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    if (!GoodsReceiveMainExists(goodsReceiveMain.GRRMainId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.ComId == comid), "PrdUnitId", "PrdUnitName", goodsReceiveMain.PrdUnitId);
            ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
            ViewData["WarehouseId"] = new SelectList(db.Warehouses.Where(x => x.ComId == comid && x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
            return View(goodsReceiveMain);
        }

        // GET: GoodsReceive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";
            var comid = HttpContext.Session.GetString("comid");

            var goodsReceiveMain = await db.GoodsReceiveMain
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


            if (goodsReceiveMain == null)
            {
                return NotFound();
            }
            //ViewData["SectId"] = new SelectList(db.Cat_Section.Where(x => x.ComId == comid), "SectId", "SectName", goodsReceiveMain.SectId);
            ViewData["CurrencyId"] = new SelectList(db.Currency.OrderByDescending(x => x.isDefault), "CurrencyId", "CurCode", goodsReceiveMain.CurrencyId);
            ViewData["PaymentTypeId"] = new SelectList(db.PaymentTypes, "PaymentTypeId", "TypeName", goodsReceiveMain.PaymentTypeId);
            ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit(), "PrdUnitId", "PrdUnitName", goodsReceiveMain.PrdUnitId);
            ViewData["PurOrderMainId"] = new SelectList(db.PurchaseOrderMain.Where(x => x.ComId == comid && x.Status == 1), "PurOrderMainId", "PONo", goodsReceiveMain.PurOrderMainId);
            ViewData["PurReqId"] = new SelectList(db.PurchaseRequisitionMain.Where(x => x.ComId == comid && x.Status == 1), "PurReqId", "PRNo", goodsReceiveMain.PurReqId);
            ViewData["SupplierId"] = new SelectList(db.Suppliers.Where(x => x.ComId == comid), "SupplierId", "SupplierName", goodsReceiveMain.SupplierId);
            ViewData["WarehouseId"] = new SelectList(PL.GetWarehouse().Where(x => x.WhType == "L" && x.ParentId != null), "WarehouseId", "WhShortName");
            this.ViewBag.AccountMain = new SelectList(db.Acc_ChartOfAccounts.Where(p => p.ComId == comid && p.AccType == "L" && p.AccCode.Contains("2-1-30") || p.AccCode.Contains("1-1-64") && p.AccType == "L" || p.AccCode.Contains("1-1-28") && p.AccType == "L").Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");


            if (goodsReceiveMain.IsDirectGRR == true)
            {
                goodsReceiveMain = await db.GoodsReceiveMain

                .Include(g => g.GoodsReceiveSub)
                .ThenInclude(g => g.vProduct)
                .ThenInclude(g => g.vProductUnit)

                  .Include(g => g.GoodsReceiveSub)
                .ThenInclude(s => s.vProduct)
                .ThenInclude(s => s.vPrimaryCategory)
                .ThenInclude(s => s.vProducts)
                .ThenInclude(s => s.vSubCategory)


                .Include(g => g.GoodsReceiveSub)
                .ThenInclude(g => g.vWarehouse)



                .Include(g => g.GoodsReceiveSub)
                .ThenInclude(p => p.GoodsReceiveSubWarehouse)
                .ThenInclude(p => p.vWarehouse)

                .Where(g => g.GRRMainId == id && g.Status == 0).FirstOrDefaultAsync();
                // goodsReceiveMain = await db.GoodsReceiveMain               
                //.Include(g => g.GoodsReceiveSub)
                //.ThenInclude(g => g.vProduct)
                //.ThenInclude(g => g.vProductUnit)
                //.Where(g => g.GRRMainId == id).FirstOrDefaultAsync();


                ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid).Select(x => new { x.EmpId, x.EmpCode, x.EmpName }), "EmpId", "EmpName");
                #region CategoryId viewbag selectlist
                List<Category> categorydb = POS.GetCategory(comid).ToList();

                List<SelectListItem> categoryid = new List<SelectListItem>();
                categoryid.Add(new SelectListItem { Text = "--Please Select--", Value = "-1" });
                categoryid.Add(new SelectListItem { Text = "=ALL=", Value = "0" });



                //licities.Add(new SelectListItem { Text = "--Select State--", Value = "0" });
                if (categorydb != null)
                {
                    foreach (Category x in categorydb)
                    {
                        categoryid.Add(new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
                    }
                }
                ViewData["CategoryId"] = (categoryid);
                #endregion
                this.ViewBag.WarehouseList = PL.GetWarehouse().Where(x => x.WhType == "L" && x.WarehouseId != 0);

                ViewData["DeptId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid).Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName");
                ViewData["PrdUnitId"] = new SelectList(PL.GetPrdUnit().Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName");
                ViewData["PurposeId"] = new SelectList(db.Purpose.Where(x => x.ComId == comid).Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName");

                ViewData["ProductId"] = new SelectList(db.Products.Where(x => x.comid == comid).Take(0).Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");

                ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName");

                return View("DirectGrr", goodsReceiveMain);
            }
            else
            {
                return View("Create", goodsReceiveMain);
            }


            //db.GoodsReceiveMain.Remove(goodsReceiveMain);
            //await db.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }



        [HttpPost, ActionName("Delete")]
        //public JsonResult DeleteConfirmed(int id)
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var goodsReceiveMain = await db.GoodsReceiveMain.FindAsync(id);
                var costcalculated = db.CostCalculated.Where(x => x.GRRMainId == id).ToList();

                if (costcalculated != null)
                {
                    foreach (var item in costcalculated)
                    {
                        db.CostCalculated.Remove(item);
                    }
                    await db.SaveChangesAsync();
                }


                var grrprovisiondetails = db.GoodsReceiveProvision.Where(x => x.GRRMainId == id).ToList();

                if (grrprovisiondetails != null)
                {
                    foreach (var item in grrprovisiondetails)
                    {
                        db.GoodsReceiveProvision.Remove(item);
                    }
                    await db.SaveChangesAsync();
                }


                if (goodsReceiveMain != null)
                {
                    db.GoodsReceiveMain.Remove(goodsReceiveMain);
                    await db.SaveChangesAsync();
                }


                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), goodsReceiveMain.GRRMainId.ToString(), "Delete", goodsReceiveMain.GRRNo);

                return Json(new { Success = 1, VoucherID = goodsReceiveMain.GRRMainId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }


            // return RedirectToAction("Index");
        }




        private bool GoodsReceiveMainExists(int id)
        {
            return db.GoodsReceiveMain.Any(e => e.GRRMainId == id);
        }






        public ActionResult Print(int? id, string type)
        {
            string SqlCmd = "";
            string ReportPath = "";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";
            var comid = HttpContext.Session.GetString("comid");


            //var abcvouchermain = db.PurchaseRequisitionMain.Where(x => x.PurReqId == id && x.ComId == comid).FirstOrDefault();

            //var reportname = "rptGRRIndividual";
            var reportname = "rptMRRForm";

            ///@ComId nvarchar(200),@Type varchar(10),@ID int,@dtFrom smalldatetime,@dtTo smalldatetime
            HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
            //var str = db.Acc_VoucherMains.Include(x => x.Acc_VoucherType).FirstOrDefault().Acc_VoucherType.VoucherTypeNameShort;// "VPC";
            HttpContext.Session.SetString("reportquery", "Exec [Inv_rptIndGRRDetails] '" + comid + "', 'GRRNW' ," + id + ", '01-Jan-1900', '01-Jan-1900'");


            //Session["reportquery"] = "Exec " + AppData.AppData.dbGTCommercial.ToString() + ".dbo.[rptCommercialInvoice_Export] '" + id + "','" + AppData.AppData.intComId + "'";
            string filename = db.GoodsReceiveMain.Where(x => x.GRRMainId == id).Select(x => x.GRRNo).Single();
            HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));

            //var a = Session["PrintFileName"].ToString();


            string DataSourceName = "DataSet1";
            HttpContext.Session.SetObject("rptList", postData);


            //Common.Classes.clsMain.intHasSubReport = 0;
            clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
            clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
            clsReport.strDSNMain = DataSourceName;




            SqlCmd = clsReport.strQueryMain;
            ReportPath = clsReport.strReportPathMain;
            ReportType = "PDF";

            /////////////////////// sub report test to our report server


            //var subReport = new SubReport();
            //var subReportObject = new List<SubReport>();

            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ChequeNo'";
            //subReport.strRptPathSub = "rptShowVoucher_ChequeNo";
            //subReportObject.Add(subReport);


            //subReport = new SubReport();
            //subReport.strDSNSub = "DataSet1";
            //subReport.strRFNSub = "";
            //subReport.strQuerySub = "Exec [rptShowVoucher_Referance] '" + id + "','" + comid + "','ReceiptPerson'";
            //subReport.strRptPathSub = "rptShowVoucher_ReceiptPerson";
            //subReportObject.Add(subReport);


            //var jsonData = JsonConvert.SerializeObject(subReportObject);

            ////string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType, jsonData);
            //string callBackUrl = Repository.GenerateReport(ReportPath, SqlCmd, ConstrName, ReportType);

            //return Redirect(callBackUrl);

            ///return RedirectToAction("Index", "ReportViewer");

            string redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = ReportType });//, new { id = 1 }
            return Redirect(redirectUrl);
        }
        [HttpPost, ActionName("PrintGrrSummary")]
        public JsonResult GrrSummaryReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintGrrSummary")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptGRRSummary";
                    filename = "GRRSummary" + DateTime.Now.Date.ToString();
                    var query = "Exec [Inv_GRRSummary] '" + comid + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRSummary] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;


                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }


        [HttpPost, ActionName("PrintGrrDetails")]
        public JsonResult GrrDetailsReport(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintGrrDetails")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptGRRDetails";
                    filename = "GrrDetails" + DateTime.Now.Date.ToString();
                    var query = "Exec [Inv_GRRDetails] '" + comid + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRDetails] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;

                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }



        [HttpPost, ActionName("PrintGrrVoucherLocal")]
        public JsonResult PrintGrrVoucherLocal(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

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
                    HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRVoucher_Local] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;

                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }

        [HttpPost, ActionName("PrintGrrVoucherForeign")]
        public JsonResult PrintGrrVoucherForeign(string rptFormat, string action, string FromDate, string ToDate, int PrdUnitId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

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

                    HttpContext.Session.SetString("reportquery", "Exec [Inv_GRRVoucher_Foreign] '" + comid + "','" + FromDate + "','" + ToDate + "','" + PrdUnitId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;


                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }


        [HttpPost, ActionName("PrintMissingSequence")]
        public JsonResult PrintMissingSequence(string rptFormat, string action, string Type, string FromNo, string ToNo, int PrdUnitId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

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
                    HttpContext.Session.SetString("reportquery", "Exec [rpt_MissingSequence] '" + comid + "',  '" + Type + "' , '" + FromNo + "','" + ToNo + "','" + PrdUnitId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Inventory/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;


                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });//, new { id = 1 }
                return Json(new { Url = redirectUrl });
            }

            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }

            return Json(new { Success = 0, ex = new Exception("Unable to Open").Message.ToString() });
            //return RedirectToAction("Index");

        }

    }
}
