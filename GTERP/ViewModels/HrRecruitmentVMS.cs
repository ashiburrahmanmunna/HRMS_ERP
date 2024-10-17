using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using GTERP.Models;
using GTERP.Models.Recruitment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    
    public class PostVM
    {
        public int PostId { get; set; }
        public string ComId { get; set; }
        public string UserId { get; set; }
        public double Salary { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public DateTime LastDate { get; set; }
        public string EmployeeStatus { get; set; }
        public int Vacancy { get; set; }
        public string PostTitle { get; set; }
        public string JobContext { get; set; }
        public string JobResponsibility { get; set; }
        public string E_Requirement { get; set; } //educational requirement
        public string A_Requirement { get; set; } //additional requirewment
        public string OtherBenifits { get; set; }
        public int DeptId { get; set; }
        public string Dept_Name { get; set; }
        public HR_Recruitment_Department Department { get; set; }
        public int TempId { get; set; }
        public string TempTitle { get; set; }
    }

    public class DeptVm {

        public int DeptId { get; set; }
        
        public string ComId { get; set; }
       
        public string UserId { get; set; }
        
        public string Dept_Name { get; set; }

    }
    public class SubmittedCVVM
    {

        public int S_Id { get; set; }
        public int PostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string linkedin_Url { get; set; }
        public double ExpectedSalary { get; set; }
        public string CoverLetter { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public IFormFile Attachment { get; set; } //change
        public bool IsDelete { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string PostTitle { get; set; }
    }
    public class cvVM
    {
        public int S_Id { get; set; }
        public string PostTitle { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string linkedin_Url { get; set; }
        public double ExpectedSalary { get; set; }
        public string CoverLetter { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] Attachment { get; set; }//change
        public DateTime SubmittedDate { get; set; }
    }
    public class ListVM
    {
        public int App_Id { get; set; }
        public string App_Name { get; set; }
        public string PostTitle { get; set; }
        public double ExamResult { get; set; }
        public double VivaResult { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int PostId { get; set; }
        public HR_Recruitment_Post post { get; set; }
        public string Email { get; set; }
        public int S_Id { get; set; }
        public List<Exam_Module> modulelist { get; set; }
        public HR_Recruitment_SubmittedCV submittedCV { get; set; }
        public string Number { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string linkedin_Url { get; set; }
    }
    public class ApplicantVM
    {
        public int App_Id { get; set; }
        public double ExamResult { get; set; }
        public double VivaResult { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public int PostId { get; set; }

        public HR_Recruitment_Post post { get; set; }
        public int S_Id { get; set; }
        public HR_Recruitment_SubmittedCV submittedCV { get; set; }
    }
    public class modlisti

    {

        public int s_id { get; set; }
        public int Appid { get; set; }
        public string module { get; set; }
    }
    public class QuizSave
    {



        public int Id { get; set; }
        public string quiz { get; set; }

        public int ModuleId { get; set; }

        public int passMark { get; set; }
        public int timer { get; set; }
        public int hoursdd { get; set; }
        public int minutedd { get; set; }
        public int secondsdd { get; set; }
        public string preparedBy { get; set; }
        public string remark { get; set; }
        public ICollection<Exam_Answer> Answer { get; set; }



    }
    public class quizAnsVms

    {

        public int id { get; set; }
        public int ModuleId { get; set; }
        public double timer { get; set; }
        public string time { get; set; }
        public string ModuleName { get; set; }
        public int quizId { get; set; }
        public string question { get; set; }
        public int passMark { get; set; }
        public List<string> answer { get; set; }
        public List<Exam_Answer> AnsTable { get; set; }
        public string optionType { get; set; }
        public string preparedBy { get; set; }
        public int totalques { get; set; }
        public bool isRight { get; set; }
        public int S_Id { get; set; }
        public int applicantid { get; set; }
    }
    public class ListVMExam
    {
        public int App_Id { get; set; }
        public string App_Name { get; set; }
        public string PostTitle { get; set; }


    }
    public class DeleteVM
    {

        public bool IsDelete { get; set; }


    }
}
