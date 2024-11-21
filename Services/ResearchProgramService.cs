using csiro_mvc.Data;
using csiro_mvc.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

namespace csiro_mvc.Services
{
    public class ResearchProgramService : IResearchProgramService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ResearchProgramService(ApplicationDbContext context)
        {
            _context = context;
            _logger = Log.ForContext<ResearchProgramService>();
        }

        public async Task<List<ResearchProgram>> GetAllProgramsAsync()
        {
            try
            {
                return await _context.ResearchPrograms
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting all research programs");
                return new List<ResearchProgram>();
            }
        }

        public async Task<ResearchProgram?> GetProgramByIdAsync(int id)
        {
            try
            {
                return await _context.ResearchPrograms.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting research program with ID: {Id}", id);
                return null;
            }
        }

        public async Task<List<ResearchProgram>> GetRecentProgramsAsync(int count = 5)
        {
            try
            {
                return await _context.ResearchPrograms
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting recent research programs");
                return new List<ResearchProgram>();
            }
        }

        public async Task<ResearchProgram> CreateProgramAsync(ResearchProgram program)
        {
            try
            {
                program.CreatedAt = DateTime.UtcNow;
                program.UpdatedAt = DateTime.UtcNow;

                _context.ResearchPrograms.Add(program);
                await _context.SaveChangesAsync();
                return program;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating research program");
                throw;
            }
        }

        public async Task UpdateProgramAsync(ResearchProgram program)
        {
            try
            {
                program.UpdatedAt = DateTime.UtcNow;
                _context.Entry(program).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating research program with ID: {Id}", program.Id);
                throw;
            }
        }

        public async Task DeleteProgramAsync(int id)
        {
            try
            {
                var program = await _context.ResearchPrograms.FindAsync(id);
                if (program != null)
                {
                    _context.ResearchPrograms.Remove(program);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while deleting research program with ID: {Id}", id);
                throw;
            }
        }

        public async Task<ResearchProgram> GetProgramByTitleAsync(string title)
        {
            return await _context.ResearchPrograms
                .FirstOrDefaultAsync(p => p.Title == title) 
                ?? throw new InvalidOperationException($"Research program with title '{title}' not found.");
        }
    }
}
