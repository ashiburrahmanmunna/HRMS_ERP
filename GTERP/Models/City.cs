using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class City
    {





        public int CityId { get; set; }

        [Required]
        [Display(Name = "City Code")]
        public string CityCode { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }



        [Required]
        [Display(Name = "City Name")]
        public string CityName { get; set; }



        [Display(Name = "State")]
        public virtual State vStateCity { get; set; }


        // [Display(Name = "Category Name")]

        // public virtual ICollection<City> vProducts { get; set; }
    }

    public class CityViewModel
    {

        public int CityViewId { get; set; }


        [Display(Name = "City Code")]
        public string CityCode { get; set; }

        [Display(Name = "City Name")]
        public string CityName { get; set; }


        [Display(Name = "State")]
        public string StateName { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

    }


    public class State
    {

        public int StateId { get; set; }

        [Required]
        [Display(Name = "State Code")]
        public string StateCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }



        [Required]
        [Display(Name = "State Name")]
        public string StateName { get; set; }



        [Display(Name = "Country")]
        public virtual Country vStateCountry { get; set; }


        [Display(Name = "City")]

        public virtual ICollection<City> vProducts { get; set; }
    }
}