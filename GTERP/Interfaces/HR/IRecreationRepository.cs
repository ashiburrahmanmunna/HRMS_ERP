using GTERP.Models;
using GTERP.ViewModels;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IRecreationRepository
    {
        List<RecreationViewModel> prcRecationList();
        void CreateRecreation(List<HR_Emp_Recreation> recreations);
    }
}
