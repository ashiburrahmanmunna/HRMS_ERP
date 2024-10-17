using GTERP.Interfaces.Tax;
using GTERP.Models;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Repository.Tax
{
    public class ClientTaxPayment : BaseRepository<Tax_PaymentReceived>, IClientTaxPayment
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ClientTaxPayment(GTRDBContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }
    }
}
