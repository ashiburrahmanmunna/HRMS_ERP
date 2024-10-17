using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class Warrenty
    {
        public int WarrentyId { get; set; }

        [Required]
        [Display(Name = "Remarks")]
        public string WarrentyName { get; set; }

        [Required]
        [Display(Name = "Warrenty Day")]
        public int WarrentyDay { get; set; }


        public virtual ICollection<ProductSerial> vProductWarrenty { get; set; }


    }
}