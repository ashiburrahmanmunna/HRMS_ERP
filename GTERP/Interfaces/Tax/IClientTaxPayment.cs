using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.Tax
{
    public interface IClientTaxPayment : IBaseRepository<Tax_PaymentReceived>
    {
    }
}
