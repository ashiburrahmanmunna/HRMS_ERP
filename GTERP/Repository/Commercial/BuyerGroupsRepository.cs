using GTERP.Interfaces.Commercial;
using GTERP.Models;
using GTERP.Repository.Base;
using GTERP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;


namespace GTERP.Repository.Commercial
{
    public class BuyerGroupsRepository : BaseRepository<BuyerGroup>, IBuyerGroupsRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<BuyerGroupsRepository> _logger;
        private readonly IUrlHelper _urlHelper;

        public WebHelper _webHelper { get; }
        public BuyerGroupsRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<BuyerGroupsRepository> logger,
            WebHelper webHelper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
            _webHelper = webHelper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public string ReportGenerate()
        {
            int id = 1;
            int comid = 4;
            var ReportPath = "CommercialReport/rptMasterLCSalesContact.rdlc";
            var SqlCmd = "Exec [rptMasterLCSalesContact] '" + id + "','" + comid + "'";
            var ConstrName = "ApplicationServices";
            var ReportType = "PDF";

            string callBackUrl = _urlHelper.Action("Index", "ReportViewer", new { reporttype = ReportType });
            return callBackUrl;
        }
    }
}
