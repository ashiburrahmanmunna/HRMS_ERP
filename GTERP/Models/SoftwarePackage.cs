using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class SoftwarePackage
    {

        public int SoftwarePackageId { get; set; }

        [Required]
        [Display(Name = "SoftwarePackage Code")]
        public string SoftwarePackageCode { get; set; }

        [Required]
        [Display(Name = "SoftwarePackage")]
        public string Name { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "SoftwarePackage Description")]
        public string SoftwarePackageDescription { get; set; }

        [Display(Name = "SoftwarePackage Link")]
        public string LinkAdd { get; set; }


        [Display(Name = "SoftwarePackage Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] SoftwarePackageImage { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]

        [Display(Name = "SoftwarePackage Image [Folder]")]

        public string SoftwarePackageImagePath { get; set; }

        [Display(Name = "SoftwarePackage Files Extension")]
        public string SoftwarePackageFileExtension { get; set; }

        [Display(Name = "Start From Date")]
        public string PackageStartDate { get; set; }


        [Display(Name = "End Date")]
        public string PackageEndDate { get; set; }

        //[Display(Name = "SoftwarePackage")]

        //public virtual ICollection<Product> vProducts { get; set; }
    }
}