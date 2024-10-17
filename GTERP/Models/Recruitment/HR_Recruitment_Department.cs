using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models.Recruitment
{
    public class HR_Recruitment_Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeptId { get; set; }
        [StringLength(60)]
        public string ComId { get; set; }
        [Required]
        [StringLength(60)]
        public string UserId { get; set; }
        [Display(Name = "Department Name")]
        public string Dept_Name { get; set; }

        public ICollection<HR_Recruitment_Post> JobPost { get; set; }
    }
}
