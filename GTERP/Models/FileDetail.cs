using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class FileDetail
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int SupportId { get; set; }
        public virtual Support Support { get; set; }
        [Display(Name = "Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] DBFile { get; set; }

    }

    public class HomeSlider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HomeSliderId { get; set; }

        [Required]
        [Display(Name = "Slider Code")]
        public string SliderCode { get; set; }

        [Required]
        [Display(Name = "Slider")]
        public string SliderName { get; set; }

        [Display(Name = "Information")]
        public string SliderInformation { get; set; }


        [Display(Name = "Image [DB]")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] SliderImage { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]

        [Display(Name = "Image [Folder]")]

        public string ImagePath { get; set; }

        [Display(Name = "Files Extension")]
        public string FileExtension { get; set; }

        public string SliderImg { get; set; }

        public string LinkAddress { get; set; }
        //[Display(Name = "Slider")]

        //public virtual ICollection<Product> vProducts { get; set; }
    }
}