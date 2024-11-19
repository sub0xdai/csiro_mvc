using csiro_mvc.Data;
using Microsoft.Extensions.Configuration;

namespace csiro_mvc.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;
        private IApplicationRepository _applications;

        public UnitOfWork(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _applications = new ApplicationRepository(_context, configuration);
        }

        public IApplicationRepository Applications => _applications;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
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
