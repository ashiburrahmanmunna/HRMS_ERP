using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HR
{
    public class GatePassMsRepository : BaseRepository<GatePassMs>,IGatePassMsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;

        public GatePassMsRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> CatVariableList()
        {
            throw new NotImplementedException();
        }

        public void DeleteGatePassMs(int id)
        {
            var gatePassMs = _context.gatePassMs.Find(id);
            var gatePass = _context.gatePassMs.Where(e => e.GatePassID == gatePassMs.GatePassID).FirstOrDefault();
            if (gatePass != null)
            {
                gatePass.IsApproved = false;
                _context.Entry(gatePass).State = EntityState.Modified;

            }
            _context.SaveChanges();
        }

        public IEnumerable<GatePassMs> GetGatePassMsAll()
        {
            return _context.gatePassMs.Include(h => h.GatePassID).Where(x => !x.IsDelete).ToList();
        }
              

        IEnumerable<SelectListItem> IGatePassMsRepository.GetGatePassMsList()
        {
            throw new NotImplementedException();
        }
    }
}
