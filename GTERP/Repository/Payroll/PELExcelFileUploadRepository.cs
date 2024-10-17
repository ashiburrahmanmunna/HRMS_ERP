using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GTERP.Controllers;
using GTERP.Interfaces.Payroll;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace GTERP.Repository.Payroll
{
    public class PELExcelFileUploadRepository : IPELExcelFileUploadRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<PELExcelFileUploadRepository> _logger;
        public PELExcelFileUploadRepository(
            GTRDBContext context,
            IHttpContextAccessor httpContext,
            ILogger<PELExcelFileUploadRepository> logger
            )
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
        }

        public DataTable PELCustomTable(DataTable excelTable, string currentuserid)
        {
            DataTable table = new DataTable();
            for (int index = 0; index < excelTable.Rows.Count; index++)
            {
                DataRow excelRow = excelTable.Rows[index];

                string x = excelTable.Rows[index][1].ToString();
                if (x.Length > 1)
                {
                    //var col = 
                    table.Columns.Add(x, typeof(string));
                }
            }
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("SL", typeof(int));
            table.Columns.Add("userid", typeof(string));
            table.Rows.Add();
            DataRow fahad = table.Rows[0];
            object userid = currentuserid;
            fahad["SL"] = 1;
            fahad["Id"] = 1;
            fahad["userid"] = currentuserid;

            int i = 0;
            for (int index = 0; index < excelTable.Rows.Count; index++)
            {
                var colname = table.Columns[i].ColumnName.ToUpper();

                DataRow excelRow = excelTable.Rows[index];
                string x = excelTable.Rows[index][2].ToString();

                if (colname.Contains("date".ToUpper()))
                {
                    if (x.Length > 1 && x.IsNumeric())
                    {

                        fahad[i] = string.Format("{0}", DateTime.FromOADate(int.Parse(excelRow["Information"].ToString())));

                        //i++;
                    }
                }
                else
                {
                    if (x.Length > 1)
                    {

                        fahad[i] = string.Format("{0}", excelRow["Information"].ToString());

                    }

                }
                i++;

            }
            return table;
        }

        public void PELExcelUploadFiles(IList<IFormFile> fileData)
        {
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var conString = _context.Database.GetDbConnection().ConnectionString;

            IList<IFormFile> files = _httpContext.HttpContext.Request.Form.Files.ToList();

            foreach (IFormFile file in files)
            {

                string uploadlocation = Path.GetFullPath("Content/Upload/");

                if (!Directory.Exists(uploadlocation))
                {
                    Directory.CreateDirectory(uploadlocation);
                }

                string filePath = uploadlocation + Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
                fileStream.Close();

                ReadExcelFile(userid, conString, filePath);

            }

        }

        public List<Temp_COM_MasterLC_Detail> PELUploadFilePO()
        {
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            return _context.Temp_COM_MasterLC_Details.Where(m => m.userid == userid.ToString()).ToList();
        }
        static void ReadExcelFile(string userid, string conString, string filepath)
        {
            try
            {
                DataTable dt0 = new DataTable();
                DataTable dt1 = new DataTable();

                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filepath, false))
                {

                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();


                    foreach (var everysheet in sheets)
                    {
                        string relationshipId = everysheet.Id.Value;
                        string SheetName = everysheet.Name;

                        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();


                        if (SheetName == "PEL")
                        {
                            foreach (Cell cell in rows.ElementAt(0))
                            {
                                dt0.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                            }

                            int countcolumn = dt0.Columns.Count;
                            //dt0.Columns.Add("UserId");



                            foreach (Row row in rows) //this will also include your header row...
                            {
                                if (row.RowIndex > 0) // && row.RowIndex < rows.Count() - 3
                                {
                                    DataRow tempRow = dt0.NewRow();

                                    for (int i = 0; i < countcolumn; i++)
                                    {
                                        tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));

                                        var da = tempRow[i].ToString();
                                        if (i == 1 && da == "")   // for ignore null value emp code 
                                        {
                                            goto NotFoundEmpCode;
                                        }


                                        Console.WriteLine(dt0.Columns[i].ColumnName.ToUpper().Contains("InputDate".ToUpper()));

                                        if (dt0.Columns[i].ColumnName.ToUpper().Contains("InputDate".ToUpper()))
                                        {
                                            if (tempRow[i].ToString().Length > 1 && tempRow[i].ToString().IsNumeric())
                                            {
                                                tempRow[i] = string.Format("{0}", DateTime.FromOADate(int.Parse(tempRow[i].ToString())));

                                            }
                                        }

                                    }

                                    dt0.Rows.Add(tempRow);

                                NotFoundEmpCode:
                                    Console.WriteLine("Not Found Emp Code That's why this row not added");
                                }


                            }

                            dt0.Rows.RemoveAt(0); //...so i'm taking it out here.


                        }
                    }
                }

                #region details ///details table function///


                string table_Details = "HR_Earn_Leave_Prev";

                SqlConnection con = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("delete from dbo." + table_Details, con);
                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                    {
                        //bulkCopy.DestinationTableName = table;
                        DestinationTableName = "dbo." + table_Details // "+"_Temp
                    };
                    conn.Open();


                    DataTable schema = conn.GetSchema("Columns", new[] { null, null, table_Details, null });
                    foreach (DataColumn sourceColumn in dt0.Columns)
                    {
                        foreach (DataRow row in schema.Rows)
                        {
                            if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                            {
                                bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                                break;
                            }

                        }
                    }
                    bulkCopy.WriteToServer(dt0);

                    conn.Close();
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {


            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            var x = cell.StyleIndex;

            if (cell.CellValue == null) { return ""; }

            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                string a = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                return a;
            }
            else
            {
                return value;
            }
        }
        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}
