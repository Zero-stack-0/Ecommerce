using AutoMapper;
using Entities.Models;
using Service.Dto;
using Service.Dto.Response;

namespace Service.Helper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Users, UserResponse>();
            CreateMap<Role, RoleResponse>();
        }
    }
}