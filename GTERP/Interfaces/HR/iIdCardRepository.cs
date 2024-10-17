using GTERP.ViewModels;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface IIdCardRepository
    {
        List<IdcardGreadData> GetIdcard();
        void SaveIdcard(IdCard idCard);
        string PrintIdcardReport(DateTime fromDate, DateTime toDate, IdCard IdCard);
        List<IdcardGreadData> loadDataByDate(string FromDate, string ToDate);

    }
}
