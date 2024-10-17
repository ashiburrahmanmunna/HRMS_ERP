using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Customer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }


        [Required]
        [Display(Name = "Company Id")]
        [StringLength(128)]
        //[Index("IX_ComIdCustomerCode", 1, IsUnique = true)]
        public string comid { get; set; }

        [NotMapped]
        public string OldCustomerCode { get; set; }


        [Required]
        //[Index("IX_FComIdCustomerCode", 2, IsUnique = true)]
        //[Column(TypeName = "VARCHAR")]
        //[StringLength(100)]
        //[Index(IsUnique = true)]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        public string CustomerCode { get; set; }


        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }

        //[Required(ErrorMessage = "Field can't be empty")]

        //[Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address")]
        public string EmailId { get; set; }




        //[Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }


        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string SecoundaryAddress { get; set; }


        [StringLength(5, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        //[Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        //[Required]
        //[StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        //[DataType(DataType.Text)]
        //[Display(Name = "District Bangla")]
        //public string District { get; set; }

        ////[Required]
        //[StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        //[DataType(DataType.Text)]
        //[Display(Name = "Police Station Bangla")]
        //public string PoliceStation { get; set; }

        //[Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]

        public string PhoneNo { get; set; }




        [StringLength(128)]
        public string userid { get; set; }

        [StringLength(128)]
        [Display(Name = "Update By")]

        public string useridUpdate { get; set; }

        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }


        [Display(Name = "Is InActive")]
        public bool IsInActive { get; set; }

        [Display(Name = "Is Dealer")]
        public bool IsDealer { get; set; }

        [Display(Name = "Opening Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal OpBalance { get; set; }

        [Display(Name = "Country")]
        public virtual ICollection<SalesMain> vCountry { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        [Display(Name = "Country")]
        public virtual Country vCustomerCountry { get; set; }



        //[Required]
        [Display(Name = "Police Station")]
        public int? PStationId { get; set; }
        [Display(Name = "Police Station")]
        [ForeignKey("PStationId")]

        public virtual Cat_PoliceStation vCat_PoliceStation { get; set; }



        //[Required]
        [Display(Name = "District")]
        public int? DistId { get; set; }
        [Display(Name = "District")]
        [ForeignKey("DistId")]

        public virtual Cat_District vCat_District { get; set; }


        public int? SLNo { get; set; }

    }

    public class CustomerContact
    {

        [Key, Column(Order = 0)]
        public int CustomerContactId { get; set; }

        //[Key, Column(Order = 1)]
        //public string ItemName { get; set; }
        //public int CategoryId { get; set; }


        public int CustomerId { get; set; }
        //public int ProductId { get; set; }


        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string ContactEmailNo { get; set; }



        public virtual Customer vCustomer { get; set; }

        //public virtual Product vProductName { get; set; }


        //public virtual ICollection<Product> vProducts { get; set; }




    }


    public class CustomerPayment
    {

        public int CustomerPaymentId { get; set; }

        [Required(ErrorMessage = "ইনপূট নিশ্চিত করুন ।")]
        //[Display(Name = "Serial No")]
        [Display(Name = "সিরিয়াল নাম্বার :")]
        //[StringLength(20, ErrorMessage = "The {0} অবশ্যই কমপক্ষে {10} অক্ষর দীর্ঘ হতে হবে |", MinimumLength = 10)]
        //[RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "ইংরেজি তে লিখুন । বিশেষ অক্ষর অনুমোদিত নয় |")]

        public string CustomerPaymentNo { get; set; }
        public string TrxID { get; set; }
        public string PaymentId { get; set; }
        public string transactionStatus { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string merchantInvoiceNumber { get; set; }
        public int SoftwarePackageId { get; set; }
        public int UsageDuration { get; set; }




        //[Display(Name = "Entry Date")]
        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; }


        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime ActiveFromDate { get; set; }


        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime ActiveToDate { get; set; }


        [Required]
        [StringLength(128)]
        public string userid { get; set; }

        [Required]
        [Phone]
        public string UserPhoneNo { get; set; }


        //public int ?comid { get; set; }


        [Column(TypeName = "nvarchar(3000)")]
        [StringLength(300)]
        [Display(Name = "স্ট্যাটাস")]
        public string Status { get; set; }

        public bool ActiveYesNo { get; set; }


    }

    public class CustomerSerial
    {

        public int CustomerSerialId { get; set; }

        [MaxLength(30)]
        [Required(ErrorMessage = "ইনপূট নিশ্চিত করুন ।")]
        //[Display(Name = "Serial No")]
        [Display(Name = "সিরিয়াল নাম্বার :")]
        [StringLength(30, ErrorMessage = "সিরিয়াল নাম্বার অবশ্যই কমপক্ষে ১০ অক্ষর এবং সর্বোচ্চ ৩০ অক্ষর দীর্ঘ হতে হবে |", MinimumLength = 10)]
        [RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "ইংরেজি তে লিখুন । বিশেষ অক্ষর অনুমোদিত নয় |")]

        public string CustomerSerialNo { get; set; }


        //[Display(Name = "Entry Date")]
        [Display(Name = "এন্ট্রি তারিখ :")]

        [DataType(DataType.DateTime)]
        public DateTime EntryDate { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        //[Display(Name = "Customer Name")]
        [Display(Name = "গ্রাহকের নাম :")]
        [StringLength(20, ErrorMessage = "গ্রাহকের নাম অবশ্যই কমপক্ষে ১০ অক্ষর এবং সর্বোচ্চ ২০ অক্ষর দীর্ঘ হতে হবে |", MinimumLength = 10)]

        public string CustomerSerialName { get; set; }

        [Required(ErrorMessage = "ইনপূট নিশ্চিত করুন।")]
        //[Display(Name = "Customer Mobile")]
        [Display(Name = "গ্রাহকের মোবাইল নাম্বার :")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ইংরেজি তে মোবাইল নাম্বার লিখুন । বিশেষ অক্ষর অনুমোদিত নয়।")]
        [StringLength(11, ErrorMessage = "১১ সংখ্যার গ্রাহকের মোবাইল নাম্বার দিন। উদাহরনঃ ০১৬৭১৩০৩৩০২", MinimumLength = 11)]
        [phoneValidation(ErrorMessage = "মোবাইল নাম্বার সঠিক নয় ।")]

        //[Remote("CheckUserName", "Student")]
        [DataType(DataType.PhoneNumber)]
        public string CustomerSerialMobile { get; set; }


        //[CustomEmailValidator(ErrorMessage = "Custom error")]
        [MaxLength(30)]
        [Required(ErrorMessage = "ইনপূট নিশ্চিত করুন ।")]
        [StringLength(30, ErrorMessage = "গ্রাহকের ঠিকানা অবশ্যই কমপক্ষে ১০ অক্ষর এবং সর্বোচ্চ ৩০ অক্ষর দীর্ঘ হতে হবে |", MinimumLength = 10)]
        [DataType(DataType.MultilineText)]

        //[Display(Name = "Customer Address")]
        [Display(Name = "গ্রাহকের ঠিকানা :")]

        public string CustomerSerialAddress { get; set; }


        [Required]
        [StringLength(128)]
        public string userid { get; set; }

        [Required]
        [Phone]
        public string UserPhoneNo { get; set; }


        //public int ?comid { get; set; }


        [Column(TypeName = "nvarchar(3000)")]
        [StringLength(300)]
        [Display(Name = "স্ট্যাটাস")]
        public string Status { get; set; }
        //[Required]
        //[StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.MultilineText)]
        //[Display(Name = "Remarks")]
        //public string CustomerSerialRemarks { get; set; }


        //public class phoneValidation : ValidationAttribute
        //{
        //    public override bool IsValid(object value)
        //    {
        //        var x = GTERP.Models.Common.clsMain.CheckIsPhoneNumber(value.ToString());

        //        if (x == "NAN")
        //        {
        //            return false;


        //        }
        //        else
        //        {
        //            return true;

        //        }
        //    }
        //}
        public class phoneValidation : ValidationAttribute
        {
            //private readonly int _maxWords;
            //public phoneValidation(int maxWords)
            //    : base("{0} Phone no is Not Valid.")
            //{
            //    _maxWords = maxWords;
            //}
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var x = GTERP.Models.Common.clsMain.CheckIsPhoneNumber(value.ToString());
                if (x != "NAN")
                {

                    return ValidationResult.Success;
                }
                else
                {
                    var errorMessage = FormatErrorMessage((validationContext.DisplayName));
                    return new ValidationResult(errorMessage);
                }

                //if (value == null) return ValidationResult.Success;
                //var textValue = value.ToString();
                //if (textValue.Split(' ').Length <= _maxWords) return ValidationResult.Success;
                //var errorMessage = FormatErrorMessage((validationContext.DisplayName));
                //return new ValidationResult(errorMessage);
            }
        }

        //public JsonResult CheckUserName(string userName)
        //{


        //    var studentUserList = new List<string> { "Manish", "Saurabh", "Akansha", "Ekta", "Rakesh", "Bhayia Jee" };

        //    var result = studentUserList.Any(userName.Contains);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //public class MaxWordAttributes : ValidationAttribute
        //{
        //    private readonly int _maxWords;
        //    public MaxWordAttributes(int maxWords)
        //        : base("{0} has to many words.")
        //    {
        //        _maxWords = maxWords;
        //    }
        //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //    {
        //        if (value == null) return ValidationResult.Success;
        //        var textValue = value.ToString();
        //        if (textValue.Split(' ').Length <= _maxWords) return ValidationResult.Success;
        //        var errorMessage = FormatErrorMessage((validationContext.DisplayName));
        //        return new ValidationResult(errorMessage);
        //    }
        //}

    }
}