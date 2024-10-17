using GTERP.Interfaces.HR;
using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;

namespace GTERP.Controllers.HR
{
    public class DailyAttendActiveController : Controller
    {
        private readonly IHRRepository _hRRepository;
        public DailyAttendActiveController(IHRRepository hRRepository)
        {
            _hRRepository = hRRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Absent(DateTime Date)
        {
            var data = _hRRepository.Absent(Date);
            return View(data);
        }
        public IActionResult Present(DateTime Date)
        {
            var data = _hRRepository.Present(Date);
            return View(data);
        }
        public IActionResult Late(DateTime Date)
        {
            var data = _hRRepository.Late(Date);
            return View(data);
        }
        public IActionResult Leave(DateTime Date)
        {
            var data = _hRRepository.Leave(Date);
            return View(data);
        }

        public IActionResult WHDay(DateTime Date)
        {
            var data = _hRRepository.WHDay(Date);
            return View(data);
        }
    }
}
