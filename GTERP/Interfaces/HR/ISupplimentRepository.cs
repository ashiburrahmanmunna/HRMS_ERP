using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ISupplimentRepository : IBaseRepository<HR_Emp_Suppliment>
    {
        List<SupplimentViewModel> HREmpSuppliment1(DateTime? dtInput, int? sectId);
        List<SupplimentViewModel> HREmpSuppliment2(DateTime? dtInput, int? sectId);
        List<SupplimentViewModel> HREmpSuppliment3(DateTime? dtInput, int? sectId);
        List<SupplimentViewModel> HREmpSuppliment4(DateTime? dtInput, int? sectId);
        void CreateSuppliment(List<HR_Emp_Suppliment> suppliments);
    }
}
