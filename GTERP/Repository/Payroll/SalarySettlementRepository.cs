using GTERP.Interfaces.Payroll;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.Payroll
{
    public class SalarySettlementRepository : BaseRepository<HR_SalarySettlement>, ISalarySettlementRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public SalarySettlementRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpcontext = new HttpContextAccessor();
        }

        public IEnumerable<SelectListItem> GetSalarySettlementList()
        {
            throw new NotImplementedException();
        }

        public List<Pross> GetProssType()
        {
            var comid = _httpcontext.HttpContext.Session.GetString("comid");
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@ComId", comid);
            var prossType = Helper.ExecProcMapTList<Pross>("GetProssType", parameter);
            return prossType;
        }

        public List<OTFCSalarySettlement> Search(string prossType)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            var OTFCList = Helper.ExecProcMapTList<OTFCSalarySettlement>("Payroll_prcGetManaulSalaryDismiss", parameter);
            string query = $"Exec Payroll_prcGetManaulSalaryDismiss '{comid}', '{prossType}'";
            return OTFCList;
        }

        public List<OTFCSalarySettlement> UpdateData(string prossType)
        {
            string comid = _httpcontext.HttpContext.Session.GetString("comid");

            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@ComId", comid);
            parameter[1] = new SqlParameter("@ProssType", prossType);
            var OTFCList1 = Helper.ExecProcMapTList<OTFCSalarySettlement>("Payroll_prcGetManaulSalaryDismissUpdate", parameter);

            return OTFCList1;
        }

        public void CreateSalarySettlement(List<HR_SalarySettlement> HR_SalarySettlements, string ProssType)
        {
            foreach (var item in HR_SalarySettlements)
            {

                var exist = _context.HR_SalarySettlement.Where(o => o.EmpId == item.EmpId && o.ProssType == item.ProssType && o.ComId == item.ComId).FirstOrDefault();
                if (exist == null)
                    _context.Add(item);
                else
                {
                    exist.AttBonus = item.AttBonus;
                    exist.Arrear = item.Arrear;
                    exist.OtherAllow = item.OtherAllow;
                    exist.TrnAllow = item.TrnAllow;
                    exist.ExOTAmount = item.ExOTAmount;
                    exist.OthersAddition = item.OthersAddition;
                    exist.CompensationDay = item.CompensationDay;
                    exist.CompensationAdd = item.CompensationAdd;
                    exist.SubsistenceDay = item.SubsistenceDay;
                    exist.SubsistenceAdd = item.SubsistenceAdd;
                    exist.DeathFacilityDay = item.DeathFacilityDay;
                    exist.DeathFacilityAdd = item.DeathFacilityAdd;
                    exist.NoticePayDay = item.NoticePayDay;
                    exist.NoticePayAdd = item.NoticePayAdd;
                    exist.ServiceBenifitDay = item.ServiceBenifitDay;
                    exist.ServiceBenifitAdd = item.ServiceBenifitAdd;
                    exist.CurrEL = item.CurrEL;
                    exist.IsPaid = item.IsPaid;
                    //exist.OtherAllow = item.OtherAllow;

                    _context.Entry(exist).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();

            UpdateData(ProssType);
        }
    }
}
