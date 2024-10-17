using GTERP.ViewModels;
using System.Collections.Generic;
using System;
using GTERP.Models.Recruitment;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using GTERP.Models;
using Nancy.Json;
using Microsoft.AspNetCore.Mvc;

namespace GTERP.Interfaces.HRrecruitment
{
    public interface IPostRepository
    {

        List<HR_Recruitment_Post> GetAll();
        void CreatePost(HR_Recruitment_Post model);
        
        List<HR_Recruitment_Post> GetPostbyId(int id);
       void DeleteConfirmed(int id);
       bool PostExists(int id);
       void UpdatePost(HR_Recruitment_Post post);

        List<HR_Recruitment_Templete> GetTemplete(int id);
        List<HR_Recruitment_Department> GetDepartment();
        List<HR_Recruitment_Department> DepartmentDetails(int id);
        void DepartmentCreate(DeptVm model);
        bool DepartmentExists(int id);
        void DepartmentDelete(int id);
        void DepartmentEdit( HR_Recruitment_Department model);

    }
}
