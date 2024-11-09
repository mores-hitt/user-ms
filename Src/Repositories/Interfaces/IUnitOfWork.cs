namespace user_ms.Src.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// Gets the subjects repository.
        /// </summary>
        /// <value>A Concrete class for ISubjectsRepository</value>
        public ISubjectsRepository SubjectsRepository { get; }
        public ISubjectRelationshipsRepository SubjectRelationshipsRepository { get; }

        public IUsersRepository UsersRepository { get; }

        public IRolesRepository RolesRepository { get; }

        public ICareersRepository CareersRepository { get; }

    }
}