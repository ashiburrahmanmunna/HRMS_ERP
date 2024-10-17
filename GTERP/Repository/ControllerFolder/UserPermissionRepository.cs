using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Repository.Base;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.CommercialVM;

namespace GTERP.Repository.ControllerFolder
{
    public class UserPermissionRepository : BaseRepository<UserPermission>, IUserPermissionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<UserPermissionRepository> _logger;
        private readonly IUrlHelper _urlHelper;
        private readonly IConfiguration _configuration;

        public WebHelper _webHelper { get; }
        public UserPermissionRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<UserPermissionRepository> logger,
            WebHelper webHelper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IConfiguration configuration
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
            _webHelper = webHelper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _configuration = configuration;
        }

        public IEnumerable<SelectListItem> Userlist()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            var userid = _httpcontext.HttpContext.Session.GetString("userid");
            var appKey = _httpcontext.HttpContext.Session.GetString("appkey");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();
            string req = JsonConvert.SerializeObject(model);
            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));
            string response = webHelper.GetUserCompany(url, req);
            GetResponse res = new GetResponse(response);

            var list = res.MyUsers.ToList();
            var l = new List<AspnetUserList>();
            foreach (var c in list)
            {
                var le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }
            return new SelectList(l.Where(u => !_context.UserPermission.Any(p => p.AppUserId == u.UserId)), "UserId", "UserName");
        }
    }
}
