namespace csiro_mvc.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationRepository Applications { get; }
        Task<int> SaveChangesAsync();
    }
}
