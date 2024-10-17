using GTERP.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace GTERP.Interfaces.Payroll
{
    public interface IPELExcelFileUploadRepository
    {
        void PELExcelUploadFiles(IList<IFormFile> fileData);
        List<Temp_COM_MasterLC_Detail> PELUploadFilePO();
        DataTable PELCustomTable(DataTable excelTable, string currentuserid);
    }
}
