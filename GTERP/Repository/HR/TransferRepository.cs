using GTERP.Interfaces.HR;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class TransferRepository : ITransferRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public TransferRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public void DeleteTransfer(int id)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            var hR_Emp_Increment = _context.HR_Emp_Increment.Find(id);

            HR_Emp_Salary empsal = _context.HR_Emp_Salary
                        .Include(e => e.HR_Emp_Info)
                        .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();
            if (empsal != null)
            {
                empsal.BasicSalary = (float)hR_Emp_Increment.OldBS;
                empsal.PersonalPay = (float)hR_Emp_Increment.OldSalary;
                empsal.HouseRent = (float)hR_Emp_Increment.OldHR;
                empsal.HrExp = (float)hR_Emp_Increment.OldHRExp;
                empsal.HRExpensesOther = (float)hR_Emp_Increment.OldHRExpOther;
                empsal.MadicalAllow = (float)hR_Emp_Increment.OldMA;
                empsal.CanteenAllow = (float)hR_Emp_Increment.OldFA;
                empsal.ConveyanceAllow = (float)hR_Emp_Increment.NewTA;
                var oldDesigId = Convert.ToInt32(hR_Emp_Increment.OldDesigId);
                if (oldDesigId > 0)
                {
                    empsal.HR_Emp_Info.DesigId = oldDesigId;
                }

                var oldSectId = Convert.ToInt32(hR_Emp_Increment.OldSectId);
                if (oldSectId > 0)
                {
                    empsal.HR_Emp_Info.SectId = oldSectId;
                }

                var oldEmpTypeId = Convert.ToInt32(hR_Emp_Increment.OldEmpTypeId);
                if (oldEmpTypeId > 0)
                {
                    empsal.HR_Emp_Info.EmpTypeId = oldEmpTypeId;
                }

                empsal.HR_Emp_Info.DtIncrement = hR_Emp_Increment.DtIncrement;
                _context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                _context.Entry(empsal).State = EntityState.Modified;
                //db.SaveChanges();
            }

            _context.HR_Emp_Increment.Remove(hR_Emp_Increment);
            _context.SaveChanges();
        }

        public List<HR_Emp_Info> EmpInfo(int empid)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var EmpInfo = _context.HR_Emp_Info.Select(e => new
            {
                e.EmpId,
                e.EmpCode,
                e.EmpName,
                DtJoin = Convert.ToDateTime(e.DtJoin).ToString("dd-MMM-yyyy"),
                DesigName = e.Cat_Designation.DesigName,
                SectName = e.Cat_Section.SectName,
                DesigId = e.DesigId,
                SectId = e.SectId,
                EmpTypeId = e.EmpTypeId,
                e.ComId,
                DtIncrement = Convert.ToDateTime(e.DtIncrement).ToString("dd-MMM-yyyy"),
                IncTypeId = e.HR_Emp_Increment.IncTypeId
            }).Where(e => e.EmpId == empid && e.ComId == comid).ToList();
            return null;
        }

        //public List<HR_Emp_Salary> EmpSalary(int empid)
        //{
        //    var Salary = _context.HR_Emp_Salary.Select(s => new
        //    {
        //        s.PersonalPay,
        //        s.BasicSalary,
        //        s.HouseRent,
        //        s.HrExp,
        //        s.HRExpensesOther,
        //        s.CanteenAllow,
        //        s.MadicalAllow,
        //        s.ConveyanceAllow,
        //        s.EmpId
        //    }).Where(s => s.EmpId == empid).ToList();
        //    return null;
        //}

        public IEnumerable<SelectListItem> GetEmpList1()
        {
            var data = _context.HR_Emp_Info.Select(e => new { Text = e.EmpName + "- [" + e.EmpCode + "]", Value = e.EmpId })
                                 .OrderBy(e => e.Value).ToList();
            return new SelectList(data, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetEmpList2()
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var empInfo = (from emp in _context.HR_Emp_Info
                           join d in _context.Cat_Department on emp.DeptId equals d.DeptId
                           join s in _context.Cat_Section on emp.SectId equals s.SectId
                           join des in _context.Cat_Designation on emp.DesigId equals des.DesigId
                           where emp.ComId == comid && emp.IsDelete==false
                           select new
                           {
                               Value = emp.EmpId,
                               Text = emp.EmpCode + " - [ " + emp.EmpName + " ] - [ " + des.DesigName + " ] - [ " + d.DeptName + " ] - [ " + s.SectName + " ]"
                           }).ToList();

            return new SelectList(empInfo, "Value", "Text");
        }

        public void GetIncrementId(int incId)
        {
            var inc = _context.HR_Emp_Increment.Where(i => i.IncId == incId);
            _context.Remove(inc);
            _context.SaveChanges();
        }

        public IQueryable<HR_Emp_Increment> GetTransferInfo(int IncId)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var IncrementInfo = _context.HR_Emp_Increment.Select(i => new
            {
                IncId = i.IncId,
                EmpCode = i.HR_Emp_Info.EmpCode,
                EmpName = i.HR_Emp_Info.EmpName,
                SectName = i.HR_Emp_Info.Cat_Section.SectName,
                DeptName = i.Cat_DepartmentOld.DeptName,
                DesigName = i.Cat_DesignationOld.DesigName,
                OldEmpTypeId = i.OldEmpTypeId,
                NewEmpTypeId = i.NewEmpTypeId,
                IncType = i.HR_IncType.IncType,
                // VarType = _context.Cat_Variable.Where(w => w.ComId == comid && w.VarType == "IncType").Select(s => new { s.VarId, s.VarName }).ToList(),
                DtIncrement = Convert.ToDateTime(i.DtIncrement).ToString("dd-MMM-yyyy"),
                DtInput = i.DtInput,
                IncTypeId = i.IncTypeId               
            }).Where(i => i.IncId == IncId);

            return null;
        }

        //public void GetSalaryEmp(int empId, float BS)
        //{
        //    var empTypeId = _context.HR_Emp_Info.Find(empId).EmpTypeId;
        //    var empSalary = _context.HR_Emp_Salary.Where(s => s.EmpId == empId).FirstOrDefault();
        //    Cat_HRSetting hr = null;
        //    Cat_HRExpSetting hrExp = null;

        //    if (empSalary.BId == 11)
        //    {
        //        if (empTypeId != null)
        //        {
        //            hr = _context.Cat_HRSetting
        //                .Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId2 && h.BS <= BS && h.BId == empSalary.BId)
        //                .OrderByDescending(h => h.BS).FirstOrDefault();                    //.ToList();
        //            hrExp = _context.Cat_HRExpSetting
        //               .Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId2 && h.BId == empSalary.BId && h.BS <= BS)
        //               .OrderByDescending(h => h.BS).FirstOrDefault();
        //        }

        //    }
        //    else
        //    {
        //        if (empTypeId != null && empSalary != null)
        //        {
        //            hr = _context.Cat_HRSetting
        //                .Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId1 && h.BS <= BS && h.BId == empSalary.BId)
        //                .OrderByDescending(h => h.BS).FirstOrDefault();                    //.ToList();
        //            hrExp = _context.Cat_HRExpSetting
        //               .Where(h => h.EmpTypeId == empTypeId && h.LId == empSalary.LId1 && h.BId == empSalary.BId && h.BS <= BS)
        //               .OrderByDescending(h => h.BS).FirstOrDefault();
        //        }
        //    }
        //}

        public IEnumerable<SelectListItem> IncTypeList()
        {
            return new SelectList(_context.HR_IncType.Where(n=>n.IncTypeId== 8 || n.IncTypeId == 9 || n.IncTypeId == 10 || n.IncTypeId == 11).ToList(), "IncTypeId", "IncType");
            //return new SelectList(_context.Cat_Variable.Where(n => n.VarId == 1183 || n.VarId == 1184|| n.VarId == 1185).ToList(), "VarId", "VarName");
            //return new SelectList(_context.HR_IncType, "IncTypeId", "IncType");
        }

        public void SaveTransferInfo(HR_Emp_Increment hR_Emp_Increment)
        {
            string comid = _httpContext.HttpContext.Session.GetString("comid");
            string userid = _httpContext.HttpContext.Session.GetString("userid");
            hR_Emp_Increment.UserId = userid;
            hR_Emp_Increment.ComId = comid;
            hR_Emp_Increment.DtInput = DateTime.Now;

            if (hR_Emp_Increment.IncId > 0)
            {


                HR_Emp_Info empsal = _context.HR_Emp_Info                    
                 .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();
                if (empsal != null)
                {
                  
                    var newdesigid = Convert.ToInt32(hR_Emp_Increment.NewDesigId);
                    if (newdesigid > 0)
                    {
                        empsal.DesigId = newdesigid;
                    }

                    var newdeptid = Convert.ToInt32(hR_Emp_Increment.NewDeptId);
                    if (newdeptid > 0)
                    {
                        empsal.DeptId = newdeptid;
                    }
                    var newsectid = Convert.ToInt32(hR_Emp_Increment.NewSectId);
                    if (newsectid > 0)
                    {
                        empsal.SectId = newsectid;
                    }
                    var newemptypeid = Convert.ToInt32(hR_Emp_Increment.NewEmpTypeId);
                    if (newemptypeid > 0)
                    {
                        empsal.EmpTypeId = newemptypeid;
                    }
                    empsal.DtIncrement = hR_Emp_Increment.DtIncrement;
                    //_context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                    _context.Entry(empsal).State = EntityState.Modified;
                }

                hR_Emp_Increment.UpdateByUserId = userid;
                hR_Emp_Increment.DateUpdated = DateTime.Now;
                _context.Entry(hR_Emp_Increment).State = EntityState.Modified;
            }
            else
            {


                var check = _context.HR_Emp_Increment.Where(i => i.EmpId == hR_Emp_Increment.EmpId
                        && i.DtIncrement == hR_Emp_Increment.DtIncrement
                        && i.IncTypeId == hR_Emp_Increment.IncTypeId).FirstOrDefault();




                HR_Emp_Info empsal = _context.HR_Emp_Info
                    //.Include(e => e.HR_Emp_Info)
                    .Where(s => s.EmpId == hR_Emp_Increment.EmpId && s.ComId == comid).FirstOrDefault();
                if (empsal != null)
                {
                    
                    var newdesigid = Convert.ToInt32(hR_Emp_Increment.NewDesigId);
                    if (newdesigid > 0)
                    {
                        empsal.DesigId = newdesigid;
                    }

                    var newdeptid = Convert.ToInt32(hR_Emp_Increment.NewDeptId);
                    if (newdeptid > 0)
                    {
                        empsal.DeptId = newdeptid;
                    }

                    var newsectid = Convert.ToInt32(hR_Emp_Increment.NewSectId);
                    if (newsectid > 0)
                    {
                        empsal.SectId = newsectid;
                    }

                    var newemptypeid = Convert.ToInt32(hR_Emp_Increment.NewEmpTypeId);
                    if (newemptypeid > 0)
                    {
                        empsal.EmpTypeId = newemptypeid;
                    }

                    empsal.DtIncrement = hR_Emp_Increment.DtIncrement;
                    //_context.Entry(empsal.HR_Emp_Info).State = EntityState.Modified;
                    _context.Entry(empsal).State = EntityState.Modified;
                    // db.SaveChanges();
                }

                hR_Emp_Increment.DateAdded = DateTime.Now;
                _context.Add(hR_Emp_Increment);
            }
        }
    }
}
