using DocumentFormat.OpenXml.Wordprocessing;
using GTERP.Models.Letter;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using GTERP.Models.Payroll;

namespace GTERP.Models.ViewModels
{
    public class TaxDto
    {
        public int ClientId { get; set; }
        public int DesigId { get; set; }
        public int FiscalYearId { get; set; }
        public string ReportType { get; set; }
        public string ReportFormat { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
