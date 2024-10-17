using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.ViewModels
{
    public class RawDataEditVM
    {
        public int aId { get; set; }
        public string EmpCode { get; set; }
        public DateTime DtPunchDate { get; set; }
        public DateTime DtPunchTime { get; set; }
    }
}
