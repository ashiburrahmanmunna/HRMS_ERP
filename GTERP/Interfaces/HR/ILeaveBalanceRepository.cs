using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HR
{
    public interface ILeaveBalanceRepository : IBaseRepository<HR_Leave_Balance>
    {
        IEnumerable<SelectListItem> GetLeaveBalanceList();
        IEnumerable<SelectListItem> GetOpeningYear();
        IEnumerable<SelectListItem> EmpList();
        Task<List<LeaveBalance>> GetLeaveBalance(string Criteria, int EmpId, int SectId,int DeptId,int LineId,int FloorId, string DtOpBal);
        Task<int> SaveLeaveBalance(List<HR_Leave_Balance> LeaveBalance);
        List<HR_TempLeaveBalanceExcel> GetLeaveBalanceExcel(string fName);
        void FileUploadDirectory(IFormFile file);
        FileContentResult DownloadSampleFile(string file);
        void SaveUploadedData();
    }
}
