using GTERP.Models;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GTERP.Interfaces.HR
{
    public interface ILeaveEntryRepository
    {
        List<HR_Leave_Balance> LeaveEntry(int? empId, DateTime? date);
        List<HR_Leave_Balance> LeaveInfoEntry(int? empId, DateTime? date);
        IEnumerable<SelectListItem> LeaveTypeList();
        IEnumerable<SelectListItem> LeaveTypeListInfo();
        HR_Leave_Balance LeaveBalance();
        HR_Leave_Balance LeaveBalanceInfo();
        HR_Leave_Avail LeaveAvail();
        HR_Leave_Avail LeaveAvailInfo();
        Cat_Leave_Type FindById(int id);
        //IQueryable<HR_Leave_Balance> LoadEmployeeLeaveData (int? empId, DateTime? date);
        List<LeaveEntryView> LoadLeaveEntryPartial(int empId, DateTime? date);
        List<LeaveEntryView> LoadLeaveInfoEntryPartial(int? empId);
        HR_Leave_Balance GetBalance(int empid);
        HR_Leave_Balance GetBalanceInfo(int empid);
        Cat_Leave_Type GetLeaveType();
        Cat_Leave_Type GetLeaveTypeInfo();
        List<Cat_Leave_Type> GetLeaveAll();
        Cat_Leave_Type CreateLeaveType(HR_Leave_Avail hR_Leave_Avail);
        Cat_Leave_Type CreateLeaveTypeInfo(HR_Leave_Avail hR_Leave_Avail);
        HR_Leave_Balance CreateLeaveBalance(HR_Leave_Avail hR_Leave_Avail);
        HR_Leave_Balance CreateLeaveBalanceInfo(HR_Leave_Avail hR_Leave_Avail);
        void CreateLeaveEntryPost(HR_Leave_Avail hR_Leave_Avail);
        void CreateLeaveInfoEntryPost(HR_Leave_Avail hR_Leave_Avail);
        void CreateLeaveEntryPost2(HR_Leave_Avail hR_Leave_Avail);
        void CreateLeaveInfoEntryPost2(HR_Leave_Avail hR_Leave_Avail);
        HR_Leave_Avail LoadGridLeaveData(int lvid);
        HR_Leave_Avail LoadGridLeaveInfoData(int lvid);
        LeaveEntryView GridData(int lvid);
        LeaveEntryView GridDataInfo(int lvid);
        HR_Leave_Balance UpdateLB(HR_Leave_Avail LeaveAvail);
        HR_Leave_Balance UpdateLBInfo(HR_Leave_Avail LeaveAvail);
        HR_Leave_Balance PreviousLB(HR_Leave_Avail LeaveAvail);
        HR_Leave_Balance PreviousLBInfo(HR_Leave_Avail LeaveAvail);
        HR_Leave_Avail PreviousLA(HR_Leave_Avail LeaveAvail);
        HR_Leave_Avail PreviousLAInfo(HR_Leave_Avail LeaveAvail);
        void UpdateLAB(HR_Leave_Avail LeaveAvail);
        void UpdateLABInfo(HR_Leave_Avail LeaveAvail);
        void UpdateLeaveAvail(HR_Leave_Avail Leave_Avail);
        void UpdateLeaveAvailInfo(HR_Leave_Avail Leave_Avail);
        void DeleteLeaveEntry(HR_Leave_Avail LeaveAvail);
        void DeleteLeaveInfoEntry(HR_Leave_Avail LeaveAvail);
    }
}
