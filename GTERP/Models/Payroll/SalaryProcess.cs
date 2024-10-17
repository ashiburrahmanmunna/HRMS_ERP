using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace GTERP.Models.Payroll
{
    public class SalaryProcess

    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = @"Payment Date :")]
        public string dtPayment { get; set; }

        [Display(Name = @"Month First Date :")]
        public string dtFirst { get; set; }

        [Display(Name = @"Month Last Date :")]
        public string dtLast { get; set; }

        [Display(Name = @"Bonus Date :")]
        public string dtFest { get; set; }

        [Display(Name = @"Effect Date :")]
        public string dtFestEffect { get; set; }

        [Display(Name = @"Days :")]
        public string Days { get; set; }


        [Display(Name = @"Salary Effect Month :")]
        public string dtSalAdv { get; set; }

        [Display(Name = @"First Date")]
        public string dtELPrcFirst { get; set; }

        [Display(Name = @"Last Date")]
        public string dtELPrcLast { get; set; }

        [Display(Name = @"Salary Type :")]
        public string SalType { get; set; }

        [Display(Name = @"Bonus Type :")]
        public string FestType { get; set; }

        [Display(Name = @"Relegion :")]
        public string Religion { get; set; }

        [Display(Name = @"Advance Type :")]
        public string AdvType { get; set; }

        [Display(Name = @"Advance Festival Type :")]
        public string AdvFestType { get; set; }
        public string salDesc { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string bonusPer { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Display(Name = @"Percentage :")]
        public string bonusAdvPer { get; set; }

        [Display(Name = @"Employee Type :")]
        public string EmpType { get; set; }

        [Display(Name = @"From Date :")]
        public string PFIndDtFrom { get; set; }

        [Display(Name = @"To Date :")]
        public string PFIndDtTo { get; set; }

        [Display(Name = @"To Date :")]
        public string PFIndPercentage { get; set; }
        [NotMapped]
        public string CasualDtFrom { get; set; }
        [NotMapped]
        public string CasualDtTo { get; set; }



        public string Command { get; set; }

        private void prcSetData(IDataRecord reader)
        {

        }

        //public static DataSet PrcGetData()
        //{
        //    DataSet dsList = new DataSet();
        //    clsConnectionNew clsCon = new clsConnectionNew("GTRHRIS_WEBDEMO", true);
        //    try
        //    {
        //        string sqlQuery = "Exec prcGetSalPross '" + HttpContext.Session.GetString("ComId") + "' ";
        //        clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);


        //        return dsList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        clsCon = null;
        //    }
        //}


    }
}
