using AutoMapper;
using ENyayPath.PICS.Application.Prisoner.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Authorization.Permissions;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Prisoner
{
    [Authorize]
    public class PrisonerAppService : ApplicationService, IPrisonerAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.Prisoner, int> _prisonerRepository;
        private readonly IMapper _mapper;

        public PrisonerAppService(
            IRepository<Core.Eny.Prisoner.Prisoner, int> prisonerRepository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _prisonerRepository = prisonerRepository;
            _mapper = mapper;
        }

        [Authorize(Policy = PermissionNames.Prisoners_Read)]
        public async Task<PrisonerDto> GetAsync(int id)
        {
            var prisoner = await _prisonerRepository.GetAsync(id);
            return _mapper.Map<PrisonerDto>(prisoner);
        }

        [Authorize(Policy = PermissionNames.Prisoners_Read)]
        public async Task<List<PrisonerDto>> GetAllAsync()
        {
            var prisoners = await _prisonerRepository.GetAllListAsync();
            return _mapper.Map<List<PrisonerDto>>(prisoners);
        }

        [Authorize(Policy = PermissionNames.Prisoners_Create)]
        public async Task<PrisonerDto> CreateAsync(CreatePrisonerDto input)
        {
            var prisoner = _mapper.Map<Core.Eny.Prisoner.Prisoner>(input);
            prisoner.PrisonerId = Guid.NewGuid();
            prisoner.CreatedDate = DateTime.UtcNow;

            var created = await _prisonerRepository.InsertAsync(prisoner);
            return _mapper.Map<PrisonerDto>(created);
        }

        [Authorize(Policy = PermissionNames.Prisoners_Update)]
        public async Task<PrisonerDto> UpdateAsync(UpdatePrisonerDto input)
        {
            var prisoner = await _prisonerRepository.GetAsync(input.Id);

            prisoner.PrisonId = input.PrisonId;
            prisoner.PrisonerBatchNo = input.PrisonerBatchNo;
            prisoner.PrisonerName = input.PrisonerName;
            prisoner.Dob = input.Dob;
            prisoner.PrisonerStatus = input.PrisonerStatus;
            prisoner.AllowedMinutesPerWeek = input.AllowedMinutesPerWeek;
            prisoner.IsAudioCallEnabled = input.IsAudioCallEnabled;
            prisoner.IsVideoCallEnabled = input.IsVideoCallEnabled;
            prisoner.IsActive = input.IsActive;
            prisoner.ModifiedBy = Guid.Empty;
            prisoner.ModifiedDate = DateTime.UtcNow;

            var updated = await _prisonerRepository.UpdateAsync(prisoner);
            return _mapper.Map<PrisonerDto>(updated);
        }

        [Authorize(Policy = PermissionNames.Prisoners_Delete)]
        public async Task DeleteAsync(int id)
        {
            await _prisonerRepository.DeleteAsync(id);
        }
    }
}
