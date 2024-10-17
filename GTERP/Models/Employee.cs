using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Employee
    {

        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }


        [Required]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }




        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:#,#.00}")]
        [Display(Name = "Gross Salary")]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal GS { get; set; }




        [Display(Name = "Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] EmployeeImage { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]

        [Display(Name = "Image [Folder]")]

        public string ImagePath { get; set; }

        [Display(Name = "Files Extension")]
        public string FileExtension { get; set; }


        [Required]
        [Display(Name = "Company")]
        public int CommercialCompanyId { get; set; }
        public virtual SisterConcernCompany CommercialCompany { get; set; }
        [StringLength(128)]

        public string comid { get; set; }
        [Display(Name = "Section")]
        public Nullable<int> SubSectId { get; set; }

        //[Display(Name = "Category Name")]
        [ForeignKey("SubSectId")]
        public virtual Cat_SubSection Cat_SubSection { get; set; }

        [Display(Name = "Dept Head Name :")]
        public Nullable<int> BaseEmployeeId { get; set; }

        //[Display(Name = "Category Name")]
        [ForeignKey("BaseEmployeeId")]
        public virtual Employee EmployeeHOD { get; set; }


        //[Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address")]
        public string EmailId { get; set; }

        [Display(Name = "Link User Id From Chitra Panel")]
        public string LinkUserId { get; set; }

    }
}