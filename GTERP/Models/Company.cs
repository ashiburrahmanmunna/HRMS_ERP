using GTERP.Models.Self;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Company : SelfModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Company ID")]

        public int ComId { get; set; }


        [Required]
        [Display(Name = "Secret Code")]
        [StringLength(128)]
        public string CompanySecretCode { get; set; }

        [Display(Name = "App Key")]
        [StringLength(128)]
        public string AppKey { get; set; }
        public ICollection<AppKeys> AppKeys { get; set; }


        [Required]
        [Display(Name = "Code")]
        [StringLength(128)]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }


        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Com Name Bangla")]
        public string CompanyNameBangla { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Short Name")]
        public string CompanyShortName { get; set; }


        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Address")]
        public string PrimaryAddress { get; set; }


        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Com Address Bangla")]
        public string CompanyAddressBangla { get; set; }


        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Secoundary Address")]
        public string SecoundaryAddress { get; set; }


        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]

        public string comPhone { get; set; }


        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No 2")]

        public string comPhone2 { get; set; }


        //[Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Fax")]

        public string comFax { get; set; }



        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address")]
        public string comEmail { get; set; }



        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Web Site")]
        public string comWeb { get; set; }


        //[StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        //[DataType(DataType.Text)]
        [Display(Name = "Business Type")]
        public int BusinessTypeId { get; set; }



        [Display(Name = "Company")]
        public int? BaseComId { get; set; }
        /// <summary>
        /// fahad
        /// </summary>


        [Display(Name = "Country")]
        public int CountryId { get; set; }


        [Display(Name = "Decimal Field")]
        public int DecimalField { get; set; }

        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Person")]
        public string ContPerson { get; set; }

        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Designation")]
        public string ContDesig { get; set; }


        [Display(Name = "Show Currency Symbol")]
        public bool IsShowCurrencySymbol { get; set; }

        [Display(Name = "Is InActive")]
        public bool IsInActive { get; set; }

        [Display(Name = "Is Group")]
        public bool IsGroup { get; set; }

        [Display(Name = "Is Dollar")]
        public bool IsDoller { get; set; }


        [Display(Name = "Barcode Search")]
        public bool isBarcode { get; set; }

        [Display(Name = "Product Search")]
        public bool isProduct { get; set; }

        [Display(Name = "Corporate")]
        public bool isCorporate { get; set; }

        [Display(Name = "POSprint")]
        public bool isPOSprint { get; set; }

        [Display(Name = "Service")]
        public bool IsService { get; set; }

        [Display(Name = "Old Due Show")]
        public bool isOldDue { get; set; }

        [Display(Name = "Faster Sale Input")]
        public bool isShortcutSale { get; set; }

        [Display(Name = "Restaurant Sale")]
        public bool isRestaurantSale { get; set; }

        [Display(Name = "Touch")]
        public bool isTouch { get; set; }

        [Display(Name = "ShoeSale")]
        public bool isShoeSale { get; set; }

        [Display(Name = "IMEI Base Sale")]
        public bool isIMEISale { get; set; }

        [Display(Name = "Multiple Warehouse")]
        public bool isMultipleWh { get; set; }



        [Display(Name = "Enable Multi Currency")]
        public bool isMultiCurrency { get; set; }

        [Display(Name = "Enable Multi Debit Credit")]
        public bool isMultiDebitCredit { get; set; }

        [Display(Name = "Voucher Accunt Head Distribution Expense")]
        public bool isVoucherDistributionEntry { get; set; }

        [Display(Name = "Voucher Cheque Details / Head Wise Cheque No")]
        public bool isChequeDetails { get; set; }


        [Display(Name = "Company Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ComImageHeader { get; set; }

        [Display(Name = "Header Image [Folder]")]

        public string HeaderImagePath { get; set; }

        [Display(Name = "Header Files Extension")]
        public string HeaderFileExtension { get; set; }




        [Display(Name = "Company Logo [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ComLogo { get; set; }

        [Display(Name = "Logo Image [Folder]")]

        public string LogoImagePath { get; set; }

        [Display(Name = "Header Files Extension")]
        public string LogoFileExtension { get; set; }



        [Display(Name = "Company Logo [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ComSign { get; set; }

        [Display(Name = "Sign Image [Folder]")]

        public string SignImagePath { get; set; }

        [Display(Name = "Sign Files Extension")]
        public string SignFileExtension { get; set; }



        public virtual BusinessType vBusinessTypeCompany { get; set; }


        [Display(Name = "Voucher No Created Types")]
        public Nullable<int> VoucherNoCreatedTypeId { get; set; }
        [ForeignKey("VoucherNoCreatedTypeId")]
        public virtual Acc_VoucherNoCreatedType vAcc_VoucherNoCreatedTypes { get; set; }


        [Display(Name = "Currency")]
        public virtual Country vCountryCompany { get; set; }



        [DataType(DataType.Text)]
        [Display(Name = "Addvertise")]
        public string Addvertise { get; set; }


        public bool IsEPZ { get; set; }
        //[Display(Name = "Base Company")]
        //public virtual Company vCompanyCompany { get; set; } = null;



        // [Display(Name = "Company Name")]


        //public virtual ICollection<Product> vProducts { get; set; }
    }


    public class BusinessType
    {

        public int BusinessTypeId { get; set; }

        [Required]
        [Display(Name = "BusinessType Code")]
        public string BusinessTypeCode { get; set; }

        [Required]
        [Display(Name = "BusinessType Name")]
        public string Name { get; set; }

        //[Display(Name = "BusinessType Name")]

        //public virtual ICollection<Company> vBusinessTypesCompany { get; set; }
    }

    public class AppKeys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppKeysId { get; set; }

        [ForeignKey("Companies")]
        public int ComId { get; set; }

        public virtual Company Companies { get; set; }

        public string AppKey { get; set; }

    }

}