using FileServerServiceLogic.Entities.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using AutoMapper;
using FileServerServiceLogic.Contracts.Authorization;

namespace BackEnd
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<UserRegistration, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
