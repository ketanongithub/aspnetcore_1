using AutoMapper;
using ENyayPath.PICS.Application.FileStorage;
using ENyayPath.PICS.Application.PrisonerContactPerson.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny.Prison;
using ENyayPath.PICS.Core.Eny.Prisoner;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace ENyayPath.PICS.Application.PrisonerContactPerson
{
    [AllowAnonymous]
    public class PrisonerContactPersonAppService : ApplicationService, IPrisonerContactPersonAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactPerson, Guid> _contactPersonRepo;
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactDetail, Guid> _contactDetailRepo;
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactPersonDocument, Guid> _contactDocRepo;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;

        public PrisonerContactPersonAppService(
            IRepository<Core.Eny.Prisoner.PrisonerContactPerson, Guid> contactPersonRep,
            IRepository<Core.Eny.Prisoner.PrisonerContactDetail, Guid> contactDetailRepo,
            IRepository<Core.Eny.Prisoner.PrisonerContactPersonDocument, Guid> contactDocRepo,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _contactPersonRepo = contactPersonRep;
            _contactDetailRepo = contactDetailRepo;
            _contactDocRepo = contactDocRepo;
            _mapper = mapper;
        }

        public async Task<List<PrisonerContactPersonDto>> GetAllAsync()
        {
            var items = await _contactPersonRepo.GetAllListAsync();
            return _mapper.Map<List<PrisonerContactPersonDto>>(items);
        }

        public async Task<PrisonerContactPersonDto> GetAsync(Guid id)
        {
            var item = await _contactPersonRepo.GetAsync(id);
            return _mapper.Map<PrisonerContactPersonDto>(item);
        }
        public async Task<CreatePrisonerContactPersonDto> CreateAsync(CreatePrisonerContactPersonDto input, List<IFormFile> files)
        {
            // Validation: only one ContactPhoneNumber per Prisoner
            var existingAudioDetails = (from Person in _contactPersonRepo.GetAll()
                                  join Detail in _contactDetailRepo.GetAll()
                                  on Person.Id equals Detail.PrisonerContactPersonId
                                  where (Person.PrisonerId == input.PrisonerId && Detail.PhoneNumber == input.PhoneNumber && input.IsAudioCall == true)
                                  select Person).FirstOrDefault();

            var existingVideoDetails = (from Person in _contactPersonRepo.GetAll()
                                        join Detail in _contactDetailRepo.GetAll()
                                        on Person.Id equals Detail.PrisonerContactPersonId
                                        where (Person.PrisonerId == input.PrisonerId && Detail.AppId == input.AppId && input.IsVideoCall == true)
                                        select Person).FirstOrDefault();

            if (existingAudioDetails != null || existingAudioDetails != null )
            {
                throw new Exception($"ContactPhoneNumber '{input.PhoneNumber}' already exists for Prisoner {input.PrisonerId}.");
            }
            

            // Step 1: Insert PrisonerContactPerson
            var contactPerson = new Core.Eny.Prisoner.PrisonerContactPerson
            {
                PrisonerId = input.PrisonerId,
                ContactPersonName = input.ContactPersonName,
                SonDaughterOf = input.SonDaughterOf,
                Relation = input.Relation,
                IsActive = true
            };

            var insertedPerson = await _contactPersonRepo.InsertAsync(contactPerson);

            // Step 2: Insert PrisonerContactDetail using generated PrisonerContactPersonId
            var contactDetail = new Core.Eny.Prisoner.PrisonerContactDetail
            {
                PrisonerContactPersonId = insertedPerson.Id,
                PhoneNumber = input.PhoneNumber,
                PhoneNumberPrefix = input.PhoneNumberPrefix,
                IsAudioCall = input.IsAudioCall,
                AppId = input.IsAudioCall == false? input.AppId:null,
                RegisteredName = input.IsAudioCall == false ? input.RegisteredName : null,
            };

            var insertedDetail = await _contactDetailRepo.InsertAsync(contactDetail);
            if( files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    // Basic validations
                    var allowed = new[] { ".pdf", ".png", ".jpg", ".jpeg" };
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!Array.Exists(allowed, e => e == ext)) throw new InvalidOperationException("Invalid file type");
                    const long maxBytes = 10 * 1024 * 1024;
                    if (file.Length > maxBytes) throw new InvalidOperationException("File too large");
                    // Save file
                    var relativePath = await _fileStorage.SaveAsync(file, $"prisonerContact-docs/{insertedDetail.Id}");
                    var insertedDoc = await _contactDocRepo.InsertAsync(new Core.Eny.Prisoner.PrisonerContactPersonDocument
                    {
                        PrisonerContactPersonId = insertedDetail.Id,
                        DocumentId = Guid.NewGuid(), // Assuming a new DocumentId is generated for each file
                        DocumentUploadLink = relativePath,
                        DocumentName = file.FileName,
                        IsValidDocument = true // Assuming the uploaded document is valid by default
                    });
                }
            }
           

            return new CreatePrisonerContactPersonDto
            {
                PrisonerId = input.PrisonerId,
                ContactPersonName = insertedPerson.ContactPersonName,
                PhoneNumber = insertedDetail.PhoneNumber,
                PhoneNumberPrefix = insertedDetail.PhoneNumberPrefix,
                AppId = insertedDetail.AppId,
                RegisteredName = insertedDetail.RegisteredName,
                Documents = input.Documents
            };
        }
    }
}
