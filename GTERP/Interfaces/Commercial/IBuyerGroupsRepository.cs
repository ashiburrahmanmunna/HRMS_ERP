using GTERP.Interfaces.Base;
using GTERP.Models;

namespace GTERP.Interfaces.Commercial
{
    public interface IBuyerGroupsRepository : IBaseRepository<BuyerGroup>
    {
        string ReportGenerate();
    }
}
