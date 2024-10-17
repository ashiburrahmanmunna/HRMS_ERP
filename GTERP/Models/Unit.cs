using GTERP.Models.Self;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class Unit : SelfModel
    {

        public int UnitId { get; set; }

        [Required]
        [Display(Name = "Unit Name")]
        [StringLength(100)]
        public string UnitName { get; set; }

        [Display(Name = "Unit Name")]
        [StringLength(100)]
        public string UnitNameBangla { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Unit Short Name")]
        public string UnitShortName { get; set; }
        [Display(Name = "SL No")]
        public short? Slno { get; set; }

        public virtual ICollection<Product> vProductUnit { get; set; }



    }
}