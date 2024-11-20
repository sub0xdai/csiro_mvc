using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Data;
using Microsoft.Extensions.DependencyInjection;

namespace csiro_mvc.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private IApplicationRepository? _applications;
        private IApplicationSettingsRepository? _applicationSettings;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _repositories = new Dictionary<Type, object>();
        }

        public IApplicationRepository Applications
        {
            get
            {
                _applications ??= _serviceProvider.GetRequiredService<IApplicationRepository>();
                return _applications;
            }
        }

        public IApplicationSettingsRepository ApplicationSettings
        {
            get
            {
                _applicationSettings ??= _serviceProvider.GetRequiredService<IApplicationSettingsRepository>();
                return _applicationSettings;
            }
        }

        public T GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = _serviceProvider.GetRequiredService<T>();
            }
            return (T)_repositories[type];
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await SaveAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
