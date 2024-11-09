using System.Linq.Expressions;
using user_ms.Src.Data;
using user_ms.Src.Models;
using user_ms.Src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace user_ms.Src.Repositories
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        private readonly Expression<Func<User, bool>> softDeleteFilter = x => x.DeletedAt == null;

        public UsersRepository(DataContext context) : base(context) { }

        public async Task<List<User>> GetAll()
        {
            var users = await dbSet
                            .Where(softDeleteFilter)
                            .Include(x => x.Role)
                            .Include(x => x.Career)
                            .ToListAsync();
            return users;
        }

        public async Task<User?> GetByEmail(string email)
        {
            var user = await dbSet
                        .Where(softDeleteFilter)
                        .Include(x => x.Role)
                        .Include(x => x.Career)
                        .FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<User?> GetByRut(string rut)
        {
            var user = await dbSet
                        .Where(softDeleteFilter)
                        .Include(x => x.Role)
                        .Include(x => x.Career)
                        .FirstOrDefaultAsync(x => x.RUT == rut);
            return user;
        }

        public async Task<List<UserProgress>?> GetProgressByUser(int userId)
        {
            var userProgress = await context.UserProgresses
                                .Where(x => x.UserId == userId)
                                .Include(x => x.User)
                                .Include(x => x.Subject)
                                .ToListAsync();
            return userProgress;
        }

        public async Task<bool> AddProgress(List<UserProgress> progress)
        {
            await context.UserProgresses.AddRangeAsync(progress);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveProgress(List<UserProgress> progress, int userId)
        {
            var subjectIdsToRemove = progress.Select(p => p.SubjectId).ToList();

            var result = await context.UserProgresses
                .Where(u => u.UserId == userId && subjectIdsToRemove
                .Contains(u.SubjectId))
                .ExecuteDeleteAsync() > 0;

            return result;
        }
    }
}