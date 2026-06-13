using AutoMapper;
using ENyayPath.PICS.Application.Lookup.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Lookup
{
    [AllowAnonymous]
    public class LookupAppService : ApplicationService, ILookupAppService
    {
        private readonly IRepository<LookupMaster, int> _lookupRepository;
        private readonly IMapper _mapper;

        public LookupAppService(
            IRepository<LookupMaster, int> lookupRepository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _lookupRepository = lookupRepository;
            _mapper = mapper;
        }

        public async Task<List<LookupDto>> GetAllAsync()
        {
            var lookups = await _lookupRepository.GetAllListAsync();
            return _mapper.Map<List<LookupDto>>(lookups);
        }

        public async Task<LookupDto> GetAsync(int id)
        {
            var lookup = await _lookupRepository.GetAsync(id);
            return _mapper.Map<LookupDto>(lookup);
        }
    }
}
