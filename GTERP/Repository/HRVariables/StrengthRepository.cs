using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using GTERP.Repository.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Repository.HRVariables
{
    public class StrengthRepository : BaseRepository<Cat_Strength>, IStrengthRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public StrengthRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetStrengthList()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "--Please Select--", Value = "0" });
            li.Add(new SelectListItem { Text = "Department", Value = "1" });
            li.Add(new SelectListItem { Text = "Section", Value = "2" });
            li.Add(new SelectListItem { Text = "Designation", Value = "3" });

            return li;
        }
    }
}
