using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace GTERP.Models.Recruitment
{
    public class HR_Recruitment_Applicant
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Applicant_Id { get; set; }
        [StringLength(60)]
        public string ComId { get; set; }
        [Required]
        [StringLength(60)]
        public string UserId { get; set; }
        public double ExamResult { get; set; }
        public double VivaResult { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }

        public int Moduleid { get; set; }

        public int PostId { get; set; }
        public int S_Id { get; set; }
        public DateTime A_Date { get; set; }
    }
}
