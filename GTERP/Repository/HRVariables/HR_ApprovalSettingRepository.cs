using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.ViewModels;
using GTERP.Repository.Self;
using GTERP.Services;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nancy.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GTERP.Controllers.ControllerFolder.ControllerFolderController;
using static GTERP.ViewModels.CommercialVM;

namespace GTERP.Repository.HRVariables
{
    public class HR_ApprovalSettingRepository : SelfRepository<HR_ApprovalSetting>, IHR_ApprovalSettingRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;

        public HR_ApprovalSettingRepository
            (GTRDBContext context,
            IHttpContextAccessor httpContext,
            IConfiguration configuration
            ) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
            _configuration = configuration;
        }

        public void Approved(List<ApprovalVM> approve)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            foreach (var a in approve)
            {
                if (a.ApproveType == 1173)
                {
                    var empSalary = _context.HR_Emp_Salary.Where(x => x.ComId == comid && !x.IsDelete && x.EmpId == a.EmpId).ToList();
                    foreach (var s in empSalary)
                    {
                        s.IsApprove = true;
                        s.ApprovedBy = a.ApprovedBy;
                        s.Remarks = a.Remarks;
                        _context.Entry(s).State = EntityState.Modified;
                    }
                }
                else if (a.ApproveType == 1174)
                {
                    var empinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete && x.EmpId == a.EmpId).ToList();
                    foreach (var e in empinfo)
                    {
                        e.IsApprove = true;
                        e.ApprovedBy = a.ApprovedBy;
                        e.Remarks = a.Remarks;
                        _context.Entry(e).State = EntityState.Modified;
                    }
                }
                else if (a.ApproveType == 1175)
                {
                    var empleave = _context.HR_Leave_Avail.Where(x => x.ComId == comid && x.EmpId == a.EmpId && x.DtLvInput == a.InputDate && x.LvType == a.LeaveType && x.IsApprove == false).ToList();

                    foreach (var l in empleave)
                    {
                        var empleavebalance = _context.HR_Leave_Balance.Where(x => x.ComId == comid && x.EmpId == l.EmpId && x.DtOpeningBalance == l.DtInput.Value.Year).FirstOrDefault();
                        var leavetype = _context.Cat_Leave_Type.Where(x => x.LTypeId == l.LTypeId).FirstOrDefault();
                        var AvailCL = (float)(empleavebalance.CL - empleavebalance.ACL);
                        var AvailSL = (float)(empleavebalance.SL - empleavebalance.ASL);
                        var AvailEL = (float)(empleavebalance.EL - empleavebalance.AEL);
                        var AvailML = (float)(empleavebalance.ML - empleavebalance.AML);
                        l.Status = 1;
                        l.IsApprove = true;
                        l.ApprovedBy = a.ApprovedBy;
                        l.Remarks = a.Remarks;

                        _context.Entry(l).State = EntityState.Modified;

                        if (leavetype.LTypeNameShort == "CL") //|| leavetype.LTypeNameShort == "CLH")
                        {
                            if (AvailCL >= l.TotalDay)
                            {
                                empleavebalance.PreviousLeave = empleavebalance.ACL;
                                empleavebalance.ACL = (empleavebalance.PreviousLeave + l.TotalDay);

                            }

                        }

                        else if (leavetype.LTypeNameShort == "CLH") //|| leavetype.LTypeNameShort == "CLH")
                        {
                            if (AvailCL >= l.TotalDay)
                            {
                                empleavebalance.PreviousLeave = empleavebalance.ACL;
                                empleavebalance.ACL = (empleavebalance.PreviousLeave + l.TotalDay);
                            }

                        }

                        else if (leavetype.LTypeNameShort == "EL" || leavetype.LTypeNameShort == "ELH")
                        {
                            if (AvailEL >= l.TotalDay)
                            {
                                empleavebalance.PreviousLeave = empleavebalance.AEL;
                                empleavebalance.AEL = (empleavebalance.PreviousLeave + l.TotalDay);
                            }

                        }
                        else if (leavetype.LTypeNameShort == "SL" || leavetype.LTypeNameShort == "SLH")
                        {
                            if (AvailSL >= l.TotalDay)
                            {
                                empleavebalance.PreviousLeave = empleavebalance.ASL;
                                empleavebalance.ASL = (empleavebalance.PreviousLeave + l.TotalDay);
                            }

                        }
                        _context.Entry(empleavebalance).State = EntityState.Modified;

                    }
                }
                else if (a.ApproveType == 1176)
                {
                    var empincrement = _context.HR_Emp_Increment.Where(x => x.ComId == comid && x.EmpId == a.EmpId).ToList();
                    foreach (var i in empincrement)
                    {
                        i.IsApprove = true;
                        i.ApprovedBy = a.ApprovedBy;
                        i.Remarks = a.Remarks;
                        _context.Entry(i).State = EntityState.Modified;

                        // for salary update
                        HR_Emp_Salary empsal = _context.HR_Emp_Salary
                        .Include(e => e.HR_Emp_Info)
                        .Where(s => s.EmpId == i.EmpId && s.ComId == comid).FirstOrDefault();
                        if (empsal != null)
                        {
                            empsal.PersonalPay = (float)i.NewSalary;
                            empsal.BasicSalary = (float)i.NewBS;
                            empsal.HouseRent = (float)i.NewHR;
                            empsal.HrExp = (float)i.NewHRExp;
                            empsal.HRExpensesOther = (float)i.NewHRExpOther;
                            empsal.MadicalAllow = (float)i.NewMA;
                            empsal.FoodAllow = (float)i.NewFA;
                            empsal.ConveyanceAllow = (float)i.NewTA;
                            var newdesigid = Convert.ToInt32(i.NewDesigId);
                            if (newdesigid > 0)
                            {
                                empsal.HR_Emp_Info.DesigId = newdesigid;
                            }

                            var newsectid = Convert.ToInt32(i.NewSectId);
                            if (newsectid > 0)
                            {
                                empsal.HR_Emp_Info.SectId = newsectid;
                            }

                            var newemptypeid = Convert.ToInt32(i.NewEmpTypeId);
                            if (newemptypeid > 0)
                            {
                                empsal.HR_Emp_Info.EmpTypeId = newemptypeid;
                            }

                            empsal.HR_Emp_Info.DtIncrement = i.DtIncrement;
                            _context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                            _context.Entry(empsal).State = EntityState.Modified;
                        }
                    }
                    _context.SaveChanges();
                }

                else if (a.ApproveType == 1177)
                {
                    var emprelease = _context.HR_Emp_Released.Where(x => x.ComId == comid && !x.IsDelete && x.EmpId == a.EmpId).ToList();
                    foreach (var r in emprelease)
                    {
                        r.IsApprove = true;
                        r.ApprovedBy = a.ApprovedBy;
                        r.Remarks = a.Remarks;
                        _context.Entry(r).State = EntityState.Modified;
                    }
                }

                else if (a.ApproveType == 1178)
                {
                    var fixedatt = _context.HR_AttFixed.Where(x => x.ComId == comid && x.EmpId == a.EmpId && x.DtPunchDate == DateTime.Parse(a.LeaveType)).ToList();
                    foreach (var f in fixedatt)
                    {
                        f.IsApprove = true;
                        f.ApprovedBy = a.ApprovedBy;
                        f.Remarks = a.Remarks;
                        _context.Entry(f).State = EntityState.Modified;
                    }
                }
                _context.SaveChanges();
            }
        }

        public void Disapproved(List<ApprovalVM> disapprove)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            foreach (var a in disapprove)
            {
                if (a.ApproveType == 1173)
                {
                    var empSalary = _context.HR_Emp_Salary.Where(x => x.ComId == comid && !x.IsDelete && x.EmpId == a.EmpId).ToList();
                    foreach (var s in empSalary)
                    {
                        s.IsApprove = false;
                        _context.Entry(s).State = EntityState.Modified;
                    }
                }
                else if (a.ApproveType == 1174)
                {
                    var empinfo = _context.HR_Emp_Info.Where(x => x.ComId == comid && !x.IsDelete && x.EmpId == a.EmpId).ToList();
                    foreach (var e in empinfo)
                    {
                        e.IsApprove = true;
                        _context.Entry(e).State = EntityState.Modified;
                    }
                }
                _context.SaveChanges();
            }
        }

        public List<SetApproveViewModel> GetApprovalList(string comid, int approve)
        {
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@ComId", comid);
            parameters[1] = new SqlParameter("@UserId", userid);
            parameters[2] = new SqlParameter("@ApproveType", approve);

            var query = $"Exec HR_PrcGetApprove '{comid}','{userid}','{approve}'";

            List<SetApproveViewModel> data = Helper.ExecProcMapTList<SetApproveViewModel>("HR_PrcGetApprove", parameters);

            return data;
        }

        public IEnumerable<SelectListItem> GetApprovalType()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");

            var data = _context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
            return new SelectList(data, "VarId", "VarName", comid);

            //if (comid == "576B68B3-DA3F-4FE5-9656-BC92E4DCDF72")
            //{
            //  var data = _context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
            //  return new SelectList(data, "VarId", "VarName", comid);
            //}
            //else
            //{
            //   var data = (from a in _context.Cat_Variable join b in _context.HR_ApprovalSettings on a.VarId equals b.ApprovalType where (a.VarType == "Approval Type" && b.ComId == comid && b.IsDelete == false) select a).ToList();
            //   return new SelectList(data, "VarId", "VarName", comid);
            //}
        }

        public SelectList GetApprovedBy()
        {
            var appKey = _httpContext.HttpContext.Session.GetString("appkey");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var model = new GetUserModel();
            model.AppKey = Guid.Parse(appKey);
            WebHelper webHelper = new WebHelper();

            string req = JsonConvert.SerializeObject(model);

            Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            string response = webHelper.GetUserCompany(url, req);

            GetResponse res = new GetResponse(response);

            // for user list
            var list = res.MyUsers.ToList();
            var distinctList = list.DistinctBy(x => x.UserName).Where(x => x.UserID == userid).ToList();
            var l = new List<AspnetUserList>();


            foreach (var c in distinctList)
            {
                var le = new AspnetUserList();
                le.Email = c.UserName;
                le.UserId = c.UserID;
                le.UserName = c.UserName;
                l.Add(le);
            }

            return new SelectList(l, "UserId", "UserName", userid);

        }

        public List<GetApprovalListVM> GetApproveList()
        {
            //var appKey = _httpContext.HttpContext.Session.GetString("appkey");
            //var userid = _httpContext.HttpContext.Session.GetString("userid");
            var comid = _httpContext.HttpContext.Session.GetString("comid");


            //var model = new GetUserModel();
            //model.AppKey = Guid.Parse(appKey);
            //WebHelper webHelper = new WebHelper();

            //string req = JsonConvert.SerializeObject(model);

            //Uri url = new Uri(string.Format(_configuration.GetValue<string>("API:GetCompanies")));

            //string response = webHelper.GetUserCompany(url, req);

            //GetResponse res = new GetResponse(response);


            var getCompanyUsersURL = $"{_configuration.GetValue<string>("API:GetAllCompnayUsers")}/{comid}";
            var companyUserList = APIHelper.GetRequest<List<CompanyUserVM>>(getCompanyUsersURL, false).Result;


            // for user list
            //var list = res.MyUsers.ToList();
            var distinctList = companyUserList.DistinctBy(a => a.UserName);
            var company = _context.Companys.ToList();
            var vartype = _context.Cat_Variable.ToList();
            var approve = _context.HR_ApprovalSettings.ToList();

            var data = (from a in approve
                        join c in company on a.ComId equals c.CompanyCode into table1
                        from t1 in table1.ToList().DefaultIfEmpty()
                        join u in distinctList on a.UserId equals u.UserId into table2
                        from t2 in table2.ToList().DefaultIfEmpty()
                        join v in vartype on a.ApprovalType equals v.VarId into table3
                        from t3 in table3.ToList().DefaultIfEmpty()
                        where a.ComId == comid && a.IsDelete == false
                        select new GetApprovalListVM
                        {
                            ApprovalSettingId = a.ApprovalSettingId,
                            CompanyName = t1.CompanyName,
                            UserName = t2.UserName,
                            ApproveType = t3.VarName,
                            IsApprove = a.IsApprove
                        }).ToList();

            return data;

            //if (comid== "576B68B3-DA3F-4FE5-9656-BC92E4DCDF72")
            //{
            //     var data = (from a in approve
            //                join c in company on a.ComId equals c.CompanyCode into table1
            //                from t1 in table1.ToList().DefaultIfEmpty()
            //                join u in distinctList on a.UserId equals u.UserID into table2
            //                from t2 in table2.ToList().DefaultIfEmpty()
            //                join v in vartype on a.ApprovalType equals v.VarId into table3
            //                from t3 in table3.ToList().DefaultIfEmpty()
            //                where a.IsDelete == false
            //                 select new GetApprovalListVM
            //                {
            //                    ApprovalSettingId = a.ApprovalSettingId,
            //                    CompanyName = t1.CompanyName,
            //                    UserName = t2.UserName,
            //                    ApproveType = t3.VarName,
            //                    IsApprove = a.IsApprove
            //                }).ToList();
            //    return data;
            //}
            //else
            //{
            //    var data = (from a in approve
            //                join c in company on a.ComId equals c.CompanyCode into table1
            //                from t1 in table1.ToList().DefaultIfEmpty()
            //                join u in distinctList on a.UserId equals u.UserID into table2
            //                from t2 in table2.ToList().DefaultIfEmpty()
            //                join v in vartype on a.ApprovalType equals v.VarId into table3
            //                from t3 in table3.ToList().DefaultIfEmpty()
            //                where a.ComId == comid && a.IsDelete==false
            //                select new GetApprovalListVM
            //                {
            //                    ApprovalSettingId = a.ApprovalSettingId,
            //                    CompanyName = t1.CompanyName,
            //                    UserName = t2.UserName,
            //                    ApproveType = t3.VarName,
            //                    IsApprove = a.IsApprove
            //                }).ToList();
            //    return data;
            //}


        }

        public SelectList GetCompanyList()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var company = _context.Companys.Where(x => x.CompanyCode == comid).ToList();
            return new SelectList(company, "CompanyCode", "CompanyName", comid);
        }

        public Models.Company GetCompanyName()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Companys.Where(x => x.CompanyCode == comid).FirstOrDefault();
        }

        public SelectList GetUserList()
        {
            var comId = _httpContext.HttpContext.Session.GetString("comid");

            var getCompanyUsersURL = $"{_configuration.GetValue<string>("API:GetAllCompnayUsers")}/{comId}";
            var companyUserList = APIHelper.GetRequest<List<CompanyUserVM>>(getCompanyUsersURL, false).Result;

            return new SelectList(companyUserList, "UserId", "UserName");

        }

        public List<Cat_Variable> VarType()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var data = (from a in _context.Cat_Variable join b in _context.HR_ApprovalSettings on a.VarId equals b.ApprovalType where (a.VarType == "Approval Type" && b.ComId == comid && b.IsDelete == false) select a).Distinct().ToList();
            return data;
            //if (comid == "576B68B3-DA3F-4FE5-9656-BC92E4DCDF72")
            //{
            //    var data = _context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
            //    return data;
            //}
            //else
            //{
            //    var data = (from a in _context.Cat_Variable join b in _context.HR_ApprovalSettings on a.VarId equals b.ApprovalType where (a.VarType == "Approval Type" && b.ComId == comid && b.IsDelete == false) select a).ToList();
            //    return data;
            //}
            //_context.Cat_Variable.Where(x => x.VarType == "Approval Type").ToList();
        }
    }
}
