using GTERP.ViewModels;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IProductionRepository
    {
        List<ProdGrid> GetProduction(string DtFrom, string DtTo, string criteria, string value);
        void CreateProduction(string GridDataList);
        void UpdateProduction(string GridDataList);
    }
}
