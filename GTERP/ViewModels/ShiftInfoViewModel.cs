using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    public class ShiftInfoViewModel
    {
        public int Id { get; set; } 
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DeptName { get; set; }
        public string SectName { get; set; }
        public string DesigName { get; set; }
        public string Main_Shift { get; set; }
        public string Assigned_Shift { get; set; }
        [Display(Name = "Shift")]
        public int ShiftId { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
}
