using AutoMapper;
using ENyayPath.PICS.Application.Authorization.Roles.Dtos;
using ENyayPath.PICS.Application.City.Dtos;
using ENyayPath.PICS.Application.ContactApprovalProcess.Dtos;
using ENyayPath.PICS.Application.Country.Dtos;
using ENyayPath.PICS.Application.Document.Dtos;
using ENyayPath.PICS.Application.DTOs;
using ENyayPath.PICS.Application.Lookup.Dtos;
using ENyayPath.PICS.Application.Prisoner.Dtos;
using ENyayPath.PICS.Application.PrisonerBiometricData.Dtos;
using ENyayPath.PICS.Application.PrisonerCallRecord.Dtos;
using ENyayPath.PICS.Application.PrisonerContactDetail.Dtos;
using ENyayPath.PICS.Application.PrisonerContactPerson.Dtos;
using ENyayPath.PICS.Application.PrisonerContactPersonDocument.Dtos;
using ENyayPath.PICS.Application.Recharge.Dtos;
using ENyayPath.PICS.Application.State.Dtos;
using ENyayPath.PICS.Application.Wallet.Dtos;
using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Eny.Prisoner;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using ENyayPath.PICS.Core.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tenant, TenantDto>().ReverseMap();
            CreateMap<Setting, SettingDto>().ReverseMap();
            
            // Role mappings
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => long.Parse(src.Id)));

            // Prisoner mappings
            CreateMap<Core.Eny.Prisoner.Prisoner, PrisonerDto>().ReverseMap();
            CreateMap<CreatePrisonerDto, Core.Eny.Prisoner.Prisoner>();
            CreateMap<UpdatePrisonerDto, Core.Eny.Prisoner.Prisoner>();

            // Country mappings
            CreateMap<CountryMaster, CountryDto>()
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Id));

            // State mappings
            CreateMap<StateMaster, StateDto>()
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.Id));

            // City mappings
            CreateMap<CityMaster, CityDto>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.Id));

            // Lookup mappings
            CreateMap<LookupMaster, LookupDto>()
                .ForMember(dest => dest.LookupId, opt => opt.MapFrom(src => src.Id));

            // Document mappings
            CreateMap<DocumentMaster, DocumentDto>()
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.Id));

            // PrisonerBiometricData mappings
            CreateMap<Core.Eny.Prisoner.PrisonerBiometricData, PrisonerBiometricDataDto>()
                .ForMember(dest => dest.PrisonerBiometricDataId, opt => opt.MapFrom(src => src.Id));

            // PrisonerContactPerson mappings
            CreateMap<Core.Eny.Prisoner.PrisonerContactPerson, PrisonerContactPersonDto>()
                .ForMember(dest => dest.PrisonerContactPersonId, opt => opt.MapFrom(src => src.Id));

            // PrisonerContactDetail mappings
            CreateMap<Core.Eny.Prisoner.PrisonerContactDetail, PrisonerContactDetailDto>()
                .ForMember(dest => dest.PrisonerContactDetailsId, opt => opt.MapFrom(src => src.Id));

            // PrisonerContactPersonDocument mappings
            CreateMap<Core.Eny.Prisoner.PrisonerContactPersonDocument, PrisonerContactPersonDocumentDto>()
                .ForMember(dest => dest.PrisonerContactPersonDocumentId, opt => opt.MapFrom(src => src.Id));

            // ContactApprovalProcess mappings
            CreateMap<PrisonalContactApprovalProcess, ContactApprovalProcessDto>()
                .ForMember(dest => dest.PrisonalContactApprovalProcessId, opt => opt.MapFrom(src => src.Id));

            // PrisonerCallRecord mappings
            CreateMap<Core.Eny.Prisoner.PrisonerCallRecord, PrisonerCallRecordDto>()
                .ForMember(dest => dest.PrisonerCallRecordId, opt => opt.MapFrom(src => src.Id));

            // Recharge mappings
            CreateMap<Core.Eny.Prisoner.Recharge, RechargeDto>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id));

            // Wallet mappings
            CreateMap<Core.Eny.Prisoner.Wallet, WalletDto>()
                .ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
