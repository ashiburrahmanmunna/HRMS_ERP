using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class RecreationRepository : IRecreationRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public RecreationRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public void CreateRecreation(List<HR_Emp_Recreation> recreations)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");
            foreach (var item in recreations)
            {
                var exist = _context.HR_Emp_Recreation
                    .Where(r => r.DtPayment.Value.Year == item.DtPayment.Value.Year && r.EmpId == item.EmpId)
                    .ToList();

                if (exist.Count > 0)
                    _context.HR_Emp_Recreation.RemoveRange(exist);

                _context.HR_Emp_Recreation.Add(item);
            }
            _context.SaveChanges();
        }

        public List<RecreationViewModel> prcRecationList()
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@ComId", comid);
            var reCreation = Helper.ExecProcMapTList<RecreationViewModel>("HR_prcGetReCreation", parameter);
            return reCreation;
        }
    }
}
