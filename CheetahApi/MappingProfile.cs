using AutoMapper;
using Cheetah;
using CheetahApi.DTO;
using CheetahDB;

namespace CheetahApi

{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Account, AccountDTO>().ReverseMap();
        }
    }
}
