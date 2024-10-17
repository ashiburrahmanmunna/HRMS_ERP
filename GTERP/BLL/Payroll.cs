using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;

namespace GTERP.BLL
{
    public class PayrollRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        public PayrollRepository(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public List<string> GetProssTypes()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            List<string> prossList = new List<string>();
            clsConnectionNew clsCon = new clsConnectionNew();
            string sqlQuery = "Select Distinct Prosstype,dtInput from HR_ProcessedDataSal Where ComId = '" + comid + "' order by dtInput Desc";
            IDataReader reader = clsCon.GetReader(sqlQuery);

            string process = "";
            while (reader.Read())
            {
                process = reader["ProssType"].ToString();
                DateTime d = DateTime.Parse(process);
                if (prossList.Contains(process))
                {
                    continue;
                }
                else
                {
                    prossList.Add(process);
                }
            }
            return prossList;
        }

        public List<string> GetPFProssTypes()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            List<string> prossList = new List<string>();
            clsConnectionNew clsCon = new clsConnectionNew();
            string sqlQuery = "Select Distinct Prosstype,dtInput from HR_Emp_PF Where ComId = '" + comid + "' order by dtInput Desc";
            IDataReader reader = clsCon.GetReader(sqlQuery);

            string process = "";
            while (reader.Read())
            {
                process = reader["ProssType"].ToString();
                DateTime d = DateTime.Parse(process);
                if (prossList.Contains(process))
                {
                    continue;
                }
                else
                {
                    prossList.Add(process);
                }
            }
            return prossList;
        }

        public List<string> GetCasualProssTypes()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            List<string> prossList = new List<string>();
            clsConnectionNew clsCon = new clsConnectionNew();
            string sqlQuery = "Select Distinct Prosstype,dtInput from HR_ProcessedDataSalCasual Where ComId = '" + comid + "' order by dtInput Desc";
            IDataReader reader = clsCon.GetReader(sqlQuery);

            string process = "";
            while (reader.Read())
            {
                process = reader["ProssType"].ToString();
                //DateTime d = DateTime.Parse(process);
                if (prossList.Contains(process))
                {
                    continue;
                }
                else
                {
                    prossList.Add(process);
                }
            }
            return prossList;
        }


        public List<string> GetElProssTypes()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<string> prossList = new List<string>();
            clsConnectionNew clsCon = new clsConnectionNew();
            string sqlQuery = "Select  distinct concat( (DATENAME(MM,dtInput)),'-',(DATENAME(YYYY,dtInput))) as dteffective,dtInput from HR_Earn_Leave Where ComId = '" + comid + "' order by dteffective Desc";
                
                // "Select Distinct(CONVERT(VARCHAR(13), DATENAME(MM, dteffective), 100)) +'-' + convert(varchar, Year(dteffective)) as dteffective,dtInput from HR_Earn_Leave Where ComId = '" + comid + "' order by dteffective Desc";
            IDataReader reader = clsCon.GetReader(sqlQuery);

            string process = "";
            while (reader.Read())
            {
                process = reader["dteffective"].ToString();
                DateTime d = DateTime.Parse(process);
                if (prossList.Contains(process))
                {
                    continue;
                }
                else
                {
                    prossList.Add(process);
                }
            }
            return prossList;
        }
        public List<string> GetFestBonusProssTypes()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            List<string> prossList = new List<string>();
            clsConnectionNew clsCon = new clsConnectionNew();
            //string sqlQuery = "Select Distinct ProssType,dtBonus as dt from HR_FestBonusAll " +
            //                  "union " +
            //                  "Select Distinct ProssType, dtInput as dt from HR_FestAdvSalary " +
            //                  "order by dt desc";
            string sqlQuery = "Select Distinct Prosstype,dtInput,FestivalType from HR_FestBonusAll Where ComId = '" + comid + "' " +
                                "union " +
                               "Select Distinct ProssType, dtInput as dt,FestivalType from HR_FestAdvSalary Where ComId = '" + comid + "' " +
                                "union " +
                               "Select Distinct ProssType, dtInput as dt,FestivalType from HR_Gratuity Where ComId = '" + comid + "' " +
                                "union " +
                               "Select Distinct ProssType, dtInput as dt,FestivalType from HR_NaboBarsha Where ComId = '" + comid + "' " +
                                "union " +
                               "Select Distinct ProssType, dtInput as dt,FestivalType from HR_RecreationAllow Where ComId = '" + comid + "' " +
                               "order by dtInput Desc";
            IDataReader reader = clsCon.GetReader(sqlQuery);

            string process = "";
            while (reader.Read())
            {
                process = reader["ProssType"].ToString();
                DateTime d = DateTime.Parse(process);
                if (prossList.Contains(process))
                {
                    continue;
                }
                else
                {
                    prossList.Add(process);
                }
            }
            return prossList;
        }

    }
}