using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models.Recruitment
{
    public class Exam_Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey("Quiz")]
        public int quizid { get; set; }

        public string ans { get; set; }
        public string optionType { get; set; }

        public bool isRight { get; set; }
        public virtual Exam_Quiz Quiz { get; set; }

        // public Module Modules { get; set; }//-----ModuleCourses > Modules

    }
}
