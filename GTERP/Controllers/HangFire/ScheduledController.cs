using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace GTERP.Controllers.HangFire
{
    public class ScheduledController : Controller
    {
       // static string conString = AppData.AppData.dbdaperpconstring;
        static string conString = "Server=118.67.215.112; Database=GTRHRIS_ND; user id=sa; password=gTR@!@NeeDle4#@839%&hR$#; MultipleActiveResultSets=true; Connection Timeout=300;";

        public IActionResult Index()
        {
            string CronExpressionForDailyJob = "30 22 * * *";
            var jobIdDaily = "Data Send To Needle Drop";
            //TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dhaka");
            TimeZoneInfo bdTimeZone = TZConvert.GetTimeZoneInfo("Asia/Dhaka");
            var options = new RecurringJobOptions
            {
                //TimeZone = TimeZoneInfo.Utc // Time zone for the job schedule
                TimeZone = bdTimeZone                          // You can add more options here if needed
            };
            RecurringJob.AddOrUpdate(jobIdDaily, () => DailyJob(), CronExpressionForDailyJob, options);
            return View();
        }

        public async Task DailyJob()
        {
            string parameter = "ebd248d9-d0c3-41a6-81e1-bcccfff41bbe";
            ExecProc("DataMigrationERPToND", parameter);
            //ExecProc("DataMigrationERPToND");
        }

        public IActionResult Test()
        {
            string parameter = "ebd248d9-d0c3-41a6-81e1-bcccfff41bbe";
             ExecProc("DataMigrationERPToND", parameter);

           // ExecProc("DataMigrationERPToND");
            return View();  
        }
        public static void ExecProc(string storedProcedureName, string parameter)
        {
            SqlConnection connection = null;

            try
            {
                AppData.AppData.globalException = "";

                using (connection = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                    {
                        cmd.CommandTimeout = 1000; 

                        cmd.CommandType = CommandType.StoredProcedure;                     
                        cmd.Parameters.AddWithValue("@ComId", parameter);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.AppData.globalException = ex.Message;
                Console.WriteLine(ex.Message);
            }
            finally
            {
               
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }



        public static void ExecProc(string storedProcedureName)
        {
            try
            {
                AppData.AppData.globalException = "";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.AppData.globalException = ex.Message;
                Console.WriteLine(ex.Message);
                
            }
        }


    }
}
