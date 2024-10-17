using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Repository.HR
{
    public class EmpInfoRepository : BaseRepository<HR_Emp_Info>, IEmpInfoRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public EmpInfoRepository(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IQueryable<EmployeeInfo> EmpInfo()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var query = from e in _context.HR_Emp_Info
                        .Where(x => x.ComId == comid
                        && x.IsInactive == false
                        && !x.IsDelete
                        ).OrderByDescending(x => x.EmpId)
                        select new EmployeeInfo
                        {
                            EmpId = e.EmpId,
                            EmpCode = e.EmpCode,
                            EmpName = e.EmpName,
                            DesigName = e.Cat_Designation != null ? e.Cat_Designation.DesigName : "",
                            DeptName = e.Cat_Department != null ? e.Cat_Department.DeptName : "",
                            SectName = e.Cat_Section != null ? e.Cat_Section.SectName : "",
                            EmpType = e.Cat_Emp_Type != null ? e.Cat_Emp_Type.EmpTypeName : "",
                            //DtJoin= e.DtJoin.HasValue ? e.DtJoin.Value.ToString("dd-MMM-yyyy") : "",
                            //DtJoin= e.DtJoin!=null ? e.DtJoin.Value.ToString("dd-MMM-yyyy") : "",
                            DtJoin = e.DtJoin,
                            Email = e.EmpEmail,
                            FloorName=e.Cat_Floor!=null?e.Cat_Floor.FloorName:"",
                            LineName=e.Cat_Line!=null?e.Cat_Line.LineName:"",
                            FingerId=e.FingerId
                            
                        };
            return query;
        }

        public void EmpInfoPost(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            hrEmpInfo.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            hrEmpInfo.DateUpdated = DateTime.Now;
            
            _context.Entry(hrEmpInfo).State = EntityState.Modified;

            if (hrEmpInfo.HR_Emp_PersonalInfo.EmpPersInfoId > 0 )
            {
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_PersonalInfo).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_PersonalInfo);
            }
                

            if (hrEmpInfo.HR_Emp_Address.EmpAddId > 0)
            {
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_Address).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Address);
            }
              

            if (hrEmpInfo.HR_Emp_Family.EmpFamilyId > 0)
            {
                hrEmpInfo.HR_Emp_Family.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_Family).State = EntityState.Modified;
            
            }
            else
            {
                hrEmpInfo.HR_Emp_Family.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Family);
            }
                

            if (hrEmpInfo.HR_Emp_Nominee.EmpNomId > 0)
            {
                hrEmpInfo.HR_Emp_Nominee.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_Nominee).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_Nominee.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Nominee);
            }
                

            if (hrEmpInfo.HR_Emp_BankInfo.BankInfoId > 0)
            {
                hrEmpInfo.HR_Emp_BankInfo.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_BankInfo).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_BankInfo.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_BankInfo);
            }
               

            if (hrEmpInfo.HR_Emp_Image.EmpImageId > 0)
            {
                hrEmpInfo.HR_Emp_Image.ComId = comid;
                if (file != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpImage = SetImage(file);
                }
                if (signFile != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpSign = SetImage(signFile);
                }
                _context.Entry(hrEmpInfo.HR_Emp_Image).State = EntityState.Modified;
                _context.Update(hrEmpInfo.HR_Emp_Image);
                _context.SaveChanges();
            }

            else
            {
                hrEmpInfo.HR_Emp_Image.ComId = comid;
                if (file != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpImage = SetImage(file);
                }
                if (signFile != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpSign = SetImage(signFile);
                }
                _context.Add(hrEmpInfo.HR_Emp_Image);
                _context.SaveChanges();
            }

            //else
            //{
            //    hrEmpInfo.HR_Emp_Image.EmpSign = hrEmpInfo.HR_Emp_Image.EmpSign;
            //    _context.Update(hrEmpInfo.HR_Emp_Image);

            //}

        }

        public void VendorInfoPost(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            hrEmpInfo.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            hrEmpInfo.DateUpdated = DateTime.Now;

            _context.Entry(hrEmpInfo).State = EntityState.Modified;

            if (hrEmpInfo.HR_Emp_PersonalInfo.EmpPersInfoId > 0)
            {
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_PersonalInfo).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_PersonalInfo);
            }


            if (hrEmpInfo.HR_Emp_Address.EmpAddId > 0)
            {
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_Address).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Address);
            }


            if (hrEmpInfo.HR_Emp_Family.EmpFamilyId > 0)
            {
                hrEmpInfo.HR_Emp_Family.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_Family).State = EntityState.Modified;

            }
            else
            {
                hrEmpInfo.HR_Emp_Family.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Family);
            }


            //if (hrEmpInfo.HR_Emp_Nominee.EmpNomId > 0)
            //{
            //    hrEmpInfo.HR_Emp_Nominee.ComId = comid;
            //    _context.Entry(hrEmpInfo.HR_Emp_Nominee).State = EntityState.Modified;
            //}

            //else
            //{
            //    hrEmpInfo.HR_Emp_Nominee.ComId = comid;
            //    _context.Add(hrEmpInfo.HR_Emp_Nominee);
            //}


            if (hrEmpInfo.HR_Emp_BankInfo.BankInfoId > 0)
            {
                hrEmpInfo.HR_Emp_BankInfo.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_BankInfo).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_BankInfo.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_BankInfo);
            }


            if (hrEmpInfo.HR_Emp_Image.EmpImageId > 0)
            {
                hrEmpInfo.HR_Emp_Image.ComId = comid;
                if (file != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpImage = SetImage(file);
                }
                if (signFile != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpSign = SetImage(signFile);
                }
                _context.Entry(hrEmpInfo.HR_Emp_Image).State = EntityState.Modified;
                _context.Update(hrEmpInfo.HR_Emp_Image);
                _context.SaveChanges();
            }

            else
            {
                hrEmpInfo.HR_Emp_Image.ComId = comid;
                if (file != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpImage = SetImage(file);
                }
                if (signFile != null)
                {
                    hrEmpInfo.HR_Emp_Image.EmpSign = SetImage(signFile);
                }
                _context.Add(hrEmpInfo.HR_Emp_Image);
                _context.SaveChanges();
            }

            //else
            //{
            //    hrEmpInfo.HR_Emp_Image.EmpSign = hrEmpInfo.HR_Emp_Image.EmpSign;
            //    _context.Update(hrEmpInfo.HR_Emp_Image);

            //}

        }

        public void StudentInfoPost(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            hrEmpInfo.UpdateByUserId = _httpContext.HttpContext.Session.GetString("userid");
            hrEmpInfo.DateUpdated = DateTime.Now;

            _context.Entry(hrEmpInfo).State = EntityState.Modified;

            if (hrEmpInfo.HR_Emp_PersonalInfo.EmpPersInfoId > 0)
            {
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_PersonalInfo).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_PersonalInfo);
            }


            if (hrEmpInfo.HR_Emp_Address.EmpAddId > 0)
            {
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_Address).State = EntityState.Modified;
            }

            else
            {
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Address);
            }


            if (hrEmpInfo.HR_Emp_Family.EmpFamilyId > 0)
            {
                hrEmpInfo.HR_Emp_Family.ComId = comid;
                _context.Entry(hrEmpInfo.HR_Emp_Family).State = EntityState.Modified;

            }
            else
            {
                hrEmpInfo.HR_Emp_Family.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Family);
            }    

        }
        //private byte[] SetImage(IFormFile file)
        //{
        //    byte[] image = null;
        //    using (var fs = file.OpenReadStream())
        //    using (var ms = new MemoryStream())
        //    {
        //        fs.CopyTo(ms);
        //        image = ms.ToArray();
        //    }
        //    return image;
        //}


        public byte[] SetImage(IFormFile file)
        {
            byte[] imageBytes = null;

            // Load the image and resize it
            using (var imageStream = file.OpenReadStream())
            using (var originalImage = System.Drawing.Image.FromStream(imageStream))
            {
                // Calculate the aspect ratio
                var aspectRatio = (double)originalImage.Width / originalImage.Height;

                // Calculate the new width based on the fixed height of 480px
                var newWidth = (int)(aspectRatio * 480);

                // Resize the image
                using (var resizedImage = new Bitmap(newWidth, 480))
                {
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        // Ensure high quality during resizing
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;

                        // Draw the image onto the resized bitmap
                        graphics.DrawImage(originalImage, 0, 0, newWidth, 480);
                    }

                    // Convert the resized image to byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        resizedImage.Save(memoryStream, originalImage.RawFormat);
                        imageBytes = memoryStream.ToArray();
                    }
                }
            }

            return imageBytes;
        }


        public List<HR_Emp_Info> GetEmp()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Emp_Info.Where(x => x.ComId == comid).ToList();
        }

        public IEnumerable<SelectListItem> GetEmpInfoAllList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return new SelectList(_context.HR_Emp_Info.Where(x => x.ComId == comid).ToList(), "EmpId", "EmpName");
        }

        public void EmpInfoPostElse(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1174 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            if (approveData == null)
            {
                hrEmpInfo.IsApprove = true;
            }
            else if (approveData.IsApprove == true)
            {
                hrEmpInfo.IsApprove = false;
            }
            else
            {
                hrEmpInfo.IsApprove = true;
            }
            //if (hrEmpInfo.VendorType != 0)
            //{
            //    hrEmpInfo.HR_Emp_Address.ComId = comid;
            //    _context.Add(hrEmpInfo.HR_Emp_Address);
            //    _context.Add(hrEmpInfo);
            //}
           
                hrEmpInfo.DateAdded = DateTime.Now;

                hrEmpInfo.HR_Emp_BankInfo.ComId = comid;
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                hrEmpInfo.HR_Emp_Nominee.ComId = comid;
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                hrEmpInfo.HR_Emp_Family.ComId = comid;


                _context.Add(hrEmpInfo.HR_Emp_BankInfo);
                _context.Add(hrEmpInfo.HR_Emp_Address);
                _context.Add(hrEmpInfo.HR_Emp_Nominee);
                _context.Add(hrEmpInfo.HR_Emp_PersonalInfo);
                _context.Add(hrEmpInfo.HR_Emp_Family);
                _context.Add(hrEmpInfo);

                if (file != null)
                {
                    hrEmpInfo.HR_Emp_Image.ComId = comid;
                    hrEmpInfo.HR_Emp_Image.EmpImage = SetImage(file);
                    _context.Add(hrEmpInfo.HR_Emp_Image);
                }
                if (signFile != null)
                {
                    hrEmpInfo.HR_Emp_Image.ComId = comid;
                    hrEmpInfo.HR_Emp_Image.EmpSign = SetImage(signFile);
                    _context.Add(hrEmpInfo.HR_Emp_Image);
                }
                else
                {
                    hrEmpInfo.HR_Emp_Image.ComId = comid;
                    _context.Add(hrEmpInfo.HR_Emp_Image);
                }
            
            

        }

        public void VendorInfoPostElse(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1174 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            if (approveData == null)
            {
                hrEmpInfo.IsApprove = true;
            }
            else if (approveData.IsApprove == true)
            {
                hrEmpInfo.IsApprove = false;
            }
            else
            {
                hrEmpInfo.IsApprove = true;
            }

            if (hrEmpInfo.VendorType != 0)
            {
               
                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Address);
                hrEmpInfo.DateAdded = DateTime.Now;
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                hrEmpInfo.HR_Emp_Family.ComId = comid;

                _context.Add(hrEmpInfo.HR_Emp_BankInfo);
                _context.Add(hrEmpInfo.HR_Emp_Address);        
                _context.Add(hrEmpInfo.HR_Emp_PersonalInfo);
                _context.Add(hrEmpInfo.HR_Emp_Family);
                _context.Add(hrEmpInfo);
            }
            if (file != null)
            {
                hrEmpInfo.HR_Emp_Image.ComId = comid;
                hrEmpInfo.HR_Emp_Image.EmpImage = SetImage(file);
                _context.Add(hrEmpInfo.HR_Emp_Image);
            }
            if (signFile != null)
            {
                hrEmpInfo.HR_Emp_Image.ComId = comid;
                hrEmpInfo.HR_Emp_Image.EmpSign = SetImage(signFile);
                _context.Add(hrEmpInfo.HR_Emp_Image);
            }
            else
            {
                hrEmpInfo.HR_Emp_Image.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Image);
            }


        }

        public void StudentInfoPostElse(HR_Emp_Info hrEmpInfo, IFormFile file, IFormFile signFile)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1174 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            if (approveData == null)
            {
                hrEmpInfo.IsApprove = true;
            }
            else if (approveData.IsApprove == true)
            {
                hrEmpInfo.IsApprove = false;
            }
            else
            {
                hrEmpInfo.IsApprove = true;
            }

            if (hrEmpInfo.VendorType != 0)
            {

                hrEmpInfo.HR_Emp_Address.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_Address);
                hrEmpInfo.DateAdded = DateTime.Now;
                hrEmpInfo.HR_Emp_PersonalInfo.ComId = comid;
                hrEmpInfo.HR_Emp_Family.ComId = comid;
                _context.Add(hrEmpInfo.HR_Emp_PersonalInfo);
                _context.Add(hrEmpInfo.HR_Emp_Family);
                _context.Add(hrEmpInfo);
            }


        }


        public async Task<HR_Emp_Info> EmpInfoEdit(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var hrEmpInfo =await _context.HR_Emp_Info
                .Include(h => h.HR_Emp_PersonalInfo)
                .Include(h => h.Cat_Skill)
                .Include(h => h.HR_Emp_Address)
                .Include(h => h.Cat_Department)
                .Include(h => h.Cat_Designation)
                .Include(h => h.Cat_Line)
                .Include(h => h.Cat_Floor)
                .Include(h => h.Cat_Unit)
                .Include(h => h.Cat_BloodGroup)
                .Include(h => h.Cat_Grade)
                .Include(h => h.Cat_Gender)
                .Include(h => h.Cat_Religion)
                .Include(h => h.HR_Emp_Address.Cat_DistrictCurr)
                .Include(h => h.HR_Emp_Address.Cat_DistrictPer)
                .Include(h => h.HR_Emp_Address.Cat_PoliceStationCurr)
                .Include(h => h.HR_Emp_Address.Cat_PoliceStationPer)
                .Include(h => h.HR_Emp_Address.Cat_PostOfficeCurr)
                .Include(h => h.HR_Emp_Address.Cat_PostOfficePer)
                .Include(h => h.HR_Emp_Educations)
                .Include(h => h.HR_Emp_Family)
                .Include(h => h.HR_Emp_Experiences)
				.Include(h => h.HR_Emp_Projects)
                .Include(h => h.HR_Emp_Devices)
                .Include(h => h.HR_Emp_Image)
                .Include(h => h.HR_Emp_Nominee)
                .Include(h => h.HR_Emp_BankInfo)
                .Include(h => h.HR_Emp_BankInfo.Cat_PayMode)
                .Include(h => h.HR_Emp_BankInfo.Cat_Bank)
                .Include(h => h.HR_Emp_BankInfo.Cat_BankBranch)
                .Include(h => h.HR_Emp_BankInfo.Cat_AccountType).Where(e => e.EmpId == id).FirstOrDefaultAsync();
            return hrEmpInfo;
        }

        public Task<HR_Emp_Info> VendorInfoEdit(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var hrEmpInfo = _context.HR_Emp_Info
                .Include(h => h.HR_Emp_PersonalInfo)
                .Include(h => h.HR_Emp_Address)
                .Include(h => h.Cat_Department)
                .Include(h => h.Cat_Designation)
                .Include(h => h.Cat_Unit)
                .Include(h => h.Cat_BloodGroup)
                .Include(h => h.Cat_Gender)
                .Include(h => h.Cat_Religion)
                .Include(h => h.HR_Emp_Image)
                .Include(h => h.HR_Emp_Address.Cat_DistrictCurr)
                .Include(h => h.HR_Emp_Address.Cat_DistrictPer)
                .Include(h => h.HR_Emp_Address.Cat_PoliceStationCurr)
                .Include(h => h.HR_Emp_Address.Cat_PoliceStationPer)
                .Include(h => h.HR_Emp_Address.Cat_PostOfficeCurr)
                .Include(h => h.HR_Emp_Address.Cat_PostOfficePer)
                .Include(h => h.HR_Emp_BankInfo)
                .Include(h => h.HR_Emp_BankInfo.Cat_PayMode)
                .Include(h => h.HR_Emp_BankInfo.Cat_Bank)
                .Include(h => h.HR_Emp_BankInfo.Cat_BankBranch)
                .Include(h => h.HR_Emp_BankInfo.Cat_AccountType)
                .Include(h => h.HR_Emp_Family).Where(e => e.EmpId == id).FirstOrDefaultAsync();
            return hrEmpInfo;
        }
        public Task<HR_Emp_Info> StudentInfoEdit(int? id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var hrEmpInfo = _context.HR_Emp_Info
                .Include(h => h.HR_Emp_PersonalInfo)
                .Include(h => h.HR_Emp_Address)
                .Include(h => h.Cat_Department)
                .Include(h => h.Cat_Designation)
                .Include(h => h.Cat_Unit)
                .Include(h => h.Cat_BloodGroup)
                .Include(h => h.Cat_Gender)
                .Include(h => h.Cat_Religion)
                .Include(h => h.HR_Emp_Address.Cat_DistrictCurr)
                .Include(h => h.HR_Emp_Address.Cat_DistrictPer)
                .Include(h => h.HR_Emp_Address.Cat_PoliceStationCurr)
                .Include(h => h.HR_Emp_Address.Cat_PoliceStationPer)
                .Include(h => h.HR_Emp_Address.Cat_PostOfficeCurr)
                .Include(h => h.HR_Emp_Address.Cat_PostOfficePer)

                .Include(h => h.HR_Emp_Family).Where(e => e.EmpId == id).FirstOrDefaultAsync();
            return hrEmpInfo;
        }

        public List<EmployeeInfoVM> GetEmpInfoAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            string userId = _httpContext.HttpContext.Session.GetString("userid");
          
            
            var query = $"Exec HR_PrcGetEmployeeInfo '{comid}'";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ComId", comid);
           
            List<EmployeeInfoVM> data = Helper.ExecProcMapTList<EmployeeInfoVM>("HR_PrcGetEmployeeInfo", parameters);
            return data;
        }
        public List<VendorInfoVM> GetStudentInfoAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ComId", comid);

            //var query = $"Exec HR_PrcGetEmployeeInfo '{comid}'";

            List<VendorInfoVM> data = Helper.ExecProcMapTList<VendorInfoVM>("HR_PrcGetStudentInfo", parameters);
            return data;
        }
        public List<VendorInfoVM> GetVendorInfoAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ComId", comid);
          
            //var query = $"Exec HR_PrcGetEmployeeInfo '{comid}'";

            List<VendorInfoVM> data = Helper.ExecProcMapTList<VendorInfoVM>("[HR_prcGetVendorInfo]", parameters);
            return data;
        }

        public FileContentResult DownloadEducationFile(string file)
        {
            string filepath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\EmpDocument\Certificates"}" + "\\" + file;

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            return response;
        }

        public IEnumerable<SelectListItem> VendorType()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var data = _context.Cat_Variable.Where(w => w.VarType == "Vendor Type").Select(x => new SelectListItem
            {
                Value = x.VarId.ToString(),
                Text = x.VarName
            }).ToList();
            return data;
            //_context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
        }

        //kamrul

         public IEnumerable<SelectListItem> VendorRelay()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var data = _context.Cat_Variable.Where(w => w.VarType == "Relay").Select(x => new SelectListItem
            {
                Value = x.VarId.ToString(),
                Text = x.VarName
            }).ToList();
            return data;
            //_context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
        }


        public IEnumerable<SelectListItem> VendorCategory()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var data = _context.Cat_Variable.Where(w => w.VarType == "CatagoryType").Select(x => new SelectListItem
            {
                Value = x.VarId.ToString(),
                Text = x.VarName
            }).ToList();
            return data;
            //_context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
        }

        public IEnumerable<SelectListItem> JobNatureType()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var data = _context.Cat_Variable.Where (w=>w.VarType == "DesignationType").Select(x => new SelectListItem
            {
                Value = x.VarId.ToString(),
                Text = x.VarName
            }).ToList();
            return data;
            //_context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
        }

        public IEnumerable<SelectListItem> AltitudeType()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var data = _context.Cat_Variable.Where(w => w.VarType == "AltitudeType").Select(x => new SelectListItem
            {
                Value = x.VarId.ToString(),
                Text = x.VarName
            }).ToList();
            return data;
            //_context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
        }

        public List<Daily_req_entry> GetRequisitionInfo(string searchDate)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@dtDate", searchDate);
            

            //var query = $"Exec HR_PrcGetEmployeeInfo '{comid}'";

            List<Daily_req_entry> data = Helper.ExecProcMapTList<Daily_req_entry>("[HR_RptAttendSumEntry]", parameters);
            if (data.Count > 0) { return data; }
            else {
                
                parameters[0] = new SqlParameter("@ComId", comid);
                parameters[1] = new SqlParameter("@dtDate", "");

                List<Daily_req_entry> dataDefault = Helper.ExecProcMapTList<Daily_req_entry>("[HR_RptAttendSumEntry]", parameters);
                return dataDefault;
            }

            
        }
        public void SaveRequisitionInfo(List<Daily_req_entry> requisition, string searchDate) {
           var comid= _httpContext.HttpContext.Session.GetString("comid");
            Daily_req_entry exist = new()  ;
            foreach (var d in requisition) {
                if (d.ID != 0)
                {
                     exist = _context.Daily_req_entry.Where(w => w.ID == d.ID && w.Comid == comid && w.dateTime == DateTime.Parse(searchDate)).Select(s => s).FirstOrDefault();
                }
                else ///for checking duplicate excel upload. excel upload don't have id
                {
                     exist = _context.Daily_req_entry.Where(w => w.Comid == comid && w.dateTime == DateTime.Parse(searchDate) && w.unitid == d.unitid && w.deptid == d.deptid && w.SectId == d.SectId && w.desigid==d.desigid).Select(s => s).FirstOrDefault();
                }

                if (exist != null)
                {
                    exist.Sup_A = d.Sup_A;
                    exist.Sup_G = d.Sup_G;
                    exist.Sup_B = d.Sup_B;
                    exist.Sup_C = d.Sup_C;
                    exist.Exc_A = d.Exc_A;
                    exist.Exc_G = d.Exc_G;
                    exist.Exc_B = d.Exc_B;
                    exist.Exc_C = d.Exc_C;
                    exist.Wor_A = d.Wor_A;
                    exist.Wor_G = d.Wor_G;
                    exist.Wor_B = d.Wor_B;
                    exist.Wor_C = d.Wor_C;

                    _context.Daily_req_entry.Update(exist);
                }
                else
                {
                    Daily_req_entry dr = new Daily_req_entry();
                    dr.Comid = comid;
                    dr.dateTime = DateTime.Parse(searchDate);
                    dr.deptid = d.deptid;
                    dr.DeptName = d.DeptName;
                    dr.Job_Nat = d.Job_Nat;
                    dr.Job_Loc = d.Job_Loc;
                    dr.Cost_head = d.Cost_head;
                    dr.unitid = d.unitid;
                    dr.desigid = d.desigid;
                    dr.SectId = d.SectId;
                    dr.Sup_A = d.Sup_A;
                    dr.Sup_G = d.Sup_G;
                    dr.Sup_B = d.Sup_B;
                    dr.Sup_C = d.Sup_C;
                    dr.Exc_A = d.Exc_A;
                    dr.Exc_G = d.Exc_G;
                    dr.Exc_B = d.Exc_B;
                    dr.Exc_C = d.Exc_C;
                    dr.Wor_A = d.Wor_A;
                    dr.Wor_G = d.Wor_G;
                    dr.Wor_B = d.Wor_B;
                    dr.Wor_C = d.Wor_C;

                    _context.Add(dr);
                }
               
            }
             _context.SaveChanges();
            

        }

    }
}
