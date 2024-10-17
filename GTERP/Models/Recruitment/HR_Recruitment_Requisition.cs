using DocumentFormat.OpenXml.Bibliography;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models.Recruitment
{
    public class HR_Recruitment_Requisition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequId { get; set; }
        [Required]
        [StringLength(60)]
        public string ComId { get; set; }
        [Required]
        [StringLength(60)]
        public string UserId { get; set; }
        public string Requ_Title { get; set; }

        public string Designation { get; set; }

        public int Vacancy { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }

        [ForeignKey(nameof(Department))]
        public int DeptId { get; set; }
        public HR_Recruitment_Department Department { get; set; }

        public bool IsDelete { get; set; }

        public string Comment { get; set; }
    }
}
