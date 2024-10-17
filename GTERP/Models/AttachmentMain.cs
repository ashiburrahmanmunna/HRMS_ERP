using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class AttachmentMain
    {
        [Key]
        public int AttachmentMainID { get; set; }
        public string BasedOn { get; set; }
        public int BasedId { get; set; }
        public Guid Wid { get; set; }
        public DateTime AttachmentDate { get; set; }
        public string AttachmentDesc { get; set; }


        public virtual ICollection<AttachmentSub> vAttachmentSub { get; set; }

    }

    public class AttachmentSub
    {
        [Key]
        public int AttachmentSubID { get; set; }

        public int AttachmentMainId { get; set; }


        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }

        public virtual AttachmentMain vAttachmentMain { get; set; }

    }

}