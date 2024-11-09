using user_ms.Src.Data;
using user_ms.Src.Models;
using user_ms.Src.Repositories.Interfaces;

namespace user_ms.Src.Repositories
{
    public class CareersRepository : GenericRepository<Career>, ICareersRepository
    {
        public CareersRepository(DataContext context) : base(context)
        {
        }
    }
}