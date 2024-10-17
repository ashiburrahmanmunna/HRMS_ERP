using GTERP.BLL;
using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.POS
{
    [OverridableAuthorize]
    public class IntegrationSettingController : Controller
    {
        private readonly GTRDBContext db;
        private TransactionLogRepository tranlog;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        public clsProcedure clsProc { get; }
        //public CommercialRepository Repository { get; set; }



        public IntegrationSettingController(GTRDBContext gtrdb, TransactionLogRepository tran)
        {
            db = gtrdb;
            tranlog = tran;
            //Repository = rep;
        }

        // GET: PurchaseRequisition
        public async Task<IActionResult> Index()
        {
            //var comid = HttpContext.Session.GetString("comid");
            //var userid = HttpContext.Session.GetString("userid");
            ////var gTRDBContext = db.StoreRequisitionMain.Where(x => x.ComId == comid).Include(s => s.ApprovedBy).Include(s => s.Department).Include(s => s.PrdUnit).Include(s => s.Purpose).Include(s => s.RecommenedBy);

            /////////////get user list from the server //////

            //var appKey = HttpContext.Session.GetString("appkey");
            //var model = new GetUserModel();
            //model.AppKey = Guid.Parse(appKey);
            //WebHelper webHelper = new WebHelper();
            //string req = JsonConvert.SerializeObject(model);
            ////Uri url = new Uri(string.Format("https://localhost:44336/api/user/GetUsersCompanies"));
            ////Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/GetUsersCompanies")); ///enable ssl certificate for secure connection
            //Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            //string response = webHelper.GetUserCompany(url, req);
            //GetResponse res = new GetResponse(response);

            //var list = res.MyUsers.ToList();
            //var l = new List<CompanyPermissionsController.AspnetUserList>();
            //foreach (var c in list)
            //{
            //    var le = new CompanyPermissionsController.AspnetUserList();
            //    le.Email = c.UserName;
            //    le.UserId = c.UserID;
            //    le.UserName = c.UserName;
            //    l.Add(le);
            //}

            //ViewBag.Userlist = new SelectList(l, "UserId", "UserName", userid);
            string comid = HttpContext.Session.GetString("comid");



            //ViewBag.IntegrationSettingMainId = new SelectList(db.Cat_Integration_Setting_Mains.Where(x => x.ComId == comid).Select(s => new { Text = s.Name, Value = s.IntegrationSettingMainId }).ToList(), "Value", "Text");


            return View(await db.Cat_Integration_Setting_Mains.Include(a => a.Acc_VoucherType).Include(a => a.Cat_Integration_Setting_Details).ThenInclude(a => a.Acc_ChartOfAccounts).ToListAsync());


            //return View();
        }

        public partial class PurchaseReQuisitionResult
        {
            public int PurReqId { get; set; }
            public string PRNo { get; set; }
            [Display(Name = "Product Unit")]
            public string PrdUnitName { get; set; }

            [Display(Name = "Requisition Ref")]
            public string ReqRef { get; set; }

            [Display(Name = "Requisition Date")]
            public string ReqDate { get; set; }

            [Display(Name = "Board Meeting Date")]
            public string BoardMeetingDate { get; set; }

            public string PurposeName { get; set; }


            public string DeptName { get; set; }


            public string ApprovedBy { get; set; }


            public string RecommenedBy { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }

            public string Remarks { get; set; }

            [Display(Name = "Required Date")]
            public string RequiredDate { get; set; }




        }

        public IActionResult GetTableColumn(string tableName)
        {
            string comid = HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@TableName", tableName);
            var columns = Helper.ExecProcMapTList<TableColumn>("prcGetTableColumn", parameter);
            return Json(columns);
        }

        public class TableColumn
        {
            public string ColumnName { get; set; }
            public string ORDINAL_POSITION { get; set; }
        }


        // GET: PurchaseRequisition/Create
        public IActionResult Create()
        {

            InitViewBag("Create");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cat_Integration_Setting_Main Cat_Integration_Setting_Main)
        {
            try
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

                if (ModelState.IsValid)
                {
                    if (Cat_Integration_Setting_Main.IntegrationSettingMainId > 0)
                    {
                        Cat_Integration_Setting_Main.DateUpdated = DateTime.Now;
                        Cat_Integration_Setting_Main.ComId = HttpContext.Session.GetString("comid");
                        Cat_Integration_Setting_Main.UpdateByUserId = HttpContext.Session.GetString("userid");
                        Cat_Integration_Setting_Main.DateUpdated = DateTime.Now;

                        if (Cat_Integration_Setting_Main.UserId == null)
                        {
                            Cat_Integration_Setting_Main.UserId = HttpContext.Session.GetString("userid");
                        }
                        Cat_Integration_Setting_Main.UpdateByUserId = HttpContext.Session.GetString("userid");


                        if (Cat_Integration_Setting_Main.Cat_Integration_Setting_Details != null)
                        {
                            foreach (var item in Cat_Integration_Setting_Main.Cat_Integration_Setting_Details)
                            {
                                if (item.IsDelete)
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                }
                                else
                                {

                                    if (item.IntegrationSettingDetailsId > 0)
                                    {
                                        item.DateUpdated = DateTime.Now;
                                        item.UpdateByUserId = HttpContext.Session.GetString("userid");

                                        db.Entry(item).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        item.UserId = HttpContext.Session.GetString("userid");
                                        item.ComId = HttpContext.Session.GetString("comid");

                                        item.DateAdded = DateTime.Now;
                                        db.Entry(item).State = EntityState.Added;
                                    }
                                }

                            }
                        }

                        db.Entry(Cat_Integration_Setting_Main).State = EntityState.Modified;
                        db.SaveChanges();

                        TempData["Status"] = "2";
                        TempData["Message"] = "Data Update Successfully";

                        return Json(new { Success = 2, data = Cat_Integration_Setting_Main.IntegrationSettingMainId, ex = TempData["Message"].ToString() });
                    }
                    else
                    {
                        Cat_Integration_Setting_Main.UserId = HttpContext.Session.GetString("userid");
                        Cat_Integration_Setting_Main.ComId = HttpContext.Session.GetString("comid");
                        Cat_Integration_Setting_Main.DateAdded = DateTime.Now;

                        if (Cat_Integration_Setting_Main.Cat_Integration_Setting_Details != null)
                        {
                            foreach (var item in Cat_Integration_Setting_Main.Cat_Integration_Setting_Details)
                            {
                                item.UserId = HttpContext.Session.GetString("userid");
                                item.ComId = HttpContext.Session.GetString("comid");
                                item.DateAdded = DateTime.Now;
                            }
                        }


                        db.Cat_Integration_Setting_Mains.Add(Cat_Integration_Setting_Main);
                        db.SaveChanges();
                        //TempData["Status"] = "1";
                        TempData["Message"] = "Data Save Successfully";

                        return Json(new { Success = 1, data = Cat_Integration_Setting_Main.IntegrationSettingMainId, ex = TempData["Message"].ToString() }); ;
                    }
                }
                else
                {
                    return Json(new { Success = 3, ex = "Model State Not Valid" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = 3, ex = e.Message });

            }


        }

        public JsonResult ProductInfo(int id)
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));



                var product = db.Products.Where(y => y.ProductId == id && y.comid == comid).SingleOrDefault();
                var unit = db.Unit.Where(y => y.UnitId == product.UnitId).SingleOrDefault();


                return Json(unit);
                //return Json("tesst" );

            }
            catch (Exception ex)
            {
                return Json(new { success = false, values = ex.Message.ToString() });
            }
            //return Json(new SelectList(product, "Value", "Text" ));
        }

        // POST: PurchaseRequisition/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(PurchaseRequisitionMain purchaseRequisitionMain)
        //{
        //    var ex = "";
        //    try
        //    {
        //        //if (ModelState.IsValid)
        //        //{

        //        #region Mandatory Parameter

        //        var comid = HttpContext.Session.GetString("comid");
        //        var userid = HttpContext.Session.GetString("userid");
        //        var AddDate = DateTime.Now;
        //        var UpdateDate = DateTime.Now;
        //        var PcName = HttpContext.Session.GetString("pcname");
        //        #endregion


        //        DateTime date = purchaseRequisitionMain.ReqDate;
        //        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        //        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        //        var activefiscalmonth = db.Acc_FiscalMonths.Where(x => x.ComId == comid && x.OpeningdtFrom >= firstDayOfMonth && x.ClosingdtTo <= lastDayOfMonth).FirstOrDefault();// && x.dtFrom.ToString() == monthid.ToString()
        //        if (activefiscalmonth == null)
        //        {
        //            return Json(new { Success = 0, ex = "No Active Fiscal Month Found" });
        //        }
        //        var activefiscalyear = db.Acc_FiscalYears.Where(x => x.FYId == activefiscalmonth.FYId).FirstOrDefault();
        //        if (activefiscalyear == null)
        //        {
        //            return Json(new { Success = 0, ex = "No Active Fiscal Year Found" });
        //        }


        //        //if (ModelState.IsValid)
        //        //{
        //        #region Edit request 
        //        if (purchaseRequisitionMain.PurReqId > 0)
        //        {

        //            purchaseRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
        //            purchaseRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;


        //            purchaseRequisitionMain.ComId = comid;
        //            purchaseRequisitionMain.UpdateByUserId = userid;
        //            purchaseRequisitionMain.DateUpdated = UpdateDate;
        //            IQueryable<PurchaseRequisitionSub> PurchaseRequisitionSub = db.PurchaseRequisitionSub.Where(p => p.PurReqId == purchaseRequisitionMain.PurReqId);

        //            //foreach (PurchaseRequisitionSub prsdelete in PurchaseRequisitionSub)
        //            //{
        //            //    db.PurchaseRequisitionSub.Remove(prsdelete);
        //            //}
        //            var sl = 0;
        //            foreach (PurchaseRequisitionSub item in purchaseRequisitionMain.PurchaseRequisitionSub)
        //            {
        //                sl++;
        //                if (item.PurReqSubId > 0)
        //                {
        //                    if (item.IsDelete!=true)
        //                    {
        //                        db.Entry(item).State = EntityState.Modified;
        //                        db.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        db.Entry(item).State = EntityState.Deleted;
        //                        db.SaveChanges();
        //                    }

        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        var sub = new PurchaseRequisitionSub();
        //                        sub.ComId = comid;
        //                        sub.DateAdded = item.DateAdded;
        //                        sub.DateUpdated = item.DateUpdated;
        //                        sub.Note = item.Note;
        //                        sub.PcName = PcName;
        //                        sub.ProductId = item.ProductId;
        //                        sub.PurReqId = item.PurReqId;
        //                        sub.PurReqQty = item.PurReqQty;
        //                        sub.PurReqSubId = item.PurReqSubId;
        //                        sub.RemainingReqQty = item.RemainingReqQty;
        //                        sub.SLNo = sl;
        //                        sub.UpdateByUserId = item.UpdateByUserId;

        //                        sub.UserId = userid;

        //                        db.PurchaseRequisitionSub.Add(sub);

        //                    }
        //                    catch (Exception e)
        //                    {

        //                        Console.WriteLine(e.Message);
        //                    }

        //                }

        //            }

        //            db.Entry(purchaseRequisitionMain).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }
        //        #endregion

        //        #region Create Request

        //        else
        //        {
        //            using (var tr = db.Database.BeginTransaction())
        //            {

        //                purchaseRequisitionMain.FiscalMonthId = activefiscalmonth.FiscalMonthId;
        //                purchaseRequisitionMain.FiscalYearId = activefiscalyear.FiscalYearId;

        //                purchaseRequisitionMain.ComId = comid;
        //                purchaseRequisitionMain.UserId = userid;
        //                purchaseRequisitionMain.DateAdded = AddDate;
        //                var main = new PurchaseRequisitionMain();
        //                main = purchaseRequisitionMain;
        //                var sl = 0;
        //                foreach (var item in purchaseRequisitionMain.PurchaseRequisitionSub)
        //                {
        //                    sl++;
        //                    var sub = new PurchaseRequisitionSub();
        //                    sub.ComId = comid;
        //                    sub.DateAdded = AddDate;
        //                    sub.DateUpdated = item.DateUpdated;
        //                    sub.Note = item.Note;
        //                    sub.PcName = PcName;
        //                    sub.ProductId = item.ProductId;
        //                    sub.PurReqId = purchaseRequisitionMain.PurReqId;
        //                    sub.PurReqQty = item.PurReqQty;
        //                    sub.LastPurchasePrice = item.LastPurchasePrice;
        //                    sub.RemainingReqQty = item.RemainingReqQty;
        //                    sub.SLNo = sl;
        //                    sub.UpdateByUserId = item.UpdateByUserId;
        //                    sub.UserId = userid;
        //                }

        //                db.PurchaseRequisitionMain.Add(main);
        //                db.SaveChanges();

        //                tr.Commit();
        //            }

        //        } 
        //        #endregion


        //        return Json(new { Success = 1, PurReqId = purchaseRequisitionMain.PurReqId, ex = "" });
        //        //}
        //        //}



        //    }
        //    catch (Exception e)
        //    {
        //        ex = e.Message;
        //        //return Json(new { Success = 0, error = 1, ex = e.Message });
        //    }
        //    #region ViewBag Initialization

        //    //ViewData["Employees"] = new SelectList(db.HR_Emp_Info.Select(x => new { x.EmpId, x.EmpCode, x.EmpName }), "EmpId", "EmpName", purchaseRequisitionMain.ApprovedByEmpId);
        //    //ViewData["CategoryId"] = new SelectList(db.Categories.Select(x => new { x.CategoryId, x.Name }), "CategoryId", "Name");
        //    //ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Select(x => new { x.DeptId, x.DeptName }), "DeptId", "DeptName", purchaseRequisitionMain.DepartmentId);
        //    //ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Select(x => new { x.PrdUnitId, x.PrdUnitShortName }), "PrdUnitId", "PrdUnitShortName", purchaseRequisitionMain.PrdUnitId);
        //    //ViewData["PurposeId"] = new SelectList(db.Purpose.Select(x => new { x.PurposeId, x.PurposeName }), "PurposeId", "PurposeName", purchaseRequisitionMain.PurposeId);

        //    //ViewData["ProductId"] = new SelectList(db.Products.Select(x => new { x.ProductId, x.ProductName }), "ProductId", "ProductName");

        //    //ViewData["UnitId"] = new SelectList(db.Unit.Select(x => new { x.UnitId, x.UnitName }), "UnitId", "UnitName"); 
        //    #endregion

        //    return Json(new { Success = 0, error = 1, ex = ex });

        //}
        //[ValidateAntiForgeryToken]

        public JsonResult GetProducts(int? id)
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

        [HttpPost]
        public ActionResult DeletePrSub(int prsubid)
        {
            try
            {
                var sub = db.PurchaseRequisitionSub.Find(prsubid);
                db.PurchaseRequisitionSub.Remove(sub);
                db.SaveChanges();
                return Json(new { error = 0, success = 1, message = "This record deleted successfully" });
            }
            catch (Exception ex)
            {
                var m = $" Message:{ex.Message}\nInner Exception:{ex.InnerException.Message}";
                return Json(new { error = 1, success = 0, message = m });
            }

        }

        // GET: PurchaseRequisition/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit";

            Cat_Integration_Setting_Main Cat_Integration_Setting_Main = await db.Cat_Integration_Setting_Mains
                .Include(p => p.Cat_Integration_Setting_Details)
                .ThenInclude(x => x.Acc_ChartOfAccounts)

                .Where(p => p.IntegrationSettingMainId == id)
                .FirstOrDefaultAsync();

            if (Cat_Integration_Setting_Main == null)
            {
                return NotFound();
            }
            InitViewBag("Edit", id, Cat_Integration_Setting_Main);
            //return Json(new {data= purchaseRequisitionMain });
            return View("Create", Cat_Integration_Setting_Main);
        }



        public async Task<IActionResult> CreateCopy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Cat_Integration_Setting_Main Cat_Integration_Setting_Main = await db.Cat_Integration_Setting_Mains
                .Include(p => p.Cat_Integration_Setting_Details)
                .ThenInclude(x => x.Acc_ChartOfAccounts)

                .Where(p => p.IntegrationSettingMainId == id)
                .FirstOrDefaultAsync();

            Cat_Integration_Setting_Main.IntegrationSettingMainId = 0;

            Cat_Integration_Setting_Main.Cat_Integration_Setting_Details.ToList().ForEach(a => a.IntegrationSettingDetailsId = 0);
            Cat_Integration_Setting_Main.Cat_Integration_Setting_Details.ToList().ForEach(a => a.IntegrationSettingMainId = 0);


            if (Cat_Integration_Setting_Main == null)
            {
                return NotFound();
            }
            InitViewBag("Edit", id, Cat_Integration_Setting_Main);
            ViewBag.Title = "Create";

            //return Json(new {data= purchaseRequisitionMain });
            return View("Create", Cat_Integration_Setting_Main);
        }

        // POST: PurchaseRequisition/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, PurchaseRequisitionMain purchaseRequisitionMain)
        //{
        //    var comid = HttpContext.Session.GetString("comid");

        //    if (id != purchaseRequisitionMain.PurReqId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            db.Update(purchaseRequisitionMain);
        //            await db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PurchaseRequisitionMainExists(purchaseRequisitionMain.PurReqId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ApprovedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x=>x.ComId == comid), "EmployeeId", "EmployeeName", purchaseRequisitionMain.ApprovedByEmpId);
        //    ViewData["DepartmentId"] = new SelectList(db.Cat_Department.Where(x => x.ComId == comid), "DeptId", "DeptName", purchaseRequisitionMain.DepartmentId);
        //    ViewData["PrdUnitId"] = new SelectList(db.PrdUnits.Where(x => x.comid == comid), "PrdUnitId", "PrdUnitShortName", purchaseRequisitionMain.PrdUnitId);
        //    ViewData["PurposeId"] = new SelectList(db.Purpose, "PurposeId", "PurposeName", purchaseRequisitionMain.PurposeId);
        //    ViewData["RecommenedByEmpId"] = new SelectList(db.HR_Emp_Info.Where(x => x.ComId == comid), "EmployeeId", "EmployeeName", purchaseRequisitionMain.RecommenedByEmpId);
        //    ViewData["ProductId"] = new SelectList(db.Products.Where(x => x.comid == comid), "ProductId", "ProductName");
        //    ViewData["UnitId"] = new SelectList(db.Unit, "UnitId", "UnitName");
        //    ViewData["SectId"] = new SelectList(db.Cat_Section.Select(x => new { x.SectId, x.SectName }), "SectId", "SectName", purchaseRequisitionMain.SectId);
        //    return View(purchaseRequisitionMain);
        //}

        // GET: PurchaseRequisition/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Delete";

            Cat_Integration_Setting_Main Cat_Integration_Setting_Main = await db.Cat_Integration_Setting_Mains
                .Include(p => p.Cat_Integration_Setting_Details)
                .ThenInclude(x => x.Acc_ChartOfAccounts)
                .Where(p => p.IntegrationSettingMainId == id)
                .FirstOrDefaultAsync();
            // PurchaseRequisitionMain purchaseRequisitionMain = await db.PurchaseRequisitionMain.FindAsync(id);
            if (Cat_Integration_Setting_Main == null)
            {
                return NotFound();
            }
            InitViewBag("Delete", id, Cat_Integration_Setting_Main);
            //return Json(new {data= purchaseRequisitionMain });
            return View("Create", Cat_Integration_Setting_Main);

        }

        // POST: PurchaseRequisition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                var datas = await db.Cat_Integration_Setting_Details.Where(a => a.IntegrationSettingMainId == id).ToListAsync();
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        db.Remove(item);
                    }

                }
                var Cat_Integration_Setting_Main = await db.Cat_Integration_Setting_Mains.FindAsync(id);
                db.Cat_Integration_Setting_Mains.Remove(Cat_Integration_Setting_Main);
                await db.SaveChangesAsync();


                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), Cat_Integration_Setting_Main.IntegrationSettingMainId.ToString(), "Delete", Cat_Integration_Setting_Main.IntegrationSettingMainId.ToString());

                return Json(new { Success = 1, IntegrationSettingMainId = Cat_Integration_Setting_Main.IntegrationSettingMainId, ex = "" });

            }
            catch (Exception ex)
            {
                // If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                return Json(new
                {
                    Success = 0,
                    ex = ex.Message.ToString()
                });
            }
        }
        private void InitViewBag(string title, int? id = null, Cat_Integration_Setting_Main IntegrationSettingMian = null)
        {
            var comid = HttpContext.Session.GetString("comid");

            ViewBag.Title = title;
            if (title == "Create")
            {


                ViewBag.AccId = new SelectList(db.Acc_ChartOfAccounts.Where(x => x.ComId == comid && x.AccType == "L" && (x.AccCode.Contains("4-6-") || x.AccCode.Contains("4-1-") || x.AccCode.Contains("2-1-") || x.AccCode.Contains("1-1-11-") || x.AccCode.Contains("1-1-14-"))).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");
                ViewBag.IntegrationTableName = new SelectList(db.Cat_Variable.Where(x => x.ComId == comid && x.VarType == "TableName").Select(s => new { Text = s.VarName, Value = s.VarName }).ToList(), "Value", "Text");

                ViewBag.VoucherTypeId = new SelectList(db.Acc_VoucherTypes.Where(x => x.isSystem == false).Select(s => new { Text = s.VoucherTypeNameShort + " - [ " + s.VoucherTypeName + " ]", Value = s.VoucherTypeId }).ToList(), "Value", "Text");




                //ViewData["ProductId"] = new SelectList(db.Products.Where(c => c.comid == comid), "ProductId", "ProductName");

            }
            else if (title == "Edit" || title == "Delete")
            {

                ViewBag.IntegrationTableName = new SelectList(db.Cat_Variable.Where(x => x.ComId == comid && x.VarType == "TableName").Select(s => new { Text = s.VarName, Value = s.VarName }).ToList(), "Value", "Text", IntegrationSettingMian.IntegrationTableName);

                ViewBag.AccId = new SelectList(db.Acc_ChartOfAccounts.Where(x => x.ComId == comid && x.AccType == "L" && (x.AccCode.Contains("4-6-") || x.AccCode.Contains("4-1-") || x.AccCode.Contains("2-1-") || x.AccCode.Contains("1-1-11-") || x.AccCode.Contains("1-1-14-"))).Select(s => new { Text = s.AccName + " - [ " + s.AccCode + " ]", Value = s.AccId }).ToList(), "Value", "Text");

                ViewBag.VoucherTypeId = new SelectList(db.Acc_VoucherTypes.Where(x => x.isSystem == false).Select(s => new { Text = s.VoucherTypeNameShort + " - [ " + s.VoucherTypeName + " ]", Value = s.VoucherTypeId }).ToList(), "Value", "Text", IntegrationSettingMian.VoucherTypeId);

            }

        }
        public JsonResult SetSessionAccountReport(string rptFormat, string IntegrationSettingMainId, string FiscalYearId, string FiscalMonthId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                string userid = HttpContext.Session.GetString("userid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                string query = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (true)
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptIntegrationSetting";
                    filename = "Notes_" + DateTime.Now.Date;
                    query = "Exec Acc_rptIntegrationSetting '" + IntegrationSettingMainId + "', '" + FiscalYearId + "' ,'" + FiscalMonthId + "','" + comid + "','" + userid + "'";


                    HttpContext.Session.SetString("reportquery", query);
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
                }



                HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("Acc_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;



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


        private bool PurchaseRequisitionMainExists(int id)
        {
            return db.PurchaseRequisitionMain.Any(e => e.PurReqId == id);
        }
    }
}
