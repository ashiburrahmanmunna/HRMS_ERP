using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;


namespace GTERP.Models.Recruitment
{
    public class HR_Recruitment_Post
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        [Required]
        [StringLength(60)]
        [Display(Name = "Company Id: ")]
        public string ComId { get; set; }
        [Required]
        [StringLength(60)]
        public string UserId { get; set; }
        public double Salary { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public DateTime LastDate { get; set; }
        public string EmployeeStatus { get; set; }
        public int Vacancy { get; set; }
        public string PostTitle { get; set; }
        public int TempId { get; set; }
        public string JobContext { get; set; }
        public string JobResponsibility { get; set; }
        public string E_Requirement { get; set; } //educational requirement
        public string A_Requirement { get; set; } //additional requirewment
        public string OtherBenifits { get; set; }

        [ForeignKey(nameof(Department))]
        public int DeptId { get; set; }
        public HR_Recruitment_Department Department { get; set; }
    }
}
