using System.Threading.Tasks;

namespace csiro_mvc.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationRepository Applications { get; }
        IApplicationSettingsRepository ApplicationSettings { get; }
        
        T GetRepository<T>() where T : class;
        Task<int> SaveAsync();
        Task<int> SaveChangesAsync();
    }
}
