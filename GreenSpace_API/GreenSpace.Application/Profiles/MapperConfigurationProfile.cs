using AutoMapper;
using GreenSpace.Application.ViewModels.Roles;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Domain.Entities;

namespace GreenSpace.Application.Profiles;

public class MapperConfigurationProfile : Profile
{
    public MapperConfigurationProfile()
    {
        CreateMap<Role, RoleViewModel>().ReverseMap();

        CreateMap<User, UserViewModel>()
            .ForMember(
                x => x.RoleName,
                opt => opt.MapFrom(x => x.Role.RoleName)
            )
            .ReverseMap();
        CreateMap<User, UserCreateModel>().ReverseMap();
        CreateMap<User, UserUpdateModel>().ReverseMap();
        //#endregion

        //#region Wallet
        //CreateMap<Wallet, WalletViewModel>()
        //    .ForMember(x => x.WalletLogs, opt => opt.MapFrom(x => x.WalletLogs))
        //    .ForMember(x => x.Transactions, opt => opt.Ignore())
        //    .ReverseMap();
        //#endregion




    }
}
