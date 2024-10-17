using GTERP.Models;
using GTERP.ViewModels;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{


    public interface ITotalFCRepository
    {
        List<Pross> TotalFCList();
        void CreateTotalFC(List<HR_OT_FC> hR_OT_FCs);
        List<OTFC> SearchTotalFC(string prossType);

    }
}
