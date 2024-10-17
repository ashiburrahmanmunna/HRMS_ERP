using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models.Recruitment
{
    public class Exam_Module
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
        [Required]
        [StringLength(60)]
        public string ComId { get; set; }
        [Required]
        [StringLength(60)]
        public string UserId { get; set; }
        public string module { get; set; }
        public int timer { get; set; }

    }
}
