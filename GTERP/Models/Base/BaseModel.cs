using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models.Base
{
    public class BaseModel
    {
        [Display(Name = "Company Id")]
        public string ComId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string UpdateByUserId { get; set; }

        public bool IsDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateAdded { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateUpdated { get; set; }

    }
}
