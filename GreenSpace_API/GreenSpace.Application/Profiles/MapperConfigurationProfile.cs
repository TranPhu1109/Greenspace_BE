using AutoMapper;

namespace GreenSpace.Application.Profiles;

public class MapperConfigurationProfile : Profile
{
    public MapperConfigurationProfile()
    {
        //CreateMap<Role, RoleViewModel>().ReverseMap();


        //#region User
        //CreateMap<User, UserViewModel>()
        //.ForMember(
        //x => x.RoleName,
        //opt => opt.MapFrom(x => x.Role.Name)
        //)
        //.ReverseMap();
        //CreateMap<User, UserCreateModel>().ReverseMap();
        //CreateMap<User, UserUpdateModel>()
        //    .ForMember(x => x.RoleName, opt => opt.Ignore())
        //    .ReverseMap();
        //#endregion

        //#region Wallet
        //CreateMap<Wallet, WalletViewModel>()
        //    .ForMember(x => x.WalletLogs, opt => opt.MapFrom(x => x.WalletLogs))
        //    .ForMember(x => x.Transactions, opt => opt.Ignore())
        //    .ReverseMap();
        //#endregion


        //#region Carts
        //CreateMap<CartEntity, CartCreateModel>().ReverseMap();
        //CreateMap<CartEntity, CartViewModel>().ReverseMap()
        //    .ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)))
        //    .ForMember(x => x.Items, cfg => cfg.MapFrom(x => x.Items));
        //CreateMap<CartEntity, CartUpdateModel>().ReverseMap()
        //    .ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)))
        //    .ForMember(x => x.Items, cfg => cfg.MapFrom(x => x.Items));
        //CreateMap<CartItemCreateModel, CartItemEntity>().ReverseMap();
        //CreateMap<CartItemEntity, CartItemViewModel>().ReverseMap();
        //CreateMap<CartItemEntity, CartItemUpdateModel>().ReverseMap();
        //#endregion
        //#region Notifications
        //CreateMap<NotificationEntity, NotificationViewModel>()
        //    .ReverseMap()
        //    .ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)));
        //CreateMap<NotificationEntity, NotificationCreateModel>()
        //.ReverseMap()
        //    .ForMember(x => x.Source, cfg => cfg.MapFrom(x => (NotificationSourceEnum)x.Source));
        //CreateMap<NotificationEntity, NotificationUpdateModel>()
        //    .ReverseMap()
        //    .ForMember(x => x.Id, cfg => cfg.MapFrom(x => ObjectId.Parse(x.Id)));
        //#endregion
    }
}
