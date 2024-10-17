using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.LeaveVM;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class LeaveEntryRepository : ILeaveEntryRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public LeaveEntryRepository(GTRDBContext context, IHttpContextAccessor httpcontext)
        {
            _context = context;
            _httpContext = httpcontext;
        }

        public HR_Leave_Balance CreateLeaveBalance(HR_Leave_Avail hR_Leave_Avail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Balance
                .Where(e => e.EmpId == hR_Leave_Avail.EmpId &&
                e.ComId == hR_Leave_Avail.ComId &&
                e.DtOpeningBalance == hR_Leave_Avail.DtFrom.Year && e.IsDelete==false)
                .FirstOrDefault();
        }
        public HR_Leave_Balance CreateLeaveBalanceInfo(HR_Leave_Avail hR_Leave_Avail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Balance
                .Where(e => e.EmpId == hR_Leave_Avail.EmpId &&
                e.ComId == hR_Leave_Avail.ComId &&
                e.DtOpeningBalance == hR_Leave_Avail.DtFrom.Year && e.IsDelete == false)
                .FirstOrDefault();
        }

        public void CreateLeaveEntryPost(HR_Leave_Avail hR_Leave_Avail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            HR_Leave_Balance LeaveBalance = _context.HR_Leave_Balance.Where(e => e.EmpId == hR_Leave_Avail.EmpId && e.ComId == hR_Leave_Avail.ComId && e.DtOpeningBalance == hR_Leave_Avail.DtFrom.Year && e.IsDelete == false).FirstOrDefault();
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(t => t.LTypeId == hR_Leave_Avail.LTypeId && t.IsDelete == false).FirstOrDefault();
            LeaveBalance.DateAdded = DateTime.Now;
            LeaveBalance.UpdateByUserId = hR_Leave_Avail.UserId;

            // for isapprove field
            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (singleapprove == null && doubleapprove == null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }
            else if (singleapprove is not null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = false;
            }
            else if (doubleapprove is not null)
            {
                hR_Leave_Avail.Status = 0;
                hR_Leave_Avail.IsApprove = false;
            }
            else
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }

            _context.Update(LeaveBalance);
            _context.SaveChanges();
        }
        public void CreateLeaveInfoEntryPost(HR_Leave_Avail hR_Leave_Avail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            HR_Leave_Balance LeaveBalance = _context.HR_Leave_Balance.Where(e => e.EmpId == hR_Leave_Avail.EmpId && e.ComId == hR_Leave_Avail.ComId && e.DtOpeningBalance == hR_Leave_Avail.DtFrom.Year && e.IsDelete == false).FirstOrDefault();
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(t => t.LTypeId == hR_Leave_Avail.LTypeId && t.IsDelete == false).FirstOrDefault();
            LeaveBalance.DateAdded = DateTime.Now;
            LeaveBalance.UpdateByUserId = hR_Leave_Avail.UserId;

            // for isapprove field
            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (singleapprove == null && doubleapprove == null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }
            else if (singleapprove is not null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = false;
            }
            else if (doubleapprove is not null)
            {
                hR_Leave_Avail.Status = 0;
                hR_Leave_Avail.IsApprove = false;
            }
            else
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }


            _context.Update(LeaveBalance);
            _context.SaveChanges();
        }


        public void CreateLeaveEntryPost2(HR_Leave_Avail hR_Leave_Avail)
        {

            var comid = _httpContext.HttpContext.Session.GetString("comid");
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(t => t.LTypeId == hR_Leave_Avail.LTypeId && t.IsDelete == false).FirstOrDefault();
            hR_Leave_Avail.LvType = LeaveType.LTypeNameShort;
            hR_Leave_Avail.DateAdded = DateTime.Now;

            // for isapprove field
            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (singleapprove == null && doubleapprove == null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }
            else if (singleapprove is not null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = false;
            }
            else if (doubleapprove is not null)
            {
                hR_Leave_Avail.Status = 0;
                hR_Leave_Avail.IsApprove = false;
            }
            else
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }




            _context.Add(hR_Leave_Avail);
            _context.SaveChanges();
        }

        public void CreateLeaveInfoEntryPost2(HR_Leave_Avail hR_Leave_Avail)
        {

            var comid = _httpContext.HttpContext.Session.GetString("comid");
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(t => t.LTypeId == hR_Leave_Avail.LTypeId && t.IsDelete == false).FirstOrDefault();
            hR_Leave_Avail.LvType = LeaveType.LTypeNameShort;
            hR_Leave_Avail.DateAdded = DateTime.Now;

            // for isapprove field
            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (singleapprove == null && doubleapprove == null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }
            else if (singleapprove is not null)
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = false;
            }
            else if (doubleapprove is not null)
            {
                hR_Leave_Avail.Status = 0;
                hR_Leave_Avail.IsApprove = false;
            }
            else
            {
                hR_Leave_Avail.Status = 1;
                hR_Leave_Avail.IsApprove = true;
            }




            _context.Add(hR_Leave_Avail);
            _context.SaveChanges();
        }
        public Cat_Leave_Type CreateLeaveType(HR_Leave_Avail hR_Leave_Avail)
        {
            return _context.Cat_Leave_Type
                .Where(t => t.LTypeId == hR_Leave_Avail.LTypeId && t.IsDelete == false)
                .FirstOrDefault();
        }

        public Cat_Leave_Type CreateLeaveTypeInfo(HR_Leave_Avail hR_Leave_Avail)
        {
            return _context.Cat_Leave_Type
                .Where(t => t.LTypeId == hR_Leave_Avail.LTypeId && t.IsDelete == false)
                .FirstOrDefault();
        }

        public HR_Leave_Balance GetBalance(int empid)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Balance.Where(b => b.EmpId == empid && b.ComId == comid && b.IsDelete == false).FirstOrDefault();
        }
        public HR_Leave_Balance GetBalanceInfo(int empid)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Balance.Where(b => b.EmpId == empid && b.ComId == comid && b.IsDelete == false).FirstOrDefault();
        }
        public Cat_Leave_Type FindById(int id)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_Leave_Type.Where(t => t.LTypeId == id && t.IsDelete == false).FirstOrDefault();
        }

        public Cat_Leave_Type GetLeaveType()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_Leave_Type.Where(t => t.ComId == comid && t.IsDelete == false).FirstOrDefault();
        }

        public List<Cat_Leave_Type> GetLeaveAll()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_Leave_Type.Where(x => x.IsDelete == false).ToList();
        }
        public Cat_Leave_Type GetLeaveTypeInfo()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.Cat_Leave_Type.Where(t => t.ComId == comid && t.IsDelete == false).FirstOrDefault();
        }
        public LeaveEntryView GridData(int lvid)
        {
            HR_Leave_Avail LeaveData = _context.HR_Leave_Avail.Where(l => l.LvId == lvid && l.IsDelete == false).FirstOrDefault();
            LeaveEntryView data = new LeaveEntryView();
            //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
            data.LvId = LeaveData.LvId;
            data.EmpId = LeaveData.EmpId;
            //asdf.EmpName = LeaveData.EmpName;
            data.LTypeId = LeaveData.LTypeId;

            var Leavetyid = LeaveData.LTypeId;
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(x => x.LTypeId == Leavetyid && x.IsDelete == false).FirstOrDefault();
            data.LTypeNameShort = LeaveType.LTypeNameShort;

            data.DtFrom = DateTime.Parse(LeaveData.DtFrom.ToString()).ToString("dd-MMM-yy");
            data.DtTo = DateTime.Parse(LeaveData.DtTo.ToString()).ToString("dd-MMM-yy");
            data.DtLvInput = DateTime.Parse(LeaveData.DtLvInput.ToString()).ToString("dd-MMM-yy");
            data.DtInput = DateTime.Parse(LeaveData.DtInput.ToString()).ToString("dd-MMM-yy");

            data.TotalDay = LeaveData.TotalDay;
            data.LvApp = LeaveData.LvApp;
            data.Remark = LeaveData.Remark;
            data.Status = LeaveData.Status.ToString();
            data.LeaveOption = LeaveData.LeaveOption;


            return data;
        }
        public LeaveEntryView GridDataInfo(int lvid)
        {
            HR_Leave_Avail LeaveData = _context.HR_Leave_Avail.Where(l => l.LvId == lvid && l.IsDelete == false).FirstOrDefault();
            LeaveEntryView data = new LeaveEntryView();
            //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
            data.LvId = LeaveData.LvId;
            data.EmpId = LeaveData.EmpId;
            //asdf.EmpName = LeaveData.EmpName;
            data.LTypeId = LeaveData.LTypeId;

            var Leavetyid = LeaveData.LTypeId;
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(x => x.LTypeId == Leavetyid && x.IsDelete == false).FirstOrDefault();
            data.LTypeNameShort = LeaveType.LTypeNameShort;

            data.DtFrom = DateTime.Parse(LeaveData.DtFrom.ToString()).ToString("dd-MMM-yy");
            data.DtTo = DateTime.Parse(LeaveData.DtTo.ToString()).ToString("dd-MMM-yy");
            data.DtLvInput = DateTime.Parse(LeaveData.DtLvInput.ToString()).ToString("dd-MMM-yy");
            data.DtInput = DateTime.Parse(LeaveData.DtInput.ToString()).ToString("dd-MMM-yy");

            data.TotalDay = LeaveData.TotalDay;
            data.LvApp = LeaveData.LvApp;
            data.Remark = LeaveData.Remark;
            data.Status = LeaveData.Status.ToString();


            return data;
        }

        public HR_Leave_Avail LeaveAvail()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Avail.Where(a => a.EmpId == 0 & a.ComId == comid && a.IsDelete == false).FirstOrDefault();
        }
        public HR_Leave_Avail LeaveAvailInfo()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Avail.Where(a => a.EmpId == 0 & a.ComId == comid && a.IsDelete == false).FirstOrDefault();
        }

        public HR_Leave_Balance LeaveBalance()
        {
            return _context.HR_Leave_Balance.Where(l => l.LvBalId == 0).FirstOrDefault();
        }
        public HR_Leave_Balance LeaveBalanceInfo()
        {
            return _context.HR_Leave_Balance.Where(l => l.LvBalId == 0).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> LeaveTypeList()
        {
            return new SelectList(_context.Cat_Leave_Type, "LTypeId", "LTypeNameShort");
        }
        public IEnumerable<SelectListItem> LeaveTypeListInfo()
        {
            return new SelectList(_context.Cat_Leave_Type.Where(x => x.IsInActive == false && x.IsDelete == false), "LTypeId", "FullType");
        }

        //public IQueryable<HR_Leave_Balance> LoadEmployeeLeaveData(int? empId, DateTime? date)
        //{

        //    DateTime asdf = DateTime.Now.Date;
        //    if (date is null)
        //    {
        //        var adate = DateTime.Now.Year;
        //    }
        //    else
        //    {
        //        //asdf = date;
        //    }

        //    var comid = _httpContext.HttpContext.Session.GetString("comid");
        //    string year = date.Value.Year.ToString();

        //    var data = _context.HR_Leave_Balance
        //        .Include(lb => lb.HR_Emp_Info)
        //        .Where(l => l.ComId == comid && l.EmpId == empId && l.DtOpeningBalance.ToString() == year.ToString())
        //        .Select(d => new
        //        {
        //            LeaveId = d.LvBalId,
        //            Code = d.HR_Emp_Info.EmpCode,
        //            EmployeeName = d.HR_Emp_Info.EmpName,
        //            Year = year,
        //            CLTotal = d.CL,
        //            CLEnjoyed = d.ACL,
        //            SLTotal = d.SL,
        //            SLEnjoyed = d.ASL,
        //            ELTotal = d.EL,
        //            ELEnjoyed = d.AEL,
        //            MLTotal = d.ML,
        //            MLEnjoyed = d.AML
        //        }).AsQueryable();
        //    return (IQueryable<HR_Leave_Balance>) data;

        //}

        public HR_Leave_Avail LoadGridLeaveData(int lvid)
        {
            return _context.HR_Leave_Avail.Where(l => l.LvId == lvid && l.IsDelete == false).FirstOrDefault();
        }
        public HR_Leave_Avail LoadGridLeaveInfoData(int lvid)
        {
            return _context.HR_Leave_Avail.Where(l => l.LvId == lvid && l.IsDelete == false).FirstOrDefault();
        }

        public List<LeaveEntryView> LoadLeaveEntryPartial(int empId, DateTime? date)
        {

            List<LeaveEntryView> data = new List<LeaveEntryView>();
            var qury = _context.HR_Leave_Avail
               .Include(l => l.Cat_Leave_Type)
               .Include(la => la.HR_Emp_Info)
               .Where(l => l.EmpId == empId && l.DtFrom.Year >= date.Value.Year && l.IsDelete == false)
               .Select(l => new
               {
                   l.LvId,
                   l.HR_Emp_Info.EmpCode,
                   l.HR_Emp_Info.EmpName,
                   l.Cat_Leave_Type.LTypeNameShort,
                   l.Cat_Leave_Type.LTypeName,
                   l.DtFrom,
                   l.DtTo,
                   l.dtWork,
                   l.TotalDay,
                   l.LvApp,
                   l.Remark,
                   l.Status,
                   l.LeaveOption,
                   l.IsApprove,
                   l.FileName
               }).ToQueryString();
            var leaveEntries = _context.HR_Leave_Avail
                .Include(l => l.Cat_Leave_Type)
                .Include(la => la.HR_Emp_Info)
                .Where(l => l.EmpId == empId && l.DtFrom.Year >= date.Value.Year && l.IsDelete ==false)
                .Select(l => new
                {
                    l.LvId,
                    l.HR_Emp_Info.EmpCode,
                    l.HR_Emp_Info.EmpName,
                    l.Cat_Leave_Type.LTypeNameShort,
                    l.Cat_Leave_Type.LTypeName,
                    l.DtFrom,
                    l.DtTo,
                    l.dtWork,
                    l.TotalDay,
                    l.LvApp,
                    l.Remark,
                    l.Status,
                    l.LeaveOption,
                    l.IsApprove,
                    l.FileName
                }).ToList().OrderByDescending(l => l.LvId);

            foreach (var item in leaveEntries)
            {
                LeaveEntryView asdf = new LeaveEntryView();
                //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
                asdf.LvId = item.LvId;
                asdf.EmpCode = item.EmpCode;
                asdf.EmpName = item.EmpName;
                asdf.LTypeNameShort = item.LTypeNameShort;
                asdf.LTypeName = item.LTypeName;

                asdf.DtFrom = DateTime.Parse(item.DtFrom.ToString()).ToString("dd-MMM-yy");
                asdf.DtTo = DateTime.Parse(item.DtTo.ToString()).ToString("dd-MMM-yy");
                if (item.dtWork != null)
                {
                    asdf.dtWork = DateTime.Parse(item.dtWork.ToString()).ToString("dd-MMM-yy");
                }
                else
                {
                    asdf.dtWork = "";
                }
                asdf.TotalDay = item.TotalDay;
                asdf.LvApp = item.LvApp;
                asdf.Remark = item.Remark;
                asdf.FileName = item.FileName;
                asdf.LeaveOption = item.LeaveOption;

                if (item.Status == 0 && item.IsApprove == false)
                    asdf.Status = "Pending";
                else if (item.Status == 1 && item.IsApprove == true)
                    asdf.Status = "Approved";
                else if (item.Status == 1 && item.IsApprove == false)
                    asdf.Status = "First Approved";
                else if (item.Status == 2)
                    asdf.Status = "Disapproved";
                else
                    asdf.Status = "Approved";

                data.Add(asdf);

            }
            return data;
        }
        public List<LeaveEntryView> LoadLeaveInfoEntryPartial(int? empId)
        {

            List<LeaveEntryView> data = new List<LeaveEntryView>();
            var leaveEntries = _context.HR_Leave_Avail
                .Include(l => l.Cat_Leave_Type)
                .Include(la => la.HR_Emp_Info)
                .Where(l => l.EmpId == empId && l.IsDelete == false)
                .Select(l => new
                {
                    l.LvId,
                    l.HR_Emp_Info.EmpCode,
                    l.HR_Emp_Info.EmpName,
                    l.Cat_Leave_Type.LTypeNameShort,
                    l.DtFrom,
                    l.DtTo,
                    l.TotalDay,
                    l.LvApp,
                    l.Remark,
                    l.Status,
                    l.IsApprove
                }).ToList().OrderByDescending(l => l.DtFrom);

            foreach (var item in leaveEntries)
            {
                LeaveEntryView asdf = new LeaveEntryView();
                //asdf.MasterLCID = item.ExportInvoiceMasters.COM_MasterLC.MasterLCID;
                asdf.LvId = item.LvId;
                asdf.EmpCode = item.EmpCode;
                asdf.EmpName = item.EmpName;
                asdf.LTypeNameShort = item.LTypeNameShort;

                asdf.DtFrom = DateTime.Parse(item.DtFrom.ToString()).ToString("dd-MMM-yy");
                asdf.DtTo = DateTime.Parse(item.DtTo.ToString()).ToString("dd-MMM-yy");

                asdf.TotalDay = item.TotalDay;
                asdf.LvApp = item.LvApp;
                asdf.Remark = item.Remark;

                if (item.Status == 0 && item.IsApprove == false)
                    asdf.Status = "Pending";
                else if (item.Status == 1 && item.IsApprove == true)
                    asdf.Status = "Approved";
                else if (item.Status == 1 && item.IsApprove == false)
                    asdf.Status = "First Approved";
                else if (item.Status == 2)
                    asdf.Status = "Disapproved";
                else
                    asdf.Status = "Approved";

                data.Add(asdf);

            }
            return data;
        }

        public HR_Leave_Avail PreviousLA(HR_Leave_Avail LeaveAvail)
        {
            return _context.HR_Leave_Avail
                .Include(l => l.Cat_Leave_Type)
                .AsNoTracking()
                .Where(l => l.LvId == LeaveAvail.LvId && l.IsDelete == false)
                .FirstOrDefault();
        }
        public HR_Leave_Avail PreviousLAInfo(HR_Leave_Avail LeaveAvail)
        {
            return _context.HR_Leave_Avail
                .Include(l => l.Cat_Leave_Type)
                .AsNoTracking()
                .Where(l => l.LvId == LeaveAvail.LvId && l.IsDelete == false)
                .FirstOrDefault();
        }
        public HR_Leave_Balance PreviousLB(HR_Leave_Avail LeaveAvail)
        {
            return _context.HR_Leave_Balance
                .Where(e => e.EmpId == LeaveAvail.EmpId
                && e.ComId == LeaveAvail.ComId &&
                e.DtOpeningBalance == LeaveAvail.DtFrom.Year)
                .FirstOrDefault();
        }
        public HR_Leave_Balance PreviousLBInfo(HR_Leave_Avail LeaveAvail)
        {
            return _context.HR_Leave_Balance
                .Where(e => e.EmpId == LeaveAvail.EmpId
                && e.ComId == LeaveAvail.ComId &&
                e.DtOpeningBalance == LeaveAvail.DtFrom.Year)
                .FirstOrDefault();
        }

        public HR_Leave_Balance UpdateLB(HR_Leave_Avail LeaveAvail)
        {
            return _context.HR_Leave_Balance
                .Where(e => e.EmpId == LeaveAvail.EmpId &&
                e.ComId == LeaveAvail.ComId &&
                e.DtOpeningBalance == LeaveAvail.DtFrom.Year)
                .FirstOrDefault();
        }
        public HR_Leave_Balance UpdateLBInfo(HR_Leave_Avail LeaveAvail)
        {
            return _context.HR_Leave_Balance
                .Where(e => e.EmpId == LeaveAvail.EmpId &&
                e.ComId == LeaveAvail.ComId &&
                e.DtOpeningBalance == LeaveAvail.DtFrom.Year)
                .FirstOrDefault();
        }

        public void UpdateLAB(HR_Leave_Avail LeaveAvail)
        {



            HR_Leave_Balance lb = _context.HR_Leave_Balance.Where(e => e.EmpId == LeaveAvail.EmpId && e.ComId == LeaveAvail.ComId && e.DtOpeningBalance == LeaveAvail.DtFrom.Year).FirstOrDefault();


            lb.DateUpdated = DateTime.Now;


            _context.Update(lb);
            _context.SaveChanges();
        }
        public void UpdateLABInfo(HR_Leave_Avail LeaveAvail)
        {



            HR_Leave_Balance lb = _context.HR_Leave_Balance.Where(e => e.EmpId == LeaveAvail.EmpId && e.ComId == LeaveAvail.ComId && e.DtOpeningBalance == LeaveAvail.DtFrom.Year).FirstOrDefault();


            lb.DateUpdated = DateTime.Now;


            _context.Update(lb);
            _context.SaveChanges();
        }
        public void UpdateLeaveAvail(HR_Leave_Avail LeaveAvail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            LeaveAvail.DateUpdated = DateTime.Now;


            // for isapprove field
            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (singleapprove == null && doubleapprove == null)
            {
                LeaveAvail.Status = 1;
                LeaveAvail.IsApprove = true;
            }
            else if (singleapprove is not null)
            {
                LeaveAvail.Status = 1;
                LeaveAvail.IsApprove = false;
            }
            else if (doubleapprove is not null)
            {
                LeaveAvail.Status = 0;
                LeaveAvail.IsApprove = false;
            }
            else
            {
                LeaveAvail.Status = 1;
                LeaveAvail.IsApprove = true;
            }
            _context.Update(LeaveAvail);
            _context.SaveChanges();
            //Old Code---
            //// for isapprove field
            //var approveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && !x.IsDelete).FirstOrDefault();
            //var firstapproveData = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            //if (comid == "C2D5D66E-6406-4569-B7B5-BD272A5B520C")
            //{
            //    _context.Update(LeaveAvail);
            //    _context.SaveChanges();
            //}
            //else
            //{
            //    if (approveData == null)
            //    {
            //        LeaveAvail.Status = 1;
            //        LeaveAvail.IsApprove = true;
            //    }
            //    else if (approveData.IsApprove == true && firstapproveData is not null)
            //    {
            //        //LeaveAvail.Status = 0;
            //        //LeaveAvail.IsApprove = false;
            //        LeaveAvail.Status = 1;
            //        LeaveAvail.IsApprove = true;
            //    }
            //    else if (approveData.IsApprove == true && firstapproveData == null)
            //    {
            //        //LeaveAvail.Status = 1;
            //        //LeaveAvail.IsApprove = false;
            //        LeaveAvail.Status = 1;
            //        LeaveAvail.IsApprove = true;
            //    }
            //    else if (approveData.IsApprove == false && firstapproveData == null)
            //    {
            //        //LeaveAvail.Status = 0;
            //        //LeaveAvail.IsApprove = false;
            //        LeaveAvail.Status = 1;
            //        LeaveAvail.IsApprove = true;
            //    }
            //    else if (approveData.IsApprove == false && firstapproveData != null)
            //    {
            //        //LeaveAvail.Status = 1;
            //        //LeaveAvail.IsApprove = false;
            //        LeaveAvail.Status = 1;
            //        LeaveAvail.IsApprove = true;
            //    }
            //    else
            //    {
            //        LeaveAvail.Status = 1;
            //        LeaveAvail.IsApprove = true;
            //    }
            //    _context.Update(LeaveAvail);
            //    _context.SaveChanges();
            //}

        }
        public void UpdateLeaveAvailInfo(HR_Leave_Avail LeaveAvail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            LeaveAvail.DateUpdated = DateTime.Now;


            // for isapprove field
            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();

            if (singleapprove == null && doubleapprove == null)
            {
                LeaveAvail.Status = 1;
                LeaveAvail.IsApprove = true;
            }
            else if (singleapprove is not null)
            {
                LeaveAvail.Status = 1;
                LeaveAvail.IsApprove = false;
            }
            else if (doubleapprove is not null)
            {
                LeaveAvail.Status = 0;
                LeaveAvail.IsApprove = false;
            }
            else
            {
                LeaveAvail.Status = 1;
                LeaveAvail.IsApprove = true;
            }


            _context.Update(LeaveAvail);
            _context.SaveChanges();
        }
        public void DeleteLeaveEntry(HR_Leave_Avail LeaveAvail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            //LeaveAvail.ComId = _httpContext.HttpContext.Session.GetString("comid");
            HR_Leave_Balance lb = _context.HR_Leave_Balance.Where(e => e.EmpId == LeaveAvail.EmpId && e.ComId == comid && e.DtOpeningBalance == LeaveAvail.DtFrom.Year).FirstOrDefault();
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(t => t.LTypeId == LeaveAvail.LTypeId).FirstOrDefault();
            HR_Leave_Avail PreviousLeave = _context.HR_Leave_Avail.Include(l => l.Cat_Leave_Type).AsNoTracking().Where(l => l.LvId == LeaveAvail.LvId).FirstOrDefault();

            var previoustypeid = LeaveType.LTypeId;
            var presenttypeid = LeaveAvail.LTypeId;
            float ACL = 0;
            float AEL = 0;
            float ASL = 0;
            float AML = 0;
            LeaveAvail.PreviousDay = PreviousLeave.TotalDay;

            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();



            var id = LeaveAvail.LvId;
            var hR_Leave_Avail = _context.HR_Leave_Avail.Find(id);

            if (hR_Leave_Avail.IsApprove == false)
            {
                hR_Leave_Avail.IsDelete = true;
                _context.HR_Leave_Avail.Update(hR_Leave_Avail);
                _context.SaveChanges();
            }


            else
            {

                if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CL")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);
                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);
                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SL")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "EL")
                {
                    AEL = (float)(lb.AEL - LeaveAvail.PreviousDay);
                    lb.AEL = AEL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML - LeaveAvail.PreviousDay);
                    lb.AML = AML;
                }


                _context.Update(lb);
                _context.SaveChanges();

                var ID = LeaveAvail.LvId;
                var hR_Leave_Availl = _context.HR_Leave_Avail.Find(ID);
                hR_Leave_Availl.IsDelete = true;
                _context.HR_Leave_Avail.Update(hR_Leave_Availl);
                _context.SaveChanges();
            }

        }

        public void DeleteLeaveInfoEntry(HR_Leave_Avail LeaveAvail)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            HR_Leave_Balance lb = _context.HR_Leave_Balance.Where(e => e.EmpId == LeaveAvail.EmpId && e.ComId == comid && e.DtOpeningBalance == LeaveAvail.DtFrom.Year).FirstOrDefault();
            Cat_Leave_Type LeaveType = _context.Cat_Leave_Type.Where(t => t.LTypeId == LeaveAvail.LTypeId).FirstOrDefault();
            HR_Leave_Avail PreviousLeave = _context.HR_Leave_Avail.Include(l => l.Cat_Leave_Type).AsNoTracking().Where(l => l.LvId == LeaveAvail.LvId).FirstOrDefault();

            var previoustypeid = LeaveType.LTypeId;
            var presenttypeid = LeaveAvail.LTypeId;
            float ACL = 0;
            float AEL = 0;
            float ASL = 0;
            float AML = 0;
            LeaveAvail.PreviousDay = PreviousLeave.TotalDay;

            var singleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == false && !x.IsDelete).FirstOrDefault();
            var doubleapprove = _context.HR_ApprovalSettings.Where(x => x.ComId == comid && x.ApprovalType == 1175 && x.IsApprove == true && x.IsFirstLeaveApprove == true && !x.IsDelete).FirstOrDefault();



            var id = LeaveAvail.LvId;
            var hR_Leave_Avail = _context.HR_Leave_Avail.Find(id);

            if (hR_Leave_Avail.IsApprove == false)
            {

                //_context.HR_Leave_Avail.Remove(hR_Leave_Avail);
                hR_Leave_Avail.IsDelete = true;
                _context.HR_Leave_Avail.Update(hR_Leave_Avail);
                _context.SaveChanges();
            }


            else
            {

                if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CL")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);
                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "CLH")
                {
                    ACL = (float)(lb.ACL - LeaveAvail.PreviousDay);
                    lb.ACL = ACL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SL")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "SLH")
                {
                    ASL = (float)(lb.ASL - LeaveAvail.PreviousDay);
                    lb.ASL = ASL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "EL")
                {
                    AEL = (float)(lb.AEL - LeaveAvail.PreviousDay);
                    lb.AEL = AEL;
                }
                else if (PreviousLeave.Cat_Leave_Type.LTypeNameShort == "ML")
                {
                    AML = (float)(lb.AML - LeaveAvail.PreviousDay);
                    lb.AML = AML;
                }


                _context.Update(lb);
                _context.SaveChanges();

                var ID = LeaveAvail.LvId;
                var hR_Leave_Availl = _context.HR_Leave_Avail.Find(ID);
                // _context.HR_Leave_Avail.Remove(hR_Leave_Availl);
                hR_Leave_Avail.IsDelete = true;
                _context.HR_Leave_Avail.Update(hR_Leave_Avail);
                _context.SaveChanges();
            }

        }
        public List<HR_Leave_Balance> LeaveEntry(int? empId, DateTime? date)
        {
            string year = date.Value.Year.ToString();
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Balance
                .Include(lb => lb.HR_Emp_Info)
                .Where(l => l.ComId == comid && l.EmpId == empId && l.DtOpeningBalance.ToString() == year.ToString())
                .ToList();
        }
        public List<HR_Leave_Balance> LeaveInfoEntry(int? empId, DateTime? date)
        {
            string year = date.Value.Year.ToString();
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            return _context.HR_Leave_Balance
                .Include(lb => lb.HR_Emp_Info)
                .Where(l => l.ComId == comid && l.EmpId == empId && l.DtOpeningBalance.ToString() == year.ToString())
                .ToList();
        }
    }
}
