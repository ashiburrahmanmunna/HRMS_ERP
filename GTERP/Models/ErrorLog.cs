using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public partial class ErrorLog
    {
        [Key]
        public int ErrorLogId { get; set; }
        public string UserName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ErrorType { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string CommandType { get; set; }
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public partial class SuccessLog
    {
        [Key]
        public int SuccessLogId { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string CommandType { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
    }

    class Log
    {
        GTRDBContext db;
        SuccessLog sLog;
        ErrorLog eLog;

        public Log(GTRDBContext context)
        {
            db = context;
            sLog = new SuccessLog();
            eLog = new ErrorLog();
        }


        public void Success(string uName, string cName, string aName, string cType)
        {
            sLog.CompanyName = "";
            sLog.UserName = uName;
            sLog.ControllerName = cName;
            sLog.ActionName = aName;
            sLog.DateTime = DateTime.Now;
            sLog.CommandType = cType;
            db.SuccessLogs.Add(sLog);
            db.SaveChangesAsync();
        }

        public void Error(string uName, string cName, string aName, string cType, string eType)
        {
            eLog.UserName = uName;
            eLog.ControllerName = cName;
            eLog.ActionName = aName;
            eLog.DateTime = DateTime.Now;
            eLog.CommandType = cType;
            eLog.ErrorType = eType;
            db.ErrorLogs.Add(eLog);
            db.SaveChangesAsync();
        }
    }
}