using GTERP.Interfaces;
using GTERP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly GTRDBContext _context;

        public CompanyRepository(GTRDBContext context)
        {
            _context = context;
        }

        public async Task<bool> PostCompanyInformation(CompanyAPIModel model)
        {
            try
            {
                if (!_context.Companys.Any(a => a.CompanyCode == model.CompanyId || a.CompanySecretCode == model.CompanyId))
                {
                    Company company = new Company
                    {
                        CompanyName = model.CompanyName,
                        comEmail = model.Email,
                        comPhone = model.Phone,
                        CompanySecretCode = model.CompanyId,
                        CompanyCode = model.CompanyId,
                        PrimaryAddress = model.CompanyAddress,
                        BusinessTypeId = 6
                    };

                    await _context.Companys.AddAsync(company);
                    int response = await _context.SaveChangesAsync();
                    if (response > 0) return true;
                    else return false;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return false;
        }

        public async Task<bool> PostSoftwarePurchaseInformation(SoftwareAPIModel model)
        {
            try
            {
                var companyId = await _context.Companys.Where(x => x.CompanySecretCode == model.ComId).Select(x => x.ComId).FirstOrDefaultAsync();

                if (companyId is not 0)
                {

                    AppKeys appKeys = new AppKeys
                    {
                        ComId = companyId,
                        AppKey = model.AppKey
                    };
                    await _context.AppKeys.AddAsync(appKeys);
                    int response = await _context.SaveChangesAsync();
                    if (response > 0) return true;
                    else return false;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return false;
        }
    }
}
