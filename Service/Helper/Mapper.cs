using AutoMapper;
using Entities.Models;
using Service.Dto;

namespace Service.Helper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Users, UserResponse>();
        }
    }
}