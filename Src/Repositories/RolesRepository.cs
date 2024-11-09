using user_ms.Src.Data;
using user_ms.Src.Models;
using user_ms.Src.Repositories.Interfaces;

namespace user_ms.Src.Repositories
{
    public class RolesRepository : GenericRepository<Role>, IRolesRepository
    {
        public RolesRepository(DataContext context) : base(context)
        {
        }
    }
}