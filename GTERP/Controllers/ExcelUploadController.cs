using GTCommercial.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class ExcelUploadController : Controller
    {
        private GTRDBContext db = new GTRDBContext();
        //GET: ExcelUpload
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            try
            {
                var userid = Session["userid"];

                string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Content/Upload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);


                
               


                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();

                            //dt.Load(cmdExcel.ExecuteReader());
                          
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();
                   
                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT *,'"+ userid + "' as userid  From [" + sheetName + "] where len(pono) > 2";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                    //conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    //using (SqlConnection con = new SqlConnection(conString))
                    //{
                    //        con.Open();
                    //        DataTable schema = con.GetSchema(tablename);

                    //        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    //        {

                    //            //Set the database table name.
                    //            sqlBulkCopy.DestinationTableName = "dbo." + tablename; // "+"_Temp


                    //            foreach (var item in dt.Columns)
                    //            {
                    //                sqlBulkCopy.ColumnMappings.Add(item.ToString(), "ExportPONo");
                    //            }

                    //            //for (int i = 0; i < dt.Columns.Count; i++)
                    //            //{
                    //            //    sqlBulkCopy.ColumnMappings.Add(i, i);
                    //            //}
                    //            con.Open();


                    //            sqlBulkCopy.WriteToServer(dt);
                    //            con.Close();
                    //        }
                    //}

                    var table = "Temp_COM_MasterLC_Details";

                    String connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("delete from dbo." + table + " where userid ='" + userid + "'", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //Response.Redirect("done.aspx");
                    con.Close();



                    conString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(conString))
                    {

                        //conn.Open();

                        //////delete data already exist
                        //SqlTransaction sqlTran = conn.BeginTransaction();
                        //string deleteQuery = "delete from dbo." + table + " where userid ='" + userid + "'";
                        //SqlCommand sqlComm = new SqlCommand(deleteQuery, conn, sqlTran);
                        //sqlComm.ExecuteNonQuery();
                        ////delete data already exist
                        //conn.Close();

                        var bulkCopy = new SqlBulkCopy(conn);
                        //bulkCopy.DestinationTableName = table;
                        bulkCopy.DestinationTableName = "dbo." + table; // "+"_Temp
                        conn.Open();

                        //////delete data already exist
                        //SqlTransaction sqlTran = conn.BeginTransaction();
                        //string deleteQuery = "delete from dbo." + table + " where userid ='" + userid + "'";
                        //SqlCommand sqlComm = new SqlCommand(deleteQuery, conn, sqlTran);
                        //sqlComm.ExecuteNonQuery();
                        ////delete data already exist


                        var schema = conn.GetSchema("Columns", new[] { null, null, table, null });
                        foreach (DataColumn sourceColumn in dt.Columns)
                        {
                            foreach (DataRow row in schema.Rows)
                            {
                                if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                                {
                                    bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                                    break;
                                }
                                //bulkCopy.ColumnMappings.Add("userid", (string)row["COLUMN_NAME"]);
                            }
                        }
                        bulkCopy.WriteToServer(dt);
                    }
                //}

            }
            //var ProductSerialresult = (db.Database.SqlQuery<ProductSerialtemp>("[prcGetExcelUploadData]  @comid, @userid , @tablename ", new SqlParameter("comid", Session["comid"]), new SqlParameter("userid", Session["userid"]), new SqlParameter("tablename", tablename))).ToList();

            return View();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public class ProductSerialtemp
        {
            public int ProductId { get; set; }

            public int ProductSerialId { get; set; }
            public string ProductSerialNo { get; set; }
        }
        //private static DataTable GetSchemaFromExcel(OleDbConnection excelOledbConnection)
        //{
        //    return excelOledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //}

        //private StyleInformation GetStyleInfo(DataRow row)
        //{
        //    return new StyleInformation
        //    {
        //        //StyleID = int.Parse(row[0].ToString()),
        //        StyleName = row[1].ToString(),
        //        StyleCode = row[2].ToString(),
        //        OrderQty = int.Parse(row[3].ToString())
        //    };
        //}

    }
}