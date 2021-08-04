using AutoMapper;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Infra.Structure.IoC.MapEntitiesXDto
{
    public static class MapperConfigurations
    {
        public static MapperConfiguration CreateMap()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrganizationDto, Organization>().ReverseMap();
                cfg.CreateMap<AddressDTO, Address>().ReverseMap();
                cfg.CreateMap<PhoneDTO, Phone>().ReverseMap();
                cfg.CreateMap<LicenseDto, License>().ReverseMap();
                cfg.CreateMap<AccessGroupDto, AccessGroup>().ReverseMap();
                cfg.CreateMap<UserSendDto, User>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id.HasValue ? src.Id : null))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsAuthenticated, act => act.MapFrom(src => src.IsAuthenticated))
                .ForMember(dest => dest.LastPassword, act => act.MapFrom(src => src.LastPassword))
                .ForMember(dest => dest.Login, act => act.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, act => act.MapFrom(src => src.Password))
                .ForMember(dest => dest.IdProfile, act => act.MapFrom(src => src.IdProfile));

                cfg.CreateMap<User, UserReturnedDto>()
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsAuthenticated, act => act.MapFrom(src => src.IsAuthenticated))
                .ForMember(dest => dest.LastPassword, act => act.MapFrom(src => src.LastPassword))
                .ForMember(dest => dest.Login, act => act.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, act => act.MapFrom(src => src.Password));
            });
        }
    }
}
