using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    public class AttFixGridUBL
    {
        public bool IsChecked { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string SectName { get; set; }
        public string DeptName { get; set; }
        public string DesigName { get; set; }
        public string? VandorName { get; set; }
        public int ShiftId { get; set; }
        // public string ShiftName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtPunchDate { get; set; } = DateTime.Now.Date;

        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeIn { get; set; } = DateTime.Now.ToShortTimeString();
        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string TimeOut { get; set; } = DateTime.Now.ToShortTimeString();

        [DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string OTHourInTime { get; set; } = DateTime.Now.ToShortTimeString();

        public string Status { get; set; }
        public int StatusId { get; set; }

        public string Remarks { get; set; }
        //[DisplayFormat(DataFormatString = "{0:HH-MM}", ApplyFormatInEditMode = true)]
        public string OtHour { get; set; }
        public float OT { get; set; }
        public string Line { get; set; }
        public bool IsInactive { get; set; }
        public int SectId { get; set; }
        public string Criteria { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DtFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DtTo { get; set; }
    }
}
