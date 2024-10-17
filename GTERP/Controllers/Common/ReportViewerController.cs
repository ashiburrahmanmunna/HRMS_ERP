using GTERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;



namespace GTERP.Controllers
{
    ////[OverridableAuthorize]
    public class ReportViewerController : Controller
    {
        public ReportViewerController()
        {
        }

        private string strDSN = "";
        private string strQuery = "";
        private string strRFN = "";



        public void errorlog(Exception ex)
        {
            string filePath = @"C:\DevelopmentError\DevelopmentFile.txt";


            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.Message);
                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                    ex = ex.InnerException;
                }
            }
        }

        public void errorlog(string ex)
        {
            string filePath = @"C:\DevelopmentError\DevelopmentFile.txt";


            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(ex);
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();


            }
        }
        public ActionResult Index(string reporttype = "PDF")
        {

            try
            {

                LocalReport lr = new LocalReport();


                clsConnectionNew clsCon = new clsConnectionNew();
                string mimeType = "application/pdf";

                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

                string reportpath = "";
                string reportquery = "";
                string reportformat = "PDF";

                if (reporttype.ToString().ToUpper() == "EXCEL")
                {
                    reportformat = "xls";
                }
                else if (reporttype.ToString().ToUpper() == "WORD")
                {
                    reportformat = "doc";
                }
                else if (reporttype.ToString().ToUpper() == "HTML5")
                {
                    reportformat = "html";
                    mimeType = "text/html";
                }
                else
                {
                    reportformat = "pdf";
                    mimeType = "application/pdf";
                }

                if (HttpContext.Session.GetString("ReportPath") != null)
                {
                    reportpath = HttpContext.Session.GetString("ReportPath");
                    HttpContext.Session.SetString("ReportPath", "");
                }
                if (HttpContext.Session.GetString("reportquery") != null)
                {
                    reportquery = HttpContext.Session.GetString("reportquery");
                    //HttpContext.Session.SetString("reportquery","");
                }
                if (HttpContext.Session.GetString("ReportQuery") != null)
                {
                    reportquery = HttpContext.Session.GetString("ReportQuery");
                    //HttpContext.Session.SetString("reportquery","");
                }
                if (HttpContext.Session.GetString("ReportType") != null)
                {
                    reporttype = HttpContext.Session.GetString("ReportType");

                    if (reporttype.ToString().ToUpper() == "EXCEL")
                    {
                        reportformat = "xls";
                    }
                    else if (reporttype.ToString().ToUpper() == "WORD")
                    {
                        reportformat = "doc";
                    }
                    else if (reporttype.ToString().ToUpper() == "HTML5")
                    {
                        reportformat = "html";
                    }
                    else
                    {
                        reportformat = "pdf";
                    }
                }




                string path = "." + reportpath.Replace("~", "");//_hostingEnvironment.ContentRootPath + 

                //"~/ReportViewer/coatree.rdlc";

                //errorlog(path);

                //errorlog("3rd LINE");


                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    errorlog("location not found - path not found");
                    return View("Index");

                }

                //errorlog("exist ok LINE");



                lr.EnableExternalImages = true;

                clsCon.GTRFillDatasetWithSQLCommand(ref ds, reportquery);

                dt = ds.Tables[0];

                ReportDataSource rd = new ReportDataSource();
                rd.Name = "DataSet1";
                rd.Value = dt;

                lr.DataSources.Add(rd);
                lr.SubreportProcessing += new SubreportProcessingEventHandler(prcProcessSubReport);
                //reportType = "PDF";
                //string mimeType;
                string encoding;
                string fileNameExtension = "test";

                ReportPageSettings aPageSettings = lr.GetDefaultPageSettings();
                int width = aPageSettings.PaperSize.Width;
                int height = aPageSettings.PaperSize.Height;
                int margintop = aPageSettings.Margins.Top;
                int marginbottom = aPageSettings.Margins.Bottom;
                int marginleft = aPageSettings.Margins.Left;
                int marginright = aPageSettings.Margins.Right;

                //new LocalReport().EnableExternalImages = true;

                // errorlog("4th LINE");
                string deviceInfo =

                    "<DeviceInfo>" +
                    "  <OutputFormat>" + "PDF" + "</OutputFormat>" +
                    "  <PageWidth>" + width + "</PageWidth>" +
                    "  <PageHeight>" + height + "</PageHeight>" +
                    "  <MarginTop>" + margintop + "</MarginTop>" +
                    "  <MarginLeft>" + marginleft + "</MarginLeft>" +
                    "  <MarginRight>" + marginright + "</MarginRight>" +
                    "  <MarginBottom>" + marginbottom + "</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                lr.DisplayName = "GTR_Report";

                renderedBytes = lr.Render(
                    reporttype,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings
                    );

                var sFileName = HttpContext.Session.GetString("PrintFileName") + "_" + DateTime.Now.ToString("yyyyMMdd");


                errorlog("5th LINE");

                //Response..Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.Headers.Add("Content-Disposition", "inline; filename=" + sFileName + "." + reportformat);



                Stream stream = new MemoryStream(renderedBytes);

                //var fsResult = new FileStreamResult(stream, "application/pdf");
                var fsResult = new FileStreamResult(stream, mimeType);
                return fsResult;
            }
            catch (Exception ex)
            {

                errorlog(ex);
                Console.WriteLine(ex.Message);
                throw ex;

            }
        }
        public ActionResult Create()
        {

            errorlog("report viewer create");

            return View("Create");
        }


        private void prcProcessSubReport(object sender, SubreportProcessingEventArgs e)
        {


            DataTable dtSub = new DataTable();
            string sqlQuery = "", param = "";

            //strRFN = "VoucherId";


            prcGetSubReportDetails(e.ReportPath);
            param = strRFN.Length == 0 ? "" : e.Parameters[strRFN].Values[0].ToString();
            //sqlQuery = strQuery + " " + param;

            sqlQuery = strQuery.Replace("xxxx", param);

            //Ready a datatable for report based on parameter data
            dtSub = prcGetDataSub(sqlQuery);

            //Processing sub report data
            // e.DataSources.Add(new ReportDataSource(strDSN, dtSub)); //old

            ReportDataSource rpd = new ReportDataSource();
            rpd.Name = strDSN;
            rpd.Value = dtSub;
            e.DataSources.Add(rpd);


            //            this.Page.Title = "final checking title";
        }

        private DataTable prcGetDataSub(string strQuery)
        {
            //System.Data.DataSet 
            System.Data.DataSet ds = new System.Data.DataSet();
            clsConnectionNew clsCon = new clsConnectionNew();
            try
            {
                //SQL Query (Here i use Store procedure)
                clsCon.GTRFillDatasetWithSQLCommand(ref ds, strQuery);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
            }
            return ds.Tables[0];
        }

        private void prcGetSubReportDetails(string rptPath)
        {
            //Session["MyArrayList"] = new ArrayList();
            Dictionary<int, dynamic> postData = new Dictionary<int, dynamic>();
            //ArrayList rptList = new ArrayList();

            //postData = Session["rptList"] as Dictionary<int, dynamic>;

            postData = HttpContext.Session.GetObject<Dictionary<int, dynamic>>("rptList");


            foreach (var lst in postData)
            {

                string abcd = lst.Value.strRptPathSub;

                if (abcd.ToUpper() == rptPath.ToUpper())
                {
                    strDSN = lst.Value.strDSNSub;
                    strQuery = lst.Value.strQuerySub;
                    strRFN = lst.Value.strRFNSub;
                }
            }

            //foreach (var lst in postData)
            //{
            //}

            //subrp = "rptInvoice_PM";
            //strDSN = "DataSet1";
            //strQuery = "Exec custbill_corporate.dbo.rptInvoice_PM 65,1";
            //strRFN = "";



            //foreach (var lst in Common.Classes.clsReport.rptList)
            //{
            //    if (lst.strRptPathSub.ToUpper() == rptPath.ToUpper())
            //    {
            //        strDSN = lst.strDSNSub;
            //        strQuery = lst.strQuerySub;
            //        strRFN = lst.strRFNSub;
            //    }
            //}

        }
    }
}