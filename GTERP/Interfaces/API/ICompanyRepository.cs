using GTERP.Models;
using System.Threading.Tasks;

namespace GTERP.Interfaces
{
    public interface ICompanyRepository
    {
        Task<bool> PostCompanyInformation(CompanyAPIModel model);
        Task<bool> PostSoftwarePurchaseInformation(SoftwareAPIModel model);
    }
}
