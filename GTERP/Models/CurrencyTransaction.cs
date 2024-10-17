using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class CurrencyTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrencyTransactionId { get; set; }


        [Required]
        [Display(Name = "Input Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? tranDate { get; set; }
        [Display(Name = "Foreign Currency")]

        public int CountryIdForeign { get; set; }
        [Display(Name = "Foreign Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountForeign { get; set; }
        [Display(Name = "Local Currency")]

        public int CountryIdLocal { get; set; }
        [Display(Name = "Buying Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountLocalBuy { get; set; }
        [Display(Name = "Selling Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountLocalSale { get; set; }
        [Display(Name = "Is Auto Entry")]

        public Boolean isAutoEntry { get; set; }

        public string ComId { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<City> vProducts { get; set; }
    }
}