using GTERP.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace GTERP.ViewModels
{
    public class PRcostEntryVM
    {
        public int id { get; set; }
        [Display(Name = "Vendor Name")]
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Cat_Unit Cat_Unit { get; set; }
        public String? EmpName { get; set; }
        public int CategoryId { get; set; }
        public String? CategoryName { get; set; }
        public double? SafetyShoe { get; set; } = 0.0;
        public double? Uniform { get; set; } = 0.0;
        public double? ServiceComission { get; set; } = 0.0;
        public double? MedicalCost { get; set; } = 0.0;
    }
}
