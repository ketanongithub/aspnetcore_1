using AutoMapper;
using ENyayPath.PICS.Application.ContactApprovalProcess.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny.Prisoner;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.ContactApprovalProcess
{
    [AllowAnonymous]
    public class ContactApprovalProcessAppService : ApplicationService, IContactApprovalProcessAppService
    {
        private readonly IRepository<PrisonalContactApprovalProcess, Guid> _repository;
        private readonly IMapper _mapper;

        public ContactApprovalProcessAppService(
            IRepository<PrisonalContactApprovalProcess, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ContactApprovalProcessDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<ContactApprovalProcessDto>>(items);
        }

        public async Task<ContactApprovalProcessDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<ContactApprovalProcessDto>(item);
        }
    }
}
