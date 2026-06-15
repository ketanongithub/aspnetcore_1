using ENyayPath.PICS.Application.Session.Dtos;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Session
{
    public interface ISessionAppService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
