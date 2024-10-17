using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class ShiftRepository : BaseRepository<Cat_Shift>, IShiftRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public ShiftRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetShiftList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.ShiftId.ToString(),
                Text = x.ShiftName + " [" + x.ShiftIn.ToShortTimeString() + " - " + x.ShiftOut.ToShortTimeString() + "]"
            });
        }
        public IEnumerable<SelectListItem> GetAttShiftNameList()
        {
            return _context.Cat_Shift.Select(x => new SelectListItem
            {
                Value = x.ShiftId.ToString(),
                Text = x.ShiftName
            });
        }
        public List<Cat_Shift> GetShiftByCompany(string comid)
        {
            var shiftList = _context.Cat_Shift.Where(a => a.ComId == comid).ToList();
            return shiftList;
        }
        public void ShiftDefalutDate(Cat_Shift cat_Shift)
        {
            TimeSpan time = cat_Shift.ShiftIn.TimeOfDay;
            cat_Shift.ShiftIn = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.ShiftOut.TimeOfDay;
            cat_Shift.ShiftOut = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.ShiftLate.TimeOfDay;
            cat_Shift.ShiftLate = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);


            time = cat_Shift.LunchTime.TimeOfDay;
            cat_Shift.LunchTime = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.LunchIn.TimeOfDay;
            cat_Shift.LunchIn = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.LunchOut.TimeOfDay;
            cat_Shift.LunchOut = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);


            time = cat_Shift.RegHour.TimeOfDay;
            cat_Shift.RegHour = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.TiffinTime.TimeOfDay;
            cat_Shift.TiffinTime = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.TiffinIn.TimeOfDay;
            cat_Shift.TiffinIn = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.TiffinOut.TimeOfDay;
            cat_Shift.TiffinOut = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.TiffinTime1.Value.TimeOfDay;
            cat_Shift.TiffinTime1 = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.TiffinTimeIn1.Value.TimeOfDay;
            cat_Shift.TiffinTimeIn1 = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.TiffinTime2.Value.TimeOfDay;
            cat_Shift.TiffinTime2 = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.TiffinTimeIn2.Value.TimeOfDay;
            cat_Shift.TiffinTimeIn2 = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.NightAllowTime.Value.TimeOfDay;
            cat_Shift.NightAllowTime = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.DinnerAllowTime.Value.TimeOfDay;
            cat_Shift.DinnerAllowTime = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            time = cat_Shift.FridayAllowTime.Value.TimeOfDay;
            cat_Shift.FridayAllowTime = new DateTime(1900, 01, 01, time.Hours, time.Minutes, time.Seconds);

            _context.Cat_Shift.Add(cat_Shift);
        }
    }
}
