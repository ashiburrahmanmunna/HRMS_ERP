using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models.Recruitment
{
    public class HR_Recruitment_Templete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TempId { get; set; }
        public string ComId { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Title { get; set; }
        public string JobContext { get; set; }
        public string JobResponsibility { get; set; }
        public string E_Requirement { get; set; } //educational requirement
        public string A_Requirement { get; set; } //additional requirewment
        public string OtherBenifits { get; set; }
    }
}
