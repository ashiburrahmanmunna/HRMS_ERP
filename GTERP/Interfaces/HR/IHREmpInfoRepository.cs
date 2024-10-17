using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace GTERP.Interfaces.HR
{
    public interface IHREmpInfoRepository
    {
        IEnumerable<SelectListItem> PayModeList();
        IEnumerable<SelectListItem> AccTypeList();
        IEnumerable<SelectListItem> SubSectList();
        IEnumerable<SelectListItem> PFFinalYList();
        IEnumerable<SelectListItem> PFFundTransferYList();
        IEnumerable<SelectListItem> WFFinalYList();
        IEnumerable<SelectListItem> WFFundTransferYList();
        IEnumerable<SelectListItem> GratuityFinalYList();
        IEnumerable<SelectListItem> GratuityFundTransferYList();
        IEnumerable<SelectListItem> EmpCurrPSList(HR_Emp_Info hrEmpInfo);
        IEnumerable<SelectListItem> EmpPerPSList(HR_Emp_Info hrEmpInfo);
        IEnumerable<SelectListItem> EmpCurrPOList(HR_Emp_Info hrEmpInfo);
        IEnumerable<SelectListItem> EmpPerPOList(HR_Emp_Info hrEmpInfo);

        IEnumerable<SelectListItem> EmpAccTypeList(HR_Emp_Info hrEmpInfo);
        IEnumerable<SelectListItem> EmpBankList(HR_Emp_Info hrEmpInfo);
        IEnumerable<SelectListItem> EmpBranchList(HR_Emp_Info hrEmpInfo);
        IEnumerable<SelectListItem> EmpPayModeList(HR_Emp_Info hrEmpInfo);

        IEnumerable<SelectListItem> EmpBuildingTypeList(HR_Emp_Info hrEmpInfo);

        List<HR_Emp_Experience> EmpExperienceDelete(string HR_Emp_Experiences);
        List<HR_Emp_Education> EmpEducationDelete(string HR_Emp_Experiences); 
        List<HR_Emp_Projects> EmpProjectDelete(string HR_Emp_Projects);
        List<HR_Emp_Devices> EmpDeviceDelete(string HR_Emp_Devices);
        IEnumerable<SelectListItem> SubCategoryList();
        IEnumerable<SelectListItem> CategoryList();
    }
}
