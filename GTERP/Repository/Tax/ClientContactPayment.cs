using GTERP.Interfaces.Tax;
using GTERP.Models;
using GTERP.Repository.Base;

namespace GTERP.Repository.Tax
{
    public class ClientContactPayment: BaseRepository<Tax_ClientContactPayment>, IClientContactPayment
    {
        private readonly GTRDBContext _context;
        public ClientContactPayment(GTRDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
