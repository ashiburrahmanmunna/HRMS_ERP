using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.ControllerFolder
{
    public class CompanyPermissionsRepository : BaseRepository<CompanyPermission>, ICompanyPermissionsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<CompanyPermissionsRepository> _logger;
        private readonly IConfiguration _configuration;


        public CompanyPermissionsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<CompanyPermissionsRepository> logger,
            IConfiguration configuration
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
            _configuration = configuration;
        }

        public void CompanyPermissionDelete(int id)
        {
            Country country = _context.Countries.Find(id);
            _context.Countries.Remove(country);
            _context.SaveChanges();
        }

        public List<CompanyUser> CompanyUserList(string UserId)
        {
            var appKey = _httpcontext.HttpContext.Session.GetString("appkey");
            Microsoft.Data.SqlClient.SqlParameter[] sqlParameter1 = new Microsoft.Data.SqlClient.SqlParameter[2];
            sqlParameter1[0] = new Microsoft.Data.SqlClient.SqlParameter("@BasedUserId", UserId);
            sqlParameter1[1] = new Microsoft.Data.SqlClient.SqlParameter("@AppKey", appKey);

            List<CompanyUser> abc = Helper.ExecProcMapTList<CompanyUser>("prcgetCompanyList", sqlParameter1).ToList();
            return abc;
        }
    }
}
