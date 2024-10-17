using GTERP.Interfaces.HRVariables;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HRVariables
{
    public class BankBranchRepository : BaseRepository<Cat_BankBranch>, IBankBranchRepository
    {
        private readonly GTRDBContext _context;
        private readonly HttpContextAccessor _httpContext;
        public BankBranchRepository(GTRDBContext context) : base(context)
        {
            _context = context;
            _httpContext = new HttpContextAccessor();
        }
        public List<Cat_BankBranch> GetBankBranchInfo()
        {
            return _context.Cat_BankBranch.Where(x => x.IsDelete == false).ToList();
        }

        public IEnumerable<SelectListItem> GetBankBranchList()
        {
            return GetBankBranchInfo().Select(x => new SelectListItem
            {
                Value = x.BranchId.ToString(),
                Text = x.BranchName
            });
        }
       
    }
}
