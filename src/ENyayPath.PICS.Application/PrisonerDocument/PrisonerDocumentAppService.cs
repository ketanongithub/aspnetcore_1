using AutoMapper;
using ENyayPath.PICS.Application.Prisoner;
using ENyayPath.PICS.Application.PrisonerDocument.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ENyayPath.PICS.Application.FileStorage;
using ENyayPath.PICS.Core.Sessions;

namespace ENyayPath.PICS.Application.PrisonerDocument
{
    [Authorize]
    public class PrisonerDocumentAppService : ApplicationService, IPrisonerDocumentAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.Prisoner, int> _prisonerRepo;
        private readonly IRepository<Core.Eny.Prisoner.PrisonerDocument, Guid> _docRepo;
        private IFileStorageService _fileStorage;
        private readonly IMapper _mapper;

        public PrisonerDocumentAppService(
            IRepository<Core.Eny.Prisoner.Prisoner, int> prisonerRepo,
            IRepository<Core.Eny.Prisoner.PrisonerDocument, Guid> docRepo,
            IFileStorageService fileStorage,
            IMapper mapper,
            IAppSession appSession) : base(appSession)
        {
            _prisonerRepo = prisonerRepo;
            _docRepo = docRepo;
            _fileStorage = fileStorage;
            _mapper = mapper;
        }

        public async Task<PrisonerDocumentDto> UploadAsync(UploadPrisonerDocumentDto input, IFormFile file)
        {
            if (file == null || file.Length == 0) throw new ArgumentException("No file provided");

            // Validate prisoner exists by PrisonerId GUID (Prisoner entity uses int PK but has PrisonerId GUID column)
            var prisoner = await _prisonerRepo.FirstOrDefaultAsync(p => p.PrisonerId == input.PrisonerId);
            if (prisoner == null) throw new InvalidOperationException("Prisoner not found");

            // Basic validations
            var allowed = new[] { ".pdf", ".png", ".jpg", ".jpeg" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!Array.Exists(allowed, e => e == ext)) throw new InvalidOperationException("Invalid file type");
            const long maxBytes = 10 * 1024 * 1024;
            if (file.Length > maxBytes) throw new InvalidOperationException("File too large");

            // Save file
            var relativePath = await _fileStorage.SaveAsync(file, $"prisoner-docs/{input.PrisonerId}");

            // Create record
            var entity = new Core.Eny.Prisoner.PrisonerDocument
            {
                Id = Guid.NewGuid(),
                PrisonerId = input.PrisonerId,
                DocumentId = input.DocumentId,
                DocumentUploadLink = relativePath,
                IsActive = true,
                IsValidDocument = false,
                CreatedBy = Guid.Empty, // set appropriately
                CreatedDate = DateTime.UtcNow
            };

           var created = await _docRepo.InsertAsync(entity);

            return _mapper.Map<PrisonerDocumentDto>(created);
        }

        public async Task<PrisonerDocumentDto> GetAsync(Guid id)
        {
            var e = await _docRepo.GetAsync(id);
            return _mapper.Map<PrisonerDocumentDto>(e);
        }

        public async Task<List<PrisonerDocumentDto>> GetAllAsync(Guid prisonerId)
        {
            var docs = await _docRepo.GetAllListAsync(d => d.PrisonerId == prisonerId && d.IsActive == true);
            return _mapper.Map<List<PrisonerDocumentDto>>(docs);
        }

    }
}
