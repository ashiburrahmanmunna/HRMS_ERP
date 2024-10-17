using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class BuildingTypeRepository : BaseRepository<Cat_BuildingType>, IBuildingTypeRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public BuildingTypeRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetBuildingType()
        {
            return _context.Cat_BuildingType.Select(x => new SelectListItem
            {
                Value = x.BId.ToString(),
                Text = x.BuildingName
            });
        }
    }
}
