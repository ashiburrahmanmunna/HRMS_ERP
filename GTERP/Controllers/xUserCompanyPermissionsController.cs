using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GTCommercial.Models;
using GTCommercial.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GTCommercial.Controllers
{
    public class xUserCompanyPermissionsController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: UserCompanyPermissions
        public ActionResult Index()
        {
            var userCompanyPermissions = db.UserCompanyPermissions;//.Include(u => u.vCompanyList);
            return View(userCompanyPermissions.ToList());
        }

        // GET: UserCompanyPermissions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            UserCompanyPermission userCompanyPermission = db.UserCompanyPermissions.Find(id);
            if (userCompanyPermission == null)
            {
                return NotFound();
            }
            return View(userCompanyPermission);
        }

        // GET: UserCompanyPermissions/Create
        public ActionResult Create()
        {
            //var x = db.Identity//UserLogin;


            //ViewBag.CompanyId = new SelectList(db.Companys, "CompanyId", "CompanyName");
            ViewBag.Userlist = new SelectList(UserManager.Users.ToList(), "Id", "Email");



            return View();
        }

        // POST: UserCompanyPermissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "UserId,CompanyId,isDefault,SortNo")] UserCompanyPermission userCompanyPermission)
        {
            if (ModelState.IsValid)
            {
                db.UserCompanyPermissions.Add(userCompanyPermission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CompanyId = new SelectList(db.Companys, "CompanyId", "CompanyCode", userCompanyPermission.CompanyId);
            return View(userCompanyPermission);
        }

        // GET: UserCompanyPermissions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            UserCompanyPermission userCompanyPermission = db.UserCompanyPermissions.Find(id);
            if (userCompanyPermission == null)
            {
                return NotFound();
            }
            //ViewBag.CompanyId = new SelectList(db.Companys, "CompanyId", "CompanyCode", userCompanyPermission.CompanyId);
            return View(userCompanyPermission);
        }

        // POST: UserCompanyPermissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "UserId,CompanyId,isDefault,SortNo")] UserCompanyPermission userCompanyPermission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userCompanyPermission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CompanyId = new SelectList(db.Companys, "CompanyId", "CompanyCode", userCompanyPermission.CompanyId);
            return View(userCompanyPermission);
        }

        // GET: UserCompanyPermissions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            UserCompanyPermission userCompanyPermission = db.UserCompanyPermissions.Find(id);
            if (userCompanyPermission == null)
            {
                return NotFound();
            }
            return View(userCompanyPermission);
        }

        // POST: UserCompanyPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserCompanyPermission userCompanyPermission = db.UserCompanyPermissions.Find(id);
            db.UserCompanyPermissions.Remove(userCompanyPermission);
            db.SaveChanges();
            return RedirectToAction("Index");
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
