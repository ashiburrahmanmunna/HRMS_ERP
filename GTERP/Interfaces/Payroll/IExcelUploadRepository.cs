using GTERP.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace GTERP.Interfaces.Payroll
{
    public interface IExcelUploadRepository
    {
        void ExcelUploadFiles(IList<IFormFile> fileData);
        List<Temp_COM_MasterLC_Detail> UploadFilePO();
        DataTable CustomTable(DataTable excelTable, string currentuserid);
    }
}
