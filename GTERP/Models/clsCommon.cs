using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace GTERP.Models
{
    public class clsCommon
    {
        #region Combo
        public class clsCombo1
        {
            public string Name { get; set; }
        }

        public class clsCombo2
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }


        //public class clsCombo3
        //{
        //    public string Id { get; set; }
        //    public string Name { get; set; }
        //}

        public class clsCombo3
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }




        #endregion Combo
    }

    public static class clsGenerateList
    {
        public static List<clsCommon.clsCombo1> prcColumnOne(DataTable dt)
        {
            List<clsCommon.clsCombo1> list = new List<clsCommon.clsCombo1>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                clsCommon.clsCombo1 item = new clsCommon.clsCombo1();
                item.Name = dt.Rows[i][0].ToString();
                list.Add(item);
            }
            return list;
        }
        public static List<clsCommon.clsCombo2> prcColumnTwo(DataTable dt)
        {
            List<clsCommon.clsCombo2> list = new List<clsCommon.clsCombo2>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                clsCommon.clsCombo2 item = new clsCommon.clsCombo2();
                item.Id = int.Parse(dt.Rows[i][0].ToString());
                item.Name = dt.Rows[i][1].ToString();
                list.Add(item);
            }
            return list;
        }
        public static List<clsCommon.clsCombo3> prcColumnTwo2(DataTable dt)
        {
            List<clsCommon.clsCombo3> list = new List<clsCommon.clsCombo3>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                clsCommon.clsCombo3 item = new clsCommon.clsCombo3();
                item.Id = dt.Rows[i][0].ToString();
                item.Name = dt.Rows[i][1].ToString();
                list.Add(item);
            }
            return list;
        }
        public static List<clsCommon.clsCombo2> prcColumnTwo(DataRow[] datarow)
        {
            List<clsCommon.clsCombo2> list = new List<clsCommon.clsCombo2>();

            foreach (DataRow dr in datarow)
            {
                clsCommon.clsCombo2 item = new clsCommon.clsCombo2();
                item.Id = int.Parse(dr[0].ToString());
                item.Name = dr[1].ToString();
                list.Add(item);
            }
            return list;
        }

        public static List<clsCommon.clsCombo3> prcColumnTwo(DataTable dt, string Flag)
        {
            List<clsCommon.clsCombo3> list = new List<clsCommon.clsCombo3>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                clsCommon.clsCombo3 item = new clsCommon.clsCombo3();
                item.Id = dt.Rows[i][0].ToString();
                item.Name = dt.Rows[i][1].ToString();
                list.Add(item);
            }
            return list;
        }
    }

    public class clsSendingMain
    {
        public string fncSendMail(string mailType, string addFrom, string addFromCaption, string addTo, string addToCc, string addToBcc,
                                  string mailSubject, string mailBody, string pm)
        {
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => { return true; };


            SmtpClient SmtpServer = new SmtpClient("smtp.gtrbd.com");
            SmtpServer.Credentials = new System.Net.NetworkCredential(addFrom, pm);
            SmtpServer.Port = 25;//Office 2003 : 265
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.EnableSsl = true;
            SmtpServer.Host = "smtp.gtrbd.com";
            MailMessage mail = new MailMessage();
            SmtpServer.EnableSsl = false;

            try
            {
                mail.From = new MailAddress(addFrom, addFromCaption);
                mail.To.Add(addTo);
                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }
    }
}