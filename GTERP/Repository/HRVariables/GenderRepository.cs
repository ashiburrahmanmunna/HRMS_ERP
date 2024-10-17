using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Repository.HRVariables
{
    public class GenderRepository : IGenderRepository
    {
        private readonly GTRDBContext db;
        public GenderRepository(GTRDBContext context)
        {
            db = context;
        }
        public IEnumerable<SelectListItem> GenderList()
        {
            return new SelectList(db.Cat_Gender, "GenderId", "GenderName");
        }
    }
}
