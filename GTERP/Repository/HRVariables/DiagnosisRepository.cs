using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class DiagnosisRepository : BaseRepository<Cat_MedicalDiagnosis>, IDiagnosisRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public DiagnosisRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public IEnumerable<SelectListItem> GetDiagnosisList()
        {
            return GetAll().Select(x => new SelectListItem
            {
                Value = x.DiagId.ToString(),
                Text = x.DiagName
            });
        }
    }
}
