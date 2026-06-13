using AutoMapper;
using ENyayPath.PICS.Application.Document.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Document
{
    [AllowAnonymous]
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IRepository<DocumentMaster, Guid> _documentRepository;
        private readonly IMapper _mapper;

        public DocumentAppService(
            IRepository<DocumentMaster, Guid> documentRepository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
        }

        public async Task<List<DocumentDto>> GetAllAsync()
        {
            var documents = await _documentRepository.GetAllListAsync();
            return _mapper.Map<List<DocumentDto>>(documents);
        }

        public async Task<DocumentDto> GetAsync(Guid id)
        {
            var document = await _documentRepository.GetAsync(id);
            return _mapper.Map<DocumentDto>(document);
        }
    }
}
