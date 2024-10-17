
using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HR
{
    public interface IEmpInfoRepository : IBaseRepository<HR_Emp_Info>
    {
        IEnumerable<SelectListItem> GetEmpInfoAllList();
        List<HR_Emp_Info> GetEmp();
        IQueryable<EmployeeInfo> EmpInfo();
        void EmpInfoPost(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile);
        void VendorInfoPost(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile);
        void EmpInfoPostElse(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile);
        void VendorInfoPostElse(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile);
        Task<HR_Emp_Info> EmpInfoEdit(int? id);
        Task<HR_Emp_Info> VendorInfoEdit(int? id);

        List<EmployeeInfoVM> GetEmpInfoAll();
        List<VendorInfoVM> GetVendorInfoAll();
        FileContentResult DownloadEducationFile(string file);
        IEnumerable<SelectListItem> VendorType();
        IEnumerable<SelectListItem> JobNatureType();
        IEnumerable<SelectListItem> AltitudeType();
        IEnumerable<SelectListItem> VendorCategory();
        IEnumerable<SelectListItem> VendorRelay();
        List<Daily_req_entry> GetRequisitionInfo(string searchDate);
        void SaveRequisitionInfo(List<Daily_req_entry> requisition, string searchDate);
        List<VendorInfoVM> GetStudentInfoAll();
        Task<HR_Emp_Info> StudentInfoEdit(int? id);
        void StudentInfoPost(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile);
        void StudentInfoPostElse(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile);





    }
}
