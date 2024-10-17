using GTERP.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GTERP.Controllers.HR
{
    public class ActiveUsersController : Controller
    {
        private readonly IHRRepository _hRRepository;
        public ActiveUsersController(IHRRepository hRRepository)
        {
            _hRRepository = hRRepository;
        }
        DateTime Date = DateTime.Now.Date;
        public IActionResult ActiveEmp(DateTime date)
        {
            var data = _hRRepository.ActiveEmpList(date);
            return View(data);

        }
        public IActionResult ActiveEmpMale(DateTime date)
        {
            var data = _hRRepository.ActiveEmpMale(date);
            return View(data);
        }
        public IActionResult ActiveEmpFemale(DateTime date)
        {
            var data = _hRRepository.ActiveEmpFemale(date);
            return View(data);
        }
        public IActionResult ActiveEmpOTYes(DateTime date)
        {
            var data = _hRRepository.ActiveEmpOTYes(date);
            return View(data);
        }
        public IActionResult ActiveEmpRelease(DateTime date)
        {
            var data = _hRRepository.ActiveEmpRelease(date);
            return View(data);
        }

        public IActionResult TotalEmployees(DateTime date)
        {
            var data = _hRRepository.TotalEmployees(date);
            return View(data);
        }

    }
}
