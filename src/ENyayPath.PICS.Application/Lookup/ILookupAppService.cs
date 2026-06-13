using ENyayPath.PICS.Application.Lookup.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Lookup
{
    public interface ILookupAppService
    {
        Task<List<LookupDto>> GetAllAsync();
        Task<LookupDto> GetAsync(int id);
    }
}
