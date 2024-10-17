using GTERP.Interfaces.Base;
using GTERP.Models;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IHolidaySetupRepository : IBaseRepository<HR_ProssType_WHDay>
    {
        List<HR_ProssType_WHDay> ProssTypeWHDayPartial(DateTime date);
        void CreateHoliDaySetUp(HR_ProssType_WHDay WHProssType);
        void UpdateHoliDaySetUp(HR_ProssType_WHDay WHProssType);
    }
}
