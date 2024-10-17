using GTERP.Interfaces.HR;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HR
{
    public class RawDataImportRepository : IRawDataImportRepository
    {
        private readonly GTRDBContext db;
        private readonly IHttpContextAccessor _httpContext;
        public RawDataImportRepository(GTRDBContext context,IHttpContextAccessor httpContext)
        {
            db = context;
            _httpContext = httpContext;
        }
        public void UploadFiles(IFormFile file)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            string uploadlocation = Path.GetFullPath("wwwroot/Content/Upload/" + comid + "/" + userid + "/");

            if (!Directory.Exists(uploadlocation))
            {
                Directory.CreateDirectory(uploadlocation);
            }

            string filePath = uploadlocation + Path.GetFileName(file.FileName);

            string extension = Path.GetExtension(file.FileName);
            var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Close();

            string path = filePath;
            string[] readText = System.IO.File.ReadAllLines(path);

            //var DeviceNo = readText[0][1];
            List<string> newList = readText.ToList();
            List<Hr_RawData> hr_RawDataList = new List<Hr_RawData>();
            foreach (var item in newList)
            {

                var data = item.Split(':');
                var deviceNo = data[0];
                var cardNo = data[1];
                var punchDate = data[2];
                var punchTime = data[3] + ":" + data[4] + ":" + data[5];
                Hr_RawData hr_RawData = new Hr_RawData
                {

                    DeviceNo = deviceNo,
                    CardNo = cardNo,
                    DtPunchDate = DateTime.Parse(punchDate),
                    DtPunchTime = DateTime.Parse(punchTime),
                    ComId = comid
                };

                hr_RawDataList.Add(hr_RawData);
            }
             db.Hr_RawData.AddRange(hr_RawDataList);
             db.SaveChanges();
        }
    }
}
