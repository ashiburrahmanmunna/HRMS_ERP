using GTERP.Models;
using GTERP.ViewModels;
using System.Collections.Generic;



namespace GTERP.Interfaces.HR
{

    public interface ITotalNightRepository
    {
        List<Pross> TotalNightList();
        void CreateTotalNight(List<HR_OT_FC> hR_OT_FCs);
        List<OTFC> SearchTotalNight(string prossType);

    }
}
