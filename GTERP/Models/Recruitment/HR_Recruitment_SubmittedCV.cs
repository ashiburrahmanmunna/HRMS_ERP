using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace GTERP.Models.Recruitment
{
    public class HR_Recruitment_SubmittedCV
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int S_Id { get; set; }
        public int PostId { get; set; }
        [Required]
        public string SubmittedCV_Name { get; set; }
        [EmailAddress]
        public string SubmittedCV_Email { get; set; }
        [Required]
        [MaxLength(12)]
        public string SubmittedCV_Number { get; set; }
        [Url]
        public string SubmittedCV_linkedin_Url { get; set; }
        public double SubmittedCV_ExpectedSalary { get; set; }
        public string SubmittedCV_CoverLetter { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] Attachment { get; set; }//change

        public bool IsDelete { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
