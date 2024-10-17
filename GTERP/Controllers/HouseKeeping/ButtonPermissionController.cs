using GTERP.Models;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    public class ButtonPermissionController : Controller
    {
        private readonly GTRDBContext _context;
        public ButtonPermissionController(GTRDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string ComId)
        {
            ViewBag.ComName = new SelectList(_context.Companys.ToList(),"CompanyCode","CompanyName", ComId);
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ComId", ComId);
            var ButtonPermission = Helper.ExecProcMapTList<ButtonPermissionVM>("prcGetButtonPermission", sqlParameter);

            var data = ButtonPermission.ToList();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public JsonResult CreateButtonPermissions(List<ButtonPermission> BtnPermissions)
        {
            try
            {
                    if (BtnPermissions != null)
                    {
                        BtnPermissions.ForEach(x =>
                        {
                            var exist = _context.ButtonPermissions.Where(c => c.ComId == x.ComId).ToList();
                            if (exist.Count > 0)
                            {
                                _context.RemoveRange(exist);
                                _context.SaveChanges();
                            }
                        });

                        _context.ButtonPermissions.AddRange(BtnPermissions);
                        _context.SaveChanges();

                        return Json(new { Success = 1, ex = "" });
                    }
                    return Json(new { Success = 1, ex = "" });
                }
            
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
            return Json(new { Success = 0, ex = new Exception("Unable to save").Message.ToString() });
        }


        public class ButtonPermissionVM
        {
            public string ComId { get; set; }
            public string CompanyName { get; set; }
            public bool IsShowEarn { get; set; }
            public bool IsShowSettlement { get; set; }
            public bool IsShowSendEmail { get; set; }

        }

    }
}
