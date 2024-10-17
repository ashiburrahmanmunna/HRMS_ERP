using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class EmployeesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //UserLog //UserLog;

        public EmployeesController()
        {
            //UserLog = new //UserLog();
        }
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        // GET: Employees
        public ActionResult Index()
        {
            var employee = db.Employee.Include(e => e.CommercialCompany).Include(e => e.SubSection);
            return View(employee.ToList());
        }

        // GET: Employees/Create
        [Authorize]
        //[OverridableAuthorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName");
            ViewBag.SubSectId = new SelectList(db.SubSections, "SubSectId", "SubSectName");
            ViewBag.BaseEmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName");

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "EmployeeId,BaseEmployeeId,EmployeeCode,EmployeeName,Description,GS,EmployeeImage,ImagePath,FileExtension,CommercialCompanyId,comid,SubSectId,emailid")] Employee employee)
        {
            //if (ModelState.IsValid)

                try
                {
                if (employee.EmployeeId > 0)
                {
                    employee.comid = int.Parse(AppData.intComId);

                    //employee.DateUpdated = DateTime.Now;
                    //supplier.comid = int.Parse(AppData.intComId);
                    db.Entry(employee).State = EntityState.Modified;
   
                    TempData["Message"] = "Data Update Successfully";

                    db.SaveChanges();
                    TempData["Status"] = "2";

                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), employee.EmployeeId.ToString() , "Update");
                    

                }
                else
                {
                    employee.comid = int.Parse(AppData.intComId);
                    
                    db.Employee.Add(employee);
                    

                    //employee.DateAdded = DateTime.Now;
                    //employee.DateUpdated = DateTime.Now;
                    TempData["Message"] = "Data Save Successfully";

                    db.SaveChanges();
                    TempData["Status"] = "1";
                    //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), employee.EmployeeId.ToString(), "Create");


                }



                //return View();

                return RedirectToAction("Index");
                }



                catch (Exception ex)
                {

                    ViewBag.Title = "Create";

                    ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", employee.CommercialCompanyId);
                    ViewBag.SubSectId = new SelectList(db.SubSections, "SubSectId", "SubSectName", employee.SubSectId);
                    ViewBag.BaseEmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName", employee.BaseEmployeeId);
                    TempData["Message"] = "Unable to Save / Update";
                    employee.EmployeeId = 0;
                    TempData["Status"] = "0";

                return View(employee);
                throw ex;
            }

            return RedirectToAction("Index");
        }
        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit";

            if (id == null)
            {
                return BadRequest();
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", employee.CommercialCompanyId);
            ViewBag.SubSectId = new SelectList(db.SubSections, "SubSectId", "SubSectName", employee.SubSectId);
            ViewBag.BaseEmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName", employee.BaseEmployeeId);
            return View("Create", employee);
        }


        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete";

            if (id == null)
            {
                return BadRequest();
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.CommercialCompanyId = new SelectList(db.CommercialCompany, "CommercialCompanyId", "CompanyName", employee.CommercialCompanyId);
            ViewBag.SubSectId = new SelectList(db.SubSections, "SubSectId", "SubSectName", employee.SubSectId);
            ViewBag.BaseEmployeeId = new SelectList(db.Employee, "EmployeeId", "EmployeeName", employee.BaseEmployeeId);
            return View("Create", employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, SuperAdmin , Commercial-Admin ")]
        //[OverridableAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {


            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);

            TempData["Message"] = "Data Delete Successfully";

            db.SaveChanges();
            TempData["Status"] = "3";
            //UserLog.TransactionLog(ControllerContext.HttpContext.Request, Session, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), TempData["Message"].ToString(), employee.EmployeeId.ToString(),"Delete");


            return Json(new { Success = 1, id = employee.EmployeeId, ex = "Data Delete Successfully" });
            }
            catch (Exception ex)
            {

                TempData["Message"] = "Unable to Delete the Data.";
                TempData["Status"] = "3";

                return Json(new { Success = 0, ex = ex.Message.ToString() });

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
