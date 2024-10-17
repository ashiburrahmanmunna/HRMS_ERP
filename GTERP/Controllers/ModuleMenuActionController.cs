using GTERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers
{
    public class ModuleMenuActionController : Controller
    {
        private readonly GTRDBContext _context;

        public ModuleMenuActionController(GTRDBContext context)
        {
            _context = context;
        }
        public IActionResult ModuleMenuActionList()
        {
            var data = _context.Module_Menu_Action.Include(x=>x.ModuleMenu).Where(x=>!x.IsDelete)
                       .ToList();
            return View(data);
        }

        public IActionResult CreateModuleMenuAction()
        {
            ViewBag.ModuleMenu = new SelectList(_context.ModuleMenus, "ModuleMenuId", "ModuleMenuName");
            ViewBag.Title = "Create";
            return View();
        }

        [HttpPost]
        public IActionResult CreateModuleMenuAction(Module_Menu_Action mmAction)
        {
            ViewBag.ModuleMenu = new SelectList(_context.ModuleMenus, "ModuleMenuId", "ModuleMenuName");
            ViewBag.Title = "Create";

            if (mmAction.Id > 0)
            {
                _context.Entry(mmAction).State = EntityState.Modified;
            }
            else
            {
                _context.Add(mmAction);
            }
            _context.SaveChanges();
            return RedirectToAction ("ModuleMenuActionList", mmAction);
        }

        public IActionResult EditModuleMenuAction(int id)
        {
            var data = _context.Module_Menu_Action.Find(id);
            ViewBag.ModuleMenu = new SelectList(_context.ModuleMenus, "ModuleMenuId", "ModuleMenuName");
            ViewBag.Title = "Edit";
            return View("CreateModuleMenuAction",data);
        }

        public IActionResult DeleteModuleMenuAction(int id)
        {
            var data = _context.Module_Menu_Action.Find(id);
            ViewBag.ModuleMenu = new SelectList(_context.ModuleMenus, "ModuleMenuId", "ModuleMenuName");
            ViewBag.Title = "Delete";
            return View("CreateModuleMenuAction", data);
        }

        [HttpPost, ActionName("DeleteModuleMenuAction")]
        public IActionResult DeleteModuleMenuActionConfirmed(int id)
        {
            ViewBag.ModuleMenu = new SelectList(_context.ModuleMenus, "ModuleMenuId", "ModuleMenuName");
            ViewBag.Title = "Delete";

            try
            {
                var data = _context.Module_Menu_Action.Find(id);
                data.IsDelete = true;
                _context.Update(data);
                _context.SaveChanges();

                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";

                return Json(new { Success = 1, DeptId = data.Id, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
    }
}
