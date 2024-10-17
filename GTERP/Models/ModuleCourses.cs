using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Models
{
    public class ModuleCourses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleCourseId { get; set; }
        public string VideoLink { get; set; }
        public string FilePath { get; set; }
        public int ModuleMenuChildId { get; set; }
        public string ModuleMenuParentName { get; set; }
        public string ModuleMenuChildName { get; set; }
        public string Description { get; set; }
        [ForeignKey("Modules")]
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public virtual Module Modules { get; set; }

        [ForeignKey("ModuleMenus")]
        public int ModuleMenuId { get; set; }
        public virtual ModuleMenu ModuleMenus { get; set; }

        public ICollection<UserLoggingTrack> UserLoggingTracks { get; set; }
        public ICollection<FileCollection> fileCollections { get; set; }
        [NotMapped]
        public List<IFormFile> File { get; set; }
    }

    public class FileCollection
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("moduleCourses")]
        public int ModuleCourseId { get; set; }
        public virtual ModuleCourses moduleCourses { get; set; }
        public string URL { get; set; }
    }

    public class ModuleCoursesViewModel
    {

        public string VideoLink { get; set; }
        public List<IFormFile> File { get; set; }
        public int ModuleMenuChildId { get; set; }
        public string Description { get; set; }
        [ForeignKey("Modules")]
        public int ModuleId { get; set; }
        public virtual Module Modules { get; set; }
        public string ModuleName { get; set; }
        public string ModuleMenuParentName { get; set; }
        public string ModuleMenuChildName { get; set; }
        [ForeignKey("ModuleMenus")]
        public int ModuleMenuId { get; set; }
        public virtual ModuleMenu ModuleMenus { get; set; }
        public int ModuleCourseId { get; set; }
        public List<string>? FilePath { get; set; }
    }
    public class UserLoggingTrack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoggingTrackId { get; set; }

        public string UserId { get; set; }
        [ForeignKey("ModuleCourses")]
        public int ModuleCourseId { get; set; }
        public ModuleCourses ModuleCourses { get; set; }
        public bool IsComplete { get; set; }

    }
    public class CertificateInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CertificateId { get; set; }
        //[ForeignKey("Modules")]//-----Should be ModuleCourses > Modules
        public int ModuleId { get; set; }
        // public Module Modules { get; set; }//-----ModuleCourses > Modules
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string QrCodeUrl { get; set; }
    }

    public class Quiz {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string quiz { get; set; }
        [ForeignKey("Module")]//-----Should be ModuleCourses > Modules
        public int ModuleId { get; set; }

        public int passMark { get; set; }
        public string preparedBy { get; set; }
        public string authDesig { get; set; }
        public string? authSign { get; set; }
        public string remark { get; set; }
        
        public virtual Module Module { get; set; }
        public ICollection<Answer> Answer { get; set; }
        [NotMapped]
        public IFormFile sign { get; set; }

    }
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Quiz")]
        public int quizid { get; set; }
        public string ans { get; set; }
        public string optionType { get; set; }

        public bool isRight { get; set; }
        public virtual Quiz Quiz { get; set; }

        // public Module Modules { get; set; }//-----ModuleCourses > Modules

    }
    public class quizAnsVm

    {   public int id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int quizId { get; set; }
        public string question { get; set; }
        public string authSign { get; set; }
        public string authDesig { get; set; }
        public int passMark { get; set; }
        public List<string> answer { get; set; }
        public List<Answer> AnsTable { get; set; }
        public string optionType { get; set; }
        public string preparedBy { get; set; }
        public int totalques{ get; set; }
        public bool isRight { get; set; }
    }
    public class GenerateQRCodeModel
    {
        [Display(Name = "Enter QR Code Text")]
        public string QRCodeText
        {
            get;
            set;
        }
    }



}
