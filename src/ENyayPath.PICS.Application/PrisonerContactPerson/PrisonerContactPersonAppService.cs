using AutoMapper;
using ENyayPath.PICS.Application.PrisonerContactPerson.Dtos;
using ENyayPath.PICS.Application.PrisonerContactPerson.Dtos.ENyayPath.PICS.Application.PrisonerContactPerson.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny.Prisoner;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactPerson
{
    [AllowAnonymous]
    public class PrisonerContactPersonAppService : ApplicationService, IPrisonerContactPersonAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactPerson, Guid> _repository;
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactDetail, Guid> _contactDetailRepository;
        private readonly IMapper _mapper;

        public PrisonerContactPersonAppService(
            IRepository<Core.Eny.Prisoner.PrisonerContactPerson, Guid> repository,
            IRepository<Core.Eny.Prisoner.PrisonerContactDetail, Guid> contactDetailRepository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _contactDetailRepository = contactDetailRepository;
            _mapper = mapper;
        }

        public async Task<List<PrisonerContactPersonDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<PrisonerContactPersonDto>>(items);
        }

        public async Task<PrisonerContactPersonDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<PrisonerContactPersonDto>(item);
        }

        public async Task CreateAsync(CreatePrisonerContactDto input)
        {
            try
            {
                var contactPerson = new Core.Eny.Prisoner.PrisonerContactPerson
                {
                    Id = Guid.NewGuid(),
                    PrisonerId = input.PrisonerId,
                    ContactPersonName = input.ContactPersonName,
                    Relation = input.Relation,
                    CreatedBy = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsApproved = false,
                    IsTopOnCallList = false
                };

                await _repository.InsertAsync(contactPerson);

                if (input.PhoneNumbers != null)
                {
                    foreach (var phone in input.PhoneNumbers)
                    {
                        var contactDetail = new Core.Eny.Prisoner.PrisonerContactDetail
                        {
                            Id = Guid.NewGuid(),
                            PrisonerContactPersonId = contactPerson.Id,
                            IsAudioCall = true,
                            PhoneNumberPrefix = "+91",
                            PhoneNumber = phone,
                            IsSIMAffedavitUploaded = false,
                            IsSimValidatedSuccessfully = false,
                            IsApproved = false,
                            IsAdharCardUploaded = false,
                            IsActive = true,
                            CreatedBy = Guid.NewGuid(),
                            CreatedDate = DateTime.UtcNow
                        };

                        await _contactDetailRepository.InsertAsync(contactDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    ex.InnerException?.InnerException?.Message ??
                    ex.InnerException?.Message ??
                    ex.Message
                );
            }
        }

        public async Task UpdateAsync(UpdatePrisonerContactDto input)
        {
            var contactPerson = await _repository.GetAsync(input.Id);

            contactPerson.ContactPersonName = input.ContactPersonName;
            contactPerson.Relation = input.Relation;
            contactPerson.ModifiedBy = Guid.NewGuid();
            contactPerson.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(contactPerson);
        }
    }
}