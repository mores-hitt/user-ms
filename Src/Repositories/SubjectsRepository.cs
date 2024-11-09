using user_ms.Src.Data;
using user_ms.Src.Models;
using user_ms.Src.Repositories.Interfaces;

namespace user_ms.Src.Repositories
{
    public class SubjectsRepository : GenericRepository<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(DataContext context): base(context)
        {
        }
    }
}