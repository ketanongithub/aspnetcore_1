using AutoMapper;
using ENyayPath.PICS.Application.Authorization.Roles.Dtos;
using ENyayPath.PICS.Application.DTOs;
using ENyayPath.PICS.Application.Prisoner.Dtos;
using ENyayPath.PICS.Core.Authorization.Roles;
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
        }
    }
}
