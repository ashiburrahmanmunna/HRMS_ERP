using GTERP.Interfaces.Base;
using GTERP.Models;
using GTERP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Interfaces.HRVariables
{
    public interface ITaxAmountSettingRepository:IBaseRepository<Payroll_InComeTaxAmountSetting>
    {
        public void SaveData(Payroll_InComeTaxAmountSetting payroll_InComeTaxAmountSetting);
        public void UpdateData(Payroll_InComeTaxAmountSetting payroll_InComeTaxAmountSetting);
        public List<Payroll_IncomeTaxAmountVM> GetIncometaxList();
    }
}
