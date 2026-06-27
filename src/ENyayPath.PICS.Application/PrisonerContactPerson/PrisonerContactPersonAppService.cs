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
        private readonly IRepository<Core.Eny.Common.DocumentMaster, Guid> _documentMasterRepo;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;

        public PrisonerContactPersonAppService(
            IRepository<Core.Eny.Prisoner.PrisonerContactPerson, Guid> contactPersonRep,
            IRepository<Core.Eny.Prisoner.PrisonerContactDetail, Guid> contactDetailRepo,
            IRepository<Core.Eny.Prisoner.PrisonerContactPersonDocument, Guid> contactDocRepo,
            IRepository<Core.Eny.Common.DocumentMaster, Guid> documentMasterRepo,
        IFileStorageService fileStorage,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _contactPersonRepo = contactPersonRep;
            _contactDetailRepo = contactDetailRepo;
            _contactDocRepo = contactDocRepo;
            _documentMasterRepo = documentMasterRepo;
            _fileStorage = fileStorage;
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
            if (input == null) throw new ArgumentNullException(nameof(input));
            files ??= new List<IFormFile>();
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
            
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                // Basic validations
                var allowed = new[] { ".pdf", ".png", ".jpg", ".jpeg" };
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!Array.Exists(allowed, e => e == ext)) throw new InvalidOperationException("Invalid file type");
                const long maxBytes = 10 * 1024 * 1024;
                if (file.Length > maxBytes) throw new InvalidOperationException("File too large");
                // Save file
                var Filename = insertedDetail.Id.ToString().Replace("-","");
                var relativePath = await _fileStorage.SaveAsync(file, $"prisonerContact-docs/{Filename}");

                var hasDocEntry = input.Documents != null && i < input.Documents.Count;
                List<Guid> doclist = new List<Guid>();
                if (!hasDocEntry)
                {
                    var docMaster = await _documentMasterRepo.GetAllAsync();
                    doclist = docMaster.ToList().Take(files.Count()).Select(x=>x.Id).ToList();
                    if (docMaster == null) throw new InvalidOperationException($"DocumentMaster with ID {input.Documents[i].DocumentId} not found");
                }
                var docid = hasDocEntry ? input.Documents[i].DocumentId : doclist[i];
                var IsValid = hasDocEntry ? input.Documents[i].IsValidDocument : false;
                var insertedDoc = await _contactDocRepo.InsertAsync(new Core.Eny.Prisoner.PrisonerContactPersonDocument
                {
                    PrisonerContactPersonId = insertedPerson.Id,
                    DocumentId = docid,
                    DocumentUploadLink = relativePath,
                    DocumentName = file.FileName,
                    IsValidDocument = IsValid
                });
            }
           

            return new CreatePrisonerContactPersonDto
            {
                PrisonerId = input.PrisonerId,
                ContactPersonName = insertedPerson.ContactPersonName,
                PhoneNumber = insertedDetail.PhoneNumber,
                PhoneNumberPrefix = insertedDetail.PhoneNumberPrefix,
                AppId = insertedDetail.AppId,
                RegisteredName = insertedDetail.RegisteredName
                //Documents = input.Documents
            };
        }
        public async Task<PrisonerContactPersonAllDto> GetByPrisonerIdAsync(Guid PrisonerID, bool IsAudioCall)
        {
            PrisonerContactPersonAllDto PrisonContactdetails = new PrisonerContactPersonAllDto();
            PrisonContactdetails.Person = _contactPersonRepo.GetAll().Where(x => x.PrisonerId == PrisonerID).FirstOrDefault();
            PrisonContactdetails.Detail = _contactDetailRepo.GetAll().Where(x => x.PrisonerContactPersonId == PrisonContactdetails.Person.Id && x.IsAudioCall == IsAudioCall).FirstOrDefault();
            PrisonContactdetails.Documents =  _contactDocRepo.GetAll().Where(x=>x.PrisonerContactPersonId == PrisonContactdetails.Detail.Id).ToList();

            return PrisonContactdetails;
        }
    }
}
