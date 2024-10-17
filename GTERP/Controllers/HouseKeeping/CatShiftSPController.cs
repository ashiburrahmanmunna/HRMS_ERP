using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Controllers.HouseKeeping
{
    public class CatShiftSPController : Controller
    {
        private readonly ILogger<MenuPermissionController> _logger;
        private GTRDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IShiftRepository _shiftRepository;

        public CatShiftSPController(
            IConfiguration configuration,
            GTRDBContext context, 
            ILogger<MenuPermissionController> logger,
            IShiftRepository shiftRepository

            )
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _shiftRepository = shiftRepository;
        }
        public IActionResult Index()
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.NewShift = _context.HR_Cat_Shift_SP.Where(x=>x.ComId == comid).ToList();
            return View(_shiftRepository.GetAll().ToList());
            //return View();
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HR_Cat_Shift_SP hR_Cat_Shift = new HR_Cat_Shift_SP();
            var Cat_Shift = _shiftRepository.FindById(id);
            ViewBag.ShiftName = Cat_Shift.ShiftName;
            hR_Cat_Shift.ShiftId = Cat_Shift.ShiftId;
            hR_Cat_Shift.ShiftIn = Cat_Shift.ShiftIn;
            hR_Cat_Shift.ShiftOut = Cat_Shift.ShiftOut;
            hR_Cat_Shift.ShiftLate = Cat_Shift.ShiftLate;
            hR_Cat_Shift.ComId = Cat_Shift.ComId;
            hR_Cat_Shift.RegHour = Cat_Shift.RegHour;
            hR_Cat_Shift.ShiftDate = Cat_Shift.DtInput;
            //TimeSpan time = Cat_Shift.TiffinIn.TimeOfDay;
            if (Cat_Shift == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Create";
            return View(hR_Cat_Shift);
        }
        [HttpPost]
        public IActionResult Create(HR_Cat_Shift_SP hR_Cat_Shift_SP)
        {

            if (hR_Cat_Shift_SP.SP_ShiftId > 0)
            {
                _context.Entry(hR_Cat_Shift_SP).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                _context.Entry(hR_Cat_Shift_SP).State = EntityState.Added;
                _context.SaveChanges();
            }

            //TimeSpan time = Cat_Shift.TiffinIn.TimeOfDay;
            if (hR_Cat_Shift_SP == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Create";
            return RedirectToAction("Index");
        }

        public IActionResult EditShiftSP(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Edit";
            ViewBag.ShiftName = _context.HR_Cat_Shift_SP
                                .Include(x=>x.Cat_Shift)
                                .Where(x=>x.ShiftId == x.Cat_Shift.ShiftId && x.ComId==comid)
                                .Select(x=>x.Cat_Shift.ShiftName).FirstOrDefault();

            var data = _context.HR_Cat_Shift_SP.Find(id);
            return View("Create",data);
        }

        public IActionResult DeleteShiftSP(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            ViewBag.Title = "Delete";
            ViewBag.ShiftName = _context.HR_Cat_Shift_SP
                                .Include(x => x.Cat_Shift)
                                .Where(x => x.ShiftId == x.Cat_Shift.ShiftId && x.ComId == comid)
                                .Select(x => x.Cat_Shift.ShiftName).FirstOrDefault();

            var data = _context.HR_Cat_Shift_SP.Find(id);
            
            return View("Create", data);
        }

        [HttpPost, ActionName("DeleteShiftSP")]
        public IActionResult DeleteShiftSPConfirm(int? id)
        {
            var comid = HttpContext.Session.GetString("comid");
            try
            {
                var Cat_Shift = _context.HR_Cat_Shift_SP
                    .Include(x=>x.Cat_Shift)
                    .Where(x=>x.SP_ShiftId==id && x.ComId == comid)
                    .FirstOrDefault();
                _context.HR_Cat_Shift_SP.Remove(Cat_Shift);
                _context.SaveChanges();
                TempData["Message"] = "Data Delete Successfully";
                TempData["Status"] = "3";
               
                return Json(new { Success = 3, SP_ShiftId = Cat_Shift.SP_ShiftId, ex = TempData["Message"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.InnerException.InnerException.Message.ToString() });
            }
        }
    }
}
