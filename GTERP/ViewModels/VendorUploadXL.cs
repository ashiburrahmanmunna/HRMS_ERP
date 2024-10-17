using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.ViewModels
{
    public class VendorUploadXL
    {
      
        public int? EmpId { get; set; }
        public string? EmpCode { get; set; }     
        public string? EmpName { get; set; }
        public string? EmpNameB { get; set; }
        public int? RelgionId { get; set; } 
        public int? BloodId { get; set; }    
        public int? UnitId { get; set; }      
        public int?  DeptId { get; set; }       
        public int? ShiftId { get; set; }      
        public int? DesigId { get; set; }     
        public int? SectId { get; set; }
        public int? SubSectId { get; set; }
        public DateTime? DtBirth { get; set; }
        public DateTime? DtJoin { get; set; }    
        public DateTime? DtIncrement { get; set; }  
        public DateTime? DtPromotion { get; set; }      
        public DateTime? DtConfirm { get; set; }
        public int? EmpTypeId { get; set; }     
        public int? EmploymentTypeId { get; set; }  
        public int? GenderId { get; set; }     
        public int? CategoryId { get; set; }   
        public int? SubCategoryId { get; set; }      
        public string? NID { get; set; }
        public string? FingerId { get; set; }    
        public bool? IsCasual { get; set; }   
        public bool? IsConfirm { get; set; }   
        public int? SkillId { get; set; }  
        public string? EmpPhone1 { get; set; }
        public string? EmpPhone2 { get; set; }
        public bool? IsInactive { get; set; }        
        public bool? IsIncentiveBns { get; set; }     
        public bool? IsNightAllow { get; set; }  
        public bool? IsHolidayAllow { get; set; }
        public string? EmpPerZip { get; set; }
        public string? EmpEmail { get; set; }     
        public string? EmpRemarks { get; set; }
        public int? GradeId { get; set; }   
        public int? FloorId { get; set; }      
        public int? LineId { get; set; }      
        public bool? IsAllowOT { get; set; }
        public DateTime? DtLocalJoin { get; set; }
        public int? ManageType { get; set; } = 0;
        public string? PcName { get; set; }
        public int? VendorType { get; set; }
        public int? JobNatureType { get; set; }
        public int? AltitudeData { get; set; }
        public double? Rate { get; set; }
        public double? Gs { get; set; }
        public string? Token { get; set; }
        public string? Category { get; set; }
        public int? WeekDay { get; set; }
        public int? WeekDay2 { get; set; }

    }
}
