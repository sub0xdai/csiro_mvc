using csiro_mvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace csiro_mvc.Services
{
    public interface IResearchProgramService
    {
        Task<List<ResearchProgram>> GetAllProgramsAsync();
        Task<ResearchProgram?> GetProgramByIdAsync(int id);
        Task<ResearchProgram> GetProgramByTitleAsync(string title);
        Task<List<ResearchProgram>> GetRecentProgramsAsync(int count = 5);
        Task<ResearchProgram> CreateProgramAsync(ResearchProgram program);
        Task UpdateProgramAsync(ResearchProgram program);
        Task DeleteProgramAsync(int id);
    }
}
