using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class SalesType
    {



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesTypeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string TypeName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Type Short Name")]
        public string TypeShortName { get; set; }

        //public string ComId { get; set; }
        public virtual ICollection<SalesSub> vTypeSalesSubs { get; set; }



    }






}