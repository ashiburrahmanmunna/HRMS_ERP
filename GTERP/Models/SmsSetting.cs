using System;

namespace GTERP.Models
{
    public class SmsSetting
    {

        public int SmsSettingID { get; set; }
        public string smsAddress { get; set; }
        public string smsUser { get; set; }
        public string smsPassword { get; set; }
        public string smsSender { get; set; }
        public string smsCollection { get; set; }
        public string smsAbsent { get; set; }
        public string smsPresent { get; set; }
        public string smsLate { get; set; }
        public string isInactive { get; set; }
        public Guid Wid { get; set; }
        public string Company { get; set; }
        //public string comid { get; set; }


    }
}