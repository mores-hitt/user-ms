using user_ms.Src.Data;
using user_ms.Src.Models;
using user_ms.Src.Repositories.Interfaces;

namespace user_ms.Src.Repositories
{
    public class SubjectRelationshipsRepository : GenericRepository<SubjectRelationship>,
                                                ISubjectRelationshipsRepository
    {
        public SubjectRelationshipsRepository(DataContext context) : base(context)
        {
        }
    }
}