using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    public class RawDataVM
    {

        public int aId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtJoin { get; set; }
        public string DesigName { get; set; }
        public string SectName { get; set; }
        public string DeptName { get; set; }

        public string Mobile { get; set; }
        public DateTime DtPunchDate { get; set; }
        public DateTime DtPunchTime { get; set; }
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string PunchLocation { get; set; }
        public string InOut { get; set; }
        public byte[]? PicFront { get; set; }
        public byte[]? PicBack { get; set; }
    }
}
