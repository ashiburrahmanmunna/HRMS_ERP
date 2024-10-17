using GTERP.ViewModels;
using System.Collections.Generic;


namespace GTERP.Interfaces.HR
{
    public interface IFixAttRepository
    {
        List<AttFixGrid> FixAttendanceList(string DtFrom, string DtTo, string criteria, string value);
        List<AttFixGrid> FixAttendanceListB(string DtFrom, string DtTo, string criteria, string value);
        List<AttFixGridUBL> FixAttendanceListUBL(string DtFrom, string DtTo, string criteria, string value);
        void UpdateSelectedData(string GridDataList);
        void UpdateSelectedDataB(string GridDataList);
    }
}
