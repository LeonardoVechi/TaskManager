using AutoMapper;
using ProjetoTaskManager.Application.DTOs;
using ProjetoTaskManager.Domain.Entities;

namespace ProjetoTaskManager.Application.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>();

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore());
        }
    }
}