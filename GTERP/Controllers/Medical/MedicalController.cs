using GTERP.Models;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.ControllerFolderVM;

namespace GTERP.Controllers.Medical
{
    [OverridableAuthorize]
    public class MedicalController : Controller
    {
        private readonly GTRDBContext _context;
        private readonly IConfiguration _configuration;
        //public CommercialRepository Repository { get; set; }
        public MedicalController(IConfiguration configuration, GTRDBContext context)
        {
            _context = context;
            _configuration = configuration;
            //Repository = rep;
        }

        // GET: Medical
        public IActionResult Index()
        {

            string comid = HttpContext.Session.GetString("comid");
            // ViewData["DesigId"] = new SelectList(_context.Cat_Designation, "DesigId", "DesigName");
            // ViewData["DiagId"] = new SelectList(_context.Cat_MedicalDiagnosis, "DiagId", "DiagName");
            //ViewData["SectId"] = new SelectList(_context.Cat_Section, "SectId", "SectName");
            //ViewData["UnitId"] = new SelectList(_context.Cat_Unit, "UnitId", "UnitName");
            //ViewData["UnitId"] = new SelectList(_context.Cat_Unit, "UnitId", "UnitName");
            //ViewData["UnitId"] = new SelectList(_context.Cat_Unit, "UnitId", "UnitName");


            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpCode");

            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpCode");
            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpCode");
            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpCode");
            //ViewData["EmpId"] = new SelectList(_context.HR_Emp_Info, "EmpId", "EmpCode");


            //ViewData["PrdUnitId"] = new SelectList(_context.PrdUnits, "PrdUnitId", "PrdUnitName");
            ViewData["Patient"] = new SelectList(_context.Cat_Variable.Where(c => c.VarType == "ReleationType").OrderBy(c => c.SLNo), "VarId", "VarName");

            // for medical products
            ViewData["ProductId"] = new SelectList(_context.Products.Where(p => p.CategoryId.Equals(35)), "ProductId", "ProductName");

            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == comid
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");

            var doctorInfo = (from emp in _context.HR_Emp_Info
                              join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                              join s in _context.Cat_Section on emp.SectId equals s.SectId
                              join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                              where emp.ComId == comid && d.DeptId == 26
                              select new
                              {
                                  Value = emp.EmpId,
                                  Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                              }).ToList();

            ViewData["DoctorId"] = new SelectList(doctorInfo, "Value", "Text"); // only for medical

            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(Medical_Master medicalMaster)
        {
            string comid = HttpContext.Session.GetString("comid");
            string userid = HttpContext.Session.GetString("userid");

            if (medicalMaster.MedicalMasterId > 0)
            {
                var old = _context.Medical_Details.
                    Where(m => m.MedicalMasterId == medicalMaster.MedicalMasterId).ToList();
                if (old.Count > 0)
                {
                    _context.RemoveRange(old);
                    _context.SaveChanges();
                }
                medicalMaster.DateUpdated = DateTime.Now;
                medicalMaster.UpdateByUserId = userid;
                _context.Entry(medicalMaster).State = EntityState.Modified;

                if (medicalMaster.Medical_Detailses != null)
                {
                    foreach (var item in medicalMaster.Medical_Detailses)
                    {
                        item.MedicalDetaisId = 0;
                        item.ComId = comid;
                        item.UserId = userid;
                        item.DateAdded = DateTime.Now;
                        item.MedicalDetaisId = 0;
                        _context.Add(item);
                    }
                }

            }
            else
            {
                medicalMaster.DateAdded = DateTime.Now;
                medicalMaster.ComId = comid;
                medicalMaster.UserId = userid;

                _context.Add(medicalMaster);

            }

            _context.SaveChanges();

            TempData["Message"] = "Treatment Save/Update Successfully";
            return Json(new { Success = 1, ex = TempData["Message"].ToString() });


        }


        public void IssueCalculation(Medical_Master master)
        {
            try
            {
                IssueMain issueMain = new IssueMain();

                issueMain.IssueNo = "New Medical";
                issueMain.IssueDate = DateTime.Now;
                issueMain.IssueRef = "Medical";
                //issueMain.IsSubStore = true;
                issueMain.IsDirectIssue = true;

                foreach (var item in master.Medical_Detailses)
                {
                    var product = _context.Products.Where(p => p.ProductId == item.ProductId).FirstOrDefault();
                    IssueSub issueSub = new IssueSub();
                    issueSub.ProductId = item.ProductId;
                    issueSub.WarehouseId = 7; // this is hard code for medical sub store
                    issueSub.IssueQty = (decimal)item.Quantity;
                    issueSub.Rate = product.RetailerPrice; // or saleprice

                    issueMain.IssueSub.Add(issueSub);
                }
                _context.Add(issueMain);
                _context.SaveChanges();

            }
            catch (Exception e)
            {

                throw;
            }




        }


        public class Family
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        [HttpGet]
        public IActionResult GerEmpInfo(int id)
        {
            var empInfo = _context.HR_Emp_Info.Select(e => new
            {
                EmpId = e.EmpId,
                DeptId = e.DeptId,
                SectId = e.SectId,
                DesigId = e.DesigId,
                UnitId = e.UnitId,
                EmpName = e.EmpName
            }).Where(e => e.EmpId == id).FirstOrDefault();

            //var family = _context.HR_Emp_Family.Where(e => e.EmpId == id).FirstOrDefault();


            //List<Family> fMembers = new List<Family>()
            //  {
            //      new Family() { Text = empInfo.EmpName, Value = empInfo.EmpName }
            //  };

            //if (family != null)
            //{
            //    if (family.EmpFather!=null) fMembers.Add(new Family() { Text = family.EmpFather, Value = family.EmpFather });
            //    if (family.EmpMother!=null) fMembers.Add(new Family() { Text = family.EmpMother, Value = family.EmpMother });
            //    if (family.EmpSpouse!=null) fMembers.Add(new Family() { Text = family.EmpSpouse, Value = family.EmpSpouse });
            //    if (family.EmpChildName1 != null) fMembers.Add(new Family() { Text = family.EmpChildName1, Value = family.EmpChildName1 });
            //    if (family.EmpChildName2 != null) fMembers.Add(new Family() { Text = family.EmpChildName2, Value = family.EmpChildName2 });
            //    if (family.EmpChildName3 != null) fMembers.Add(new Family() { Text = family.EmpChildName3, Value = family.EmpChildName3 });
            //    if (family.EmpChildName4 != null) fMembers.Add(new Family() { Text = family.EmpChildName4, Value = family.EmpChildName4 });
            //}
            //var prescriptions = _context.Medical_Master
            //    .Join(_context.Medical_Details,
            //        m => m.MedicalMasterId,
            //        d => d.MedicalMasterId,
            //        (m, d) => new {
            //            MedicalMasterId=m.MedicalMasterId,
            //            Date=m.DateAdded,
            //            Advice=
            //        });



            var prescription = _context.Medical_Master.Include(e => e.Doctor).Select(a => new
            {
                MedicalMasterId = a.MedicalMasterId,
                //Patient = a.Patient,
                Doctor = a.Doctor.EmpName,
                Date = a.DtInput,
                Advice = a.Advice,
                EmpId = a.EmpId
            }).Where(a => a.EmpId == id).OrderByDescending(a => a.Date).ToList();


            return Json(new { EmpInfo = empInfo, Prescription = prescription });
        }



        [HttpGet]
        public IActionResult GerUOMInfo(int id)
        {
            var data = _context.Products.Include(p => p.vProductUnit)
                .Select(e => new
                {
                    UnitName = e.vProductUnit.UnitName,
                    ProductId = e.ProductId
                }).Where(p => p.ProductId == id).FirstOrDefault();

            return Json(data);
        }

        [HttpGet]
        public IActionResult GerPrescriptoin(int id)
        {
            var master = _context.Medical_Master
                .Select(e => new
                {
                    MedicalMasterId = e.MedicalMasterId,
                    EmpId = e.EmpId,
                    DoctorId = e.DoctorId,
                    Patient = e.Patient,
                    DtInput = e.DtInput,
                    Weight = e.Weight,
                    Pulse = e.Pulse,
                    BP = e.BP,
                    //DiagId = e.DiagId,
                    Advice = e.Advice
                }).Where(p => p.MedicalMasterId == id).FirstOrDefault();

            var details = _context.Medical_Details
                .Select(e => new
                {
                    MedicalMasterId = e.MedicalMasterId,
                    MedicalDetaisId = e.MedicalDetaisId,
                    ProductId = e.ProductId,
                    Patient = e.Patient,
                    MedicineName = e.MedicineName,
                    UOM = e.UOM,
                    Quantity = e.Quantity,
                    Remarks = e.Remarks != null ? e.Remarks : ""
                }).Where(p => p.MedicalMasterId == id).ToList();


            return Json(new { Master = master, Details = details });
        }


        // POST: Medical/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: Medical/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medical_Master = await _context.Medical_Master
                //.Include(m => m.Cat_MedicalDiagnosis)
                .Include(m => m.HR_Emp_Info)
                .Include(m => m.Doctor)
                .FirstOrDefaultAsync(m => m.MedicalMasterId == id);
            if (medical_Master == null)
            {
                return NotFound();
            }

            return View(medical_Master);
        }

        // POST: Medical/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var details = await _context.Medical_Master.Where(d => d.MedicalMasterId == id).ToListAsync();
                if (details.Count() > 0)
                {
                    _context.RemoveRange(details);
                    await _context.SaveChangesAsync();
                }
                var medical_Master = await _context.Medical_Master.FindAsync(id);
                if (medical_Master != null)
                {
                    _context.Medical_Master.Remove(medical_Master);
                    await _context.SaveChangesAsync();
                }

                return Json(new { Success = 1, ex = "Data delete successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 2, ex = ex.Message });

            }
        }
        private bool Medical_MasterExists(int id)
        {
            return _context.Medical_Master.Any(e => e.MedicalMasterId == id);
        }

        public static List<SelectListItem> ReportTypes = new List<SelectListItem>()
        {
        new SelectListItem() {Text="Internal Store", Value="IS"},
        new SelectListItem() { Text="Pharmacy Stock Resister", Value="PSR"},
        new SelectListItem() { Text="Log Resister", Value="LR"},
        new SelectListItem() { Text="Prescription", Value="rptPrescription"},
        new SelectListItem() { Text="Chikitsa bebosta patra", Value="rptChikitsabebostapatra"},
        new SelectListItem() { Text="Monthly TreatMent Report", Value="MT"}
        };

        public IActionResult Report()
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
            //Uri url = new Uri(string.Format("https://localhost:44336/api/user/GetUsersCompanies"));
            //Uri url = new Uri(string.Format("https://pqstec.com:92/api/User/GetUsersCompanies")); ///enable ssl certificate for secure connection
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            var l = new List<AspnetUserList>();
            foreach (var c in list)
            {
                var le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }

            ViewBag.Userlist = new SelectList(l, "UserId", "UserName", userid);
            //ViewBag.PrdUnitId = new SelectList(_context.PrdUnits, "PrdUnitId", "PrdUnitName", userid);
            ViewData["ProductId"] = new SelectList(_context.Products.Where(p => p.CategoryId.Equals(35)), "ProductId", "ProductName");
            ViewData["ReportType"] = new SelectList(ReportTypes, "Value", "Text");

            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == comid
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            ViewData["EmpId"] = new SelectList(empInfo, "Value", "Text");
            ViewBag.WarehouseId = new SelectList(_context.Warehouses.Where(w => w.IsMedicalWarehouse == true), "WarehouseId", "WhName");

            return View();
        }

        [HttpGet]
        public IActionResult GetReport(string userId, int productId, string reportType, int empId, DateTime fromDate, DateTime toDate, string rptFormat, string product, int? WarehouseId)
        {
            try
            {
                string comid = HttpContext.Session.GetString("comid");
                var reportname = "";
                var filename = "";
                string redirectUrl = "";
                if (reportType == "rptPrescription")
                {
                    reportname = "rptPrescription";
                    filename = "rptPrescription_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Medical_rptPrescription '" + comid + "', '" + empId + "','" + fromDate + "','" + toDate + "'";
                    HttpContext.Session.SetString("reportquery", "Exec Medical_rptPrescription '" + comid + "', '" + empId + "','" + fromDate + "','" + toDate + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Medical/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "IS")
                {
                    reportname = "rptInternalStore";
                    filename = "rptInternalStore_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Medical_rptInternalStore '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec Medical_rptInternalStore '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Medical/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "PSR")
                {
                    reportname = "rptFarStore";
                    filename = "rptFarStore_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Medical_rptFarStore '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec Medical_rptFarStore '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Medical/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "LR")
                {
                    reportname = "rptlogRegister";
                    filename = "rptlogRegister_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Medical_rptlogRegister '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec Medical_rptlogRegister '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Medical/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "rptChikitsabebostapatra")
                {
                    reportname = "rptChikitsabebostapatra";
                    filename = "rptChikitsabebostapatra_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Medical_rptPrescription '" + comid + "', '" + empId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec Medical_rptPrescription '" + comid + "', '" + empId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Medical/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }
                else if (reportType == "MT")
                {
                    reportname = "rptTotalTreatment";
                    filename = "rptTotalTreatment_List" + DateTime.Now.Date.ToString();
                    var query = "Exec Medical_rptMonthlyTreat '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'";
                    HttpContext.Session.SetString("reportquery", "Exec Medical_rptMonthlyTreat '" + comid + "', '" + productId + "','" + fromDate + "','" + toDate + "','" + WarehouseId + "'");
                    HttpContext.Session.SetString("ReportPath", "~/ReportViewer/Medical/" + reportname + ".rdlc");
                    HttpContext.Session.SetString("PrintFileName", filename.Replace(".", "").Replace(" ", "").Replace(",", "").Replace("\"", ""));
                }

                string DataSourceName = "DataSet1";
                GTERP.Models.Common.clsReport.strReportPathMain = HttpContext.Session.GetString("ReportPath");
                GTERP.Models.Common.clsReport.strQueryMain = HttpContext.Session.GetString("reportquery");
                GTERP.Models.Common.clsReport.strDSNMain = DataSourceName;

                //var ConstrName = "ApplicationServices";
                //string callBackUrl = Repository.GenerateReport(clsReport.strReportPathMain, clsReport.strQueryMain, ConstrName, rptFormat);
                //redirectUrl = callBackUrl;

                redirectUrl = this.Url.Action("Index", "ReportViewer", new { reporttype = rptFormat });
                return Json(new { Url = redirectUrl });
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
