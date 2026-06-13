using AutoMapper;
using ENyayPath.PICS.Application.PrisonerContactPersonDocument.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactPersonDocument
{
    [AllowAnonymous]
    public class PrisonerContactPersonDocumentAppService : ApplicationService, IPrisonerContactPersonDocumentAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactPersonDocument, Guid> _repository;
        private readonly IMapper _mapper;

        public PrisonerContactPersonDocumentAppService(
            IRepository<Core.Eny.Prisoner.PrisonerContactPersonDocument, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrisonerContactPersonDocumentDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<PrisonerContactPersonDocumentDto>>(items);
        }

        public async Task<PrisonerContactPersonDocumentDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<PrisonerContactPersonDocumentDto>(item);
        }
    }
}
