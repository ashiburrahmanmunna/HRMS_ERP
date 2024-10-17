using GTERP.Interfaces.Tax;
using GTERP.Models;
using GTERP.Repository.Base;

namespace GTERP.Repository.Tax
{
    public class ClientCompany : BaseRepository<Tax_ClientCompany>, IClientCompany
    {
        private readonly GTRDBContext _context;
        public ClientCompany(GTRDBContext context) : base(context)
        {
            _context = context;
        }
    }
}


