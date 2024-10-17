using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models.Recruitment
{
    public class Exam_Quiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Quiz_Id { get; set; }
        [Required]
        [StringLength(60)]
        public string ComId { get; set; }
        [Required]
        [StringLength(60)]
        public string UserId { get; set; }
        public string quiz { get; set; }
        [ForeignKey(nameof(Module))]
        public int ModuleId { get; set; }

        public int passMark { get; set; }
        public int timer { get; set; }
        public string preparedBy { get; set; }
        public string remark { get; set; }

        public virtual Exam_Module Module { get; set; }
        public ICollection<Exam_Answer> Answer { get; set; }


    }
}
