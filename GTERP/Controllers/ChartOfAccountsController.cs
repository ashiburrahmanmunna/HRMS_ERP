using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using GTERP.AppData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Common;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ChartOfAccountsController : Controller
    {
        private GTRDBContext db;
        public ChartOfAccountsController(GTRDBContext context)
        {
            db = context;
        }

        #region Actions
        //[OverridableAuthorize]
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("userid")==null) 
            {
                return RedirectToRoute("GTR"); ;
            }
            //List<ChartOfAccount> modelList = new List<ChartOfAccount>(); && c.IsSysDefined == 0

            return View(db.ChartOfAccount.Where(c => c.AccId > 0).ToList());
            //modelList = ChartOfAccount.prcGetData();
            //return View(modelList);
        }

        //[OverridableAuthorize]
        public ActionResult Details(int Id)
        {
            //if (HttpContext.Session.GetString("userid")==null)
            //{
            //    return RedirectToRoute("GTR"); ;
            //}
            //prcLoadCombo();
            //return View(ChartOfAccount.prcGetData(Id));
            return View();

        }
                 



        //[OverridableAuthorize]
        public ActionResult Create()
        {               

            try
            {

                ViewBag.Title = "Create";
                if (HttpContext.Session.GetString("userid")==null)
            {
                return RedirectToRoute("GTR"); ;
            }

                //ViewBag.ParentId = new SelectList(db.ChartOfAccount, "AccId", "AccName").ToList();

                //ChartOfAccount model = new ChartOfAccount();
                //ViewBag.Parent = new SelectList(db.ChartOfAccount, "AccId", "AccName").ToList();

                List<COAtemp> COAParent = new List<COAtemp>();//(db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", HttpContext.Session.GetString("comid")))).ToList();
                


                //HttpContext.Session.GetString("comid");

                //List<COAtemp> COAParent = db.Database.ExecuteSqlCommand<COAtemp>("CreateStudents @p0, @p1", parameters: new[] { "Bill", "Gates" });


                ViewBag.Parent = COAParent; // new SelectList(COAParent, "AccId", "AccName").ToList();

                ViewBag.opFYId = new SelectList(db.FiscalYears.Where(c => c.FYId > 0 && c.ComId.ToString() == (AppData.intComId)), "FYId", "FYName");
                ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");
                //ViewBag.Parent = db.ChartOfAccount.Where(m => m.AccType == "G").ToList();

            //return View(model);
            return View();
            }
            catch (Exception ex)
            {

                throw(ex);
            }

        }

        public class COAtemp
        {
            public int AccId { get; set; }

            public string AccName { get; set; }
            //public string ProductSerialNo { get; set; }
        }

        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChartOfAccount model ,string title)
        {
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

                        if (title == "Delete")
                        {

                           var count = db.ChartOfAccount.Where(m => m.ParentID == model.AccId && m.ComId  ==  int.Parse(HttpContext.Session.GetString("comid"))).ToList();

                            if (count.Count > 0)
                            {
                                ViewBag.ErrorMessage =  "delete not possible";

                            }
                        }



                        db.Database.ExecuteSqlCommand("prcTran_Acc_Account  @p0,@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15 ", parameters: new[]
                        { title, Session.GetString("comid"),  model.AccId, model.AccName, model.AccType,model.ParentID,double.Parse(model.OpDebit.ToString()) ,double.Parse(model.OpCredit.ToString()), 0, 0, 0,model.CountryID, double.Parse(model.Rate.ToString()),model.opFYId.ToString(),DateTime.Now.Date, 0 });

                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("prcTran_Acc_Account  @p0,@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15 ", parameters: new[]
                        { "New", Microsoft.AspNetCore.Session.GetString("comid"), 0, model.AccName, model.AccType,model.ParentID,double.Parse(model.OpDebit.ToString()) ,double.Parse(model.OpCredit.ToString()), 0, 0, 0,model.CountryID, double.Parse(model.Rate.ToString()),model.opFYId.ToString(),DateTime.Now.Date, 0 });

                    }
                    //string sqlQuery = "Exec prcTran_Acc_Account 'NEW', " + Session["ComId"] + ", 0, '" + model.AccName + "', '" + model.AccType + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.OpDate) + "',0,0";


                    //var values = prcDataSave(model);
                    //if (values == "Data Saved Sucessfully")
                    //{
                        return RedirectToAction("Index");
                    //}
                    //ModelState.AddModelError("CustomError", values);
                    //prcLoadCombo();
                    //return View(model);
                }
                //prcLoadCombo();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                prcLoadCombo();
                return View(model);
            }
        }

        //[OverridableAuthorize]
        public ActionResult Edit(int ?Id)
        {
            //if (HttpContext.Session.GetString("userid")==null)
            //{
            //    return RedirectToRoute("GTR"); 
            //}
            //prcLoadCombo();
            //return View(ChartOfAccount.prcGetData(Id));
            //string cultureinfo = "bd-BD";
            ////string cultureinfo = "th-TH";

            //CultureInfo culture = new CultureInfo(cultureinfo, false);
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            //HttpContext.Session["isBarcodeSearch"] = true;
            //HttpContext.Session["isProductSearch"] = true;
            //HttpContext.Session["isIMEISearch"] = true;


            if (Id == null)
            {
                return BadRequest();
            }


            ChartOfAccount chartofaccount = db.ChartOfAccount.Find(Id);

            //if (chartofaccount.IsSysDefined == 0)
            //{
            //    return BadRequest();
            //}

            //SalesMain salesmain = db.SalesMains.Find(id);



            if (chartofaccount == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";


            //ViewBag.ParentId = new SelectList(db.ChartOfAccount, "AccId", "AccName").ToList();
            //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", Session["comid"]))).ToList();
            //ViewBag.ParentId = new SelectList(COAParent, "ParentId", "AccName");
            //ViewBag.ParentId = COAParent;


            List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", Session["comid"].ToString()))).ToList();
            ViewBag.Parent = COAParent; // new SelectList(COAParent, "AccId", "AccName").ToList();

            ViewBag.opFYId = new SelectList(db.FiscalYears.Where(c => c.FYId > 0 && c.ComId.ToString() == (AppData.intComId)), "FYId", "FYName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

            //Call Create View
            return View("Create", chartofaccount);
        }

        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChartOfAccount model)
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
                    prcLoadCombo();
                    return View(model);
                }
                prcLoadCombo();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(model);
            }
        }

        //[OverridableAuthorize]
        public ActionResult Delete(int Id)
        {

            if (Id == null)
            {
                return BadRequest();
            }


            ChartOfAccount chartofaccount = db.ChartOfAccount.Find(Id);

            //if (chartofaccount.IsSysDefined == 0)
            //{
            //    return BadRequest();
            //}
             
            //SalesMain salesmain = db.SalesMains.Find(id);



            if (chartofaccount == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Delete";


            //ViewBag.ParentId = new SelectList(db.ChartOfAccount, "AccId", "AccName").ToList();
            //List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", Session["comid"]))).ToList();
            //ViewBag.ParentId = new SelectList(COAParent, "ParentId", "AccName");
            //ViewBag.ParentId = COAParent;


            List<COAtemp> COAParent = (db.Database.SqlQuery<COAtemp>("[prcgetCOAParent]  @comid", new SqlParameter("comid", Session["comid"].ToString()))).ToList();
            ViewBag.Parent = COAParent; // new SelectList(COAParent, "AccId", "AccName").ToList();

            ViewBag.opFYId = new SelectList(db.FiscalYears.Where(c => c.FYId > 0 && c.ComId.ToString() == (AppData.intComId)), "FYId", "FYName");
            ViewBag.Country = new SelectList(db.Countries, "CountryId", "CountryName");

            //Call Create View
            return View("Create", chartofaccount);
    

        }

        [HttpPost]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ChartOfAccount model)
        {
            try
            {
                if (model.AccId > 0)
                {
                    db.Database.ExecuteSqlCommand("prcTran_Acc_Account  @p0,@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15 ", parameters: new[]
                    { "DELETE", Session["comid"],  model.AccId, model.AccName, model.AccType,model.ParentID,double.Parse(model.OpDebit.ToString()) ,double.Parse(model.OpCredit.ToString()), 0, 0, 0,model.CountryID, double.Parse(model.Rate.ToString()),model.opFYId.ToString(),DateTime.Now.Date, 0 });

                    return View(model);
                }
                prcLoadCombo();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                prcLoadCombo();
                return View(model);
            }
        }
        #endregion Actions



        #region Procedures
        public void prcLoadCombo()
        {
            //DataSet dsList = new DataSet();
            //clsConnectionNew clsCon = new clsConnectionNew(Session["dbACC"].ToString(), true);
            try
            {
                //string sqlQuery = "Exec prcGetCOA_Web '" + Session["ComId"] + "', -1 ";
                //clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                //dsList.Tables[0].TableName = "tblAccounts";
                //dsList.Tables[1].TableName = "tblCurrency";
                //dsList.Tables[2].TableName = "tblCurrencyLocal";
                //dsList.Tables[3].TableName = "tblFYear";

                //// Load Combo
                //List<clsCommon.clsCombo2> CstType = clsGenerateList.prcColumnTwo(dsList.Tables["tblAccounts"]);
                //ViewBag.COA_List = CstType;

                //List<clsCommon.clsCombo2> Cullist = clsGenerateList.prcColumnTwo(dsList.Tables["tblCurrency"]);
                //ViewBag.Currincy_List = Cullist;

                //List<clsCommon.clsCombo2> CurLlist = clsGenerateList.prcColumnTwo(dsList.Tables["tblCurrencyLocal"]);
                //ViewBag.CurrincyLocal_List = CurLlist;

                //List<clsCommon.clsCombo2> FYlist = clsGenerateList.prcColumnTwo(dsList.Tables["tblFYear"]);
                //ViewBag.FY_List = FYlist;
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

        public string prcDataSave(ChartOfAccount model)
        {
            //ArrayList arQuery = new ArrayList();
            //clsProcedure clsProc = new clsProcedure();
            //clsConnectionNew clsCon = new clsConnectionNew(Session["dbACC"].ToString(), true);
            Int32 acccode = 0;
            try
            {
                //add new
                //var Group = "L";
                //if (model.isGroup) Group = "G";

                //var sqlQuery1 = "Select convert(int,Acccode) As NewId from tblacc_coa where accid = " + model.ParentID + " and comid =  " + Session["ComId"] + "";
                //acccode = clsCon.GTRCountingData(sqlQuery1);

                //string sqlQuery = "Exec prcTran_Acc_Account 'NEW', " +Session["ComId"] + ", 0, '" + model.AccName + "', '" + ((model.isGroup == true) ? 'G' : 'L') + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.opDate) + "'," + ((model.isSubsidiaryLedger == true) ? 1 : 0) + ",0";
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

        public string prcUpdateData(ChartOfAccount model)
        {
            //ArrayList arQuery = new ArrayList();
            //clsProcedure clsProc = new clsProcedure();
            //clsConnectionNew clsCon = new clsConnectionNew(Session["dbACC"].ToString(), true);
            try
            {
                ////Update Data
                //string sqlQuery = "Exec prcTran_Acc_Account 'UPDATE', " + Session["ComId"] + ",'" + model.accId + "' , '" + model.AccName + "', '" + ((model.isGroup == true) ? 'G' : 'L') + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.opDate) + "'," + ((model.isSubsidiaryLedger == true) ? 1 : 0) + ",0";
                //arQuery.Add(sqlQuery);

                //double dblDebit = 0;//clsProc.GTRValidateDouble(txtDebit.Tag.ToString()) -  clsProc.GTRValidateDouble(txtDebit.Value.ToString());
                //double dblCredit = 0;//clsProc.GTRValidateDouble(txtCredit.Tag.ToString()) -  clsProc.GTRValidateDouble(txtCredit.Value.ToString());
                //double dblamnt = dblDebit - dblCredit;
                //double dblBalance = 0;//clsProc.GTRValidateDouble(txtbalance.Text.ToString()) - dblamnt;

                //sqlQuery = "Update  tblAcc_COA Set Balance=" + dblBalance + ",IsSubsidiaryLedger=" + (((model.isSubsidiaryLedger)== true)? 1 : 0) + " where AccId=" + Int32.Parse(model.accId.ToString()) + " AND COMID =  " + Session["ComId"] + "";
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

        public string prcDeleteData(ChartOfAccount model)
        {
            //ArrayList arQuery = new ArrayList();
            //clsProcedure clsProc = new clsProcedure();
            //clsConnectionNew clsCon = new clsConnectionNew(Session["dbACC"].ToString(), true);
            try
            {

                //string sqlQuery = "Exec prcTran_Acc_Account 'DELETE', " + Session["ComId"] + ",'" + model.accId + "' , '" + model.AccName + "', '" + ((model.isGroup == true) ? 'G' : 'L') + "', " + model.ParentID + ", " + double.Parse(model.OpDebit.ToString()) + ", " + double.Parse(model.OpCredit.ToString()) + ", 0, 0, 0 , " + model.CountryID + "," + double.Parse(model.Rate.ToString()) + ", " + model.opFYId.ToString() + ", '" + (model.opDate) + "'," + ((model.isSubsidiaryLedger == true) ? 1 : 0) + ",0";
                //arQuery.Add(sqlQuery);

                ////string  sqlQuery = "Exec prcTran_Acc_Account 'DELETE', " + Session["ComId"] + ", " + model.accId + ",'" + model.AccCode + "', '', " + model.ParentID + ", 0, 0, 0, 0, 0,0, 0, 0, ''";
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
    }
}