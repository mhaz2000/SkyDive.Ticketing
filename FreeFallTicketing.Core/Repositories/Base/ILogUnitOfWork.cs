namespace SkyDiveTicketing.Core.Repositories.Base
{
    public interface ILogUnitOfWork
    {
        Task<int> CommitAsync();

        public int Commit();
    }
}

