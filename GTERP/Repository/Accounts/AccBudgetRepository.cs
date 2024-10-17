using GTERP.Interfaces.Accounts;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GTERP.Repository.Accounts
{
    public class AccBudgetRepository : IAccBudgetRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public AccBudgetRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public Product ProductList(int id)
        {
            var data = _context.Products.Where(y => y.ProductId == id).SingleOrDefault();
            return data;
        }
    }
}
