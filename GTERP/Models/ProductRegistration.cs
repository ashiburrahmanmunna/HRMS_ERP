using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GTERP.Models.CustomerSerial;

namespace GTERP.Models
{
    public class ProductRegistration
    {

        public int ProductRegistrationId { get; set; }
        [MaxLength(50)]
        [DataType(DataType.Text)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন ।")]
        //[Display(Name = "Serial No")]
        [Display(Name = "সিরিয়াল নাম্বার :")]
        //[StringLength(50, ErrorMessage = "সিরিয়াল নাম্বার অবশ্যই কমপক্ষে ১০ অক্ষর এবং সর্বোচ্চ ৫০ অক্ষর দীর্ঘ হতে হবে |", MinimumLength = 10)]
        //[RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "ইংরেজি তে লিখুন । বিশেষ অক্ষর অনুমোদিত নয় |")]
        public string ProductRegistrationSerialNo { get; set; }


        [MaxLength(20)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        //[Display(Name = "Customer Mobile")]
        [Display(Name = "গ্রাহকের মোবাইল নাম্বার :")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "ইংরেজি তে মোবাইল নাম্বার লিখুন । বিশেষ অক্ষর অনুমোদিত নয়।")]
        //[StringLength(11, ErrorMessage = "১১ সংখ্যার গ্রাহকের মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        //[phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        public string CustomerPhoneNumber { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        //[Display(Name = "Customer Mobile")]
        [Display(Name = "ডিলার এর মোবাইল নাম্বার :")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ইংরেজি তে মোবাইল নাম্বার লিখুন । বিশেষ অক্ষর অনুমোদিত নয়।")]
        [StringLength(11, ErrorMessage = "১১ সংখ্যার ডিলার এর মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]
        public string DealerPhoneNumber { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Item_Code { get; set; }

        [MaxLength(200)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Item_Name { get; set; }

        [MaxLength(200)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string ProductType { get; set; }

        [MaxLength(20)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Party_Number { get; set; }

        [MaxLength(200)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Account_Name { get; set; }

        [MaxLength(200)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Organization_Name { get; set; }

        [MaxLength(200)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Model { get; set; }


        //[Display(Name = "Entry Date")]
        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime ProductRegistrationDate { get; set; }

        [MaxLength(300)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string OfferSmsResult { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Brand { get; set; }

        [MaxLength(100)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string ProductSize { get; set; }

        [MaxLength(20)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Source { get; set; }


        [MaxLength(200)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Zone_Name { get; set; }

        [MaxLength(200)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string Area_Name { get; set; }

        [MaxLength(300)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string SalesChannel { get; set; }
        [MaxLength(300)]
        //[Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        public string AddedBy { get; set; }

        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime Addeddate { get; set; }

        [Required]
        [StringLength(128)]
        public string userid { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string UserPhoneNo { get; set; }


        //public int ?comid { get; set; }


        [Column(TypeName = "nvarchar(3000)")]
        [StringLength(300)]
        [Display(Name = "স্ট্যাটাস")]
        public string Status { get; set; }

    }


}