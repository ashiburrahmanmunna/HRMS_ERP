using GTERP.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace GTERP.Interfaces.Payroll
{
    public interface IMLExcelUploadRepository
    {
        void MLExcelUploadFiles(IList<IFormFile> fileData);
        List<Temp_COM_MasterLC_Detail> MLUploadFilePO();
        DataTable MLCustomTable(DataTable excelTable, string currentuserid);
    }
}
