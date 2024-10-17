using GTERP.Interfaces;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GTERP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyAPIController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CompanyAPIController> _logger;
        public CompanyAPIController(ILogger<CompanyAPIController> logger, ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }


        [HttpPost]
        [Route("PostCompany")]
        public async Task<object> PostCompanyInformation(CompanyAPIModel model)
        {
            try
            {
                bool response = await _companyRepository.PostCompanyInformation(model);
                return Ok(response);
            }
            catch (Exception ex)

            {
                _logger.LogError(ex.InnerException.ToString());
                throw (ex);
            }
        }

        [HttpPost]
        [Route("PostSoftwarePurchase")]
        public async Task<object> PostSoftwareInformation(SoftwareAPIModel model)
        {
            try
            {
                bool response = await _companyRepository.PostSoftwarePurchaseInformation(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw (ex);
            }
        }
    }



}
