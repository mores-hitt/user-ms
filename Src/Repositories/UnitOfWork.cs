using user_ms.Src.Data;
using user_ms.Src.Repositories.Interfaces;

namespace user_ms.Src.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISubjectsRepository _subjectsRepository = null!;
        private ISubjectRelationshipsRepository _subjectRelationshipsRepository = null!;

        private IUsersRepository _usersRepository = null!;

        private IRolesRepository _rolesRepository = null!;

        private ICareersRepository  _careersRepository = null!;

        private readonly DataContext _context;
        private bool _disposed = false;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public ISubjectsRepository SubjectsRepository
        {
            get
            {
                _subjectsRepository ??= new SubjectsRepository(_context);

                return _subjectsRepository;
            }
        }

        public ISubjectRelationshipsRepository SubjectRelationshipsRepository
        {
            get
            {
                _subjectRelationshipsRepository ??= new SubjectRelationshipsRepository(_context);

                return _subjectRelationshipsRepository;
            }
        }

        public IRolesRepository RolesRepository
        {
            get
            {
                _rolesRepository ??= new RolesRepository(_context);

                return _rolesRepository;
            }
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                _usersRepository ??= new UsersRepository(_context);

                return _usersRepository;
            }
        }

        public ICareersRepository CareersRepository
        {
            get
            {
                _careersRepository ??= new CareersRepository(_context);

                return _careersRepository;
            }
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}