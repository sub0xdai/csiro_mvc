using System.Threading.Tasks;

namespace csiro_mvc.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationRepository Applications { get; }
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
