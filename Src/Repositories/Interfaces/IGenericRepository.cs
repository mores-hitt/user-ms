using System.Linq.Expressions;

namespace user_ms.Src.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = ""       
        );

        Task<TEntity?> GetByID(object id);

        Task<TEntity> Insert(TEntity entity);

        Task Delete(object id);

        Task Delete(TEntity entityToDelete);

        Task SoftDelete(object id);

        Task SoftDelete(TEntity entityToDelete);

        Task<TEntity> Update(TEntity entityToUpdate);
    }
}