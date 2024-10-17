using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HR
{
    public interface IRawDataImportRepository
    {
        public void UploadFiles(IFormFile file);
    }
}
