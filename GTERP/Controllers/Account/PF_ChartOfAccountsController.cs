using DataTablesParser;
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
using System.Data;
using System.Linq;

namespace GTERP.Controllers.Account
{
    [OverridableAuthorize]
    public class PF_ChartOfAccountsController : Controller
    {
        private TransactionLogRepository tranlog;

        private GTRDBContext db;
        Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
        //public CommercialRepository Repository { get; set; }


        public int isModelBase = 1;
        public PF_ChartOfAccountsController(GTRDBContext _db, TransactionLogRepository tran)
        {
            tranlog = tran;
            db = _db;
            //Repository = rep;
        }


        public static string AccCodeFirst = "";
        public static string AccCodeSecound = "-0";
        public static string AccCodeThird = "-00";
        public static string AccCodeFourth = "-000";
        public static string AccCodeFifth = "-00000";




        #region Actions
        // [OverridableAuthorize]
        public ActionResult Index()
        {
            TempData["Message"] = "Data Load Successfully";
            TempData["Status"] = "1";
            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), "", "List Show", "");

            string comid = HttpContext.Session.GetString("comid");
            //if (Session["userid"] == null)
            //{
            //    return RedirectToRoute("GTR"); ;
            //}

            List<PF_ChartOfAccount> chartofaccountlist = db.PF_ChartOfAccounts.Take(1).Include(x => x.ParentChartOfAccount).Where(c => c.AccId > 0 && c.comid == comid && c.IsSysDefined == 0).ToList();

            return View(chartofaccountlist);
            //modelList = PF_ChartOfAccount.prcGetData();
            //return View(modelList);
        }

        public ActionResult COATree()
        {
            var comid = HttpContext.Session.GetString("comid");
            //List<PF_ChartOfAccount> all = new List<PF_ChartOfAccount>();
            //using (GTRDBContext db = new GTRDBContext())
            //{
            var all = db.PF_ChartOfAccounts.Where(a => a.comid == comid).OrderBy(a => a.ParentID).ToList();
            //}
            return View(all);
        }


        // [OverridableAuthorize]
        public ActionResult Details(int Id)
        {
            //if (Session["userid"] == null)
            //{
            //    return RedirectToRoute("GTR"); ;
            //}
            //prcLoadCombo();
            //return View(PF_ChartOfAccount.prcGetData(Id));
            return View();

        }

        // [OverridableAuthorize]
        public ActionResult Create()
        {
            try
            {



                string comid = HttpContext.Session.GetString("comid");
                var model = new PF_ChartOfAccount();


                var lastacccoa = db.PF_ChartOfAccounts.Where(x => x.comid == comid).OrderByDescending(x => x.AccId).FirstOrDefault();



                ViewBag.Title = "Create";
                //    if (Session["userid"] == null)
                //{
                //    return RedirectToRoute("GTR"); ;
                //}

                //ViewBag.ParentIdId = new SelectList(db.PF_ChartOfAccounts, "AccId", "AccName").ToList();

                //PF_ChartOfAccount model = new PF_ChartOfAccount();
                //ViewBag.ParentId = new SelectList(db.PF_ChartOfAccounts, "AccId", "AccName").ToList();


                SqlParameter[] sqlParameter = new SqlParameter[1];
                sqlParameter[0] = new SqlParameter("@comid", comid);

                List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter);

                //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();

                ViewBag.opFYId = new SelectList(db.PF_FiscalYear.OrderByDescending(c => c.FYId).Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName");
                var defaultcurrency = db.Companys.Where(x => x.CompanyCode == comid).Select(x => x.CountryId).FirstOrDefault();
                model.CountryID = defaultcurrency;
                model.CountryIdLocal = defaultcurrency;
                model.AccType = "L";
                if (lastacccoa != null)
                {
                    //model.ParentID = lastacccoa.ParentID;
                    ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", lastacccoa.ParentID).ToList(); // new SelectList(COAParent, "AccId", "AccName").ToList();

                }
                else
                {
                    ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName").ToList(); // new SelectList(COAParent, "AccId", "AccName").ToList();

                }


                ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", defaultcurrency).ToList();
                //ViewBag.ParentId = db.PF_ChartOfAccounts.Where(m => m.AccType == "G").ToList();

                //return View(model);
                return View(model);
            }
            catch (Exception ex)
            {

                throw (ex);
            }

        }

        public class COAtemp
        {
            public int AccId { get; set; }

            public string AccName { get; set; }
            //public string ProductSerialNo { get; set; }
        }

        [HttpPost]
        // [OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PF_ChartOfAccount model, string title)
        {

            var comid = HttpContext.Session.GetString("comid");
            var userid = HttpContext.Session.GetString("userid");


            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);
            try
            {

                //var errors = ModelState.Where(x => x.Value.Errors.Any())
                //.Select(x => new { x.Key, x.Value.Errors });

                //if (ModelState.IsValid)
                {
                    if (model.AccId > 0)
                    {
                        //CustomerSerials.EntryDate = DateTime.Now;
                        //db.Entry(CustomerSerials).State = EntityState.Modified;
                        //db.SaveChanges();

                        if (isModelBase == 1)
                        {
                            if (model.ParentID == model.PrevParentID && model.AccType == model.PrevAccType)
                            {
                                model.useridUpdate = userid;
                                model.DateUpdated = DateTime.Now;
                                db.Entry(model).State = EntityState.Modified;
                                db.SaveChanges();

                                TempData["Message"] = "Data Update Successfully";
                                TempData["Status"] = "2";
                                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Update", model.AccName.ToString() + " " + model.AccCode);


                            }
                            else
                            {
                                var parentcodeinfo = db.PF_ChartOfAccounts.Where(x => x.AccId == model.ParentID && x.comid == comid).FirstOrDefault();
                                string maxacccsubid = db.PF_ChartOfAccounts.Where(x => x.ParentID == model.ParentID && x.comid == comid).Max(x => x.AccSubId).ToString();

                                if (model.AccType != model.PrevAccType)
                                {


                                    int ExistAccIdVoucher = db.PF_VoucherSubs.Where(x => x.AccId == model.AccId).Count();
                                    int ExistAccIdSubAccountsHead = db.PF_ChartOfAccounts.Where(x => x.ParentID == model.AccId && x.comid == comid).Count();


                                    ///blocking for existing data in the voucher
                                    //////blocking for it have some sub account or ledger account
                                    if (ExistAccIdVoucher > 0)
                                    {
                                        ModelState.AddModelError(string.Empty, "Not Possible to Change the Type. Cause It Already Contains Some Voucher.");
                                        ViewBag.Title = title;

                                        List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter);

                                        //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
                                        ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", model.ParentID);


                                        ViewBag.opFYId = new SelectList(db.Acc_FiscalYears.Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName", model.opFYId);
                                        ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", model.comid);

                                        return this.View(model);
                                    }
                                    else if (ExistAccIdSubAccountsHead > 0)
                                    {
                                        ModelState.AddModelError(string.Empty, "Not Possible to Change the Type. Cause It Already Contains Some Sub Accounts Head / Group");
                                        ViewBag.Title = title;


                                        List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter);
                                        //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
                                        ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", model.ParentID);


                                        ViewBag.opFYId = new SelectList(db.Acc_FiscalYears.Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName", model.opFYId);
                                        ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", model.comid);

                                        return this.View(model);

                                    }



                                    //if (maxacccsubid == null || maxacccsubid == "")
                                    //{
                                    //    maxacccsubid = "100";
                                    //}

                                    //model.ParentCode = parentcodeinfo.ParentCode;
                                    //model.DateAdded = DateTime.Now.Date;
                                    //model.DateUpdated = DateTime.Now.Date;
                                    //model.OpDate = DateTime.Now.Date;
                                    //model.AccSubId = int.Parse(maxacccsubid) + 1;
                                    //model.Level = parentcodeinfo.Level + 1;
                                    //model.comid = int.Parse(comid);
                                    //model.userid = Session["userid"].ToString();
                                    //model.AccCode = parentcodeinfo.ParentCode + "-" + model.AccSubId;
                                    //model.CountryIdLocal = model.CountryID;
                                    //model.Rate = 1;
                                    //model.OpDebitLocal = float.Parse((model.Rate * model.OpDebit).ToString());
                                    //model.OpCreditLocal = float.Parse((model.Rate * model.OpCredit).ToString());

                                    //db.Entry(model).State = EntityState.Modified;
                                    //db.SaveChanges();
                                }


                                //var parentcodeinfo = db.PF_ChartOfAccounts.Where(x => x.AccId == model.ParentID && x.ComId == comid).FirstOrDefault();
                                //string maxacccsubid = db.PF_ChartOfAccounts.Where(x => x.ParentID == model.ParentID && x.ComId == comid).Max(x => x.AccSubId).ToString();

                                if (maxacccsubid == null || maxacccsubid == "")
                                {
                                    int level = int.Parse(parentcodeinfo.Level.ToString());
                                    if (level == 0 || level == 1)///need to check the level if level 0 and 1 then "0" and  if level 2 then "00" if level 3 then "000"
                                    {
                                        maxacccsubid = "0";

                                    }
                                    else if (level == 2)
                                    {
                                        maxacccsubid = "10";

                                    }
                                    else if (level == 3)
                                    {
                                        maxacccsubid = "100";

                                    }
                                    else if (level == 4)
                                    {
                                        maxacccsubid = "10000";

                                    }
                                    else
                                    {
                                        ModelState.AddModelError(string.Empty, "Not Supportd Level. Maximum Level 5.");
                                        ViewBag.Title = title;

                                        List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter).ToList();

                                        //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
                                        ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", model.ParentID);


                                        ViewBag.opFYId = new SelectList(db.Acc_FiscalYears.Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName", model.opFYId);
                                        ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", model.comid);

                                        return this.View(model);

                                    }

                                }




                                string fahad = AccCodeMaker(model.AccType, int.Parse(parentcodeinfo.Level.ToString()), int.Parse(maxacccsubid) + 1, parentcodeinfo.AccCode);



                                //model.ParentCode = parentcodeinfo.AccCode;
                                ////model.DateAdded = DateTime.Now.Date;
                                //model.DateUpdated = DateTime.Now.Date;
                                ////model.OpDate = DateTime.Now.Date;
                                //model.AccSubId = int.Parse(maxacccsubid) + 1;
                                //model.Level = parentcodeinfo.Level + 1;
                                //model.comid = int.Parse(comid);
                                //model.useridUpdate = Session["userid"].ToString();
                                //model.AccCode = fahad;
                                //model.CountryIdLocal = model.CountryID;


                                model.ParentCode = parentcodeinfo.AccCode; //fahad test test test
                                //model.DateAdded = DateTime.Now.Date;
                                model.DateUpdated = DateTime.Now.Date;
                                //model.OpDate = DateTime.Now.Date;
                                model.AccSubId = int.Parse(maxacccsubid) + 1;
                                model.Level = parentcodeinfo.Level + 1;
                                model.comid = comid;
                                model.userid = HttpContext.Session.GetString("userid");
                                model.AccCode = fahad;// parentcodeinfo.ParentCode + "-" + model.AccSubId;
                                model.CountryIdLocal = model.CountryID;
                                model.IsCashItem = parentcodeinfo.IsCashItem;
                                model.IsBankItem = parentcodeinfo.IsBankItem;


                                if (model.AccType == "G")
                                {
                                    model.OpDebit = 0;
                                    model.OpCredit = 0;
                                    model.Rate = 0;
                                    model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
                                    model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());

                                }
                                else
                                {
                                    model.Rate = 1;
                                    model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
                                    model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());


                                }





                                db.Entry(model).State = EntityState.Modified;
                                db.SaveChanges();

                                TempData["Message"] = "Data Update Successfully";
                                TempData["Status"] = "2";
                                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Update", model.AccName.ToString() + " " + model.AccCode);

                            }


                            if (title == "Delete")
                            {

                                var count = db.PF_ChartOfAccounts.Where(m => m.ParentID == model.AccId && m.comid == comid).ToList();

                                if (count.Count > 0)
                                {
                                    ViewBag.ErrorMessage = "delete not possible";

                                }
                            }

                        }
                        else
                        {
                            // need change

                            ////db.Database.ExecuteSqlCommand("prcTran_PF_Account  @p0,@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15 ", parameters: new[]
                            ////{ title, Session["comid"],  model.AccId, model.AccName, model.AccType,model.ParentID,double.Parse(model.OpDebit.ToString()) ,double.Parse(model.OpCredit.ToString()), 0, 0, 0,model.CountryID, double.Parse(model.Rate.ToString()),model.opFYId.ToString(),DateTime.Now.Date, 0 });
                        }

                        TempData["Message"] = "Data Update Successfully";
                        TempData["Status"] = "2";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Update", model.AccName.ToString() + " " + model.AccCode);


                    }
                    else
                    {


                        var parentcodeinfo = db.PF_ChartOfAccounts.Where(x => x.AccId == model.ParentID && x.comid == comid).FirstOrDefault();
                        string maxacccsubid = db.PF_ChartOfAccounts.Where(x => x.ParentID == model.ParentID && x.comid == comid).Max(x => x.AccSubId).ToString();
                        if (maxacccsubid == null || maxacccsubid == "")
                        {
                            int level = int.Parse(parentcodeinfo.Level.ToString());
                            if (level == 0 || level == 1)///need to check the level if level 0 and 1 then "0" and  if level 2 then "00" if level 3 then "000"
                            {
                                maxacccsubid = "0";

                            }
                            else if (level == 2)
                            {
                                maxacccsubid = "10";

                            }
                            //else if (level > 2 && level < 4)
                            else if (level == 3)
                            {
                                maxacccsubid = "100";

                            }
                            else if (level == 4)
                            {
                                maxacccsubid = "10000";

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Not Supportd Level. Maximum Level 5.");
                                ViewBag.Title = title;

                                List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter).ToList();
                                //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
                                ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", model.ParentID);


                                ViewBag.opFYId = new SelectList(db.Acc_FiscalYears.Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName", model.opFYId);
                                ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", model.comid);

                                return this.View(model);

                            }

                        }

                        string fahad = "";

                        if (model.AccCode != null)
                        {
                            if (model.AccCode.Length > 9)
                            {
                                fahad = model.AccCode;

                            }

                        }
                        else
                        {
                            fahad = AccCodeMaker(model.AccType, int.Parse(parentcodeinfo.Level.ToString()), int.Parse(maxacccsubid) + 1, parentcodeinfo.AccCode);

                        }


                        if (isModelBase == 1)
                        {

                            model.ParentCode = parentcodeinfo.AccCode;
                            model.DateAdded = DateTime.Now.Date;
                            //model.DateUpdated = DateTime.Now.Date;
                            model.OpDate = DateTime.Now.Date;
                            model.AccSubId = int.Parse(maxacccsubid) + 1;
                            model.Level = parentcodeinfo.Level + 1;
                            model.comid = comid;
                            model.userid = HttpContext.Session.GetString("userid");
                            model.AccCode = fahad;// parentcodeinfo.ParentCode + "-" + model.AccSubId;
                            model.CountryIdLocal = model.CountryID;
                            model.IsCashItem = parentcodeinfo.IsCashItem;
                            model.IsBankItem = parentcodeinfo.IsBankItem;


                            if (model.AccType == "G")
                            {



                                model.OpDebit = 0;
                                model.OpCredit = 0;
                                model.Rate = 0;
                                model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
                                model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());

                            }
                            else
                            {
                                model.Rate = 1;
                                model.OpDebitLocal = decimal.Parse((model.Rate * model.OpDebit).ToString());
                                model.OpCreditLocal = decimal.Parse((model.Rate * model.OpCredit).ToString());


                            }


                            db.PF_ChartOfAccounts.Add(model);
                            db.SaveChanges();

                            TempData["Message"] = "Data Save Successfully";
                            TempData["Status"] = "1";
                            tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), model.AccId.ToString(), "Create", model.AccName.ToString() + "" + model.AccCode);


                        }
                        else
                        {
                            // need to change
                            ////db.Database.ExecuteSqlCommand("prcTran_PF_Account  @p0,@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15 ", parameters: new[]
                            ////{ "New", Session["comid"], 0, model.AccName, model.AccType,model.ParentID,double.Parse(model.OpDebit.ToString()) ,double.Parse(model.OpCredit.ToString()), 0, 0, 0,model.CountryID, double.Parse(model.Rate.ToString()),model.opFYId.ToString(),DateTime.Now.Date, 0 });


                        }


                    }

                    //string sqlQuery = "Exec prcTran_PF_Account 'NEW', " + Session["comid"] + ", 0, '" + model.AccName + "', '" + model.AccType + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.OpDate) + "',0,0";


                    //var values = prcDataSave(model);
                    //if (values == "Data Saved Sucessfully")
                    //{
                    return RedirectToAction("Create");
                    //}
                    //ModelState.AddModelError("CustomError", values);
                    //prcLoadCombo();
                    //return View(model);
                }
                //prcLoadCombo();

            }
            catch (Exception ex)
            {
                ViewBag.Title = title;
                List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter).ToList();

                //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
                //ViewBag.ParentId = COAParent; // new SelectList(COAParent, "AccId", "AccName").ToList();
                ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", model.ParentID); ; // new SelectList(COAParent, "AccId", "AccName").ToList();


                ViewBag.opFYId = new SelectList(db.Acc_FiscalYears.Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName", model.opFYId);
                ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", model.comid);

                ModelState.AddModelError("CustomError", ex.Message);

                return View(model);
            }
        }


        public static string AccCodeMaker(string type, int level, int accsubid, string parentcode)
        {
            string FinalAccCode = "";

            string[] splitcode = parentcode.Split('-');

            if (level == 0)
            {
                FinalAccCode = accsubid.ToString() + AccCodeSecound + AccCodeThird + AccCodeFourth + AccCodeFifth;

            }
            else if (level == 1)
            {
                FinalAccCode = splitcode[0].ToString() + "-" + accsubid.ToString() + AccCodeThird + AccCodeFourth + AccCodeFifth;

            }
            else if (level == 2)
            {
                FinalAccCode = splitcode[0].ToString() + "-" + splitcode[1].ToString() + "-" + accsubid.ToString() + AccCodeFourth + AccCodeFifth;

            }
            else if (level == 3)
            {
                FinalAccCode = splitcode[0].ToString() + "-" + splitcode[1].ToString() + "-" + splitcode[2].ToString() + "-" + accsubid.ToString() + AccCodeFifth;

            }
            else if (level == 4)
            {
                FinalAccCode = splitcode[0].ToString() + "-" + splitcode[1].ToString() + "-" + splitcode[2].ToString() + "-" + splitcode[3].ToString() + "-" + accsubid.ToString();

            }
            //else if (level == 5)
            //{
            //    FinalAccCode = splitcode[0].ToString() + "-" + splitcode[1].ToString() + "-" + splitcode[2].ToString() + "-" + splitcode[3].ToString() + "-" + splitcode[4].ToString() + "-"  + accsubid.ToString();
            //}
            else
            {

                FinalAccCode = "Maximum 5 Level Supported";

            }

            return FinalAccCode;
        }


        // [OverridableAuthorize]
        public ActionResult Edit(int? Id)
        {

            if (Id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return BadRequest();
            }


            PF_ChartOfAccount chartofaccount = db.PF_ChartOfAccounts.Find(Id);

            //if (chartofaccount.IsSysDefined == 0)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //SalesMain salesmain = db.SalesMains.Find(id);



            if (chartofaccount == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";

            chartofaccount.PrevParentID = chartofaccount.ParentID;
            chartofaccount.PrevAccType = chartofaccount.AccType;

            string comid = HttpContext.Session.GetString("comid");

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);


            //ViewBag.ParentIdId = new SelectList(db.PF_ChartOfAccounts, "AccId", "AccName").ToList();
            //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", Session["comid"]))).ToList();
            //ViewBag.ParentIdId = new SelectList(COAParent, "ParentId", "AccName");
            //ViewBag.ParentIdId = COAParent;



            List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter).ToList();

            //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
            ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", chartofaccount.ParentID).ToList(); // new SelectList(COAParent, "AccId", "AccName").ToList();

            ViewBag.opFYId = new SelectList(db.Acc_FiscalYears.OrderByDescending(c => c.FYId).Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName", chartofaccount.opFYId);
            ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", chartofaccount.CountryID).ToList();

            //var defaultcurrency = db.Companys.Where(x => x.ComId == comid).Select(x => x.CountryId).FirstOrDefault();


            //Call Create View
            return View("Create", chartofaccount);
        }

        [HttpPost]
        // [OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PF_ChartOfAccount model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var values = prcUpdateData(model);
                    if (values == "Data Updated Sucessfully")
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("CustomError", values);

                    return View(model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(model);
            }
        }

        [OverridableAuthorize]
        public ActionResult Delete(int Id)
        {

            if (Id == 0)
            {
                return BadRequest();
            }


            PF_ChartOfAccount chartofaccount = db.PF_ChartOfAccounts.Find(Id);

            //if (chartofaccount.IsSysDefined == 0)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //SalesMain salesmain = db.SalesMains.Find(id);



            if (chartofaccount == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";


            //ViewBag.ParentIdId = new SelectList(db.PF_ChartOfAccounts, "AccId", "AccName").ToList();
            //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", Session["comid"]))).ToList();
            //ViewBag.ParentIdId = new SelectList(COAParent, "ParentId", "AccName");
            //ViewBag.ParentIdId = COAParent;


            string comid = HttpContext.Session.GetString("comid");
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@comid", comid);

            List<COAtemp> COAParent = Helper.ExecProcMapTList<COAtemp>("PF_prcgetCOAParent", sqlParameter);

            //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
            ViewBag.ParentId = new SelectList(COAParent, "AccId", "AccName", chartofaccount.ParentID).ToList(); // new SelectList(COAParent, "AccId", "AccName").ToList();

            ViewBag.opFYId = new SelectList(db.Acc_FiscalYears.OrderByDescending(c => c.FYId).Where(c => c.FYId > 0 && c.ComId == (HttpContext.Session.GetString("comid"))), "FYId", "FYName", chartofaccount.opFYId);
            ViewBag.CountryId = new SelectList(db.Countries.Where(x => x.isActive == true), "CountryId", "CurrencyShortName", chartofaccount.CountryID).ToList();

            //Call Create View
            return View("Create", chartofaccount);


        }

        [HttpPost, ActionName("Delete")]
        //      [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            try
            {
                var comid = HttpContext.Session.GetString("comid");


                if (isModelBase == 1)
                {

                    PF_ChartOfAccount coa = db.PF_ChartOfAccounts.Where(c => c.comid == (HttpContext.Session.GetString("comid")) && c.AccId == id).FirstOrDefault();

                    int ExistAccIdVoucher = db.PF_VoucherSubs.Where(x => x.AccId == coa.AccId).Count();
                    int ExistAccIdSubAccountsHead = db.PF_ChartOfAccounts.Where(x => x.ParentID == coa.AccId && x.comid == comid).Count();



                    ///blocking for existing data in the voucher
                    //////blocking for it have some sub account or ledger account
                    if (ExistAccIdVoucher > 0)
                    {

                        return Json(new { Success = 0, ex = new Exception("Unable to Delete . Not Possible to Change the Type. Cause It Already Contains Some Voucher.").Message.ToString() });

                    }
                    else if (ExistAccIdSubAccountsHead > 0)
                    {

                        return Json(new { Success = 0, ex = new Exception("Unable to Delete . Not Possible to Change the Type. Cause It Already Contains Some Sub Accounts Head / Group").Message.ToString() });

                    }
                    else
                    {
                        db.PF_ChartOfAccounts.Remove(coa);
                        db.SaveChanges();

                        TempData["Message"] = "Data Delete Successfully";
                        TempData["Status"] = "3";
                        tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), coa.AccId.ToString(), "Delete", coa.AccName + " " + coa.AccCode);



                        return Json(new { Success = 1, AccId = 1, ex = "Delete Done Successfully." });

                    }

                }
                else
                {

                    //db.Database.ExecuteSqlCommand("prcTran_PF_Account  @p0,@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15 ", parameters: new[]
                    //{ "DELETE", Session["comid"],  model.AccId, model.AccName, model.AccType,model.ParentID,double.Parse(model.OpDebit.ToString()) ,double.Parse(model.OpCredit.ToString()), 0, 0, 0,model.CountryID, double.Parse(model.Rate.ToString()),model.opFYId.ToString(),DateTime.Now.Date, 0 });




                }
                return Json(new { Success = 1, AccId = 1, ex = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }

        }



        public class ChartOfAccountsabc
        {
            public int AccId { get; set; }
            public string AccName { get; set; }
            public string AccCode { get; set; }

            public string AccType { get; set; }
            public string ParentName { get; set; }



            //public DateTime BirthDate { get; set; }
            //public string BirthDateFormatted
            //{
            //    get
            //    {
            //        return String.Format("{0:M/d/yyyy}", BirthDate);
            //    }
            //}
        }

        public IActionResult Get()
        {
            try
            {
                var comid = (HttpContext.Session.GetString("comid"));
                //var abc = db.Products.Include(y => y.vPrimaryCategory);
                var query = from e in db.PF_ChartOfAccounts.Where(x => x.AccId > 0 && x.IsSysDefined == 0 && x.comid == comid).OrderByDescending(x => x.AccId)
                            select new ChartOfAccountsabc
                            {
                                AccId = e.AccId,
                                AccCode = e.AccCode,
                                AccName = e.AccName,
                                AccType = e.AccType,
                                ParentName = e.ParentChartOfAccount.AccName

                            };



                var parser = new Parser<ChartOfAccountsabc>(Request.Form, query);

                return Json(parser.Parse());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion Actions



        #region Procedures


        public string prcDataSave(PF_ChartOfAccount model)
        {
            //ArrayList arQuery = new ArrayList();
            //clsProcedure clsProc = new clsProcedure();
            //clsConnectionNew clsCon = new clsConnectionNew(Session["dbACC"].ToString(), true);

            try
            {
                //add new
                //var Group = "L";
                //if (model.isGroup) Group = "G";

                //var sqlQuery1 = "Select convert(int,Acccode) As NewId from tblacc_coa where accid = " + model.ParentID + " and comid =  " + Session["comid"] + "";
                //acccode = clsCon.GTRCountingData(sqlQuery1);

                //string sqlQuery = "Exec prcTran_PF_Account 'NEW', " +Session["comid"] + ", 0, '" + model.AccName + "', '" + ((model.isGroup == true) ? 'G' : 'L') + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.opDate) + "'," + ((model.isSubsidiaryLedger == true) ? 1 : 0) + ",0";
                //arQuery.Add(sqlQuery);

                //clsCon.GTRSaveDataWithSQLCommand(arQuery);
                return "Data Saved Sucessfully";
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //finally
            //{
            //    clsCon = null;
            //}
        }

        public string prcUpdateData(PF_ChartOfAccount model)
        {
            //ArrayList arQuery = new ArrayList();
            //clsProcedure clsProc = new clsProcedure();
            //clsConnectionNew clsCon = new clsConnectionNew(Session["dbACC"].ToString(), true);
            try
            {
                ////Update Data
                //string sqlQuery = "Exec prcTran_PF_Account 'UPDATE', " + Session["comid"] + ",'" + model.accId + "' , '" + model.AccName + "', '" + ((model.isGroup == true) ? 'G' : 'L') + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.opDate) + "'," + ((model.isSubsidiaryLedger == true) ? 1 : 0) + ",0";
                //arQuery.Add(sqlQuery);

                //double dblDebit = 0;//clsProc.GTRValidateDouble(txtDebit.Tag.ToString()) -  clsProc.GTRValidateDouble(txtDebit.Value.ToString());
                //double dblCredit = 0;//clsProc.GTRValidateDouble(txtCredit.Tag.ToString()) -  clsProc.GTRValidateDouble(txtCredit.Value.ToString());
                //double dblamnt = dblDebit - dblCredit;
                //double dblBalance = 0;//clsProc.GTRValidateDouble(txtbalance.Text.ToString()) - dblamnt;

                //sqlQuery = "Update  tblPF_COA Set Balance=" + dblBalance + ",IsSubsidiaryLedger=" + (((model.isSubsidiaryLedger)== true)? 1 : 0) + " where AccId=" + Int32.Parse(model.accId.ToString()) + " AND comid =  " + Session["comid"] + "";
                //arQuery.Add(sqlQuery);

                //clsCon.GTRSaveDataWithSQLCommand(arQuery);
                return "Data Updated Sucessfully";
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //finally
            //{
            //    clsCon = null;
            //}
        }

        public string prcDeleteData(PF_ChartOfAccount model)
        {
            //ArrayList arQuery = new ArrayList();
            //clsProcedure clsProc = new clsProcedure();
            //clsConnectionNew clsCon = new clsConnectionNew(Session["dbACC"].ToString(), true);
            try
            {

                //string sqlQuery = "Exec prcTran_PF_Account 'DELETE', " + Session["comid"] + ",'" + model.accId + "' , '" + model.AccName + "', '" + ((model.isGroup == true) ? 'G' : 'L') + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.opDate) + "'," + ((model.isSubsidiaryLedger == true) ? 1 : 0) + ",0";
                //arQuery.Add(sqlQuery);

                ////string  sqlQuery = "Exec prcTran_PF_Account 'DELETE', " + Session["comid"] + ", " + model.accId + ",'" + model.AccCode + "', '', " + model.ParentID + ", 0, 0, 0, 0, 0,0, 0, 0, ''";
                ////arQuery.Add(sqlQuery);

                //clsCon.GTRSaveDataWithSQLCommand(arQuery);
                return "Sucessfully Deleted.";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            //finally
            //{
            //    clsCon = null;
            //}
        }
        #endregion Procedures




        [HttpPost, ActionName("AccountsReport")]
        public JsonResult AccountsReport(string rptFormat, string action)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");

                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                //return Json(new { Success = 1, TermsId = param, ex = "" });
                if (action == "PrintAccCOA")
                {
                    //redirectUrl = new UrlHelper(Request.RequestContext).Action(action, "COM_BBLC_Master", new { FromDate = FromDate, ToDate = ToDate, Supplierid = SupplierId, CommercialCompanyId = CommercialCompanyId }); //, new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" }
                    reportname = "rptCOA";
                    filename = "rptCOA_List" + DateTime.Now.Date.ToString();
                    var query = "Exec [PF_rptCOA] '" + comid + "'";
                    HttpContext.Session.SetString("reportquery", "Exec [PF_rptCOA] '" + comid + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Accounts/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }


                string DataSourceName = "DataSet1";

                //HttpContext.Session.SetObject("PF_rptList", postData);

                //Common.Classes.clsMain.intHasSubReport = 0;
                clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");// Session["ReportPath"].ToString();
                clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;

                TempData["Message"] = "Chart Of Accounts Report Show";

                tranlog.TransactionLog(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), reportname, "Report", action);

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