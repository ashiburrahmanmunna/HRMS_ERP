using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{



    public class UserLogingInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserLogingInfoId { get; set; }


        [Required]
        [StringLengthAttribute(128)]
        public string userid { get; set; }


        [StringLength(300)]
        [Display(Name = "Web Link")]
        public string WebLink { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
        public string LongString { get; set; }
        public string UserName { get; set; }






        //[Required]
        [Display(Name = "Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LoginDate { get; set; }

        public DateTime? LoginTime { get; set; }

        [StringLength(50)]

        public string PcName { get; set; }
        [StringLength(30)]

        public string MacAddress { get; set; }
        [StringLength(128)]


        public string IPAddress { get; set; }
        [StringLength(50)]


        public string DeviceType { get; set; }
        [StringLength(128)]


        public string Platform { get; set; }
        [StringLength(128)]

        public string WebBrowserName { get; set; }
        [StringLength(128)]

        public string comid { get; set; }

        [StringLength(128)]

        public string Latitude { get; set; }
        [StringLength(128)]

        public string Longitude { get; set; }


    }



    public class UserLogingInfoVM
    {
 
        public int UserLogingInfoId { get; set; }


        [Required]
        [StringLengthAttribute(128)]
        public string userid { get; set; }


        [StringLength(300)]
        [Display(Name = "Web Link")]
        public string WebLink { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
        public string LongString { get; set; }
        public string UserName { get; set; }






        //[Required]
        [Display(Name = "Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string LoginDate { get; set; }

        public string LoginTime { get; set; }

        [StringLength(50)]

        public string PcName { get; set; }
        [StringLength(30)]

        public string MacAddress { get; set; }
        [StringLength(128)]


        public string IPAddress { get; set; }
        [StringLength(50)]


        public string DeviceType { get; set; }
        [StringLength(128)]


        public string Platform { get; set; }
        [StringLength(128)]

        public string WebBrowserName { get; set; }
        [StringLength(128)]

        public string comid { get; set; }

        [StringLength(128)]

        public string Latitude { get; set; }
        [StringLength(128)]

        public string Longitude { get; set; }


    }

    public class UserTransactionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTransactionLogId { get; set; }


        [Required]
        [StringLength(128)]
        public string userid { get; set; }


        [StringLength(300)]
        [Display(Name = "Web Link")]
        public string WebLink { get; set; }


        [StringLength(300)]
        [Display(Name = "Transaction Statement")]
        public string TransactionStatement { get; set; }
        [StringLength(60)]

        [Display(Name = "ControllerName")]
        public string ControllerName { get; set; }
        [StringLength(100)]

        [Display(Name = "Action")]
        public string ActionName { get; set; }
        [StringLength(300)]

        [Display(Name = "DocumentReferance")]
        public string DocumentReferance { get; set; }
        [StringLength(300)]

        public string CommandType { get; set; }

        [StringLength(200)]
        public string PcName { get; set; }

        //[Required]
        [Display(Name = "Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }
        [StringLength(100)]

        public string FlagValue { get; set; }

        [StringLength(100)]
        public string IPAddress { get; set; }

        [StringLength(128)]
        public string comid { get; set; }
    }

}